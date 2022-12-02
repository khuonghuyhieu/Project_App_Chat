using ClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_App_Chat
{
    public partial class MainChat : Form
    {
        private Thread threadReceive;
        private int oldSelectedIndexOnline = -1;
        private int oldSelectedIndexGroup = -1;

        public MainChat()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            //tao thread de nhan tin nhan
            threadReceive = new Thread(new ThreadStart(ResponeFromServer));
            threadReceive.IsBackground = true;
            threadReceive.Start();

            //gui goi tin lay cac user dang online
            RequestAccountsOnline(MainForm.userName);
            //gui goi tin lay cac group ma user dang login da join
            RequestGroupsJoined(MainForm.userName);

            labelUserLogin.Text = MainForm.userName;
        }

        #region Response From Server
        private void ResponeFromServer()
        {
            while (true)
            {
                var byteRes = new byte[1024];
                var bytesReceiver = MainForm.client.Receive(byteRes);

                if (bytesReceiver == 0)
                    continue;

                dynamic originRes = Encoding.ASCII.GetString(byteRes, 0, bytesReceiver);
                originRes = Utils.ClearJson(originRes);
                originRes = Utils.SplitCommon(originRes); //truong hop server tra ve 2 goi tin cung luc

                foreach (var res in originRes)
                {
                    var packetRes = JsonSerializer.Deserialize<Common>(res);

                    if (packetRes != null && packetRes.content != null)
                    {
                        switch (packetRes.kind)
                        {
                            case "accountsOnlineRes":
                                {
                                    var accountsOnline = JsonSerializer.Deserialize<IEnumerable<string>>(packetRes.content);

                                    listBoxOnline.DataSource = accountsOnline;
                                    listBoxOnline.ClearSelected();

                                    break;
                                }
                            case "groupsJoinedRes":
                                {
                                    var groupJoined = JsonSerializer.Deserialize<IEnumerable<string>>(packetRes.content);

                                    listBoxGroup.DataSource = groupJoined;
                                    listBoxGroup.ClearSelected();

                                    break;
                                }
                            default: //nhan tin nhan giua cac client voi nhau
                                {
                                    var message = JsonSerializer.Deserialize<ClassLibrary.Message>(packetRes.content);

                                    if (message != null)
                                        AddMessage(message.Sender + ": " + message.Content);

                                    break;
                                }

                        }
                    }
                }


            }
            MainForm.client.Disconnect(true);
            MainForm.client.Close();
        }
        #endregion

        #region Load + Close Form
        private void RequestAccountsOnline(string userName)
        {
            var common = new Common
            {
                kind = "getAccountsOnline",
                content = JsonSerializer.Serialize<string>(userName),
            };

            Utils.SendCommon(common, MainForm.client);
        }
        private void RequestGroupsJoined(string userName)
        {
            var common = new Common
            {
                kind = "getGroupsJoined",
                content = JsonSerializer.Serialize<string>(userName),
            };

            Utils.SendCommon(common, MainForm.client);
        }
        private void MainChat_FormClosing(object sender, FormClosingEventArgs e)
        {
            Utils.KillThread(threadReceive);
        }
        #endregion

        #region Send Message
        private void Chat()
        {
            var message = new ClassLibrary.Message
            {
                Sender = MainForm.userName,
                Receiver = listBoxOnline.GetItemText(listBoxOnline.SelectedItem), //user name
                Content = txbChat.Text
            };

            var common = new Common
            {
                kind = "chatUserToUser",
                content = JsonSerializer.Serialize(message)
            };

            if (!message.Receiver.Equals(string.Empty))
            {
                Utils.SendCommon(common, MainForm.client);

                AddMessage(message.Sender + ": " + message.Content);
            }
            else
                MessageBox.Show("Vui lòng chọn user để gửi tin nhắn");

        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            Chat();
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
            txbChat.Clear();
        }
        private void listBoxOnline_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (oldSelectedIndexOnline == listBoxOnline.SelectedIndex)
            {
                listBoxOnline.ClearSelected();
            }
            else
                oldSelectedIndexOnline = listBoxOnline.SelectedIndex;
        }
        private void listBoxGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (oldSelectedIndexGroup == listBoxGroup.SelectedIndex)
            {
                listBoxGroup.ClearSelected();
            }
            else
                oldSelectedIndexGroup = listBoxGroup.SelectedIndex;
        }
    }
}
