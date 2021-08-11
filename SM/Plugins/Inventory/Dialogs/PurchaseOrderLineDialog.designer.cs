using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    partial class PurchaseOrderLineDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PurchaseOrderLineDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkCreateAnother = new LSOne.Controls.DoubleBufferedCheckbox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tbBarcode = new System.Windows.Forms.TextBox();
            this.cmbItem = new LSOne.Controls.DualDataComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbVariant = new LSOne.Controls.DualDataComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbUnit = new LSOne.Controls.DualDataComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblTaxGroup = new System.Windows.Forms.Label();
            this.cmbTax = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNewItem = new LSOne.Controls.ContextButton();
            this.ntbQuantity = new LSOne.Controls.NumericTextBox();
            this.ntbPrice = new LSOne.Controls.NumericTextBox();
            this.ntbDiscountAmount = new LSOne.Controls.NumericTextBox();
            this.ntbDiscountPercentage = new LSOne.Controls.NumericTextBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.chkCreateAnother);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // chkCreateAnother
            // 
            resources.ApplyResources(this.chkCreateAnother, "chkCreateAnother");
            this.chkCreateAnother.Checked = true;
            this.chkCreateAnother.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCreateAnother.Name = "chkCreateAnother";
            this.chkCreateAnother.UseVisualStyleBackColor = true;
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
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // tbBarcode
            // 
            resources.ApplyResources(this.tbBarcode, "tbBarcode");
            this.tbBarcode.Name = "tbBarcode";
            this.tbBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbBarcode_KeyDown);
            this.tbBarcode.Leave += new System.EventHandler(this.tbBarcode_Leave);
            // 
            // cmbItem
            // 
            this.cmbItem.AddList = null;
            this.cmbItem.AllowKeyboardSelection = false;
            this.cmbItem.EnableTextBox = true;
            resources.ApplyResources(this.cmbItem, "cmbItem");
            this.cmbItem.MaxLength = 32767;
            this.cmbItem.Name = "cmbItem";
            this.cmbItem.NoChangeAllowed = false;
            this.cmbItem.OnlyDisplayID = false;
            this.cmbItem.RemoveList = null;
            this.cmbItem.RowHeight = ((short)(22));
            this.cmbItem.SecondaryData = null;
            this.cmbItem.SelectedData = null;
            this.cmbItem.SelectedDataID = null;
            this.cmbItem.SelectionList = null;
            this.cmbItem.ShowDropDownOnTyping = true;
            this.cmbItem.SkipIDColumn = false;
            this.cmbItem.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbItem_DropDown);
            this.cmbItem.SelectedDataChanged += new System.EventHandler(this.cmbItem_SelectedDataChanged);
            this.cmbItem.RequestClear += new System.EventHandler(this.cmbItem_RequestClear);
            this.cmbItem.Leave += new System.EventHandler(this.cmbItem_Leave);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbVariant
            // 
            this.cmbVariant.AddList = null;
            this.cmbVariant.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbVariant, "cmbVariant");
            this.cmbVariant.EnableTextBox = true;
            this.cmbVariant.MaxLength = 32767;
            this.cmbVariant.Name = "cmbVariant";
            this.cmbVariant.NoChangeAllowed = false;
            this.cmbVariant.OnlyDisplayID = false;
            this.cmbVariant.ReceiveKeyboardEvents = true;
            this.cmbVariant.RemoveList = null;
            this.cmbVariant.RowHeight = ((short)(22));
            this.cmbVariant.SecondaryData = null;
            this.cmbVariant.SelectedData = null;
            this.cmbVariant.SelectedDataID = null;
            this.cmbVariant.SelectionList = null;
            this.cmbVariant.ShowDropDownOnTyping = true;
            this.cmbVariant.SkipIDColumn = true;
            this.cmbVariant.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbVariant_DropDown);
            this.cmbVariant.SelectedDataChanged += new System.EventHandler(this.cmbVariant_SelectedDataChanged);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbUnit
            // 
            this.cmbUnit.AddList = null;
            this.cmbUnit.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbUnit, "cmbUnit");
            this.cmbUnit.EnableTextBox = true;
            this.cmbUnit.MaxLength = 32767;
            this.cmbUnit.Name = "cmbUnit";
            this.cmbUnit.NoChangeAllowed = false;
            this.cmbUnit.OnlyDisplayID = false;
            this.cmbUnit.ReceiveKeyboardEvents = true;
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
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // lblTaxGroup
            // 
            this.lblTaxGroup.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblTaxGroup, "lblTaxGroup");
            this.lblTaxGroup.Name = "lblTaxGroup";
            // 
            // cmbTax
            // 
            this.cmbTax.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTax.FormattingEnabled = true;
            this.cmbTax.Items.AddRange(new object[] {
            resources.GetString("cmbTax.Items"),
            resources.GetString("cmbTax.Items1"),
            resources.GetString("cmbTax.Items2")});
            resources.ApplyResources(this.cmbTax, "cmbTax");
            this.cmbTax.Name = "cmbTax";
            this.cmbTax.SelectedValueChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnNewItem
            // 
            this.btnNewItem.BackColor = System.Drawing.Color.Transparent;
            this.btnNewItem.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnNewItem, "btnNewItem");
            this.btnNewItem.Name = "btnNewItem";
            this.btnNewItem.Click += new System.EventHandler(this.btnNewItem_Click);
            // 
            // ntbQuantity
            // 
            this.ntbQuantity.AllowDecimal = true;
            this.ntbQuantity.AllowNegative = true;
            this.ntbQuantity.CultureInfo = null;
            this.ntbQuantity.DecimalLetters = 2;
            this.ntbQuantity.ForeColor = System.Drawing.Color.Black;
            this.ntbQuantity.HasMinValue = false;
            resources.ApplyResources(this.ntbQuantity, "ntbQuantity");
            this.ntbQuantity.MaxValue = 999999999999D;
            this.ntbQuantity.MinValue = 0D;
            this.ntbQuantity.Name = "ntbQuantity";
            this.ntbQuantity.Value = 0D;
            this.ntbQuantity.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // ntbPrice
            // 
            this.ntbPrice.AllowDecimal = true;
            this.ntbPrice.AllowNegative = false;
            this.ntbPrice.CultureInfo = null;
            this.ntbPrice.DecimalLetters = 2;
            this.ntbPrice.ForeColor = System.Drawing.Color.Black;
            this.ntbPrice.HasMinValue = false;
            resources.ApplyResources(this.ntbPrice, "ntbPrice");
            this.ntbPrice.MaxValue = 999999999999D;
            this.ntbPrice.MinValue = 0D;
            this.ntbPrice.Name = "ntbPrice";
            this.ntbPrice.Value = 0D;
            this.ntbPrice.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // ntbDiscountAmount
            // 
            this.ntbDiscountAmount.AllowDecimal = true;
            this.ntbDiscountAmount.AllowNegative = false;
            this.ntbDiscountAmount.CultureInfo = null;
            this.ntbDiscountAmount.DecimalLetters = 2;
            this.ntbDiscountAmount.ForeColor = System.Drawing.Color.Black;
            this.ntbDiscountAmount.HasMinValue = false;
            resources.ApplyResources(this.ntbDiscountAmount, "ntbDiscountAmount");
            this.ntbDiscountAmount.MaxValue = 999999999999D;
            this.ntbDiscountAmount.MinValue = 0D;
            this.ntbDiscountAmount.Name = "ntbDiscountAmount";
            this.ntbDiscountAmount.Value = 0D;
            this.ntbDiscountAmount.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // ntbDiscountPercentage
            // 
            this.ntbDiscountPercentage.AllowDecimal = true;
            this.ntbDiscountPercentage.AllowNegative = false;
            this.ntbDiscountPercentage.CultureInfo = null;
            this.ntbDiscountPercentage.DecimalLetters = 2;
            this.ntbDiscountPercentage.ForeColor = System.Drawing.Color.Black;
            this.ntbDiscountPercentage.HasMinValue = false;
            resources.ApplyResources(this.ntbDiscountPercentage, "ntbDiscountPercentage");
            this.ntbDiscountPercentage.MaxValue = 100D;
            this.ntbDiscountPercentage.MinValue = 0D;
            this.ntbDiscountPercentage.Name = "ntbDiscountPercentage";
            this.ntbDiscountPercentage.Value = 0D;
            this.ntbDiscountPercentage.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // PurchaseOrderLineDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnNewItem);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbTax);
            this.Controls.Add(this.lblTaxGroup);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ntbDiscountPercentage);
            this.Controls.Add(this.ntbDiscountAmount);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.ntbPrice);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ntbQuantity);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbVariant);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbItem);
            this.Controls.Add(this.tbBarcode);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.KeyPreview = true;
            this.Name = "PurchaseOrderLineDialog";
            this.Shown += new System.EventHandler(this.PurchaseOrderLineDialog_Shown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PurchaseOrderLineDialog_KeyUp);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.tbBarcode, 0);
            this.Controls.SetChildIndex(this.cmbItem, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.cmbVariant, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.cmbUnit, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.ntbQuantity, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.ntbPrice, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.ntbDiscountAmount, 0);
            this.Controls.SetChildIndex(this.ntbDiscountPercentage, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.lblTaxGroup, 0);
            this.Controls.SetChildIndex(this.cmbTax, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.btnNewItem, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox tbBarcode;
        private System.Windows.Forms.Label label3;
        private DualDataComboBox cmbItem;
        private DualDataComboBox cmbVariant;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private DualDataComboBox cmbUnit;
        private System.Windows.Forms.Label label6;
        private NumericTextBox ntbQuantity;
        private System.Windows.Forms.Label label7;
        private NumericTextBox ntbPrice;
        private NumericTextBox ntbDiscountAmount;
        private NumericTextBox ntbDiscountPercentage;
        private System.Windows.Forms.Label lblTaxGroup;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbTax;
        private DoubleBufferedCheckbox chkCreateAnother;
        private System.Windows.Forms.Label label1;
        private ContextButton btnNewItem;
    }
}