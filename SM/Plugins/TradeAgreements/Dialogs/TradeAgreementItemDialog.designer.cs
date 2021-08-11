﻿using LSOne.Controls;

namespace LSOne.ViewPlugins.TradeAgreements.Dialogs
{
    partial class TradeAgreementItemDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TradeAgreementItemDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbAccountCode = new System.Windows.Forms.ComboBox();
            this.lblVariant = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblPriceWithTax = new System.Windows.Forms.Label();
            this.lblPriceMiscCharges = new System.Windows.Forms.Label();
            this.ntbMiscCharges = new LSOne.Controls.NumericTextBox();
            this.ntbPriceWithVAT = new LSOne.Controls.NumericTextBox();
            this.ntbPrice = new LSOne.Controls.NumericTextBox();
            this.ntbQuantity = new LSOne.Controls.NumericTextBox();
            this.cmbUnit = new LSOne.Controls.DualDataComboBox();
            this.cmbVariantNumber = new LSOne.Controls.DualDataComboBox();
            this.cmbAccountSelection = new LSOne.Controls.DualDataComboBox();
            this.cmbCurrency = new LSOne.Controls.DualDataComboBox();
            this.btnAddUnit = new LSOne.Controls.ContextButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddCurrency = new LSOne.Controls.ContextButton();
            this.lfPriceLink = new LSOne.Controls.LinkFields();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
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
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
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
            // cmbAccountCode
            // 
            this.cmbAccountCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccountCode.FormattingEnabled = true;
            this.cmbAccountCode.Items.AddRange(new object[] {
            resources.GetString("cmbAccountCode.Items"),
            resources.GetString("cmbAccountCode.Items1"),
            resources.GetString("cmbAccountCode.Items2")});
            resources.ApplyResources(this.cmbAccountCode, "cmbAccountCode");
            this.cmbAccountCode.Name = "cmbAccountCode";
            this.cmbAccountCode.SelectedIndexChanged += new System.EventHandler(this.cmbAccountCode_SelectedIndexChanged);
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
            this.dtpToDate.ValueChanged += new System.EventHandler(this.dtpToDate_ValueChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // lblPrice
            // 
            resources.ApplyResources(this.lblPrice, "lblPrice");
            this.lblPrice.Name = "lblPrice";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // lblPriceWithTax
            // 
            resources.ApplyResources(this.lblPriceWithTax, "lblPriceWithTax");
            this.lblPriceWithTax.Name = "lblPriceWithTax";
            // 
            // lblPriceMiscCharges
            // 
            resources.ApplyResources(this.lblPriceMiscCharges, "lblPriceMiscCharges");
            this.lblPriceMiscCharges.Name = "lblPriceMiscCharges";
            // 
            // ntbMiscCharges
            // 
            this.ntbMiscCharges.AllowDecimal = true;
            this.ntbMiscCharges.AllowNegative = false;
            this.ntbMiscCharges.CultureInfo = null;
            this.ntbMiscCharges.DecimalLetters = 2;
            this.ntbMiscCharges.ForeColor = System.Drawing.Color.Black;
            this.ntbMiscCharges.HasMinValue = false;
            resources.ApplyResources(this.ntbMiscCharges, "ntbMiscCharges");
            this.ntbMiscCharges.MaxValue = 999999999999D;
            this.ntbMiscCharges.MinValue = 0D;
            this.ntbMiscCharges.Name = "ntbMiscCharges";
            this.ntbMiscCharges.Value = 0D;
            this.ntbMiscCharges.TextChanged += new System.EventHandler(this.ntbMiscCharges_TextChanged);
            // 
            // ntbPriceWithVAT
            // 
            this.ntbPriceWithVAT.AllowDecimal = true;
            this.ntbPriceWithVAT.AllowNegative = false;
            this.ntbPriceWithVAT.CultureInfo = null;
            this.ntbPriceWithVAT.DecimalLetters = 2;
            this.ntbPriceWithVAT.ForeColor = System.Drawing.Color.Black;
            this.ntbPriceWithVAT.HasMinValue = false;
            resources.ApplyResources(this.ntbPriceWithVAT, "ntbPriceWithVAT");
            this.ntbPriceWithVAT.MaxValue = 999999999999D;
            this.ntbPriceWithVAT.MinValue = 0D;
            this.ntbPriceWithVAT.Name = "ntbPriceWithVAT";
            this.ntbPriceWithVAT.Value = 0D;
            this.ntbPriceWithVAT.TextChanged += new System.EventHandler(this.ntbPriceWithVAT_TextChanged);
            this.ntbPriceWithVAT.Leave += new System.EventHandler(this.ntbPriceWithVAT_CalculateAndSetPriceWithoutTax);
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
            this.ntbPrice.TextChanged += new System.EventHandler(this.ntbPrice_TextChanged);
            this.ntbPrice.Leave += new System.EventHandler(this.ntbPrice_CalculateAndSetPriceWithTax);
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
            this.cmbVariantNumber.RequestClear += new System.EventHandler(this.cmbVariantNumber_RequestClear);
            // 
            // cmbAccountSelection
            // 
            this.cmbAccountSelection.AddList = null;
            this.cmbAccountSelection.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbAccountSelection, "cmbAccountSelection");
            this.cmbAccountSelection.EnableTextBox = true;
            this.cmbAccountSelection.MaxLength = 32767;
            this.cmbAccountSelection.Name = "cmbAccountSelection";
            this.cmbAccountSelection.NoChangeAllowed = false;
            this.cmbAccountSelection.OnlyDisplayID = false;
            this.cmbAccountSelection.RemoveList = null;
            this.cmbAccountSelection.RowHeight = ((short)(22));
            this.cmbAccountSelection.SecondaryData = null;
            this.cmbAccountSelection.SelectedData = null;
            this.cmbAccountSelection.SelectedDataID = null;
            this.cmbAccountSelection.SelectionList = null;
            this.cmbAccountSelection.ShowDropDownOnTyping = true;
            this.cmbAccountSelection.SkipIDColumn = true;
            this.cmbAccountSelection.RequestData += new System.EventHandler(this.cmbAccountSelection_RequestData);
            this.cmbAccountSelection.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbAccountSelection_DropDown);
            this.cmbAccountSelection.SelectedDataChanged += new System.EventHandler(this.cmbAccountSelection_SelectedDataChanged);
            this.cmbAccountSelection.RequestClear += new System.EventHandler(this.cmbAccountSelection_RequestClear);
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
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label11);
            this.flowLayoutPanel1.Controls.Add(this.dtpToDate);
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // btnAddCurrency
            // 
            this.btnAddCurrency.BackColor = System.Drawing.Color.Transparent;
            this.btnAddCurrency.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddCurrency, "btnAddCurrency");
            this.btnAddCurrency.Name = "btnAddCurrency";
            this.btnAddCurrency.Click += new System.EventHandler(this.btnAddCurrency_Click);
            // 
            // lfPriceLink
            // 
            this.lfPriceLink.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lfPriceLink, "lfPriceLink");
            this.lfPriceLink.Name = "lfPriceLink";
            this.lfPriceLink.TabStop = false;
            // 
            // TradeAgreementItemDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lfPriceLink);
            this.Controls.Add(this.btnAddCurrency);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.btnAddUnit);
            this.Controls.Add(this.lblPriceMiscCharges);
            this.Controls.Add(this.ntbMiscCharges);
            this.Controls.Add(this.lblPriceWithTax);
            this.Controls.Add(this.ntbPriceWithVAT);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.ntbPrice);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.ntbQuantity);
            this.Controls.Add(this.dtpFromDate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.cmbVariantNumber);
            this.Controls.Add(this.lblVariant);
            this.Controls.Add(this.cmbAccountCode);
            this.Controls.Add(this.cmbCurrency);
            this.Controls.Add(this.cmbAccountSelection);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "TradeAgreementItemDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.cmbAccountSelection, 0);
            this.Controls.SetChildIndex(this.cmbCurrency, 0);
            this.Controls.SetChildIndex(this.cmbAccountCode, 0);
            this.Controls.SetChildIndex(this.lblVariant, 0);
            this.Controls.SetChildIndex(this.cmbVariantNumber, 0);
            this.Controls.SetChildIndex(this.cmbUnit, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.dtpFromDate, 0);
            this.Controls.SetChildIndex(this.ntbQuantity, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.ntbPrice, 0);
            this.Controls.SetChildIndex(this.lblPrice, 0);
            this.Controls.SetChildIndex(this.ntbPriceWithVAT, 0);
            this.Controls.SetChildIndex(this.lblPriceWithTax, 0);
            this.Controls.SetChildIndex(this.ntbMiscCharges, 0);
            this.Controls.SetChildIndex(this.lblPriceMiscCharges, 0);
            this.Controls.SetChildIndex(this.btnAddUnit, 0);
            this.Controls.SetChildIndex(this.flowLayoutPanel1, 0);
            this.Controls.SetChildIndex(this.btnAddCurrency, 0);
            this.Controls.SetChildIndex(this.lfPriceLink, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private DualDataComboBox cmbAccountSelection;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DualDataComboBox cmbCurrency;
        private System.Windows.Forms.ComboBox cmbAccountCode;
        private System.Windows.Forms.Label label6;
        private DualDataComboBox cmbUnit;
        private DualDataComboBox cmbVariantNumber;
        private System.Windows.Forms.Label lblVariant;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblPrice;
        private NumericTextBox ntbPrice;
        private System.Windows.Forms.Label label9;
        private NumericTextBox ntbQuantity;
        private System.Windows.Forms.Label lblPriceWithTax;
        private NumericTextBox ntbPriceWithVAT;
        private NumericTextBox ntbMiscCharges;
        private System.Windows.Forms.Label lblPriceMiscCharges;
        private ContextButton btnAddUnit;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private ContextButton btnAddCurrency;
        private LinkFields lfPriceLink;
    }
}