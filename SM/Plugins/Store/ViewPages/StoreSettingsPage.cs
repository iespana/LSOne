using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.BusinessObjects.StoreManagement;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    public partial class StoreSettingsPage : UserControl, ITabView
    {
        LSOne.DataLayer.BusinessObjects.StoreManagement.Store store;
        IPlugin functionalityProfileEditor;
        IPlugin customerEditor;
        WeakReference customerCreator;
        WeakReference salesTaxGroupEditor;
        WeakReference currencyEditor;
        WeakReference layoutEditor;
        WeakReference functionalityProfile;
        Dictionary<string, string[]> keyboardCodes = new Dictionary<string, string[]>();
        private int orgStorePriceSettings;
        private RecordIdentifier oldSalesTaxID;

        public StoreSettingsPage()
        {
            IPlugin plugin;

            InitializeComponent();

            DoubleBuffered = true;

            plugin = PluginEntry.Framework.FindImplementor(this, "ViewSalesTaxGroups", null);
            salesTaxGroupEditor = plugin != null ? new WeakReference(plugin) : null;
            btnEditSalesTaxGroup.Visible = (salesTaxGroupEditor != null);
            orgStorePriceSettings = -1;

            plugin = PluginEntry.Framework.FindImplementor(this, "ViewCurrency", null);
            currencyEditor = plugin != null ? new WeakReference(plugin) : null;
            btnCurrenciesEdit.Visible = (currencyEditor != null);

            plugin = PluginEntry.Framework.FindImplementor(this, "CanEditLayouts", null);
            layoutEditor = plugin != null ? new WeakReference(plugin) : null;
            btnEditTouchButtons.Visible = (layoutEditor != null);

            plugin = PluginEntry.Framework.FindImplementor(this, "CanCreateCustomer", null);
            customerCreator = plugin != null ? new WeakReference(plugin) : null;
            btnAddCustomer.Visible = (customerCreator != null);

            plugin = PluginEntry.Framework.FindImplementor(this, "CanEditFunctionalityProfiles", null);
            functionalityProfile = plugin != null ? new WeakReference(plugin) : null;
            btnEditFunctionalProfile.Visible = (functionalityProfile != null);

            addressField.DataModel = PluginEntry.DataModel;
            addressField.AddressFormat = PluginEntry.DataModel.Settings.AddressFormat;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.StoreSettingsPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            functionalityProfileEditor = PluginEntry.Framework.FindImplementor(this, "CanEditFunctionalityProfiles", null);
            customerEditor = PluginEntry.Framework.FindImplementor(this, "CanEditCustomer", null);

            if (functionalityProfileEditor != null)
            {
                btnEditFunctionalProfile.Visible = true;
            }

            if (customerEditor != null)
            {
                btnEditCustomer.Visible = true;
            }

            if (layoutEditor != null)
            {
                btnEditTouchButtons.Visible = true;
            }
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            if (isRevert)
            {
                // We need to check for tax changes if we are changing the stores tax group
                if (cmbSalesTaxGroup.SelectedData != new DataEntity(store.TaxGroup, store.TaxGroupName))
                {
                    cmbSalesTaxGroup.SelectedData = new DataEntity(store.TaxGroup, store.TaxGroupName);
                    cmbSalesTaxGroup_SelectedDataChanged(this, EventArgs.Empty);
                }
            }

            store = (LSOne.DataLayer.BusinessObjects.StoreManagement.Store)internalContext;

            addressField.SetData(PluginEntry.DataModel, store.Address);

            cmbFunctionalProfile.SelectedData = new DataEntity(store.FunctionalityProfile, store.FunctionalityProfileDescription);
            cmbCurrencies.SelectedData = new DataEntity(store.Currency, store.CurrencyDescription);

            cmbPriceTaxSetting.SelectedIndex = StorePriceSettingsToIndex();
            cmbPriceTaxSetting.Enabled = PluginEntry.DataModel.HasPermission(Permission.ManagePriceSettings);

            cmbRegion.SelectedData = new DataEntity(store.RegionID, store.RegionDescription);
            cmbSalesTaxGroup.SelectedData = new DataEntity(store.TaxGroup, store.TaxGroupName);
            oldSalesTaxID = cmbSalesTaxGroup.SelectedData.ID;

            if (store.UseDefaultCustomerAccount)
            {
                cmbDefaultCustomer.SelectedData = new DataEntity(store.DefaultCustomerAccount, store.DefaultCustomerAccountDescription);
            }
            else
            {
                cmbDefaultCustomer.SelectedData = new DataEntity("", "");
            }
            
            cmbTouchLayout.SelectedData = new DataEntity(store.LayoutID, store.LayoutDescription);

            foreach (object item in cmbLanguage.Items)
            {
                if (item.ToString() == store.LanguageCode)
                {
                    cmbLanguage.SelectedItem = item;
                    break;
                }
            }

            cmbDefaultCustomer_SelectedDataChanged(this, EventArgs.Empty);
            cmbRegion_SelectedDataChanged(this, EventArgs.Empty);

            cmbKeyboard.Items.Add(Properties.Resources.SystemDefault);
            if (store.KeyboardLayoutName == "" && store.KeyboardCode == "")
            {
                cmbKeyboard.SelectedItem = Properties.Resources.SystemDefault;
            }

            InputLanguageCollection languages = InputLanguage.InstalledInputLanguages;
            foreach (InputLanguage language in languages)
            {
                string keyboardCode = language.LayoutName + " - (" + language.Culture + ")";
                keyboardCodes[keyboardCode] = new string[] { language.LayoutName, language.Culture.ToString() };
                cmbKeyboard.Items.Add(keyboardCode);
                if (language.LayoutName == store.KeyboardLayoutName && language.Culture.ToString() == store.KeyboardCode)
                {
                    cmbKeyboard.SelectedItem = keyboardCode;
                }
            }

            cmbOperationAuditing.SelectedIndex = (int)store.OperationAuditSetting;

            ntbStartAmount.Value = (double)store.StartAmount;
        }

        public bool DataIsModified()
        {
            bool useDefaultCustomerAccount;
            string selectedLanguage;

            if (cmbFunctionalProfile.SelectedData.ID != store.FunctionalityProfile) return true;
            if (addressField.AddressRecord != store.Address) return true;
            if (cmbCurrencies.SelectedData.ID != store.Currency) return true;
            if (cmbSalesTaxGroup.SelectedData.ID != store.TaxGroup) return true;
            if (StorePriceIndexToEnum(cmbPriceTaxSetting.SelectedIndex) != store.StorePriceSetting || orgStorePriceSettings == 0) return true;
            if (cmbRegion.SelectedData.ID != store.RegionID) return true;

            selectedLanguage = (cmbLanguage.SelectedIndex < 0) ? "" : cmbLanguage.SelectedItem.ToString();

            useDefaultCustomerAccount = (((DataEntity)cmbDefaultCustomer.SelectedData).ID != "");

            if (useDefaultCustomerAccount != store.UseDefaultCustomerAccount) return true;
            if (((DataEntity)cmbDefaultCustomer.SelectedData).ID != store.DefaultCustomerAccount) return true;
            if (cmbTouchLayout.SelectedData.ID != store.LayoutID) return true;
            if (selectedLanguage != store.LanguageCode) return true;

            if (cmbKeyboard.Text != Properties.Resources.SystemDefault)
            {
                if (keyboardCodes[cmbKeyboard.SelectedItem.ToString()][0] != store.KeyboardLayoutName) return true;
                if (keyboardCodes[cmbKeyboard.SelectedItem.ToString()][1] != store.KeyboardCode) return true;
            }
            else
            {
                if (store.KeyboardLayoutName != "") return true;
                if (store.KeyboardCode != "") return true;
            }

            if (cmbOperationAuditing.SelectedIndex != (int) store.OperationAuditSetting) return true;

            if (ntbStartAmount.Value != (double) store.StartAmount) return true;
            
            return false;
        }

        public bool SaveData()
        {
            store.FunctionalityProfile = (string)cmbFunctionalProfile.SelectedData.ID;
            store.Currency = (string)cmbCurrencies.SelectedData.ID;
            store.TaxGroup = (string)cmbSalesTaxGroup.SelectedData.ID;
            store.UseDefaultCustomerAccount = (((DataEntity)cmbDefaultCustomer.SelectedData).ID != "");
            store.DefaultCustomerAccount = ((DataEntity)cmbDefaultCustomer.SelectedData).ID;
            store.LayoutID = cmbTouchLayout.SelectedData.ID;
            store.StorePriceSetting = StorePriceIndexToEnum(cmbPriceTaxSetting.SelectedIndex);
            store.Address = addressField.AddressRecord;
            store.RegionID = cmbRegion.SelectedData.ID;

            string selectedLanguage = (cmbLanguage.SelectedIndex < 0) ? "" : cmbLanguage.SelectedItem.ToString();
            store.LanguageCode = selectedLanguage;

            if (cmbKeyboard.Text != Properties.Resources.SystemDefault)
            {
                store.KeyboardLayoutName = keyboardCodes[cmbKeyboard.SelectedItem.ToString()][0];
                store.KeyboardCode = keyboardCodes[cmbKeyboard.SelectedItem.ToString()][1];
            }
            else
            {
                store.KeyboardLayoutName = "";
                store.KeyboardCode = "";
            }

            store.OperationAuditSetting = (OperationAuditEnum) cmbOperationAuditing.SelectedIndex;

            store.StartAmount = (decimal)ntbStartAmount.Value;

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (changeHint == DataEntityChangeType.VariableChanged)
            {
                if (objectName == "CalculateDiscountsUpdate")
                {
                    if ((param != null) && store.CalculateDiscountsFrom != (LSOne.DataLayer.BusinessObjects.StoreManagement.Store.CalculateDiscountsFromEnum)param)
                    {
                        store.CalculateDiscountsFrom = (LSOne.DataLayer.BusinessObjects.StoreManagement.Store.CalculateDiscountsFromEnum)param;
                        CheckPriceAndDiscountSettings(false);
                    }
                }                
            }

            if(changeHint == DataEntityChangeType.Edit)
            {
                if(objectName == "StoreRegion" && changeIdentifier == store.ID)
                {
                    if(param == null)
                    {
                        cmbRegion.SelectedData = new DataEntity("", "");
                    }
                    else
                    {
                        cmbRegion.SelectedData = (DataEntity)param;
                    }
                }
            }
        }

        public void OnClose()
        {
 
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private LSOne.DataLayer.BusinessObjects.StoreManagement.Store.StorePriceSettingsEnum StorePriceIndexToEnum(int index)
        {
            return index == 0 ? LSOne.DataLayer.BusinessObjects.StoreManagement.Store.StorePriceSettingsEnum.PricesIncludeTax : LSOne.DataLayer.BusinessObjects.StoreManagement.Store.StorePriceSettingsEnum.PricesExcludeTax;
        }

        private int StorePriceSettingsToIndex()
        {
            if (store.StorePriceSetting == LSOne.DataLayer.BusinessObjects.StoreManagement.Store.StorePriceSettingsEnum.UsePriceGroupSettings)
            {
                orgStorePriceSettings = (int)LSOne.DataLayer.BusinessObjects.StoreManagement.Store.StorePriceSettingsEnum.UsePriceGroupSettings;
                store.StorePriceSetting = Providers.StoreData.GetPriceWithTaxForStore(PluginEntry.DataModel, store.ID) ? LSOne.DataLayer.BusinessObjects.StoreManagement.Store.StorePriceSettingsEnum.PricesIncludeTax : LSOne.DataLayer.BusinessObjects.StoreManagement.Store.StorePriceSettingsEnum.PricesExcludeTax;
            }

            switch (store.StorePriceSetting)
            {
                case LSOne.DataLayer.BusinessObjects.StoreManagement.Store.StorePriceSettingsEnum.PricesIncludeTax:
                    return 0;
                case LSOne.DataLayer.BusinessObjects.StoreManagement.Store.StorePriceSettingsEnum.PricesExcludeTax:
                    return 1;
                default:
                    return 0;
            }
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
                initialSearchText = ((DataEntity) cmbDefaultCustomer.SelectedData).ID;
                textInitallyHighlighted = true;
            }

            e.ControlToEmbed = new SingleSearchPanel(
            PluginEntry.DataModel,
            false,
            initialSearchText,
            SearchTypeEnum.Customers,
            textInitallyHighlighted);
        }

        private void cmbFunctionalProfile_RequestData(object sender, EventArgs e)
        {
            cmbFunctionalProfile.SetData(Providers.FunctionalityProfileData.GetList(PluginEntry.DataModel),
                null);
        }

        private void cmbSalesTaxGroup_RequestData(object sender, EventArgs e)
        {
            cmbSalesTaxGroup.SetData(Providers.SalesTaxGroupData.GetListWithTaxCodes(PluginEntry.DataModel), null);
        }

        private void ClearData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void btnEditFunctionalProfile_Click(object sender, EventArgs e)
        {
            functionalityProfileEditor.Message(this, "EditfunctionalityProfile", cmbFunctionalProfile.SelectedData.ID);
        }

        private void cmbDefaultCustomer_SelectedDataChanged(object sender, EventArgs e)
        {
            btnEditCustomer.Enabled = !(((DataEntity)cmbDefaultCustomer.SelectedData).ID == RecordIdentifier.Empty ||
                ((DataEntity)cmbDefaultCustomer.SelectedData).ID == "");            


        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            customerEditor.Message(this, "EditCustomer", ((DataEntity)cmbDefaultCustomer.SelectedData).ID);
        }

        private void btnEditItemSalesTaxGroup_Click(object sender, EventArgs e)
        {
            if (salesTaxGroupEditor.IsAlive)
            {
                ((IPlugin)salesTaxGroupEditor.Target).Message(this, "ViewSalesTaxGroups", cmbSalesTaxGroup.SelectedData.ID);
            }
        }

        private void cmbCurrencies_RequestData(object sender, EventArgs e)
        {
            cmbCurrencies.SetData(Providers.CurrencyData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbCurrencies_SelectedDataChanged(object sender, EventArgs e)
        {
            btnCurrenciesEdit.Enabled = (cmbCurrencies.SelectedData.ID != "") &&
                              PluginEntry.DataModel.HasPermission(Permission.CurrencyView) &&
                              currencyEditor != null;
        }
      
        private void btnCurrenciesEdit_Click(object sender, EventArgs e)
        {
            if (currencyEditor.IsAlive)
            {
                ((IPlugin)currencyEditor.Target).Message(this, "ViewCurrency", cmbCurrencies.SelectedData.ID);
            }
        }

        private void cmbTouchLayout_RequestData(object sender, EventArgs e)
        {
            cmbTouchLayout.SetData(Providers.TouchLayoutData.GetList(PluginEntry.DataModel),null);
        }

        private void btnEditTouchButtons_Click(object sender, EventArgs e)
        {
            if (layoutEditor.IsAlive)
            {
                ((IPlugin)layoutEditor.Target).Message(this, "EditLayout", cmbTouchLayout.SelectedData.ID);
            }
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            if (customerCreator.IsAlive)
            {
                ((IPlugin)customerCreator.Target).Message(this, "NewCustomer", null);
            }
        }

        private void cmbDefaultCustomer_RequestClear(object sender, EventArgs e)
        {
            cmbDefaultCustomer.SelectedData = new DataEntity("", "");
        }

        private void cmbSalesTaxGroup_SelectedDataChanged(object sender, EventArgs e)
        {
            if (oldSalesTaxID == cmbSalesTaxGroup.SelectedData.ID)
                return;

            // Update tax information if store is default store
            RecordIdentifier defaulstStoreID = Providers.StoreData.GetDefaultStoreID(PluginEntry.DataModel);
            oldSalesTaxID = cmbSalesTaxGroup.SelectedData.ID;

            if (defaulstStoreID == store.ID)
            {
                UpdateItemPricesTaxQuestionDialog dlg = new UpdateItemPricesTaxQuestionDialog();
                if (dlg.Show(PluginEntry.DataModel) == DialogResult.Yes)
                {
                    // Update item prices within a progress indicator
                    PluginEntry.Framework.ViewController.CurrentView.ShowProgress(delegate
                                                                                  {
                            int updatedItemsCount;
                            int updatedTradeAgreementsCount;
                            int updatedPromotionOfferLinesCount;

                            Services.Interfaces.Services.TaxService(PluginEntry.DataModel).UpdatePrices(
                                PluginEntry.DataModel,
                                new RecordIdentifier(defaulstStoreID,cmbSalesTaxGroup.SelectedData.ID),
                                UpdateItemTaxPricesEnum.DefaultStoreTaxGroup,
                                out updatedItemsCount,
                                out updatedTradeAgreementsCount,
                                out updatedPromotionOfferLinesCount);

                            PluginEntry.Framework.ViewController.CurrentView.HideProgress();

                            MessageDialog.Show(Properties.Resources.ItemsUpdated + ": " + updatedItemsCount + " \n" +
                                               Properties.Resources.TradeAgreementsUpdated + ": " + updatedTradeAgreementsCount + "\n" +
                                               Properties.Resources.PromotionLinesUpdated + ": " + updatedPromotionOfferLinesCount + "\n");
                        }, Properties.Resources.Processing);
                }
            }
        }
        
        private void cmbPriceTaxSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckPriceAndDiscountSettings(true);            
        }

        private void CheckPriceAndDiscountSettings(bool doNotify)
        {
            errorProvider.Clear();

            if (StorePriceIndexToEnum(cmbPriceTaxSetting.SelectedIndex) == LSOne.DataLayer.BusinessObjects.StoreManagement.Store.StorePriceSettingsEnum.PricesExcludeTax &&
                store.CalculateDiscountsFrom != LSOne.DataLayer.BusinessObjects.StoreManagement.Store.CalculateDiscountsFromEnum.Price)
            {
                errorProvider.SetError(cmbPriceTaxSetting, Properties.Resources.PriceSettingsAndCalculations);                
            }

            else if (StorePriceIndexToEnum(cmbPriceTaxSetting.SelectedIndex) == LSOne.DataLayer.BusinessObjects.StoreManagement.Store.StorePriceSettingsEnum.PricesIncludeTax &&
                store.CalculateDiscountsFrom != LSOne.DataLayer.BusinessObjects.StoreManagement.Store.CalculateDiscountsFromEnum.PriceWithTax)
            {
                errorProvider.SetError(cmbPriceTaxSetting, Properties.Resources.PriceSettingsAndCalculations);                
            }

            else if (StorePriceIndexToEnum(cmbPriceTaxSetting.SelectedIndex) == LSOne.DataLayer.BusinessObjects.StoreManagement.Store.StorePriceSettingsEnum.UsePriceGroupSettings)
            {
                errorProvider.SetError(cmbPriceTaxSetting, Properties.Resources.UseFirstPriceGroup);
                doNotify = false;
            }

            if (doNotify)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.VariableChanged, "PriceTaxSettingsUpdate", RecordIdentifier.Empty, StorePriceIndexToEnum(cmbPriceTaxSetting.SelectedIndex));
            }
        }

        private void cmbRegion_RequestData(object sender, EventArgs e)
        {
            cmbRegion.SetData(Providers.RegionData.GetList(PluginEntry.DataModel, DataLayer.BusinessObjects.StoreManagement.Region.SortEnum.Description, false), null);
        }

        private void btnEditRegion_Click(object sender, EventArgs e)
        {
            Region newRegion = PluginOperations.EditRegion(cmbRegion.SelectedData.ID);

            if (newRegion != null)
            {
                cmbRegion.SelectedData = newRegion;
            }
        }

        private void btnAddRegion_Click(object sender, EventArgs e)
        {
            Region newRegion = PluginOperations.EditRegion(RecordIdentifier.Empty);

            if (newRegion != null)
            {
                cmbRegion.SelectedData = newRegion;
            }
        }

        private void cmbRegion_RequestClear(object sender, EventArgs e)
        {

        }

        private void cmbRegion_SelectedDataChanged(object sender, EventArgs e)
        {
            btnEditRegion.Enabled = cmbRegion.SelectedData.ID != "";
        }
    }
}
