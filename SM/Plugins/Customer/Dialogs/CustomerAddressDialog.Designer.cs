using LSOne.Controls;

namespace LSOne.ViewPlugins.Customer.Dialogs
{
    partial class CustomerAddressDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerAddressDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblAddressType = new System.Windows.Forms.Label();
            this.addressField = new LSOne.Controls.AddressControl();
            this.tbPhone = new System.Windows.Forms.TextBox();
            this.lblContact = new System.Windows.Forms.Label();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.lblEmailAddress = new System.Windows.Forms.Label();
            this.tbMobilePhone = new System.Windows.Forms.TextBox();
            this.lblMobilePhone = new System.Windows.Forms.Label();
            this.cmAddressType = new System.Windows.Forms.ComboBox();
            this.lbTaxGroup = new System.Windows.Forms.Label();
            this.cmbSalesTaxGroup = new LSOne.Controls.DualDataComboBox();
            this.tbContact = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
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
            // lblAddressType
            // 
            resources.ApplyResources(this.lblAddressType, "lblAddressType");
            this.lblAddressType.Name = "lblAddressType";
            // 
            // addressField
            // 
            this.addressField.BackColor = System.Drawing.Color.Transparent;
            this.addressField.DataModel = null;
            resources.ApplyResources(this.addressField, "addressField");
            this.addressField.Name = "addressField";
            this.addressField.ValueChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // tbPhone
            // 
            resources.ApplyResources(this.tbPhone, "tbPhone");
            this.tbPhone.Name = "tbPhone";
            this.tbPhone.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblContact
            // 
            this.lblContact.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblContact, "lblContact");
            this.lblContact.Name = "lblContact";
            // 
            // tbEmail
            // 
            resources.ApplyResources(this.tbEmail, "tbEmail");
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblEmailAddress
            // 
            this.lblEmailAddress.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblEmailAddress, "lblEmailAddress");
            this.lblEmailAddress.Name = "lblEmailAddress";
            // 
            // tbMobilePhone
            // 
            resources.ApplyResources(this.tbMobilePhone, "tbMobilePhone");
            this.tbMobilePhone.Name = "tbMobilePhone";
            this.tbMobilePhone.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblMobilePhone
            // 
            this.lblMobilePhone.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblMobilePhone, "lblMobilePhone");
            this.lblMobilePhone.Name = "lblMobilePhone";
            // 
            // cmAddressType
            // 
            this.cmAddressType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmAddressType.FormattingEnabled = true;
            resources.ApplyResources(this.cmAddressType, "cmAddressType");
            this.cmAddressType.Name = "cmAddressType";
            // 
            // lbTaxGroup
            // 
            this.lbTaxGroup.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lbTaxGroup, "lbTaxGroup");
            this.lbTaxGroup.Name = "lbTaxGroup";
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
            this.cmbSalesTaxGroup.RequestClear += new System.EventHandler(this.ClearData);
            this.cmbSalesTaxGroup.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // tbContact
            // 
            resources.ApplyResources(this.tbContact, "tbContact");
            this.tbContact.Name = "tbContact";
            this.tbContact.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblPhone
            // 
            this.lblPhone.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblPhone, "lblPhone");
            this.lblPhone.Name = "lblPhone";
            // 
            // CustomerAddressDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tbContact);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.cmbSalesTaxGroup);
            this.Controls.Add(this.lbTaxGroup);
            this.Controls.Add(this.cmAddressType);
            this.Controls.Add(this.tbPhone);
            this.Controls.Add(this.lblContact);
            this.Controls.Add(this.tbEmail);
            this.Controls.Add(this.lblEmailAddress);
            this.Controls.Add(this.tbMobilePhone);
            this.Controls.Add(this.lblMobilePhone);
            this.Controls.Add(this.lblAddressType);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.addressField);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "CustomerAddressDialog";
            this.Controls.SetChildIndex(this.addressField, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblAddressType, 0);
            this.Controls.SetChildIndex(this.lblMobilePhone, 0);
            this.Controls.SetChildIndex(this.tbMobilePhone, 0);
            this.Controls.SetChildIndex(this.lblEmailAddress, 0);
            this.Controls.SetChildIndex(this.tbEmail, 0);
            this.Controls.SetChildIndex(this.lblContact, 0);
            this.Controls.SetChildIndex(this.tbPhone, 0);
            this.Controls.SetChildIndex(this.cmAddressType, 0);
            this.Controls.SetChildIndex(this.lbTaxGroup, 0);
            this.Controls.SetChildIndex(this.cmbSalesTaxGroup, 0);
            this.Controls.SetChildIndex(this.lblPhone, 0);
            this.Controls.SetChildIndex(this.tbContact, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AddressControl addressField;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblAddressType;
        private System.Windows.Forms.TextBox tbPhone;
        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.Label lblEmailAddress;
        private System.Windows.Forms.TextBox tbMobilePhone;
        private System.Windows.Forms.Label lblMobilePhone;
        private System.Windows.Forms.ComboBox cmAddressType;
        private System.Windows.Forms.Label lbTaxGroup;
        private DualDataComboBox cmbSalesTaxGroup;
        private System.Windows.Forms.TextBox tbContact;
        private System.Windows.Forms.Label lblPhone;
    }
}