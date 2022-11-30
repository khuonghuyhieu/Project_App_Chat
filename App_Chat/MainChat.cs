﻿using ClassLibrary;
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

        public MainChat()
        {
            InitializeComponent();
        }

        private void SendPacketAccountsOnline()
        {        
            var common = new Common
            {
                kind = "getAccountsOnline",              
            };

            Utils.SendCommon(common, MainForm.client);
        }
        
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
                        case "accountsOnlineRes":
                            {
                                var accountsOnline = JsonSerializer.Deserialize<List<string>>(packetRes.content);

                                if (accountsOnline.Any())
                                {
                                    listBoxOnline.DataSource = accountsOnline;
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

        private void MainChat_Load(object sender, EventArgs e)
        {
            //gui goi tin lay cac user dang online
            SendPacketAccountsOnline();

            //tao thread de nhan tin nhan
            threadReceive = new Thread(new ThreadStart(ResponeFromServer));
            threadReceive.IsBackground = true;
            threadReceive.Start();

            string text = listBoxOnline.GetItemText(listBoxOnline.SelectedItem);
        }
        private void MainChat_FormClosing(object sender, FormClosingEventArgs e)
        {
            Utils.KillThread(threadReceive);
        }
    }
}
