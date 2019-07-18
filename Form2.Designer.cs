namespace myOpenGL
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
            this.components = new System.ComponentModel.Container();
            this.button1Server = new System.Windows.Forms.Button();
            this.button2Connect = new System.Windows.Forms.Button();
            this.textBox1IP = new System.Windows.Forms.TextBox();
            this.label1myIP = new System.Windows.Forms.Label();
            this.button1Close = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.textBoxChat = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // button1Server
            // 
            this.button1Server.Location = new System.Drawing.Point(221, 12);
            this.button1Server.Name = "button1Server";
            this.button1Server.Size = new System.Drawing.Size(103, 53);
            this.button1Server.TabIndex = 0;
            this.button1Server.Text = "Start Server";
            this.button1Server.UseVisualStyleBackColor = true;
            this.button1Server.Click += new System.EventHandler(this.button1Server_Click);
            // 
            // button2Connect
            // 
            this.button2Connect.Location = new System.Drawing.Point(221, 85);
            this.button2Connect.Name = "button2Connect";
            this.button2Connect.Size = new System.Drawing.Size(103, 53);
            this.button2Connect.TabIndex = 1;
            this.button2Connect.Text = "Connect";
            this.button2Connect.UseVisualStyleBackColor = true;
            this.button2Connect.Click += new System.EventHandler(this.button2Connect_Click);
            // 
            // textBox1IP
            // 
            this.textBox1IP.Location = new System.Drawing.Point(77, 102);
            this.textBox1IP.Name = "textBox1IP";
            this.textBox1IP.Size = new System.Drawing.Size(100, 20);
            this.textBox1IP.TabIndex = 2;
            // 
            // label1myIP
            // 
            this.label1myIP.AutoSize = true;
            this.label1myIP.Location = new System.Drawing.Point(29, 32);
            this.label1myIP.Name = "label1myIP";
            this.label1myIP.Size = new System.Drawing.Size(40, 13);
            this.label1myIP.TabIndex = 4;
            this.label1myIP.Text = "My IP: ";
            // 
            // button1Close
            // 
            this.button1Close.Location = new System.Drawing.Point(106, 311);
            this.button1Close.Name = "button1Close";
            this.button1Close.Size = new System.Drawing.Size(103, 31);
            this.button1Close.TabIndex = 5;
            this.button1Close.Text = "Close";
            this.button1Close.UseVisualStyleBackColor = true;
            this.button1Close.Click += new System.EventHandler(this.button1Close_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            // 
            // textBoxChat
            // 
            this.textBoxChat.Location = new System.Drawing.Point(12, 157);
            this.textBoxChat.Multiline = true;
            this.textBoxChat.Name = "textBoxChat";
            this.textBoxChat.Size = new System.Drawing.Size(312, 133);
            this.textBoxChat.TabIndex = 7;
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 354);
            this.Controls.Add(this.textBoxChat);
            this.Controls.Add(this.button1Close);
            this.Controls.Add(this.label1myIP);
            this.Controls.Add(this.textBox1IP);
            this.Controls.Add(this.button2Connect);
            this.Controls.Add(this.button1Server);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1Server;
        private System.Windows.Forms.Button button2Connect;
        private System.Windows.Forms.TextBox textBox1IP;
        private System.Windows.Forms.Label label1myIP;
        private System.Windows.Forms.Button button1Close;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.TextBox textBoxChat;
        private System.Windows.Forms.Timer timer1;
    }
}