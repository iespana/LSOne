using System;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Panels
{
    public partial class CustomerDescriptionPanel : UserControl
    {
        private IConnectionManager entry;
        private ISettings settings;

        public bool NextEnabled { get; set; }

        public CustomerDescriptionPanel(IConnectionManager entry)
        {
            NextEnabled = false;

            InitializeComponent();

            this.entry = entry;
            settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            fullNameControlTouch.PopulateNamePrefixes(entry.Cache.GetNamePrefixes());            

            tbID.Enabled = btnVerify.Enabled = settings.SiteServiceProfile.AllowCustomerManualID;
            cmbReceiptOption.SelectedIndex = 0;

            SetStyle(ControlStyles.Selectable, true);

            DoubleBuffered = true;
        }

        public string Email => tbEmail.Text;

        public string ReceiptEmailAddress => tbReceiptEmail.Text;

        public string Phone => tbPhone.Text;

        public string SearchAlias => tbSearchAlias.Text;

        public Name NameRecord => fullNameControlTouch.NameRecord;

        public string DisplayName => tbDisplayName.Text;

        public ReceiptSettingsEnum ReceiptOption => (ReceiptSettingsEnum) cmbReceiptOption.SelectedIndex;

        public string CustomerID => tbID.Text.Trim();

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            fullNameControlTouch.Focus();
        }

        private void fullNameControlTouch_ValueChanged(object sender, EventArgs e)
        {
            tbDisplayName.Enabled = fullNameControlTouch.IsEmpty;
            tbDisplayName.Text = entry.Settings.NameFormatter.Format(fullNameControlTouch.NameRecord);
            ClearErrorProvider(this, EventArgs.Empty);
        }       

        public void GetNameIntoRecord(Name name)
        {
            fullNameControlTouch.GetNameIntoRecord(name);
        }
        
        public bool ValidateData()
        {
            bool reply = true;
            
            if (settings.SiteServiceProfile.CustomerSearchAliasIsMandatory && tbSearchAlias.Text.Trim() == "")
            {
                reply = false;
                errorProvider1.SetError(tbSearchAlias, Properties.Resources.SearchAliasMandatoryError);
            }

            if (settings.SiteServiceProfile.CustomerPhoneIsMandatory && tbPhone.Text.Trim() == "")
            {
                reply = false;
                errorProvider1.SetError(tbPhone, Properties.Resources.PhoneMandatoryError);
            }

            if (settings.SiteServiceProfile.CustomerEmailIsMandatory && tbEmail.Text.Trim() == "")
            {
                reply = false;
                errorProvider1.SetError(tbEmail, Properties.Resources.EmailMandatoryError);                                
            }

            if (settings.SiteServiceProfile.CustomerReceiptEmailIsMandatory && tbReceiptEmail.Text.Trim() == "")
            {
                reply = false;
                errorProvider1.SetError(tbReceiptEmail, Properties.Resources.ReceiptEmailMandatoryError);
            }

            if (settings.SiteServiceProfile.CustomerNameIsMandatory && fullNameControlTouch.IsEmpty)
            {
                reply = false;
                errorProvider1.SetError(tbDisplayName, Properties.Resources.CustomerNameMandatoryError);
            }

            if (settings.SiteServiceProfile.AllowCustomerManualID && tbID.Text.Trim() != "")
            {
                reply = ValidateCustomerID();
            }

            if (tbEmail.Text != "")
            {
                if (!tbEmail.Text.IsEmail())
                {
                    errorProvider1.SetError(tbEmail, Properties.Resources.FieldDoesNotContainValidEmail);
                    reply = false;
                }
            }

            if (tbReceiptEmail.Text != "")
            {
                if (!tbReceiptEmail.Text.IsEmail())
                {
                    errorProvider1.SetError(tbReceiptEmail, Properties.Resources.FieldDoesNotContainValidEmail);
                    reply = false;
                }

            }

            return reply;
        }


        private void ClearErrorProvider(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }

        private bool ValidateCustomerID()
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

            if (customerExists)
            {
                errorProvider1.SetError(tbID, Properties.Resources.CustomerIDAlreadyExists);
            }

            return customerExists;
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            ValidateCustomerID();
        }
    }
}
