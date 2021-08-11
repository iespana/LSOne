using LSOne.Controls;

namespace LSOne.ViewPlugins.SiteService.Dialogs
{
    partial class SMTPSettingsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SMTPSettingsDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblServer = new System.Windows.Forms.Label();
            this.tbSmtpServer = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.tbSignature = new System.Windows.Forms.TextBox();
            this.chkTextOnly = new System.Windows.Forms.CheckBox();
            this.chkUseSSL = new System.Windows.Forms.CheckBox();
            this.lblMaximumAttempts = new System.Windows.Forms.Label();
            this.lblUseSSL = new System.Windows.Forms.Label();
            this.lblTextOnly = new System.Windows.Forms.Label();
            this.grpStore = new System.Windows.Forms.GroupBox();
            this.cmbStore = new DualDataComboBox();
            this.lblStore = new System.Windows.Forms.Label();
            this.grpServer = new System.Windows.Forms.GroupBox();
            this.ntbPort = new NumericTextBox();
            this.grpAuthentication = new System.Windows.Forms.GroupBox();
            this.tbDisplayName = new System.Windows.Forms.TextBox();
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lblEmailAddress = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.tbEmailAddress = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.grpOptions.SuspendLayout();
            this.grpStore.SuspendLayout();
            this.grpServer.SuspendLayout();
            this.grpAuthentication.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.flowLayoutPanel1);
            this.panel2.Name = "panel2";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.btnTest);
            this.flowLayoutPanel1.Controls.Add(this.btnClear);
            this.flowLayoutPanel1.Controls.Add(this.btnSave);
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // btnTest
            // 
            resources.ApplyResources(this.btnTest, "btnTest");
            this.btnTest.Name = "btnTest";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.OnSaveClick);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lblServer
            // 
            resources.ApplyResources(this.lblServer, "lblServer");
            this.lblServer.Name = "lblServer";
            // 
            // tbSmtpServer
            // 
            resources.ApplyResources(this.tbSmtpServer, "tbSmtpServer");
            this.tbSmtpServer.Name = "tbSmtpServer";
            this.tbSmtpServer.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblPort
            // 
            resources.ApplyResources(this.lblPort, "lblPort");
            this.lblPort.Name = "lblPort";
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.tbSignature);
            this.grpOptions.Controls.Add(this.chkTextOnly);
            this.grpOptions.Controls.Add(this.chkUseSSL);
            this.grpOptions.Controls.Add(this.lblMaximumAttempts);
            this.grpOptions.Controls.Add(this.lblUseSSL);
            this.grpOptions.Controls.Add(this.lblTextOnly);
            resources.ApplyResources(this.grpOptions, "grpOptions");
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.TabStop = false;
            // 
            // tbSignature
            // 
            this.tbSignature.AcceptsReturn = true;
            resources.ApplyResources(this.tbSignature, "tbSignature");
            this.tbSignature.Name = "tbSignature";
            // 
            // chkTextOnly
            // 
            resources.ApplyResources(this.chkTextOnly, "chkTextOnly");
            this.chkTextOnly.Name = "chkTextOnly";
            this.chkTextOnly.UseVisualStyleBackColor = true;
            // 
            // chkUseSSL
            // 
            resources.ApplyResources(this.chkUseSSL, "chkUseSSL");
            this.chkUseSSL.Name = "chkUseSSL";
            this.chkUseSSL.UseVisualStyleBackColor = true;
            // 
            // lblMaximumAttempts
            // 
            resources.ApplyResources(this.lblMaximumAttempts, "lblMaximumAttempts");
            this.lblMaximumAttempts.Name = "lblMaximumAttempts";
            // 
            // lblUseSSL
            // 
            resources.ApplyResources(this.lblUseSSL, "lblUseSSL");
            this.lblUseSSL.Name = "lblUseSSL";
            // 
            // lblTextOnly
            // 
            resources.ApplyResources(this.lblTextOnly, "lblTextOnly");
            this.lblTextOnly.Name = "lblTextOnly";
            // 
            // grpStore
            // 
            this.grpStore.Controls.Add(this.cmbStore);
            this.grpStore.Controls.Add(this.lblStore);
            resources.ApplyResources(this.grpStore, "grpStore");
            this.grpStore.Name = "grpStore";
            this.grpStore.TabStop = false;
            // 
            // cmbStore
            // 
            this.cmbStore.AddList = null;
            this.cmbStore.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbStore, "cmbStore");
            this.cmbStore.MaxLength = 32767;
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.OnlyDisplayID = false;
            this.cmbStore.RemoveList = null;
            this.cmbStore.RowHeight = ((short)(22));
            this.cmbStore.SelectedData = null;
            this.cmbStore.SelectedDataID = null;
            this.cmbStore.SelectionList = null;
            this.cmbStore.SkipIDColumn = false;
            this.cmbStore.RequestData += new System.EventHandler(this.cmbStore_RequestData);
            this.cmbStore.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblStore
            // 
            this.lblStore.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblStore, "lblStore");
            this.lblStore.Name = "lblStore";
            // 
            // grpServer
            // 
            this.grpServer.Controls.Add(this.ntbPort);
            this.grpServer.Controls.Add(this.lblServer);
            this.grpServer.Controls.Add(this.lblPort);
            this.grpServer.Controls.Add(this.tbSmtpServer);
            resources.ApplyResources(this.grpServer, "grpServer");
            this.grpServer.Name = "grpServer";
            this.grpServer.TabStop = false;
            // 
            // ntbPort
            // 
            this.ntbPort.AllowDecimal = false;
            this.ntbPort.AllowNegative = false;
            this.ntbPort.CultureInfo = null;
            this.ntbPort.DecimalLetters = 2;
            this.ntbPort.HasMinValue = false;
            resources.ApplyResources(this.ntbPort, "ntbPort");
            this.ntbPort.MaxValue = 0D;
            this.ntbPort.MinValue = 0D;
            this.ntbPort.Name = "ntbPort";
            this.ntbPort.Value = 25D;
            this.ntbPort.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // grpAuthentication
            // 
            this.grpAuthentication.Controls.Add(this.tbDisplayName);
            this.grpAuthentication.Controls.Add(this.lblDisplayName);
            this.grpAuthentication.Controls.Add(this.tbPassword);
            this.grpAuthentication.Controls.Add(this.lblEmailAddress);
            this.grpAuthentication.Controls.Add(this.lblPassword);
            this.grpAuthentication.Controls.Add(this.tbEmailAddress);
            resources.ApplyResources(this.grpAuthentication, "grpAuthentication");
            this.grpAuthentication.Name = "grpAuthentication";
            this.grpAuthentication.TabStop = false;
            // 
            // tbDisplayName
            // 
            resources.ApplyResources(this.tbDisplayName, "tbDisplayName");
            this.tbDisplayName.Name = "tbDisplayName";
            // 
            // lblDisplayName
            // 
            resources.ApplyResources(this.lblDisplayName, "lblDisplayName");
            this.lblDisplayName.Name = "lblDisplayName";
            // 
            // tbPassword
            // 
            resources.ApplyResources(this.tbPassword, "tbPassword");
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblEmailAddress
            // 
            resources.ApplyResources(this.lblEmailAddress, "lblEmailAddress");
            this.lblEmailAddress.Name = "lblEmailAddress";
            // 
            // lblPassword
            // 
            resources.ApplyResources(this.lblPassword, "lblPassword");
            this.lblPassword.Name = "lblPassword";
            // 
            // tbEmailAddress
            // 
            resources.ApplyResources(this.tbEmailAddress, "tbEmailAddress");
            this.tbEmailAddress.Name = "tbEmailAddress";
            this.tbEmailAddress.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // btnClear
            // 
            resources.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.Name = "btnClear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.OnClearClick);
            // 
            // SMTPSettingsDialog
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.grpAuthentication);
            this.Controls.Add(this.grpServer);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.grpStore);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "SMTPSettingsDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.grpStore, 0);
            this.Controls.SetChildIndex(this.grpOptions, 0);
            this.Controls.SetChildIndex(this.grpServer, 0);
            this.Controls.SetChildIndex(this.grpAuthentication, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.grpStore.ResumeLayout(false);
            this.grpServer.ResumeLayout(false);
            this.grpServer.PerformLayout();
            this.grpAuthentication.ResumeLayout(false);
            this.grpAuthentication.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox tbSmtpServer;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.Label lblMaximumAttempts;
        private System.Windows.Forms.Label lblUseSSL;
        private System.Windows.Forms.Label lblTextOnly;
        private System.Windows.Forms.GroupBox grpServer;
        private System.Windows.Forms.GroupBox grpStore;
        private DualDataComboBox cmbStore;
        private System.Windows.Forms.Label lblStore;
        private System.Windows.Forms.GroupBox grpAuthentication;
        private System.Windows.Forms.TextBox tbDisplayName;
        private System.Windows.Forms.Label lblDisplayName;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lblEmailAddress;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox tbEmailAddress;
        private NumericTextBox ntbPort;
        private System.Windows.Forms.TextBox tbSignature;
        private System.Windows.Forms.CheckBox chkTextOnly;
        private System.Windows.Forms.CheckBox chkUseSSL;
        private System.Windows.Forms.Button btnClear;
    }
}