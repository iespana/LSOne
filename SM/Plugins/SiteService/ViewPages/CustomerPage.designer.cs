using LSOne.Controls;

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    partial class CustomerPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerPage));
            this.cmbSalesTaxGroup = new LSOne.Controls.DualDataComboBox();
            this.btnEditSalesTaxGroup = new LSOne.Controls.ContextButton();
            this.chkCentralizedCustomers = new System.Windows.Forms.CheckBox();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.lblDefaultSalesTaxGroup = new System.Windows.Forms.Label();
            this.lblCashCustomer = new System.Windows.Forms.Label();
            this.cmbCashCustomer = new System.Windows.Forms.ComboBox();
            this.chkManualID = new System.Windows.Forms.CheckBox();
            this.lblManualID = new System.Windows.Forms.Label();
            this.ntbDefaultCreditLimit = new LSOne.Controls.NumericTextBox();
            this.lblDefaultCredit = new System.Windows.Forms.Label();
            this.lblMandatoryFields = new System.Windows.Forms.Label();
            this.chkListMandatoryFields = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
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
            // chkManualID
            // 
            resources.ApplyResources(this.chkManualID, "chkManualID");
            this.chkManualID.Name = "chkManualID";
            this.chkManualID.UseVisualStyleBackColor = true;
            // 
            // lblManualID
            // 
            resources.ApplyResources(this.lblManualID, "lblManualID");
            this.lblManualID.Name = "lblManualID";
            // 
            // ntbDefaultCreditLimit
            // 
            this.ntbDefaultCreditLimit.AcceptsTab = true;
            this.ntbDefaultCreditLimit.AllowDecimal = true;
            this.ntbDefaultCreditLimit.AllowNegative = false;
            this.ntbDefaultCreditLimit.BackColor = System.Drawing.SystemColors.Window;
            this.ntbDefaultCreditLimit.CultureInfo = null;
            this.ntbDefaultCreditLimit.DecimalLetters = 2;
            this.ntbDefaultCreditLimit.ForeColor = System.Drawing.Color.Black;
            this.ntbDefaultCreditLimit.HasMinValue = false;
            resources.ApplyResources(this.ntbDefaultCreditLimit, "ntbDefaultCreditLimit");
            this.ntbDefaultCreditLimit.MaxValue = 0D;
            this.ntbDefaultCreditLimit.MinValue = 0D;
            this.ntbDefaultCreditLimit.Name = "ntbDefaultCreditLimit";
            this.ntbDefaultCreditLimit.Value = 0D;
            // 
            // lblDefaultCredit
            // 
            resources.ApplyResources(this.lblDefaultCredit, "lblDefaultCredit");
            this.lblDefaultCredit.Name = "lblDefaultCredit";
            // 
            // lblMandatoryFields
            // 
            resources.ApplyResources(this.lblMandatoryFields, "lblMandatoryFields");
            this.lblMandatoryFields.Name = "lblMandatoryFields";
            // 
            // chkListMandatoryFields
            // 
            this.chkListMandatoryFields.CheckOnClick = true;
            this.chkListMandatoryFields.FormattingEnabled = true;
            resources.ApplyResources(this.chkListMandatoryFields, "chkListMandatoryFields");
            this.chkListMandatoryFields.Name = "chkListMandatoryFields";
            // 
            // CustomerPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkListMandatoryFields);
            this.Controls.Add(this.lblMandatoryFields);
            this.Controls.Add(this.lblDefaultCredit);
            this.Controls.Add(this.ntbDefaultCreditLimit);
            this.Controls.Add(this.chkManualID);
            this.Controls.Add(this.lblManualID);
            this.Controls.Add(this.cmbSalesTaxGroup);
            this.Controls.Add(this.btnEditSalesTaxGroup);
            this.Controls.Add(this.chkCentralizedCustomers);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.lblDefaultSalesTaxGroup);
            this.Controls.Add(this.lblCashCustomer);
            this.Controls.Add(this.cmbCashCustomer);
            this.DoubleBuffered = true;
            this.Name = "CustomerPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DualDataComboBox cmbSalesTaxGroup;
        private ContextButton btnEditSalesTaxGroup;
        private System.Windows.Forms.CheckBox chkCentralizedCustomers;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label lblDefaultSalesTaxGroup;
        private System.Windows.Forms.Label lblCashCustomer;
        private System.Windows.Forms.ComboBox cmbCashCustomer;
        private System.Windows.Forms.CheckBox chkManualID;
        private System.Windows.Forms.Label lblManualID;
        private NumericTextBox ntbDefaultCreditLimit;
        private System.Windows.Forms.Label lblDefaultCredit;
        private System.Windows.Forms.Label lblMandatoryFields;
        private System.Windows.Forms.CheckedListBox chkListMandatoryFields;
    }
}
