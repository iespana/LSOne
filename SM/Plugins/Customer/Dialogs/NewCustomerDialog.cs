using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Infocodes;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Customer.Dialogs
{
    public partial class NewCustomerDialog : DialogBase
    {
        RecordIdentifier customerID;
        bool manuallyEnterID = false;

        public bool MultiAdd { get; private set; }
    
        public NewCustomerDialog()
        {
            customerID = RecordIdentifier.Empty;
            MultiAdd = false;

            InitializeComponent();

            Parameters parameters = Providers.ParameterData.Get(PluginEntry.DataModel);
            manuallyEnterID = parameters.ManuallyEnterCustomerID;

            tbID.Visible = manuallyEnterID;
            lblID.Visible = manuallyEnterID;

            addressField.DataModel = PluginEntry.DataModel;

            fullNameControl.PopulateNamePrefixes(PluginEntry.DataModel.Cache.GetNamePrefixes());
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            cmbDefaultCustomer.SelectedData = new DataEntity("", Properties.Resources.DoNotCopy);
            cmbInvoicedCustomer.SelectedData = new DataEntity("", "");
            cmbSalesTaxGroup.SelectedData = new DataEntity("", "");

            addressField.SetData(PluginEntry.DataModel, new Address(PluginEntry.DataModel.Settings.AddressFormat));
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier CustomerID
        {
            get { return customerID; }
        }

        private bool Save()
        {
            DataLayer.BusinessObjects.Customers.Customer customer;

            if (((DataEntity)cmbDefaultCustomer.SelectedData).ID != "")
            {
                customer = Providers.CustomerData.Get(PluginEntry.DataModel, ((DataEntity)cmbDefaultCustomer.SelectedData).ID, UsageIntentEnum.Reporting);

                customer.ID = RecordIdentifier.Empty;
                customer.ReceiptEmailAddress = string.Empty;
                customer.VatNum = string.Empty;
                customer.Gender = GenderEnum.None;
                customer.DateOfBirth = new DateTime(1900, 1, 1);
                customer.MobilePhone = string.Empty;
                customer.Email = string.Empty;
                customer.Telephone = string.Empty;
            }
            else
            {
                customer = new DataLayer.BusinessObjects.Customers.Customer();
            }

            if (manuallyEnterID)
            {
                if (tbID.Text.Trim() == "")
                {
                    if (QuestionDialog.Show(Properties.Resources.IDMissingQuestion, Properties.Resources.IDMissing) != DialogResult.Yes)
                    {
                        return false;
                    }
                }
                else
                {
                    if (!tbID.Text.IsAlphaNumeric())
                    {
                        errorProvider1.SetError(tbID, Properties.Resources.OnlyCharAndNumbers);
                        return false;
                    }
                    customer.ID = tbID.Text.Trim();

                    if (Providers.CustomerData.Exists(PluginEntry.DataModel, customer.ID))
                    {
                        errorProvider1.SetError(tbID, Properties.Resources.CustomerIDExists);
                        return false;
                    }
                }
            }

            var address = new CustomerAddress {Address = addressField.AddressRecord};

            //fullNameControl.GetNameIntoRecord(customer.CopyName());
            if (!customer.CompareNames(fullNameControl.NameRecord))
            {
                customer.FirstName = fullNameControl.NameRecord.First;
                customer.MiddleName = fullNameControl.NameRecord.Middle;
                customer.LastName = fullNameControl.NameRecord.Last;
                customer.Prefix = fullNameControl.NameRecord.Prefix;
                customer.Suffix = fullNameControl.NameRecord.Suffix;
            }
            customer.Text = tbDisplayName.Text;
            customer.Addresses.Add(address);
            customer.IdentificationNumber = tbIdentificationNumber.Text;
            customer.SearchName = tbSearchAlias.Text;
            if (((DataEntity) cmbDefaultCustomer.SelectedData).ID == "")
            {
                customer.AccountNumber = ((DataEntity) cmbInvoicedCustomer.SelectedData).ID.ToString();
                customer.TaxGroup = ((DataEntity) cmbSalesTaxGroup.SelectedData).ID.ToString();
            }

            Providers.CustomerData.Save(PluginEntry.DataModel, customer);

            address.CustomerID = customer.ID;
            address.Address.AddressType = Address.AddressTypes.Shipping;
            address.IsDefault = true;
            Providers.CustomerAddressData.Save(PluginEntry.DataModel, address);

            SaveAdditionalInformation(customer.ID, ((DataEntity) cmbDefaultCustomer.SelectedData).ID != "");

            customerID = customer.ID;

            if (chkCreateAnother.Checked)
            {
                MultiAdd = true;
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, NotifyContexts.Customer, customer.ID, customer);
            }

            return true;
        }

        private void SaveAdditionalInformation(RecordIdentifier newCustomer, bool isCopied)
        {
            if (isCopied)
            {
                DataLayer.BusinessObjects.Customers.Customer customer = Providers.CustomerData.Get(PluginEntry.DataModel, ((DataEntity)cmbDefaultCustomer.SelectedData).ID, UsageIntentEnum.Reporting);

                List<CustomerGroup> customerGroups = Providers.CustomerGroupData.GetGroupsForCustomer(PluginEntry.DataModel, customer.ID);

                foreach (CustomerGroup customerGroup in customerGroups)
                {
                    CustomerInGroup group = new CustomerInGroup();
                    group.ID = new RecordIdentifier(newCustomer, customerGroup.ID);
                    group.Default = customerGroup.DefaultGroup;
                    Providers.CustomersInGroupData.Save(PluginEntry.DataModel, group);
                }

                List<TradeAgreementEntry> lineDiscounts = Providers.TradeAgreementData.GetForCustomerAndGroup(PluginEntry.DataModel, customer.ID, RecordIdentifier.Empty, TradeAgreementRelation.LineDiscSales);
                List<TradeAgreementEntry> multiLineDiscounts = Providers.TradeAgreementData.GetForCustomerAndGroup(PluginEntry.DataModel, customer.ID, RecordIdentifier.Empty, TradeAgreementRelation.MultiLineDiscSales);
                List<TradeAgreementEntry> totalLineDiscounts = Providers.TradeAgreementData.GetTotalDiscForCustomer(PluginEntry.DataModel, customer.ID, RecordIdentifier.Empty);

                foreach (TradeAgreementEntry lineDiscount in lineDiscounts.Union(multiLineDiscounts).Union(totalLineDiscounts))
                {
                    lineDiscount.ID = RecordIdentifier.Empty;
                    lineDiscount.AccountRelation = newCustomer;
                    Providers.TradeAgreementData.Save(PluginEntry.DataModel, lineDiscount, Permission.ManageDiscounts);
                }

                List<DiscountOffer> periodicDiscountOffers = Providers.DiscountOfferData.GetForCustomer(PluginEntry.DataModel, DiscountOfferFilter.AllExceptPromotions, customer.ID);

                foreach (DiscountOffer periodicDiscount in periodicDiscountOffers)
                {
                    periodicDiscount.ID = RecordIdentifier.Empty;
                    periodicDiscount.AccountRelation = newCustomer;
                    Providers.DiscountOfferData.Save(PluginEntry.DataModel, periodicDiscount);
                }

                List<InfocodeSpecific> infocodes = Providers.InfocodeSpecificData.GetListForRefRelation(PluginEntry.DataModel, new RecordIdentifier(customer.ID,
                                new RecordIdentifier(RecordIdentifier.Empty, RecordIdentifier.Empty)), UsageCategoriesEnum.None, RefTableEnum.Customer, InfocodeSpecificSorting.InfocodeDescription, false);

                foreach (InfocodeSpecific infocode in infocodes)
                {
                    infocode.RefRelation = newCustomer;
                    Providers.InfocodeSpecificData.Save(PluginEntry.DataModel, infocode);
                }

                List<TradeAgreementEntry> salesPrices = Providers.TradeAgreementData.GetForCustomerAndGroup(PluginEntry.DataModel, customer.ID, RecordIdentifier.Empty, TradeAgreementRelation.PriceSales);

                foreach (TradeAgreementEntry salsePrice in salesPrices)
                {
                    salsePrice.ID = RecordIdentifier.Empty;
                    salsePrice.AccountRelation = newCustomer;
                    Providers.TradeAgreementData.Save(PluginEntry.DataModel, salsePrice, Permission.ManageTradeAgreementPrices);
                }

                List<DiscountOffer> customerPromotions = Providers.DiscountOfferData.GetForCustomer(PluginEntry.DataModel, DiscountOfferFilter.OnlyPromotions, customer.ID);

                foreach (DiscountOffer promo in customerPromotions)
                {
                    promo.ID = RecordIdentifier.Empty;
                    promo.AccountRelation = newCustomer;
                    Providers.DiscountOfferData.Save(PluginEntry.DataModel, promo);
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                if (chkCreateAnother.Checked)
                {
                    tbID.Text = "";
                    fullNameControl.Prefix = "";
                    fullNameControl.FirstName = "";
                    fullNameControl.MiddleName = "";
                    fullNameControl.LastName = "";
                    fullNameControl.Suffix = "";
                    tbDisplayName.Text = "";
                    addressField.SetData(PluginEntry.DataModel, new Address());
                    tbSearchAlias.Text = "";
                    tbIdentificationNumber.Text = "";
                    if (manuallyEnterID)
                    {
                        tbID.Focus();
                    }
                    else
                    {
                        fullNameControl.Focus();
                    }
                    
                }
                else
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbDefaultCustomer_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;
            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)cmbDefaultCustomer.SelectedData).ID;
                textInitallyHighlighted = true;
            }

            e.ControlToEmbed = new SingleSearchPanel(
            PluginEntry.DataModel,
            false,
            initialSearchText,
            SearchTypeEnum.Customers,
            textInitallyHighlighted);
        }

        private void cmbDefaultCustomer_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            btnOK.Enabled = fullNameControl.FirstName.Length > 0 || fullNameControl.LastName.Length > 0 || (tbDisplayName.Enabled && tbDisplayName.Text.Trim() != "");
        }

        private void fullNameControl_ValueChanged(object sender, EventArgs e)
        {
            tbDisplayName.Enabled = fullNameControl.IsEmpty;
            tbDisplayName.Text = PluginEntry.DataModel.Settings.NameFormatter.Format(fullNameControl.NameRecord);
            CheckEnabled(sender, e);
        }
        
        private void cmbInvoicedCustomer_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;
            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)cmbInvoicedCustomer.SelectedData).ID;
                textInitallyHighlighted = true;
            }

            e.ControlToEmbed = new SingleSearchPanel(
            PluginEntry.DataModel,
            false,
            initialSearchText,
            SearchTypeEnum.Customers,
            textInitallyHighlighted);
        }

        private void cmbInvoicedCustomer_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void cmbSalesTaxGroup_RequestData(object sender, EventArgs e)
        {
            cmbSalesTaxGroup.SetData(Providers.SalesTaxGroupData.GetList(PluginEntry.DataModel, TaxGroupTypeFilter.Standard), null);
        }

        private void cmbSalesTaxGroup_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }
        
    }
}
