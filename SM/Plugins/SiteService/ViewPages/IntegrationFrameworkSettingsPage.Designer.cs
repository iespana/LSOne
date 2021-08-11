namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    partial class IntegrationFrameworkSettingsPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.btnConfiguration = new System.Windows.Forms.Button();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.chkHttp = new System.Windows.Forms.CheckBox();
            this.chkNetPort = new System.Windows.Forms.CheckBox();
            this.txtHttpPort = new System.Windows.Forms.TextBox();
            this.txtNetPort = new System.Windows.Forms.TextBox();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTcp = new System.Windows.Forms.Label();
            this.lblHost = new System.Windows.Forms.Label();
            this.grpSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.btnConfiguration);
            this.grpSettings.Controls.Add(this.btnTestConnection);
            this.grpSettings.Controls.Add(this.chkHttp);
            this.grpSettings.Controls.Add(this.chkNetPort);
            this.grpSettings.Controls.Add(this.txtHttpPort);
            this.grpSettings.Controls.Add(this.txtNetPort);
            this.grpSettings.Controls.Add(this.txtHost);
            this.grpSettings.Controls.Add(this.label2);
            this.grpSettings.Controls.Add(this.lblTcp);
            this.grpSettings.Controls.Add(this.lblHost);
            this.grpSettings.Location = new System.Drawing.Point(3, 3);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(392, 118);
            this.grpSettings.TabIndex = 0;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "General IF settings";
            // 
            // btnConfiguration
            // 
            this.btnConfiguration.Location = new System.Drawing.Point(256, 80);
            this.btnConfiguration.Name = "btnConfiguration";
            this.btnConfiguration.Size = new System.Drawing.Size(115, 23);
            this.btnConfiguration.TabIndex = 9;
            this.btnConfiguration.Text = "Configuration";
            this.btnConfiguration.UseVisualStyleBackColor = true;
            this.btnConfiguration.Click += new System.EventHandler(this.btnConfiguration_Click);
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(256, 54);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(115, 23);
            this.btnTestConnection.TabIndex = 5;
            this.btnTestConnection.Text = "Test connection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // chkHttp
            // 
            this.chkHttp.AutoSize = true;
            this.chkHttp.Location = new System.Drawing.Point(168, 85);
            this.chkHttp.Name = "chkHttp";
            this.chkHttp.Size = new System.Drawing.Size(15, 14);
            this.chkHttp.TabIndex = 7;
            this.chkHttp.UseVisualStyleBackColor = true;
            this.chkHttp.CheckedChanged += new System.EventHandler(this.chkHttp_CheckedChanged);
            // 
            // chkNetPort
            // 
            this.chkNetPort.AutoSize = true;
            this.chkNetPort.Location = new System.Drawing.Point(168, 59);
            this.chkNetPort.Name = "chkNetPort";
            this.chkNetPort.Size = new System.Drawing.Size(15, 14);
            this.chkNetPort.TabIndex = 3;
            this.chkNetPort.UseVisualStyleBackColor = true;
            this.chkNetPort.CheckedChanged += new System.EventHandler(this.chkNetPort_CheckedChanged);
            // 
            // txtHttpPort
            // 
            this.txtHttpPort.Enabled = false;
            this.txtHttpPort.Location = new System.Drawing.Point(189, 82);
            this.txtHttpPort.MaxLength = 6;
            this.txtHttpPort.Name = "txtHttpPort";
            this.txtHttpPort.Size = new System.Drawing.Size(62, 20);
            this.txtHttpPort.TabIndex = 8;
            this.txtHttpPort.Text = "9103";
            this.txtHttpPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxKeyPress);
            // 
            // txtNetPort
            // 
            this.txtNetPort.Enabled = false;
            this.txtNetPort.Location = new System.Drawing.Point(189, 56);
            this.txtNetPort.MaxLength = 6;
            this.txtNetPort.Name = "txtNetPort";
            this.txtNetPort.Size = new System.Drawing.Size(62, 20);
            this.txtNetPort.TabIndex = 4;
            this.txtNetPort.Text = "9102";
            this.txtNetPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxKeyPress);
            // 
            // txtHost
            // 
            this.txtHost.Enabled = false;
            this.txtHost.Location = new System.Drawing.Point(168, 28);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(203, 20);
            this.txtHost.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 23);
            this.label2.TabIndex = 6;
            this.label2.Text = "Http port:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTcp
            // 
            this.lblTcp.Location = new System.Drawing.Point(15, 54);
            this.lblTcp.Name = "lblTcp";
            this.lblTcp.Size = new System.Drawing.Size(147, 23);
            this.lblTcp.TabIndex = 2;
            this.lblTcp.Text = "Net/TCP port:";
            this.lblTcp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHost
            // 
            this.lblHost.Location = new System.Drawing.Point(9, 26);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(153, 23);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "Host:";
            this.lblHost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IntegrationFrameworkSettingsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpSettings);
            this.Name = "IntegrationFrameworkSettingsPage";
            this.Size = new System.Drawing.Size(441, 157);
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.CheckBox chkHttp;
        private System.Windows.Forms.CheckBox chkNetPort;
        private System.Windows.Forms.TextBox txtHttpPort;
        private System.Windows.Forms.TextBox txtNetPort;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTcp;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.Button btnConfiguration;
        private System.Windows.Forms.Button btnTestConnection;
    }
}
