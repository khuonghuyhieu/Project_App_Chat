using App_Chat;
using ClassLibrary;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
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
        public static CheckedListBox checkedListBoxGroup;
        private PopupAddGroup popupAddGroup;

        public MainChat(PopupAddGroup popupAddGroup)
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            //tao thread de nhan tin nhan
            threadReceive = new Thread(new ThreadStart(ResponeFromServer));
            threadReceive.IsBackground = true;
            threadReceive.Start();

            RequestAccountsOnOffline(MainForm.accountLogin.Id);
            RequestGroupsJoined(MainForm.accountLogin.Id);
            RequestAllAccount(MainForm.accountLogin.Id);

            labelUserLogin.Text = MainForm.accountLogin.FullName;

            listViewIcons.Hide();

            this.popupAddGroup = popupAddGroup;
        }

        #region Response From Server
        private void ResponeFromServer()
        {
            while (true)
            {
                var byteRes = new byte[Utils.SIZE_BYTE];
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

                    if (packetRes != null && packetRes.Content != null)
                    {
                        switch (packetRes.Kind)
                        {
                            case "accountsOnOfflineRes":
                                {
                                    var accountsOnline = JsonSerializer.Deserialize<List<AccountOnOffline>>(packetRes.Content);

                                    listBoxOnline.DataSource = null;
                                    listBoxOnline.DataSource = accountsOnline;                                   
                                    
                                    for (int i = 0; i < listBoxOnline.Items.Count; i++)
                                    {
                                        var item = listBoxOnline.Items[i] as AccountOnOffline;

                                        item.FullName = item.IsOnline ? item.FullName + " - Online" : item.FullName + " - Offline";                                      
                                    }

                                    listBoxOnline.DisplayMember = "FullName";
                                    listBoxOnline.ValueMember = "Id";

                                    listBoxOnline.ClearSelected();

                                    break;
                                }
                            case "groupsJoinedRes":
                                {
                                    var groupJoined = JsonSerializer.Deserialize<List<GroupDto>>(packetRes.Content);

                                    //listBox Group
                                    listBoxGroup.DataSource = groupJoined;
                                    listBoxGroup.DisplayMember = "Name";
                                    listBoxGroup.ValueMember = "Id";

                                    listBoxGroup.ClearSelected();

                                    //comboBox Group
                                    popupAddGroup.comboBoxGroup.DataSource = groupJoined;
                                    popupAddGroup.comboBoxGroup.DisplayMember = "Name";
                                    popupAddGroup.comboBoxGroup.ValueMember = "Id";
                                    popupAddGroup.comboBoxGroup.SelectedIndex = -1;

                                    checkedListBoxGroup = checkAddToGroup;

                                    break;
                                }
                            case "allAccountRes":
                                {
                                    var allAccounts = JsonSerializer.Deserialize<Dictionary<int, string>>(packetRes.Content);
                                    var accountDto = new List<AccountDto>();

                                    checkAddToGroup.Items.Clear();

                                    foreach (var item in allAccounts)
                                    {
                                        //accountDto.Add(new AccountDto { Id = item.Key, FullName = item.Value });
                                        checkAddToGroup.Items.Add(new AccountDto { Id = item.Key, FullName = item.Value });                                          
                                    }
                                    
                                    //checkAddToGroup.DataSource = accountDto; //null ??
                                    checkAddToGroup.DisplayMember = "FullName";
                                    checkAddToGroup.ValueMember = "Id";

                                    break;
                                }
                            case "OldMessageUserRes":
                                {
                                    var messageOldUser = JsonSerializer.Deserialize<List<MessageOldRes>>(packetRes.Content);

                                    txbKhungChat.Clear();

                                    foreach (var item in messageOldUser)
                                    {
                                        AddMessage(item.FullName + ": " + item.Message);
                                    }

                                    break;
                                }
                            case "OldMessageGroupRes":
                                {
                                    var messageOldUser = JsonSerializer.Deserialize<List<MessageOldRes>>(packetRes.Content);

                                    txbKhungChat.Clear();

                                    foreach (var item in messageOldUser)
                                    {
                                        AddMessage(item.FullName + ": " + item.Message);
                                    }

                                    break;
                                }                            
                            case "MessageRes": //nhan tin nhan giua cac client + group voi nhau
                                {
                                    var message = JsonSerializer.Deserialize<MessageRes>(packetRes.Content);

                                    if (message != null && !message.Content.Equals(" is leave group") && !message.Content.Equals(" is joined group")) //tin nhan thuong
                                        AddMessage(message.FullName + ": " + message.Content);
                                    else
                                        AddMessage(message.FullName + message.Content); // tin nhan thong bao joined + out group

                                    break;
                                }
                            case "OutGroupRes":
                                {
                                    var outGroupMess = JsonSerializer.Deserialize<string>(packetRes.Content);

                                    if (outGroupMess.Equals("OutGroupSuccess"))
                                        MessageBox.Show("Out group successfully");
                                    else
                                        MessageBox.Show("Out group fails");

                                    break;
                                }
                            default:
                                break;
                        }
                    }
                }


            }
            MainForm.client.Disconnect(true);
            MainForm.client.Close();
        }
        #endregion

        #region Send Message User + Group
        private void Chat(string methodChat)
        {
            if (methodChat.Equals(string.Empty))
            {
                MessageBox.Show("Vui lòng chọn đối tượng để gửi tin nhắn");

                return;
            }

            var message = new MessageReq
            {
                SenderId = MainForm.accountLogin.Id,
                ReceiverId = methodChat.Equals("chatUserToUser")
                            ? int.Parse(listBoxOnline.GetItemText(listBoxOnline.SelectedValue))
                            : int.Parse(listBoxGroup.GetItemText(listBoxGroup.SelectedValue)),
                Content = txbChat.Text,
                TimeSend = DateTime.Now,
            };

            var common = new Common
            {
                Kind = methodChat,
                Content = JsonSerializer.Serialize(message)
            };

            Utils.SendCommon(common, MainForm.client);

            if (listBoxOnline.SelectedItem != null)
                AddMessage(MainForm.accountLogin.FullName + ": " + message.Content);
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            var methodChat = listBoxOnline.GetItemText(listBoxOnline.SelectedItem).Equals("") ?
                listBoxGroup.GetItemText(listBoxGroup.SelectedItem).Equals("") ? string.Empty : "chatUserToGroup"
                : "chatUserToUser";

            Chat(methodChat);
        }
        #endregion

        #region Get Old Message User
        private void OldMessageUser()
        {
            //gui goi tin lay tin nhan cu
            var messagOld = new MessageOldReq
            {
                SenderId = MainForm.accountLogin.Id,
                ReceiverId = int.Parse(listBoxOnline.GetItemText(listBoxOnline.SelectedValue)),
            };

            var common = new Common
            {
                Kind = "OldMessageUser",
                Content = JsonSerializer.Serialize(messagOld)
            };

            Utils.SendCommon(common, MainForm.client);
        }
        private void listBoxOnline_Click(object sender, EventArgs e)
        {
            UnSelectedListBoxOnline();

            if (listBoxOnline.SelectedValue != null)
                OldMessageUser();
        }
        #endregion

        #region Get Old Message Group
        private void OldMessageGroup()
        {
            //gui goi tin lay tin nhan cu
            var messagOld = new MessageOldReq
            {
                GroupId = int.Parse(listBoxGroup.GetItemText(listBoxGroup.SelectedValue)),
            };

            var common = new Common
            {
                Kind = "OldMessageGroup",
                Content = JsonSerializer.Serialize(messagOld)
            };

            Utils.SendCommon(common, MainForm.client);
        }
        private void listBoxGroup_Click(object sender, EventArgs e)
        {
            UnSelectedListBoxGroup();

            if (listBoxGroup.SelectedValue != null)
                OldMessageGroup();
        }
        #endregion

        #region Show Icon
        private void btnIcon_Click(object sender, EventArgs e)
        {
            if (listViewIcons.IsAccessible)
            {
                listViewIcons.Hide();
                listViewIcons.IsAccessible = false;
            }

            else
            {
                listViewIcons.Show();
                listViewIcons.IsAccessible = true;
            }

        }
        private void listViewIcons_Click(object sender, EventArgs e)
        {
            txbChat.AppendText(listViewIcons.SelectedItems[0].Text);
        }
        #endregion

        #region Send packet AccountOnOffLine + GroupJoined + AllAccount
        private void RequestAccountsOnOffline(int idAccountLogin)
        {
            var common = new Common
            {
                Kind = "getAccountsOnOffline",
                Content = JsonSerializer.Serialize(idAccountLogin),
            };

            Utils.SendCommon(common, MainForm.client);
        }
        private void RequestGroupsJoined(int idAccountLogin)
        {
            var common = new Common
            {
                Kind = "getGroupsJoined",
                Content = JsonSerializer.Serialize(idAccountLogin),
            };

            Utils.SendCommon(common, MainForm.client);
        }
        private void RequestAllAccount(int idAccountLogin)
        {
            var common = new Common
            {
                Kind = "getAllAccounts",               
            };

            Utils.SendCommon(common, MainForm.client);
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RequestAccountsOnOffline(MainForm.accountLogin.Id);
            RequestGroupsJoined(MainForm.accountLogin.Id);
            RequestAllAccount(MainForm.accountLogin.Id);
        }
        private void btnAddToGroup_Click(object sender, EventArgs e)
        {
            if (checkAddToGroup.CheckedItems.Count != 0)
            {
                RequestGroupsJoined(MainForm.accountLogin.Id);
                popupAddGroup.ShowDialog();
            }
            else
                MessageBox.Show("Please checked user you need add to group");
        }
        #endregion

        #region Logout
        private void Logout()
        {
            var common = new Common
            {
                Kind = "Logout",
                Content = JsonSerializer.Serialize(MainForm.accountLogin.Id),
            };

            Utils.SendCommon(common, MainForm.client);
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Logout();
            //RequestAccountsOnline(MainForm.accountLogin.Id);

            this.Close();

            ReConnectToServer(); //fix loi khong hien thi lai listBoxOnline = cach nay
            Program.mainForm.Show();
        }
        #endregion

        #region Out Group
        private void OutGroup ()
        {
            var outGroup = new OutGroup
            {
                AccountId = MainForm.accountLogin.Id,
                GroupId = int.Parse(listBoxGroup.GetItemText(listBoxGroup.SelectedValue))
            };

            var common = new Common
            {
                Kind = "OutGroup",
                Content = JsonSerializer.Serialize(outGroup),
            };

            Utils.SendCommon(common, MainForm.client);
        }
        private void btnOutGroup_Click(object sender, EventArgs e)
        {
            if (listBoxGroup.SelectedIndex == -1)
                MessageBox.Show("Pleas choose group you want out");
            else
            {
                OutGroup();
                RequestGroupsJoined(MainForm.accountLogin.Id);
            }
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
        private void MainChat_FormClosing(object sender, FormClosingEventArgs e)
        {
            Utils.KillThread(threadReceive);
        }
        private void UnSelectedListBoxOnline()
        {
            if (oldSelectedIndexOnline == listBoxOnline.SelectedIndex)
            {
                listBoxOnline.ClearSelected();
                oldSelectedIndexOnline = -1;
            }
            else
            {
                oldSelectedIndexOnline = listBoxOnline.SelectedIndex;
            }

            listBoxGroup.ClearSelected();
            oldSelectedIndexGroup = -1;
        }
        private void UnSelectedListBoxGroup()
        {
            if (oldSelectedIndexGroup == listBoxGroup.SelectedIndex)
            {
                listBoxGroup.ClearSelected();
                oldSelectedIndexGroup = -1;
            }
            else
            {
                oldSelectedIndexGroup = listBoxGroup.SelectedIndex;
            }

            listBoxOnline.ClearSelected();
            oldSelectedIndexOnline = -1;
        }
        private void ReConnectToServer()
        {
            try
            {
                //tao connect voi server
                MainForm.ipServer = new IPEndPoint(IPAddress.Parse(MainForm.ip), int.Parse(MainForm.port));
                MainForm.client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                MainForm.client.Connect(MainForm.ipServer);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}
