using LSOne.Controls;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    partial class MixAndMatchLineDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MixAndMatchLineDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.groupPanel2 = new LSOne.Controls.GroupPanel();
            this.lblDealPriceDiscountPct = new System.Windows.Forms.Label();
            this.ntbDealPriceDiscountPct = new LSOne.Controls.NumericTextBox();
            this.lblDiscountType = new System.Windows.Forms.Label();
            this.cmbDiscountType = new System.Windows.Forms.ComboBox();
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
            this.label1 = new System.Windows.Forms.Label();
            this.cmbLineGroup = new LSOne.Controls.DualDataComboBox();
            this.btnEditLineGroups = new LSOne.Controls.ContextButton();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
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
            this.groupPanel2.Controls.Add(this.lblDealPriceDiscountPct);
            this.groupPanel2.Controls.Add(this.ntbDealPriceDiscountPct);
            this.groupPanel2.Controls.Add(this.lblDiscountType);
            this.groupPanel2.Controls.Add(this.cmbDiscountType);
            this.groupPanel2.Controls.Add(this.ntbStandardPriceWithTax);
            this.groupPanel2.Controls.Add(this.label7);
            this.groupPanel2.Controls.Add(this.label6);
            this.groupPanel2.Controls.Add(this.ntbStandardPrice);
            resources.ApplyResources(this.groupPanel2, "groupPanel2");
            this.groupPanel2.Name = "groupPanel2";
            // 
            // lblDealPriceDiscountPct
            // 
            this.lblDealPriceDiscountPct.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDealPriceDiscountPct, "lblDealPriceDiscountPct");
            this.lblDealPriceDiscountPct.Name = "lblDealPriceDiscountPct";
            // 
            // ntbDealPriceDiscountPct
            // 
            this.ntbDealPriceDiscountPct.AllowDecimal = true;
            this.ntbDealPriceDiscountPct.AllowNegative = false;
            this.ntbDealPriceDiscountPct.BackColor = System.Drawing.SystemColors.Window;
            this.ntbDealPriceDiscountPct.CultureInfo = null;
            this.ntbDealPriceDiscountPct.DecimalLetters = 2;
            resources.ApplyResources(this.ntbDealPriceDiscountPct, "ntbDealPriceDiscountPct");
            this.ntbDealPriceDiscountPct.ForeColor = System.Drawing.Color.Black;
            this.ntbDealPriceDiscountPct.HasMinValue = false;
            this.ntbDealPriceDiscountPct.MaxValue = 0D;
            this.ntbDealPriceDiscountPct.MinValue = 0D;
            this.ntbDealPriceDiscountPct.Name = "ntbDealPriceDiscountPct";
            this.ntbDealPriceDiscountPct.Value = 0D;
            this.ntbDealPriceDiscountPct.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblDiscountType
            // 
            this.lblDiscountType.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDiscountType, "lblDiscountType");
            this.lblDiscountType.Name = "lblDiscountType";
            // 
            // cmbDiscountType
            // 
            this.cmbDiscountType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbDiscountType, "cmbDiscountType");
            this.cmbDiscountType.FormattingEnabled = true;
            this.cmbDiscountType.Items.AddRange(new object[] {
            resources.GetString("cmbDiscountType.Items"),
            resources.GetString("cmbDiscountType.Items1")});
            this.cmbDiscountType.Name = "cmbDiscountType";
            this.cmbDiscountType.SelectedIndexChanged += new System.EventHandler(this.cmbDiscountType_SelectedIndexChanged);
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
            this.ntbStandardPriceWithTax.TextChanged += new System.EventHandler(this.CheckEnabled);
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
            this.ntbStandardPrice.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            resources.GetString("cmbType.Items"),
            resources.GetString("cmbType.Items1"),
            resources.GetString("cmbType.Items2"),
            resources.GetString("cmbType.Items3")});
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.Name = "cmbType";
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // cmbRelation
            // 
            this.cmbRelation.AddList = null;
            this.cmbRelation.AllowKeyboardSelection = false;
            this.cmbRelation.EnableTextBox = true;
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
            this.cmbRelation.ShowDropDownOnTyping = true;
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
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbLineGroup
            // 
            this.cmbLineGroup.AddList = null;
            this.cmbLineGroup.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbLineGroup, "cmbLineGroup");
            this.cmbLineGroup.MaxLength = 32767;
            this.cmbLineGroup.Name = "cmbLineGroup";
            this.cmbLineGroup.NoChangeAllowed = false;
            this.cmbLineGroup.OnlyDisplayID = false;
            this.cmbLineGroup.RemoveList = null;
            this.cmbLineGroup.RowHeight = ((short)(22));
            this.cmbLineGroup.SecondaryData = null;
            this.cmbLineGroup.SelectedData = null;
            this.cmbLineGroup.SelectedDataID = null;
            this.cmbLineGroup.SelectionList = null;
            this.cmbLineGroup.SkipIDColumn = true;
            this.cmbLineGroup.RequestData += new System.EventHandler(this.cmbLineGroup_RequestData);
            this.cmbLineGroup.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // btnEditLineGroups
            // 
            this.btnEditLineGroups.BackColor = System.Drawing.Color.Transparent;
            this.btnEditLineGroups.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditLineGroups, "btnEditLineGroups");
            this.btnEditLineGroups.Name = "btnEditLineGroups";
            this.btnEditLineGroups.Click += new System.EventHandler(this.btnEditLineGroups_Click);
            // 
            // MixAndMatchLineDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnEditLineGroups);
            this.Controls.Add(this.cmbLineGroup);
            this.Controls.Add(this.label1);
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
            this.Name = "MixAndMatchLineDialog";
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.groupPanel2, 0);
            this.Controls.SetChildIndex(this.cmbType, 0);
            this.Controls.SetChildIndex(this.cmbRelation, 0);
            this.Controls.SetChildIndex(this.lblRelation, 0);
            this.Controls.SetChildIndex(this.lblVariantNumber, 0);
            this.Controls.SetChildIndex(this.cmbVariantNumber, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbLineGroup, 0);
            this.Controls.SetChildIndex(this.btnEditLineGroups, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
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
        private NumericTextBox ntbStandardPriceWithTax;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private NumericTextBox ntbStandardPrice;
        private DualDataComboBox cmbVariantNumber;
        private System.Windows.Forms.Label lblVariantNumber;
        private System.Windows.Forms.ErrorProvider errorProvider2;
        private System.Windows.Forms.Label label1;
        private DualDataComboBox cmbLineGroup;
        private LSOne.Controls.ContextButton btnEditLineGroups;
        private System.Windows.Forms.Label lblDealPriceDiscountPct;
        private NumericTextBox ntbDealPriceDiscountPct;
        private System.Windows.Forms.Label lblDiscountType;
        private System.Windows.Forms.ComboBox cmbDiscountType;
    }
}
