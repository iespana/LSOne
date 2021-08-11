using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    partial class InventoryTransferRequestDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryTransferRequestDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmSendingStore = new LSOne.Controls.DualDataComboBox();
            this.cmbReceivingStore = new LSOne.Controls.DualDataComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // cmSendingStore
            // 
            this.cmSendingStore.AddList = null;
            this.cmSendingStore.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmSendingStore, "cmSendingStore");
            this.cmSendingStore.MaxLength = 32767;
            this.cmSendingStore.Name = "cmSendingStore";
            this.cmSendingStore.OnlyDisplayID = false;
            this.cmSendingStore.RemoveList = null;
            this.cmSendingStore.RowHeight = ((short)(22));
            this.cmSendingStore.SecondaryData = null;
            this.cmSendingStore.SelectedData = null;
            this.cmSendingStore.SelectedDataID = null;
            this.cmSendingStore.SelectionList = null;
            this.cmSendingStore.SkipIDColumn = true;
            this.cmSendingStore.RequestData += new System.EventHandler(this.cmbRequstingStore_RequestData);
            this.cmSendingStore.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
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
            this.cmbReceivingStore.RequestData += new System.EventHandler(this.cmbRequestedStore_RequestData);
            this.cmbReceivingStore.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // InventoryTransferRequestDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbReceivingStore);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmSendingStore);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "InventoryTransferRequestDialog";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.cmSendingStore, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cmbReceivingStore, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private DualDataComboBox cmSendingStore;
        private DualDataComboBox cmbReceivingStore;
        private System.Windows.Forms.Label label2;
    }
}