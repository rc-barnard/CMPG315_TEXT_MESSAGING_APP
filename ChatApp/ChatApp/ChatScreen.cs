using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading; //Using threading to run threads other than the main UI thread.
using System.Net.Sockets; //Using sockets to create a TCP connection.
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ChatApp
{
    public partial class ChatScreen : Form
    {
        //Create a TcpClient object to connect to the server.
        TcpClient client;
        //Create a NetworkStream object to send and receive data from the server.
        NetworkStream stream;
        //Create a Thread object to receive incoming messages from the server.
        Thread receiveIncommingMessages;

        //Variable to hold user's username for communication.
        string username = "";
        private bool isClosing = false;

        //Constructor to initialize the ChatScreen form with TcpClient, NetworkStream, Thread, and username in order to use a continious client, stream etc.
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
            setupReceiveMessagesThread(); //This method sets up the thread to start receiving messages from the server.
        }

        //Timer to continious update the date and time on the chat screen each second.
        private void dateTimer_Tick(object sender, EventArgs e)
        {
            lblDate.Text = "Date: " + DateTime.Now.ToString("ddd-dd/MM/yyyy").ToUpper() + ", " + DateTime.Now.ToString("HH:mm:ss");
        }

        private void IsDirectMessage_CheckedChanged(object sender, EventArgs e)
        {
            //Ensure that when the user wants to send a direct message, the user is asked for a recipient username.
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

        //Method to setup the thread to receive messages from the server.
        public void setupReceiveMessagesThread()
        {
            receiveIncommingMessages = new Thread(receiveMessages); //This method is called to start the thread to receive messages from the server.
            receiveIncommingMessages.IsBackground = true;
            receiveIncommingMessages.Start();

            //User's registered username is displayed on the chat screen for identification purposes.
            lstChat.Items.Add("\t  * * * ENTERED CHAT AS " + username + " * * *");
        }

        //Method to receive messages from the server.
        public void receiveMessages()
        {
            //wHILE to keep receiving messages from the server until the user exits the chat.
            while (!isClosing)
            {
                try //Try catch statement to ensure proper exception handling.
                {
                    //Create a byte array to hold the incoming message buffer.
                    byte[] incommingMessageBuffer = new byte[1024];

                    //Determine the number of bytes to read from the network stream.
                    int noBytesToRead = stream.Read(incommingMessageBuffer, 0, incommingMessageBuffer.Length);
                    if (noBytesToRead == 0) //If there are no bytes to read, the server is stopped, thus the client will be exited from the app.
                    {
                        MessageBox.Show("Exiting the Chat Server..."); //Message to inform the user that the server is stopped.
                        break; //Exit the while loop to stop receiving messages.
                    }

                    //If there are bytes to read, convert the byte array to a string for the message.
                    string messageReceived = Encoding.UTF8.GetString(incommingMessageBuffer, 0, noBytesToRead);
                    if (messageReceived.ToLower().EndsWith("xping")) //If it is a ping to keep client active, ignor it. 
                    {
                        continue;
                    }
                    else
                    {
                        //Else, display the message received from the background thread in the chat window.
                        this.Invoke((MethodInvoker)delegate { lstChat.Items.Add(""); });
                        this.Invoke((MethodInvoker)delegate { lstChat.Items.Add(messageReceived); });
                    }

                }
                catch (Exception)
                {
                    //If an exception occurs, the server is stopped, thus the client will be exited from the app. Invoke action is required to update the UI from the background thread.
                    this.Invoke((MethodInvoker)delegate { MessageBox.Show(this, "Server stopped! Exiting the app..."); });
                    break;
                }
            }
            stream.Close(); //Close the network stream to stop receiving messages if the server is stopped or the client exited the chat.
            client.Close(); //Close the TcpClient to stop sending messages if the server is stopped or the client exited the chat.
            Environment.Exit(0); //Stop all threads and exit the application.
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            //Ensure there is a message to send before sending it.
            if (txtMessage.Text.Length != 0)
            {
                if (IsDirectMessage.Checked) //Check to see if the user wants to send a direct message to another user.
                {
                    if (txtRecipient.Text.Length != 0) //If direct message, ensure receipient is entered.
                    {
                        //Variable to hold raw message.
                        string rawMessageToSend = txtMessage.Text.Trim();

                        //Format message to send to the server for the server to recognize it as a direct message.
                        string messageToSend = "@" + txtRecipient.Text.ToUpper() + ":" + rawMessageToSend;
                        byte[] messageBufferToSend = Encoding.UTF8.GetBytes(messageToSend);
                        stream.Write(messageBufferToSend, 0, messageBufferToSend.Length); //Send the message to the server.
                        lstChat.Items.Add("");
                        lstChat.Items.Add("YOU TO " + txtRecipient.Text.ToUpper() + ": " + rawMessageToSend); //Display the message sent in the chat window.
                        txtRecipient.Clear(); //Clear the recipient box.
                        IsDirectMessage.Checked = false; //Uncheck the direct message checkbox.
                        txtMessage.Clear(); //Clear the message textbox.
                    }
                    else
                    {
                        //If the recipient is not entered, show a message box to inform the user to enter a recipient.
                        MessageBox.Show("Please enter a recipient to send the message to!");
                    }
                }
                else
                {
                    string messageToSend = txtMessage.Text;
                    lstChat.Items.Add("");
                    lstChat.Items.Add("YOU: " + messageToSend); // Display the message sent in the chat window.
                    byte[] messageBufferToSend = Encoding.UTF8.GetBytes(messageToSend); 
                    stream.Write(messageBufferToSend, 0, messageBufferToSend.Length); //Send the message to the server.
                    txtMessage.Clear(); //Clear the message textbox.
                }
            }
            else
            {
                MessageBox.Show("Please enter a message to send!"); //Message to inform the user to enter a message.
            }
        }

        private void btnExitChat_Click(object sender, EventArgs e)
        {
            isClosing = true; //Set the isClosing variable to true to exit loop and stop receiving messages from the server.
            string messageToSend = "xxx"; //Message to inform the server that the client is exiting the chat for unregistering.
            byte[] messageBufferToSend = Encoding.UTF8.GetBytes(messageToSend);
            stream.Write(messageBufferToSend, 0, messageBufferToSend.Length); //Send the message to the server.
            stream.Close(); //Close the network stream.
            client.Close(); //Close the client.
            
            Environment.Exit(0); //Stop all threads and exit the application.
        }

        //Timer to keep the client active on the server.
        private void aliveTimer_Tick(object sender, EventArgs e)
        {
            try//Try catch statement to ensure proper exception handling.
            {
                byte[] ping = Encoding.UTF8.GetBytes("xping");
                stream.Write(ping, 0, ping.Length); //Send a ping to the server to keep the client active.
            }
            catch
            {

            }
        }
    }
}
