namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    partial class TerminalSspSettingPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerminalSspSettingPage));
            this.btnEditStoreSiteServiceProfile = new LSOne.Controls.ContextButton();
            this.cmbStoreSiteServiceProfile = new LSOne.Controls.DualDataComboBox();
            this.lblStoreSiteServiceProfile = new System.Windows.Forms.Label();
            this.btnEditHoSiteServiceProfile = new LSOne.Controls.ContextButton();
            this.cmbHoSiteServiceProfile = new LSOne.Controls.DualDataComboBox();
            this.lblHOSiteServiceProfile = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnEditStoreSiteServiceProfile
            // 
            this.btnEditStoreSiteServiceProfile.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditStoreSiteServiceProfile, "btnEditStoreSiteServiceProfile");
            this.btnEditStoreSiteServiceProfile.Name = "btnEditStoreSiteServiceProfile";
            this.btnEditStoreSiteServiceProfile.Click += new System.EventHandler(this.btnEditStoreSiteServiceProfile_Click);
            // 
            // cmbStoreSiteServiceProfile
            // 
            this.cmbStoreSiteServiceProfile.AddList = null;
            this.cmbStoreSiteServiceProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbStoreSiteServiceProfile, "cmbStoreSiteServiceProfile");
            this.cmbStoreSiteServiceProfile.MaxLength = 32767;
            this.cmbStoreSiteServiceProfile.Name = "cmbStoreSiteServiceProfile";
            this.cmbStoreSiteServiceProfile.OnlyDisplayID = false;
            this.cmbStoreSiteServiceProfile.RemoveList = null;
            this.cmbStoreSiteServiceProfile.RowHeight = ((short)(22));
            this.cmbStoreSiteServiceProfile.SecondaryData = null;
            this.cmbStoreSiteServiceProfile.SelectedData = null;
            this.cmbStoreSiteServiceProfile.SelectedDataID = null;
            this.cmbStoreSiteServiceProfile.SelectionList = null;
            this.cmbStoreSiteServiceProfile.SkipIDColumn = true;
            this.cmbStoreSiteServiceProfile.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbStoreSiteServiceProfile.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // lblStoreSiteServiceProfile
            // 
            this.lblStoreSiteServiceProfile.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblStoreSiteServiceProfile, "lblStoreSiteServiceProfile");
            this.lblStoreSiteServiceProfile.Name = "lblStoreSiteServiceProfile";
            // 
            // btnEditHoSiteServiceProfile
            // 
            this.btnEditHoSiteServiceProfile.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditHoSiteServiceProfile, "btnEditHoSiteServiceProfile");
            this.btnEditHoSiteServiceProfile.Name = "btnEditHoSiteServiceProfile";
            this.btnEditHoSiteServiceProfile.Click += new System.EventHandler(this.btnEditHoSiteServiceProfile_Click);
            // 
            // cmbHoSiteServiceProfile
            // 
            this.cmbHoSiteServiceProfile.AddList = null;
            this.cmbHoSiteServiceProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbHoSiteServiceProfile, "cmbHoSiteServiceProfile");
            this.cmbHoSiteServiceProfile.MaxLength = 32767;
            this.cmbHoSiteServiceProfile.Name = "cmbHoSiteServiceProfile";
            this.cmbHoSiteServiceProfile.OnlyDisplayID = false;
            this.cmbHoSiteServiceProfile.RemoveList = null;
            this.cmbHoSiteServiceProfile.RowHeight = ((short)(22));
            this.cmbHoSiteServiceProfile.SecondaryData = null;
            this.cmbHoSiteServiceProfile.SelectedData = null;
            this.cmbHoSiteServiceProfile.SelectedDataID = null;
            this.cmbHoSiteServiceProfile.SelectionList = null;
            this.cmbHoSiteServiceProfile.SkipIDColumn = true;
            this.cmbHoSiteServiceProfile.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbHoSiteServiceProfile.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // lblHOSiteServiceProfile
            // 
            this.lblHOSiteServiceProfile.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblHOSiteServiceProfile, "lblHOSiteServiceProfile");
            this.lblHOSiteServiceProfile.Name = "lblHOSiteServiceProfile";
            // 
            // TerminalSspSettingPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnEditStoreSiteServiceProfile);
            this.Controls.Add(this.cmbStoreSiteServiceProfile);
            this.Controls.Add(this.lblStoreSiteServiceProfile);
            this.Controls.Add(this.btnEditHoSiteServiceProfile);
            this.Controls.Add(this.cmbHoSiteServiceProfile);
            this.Controls.Add(this.lblHOSiteServiceProfile);
            this.Name = "TerminalSspSettingPage";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ContextButton btnEditStoreSiteServiceProfile;
        private Controls.DualDataComboBox cmbStoreSiteServiceProfile;
        private System.Windows.Forms.Label lblStoreSiteServiceProfile;
        private Controls.ContextButton btnEditHoSiteServiceProfile;
        private Controls.DualDataComboBox cmbHoSiteServiceProfile;
        private System.Windows.Forms.Label lblHOSiteServiceProfile;


    }
}
