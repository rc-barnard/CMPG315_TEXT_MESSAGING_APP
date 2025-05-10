namespace ChatApp
{
    partial class ChatScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatScreen));
            this.IsDirectMessage = new System.Windows.Forms.CheckBox();
            this.lblRecipient = new System.Windows.Forms.Label();
            this.txtRecipient = new System.Windows.Forms.TextBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.lstChat = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.btnExitChat = new System.Windows.Forms.Button();
            this.dateTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // IsDirectMessage
            // 
            this.IsDirectMessage.AutoSize = true;
            this.IsDirectMessage.Font = new System.Drawing.Font("Rockwell", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsDirectMessage.Location = new System.Drawing.Point(12, 12);
            this.IsDirectMessage.Name = "IsDirectMessage";
            this.IsDirectMessage.Size = new System.Drawing.Size(137, 21);
            this.IsDirectMessage.TabIndex = 0;
            this.IsDirectMessage.Text = "Direct Message";
            this.IsDirectMessage.UseVisualStyleBackColor = true;
            this.IsDirectMessage.CheckedChanged += new System.EventHandler(this.IsDirectMessage_CheckedChanged);
            // 
            // lblRecipient
            // 
            this.lblRecipient.AutoSize = true;
            this.lblRecipient.Font = new System.Drawing.Font("Rockwell", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecipient.Location = new System.Drawing.Point(12, 42);
            this.lblRecipient.Name = "lblRecipient";
            this.lblRecipient.Size = new System.Drawing.Size(83, 17);
            this.lblRecipient.TabIndex = 1;
            this.lblRecipient.Text = "Username:";
            this.lblRecipient.Visible = false;
            // 
            // txtRecipient
            // 
            this.txtRecipient.Location = new System.Drawing.Point(15, 62);
            this.txtRecipient.Name = "txtRecipient";
            this.txtRecipient.Size = new System.Drawing.Size(110, 22);
            this.txtRecipient.TabIndex = 2;
            this.txtRecipient.Visible = false;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Rockwell", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(389, 14);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(41, 16);
            this.lblDate.TabIndex = 3;
            this.lblDate.Text = "Date: ";
            // 
            // lstChat
            // 
            this.lstChat.BackColor = System.Drawing.SystemColors.Info;
            this.lstChat.FormattingEnabled = true;
            this.lstChat.HorizontalScrollbar = true;
            this.lstChat.ItemHeight = 16;
            this.lstChat.Location = new System.Drawing.Point(248, 40);
            this.lstChat.Name = "lstChat";
            this.lstChat.Size = new System.Drawing.Size(398, 324);
            this.lstChat.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Rockwell", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Enter message:";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(12, 152);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(199, 107);
            this.txtMessage.TabIndex = 6;
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnSendMessage.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSendMessage.Font = new System.Drawing.Font("Rockwell", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSendMessage.Location = new System.Drawing.Point(32, 277);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(159, 35);
            this.btnSendMessage.TabIndex = 7;
            this.btnSendMessage.Text = "Send Message";
            this.btnSendMessage.UseVisualStyleBackColor = false;
            this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
            // 
            // btnExitChat
            // 
            this.btnExitChat.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnExitChat.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExitChat.Font = new System.Drawing.Font("Rockwell", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExitChat.Location = new System.Drawing.Point(32, 319);
            this.btnExitChat.Name = "btnExitChat";
            this.btnExitChat.Size = new System.Drawing.Size(159, 35);
            this.btnExitChat.TabIndex = 8;
            this.btnExitChat.Text = "Exit Message";
            this.btnExitChat.UseVisualStyleBackColor = false;
            this.btnExitChat.Click += new System.EventHandler(this.btnExitChat_Click);
            // 
            // dateTimer
            // 
            this.dateTimer.Enabled = true;
            this.dateTimer.Interval = 1000;
            this.dateTimer.Tick += new System.EventHandler(this.dateTimer_Tick);
            // 
            // ChatScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(658, 376);
            this.Controls.Add(this.btnExitChat);
            this.Controls.Add(this.btnSendMessage);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstChat);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.txtRecipient);
            this.Controls.Add(this.lblRecipient);
            this.Controls.Add(this.IsDirectMessage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ChatScreen";
            this.Text = "Chat Screen";
            this.Load += new System.EventHandler(this.ChatScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox IsDirectMessage;
        private System.Windows.Forms.Label lblRecipient;
        private System.Windows.Forms.TextBox txtRecipient;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.ListBox lstChat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.Button btnExitChat;
        private System.Windows.Forms.Timer dateTimer;
    }
}