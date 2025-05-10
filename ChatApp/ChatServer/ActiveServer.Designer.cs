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
            this.lstServerActivity.Location = new System.Drawing.Point(9, 10);
            this.lstServerActivity.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lstServerActivity.Name = "lstServerActivity";
            this.lstServerActivity.Size = new System.Drawing.Size(518, 251);
            this.lstServerActivity.TabIndex = 0;
            // 
            // btnStopServer
            // 
            this.btnStopServer.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnStopServer.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStopServer.Font = new System.Drawing.Font("Stencil", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopServer.Location = new System.Drawing.Point(194, 271);
            this.btnStopServer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(140, 41);
            this.btnStopServer.TabIndex = 1;
            this.btnStopServer.Text = "Stop Server";
            this.btnStopServer.UseVisualStyleBackColor = false;
            this.btnStopServer.Click += new System.EventHandler(this.btnStopServer_Click);
            // 
            // ActiveServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(535, 321);
            this.Controls.Add(this.btnStopServer);
            this.Controls.Add(this.lstServerActivity);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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