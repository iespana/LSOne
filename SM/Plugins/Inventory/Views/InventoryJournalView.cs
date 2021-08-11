using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Inventory.Properties;
using System;
using System.Collections.Generic;

namespace LSOne.ViewPlugins.Inventory.Views
{
    public partial class InventoryJournalView : ViewBase
    {
        private readonly IConnectionManager dlgEntry;
        private readonly SiteServiceProfile dlgSiteService;
        private readonly InventoryJournalTypeEnum journalType;
       
        private RecordIdentifier journalID = RecordIdentifier.Empty;
        private InventoryAdjustment journal;

       private TabControl.Tab summaryTab;
       private TabControl.Tab itemsTab;
       int selectedTabIndex = -1;
       private bool saveData;

        public override RecordIdentifier ID
        {
            get
            {
                return journalID;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                string typeOfAdjustment;

                switch (journalType)
                {
                    case InventoryJournalTypeEnum.Adjustment:
                        typeOfAdjustment = Resources.InventoryAdjustment;
                        break;
                    case InventoryJournalTypeEnum.Reservation:
                        typeOfAdjustment = Resources.StockReservation;
                        break;
                    case InventoryJournalTypeEnum.Parked:
                        typeOfAdjustment = Resources.ParkedInventory;
                        break;
                    default:
                        typeOfAdjustment = string.Empty;
                        break;
                }

                return typeOfAdjustment;
            }
        }

        public InventoryJournalView()
        {
            InitializeComponent();

            Attributes =
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help;
        }

        public InventoryJournalView(IConnectionManager entry, InventoryJournalTypeEnum journalType)
            : this(entry, PluginOperations.GetSiteServiceProfile(), journalType, RecordIdentifier.Empty, RecordIdentifier.Empty, 0)
        { }

        public InventoryJournalView(IConnectionManager entry, InventoryJournalTypeEnum journalType, RecordIdentifier journalId)
            : this(entry, PluginOperations.GetSiteServiceProfile(), journalType, journalId, RecordIdentifier.Empty, 0)
        { }

        public InventoryJournalView(IConnectionManager entry, SiteServiceProfile profile, 
                                    InventoryJournalTypeEnum journalType, 
                                    RecordIdentifier journalId, RecordIdentifier selectedStoreId,
                                    int selectedTabIndex)
            : this()
        {
            this.dlgEntry = entry;
            this.dlgSiteService = profile;
            this.journalType = journalType;

            this.journalID = journalId;
            
            this.selectedTabIndex = selectedTabIndex;
            HeaderText = string.Format( "{0} - {1} - ", LogicalContextName, journalID);
        }

        protected override HelpSettings GetOnlineHelpSettings()
        {
            switch (journalType)
            {
                case InventoryJournalTypeEnum.Adjustment:
                    return new HelpSettings("InventoryAdjustmentView");

                case InventoryJournalTypeEnum.Reservation:
                    return new HelpSettings("StockReservationView");

                case InventoryJournalTypeEnum.Parked:
                    return new HelpSettings("ParkedInventoryView");
                default:
                    return new HelpSettings(this.Name);
            }

        }

        #region ViewBase

        protected override void LoadData(bool isRevert)
        {
            try
            {
                var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                journal = service.GetInventoryAdjustment(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), journalID, true);
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                return;
            }

            if (!isRevert)
            {
                summaryTab = new TabControl.Tab(Resources.Summary, "InventoryJournalSummaryTab", ViewPages.InventoryJournalSummaryPage.CreateInstance);
                itemsTab = new TabControl.Tab(Resources.Items, "InventoryJournalItemsTab", ViewPages.InventoryJournalItemsPage.CreateInstance);

                tabSheetTabs.AddTab(itemsTab);
                tabSheetTabs.AddTab(summaryTab);

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, journalID, journal);
            }

            tabSheetTabs.SetData(isRevert, journalID, journal);

            if (selectedTabIndex != -1)
            {
                tabSheetTabs.SelectedTab = tabSheetTabs[selectedTabIndex];
            }
        }

        protected override bool DataIsModified()
        {
            bool tabsModified = tabSheetTabs.IsModified();
            return tabsModified;
        }

        protected override bool SaveData()
        {
            tabSheetTabs.GetData();
            saveData = false;

            // Added items are saved in the DB by InventoryAdjustmentDialog. No save is required here.

            return saveData;
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
           tabSheetTabs.BroadcastChangeInformation(changeAction, objectName, changeIdentifier, param);
        }

        #endregion
    }
}
