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

            //set cung cac gia tri
            dsUsers.Add("user1", "123");
            dsUsers.Add("user2", "123");
            txbIp.Text = (Utils.GetLocalIPAddress());
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
                    var common = JsonSerializer.Deserialize<Common>(originalMessage);

                    if (common != null & common.kind != null)
                    {
                        switch (common.kind)
                        {                          
                            case "register":
                                {
                                    var registerReq = JsonSerializer.Deserialize<Login>(common.content);
                                    var registerRes = new Common();

                                    registerRes.kind = "registerRes";

                                    if (!dsUsers.Keys.Any(item => item.Equals(registerReq.userName)))
                                    {
                                        dsUsers.Remove(registerReq.userName);
                                        dsUsers.Add(registerReq.userName, registerReq.password);

                                        registerRes.content = "registerSuccessful";
                                        Utils.SendCommon(registerRes, client);

                                        AddMessage(String.Format("{0} Regitered Successful", registerReq.userName));
                                    }
                                    else
                                    {
                                        registerRes.content = "userNameIsExits";
                                        Utils.SendCommon(registerRes, client);
                                    }                                 
                                   
                                    break;
                                }
                            case "getAccountsOnline":
                                {                                    
                                    var accountOnlineRes = new Common();

                                    accountOnlineRes.kind = "accountsOnlineRes";
                                    accountOnlineRes.content = JsonSerializer.Serialize<List<string>>(dsUsers.Keys.ToList());

                                    Utils.SendCommon(accountOnlineRes, client);

                                    break;
                                }
                            case "login":
                                {
                                    var loginReq = JsonSerializer.Deserialize<Login>(common.content);
                                    var loginRes = new Common();
                                    
                                    loginRes.kind = "loginRes";

                                    if (dsUsers.Keys.Contains(loginReq.userName as string) && dsUsers[loginReq.userName].Equals(loginReq.password))
                                    {
                                        dsSocketClient.Remove(loginReq.userName);
                                        dsSocketClient.Add(loginReq.userName, client);

                                        loginRes.content = "loginSuccessful";                                    
                                        Utils.SendCommon(loginRes, client);
                                        AddMessage(loginReq.userName + "chào Server");
                                    }
                                    else
                                    {
                                        loginRes.content = "loginFail";                               
                                        Utils.SendCommon(loginRes, client);                                      
                                    }

                                    break;
                                }
                            case "logout":
                                {
                                    var logoutReq = common.content; // chi bao gom userName
                                    var logoutRes = new Common();

                                    logoutRes.kind = "logoutRes";

                                    //xoa Client
                                    dsUsers.Remove(logoutReq);
                                    //xoa Socket cua Client logout
                                    dsSocketClient.Remove(logoutReq);

                                    logoutRes.content = "logoutSuccessful";
                                    Utils.SendCommon(logoutRes, client);

                                    AddMessage(String.Format("{0} is Logout", common.content));

                                    break;
                                }
                            case "chatUserToUser":
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
                                    //else
                                    //{
                                    //    ResponseToClient("Doi phuong hien tai khong online !", client);
                                    //}

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

                                    client.Send(message, message.Length, SocketFlags.None);
                                    AddMessage(String.Format("{0} join Group {1} thanh cong", packetReq.userName, packetReq.groupName));

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
        private void ServerForm_Load(object sender, EventArgs e)
        {
            //Mở kết nối cho Server để chuẩn bị lắng nghe các Client
            StartServer();

            //tao thread de start server
            Thread thread = new Thread(new ThreadStart(ThreadStartServer));
            thread.IsBackground = true;
            thread.Start();         
        }
    }
}