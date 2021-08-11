namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    partial class NewStoreTransfer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewStoreTransfer));
            this.cmbReceivingStore = new LSOne.Controls.DualDataComboBox();
            this.lblReceivingStore = new System.Windows.Forms.Label();
            this.cmbSendingStore = new LSOne.Controls.DualDataComboBox();
            this.lblSendingStore = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDelivery = new System.Windows.Forms.Label();
            this.dtExpectedDelivery = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // cmbReceivingStore
            // 
            this.cmbReceivingStore.AddList = null;
            this.cmbReceivingStore.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbReceivingStore, "cmbReceivingStore");
            this.cmbReceivingStore.MaxLength = 32767;
            this.cmbReceivingStore.Name = "cmbReceivingStore";
            this.cmbReceivingStore.NoChangeAllowed = false;
            this.cmbReceivingStore.OnlyDisplayID = false;
            this.cmbReceivingStore.RemoveList = null;
            this.cmbReceivingStore.RowHeight = ((short)(22));
            this.cmbReceivingStore.SecondaryData = null;
            this.cmbReceivingStore.SelectedData = null;
            this.cmbReceivingStore.SelectedDataID = null;
            this.cmbReceivingStore.SelectionList = null;
            this.cmbReceivingStore.SkipIDColumn = true;
            this.cmbReceivingStore.RequestData += new System.EventHandler(this.cmbReceivingStore_RequestData);
            this.cmbReceivingStore.SelectedDataChanged += new System.EventHandler(this.cmbReceivingStore_SelectedDataChanged);
            // 
            // lblReceivingStore
            // 
            this.lblReceivingStore.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblReceivingStore, "lblReceivingStore");
            this.lblReceivingStore.Name = "lblReceivingStore";
            // 
            // cmbSendingStore
            // 
            this.cmbSendingStore.AddList = null;
            this.cmbSendingStore.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbSendingStore, "cmbSendingStore");
            this.cmbSendingStore.MaxLength = 32767;
            this.cmbSendingStore.Name = "cmbSendingStore";
            this.cmbSendingStore.NoChangeAllowed = false;
            this.cmbSendingStore.OnlyDisplayID = false;
            this.cmbSendingStore.RemoveList = null;
            this.cmbSendingStore.RowHeight = ((short)(22));
            this.cmbSendingStore.SecondaryData = null;
            this.cmbSendingStore.SelectedData = null;
            this.cmbSendingStore.SelectedDataID = null;
            this.cmbSendingStore.SelectionList = null;
            this.cmbSendingStore.SkipIDColumn = true;
            this.cmbSendingStore.RequestData += new System.EventHandler(this.cmbSendingStore_RequestData);
            this.cmbSendingStore.SelectedDataChanged += new System.EventHandler(this.cmbSendingStore_SelectedDataChanged);
            // 
            // lblSendingStore
            // 
            this.lblSendingStore.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblSendingStore, "lblSendingStore");
            this.lblSendingStore.Name = "lblSendingStore";
            // 
            // lblDescription
            // 
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.Name = "lblDescription";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            // 
            // lblDelivery
            // 
            this.lblDelivery.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDelivery, "lblDelivery");
            this.lblDelivery.Name = "lblDelivery";
            // 
            // dtExpectedDelivery
            // 
            this.dtExpectedDelivery.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtExpectedDelivery, "dtExpectedDelivery");
            this.dtExpectedDelivery.Name = "dtExpectedDelivery";
            this.dtExpectedDelivery.ValueChanged += new System.EventHandler(this.dtExpectedDelivery_ValueChanged);
            // 
            // NewStoreTransfer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.dtExpectedDelivery);
            this.Controls.Add(this.lblDelivery);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.cmbReceivingStore);
            this.Controls.Add(this.lblReceivingStore);
            this.Controls.Add(this.cmbSendingStore);
            this.Controls.Add(this.lblSendingStore);
            this.Name = "NewStoreTransfer";
            resources.ApplyResources(this, "$this");
            this.Load += new System.EventHandler(this.NewStoreTransfer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.DualDataComboBox cmbReceivingStore;
        private System.Windows.Forms.Label lblReceivingStore;
        private Controls.DualDataComboBox cmbSendingStore;
        private System.Windows.Forms.Label lblSendingStore;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblDelivery;
        private System.Windows.Forms.DateTimePicker dtExpectedDelivery;
    }
}
