using ClassLibrary;
using DTO;
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

        bool tmp = false;

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

            listView1.Hide();
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

                var originRes = Encoding.ASCII.GetString(byteRes, 0, bytesReceiver);
                IEnumerable<string> listOriginRes;

                originRes = Utils.ClearJson(originRes);
                listOriginRes = Utils.SplitCommon(originRes); //truong hop server tra ve 2 goi tin cung luc

                foreach (var res in listOriginRes)
                {
                    var packetRes = JsonSerializer.Deserialize<Common>(res);

                    if (packetRes != null && packetRes.content != null)
                    {
                        switch (packetRes.kind)
                        {
                            case "accountsOnlineRes":
                                {
                                    var accountsOnline = JsonSerializer.Deserialize<Dictionary<string, string>>(packetRes.content);

                                    if (accountsOnline.Any())
                                    {
                                        var accountDto = new List<AccountDto>();

                                        foreach (var item in accountsOnline)
                                            accountDto.Add(new AccountDto { UserName = item.Key, FullName = item.Value });

                                        listBoxOnline.DataSource = accountDto;
                                        listBoxOnline.DisplayMember = "FullName";
                                        listBoxOnline.ValueMember = "UserName";

                                        listBoxOnline.ClearSelected();
                                    }

                                    break;
                                }
                            case "groupsJoinedRes":
                                {
                                    var groupJoined = JsonSerializer.Deserialize<List<string>>(packetRes.content);

                                    listBoxGroup.DataSource = groupJoined;
                                    listBoxGroup.ClearSelected();

                                    break;
                                }
                            default: //nhan tin nhan giua cac client + group voi nhau
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
        private void Chat(string methodChat)
        {
            if (methodChat.Equals(string.Empty))
            {
                MessageBox.Show("Vui lòng chọn đối tượng để gửi tin nhắn");

                return;
            }

            var message = new ClassLibrary.Message
            {
                Sender = MainForm.userName,
                Receiver = methodChat.Equals("chatUserToUser")
                            ? listBoxOnline.GetItemText(listBoxOnline.SelectedValue)
                            : listBoxGroup.GetItemText(listBoxGroup.SelectedItem),
                Content = txbChat.Text
            };

            var common = new Common
            {
                kind = methodChat,
                content = JsonSerializer.Serialize(message)
            };

            Utils.SendCommon(common, MainForm.client);

            if (listBoxOnline.SelectedItem != null)
                AddMessage(message.Sender + ": " + message.Content);
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            var methodChat = listBoxOnline.GetItemText(listBoxOnline.SelectedItem).Equals("") ?
                listBoxGroup.GetItemText(listBoxGroup.SelectedItem).Equals("") ? string.Empty : "chatUserToGroup"
                : "chatUserToUser";

            Chat(methodChat);
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
        private void listBoxOnline_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (oldSelectedIndexOnline == listBoxOnline.SelectedIndex)
            {
                listBoxOnline.ClearSelected();
            }
            else
            {
                oldSelectedIndexOnline = listBoxOnline.SelectedIndex;

            }

            listBoxGroup.ClearSelected();
        }
        private void listBoxGroup_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (oldSelectedIndexGroup == listBoxGroup.SelectedIndex)
            {
                listBoxGroup.ClearSelected();
            }
            else
            {
                oldSelectedIndexGroup = listBoxGroup.SelectedIndex;

            }

            listBoxOnline.ClearSelected();
        }

        private void btnIcon_Click(object sender, EventArgs e)
        {
            if (listView1.IsAccessible)
            {
                listView1.Hide();
                listView1.IsAccessible = false;
            }

            else
            {
                listView1.Show();
                listView1.IsAccessible = true;
            }

        }

        private void listView1_Click_1(object sender, EventArgs e)
        {
            txbChat.AppendText(listView1.SelectedItems[0].Text);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //gui goi tin lay cac user dang online
            RequestAccountsOnline(MainForm.userName);
            //gui goi tin lay cac group ma user dang login da join
            RequestGroupsJoined(MainForm.userName);
        }
    }
}
