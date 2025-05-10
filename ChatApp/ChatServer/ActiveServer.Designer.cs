namespace ChatServer
{
    partial class ActiveServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActiveServer));
            this.lstServerActivity = new System.Windows.Forms.ListBox();
            this.btnStopServer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstServerActivity
            // 
            this.lstServerActivity.BackColor = System.Drawing.SystemColors.Info;
            this.lstServerActivity.FormattingEnabled = true;
            this.lstServerActivity.HorizontalScrollbar = true;
            this.lstServerActivity.ItemHeight = 16;
            this.lstServerActivity.Location = new System.Drawing.Point(12, 12);
            this.lstServerActivity.Name = "lstServerActivity";
            this.lstServerActivity.Size = new System.Drawing.Size(689, 308);
            this.lstServerActivity.TabIndex = 0;
            // 
            // btnStopServer
            // 
            this.btnStopServer.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnStopServer.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStopServer.Font = new System.Drawing.Font("Stencil", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopServer.Location = new System.Drawing.Point(258, 333);
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(186, 50);
            this.btnStopServer.TabIndex = 1;
            this.btnStopServer.Text = "Stop Server";
            this.btnStopServer.UseVisualStyleBackColor = false;
            this.btnStopServer.Click += new System.EventHandler(this.btnStopServer_Click);
            // 
            // ActiveServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(713, 395);
            this.Controls.Add(this.btnStopServer);
            this.Controls.Add(this.lstServerActivity);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ActiveServer";
            this.Text = "Active Server View";
            this.Load += new System.EventHandler(this.ActiveServer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstServerActivity;
        private System.Windows.Forms.Button btnStopServer;
    }
}