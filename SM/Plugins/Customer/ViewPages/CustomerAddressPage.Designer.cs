using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.Customer.ViewPages
{
    partial class CustomerAddressPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerAddressPage));
            this.lvAddresses = new LSOne.Controls.ListView();
            this.colDefault = new LSOne.Controls.Columns.Column();
            this.colType = new LSOne.Controls.Columns.Column();
            this.colContact = new LSOne.Controls.Columns.Column();
            this.colPhone = new LSOne.Controls.Columns.Column();
            this.colMobile = new LSOne.Controls.Columns.Column();
            this.colEmail = new LSOne.Controls.Columns.Column();
            this.colSalesTax = new LSOne.Controls.Columns.Column();
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.addressControl = new LSOne.Controls.AddressControl();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvAddresses
            // 
            resources.ApplyResources(this.lvAddresses, "lvAddresses");
            this.lvAddresses.BackColor = System.Drawing.Color.Transparent;
            this.lvAddresses.BuddyControl = null;
            this.lvAddresses.Columns.Add(this.colDefault);
            this.lvAddresses.Columns.Add(this.colType);
            this.lvAddresses.Columns.Add(this.colContact);
            this.lvAddresses.Columns.Add(this.colPhone);
            this.lvAddresses.Columns.Add(this.colMobile);
            this.lvAddresses.Columns.Add(this.colEmail);
            this.lvAddresses.Columns.Add(this.colSalesTax);
            this.lvAddresses.ContentBackColor = System.Drawing.Color.White;
            this.lvAddresses.DefaultRowHeight = ((short)(18));
            this.lvAddresses.EvenRowColor = System.Drawing.Color.White;
            this.lvAddresses.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvAddresses.HeaderHeight = ((short)(20));
            this.lvAddresses.HorizontalScrollbar = true;
            this.lvAddresses.Name = "lvAddresses";
            this.lvAddresses.OddRowColor = System.Drawing.Color.White;
            this.lvAddresses.RowLineColor = System.Drawing.Color.LightGray;
            this.lvAddresses.SecondarySortColumn = ((short)(-1));
            this.lvAddresses.SortSetting = "1:1";
            this.lvAddresses.SelectionChanged += new System.EventHandler(this.lvAddresses_SelectionChanged);
            this.lvAddresses.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvAddresses_RowDoubleClick);
            // 
            // colDefault
            // 
            this.colDefault.AutoSize = true;
            this.colDefault.DefaultStyle = null;
            resources.ApplyResources(this.colDefault, "colDefault");
            this.colDefault.MaximumWidth = ((short)(0));
            this.colDefault.MinimumWidth = ((short)(10));
            this.colDefault.SecondarySortColumn = ((short)(-1));
            this.colDefault.Tag = null;
            this.colDefault.Width = ((short)(50));
            // 
            // colType
            // 
            this.colType.AutoSize = true;
            this.colType.Clickable = false;
            this.colType.DefaultStyle = null;
            resources.ApplyResources(this.colType, "colType");
            this.colType.MaximumWidth = ((short)(0));
            this.colType.MinimumWidth = ((short)(10));
            this.colType.SecondarySortColumn = ((short)(-1));
            this.colType.Tag = null;
            this.colType.Width = ((short)(100));
            // 
            // colContact
            // 
            this.colContact.AutoSize = true;
            this.colContact.DefaultStyle = null;
            resources.ApplyResources(this.colContact, "colContact");
            this.colContact.MaximumWidth = ((short)(0));
            this.colContact.MinimumWidth = ((short)(10));
            this.colContact.SecondarySortColumn = ((short)(-1));
            this.colContact.Tag = null;
            this.colContact.Width = ((short)(150));
            // 
            // colPhone
            // 
            this.colPhone.AutoSize = true;
            this.colPhone.Clickable = false;
            this.colPhone.DefaultStyle = null;
            resources.ApplyResources(this.colPhone, "colPhone");
            this.colPhone.MaximumWidth = ((short)(0));
            this.colPhone.MinimumWidth = ((short)(10));
            this.colPhone.SecondarySortColumn = ((short)(-1));
            this.colPhone.Tag = null;
            this.colPhone.Width = ((short)(100));
            // 
            // colMobile
            // 
            this.colMobile.AutoSize = true;
            this.colMobile.Clickable = false;
            this.colMobile.DefaultStyle = null;
            resources.ApplyResources(this.colMobile, "colMobile");
            this.colMobile.MaximumWidth = ((short)(0));
            this.colMobile.MinimumWidth = ((short)(10));
            this.colMobile.SecondarySortColumn = ((short)(-1));
            this.colMobile.Tag = null;
            this.colMobile.Width = ((short)(100));
            // 
            // colEmail
            // 
            this.colEmail.AutoSize = true;
            this.colEmail.Clickable = false;
            this.colEmail.DefaultStyle = null;
            resources.ApplyResources(this.colEmail, "colEmail");
            this.colEmail.MaximumWidth = ((short)(0));
            this.colEmail.MinimumWidth = ((short)(10));
            this.colEmail.SecondarySortColumn = ((short)(-1));
            this.colEmail.Tag = null;
            this.colEmail.Width = ((short)(100));
            // 
            // colSalesTax
            // 
            this.colSalesTax.AutoSize = true;
            this.colSalesTax.DefaultStyle = null;
            resources.ApplyResources(this.colSalesTax, "colSalesTax");
            this.colSalesTax.MaximumWidth = ((short)(0));
            this.colSalesTax.MinimumWidth = ((short)(10));
            this.colSalesTax.SecondarySortColumn = ((short)(-1));
            this.colSalesTax.Tag = null;
            this.colSalesTax.Width = ((short)(50));
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // addressControl
            // 
            resources.ApplyResources(this.addressControl, "addressControl");
            this.addressControl.BackColor = System.Drawing.Color.Transparent;
            this.addressControl.DataModel = null;
            this.addressControl.Name = "addressControl";
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // CustomerAddressPage
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.addressControl);
            this.Controls.Add(this.btnsContextButtons);
            this.Controls.Add(this.lvAddresses);
            this.Controls.Add(this.lblNoSelection);
            this.Name = "CustomerAddressPage";
            this.ResumeLayout(false);

        }

        #endregion
        private ListView lvAddresses;
        private Column colType;
        private ContextButtons btnsContextButtons;
        private Column colPhone;
        private Column colMobile;
        private Column colEmail;
        private Column colContact;
        private Column colSalesTax;
        private AddressControl addressControl;
        private Column colDefault;
        private System.Windows.Forms.Label lblNoSelection;
    }
}
