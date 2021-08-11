namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    partial class StoreSspSettingPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreSspSettingPage));
            this.btnEditTransactionProfiles = new LSOne.Controls.ContextButton();
            this.cmbTransactionServiceProfile = new LSOne.Controls.DualDataComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnEditTransactionProfiles
            // 
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
            this.cmbTransactionServiceProfile.RequestClear += new System.EventHandler(this.cmbTransactionServiceProfile_RequestClear);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // StoreSspSettingPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnEditTransactionProfiles);
            this.Controls.Add(this.cmbTransactionServiceProfile);
            this.Controls.Add(this.label9);
            this.Name = "StoreSspSettingPage";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ContextButton btnEditTransactionProfiles;
        private Controls.DualDataComboBox cmbTransactionServiceProfile;
        private System.Windows.Forms.Label label9;

    }
}
