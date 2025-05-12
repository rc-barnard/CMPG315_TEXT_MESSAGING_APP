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
using System.Net.Sockets; //Using sockets to create a TCP connection.

namespace ChatApp
{
    public partial class ConnectToServer : Form
    {
        //Create a TcpClient object to connect to the server.
        TcpClient client;
        //Create a NetworkStream object to send and receive data from the server.
        NetworkStream stream;
        //Create a Thread object to receive incoming messages from the server.
        Thread receiveIncommingMessages;

        //Constructor to initialize the ChatScreen form with TcpClient, NetworkStream, and thread in order to use a continious client, stream etc.
        public ConnectToServer(TcpClient client, NetworkStream stream, Thread receiveIncommingMessages)
        {
            InitializeComponent();
            this.client = client;
            this.stream = stream;
            this.receiveIncommingMessages = receiveIncommingMessages;
        }

        private void btnContinueToChat_Click(object sender, EventArgs e)
        {
            //Check if the username is not empty and does not exceed 15 characters.
            if (txtUsername.Text.Length != 0 & !(txtUsername.Text.Length > 15))
            {
                string username = txtUsername.Text.ToString().ToUpper(); //Convert the username to uppercase for consistency.
                byte[] usernameLength = BitConverter.GetBytes(username.Length);
                stream.Write(usernameLength, 0, usernameLength.Length); //Send the length of the username to the server for accuracy.
                byte[] usernameBuffer = Encoding.UTF8.GetBytes(username);
                stream.Write(usernameBuffer, 0, usernameBuffer.Length); //Send the username to the server.

                //Open the ChatScreen form when the button is clicked, to enter chat.
                ChatScreen myChat = new ChatScreen(client,stream,receiveIncommingMessages,username);
                myChat.Show();
                this.Hide(); //Hide the current form to show the next form.
            }
            else
            {
                MessageBox.Show("Please enter a username with 14 characters max!"); //Message to inform the user that the username is invalid.
            }
        }

        private void ConnectToServer_Load(object sender, EventArgs e)
        {

        }
    }
}
