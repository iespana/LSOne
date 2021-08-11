using LSOne.Controls;

namespace LSOne.ViewPlugins.Customer.Dialogs
{
    partial class NewCustomerDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewCustomerDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbDefaultCustomer = new LSOne.Controls.DropDownFormComboBox(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkCreateAnother = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tbDisplayName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.linkFields1 = new LSOne.Controls.LinkFields();
            this.lblID = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.tbIdentificationNumber = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.addressField = new LSOne.Controls.AddressControl();
            this.fullNameControl = new LSOne.Controls.FullNameControl();
            this.cmbInvoicedCustomer = new LSOne.Controls.DualDataComboBox();
            this.tbSearchAlias = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbSalesTaxGroup = new LSOne.Controls.DualDataComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbDefaultCustomer
            // 
            this.cmbDefaultCustomer.AddList = null;
            this.cmbDefaultCustomer.EnableTextBox = true;
            resources.ApplyResources(this.cmbDefaultCustomer, "cmbDefaultCustomer");
            this.cmbDefaultCustomer.MaxLength = 32767;
            this.cmbDefaultCustomer.Name = "cmbDefaultCustomer";
            this.cmbDefaultCustomer.NoChangeAllowed = false;
            this.cmbDefaultCustomer.RemoveList = null;
            this.cmbDefaultCustomer.RowHeight = ((short)(22));
            this.cmbDefaultCustomer.SecondaryData = null;
            this.cmbDefaultCustomer.SelectedData = null;
            this.cmbDefaultCustomer.SelectionList = null;
            this.cmbDefaultCustomer.ShowDropDownOnTyping = true;
            this.cmbDefaultCustomer.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbDefaultCustomer_DropDown);
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
            // tbDisplayName
            // 
            this.tbDisplayName.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.tbDisplayName, "tbDisplayName");
            this.tbDisplayName.Name = "tbDisplayName";
            this.tbDisplayName.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // linkFields1
            // 
            this.linkFields1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields1, "linkFields1");
            this.linkFields1.Name = "linkFields1";
            this.linkFields1.TabStop = false;
            // 
            // lblID
            // 
            resources.ApplyResources(this.lblID, "lblID");
            this.lblID.Name = "lblID";
            // 
            // tbID
            // 
            resources.ApplyResources(this.tbID, "tbID");
            this.tbID.Name = "tbID";
            this.tbID.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // tbIdentificationNumber
            // 
            resources.ApplyResources(this.tbIdentificationNumber, "tbIdentificationNumber");
            this.tbIdentificationNumber.Name = "tbIdentificationNumber";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // addressField
            // 
            this.addressField.BackColor = System.Drawing.Color.Transparent;
            this.addressField.DataModel = null;
            resources.ApplyResources(this.addressField, "addressField");
            this.addressField.Name = "addressField";
            // 
            // fullNameControl
            // 
            this.fullNameControl.Alias = "";
            resources.ApplyResources(this.fullNameControl, "fullNameControl");
            this.fullNameControl.BackColor = System.Drawing.Color.Transparent;
            this.fullNameControl.FirstName = "";
            this.fullNameControl.LastName = "";
            this.fullNameControl.MiddleName = "";
            this.fullNameControl.Name = "fullNameControl";
            this.fullNameControl.Prefix = "";
            this.fullNameControl.ShowAlias = false;
            this.fullNameControl.Suffix = "";
            this.fullNameControl.ValueChanged += new System.EventHandler(this.fullNameControl_ValueChanged);
            // 
            // cmbInvoicedCustomer
            // 
            this.cmbInvoicedCustomer.AddList = null;
            this.cmbInvoicedCustomer.AllowKeyboardSelection = false;
            this.cmbInvoicedCustomer.EnableTextBox = true;
            resources.ApplyResources(this.cmbInvoicedCustomer, "cmbInvoicedCustomer");
            this.cmbInvoicedCustomer.MaxLength = 32767;
            this.cmbInvoicedCustomer.Name = "cmbInvoicedCustomer";
            this.cmbInvoicedCustomer.NoChangeAllowed = false;
            this.cmbInvoicedCustomer.OnlyDisplayID = false;
            this.cmbInvoicedCustomer.RemoveList = null;
            this.cmbInvoicedCustomer.RowHeight = ((short)(22));
            this.cmbInvoicedCustomer.SecondaryData = null;
            this.cmbInvoicedCustomer.SelectedData = null;
            this.cmbInvoicedCustomer.SelectedDataID = null;
            this.cmbInvoicedCustomer.SelectionList = null;
            this.cmbInvoicedCustomer.ShowDropDownOnTyping = true;
            this.cmbInvoicedCustomer.SkipIDColumn = true;
            this.cmbInvoicedCustomer.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbInvoicedCustomer_DropDown);
            this.cmbInvoicedCustomer.RequestClear += new System.EventHandler(this.cmbInvoicedCustomer_RequestClear);
            // 
            // tbSearchAlias
            // 
            resources.ApplyResources(this.tbSearchAlias, "tbSearchAlias");
            this.tbSearchAlias.Name = "tbSearchAlias";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
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
            this.cmbSalesTaxGroup.RequestClear += new System.EventHandler(this.cmbSalesTaxGroup_RequestClear);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // NewCustomerDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbSalesTaxGroup);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbSearchAlias);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbInvoicedCustomer);
            this.Controls.Add(this.tbIdentificationNumber);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.linkFields1);
            this.Controls.Add(this.tbDisplayName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbDefaultCustomer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addressField);
            this.Controls.Add(this.fullNameControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "NewCustomerDialog";
            this.Controls.SetChildIndex(this.fullNameControl, 0);
            this.Controls.SetChildIndex(this.addressField, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbDefaultCustomer, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.tbDisplayName, 0);
            this.Controls.SetChildIndex(this.linkFields1, 0);
            this.Controls.SetChildIndex(this.tbID, 0);
            this.Controls.SetChildIndex(this.lblID, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.tbIdentificationNumber, 0);
            this.Controls.SetChildIndex(this.cmbInvoicedCustomer, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbSearchAlias, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.cmbSalesTaxGroup, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AddressControl addressField;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private DropDownFormComboBox cmbDefaultCustomer;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private FullNameControl fullNameControl;
        private System.Windows.Forms.TextBox tbDisplayName;
        private System.Windows.Forms.Label label2;
        private LinkFields linkFields1;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.TextBox tbIdentificationNumber;
        private System.Windows.Forms.Label label4;
        private DualDataComboBox cmbInvoicedCustomer;
        private System.Windows.Forms.TextBox tbSearchAlias;
        private System.Windows.Forms.Label label3;
        private DualDataComboBox cmbSalesTaxGroup;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkCreateAnother;
    }
}