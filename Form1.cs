using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets; //To access ports & IPAddresses
using System.Threading; //Using Threads to ensure that multiple actions is allowed, like receivng multiple messages.

//textapp
namespace TextMessagingApp
{
    public partial class Form1 : Form
    {
        //Create new TCP client to connect to the server/
        public static TcpClient client;
        //Create a new NetworkStream to sent and receive message to and from the server.
        public static NetworkStream stream;
	//Create thread to receive multiple messages from the server.
        public static Thread receiveMessagesThread;

        public static string username = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
	
	//Method to register client with server, once a client opens the app, they are prompt to enter a username, where the username is immediately sent to the server 
	//and recorded in the Dictionary as a key, and the client is recorder as the value.
        public void registerClientWithServer(string IpAddress, int portNumber) //Receive server IPAddress and the port where the server listens on.
        {
            client = new TcpClient(IpAddress, portNumber); //Create a new TCP Client instance to the server.
            stream = client.GetStream(); //Initialize the network stream to send the username to the server.
            username = txtUsername.Text; //Get username

            byte[] usernameBufferToSend = Encoding.UTF8.GetBytes(username); //Create a byte array to hold the username as a sequence of bytes.
            stream.Write(usernameBufferToSend, 0, usernameBufferToSend.Length); //Send the username to the server for registration.

            MessageBox.Show("- - - Connected to server successfully - - -"); //Indicate that the client is connected to the server successfully.
        }

	private void button1_Click(object sender, EventArgs e)
        {
		registerClientWithServer("127.0.0.1",3258); //Once user hits Enter the Chat, the user is immediately registered with the chat server.
        }

        private void Form1_Load(object sender, EventArgs e)
        {
		
        }
    }
}
