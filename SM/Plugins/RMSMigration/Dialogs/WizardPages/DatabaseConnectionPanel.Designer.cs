namespace LSOne.ViewPlugins.RMSMigration.Dialogs.WizardPages
{
    partial class DatabaseConnectionPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseConnectionPanel));
            this.lblDetails = new System.Windows.Forms.Label();
            this.lblUseWindowsAuthentication = new System.Windows.Forms.Label();
            this.lblDatabaseUser = new System.Windows.Forms.Label();
            this.lblConnectionType = new System.Windows.Forms.Label();
            this.cmbConnectionType = new System.Windows.Forms.ComboBox();
            this.lblServerHost = new System.Windows.Forms.Label();
            this.lblDatabasePassword = new System.Windows.Forms.Label();
            this.tbDbPwd = new System.Windows.Forms.TextBox();
            this.tbDbServer = new System.Windows.Forms.TextBox();
            this.lblDatabaseName = new System.Windows.Forms.Label();
            this.tbDbUser = new System.Windows.Forms.TextBox();
            this.tbDbDatabase = new System.Windows.Forms.TextBox();
            this.xDbWinAuth = new System.Windows.Forms.CheckBox();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblDetails
            // 
            this.lblDetails.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDetails, "lblDetails");
            this.lblDetails.Name = "lblDetails";
            // 
            // lblUseWindowsAuthentication
            // 
            resources.ApplyResources(this.lblUseWindowsAuthentication, "lblUseWindowsAuthentication");
            this.lblUseWindowsAuthentication.Name = "lblUseWindowsAuthentication";
            // 
            // lblDatabaseUser
            // 
            resources.ApplyResources(this.lblDatabaseUser, "lblDatabaseUser");
            this.lblDatabaseUser.Name = "lblDatabaseUser";
            // 
            // lblConnectionType
            // 
            resources.ApplyResources(this.lblConnectionType, "lblConnectionType");
            this.lblConnectionType.Name = "lblConnectionType";
            // 
            // cmbConnectionType
            // 
            this.cmbConnectionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConnectionType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbConnectionType, "cmbConnectionType");
            this.cmbConnectionType.Name = "cmbConnectionType";
            this.cmbConnectionType.SelectedIndexChanged += new System.EventHandler(this.cmbConnectionType_SelectedIndexChanged);
            // 
            // lblServerHost
            // 
            resources.ApplyResources(this.lblServerHost, "lblServerHost");
            this.lblServerHost.Name = "lblServerHost";
            // 
            // lblDatabasePassword
            // 
            resources.ApplyResources(this.lblDatabasePassword, "lblDatabasePassword");
            this.lblDatabasePassword.Name = "lblDatabasePassword";
            // 
            // tbDbPwd
            // 
            resources.ApplyResources(this.tbDbPwd, "tbDbPwd");
            this.tbDbPwd.Name = "tbDbPwd";
            // 
            // tbDbServer
            // 
            resources.ApplyResources(this.tbDbServer, "tbDbServer");
            this.tbDbServer.Name = "tbDbServer";
            // 
            // lblDatabaseName
            // 
            resources.ApplyResources(this.lblDatabaseName, "lblDatabaseName");
            this.lblDatabaseName.Name = "lblDatabaseName";
            // 
            // tbDbUser
            // 
            resources.ApplyResources(this.tbDbUser, "tbDbUser");
            this.tbDbUser.Name = "tbDbUser";
            // 
            // tbDbDatabase
            // 
            resources.ApplyResources(this.tbDbDatabase, "tbDbDatabase");
            this.tbDbDatabase.Name = "tbDbDatabase";
            // 
            // xDbWinAuth
            // 
            resources.ApplyResources(this.xDbWinAuth, "xDbWinAuth");
            this.xDbWinAuth.Name = "xDbWinAuth";
            this.xDbWinAuth.UseVisualStyleBackColor = true;
            this.xDbWinAuth.CheckedChanged += new System.EventHandler(this.xDbWinAuth_CheckedChanged);
            // 
            // btnTestConnection
            // 
            resources.ApplyResources(this.btnTestConnection, "btnTestConnection");
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // DatabaseConnectionPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.lblUseWindowsAuthentication);
            this.Controls.Add(this.lblDatabaseUser);
            this.Controls.Add(this.lblConnectionType);
            this.Controls.Add(this.cmbConnectionType);
            this.Controls.Add(this.lblServerHost);
            this.Controls.Add(this.lblDatabasePassword);
            this.Controls.Add(this.tbDbPwd);
            this.Controls.Add(this.tbDbServer);
            this.Controls.Add(this.lblDatabaseName);
            this.Controls.Add(this.tbDbUser);
            this.Controls.Add(this.tbDbDatabase);
            this.Controls.Add(this.xDbWinAuth);
            this.Controls.Add(this.lblDetails);
            this.Name = "DatabaseConnectionPanel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.Label lblUseWindowsAuthentication;
        private System.Windows.Forms.Label lblDatabaseUser;
        private System.Windows.Forms.Label lblConnectionType;
        private System.Windows.Forms.ComboBox cmbConnectionType;
        private System.Windows.Forms.Label lblServerHost;
        private System.Windows.Forms.Label lblDatabasePassword;
        private System.Windows.Forms.TextBox tbDbPwd;
        private System.Windows.Forms.TextBox tbDbServer;
        private System.Windows.Forms.Label lblDatabaseName;
        private System.Windows.Forms.TextBox tbDbUser;
        private System.Windows.Forms.TextBox tbDbDatabase;
        private System.Windows.Forms.CheckBox xDbWinAuth;
        private System.Windows.Forms.Button btnTestConnection;
    }
}
