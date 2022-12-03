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

            //tao thread de nhan tin nhan
            threadReceive = new Thread(new ThreadStart(ResponeFromServer));
            threadReceive.IsBackground = true;
            threadReceive.Start();
        
            //set cung test cho nhanh
            txbUserName.Text = "user1";
            txbPassword.Text = "123";            
        }

        #region Login
        private void RequestLogin()
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
            RequestLogin();
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
                                    MainForm.userName = txbUserName.Text;
                                    Program.mainForm.Hide();

                                    this.Close();
                                    Utils.KillThread(threadReceive);

                                    var mainChat = new MainChat();
                                    mainChat.ShowDialog();

                                    //return;
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
    }
}
