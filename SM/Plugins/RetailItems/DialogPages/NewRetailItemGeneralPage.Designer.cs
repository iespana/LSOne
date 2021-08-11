namespace LSOne.ViewPlugins.RetailItems.DialogPages
{
    partial class NewRetailItemGeneralPage
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewRetailItemGeneralPage));
            this.tbID = new System.Windows.Forms.TextBox();
            this.lblID = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblRetailGroup = new System.Windows.Forms.Label();
            this.lblInventoryUnit = new System.Windows.Forms.Label();
            this.lblSalesUnit = new System.Windows.Forms.Label();
            this.lblItemSalesTaxGroup = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblExtendedDescription = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tbExtendedDescription = new System.Windows.Forms.TextBox();
            this.ntbPrice = new LSOne.Controls.NumericTextBox();
            this.btnAddSalesUnit = new LSOne.Controls.ContextButton();
            this.btnAddInventoryUnit = new LSOne.Controls.ContextButton();
            this.btnAddRetailGroup = new LSOne.Controls.ContextButton();
            this.cmbItemSalesTaxGroup = new LSOne.Controls.DualDataComboBox();
            this.cmbSalesUnit = new LSOne.Controls.DualDataComboBox();
            this.cmbInventoryUnit = new LSOne.Controls.DualDataComboBox();
            this.cmbRetailGroup = new LSOne.Controls.DualDataComboBox();
            this.lblItemType = new System.Windows.Forms.Label();
            this.cmbItemType = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
            this.SuspendLayout();
            // 
            // tbID
            // 
            resources.ApplyResources(this.tbID, "tbID");
            this.tbID.Name = "tbID";
            this.tbID.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblID
            // 
            resources.ApplyResources(this.lblID, "lblID");
            this.lblID.Name = "lblID";
            // 
            // lblDescription
            // 
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.Name = "lblDescription";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblRetailGroup
            // 
            resources.ApplyResources(this.lblRetailGroup, "lblRetailGroup");
            this.lblRetailGroup.Name = "lblRetailGroup";
            // 
            // lblInventoryUnit
            // 
            this.lblInventoryUnit.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblInventoryUnit, "lblInventoryUnit");
            this.lblInventoryUnit.Name = "lblInventoryUnit";
            // 
            // lblSalesUnit
            // 
            this.lblSalesUnit.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblSalesUnit, "lblSalesUnit");
            this.lblSalesUnit.Name = "lblSalesUnit";
            // 
            // lblItemSalesTaxGroup
            // 
            this.lblItemSalesTaxGroup.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblItemSalesTaxGroup, "lblItemSalesTaxGroup");
            this.lblItemSalesTaxGroup.Name = "lblItemSalesTaxGroup";
            // 
            // lblPrice
            // 
            this.lblPrice.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblPrice, "lblPrice");
            this.lblPrice.Name = "lblPrice";
            // 
            // lblExtendedDescription
            // 
            this.lblExtendedDescription.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblExtendedDescription, "lblExtendedDescription");
            this.lblExtendedDescription.Name = "lblExtendedDescription";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // errorProvider2
            // 
            this.errorProvider2.ContainerControl = this;
            // 
            // tbExtendedDescription
            // 
            this.tbExtendedDescription.AcceptsReturn = true;
            resources.ApplyResources(this.tbExtendedDescription, "tbExtendedDescription");
            this.tbExtendedDescription.Name = "tbExtendedDescription";
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
            this.ntbPrice.MaxValue = 0D;
            this.ntbPrice.MinValue = 0D;
            this.ntbPrice.Name = "ntbPrice";
            this.ntbPrice.Value = 0D;
            // 
            // btnAddSalesUnit
            // 
            this.btnAddSalesUnit.BackColor = System.Drawing.Color.Transparent;
            this.btnAddSalesUnit.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddSalesUnit, "btnAddSalesUnit");
            this.btnAddSalesUnit.Name = "btnAddSalesUnit";
            this.btnAddSalesUnit.Click += new System.EventHandler(this.btnAddSalesUnit_Click);
            // 
            // btnAddInventoryUnit
            // 
            this.btnAddInventoryUnit.BackColor = System.Drawing.Color.Transparent;
            this.btnAddInventoryUnit.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddInventoryUnit, "btnAddInventoryUnit");
            this.btnAddInventoryUnit.Name = "btnAddInventoryUnit";
            this.btnAddInventoryUnit.Click += new System.EventHandler(this.btnAddInventoryUnit_Click);
            // 
            // btnAddRetailGroup
            // 
            this.btnAddRetailGroup.BackColor = System.Drawing.Color.Transparent;
            this.btnAddRetailGroup.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddRetailGroup, "btnAddRetailGroup");
            this.btnAddRetailGroup.Name = "btnAddRetailGroup";
            this.btnAddRetailGroup.Click += new System.EventHandler(this.btnAddRetailGroup_Click);
            // 
            // cmbItemSalesTaxGroup
            // 
            this.cmbItemSalesTaxGroup.AddList = null;
            this.cmbItemSalesTaxGroup.AllowKeyboardSelection = false;
            this.cmbItemSalesTaxGroup.EnableTextBox = true;
            resources.ApplyResources(this.cmbItemSalesTaxGroup, "cmbItemSalesTaxGroup");
            this.cmbItemSalesTaxGroup.MaxLength = 32767;
            this.cmbItemSalesTaxGroup.Name = "cmbItemSalesTaxGroup";
            this.cmbItemSalesTaxGroup.NoChangeAllowed = false;
            this.cmbItemSalesTaxGroup.OnlyDisplayID = false;
            this.cmbItemSalesTaxGroup.RemoveList = null;
            this.cmbItemSalesTaxGroup.RowHeight = ((short)(22));
            this.cmbItemSalesTaxGroup.SecondaryData = null;
            this.cmbItemSalesTaxGroup.SelectedData = null;
            this.cmbItemSalesTaxGroup.SelectedDataID = null;
            this.cmbItemSalesTaxGroup.SelectionList = null;
            this.cmbItemSalesTaxGroup.ShowDropDownOnTyping = true;
            this.cmbItemSalesTaxGroup.SkipIDColumn = true;
            this.cmbItemSalesTaxGroup.RequestData += new System.EventHandler(this.cmbItemSalesTaxGroup_RequestData);
            this.cmbItemSalesTaxGroup.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbSalesUnit
            // 
            this.cmbSalesUnit.AddList = null;
            this.cmbSalesUnit.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbSalesUnit, "cmbSalesUnit");
            this.cmbSalesUnit.EnableTextBox = true;
            this.cmbSalesUnit.MaxLength = 32767;
            this.cmbSalesUnit.Name = "cmbSalesUnit";
            this.cmbSalesUnit.NoChangeAllowed = false;
            this.cmbSalesUnit.OnlyDisplayID = false;
            this.cmbSalesUnit.RemoveList = null;
            this.cmbSalesUnit.RowHeight = ((short)(22));
            this.cmbSalesUnit.SecondaryData = null;
            this.cmbSalesUnit.SelectedData = null;
            this.cmbSalesUnit.SelectedDataID = null;
            this.cmbSalesUnit.SelectionList = null;
            this.cmbSalesUnit.ShowDropDownOnTyping = true;
            this.cmbSalesUnit.SkipIDColumn = true;
            this.cmbSalesUnit.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbSalesUnit_DropDown);
            this.cmbSalesUnit.SelectedDataChanged += new System.EventHandler(this.cmbSalesUnit_SelectedDataChanged);
            // 
            // cmbInventoryUnit
            // 
            this.cmbInventoryUnit.AddList = null;
            this.cmbInventoryUnit.AllowKeyboardSelection = false;
            this.cmbInventoryUnit.EnableTextBox = true;
            resources.ApplyResources(this.cmbInventoryUnit, "cmbInventoryUnit");
            this.cmbInventoryUnit.MaxLength = 32767;
            this.cmbInventoryUnit.Name = "cmbInventoryUnit";
            this.cmbInventoryUnit.NoChangeAllowed = false;
            this.cmbInventoryUnit.OnlyDisplayID = false;
            this.cmbInventoryUnit.RemoveList = null;
            this.cmbInventoryUnit.RowHeight = ((short)(22));
            this.cmbInventoryUnit.SecondaryData = null;
            this.cmbInventoryUnit.SelectedData = null;
            this.cmbInventoryUnit.SelectedDataID = null;
            this.cmbInventoryUnit.SelectionList = null;
            this.cmbInventoryUnit.ShowDropDownOnTyping = true;
            this.cmbInventoryUnit.SkipIDColumn = true;
            this.cmbInventoryUnit.RequestData += new System.EventHandler(this.cmbInventoryUnit_RequestData);
            this.cmbInventoryUnit.SelectedDataChanged += new System.EventHandler(this.cmbInventoryUnit_SelectedDataChanged);
            // 
            // cmbRetailGroup
            // 
            this.cmbRetailGroup.AddList = null;
            this.cmbRetailGroup.AllowKeyboardSelection = false;
            this.cmbRetailGroup.EnableTextBox = true;
            resources.ApplyResources(this.cmbRetailGroup, "cmbRetailGroup");
            this.cmbRetailGroup.MaxLength = 32767;
            this.cmbRetailGroup.Name = "cmbRetailGroup";
            this.cmbRetailGroup.NoChangeAllowed = false;
            this.cmbRetailGroup.OnlyDisplayID = false;
            this.cmbRetailGroup.RemoveList = null;
            this.cmbRetailGroup.RowHeight = ((short)(22));
            this.cmbRetailGroup.SecondaryData = null;
            this.cmbRetailGroup.SelectedData = null;
            this.cmbRetailGroup.SelectedDataID = null;
            this.cmbRetailGroup.SelectionList = null;
            this.cmbRetailGroup.ShowDropDownOnTyping = true;
            this.cmbRetailGroup.SkipIDColumn = true;
            this.cmbRetailGroup.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbRetailGroup_DropDown);
            this.cmbRetailGroup.SelectedDataChanged += new System.EventHandler(this.cmbRetailGroup_SelectedDataChanged);
            this.cmbRetailGroup.RequestClear += new System.EventHandler(this.cmbRetailGroup_RequestClear);
            // 
            // lblItemType
            // 
            resources.ApplyResources(this.lblItemType, "lblItemType");
            this.lblItemType.Name = "lblItemType";
            // 
            // cmbItemType
            // 
            this.cmbItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbItemType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbItemType, "cmbItemType");
            this.cmbItemType.Name = "cmbItemType";
            this.cmbItemType.SelectedIndexChanged += new System.EventHandler(this.cmbItemType_SelectedIndexChanged);
            this.cmbItemType.SelectedValueChanged += new System.EventHandler(this.cmbItemType_SelectedValueChanged);
            // 
            // NewRetailItemGeneralPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cmbItemType);
            this.Controls.Add(this.lblItemType);
            this.Controls.Add(this.tbExtendedDescription);
            this.Controls.Add(this.lblExtendedDescription);
            this.Controls.Add(this.ntbPrice);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.lblItemSalesTaxGroup);
            this.Controls.Add(this.cmbItemSalesTaxGroup);
            this.Controls.Add(this.btnAddSalesUnit);
            this.Controls.Add(this.lblSalesUnit);
            this.Controls.Add(this.cmbSalesUnit);
            this.Controls.Add(this.lblInventoryUnit);
            this.Controls.Add(this.btnAddInventoryUnit);
            this.Controls.Add(this.cmbInventoryUnit);
            this.Controls.Add(this.lblRetailGroup);
            this.Controls.Add(this.btnAddRetailGroup);
            this.Controls.Add(this.cmbRetailGroup);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.tbID);
            this.DoubleBuffered = true;
            this.Name = "NewRetailItemGeneralPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox tbDescription;
        private Controls.DualDataComboBox cmbRetailGroup;
        private Controls.ContextButton btnAddRetailGroup;
        private System.Windows.Forms.Label lblRetailGroup;
        private Controls.DualDataComboBox cmbInventoryUnit;
        private Controls.ContextButton btnAddInventoryUnit;
        private System.Windows.Forms.Label lblInventoryUnit;
        private Controls.ContextButton btnAddSalesUnit;
        private System.Windows.Forms.Label lblSalesUnit;
        private Controls.DualDataComboBox cmbSalesUnit;
        private System.Windows.Forms.Label lblItemSalesTaxGroup;
        private Controls.DualDataComboBox cmbItemSalesTaxGroup;
        private Controls.NumericTextBox ntbPrice;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblExtendedDescription;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ErrorProvider errorProvider2;
        private System.Windows.Forms.TextBox tbExtendedDescription;
        private System.Windows.Forms.Label lblItemType;
        private System.Windows.Forms.ComboBox cmbItemType;
    }
}
