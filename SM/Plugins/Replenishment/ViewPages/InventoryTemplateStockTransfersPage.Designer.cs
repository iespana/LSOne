namespace LSOne.ViewPlugins.Replenishment.ViewPages
{
    partial class InventoryTemplateStockTransfersPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryTemplateStockTransfersPage));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbDefaultReceivingStore = new LSOne.Controls.DualDataComboBox();
            this.chkAutoPopulateReceiving = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbDefaultReceivingStore
            // 
            this.cmbDefaultReceivingStore.AddList = null;
            this.cmbDefaultReceivingStore.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbDefaultReceivingStore, "cmbDefaultReceivingStore");
            this.cmbDefaultReceivingStore.MaxLength = 32767;
            this.cmbDefaultReceivingStore.Name = "cmbDefaultReceivingStore";
            this.cmbDefaultReceivingStore.NoChangeAllowed = false;
            this.cmbDefaultReceivingStore.OnlyDisplayID = false;
            this.cmbDefaultReceivingStore.RemoveList = null;
            this.cmbDefaultReceivingStore.RowHeight = ((short)(22));
            this.cmbDefaultReceivingStore.SecondaryData = null;
            this.cmbDefaultReceivingStore.SelectedData = null;
            this.cmbDefaultReceivingStore.SelectedDataID = null;
            this.cmbDefaultReceivingStore.SelectionList = null;
            this.cmbDefaultReceivingStore.SkipIDColumn = true;
            this.cmbDefaultReceivingStore.RequestData += new System.EventHandler(this.cmbDefaultReceivingStore_RequestData);
            this.cmbDefaultReceivingStore.RequestClear += new System.EventHandler(this.cmbDefaultReceivingStore_RequestClear);
            // 
            // chkAutoPopulateReceiving
            // 
            resources.ApplyResources(this.chkAutoPopulateReceiving, "chkAutoPopulateReceiving");
            this.chkAutoPopulateReceiving.Name = "chkAutoPopulateReceiving";
            this.chkAutoPopulateReceiving.UseVisualStyleBackColor = true;
            // 
            // InventoryTemplateStockTransfersPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkAutoPopulateReceiving);
            this.Controls.Add(this.cmbDefaultReceivingStore);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Name = "InventoryTemplateStockTransfersPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Controls.DualDataComboBox cmbDefaultReceivingStore;
        private System.Windows.Forms.CheckBox chkAutoPopulateReceiving;
    }
}
