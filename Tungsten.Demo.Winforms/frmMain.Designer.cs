namespace W.Demo.Winforms
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
            this.components = new System.ComponentModel.Container();
            this.txtLast = new System.Windows.Forms.TextBox();
            this.lblLast = new System.Windows.Forms.Label();
            this.lblFirst = new System.Windows.Forms.Label();
            this.txtFirst = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblFullName = new System.Windows.Forms.Label();
            this.grpMain = new System.Windows.Forms.GroupBox();
            this.pbSaveProgress = new System.Windows.Forms.ProgressBar();
            this.lblSaveComplete = new System.Windows.Forms.Label();
            this.viewModelTestBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.viewModelTestBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLast
            // 
            this.txtLast.Location = new System.Drawing.Point(9, 32);
            this.txtLast.Name = "txtLast";
            this.txtLast.Size = new System.Drawing.Size(132, 20);
            this.txtLast.TabIndex = 0;
            this.txtLast.TextChanged += new System.EventHandler(this.txtLast_TextChanged);
            // 
            // lblLast
            // 
            this.lblLast.AutoSize = true;
            this.lblLast.Location = new System.Drawing.Point(6, 16);
            this.lblLast.Name = "lblLast";
            this.lblLast.Size = new System.Drawing.Size(27, 13);
            this.lblLast.TabIndex = 1;
            this.lblLast.Text = "Last";
            // 
            // lblFirst
            // 
            this.lblFirst.AutoSize = true;
            this.lblFirst.Location = new System.Drawing.Point(144, 16);
            this.lblFirst.Name = "lblFirst";
            this.lblFirst.Size = new System.Drawing.Size(26, 13);
            this.lblFirst.TabIndex = 3;
            this.lblFirst.Text = "First";
            // 
            // txtFirst
            // 
            this.txtFirst.Location = new System.Drawing.Point(147, 32);
            this.txtFirst.Name = "txtFirst";
            this.txtFirst.Size = new System.Drawing.Size(132, 20);
            this.txtFirst.TabIndex = 2;
            this.txtFirst.TextChanged += new System.EventHandler(this.txtFirst_TextChanged);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Location = new System.Drawing.Point(124, 106);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblFullName
            // 
            this.lblFullName.Location = new System.Drawing.Point(7, 61);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(272, 13);
            this.lblFullName.TabIndex = 6;
            this.lblFullName.Text = "Full Name";
            // 
            // grpMain
            // 
            this.grpMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMain.Controls.Add(this.btnSave);
            this.grpMain.Controls.Add(this.lblFullName);
            this.grpMain.Controls.Add(this.lblLast);
            this.grpMain.Controls.Add(this.lblFirst);
            this.grpMain.Controls.Add(this.txtLast);
            this.grpMain.Controls.Add(this.txtFirst);
            this.grpMain.Location = new System.Drawing.Point(0, 0);
            this.grpMain.Name = "grpMain";
            this.grpMain.Size = new System.Drawing.Size(323, 137);
            this.grpMain.TabIndex = 7;
            this.grpMain.TabStop = false;
            // 
            // pbSaveProgress
            // 
            this.pbSaveProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbSaveProgress.Location = new System.Drawing.Point(0, 137);
            this.pbSaveProgress.Name = "pbSaveProgress";
            this.pbSaveProgress.Size = new System.Drawing.Size(323, 23);
            this.pbSaveProgress.TabIndex = 8;
            this.pbSaveProgress.Visible = false;
            // 
            // lblSaveComplete
            // 
            this.lblSaveComplete.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblSaveComplete.Location = new System.Drawing.Point(0, 124);
            this.lblSaveComplete.Name = "lblSaveComplete";
            this.lblSaveComplete.Size = new System.Drawing.Size(323, 13);
            this.lblSaveComplete.TabIndex = 7;
            this.lblSaveComplete.Text = "Save Complete";
            this.lblSaveComplete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSaveComplete.Visible = false;
            // 
            // viewModelTestBindingSource
            // 
            this.viewModelTestBindingSource.DataSource = typeof(CPerson);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 160);
            this.Controls.Add(this.grpMain);
            this.Controls.Add(this.lblSaveComplete);
            this.Controls.Add(this.pbSaveProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmMain";
            this.Text = "Tungsten.Demo.Winforms";
            this.grpMain.ResumeLayout(false);
            this.grpMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.viewModelTestBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtLast;
        private System.Windows.Forms.Label lblLast;
        private System.Windows.Forms.BindingSource viewModelTestBindingSource;
        private System.Windows.Forms.Label lblFirst;
        private System.Windows.Forms.TextBox txtFirst;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.GroupBox grpMain;
        private System.Windows.Forms.ProgressBar pbSaveProgress;
        private System.Windows.Forms.Label lblSaveComplete;
    }
}

