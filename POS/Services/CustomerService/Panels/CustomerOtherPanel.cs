using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LSOne.Services.Panels
{
    public partial class CustomerOtherPanel : UserControl
    {
        private IConnectionManager entry;
        private ISettings settings;
        private DateTime defaultDateTime;
        private Customer customer;
        private TaxGroupTypeFilter customerTaxGroupFilter = TaxGroupTypeFilter.Standard;

        public CustomerOtherPanel(IConnectionManager entry)
        {
            InitializeComponent();

            this.entry = entry;
            settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            SetStyle(ControlStyles.Selectable, true);

            DoubleBuffered = true;

            defaultDateTime = new DateTime(1900, 1, 1);
            dtDateOfBirth.Value = DateTime.Now;
            dtDateOfBirth.Checked = false;
            cmbGender.SelectedData = new DataEntity((int)GenderEnum.None, GenderEnum.None.ToLocalizedString());
            cmbBlocking.SelectedData = new DataEntity((int)BlockedEnum.Nothing, BlockedEnum.Nothing.ToLocalizedString());
            cmbInvoiceCustomer.SelectedData = new DataEntity("", "");

            chkIsCashCustomer.Enabled = settings.SiteServiceProfile.CashCustomerSetting == SiteServiceProfile.CashCustomerSettingEnum.YesAndSetOnPos
                                        || settings.SiteServiceProfile.CashCustomerSetting == SiteServiceProfile.CashCustomerSettingEnum.NoAndSetOnPos;

            chkIsCashCustomer.Checked = settings.SiteServiceProfile.CashCustomerSetting == SiteServiceProfile.CashCustomerSettingEnum.YesAndSetOnPos
                                        || settings.SiteServiceProfile.CashCustomerSetting == SiteServiceProfile.CashCustomerSettingEnum.AlwaysYes;

            cmbBlocking.Enabled = entry.HasPermission(Permission.ManagePOSCustomerBlocking);
        }

        //Used for edit
        public CustomerOtherPanel(IConnectionManager entry, Customer customer) : this(entry)
        {
            this.customer = customer;
            cmbSalesTaxGroup.SelectedData = Providers.SalesTaxGroupData.Get(entry, customer.TaxGroup);
            cmbGender.SelectedData = new DataEntity((int)customer.Gender, customer.Gender.ToLocalizedString());
            cmbBlocking.SelectedData = new DataEntity((int)customer.Blocked, customer.Blocked.ToLocalizedString());
            chkIsCashCustomer.Checked = customer.NonChargableAccount;
            dtDateOfBirth.Value = customer.DateOfBirth == defaultDateTime ? DateTime.Now : customer.DateOfBirth;
            dtDateOfBirth.Checked = customer.DateOfBirth != defaultDateTime;
            cmbInvoiceCustomer.SelectedData = Providers.CustomerData.Get(entry, customer.AccountNumber, UsageIntentEnum.Minimal);

            if (customer.TaxExempt == TaxExemptEnum.EU)
            {
                customerTaxGroupFilter = TaxGroupTypeFilter.EU;
            }

            cmbSalesTaxGroup.Select();
        }

        public DateTime DateOfBirth => dtDateOfBirth.Checked ? dtDateOfBirth.Value.Date : new DateTime(1900, 1, 1);

        public GenderEnum Gender => !RecordIdentifier.IsEmptyOrNull(cmbGender.SelectedDataID) ? (GenderEnum)(int)cmbGender.SelectedDataID : GenderEnum.None;

        public BlockedEnum Blocked => !RecordIdentifier.IsEmptyOrNull(cmbBlocking.SelectedDataID) ? (BlockedEnum)(int)cmbBlocking.SelectedDataID : BlockedEnum.Nothing;

        public bool IsCashCustomer => chkIsCashCustomer.Checked;

        public RecordIdentifier SalesTaxGroupID => cmbSalesTaxGroup.SelectedDataID ?? RecordIdentifier.Empty;

        public string InvoiceCustomerID => cmbInvoiceCustomer.SelectedDataID?.ToString() ?? string.Empty;

        private void cmbSalesTaxGroup_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier selectedID = cmbSalesTaxGroup.SelectedDataID ?? RecordIdentifier.Empty;
            DualDataPanel panelToEmbed = new DualDataPanel(
                selectedID,
                Providers.SalesTaxGroupData.GetList(entry, customerTaxGroupFilter),
                null,
                true,
                cmbSalesTaxGroup.SkipIDColumn,
                false,
                50,
                false);

            panelToEmbed.SelectNoneAllowed = true;
            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }

        public bool CustomerErrorProviderVisible
        {
            get { return touchErrorProvider.Visible; }
            set { touchErrorProvider.Visible = value; }
        }

        public string CustomerErrorProviderText
        {
            get { return touchErrorProvider.ErrorText; }
            set { touchErrorProvider.ErrorText = value; }
        }

        public bool ValidateData()
        {
            bool reply = true;
            touchErrorProvider.Clear();

            if (settings.SiteServiceProfile.CustomerGenderIsMandatory && (GenderEnum)(int)cmbGender.SelectedDataID == GenderEnum.None)
            {
                reply = false;
                touchErrorProvider.AddErrorMessage(Properties.Resources.GenderMandatoryError);
            }

            if (settings.SiteServiceProfile.CustomerBirthDateIsMandatory && DateOfBirth == defaultDateTime)
            {
                reply = false;
                touchErrorProvider.AddErrorMessage(Properties.Resources.DateOfBirthMandatoryError);
            }

            touchErrorProvider.Visible = !reply;
            return reply;
        }

        private void ClearErrorProvider(object sender, EventArgs args)
        {
            if (sender is ShadeTextBoxTouch)
            {
                ((ShadeTextBoxTouch)(sender)).HasError = false;
            }
        }

        private void CustomerContactInfoPanel_Load(object sender, EventArgs e)
        {
            if(customer == null) //Creating a new customer
            {
                cmbSalesTaxGroup.SelectedData = new DataEntity(
                    settings.SiteServiceProfile.NewCustomerDefaultTaxGroup,
                    settings.SiteServiceProfile.NewCustomerDefaultTaxGroupName);
            }
        }

        private void cmbInvoiceCustomer_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier selectedID = cmbSalesTaxGroup.SelectedDataID ?? RecordIdentifier.Empty;
            DualDataPanel panelToEmbed = new DualDataPanel(
                selectedID,
                Providers.CustomerData.GetAllCustomers(entry, UsageIntentEnum.Minimal, false),
                null,
                true,
                cmbInvoiceCustomer.SkipIDColumn,
                false,
                50,
                false);

            panelToEmbed.SelectNoneAllowed = true;
            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            cmbSalesTaxGroup.Focus();
        }

        private void cmbInvoiceCustomer_RequestClear(object sender, EventArgs e)
        {
            cmbInvoiceCustomer.SelectedData = null;
        }

        private void cmbGender_DropDown(object sender, DropDownEventArgs e)
        {
            DualDataPanel panelToEmbed = new DualDataPanel(
                "",
                GetGenderOptions(),
                null,
                true,
                cmbGender.SkipIDColumn,
                false,
                50,
                false);

            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }

        private void cmbBlocking_DropDown(object sender, DropDownEventArgs e)
        {
            DualDataPanel panelToEmbed = new DualDataPanel(
                "",
                GetBlockingOptions(),
                null,
                true,
                cmbGender.SkipIDColumn,
                false,
                50,
                false);

            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }

        private List<DataEntity> GetGenderOptions()
        {
            List<DataEntity> genderOptions = new List<DataEntity>();

            foreach (GenderEnum item in (GenderEnum[])Enum.GetValues(typeof(GenderEnum)))
            {
                switch (item)
                {
                    case GenderEnum.None:
                        genderOptions.Add(new DataEntity(Convert.ToByte(item), Properties.Resources.NotAvailable));
                        break;
                    case GenderEnum.Female:
                        genderOptions.Add(new DataEntity(Convert.ToByte(item), Properties.Resources.Female));
                        break;
                    case GenderEnum.Male:
                        genderOptions.Add(new DataEntity(Convert.ToByte(item), Properties.Resources.Male));
                        break;
                }
            }

            return genderOptions;
        }

        private List<DataEntity> GetBlockingOptions()
        {
            List<DataEntity> blockingOptions = new List<DataEntity>();

            foreach (BlockedEnum item in (BlockedEnum[])Enum.GetValues(typeof(BlockedEnum)))
            {
                switch (item)
                {
                    case BlockedEnum.Nothing:
                        blockingOptions.Add(new DataEntity(Convert.ToByte(item), Properties.Resources.CustomerNotBlocked));
                        break;
                    case BlockedEnum.Invoice:
                        blockingOptions.Add(new DataEntity(Convert.ToByte(item), Properties.Resources.CustomerLimitedToInvoices));
                        break;
                    case BlockedEnum.All:
                        blockingOptions.Add(new DataEntity(Convert.ToByte(item), Properties.Resources.CustomerIsBlocked));
                        break;
                }
            }

            return blockingOptions;
        }
    }
}
