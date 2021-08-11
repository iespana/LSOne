using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.BusinessObjects.Companies;
using System.Drawing;

namespace LSOne.ViewPlugins.Customer.ViewPages
{
    public partial class CustomerDetailPage : UserControl, ITabView
    {

        private LSOne.DataLayer.BusinessObjects.Customers.Customer customer;
        WeakReference salesTaxGroupEditor;
        private WeakReference currencyEditor;
        List<CustomerListItem> items;

        private SalesTaxGroup taxExemptGroup;

        public CustomerDetailPage()
        {
            InitializeComponent();

            IPlugin plugin;

            plugin = PluginEntry.Framework.FindImplementor(this, "ViewSalesTaxGroups", null);
            salesTaxGroupEditor = plugin != null ? new WeakReference(plugin) : null;
            btnEditSalesTaxGroup.Visible = (salesTaxGroupEditor != null);

            plugin = PluginEntry.Framework.FindImplementor(this, "ViewCurrency", null);
            currencyEditor = plugin != null ? new WeakReference(plugin) : null;
            btnCurrenciesEdit.Visible = (currencyEditor != null);

            cmbCust.SelectedData = new DataEntity(RecordIdentifier.Empty, "");

            taxExemptGroup = PluginOperations.GetTaxExemptInformation();

            plugin = PluginEntry.Framework.FindImplementor(null, "SAPBusinessOne", null);

            List<string> taxOptions = new List<string>
            {
                Properties.Resources.No,
                Properties.Resources.Yes
            };

            if (plugin != null)
            {
                taxOptions.Add(Properties.Resources.EU);
            }

            cmbTaxExempt.Items.AddRange(taxOptions.ToArray());
        }        

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.CustomerDetailPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            customer = (DataLayer.BusinessObjects.Customers.Customer)internalContext;

            cmbLanguage.SelectedIndex = 0;
            foreach (object item in cmbLanguage.Items)
            {
                if (item.ToString() == customer.LanguageCode)
                {
                    cmbLanguage.SelectedItem = item;
                    break;
                }
            }

            if (customer.AccountNumber == "")
            {
                cmbCust.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
            }
            else
            {
                cmbCust.SelectedData = new DataEntity(customer.AccountNumber, Providers.CustomerData.GetCustomerName(PluginEntry.DataModel, customer.AccountNumber));
            }

            tbIdentificationNumber.Text = customer.IdentificationNumber;
            tbReceiptEmail.Text = customer.ReceiptEmailAddress;
            tbSearchAlias.Text = customer.SearchName;

            cmbReceiptOption.SelectedIndex = (int)customer.ReceiptSettings;

            cmbGender.SelectedIndex = (int)customer.Gender;
            
            dtDateOfBirth.Value = customer.DateOfBirth;
            dtDateOfBirth.Checked = customer.DateOfBirth.Date != new DateTime(1900, 1, 1);

            cmbCurrencies.SelectedData = new DataEntity(customer.Currency, customer.CurrencyDescription);
            cmbBlocking.SelectedIndex = (int)customer.Blocked;
            chkIsCashCustomer.Checked = customer.NonChargableAccount;
            chkPricesIncludeSalesTax.Checked = customer.PricesIncludeSalesTax;
            cmbTaxExempt.SelectedIndex = (customer.TaxExempt == TaxExemptEnum.EU && cmbTaxExempt.Items.Count == 2) ? 0 : (int)customer.TaxExempt;
            cmbSalesTaxGroup.SelectedData = new DataEntity(customer.TaxGroup, customer.TaxGroupName);
            tbVATnumber.Text = customer.VatNum;
        }

        public bool DataIsModified()
        {
            string selectedLanguage = (cmbLanguage.SelectedIndex < 1) ? "" : cmbLanguage.SelectedItem.ToString();
            if (
                selectedLanguage != customer.LanguageCode ||
                cmbCust.SelectedData.ID != customer.AccountNumber ||
                tbIdentificationNumber.Text != customer.IdentificationNumber ||
                tbReceiptEmail.Text != customer.ReceiptEmailAddress ||
                tbSearchAlias.Text != customer.SearchName ||
                ((ReceiptSettingsEnum)cmbReceiptOption.SelectedIndex) != customer.ReceiptSettings ||
                (cmbCurrencies.SelectedData.ID != customer.Currency) ||
                (cmbSalesTaxGroup.SelectedData.ID != customer.TaxGroup) ||
                (cmbBlocking.SelectedIndex != (int)customer.Blocked) ||
                (chkIsCashCustomer.Checked != customer.NonChargableAccount) ||
                tbVATnumber.Text != customer.VatNum ||
                cmbGender.SelectedIndex != (int)customer.Gender ||
                dtDateOfBirth.Value.Date != customer.DateOfBirth.Date ||
                cmbTaxExempt.SelectedIndex != (int)customer.TaxExempt ||
                chkPricesIncludeSalesTax.Checked != customer.PricesIncludeSalesTax)
            {
                customer.Dirty = true;
                return true;
            }

            return false;
        }
        
