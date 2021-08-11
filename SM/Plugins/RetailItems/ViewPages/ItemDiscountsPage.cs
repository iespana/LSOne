using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.BusinessObjects.ItemMaster.MultiEditing;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    public partial class ItemDiscountsPage : UserControl, ITabViewV2, IMultiEditTabExtension
    {
        WeakReference owner;
        WeakReference discountEditor;
        RetailItem item;
        private bool hasPermission;

        public ItemDiscountsPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
            hasPermission = PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ManageTradeAgreementPrices);

            cmbSalesLineDiscount.Enabled = hasPermission;
            btnEditSalesLineDiscount.Enabled = hasPermission;
            cmbSalesMultilineDiscount.Enabled = hasPermission;
            btnEditSalesMultilineDiscount.Enabled = hasPermission;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.ItemDiscountsPage((TabControl)sender);
        }

        public ItemDiscountsPage()
        {
            IPlugin plugin;

            InitializeComponent();

            plugin = PluginEntry.Framework.FindImplementor(this, "ItemDiscountGroups", null);
            discountEditor = (plugin != null) ? new WeakReference(plugin) : null;
            btnEditSalesLineDiscount.Visible = btnEditSalesMultilineDiscount.Visible = (discountEditor != null);
            
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
        }

        public void InitializeView(RecordIdentifier context, object internalContext)
        {
            if (internalContext != null)
            {
                item = (RetailItem)internalContext;

                if(item.ID == RecordIdentifier.Empty)
                {
                    // Multiedit

                    cmbSalesLineDiscount.SetSelectionToNoChange(false);
                    cmbSalesMultilineDiscount.SetSelectionToNoChange(false);
                }
            }

            // Allow other plugins to extend this tab panel
            if (item.ID == RecordIdentifier.Empty)
            {
                var dict = new Dictionary<string, object>();
                dict["Item"] = item;
                dict["LineDiscount"] = new RecordIdentifier("");
                dict["MultilineDiscount"] = new RecordIdentifier("");

                tabSheetTabs.Visible = false; // Temporary while we dont support the tabs in multiedit.

                tabSheetTabs.InitializeViews(context,dict);
            }

            tabSheetTabs.Broadcast(this, item.ID);

            
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            item = (RetailItem)internalContext;

            cmbSalesLineDiscount.SelectedData = new DataEntity(item.SalesLineDiscount, item.SalesLineDiscountName);
            cmbSalesMultilineDiscount.SelectedData = new DataEntity(item.SalesMultiLineDiscount, item.SalesMultiLineDiscountName);

            chkSalesTotalDiscount.Checked = item.SalesAllowTotalDiscount;

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict["Item"] = item;
            dict["LineDiscount"] = cmbSalesLineDiscount.SelectedData.ID;
            dict["MultilineDiscount"] = cmbSalesMultilineDiscount.SelectedData.ID;

            tabSheetTabs.SetData(isRevert, item.ID, dict);
        }

        public bool DataIsModified()
        {
            if (chkSalesTotalDiscount.Checked != item.SalesAllowTotalDiscount) item.Dirty = true;
            if (cmbSalesLineDiscount.SelectedData.ID != item.SalesLineDiscount) item.Dirty = true;
            if (cmbSalesMultilineDiscount.SelectedData.ID != item.SalesMultiLineDiscount) item.Dirty = true;

            return item.Dirty;
        }

        public bool SaveData()
        {
            if (item.Dirty)
            {
                item.SalesAllowTotalDiscount = chkSalesTotalDiscount.Checked;
                item.SalesLineDiscount = cmbSalesLineDiscount.SelectedData.ID;
                item.SalesMultiLineDiscount = cmbSalesMultilineDiscount.SelectedData.ID;
            }

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        private void btnEditSalesLineDiscount_Click(object sender, EventArgs e)
        {
            if (discountEditor.IsAlive)
            {
                ((IPlugin)discountEditor.Target).Message(this, "ViewItemDiscountGroups", 1);
            }
        }

        private void btnEditSalesMultilineDiscount_Click(object sender, EventArgs e)
        {
            if (discountEditor.IsAlive)
            {
                ((IPlugin)discountEditor.Target).Message(this, "ViewItemDiscountGroups", 2);
            }
        }

        private void cmbSalesLineDiscount_RequestData(object sender, EventArgs e)
        {
            cmbSalesLineDiscount.SetData(Providers.PriceDiscountGroupData.GetGroupList(PluginEntry.DataModel, PriceDiscountModuleEnum.Item, PriceDiscGroupEnum.LineDiscountGroup),null);
        }

        private void cmbSalesMultilineDiscount_RequestData(object sender, EventArgs e)
        {
            cmbSalesMultilineDiscount.SetData(Providers.PriceDiscountGroupData.GetGroupList(PluginEntry.DataModel, PriceDiscountModuleEnum.Item, PriceDiscGroupEnum.MultilineDiscountGroup),null);
        }

        private void cmbSalesLineDiscount_SelectedDataChanged(object sender, EventArgs e)
        {
            var dict = new Dictionary<string, object>();
            dict["Item"] = item;
            dict["LineDiscount"] = cmbSalesLineDiscount.SelectedData.ID;
            dict["MultilineDiscount"] = cmbSalesMultilineDiscount.SelectedData.ID;
            tabSheetTabs.UpdateDataHandler(item.ID, dict);
        }

        private void cmbSalesMultilineDiscount_SelectedDataChanged(object sender, EventArgs e)
        {
            var dict = new Dictionary<string, object>();
            dict["Item"] = item;
            dict["LineDiscount"] = cmbSalesLineDiscount.SelectedData.ID;
            dict["MultilineDiscount"] = cmbSalesMultilineDiscount.SelectedData.ID;

            tabSheetTabs.UpdateDataHandler(item.ID, dict);
        }

        private void cmbSalesMultilineDiscount_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void cmbSalesLineDiscount_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        public void MultiEditCollectData(IDataEntity dataEntity, HashSet<int> changedControlHashes, object param)
        {
            RetailItemMultiEdit itemObject = (RetailItemMultiEdit)dataEntity;

            if (changedControlHashes.Contains(chkSalesTotalDiscount.GetHashCode()))
            {
                itemObject.SalesAllowTotalDiscount = chkSalesTotalDiscount.Checked;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.SalesAllowTotalDiscount;
            }

            if (changedControlHashes.Contains(cmbSalesLineDiscount.GetHashCode()))
            {
                itemObject.SalesLineDiscount = cmbSalesLineDiscount.SelectedData.ID;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.SalesLineDiscount;
            }

            if (changedControlHashes.Contains(cmbSalesMultilineDiscount.GetHashCode()))
            {
                itemObject.SalesMultiLineDiscount = cmbSalesMultilineDiscount.SelectedData.ID;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.SalesMultiLineDiscount;
            }
        }

        public bool MultiEditValidateSaveUnknownControls()
        {
            return false;
        }

        public void MultiEditSaveSecondaryRecords(DataLayer.GenericConnector.Interfaces.IConnectionManager threadedConnection, IDataEntity dataEntity, RecordIdentifier primaryRecordID)
        {
            
        }

        public void MultiEditRevertUnknownControl(Control control, bool isRevertField, ref bool handled)
        {
            
        }

        public void MultiEditSaveSecondaryRecordsFinalizer()
        {
            
        }
    }
}
