namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    partial class StoreInventoryPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreInventoryPage));
            this.lblReason = new System.Windows.Forms.Label();
            this.grpStoreTransfers = new System.Windows.Forms.GroupBox();
            this.lblDefaultDelivery = new System.Windows.Forms.Label();
            this.cmbDays = new System.Windows.Forms.ComboBox();
            this.btnEdit = new LSOne.Controls.ContextButton();
            this.ntbDefaultDeliveryTime = new LSOne.Controls.NumericTextBox();
            this.cmbReason = new LSOne.Controls.DualDataComboBox();
            this.grpStoreTransfers.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblReason
            // 
            resources.ApplyResources(this.lblReason, "lblReason");
            this.lblReason.Name = "lblReason";
            // 
            // grpStoreTransfers
            // 
            this.grpStoreTransfers.Controls.Add(this.cmbDays);
            this.grpStoreTransfers.Controls.Add(this.ntbDefaultDeliveryTime);
            this.grpStoreTransfers.Controls.Add(this.lblDefaultDelivery);
            resources.ApplyResources(this.grpStoreTransfers, "grpStoreTransfers");
            this.grpStoreTransfers.Name = "grpStoreTransfers";
            this.grpStoreTransfers.TabStop = false;
            // 
            // lblDefaultDelivery
            // 
            resources.ApplyResources(this.lblDefaultDelivery, "lblDefaultDelivery");
            this.lblDefaultDelivery.Name = "lblDefaultDelivery";
            // 
            // cmbDays
            // 
            this.cmbDays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDays.FormattingEnabled = true;
            this.cmbDays.Items.AddRange(new object[] {
            resources.GetString("cmbDays.Items"),
            resources.GetString("cmbDays.Items1")});
            resources.ApplyResources(this.cmbDays, "cmbDays");
            this.cmbDays.Name = "cmbDays";
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.Transparent;
            this.btnEdit.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // ntbDefaultDeliveryTime
            // 
            this.ntbDefaultDeliveryTime.AllowDecimal = false;
            this.ntbDefaultDeliveryTime.AllowNegative = false;
            this.ntbDefaultDeliveryTime.CultureInfo = null;
            this.ntbDefaultDeliveryTime.DecimalLetters = 2;
            this.ntbDefaultDeliveryTime.ForeColor = System.Drawing.Color.Black;
            this.ntbDefaultDeliveryTime.HasMinValue = true;
            resources.ApplyResources(this.ntbDefaultDeliveryTime, "ntbDefaultDeliveryTime");
            this.ntbDefaultDeliveryTime.MaxValue = 999D;
            this.ntbDefaultDeliveryTime.MinValue = 0D;
            this.ntbDefaultDeliveryTime.Name = "ntbDefaultDeliveryTime";
            this.ntbDefaultDeliveryTime.Value = 0D;
            // 
            // cmbReason
            // 
            this.cmbReason.AddList = null;
            this.cmbReason.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbReason, "cmbReason");
            this.cmbReason.MaxLength = 32767;
            this.cmbReason.Name = "cmbReason";
            this.cmbReason.NoChangeAllowed = false;
            this.cmbReason.OnlyDisplayID = false;
            this.cmbReason.RemoveList = null;
            this.cmbReason.RowHeight = ((short)(22));
            this.cmbReason.SecondaryData = null;
            this.cmbReason.SelectedData = null;
            this.cmbReason.SelectedDataID = null;
            this.cmbReason.SelectionList = null;
            this.cmbReason.SkipIDColumn = true;
            this.cmbReason.RequestData += new System.EventHandler(this.cmbReason_RequestData);
            this.cmbReason.RequestClear += new System.EventHandler(this.cmbReason_RequestClear);
            // 
            // StoreInventoryPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.grpStoreTransfers);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.cmbReason);
            this.Controls.Add(this.lblReason);
            this.DoubleBuffered = true;
            this.Name = "StoreInventoryPage";
            this.grpStoreTransfers.ResumeLayout(false);
            this.grpStoreTransfers.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblReason;
        private Controls.DualDataComboBox cmbReason;
        private Controls.ContextButton btnEdit;
        private System.Windows.Forms.GroupBox grpStoreTransfers;
        private System.Windows.Forms.Label lblDefaultDelivery;
        private Controls.NumericTextBox ntbDefaultDeliveryTime;
        private System.Windows.Forms.ComboBox cmbDays;
    }
}
