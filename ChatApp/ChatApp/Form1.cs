using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace ChatApp
{
    public partial class Form1 : Form
    {
        public static TcpClient client;
        public static NetworkStream stream;
        public static Thread receiveMessagesThread;

        public static string username = "";
        public static string accessPassword = "CMPG@315PROJ";


        public Form1()
        {
            InitializeComponent();
        }

        public async Task authenticateClientWithServer(string IpAddress, int portNumber)
        {
            client = new TcpClient(IpAddress,portNumber);
            stream = client.GetStream();

            byte[] accessPasswordBuffer = Encoding.ASCII.GetBytes(accessPassword);
            await stream.WriteAsync(accessPasswordBuffer, 0, accessPassword.Length);

            if (await receiveAuthentication() == true)
            {
                MessageBox.Show("Connected to the Chat Server succesfully!");
                ConnectToServer connectToServer = new ConnectToServer(client, stream, receiveMessagesThread);
                connectToServer.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Failed to connect to the Chat Server!");
            }
        }

        public async Task<bool> receiveAuthentication()
        {
            try
            {
                byte[] authenticationSuccess = new byte[1];
                await stream.ReadAsync(authenticationSuccess, 0, 1);
                if (authenticationSuccess[0] == 1)
                {
                    MessageBox.Show("Authentication Successful");
                    return true;
                }
                else
                {
                    MessageBox.Show("Authentication Failed");
                    return false;
                }

            }
            catch (Exception error)
            {
                MessageBox.Show("Authentication Error: "+error.Message);
                return false;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private async void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void btnEnterChat_Click(object sender, EventArgs e)
        {
            await authenticateClientWithServer("28.ip.gl.ply.gg", 19073);
        }
    }
}
