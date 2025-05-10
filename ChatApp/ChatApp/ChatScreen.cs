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
using System.Net.Sockets;
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace ChatApp
{
    public partial class ChatScreen : Form
    {
        TcpClient client;
        NetworkStream stream;
        Thread receiveIncommingMessages;
        string username = "";
        private bool isClosing = false;

        public ChatScreen(TcpClient client, NetworkStream stream, Thread receiveIncommingMessages, string username)
        {
            InitializeComponent();
            this.client = client;
            this.stream = stream;
            this.receiveIncommingMessages = receiveIncommingMessages;
            this.username = username;
        }

        private void ChatScreen_Load(object sender, EventArgs e)
        {
            setupReceiveMessagesThread();
        }

        private void dateTimer_Tick(object sender, EventArgs e)
        {
            lblDate.Text = "Date: " + DateTime.Now.ToString("ddd-MM/yyyy") + ", " + DateTime.Now.ToString("HH:mm:ss");
        }

        private void IsDirectMessage_CheckedChanged(object sender, EventArgs e)
        {
            if (IsDirectMessage.Checked)
            {
                lblRecipient.Visible = true;
                txtRecipient.Visible = true;
            }
            else
            {
                lblRecipient.Visible = false;
                txtRecipient.Visible = false;
            }
        }

        public void setupReceiveMessagesThread()
        {
            receiveIncommingMessages = new Thread(receiveMessages);
            receiveIncommingMessages.IsBackground = true;
            receiveIncommingMessages.Start();

            lstChat.Items.Add("\t  * * * ENTERED CHAT AS " + username + " * * *");
        }

        public void receiveMessages()
        {
            while (!isClosing)
            {
                try
                {
                    byte[] incommingMessageBuffer = new byte[1024];
                    int noBytesToRead = stream.Read(incommingMessageBuffer, 0, incommingMessageBuffer.Length);
                    if (noBytesToRead == 0)
                    {
                        MessageBox.Show("Exiting the Chat Server...");
                        break;
                    }

                    string messageReceived = Encoding.UTF8.GetString(incommingMessageBuffer, 0, noBytesToRead);
                    this.Invoke((MethodInvoker)delegate { lstChat.Items.Add(""); });
                    this.Invoke((MethodInvoker)delegate { lstChat.Items.Add(messageReceived); });
                }
                catch (Exception)
                {
                    this.Invoke((MethodInvoker)delegate { MessageBox.Show(this, "Server stopped! Exiting the app..."); });
                    break;
                }
            }
            stream.Close();
            client.Close();
            Environment.Exit(0);
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (txtMessage.Text.Length != 0)
            {
                if (IsDirectMessage.Checked)
                {
                    if (txtRecipient.Text.Length != 0)
                    {
                        string rawMessageToSend = txtMessage.Text.Trim();
                        string messageToSend = "@" + txtRecipient.Text.ToUpper() + ":" + rawMessageToSend;
                        byte[] messageBufferToSend = Encoding.UTF8.GetBytes(messageToSend);
                        stream.Write(messageBufferToSend, 0, messageBufferToSend.Length);
                        lstChat.Items.Add("");
                        lstChat.Items.Add("\tYou to " + txtRecipient.Text + ": " + rawMessageToSend);
                        txtRecipient.Clear();
                        IsDirectMessage.Checked = false;
                        txtMessage.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Please enter a recipient to send the message to!");
                    }
                }
                else
                {
                    string messageToSend = txtMessage.Text;
                    lstChat.Items.Add("");
                    lstChat.Items.Add("\tYou: " + messageToSend);
                    byte[] messageBufferToSend = Encoding.UTF8.GetBytes(messageToSend);
                    stream.Write(messageBufferToSend, 0, messageBufferToSend.Length);
                    txtMessage.Clear();
                }
            }
            else
            {
                MessageBox.Show("Please enter a message to send!");
            }
        }

        private void btnExitChat_Click(object sender, EventArgs e)
        {
            isClosing = true;
            string messageToSend = "xxx";
            byte[] messageBufferToSend = Encoding.UTF8.GetBytes(messageToSend);
            stream.Write(messageBufferToSend, 0, messageBufferToSend.Length);
            stream.Close();
            client.Close();
            
            Environment.Exit(0);
        }

        private void aliveTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                byte[] ping = Encoding.UTF8.GetBytes("ping");
                stream.Write(ping, 0, ping.Length);
            }
            catch
            {

            }
        }
    }
}
