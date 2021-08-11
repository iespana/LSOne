using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.Services.Panels
{
    public partial class CustomerDetailsPanel : UserControl
    {
        private IConnectionManager entry;
        private ISettings settings;
        private List<DataEntity> namePrefixes;
        private Customer customer;

        public bool NextEnabled { get; set; }

        public CustomerDetailsPanel(IConnectionManager entry)
        {
            NextEnabled = false;

            InitializeComponent();
            this.entry = entry;
            settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            namePrefixes = AddPrefixes(entry.Cache.GetNamePrefixes());

            tbID.Enabled = btnVerify.Enabled = settings.SiteServiceProfile.AllowCustomerManualID;
            cmbReceiptOption.SelectedData = new DataEntity((int)ReceiptSettingsEnum.Printed, ReceiptSettingsEnum.Printed.ToLocalizedString());

            SetStyle(ControlStyles.Selectable, true);

            DoubleBuffered = true;
        }

        //Used when editing
        public CustomerDetailsPanel(IConnectionManager entry, Customer customer) : this(entry)
        {
            namePrefixes = AddPrefixes(entry.Cache.GetNamePrefixes());

            this.customer = customer;
            btnVerify.Visible = false;
            tbID.Width = tbEmail.Width;
            tbID.Enabled = false;
            tbID.Text = customer.ID.StringValue;
            tbFirstName.Text = customer.FirstName;
            tbLastName.Text = customer.LastName;
            tbDisplayName.Text = customer.Text;
            tbEmail.Text = customer.Email;
            tbSearchAlias.Text = customer.SearchName;
            tbReceiptEmail.Text = customer.ReceiptEmailAddress;
            tbPhone.Text = customer.Telephone;
            cmbReceiptOption.SelectedData = new DataEntity((int)customer.ReceiptSettings, customer.ReceiptSettings.ToLocalizedString());
            cmbTitle.SelectedData = new DataEntity("", customer.Prefix);

            if (string.IsNullOrEmpty(tbFirstName.Text) && string.IsNullOrEmpty(tbLastName.Text))
            {
                tbDisplayName.Text = customer.Text;
            }
        }

        public string Email => tbEmail.Text;
        public string ReceiptEmailAddress => tbReceiptEmail.Text;
        public string Phone => tbPhone.Text;
        public string SearchAlias => tbSearchAlias.Text;
        public string Title => cmbTitle.Text;
        public string FirstName => tbFirstName.Text;
        public string LastName => tbLastName.Text;
        public string DisplayName => tbDisplayName.Text;

        public ReceiptSettingsEnum ReceiptOption => !RecordIdentifier.IsEmptyOrNull(cmbReceiptOption.SelectedDataID) ? (ReceiptSettingsEnum)(int)cmbReceiptOption.SelectedDataID : ReceiptSettingsEnum.Printed;

        public string CustomerID => tbID.Text.Trim();

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            EvaluateName();
        }

        public bool CustomerErrorProviderVisible
        {
            get { return touchErrorProvider.Visible; }
            set { touchErrorProvider.Visible = value; }
        }        

        public bool ValidateData()
        {
            bool reply = true;
            touchErrorProvider.Clear();
            
            if (settings.SiteServiceProfile.CustomerSearchAliasIsMandatory && tbSearchAlias.Text.Trim() == "")
            {
                reply = false;
                tbSearchAlias.HasError = true;
                touchErrorProvider.AddErrorMessage(Properties.Resources.SearchAliasMandatoryError);
            }

            if (settings.SiteServiceProfile.CustomerPhoneIsMandatory && tbPhone.Text.Trim() == "")
            {
                reply = false;
                tbPhone.HasError = true;
                touchErrorProvider.AddErrorMessage(Properties.Resources.PhoneMandatoryError);
            }

            if (settings.SiteServiceProfile.CustomerEmailIsMandatory && tbEmail.Text.Trim() == "")
            {
                reply = false;
                tbEmail.HasError = true;
                touchErrorProvider.AddErrorMessage(Properties.Resources.EmailMandatoryError);
            }

            if (settings.SiteServiceProfile.CustomerReceiptEmailIsMandatory && tbReceiptEmail.Text.Trim() == "")
            {
                reply = false;
                tbReceiptEmail.HasError = true;
                touchErrorProvider.AddErrorMessage(Properties.Resources.ReceiptEmailMandatoryError);
            }

            if (settings.SiteServiceProfile.CustomerNameIsMandatory)
            {
                bool isCustomerNameValid = (tbDisplayName.Enabled && tbDisplayName.Text.Trim() != "") || tbFirstName.Text.Length > 0 || tbLastName.Text.Length > 0;

                if (!isCustomerNameValid)
                {
                    reply = false;
                    tbDisplayName.HasError = true;
                    touchErrorProvider.AddErrorMessage(Properties.Resources.CustomerNameMandatoryError);
                }
            }

            if (customer == null && settings.SiteServiceProfile.AllowCustomerManualID && tbID.Text.Trim() != "")
            {
                reply =  reply && !ValidateCustomerID(true);
            }

            if (tbEmail.Text != "")
            {
                if (!tbEmail.Text.IsEmail())
                {
                    reply = false;
                    tbEmail.HasError = true;
                    touchErrorProvider.AddErrorMessage(Properties.Resources.EmailFieldDoesNotContainValidEmail);
                }
            }

            if (tbReceiptEmail.Text != "")
            {
                if (!tbReceiptEmail.Text.IsEmail())
                {
                    reply = false;
                    tbReceiptEmail.HasError = true;
                    touchErrorProvider.AddErrorMessage(Properties.Resources.ReceiptEmailFieldDoesNotContainValidEmail);
                }

            }

            touchErrorProvider.Visible = !reply;
            return reply;
        }

        private List<DataEntity> AddPrefixes(List<string> prefixeList)
        {
            List<DataEntity> prefixes = new List<DataEntity>();
            foreach (string value in prefixeList)
            {
                prefixes.Add(new DataEntity("", value));
            }
            return prefixes;
        }

        private void ClearErrorProvider(object sender, EventArgs e)
        {
            if(((Control)sender).Parent is ShadeTextBoxTouch)
            {
                ((ShadeTextBoxTouch)(((Control)sender).Parent)).HasError = false;
            }
        }

        private bool ValidateCustomerID(bool validatingFullForm)
        {
            bool customerExists = false;
            bool checkLocalDatabase = false;

            if (settings.SiteServiceProfile.SiteServiceAddress != "")
            {
                Exception exception;
                Action action =
                    () => customerExists = Interfaces.Services.SiteServiceService(entry).CustomerExists(entry, settings.SiteServiceProfile, tbID.Text, true, true);

                Interfaces.Services.DialogService(entry).ShowSpinnerDialog(
                    action,
                    Properties.Resources.VerifyingID,
                    "",
                    out exception);

                if (exception != null)
                {
                    checkLocalDatabase = true;
                }
            }

            if (checkLocalDatabase)
            {
                customerExists = Providers.CustomerData.Exists(entry, tbID.Text);
            }

            if(!validatingFullForm)
            {
                touchErrorProvider.Clear();
                touchErrorProvider.Visible = false;
            }

            if (customerExists)
            {
                tbID.HasError = true;
                touchErrorProvider.AddErrorMessage(Properties.Resources.CustomerIDAlreadyExists);
                touchErrorProvider.Visible = true;
            }
            else
            {
                tbID.HasError = false;
            }

            return customerExists;
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            ValidateCustomerID(false);
        }

        private string FormatName()
        {
            string result = "";

            if (cmbTitle.Text != "") result += cmbTitle.Text + " ";
            if (tbFirstName.Text != "") result += tbFirstName.Text + " ";
            if (tbLastName.Text != "") result += tbLastName.Text + " ";

            return result.Trim();
        }

        private void EvaluateName()
        {
            tbDisplayName.Enabled = true;

            if (!string.IsNullOrEmpty(tbFirstName.Text) || !string.IsNullOrEmpty(tbLastName.Text) || !string.IsNullOrEmpty(cmbTitle.Text))
            {
                tbDisplayName.Enabled = false;
                tbDisplayName.Text = FormatName();
            }
        }

        private void tbFirstName_TextChanged(object sender, EventArgs e)
        {
            EvaluateName();
        }

        private void tbLastName_TextChanged(object sender, EventArgs e)
        {
            EvaluateName();
        }

        private void cmbTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            EvaluateName();
        }

        private void cmbTitle_DropDown(object sender, DropDownEventArgs e)
        {
            DualDataPanel panelToEmbed = new DualDataPanel(
                "",
                namePrefixes,
                null,
                true,
                cmbTitle.SkipIDColumn,
                false,
                50,
                false);

            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }

        private void cmbReceiptOption_DropDown(object sender, DropDownEventArgs e)
        {
            DualDataPanel panelToEmbed = new DualDataPanel(
                "",
                GetReceiptOptions(),
                null,
                true,
                cmbReceiptOption.SkipIDColumn,
                false,
                50,
                false);

            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }

        private void cmbTitle_RequestClear(object sender, EventArgs e)
        {
            cmbTitle.SelectedData = new DataEntity("", "");
        }

        private List<DataEntity> GetReceiptOptions()
        {
            List<DataEntity> receiptOptions = new List<DataEntity>();

            foreach (ReceiptSettingsEnum item in (ReceiptSettingsEnum[])Enum.GetValues(typeof(ReceiptSettingsEnum)))
            {
                switch (item)
                {
                    case ReceiptSettingsEnum.Printed:
                        receiptOptions.Add(new DataEntity(Convert.ToByte(item), item.ToLocalizedString()));
                        break;
                    case ReceiptSettingsEnum.Email:
                        receiptOptions.Add(new DataEntity(Convert.ToByte(item), item.ToLocalizedString()));
                        break;
                    case ReceiptSettingsEnum.PrintAndEmail:
                        receiptOptions.Add(new DataEntity(Convert.ToByte(item), item.ToLocalizedString()));
                        break;
                }
            }

            return receiptOptions;
        }
    }
}
