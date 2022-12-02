namespace Server
{
    partial class ServerForm
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.txbPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txbIp = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txbKhungHoatDong = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(301, 32);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(79, 32);
            this.btnStart.TabIndex = 14;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txbPort
            // 
            this.txbPort.Location = new System.Drawing.Point(129, 53);
            this.txbPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txbPort.Name = "txbPort";
            this.txbPort.Size = new System.Drawing.Size(148, 23);
            this.txbPort.TabIndex = 13;
            this.txbPort.Text = "2008";
            this.txbPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 61);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 15);
            this.label4.TabIndex = 12;
            this.label4.Text = "Port kết nối:";
            // 
            // txbIp
            // 
            this.txbIp.Location = new System.Drawing.Point(129, 12);
            this.txbIp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txbIp.Name = "txbIp";
            this.txbIp.Size = new System.Drawing.Size(148, 23);
            this.txbIp.TabIndex = 11;
            this.txbIp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "Địa chỉ IP Server:";
            // 
            // txbKhungHoatDong
            // 
            this.txbKhungHoatDong.Location = new System.Drawing.Point(11, 99);
            this.txbKhungHoatDong.Multiline = true;
            this.txbKhungHoatDong.Name = "txbKhungHoatDong";
            this.txbKhungHoatDong.Size = new System.Drawing.Size(777, 339);
            this.txbKhungHoatDong.TabIndex = 15;
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txbKhungHoatDong);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txbPort);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txbIp);
            this.Controls.Add(this.label3);
            this.Name = "ServerForm";
            this.Text = "Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnStart;
        private TextBox txbPort;
        private Label label4;
        private TextBox txbIp;
        private Label label3;
        private TextBox txbKhungHoatDong;
    }
}