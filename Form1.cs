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

namespace TextMessagingApp
{
    public partial class Form1 : Form
    {
        //Create new client to connect to server
        public static TcpClient client;
        //Create new stream to send and receive data
        public static NetworkStream stream;
        public static Thread receiveMessagesThread;
        public static string username = "";
        // The shared password must match the one on the server
        private static String sharedPassword = "CMPG@315PROJ";

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        public void registerClientWithServer(string IpAddress, int portNumber)
        {
            try
            {
                // Create client with timeout to prevent freezing
                client = new TcpClient();

                // Set a timeout for the connection attempt (3 seconds)
                IAsyncResult result = client.BeginConnect(IpAddress, portNumber, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(3000, true); // 3 second timeout

                if (!success)
                {
                    // Connection attempt timed out
                    client.Close();
                    MessageBox.Show("Unable to connect to server: Connection timed out", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Finish the connection process
                client.EndConnect(result);

                // Get the stream after successful connection
                stream = client.GetStream();

                // First send the password for authentication
                byte[] passwordBufferToSend = Encoding.UTF8.GetBytes(sharedPassword);
                stream.Write(passwordBufferToSend, 0, passwordBufferToSend.Length);

                // Read authentication response
                byte[] authResponseBuffer = new byte[1024];
                int bytesRead = stream.Read(authResponseBuffer, 0, authResponseBuffer.Length);
                string authResponse = Encoding.UTF8.GetString(authResponseBuffer, 0, bytesRead);

                if (authResponse.StartsWith("AUTH_FAIL"))
                {
                    MessageBox.Show("Authentication failed: " + authResponse.Substring(10), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    client.Close();
                    return;
                }

                // Now send the username
                username = txtUsername.Text;
                byte[] usernameBufferToSend = Encoding.UTF8.GetBytes(username);
                stream.Write(usernameBufferToSend, 0, usernameBufferToSend.Length);

                // Wait for a potential error response for username
                if (client.Available > 0)
                {
                    bytesRead = stream.Read(authResponseBuffer, 0, authResponseBuffer.Length);
                    string usernameResponse = Encoding.UTF8.GetString(authResponseBuffer, 0, bytesRead);

                    if (usernameResponse.StartsWith("AUTH_FAIL"))
                    {
                        MessageBox.Show("Username error: " + usernameResponse.Substring(10), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        client.Close();
                        return;
                    }
                }

                MessageBox.Show("- - - Connected to server successfully - - -");
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Error connecting to server: " + ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (client != null)
                    client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to server: " + ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (client != null)
                    client.Close();
            }
        }

        private void btnEnterChat_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Length == 0)
            {
                MessageBox.Show("Please enter a username", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtUsername.Text.Length > 15)
            {
                MessageBox.Show("Username must be 15 characters or less", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            registerClientWithServer("28.ip.gl.ply.gg", 20931);

            // Only show Chat form if connection was successful
            if (client != null && client.Connected)
            {
                Chat myChat = new Chat(client, username, stream, receiveMessagesThread);
                myChat.Show();
                this.Hide();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}