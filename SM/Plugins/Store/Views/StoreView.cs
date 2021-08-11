using System;
using System.Collections.Generic;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.Store.Views
{
    public partial class StoreView : ViewBase
    {
        RecordIdentifier storeID = "";
        LSOne.DataLayer.BusinessObjects.StoreManagement.Store store;
        string selectedTabKey;

        bool mainRecordIsDirty = false;

        public StoreView(RecordIdentifier storeID)
            : this()
        {
            this.storeID = storeID;


            this.ReadOnly = !(PluginEntry.DataModel.HasPermission(Permission.StoreEdit) &&
                (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty || PluginEntry.DataModel.CurrentStoreID == storeID));

            if (PluginEntry.DataModel.CurrentStoreID != RecordIdentifier.Empty)
            {
                Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close;
            }
        }

        public StoreView(RecordIdentifier storeID, string tabKey)
            :this(storeID)
        {
            selectedTabKey = tabKey;
        }

        private StoreView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("Store", storeID, Properties.Resources.StoreText, true));

            tabSheetTabs.GetAuditContexts(contexts);
        }

        public string Description
        {
            get
            {
                return Properties.Resources.StoreText + ": " + storeID + " - " + tbDescription.Text;
            }
        }

        public override string ContextDescription
        {
            get
            {
                return tbDescription.Text;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.StoreText;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return storeID;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            store = Providers.StoreData.Get(PluginEntry.DataModel, (string)storeID);

            if (!isRevert)
            {
                

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this,storeID, store);
            }

            tbID.Text = (string)store.ID;
            tbDescription.Text = store.Text;

            HeaderText = Description;
            //HeaderIcon = Properties.Resources.Store;

            tabSheetTabs.SetData(isRevert,storeID,store);

            if (selectedTabKey != "")
            {
                for (int i = 0; i < tabSheetTabs.TabCount; i++)
                {
                    if (tabSheetTabs[i].Key == selectedTabKey)
                    {
                        tabSheetTabs.SelectedTab = tabSheetTabs[i];
                        break;
                    }
                }
            }
            
        }

        protected override bool DataIsModified()
        {
            mainRecordIsDirty = true;

            if (tbDescription.Text != store.Text) return true;
            if (tabSheetTabs.IsModified()) return true;

            mainRecordIsDirty = false;

            return false;
        }

        protected override bool SaveData()
        {
            store.Text = tbDescription.Text;
            
            if (mainRecordIsDirty)
            {
                tabSheetTabs.GetData();
            }

            Providers.StoreData.Save(PluginEntry.DataModel, store);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Store", store.ID, null);
            PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.InitialConfigurationDashboardID);

            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "Store":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == storeID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    else if (changeHint == DataEntityChangeType.Edit && (changeIdentifier == storeID || changeIdentifier == RecordIdentifier.Empty))
                    {
                        LoadData(true);
                    }
                    break;

            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteStore(storeID);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.ViewAllStores, PluginOperations.ShowStoresListView), 200);
                arguments.Add(new ContextBarItem(Properties.Resources.Regions, PluginOperations.ShowRegionsView), 400);
            }
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }
    }
}