        public bool SaveData()
        {
            string selectedLanguage = (cmbLanguage.SelectedIndex < 1) ? "" : cmbLanguage.SelectedItem.ToString();
            customer.LanguageCode = selectedLanguage;
            customer.IdentificationNumber = tbIdentificationNumber.Text;
            customer.AccountNumber = (string)cmbCust.SelectedData.ID;
            customer.ReceiptEmailAddress = tbReceiptEmail.Text;
            customer.ReceiptSettings = ((ReceiptSettingsEnum)cmbReceiptOption.SelectedIndex);
            customer.SearchName = tbSearchAlias.Text;
            customer.Currency = (string)cmbCurrencies.SelectedData.ID;
            customer.TaxGroup = (string)cmbSalesTaxGroup.SelectedData.ID;
            customer.TaxExempt = (TaxExemptEnum)cmbTaxExempt.SelectedIndex;
            customer.DateOfBirth = dtDateOfBirth.Checked ? dtDateOfBirth.Value.Date : new DateTime(1900, 1, 1);
            customer.Gender = (GenderEnum)cmbGender.SelectedIndex;

            customer.Blocked = (BlockedEnum)cmbBlocking.SelectedIndex;
            customer.NonChargableAccount = chkIsCashCustomer.Checked;
            customer.PricesIncludeSalesTax = chkPricesIncludeSalesTax.Checked;
            customer.VatNum = tbVATnumber.Text;

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "Parameters")
            {
                taxExemptGroup = PluginOperations.GetTaxExemptInformation();
                cmbTaxExempt_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void cmbSalesTaxGroup_RequestData(object sender, EventArgs e)
        {
            cmbSalesTaxGroup.SetData(Providers.SalesTaxGroupData.GetListWithTaxCodes(PluginEntry.DataModel, cmbTaxExempt.SelectedIndex == 2 ? TaxGroupTypeFilter.EU : TaxGroupTypeFilter.Standard), null);
        }

        private void ClearData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void btnEditItemSalesTaxGroup_Click(object sender, EventArgs e)
        {
            if (salesTaxGroupEditor.IsAlive)
            {
                ((IPlugin)salesTaxGroupEditor.Target).Message(this, "ViewSalesTaxGroups", cmbSalesTaxGroup.SelectedData.ID);
            }
        }

        private void cmbCurrencies_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
            cmbCurrencies_SelectedDataChanged(this, EventArgs.Empty);
        }

        private void cmbCurrencies_RequestData(object sender, EventArgs e)
        {
            cmbCurrencies.SetData(Providers.CurrencyData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbCurrencies_SelectedDataChanged(object sender, EventArgs e)
        {
            btnCurrenciesEdit.Enabled = (cmbCurrencies.SelectedData.ID != "") &&
                              PluginEntry.DataModel.HasPermission(Permission.CustomerEdit) &&
                              currencyEditor != null;
        }

        private void btnCurrenciesEdit_Click(object sender, EventArgs e)
        {
            if (currencyEditor.IsAlive)
            {
                ((IPlugin)currencyEditor.Target).Message(this, "ViewCurrency", cmbCurrencies.SelectedData.ID);
            }
        }

        private void cmbCust_RequestData(object sender, EventArgs e)
        {
            items = CustomerSearchPanelFactory.GetAllExceptCurrentCust("", CustomerSorting.ID, false, false, customer.ID);
            cmbCust.SetData(items,null);
        }

        private void cmbCust_DropDown(object sender, DropDownEventArgs e)
        {
            e.ControlToEmbed = new SingleSearchPanel(
                PluginEntry.DataModel,
                false,
                cmbCust.SelectedData.ID, SearchTypeEnum.Customers, new List<RecordIdentifier>(){customer.ID});
        }

        private void cmbCust_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }    

        private void cmbTaxExempt_SelectedIndexChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            cmbSalesTaxGroup.Enabled =
            btnEditSalesTaxGroup.Enabled = cmbTaxExempt.SelectedIndex != 1;

            cmbSalesTaxGroup.SelectedData = new DataEntity(RecordIdentifier.Empty, "");

            if (!cmbSalesTaxGroup.Enabled && taxExemptGroup != null)
            {
                cmbSalesTaxGroup.SelectedData = new DataEntity(taxExemptGroup.ID, taxExemptGroup.Text);
            }
            else if (!cmbSalesTaxGroup.Enabled && taxExemptGroup == null)
            {
                errorProvider1.Icon = Icon.FromHandle(((Bitmap)PluginEntry.Framework.GetImage(ImageEnum.InformationErrorProvider)).GetHicon());
                errorProvider1.SetError(btnEditSalesTaxGroup, Properties.Resources.DefaultTaxExemptionGroupCanBeSet);
            }
            else if (cmbSalesTaxGroup.Enabled && taxExemptGroup != null && customer.TaxGroup != taxExemptGroup.ID && customer.TaxExempt == (TaxExemptEnum)cmbTaxExempt.SelectedIndex)
            {
                cmbSalesTaxGroup.SelectedData = new DataEntity(customer.TaxGroup, customer.TaxGroupName);
            }
        }

        public void SetDefaultTaxExemptionGroup(object sender, EventArgs e)
        {            
            IPlugin plugin = PluginEntry.Framework.FindImplementor(null, "CanViewAdministrationTab", "StoreManagementTab");

            if (plugin != null)
            {
                plugin.Message(null, "ViewStoreManagementTab", null);                
            }
        }
    }
}
