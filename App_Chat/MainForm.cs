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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_App_Chat
{
    public partial class MainForm : Form
    {
        public static IPEndPoint ipServer;
        public static Socket client;
        public static AccountDto accountLogin;
        public static string ip;
        public static string port;
        private bool isConnectToServer = false;

        public MainForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (!txbIp.Text.Equals(string.Empty) && !txbPort.Text.Equals(string.Empty))
            {
                if(isConnectToServer)
                {
                    PopupRegister popup = new PopupRegister();
                    popup.Show();
                }                   
                else
                {
                    MessageBox.Show("Your connection to server fails \nPlease try to connect again");
                }
            }
            else
            {
                MessageBox.Show("Please connect to server before Register");
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!txbIp.Text.Equals(string.Empty) && !txbPort.Text.Equals(string.Empty))
            {
                if(isConnectToServer)
                {
                    PopupLogin popup = new PopupLogin();
                    popup.Show();
                }
                else
                    MessageBox.Show("Your connection to server fails \nPlease try to connect again");
            }
            else
            {
                MessageBox.Show("Please connect to server before Login");
            }
        }
        public void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                txbIp.Text = Utils.GetLocalIPAddress();
                txbPort.Text = "2008";

                ip = txbIp.Text;
                port = txbPort.Text;

                //tao connect voi server
                ipServer = new IPEndPoint(IPAddress.Parse(txbIp.Text), int.Parse(txbPort.Text));
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(MainForm.ipServer);

                isConnectToServer = true;

                MessageBox.Show("Connect Successful");
            }
            catch (Exception)
            {
                isConnectToServer = false;

                MessageBox.Show("Connect Fails");
                //throw;
            }
        }
    }
}
