using LSOne.Controls;

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    partial class AdministrationStoreServerPage
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdministrationStoreServerPage));
			this.lblAddress = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.tbHost = new System.Windows.Forms.TextBox();
			this.ntbPort = new LSOne.Controls.NumericTextBox();
			this.btnTestConnection = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnEditTransactionProfiles = new LSOne.Controls.ContextButton();
			this.cmbTransactionServiceProfile = new LSOne.Controls.DualDataComboBox();
			this.label11 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblAddress
			// 
			resources.ApplyResources(this.lblAddress, "lblAddress");
			this.lblAddress.Name = "lblAddress";
			// 
			// lblName
			// 
			resources.ApplyResources(this.lblName, "lblName");
			this.lblName.Name = "lblName";
			// 
			// tbHost
			// 
			resources.ApplyResources(this.tbHost, "tbHost");
			this.tbHost.Name = "tbHost";
			this.tbHost.TextChanged += new System.EventHandler(this.tbHost_TextChanged);
			// 
			// ntbPort
			// 
			this.ntbPort.AllowDecimal = false;
			this.ntbPort.AllowNegative = false;
			this.ntbPort.CultureInfo = null;
			this.ntbPort.DecimalLetters = 2;
			resources.ApplyResources(this.ntbPort, "ntbPort");
			this.ntbPort.ForeColor = System.Drawing.Color.Black;
			this.ntbPort.HasMinValue = false;
			this.ntbPort.MaxValue = 65535D;
			this.ntbPort.MinValue = 0D;
			this.ntbPort.Name = "ntbPort";
			this.ntbPort.Value = 0D;
			this.ntbPort.TextChanged += new System.EventHandler(this.ntbPort_TextChanged);
			// 
			// btnTestConnection
			// 
			resources.ApplyResources(this.btnTestConnection, "btnTestConnection");
			this.btnTestConnection.Name = "btnTestConnection";
			this.btnTestConnection.UseVisualStyleBackColor = true;
			this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
			// 
			// groupBox1
			// 
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.btnEditTransactionProfiles);
			this.groupBox1.Controls.Add(this.cmbTransactionServiceProfile);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.btnTestConnection);
			this.groupBox1.Controls.Add(this.lblAddress);
			this.groupBox1.Controls.Add(this.lblName);
			this.groupBox1.Controls.Add(this.tbHost);
			this.groupBox1.Controls.Add(this.ntbPort);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// btnEditTransactionProfiles
			// 
			this.btnEditTransactionProfiles.BackColor = System.Drawing.Color.Transparent;
			this.btnEditTransactionProfiles.Context = LSOne.Controls.ButtonType.Edit;
			resources.ApplyResources(this.btnEditTransactionProfiles, "btnEditTransactionProfiles");
			this.btnEditTransactionProfiles.Name = "btnEditTransactionProfiles";
			this.btnEditTransactionProfiles.Click += new System.EventHandler(this.btnEditTransactionProfiles_Click);
			// 
			// cmbTransactionServiceProfile
			// 
			this.cmbTransactionServiceProfile.AddList = null;
			this.cmbTransactionServiceProfile.AllowKeyboardSelection = false;
			resources.ApplyResources(this.cmbTransactionServiceProfile, "cmbTransactionServiceProfile");
			this.cmbTransactionServiceProfile.MaxLength = 32767;
			this.cmbTransactionServiceProfile.Name = "cmbTransactionServiceProfile";
			this.cmbTransactionServiceProfile.NoChangeAllowed = false;
			this.cmbTransactionServiceProfile.OnlyDisplayID = false;
			this.cmbTransactionServiceProfile.RemoveList = null;
			this.cmbTransactionServiceProfile.RowHeight = ((short)(22));
			this.cmbTransactionServiceProfile.SecondaryData = null;
			this.cmbTransactionServiceProfile.SelectedData = null;
			this.cmbTransactionServiceProfile.SelectedDataID = null;
			this.cmbTransactionServiceProfile.SelectionList = null;
			this.cmbTransactionServiceProfile.SkipIDColumn = true;
			this.cmbTransactionServiceProfile.RequestData += new System.EventHandler(this.cmbTransactionServiceProfile_RequestData);
			this.cmbTransactionServiceProfile.SelectedDataChanged += new System.EventHandler(this.cmbTransactionServiceProfile_SelectedDataChanged);
			this.cmbTransactionServiceProfile.RequestClear += new System.EventHandler(this.cmbTransactionServiceProfile_RequestClear);
			// 
			// label11
			// 
			this.label11.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label11, "label11");
			this.label11.Name = "label11";
			// 
			// AdministrationStoreServerPage
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.groupBox1);
			this.Name = "AdministrationStoreServerPage";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbHost;
        private NumericTextBox ntbPort;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.GroupBox groupBox1;
        private ContextButton btnEditTransactionProfiles;
        private DualDataComboBox cmbTransactionServiceProfile;
        private System.Windows.Forms.Label label11;
    }
}
