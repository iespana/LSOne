using System;
using System.Collections.Generic;
using System.Linq;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory.Views
{
    public partial class VendorView : ViewBase
    {
        private RecordIdentifier vendorID = RecordIdentifier.Empty;
        private Vendor vendor;
        private TabControl.Tab generalTab;
        private TabControl.Tab notesTab;
        private TabControl.Tab retailItemsTab;
        private TabControl.Tab contactTab;
        int selectedTabIndex = -1;
        private bool saveData;

        IEnumerable<IDataEntity> recordBrowsingContext;

        public VendorView(RecordIdentifier vendorID, IEnumerable<IDataEntity> recordBrowsingContext)
            : this()
        {            
            this.recordBrowsingContext = recordBrowsingContext;
            this.vendorID = vendorID;
        }

        public VendorView(RecordIdentifier vendorID, IEnumerable<IDataEntity> recordBrowsingContext, int selectedTabIndex)
            : this(vendorID, recordBrowsingContext)
        {
            this.selectedTabIndex = selectedTabIndex;
        }

        public VendorView(RecordIdentifier vendorID)
            : this()
        {
            this.vendorID = vendorID;
        }

        public VendorView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.RecordCursor;

            this.ReadOnly = !(PluginEntry.DataModel.HasPermission(Permission.VendorEdit));
        }

        protected override ViewBase GetRecordCursorView(int cursorIndex)
        {
           if (recordBrowsingContext != null)
           {
                RecordIdentifier id = recordBrowsingContext.ElementAt<IDataEntity>(cursorIndex).ID;
                try
                {
                    var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                    if (service.VendorExists(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), id, true))
                    {
                        return new VendorView(recordBrowsingContext.ElementAt<IDataEntity>(cursorIndex).ID, recordBrowsingContext, tabSheetTabs.SelectedTabIndex);
                    }
                }
                catch (Exception)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                }
            
            }
            return null;
        }

        protected override void GetRecordCursorInfo(ContextBarCursorEventArguments args)
        {
            if (recordBrowsingContext != null)
            {
                args.Position = 0;
                args.Count = recordBrowsingContext.Count<IDataEntity>();

                foreach (IDataEntity entity in recordBrowsingContext)
                {
                    if (entity.ID == vendorID)
                    {
                        return;
                    }

                    args.Position++;
                }
            }
            else
            {
                args.Count = 1;
                args.Position = 0;
            }
        }
        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("Vendor", vendorID, Properties.Resources.Vendor, true));

            tabSheetTabs.GetAuditContexts(contexts);
        }

        public string Description
        {
            get
            {
                return Properties.Resources.Vendor + ": " + vendorID + " - " + tbDescription.Text;
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
                return Properties.Resources.Vendor;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return vendorID;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            try
            {
                var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                vendor = service.GetVendor(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), vendorID, true);
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                return;
            }

            if (vendor.Deleted)
            {
                this.ReadOnly = true;
                this.ReadOnlyReason = Resources.VendorIsDeleted;
                this.ReadOnlyReasonShort = Resources.VendorIsDeleted;
            }

            if (!isRevert)
            {
                generalTab = new TabControl.Tab(Properties.Resources.General, ViewPages.VendorGeneralPage.CreateInstance);
                contactTab = new TabControl.Tab(Properties.Resources.Contacts, ViewPages.VendorContactsPage.CreateInstance);
                retailItemsTab = new TabControl.Tab(Properties.Resources.RetailItems, ViewPages.VendorItemsPage.CreateInstance);
                notesTab = new TabControl.Tab(Properties.Resources.Notes, ViewPages.VendorNotesPage.CreateInstance);

                tabSheetTabs.AddTab(generalTab);
                tabSheetTabs.AddTab(retailItemsTab);
                tabSheetTabs.AddTab(contactTab);
                
                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this,vendorID, vendor);

                tabSheetTabs.AddTab(notesTab);
            }

            tbID.Text = (string)vendor.ID;
            tbDescription.Text = vendor.Text;
            
            HeaderText = Description;
            tabSheetTabs.SetData(isRevert,vendorID,vendor);

            if (selectedTabIndex != -1)
            {
                tabSheetTabs.SelectedTab = tabSheetTabs[selectedTabIndex];
            }
        }

        protected override bool DataIsModified()
        {
            vendor.Dirty = false;

            bool tabsModified = tabSheetTabs.IsModified();

            vendor.Dirty = vendor.Dirty |
                (tbDescription.Text != vendor.Text);


            return tabsModified | vendor.Dirty;
        }

        protected override bool SaveData()
        {
            tabSheetTabs.GetData();
            saveData = false;
            if (vendor.Dirty)
            {
                vendor.Text = tbDescription.Text;

                try
                {
                    saveData = true;

                    var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                    service.SaveVendor(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), vendor, true);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Vendor", vendor.ID, null);
                }
                catch (Exception)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                    saveData = false;
                }
            }
            return saveData;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "Vendor":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == vendorID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    else if (changeHint == DataEntityChangeType.MultiDelete)
                    {
                        if (((List<RecordIdentifier>)param).Contains(vendorID))
                        {
                            PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                        }
                    }

                    break;

            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteVendor(vendor);
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }
    }
}
