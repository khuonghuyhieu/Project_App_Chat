using ClassLibrary;
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
        Dictionary<string, string> dsUsers; //userName: password
        Dictionary<string, Socket> dsSocketClient; //userName : socket
        Dictionary<string, List<string>> dsGroup; //groupName: list<userName>

        public ServerForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            dsUsers = new Dictionary<string, string>();
            dsSocketClient = new Dictionary<string, Socket>();
            dsGroup = new Dictionary<string, List<string>>();
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
        private void ThreadClient(Socket client)
        {
            try
            {
                while (true)
                {
                    var reqClient = new byte[1024];
                    int byteReceive = client.Receive(reqClient);

                    if (byteReceive == 0)
                        continue;

                    var originalMessage = Encoding.ASCII.GetString(reqClient, 0, byteReceive);
                    //originalMessage = Utils.ClearJson(originalMessage);
                    var common = JsonSerializer.Deserialize<Common>(originalMessage);

                    if (common != null & common.kind != null)
                    {
                        switch (common.kind)
                        {
                            case "register":
                                {
                                    Register(common, client);

                                    break;
                                }
                            case "getAccountsOnline":
                                {
                                    GetAccountsOnline(common, client);

                                    break;
                                }
                            case "getGroupsJoined":
                                {
                                    GetGroupsJoined(common, client);

                                    break;
                                }
                            case "login":
                                {
                                    Login(common, client);

                                    break;
                                }
                            case "logout":
                                {
                                    Logout(common, client);

                                    break;
                                }
                            case "chatUserToUser":
                                {
                                    ChatUserToUser(common, reqClient);

                                    break;
                                }
                            case "chatUserToGroup":
                                {
                                    //client gui ca Group (1 gui - n nhan)
                                    var packetReq = JsonSerializer.Deserialize<Message>(common.content);
                                    var groupTarger = dsGroup.FirstOrDefault(item => item.Key.Equals(packetReq.Receiver, StringComparison.CurrentCultureIgnoreCase));
                                    //var socketsTarget = dsSocketClient.

                                    break;
                                }
                            case "joinGroup":
                                {
                                    JoinGroup(common, client);

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

        #region Process Request
        private void Register(Common common, Socket socket)
        {
            var registerReq = JsonSerializer.Deserialize<Login>(common.content);
            var registerRes = new Common();

            registerRes.kind = "registerRes";

            if (!dsUsers.Keys.Any(item => item.Equals(registerReq.userName)))
            {
                dsUsers.Remove(registerReq.userName);
                dsUsers.Add(registerReq.userName, registerReq.password);

                registerRes.content = "registerSuccessful";
                Utils.SendCommon(registerRes, socket);

                AddMessage(String.Format("{0} Regitered Successful", registerReq.userName));
            }
            else
            {
                registerRes.content = "userNameIsExits";
                Utils.SendCommon(registerRes, socket);
            }
        }
        private void GetAccountsOnline(Common common, Socket socket)
        {
            var accountOnlineReq = JsonSerializer.Deserialize<string>(common.content); //chi co user Name
            var accountOnlineRes = new Common();
            var accountsOnline = dsUsers.Keys.Where(item => !item.Equals(accountOnlineReq));

            accountOnlineRes.kind = "accountsOnlineRes";
            accountOnlineRes.content = JsonSerializer.Serialize<IEnumerable<string>>(accountsOnline);

            Utils.SendCommon(accountOnlineRes, socket);
        }
        private void GetGroupsJoined(Common common, Socket socket)
        {
            var groupJoinedReq = JsonSerializer.Deserialize<string>(common.content); //chi co user Name
            var groupJoinRes = new Common();
            var groupJoined = new List<string>();

            foreach (var item in dsGroup)
            {
                foreach (var subItem in item.Value)
                {
                    if (subItem.Equals(groupJoinedReq))
                        groupJoined.Add(item.Key);
                }
            }

            groupJoinRes.kind = "groupsJoinedRes";
            groupJoinRes.content = JsonSerializer.Serialize<IEnumerable<string>>(groupJoined);

            Utils.SendCommon(groupJoinRes, socket);
        }
        private void Login(Common common, Socket socket)
        {
            var loginReq = JsonSerializer.Deserialize<Login>(common.content);
            var loginRes = new Common();

            loginRes.kind = "loginRes";

            if (dsUsers.Keys.Contains(loginReq.userName as string) && dsUsers[loginReq.userName].Equals(loginReq.password))
            {
                dsSocketClient.Remove(loginReq.userName);
                dsSocketClient.Add(loginReq.userName, socket);

                loginRes.content = "loginSuccessful";
                Utils.SendCommon(loginRes, socket);
                AddMessage(loginReq.userName + "chào Server");
            }
            else
            {
                loginRes.content = "loginFail";
                Utils.SendCommon(loginRes, socket);
            }
        }
        private void Logout(Common common, Socket socket)
        {
            var logoutReq = common.content; // chi bao gom userName
            var logoutRes = new Common();

            logoutRes.kind = "logoutRes";

            //xoa Client
            dsUsers.Remove(logoutReq);
            //xoa Socket cua Client logout
            dsSocketClient.Remove(logoutReq);

            logoutRes.content = "logoutSuccessful";
            Utils.SendCommon(logoutRes, socket);

            AddMessage(String.Format("{0} is Logout", common.content));
        }
        private void ChatUserToUser(Common common, byte[] reqClient)
        {
            var messageReq = JsonSerializer.Deserialize<Message>(common.content);

            if (dsSocketClient.Keys.Contains(messageReq.Receiver as string))
            {
                var socketReceiver = dsSocketClient[messageReq.Receiver];
                //var message = new byte[1024];

                //message = Encoding.ASCII.GetBytes(String.Format("{0}: {1}", messageReq.Sender, messageReq.Content));
                socketReceiver.Send(reqClient, reqClient.Length, SocketFlags.None);
                AddMessage(String.Format("{0} gui den {1}: {2}", messageReq.Sender, messageReq.Receiver, messageReq.Content));
            }
        }
        private void JoinGroup(Common common, Socket socket)
        {
            var packetReq = JsonSerializer.Deserialize<Group>(common.content);

            if (dsGroup.ContainsKey(packetReq.groupName))
            {
                dsGroup[packetReq.groupName].Add(packetReq.userName);
            }
            else //khong co group thi tao roi join vao
            {
                dsGroup.Add(packetReq.groupName, new List<string> { packetReq.userName });
            }

            var message = new byte[1024];
            message = Encoding.ASCII.GetBytes("Join Group Successful !");

            socket.Send(message, message.Length, SocketFlags.None);
            AddMessage(String.Format("{0} join Group {1} thanh cong", packetReq.userName, packetReq.groupName));
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
        private void ServerForm_Load(object sender, EventArgs e)
        {
            //set cung cac gia tri
            dsUsers.Add("user1", "123");
            dsUsers.Add("user2", "123");
            dsUsers.Add("user3", "123");
            dsGroup.Add("GroupVy", new List<string> { "user1", "use2" });
            dsGroup.Add("GroupTmp", new List<string> { "user3", "user4" });
            txbIp.Text = (Utils.GetLocalIPAddress());

            //Mở kết nối cho Server để chuẩn bị lắng nghe các Client
            StartServer();

            //tao thread de start server
            Thread thread = new Thread(new ThreadStart(ThreadStartServer));
            thread.IsBackground = true;
            thread.Start();
        }
    }
}