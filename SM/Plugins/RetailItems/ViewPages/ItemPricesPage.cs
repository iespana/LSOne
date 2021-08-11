using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.ViewPlugins.RetailItems.ViewPages.LogicHandlers;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Constants;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    public partial class ItemPricesPage : UserControl, ITabViewV2, IMultiEditTabExtension, IMessageTabExtension
    {
        WeakReference owner;
        WeakReference discountEditor;
        RetailItem item;
        Dictionary<string, object> stateData;

        public ItemPricesPage(TabControl owner)
            : this()
        {
            stateData = owner.ViewStateData;
            this.owner = new WeakReference(owner);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ItemPricesPage((TabControl)sender);
        }

        public ItemPricesPage()
        {
            IPlugin plugin;

            InitializeComponent();

            plugin = PluginEntry.Framework.FindImplementor(this, "ItemDiscountGroups", null);
            discountEditor = (plugin != null) ? new WeakReference(plugin) : null;
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
        }

        public void InitializeView(RecordIdentifier context, object internalContext)
        {
            tabSheetTabs.ViewStateData = stateData;

            if (internalContext != null)
            {
                item = (RetailItem)internalContext;
            }

            tabSheetTabs.InitializeViews(context,item);

            if (item.ItemType != ItemTypeEnum.MasterItem)
            {
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.BaseSalesPrice, ItemBaseSalesPricePage.CreateInstance) { LogicFactory = ItemBaseSalesPriceLogic.CreateInstance });

                if (item.ItemType != ItemTypeEnum.AssemblyItem 
                    && (CostCalculation)PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.CostCalculation, SettingsLevel.System).IntValue != CostCalculation.Manual)
                {
                    tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.CostPrice, ItemCostPage.CreateInstance));
                }
            }
            else
            {
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.BaseSalesPrice, ItemBaseSalePriceHeaderItemPage.CreateInstance));
            }

            // Allow other plugins to extend this tab panel
            tabSheetTabs.Broadcast(this, item.ID, item.ID == RecordIdentifier.Empty);
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            item = (RetailItem)internalContext;
       
            tabSheetTabs.SetData(isRevert, item.ID, item);
        }

        public bool DataIsModified()
        {
            return tabSheetTabs.IsModified();
        }

        public bool SaveData()
        {
            tabSheetTabs.GetData();
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

        public void MultiEditCollectData(IDataEntity dataEntity, HashSet<int> changedControlHashes, object param)
        {
            tabSheetTabs.MultiEditCollectData(dataEntity, changedControlHashes, param);
        }

        public bool MultiEditValidateSaveUnknownControls()
        {
            return tabSheetTabs.MultiEditValidateSaveUnknownControls();
        }

        public void MultiEditSaveSecondaryRecords(DataLayer.GenericConnector.Interfaces.IConnectionManager threadedConnection, IDataEntity dataEntity, RecordIdentifier primaryRecordID)
        {
            tabSheetTabs.MultiEditSaveSecondaryRecords(threadedConnection, dataEntity, primaryRecordID);
        }

        public void MultiEditRevertUnknownControl(Control control, bool isRevertField, ref bool handled)
        {
            tabSheetTabs.MultiEditRevertUnknownControl(control, isRevertField);
        }

        public void MultiEditSaveSecondaryRecordsFinalizer()
        {
            tabSheetTabs.MultiEditSaveSecondaryRecordsFinalizer();
        }

        public object OnViewPageMessage(object sender, string message, object param, ref bool handled)
        {
            switch (message)
            {
                case "GetItemSalesPrice":
                    if(tabSheetTabs != null)
                    {
                        handled = true;
                        return tabSheetTabs.SendViewPageMessage(this, "GetItemSalesPrice", null, out bool handledPrice);
                    }
                    break;

            }
            return null;
        }
    }
}