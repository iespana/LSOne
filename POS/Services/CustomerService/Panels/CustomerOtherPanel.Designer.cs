using LSOne.Controls;
using LSOne.POS.Processes.WinControls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Panels
{
    partial class CustomerOtherPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerOtherPanel));
            this.lblGender = new System.Windows.Forms.Label();
            this.cmbGender = new LSOne.Controls.DualDataComboBox();
            this.lblDateOfBirth = new System.Windows.Forms.Label();
            this.chkIsCashCustomer = new LSOne.Controls.TouchCheckBox();
            this.lblSalesTaxGroup = new System.Windows.Forms.Label();
            this.cmbSalesTaxGroup = new LSOne.Controls.DualDataComboBox();
            this.cmbInvoiceCustomer = new LSOne.Controls.DualDataComboBox();
            this.lblInvoiceCustomer = new System.Windows.Forms.Label();
            this.cmbBlocking = new LSOne.Controls.DualDataComboBox();
            this.lblBlocking = new System.Windows.Forms.Label();
            this.dtDateOfBirth = new LSOne.Controls.Dialogs.DatePickerTouch();
            this.touchErrorProvider = new LSOne.Controls.Dialogs.TouchErrorProvider();
            this.SuspendLayout();
            // 
            // lblGender
            // 
            resources.ApplyResources(this.lblGender, "lblGender");
            this.lblGender.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblGender.Name = "lblGender";
            // 
            // cmbGender
            // 
            this.cmbGender.AddList = null;
            this.cmbGender.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbGender, "cmbGender");
            this.cmbGender.EnableTextBox = true;
            this.cmbGender.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbGender.IsPOSControl = true;
            this.cmbGender.MaxLength = 32767;
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.NoChangeAllowed = false;
            this.cmbGender.OnlyDisplayID = false;
            this.cmbGender.ReadOnly = true;
            this.cmbGender.RemoveList = null;
            this.cmbGender.RowHeight = ((short)(22));
            this.cmbGender.SecondaryData = null;
            this.cmbGender.SelectedData = null;
            this.cmbGender.SelectedDataID = null;
            this.cmbGender.SelectionList = null;
            this.cmbGender.ShowDropDownOnTyping = true;
            this.cmbGender.SkipIDColumn = true;
            this.cmbGender.Touch = true;
            this.cmbGender.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbGender_DropDown);
            this.cmbGender.SelectedDataChanged += new System.EventHandler(this.ClearErrorProvider);
            // 
            // lblDateOfBirth
            // 
            resources.ApplyResources(this.lblDateOfBirth, "lblDateOfBirth");
            this.lblDateOfBirth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDateOfBirth.Name = "lblDateOfBirth";
            // 
            // chkIsCashCustomer
            // 
            resources.ApplyResources(this.chkIsCashCustomer, "chkIsCashCustomer");
            this.chkIsCashCustomer.BackColor = System.Drawing.Color.Transparent;
            this.chkIsCashCustomer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.chkIsCashCustomer.Name = "chkIsCashCustomer";
            // 
            // lblSalesTaxGroup
            // 
            resources.ApplyResources(this.lblSalesTaxGroup, "lblSalesTaxGroup");
            this.lblSalesTaxGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblSalesTaxGroup.Name = "lblSalesTaxGroup";
            // 
            // cmbSalesTaxGroup
            // 
            this.cmbSalesTaxGroup.AddList = null;
            this.cmbSalesTaxGroup.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbSalesTaxGroup, "cmbSalesTaxGroup");
            this.cmbSalesTaxGroup.EnableTextBox = true;
            this.cmbSalesTaxGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbSalesTaxGroup.IsPOSControl = true;
            this.cmbSalesTaxGroup.MaxLength = 32767;
            this.cmbSalesTaxGroup.Name = "cmbSalesTaxGroup";
            this.cmbSalesTaxGroup.NoChangeAllowed = false;
            this.cmbSalesTaxGroup.OnlyDisplayID = false;
            this.cmbSalesTaxGroup.ReadOnly = true;
            this.cmbSalesTaxGroup.RemoveList = null;
            this.cmbSalesTaxGroup.RowHeight = ((short)(22));
            this.cmbSalesTaxGroup.SecondaryData = null;
            this.cmbSalesTaxGroup.SelectedData = null;
            this.cmbSalesTaxGroup.SelectedDataID = null;
            this.cmbSalesTaxGroup.SelectionList = null;
            this.cmbSalesTaxGroup.ShowDropDownOnTyping = true;
            this.cmbSalesTaxGroup.SkipIDColumn = true;
            this.cmbSalesTaxGroup.Touch = true;
            this.cmbSalesTaxGroup.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbSalesTaxGroup_DropDown);
            // 
            // cmbInvoiceCustomer
            // 
            this.cmbInvoiceCustomer.AddList = null;
            this.cmbInvoiceCustomer.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbInvoiceCustomer, "cmbInvoiceCustomer");
            this.cmbInvoiceCustomer.EnableTextBox = true;
            this.cmbInvoiceCustomer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbInvoiceCustomer.IsPOSControl = true;
            this.cmbInvoiceCustomer.MaxLength = 32767;
            this.cmbInvoiceCustomer.Name = "cmbInvoiceCustomer";
            this.cmbInvoiceCustomer.NoChangeAllowed = false;
            this.cmbInvoiceCustomer.OnlyDisplayID = false;
            this.cmbInvoiceCustomer.ReadOnly = true;
            this.cmbInvoiceCustomer.RemoveList = null;
            this.cmbInvoiceCustomer.RowHeight = ((short)(22));
            this.cmbInvoiceCustomer.SecondaryData = null;
            this.cmbInvoiceCustomer.SelectedData = null;
            this.cmbInvoiceCustomer.SelectedDataID = null;
            this.cmbInvoiceCustomer.SelectionList = null;
            this.cmbInvoiceCustomer.ShowDropDownOnTyping = true;
            this.cmbInvoiceCustomer.SkipIDColumn = true;
            this.cmbInvoiceCustomer.Touch = true;
            this.cmbInvoiceCustomer.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbInvoiceCustomer_DropDown);
            this.cmbInvoiceCustomer.RequestClear += new System.EventHandler(this.cmbInvoiceCustomer_RequestClear);
            // 
            // lblInvoiceCustomer
            // 
            resources.ApplyResources(this.lblInvoiceCustomer, "lblInvoiceCustomer");
            this.lblInvoiceCustomer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblInvoiceCustomer.Name = "lblInvoiceCustomer";
            // 
            // cmbBlocking
            // 
            this.cmbBlocking.AddList = null;
            this.cmbBlocking.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbBlocking, "cmbBlocking");
            this.cmbBlocking.EnableTextBox = true;
            this.cmbBlocking.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbBlocking.IsPOSControl = true;
            this.cmbBlocking.MaxLength = 32767;
            this.cmbBlocking.Name = "cmbBlocking";
            this.cmbBlocking.NoChangeAllowed = false;
            this.cmbBlocking.OnlyDisplayID = false;
            this.cmbBlocking.ReadOnly = true;
            this.cmbBlocking.RemoveList = null;
            this.cmbBlocking.RowHeight = ((short)(22));
            this.cmbBlocking.SecondaryData = null;
            this.cmbBlocking.SelectedData = null;
            this.cmbBlocking.SelectedDataID = null;
            this.cmbBlocking.SelectionList = null;
            this.cmbBlocking.ShowDropDownOnTyping = true;
            this.cmbBlocking.SkipIDColumn = true;
            this.cmbBlocking.Touch = true;
            this.cmbBlocking.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbBlocking_DropDown);
            // 
            // lblBlocking
            // 
            resources.ApplyResources(this.lblBlocking, "lblBlocking");
            this.lblBlocking.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblBlocking.Name = "lblBlocking";
            // 
            // dtDateOfBirth
            // 
            resources.ApplyResources(this.dtDateOfBirth, "dtDateOfBirth");
            this.dtDateOfBirth.BackColor = System.Drawing.Color.White;
            this.dtDateOfBirth.Checked = false;
            this.dtDateOfBirth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.dtDateOfBirth.MaxDate = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
            this.dtDateOfBirth.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.dtDateOfBirth.Name = "dtDateOfBirth";
            this.dtDateOfBirth.ShowEmbeddedCheckBox = true;
            this.dtDateOfBirth.ValueChanged += new System.EventHandler(this.ClearErrorProvider);
            // 
            // touchErrorProvider
            // 
            resources.ApplyResources(this.touchErrorProvider, "touchErrorProvider");
            this.touchErrorProvider.BackColor = System.Drawing.Color.White;
            this.touchErrorProvider.Name = "touchErrorProvider";
            // 
            // CustomerOtherPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblGender);
            this.Controls.Add(this.cmbGender);
            this.Controls.Add(this.lblDateOfBirth);
            this.Controls.Add(this.dtDateOfBirth);
            this.Controls.Add(this.chkIsCashCustomer);
            this.Controls.Add(this.lblSalesTaxGroup);
            this.Controls.Add(this.touchErrorProvider);
            this.Controls.Add(this.cmbSalesTaxGroup);
            this.Controls.Add(this.lblBlocking);
            this.Controls.Add(this.cmbBlocking);
            this.Controls.Add(this.lblInvoiceCustomer);
            this.Controls.Add(this.cmbInvoiceCustomer);
            this.DoubleBuffered = true;
            this.Name = "CustomerOtherPanel";
            this.Load += new System.EventHandler(this.CustomerContactInfoPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private LSOne.Controls.DualDataComboBox cmbGender;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.Label lblDateOfBirth;
        private LSOne.Controls.Dialogs.DatePickerTouch dtDateOfBirth;
        private TouchCheckBox chkIsCashCustomer;
        private System.Windows.Forms.Label lblSalesTaxGroup;
        private DualDataComboBox cmbSalesTaxGroup;
        private Controls.Dialogs.TouchErrorProvider touchErrorProvider;
        private LSOne.Controls.DualDataComboBox cmbBlocking;
        private System.Windows.Forms.Label lblBlocking;
        private DualDataComboBox cmbInvoiceCustomer;
        private System.Windows.Forms.Label lblInvoiceCustomer;
    }
}
