
namespace bettlroal
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnHost = new System.Windows.Forms.Button();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.gbConn = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.lblPublicIP = new System.Windows.Forms.Label();
            this.btnStream = new System.Windows.Forms.Button();
            this.btnOpenStream = new System.Windows.Forms.Button();
            this.flpChat = new System.Windows.Forms.FlowLayoutPanel();
            this.gbConn.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHost
            // 
            this.btnHost.Location = new System.Drawing.Point(6, 23);
            this.btnHost.Name = "btnHost";
            this.btnHost.Size = new System.Drawing.Size(100, 23);
            this.btnHost.TabIndex = 0;
            this.btnHost.Text = "Host";
            this.btnHost.UseVisualStyleBackColor = true;
            this.btnHost.Click += new System.EventHandler(this.btnHost_Click);
            // 
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(6, 52);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(100, 20);
            this.tbIP.TabIndex = 1;
            this.tbIP.Text = "127.0.0.1";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(6, 78);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(100, 20);
            this.tbPort.TabIndex = 2;
            this.tbPort.Text = "60900";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(6, 104);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 23);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // gbConn
            // 
            this.gbConn.Controls.Add(this.btnConnect);
            this.gbConn.Controls.Add(this.btnHost);
            this.gbConn.Controls.Add(this.tbPort);
            this.gbConn.Controls.Add(this.tbIP);
            this.gbConn.Location = new System.Drawing.Point(12, 12);
            this.gbConn.Name = "gbConn";
            this.gbConn.Size = new System.Drawing.Size(115, 141);
            this.gbConn.TabIndex = 4;
            this.gbConn.TabStop = false;
            this.gbConn.Text = "Connect";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.CausesValidation = false;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(133, 461);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(702, 20);
            this.textBox1.TabIndex = 5;
            this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(842, 459);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(100, 23);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblPublicIP
            // 
            this.lblPublicIP.AutoSize = true;
            this.lblPublicIP.Location = new System.Drawing.Point(15, 156);
            this.lblPublicIP.Name = "lblPublicIP";
            this.lblPublicIP.Size = new System.Drawing.Size(0, 13);
            this.lblPublicIP.TabIndex = 7;
            // 
            // btnStream
            // 
            this.btnStream.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStream.Location = new System.Drawing.Point(12, 461);
            this.btnStream.Name = "btnStream";
            this.btnStream.Size = new System.Drawing.Size(115, 20);
            this.btnStream.TabIndex = 8;
            this.btnStream.Text = "Stream";
            this.btnStream.UseVisualStyleBackColor = true;
            this.btnStream.Click += new System.EventHandler(this.btnStream_Click);
            // 
            // btnOpenStream
            // 
            this.btnOpenStream.Location = new System.Drawing.Point(12, 281);
            this.btnOpenStream.Name = "btnOpenStream";
            this.btnOpenStream.Size = new System.Drawing.Size(75, 23);
            this.btnOpenStream.TabIndex = 9;
            this.btnOpenStream.Text = "Open Stream";
            this.btnOpenStream.UseVisualStyleBackColor = true;
            this.btnOpenStream.Click += new System.EventHandler(this.btnOpenStream_Click);
            // 
            // flpChat
            // 
            this.flpChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpChat.AutoScroll = true;
            this.flpChat.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.flpChat.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpChat.Location = new System.Drawing.Point(133, 12);
            this.flpChat.Name = "flpChat";
            this.flpChat.Size = new System.Drawing.Size(809, 441);
            this.flpChat.TabIndex = 11;
            this.flpChat.WrapContents = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 493);
            this.Controls.Add(this.flpChat);
            this.Controls.Add(this.btnOpenStream);
            this.Controls.Add(this.btnStream);
            this.Controls.Add(this.lblPublicIP);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.gbConn);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "Bettlroal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.gbConn.ResumeLayout(false);
            this.gbConn.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnHost;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.GroupBox gbConn;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label lblPublicIP;
        private System.Windows.Forms.Button btnStream;
        private System.Windows.Forms.Button btnOpenStream;
        private System.Windows.Forms.FlowLayoutPanel flpChat;
    }
}

