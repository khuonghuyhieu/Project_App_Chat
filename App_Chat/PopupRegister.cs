using ClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_App_Chat
{
    public partial class PopupRegister : Form
    {
        private Thread threadReceive;

        public PopupRegister()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            //tao thread de nhan tin nhan
            threadReceive = new Thread(new ThreadStart(ResponeFromServer));
            threadReceive.IsBackground = true;
            threadReceive.Start();
        }

        #region Register
        private void RequestRegister()
        {
            var register = new Register
            {
                userName = txbUserName.Text,
                password = txbPassword.Text,
                fullName = txbFullName.Text,
            };

            var common = new Common
            {
                Kind = "register",
                Content = JsonSerializer.Serialize(register),
            };

            var packetRegister = new byte[Utils.SIZE_BYTE];
            packetRegister = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(common));

            MainForm.client.Send(packetRegister, packetRegister.Length, SocketFlags.None);       
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (Utils.ConfirmPassword(txbPassword.Text, txbConfirmPassword.Text))
                RequestRegister();
            else
                MessageBox.Show("Password not same confirm password");
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
                        case "registerRes":
                            {
                                if (packetRes.Content.Equals("registerSuccessful"))
                                {
                                    MessageBox.Show("Register Successful");
                                   
                                    this.Close();

                                    return;
                                }                                    
                                else
                                    MessageBox.Show("User Name Is Exits");

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

        private void PopupRegister_FormClosing(object sender, FormClosingEventArgs e)
        {
            Utils.KillThread(threadReceive);
        }
    }



}
