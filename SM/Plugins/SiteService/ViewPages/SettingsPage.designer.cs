using LSOne.Controls;

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    partial class SettingsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsPage));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ntbMaximumGiftCardAmount = new LSOne.Controls.NumericTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbRefillableGiftcard = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkUseGiftcard = new System.Windows.Forms.CheckBox();
            this.lblUseGiftCard = new System.Windows.Forms.Label();
            this.lblGiftCardOption = new System.Windows.Forms.Label();
            this.cmbGiftCardOption = new System.Windows.Forms.ComboBox();
            this.gbCreditMemo = new System.Windows.Forms.GroupBox();
            this.chkUseCreditVouchers = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gbCentralSuspension = new System.Windows.Forms.GroupBox();
            this.chkUserConfirmation = new System.Windows.Forms.CheckBox();
            this.chkUseCentralSuspension = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUserConfirmation = new System.Windows.Forms.Label();
            this.gbCustomer = new System.Windows.Forms.GroupBox();
            this.cmbSalesTaxGroup = new LSOne.Controls.DualDataComboBox();
            this.btnEditSalesTaxGroup = new LSOne.Controls.ContextButton();
            this.chkCentralizedCustomers = new System.Windows.Forms.CheckBox();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.lblDefaultSalesTaxGroup = new System.Windows.Forms.Label();
            this.lblCashCustomer = new System.Windows.Forms.Label();
            this.cmbCashCustomer = new System.Windows.Forms.ComboBox();
            this.gbInventory = new System.Windows.Forms.GroupBox();
            this.chkUseCentralizedInventoryLookup = new System.Windows.Forms.CheckBox();
            this.lblInventoryLookup = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.gbCreditMemo.SuspendLayout();
            this.gbCentralSuspension.SuspendLayout();
            this.gbCustomer.SuspendLayout();
            this.gbInventory.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ntbMaximumGiftCardAmount);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmbRefillableGiftcard);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.chkUseGiftcard);
            this.groupBox1.Controls.Add(this.lblUseGiftCard);
            this.groupBox1.Controls.Add(this.lblGiftCardOption);
            this.groupBox1.Controls.Add(this.cmbGiftCardOption);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // ntbMaximumGiftCardAmount
            // 
            this.ntbMaximumGiftCardAmount.AllowDecimal = false;
            this.ntbMaximumGiftCardAmount.AllowNegative = false;
            this.ntbMaximumGiftCardAmount.CultureInfo = null;
            this.ntbMaximumGiftCardAmount.DecimalLetters = 2;
            this.ntbMaximumGiftCardAmount.ForeColor = System.Drawing.Color.Black;
            this.ntbMaximumGiftCardAmount.HasMinValue = false;
            resources.ApplyResources(this.ntbMaximumGiftCardAmount, "ntbMaximumGiftCardAmount");
            this.ntbMaximumGiftCardAmount.MaxValue = 0D;
            this.ntbMaximumGiftCardAmount.MinValue = 0D;
            this.ntbMaximumGiftCardAmount.Name = "ntbMaximumGiftCardAmount";
            this.ntbMaximumGiftCardAmount.Value = 0D;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbRefillableGiftcard
            // 
            this.cmbRefillableGiftcard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRefillableGiftcard.FormattingEnabled = true;
            this.cmbRefillableGiftcard.Items.AddRange(new object[] {
            resources.GetString("cmbRefillableGiftcard.Items"),
            resources.GetString("cmbRefillableGiftcard.Items1")});
            resources.ApplyResources(this.cmbRefillableGiftcard, "cmbRefillableGiftcard");
            this.cmbRefillableGiftcard.Name = "cmbRefillableGiftcard";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // chkUseGiftcard
            // 
            resources.ApplyResources(this.chkUseGiftcard, "chkUseGiftcard");
            this.chkUseGiftcard.Name = "chkUseGiftcard";
            this.chkUseGiftcard.UseVisualStyleBackColor = true;
            this.chkUseGiftcard.CheckedChanged += new System.EventHandler(this.chkUseGiftcard_CheckedChanged);
            // 
            // lblUseGiftCard
            // 
            resources.ApplyResources(this.lblUseGiftCard, "lblUseGiftCard");
            this.lblUseGiftCard.Name = "lblUseGiftCard";
            // 
            // lblGiftCardOption
            // 
            resources.ApplyResources(this.lblGiftCardOption, "lblGiftCardOption");
            this.lblGiftCardOption.Name = "lblGiftCardOption";
            // 
            // cmbGiftCardOption
            // 
            this.cmbGiftCardOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGiftCardOption.FormattingEnabled = true;
            this.cmbGiftCardOption.Items.AddRange(new object[] {
            resources.GetString("cmbGiftCardOption.Items"),
            resources.GetString("cmbGiftCardOption.Items1"),
            resources.GetString("cmbGiftCardOption.Items2")});
            resources.ApplyResources(this.cmbGiftCardOption, "cmbGiftCardOption");
            this.cmbGiftCardOption.Name = "cmbGiftCardOption";
            // 
            // gbCreditMemo
            // 
            this.gbCreditMemo.Controls.Add(this.chkUseCreditVouchers);
            this.gbCreditMemo.Controls.Add(this.label2);
            resources.ApplyResources(this.gbCreditMemo, "gbCreditMemo");
            this.gbCreditMemo.Name = "gbCreditMemo";
            this.gbCreditMemo.TabStop = false;
            // 
            // chkUseCreditVouchers
            // 
            resources.ApplyResources(this.chkUseCreditVouchers, "chkUseCreditVouchers");
            this.chkUseCreditVouchers.Name = "chkUseCreditVouchers";
            this.chkUseCreditVouchers.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // gbCentralSuspension
            // 
            this.gbCentralSuspension.Controls.Add(this.chkUserConfirmation);
            this.gbCentralSuspension.Controls.Add(this.chkUseCentralSuspension);
            this.gbCentralSuspension.Controls.Add(this.label1);
            this.gbCentralSuspension.Controls.Add(this.lblUserConfirmation);
            resources.ApplyResources(this.gbCentralSuspension, "gbCentralSuspension");
            this.gbCentralSuspension.Name = "gbCentralSuspension";
            this.gbCentralSuspension.TabStop = false;
            // 
            // chkUserConfirmation
            // 
            this.chkUserConfirmation.Checked = true;
            this.chkUserConfirmation.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.chkUserConfirmation, "chkUserConfirmation");
            this.chkUserConfirmation.Name = "chkUserConfirmation";
            this.chkUserConfirmation.UseVisualStyleBackColor = true;
            // 
            // chkUseCentralSuspension
            // 
            resources.ApplyResources(this.chkUseCentralSuspension, "chkUseCentralSuspension");
            this.chkUseCentralSuspension.Name = "chkUseCentralSuspension";
            this.chkUseCentralSuspension.UseVisualStyleBackColor = true;
            this.chkUseCentralSuspension.CheckedChanged += new System.EventHandler(this.chkUseCentralSuspension_CheckedChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lblUserConfirmation
            // 
            resources.ApplyResources(this.lblUserConfirmation, "lblUserConfirmation");
            this.lblUserConfirmation.Name = "lblUserConfirmation";
            // 
            // gbCustomer
            // 
            this.gbCustomer.Controls.Add(this.cmbSalesTaxGroup);
            this.gbCustomer.Controls.Add(this.btnEditSalesTaxGroup);
            this.gbCustomer.Controls.Add(this.chkCentralizedCustomers);
            this.gbCustomer.Controls.Add(this.lblCustomer);
            this.gbCustomer.Controls.Add(this.lblDefaultSalesTaxGroup);
            this.gbCustomer.Controls.Add(this.lblCashCustomer);
            this.gbCustomer.Controls.Add(this.cmbCashCustomer);
            resources.ApplyResources(this.gbCustomer, "gbCustomer");
            this.gbCustomer.Name = "gbCustomer";
            this.gbCustomer.TabStop = false;
            // 
            // cmbSalesTaxGroup
            // 
            this.cmbSalesTaxGroup.AddList = null;
            this.cmbSalesTaxGroup.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbSalesTaxGroup, "cmbSalesTaxGroup");
            this.cmbSalesTaxGroup.MaxLength = 32767;
            this.cmbSalesTaxGroup.Name = "cmbSalesTaxGroup";
            this.cmbSalesTaxGroup.NoChangeAllowed = false;
            this.cmbSalesTaxGroup.OnlyDisplayID = false;
            this.cmbSalesTaxGroup.RemoveList = null;
            this.cmbSalesTaxGroup.RowHeight = ((short)(22));
            this.cmbSalesTaxGroup.SecondaryData = null;
            this.cmbSalesTaxGroup.SelectedData = null;
            this.cmbSalesTaxGroup.SelectedDataID = null;
            this.cmbSalesTaxGroup.SelectionList = null;
            this.cmbSalesTaxGroup.SkipIDColumn = true;
            this.cmbSalesTaxGroup.RequestData += new System.EventHandler(this.cmbSalesTaxGroup_RequestData);
            this.cmbSalesTaxGroup.RequestClear += new System.EventHandler(this.DualDataComboBox_RequestClear);
            // 
            // btnEditSalesTaxGroup
            // 
            this.btnEditSalesTaxGroup.BackColor = System.Drawing.Color.Transparent;
            this.btnEditSalesTaxGroup.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditSalesTaxGroup, "btnEditSalesTaxGroup");
            this.btnEditSalesTaxGroup.Name = "btnEditSalesTaxGroup";
            this.btnEditSalesTaxGroup.Click += new System.EventHandler(this.btnEditSalesTaxGroup_Click);
            // 
            // chkCentralizedCustomers
            // 
            resources.ApplyResources(this.chkCentralizedCustomers, "chkCentralizedCustomers");
            this.chkCentralizedCustomers.Name = "chkCentralizedCustomers";
            this.chkCentralizedCustomers.UseVisualStyleBackColor = true;
            this.chkCentralizedCustomers.CheckedChanged += new System.EventHandler(this.chkCentralizedCustomers_CheckedChanged);
            // 
            // lblCustomer
            // 
            resources.ApplyResources(this.lblCustomer, "lblCustomer");
            this.lblCustomer.Name = "lblCustomer";
            // 
            // lblDefaultSalesTaxGroup
            // 
            resources.ApplyResources(this.lblDefaultSalesTaxGroup, "lblDefaultSalesTaxGroup");
            this.lblDefaultSalesTaxGroup.Name = "lblDefaultSalesTaxGroup";
            // 
            // lblCashCustomer
            // 
            resources.ApplyResources(this.lblCashCustomer, "lblCashCustomer");
            this.lblCashCustomer.Name = "lblCashCustomer";
            // 
            // cmbCashCustomer
            // 
            this.cmbCashCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCashCustomer.FormattingEnabled = true;
            this.cmbCashCustomer.Items.AddRange(new object[] {
            resources.GetString("cmbCashCustomer.Items"),
            resources.GetString("cmbCashCustomer.Items1"),
            resources.GetString("cmbCashCustomer.Items2"),
            resources.GetString("cmbCashCustomer.Items3")});
            resources.ApplyResources(this.cmbCashCustomer, "cmbCashCustomer");
            this.cmbCashCustomer.Name = "cmbCashCustomer";
            // 
            // gbInventory
            // 
            this.gbInventory.Controls.Add(this.chkUseCentralizedInventoryLookup);
            this.gbInventory.Controls.Add(this.lblInventoryLookup);
            resources.ApplyResources(this.gbInventory, "gbInventory");
            this.gbInventory.Name = "gbInventory";
            this.gbInventory.TabStop = false;
            // 
            // chkUseCentralizedInventoryLookup
            // 
            resources.ApplyResources(this.chkUseCentralizedInventoryLookup, "chkUseCentralizedInventoryLookup");
            this.chkUseCentralizedInventoryLookup.Name = "chkUseCentralizedInventoryLookup";
            this.chkUseCentralizedInventoryLookup.UseVisualStyleBackColor = true;
            // 
            // lblInventoryLookup
            // 
            resources.ApplyResources(this.lblInventoryLookup, "lblInventoryLookup");
            this.lblInventoryLookup.Name = "lblInventoryLookup";
            // 
            // SettingsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.gbInventory);
            this.Controls.Add(this.gbCustomer);
            this.Controls.Add(this.gbCentralSuspension);
            this.Controls.Add(this.gbCreditMemo);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Name = "SettingsPage";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbCreditMemo.ResumeLayout(false);
            this.gbCentralSuspension.ResumeLayout(false);
            this.gbCustomer.ResumeLayout(false);
            this.gbInventory.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkUseGiftcard;
        private System.Windows.Forms.Label lblUseGiftCard;
        private System.Windows.Forms.Label lblGiftCardOption;
        private System.Windows.Forms.ComboBox cmbGiftCardOption;
        private System.Windows.Forms.GroupBox gbCreditMemo;
        private System.Windows.Forms.CheckBox chkUseCreditVouchers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbCentralSuspension;
        private System.Windows.Forms.CheckBox chkUserConfirmation;
        private System.Windows.Forms.CheckBox chkUseCentralSuspension;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUserConfirmation;
        private System.Windows.Forms.GroupBox gbCustomer;
        private DualDataComboBox cmbSalesTaxGroup;
        private ContextButton btnEditSalesTaxGroup;
        private System.Windows.Forms.CheckBox chkCentralizedCustomers;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label lblDefaultSalesTaxGroup;
        private System.Windows.Forms.Label lblCashCustomer;
        private System.Windows.Forms.ComboBox cmbCashCustomer;
        private System.Windows.Forms.GroupBox gbInventory;
        private System.Windows.Forms.CheckBox chkUseCentralizedInventoryLookup;
        private System.Windows.Forms.Label lblInventoryLookup;
        private System.Windows.Forms.ComboBox cmbRefillableGiftcard;
        private System.Windows.Forms.Label label3;
        private NumericTextBox ntbMaximumGiftCardAmount;
        private System.Windows.Forms.Label label4;


    }
}
