using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    partial class VendorItemDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VendorItemDialog));
            this.tbID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkCreateAnother = new LSOne.Controls.DoubleBufferedCheckbox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblRelation = new System.Windows.Forms.Label();
            this.cmbRetailItem = new LSOne.Controls.DualDataComboBox();
            this.btnAddUnit = new LSOne.Controls.ContextButton();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbUnit = new LSOne.Controls.DualDataComboBox();
            this.btnAddItem = new LSOne.Controls.ContextButton();
            this.lblVariant = new System.Windows.Forms.Label();
            this.cmbVariant = new LSOne.Controls.DualDataComboBox();
            this.ntbDefaultCostPrice = new LSOne.Controls.NumericTextBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.tbBarcode = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbID
            // 
            resources.ApplyResources(this.tbID, "tbID");
            this.tbID.Name = "tbID";
            this.tbID.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
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
            // lblRelation
            // 
            this.lblRelation.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblRelation, "lblRelation");
            this.lblRelation.Name = "lblRelation";
            // 
            // cmbRetailItem
            // 
            this.cmbRetailItem.AddList = null;
            this.cmbRetailItem.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbRetailItem, "cmbRetailItem");
            this.cmbRetailItem.MaxLength = 32767;
            this.cmbRetailItem.Name = "cmbRetailItem";
            this.cmbRetailItem.NoChangeAllowed = false;
            this.cmbRetailItem.OnlyDisplayID = false;
            this.cmbRetailItem.RemoveList = null;
            this.cmbRetailItem.RowHeight = ((short)(22));
            this.cmbRetailItem.SecondaryData = null;
            this.cmbRetailItem.SelectedData = null;
            this.cmbRetailItem.SelectedDataID = null;
            this.cmbRetailItem.SelectionList = null;
            this.cmbRetailItem.SkipIDColumn = false;
            this.cmbRetailItem.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbRetailItem_DropDown);
            this.cmbRetailItem.SelectedDataChanged += new System.EventHandler(this.cmbRetailItem_SelectedDataChanged);
            this.cmbRetailItem.Leave += new System.EventHandler(this.cmbRetailItem_Leave);
            // 
            // btnAddUnit
            // 
            this.btnAddUnit.BackColor = System.Drawing.Color.Transparent;
            this.btnAddUnit.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddUnit, "btnAddUnit");
            this.btnAddUnit.Name = "btnAddUnit";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
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
            this.cmbUnit.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbUnit_DropDown);
            this.cmbUnit.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // btnAddItem
            // 
            this.btnAddItem.BackColor = System.Drawing.Color.Transparent;
            this.btnAddItem.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddItem, "btnAddItem");
            this.btnAddItem.Name = "btnAddItem";
            // 
            // lblVariant
            // 
            this.lblVariant.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblVariant, "lblVariant");
            this.lblVariant.Name = "lblVariant";
            // 
            // cmbVariant
            // 
            this.cmbVariant.AddList = null;
            this.cmbVariant.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbVariant, "cmbVariant");
            this.cmbVariant.MaxLength = 32767;
            this.cmbVariant.Name = "cmbVariant";
            this.cmbVariant.NoChangeAllowed = false;
            this.cmbVariant.OnlyDisplayID = false;
            this.cmbVariant.RemoveList = null;
            this.cmbVariant.RowHeight = ((short)(22));
            this.cmbVariant.SecondaryData = null;
            this.cmbVariant.SelectedData = null;
            this.cmbVariant.SelectedDataID = null;
            this.cmbVariant.SelectionList = null;
            this.cmbVariant.SkipIDColumn = false;
            this.cmbVariant.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbVariant_DropDown);
            this.cmbVariant.SelectedDataChanged += new System.EventHandler(this.cmbVariant_SelectedDataChanged);
            // 
            // ntbDefaultCostPrice
            // 
            this.ntbDefaultCostPrice.AllowDecimal = true;
            this.ntbDefaultCostPrice.AllowNegative = false;
            this.ntbDefaultCostPrice.CultureInfo = null;
            this.ntbDefaultCostPrice.DecimalLetters = 2;
            this.ntbDefaultCostPrice.ForeColor = System.Drawing.Color.Black;
            this.ntbDefaultCostPrice.HasMinValue = false;
            resources.ApplyResources(this.ntbDefaultCostPrice, "ntbDefaultCostPrice");
            this.ntbDefaultCostPrice.MaxValue = 0D;
            this.ntbDefaultCostPrice.MinValue = 0D;
            this.ntbDefaultCostPrice.Name = "ntbDefaultCostPrice";
            this.ntbDefaultCostPrice.Value = 0D;
            this.ntbDefaultCostPrice.ValueChanged += new System.EventHandler(this.ntbDefaultCostPrice_ValueChanged);
            // 
            // lblPrice
            // 
            this.lblPrice.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblPrice, "lblPrice");
            this.lblPrice.Name = "lblPrice";
            // 
            // lblBarcode
            // 
            resources.ApplyResources(this.lblBarcode, "lblBarcode");
            this.lblBarcode.Name = "lblBarcode";
            // 
            // tbBarcode
            // 
            resources.ApplyResources(this.tbBarcode, "tbBarcode");
            this.tbBarcode.Name = "tbBarcode";
            this.tbBarcode.Enter += new System.EventHandler(this.tbBarcode_Enter);
            this.tbBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbBarcode_KeyDown);
            this.tbBarcode.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbBarcode_KeyUp);
            this.tbBarcode.Leave += new System.EventHandler(this.tbBarcode_Leave);
            // 
            // VendorItemDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lblBarcode);
            this.Controls.Add(this.tbBarcode);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.ntbDefaultCostPrice);
            this.Controls.Add(this.lblVariant);
            this.Controls.Add(this.cmbVariant);
            this.Controls.Add(this.btnAddItem);
            this.Controls.Add(this.btnAddUnit);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.lblRelation);
            this.Controls.Add(this.cmbRetailItem);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "VendorItemDialog";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.VendorItemDialog_KeyUp);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.tbID, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.cmbRetailItem, 0);
            this.Controls.SetChildIndex(this.lblRelation, 0);
            this.Controls.SetChildIndex(this.cmbUnit, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.btnAddUnit, 0);
            this.Controls.SetChildIndex(this.btnAddItem, 0);
            this.Controls.SetChildIndex(this.cmbVariant, 0);
            this.Controls.SetChildIndex(this.lblVariant, 0);
            this.Controls.SetChildIndex(this.ntbDefaultCostPrice, 0);
            this.Controls.SetChildIndex(this.lblPrice, 0);
            this.Controls.SetChildIndex(this.tbBarcode, 0);
            this.Controls.SetChildIndex(this.lblBarcode, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblRelation;
        private DualDataComboBox cmbRetailItem;
        private ContextButton btnAddItem;
        private ContextButton btnAddUnit;
        private System.Windows.Forms.Label label6;
        private DualDataComboBox cmbUnit;
        private System.Windows.Forms.Label lblVariant;
        private DualDataComboBox cmbVariant;
        private NumericTextBox ntbDefaultCostPrice;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.TextBox tbBarcode;
        private DoubleBufferedCheckbox chkCreateAnother;
    }
}