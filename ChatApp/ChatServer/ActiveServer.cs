using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    public partial class ActiveServer : Form
    {
        private static int PORTNUMBER = 3258; //constant
        private static Dictionary<string, TcpClient> appClients = new Dictionary<string, TcpClient>(); //store clients in a dictionary
        private static TcpListener chatServer = null;
        private volatile bool isRunning = true;

        public ActiveServer()
        {
            InitializeComponent();
        }

        public void addToListBox(string message)//update listbox when a new message is received
        {
            if (lstServerActivity.InvokeRequired)
            {
                lstServerActivity.Invoke((MethodInvoker)(() => lstServerActivity.Items.Add("")));
                lstServerActivity.Invoke((MethodInvoker)(() => lstServerActivity.Items.Add(message)));
            }
            else
            {
                lstServerActivity.Items.Add("");
                lstServerActivity.Items.Add(message);
            }
        }

        public void mainSeverOperations(int PORTNUMBER)
        {
            chatServer = new TcpListener(IPAddress.Any, PORTNUMBER);//create TCP object that listens on specified port and any ip address
            chatServer.Start();//start server
            addToListBox("Chat Server started on port: " + PORTNUMBER);
            while (isRunning)
            {
                if (!chatServer.Pending())
                {
                    Thread.Sleep(1000); // Sleep for 1 second if no client is pending to save cpu resources.
                    continue; // Continue to check for new clients.
                }

                try
                {
                    TcpClient client = chatServer.AcceptTcpClient();//accept connection
                    addToListBox("Unknown client connected: " + client.Client.RemoteEndPoint.ToString());//show client connected
                    Thread clientThread = new Thread(() => manageClient(client));
                    clientThread.Start();
                }
                catch (Exception error)
                {
                    addToListBox("Error starting server: " + error.Message);
                    break;
                }
            }
            chatServer.Stop();
        }

        public void serverMessage(string message)
        {
            byte[] messageBufferToSend = Encoding.UTF8.GetBytes("SYSTEM: " + message);

            lock (appClients)//ensure thread safety 
            {
                foreach (var client in appClients.Values)//iterate through clients in dictionary
                {
                    try
                    {
                        NetworkStream messageStream = client.GetStream();//get all the clients connection information
                        messageStream.Write(messageBufferToSend, 0, messageBufferToSend.Length);
                    }
                    catch (Exception error)
                    {
                        addToListBox("Error broadcasting system message: " + error.Message);
                    }
                }
            }
            addToListBox("System note: " + message);//notify the server operator of changes
        }

        public void manageClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();//get client networkstream to read/write data
            byte[] streamBuffer = new byte[1024];//create message buffer

            int noOfPasswordBytes = stream.Read(streamBuffer, 0, 12);//set password buffer(12 bytes)
            string accessPassword = Encoding.ASCII.GetString(streamBuffer, 0, noOfPasswordBytes);

            if (noOfPasswordBytes != 0 && accessPassword == "CMPG@315PROJ")//authenticate password
            {
                addToListBox("Client authenticated: " + client.Client.RemoteEndPoint.ToString());
                stream.WriteByte(1);

                byte[] usernameLengthBuffer = new byte[4];
                stream.Read(usernameLengthBuffer, 0, usernameLengthBuffer.Length);
                int usernameLength = BitConverter.ToInt32(usernameLengthBuffer, 0);
                byte[] usernameBuffer = new byte[usernameLength];
                stream.Read(usernameBuffer, 0, usernameBuffer.Length);
                string username = Encoding.UTF8.GetString(usernameBuffer);

                lock (appClients)
                {
                    appClients[username] = client;//add client to dictionary
                }
                addToListBox("Client with IP: " + client.Client.RemoteEndPoint.ToString() + " registered successfully with username " + username);//notify server operator of connected client

                //Broadcast to clients that a new user has joined
                serverMessage(username + " has joined the chat...");

                try
                {
                    int NoBytesToRead = 0;
                    string messageToSend = "";

                    while (true)//create loop to read messages
                    {
                        //Read the message from the client.
                        NoBytesToRead = stream.Read(streamBuffer, 0, streamBuffer.Length);
                        if (NoBytesToRead == 0) return;
                        string message = Encoding.UTF8.GetString(streamBuffer, 0, NoBytesToRead);

                        if (message.ToLower() == "xxx") //if client wants to exit the server.
                        {
                            string exitMessage = "Client with username " + username + " exiting the Chat Server...";
                            addToListBox(exitMessage);
                            byte[] exitMessageBuffer = Encoding.UTF8.GetBytes(exitMessage);
                            stream.Write(exitMessageBuffer, 0, exitMessageBuffer.Length);
                            message = "System note: " + username + " left the Chat...";
                            message = "SYSTEM: " + username + " left the Chat...";

                            lock (appClients) //ensure multiple actions on appClients.
                            {
                                appClients.Remove(username);//remove disconnected user
                            }
                        }

                        if (message.StartsWith("@")) //used to send direct message, example: @Bob: Hello Bob!
                        {
                            string intendedRecipient = "";

                            //If direct message, i.e. message starts with @, extract the intended recipient and the message to send from the received message.
                            int indexOfDistinquisher = message.IndexOf(":");
                            intendedRecipient = message.Substring(1, indexOfDistinquisher - 1).Trim();
                            messageToSend = message.Substring(indexOfDistinquisher + 1).Trim();

                            lock (appClients) //ensure multiple actions on appClients.
                            {
                                if (appClients.ContainsKey(intendedRecipient)) //if intended recipient is on the server send message.
                                {
                                    byte[] messageBufferToSend = Encoding.UTF8.GetBytes("DM received from " + username + ": " + messageToSend);
                                    TcpClient messageReceiver = appClients[intendedRecipient];
                                    NetworkStream messageStream = messageReceiver.GetStream();
                                    messageStream.Write(messageBufferToSend, 0, messageBufferToSend.Length);
                                    addToListBox("Direct Message (DM) sent successfully from " + username + " to " + intendedRecipient);
                                }
                                else
                                {
                                    string errorMessage = "Failed to send direct message intended for client " + intendedRecipient + " due to unavailability (user is not in the server)";
                                    byte[] errorMessageBufferToSend = Encoding.UTF8.GetBytes(errorMessage);
                                    TcpClient messageSender = client;
                                    NetworkStream errorMessageStream = messageSender.GetStream();
                                    errorMessageStream.Write(errorMessageBufferToSend, 0, errorMessageBufferToSend.Length);
                                    addToListBox(errorMessage);
                                }
                            }
                        }
                        else
                        {
                            messageToSend = message;
                            byte[] messageBufferToSend = null;
                            if (message.StartsWith("Note:"))
                            {
                                messageBufferToSend = Encoding.UTF8.GetBytes(message); //convert the message to a byte array.
                            }
                            else
                            {
                                messageBufferToSend = Encoding.UTF8.GetBytes(username + ": " + message); //convert the message to a byte array.
                            }

                            lock (appClients) //ensure multiple actions on appClients.
                            {
                                for (int i = 0; i < appClients.Count; i++) //traverse each client connected to the server.
                                {
                                    TcpClient messageReceiver = appClients.Values.ElementAt(i);
                                    string receiverUsername = appClients.Keys.ElementAt(i);

                                    if (messageReceiver == client)
                                    {
                                        continue; //do not send own message to the sender.
                                    }
                                    else
                                    {
                                        try
                                        {
                                            NetworkStream messageStream = messageReceiver.GetStream();
                                            messageStream.Write(messageBufferToSend, 0, messageBufferToSend.Length);
                                        }
                                        catch (Exception error)
                                        {
                                            addToListBox("An error occurred sending the message to user @" + username + ": " + error.Message);
                                        }
                                    }
                                }
                                //Check if message is sent by the server, filters unnecessary server processes
                                if (message.StartsWith("Note:"))
                                {
                                }
                                else
                                {
                                    addToListBox("Message sent successfully from " + username + " to all connected users...");//send client messages to all other users
                                }
                            }
                        }
                        //Check if message is sent by the server, filters unnecessary server processes
                        if (message.StartsWith("Note:"))
                        {
                        }
                        else
                        {
                            addToListBox("Message '" + messageToSend + "' received from " + username);//display the message received from the client.
                        }
                    }
                }
                catch (Exception error)
                {
                    addToListBox("An error occurred while reading incomming message stream from client @" + username + ": " + error.Message);
                }
                finally
                {
                    //Remove connected client from the dictionary if it disconnects.
                    lock (appClients) //ensure multiple actions on appClients.
                    {
                        if (appClients.ContainsKey(username)) appClients.Remove(username);
                    }

                    try
                    {
                        stream.Close();
                    }
                    catch (Exception error)
                    {
                        addToListBox("An error occurred while closing the stream for client @" + username + ": " + error.Message);
                    }

                    try
                    {
                        client.Close();
                    }
                    catch (Exception error)
                    {
                        addToListBox("An error occurred while closing the client @" + username + ": " + error.Message);
                    }

                    addToListBox("Client with username " + username + " disconnected from the Chat Server.");
                }
            }
            else
            {
                addToListBox("Client authentication failed: " + client.Client.RemoteEndPoint.ToString());
                try { stream.Close(); } catch { }
                try { client.Close(); } catch { }
                return;
            }
        }

        private void ActiveServer_Load(object sender, EventArgs e)
        {
            Thread serverThread = new Thread(() => Task.Run(() => mainSeverOperations(3258)));
            serverThread.IsBackground = true;
            serverThread.Start();
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            isRunning = false;
            try { chatServer.Stop(); } catch { }
            Environment.Exit(0);
        }
    }
}