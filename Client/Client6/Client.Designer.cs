namespace Client6
{
    partial class ClientForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txbUserName = new System.Windows.Forms.TextBox();
            this.txbPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txbIp = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txbPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRegister = new System.Windows.Forms.Button();
            this.txbSendToUser = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txbChat = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.rabtnUser = new System.Windows.Forms.RadioButton();
            this.rabtnGroup = new System.Windows.Forms.RadioButton();
            this.txbSendToGroup = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txbKhungChat = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txbGroupName = new System.Windows.Forms.TextBox();
            this.btnJoinGroup = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(314, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "User name:";
            // 
            // txbUserName
            // 
            this.txbUserName.Location = new System.Drawing.Point(393, 17);
            this.txbUserName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txbUserName.Name = "txbUserName";
            this.txbUserName.Size = new System.Drawing.Size(131, 23);
            this.txbUserName.TabIndex = 1;
            // 
            // txbPassword
            // 
            this.txbPassword.Location = new System.Drawing.Point(393, 60);
            this.txbPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txbPassword.Name = "txbPassword";
            this.txbPassword.Size = new System.Drawing.Size(131, 23);
            this.txbPassword.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(314, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password:";
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.SystemColors.Control;
            this.btnLogin.Location = new System.Drawing.Point(627, 11);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(79, 32);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txbIp
            // 
            this.txbIp.Location = new System.Drawing.Point(126, 17);
            this.txbIp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txbIp.Name = "txbIp";
            this.txbIp.Size = new System.Drawing.Size(148, 23);
            this.txbIp.TabIndex = 6;
            this.txbIp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 25);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Địa chỉ IP Server:";
            // 
            // txbPort
            // 
            this.txbPort.Location = new System.Drawing.Point(126, 58);
            this.txbPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txbPort.Name = "txbPort";
            this.txbPort.Size = new System.Drawing.Size(148, 23);
            this.txbPort.TabIndex = 8;
            this.txbPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 66);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Port kết nối:";
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(627, 90);
            this.btnRegister.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(79, 32);
            this.btnRegister.TabIndex = 10;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // txbSendToUser
            // 
            this.txbSendToUser.Location = new System.Drawing.Point(121, 22);
            this.txbSendToUser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txbSendToUser.Name = "txbSendToUser";
            this.txbSendToUser.Size = new System.Drawing.Size(148, 23);
            this.txbSendToUser.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 30);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 15);
            this.label5.TabIndex = 12;
            this.label5.Text = "Send to user:";
            // 
            // txbChat
            // 
            this.txbChat.Location = new System.Drawing.Point(14, 512);
            this.txbChat.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txbChat.Multiline = true;
            this.txbChat.Name = "txbChat";
            this.txbChat.Size = new System.Drawing.Size(793, 36);
            this.txbChat.TabIndex = 14;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(816, 512);
            this.btnSend.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(104, 37);
            this.btnSend.TabIndex = 15;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.tbnSend_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(627, 51);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(79, 32);
            this.btnLogout.TabIndex = 16;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // rabtnUser
            // 
            this.rabtnUser.AutoSize = true;
            this.rabtnUser.Checked = true;
            this.rabtnUser.Location = new System.Drawing.Point(276, 26);
            this.rabtnUser.Name = "rabtnUser";
            this.rabtnUser.Size = new System.Drawing.Size(14, 13);
            this.rabtnUser.TabIndex = 17;
            this.rabtnUser.TabStop = true;
            this.rabtnUser.UseVisualStyleBackColor = true;
            // 
            // rabtnGroup
            // 
            this.rabtnGroup.AutoSize = true;
            this.rabtnGroup.Location = new System.Drawing.Point(599, 26);
            this.rabtnGroup.Name = "rabtnGroup";
            this.rabtnGroup.Size = new System.Drawing.Size(14, 13);
            this.rabtnGroup.TabIndex = 20;
            this.rabtnGroup.UseVisualStyleBackColor = true;
            // 
            // txbSendToGroup
            // 
            this.txbSendToGroup.Location = new System.Drawing.Point(444, 22);
            this.txbSendToGroup.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txbSendToGroup.Name = "txbSendToGroup";
            this.txbSendToGroup.Size = new System.Drawing.Size(148, 23);
            this.txbSendToGroup.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(332, 30);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 15);
            this.label6.TabIndex = 18;
            this.label6.Text = "Send to group:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rabtnGroup);
            this.groupBox1.Controls.Add(this.txbSendToGroup);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.rabtnUser);
            this.groupBox1.Controls.Add(this.txbSendToUser);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(12, 128);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(628, 55);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Send:";
            // 
            // txbKhungChat
            // 
            this.txbKhungChat.Location = new System.Drawing.Point(12, 189);
            this.txbKhungChat.Multiline = true;
            this.txbKhungChat.Name = "txbKhungChat";
            this.txbKhungChat.Size = new System.Drawing.Size(795, 317);
            this.txbKhungChat.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(645, 160);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 15);
            this.label7.TabIndex = 23;
            this.label7.Text = "Group Name:";
            // 
            // txbGroupName
            // 
            this.txbGroupName.Location = new System.Drawing.Point(729, 157);
            this.txbGroupName.Name = "txbGroupName";
            this.txbGroupName.Size = new System.Drawing.Size(119, 23);
            this.txbGroupName.TabIndex = 24;
            // 
            // btnJoinGroup
            // 
            this.btnJoinGroup.Location = new System.Drawing.Point(854, 151);
            this.btnJoinGroup.Name = "btnJoinGroup";
            this.btnJoinGroup.Size = new System.Drawing.Size(80, 32);
            this.btnJoinGroup.TabIndex = 25;
            this.btnJoinGroup.Text = "Join Group";
            this.btnJoinGroup.UseVisualStyleBackColor = true;
            this.btnJoinGroup.Click += new System.EventHandler(this.btnJoinGroup_Click);
            // 
            // ClientForm
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 575);
            this.Controls.Add(this.btnJoinGroup);
            this.Controls.Add(this.txbGroupName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txbKhungChat);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txbChat);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.txbPort);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txbIp);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txbPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txbUserName);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ClientForm";
            this.Text = "Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientForm_FormClosing);
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbUserName;
        private System.Windows.Forms.TextBox txbPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txbIp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.TextBox txbSendToUser;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txbChat;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnLogout;
        private RadioButton rabtnUser;
        private RadioButton rabtnGroup;
        private TextBox txbSendToGroup;
        private Label label6;
        private GroupBox groupBox1;
        private TextBox txbKhungChat;
        private Label label7;
        private TextBox txbGroupName;
        private Button btnJoinGroup;
    }
}