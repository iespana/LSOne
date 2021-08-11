using System;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Delegates;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Panels;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.WinFormsTouch
{
    public partial class AddCustomerDialog : TouchBaseForm
    {
        public Customer NewCustomer { get; set; }
        private ShowKeyboardInputHandler keyboardHandler;

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;

        CustomerDetailsPanel customerDetailsPanel;
        CustomerOtherPanel customerOtherPanel;
        CustomerAddressPanel customerAddressPanel;
        Control activePanel;

        public AddCustomerDialog(IConnectionManager entry, ShowKeyboardInputHandler keyboardHandler)
        {
            this.keyboardHandler = keyboardHandler;
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            customerDetailsPanel = new CustomerDetailsPanel(entry);
            customerDetailsPanel.Location = new Point(0, 0);
            customerDetailsPanel.Dock = DockStyle.Fill;

            panel1.Controls.Add(customerDetailsPanel);

            activePanel = customerDetailsPanel;
        }

        void customerDescriptionPanel_ContentStateChanged(object sender, EventArgs e)
        {
            btnNext.Enabled = customerDetailsPanel.NextEnabled;
        }

        protected override void OnLoad(EventArgs e)
        {
            customerDetailsPanel.Visible = true;
         
            base.OnLoad(e);

            if (customerDetailsPanel != null)
            {
                customerDetailsPanel.Select();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            
            btnNext.Text = Properties.Resources.Next;

            if (activePanel == customerOtherPanel)
            {
                customerOtherPanel.CustomerErrorProviderVisible = false;
                activePanel = customerAddressPanel;

                customerOtherPanel.Visible = false;
                customerAddressPanel.Visible = true;

                customerAddressPanel.Select();

                activePageIndicator.ActivePageIndex = 1;
            }
            else
            {
                btnBack.Enabled = false;

                activePanel = customerDetailsPanel;

                customerAddressPanel.Visible = false;
                customerDetailsPanel.Visible = true;

                customerDetailsPanel.Select();

                activePageIndicator.ActivePageIndex = 0;
            }            
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (activePanel == customerDetailsPanel)
            {
                if (customerAddressPanel == null)
                {
                    customerAddressPanel = new CustomerAddressPanel(dlgEntry);

                    customerAddressPanel.Location = new Point(0, 0);
                    customerAddressPanel.Dock = DockStyle.Fill;

                    panel1.Controls.Add(customerAddressPanel);
                }               

                activePanel = customerAddressPanel;

                customerDetailsPanel.Visible = false;
                customerAddressPanel.Visible = true;

                btnBack.Enabled = true;

                customerAddressPanel.Select();
                customerAddressPanel.Focus();

                activePageIndicator.ActivePageIndex = 1;
            }
            else if (activePanel == customerAddressPanel)
            {
                if (customerOtherPanel == null)
                {
                    customerOtherPanel = new CustomerOtherPanel(dlgEntry);

                    customerOtherPanel.Location = new Point(0, 0);
                    customerOtherPanel.Dock = DockStyle.Fill;

                    panel1.Controls.Add(customerOtherPanel);
                }

                activePanel = customerOtherPanel;

                customerAddressPanel.Visible = false;
                customerOtherPanel.Visible = true;

                btnBack.Enabled = true;
                btnNext.Text = Properties.Resources.Finish;

                customerOtherPanel.Select();
                customerOtherPanel.Focus();

                activePageIndicator.ActivePageIndex = 2;
            }
            else
            {
                // Finish button is clicked

                customerOtherPanel.CustomerErrorProviderVisible = false;

                // Need to validate each to set appropriate error providers and messages
                bool descriptionPanelValid = customerDetailsPanel.ValidateData();
                bool addressPanelValid = customerAddressPanel.ValidateData();
                bool contactInfoValid = customerOtherPanel.ValidateData();
                                
                if (descriptionPanelValid && addressPanelValid && contactInfoValid)
                {
                    var address = new CustomerAddress {Address = customerAddressPanel.AddressRecord};
                    address.Address.AddressType = Address.AddressTypes.Shipping;
                    address.IsDefault = true;
                    NewCustomer = new Customer();
                    NewCustomer.Addresses.Add(address);
                    NewCustomer.FirstName = customerDetailsPanel.FirstName;
                    NewCustomer.LastName = customerDetailsPanel.LastName;
                    NewCustomer.Prefix = customerDetailsPanel.Title;
                    NewCustomer.Text = customerDetailsPanel.DisplayName;

                    NewCustomer.Email = customerDetailsPanel.Email;
                    NewCustomer.ReceiptEmailAddress = customerDetailsPanel.ReceiptEmailAddress;
                    NewCustomer.Telephone = customerDetailsPanel.Phone;
                    NewCustomer.NonChargableAccount = customerOtherPanel.IsCashCustomer;
                    NewCustomer.Gender = customerOtherPanel.Gender;
                    NewCustomer.Blocked = customerOtherPanel.Blocked;
                    NewCustomer.SearchName = customerDetailsPanel.SearchAlias;
                    NewCustomer.DateOfBirth = customerOtherPanel.DateOfBirth;
                    
                    NewCustomer.ReceiptSettings = customerDetailsPanel.ReceiptOption;
                    
                    NewCustomer.ID = RecordIdentifier.Empty;
                                
                    if (dlgSettings.SiteServiceProfile.AllowCustomerManualID && customerDetailsPanel.CustomerID != "")
                    {
                        NewCustomer.ID = customerDetailsPanel.CustomerID;
                    }

                    NewCustomer.TaxGroup = customerOtherPanel.SalesTaxGroupID;
                    NewCustomer.AccountNumber = customerOtherPanel.InvoiceCustomerID;
                    NewCustomer.MaxCredit = dlgSettings.SiteServiceProfile.DefaultCreditLimit;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    if(contactInfoValid)
                    {
                        customerOtherPanel.CustomerErrorProviderVisible = true;
                        customerOtherPanel.CustomerErrorProviderText = Properties.Resources.CustomerCouldNotBeSaved;
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void touchKeyboard_ObtainCultureName(object sender, CultureEventArguments args)
        {
            if (dlgSettings.UserProfile.KeyboardCode != "")
            {
                args.CultureName = dlgSettings.UserProfile.KeyboardCode;
                args.LayoutName = dlgSettings.UserProfile.KeyboardLayoutName;
            }
            else
            {
                args.CultureName = dlgSettings.Store.KeyboardCode;
                args.LayoutName = dlgSettings.Store.KeyboardLayoutName;
            }
        }
    }
}
