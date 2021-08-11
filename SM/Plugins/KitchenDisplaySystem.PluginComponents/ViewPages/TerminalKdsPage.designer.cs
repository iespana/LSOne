namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    partial class TerminalKdsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerminalKdsPage));
            this.btnEditKitchenManagerProfile = new LSOne.Controls.ContextButton();
            this.cmbKitchenDisplayProfile = new LSOne.Controls.DualDataComboBox();
            this.lblKitchenManagerProfile = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnEditKitchenManagerProfile
            // 
            this.btnEditKitchenManagerProfile.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditKitchenManagerProfile, "btnEditKitchenManagerProfile");
            this.btnEditKitchenManagerProfile.Name = "btnEditKitchenManagerProfile";
            this.btnEditKitchenManagerProfile.Click += new System.EventHandler(this.btnEditKitchenManagerProfile_Click);
            // 
            // cmbKitchenDisplayProfile
            // 
            this.cmbKitchenDisplayProfile.AddList = null;
            this.cmbKitchenDisplayProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbKitchenDisplayProfile, "cmbKitchenDisplayProfile");
            this.cmbKitchenDisplayProfile.MaxLength = 32767;
            this.cmbKitchenDisplayProfile.Name = "cmbKitchenDisplayProfile";
            this.cmbKitchenDisplayProfile.OnlyDisplayID = false;
            this.cmbKitchenDisplayProfile.RemoveList = null;
            this.cmbKitchenDisplayProfile.RowHeight = ((short)(22));
            this.cmbKitchenDisplayProfile.SecondaryData = null;
            this.cmbKitchenDisplayProfile.SelectedData = null;
            this.cmbKitchenDisplayProfile.SelectedDataID = null;
            this.cmbKitchenDisplayProfile.SelectionList = null;
            this.cmbKitchenDisplayProfile.SkipIDColumn = true;
            this.cmbKitchenDisplayProfile.RequestData += new System.EventHandler(this.cmbKitchenDisplayProfile_RequestData);
            this.cmbKitchenDisplayProfile.SelectedDataChanged += new System.EventHandler(this.cmbKitchenDisplayProfile_SelectedDataChanged);
            this.cmbKitchenDisplayProfile.RequestClear += new System.EventHandler(this.cmbKitchenDisplayProfile_RequestClear);
            // 
            // lblKitchenManagerProfile
            // 
            this.lblKitchenManagerProfile.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblKitchenManagerProfile, "lblKitchenManagerProfile");
            this.lblKitchenManagerProfile.Name = "lblKitchenManagerProfile";
            // 
            // TerminalKdsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnEditKitchenManagerProfile);
            this.Controls.Add(this.cmbKitchenDisplayProfile);
            this.Controls.Add(this.lblKitchenManagerProfile);
            this.DoubleBuffered = true;
            this.Name = "TerminalKdsPage";
            this.ResumeLayout(false);

        }

        #endregion

        private LSOne.Controls.ContextButton btnEditKitchenManagerProfile;
        private LSOne.Controls.DualDataComboBox cmbKitchenDisplayProfile;
        private System.Windows.Forms.Label lblKitchenManagerProfile;




    }
}
