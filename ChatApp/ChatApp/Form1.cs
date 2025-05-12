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
        //Create a TcpClient object to connect to the server.
        public static TcpClient client;
        //Create a NetworkStream object to send and receive data from the server.
        public static NetworkStream stream;
        //Create a Thread object to receive incoming messages from the server.
        public static Thread receiveMessagesThread;

        //Variable to hold user's username for communication.
        public static string username = "";
        public static string accessPassword = "CMPG@315PROJ"; //Password used for authenticating the user to the server.


        public Form1()
        {
            InitializeComponent();
        }


        //Asynchronous method to authenticate the client with the server.
        public async Task authenticateClientWithServer(string IpAddress, int portNumber) //Receive server IP address and port number as parameters.
        {
            client = new TcpClient(IpAddress,portNumber); //Create a new TcpClient object to connect to the server using the provided IP address and port number.
            stream = client.GetStream(); //Get the network stream for communication with the server.

            byte[] accessPasswordBuffer = Encoding.ASCII.GetBytes(accessPassword);
            await stream.WriteAsync(accessPasswordBuffer, 0, accessPassword.Length); //Write the password to the server

            if (await receiveAuthentication() == true) //If the authentication is successful on the server, proceed to the chat screen.
            {
                MessageBox.Show("Connected to the Chat Server succesfully!"); //Message to inform the user that the connection is successful.
                //Open screen to enter username and continue to the chat.
                ConnectToServer connectToServer = new ConnectToServer(client, stream, receiveMessagesThread);
                connectToServer.Show();
                this.Hide(); //Hide the current form to show the next form.
            }
            else
            {
                MessageBox.Show("Failed to connect to the Chat Server!"); //Message to inform the user that the connection failed.
            }
        }

        //Asynchronous method to receive authentication response from the server.
        public async Task<bool> receiveAuthentication()
        {
            try//Try catch statement to ensure proper exception handling.
            {
                byte[] authenticationSuccess = new byte[1];
                await stream.ReadAsync(authenticationSuccess, 0, 1); //Read the authentication response from the server.
                if (authenticationSuccess[0] == 1) //If true, the authentication is successfull.
                {
                    MessageBox.Show("Authentication Successful"); //Message to inform the user that the authentication is successful.
                    return true;
                }
                else
                {
                    MessageBox.Show("Authentication Failed"); //Message to inform the user that the authentication failed.
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
            //Authenticate the client with the server using the provided server IP address and port number.
            //SERVER ON WYNANDN'S MACHINE = await authenticateClientWithServer("28.ip.gl.ply.gg", 20931);
            await authenticateClientWithServer("28.ip.gl.ply.gg", 19073);
        }
    }
}
