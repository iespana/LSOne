using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    partial class InventoryTransferOrderItemDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryTransferOrderItemDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbItem = new LSOne.Controls.DualDataComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbVariant = new LSOne.Controls.DualDataComboBox();
            this.lblVariant = new System.Windows.Forms.Label();
            this.cmbUnit = new LSOne.Controls.DualDataComboBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.ntbSendingQuantity = new LSOne.Controls.NumericTextBox();
            this.lblQuantitySending = new System.Windows.Forms.Label();
            this.lblReceivingQuantity = new System.Windows.Forms.Label();
            this.ntbReceivingQuantity = new LSOne.Controls.NumericTextBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
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
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // cmbItem
            // 
            this.cmbItem.AddList = null;
            this.cmbItem.AllowKeyboardSelection = false;
            this.cmbItem.EnableTextBox = true;
            resources.ApplyResources(this.cmbItem, "cmbItem");
            this.cmbItem.MaxLength = 32767;
            this.cmbItem.Name = "cmbItem";
            this.cmbItem.NoChangeAllowed = false;
            this.cmbItem.OnlyDisplayID = false;
            this.cmbItem.ReceiveKeyboardEvents = true;
            this.cmbItem.RemoveList = null;
            this.cmbItem.RowHeight = ((short)(22));
            this.cmbItem.SecondaryData = null;
            this.cmbItem.SelectedData = null;
            this.cmbItem.SelectedDataID = null;
            this.cmbItem.SelectionList = null;
            this.cmbItem.ShowDropDownOnTyping = true;
            this.cmbItem.SkipIDColumn = false;
            this.cmbItem.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbItem_DropDown);
            this.cmbItem.SelectedDataChanged += new System.EventHandler(this.cmbItem_SelectedDataChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
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
            this.cmbVariant.ReceiveKeyboardEvents = true;
            this.cmbVariant.RemoveList = null;
            this.cmbVariant.RowHeight = ((short)(22));
            this.cmbVariant.SecondaryData = null;
            this.cmbVariant.SelectedData = null;
            this.cmbVariant.SelectedDataID = null;
            this.cmbVariant.SelectionList = null;
            this.cmbVariant.SkipIDColumn = true;
            this.cmbVariant.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbVariant_DropDown);
            this.cmbVariant.SelectedDataChanged += new System.EventHandler(this.cmbVariant_SelectedDataChanged);
            // 
            // lblVariant
            // 
            this.lblVariant.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblVariant, "lblVariant");
            this.lblVariant.Name = "lblVariant";
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
            this.cmbUnit.ReceiveKeyboardEvents = true;
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
            // lblUnit
            // 
            this.lblUnit.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblUnit, "lblUnit");
            this.lblUnit.Name = "lblUnit";
            // 
            // ntbSendingQuantity
            // 
            this.ntbSendingQuantity.AllowDecimal = true;
            this.ntbSendingQuantity.AllowNegative = false;
            this.ntbSendingQuantity.CultureInfo = null;
            this.ntbSendingQuantity.DecimalLetters = 2;
            this.ntbSendingQuantity.ForeColor = System.Drawing.Color.Black;
            this.ntbSendingQuantity.HasMinValue = false;
            resources.ApplyResources(this.ntbSendingQuantity, "ntbSendingQuantity");
            this.ntbSendingQuantity.MaxValue = 100000D;
            this.ntbSendingQuantity.MinValue = 0D;
            this.ntbSendingQuantity.Name = "ntbSendingQuantity";
            this.ntbSendingQuantity.Value = 0D;
            this.ntbSendingQuantity.TextChanged += new System.EventHandler(this.OKBtnEnabled);
            // 
            // lblQuantitySending
            // 
            this.lblQuantitySending.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblQuantitySending, "lblQuantitySending");
            this.lblQuantitySending.Name = "lblQuantitySending";
            // 
            // lblReceivingQuantity
            // 
            this.lblReceivingQuantity.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblReceivingQuantity, "lblReceivingQuantity");
            this.lblReceivingQuantity.Name = "lblReceivingQuantity";
            // 
            // ntbReceivingQuantity
            // 
            this.ntbReceivingQuantity.AllowDecimal = true;
            this.ntbReceivingQuantity.AllowNegative = false;
            this.ntbReceivingQuantity.CultureInfo = null;
            this.ntbReceivingQuantity.DecimalLetters = 2;
            this.ntbReceivingQuantity.ForeColor = System.Drawing.Color.Black;
            this.ntbReceivingQuantity.HasMinValue = false;
            resources.ApplyResources(this.ntbReceivingQuantity, "ntbReceivingQuantity");
            this.ntbReceivingQuantity.MaxValue = 100000D;
            this.ntbReceivingQuantity.MinValue = 0D;
            this.ntbReceivingQuantity.Name = "ntbReceivingQuantity";
            this.ntbReceivingQuantity.Value = 0D;
            this.ntbReceivingQuantity.TextChanged += new System.EventHandler(this.OKBtnEnabled);
            // 
            // InventoryTransferOrderItemDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lblReceivingQuantity);
            this.Controls.Add(this.ntbReceivingQuantity);
            this.Controls.Add(this.lblQuantitySending);
            this.Controls.Add(this.ntbSendingQuantity);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.lblVariant);
            this.Controls.Add(this.cmbVariant);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbItem);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "InventoryTransferOrderItemDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.cmbItem, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.cmbVariant, 0);
            this.Controls.SetChildIndex(this.lblVariant, 0);
            this.Controls.SetChildIndex(this.cmbUnit, 0);
            this.Controls.SetChildIndex(this.lblUnit, 0);
            this.Controls.SetChildIndex(this.ntbSendingQuantity, 0);
            this.Controls.SetChildIndex(this.lblQuantitySending, 0);
            this.Controls.SetChildIndex(this.ntbReceivingQuantity, 0);
            this.Controls.SetChildIndex(this.lblReceivingQuantity, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label3;
        private DualDataComboBox cmbItem;
        private DualDataComboBox cmbVariant;
        private System.Windows.Forms.Label lblVariant;
        private System.Windows.Forms.Label lblUnit;
        private DualDataComboBox cmbUnit;
        private System.Windows.Forms.Label lblQuantitySending;
        private NumericTextBox ntbSendingQuantity;
        private System.Windows.Forms.Label lblReceivingQuantity;
        private NumericTextBox ntbReceivingQuantity;
    }
}