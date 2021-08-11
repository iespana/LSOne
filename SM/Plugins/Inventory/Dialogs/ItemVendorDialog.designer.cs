using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    partial class ItemVendorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemVendorDialog));
            this.tbID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblRelation = new System.Windows.Forms.Label();
            this.cmbVendor = new LSOne.Controls.DualDataComboBox();
            this.btnAddUnit = new LSOne.Controls.ContextButton();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbUnit = new LSOne.Controls.DualDataComboBox();
            this.btnAddItem = new LSOne.Controls.ContextButton();
            this.lblPurchasePrice = new System.Windows.Forms.Label();
            this.ntbPurchasePrice = new LSOne.Controls.NumericTextBox();
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
            // cmbVendor
            // 
            this.cmbVendor.AddList = null;
            this.cmbVendor.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbVendor, "cmbVendor");
            this.cmbVendor.MaxLength = 32767;
            this.cmbVendor.Name = "cmbVendor";
            this.cmbVendor.NoChangeAllowed = false;
            this.cmbVendor.OnlyDisplayID = false;
            this.cmbVendor.RemoveList = null;
            this.cmbVendor.RowHeight = ((short)(22));
            this.cmbVendor.SecondaryData = null;
            this.cmbVendor.SelectedData = null;
            this.cmbVendor.SelectedDataID = null;
            this.cmbVendor.SelectionList = null;
            this.cmbVendor.SkipIDColumn = false;
            this.cmbVendor.RequestData += new System.EventHandler(this.cmbRetailItemOrVendor_RequestData);
            this.cmbVendor.SelectedDataChanged += new System.EventHandler(this.cmbRetailItemOrVendor_SelectedDataChanged);
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
            // lblPurchasePrice
            // 
            resources.ApplyResources(this.lblPurchasePrice, "lblPurchasePrice");
            this.lblPurchasePrice.Name = "lblPurchasePrice";
            // 
            // ntbPurchasePrice
            // 
            this.ntbPurchasePrice.AllowDecimal = true;
            this.ntbPurchasePrice.AllowNegative = false;
            this.ntbPurchasePrice.CultureInfo = null;
            this.ntbPurchasePrice.DecimalLetters = 2;
            this.ntbPurchasePrice.ForeColor = System.Drawing.Color.Black;
            this.ntbPurchasePrice.HasMinValue = false;
            resources.ApplyResources(this.ntbPurchasePrice, "ntbPurchasePrice");
            this.ntbPurchasePrice.MaxValue = 0D;
            this.ntbPurchasePrice.MinValue = 0D;
            this.ntbPurchasePrice.Name = "ntbPurchasePrice";
            this.ntbPurchasePrice.Value = 0D;
            this.ntbPurchasePrice.ValueChanged += new System.EventHandler(this.CheckEnabled);
            this.ntbPurchasePrice.Leave += new System.EventHandler(this.CheckEnabled);
            // 
            // ItemVendorDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.ntbPurchasePrice);
            this.Controls.Add(this.lblPurchasePrice);
            this.Controls.Add(this.btnAddItem);
            this.Controls.Add(this.btnAddUnit);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.lblRelation);
            this.Controls.Add(this.cmbVendor);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "ItemVendorDialog";
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.tbID, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.cmbVendor, 0);
            this.Controls.SetChildIndex(this.lblRelation, 0);
            this.Controls.SetChildIndex(this.cmbUnit, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.btnAddUnit, 0);
            this.Controls.SetChildIndex(this.btnAddItem, 0);
            this.Controls.SetChildIndex(this.lblPurchasePrice, 0);
            this.Controls.SetChildIndex(this.ntbPurchasePrice, 0);
            this.panel2.ResumeLayout(false);
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
        private DualDataComboBox cmbVendor;
        private ContextButton btnAddItem;
        private ContextButton btnAddUnit;
        private System.Windows.Forms.Label label6;
        private DualDataComboBox cmbUnit;
        private System.Windows.Forms.Label lblPurchasePrice;
        private NumericTextBox ntbPurchasePrice;
    }
}