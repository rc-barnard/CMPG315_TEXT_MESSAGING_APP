using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Text;

namespace TextMessagingApp
{
    public partial class Chat : Form
    {
        string username;
        TcpClient client;
        NetworkStream stream;
        Thread receiveIncommingMessageThread;
        private bool isRunning = true;

        public Chat(TcpClient client, string username, NetworkStream stream, Thread receiveIncommingMessageThread)
        {
            InitializeComponent();
            this.username = username;
            this.client = client;
            this.stream = stream;
            this.receiveIncommingMessageThread = receiveIncommingMessageThread;
        }

        private void checkBox2_AppearanceChanged(object sender, EventArgs e)
        {

        }

        public void setupReceiveMessagesThread()
        {
            receiveIncommingMessageThread = new Thread(receiveIncommingMessages);
            receiveIncommingMessageThread.IsBackground = true;
            receiveIncommingMessageThread.Start();

            lstChat.Items.Add("\t* * * ENTERING CHAT AS " + username.ToUpper() + " * * *");
        }

        public void receiveIncommingMessages()
        {
            while (isRunning)
            {
                try
                {
                    byte[] incommingMessageBuffer = new byte[1024];
                    int noBytesToRead = stream.Read(incommingMessageBuffer, 0, incommingMessageBuffer.Length);
                    if (noBytesToRead == 0)
                    {
                        this.Invoke((MethodInvoker)delegate {
                            lstChat.Items.Add("\t\tDisconnected from the Chat Server...");
                        });
                        break;
                    }
                    string messageReceived = Encoding.UTF8.GetString(incommingMessageBuffer, 0, noBytesToRead);
                    this.Invoke((MethodInvoker)delegate { lstChat.Items.Add(messageReceived); });
                }
                catch (Exception error)
                {
                    if (isRunning)
                    {
                        this.Invoke((MethodInvoker)delegate {
                            lstChat.Items.Add("\t\tError receiving messages: " + error.Message);
                        });
                    }
                    break;
                }
            }
        }

        private void Chat_Load(object sender, EventArgs e)
        {
            label4.Text = "Date: " + DateTime.Today.ToString("yyyy-MM-dd");
            setupReceiveMessagesThread();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                label2.Visible = true;
                txtRecipient.Visible = true;
            }
            else
            {
                label2.Visible = false;
                txtRecipient.Visible = false;
            }
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked)
                {
                    string rawMessageToSend = txtMessage.Text.Trim();
                    if (string.IsNullOrEmpty(rawMessageToSend) || string.IsNullOrEmpty(txtRecipient.Text))
                    {
                        MessageBox.Show("Please enter both a recipient and a message", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string messageToSend = "@" + txtRecipient.Text + ":" + rawMessageToSend;
                    byte[] messageBufferToSend = Encoding.UTF8.GetBytes(messageToSend);
                    stream.Write(messageBufferToSend, 0, messageBufferToSend.Length);
                    lstChat.Items.Add("You to " + txtRecipient.Text + ": " + rawMessageToSend);
                    txtRecipient.Clear();
                    checkBox1.Checked = false;
                    txtMessage.Clear();
                }
                else
                {
                    string messageToSend = txtMessage.Text.Trim();
                    if (string.IsNullOrEmpty(messageToSend))
                    {
                        MessageBox.Show("Please enter a message", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    lstChat.Items.Add("You: " + messageToSend);
                    byte[] messageBufferToSend = Encoding.UTF8.GetBytes(messageToSend);
                    stream.Write(messageBufferToSend, 0, messageBufferToSend.Length);
                    txtMessage.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending message: " + ex.Message, "Send Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                isRunning = false;
                string messageToSend = "xxx";
                byte[] messageBufferToSend = Encoding.UTF8.GetBytes(messageToSend);
                stream.Write(messageBufferToSend, 0, messageBufferToSend.Length);
                lstChat.Items.Add("x x x - " + username + " left the Chat Server - x x x");

                // Give some time for the message to be sent
                Thread.Sleep(500);

                stream.Close();
                client.Close();
                Application.Exit();
            }
            catch (Exception ex)
            {
                // Just exit if there's an error
                Application.Exit();
            }
        }

        private void Chat_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                isRunning = false;

                // Try to send exit message if still connected
                if (client != null && client.Connected)
                {
                    byte[] exitMessage = Encoding.UTF8.GetBytes("xxx");
                    stream.Write(exitMessage, 0, exitMessage.Length);
                    Thread.Sleep(500); // Give time for message to be sent
                }

                // Clean up resources
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();

                Application.Exit();
            }
            catch
            {
                // Just exit if there's an error during cleanup
                Application.Exit();
            }
        }
    }
}