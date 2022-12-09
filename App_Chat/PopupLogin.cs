using App_Chat;
using ClassLibrary;
using DTO;
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
            txbUserName.Text = "vytruong";
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
                Kind = "login",
                Content = JsonSerializer.Serialize(login)
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
                var receiverMessage = new byte[Utils.SIZE_BYTE];
                var bytesReceiver = MainForm.client.Receive(receiverMessage);

                var originalMessage = Encoding.ASCII.GetString(receiverMessage, 0, bytesReceiver);
                originalMessage = originalMessage.Replace("\0", "");
                var packetRes = JsonSerializer.Deserialize<Common>(originalMessage);

                if (packetRes != null && packetRes.Content != null)
                {
                    switch (packetRes.Kind)
                    {
                        case "loginRes":
                            {
                                if (!packetRes.Content.Equals("loginFail"))
                                {
                                    var accountLogin = JsonSerializer.Deserialize<AccountDto>(packetRes.Content);

                                    MainForm.accountLogin = new AccountDto
                                    {
                                        Id = accountLogin.Id,
                                        UserName = accountLogin.UserName,
                                        Password = accountLogin.Password,
                                        FullName = accountLogin.FullName,
                                    };
                                    Program.mainForm.Hide();

                                    this.Close();

                                    var mainChat = new MainChat(new PopupAddGroup());
                                    mainChat.ShowDialog();

                                    return;
                                }
                                else
                                    MessageBox.Show("User Name or Password is wrong !");

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
        private void PopupLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Utils.KillThread(threadReceive);
        }
    }
}
