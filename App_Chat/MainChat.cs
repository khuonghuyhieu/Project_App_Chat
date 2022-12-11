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
    public partial class MainChat : Form
    {
        private Thread threadReceive;
        private int oldSelectedIndexOnline = -1;
        private int oldSelectedIndexGroup = -1;

        bool tmp = false;

        private OpenFileDialog openFileDialog;

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

            this.openFileDialog = new OpenFileDialog();

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
                            ? listBoxOnline.GetItemText(listBoxOnline.SelectedItem)
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

        private void btnFile_Click(object sender, EventArgs e)
        {
            var thread = new Thread(new ThreadStart(sendFile));
            thread.Start();
        }

        private void sendFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                var filePath = openFileDialog.FileName;
                var path = "";

                filePath = filePath.Replace("\\", "/");
                while (filePath.IndexOf("/") > -1)
                {
                    path += filePath.Substring(0, filePath.IndexOf("/") + 1);
                    filePath = filePath.Substring(filePath.IndexOf("/") + 1);
                }

                //byte[] fileNameByte = Encoding.ASCII.GetBytes(filePath);
                //byte[] fileData = File.ReadAllBytes(path + filePath);
                //byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];
                //byte[] fileNameLength = BitConverter.GetBytes(fileNameByte.Length);

                //fileNameLength.CopyTo(clientData, 0);
                //fileNameByte.CopyTo(clientData, 4 + fileNameByte.Length);

                var common = new Common
                {
                    kind = "SendFile",
                    content = JsonSerializer.Serialize(filePath)
                };

                Utils.SendCommon(common, MainForm.client);

                txbChat.AppendText("File:/" + filePath);

            }
        }
    }
}
