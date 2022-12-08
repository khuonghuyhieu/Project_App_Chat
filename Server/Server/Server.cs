using ClassLibrary;
using DTO;
using Models.Data;
using Service;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using MessageReq = ClassLibrary.MessageReq;

namespace Server
{
    public partial class ServerForm : Form
    {
        Socket server;
        IPEndPoint ipServer;
        Thread threadReceive;

        AppChatContext _context;

        AccountSvc _accountSvc;
        GroupSvc _groupSvc;
        MessageGroupSvc _messageGroupSvc;
        MessageUserSvc _messageUserSvc;

        Dictionary<int, Socket> dsSocketClient; //id(account) : socket

        public ServerForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
           
            dsSocketClient = new Dictionary<int, Socket>();          

            _context = new AppChatContext();
            _accountSvc = new AccountSvc(_context);
            _groupSvc = new GroupSvc(_context);
            _messageGroupSvc = new MessageGroupSvc(_context);
            _messageUserSvc = new MessageUserSvc(_context);

            //set cung cac gia tri           
            txbIp.Text = (Utils.GetLocalIPAddress());

            //Mở kết nối cho Server để chuẩn bị lắng nghe các Client
            StartServer();
            //tao thread de start server
            threadReceive = new Thread(new ThreadStart(ThreadStartServer));
            threadReceive.IsBackground = true;
            threadReceive.Start();
        }

