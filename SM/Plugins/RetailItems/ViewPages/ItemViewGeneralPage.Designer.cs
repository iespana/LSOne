using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    partial class ItemViewGeneralPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemViewGeneralPage));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new LSOne.Controls.DoubleBufferGroupBox();
            this.cmbItemType = new System.Windows.Forms.ComboBox();
            this.btnEditItemType = new LSOne.Controls.ContextButton();
            this.lblItemType = new System.Windows.Forms.Label();
            this.tbDefaultBarcode = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnEditItemSalesTaxGroup = new LSOne.Controls.ContextButton();
            this.btnEditSalesUnits = new LSOne.Controls.ContextButton();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbItemSalesTaxGroup = new LSOne.Controls.DualDataComboBox();
            this.cmbSalesUnit = new LSOne.Controls.DualDataComboBox();
            this.lblSalesUnit = new System.Windows.Forms.Label();
            this.groupBox3 = new LSOne.Controls.DoubleBufferGroupBox();
            this.linkFields1 = new LSOne.Controls.LinkFields();
            this.btnEditRetailDivision = new LSOne.Controls.ContextButton();
            this.lblDivision = new System.Windows.Forms.Label();
            this.cmbRetailDivision = new LSOne.Controls.DualDataComboBox();
            this.btnEditRetailGroup = new LSOne.Controls.ContextButton();
            this.cmbRetailDepartment = new LSOne.Controls.DualDataComboBox();
            this.btnEditRetailDepartment = new LSOne.Controls.ContextButton();
            this.lblDepartment = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbRetailGroup = new LSOne.Controls.DualDataComboBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.errorProvider3 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider4 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider4)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.cmbItemType);
            this.groupBox1.Controls.Add(this.btnEditItemType);
            this.groupBox1.Controls.Add(this.lblItemType);
            this.groupBox1.Controls.Add(this.tbDefaultBarcode);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.btnEditItemSalesTaxGroup);
            this.groupBox1.Controls.Add(this.btnEditSalesUnits);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbItemSalesTaxGroup);
            this.groupBox1.Controls.Add(this.cmbSalesUnit);
            this.groupBox1.Controls.Add(this.lblSalesUnit);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cmbItemType
            // 
            this.cmbItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            resources.ApplyResources(this.cmbItemType, "cmbItemType");
            this.cmbItemType.FormattingEnabled = true;
            this.cmbItemType.Name = "cmbItemType";
            // 
            // btnEditItemType
            // 
            this.btnEditItemType.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditItemType, "btnEditItemType");
            this.btnEditItemType.Name = "btnEditItemType";
            this.btnEditItemType.Click += new System.EventHandler(this.btnEditItemType_Click);
            // 
            // lblItemType
            // 
            this.lblItemType.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblItemType, "lblItemType");
            this.lblItemType.Name = "lblItemType";
            // 
            // tbDefaultBarcode
            // 
            resources.ApplyResources(this.tbDefaultBarcode, "tbDefaultBarcode");
            this.tbDefaultBarcode.Name = "tbDefaultBarcode";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // btnEditItemSalesTaxGroup
            // 
            this.btnEditItemSalesTaxGroup.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditItemSalesTaxGroup, "btnEditItemSalesTaxGroup");
            this.btnEditItemSalesTaxGroup.Name = "btnEditItemSalesTaxGroup";
            this.btnEditItemSalesTaxGroup.Click += new System.EventHandler(this.btnEditItemSalesTaxGroup_Click);
            // 
            // btnEditSalesUnits
            // 
            this.btnEditSalesUnits.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditSalesUnits, "btnEditSalesUnits");
            this.btnEditSalesUnits.Name = "btnEditSalesUnits";
            this.btnEditSalesUnits.Click += new System.EventHandler(this.btnEditSalesUnits_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
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
            this.cmbItemSalesTaxGroup.SelectedDataChanged += new System.EventHandler(this.cmbItemSalesTaxGroup_SelectedDataChanged);
            this.cmbItemSalesTaxGroup.RequestClear += new System.EventHandler(this.RequestClear);
            // 
            // cmbSalesUnit
            // 
            this.cmbSalesUnit.AddList = null;
            this.cmbSalesUnit.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbSalesUnit, "cmbSalesUnit");
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
            this.cmbSalesUnit.SkipIDColumn = true;
            // 
            // lblSalesUnit
            // 
            this.lblSalesUnit.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblSalesUnit, "lblSalesUnit");
            this.lblSalesUnit.Name = "lblSalesUnit";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.linkFields1);
            this.groupBox3.Controls.Add(this.btnEditRetailDivision);
            this.groupBox3.Controls.Add(this.lblDivision);
            this.groupBox3.Controls.Add(this.cmbRetailDivision);
            this.groupBox3.Controls.Add(this.btnEditRetailGroup);
            this.groupBox3.Controls.Add(this.cmbRetailDepartment);
            this.groupBox3.Controls.Add(this.btnEditRetailDepartment);
            this.groupBox3.Controls.Add(this.lblDepartment);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.cmbRetailGroup);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // linkFields1
            // 
            this.linkFields1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields1, "linkFields1");
            this.linkFields1.Mode = LSOne.Controls.LinkFields.ModeEnum.Tripple;
            this.linkFields1.Name = "linkFields1";
            this.linkFields1.TabStop = false;
            // 
            // btnEditRetailDivision
            // 
            this.btnEditRetailDivision.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditRetailDivision, "btnEditRetailDivision");
            this.btnEditRetailDivision.Name = "btnEditRetailDivision";
            this.btnEditRetailDivision.Click += new System.EventHandler(this.btnEditRetailDivision_Click);
            // 
            // lblDivision
            // 
            this.lblDivision.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDivision, "lblDivision");
            this.lblDivision.Name = "lblDivision";
            // 
            // cmbRetailDivision
            // 
            this.cmbRetailDivision.AddList = null;
            this.cmbRetailDivision.AllowKeyboardSelection = false;
            this.cmbRetailDivision.EnableTextBox = true;
            resources.ApplyResources(this.cmbRetailDivision, "cmbRetailDivision");
            this.cmbRetailDivision.MaxLength = 32767;
            this.cmbRetailDivision.Name = "cmbRetailDivision";
            this.cmbRetailDivision.NoChangeAllowed = false;
            this.cmbRetailDivision.OnlyDisplayID = false;
            this.cmbRetailDivision.RemoveList = null;
            this.cmbRetailDivision.RowHeight = ((short)(22));
            this.cmbRetailDivision.SecondaryData = null;
            this.cmbRetailDivision.SelectedData = null;
            this.cmbRetailDivision.SelectedDataID = null;
            this.cmbRetailDivision.SelectionList = null;
            this.cmbRetailDivision.ShowDropDownOnTyping = true;
            this.cmbRetailDivision.SkipIDColumn = true;
            this.cmbRetailDivision.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbRetailDivision_DropDown);
            this.cmbRetailDivision.SelectedDataChanged += new System.EventHandler(this.cmbRetailDivision_SelectedDataChanged);
            this.cmbRetailDivision.RequestClear += new System.EventHandler(this.RequestClear);
            // 
            // btnEditRetailGroup
            // 
            this.btnEditRetailGroup.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditRetailGroup, "btnEditRetailGroup");
            this.btnEditRetailGroup.Name = "btnEditRetailGroup";
            this.btnEditRetailGroup.Click += new System.EventHandler(this.btnEditRetailGroup_Click);
            // 
            // cmbRetailDepartment
            // 
            this.cmbRetailDepartment.AddList = null;
            this.cmbRetailDepartment.AllowKeyboardSelection = false;
            this.cmbRetailDepartment.EnableTextBox = true;
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
            this.cmbRetailDepartment.ShowDropDownOnTyping = true;
            this.cmbRetailDepartment.SkipIDColumn = true;
            this.cmbRetailDepartment.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbRetailDepartment_DropDown);
            this.cmbRetailDepartment.SelectedDataChanged += new System.EventHandler(this.cmbRetailDepartment_SelectedDataChanged);
            this.cmbRetailDepartment.RequestClear += new System.EventHandler(this.RequestClear);
            // 
            // btnEditRetailDepartment
            // 
            this.btnEditRetailDepartment.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditRetailDepartment, "btnEditRetailDepartment");
            this.btnEditRetailDepartment.Name = "btnEditRetailDepartment";
            this.btnEditRetailDepartment.Click += new System.EventHandler(this.btnEditRetailDepartment_Click);
            // 
            // lblDepartment
            // 
            this.lblDepartment.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDepartment, "lblDepartment");
            this.lblDepartment.Name = "lblDepartment";
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
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
            this.cmbRetailGroup.RequestClear += new System.EventHandler(this.RequestClear);
            this.cmbRetailGroup.RequestNoChange += new System.EventHandler(this.cmbRetailGroup_RequestNoChange);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // errorProvider2
            // 
            this.errorProvider2.ContainerControl = this;
            // 
            // errorProvider3
            // 
            this.errorProvider3.ContainerControl = this;
            // 
            // errorProvider4
            // 
            this.errorProvider4.ContainerControl = this;
            // 
            // ItemViewGeneralPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "ItemViewGeneralPage";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DoubleBufferGroupBox groupBox1;
        private DualDataComboBox cmbSalesUnit;
        private System.Windows.Forms.Label lblSalesUnit;
        private DualDataComboBox cmbItemSalesTaxGroup;
        private System.Windows.Forms.Label label2;
        private DoubleBufferGroupBox groupBox3;
        private System.Windows.Forms.Label label12;
        private DualDataComboBox cmbRetailGroup;
        private DualDataComboBox cmbRetailDepartment;
        private System.Windows.Forms.Label lblDepartment;
        private ContextButton btnEditItemSalesTaxGroup;
        private ContextButton btnEditSalesUnits;
        private ContextButton btnEditRetailGroup;
        private ContextButton btnEditRetailDepartment;
        private System.Windows.Forms.TextBox tbDefaultBarcode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ErrorProvider errorProvider2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ErrorProvider errorProvider3;
        private System.Windows.Forms.ErrorProvider errorProvider4;
        private ContextButton btnEditRetailDivision;
        private System.Windows.Forms.Label lblDivision;
        private DualDataComboBox cmbRetailDivision;
        private LinkFields linkFields1;
        private ContextButton btnEditItemType;
        private System.Windows.Forms.Label lblItemType;
        private System.Windows.Forms.ComboBox cmbItemType;
    }
}
