namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    partial class DataDirectorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataDirectorDialog));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panelProgress = new System.Windows.Forms.Panel();
            this.tbProgressMessage = new System.Windows.Forms.TextBox();
            this.pgbProcess = new System.Windows.Forms.ProgressBar();
            this.panelMessage = new System.Windows.Forms.Panel();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.picMessage = new System.Windows.Forms.PictureBox();
            this.panelProgress.SuspendLayout();
            this.panelMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMessage)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panelProgress
            // 
            this.panelProgress.Controls.Add(this.tbProgressMessage);
            this.panelProgress.Controls.Add(this.pgbProcess);
            resources.ApplyResources(this.panelProgress, "panelProgress");
            this.panelProgress.Name = "panelProgress";
            // 
            // tbProgressMessage
            // 
            resources.ApplyResources(this.tbProgressMessage, "tbProgressMessage");
            this.tbProgressMessage.BackColor = System.Drawing.Color.White;
            this.tbProgressMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbProgressMessage.Name = "tbProgressMessage";
            this.tbProgressMessage.ReadOnly = true;
            this.tbProgressMessage.TabStop = false;
            // 
            // pgbProcess
            // 
            resources.ApplyResources(this.pgbProcess, "pgbProcess");
            this.pgbProcess.Name = "pgbProcess";
            this.pgbProcess.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // panelMessage
            // 
            this.panelMessage.Controls.Add(this.tbMessage);
            this.panelMessage.Controls.Add(this.picMessage);
            resources.ApplyResources(this.panelMessage, "panelMessage");
            this.panelMessage.Name = "panelMessage";
            // 
            // tbMessage
            // 
            resources.ApplyResources(this.tbMessage, "tbMessage");
            this.tbMessage.BackColor = System.Drawing.Color.White;
            this.tbMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.ReadOnly = true;
            this.tbMessage.TabStop = false;
            // 
            // picMessage
            // 
            resources.ApplyResources(this.picMessage, "picMessage");
            this.picMessage.Name = "picMessage";
            this.picMessage.TabStop = false;
            // 
            // DataDirectorDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelMessage);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.panelProgress);
            this.Name = "DataDirectorDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.DataDirectorDialog_Load);
            this.Controls.SetChildIndex(this.panelProgress, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.panelMessage, 0);
            this.panelProgress.ResumeLayout(false);
            this.panelProgress.PerformLayout();
            this.panelMessage.ResumeLayout(false);
            this.panelMessage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMessage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panelProgress;
        private System.Windows.Forms.TextBox tbProgressMessage;
        private System.Windows.Forms.ProgressBar pgbProcess;
        private System.Windows.Forms.Panel panelMessage;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.PictureBox picMessage;
    }
}