using System;
using System.Windows.Forms;

namespace Project_App_Chat
{
    public partial class PopupLogin : Form
    {
        public PopupLogin()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.mainForm.Hide();
            this.Close();

            var mainChat = new MainChat();
            mainChat.Show();

            
        }
    }
}
