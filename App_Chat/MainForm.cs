using ClassLibrary;
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
        public MainForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            PopupRegister popup = new PopupRegister();
            popup.Show();

        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            PopupLogin popup = new PopupLogin();
            popup.Show();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            //tao connect voi server
            ipServer = new IPEndPoint(IPAddress.Parse(Utils.GetLocalIPAddress()), int.Parse("2008"));
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(ipServer);
        }
    }
}
