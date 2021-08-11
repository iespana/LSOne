using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    partial class InventoryTransferSendingDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryTransferSendingDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.rbFromRequst = new System.Windows.Forms.RadioButton();
            this.rbCreateEmptyTransfer = new System.Windows.Forms.RadioButton();
            this.cmbRequest = new LSOne.Controls.DualDataComboBox();
            this.lblRequest = new System.Windows.Forms.Label();
            this.cmbSendingStore = new LSOne.Controls.DualDataComboBox();
            this.lblSendingStore = new System.Windows.Forms.Label();
            this.cmbReceivingStore = new LSOne.Controls.DualDataComboBox();
            this.lblReceivingStore = new System.Windows.Forms.Label();
            this.rbCopyExisting = new System.Windows.Forms.RadioButton();
            this.lblTransferOrder = new System.Windows.Forms.Label();
            this.cmbTransferOrder = new LSOne.Controls.DualDataComboBox();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // rbFromRequst
            // 
            resources.ApplyResources(this.rbFromRequst, "rbFromRequst");
            this.rbFromRequst.Checked = true;
            this.rbFromRequst.Name = "rbFromRequst";
            this.rbFromRequst.TabStop = true;
            this.rbFromRequst.UseVisualStyleBackColor = true;
            this.rbFromRequst.CheckedChanged += new System.EventHandler(this.rbFromRequst_CheckedChanged);
            // 
            // rbCreateEmptyTransfer
            // 
            resources.ApplyResources(this.rbCreateEmptyTransfer, "rbCreateEmptyTransfer");
            this.rbCreateEmptyTransfer.Name = "rbCreateEmptyTransfer";
            this.rbCreateEmptyTransfer.UseVisualStyleBackColor = true;
            this.rbCreateEmptyTransfer.CheckedChanged += new System.EventHandler(this.rbCreateEmptyTransfer_CheckedChanged);
            // 
            // cmbRequest
            // 
            this.cmbRequest.AddList = null;
            this.cmbRequest.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbRequest, "cmbRequest");
            this.cmbRequest.MaxLength = 32767;
            this.cmbRequest.Name = "cmbRequest";
            this.cmbRequest.OnlyDisplayID = false;
            this.cmbRequest.RemoveList = null;
            this.cmbRequest.RowHeight = ((short)(22));
            this.cmbRequest.SecondaryData = null;
            this.cmbRequest.SelectedData = null;
            this.cmbRequest.SelectedDataID = null;
            this.cmbRequest.SelectionList = null;
            this.cmbRequest.SkipIDColumn = false;
            this.cmbRequest.RequestData += new System.EventHandler(this.cmbRequest_RequestData);
            this.cmbRequest.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblRequest
            // 
            this.lblRequest.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblRequest, "lblRequest");
            this.lblRequest.Name = "lblRequest";
            // 
            // cmbSendingStore
            // 
            this.cmbSendingStore.AddList = null;
            this.cmbSendingStore.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbSendingStore, "cmbSendingStore");
            this.cmbSendingStore.MaxLength = 32767;
            this.cmbSendingStore.Name = "cmbSendingStore";
            this.cmbSendingStore.OnlyDisplayID = false;
            this.cmbSendingStore.RemoveList = null;
            this.cmbSendingStore.RowHeight = ((short)(22));
            this.cmbSendingStore.SecondaryData = null;
            this.cmbSendingStore.SelectedData = null;
            this.cmbSendingStore.SelectedDataID = null;
            this.cmbSendingStore.SelectionList = null;
            this.cmbSendingStore.SkipIDColumn = true;
            this.cmbSendingStore.RequestData += new System.EventHandler(this.cmbSendingStore_RequestData);
            this.cmbSendingStore.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblSendingStore
            // 
            this.lblSendingStore.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblSendingStore, "lblSendingStore");
            this.lblSendingStore.Name = "lblSendingStore";
            // 
            // cmbReceivingStore
            // 
            this.cmbReceivingStore.AddList = null;
            this.cmbReceivingStore.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbReceivingStore, "cmbReceivingStore");
            this.cmbReceivingStore.MaxLength = 32767;
            this.cmbReceivingStore.Name = "cmbReceivingStore";
            this.cmbReceivingStore.OnlyDisplayID = false;
            this.cmbReceivingStore.RemoveList = null;
            this.cmbReceivingStore.RowHeight = ((short)(22));
            this.cmbReceivingStore.SecondaryData = null;
            this.cmbReceivingStore.SelectedData = null;
            this.cmbReceivingStore.SelectedDataID = null;
            this.cmbReceivingStore.SelectionList = null;
            this.cmbReceivingStore.SkipIDColumn = true;
            this.cmbReceivingStore.RequestData += new System.EventHandler(this.cmbReceivingStore_RequestData);
            this.cmbReceivingStore.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblReceivingStore
            // 
            this.lblReceivingStore.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblReceivingStore, "lblReceivingStore");
            this.lblReceivingStore.Name = "lblReceivingStore";
            // 
            // rbCopyExisting
            // 
            resources.ApplyResources(this.rbCopyExisting, "rbCopyExisting");
            this.rbCopyExisting.Name = "rbCopyExisting";
            this.rbCopyExisting.TabStop = true;
            this.rbCopyExisting.UseVisualStyleBackColor = true;
            this.rbCopyExisting.CheckedChanged += new System.EventHandler(this.rbCopyExisting_CheckedChanged);
            // 
            // lblTransferOrder
            // 
            resources.ApplyResources(this.lblTransferOrder, "lblTransferOrder");
            this.lblTransferOrder.Name = "lblTransferOrder";
            // 
            // cmbTransferOrder
            // 
            this.cmbTransferOrder.AddList = null;
            this.cmbTransferOrder.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbTransferOrder, "cmbTransferOrder");
            this.cmbTransferOrder.MaxLength = 32767;
            this.cmbTransferOrder.Name = "cmbTransferOrder";
            this.cmbTransferOrder.OnlyDisplayID = false;
            this.cmbTransferOrder.RemoveList = null;
            this.cmbTransferOrder.RowHeight = ((short)(22));
            this.cmbTransferOrder.SecondaryData = null;
            this.cmbTransferOrder.SelectedData = null;
            this.cmbTransferOrder.SelectedDataID = null;
            this.cmbTransferOrder.SelectionList = null;
            this.cmbTransferOrder.SkipIDColumn = false;
            this.cmbTransferOrder.RequestData += new System.EventHandler(this.cmbTransferOrder_RequestData);
            this.cmbTransferOrder.SelectedDataChanged += new System.EventHandler(this.cmbTransferOrder_SelectedDataChanged);
            this.cmbTransferOrder.CheckedChanged += new System.EventHandler(this.cmbTransferOrder_CheckedChanged);
            // 
            // InventoryTransferSendingDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbTransferOrder);
            this.Controls.Add(this.lblTransferOrder);
            this.Controls.Add(this.rbCopyExisting);
            this.Controls.Add(this.cmbReceivingStore);
            this.Controls.Add(this.lblReceivingStore);
            this.Controls.Add(this.cmbSendingStore);
            this.Controls.Add(this.lblSendingStore);
            this.Controls.Add(this.cmbRequest);
            this.Controls.Add(this.lblRequest);
            this.Controls.Add(this.rbCreateEmptyTransfer);
            this.Controls.Add(this.rbFromRequst);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "InventoryTransferSendingDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.rbFromRequst, 0);
            this.Controls.SetChildIndex(this.rbCreateEmptyTransfer, 0);
            this.Controls.SetChildIndex(this.lblRequest, 0);
            this.Controls.SetChildIndex(this.cmbRequest, 0);
            this.Controls.SetChildIndex(this.lblSendingStore, 0);
            this.Controls.SetChildIndex(this.cmbSendingStore, 0);
            this.Controls.SetChildIndex(this.lblReceivingStore, 0);
            this.Controls.SetChildIndex(this.cmbReceivingStore, 0);
            this.Controls.SetChildIndex(this.rbCopyExisting, 0);
            this.Controls.SetChildIndex(this.lblTransferOrder, 0);
            this.Controls.SetChildIndex(this.cmbTransferOrder, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rbFromRequst;
        private System.Windows.Forms.RadioButton rbCreateEmptyTransfer;
        private DualDataComboBox cmbRequest;
        private System.Windows.Forms.Label lblRequest;
        private DualDataComboBox cmbSendingStore;
        private System.Windows.Forms.Label lblSendingStore;
        private DualDataComboBox cmbReceivingStore;
        private System.Windows.Forms.Label lblReceivingStore;
        private System.Windows.Forms.RadioButton rbCopyExisting;
        private System.Windows.Forms.Label lblTransferOrder;
        private DualDataComboBox cmbTransferOrder;
    }
}