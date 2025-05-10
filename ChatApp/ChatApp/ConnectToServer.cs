using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace ChatApp
{
    public partial class ConnectToServer : Form
    {
        TcpClient client;
        NetworkStream stream;
        Thread receiveIncommingMessages;

        public ConnectToServer(TcpClient client, NetworkStream stream, Thread receiveIncommingMessages)
        {
            InitializeComponent();
            this.client = client;
            this.stream = stream;
            this.receiveIncommingMessages = receiveIncommingMessages;
        }

        private void btnContinueToChat_Click(object sender, EventArgs e)
        {
            if(txtUsername.Text.Length != 0 & !(txtUsername.Text.Length > 15))
            {
                string username = txtUsername.Text.ToString().ToUpper();
                byte[] usernameLength = BitConverter.GetBytes(username.Length);
                stream.Write(usernameLength, 0, usernameLength.Length);
                byte[] usernameBuffer = Encoding.UTF8.GetBytes(username);
                stream.Write(usernameBuffer, 0, usernameBuffer.Length);
                ChatScreen myChat = new ChatScreen(client,stream,receiveIncommingMessages,username);
                myChat.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Please enter a username with 14 characters max!");
            }
        }
    }
}
