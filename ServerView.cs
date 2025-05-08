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
        private static int portNumber = 8080;
        private static Dictionary<string, TcpClient> appClients = new Dictionary<string, TcpClient>();
        private static TcpListener appServer = null;
        private static bool isRunning = true;
        private static String sharedPassword = "CMPG@315PROJ";
        private static Dictionary<string, List<DateTime>> connectionAttempts = new Dictionary<string, List<DateTime>>();
        private static int maxAttemptsPerMinute = 5; // Maximum connection attempts allowed per minute per IP
        private static readonly object lockObject = new object(); // For thread safety

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
            addToListBox("* * * Chat Server initiated on port " + portNumber + " * * *");

            while (isRunning)
            {
                try
                {
                    TcpClient client = appServer.AcceptTcpClient();

                    // Get the client's IP address
                    string clientIp = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

                    // Check if this IP should be rate limited
                    if (ShouldRateLimit(clientIp))
                    {
                        addToListBox($"Rate limited connection attempt from {clientIp}");
                        client.Close();
                        continue;
                    }

                    // Record this connection attempt
                    RecordConnectionAttempt(clientIp);

                    addToListBox("~ ~ New text messaging app CLIENT attempting to connect to Chat Server ~ ~");
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.Start();
                }
                catch (SocketException error)
                {
                    addToListBox("!! An error occurred accepting new text messaging app CLIENT: " + error.Message);
                    isRunning = false;
                }
            }
            appServer.Stop();
        }

        private bool ShouldRateLimit(string ipAddress)
        {
            lock (lockObject)
            {
                if (!connectionAttempts.ContainsKey(ipAddress))
                {
                    return false; // First attempt from this IP
                }

                // Remove attempts older than 1 minute
                DateTime oneMinuteAgo = DateTime.Now.AddMinutes(-1);
                connectionAttempts[ipAddress].RemoveAll(time => time < oneMinuteAgo);

                // Check if there are too many recent attempts
                return connectionAttempts[ipAddress].Count >= maxAttemptsPerMinute;
            }
        }

        private void RecordConnectionAttempt(string ipAddress)
        {
            lock (lockObject)
            {
                if (!connectionAttempts.ContainsKey(ipAddress))
                {
                    connectionAttempts[ipAddress] = new List<DateTime>();
                }

                connectionAttempts[ipAddress].Add(DateTime.Now);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] streamBuffer = new byte[1024];
            string messageToSend = "";
            string username = "";

            // Read password
            int NoBytesToRead = stream.Read(streamBuffer, 0, streamBuffer.Length);
            if (NoBytesToRead == 0)
            {
                addToListBox("x x x Client Disconnected x x x");
                return;
            }

            string receivedPassword = Encoding.UTF8.GetString(streamBuffer, 0, NoBytesToRead).Trim();

            // Validate password
            if (receivedPassword != sharedPassword)
            {
                SendAuthResponse(stream, false, "Invalid password");
                addToListBox("x x x Client Disconnected - Authentication failed x x x");
                stream.Close();
                client.Close();
                return;
            }

            // Send authentication success response
            SendAuthResponse(stream, true, "Authentication successful");

            // Read username
            NoBytesToRead = stream.Read(streamBuffer, 0, streamBuffer.Length);
            if (NoBytesToRead == 0)
            {
                addToListBox("x x x Client Disconnected x x x");
                stream.Close();
                client.Close();
                return;
            }

            username = Encoding.UTF8.GetString(streamBuffer, 0, NoBytesToRead).Trim();

            if (username == null || username.Length == 0 || username.Length > 15)
            {
                SendAuthResponse(stream, false, "Invalid username");
                addToListBox("x x x Client Disconnected - Invalid username x x x");
                stream.Close();
                client.Close();
                return;
            }

            // Add new client with its username to the dictionary
            lock (appClients)
            {
                if (appClients.ContainsKey(username))
                {
                    SendAuthResponse(stream, false, "Username already in use");
                    addToListBox("x x x Client Disconnected - Username " + username + " already in use x x x");
                    stream.Close();
                    client.Close();
                    return;
                }
                appClients[username] = client;
            }

            listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("Client with username " + username + " entered the Chat Server...")));

            // Broadcast that a user has joined the chat
            BroadcastSystemMessage(username + " has joined the chat.", username);

            try
            {
                while (true)
                {
                    // Read the message from the client
                    NoBytesToRead = stream.Read(streamBuffer, 0, streamBuffer.Length);
                    if (NoBytesToRead == 0)
                    {
                        addToListBox("Client with username " + username + " disconnected unexpectedly.");
                        BroadcastSystemMessage(username + " has disconnected from the chat.", null);
                        break;
                    }
                    string message = Encoding.UTF8.GetString(streamBuffer, 0, NoBytesToRead);

                    if (message.ToLower() == "xxx")
                    {
                        addToListBox("Client with username " + username + " exiting the Chat Server...");
                        BroadcastSystemMessage(username + " has left the chat.", null);
                        break;
                    }

                    if (message.StartsWith("@"))
                    {
                        string intendedRecipient = "";
                        int indexOfDistinquisher = message.IndexOf(":");
                        intendedRecipient = message.Substring(1, indexOfDistinquisher - 1).Trim();
                        messageToSend = message.Substring(indexOfDistinquisher + 1).Trim();

                        lock (appClients)
                        {
                            if (appClients.ContainsKey(intendedRecipient))
                            {
                                byte[] messageBufferToSend = Encoding.UTF8.GetBytes("DM received from " + username + ": " + messageToSend);
                                TcpClient messageReceiver = appClients[intendedRecipient];
                                NetworkStream messageStream = messageReceiver.GetStream();
                                messageStream.Write(messageBufferToSend, 0, messageBufferToSend.Length);
                                addToListBox("Direct Message (DM) sent successfully from " + username + " to " + intendedRecipient + " - -");
                            }
                            else
                            {
                                string errorMessage = "x x x Failed to send direct message intended for client " + intendedRecipient + " due to unavailability (user is not in the server) x x x";
                                byte[] errorMessageBufferToSend = Encoding.UTF8.GetBytes(errorMessage);
                                TcpClient messageSender = client;
                                NetworkStream errorMessageStream = messageSender.GetStream();
                                errorMessageStream.Write(errorMessageBufferToSend, 0, errorMessageBufferToSend.Length);
                                listBox1.Items.Add(errorMessage);
                            }
                        }
                    }
                    else
                    {
                        messageToSend = message;
                        byte[] messageBufferToSend = Encoding.UTF8.GetBytes(username + ": " + message);

                        lock (appClients)
                        {
                            for (int i = 0; i < appClients.Count; i++)
                            {
                                TcpClient messageReceiver = appClients.Values.ElementAt(i);
                                string receiverUsername = appClients.Keys.ElementAt(i);

                                if (messageReceiver == client)
                                {
                                    continue;
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
                                        addToListBox("x x x An error occurred sending the message to user @" + username + ": " + error.Message + " x x x");
                                    }
                                }
                            }
                            addToListBox("Message sent successfully from " + username + " to all connected users...");
                        }
                    }
                    addToListBox("~ Message (" + messageToSend + ") received from " + username);
                }
            }
            catch (Exception error)
            {
                addToListBox("x x x An error occurred while reading incoming message stream from client @" + username + ": " + error.Message + " x x x");
                BroadcastSystemMessage(username + " has disconnected from the chat.", null);
            }
            finally
            {
                lock (appClients)
                {
                    if (appClients.ContainsKey(username)) appClients.Remove(username);
                }

                try
                {
                    stream.Close();
                }
                catch (Exception error)
                {
                    addToListBox("x x x An error occurred while closing the stream for client @" + username + ": " + error.Message + " x x x");
                }

                try
                {
                    client.Close();
                }
                catch (Exception error)
                {
                    addToListBox("x x x An error occurred while closing the client @" + username + ": " + error.Message + " x x x");
                }

                addToListBox("Client with username " + username + " disconnected from the Chat Server.");
            }
        }

        private void BroadcastSystemMessage(string message, string excludeUsername)
        {
            byte[] messageBufferToSend = Encoding.UTF8.GetBytes("SYSTEM: " + message);

            lock (appClients)
            {
                foreach (var clientPair in appClients)
                {
                    // Skip sending to the excluded user (if any)
                    if (excludeUsername != null && clientPair.Key == excludeUsername)
                        continue;

                    try
                    {
                        NetworkStream messageStream = clientPair.Value.GetStream();
                        messageStream.Write(messageBufferToSend, 0, messageBufferToSend.Length);
                    }
                    catch (Exception)
                    {
                        // If we can't send to this client, just continue with others
                        // They'll be cleaned up when their own thread detects the error
                    }
                }
            }
        }

        private void SendAuthResponse(NetworkStream stream, bool success, string message)
        {
            string response = success ? "AUTH_SUCCESS:" + message : "AUTH_FAIL:" + message;
            byte[] responseBuffer = Encoding.UTF8.GetBytes(response);
            stream.Write(responseBuffer, 0, responseBuffer.Length);
        }

        private void ServerView_Load(object sender, EventArgs e)
        {
            Thread serverThread = new Thread(() => serverOperations(portNumber));
            serverThread.IsBackground = true;
            serverThread.Start();
        }
    }
}