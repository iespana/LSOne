namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    partial class NewEmptyIJ
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewEmptyIJ));
            this.lblStore = new System.Windows.Forms.Label();
            this.cmbStore = new LSOne.Controls.DualDataComboBox();
            this.tbJournalDescription = new System.Windows.Forms.TextBox();
            this.lblNewJournalDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblStore
            // 
            this.lblStore.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblStore, "lblStore");
            this.lblStore.Name = "lblStore";
            // 
            // cmbStore
            // 
            this.cmbStore.AddList = null;
            this.cmbStore.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbStore, "cmbStore");
            this.cmbStore.MaxLength = 32767;
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.NoChangeAllowed = false;
            this.cmbStore.OnlyDisplayID = false;
            this.cmbStore.ReceiveKeyboardEvents = true;
            this.cmbStore.RemoveList = null;
            this.cmbStore.RowHeight = ((short)(22));
            this.cmbStore.SecondaryData = null;
            this.cmbStore.SelectedData = null;
            this.cmbStore.SelectedDataID = null;
            this.cmbStore.SelectionList = null;
            this.cmbStore.SkipIDColumn = false;
            this.cmbStore.RequestData += new System.EventHandler(this.cmbStore_RequestData);
            this.cmbStore.SelectedDataChanged += new System.EventHandler(this.cmbStore_SelectedDataChanged);
            // 
            // tbJournalDescription
            // 
            resources.ApplyResources(this.tbJournalDescription, "tbJournalDescription");
            this.tbJournalDescription.Name = "tbJournalDescription";
            this.tbJournalDescription.TextChanged += new System.EventHandler(this.tbJournalDescription_TextChanged);
            // 
            // lblNewJournalDescription
            // 
            this.lblNewJournalDescription.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblNewJournalDescription, "lblNewJournalDescription");
            this.lblNewJournalDescription.Name = "lblNewJournalDescription";
            // 
            // NewEmptyIJ
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblStore);
            this.Controls.Add(this.cmbStore);
            this.Controls.Add(this.tbJournalDescription);
            this.Controls.Add(this.lblNewJournalDescription);
            this.Name = "NewEmptyIJ";
            this.Load += new System.EventHandler(this.NewEmptyIJ_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStore;
        private Controls.DualDataComboBox cmbStore;
        private System.Windows.Forms.TextBox tbJournalDescription;
        private System.Windows.Forms.Label lblNewJournalDescription;
    }
}
