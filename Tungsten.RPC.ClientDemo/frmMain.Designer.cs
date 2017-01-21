namespace Tungsten.RPC.ClientDemo
{
    partial class frmMain
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
            this.btnSend = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnTestTimeout = new System.Windows.Forms.Button();
            this.btnTestTimeout2 = new System.Windows.Forms.Button();
            this.btnSimplestRPC = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(402, 65);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(85, 20);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "&Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Send Message";
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.Location = new System.Drawing.Point(12, 65);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(384, 20);
            this.txtMessage.TabIndex = 2;
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnect.Location = new System.Drawing.Point(402, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(85, 20);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "&Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnTestTimeout
            // 
            this.btnTestTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestTimeout.Enabled = false;
            this.btnTestTimeout.Location = new System.Drawing.Point(402, 91);
            this.btnTestTimeout.Name = "btnTestTimeout";
            this.btnTestTimeout.Size = new System.Drawing.Size(85, 20);
            this.btnTestTimeout.TabIndex = 4;
            this.btnTestTimeout.Text = "&Test Timeout";
            this.btnTestTimeout.UseVisualStyleBackColor = true;
            this.btnTestTimeout.Click += new System.EventHandler(this.btnTestTimeout_Click);
            // 
            // btnTestTimeout2
            // 
            this.btnTestTimeout2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestTimeout2.Enabled = false;
            this.btnTestTimeout2.Location = new System.Drawing.Point(351, 117);
            this.btnTestTimeout2.Name = "btnTestTimeout2";
            this.btnTestTimeout2.Size = new System.Drawing.Size(136, 20);
            this.btnTestTimeout2.TabIndex = 5;
            this.btnTestTimeout2.Text = "&Test Timeout With Result";
            this.btnTestTimeout2.UseVisualStyleBackColor = true;
            this.btnTestTimeout2.Click += new System.EventHandler(this.btnTestTimeoutWithResult_Click);
            // 
            // btnSimplestRPC
            // 
            this.btnSimplestRPC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSimplestRPC.Enabled = false;
            this.btnSimplestRPC.Location = new System.Drawing.Point(351, 143);
            this.btnSimplestRPC.Name = "btnSimplestRPC";
            this.btnSimplestRPC.Size = new System.Drawing.Size(136, 20);
            this.btnSimplestRPC.TabIndex = 6;
            this.btnSimplestRPC.Text = "&Simplest RPC";
            this.btnSimplestRPC.UseVisualStyleBackColor = true;
            this.btnSimplestRPC.Click += new System.EventHandler(this.btnSimplestRPC_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 234);
            this.Controls.Add(this.btnSimplestRPC);
            this.Controls.Add(this.btnTestTimeout2);
            this.Controls.Add(this.btnTestTimeout);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSend);
            this.Name = "frmMain";
            this.Text = "Tungsten.RPC Client Demo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnTestTimeout;
        private System.Windows.Forms.Button btnTestTimeout2;
        private System.Windows.Forms.Button btnSimplestRPC;
    }
}

