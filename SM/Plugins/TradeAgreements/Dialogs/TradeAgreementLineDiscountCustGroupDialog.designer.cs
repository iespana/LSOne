using LSOne.Controls;

namespace LSOne.ViewPlugins.TradeAgreements.Dialogs
{
    partial class TradeAgreementLineDiscountCustGroupDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TradeAgreementLineDiscountCustGroupDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblVariant = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.ntbPercentage2 = new LSOne.Controls.NumericTextBox();
            this.ntbPercentage1 = new LSOne.Controls.NumericTextBox();
            this.ntbDiscount = new LSOne.Controls.NumericTextBox();
            this.ntbQuantity = new LSOne.Controls.NumericTextBox();
            this.cmbUnit = new LSOne.Controls.DualDataComboBox();
            this.cmbVariantNumber = new LSOne.Controls.DualDataComboBox();
            this.cmbCurrency = new LSOne.Controls.DualDataComboBox();
            this.btnAddUnit = new LSOne.Controls.ContextButton();
            this.btnAddCurrency = new LSOne.Controls.ContextButton();
            this.cmbItemCode = new System.Windows.Forms.ComboBox();
            this.cmbItemSelection = new LSOne.Controls.DualDataComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
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
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lblVariant
            // 
            resources.ApplyResources(this.lblVariant, "lblVariant");
            this.lblVariant.Name = "lblVariant";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpFromDate, "dtpFromDate");
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged);
            // 
            // dtpToDate
            // 
            this.dtpToDate.Checked = false;
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpToDate, "dtpToDate");
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.ShowCheckBox = true;
            this.dtpToDate.ValueChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // ntbPercentage2
            // 
            this.ntbPercentage2.AllowDecimal = true;
            this.ntbPercentage2.AllowNegative = false;
            this.ntbPercentage2.CultureInfo = null;
            this.ntbPercentage2.DecimalLetters = 2;
            this.ntbPercentage2.ForeColor = System.Drawing.Color.Black;
            this.ntbPercentage2.HasMinValue = false;
            resources.ApplyResources(this.ntbPercentage2, "ntbPercentage2");
            this.ntbPercentage2.MaxValue = 100D;
            this.ntbPercentage2.MinValue = 0D;
            this.ntbPercentage2.Name = "ntbPercentage2";
            this.ntbPercentage2.Value = 0D;
            this.ntbPercentage2.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // ntbPercentage1
            // 
            this.ntbPercentage1.AllowDecimal = true;
            this.ntbPercentage1.AllowNegative = false;
            this.ntbPercentage1.CultureInfo = null;
            this.ntbPercentage1.DecimalLetters = 2;
            this.ntbPercentage1.ForeColor = System.Drawing.Color.Black;
            this.ntbPercentage1.HasMinValue = false;
            resources.ApplyResources(this.ntbPercentage1, "ntbPercentage1");
            this.ntbPercentage1.MaxValue = 100D;
            this.ntbPercentage1.MinValue = 0D;
            this.ntbPercentage1.Name = "ntbPercentage1";
            this.ntbPercentage1.Value = 0D;
            this.ntbPercentage1.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // ntbDiscount
            // 
            this.ntbDiscount.AllowDecimal = true;
            this.ntbDiscount.AllowNegative = false;
            this.ntbDiscount.CultureInfo = null;
            this.ntbDiscount.DecimalLetters = 2;
            this.ntbDiscount.ForeColor = System.Drawing.Color.Black;
            this.ntbDiscount.HasMinValue = false;
            resources.ApplyResources(this.ntbDiscount, "ntbDiscount");
            this.ntbDiscount.MaxValue = 999999999999D;
            this.ntbDiscount.MinValue = 0D;
            this.ntbDiscount.Name = "ntbDiscount";
            this.ntbDiscount.Value = 0D;
            this.ntbDiscount.TextChanged += new System.EventHandler(this.ntbPrice_TextChanged);
            // 
            // ntbQuantity
            // 
            this.ntbQuantity.AllowDecimal = true;
            this.ntbQuantity.AllowNegative = false;
            this.ntbQuantity.CultureInfo = null;
            this.ntbQuantity.DecimalLetters = 2;
            this.ntbQuantity.ForeColor = System.Drawing.Color.Black;
            this.ntbQuantity.HasMinValue = true;
            resources.ApplyResources(this.ntbQuantity, "ntbQuantity");
            this.ntbQuantity.MaxValue = 999999999999D;
            this.ntbQuantity.MinValue = 0D;
            this.ntbQuantity.Name = "ntbQuantity";
            this.ntbQuantity.Value = 0D;
            this.ntbQuantity.TextChanged += new System.EventHandler(this.ntbQuantity_TextChanged);
            // 
            // cmbUnit
            // 
            this.cmbUnit.AddList = null;
            this.cmbUnit.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbUnit, "cmbUnit");
            this.cmbUnit.MaxLength = 32767;
            this.cmbUnit.Name = "cmbUnit";
            this.cmbUnit.NoChangeAllowed = false;
            this.cmbUnit.OnlyDisplayID = false;
            this.cmbUnit.RemoveList = null;
            this.cmbUnit.RowHeight = ((short)(22));
            this.cmbUnit.SecondaryData = null;
            this.cmbUnit.SelectedData = null;
            this.cmbUnit.SelectedDataID = null;
            this.cmbUnit.SelectionList = null;
            this.cmbUnit.SkipIDColumn = true;
            this.cmbUnit.RequestData += new System.EventHandler(this.cmbUnit_RequestData);
            this.cmbUnit.SelectedDataChanged += new System.EventHandler(this.cmbUnit_SelectedDataChanged);
            // 
            // cmbVariantNumber
            // 
            this.cmbVariantNumber.AddList = null;
            this.cmbVariantNumber.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbVariantNumber, "cmbVariantNumber");
            this.cmbVariantNumber.EnableTextBox = true;
            this.cmbVariantNumber.MaxLength = 32767;
            this.cmbVariantNumber.Name = "cmbVariantNumber";
            this.cmbVariantNumber.NoChangeAllowed = false;
            this.cmbVariantNumber.OnlyDisplayID = false;
            this.cmbVariantNumber.RemoveList = null;
            this.cmbVariantNumber.RowHeight = ((short)(22));
            this.cmbVariantNumber.SecondaryData = null;
            this.cmbVariantNumber.SelectedData = null;
            this.cmbVariantNumber.SelectedDataID = null;
            this.cmbVariantNumber.SelectionList = null;
            this.cmbVariantNumber.ShowDropDownOnTyping = true;
            this.cmbVariantNumber.SkipIDColumn = true;
            this.cmbVariantNumber.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbVariantNumber_DropDown);
            this.cmbVariantNumber.SelectedDataChanged += new System.EventHandler(this.cmbVariantNumber_SelectedDataChanged);
            this.cmbVariantNumber.RequestClear += new System.EventHandler(this.cmbVariantNumber_RequestClear);
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
            this.cmbCurrency.RemoveList = null;
            this.cmbCurrency.RowHeight = ((short)(22));
            this.cmbCurrency.SecondaryData = null;
            this.cmbCurrency.SelectedData = null;
            this.cmbCurrency.SelectedDataID = null;
            this.cmbCurrency.SelectionList = null;
            this.cmbCurrency.SkipIDColumn = true;
            this.cmbCurrency.RequestData += new System.EventHandler(this.cmbCurrency_RequestData);
            this.cmbCurrency.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // btnAddUnit
            // 
            this.btnAddUnit.BackColor = System.Drawing.Color.Transparent;
            this.btnAddUnit.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddUnit, "btnAddUnit");
            this.btnAddUnit.Name = "btnAddUnit";
            this.btnAddUnit.Click += new System.EventHandler(this.btnAddUnit_Click);
            // 
            // btnAddCurrency
            // 
            this.btnAddCurrency.BackColor = System.Drawing.Color.Transparent;
            this.btnAddCurrency.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddCurrency, "btnAddCurrency");
            this.btnAddCurrency.Name = "btnAddCurrency";
            this.btnAddCurrency.Click += new System.EventHandler(this.btnAddCurrency_Click);
            // 
            // cmbItemCode
            // 
            this.cmbItemCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbItemCode.FormattingEnabled = true;
            this.cmbItemCode.Items.AddRange(new object[] {
            resources.GetString("cmbItemCode.Items"),
            resources.GetString("cmbItemCode.Items1"),
            resources.GetString("cmbItemCode.Items2")});
            resources.ApplyResources(this.cmbItemCode, "cmbItemCode");
            this.cmbItemCode.Name = "cmbItemCode";
            this.cmbItemCode.SelectedIndexChanged += new System.EventHandler(this.cmbItemCode_SelectedIndexChanged);
            // 
            // cmbItemSelection
            // 
            this.cmbItemSelection.AddList = null;
            this.cmbItemSelection.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbItemSelection, "cmbItemSelection");
            this.cmbItemSelection.EnableTextBox = true;
            this.cmbItemSelection.MaxLength = 32767;
            this.cmbItemSelection.Name = "cmbItemSelection";
            this.cmbItemSelection.NoChangeAllowed = false;
            this.cmbItemSelection.OnlyDisplayID = false;
            this.cmbItemSelection.RemoveList = null;
            this.cmbItemSelection.RowHeight = ((short)(22));
            this.cmbItemSelection.SecondaryData = null;
            this.cmbItemSelection.SelectedData = null;
            this.cmbItemSelection.SelectedDataID = null;
            this.cmbItemSelection.SelectionList = null;
            this.cmbItemSelection.ShowDropDownOnTyping = true;
            this.cmbItemSelection.SkipIDColumn = true;
            this.cmbItemSelection.RequestData += new System.EventHandler(this.cmbItemSelection_RequestData);
            this.cmbItemSelection.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbItemSelection_DropDown);
            this.cmbItemSelection.SelectedDataChanged += new System.EventHandler(this.cmbItemSelection_SelectedDataChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // TradeAgreementLineDiscountCustGroupDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbItemCode);
            this.Controls.Add(this.cmbItemSelection);
            this.Controls.Add(this.btnAddCurrency);
            this.Controls.Add(this.btnAddUnit);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.ntbPercentage2);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.ntbPercentage1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.ntbDiscount);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.ntbQuantity);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.dtpFromDate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.cmbVariantNumber);
            this.Controls.Add(this.lblVariant);
            this.Controls.Add(this.cmbCurrency);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "TradeAgreementLineDiscountCustGroupDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cmbCurrency, 0);
            this.Controls.SetChildIndex(this.lblVariant, 0);
            this.Controls.SetChildIndex(this.cmbVariantNumber, 0);
            this.Controls.SetChildIndex(this.cmbUnit, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.dtpFromDate, 0);
            this.Controls.SetChildIndex(this.dtpToDate, 0);
            this.Controls.SetChildIndex(this.ntbQuantity, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.ntbDiscount, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.Controls.SetChildIndex(this.ntbPercentage1, 0);
            this.Controls.SetChildIndex(this.label13, 0);
            this.Controls.SetChildIndex(this.ntbPercentage2, 0);
            this.Controls.SetChildIndex(this.label14, 0);
            this.Controls.SetChildIndex(this.btnAddUnit, 0);
            this.Controls.SetChildIndex(this.btnAddCurrency, 0);
            this.Controls.SetChildIndex(this.cmbItemSelection, 0);
            this.Controls.SetChildIndex(this.cmbItemCode, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DualDataComboBox cmbCurrency;
        private System.Windows.Forms.Label label6;
        private DualDataComboBox cmbUnit;
        private DualDataComboBox cmbVariantNumber;
        private System.Windows.Forms.Label lblVariant;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private NumericTextBox ntbDiscount;
        private System.Windows.Forms.Label label9;
        private NumericTextBox ntbQuantity;
        private System.Windows.Forms.Label label13;
        private NumericTextBox ntbPercentage1;
        private NumericTextBox ntbPercentage2;
        private System.Windows.Forms.Label label14;
        private ContextButton btnAddUnit;
        private ContextButton btnAddCurrency;
        private System.Windows.Forms.ComboBox cmbItemCode;
        private DualDataComboBox cmbItemSelection;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}