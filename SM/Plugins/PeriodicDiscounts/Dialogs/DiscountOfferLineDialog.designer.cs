using LSOne.Controls;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    partial class DiscountOfferLineDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiscountOfferLineDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.groupPanel2 = new LSOne.Controls.GroupPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDiscountAmountWithTax = new System.Windows.Forms.Label();
            this.ntbDiscountAmountWithTax = new LSOne.Controls.NumericTextBox();
            this.lblOfferPriceWithTax = new System.Windows.Forms.Label();
            this.ntbOfferPriceWithTax = new LSOne.Controls.NumericTextBox();
            this.lblOfferPrice = new System.Windows.Forms.Label();
            this.ntbOfferPrice = new LSOne.Controls.NumericTextBox();
            this.lblDiscountAmount = new System.Windows.Forms.Label();
            this.ntbDiscountAmount = new LSOne.Controls.NumericTextBox();
            this.lblDiscountPercent = new System.Windows.Forms.Label();
            this.ntbDiscountPercent = new LSOne.Controls.NumericTextBox();
            this.ntbStandardPriceWithTax = new LSOne.Controls.NumericTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ntbStandardPrice = new LSOne.Controls.NumericTextBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.cmbRelation = new LSOne.Controls.DualDataComboBox();
            this.lblRelation = new System.Windows.Forms.Label();
            this.cmbVariantNumber = new LSOne.Controls.DualDataComboBox();
            this.lblVariantNumber = new System.Windows.Forms.Label();
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider3 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider3)).BeginInit();
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
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.label8.Name = "label8";
            // 
            // groupPanel2
            // 
            this.groupPanel2.Controls.Add(this.label1);
            this.groupPanel2.Controls.Add(this.lblDiscountAmountWithTax);
            this.groupPanel2.Controls.Add(this.ntbDiscountAmountWithTax);
            this.groupPanel2.Controls.Add(this.lblOfferPriceWithTax);
            this.groupPanel2.Controls.Add(this.ntbOfferPriceWithTax);
            this.groupPanel2.Controls.Add(this.lblOfferPrice);
            this.groupPanel2.Controls.Add(this.ntbOfferPrice);
            this.groupPanel2.Controls.Add(this.lblDiscountAmount);
            this.groupPanel2.Controls.Add(this.ntbDiscountAmount);
            this.groupPanel2.Controls.Add(this.lblDiscountPercent);
            this.groupPanel2.Controls.Add(this.ntbDiscountPercent);
            this.groupPanel2.Controls.Add(this.ntbStandardPriceWithTax);
            this.groupPanel2.Controls.Add(this.label7);
            this.groupPanel2.Controls.Add(this.label6);
            this.groupPanel2.Controls.Add(this.ntbStandardPrice);
            resources.ApplyResources(this.groupPanel2, "groupPanel2");
            this.groupPanel2.Name = "groupPanel2";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lblDiscountAmountWithTax
            // 
            this.lblDiscountAmountWithTax.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDiscountAmountWithTax, "lblDiscountAmountWithTax");
            this.lblDiscountAmountWithTax.Name = "lblDiscountAmountWithTax";
            // 
            // ntbDiscountAmountWithTax
            // 
            this.ntbDiscountAmountWithTax.AcceptsTab = true;
            this.ntbDiscountAmountWithTax.AllowDecimal = true;
            this.ntbDiscountAmountWithTax.AllowNegative = false;
            this.ntbDiscountAmountWithTax.BackColor = System.Drawing.SystemColors.Window;
            this.ntbDiscountAmountWithTax.CultureInfo = null;
            this.ntbDiscountAmountWithTax.DecimalLetters = 2;
            this.ntbDiscountAmountWithTax.ForeColor = System.Drawing.Color.Black;
            this.ntbDiscountAmountWithTax.HasMinValue = false;
            resources.ApplyResources(this.ntbDiscountAmountWithTax, "ntbDiscountAmountWithTax");
            this.ntbDiscountAmountWithTax.MaxValue = 0D;
            this.ntbDiscountAmountWithTax.MinValue = 0D;
            this.ntbDiscountAmountWithTax.Name = "ntbDiscountAmountWithTax";
            this.ntbDiscountAmountWithTax.Value = 0D;
            this.ntbDiscountAmountWithTax.Leave += new System.EventHandler(this.ntbDiscountAmountWithTax_Leave);
            // 
            // lblOfferPriceWithTax
            // 
            this.lblOfferPriceWithTax.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblOfferPriceWithTax, "lblOfferPriceWithTax");
            this.lblOfferPriceWithTax.Name = "lblOfferPriceWithTax";
            // 
            // ntbOfferPriceWithTax
            // 
            this.ntbOfferPriceWithTax.AcceptsTab = true;
            this.ntbOfferPriceWithTax.AllowDecimal = true;
            this.ntbOfferPriceWithTax.AllowNegative = false;
            this.ntbOfferPriceWithTax.BackColor = System.Drawing.SystemColors.Window;
            this.ntbOfferPriceWithTax.CultureInfo = null;
            this.ntbOfferPriceWithTax.DecimalLetters = 2;
            this.ntbOfferPriceWithTax.ForeColor = System.Drawing.Color.Black;
            this.ntbOfferPriceWithTax.HasMinValue = false;
            resources.ApplyResources(this.ntbOfferPriceWithTax, "ntbOfferPriceWithTax");
            this.ntbOfferPriceWithTax.MaxValue = 0D;
            this.ntbOfferPriceWithTax.MinValue = 0D;
            this.ntbOfferPriceWithTax.Name = "ntbOfferPriceWithTax";
            this.ntbOfferPriceWithTax.Value = 0D;
            this.ntbOfferPriceWithTax.Leave += new System.EventHandler(this.ntbOfferPriceWithTax_Leave);
            // 
            // lblOfferPrice
            // 
            this.lblOfferPrice.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblOfferPrice, "lblOfferPrice");
            this.lblOfferPrice.Name = "lblOfferPrice";
            // 
            // ntbOfferPrice
            // 
            this.ntbOfferPrice.AcceptsTab = true;
            this.ntbOfferPrice.AllowDecimal = true;
            this.ntbOfferPrice.AllowNegative = false;
            this.ntbOfferPrice.BackColor = System.Drawing.SystemColors.Window;
            this.ntbOfferPrice.CultureInfo = null;
            this.ntbOfferPrice.DecimalLetters = 2;
            this.ntbOfferPrice.ForeColor = System.Drawing.Color.Black;
            this.ntbOfferPrice.HasMinValue = false;
            resources.ApplyResources(this.ntbOfferPrice, "ntbOfferPrice");
            this.ntbOfferPrice.MaxValue = 0D;
            this.ntbOfferPrice.MinValue = 0D;
            this.ntbOfferPrice.Name = "ntbOfferPrice";
            this.ntbOfferPrice.Value = 0D;
            this.ntbOfferPrice.Leave += new System.EventHandler(this.ntbOfferPrice_Leave);
            // 
            // lblDiscountAmount
            // 
            this.lblDiscountAmount.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDiscountAmount, "lblDiscountAmount");
            this.lblDiscountAmount.Name = "lblDiscountAmount";
            // 
            // ntbDiscountAmount
            // 
            this.ntbDiscountAmount.AcceptsTab = true;
            this.ntbDiscountAmount.AllowDecimal = true;
            this.ntbDiscountAmount.AllowNegative = false;
            this.ntbDiscountAmount.BackColor = System.Drawing.SystemColors.Window;
            this.ntbDiscountAmount.CultureInfo = null;
            this.ntbDiscountAmount.DecimalLetters = 2;
            this.ntbDiscountAmount.ForeColor = System.Drawing.Color.Black;
            this.ntbDiscountAmount.HasMinValue = false;
            resources.ApplyResources(this.ntbDiscountAmount, "ntbDiscountAmount");
            this.ntbDiscountAmount.MaxValue = 0D;
            this.ntbDiscountAmount.MinValue = 0D;
            this.ntbDiscountAmount.Name = "ntbDiscountAmount";
            this.ntbDiscountAmount.Value = 0D;
            this.ntbDiscountAmount.Leave += new System.EventHandler(this.ntbDiscountAmount_Leave);
            // 
            // lblDiscountPercent
            // 
            this.lblDiscountPercent.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDiscountPercent, "lblDiscountPercent");
            this.lblDiscountPercent.Name = "lblDiscountPercent";
            // 
            // ntbDiscountPercent
            // 
            this.ntbDiscountPercent.AcceptsTab = true;
            this.ntbDiscountPercent.AllowDecimal = true;
            this.ntbDiscountPercent.AllowNegative = false;
            this.ntbDiscountPercent.BackColor = System.Drawing.SystemColors.Window;
            this.ntbDiscountPercent.CultureInfo = null;
            this.ntbDiscountPercent.DecimalLetters = 2;
            this.ntbDiscountPercent.ForeColor = System.Drawing.Color.Black;
            this.ntbDiscountPercent.HasMinValue = false;
            resources.ApplyResources(this.ntbDiscountPercent, "ntbDiscountPercent");
            this.ntbDiscountPercent.MaxValue = 100D;
            this.ntbDiscountPercent.MinValue = 0D;
            this.ntbDiscountPercent.Name = "ntbDiscountPercent";
            this.ntbDiscountPercent.Value = 0D;
            this.ntbDiscountPercent.Leave += new System.EventHandler(this.ntbDiscountPercent_Leave);
            // 
            // ntbStandardPriceWithTax
            // 
            this.ntbStandardPriceWithTax.AllowDecimal = true;
            this.ntbStandardPriceWithTax.AllowNegative = false;
            this.ntbStandardPriceWithTax.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ntbStandardPriceWithTax.CultureInfo = null;
            this.ntbStandardPriceWithTax.DecimalLetters = 2;
            resources.ApplyResources(this.ntbStandardPriceWithTax, "ntbStandardPriceWithTax");
            this.ntbStandardPriceWithTax.ForeColor = System.Drawing.Color.Black;
            this.ntbStandardPriceWithTax.HasMinValue = false;
            this.ntbStandardPriceWithTax.MaxValue = 0D;
            this.ntbStandardPriceWithTax.MinValue = 0D;
            this.ntbStandardPriceWithTax.Name = "ntbStandardPriceWithTax";
            this.ntbStandardPriceWithTax.ReadOnly = true;
            this.ntbStandardPriceWithTax.TabStop = false;
            this.ntbStandardPriceWithTax.Value = 0D;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // ntbStandardPrice
            // 
            this.ntbStandardPrice.AllowDecimal = true;
            this.ntbStandardPrice.AllowNegative = false;
            this.ntbStandardPrice.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ntbStandardPrice.CultureInfo = null;
            this.ntbStandardPrice.DecimalLetters = 2;
            resources.ApplyResources(this.ntbStandardPrice, "ntbStandardPrice");
            this.ntbStandardPrice.ForeColor = System.Drawing.Color.Black;
            this.ntbStandardPrice.HasMinValue = false;
            this.ntbStandardPrice.MaxValue = 0D;
            this.ntbStandardPrice.MinValue = 0D;
            this.ntbStandardPrice.Name = "ntbStandardPrice";
            this.ntbStandardPrice.ReadOnly = true;
            this.ntbStandardPrice.TabStop = false;
            this.ntbStandardPrice.Value = 0D;
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            resources.GetString("cmbType.Items"),
            resources.GetString("cmbType.Items1"),
            resources.GetString("cmbType.Items2"),
            resources.GetString("cmbType.Items3"),
            resources.GetString("cmbType.Items4")});
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.Name = "cmbType";
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // cmbRelation
            // 
            this.cmbRelation.AddList = null;
            this.cmbRelation.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbRelation, "cmbRelation");
            this.cmbRelation.MaxLength = 32767;
            this.cmbRelation.Name = "cmbRelation";
            this.cmbRelation.NoChangeAllowed = false;
            this.cmbRelation.OnlyDisplayID = false;
            this.cmbRelation.RemoveList = null;
            this.cmbRelation.RowHeight = ((short)(22));
            this.cmbRelation.SecondaryData = null;
            this.cmbRelation.SelectedData = null;
            this.cmbRelation.SelectedDataID = null;
            this.cmbRelation.SelectionList = null;
            this.cmbRelation.SkipIDColumn = true;
            this.cmbRelation.RequestData += new System.EventHandler(this.cmbRelation_RequestData);
            this.cmbRelation.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbRelation_DropDown);
            this.cmbRelation.SelectedDataChanged += new System.EventHandler(this.cmbRelation_SelectedDataChanged);
            // 
            // lblRelation
            // 
            this.lblRelation.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblRelation, "lblRelation");
            this.lblRelation.Name = "lblRelation";
            // 
            // cmbVariantNumber
            // 
            this.cmbVariantNumber.AddList = null;
            this.cmbVariantNumber.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbVariantNumber, "cmbVariantNumber");
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
            this.cmbVariantNumber.SkipIDColumn = true;
            this.cmbVariantNumber.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbVariantNumber_DropDown);
            this.cmbVariantNumber.SelectedDataChanged += new System.EventHandler(this.cmbVariantNumber_SelectedDataChanged);
            this.cmbVariantNumber.RequestClear += new System.EventHandler(this.cmbVariantNumber_RequestClear);
            // 
            // lblVariantNumber
            // 
            this.lblVariantNumber.ForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.lblVariantNumber, "lblVariantNumber");
            this.lblVariantNumber.Name = "lblVariantNumber";
            // 
            // errorProvider2
            // 
            this.errorProvider2.ContainerControl = this;
            // 
            // errorProvider3
            // 
            this.errorProvider3.ContainerControl = this;
            // 
            // DiscountOfferLineDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbVariantNumber);
            this.Controls.Add(this.lblVariantNumber);
            this.Controls.Add(this.lblRelation);
            this.Controls.Add(this.cmbRelation);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "DiscountOfferLineDialog";
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.groupPanel2, 0);
            this.Controls.SetChildIndex(this.cmbType, 0);
            this.Controls.SetChildIndex(this.cmbRelation, 0);
            this.Controls.SetChildIndex(this.lblRelation, 0);
            this.Controls.SetChildIndex(this.lblVariantNumber, 0);
            this.Controls.SetChildIndex(this.cmbVariantNumber, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private GroupPanel groupPanel2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblRelation;
        private DualDataComboBox cmbRelation;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblOfferPriceWithTax;
        private NumericTextBox ntbOfferPriceWithTax;
        private System.Windows.Forms.Label lblOfferPrice;
        private NumericTextBox ntbOfferPrice;
        private System.Windows.Forms.Label lblDiscountAmount;
        private NumericTextBox ntbDiscountAmount;
        private System.Windows.Forms.Label lblDiscountPercent;
        private NumericTextBox ntbDiscountPercent;
        private NumericTextBox ntbStandardPriceWithTax;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private NumericTextBox ntbStandardPrice;
        private DualDataComboBox cmbVariantNumber;
        private System.Windows.Forms.Label lblVariantNumber;
        private System.Windows.Forms.ErrorProvider errorProvider2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDiscountAmountWithTax;
        private NumericTextBox ntbDiscountAmountWithTax;
        private System.Windows.Forms.ErrorProvider errorProvider3;
    }
}