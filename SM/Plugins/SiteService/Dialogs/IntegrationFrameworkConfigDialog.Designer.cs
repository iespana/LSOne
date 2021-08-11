namespace LSOne.ViewPlugins.SiteService.Dialogs
{
    partial class IntegrationFrameworkConfigDialog
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnGetConfig = new System.Windows.Forms.Button();
            this.btnSendConfig = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpHttp = new System.Windows.Forms.GroupBox();
            this.cmbCertificateStore = new System.Windows.Forms.ComboBox();
            this.lblCertificateStore = new System.Windows.Forms.Label();
            this.cmbCertificateLocation = new System.Windows.Forms.ComboBox();
            this.lblCertificateLocation = new System.Windows.Forms.Label();
            this.txtCertificateThumbnail = new System.Windows.Forms.TextBox();
            this.lblCertificateThumb = new System.Windows.Forms.Label();
            this.chkHttps = new System.Windows.Forms.CheckBox();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.grpHttp.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.flowLayoutPanel1);
            this.panel2.Location = new System.Drawing.Point(-3, 206);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(519, 46);
            this.panel2.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.btnGetConfig);
            this.flowLayoutPanel1.Controls.Add(this.btnSendConfig);
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(210, 7);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(297, 29);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btnGetConfig
            // 
            this.btnGetConfig.AutoSize = true;
            this.btnGetConfig.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnGetConfig.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnGetConfig.Location = new System.Drawing.Point(3, 3);
            this.btnGetConfig.Name = "btnGetConfig";
            this.btnGetConfig.Size = new System.Drawing.Size(98, 23);
            this.btnGetConfig.TabIndex = 0;
            this.btnGetConfig.Text = "Get configuration";
            this.btnGetConfig.UseVisualStyleBackColor = true;
            this.btnGetConfig.Click += new System.EventHandler(this.OnGetConfigClick);
            // 
            // btnSendConfig
            // 
            this.btnSendConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendConfig.AutoSize = true;
            this.btnSendConfig.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSendConfig.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSendConfig.Location = new System.Drawing.Point(107, 3);
            this.btnSendConfig.Name = "btnSendConfig";
            this.btnSendConfig.Size = new System.Drawing.Size(106, 23);
            this.btnSendConfig.TabIndex = 1;
            this.btnSendConfig.Text = "Send configuration";
            this.btnSendConfig.UseVisualStyleBackColor = true;
            this.btnSendConfig.Click += new System.EventHandler(this.OnSendConfigClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoSize = true;
            this.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(219, 3);
            this.btnCancel.MinimumSize = new System.Drawing.Size(75, 23);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grpHttp
            // 
            this.grpHttp.Controls.Add(this.cmbCertificateStore);
            this.grpHttp.Controls.Add(this.lblCertificateStore);
            this.grpHttp.Controls.Add(this.cmbCertificateLocation);
            this.grpHttp.Controls.Add(this.lblCertificateLocation);
            this.grpHttp.Controls.Add(this.txtCertificateThumbnail);
            this.grpHttp.Controls.Add(this.lblCertificateThumb);
            this.grpHttp.Controls.Add(this.chkHttps);
            this.grpHttp.Location = new System.Drawing.Point(12, 66);
            this.grpHttp.Name = "grpHttp";
            this.grpHttp.Size = new System.Drawing.Size(492, 128);
            this.grpHttp.TabIndex = 0;
            this.grpHttp.TabStop = false;
            this.grpHttp.Text = "Http configuration";
            // 
            // cmbCertificateStore
            // 
            this.cmbCertificateStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCertificateStore.Enabled = false;
            this.cmbCertificateStore.FormattingEnabled = true;
            this.cmbCertificateStore.Items.AddRange(new object[] {
            "AddressBook",
            "AuthRoot",
            "CertificateAuthority",
            "Disallowed",
            "My",
            "Root",
            "TrustedPeople",
            "TrustedPublisher"});
            this.cmbCertificateStore.Location = new System.Drawing.Point(182, 94);
            this.cmbCertificateStore.Name = "cmbCertificateStore";
            this.cmbCertificateStore.Size = new System.Drawing.Size(199, 21);
            this.cmbCertificateStore.TabIndex = 6;
            // 
            // lblCertificateStore
            // 
            this.lblCertificateStore.Location = new System.Drawing.Point(6, 92);
            this.lblCertificateStore.Name = "lblCertificateStore";
            this.lblCertificateStore.Size = new System.Drawing.Size(170, 23);
            this.lblCertificateStore.TabIndex = 5;
            this.lblCertificateStore.Text = "SSL certificate store:";
            this.lblCertificateStore.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbCertificateLocation
            // 
            this.cmbCertificateLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCertificateLocation.Enabled = false;
            this.cmbCertificateLocation.FormattingEnabled = true;
            this.cmbCertificateLocation.Items.AddRange(new object[] {
            "CurrentUser",
            "LocalMachine"});
            this.cmbCertificateLocation.Location = new System.Drawing.Point(182, 67);
            this.cmbCertificateLocation.Name = "cmbCertificateLocation";
            this.cmbCertificateLocation.Size = new System.Drawing.Size(199, 21);
            this.cmbCertificateLocation.TabIndex = 4;
            // 
            // lblCertificateLocation
            // 
            this.lblCertificateLocation.Location = new System.Drawing.Point(6, 65);
            this.lblCertificateLocation.Name = "lblCertificateLocation";
            this.lblCertificateLocation.Size = new System.Drawing.Size(170, 23);
            this.lblCertificateLocation.TabIndex = 3;
            this.lblCertificateLocation.Text = "SSL certificate location:";
            this.lblCertificateLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCertificateThumbnail
            // 
            this.txtCertificateThumbnail.Enabled = false;
            this.txtCertificateThumbnail.Location = new System.Drawing.Point(182, 41);
            this.txtCertificateThumbnail.Name = "txtCertificateThumbnail";
            this.txtCertificateThumbnail.Size = new System.Drawing.Size(281, 20);
            this.txtCertificateThumbnail.TabIndex = 2;
            // 
            // lblCertificateThumb
            // 
            this.lblCertificateThumb.Location = new System.Drawing.Point(6, 39);
            this.lblCertificateThumb.Name = "lblCertificateThumb";
            this.lblCertificateThumb.Size = new System.Drawing.Size(170, 23);
            this.lblCertificateThumb.TabIndex = 1;
            this.lblCertificateThumb.Text = "SSL certificate thumbnail:";
            this.lblCertificateThumb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkHttps
            // 
            this.chkHttps.AutoSize = true;
            this.chkHttps.Location = new System.Drawing.Point(22, 19);
            this.chkHttps.Name = "chkHttps";
            this.chkHttps.Size = new System.Drawing.Size(98, 17);
            this.chkHttps.TabIndex = 0;
            this.chkHttps.Text = "Enable HTTPS";
            this.chkHttps.UseVisualStyleBackColor = true;
            this.chkHttps.CheckedChanged += new System.EventHandler(this.chkHttps_CheckedChanged);
            // 
            // IntegrationFrameworkConfigDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(514, 248);
            this.Controls.Add(this.grpHttp);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Header = "Manage configuration settings for Integration Framework";
            this.Name = "IntegrationFrameworkConfigDialog";
            this.Text = "Integration Framework configuration";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.grpHttp, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.grpHttp.ResumeLayout(false);
            this.grpHttp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnGetConfig;
        private System.Windows.Forms.Button btnSendConfig;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpHttp;
        private System.Windows.Forms.ComboBox cmbCertificateStore;
        private System.Windows.Forms.Label lblCertificateStore;
        private System.Windows.Forms.ComboBox cmbCertificateLocation;
        private System.Windows.Forms.Label lblCertificateLocation;
        private System.Windows.Forms.TextBox txtCertificateThumbnail;
        private System.Windows.Forms.Label lblCertificateThumb;
        private System.Windows.Forms.CheckBox chkHttps;
    }
}