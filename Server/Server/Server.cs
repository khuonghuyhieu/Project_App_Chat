using ClassLibrary;
using DTO;
using Models.Data;
using Service;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Message = ClassLibrary.Message;

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

        Dictionary<string, string> dsUsers; //userName: password
        Dictionary<int, Socket> dsSocketClient; //id(account) : socket
        Dictionary<string, List<string>> dsGroup; //groupName: list<userName>

        public ServerForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            dsUsers = new Dictionary<string, string>();
            dsSocketClient = new Dictionary<int, Socket>();
            dsGroup = new Dictionary<string, List<string>>();

            _context = new AppChatContext();
            _accountSvc = new AccountSvc(_context);
            _groupSvc = new GroupSvc(_context);
            _messageGroupSvc = new MessageGroupSvc(_context);
            _messageUserSvc = new MessageUserSvc(_context);

            //set cung cac gia tri
            dsUsers.Add("user1", "123");
            dsUsers.Add("user2", "123");
            dsUsers.Add("user3", "123");
            dsGroup.Add("GroupVy", new List<string> { "user1", "user2", "user3" });
            dsGroup.Add("GroupTmp", new List<string> { "user3", "user4" });
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

                    var originalMessage = Encoding.ASCII.GetString(reqClient, 0, byteReceive);
                    originalMessage = Utils.ClearJson(originalMessage);
                    var common = JsonSerializer.Deserialize<Common>(originalMessage);

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
                                    var accountOnlineReq = JsonSerializer.Deserialize<int>(common.Content); //chi co idAccount
                                    var accountOnlineRes = new Common();
                                    var accountsOnline = dsSocketClient.Keys.Where(item => item != accountOnlineReq);
                                    var fullNameAccountOinline = await _accountSvc.GetIdAndFullNameByAccountId(accountsOnline.ToArray());

                                    accountOnlineRes.Kind = "accountsOnlineRes";
                                    accountOnlineRes.Content = JsonSerializer.Serialize(fullNameAccountOinline);

                                    Utils.SendCommon(accountOnlineRes, client);

                                    break;
                                }
                            case "getGroupsJoined":
                                {
                                    var groupJoinedReq = JsonSerializer.Deserialize<int>(common.Content); //chi co idAccount
                                    var groupJoinRes = new Common();
                                    var group = await _groupSvc.GetGroupNameByAccountId(groupJoinedReq);

                                    groupJoinRes.Kind = "groupsJoinedRes";
                                    groupJoinRes.Content = JsonSerializer.Serialize(group);

                                    Utils.SendCommon(groupJoinRes, client);

                                    break;
                                }
                            case "logout":
                                {
                                    //var logoutReq = common.Content; // chi bao gom userName
                                    //var logoutRes = new Common();

                                    //logoutRes.Kind = "logoutRes";

                                    ////xoa Client
                                    //dsUsers.Remove(logoutReq);
                                    ////xoa Socket cua Client logout
                                    //dsSocketClient.Remove(logoutReq);

                                    //logoutRes.Content = "logoutSuccessful";
                                    //Utils.SendCommon(logoutRes, client);

                                    //AddMessage(String.Format("{0} is Logout", common.Content));

                                    break;
                                }
                            case "chatUserToUser":
                                {
                                    var messageReq = JsonSerializer.Deserialize<Message>(common.Content);                                    

                                    if (dsSocketClient.Keys.Contains(messageReq.ReceiverId))
                                    {
                                        var socketReceiver = dsSocketClient[messageReq.ReceiverId];                                       

                                        socketReceiver.Send(reqClient, reqClient.Length, SocketFlags.None);

                                        await _messageUserSvc.AddMessageUserToUser(messageReq);
                                        
                                        //AddMessage(String.Format("{0} gui den {1}: {2}", messageReq.SenderId, messageReq.ReceiverId, messageReq.Content));
                                    }                                      

                                    break;
                                }
                            case "OldMessageUser":
                                {
                                    var messageOldReq = JsonSerializer.Deserialize<MessageOld>(common.Content);
                                    var messageOldRes = new Common();
                                    var messageOld = await _messageUserSvc.GetOldMessageUser(messageOldReq);

                                    messageOldRes.Kind = "OldMessageUserRes";
                                    messageOldRes.Content = JsonSerializer.Serialize(messageOld);

                                    Utils.SendCommon(messageOldRes, client);

                                    break;
                                }
                            case "chatUserToGroup":
                                {
                                    //var messageReq = JsonSerializer.Deserialize<Message>(common.Content);
                                    //var groupTarger = dsGroup.FirstOrDefault(item => item.Key.Equals(messageReq.ReceiverId, StringComparison.CurrentCultureIgnoreCase));
                                    //var socketsInGroup = dsSocketClient.Where(item => groupTarger.Value.Contains(item.Key));

                                    //foreach (var socket in socketsInGroup)
                                    //{
                                    //    socket.Value.Send(reqClient, reqClient.Length, SocketFlags.None);
                                    //}

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