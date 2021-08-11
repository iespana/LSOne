using LSOne.Controls;

namespace LSOne.ViewPlugins.BarCodes.Dialogs
{
    partial class BarCodeDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BarCodeDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.chkToBePrinted = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkScanning = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkShowForItem = new System.Windows.Forms.CheckBox();
            this.tbBarCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnAddUnit = new LSOne.Controls.ContextButton();
            this.label9 = new System.Windows.Forms.Label();
            this.ntbQuantity = new LSOne.Controls.NumericTextBox();
            this.cmbUnit = new LSOne.Controls.DualDataComboBox();
            this.cmbVariantNumber = new LSOne.Controls.DualDataComboBox();
            this.cmbBarCodeSetup = new LSOne.Controls.DualDataComboBox();
            this.label2 = new System.Windows.Forms.Label();
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
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.chkToBePrinted);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.chkScanning);
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
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // chkToBePrinted
            // 
            resources.ApplyResources(this.chkToBePrinted, "chkToBePrinted");
            this.chkToBePrinted.Name = "chkToBePrinted";
            this.chkToBePrinted.UseVisualStyleBackColor = true;
            this.chkToBePrinted.CheckedChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // chkScanning
            // 
            resources.ApplyResources(this.chkScanning, "chkScanning");
            this.chkScanning.Name = "chkScanning";
            this.chkScanning.UseVisualStyleBackColor = true;
            this.chkScanning.CheckedChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // chkShowForItem
            // 
            resources.ApplyResources(this.chkShowForItem, "chkShowForItem");
            this.chkShowForItem.Name = "chkShowForItem";
            this.chkShowForItem.UseVisualStyleBackColor = true;
            this.chkShowForItem.CheckedChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // tbBarCode
            // 
            resources.ApplyResources(this.tbBarCode, "tbBarCode");
            this.tbBarCode.Name = "tbBarCode";
            this.tbBarCode.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // btnAddUnit
            // 
            this.btnAddUnit.BackColor = System.Drawing.Color.Transparent;
            this.btnAddUnit.Context = ButtonType.Add;
            resources.ApplyResources(this.btnAddUnit, "btnAddUnit");
            this.btnAddUnit.Name = "btnAddUnit";
            this.btnAddUnit.Click += new System.EventHandler(this.btnAddUnit_Click);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // ntbQuantity
            // 
            this.ntbQuantity.AllowDecimal = false;
            this.ntbQuantity.AllowNegative = false;
            this.ntbQuantity.CultureInfo = null;
            this.ntbQuantity.DecimalLetters = 2;
            this.ntbQuantity.ForeColor = System.Drawing.Color.Black;
            this.ntbQuantity.HasMinValue = false;
            resources.ApplyResources(this.ntbQuantity, "ntbQuantity");
            this.ntbQuantity.MaxValue = 99999D;
            this.ntbQuantity.MinValue = 0D;
            this.ntbQuantity.Name = "ntbQuantity";
            this.ntbQuantity.Value = 0D;
            this.ntbQuantity.TextChanged += new System.EventHandler(this.CheckEnabled);
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
            this.cmbUnit.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            this.cmbUnit.RequestClear += new System.EventHandler(this.cmbUnit_RequestClear);
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
            this.cmbVariantNumber.RequestClear += new System.EventHandler(this.cmbVariantNumber_RequestClear);
            // 
            // cmbBarCodeSetup
            // 
            this.cmbBarCodeSetup.AddList = null;
            this.cmbBarCodeSetup.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbBarCodeSetup, "cmbBarCodeSetup");
            this.cmbBarCodeSetup.MaxLength = 32767;
            this.cmbBarCodeSetup.Name = "cmbBarCodeSetup";
            this.cmbBarCodeSetup.NoChangeAllowed = false;
            this.cmbBarCodeSetup.OnlyDisplayID = false;
            this.cmbBarCodeSetup.RemoveList = null;
            this.cmbBarCodeSetup.RowHeight = ((short)(22));
            this.cmbBarCodeSetup.SecondaryData = null;
            this.cmbBarCodeSetup.SelectedData = null;
            this.cmbBarCodeSetup.SelectedDataID = null;
            this.cmbBarCodeSetup.SelectionList = null;
            this.cmbBarCodeSetup.SkipIDColumn = true;
            this.cmbBarCodeSetup.RequestData += new System.EventHandler(this.cmbBarCodeSetup_RequestData);
            this.cmbBarCodeSetup.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // BarCodeDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ntbQuantity);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnAddUnit);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chkShowForItem);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbVariantNumber);
            this.Controls.Add(this.cmbBarCodeSetup);
            this.Controls.Add(this.tbBarCode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "BarCodeDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbBarCode, 0);
            this.Controls.SetChildIndex(this.cmbBarCodeSetup, 0);
            this.Controls.SetChildIndex(this.cmbVariantNumber, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbUnit, 0);
            this.Controls.SetChildIndex(this.chkShowForItem, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.btnAddUnit, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.ntbQuantity, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbBarCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DualDataComboBox cmbVariantNumber;
        private DualDataComboBox cmbBarCodeSetup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkShowForItem;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkToBePrinted;
        private System.Windows.Forms.CheckBox chkScanning;
        private System.Windows.Forms.Label label8;
        private DualDataComboBox cmbUnit;
        private ContextButton btnAddUnit;
        private System.Windows.Forms.Label label9;
        private NumericTextBox ntbQuantity;
        private System.Windows.Forms.Label label2;
    }
}