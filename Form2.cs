using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace myOpenGL
{
    public partial class Form2 : Form
    {
        TcpClient client;
        StreamReader STR;
        StreamWriter STW;
        string recive;
        string send;
        string myip;
        int PORT = 9034;
        Color defcolor;
        TcpListener listener;
        bool brek = false;
        Thread ctThread;

        public Form2()
        {
            InitializeComponent();
            listener = new TcpListener(IPAddress.Any, PORT);
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName()); //get my ip
            foreach (IPAddress address in localIP)
	        {
		        if (address.AddressFamily == AddressFamily.InterNetwork)
	            {
                    myip = address.ToString();
		            label1myIP.Text = "My IP: " + myip;
	            }
	        }
            defcolor = button1Server.BackColor;
        }

        private void button1Server_Click(object sender, EventArgs e) //start server
        {
            server();
            //ctThread = new Thread(server);
            //if (!brek)
            //{
            //    button1Server.BackColor = Color.LightYellow;
            //    button1Server.Text = "Waiting client...";
            //    button2Connect.Enabled = false;
            //    ctThread.Start();
            //}
            //else
            //{
            //    listener.Stop();
            //    ctThread.Abort();
            //    button2Connect.Enabled = true;
            //    button1Server.BackColor = defcolor;
            //    button1Server.Text = "Start Server";
            //    OpenGL.cOGL.networkconnected = false;
            //}
        }

        void server()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, PORT);
            listener.Start();
            client = listener.AcceptTcpClient();
            STR = new StreamReader(client.GetStream());
            STW = new StreamWriter(client.GetStream());
            STW.AutoFlush = true;

            backgroundWorker1.RunWorkerAsync();
            backgroundWorker2.WorkerSupportsCancellation = true;

            serverconnected(true);
        }

        private void button2Connect_Click(object sender, EventArgs e) //connect client to server
        {
            client = new TcpClient();
            IPEndPoint IP_End = new IPEndPoint(IPAddress.Parse(textBox1IP.Text), PORT);

            try
            {
                client.Connect(IP_End);
                if (client.Connected)
                {
                    STW = new StreamWriter(client.GetStream());
                    STR = new StreamReader(client.GetStream());
                    STW.AutoFlush = true;
                    button2Connect.BackColor = Color.LightGreen;
                    button2Connect.Text = "Disconnect";

                    backgroundWorker1.RunWorkerAsync();
                    backgroundWorker2.WorkerSupportsCancellation = true;

                    clientconnected(true);
                }
                else
                {
                    clientconnected(false);
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message.ToString(), "", MessageBoxButtons.OK);
                clientconnected(false);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) //receive data
        {
            while (client.Connected)
            {
                try
                {
                    recive = STR.ReadLine();
                    this.textBoxChat.Invoke(new MethodInvoker(delegate () { textBoxChat.AppendText("You: " + recive + "\n"); }));
                    OpenGL.cOGL.enemytank.stringToTank(recive);
                    recive = "";
                }
                catch (Exception x)
                {
                    clientconnected(false);
                    MessageBox.Show(x.Message.ToString(), "", MessageBoxButtons.OK);
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e) //send data
        {
            if (client.Connected)
            {
                //posx,posz,rotation,torretrotation
                string str = OpenGL.cOGL.tank.tankToString();
                STW.WriteLine(str);
                this.textBoxChat.Invoke(new MethodInvoker(delegate () { textBoxChat.AppendText("Me: " + send + "\n"); }));
            }
            else
            {
                MessageBox.Show("Send failed !", "", MessageBoxButtons.OK);
                serverconnected(false);
            }
            backgroundWorker2.CancelAsync();
        }

        private void button1Close_Click(object sender, EventArgs e)
        {
            this.Hide();
            Cursor.Hide();
        }

        bool IsValidIp(string addr)
        {
            IPAddress ip;
            bool valid = !string.IsNullOrEmpty(addr) && IPAddress.TryParse(addr, out ip);
            return valid;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            send = OpenGL.cOGL.tank.tankToString();
            backgroundWorker2.RunWorkerAsync();
        }

        void serverconnected(bool con)
        {
            if (con)
            {
                button1Server.Text = "Disconnect";
                button1Server.BackColor = Color.LightGreen;
                button2Connect.Enabled = false;
                timer1.Enabled = true;
                OpenGL.cOGL.networkconnected = true;
            }
            else
            {
                button1Server.Text = "Start Server";
                button1Server.BackColor = defcolor;
                button2Connect.Enabled = true;
                timer1.Enabled = false;
                OpenGL.cOGL.networkconnected = false;
            }
        }
        void clientconnected(bool con)
        {
            if (con)
            {
                button2Connect.Text = "Disconnect";
                button2Connect.BackColor = Color.LightGreen;
                button1Server.Enabled = false;
                timer1.Enabled = true;
                OpenGL.cOGL.networkconnected = true;
            }
            else
            {
                button2Connect.Text = "Connect";
                button2Connect.BackColor = defcolor;
                button1Server.Enabled = true;
                timer1.Enabled = false;
                OpenGL.cOGL.networkconnected = false;
            }
        }
    }
}
