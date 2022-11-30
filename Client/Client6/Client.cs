using ClassLibrary;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Message = ClassLibrary.Message;

namespace Client6
{
    public partial class ClientForm : Form
    {
        private IPEndPoint ipServer;
        private Socket client;
        Thread threadReceive;

        public ClientForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
        }


        #region Login
        private void LoginToServer()
        {
            //tao connect voi server
            ipServer = new IPEndPoint(IPAddress.Parse(txbIp.Text), int.Parse(txbPort.Text));
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(ipServer);

            //goi tin dang nhap
            var login = new Login
            {
                userName = txbUserName.Text,
                password = txbPassword.Text,
            };
            var common = new Common
            {
                kind = "login",
                content = JsonSerializer.Serialize(login)
            };

            Utils.SendCommon(common, client);

            //tao thread de nhan tin nhan
            threadReceive = new Thread(new ThreadStart(ResponeFromServer));
            threadReceive.IsBackground = true;
            threadReceive.Start();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginToServer();
        }
        #endregion

        #region Logout
        private void Logout()
        {
            var common = new Common
            {
                kind = "logout",
                content = txbUserName.Text
            };

            Utils.SendCommon(common, client);
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Logout();
        }
        #endregion

        #region Register
        private void Register()
        {
            //tao connect voi server
            ipServer = new IPEndPoint(IPAddress.Parse(txbIp.Text), int.Parse(txbPort.Text));
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(ipServer);

            var register = new Login
            {
                userName = txbUserName.Text,
                password = txbPassword.Text
            };

            var common = new Common
            {
                kind = "register",
                content = JsonSerializer.Serialize(register),
            };

            var packetRegister = new byte[1024];
            packetRegister = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(common));

            client.Send(packetRegister, packetRegister.Length, SocketFlags.None);
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            Register();
        }
        #endregion

        #region Send Message
        private void ChatUsertoUser(string methodChat = "chatUserToUser")
        {
            var message = methodChat.Equals("chatUserToUser")
                ? new Message
                {
                    Sender = txbUserName.Text,
                    Receiver = txbSendToUser.Text, //user
                    Content = txbChat.Text
                } :
                new Message
                {
                    Sender = txbUserName.Text,
                    Receiver = txbSendToGroup.Text, //group
                    Content = txbChat.Text
                };

            var common = new Common
            {
                kind = methodChat,
                content = JsonSerializer.Serialize(message)
            };

            Utils.SendCommon(common, client);
            AddMessage(String.Format("{0}: {1}", message.Sender, message.Content));
            txbChat.Clear();
        }
        private void tbnSend_Click(object sender, EventArgs e)
        {
            var methodChat = rabtnUser.Checked ? "chatUserToUser" : "chatUserToGroup";

            ChatUsertoUser(methodChat);
        }
        #endregion

        #region Join Group
        private void JoinGourp()
        {
            var packetJoinGroup = new Group
            {
                groupName = txbGroupName.Text,
                userName = txbUserName.Text

            };

            var common = new Common
            {
                kind = "joinGroup",
                content = JsonSerializer.Serialize(packetJoinGroup)
            };

            var packetByte = new byte[1024];
            packetByte = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(common));
            client.Send(packetByte, packetByte.Length, SocketFlags.None);
        }
        private void btnJoinGroup_Click(object sender, EventArgs e)
        {
            JoinGourp();
        }
        #endregion

        #region Response From Server + nhan message qua lai giua cac Client
        private void ResponeFromServer()
        {
            while (true)
            {
                var receiverMessage = new byte[1024];
                var bytesReceiver = client.Receive(receiverMessage);

                var originalMessage = Encoding.ASCII.GetString(receiverMessage, 0, bytesReceiver);
                originalMessage = originalMessage.Replace("\0", "");
                var packetRes = JsonSerializer.Deserialize<Common>(originalMessage);

                if (packetRes != null && packetRes.content != null)
                {
                    switch (packetRes.kind) //cac goi tin xu ly tac vu: login, logout, register, join group,...
                    {
                        case "registerRes":
                            {
                                if (packetRes.content.Equals("registerSuccessful"))
                                    MessageBox.Show("Register Successful");
                                else
                                    MessageBox.Show("User Name Is Exits");

                                break;
                            }
                        case "loginRes":
                            {
                                if (packetRes.content.Equals("loginSuccessful"))
                                    MessageBox.Show("Login Sucessful");
                                else
                                    MessageBox.Show("Login Fails");

                                break;
                            }
                        case "logoutRes":
                            {
                                if (packetRes.content.Equals("logoutSuccessful"))
                                    MessageBox.Show("Logout Successful");

                                break;
                            }
                        default: //cac tin nhan gui qua lai giua cac Client
                            {
                                var message = JsonSerializer.Deserialize<Message>(packetRes.content);

                                if (message != null)
                                    AddMessage(message.Sender + ": " + message.Content);

                                break;
                            }
                    }
                }
            }
            client.Disconnect(true);
            client.Close();
        }
        #endregion


        private void AddMessage(string message)
        {
            if (InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action<string>(AddMessage), new object[] { message });
                }
                catch (Exception) { }
                return;
            }

            //if (isAlignLeft)
            //    txbKhungChat.TextAlign = HorizontalAlignment.Left;
            //else
            //    txbKhungChat.TextAlign = HorizontalAlignment.Right;

            txbKhungChat.AppendText(message);
            txbKhungChat.AppendText(Environment.NewLine);
        }
        private void ClientForm_Load(object sender, EventArgs e)
        {
            txbIp.Text = Utils.GetLocalIPAddress();
            txbPort.Text = "2008";
            txbUserName.Text = "user1";
            txbPassword.Text = "123";
            txbSendToUser.Text = "user2";
        }
        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (threadReceive != null)
                    threadReceive.Abort();
            }
            catch (Exception)
            {

            }
        }
    }
}