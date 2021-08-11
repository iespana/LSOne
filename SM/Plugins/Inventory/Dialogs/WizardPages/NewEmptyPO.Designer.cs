namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    partial class NewEmptyPO
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewEmptyPO));
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tbPurchaseOrderDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbStore = new LSOne.Controls.DualDataComboBox();
            this.cmbCurrency = new LSOne.Controls.DualDataComboBox();
            this.cmbVendor = new LSOne.Controls.DualDataComboBox();
            this.ntbDiscountAmount = new LSOne.Controls.NumericTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ntbDiscountPercentage = new LSOne.Controls.NumericTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpOrderingDate = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpDeliveryDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // tbPurchaseOrderDescription
            // 
            resources.ApplyResources(this.tbPurchaseOrderDescription, "tbPurchaseOrderDescription");
            this.tbPurchaseOrderDescription.Name = "tbPurchaseOrderDescription";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // cmbCurrency
            // 
            this.cmbCurrency.AddList = null;
            this.cmbCurrency.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbCurrency, "cmbCurrency");
            this.cmbCurrency.MaxLength = 32767;
            this.cmbCurrency.Name = "cmbCurrency";
            this.cmbCurrency.NoChangeAllowed = false;
            this.cmbCurrency.OnlyDisplayID = false;
            this.cmbCurrency.ReceiveKeyboardEvents = true;
            this.cmbCurrency.RemoveList = null;
            this.cmbCurrency.RowHeight = ((short)(22));
            this.cmbCurrency.SecondaryData = null;
            this.cmbCurrency.SelectedData = null;
            this.cmbCurrency.SelectedDataID = null;
            this.cmbCurrency.SelectionList = null;
            this.cmbCurrency.SkipIDColumn = true;
            this.cmbCurrency.RequestData += new System.EventHandler(this.cmbCurrency_RequestData);
            this.cmbCurrency.SelectedDataChanged += new System.EventHandler(this.btnOkEnabled);
            // 
            // cmbVendor
            // 
            this.cmbVendor.AddList = null;
            this.cmbVendor.AllowKeyboardSelection = false;
            this.cmbVendor.EnableTextBox = true;
            resources.ApplyResources(this.cmbVendor, "cmbVendor");
            this.cmbVendor.MaxLength = 32767;
            this.cmbVendor.Name = "cmbVendor";
            this.cmbVendor.NoChangeAllowed = false;
            this.cmbVendor.OnlyDisplayID = false;
            this.cmbVendor.RemoveList = null;
            this.cmbVendor.RowHeight = ((short)(22));
            this.cmbVendor.SecondaryData = null;
            this.cmbVendor.SelectedData = null;
            this.cmbVendor.SelectedDataID = null;
            this.cmbVendor.SelectionList = null;
            this.cmbVendor.ShowDropDownOnTyping = true;
            this.cmbVendor.SkipIDColumn = false;
            this.cmbVendor.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbVendor_DropDown);
            this.cmbVendor.SelectedDataChanged += new System.EventHandler(this.cmbVendor_SelectedDataChanged);
            // 
            // ntbDiscountAmount
            // 
            this.ntbDiscountAmount.AllowDecimal = true;
            this.ntbDiscountAmount.AllowNegative = false;
            this.ntbDiscountAmount.CultureInfo = null;
            this.ntbDiscountAmount.DecimalLetters = 2;
            this.ntbDiscountAmount.DecimalLimit = null;
            this.ntbDiscountAmount.ForeColor = System.Drawing.Color.Black;
            this.ntbDiscountAmount.HasMinValue = false;
            resources.ApplyResources(this.ntbDiscountAmount, "ntbDiscountAmount");
            this.ntbDiscountAmount.MaxValue = 0D;
            this.ntbDiscountAmount.MinValue = 0D;
            this.ntbDiscountAmount.Name = "ntbDiscountAmount";
            this.ntbDiscountAmount.Value = 0D;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // ntbDiscountPercentage
            // 
            this.ntbDiscountPercentage.AllowDecimal = true;
            this.ntbDiscountPercentage.AllowNegative = false;
            this.ntbDiscountPercentage.CultureInfo = null;
            this.ntbDiscountPercentage.DecimalLetters = 2;
            this.ntbDiscountPercentage.DecimalLimit = null;
            this.ntbDiscountPercentage.ForeColor = System.Drawing.Color.Black;
            this.ntbDiscountPercentage.HasMinValue = false;
            resources.ApplyResources(this.ntbDiscountPercentage, "ntbDiscountPercentage");
            this.ntbDiscountPercentage.MaxValue = 100D;
            this.ntbDiscountPercentage.MinValue = 0D;
            this.ntbDiscountPercentage.Name = "ntbDiscountPercentage";
            this.ntbDiscountPercentage.Value = 0D;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // dtpOrderingDate
            // 
            this.dtpOrderingDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpOrderingDate, "dtpOrderingDate");
            this.dtpOrderingDate.Name = "dtpOrderingDate";
            this.dtpOrderingDate.ValueChanged += new System.EventHandler(this.dtpOrderingDate_ValueChanged);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // dtpDeliveryDate
            // 
            this.dtpDeliveryDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpDeliveryDate, "dtpDeliveryDate");
            this.dtpDeliveryDate.Name = "dtpDeliveryDate";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // NewEmptyPO
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ntbDiscountAmount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ntbDiscountPercentage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dtpOrderingDate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dtpDeliveryDate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbStore);
            this.Controls.Add(this.cmbCurrency);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cmbVendor);
            this.Controls.Add(this.tbPurchaseOrderDescription);
            this.Controls.Add(this.label1);
            resources.ApplyResources(this, "$this");
            this.Name = "NewEmptyPO";
            this.Load += new System.EventHandler(this.NewEmptyPO_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private Controls.DualDataComboBox cmbStore;
        private Controls.DualDataComboBox cmbCurrency;
        private System.Windows.Forms.Label label10;
        private Controls.DualDataComboBox cmbVendor;
        private System.Windows.Forms.TextBox tbPurchaseOrderDescription;
        private System.Windows.Forms.Label label1;
        private Controls.NumericTextBox ntbDiscountAmount;
        private System.Windows.Forms.Label label3;
        private Controls.NumericTextBox ntbDiscountPercentage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpOrderingDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpDeliveryDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
    }
}
