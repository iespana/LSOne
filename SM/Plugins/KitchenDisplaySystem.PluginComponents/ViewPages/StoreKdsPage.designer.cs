namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    partial class StoreKdsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreKdsPage));
            this.btnEditKitchenManagerProfile = new LSOne.Controls.ContextButton();
            this.cmbKitchenManagerProfile = new LSOne.Controls.DualDataComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnAddKitchenManagerProfile = new LSOne.Controls.ContextButton();
            this.SuspendLayout();
            // 
            // btnEditKitchenManagerProfile
            // 
            this.btnEditKitchenManagerProfile.BackColor = System.Drawing.Color.Transparent;
            this.btnEditKitchenManagerProfile.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditKitchenManagerProfile, "btnEditKitchenManagerProfile");
            this.btnEditKitchenManagerProfile.Name = "btnEditKitchenManagerProfile";
            this.btnEditKitchenManagerProfile.Click += new System.EventHandler(this.btnEditKitchenManagerProfile_Click);
            // 
            // cmbKitchenManagerProfile
            // 
            this.cmbKitchenManagerProfile.AddList = null;
            this.cmbKitchenManagerProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbKitchenManagerProfile, "cmbKitchenManagerProfile");
            this.cmbKitchenManagerProfile.MaxLength = 32767;
            this.cmbKitchenManagerProfile.Name = "cmbKitchenManagerProfile";
            this.cmbKitchenManagerProfile.NoChangeAllowed = false;
            this.cmbKitchenManagerProfile.OnlyDisplayID = false;
            this.cmbKitchenManagerProfile.RemoveList = null;
            this.cmbKitchenManagerProfile.RowHeight = ((short)(22));
            this.cmbKitchenManagerProfile.SecondaryData = null;
            this.cmbKitchenManagerProfile.SelectedData = null;
            this.cmbKitchenManagerProfile.SelectedDataID = null;
            this.cmbKitchenManagerProfile.SelectionList = null;
            this.cmbKitchenManagerProfile.SkipIDColumn = true;
            this.cmbKitchenManagerProfile.RequestData += new System.EventHandler(this.cmbKitchenManagerProfile_RequestData);
            this.cmbKitchenManagerProfile.SelectedDataChanged += new System.EventHandler(this.cmbKitchenManagerProfile_SelectedDataChanged);
            this.cmbKitchenManagerProfile.RequestClear += new System.EventHandler(this.cmbKitchenManagerProfile_RequestClear);
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // btnAddKitchenManagerProfile
            // 
            this.btnAddKitchenManagerProfile.BackColor = System.Drawing.Color.Transparent;
            this.btnAddKitchenManagerProfile.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddKitchenManagerProfile, "btnAddKitchenManagerProfile");
            this.btnAddKitchenManagerProfile.Name = "btnAddKitchenManagerProfile";
            this.btnAddKitchenManagerProfile.Click += new System.EventHandler(this.btnAddKitchenManagerProfile_Click);
            // 
            // StoreKdsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnAddKitchenManagerProfile);
            this.Controls.Add(this.btnEditKitchenManagerProfile);
            this.Controls.Add(this.cmbKitchenManagerProfile);
            this.Controls.Add(this.label11);
            this.DoubleBuffered = true;
            this.Name = "StoreKdsPage";
            this.ResumeLayout(false);

        }

        #endregion

        private LSOne.Controls.ContextButton btnEditKitchenManagerProfile;
        private LSOne.Controls.DualDataComboBox cmbKitchenManagerProfile;
        private System.Windows.Forms.Label label11;
        private LSOne.Controls.ContextButton btnAddKitchenManagerProfile;
    }
}
