using App_Chat;
using ClassLibrary;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;

namespace Project_App_Chat
{
    public partial class PopupLogin : Form
    {      
        private Thread threadReceive;

        public PopupLogin()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
        }

        #region Login
        private void LoginToServer()
        {
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

            Utils.SendCommon(common, MainForm.client);         
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginToServer();
        }
        #endregion

        #region Response From Server
        private void ResponeFromServer()
        {
            while (true)
            {
                var receiverMessage = new byte[1024];
                var bytesReceiver = MainForm.client.Receive(receiverMessage);

                var originalMessage = Encoding.ASCII.GetString(receiverMessage, 0, bytesReceiver);
                originalMessage = originalMessage.Replace("\0", "");
                var packetRes = JsonSerializer.Deserialize<Common>(originalMessage);

                if (packetRes != null && packetRes.content != null)
                {
                    switch (packetRes.kind)
                    {
                        case "loginRes":
                            {
                                if (packetRes.content.Equals("loginSuccessful"))
                                {
                                    Program.mainForm.Hide();
                                    this.Close();

                                    var mainChat = new MainChat();
                                    mainChat.ShowDialog();
                                }                                    
                                else
                                    MessageBox.Show("Login Fails");

                                break;
                            }                     
                        default:
                            break;
                    }
                }
            }
            MainForm.client.Disconnect(true);
            MainForm.client.Close();
        }    
        #endregion


        private void PopupLogin_Load(object sender, EventArgs e)
        {
            //set cung gia tri cho de test
            txbIp.Text = Utils.GetLocalIPAddress();
            txbPort.Text = "2008";
            txbUserName.Text = "user1";
            txbPassword.Text = "123";

            //tao thread de nhan tin nhan
            threadReceive = new Thread(new ThreadStart(ResponeFromServer));
            threadReceive.IsBackground = true;
            threadReceive.Start();
        }
        private void PopupLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
           Utils.KillThread(threadReceive);
        }
    }
}