        #region Start Server cho cac Client login vao
        private void StartServer()
        {
            var numberOfServingUsers = 20;
            ipServer = new IPEndPoint(IPAddress.Parse(txbIp.Text), int.Parse(txbPort.Text));
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            server.Bind(ipServer);
            server.Listen(numberOfServingUsers);

            AddMessage("Chờ Client kết nối...");
        }
        private void ThreadStartServer()
        {
            //Tao thread cho tung Client login vao
            while (true)
            {
                try
                {
                    Socket client = server.Accept();
                    var threadClient = new Thread(() => ThreadClient(client));

                    threadClient.Start();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            ////Mở kết nối cho Server để chuẩn bị lắng nghe các Client
            //StartServer();

            ////tao thread de start server
            //Thread thread = new Thread(new ThreadStart(ThreadStartServer));
            //thread.IsBackground = true;
            //thread.Start();
        }
        #endregion

        #region Nhan thong tin Client gui den
        private async void ThreadClient(Socket client)
        {
            try
            {
                while (true)
                {
                    var reqClient = new byte[Utils.SIZE_BYTE];
                    int byteReceive = client.Receive(reqClient);

                    if (byteReceive == 0)
                        continue;

                    var originReq = Encoding.ASCII.GetString(reqClient, 0, byteReceive);
                    IEnumerable<string> listOriginReq;

                    originReq = Utils.ClearJson(originReq);
                    listOriginReq = Utils.SplitCommon(originReq);

                    foreach (var req in listOriginReq)
                    {
                        var common = JsonSerializer.Deserialize<Common>(req);

                        if (common != null & common.Kind != null)
                        {
                            switch (common.Kind)
                            {
                                case "register":
                                    {
                                        var registerReq = JsonSerializer.Deserialize<Register>(common.Content);
                                        var registerRes = new Common();

                                        registerRes.Kind = "registerRes";

                                        if (!await _accountSvc.IsAccountExits(registerReq.userName))
                                        {
                                            var accountDto = new AccountDto
                                            {
                                                UserName = registerReq.userName,
                                                Password = registerReq.password,
                                                FullName = registerReq.fullName,
                                            };

                                            await _accountSvc.AddAccount(accountDto);

                                            registerRes.Content = "registerSuccessful";
                                            Utils.SendCommon(registerRes, client);
                                        }
                                        else
                                        {
                                            registerRes.Content = "userNameIsExits";
                                            Utils.SendCommon(registerRes, client);
                                        }

                                        break;
                                    }
                                case "login":
                                    {
                                        var loginReq = JsonSerializer.Deserialize<Login>(common.Content);
                                        var loginRes = new Common();

                                        loginRes.Kind = "loginRes";

                                        if (await _accountSvc.IsHaveAccount(loginReq.userName, loginReq.password)) //login successful
                                        {
                                            var accountLogin = await _accountSvc.GetAccountByUserName(loginReq.userName);

                                            dsSocketClient.Remove(accountLogin.Id);
                                            dsSocketClient.Add(accountLogin.Id, client);

                                            loginRes.Content = JsonSerializer.Serialize(accountLogin);

                                            Utils.SendCommon(loginRes, client);
                                            //AddMessage(loginReq.userName + "chào Server");
                                        }
                                        else
                                        {
                                            loginRes.Content = "loginFail";
                                            Utils.SendCommon(loginRes, client);
                                        }

                                        break;
                                    }
                                case "getAccountsOnline":
                                    {
                                        var accountIdReq = JsonSerializer.Deserialize<int>(common.Content); //chi co idAccount
                                        var accountOnlineRes = new Common();
                                        var accountsOnline = dsSocketClient.Keys.Where(item => item != accountIdReq);
                                        var fullNameAccountOinline = await _accountSvc.GetIdAndFullNameByAccountId(accountsOnline.ToArray());

                                        accountOnlineRes.Kind = "accountsOnlineRes";
                                        accountOnlineRes.Content = JsonSerializer.Serialize(fullNameAccountOinline);

                                        Utils.SendCommon(accountOnlineRes, client);

                                        break;
                                    }
                                case "getAllAccounts":
                                    {
                                        var accountIdReq = JsonSerializer.Deserialize<int>(common.Content); //chi co idAccount
                                        var allAccountRes = new Common();
                                        var allAccounts = await _accountSvc.GetAllAccountExceptId(accountIdReq);

                                        allAccountRes.Kind = "allAccountRes";
                                        allAccountRes.Content = JsonSerializer.Serialize(allAccounts);

                                        Utils.SendCommon(allAccountRes, client);

                                        break;
                                    }
                                case "getGroupsJoined":
                                    {
                                        var accountIdReq = JsonSerializer.Deserialize<int>(common.Content); //chi co idAccount
                                        var groupJoinRes = new Common();
                                        var group = await _groupSvc.GetGroupNameByAccountId(accountIdReq);

                                        groupJoinRes.Kind = "groupsJoinedRes";
                                        groupJoinRes.Content = JsonSerializer.Serialize(group);

                                        Utils.SendCommon(groupJoinRes, client);

                                        break;
                                    }
                                case "Logout":
                                    {
                                        var accountIdReq = JsonSerializer.Deserialize<int>(common.Content); // chi bao gom accountId

                                        //xoa Socket cua Client logout
                                        dsSocketClient.Remove(accountIdReq);

                                        //AddMessage(String.Format("{0} is Logout", common.Content));

                                        break;
                                    }
                                case "chatUserToUser":
                                    {
                                        var messageReq = JsonSerializer.Deserialize<MessageReq>(common.Content);
                                        var messageRes = new Common();

                                        messageRes.Kind = "MessageRes";

                                        if (dsSocketClient.Keys.Contains(messageReq.ReceiverId)) //user dang online
                                        {
                                            var socketReceiver = dsSocketClient[messageReq.ReceiverId];

                                            var message = new MessageRes
                                            {
                                                FullName = _accountSvc.GetAccountById(messageReq.SenderId).Result.FullName,
                                                Content = messageReq.Content,
                                            };

                                            messageRes.Content = JsonSerializer.Serialize(message);

                                            Utils.SendCommon(messageRes, socketReceiver);

                                            //AddMessage(String.Format("{0} gui den {1}: {2}", messageReq.SenderId, messageReq.ReceiverId, messageReq.Content));
                                        }

                                        await _messageUserSvc.AddMessageUser(messageReq); //online hay khong cung luu tin nhan

                                        break;
                                    }
                                case "chatUserToGroup":
                                    {
                                        var messageReq = JsonSerializer.Deserialize<MessageReq>(common.Content);
                                        var messageRes = new Common();
                                        var accountsIdInGroup = await _accountSvc.GetAccountsIdInGroupByGroupId(messageReq.ReceiverId);
                                        var accountIndGroupOnline = accountsIdInGroup.FindAll(accountId => dsSocketClient.Keys.Contains(accountId));

                                        messageRes.Kind = "MessageRes";

                                        foreach (var accountId in accountIndGroupOnline)
                                        {
                                            var socketReceiver = dsSocketClient[accountId];

                                            var message = new MessageRes
                                            {
                                                FullName = _accountSvc.GetAccountById(messageReq.SenderId).Result.FullName,
                                                Content = messageReq.Content,
                                            };

                                            messageRes.Content = JsonSerializer.Serialize(message);

                                            Utils.SendCommon(messageRes, socketReceiver);
                                        }

                                        await _messageGroupSvc.AddMessageGroup(messageReq);

                                        //if (dsSocketClient.Keys.Contains(messageReq.ReceiverId)) //user dang online
                                        //{
                                        //    var socketReceiver = dsSocketClient[messageReq.ReceiverId];

                                        //    var message = new MessageRes
                                        //    {
                                        //        FullName = _accountSvc.GetAccountById(messageReq.SenderId).Result.FullName,
                                        //        Content = messageReq.Content,
                                        //    };

                                        //    messageRes.Content = JsonSerializer.Serialize(message);

                                        //    Utils.SendCommon(messageRes, socketReceiver);

                                        //    //AddMessage(String.Format("{0} gui den {1}: {2}", messageReq.SenderId, messageReq.ReceiverId, messageReq.Content));
                                        //}

                                        //await _messageUserSvc.AddMessageUserToUser(messageReq); //online hay khong cung luu tin nhan

                                        break;
                                    }
                                case "OldMessageUser":
                                    {
                                        var messageOldReq = JsonSerializer.Deserialize<MessageOldReq>(common.Content);
                                        var messageOldRes = new Common();
                                        var messageOld = await _messageUserSvc.GetOldMessageUser(messageOldReq);

                                        messageOldRes.Kind = "OldMessageUserRes";
                                        messageOldRes.Content = JsonSerializer.Serialize(messageOld);

                                        Utils.SendCommon(messageOldRes, client);

                                        break;
                                    }
                                case "OldMessageGroup":
                                    {
                                        var messageOldReq = JsonSerializer.Deserialize<MessageOldReq>(common.Content);
                                        var messageOldRes = new Common();
                                        var messageOld = await _messageGroupSvc.GetMessageOldGroup(messageOldReq);

                                        messageOldRes.Kind = "OldMessageGroupRes";
                                        messageOldRes.Content = JsonSerializer.Serialize(messageOld);

                                        Utils.SendCommon(messageOldRes, client);

                                        break;
                                    }
                                case "joinGroup":
                                    {
                                        //var packetReq = JsonSerializer.Deserialize<Group>(common.Content);

                                        //if (dsGroup.ContainsKey(packetReq.groupName))
                                        //{
                                        //    dsGroup[packetReq.groupName].Add(packetReq.userName);
                                        //}
                                        //else //khong co group thi tao roi join vao
                                        //{
                                        //    dsGroup.Add(packetReq.groupName, new List<string> { packetReq.userName });
                                        //}

                                        //var message = new byte[Utils.SIZE_BYTE];
                                        //message = Encoding.ASCII.GetBytes("Join Group Successful !");

                                        //client.Send(message, message.Length, SocketFlags.None);
                                        //AddMessage(String.Format("{0} join Group {1} thanh cong", packetReq.userName, packetReq.groupName));

                                        break;
                                    }

                                default: break;
                            }
                        }
                    }                
                }
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        private void AddMessage(string message)
        {
            if (InvokeRequired)
            {
                try { this.Invoke(new Action<string>(AddMessage), new object[] { message }); }
                catch (Exception) { }
                return;
            }

            txbKhungHoatDong.AppendText(message);
            txbKhungHoatDong.AppendText(Environment.NewLine);
        }
    }
}