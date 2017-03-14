namespace W.Domains.Demo
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
            this.btnReloadDomain = new System.Windows.Forms.Button();
            this.lstMessages = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnExecute5 = new System.Windows.Forms.Button();
            this.btnExecute4 = new System.Windows.Forms.Button();
            this.btnExecute3 = new System.Windows.Forms.Button();
            this.btnExecute2 = new System.Windows.Forms.Button();
            this.btnExecute1 = new System.Windows.Forms.Button();
            this.lblMessages = new System.Windows.Forms.Label();
            this.btnDoCallback = new System.Windows.Forms.Button();
            this.btnDoCallback2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnReloadDomain
            // 
            this.btnReloadDomain.Location = new System.Drawing.Point(11, 11);
            this.btnReloadDomain.Name = "btnReloadDomain";
            this.btnReloadDomain.Size = new System.Drawing.Size(106, 23);
            this.btnReloadDomain.TabIndex = 0;
            this.btnReloadDomain.Text = "&Reload Domain";
            this.btnReloadDomain.UseVisualStyleBackColor = true;
            this.btnReloadDomain.Click += new System.EventHandler(this.btnReloadDomain_Click);
            // 
            // lstMessages
            // 
            this.lstMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstMessages.FormattingEnabled = true;
            this.lstMessages.Location = new System.Drawing.Point(3, 28);
            this.lstMessages.Name = "lstMessages";
            this.lstMessages.Size = new System.Drawing.Size(453, 160);
            this.lstMessages.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnDoCallback2);
            this.splitContainer1.Panel1.Controls.Add(this.btnDoCallback);
            this.splitContainer1.Panel1.Controls.Add(this.btnExecute5);
            this.splitContainer1.Panel1.Controls.Add(this.btnExecute4);
            this.splitContainer1.Panel1.Controls.Add(this.btnExecute3);
            this.splitContainer1.Panel1.Controls.Add(this.btnExecute2);
            this.splitContainer1.Panel1.Controls.Add(this.btnExecute1);
            this.splitContainer1.Panel1.Controls.Add(this.btnReloadDomain);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblMessages);
            this.splitContainer1.Panel2.Controls.Add(this.lstMessages);
            this.splitContainer1.Size = new System.Drawing.Size(461, 423);
            this.splitContainer1.SplitterDistance = 210;
            this.splitContainer1.TabIndex = 2;
            // 
            // btnExecute5
            // 
            this.btnExecute5.Location = new System.Drawing.Point(230, 127);
            this.btnExecute5.Name = "btnExecute5";
            this.btnExecute5.Size = new System.Drawing.Size(106, 23);
            this.btnExecute5.TabIndex = 5;
            this.btnExecute5.Text = "&Execute";
            this.btnExecute5.UseVisualStyleBackColor = true;
            this.btnExecute5.Click += new System.EventHandler(this.btnExecute5_Click);
            // 
            // btnExecute4
            // 
            this.btnExecute4.Location = new System.Drawing.Point(230, 98);
            this.btnExecute4.Name = "btnExecute4";
            this.btnExecute4.Size = new System.Drawing.Size(106, 23);
            this.btnExecute4.TabIndex = 4;
            this.btnExecute4.Text = "&Execute";
            this.btnExecute4.UseVisualStyleBackColor = true;
            this.btnExecute4.Click += new System.EventHandler(this.btnExecute4_Click);
            // 
            // btnExecute3
            // 
            this.btnExecute3.Location = new System.Drawing.Point(230, 69);
            this.btnExecute3.Name = "btnExecute3";
            this.btnExecute3.Size = new System.Drawing.Size(106, 23);
            this.btnExecute3.TabIndex = 3;
            this.btnExecute3.Text = "&Execute";
            this.btnExecute3.UseVisualStyleBackColor = true;
            this.btnExecute3.Click += new System.EventHandler(this.btnExecute3_Click);
            // 
            // btnExecute2
            // 
            this.btnExecute2.Location = new System.Drawing.Point(230, 40);
            this.btnExecute2.Name = "btnExecute2";
            this.btnExecute2.Size = new System.Drawing.Size(106, 23);
            this.btnExecute2.TabIndex = 2;
            this.btnExecute2.Text = "&Execute";
            this.btnExecute2.UseVisualStyleBackColor = true;
            this.btnExecute2.Click += new System.EventHandler(this.btnExecute2_Click);
            // 
            // btnExecute1
            // 
            this.btnExecute1.Location = new System.Drawing.Point(230, 11);
            this.btnExecute1.Name = "btnExecute1";
            this.btnExecute1.Size = new System.Drawing.Size(106, 23);
            this.btnExecute1.TabIndex = 1;
            this.btnExecute1.Text = "&Execute";
            this.btnExecute1.UseVisualStyleBackColor = true;
            this.btnExecute1.Click += new System.EventHandler(this.btnExecute1_Click);
            // 
            // lblMessages
            // 
            this.lblMessages.AutoSize = true;
            this.lblMessages.Location = new System.Drawing.Point(3, 12);
            this.lblMessages.Name = "lblMessages";
            this.lblMessages.Size = new System.Drawing.Size(55, 13);
            this.lblMessages.TabIndex = 2;
            this.lblMessages.Text = "Messages";
            // 
            // btnDoCallback
            // 
            this.btnDoCallback.Location = new System.Drawing.Point(342, 11);
            this.btnDoCallback.Name = "btnDoCallback";
            this.btnDoCallback.Size = new System.Drawing.Size(106, 23);
            this.btnDoCallback.TabIndex = 6;
            this.btnDoCallback.Text = "DoCallback";
            this.btnDoCallback.UseVisualStyleBackColor = true;
            this.btnDoCallback.Click += new System.EventHandler(this.btnDoCallback_Click);
            // 
            // btnDoCallback2
            // 
            this.btnDoCallback2.Location = new System.Drawing.Point(342, 40);
            this.btnDoCallback2.Name = "btnDoCallback2";
            this.btnDoCallback2.Size = new System.Drawing.Size(106, 23);
            this.btnDoCallback2.TabIndex = 7;
            this.btnDoCallback2.Text = "DoCallback 2";
            this.btnDoCallback2.UseVisualStyleBackColor = true;
            this.btnDoCallback2.Click += new System.EventHandler(this.btnDoCallback2_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 423);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmMain";
            this.Text = "Tungsten.Domains.Demo";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReloadDomain;
        private System.Windows.Forms.ListBox lstMessages;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lblMessages;
        private System.Windows.Forms.Button btnExecute1;
        private System.Windows.Forms.Button btnExecute2;
        private System.Windows.Forms.Button btnExecute3;
        private System.Windows.Forms.Button btnExecute5;
        private System.Windows.Forms.Button btnExecute4;
        private System.Windows.Forms.Button btnDoCallback;
        private System.Windows.Forms.Button btnDoCallback2;
    }
}

