using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    internal partial class StoreManagementAdminPage : UserControl, ITabViewV2
    {
        Parameters parameters;
        DiscountCalculation discountInfo;
        RecordIdentifier currentlySelectedStoreID;
        WeakReference salesTaxGroupReference;

        public StoreManagementAdminPage()
        {
            IPlugin plugin;

            InitializeComponent();

            plugin = PluginEntry.Framework.FindImplementor(this, "ViewSalesTaxGroups", null);
            salesTaxGroupReference = plugin != null ? new WeakReference(plugin) : null;
            btnEditTaxGroup.Visible = (salesTaxGroupReference != null);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.StoreManagementAdminPage();
        }

        public DataEntity SelectedTaxExemptGroup { get { return (DataEntity)cmbTaxExemptTaxGroup.SelectedData; } }

        #region ITabPanel Members

        public void InitializeView(RecordIdentifier context, object internalContext)
        {

        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            parameters = (Parameters)internalContext;
            cmbLocalStore.SelectedData = new DataEntity(parameters.LocalStore, parameters.LocalStoreName);
            cmbTaxExemptTaxGroup.SelectedData = new DataEntity();
            if (!RecordIdentifier.IsEmptyOrNull(parameters.TaxExemptTaxGroup))
            {
                SalesTaxGroup taxExemptGroup = Providers.SalesTaxGroupData.Get(PluginEntry.DataModel, parameters.TaxExemptTaxGroup);
                cmbTaxExemptTaxGroup.SelectedData = new DataEntity(taxExemptGroup.ID, taxExemptGroup.Text);
            }
            chkManuallyEnterItemIDs.Checked = parameters.ManuallyEnterItemID;
            chkManuallyEnterCustomerIDs.Checked = parameters.ManuallyEnterCustomerID;
            chkManuallyEnterVendorIDs.Checked = parameters.ManuallyEnterVendorID;
            chkManuallyEnterStoreIDs.Checked = parameters.ManuallyEnterStoreID;
            chkManuallyEnterTerminalIDs.Checked = parameters.ManuallyEnterTerminalID;
            chkManuallyEnterUnitIDs.Checked = parameters.ManuallyEnterUnitID;
            chkManuallyEnterTaxCodeIDs.Checked = parameters.ManuallyEnterTaxCodeID;
            chkManuallyEnterTaxGroupIDs.Checked = parameters.ManuallyEnterTaxGroupID;
            chkManuallyEnterGiftCardID.Checked = parameters.ManuallyEnterGiftCardID;

            currentlySelectedStoreID = parameters.LocalStore;

            btnEditStore.Enabled = (cmbLocalStore.SelectedData.ID != "");

            discountInfo = Providers.DiscountCalculationData.Get(PluginEntry.DataModel);            

            DataChanged(this, EventArgs.Empty);
        }

        public bool DataIsModified()
        {
            if (cmbLocalStore.SelectedData.ID != parameters.LocalStore) parameters.Dirty = true;
            if (cmbTaxExemptTaxGroup.SelectedData.ID != parameters.TaxExemptTaxGroup) parameters.Dirty = true;
            if (chkManuallyEnterItemIDs.Checked != parameters.ManuallyEnterItemID) parameters.Dirty = true;
            if (chkManuallyEnterCustomerIDs.Checked != parameters.ManuallyEnterCustomerID) parameters.Dirty = true;
            if (chkManuallyEnterVendorIDs.Checked != parameters.ManuallyEnterVendorID) parameters.Dirty = true;
            if (chkManuallyEnterStoreIDs.Checked != parameters.ManuallyEnterStoreID) parameters.Dirty = true;
            if (chkManuallyEnterTerminalIDs.Checked != parameters.ManuallyEnterTerminalID) parameters.Dirty = true;
            if (chkManuallyEnterUnitIDs.Checked != parameters.ManuallyEnterUnitID) parameters.Dirty = true;
            if (chkManuallyEnterTaxCodeIDs.Checked != parameters.ManuallyEnterTaxCodeID) parameters.Dirty = true;
            if (chkManuallyEnterTaxGroupIDs.Checked != parameters.ManuallyEnterTaxGroupID) parameters.Dirty = true;
            if (chkManuallyEnterGiftCardID.Checked != parameters.ManuallyEnterGiftCardID) parameters.Dirty = true;

            return parameters.Dirty;
        }

        public bool SaveData()
        {
            parameters.LocalStore = cmbLocalStore.SelectedData.ID;
            parameters.TaxExemptTaxGroup = cmbTaxExemptTaxGroup.SelectedData.ID;
            
            parameters.ManuallyEnterItemID = chkManuallyEnterItemIDs.Checked;
            parameters.ManuallyEnterCustomerID = chkManuallyEnterCustomerIDs.Checked;
            parameters.ManuallyEnterVendorID = chkManuallyEnterVendorIDs.Checked;
            parameters.ManuallyEnterStoreID = chkManuallyEnterStoreIDs.Checked;
            parameters.ManuallyEnterTerminalID = chkManuallyEnterTerminalIDs.Checked;
            parameters.ManuallyEnterUnitID = chkManuallyEnterUnitIDs.Checked;
            parameters.ManuallyEnterTaxCodeID = chkManuallyEnterTaxCodeIDs.Checked;
            parameters.ManuallyEnterTaxGroupID = chkManuallyEnterTaxGroupIDs.Checked;
            parameters.ManuallyEnterGiftCardID = chkManuallyEnterGiftCardID.Checked;

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("StoreManagementDiscounts", 0, Properties.Resources.StoreManagementDiscounts, true));
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
            LoadData(false, RecordIdentifier.Empty, internalContext);
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void cmbFormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void ClearData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
            DataChanged(this, EventArgs.Empty);
        }

        private void cmbLocalStore_RequestData(object sender, EventArgs e)
        {
            cmbLocalStore.SetData(Providers.StoreData.GetList(PluginEntry.DataModel), PluginEntry.Framework.GetImageList().Images[PluginEntry.StoreImageIndex]);
            cmbLocalStore.SetHeaders(new[] { "Picture", "ID", "Text" }, new int[] { 0, 1, 2 });
        }

        private void DataChanged(object sender, EventArgs e)
        {
            btnEditStore.Enabled = (cmbLocalStore.SelectedData.ID != "") && PluginEntry.DataModel.HasPermission(Permission.StoreView);
            btnEditTaxGroup.Enabled = PluginEntry.DataModel.HasPermission(Permission.EditSalesTaxSetup);            

            chkManuallyEnterItemIDs.Enabled = PluginEntry.DataModel.HasPermission(Permission.StoreView);
            chkManuallyEnterCustomerIDs.Enabled = PluginEntry.DataModel.HasPermission(Permission.StoreView);
            chkManuallyEnterVendorIDs.Enabled = PluginEntry.DataModel.HasPermission(Permission.StoreView);
            chkManuallyEnterTaxCodeIDs.Enabled = PluginEntry.DataModel.HasPermission(Permission.StoreView) && PluginEntry.DataModel.HasPermission(Permission.EditSalesTaxSetup);
            chkManuallyEnterTaxGroupIDs.Enabled = PluginEntry.DataModel.HasPermission(Permission.StoreView) && PluginEntry.DataModel.HasPermission(Permission.EditSalesTaxSetup);
            chkManuallyEnterGiftCardID.Enabled = PluginEntry.DataModel.HasPermission(Permission.StoreView) && PluginEntry.DataModel.HasPermission(Permission.ManageGiftCards);

            // Ask to update prices if the new selected store has a different tax group from the previously selected
            var storeProvider = Providers.StoreData;
            var previousStoreTaxGroup = storeProvider.GetStoresSalesTaxGroupID(PluginEntry.DataModel, currentlySelectedStoreID);
            var currentStoreTaxGroup = storeProvider.GetStoresSalesTaxGroupID(PluginEntry.DataModel, cmbLocalStore.SelectedData.ID);
            var selectedStoreID = cmbLocalStore.SelectedData.ID;

            if (previousStoreTaxGroup != currentStoreTaxGroup)
	        {
                UpdateItemPricesTaxQuestionDialog dlg = new UpdateItemPricesTaxQuestionDialog();
	            dlg.DefaultStoreID = selectedStoreID;
		         if (dlg.Show(PluginEntry.DataModel) == DialogResult.Yes)
                {
                    UpdatePrices(UpdateItemTaxPricesEnum.DefaultStoreTaxGroup, new RecordIdentifier(selectedStoreID,currentStoreTaxGroup));
                }
	        }

            currentlySelectedStoreID = cmbLocalStore.SelectedData.ID;
            
        }

        private void UpdatePrices(UpdateItemTaxPricesEnum updateItemTaxPricesEnum, RecordIdentifier currentStoreTaxGroup)
        {
            // Update item prices within a progress indicator
            PluginEntry.Framework.ViewController.CurrentView.ShowProgress(delegate(System.Object o, System.EventArgs ea)
                {
                    int updatedItemsCount;
                    int updatedTradeAgreementsCount;
                    int updatedPromotionOfferLinesCount;

                    Services.Interfaces.Services.TaxService(PluginEntry.DataModel).UpdatePrices(
                        PluginEntry.DataModel,
                        currentStoreTaxGroup,
                        updateItemTaxPricesEnum,
                        out updatedItemsCount,
                        out updatedTradeAgreementsCount,
                        out updatedPromotionOfferLinesCount);

                    PluginEntry.Framework.ViewController.CurrentView.HideProgress();

                    MessageDialog.Show(Properties.Resources.ItemsUpdated + ": " + updatedItemsCount + " \n" +
                                       Properties.Resources.TradeAgreementsUpdated + ": " + updatedTradeAgreementsCount + "\n" +
                                       Properties.Resources.PromotionLinesUpdated + ": " + updatedPromotionOfferLinesCount + "\n");
                }, Properties.Resources.Processing);
        }

        private void btnEditStore_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowStore(cmbLocalStore.SelectedData.ID);
        }

        private void btnRecalculatePrices_Click(object sender, EventArgs e)
        {
            UpdateItemPricesTaxQuestionDialog dlg = new UpdateItemPricesTaxQuestionDialog();
            if (dlg.Show(PluginEntry.DataModel) == DialogResult.Yes)
            {
                UpdatePrices(UpdateItemTaxPricesEnum.AllItems, cmbLocalStore.SelectedData.ID);
            }
        }

        private void StoreManagementAdminPage_Load(object sender, EventArgs e)
        {

        }

        private void cmbTaxExemptTaxGroup_RequestData(object sender, EventArgs e)
        {
            cmbTaxExemptTaxGroup.SetData(Providers.SalesTaxGroupData.GetListWithTaxCodes(PluginEntry.DataModel), null);            
        }

        private void cmbTaxExemptTaxGroup_RequestClear(object sender, EventArgs e)
        {
            cmbTaxExemptTaxGroup.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
        }

        private void btnNewTaxGroup_Click(object sender, EventArgs e)
        {
            if (salesTaxGroupReference.IsAlive)
            {
                ((IPlugin)salesTaxGroupReference.Target).Message(this, "ViewSalesTaxGroups", cmbTaxExemptTaxGroup.SelectedData.ID);
            }
        }

        private void cmbTaxExemptTaxGroup_SelectedDataChanged(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (parameters.TaxExemptTaxGroup.StringValue != cmbTaxExemptTaxGroup.SelectedDataID.StringValue)
            {
                errorProvider.Icon = Icon.FromHandle(((Bitmap)PluginEntry.Framework.GetImage(ImageEnum.InformationErrorProvider)).GetHicon());
                errorProvider.SetError(btnEditTaxGroup, Properties.Resources.AllCustomersMarkedAsTaxExemptWillUseThisTaxGroup);                
            }
        }
    }
}
