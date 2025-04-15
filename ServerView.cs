using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerApp
{
    public partial class ServerView : Form
    {
        private static int portNumber = 3258;
        private static Dictionary<string, TcpClient> appClients = new Dictionary<string, TcpClient>();
        private static TcpListener appServer = null;
        private static bool isRunning = true;

        public ServerView()
        {
            InitializeComponent();
        }

        public void addToListBox(string message)
        {
            if (listBox1.InvokeRequired)
            {
		listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("")));
                listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add(message)));
            }
            else
            {
                listBox1.Items.Add("");
                listBox1.Items.Add(message);
            }
        }

        public void serverOperations(int portNumber)
        {
            appServer = new TcpListener(IPAddress.Any, portNumber);
            appServer.Start();
            addToListBox($"Server started on port {portNumber}.");

            while (isRunning)
            {
                try
                {
                    TcpClient client = appServer.AcceptTcpClient();
                    addToListBox("\n- - - App Client connected - - -");
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.Start();
                }
                catch (SocketException ex)
                {
                    addToListBox($"An error occured adding client: {ex.Message}");
                    isRunning = false;
                }
            }
            appServer.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        public void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] streamBuffer = new byte[1024];
	    string messageToSend = "";

            //First read the username provided by the client for texting identification.
            int NoBytesToRead = stream.Read(streamBuffer, 0, streamBuffer.Length);
            string username = Encoding.ASCII.GetString(streamBuffer, 0, NoBytesToRead);

            //Add new client with its username to the dictionary.
            lock (appClients) //To ensure multiple actions on appClients.
            {
                appClients[username] = client;
            }

            addToListBox("\nClient with username " + username + " connected to Server successfully...");
        }

        private void ServerView_Load(object sender, EventArgs e)
        {
            Thread serverThread = new Thread(() => serverOperations(portNumber));
            serverThread.IsBackground = true;
            serverThread.Start();
        }
    }
}
