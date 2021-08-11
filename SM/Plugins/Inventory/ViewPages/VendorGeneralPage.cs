using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    public partial class VendorGeneralPage : UserControl, ITabView
    {
        Vendor vendor;
        WeakReference salesTaxGroupEditor;
        private bool saveData;

        public VendorGeneralPage()
        {
            InitializeComponent();

            IPlugin plugin;
            plugin = PluginEntry.Framework.FindImplementor(this, "ViewSalesTaxGroups", null);
            salesTaxGroupEditor = plugin != null ? new WeakReference(plugin) : null;
            btnEditSalesTaxGroup.Visible = (salesTaxGroupEditor != null);

            addressField.DataModel = PluginEntry.DataModel;
            addressField.AddressFormat = PluginEntry.DataModel.Settings.AddressFormat;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.VendorGeneralPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            vendor = (Vendor)internalContext;

            addressField.SetData(PluginEntry.DataModel, vendor.CopyAddress());
            cmbCurrency.SelectedData = new DataEntity(vendor.CurrencyID, vendor.CurrencyDescription);

            tbPhone.Text = vendor.Phone;
            tbFax.Text = vendor.Fax;
            tbEmail.Text = vendor.Email;
            cmbSalesTaxGroup.SelectedData = new DataEntity(vendor.TaxGroup, vendor.TaxGroupName);
            cmbTaxCalculationMethod.SelectedIndex = (int)vendor.TaxCalculationMethod;
            ntbDefaultDeliveryTime.Value = vendor.DefaultDeliveryTime;
            cmbDeliveryDaysType.SelectedIndex = (int)vendor.DeliveryDaysType;

            foreach (object item in cmbLanguage.Items)
            {
                if (item.ToString() == vendor.LanguageID)
                {
                    cmbLanguage.SelectedItem = item;
                    break;
                }
            }
        }

        public bool DataIsModified()
        {
            string selectedLanguage = (cmbLanguage.SelectedIndex < 0) ? "" : cmbLanguage.SelectedItem.ToString();

            if (!vendor.CompareAddress(addressField.AddressRecord) ||
                cmbCurrency.SelectedData.ID != vendor.CurrencyID ||
                selectedLanguage != vendor.LanguageID ||
                tbEmail.Text != vendor.Email ||
                tbFax.Text != vendor.Fax ||
                tbPhone.Text != vendor.Phone ||
                cmbSalesTaxGroup.SelectedData.ID != vendor.TaxGroup ||
                cmbTaxCalculationMethod.SelectedIndex != (int)vendor.TaxCalculationMethod ||
                (int)ntbDefaultDeliveryTime.Value != vendor.DefaultDeliveryTime ||
                cmbDeliveryDaysType.SelectedIndex != (int)vendor.DeliveryDaysType)
            {
                vendor.Dirty = true;
                return true;
            }

            return false;
        }

        public bool SaveData()
        {
            vendor.Address2 = addressField.AddressRecord.Address2;
            vendor.Address1 = addressField.AddressRecord.Address1;
            vendor.City = addressField.AddressRecord.City;
            vendor.ZipCode = addressField.AddressRecord.Zip;
            vendor.State = addressField.AddressRecord.State;
            vendor.Country = addressField.AddressRecord.Country;
            vendor.AddressFormat = addressField.AddressFormat;
            vendor.CurrencyID = cmbCurrency.SelectedData.ID;

            string selectedLanguage = (cmbLanguage.SelectedIndex < 0) ? "" : cmbLanguage.SelectedItem.ToString();
            vendor.LanguageID = selectedLanguage;
            vendor.Phone = tbPhone.Text;
            vendor.Fax = tbFax.Text;
            vendor.Email = tbEmail.Text;
            vendor.TaxGroup = cmbSalesTaxGroup.SelectedData.ID;
            vendor.TaxCalculationMethod = (TaxCalculationMethodEnum)cmbTaxCalculationMethod.SelectedIndex;
            vendor.DefaultDeliveryTime = (int)ntbDefaultDeliveryTime.Value;
            vendor.DeliveryDaysType = (DeliveryDaysTypeEnum) cmbDeliveryDaysType.SelectedIndex;

            saveData = false;
            try
            {
                saveData = true;

                var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                service.SaveVendor(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), vendor, true);

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "VendorChanged", vendor.ID, null);
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                saveData = false;

            }

            return saveData;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void cmbCurrency_RequestData(object sender, EventArgs e)
        {
            cmbCurrency.SetData(Providers.CurrencyData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbTaxGroup_RequestData(object sender, EventArgs e)
        {
            cmbSalesTaxGroup.SetData(Providers.SalesTaxGroupData.GetSalesTaxGroups(PluginEntry.DataModel,SalesTaxGroup.SortEnum.Description,false),null);
        }

        private void cmbTaxGroup_RequestClear(object sender, EventArgs e)
        {
            cmbSalesTaxGroup.SelectedData = new DataEntity("", "");
        }

        private void contextButton1_Click(object sender, EventArgs e)
        {
            if (salesTaxGroupEditor.IsAlive)
            {
                ((IPlugin)salesTaxGroupEditor.Target).Message(this, "ViewSalesTaxGroups", cmbSalesTaxGroup.SelectedData.ID);
            }
        }

    }
}
