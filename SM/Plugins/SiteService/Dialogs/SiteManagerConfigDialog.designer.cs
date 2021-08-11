using LSOne.Controls;

namespace LSOne.ViewPlugins.SiteService.Dialogs
{
    partial class SiteManagerConfigDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SiteManagerConfigDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnGetConfig = new System.Windows.Forms.Button();
            this.btnSendConfig = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbDbConType = new System.Windows.Forms.ComboBox();
            this.tbAreaid = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbDbPwd = new System.Windows.Forms.TextBox();
            this.tbDbServer = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbDbUser = new System.Windows.Forms.TextBox();
            this.tbDbDatabase = new System.Windows.Forms.TextBox();
            this.xDbWinAuth = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cmTruncate = new System.Windows.Forms.ComboBox();
            this.lblTruncate = new System.Windows.Forms.Label();
            this.lblMaximumAttempts = new System.Windows.Forms.Label();
            this.ntbMaximumAttempts = new LSOne.Controls.NumericTextBox();
            this.btnSmtpSettings = new System.Windows.Forms.Button();
            this.ntbSendInterval = new LSOne.Controls.NumericTextBox();
            this.lblSendInterval = new System.Windows.Forms.Label();
            this.lblMaximumEmails = new System.Windows.Forms.Label();
            this.ntbMaximumEmails = new LSOne.Controls.NumericTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ntbDaysToKeepLogs = new LSOne.Controls.NumericTextBox();
            this.lblDaysToKeepLogs = new System.Windows.Forms.Label();
            this.lblServiceOverride = new System.Windows.Forms.Label();
            this.txtServiceOverride = new System.Windows.Forms.TextBox();
            this.ntbMaxUserConnections = new LSOne.Controls.NumericTextBox();
            this.ntbMaxCount = new LSOne.Controls.NumericTextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbPrivateHashKey = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbExternalAddress = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.flowLayoutPanel1.Controls.Add(this.btnGetConfig);
            this.flowLayoutPanel1.Controls.Add(this.btnSendConfig);
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // btnGetConfig
            // 
            resources.ApplyResources(this.btnGetConfig, "btnGetConfig");
            this.btnGetConfig.Image = global::LSOne.ViewPlugins.SiteService.Properties.Resources.LockedSmallImage;
            this.btnGetConfig.Name = "btnGetConfig";
            this.btnGetConfig.UseVisualStyleBackColor = true;
            this.btnGetConfig.Click += new System.EventHandler(this.btnGetConfig_Click);
            // 
            // btnSendConfig
            // 
            resources.ApplyResources(this.btnSendConfig, "btnSendConfig");
            this.btnSendConfig.Image = global::LSOne.ViewPlugins.SiteService.Properties.Resources.LockedSmallImage;
            this.btnSendConfig.Name = "btnSendConfig";
            this.btnSendConfig.UseVisualStyleBackColor = true;
            this.btnSendConfig.Click += new System.EventHandler(this.btnSendConfig_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.cbDbConType);
            this.groupBox2.Controls.Add(this.tbAreaid);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tbDbPwd);
            this.groupBox2.Controls.Add(this.tbDbServer);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tbDbUser);
            this.groupBox2.Controls.Add(this.tbDbDatabase);
            this.groupBox2.Controls.Add(this.xDbWinAuth);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // cbDbConType
            // 
            this.cbDbConType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDbConType.FormattingEnabled = true;
            this.cbDbConType.Items.AddRange(new object[] {
            resources.GetString("cbDbConType.Items"),
            resources.GetString("cbDbConType.Items1"),
            resources.GetString("cbDbConType.Items2")});
            resources.ApplyResources(this.cbDbConType, "cbDbConType");
            this.cbDbConType.Name = "cbDbConType";
            // 
            // tbAreaid
            // 
            resources.ApplyResources(this.tbAreaid, "tbAreaid");
            this.tbAreaid.Name = "tbAreaid";
            this.tbAreaid.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // tbDbPwd
            // 
            resources.ApplyResources(this.tbDbPwd, "tbDbPwd");
            this.tbDbPwd.Name = "tbDbPwd";
            this.tbDbPwd.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // tbDbServer
            // 
            resources.ApplyResources(this.tbDbServer, "tbDbServer");
            this.tbDbServer.Name = "tbDbServer";
            this.tbDbServer.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // tbDbUser
            // 
            resources.ApplyResources(this.tbDbUser, "tbDbUser");
            this.tbDbUser.Name = "tbDbUser";
            this.tbDbUser.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // tbDbDatabase
            // 
            resources.ApplyResources(this.tbDbDatabase, "tbDbDatabase");
            this.tbDbDatabase.Name = "tbDbDatabase";
            this.tbDbDatabase.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // xDbWinAuth
            // 
            resources.ApplyResources(this.xDbWinAuth, "xDbWinAuth");
            this.xDbWinAuth.Name = "xDbWinAuth";
            this.xDbWinAuth.UseVisualStyleBackColor = true;
            this.xDbWinAuth.CheckedChanged += new System.EventHandler(this.xDbWinAuth_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cmTruncate);
            this.groupBox4.Controls.Add(this.lblTruncate);
            this.groupBox4.Controls.Add(this.lblMaximumAttempts);
            this.groupBox4.Controls.Add(this.ntbMaximumAttempts);
            this.groupBox4.Controls.Add(this.btnSmtpSettings);
            this.groupBox4.Controls.Add(this.ntbSendInterval);
            this.groupBox4.Controls.Add(this.lblSendInterval);
            this.groupBox4.Controls.Add(this.lblMaximumEmails);
            this.groupBox4.Controls.Add(this.ntbMaximumEmails);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // cmTruncate
            // 
            this.cmTruncate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmTruncate.FormattingEnabled = true;
            this.cmTruncate.Items.AddRange(new object[] {
            resources.GetString("cmTruncate.Items"),
            resources.GetString("cmTruncate.Items1"),
            resources.GetString("cmTruncate.Items2")});
            resources.ApplyResources(this.cmTruncate, "cmTruncate");
            this.cmTruncate.Name = "cmTruncate";
            // 
            // lblTruncate
            // 
            resources.ApplyResources(this.lblTruncate, "lblTruncate");
            this.lblTruncate.Name = "lblTruncate";
            // 
            // lblMaximumAttempts
            // 
            resources.ApplyResources(this.lblMaximumAttempts, "lblMaximumAttempts");
            this.lblMaximumAttempts.Name = "lblMaximumAttempts";
            // 
            // ntbMaximumAttempts
            // 
            this.ntbMaximumAttempts.AllowDecimal = false;
            this.ntbMaximumAttempts.AllowNegative = false;
            this.ntbMaximumAttempts.CultureInfo = null;
            this.ntbMaximumAttempts.DecimalLetters = 2;
            this.ntbMaximumAttempts.ForeColor = System.Drawing.Color.Black;
            this.ntbMaximumAttempts.HasMinValue = false;
            resources.ApplyResources(this.ntbMaximumAttempts, "ntbMaximumAttempts");
            this.ntbMaximumAttempts.MaxValue = 1000D;
            this.ntbMaximumAttempts.MinValue = 0D;
            this.ntbMaximumAttempts.Name = "ntbMaximumAttempts";
            this.ntbMaximumAttempts.Value = 0D;
            // 
            // btnSmtpSettings
            // 
            resources.ApplyResources(this.btnSmtpSettings, "btnSmtpSettings");
            this.btnSmtpSettings.Name = "btnSmtpSettings";
            this.btnSmtpSettings.UseVisualStyleBackColor = true;
            this.btnSmtpSettings.Click += new System.EventHandler(this.btnSmtpSettings_Click);
            // 
            // ntbSendInterval
            // 
            this.ntbSendInterval.AllowDecimal = false;
            this.ntbSendInterval.AllowNegative = false;
            this.ntbSendInterval.CultureInfo = null;
            this.ntbSendInterval.DecimalLetters = 2;
            this.ntbSendInterval.ForeColor = System.Drawing.Color.Black;
            this.ntbSendInterval.HasMinValue = false;
            resources.ApplyResources(this.ntbSendInterval, "ntbSendInterval");
            this.ntbSendInterval.MaxValue = 3600D;
            this.ntbSendInterval.MinValue = 0D;
            this.ntbSendInterval.Name = "ntbSendInterval";
            this.ntbSendInterval.Value = 0D;
            // 
            // lblSendInterval
            // 
            resources.ApplyResources(this.lblSendInterval, "lblSendInterval");
            this.lblSendInterval.Name = "lblSendInterval";
            // 
            // lblMaximumEmails
            // 
            resources.ApplyResources(this.lblMaximumEmails, "lblMaximumEmails");
            this.lblMaximumEmails.Name = "lblMaximumEmails";
            // 
            // ntbMaximumEmails
            // 
            this.ntbMaximumEmails.AllowDecimal = false;
            this.ntbMaximumEmails.AllowNegative = false;
            this.ntbMaximumEmails.CultureInfo = null;
            this.ntbMaximumEmails.DecimalLetters = 2;
            this.ntbMaximumEmails.ForeColor = System.Drawing.Color.Black;
            this.ntbMaximumEmails.HasMinValue = false;
            resources.ApplyResources(this.ntbMaximumEmails, "ntbMaximumEmails");
            this.ntbMaximumEmails.MaxValue = 1000D;
            this.ntbMaximumEmails.MinValue = 0D;
            this.ntbMaximumEmails.Name = "ntbMaximumEmails";
            this.ntbMaximumEmails.Value = 0D;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ntbDaysToKeepLogs);
            this.groupBox1.Controls.Add(this.lblDaysToKeepLogs);
            this.groupBox1.Controls.Add(this.lblServiceOverride);
            this.groupBox1.Controls.Add(this.txtServiceOverride);
            this.groupBox1.Controls.Add(this.ntbMaxUserConnections);
            this.groupBox1.Controls.Add(this.ntbMaxCount);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tbPrivateHashKey);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbExternalAddress);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // ntbDaysToKeepLogs
            // 
            this.ntbDaysToKeepLogs.AllowDecimal = false;
            this.ntbDaysToKeepLogs.AllowNegative = false;
            this.ntbDaysToKeepLogs.CultureInfo = null;
            this.ntbDaysToKeepLogs.DecimalLetters = 2;
            this.ntbDaysToKeepLogs.ForeColor = System.Drawing.Color.Black;
            this.ntbDaysToKeepLogs.HasMinValue = false;
            resources.ApplyResources(this.ntbDaysToKeepLogs, "ntbDaysToKeepLogs");
            this.ntbDaysToKeepLogs.MaxValue = 3600D;
            this.ntbDaysToKeepLogs.MinValue = 0D;
            this.ntbDaysToKeepLogs.Name = "ntbDaysToKeepLogs";
            this.ntbDaysToKeepLogs.Value = 30D;
            // 
            // lblDaysToKeepLogs
            // 
            resources.ApplyResources(this.lblDaysToKeepLogs, "lblDaysToKeepLogs");
            this.lblDaysToKeepLogs.Name = "lblDaysToKeepLogs";
            // 
            // lblServiceOverride
            // 
            resources.ApplyResources(this.lblServiceOverride, "lblServiceOverride");
            this.lblServiceOverride.Name = "lblServiceOverride";
            // 
            // txtServiceOverride
            // 
            resources.ApplyResources(this.txtServiceOverride, "txtServiceOverride");
            this.txtServiceOverride.Name = "txtServiceOverride";
            // 
            // ntbMaxUserConnections
            // 
            this.ntbMaxUserConnections.AllowDecimal = false;
            this.ntbMaxUserConnections.AllowNegative = false;
            this.ntbMaxUserConnections.CultureInfo = null;
            this.ntbMaxUserConnections.DecimalLetters = 2;
            this.ntbMaxUserConnections.ForeColor = System.Drawing.Color.Black;
            this.ntbMaxUserConnections.HasMinValue = false;
            resources.ApplyResources(this.ntbMaxUserConnections, "ntbMaxUserConnections");
            this.ntbMaxUserConnections.MaxValue = 3600D;
            this.ntbMaxUserConnections.MinValue = 0D;
            this.ntbMaxUserConnections.Name = "ntbMaxUserConnections";
            this.ntbMaxUserConnections.Value = 5D;
            // 
            // ntbMaxCount
            // 
            this.ntbMaxCount.AllowDecimal = false;
            this.ntbMaxCount.AllowNegative = false;
            this.ntbMaxCount.CultureInfo = null;
            this.ntbMaxCount.DecimalLetters = 2;
            this.ntbMaxCount.ForeColor = System.Drawing.Color.Black;
            this.ntbMaxCount.HasMinValue = false;
            resources.ApplyResources(this.ntbMaxCount, "ntbMaxCount");
            this.ntbMaxCount.MaxValue = 3600D;
            this.ntbMaxCount.MinValue = 0D;
            this.ntbMaxCount.Name = "ntbMaxCount";
            this.ntbMaxCount.Value = 50D;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // tbPrivateHashKey
            // 
            resources.ApplyResources(this.tbPrivateHashKey, "tbPrivateHashKey");
            this.tbPrivateHashKey.Name = "tbPrivateHashKey";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // tbExternalAddress
            // 
            resources.ApplyResources(this.tbExternalAddress, "tbExternalAddress");
            this.tbExternalAddress.Name = "tbExternalAddress";
            // 
            // SiteManagerConfigDialog
            // 
            this.AcceptButton = this.btnSendConfig;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "SiteManagerConfigDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.groupBox4, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSendConfig;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbDbConType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbAreaid;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox xDbWinAuth;
        private System.Windows.Forms.TextBox tbDbPwd;
        private System.Windows.Forms.TextBox tbDbServer;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbDbUser;
        private System.Windows.Forms.TextBox tbDbDatabase;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnGetConfig;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lblMaximumAttempts;
        private NumericTextBox ntbMaximumAttempts;
        private System.Windows.Forms.Button btnSmtpSettings;
        private NumericTextBox ntbSendInterval;
        private System.Windows.Forms.Label lblSendInterval;
        private System.Windows.Forms.Label lblMaximumEmails;
        private NumericTextBox ntbMaximumEmails;
        private System.Windows.Forms.Label lblTruncate;
        private System.Windows.Forms.ComboBox cmTruncate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbPrivateHashKey;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbExternalAddress;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private NumericTextBox ntbMaxUserConnections;
        private NumericTextBox ntbMaxCount;
        private System.Windows.Forms.Label lblServiceOverride;
        private System.Windows.Forms.TextBox txtServiceOverride;
        private NumericTextBox ntbDaysToKeepLogs;
        private System.Windows.Forms.Label lblDaysToKeepLogs;
	}
}