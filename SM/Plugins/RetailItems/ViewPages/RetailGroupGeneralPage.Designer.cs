using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    partial class RetailGroupGeneralPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RetailGroupGeneralPage));
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDiscountPercent = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.btnEditValidationPeriod = new LSOne.Controls.ContextButton();
            this.cmbValidationPeriod = new LSOne.Controls.DualDataComboBox();
            this.ntbProfitMargin = new LSOne.Controls.NumericTextBox();
            this.btnEditItemSalesTaxGroup = new LSOne.Controls.ContextButton();
            this.btnEditRetailDepartment = new LSOne.Controls.ContextButton();
            this.cmbRetailDepartment = new LSOne.Controls.DualDataComboBox();
            this.cmbItemSalesTaxGroup = new LSOne.Controls.DualDataComboBox();
            this.lblTareWeight = new System.Windows.Forms.Label();
            this.ntbTareWeight = new LSOne.Controls.NumericTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lblDiscountPercent
            // 
            this.lblDiscountPercent.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDiscountPercent, "lblDiscountPercent");
            this.lblDiscountPercent.Name = "lblDiscountPercent";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnEditValidationPeriod
            // 
            this.btnEditValidationPeriod.BackColor = System.Drawing.Color.Transparent;
            this.btnEditValidationPeriod.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditValidationPeriod, "btnEditValidationPeriod");
            this.btnEditValidationPeriod.Name = "btnEditValidationPeriod";
            this.btnEditValidationPeriod.Click += new System.EventHandler(this.btnEditValidationPeriod_Click);
            // 
            // cmbValidationPeriod
            // 
            this.cmbValidationPeriod.AddList = null;
            this.cmbValidationPeriod.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbValidationPeriod, "cmbValidationPeriod");
            this.cmbValidationPeriod.MaxLength = 32767;
            this.cmbValidationPeriod.Name = "cmbValidationPeriod";
            this.cmbValidationPeriod.NoChangeAllowed = false;
            this.cmbValidationPeriod.OnlyDisplayID = false;
            this.cmbValidationPeriod.RemoveList = null;
            this.cmbValidationPeriod.RowHeight = ((short)(22));
            this.cmbValidationPeriod.SecondaryData = null;
            this.cmbValidationPeriod.SelectedData = null;
            this.cmbValidationPeriod.SelectedDataID = null;
            this.cmbValidationPeriod.SelectionList = null;
            this.cmbValidationPeriod.SkipIDColumn = true;
            this.cmbValidationPeriod.RequestData += new System.EventHandler(this.cmbValidationPeriod_RequestData);
            this.cmbValidationPeriod.RequestClear += new System.EventHandler(this.cmbValidationPeriod_RequestClear);
            // 
            // ntbProfitMargin
            // 
            this.ntbProfitMargin.AcceptsTab = true;
            this.ntbProfitMargin.AllowDecimal = true;
            this.ntbProfitMargin.AllowNegative = false;
            this.ntbProfitMargin.BackColor = System.Drawing.SystemColors.Window;
            this.ntbProfitMargin.CultureInfo = null;
            this.ntbProfitMargin.DecimalLetters = 2;
            this.ntbProfitMargin.ForeColor = System.Drawing.Color.Black;
            this.ntbProfitMargin.HasMinValue = false;
            resources.ApplyResources(this.ntbProfitMargin, "ntbProfitMargin");
            this.ntbProfitMargin.MaxValue = 100D;
            this.ntbProfitMargin.MinValue = 0D;
            this.ntbProfitMargin.Name = "ntbProfitMargin";
            this.ntbProfitMargin.Value = 0D;
            // 
            // btnEditItemSalesTaxGroup
            // 
            this.btnEditItemSalesTaxGroup.BackColor = System.Drawing.Color.Transparent;
            this.btnEditItemSalesTaxGroup.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditItemSalesTaxGroup, "btnEditItemSalesTaxGroup");
            this.btnEditItemSalesTaxGroup.Name = "btnEditItemSalesTaxGroup";
            this.btnEditItemSalesTaxGroup.Click += new System.EventHandler(this.btnEditItemSalesTaxGroup_Click);
            // 
            // btnEditRetailDepartment
            // 
            this.btnEditRetailDepartment.BackColor = System.Drawing.Color.Transparent;
            this.btnEditRetailDepartment.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditRetailDepartment, "btnEditRetailDepartment");
            this.btnEditRetailDepartment.Name = "btnEditRetailDepartment";
            this.btnEditRetailDepartment.Click += new System.EventHandler(this.btnEditRetailDepartment_Click);
            // 
            // cmbRetailDepartment
            // 
            this.cmbRetailDepartment.AddList = null;
            this.cmbRetailDepartment.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbRetailDepartment, "cmbRetailDepartment");
            this.cmbRetailDepartment.MaxLength = 32767;
            this.cmbRetailDepartment.Name = "cmbRetailDepartment";
            this.cmbRetailDepartment.NoChangeAllowed = false;
            this.cmbRetailDepartment.OnlyDisplayID = false;
            this.cmbRetailDepartment.RemoveList = null;
            this.cmbRetailDepartment.RowHeight = ((short)(22));
            this.cmbRetailDepartment.SecondaryData = null;
            this.cmbRetailDepartment.SelectedData = null;
            this.cmbRetailDepartment.SelectedDataID = null;
            this.cmbRetailDepartment.SelectionList = null;
            this.cmbRetailDepartment.SkipIDColumn = true;
            this.cmbRetailDepartment.RequestData += new System.EventHandler(this.cmbRetailDepartment_RequestData);
            this.cmbRetailDepartment.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // cmbItemSalesTaxGroup
            // 
            this.cmbItemSalesTaxGroup.AddList = null;
            this.cmbItemSalesTaxGroup.AllowKeyboardSelection = false;
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
            this.cmbItemSalesTaxGroup.SkipIDColumn = true;
            this.cmbItemSalesTaxGroup.RequestData += new System.EventHandler(this.cmbItemSalesTaxGroup_RequestData);
            this.cmbItemSalesTaxGroup.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // lblTareWeight
            // 
            this.lblTareWeight.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblTareWeight, "lblTareWeight");
            this.lblTareWeight.Name = "lblTareWeight";
            // 
            // ntbTareWeight
            // 
            this.ntbTareWeight.AcceptsTab = true;
            this.ntbTareWeight.AllowDecimal = false;
            this.ntbTareWeight.AllowNegative = false;
            this.ntbTareWeight.BackColor = System.Drawing.SystemColors.Window;
            this.ntbTareWeight.CultureInfo = null;
            this.ntbTareWeight.DecimalLetters = 2;
            this.ntbTareWeight.ForeColor = System.Drawing.Color.Black;
            this.ntbTareWeight.HasMinValue = false;
            resources.ApplyResources(this.ntbTareWeight, "ntbTareWeight");
            this.ntbTareWeight.MaxValue = 99999D;
            this.ntbTareWeight.MinValue = 0D;
            this.ntbTareWeight.Name = "ntbTareWeight";
            this.ntbTareWeight.Value = 0D;
            // 
            // RetailGroupGeneralPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblTareWeight);
            this.Controls.Add(this.ntbTareWeight);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnEditValidationPeriod);
            this.Controls.Add(this.cmbValidationPeriod);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblDiscountPercent);
            this.Controls.Add(this.ntbProfitMargin);
            this.Controls.Add(this.btnEditItemSalesTaxGroup);
            this.Controls.Add(this.btnEditRetailDepartment);
            this.Controls.Add(this.cmbRetailDepartment);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbItemSalesTaxGroup);
            this.Controls.Add(this.label9);
            this.DoubleBuffered = true;
            this.Name = "RetailGroupGeneralPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DualDataComboBox cmbRetailDepartment;
        private System.Windows.Forms.Label label4;
        private DualDataComboBox cmbItemSalesTaxGroup;
        private System.Windows.Forms.Label label9;
        private ContextButton btnEditRetailDepartment;
        private ContextButton btnEditItemSalesTaxGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDiscountPercent;
        private NumericTextBox ntbProfitMargin;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private ContextButton btnEditValidationPeriod;
        private DualDataComboBox cmbValidationPeriod;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTareWeight;
        private NumericTextBox ntbTareWeight;
    }
}
