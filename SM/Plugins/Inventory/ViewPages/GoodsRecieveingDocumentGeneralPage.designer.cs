namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    partial class GoodsRecieveingDocumentGeneralPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GoodsRecieveingDocumentGeneralPage));
            this.lblVendorName = new System.Windows.Forms.Label();
            this.tbGoodsReceivingDocumentID = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblGoodsRecievingDocumentID = new System.Windows.Forms.Label();
            this.cmbVendor = new LSOne.Controls.DualDataComboBox();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblVendorName
            // 
            this.lblVendorName.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblVendorName, "lblVendorName");
            this.lblVendorName.Name = "lblVendorName";
            // 
            // tbGoodsReceivingDocumentID
            // 
            resources.ApplyResources(this.tbGoodsReceivingDocumentID, "tbGoodsReceivingDocumentID");
            this.tbGoodsReceivingDocumentID.Name = "tbGoodsReceivingDocumentID";
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblStatus, "lblStatus");
            this.lblStatus.Name = "lblStatus";
            // 
            // lblGoodsRecievingDocumentID
            // 
            this.lblGoodsRecievingDocumentID.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblGoodsRecievingDocumentID, "lblGoodsRecievingDocumentID");
            this.lblGoodsRecievingDocumentID.Name = "lblGoodsRecievingDocumentID";
            // 
            // cmbVendor
            // 
            this.cmbVendor.AddList = null;
            this.cmbVendor.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbVendor, "cmbVendor");
            this.cmbVendor.MaxLength = 32767;
            this.cmbVendor.Name = "cmbVendor";
            this.cmbVendor.NoChangeAllowed = false;
            this.cmbVendor.OnlyDisplayID = false;
            this.cmbVendor.ReceiveKeyboardEvents = true;
            this.cmbVendor.RemoveList = null;
            this.cmbVendor.RowHeight = ((short)(22));
            this.cmbVendor.SecondaryData = null;
            this.cmbVendor.SelectedData = null;
            this.cmbVendor.SelectedDataID = null;
            this.cmbVendor.SelectionList = null;
            this.cmbVendor.SkipIDColumn = false;
            // 
            // tbStatus
            // 
            resources.ApplyResources(this.tbStatus, "tbStatus");
            this.tbStatus.Name = "tbStatus";
            // 
            // GoodsRecieveingDocumentGeneralPage
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tbStatus);
            this.Controls.Add(this.cmbVendor);
            this.Controls.Add(this.lblVendorName);
            this.Controls.Add(this.tbGoodsReceivingDocumentID);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblGoodsRecievingDocumentID);
            this.Name = "GoodsRecieveingDocumentGeneralPage";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private Controls.DualDataComboBox cmbVendor;
        private System.Windows.Forms.Label lblVendorName;
        private System.Windows.Forms.TextBox tbGoodsReceivingDocumentID;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblGoodsRecievingDocumentID;
        private System.Windows.Forms.TextBox tbStatus;
    }
}
