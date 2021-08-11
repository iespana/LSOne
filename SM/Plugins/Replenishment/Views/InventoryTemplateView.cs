using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Replenishment.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LSOne.ViewPlugins.Replenishment.Views
{
    public partial class InventoryTemplateView : ViewBase
    {
        RecordIdentifier templateID;
        InventoryTemplate template;
        private DataEntitySelectionList selectionList;
        private List<InventoryTemplateStoreConnection> storeConnections;
        TabControl.Tab filterTab;
        TabControl.Tab generalTab;
        TabControl.Tab worksheetSettingsTab;
        TabControl.Tab stockCountingTab;
        TabControl.Tab transferOrdersTab;
        private bool multipleStoresSelected;
        private bool allStoresSelected;

        public InventoryTemplateView(RecordIdentifier templateID)
            : this()
        {
            this.templateID = templateID;
            try
            {
                template =
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                        .GetInventoryTemplate(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),
                            templateID, false);
                storeConnections =
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                        .GetInventoryTemplateStoreConnectionList(PluginEntry.DataModel,
                            PluginOperations.GetSiteServiceProfile(), template.ID, false);

                multipleStoresSelected = storeConnections.FindAll(sc => sc.TemplateID == templateID).Count > 1;

                allStoresSelected = template.AllStores;

                if (PluginEntry.DataModel.IsHeadOffice)
                {
                    ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageInventoryTemplates);
                }
                else
                {
                    ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageInventoryTemplates) ||
                               allStoresSelected || multipleStoresSelected;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }
        }

        private InventoryTemplateView()
        {
            InitializeComponent();
            
            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            HeaderText = Resources.InventoryTemplate;
        }

        public string Description
        {
            get
            {
                return Resources.InventoryTemplate + ": " + (string)templateID + " - " + tbDescription.Text;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.InventoryTemplate;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return templateID;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            tbID.Text = (string)templateID;
            tbDescription.Text = template.Text;

            var selectedStores = Providers.StoreData.GetList(PluginEntry.DataModel);
            selectionList = new DataEntitySelectionList(selectedStores);

            checkBox1.Checked = template.AllStores;
            ResolveSelection(selectionList, storeConnections);

            DataEntitySelectionList clonedList = new DataEntitySelectionList(CollectionHelper.Clone<DataEntity, List<DataEntity>>(selectedStores));
            ResolveSelection(clonedList, storeConnections);
            cmbStore.SelectedData = clonedList;

            if (!isRevert)
            {
                filterTab = new TabControl.Tab(Resources.ItemFilter, ViewPages.InventoryTemplateFilterPage.CreateInstance);
                generalTab = new TabControl.Tab(Resources.General, ViewPages.InventoryTemplateGeneralPage.CreateInstance);
                worksheetSettingsTab = new TabControl.Tab(Resources.PurchaseWorksheetSettings, template.TemplateEntryType == TemplateEntryTypeEnum.PurchaseOrder, ViewPages.InventoryTemplateWorksheetSettingsPage.CreateInstance);
                stockCountingTab = new TabControl.Tab(Resources.StockCounting, template.TemplateEntryType == TemplateEntryTypeEnum.StockCounting, ViewPages.InventoryTemplateStockCountingPage.CreateInstance);
                transferOrdersTab = new TabControl.Tab(Resources.TransferOrders, template.TemplateEntryType == TemplateEntryTypeEnum.TransferStock, ViewPages.InventoryTemplateStockTransfersPage.CreateInstance);

                // This is done because the filterTab requires earlier access to the entity
                filterTab.Tag = template; 

                tabSheetTabs.AddTab(filterTab);
                tabSheetTabs.AddTab(generalTab);

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, templateID, template);

                tabSheetTabs.AddTab(worksheetSettingsTab);
                tabSheetTabs.AddTab(stockCountingTab);
                tabSheetTabs.AddTab(transferOrdersTab);

                tabSheetTabs.SelectedTab = filterTab.Enabled ? filterTab : generalTab;
            }

            var internalContext = new List<object>();
            internalContext.Add(template);
            internalContext.Add(ReadOnly);

            tabSheetTabs.SetData(isRevert, templateID, internalContext);
            HeaderText = Description;

            if (ReadOnly)
            {
                tbDescription.Enabled = checkBox1.Enabled = cmbStore.Enabled = false;
            }
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("InventoryTemplate", templateID, Resources.InventoryTemplate, true));

            tabSheetTabs.GetAuditContexts(contexts);
        }

        protected override bool DataIsModified()
        {
            // Here we are supposed to validate if there is need to save
            template.Dirty = false;

            template.Dirty = (template.Text != tbDescription.Text);

            if (template.AllStores != checkBox1.Checked)
            {
                template.Dirty = true;
                return true;
            }

            var currentList = ((DataEntitySelectionList)cmbStore.SelectedData).Selections;
            var oldList = selectionList.Selections;

            for (int i = 0; i < currentList.Count; i++)
            {
                if (currentList[i] != oldList[i])
                {
                    template.Dirty = true;
                    return true;
                }
            }

            bool tabsModified = tabSheetTabs.IsModified();
            if (tabsModified || template.Dirty) return true;

            return false;
        }

        protected override bool SaveData()
        {
            //Validate stores
            var currentList = ((DataEntitySelectionList)cmbStore.SelectedData).Selections;

            if (!checkBox1.Checked && !currentList.Any(x => x))
            {
                MessageDialog.Show(Resources.NoStoresSelected);
                return true; //return true to allow users to change the view even if it's not valid
            }

            tabSheetTabs.GetData();

            template.Text = tbDescription.Text;
            template.AllStores = checkBox1.Checked;
            try
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                    .SaveInventoryTemplate(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), template, false);

                if (!template.AllStores)
                {
                    var oldList = selectionList.Selections;

                    for (var i = 0; i < currentList.Count; i++)
                    {
                        if (currentList[i] != oldList[i])
                        {
                            if (currentList[i])
                            {
                                var store = ((DataEntitySelectionList) cmbStore.SelectedData).Get(i);
                                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                                    .SaveInventoryTemplateStoreConnection(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),
                                        new InventoryTemplateStoreConnection
                                        {
                                            StoreID = store.ID,
                                            TemplateID = template.ID
                                        }, false);
                            }
                            else
                            {
                                var storeEntity = selectionList.Get(i);
                                if (storeEntity != null)
                                {
                                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                                        .DeleteInventoryTemplateStoreConnection(PluginEntry.DataModel,
                                            PluginOperations.GetSiteServiceProfile(), storeEntity.ID, false);
                                }
                            }
                        }
                    }

                    //Repopulate selection list
                    selectionList = new DataEntitySelectionList(((DataEntitySelectionList)cmbStore.SelectedData).Entities);
                    var connections =
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                        .GetInventoryTemplateStoreConnectionList(PluginEntry.DataModel,
                            PluginOperations.GetSiteServiceProfile(), template.ID, false);
                    ResolveSelection(selectionList, connections);
                }
                else
                {
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                        .DeleteInventoryTemplateTemplateConnection(PluginEntry.DataModel,PluginOperations.GetSiteServiceProfile(), template.ID, false);

                    //Reset selection list
                    selectionList = new DataEntitySelectionList(((DataEntitySelectionList)cmbStore.SelectedData).Entities);
                }

                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                    .SaveInventoryTemplate(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), template, false);

                templateID = template.ID;
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit,
                    "InventoryTemplate", templateID, template);
                var purchaseWorksheets = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).
                    GetInventoryWorksheetListByInventoryTemplate(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), templateID, false) ?? new List<PurchaseWorksheet>();

                if(template.TemplateEntryType == TemplateEntryTypeEnum.PurchaseOrder)
                {
                    if (template.AllStores)
                    {
                        var stores = Providers.StoreData.GetList(PluginEntry.DataModel);
                        foreach (var store in stores)
                        {
                            PurchaseWorksheet tempWorksheet = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetWorksheetFromTemplateIDAndStoreID(
                                PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), templateID, store.ID, false);

                            PurchaseWorksheet worksheet = new PurchaseWorksheet();
                            worksheet.StoreId = store.ID;
                            worksheet.ID = tempWorksheet?.ID;
                            worksheet.InventoryTemplateID = templateID;
                            Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).SaveInventoryWorksheet(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), worksheet, false);
                        }
                    }
                    else
                    {
                        var connections = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryTemplateStoreConnectionList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), templateID, false);

                        if (connections != null)
                        {
                            foreach (var inventoryTemplateStoreConnection in connections)
                            {
                                var worksheet =
                                    purchaseWorksheets.Find(f => f.StoreId == inventoryTemplateStoreConnection.StoreID);
                                if (worksheet == null)
                                {
                                    PurchaseWorksheet newWorksheet = new PurchaseWorksheet();
                                    newWorksheet.StoreId = inventoryTemplateStoreConnection.StoreID;
                                    newWorksheet.InventoryTemplateID = templateID;
                                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).SaveInventoryWorksheet(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), newWorksheet, false);
                                }
                                else
                                {
                                    purchaseWorksheets.Remove(worksheet);
                                }
                            }
                            foreach (var current in purchaseWorksheets)
                            {
                                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).DeleteInventoryWorksheet(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), current.ID, false);
                                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).DeleteInventoryWorksheetLineForPurchaseWorksheet(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), current.ID, false);
                            }
                        }
                    }
                }

                HeaderText = Description;

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return false;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteInventoryTemplate(templateID);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            cmbStore.Enabled = !checkBox1.Checked;
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {
            var stores = Providers.StoreData.GetList(PluginEntry.DataModel);
            stores.Insert(0, new DataEntity("", Resources.AllStores));
            cmbStore.SetData(stores, null);
        }

        private void cmbStore_DropDown(object sender, DropDownEventArgs e)
        {
            if (((DualDataComboBox)sender).SelectedData != null)
            {
                e.ControlToEmbed = new CheckBoxSelectionListPanel((DataEntitySelectionList)((DualDataComboBox)sender).SelectedData);
            }
        }

        private static void ResolveSelection(DataEntitySelectionList selectionList, IEnumerable<InventoryTemplateStoreConnection> selectedList)
        {
            foreach (var dataEntity in selectedList)
            {
                selectionList.SetSelected(dataEntity.StoreID, true);
            }
        }

        protected override void OnSetupContextBarItems(
            ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType() + ".Related")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ViewInventoryTransferRequests))
                {
                    arguments.Add(
                        new ContextBarItem(Resources.PurchaseWorksheets, null, PluginOperations.ShowPurchaseWorksheets), 100);
                }
            }
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "InventoryTemplate":
                    if (changeAction == DataEntityChangeType.Delete && changeIdentifier == templateID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;
            }

            if (changeIdentifier == template.ID)
            {
                tabSheetTabs.BroadcastChangeInformation(changeAction, objectName, changeIdentifier, param);
            }
        }
    }
}