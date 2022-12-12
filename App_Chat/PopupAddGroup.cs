using ClassLibrary;
using DTO;
using Project_App_Chat;
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

namespace App_Chat
{
    public partial class PopupAddGroup : Form
    {
        private Thread threadReceive;

        public PopupAddGroup()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
        }
      
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
                        case "AddAccountToGroupRes":
                            {
                                if (packetRes.Content.Equals("AddAccountsToGroupSuccessful"))
                                {
                                    MessageBox.Show("Add accounts to group successful");

                                    this.Close();

                                    return;
                                }

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

        #region Add User to Group
        private void RequestAddUserToGroup(string groupName, List<AccountDto> accountsDto)
        {
            var addAccountsToGroup = new AddAccountsToGroup
            {
                GroupName = groupName,
                Accounts = accountsDto,
            };

            var common = new Common
            {
                Kind = "AddAccountToGroup",
                Content = JsonSerializer.Serialize(addAccountsToGroup)
            };

            Utils.SendCommon(common, MainForm.client);
        }
        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            var accountsDtoChecked = MainChat.checkedListBoxGroup.CheckedItems.Cast<AccountDto>().ToList();

            if (!txbGroupName.Text.Equals(String.Empty))
            {
                RequestAddUserToGroup(txbGroupName.Text, accountsDtoChecked);
                RequestGroupsJoined(MainForm.accountLogin.Id);
            }

            else
                MessageBox.Show("Please type group name you need to add");
        }
        #endregion

        private void comboBoxGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            txbGroupName.Text = comboBoxGroup.GetItemText(comboBoxGroup.SelectedItem);
        }
        private void PopupAddGroup_FormClosing(object sender, FormClosingEventArgs e)
        {
            Utils.KillThread(threadReceive);
        }
        private void PopupAddGroup_Load(object sender, EventArgs e)
        {
            //tao thread de nhan tin nhan
            threadReceive = new Thread(new ThreadStart(ResponeFromServer));
            threadReceive.IsBackground = true;
            threadReceive.Start();
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
        private void RequestGroupsJoined(int idAccountLogin)
        {
            var common = new Common
            {
                Kind = "getGroupsJoined",
                Content = JsonSerializer.Serialize(idAccountLogin),
            };

            Utils.SendCommon(common, MainForm.client);
        }
    }
}
