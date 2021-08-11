using System;
using System.Collections.Generic;
using System.Linq;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.DataLayer.BusinessObjects.ItemMaster.MultiEditing;
using LSOne.ViewCore.Dialogs;
using System.Drawing;
using System.Threading.Tasks;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.ViewPlugins.RetailItems.Properties;
using LSOne.ViewPlugins.RetailItems.ViewPages.LogicHandlers;

namespace LSOne.ViewPlugins.RetailItems.Views
{
    public partial class ItemView : ViewBase
    {
        RecordIdentifier itemID;
        RetailItem item;
        IEnumerable<IDataEntity> recordBrowsingContext;
        string selectedTabKey = "";
        RecordIdentifier originalRetailGroupID;

        Image retailItemImage16;
        Image variantItem16;
        Image masterItem16;
        Image serviceItem16;
        Image assemblyItem16;

        public ItemView(RecordIdentifier itemID, IEnumerable<IDataEntity> recordBrowsingContext, string selectedTabKey)
            : this(itemID, recordBrowsingContext)
        {
            this.selectedTabKey = selectedTabKey;
        }

        public ItemView(IEnumerable<IDataEntity> items)
        {
            // This constructor is used for multiedit
            retailItemImage16 = Resources.item_16;
            variantItem16 = Resources.itemVariant_16;
            masterItem16 = Resources.header_item;
            serviceItem16 = Resources.service_items_16;
            assemblyItem16 = Resources.itemassembly_16px;

            itemID = RecordIdentifier.Empty;
            this.recordBrowsingContext = items;

            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Close |
                ViewAttributes.Delete |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.MultiEdit;

            this.ReadOnly = false;
        }       
        

        public ItemView (RecordIdentifier itemID, IEnumerable<IDataEntity> recordBrowsingContext)
            : this()
	    {
            this.recordBrowsingContext = recordBrowsingContext;
            this.itemID = itemID;
	    }

        public ItemView()
        {
            retailItemImage16 = Resources.item_16;
            variantItem16 = Resources.itemVariant_16;
            masterItem16 = Resources.header_item;
            serviceItem16 = Resources.service_items_16;
            assemblyItem16 = Resources.itemassembly_16px;

            itemID = RecordIdentifier.Empty;

            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.Delete |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.RecordCursor;

            this.ReadOnly = !PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if(MultiEditMode)
            {
                Image itemImage = null;

                foreach(SimpleRetailItem dataEntity in recordBrowsingContext)
                {

                    switch (dataEntity.ItemType)
                    {
                        case ItemTypeEnum.Item:
                            itemImage = dataEntity.HeaderItemID == RecordIdentifier.Empty ? retailItemImage16 : variantItem16;
                            break;
                        case ItemTypeEnum.Service:
                            itemImage = serviceItem16;
                            break;
                        case ItemTypeEnum.MasterItem:
                            itemImage = masterItem16;
                            break;
                        case ItemTypeEnum.BOM:
                        case ItemTypeEnum.AssemblyItem:
                            itemImage = assemblyItem16;
                            break;
                        default:
                            itemImage = retailItemImage16;
                            break;
                    }

                    AddTag(new TaggingControl.TagItem(
                        ((dataEntity.ItemType != ItemTypeEnum.MasterItem) && (dataEntity.HeaderItemID != RecordIdentifier.Empty)) ? dataEntity.Text + " - " + dataEntity.VariantName : dataEntity.Text , 
                        itemImage));
                }
            }
        }

        protected override ViewBase GetRecordCursorView(int cursorIndex)
        {
            if (recordBrowsingContext != null)
            {
                RecordIdentifier id = recordBrowsingContext.ElementAt(cursorIndex).ID;

                if(Providers.RetailItemData.Exists(PluginEntry.DataModel,id))
                {
                    return new ItemView(id, recordBrowsingContext, tabSheetTabs.SelectedTabKey);
                }
            }

            return null;
        }

        protected override void GetRecordCursorInfo(ContextBarCursorEventArguments args)
        {
            if (recordBrowsingContext != null)
            {
                args.Position = 0;
                args.Count = recordBrowsingContext.Count();

                foreach (IDataEntity entity in recordBrowsingContext)
                {
                    if (entity.ID == itemID)
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
            tabSheetTabs.GetAuditContexts(contexts);

            contexts.Add(new AuditDescriptor("InventTableModule",new RecordIdentifier(itemID.PrimaryID,0),Properties.Resources.Inventory,true));
            contexts.Add(new AuditDescriptor("InventTableModule",new RecordIdentifier(itemID.PrimaryID,1),Properties.Resources.Purchase,true));
            contexts.Add(new AuditDescriptor("InventTableModule",new RecordIdentifier(itemID.PrimaryID,2),Properties.Resources.Sales,true));
        }

        public override ParentViewDescriptor CurrentViewDescriptor()
        {
            return new ParentViewDescriptor(
                    itemID,
                    Description,
                    null,
                    PluginOperations.ShowItemSheet);
        }

        protected override string LogicalContextName
        {
            get
            {
                if(MultiEditMode)
                {
                    return Properties.Resources.EditMultipleItems;
                }
                else
                {
                    return item.ItemType == ItemTypeEnum.MasterItem ? 
                        Properties.Resources.VariantHeader : 
                        (item.HeaderItemID != RecordIdentifier.Empty ?  Properties.Resources.VariantItem : Properties.Resources.RetailItem);
                }
                
            }
        }

        public override string ShortHeaderText
        {
            get
            {
                return itemID + " - " + tbDescription.Text;
            }
        }

        public string Description
        {
            get
            {
                if (MultiEditMode)
                {
                    return Properties.Resources.EditMultipleItems;
                }
                else
                {
                    return (item.ItemType == ItemTypeEnum.MasterItem ?
                            Properties.Resources.VariantHeader : 
                            (item.HeaderItemID != RecordIdentifier.Empty ? Properties.Resources.VariantItem : Properties.Resources.RetailItem)) +
                        ": " + ShortHeaderText;
                }
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return itemID;
	        }
        }

        public override string ContextDescription
        {
            get
            {
                return tbDescription.Text;
            }
        }

        protected override void InitializeView(bool isMultiEdit)
        {
            TabControl.Tab tab;

            //initializingView = true;

            if (isMultiEdit)
            {
                item = new RetailItemMultiEdit();
                tabSheetTabs.InitializeViews(itemID, item);
            }
            else
            {
                // Loading is not standard and accepted here but due to the complexity of the item view then its braking the glass here
                item = Providers.RetailItemData.Get(PluginEntry.DataModel, itemID);
                tabSheetTabs.InitializeViews(itemID, null);
            }

            tabSheetTabs.AddTab(new TabControl.Tab(Resources.General, ItemTabKey.General.ToString(), ViewPages.ItemViewGeneralPage.CreateInstance) { LogicFactory = ItemBaseSalesPriceLogic.CreateInstance } ) ;
            tabSheetTabs.AddTab(new TabControl.Tab(Resources.Prices, ItemTabKey.Prices.ToString(), ViewPages.ItemPricesPage.CreateInstance));
            tabSheetTabs.AddTab(new TabControl.Tab(Resources.Discounts, ItemTabKey.Discounts.ToString(), ViewPages.ItemDiscountsPage.CreateInstance));
            tabSheetTabs.AddTab(new TabControl.Tab(Resources.POSSettings, ItemTabKey.POSSettings.ToString(), ViewPages.ItemPOSSettingsPage.CreateInstance) { Enabled = item.ItemType != ItemTypeEnum.MasterItem });
            if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ManageFuelSettings))
            {
                tab = new TabControl.Tab(Resources.FuelSettings, ItemTabKey.FuelSettings.ToString(), ViewPages.ItemFuelSettings.CreateInstance);
                if (item.ItemType == ItemTypeEnum.MasterItem || item.ItemType == ItemTypeEnum.AssemblyItem)
                {
                    tab.Enabled = false;
                }
                tabSheetTabs.AddTab(tab);
            }

            tabSheetTabs.AddTab(new TabControl.Tab(Resources.SpecialGroups, ItemTabKey.SpecialGroups.ToString(), ViewPages.ItemSpecialGroupsPage.CreateInstance) { Enabled = item.ItemType != ItemTypeEnum.MasterItem });

            if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ManageLinkedItems))
            {
                tab = new TabControl.Tab(Resources.LinkedItems, ItemTabKey.LinkedItems.ToString(), ViewPages.ItemLinkedItemsPage.CreateInstance);
                if (item.ItemType == ItemTypeEnum.MasterItem || item.ItemType == ItemTypeEnum.AssemblyItem || MultiEditMode)
                {
                    tab.Enabled = false;
                }
                tabSheetTabs.AddTab(tab);
            }

            tabSheetTabs.AddTab(new TabControl.Tab(Resources.AdditionalInfo, ItemTabKey.AdditionalInformation.ToString(), ViewPages.ItemAdditionalInfo.CreateInstance));

            if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ViewItemLedger))
            {
                tab = new TabControl.Tab(Resources.ItemLedger, ItemTabKey.ItemLedger.ToString(), ViewPages.ItemLedgerPage.CreateInstance) { Enabled = item.ItemType != ItemTypeEnum.MasterItem };

                if(isMultiEdit || item.ItemType == ItemTypeEnum.AssemblyItem)
                {
                    tab.Enabled = false;
                }

                tabSheetTabs.AddTab(tab);
            }

            // Allow other plugins to extend this tab panel
            tabSheetTabs.Broadcast(this, itemID, item ,isMultiEdit);

            tabSheetTabs.AddTab(new TabControl.Tab(Resources.Images, ItemTabKey.Images.ToString(), ViewPages.ItemImagePage.CreateInstance) {Enabled =  !MultiEditMode});

            AddParentViewDescriptor(new ParentViewDescriptor(
                RecordIdentifier.Empty,
                Resources.RetailItems,
                null,
                PluginOperations.ShowItemsSheet));

            HeaderText = Description;
        }

        protected override void LoadData(bool isRevert)
        {
            item = Providers.RetailItemData.Get(PluginEntry.DataModel, itemID);

            if ((Guid) item.HeaderItemID != Guid.Empty)
            {
                tbVariantName.Visible = true;
                tbVariantName.Text = item.VariantName;
            }

            originalRetailGroupID = item.RetailGroupMasterID;

            tbID.Text = item.ID.ToString();
            tbDescription.Text = item.Text;

            tabSheetTabs.ViewStateData["InventoryUnit"] = item.InventoryUnitID;

            tabSheetTabs.SetData(isRevert, itemID, item);

            HeaderText = Description;

            if (selectedTabKey != "")
            {
                tabSheetTabs.SetSelectedTabByKey(selectedTabKey);
                selectedTabKey = tabSheetTabs.SelectedTabKey; //Reset tab key in case the tab was not found
            }

            if (item.Deleted)
            {
                Attributes = Attributes | ViewAttributes.Undelete;
            }

            tabSheetTabs.PostLoad(PluginEntry.DataModel, item, item.ID);
        }

        protected override bool DataIsModified()
        {
            item.Dirty = item.Dirty || (tbDescription.Text != item.Text) || tbVariantName.Text != item.VariantName;

            return (tabSheetTabs.IsModified()) || item.Dirty;
        }

        

        protected override void MultiEditSave(HashSet<int> changedControlHashes)
        {
            /*
            1. Collect data (Collects data from all loaded tabs)
            2. Save secondary records (Saves secondary records on all loaded tabs)
            3. MultiEditPreSave (Happens before save on all tabs, not just the loaded ones)
            4. Save (Saves the record, happens in the main card only)
            5. MultiEditPostSave (Happens after save on all tabs, not just the loaded ones)
            */
            
            (item as RetailItemMultiEdit).FieldSelection = RetailItemMultiEdit.FieldSelectionEnum.NoFields;

            

            // Gather data somehow for the main record
            tabSheetTabs.MultiEditCollectData(item, changedControlHashes, null);

            RetailItemMultiEdit.FieldSelectionEnum initialValue = (item as RetailItemMultiEdit).FieldSelection;

            if (MessageDialog.Show(Properties.Resources.MultiSaveQuestion, System.Windows.Forms.MessageBoxButtons.YesNo,System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            // Save it somehow
            Providers.RetailItemMultiEditData.PrepareStatement(PluginEntry.DataModel, (RetailItemMultiEdit)item);

            IConnectionManagerTemporary threadedConnection = PluginEntry.DataModel.CreateTemporaryConnection();

            bool masterRecordMadeTaxGroupValid = (item as RetailItemMultiEdit).HasValidSalesTaxItemGroupID;

            // Prepare the progress dialog information
            using (ProgressDialog dlg = new ProgressDialog(Properties.Resources.SavingItems, Properties.Resources.SavingItemsCount, recordBrowsingContext.Count()))
            {
                Action saveAction = () =>
                {
                    int count = 1;
                    int totalCount = recordBrowsingContext.Count();

                    foreach (IDataEntity entity in this.recordBrowsingContext)
                    {
                        //(item as RetailItemMultiEdit).

                        RecordIdentifier primaryID = new RecordIdentifier(((SimpleRetailItem)entity).MasterID, ((SimpleRetailItem)entity).ID);

                        dlg.Report(count, totalCount);

                        (item as RetailItemMultiEdit).ItemType = (entity as SimpleRetailItem).ItemType;

                        (item as RetailItemMultiEdit).OldSalesUnit = null;
                        (item as RetailItemMultiEdit).OldPrices = null;
                        (item as RetailItemMultiEdit).FieldSelection = initialValue;


                        // If the tax group was changed on the master record in this multi-edit phase then we have valid tax group right there
                        // which must be honored and used rather than the one comming from the database
                        (item as RetailItemMultiEdit).HasValidSalesTaxItemGroupID = masterRecordMadeTaxGroupValid;

                        // We save secondary records first so that the secondary handler can tweak the record
                        // Give each tab chance to save secondary record one per each record we save.
                        tabSheetTabs.MultiEditSaveSecondaryRecords(threadedConnection, item, primaryID);

                        // Give tabs that might not even be loaded yet chance to preprocess the data record
                        // for example if Tax group changed on generic tab then prices tab will want to update the price for each record.
                        tabSheetTabs.MultiEditPreSave(threadedConnection, item, primaryID);

                        // And then we go adhead and save the primary record which the secondary handler may have tampered with
                        if ((item as RetailItemMultiEdit).FieldSelection != RetailItemMultiEdit.FieldSelectionEnum.NoFields)
                        {
                            Providers.RetailItemMultiEditData.Save(threadedConnection, ((SimpleRetailItem) entity).MasterID);
                        }

                        // Give tabs that might not even be loaded chance to post process the data for example if Tax group was changed then
                        // Trade agreement tab will want to update old tradeagreements regardless if the tab was loaded or not
                        tabSheetTabs.MultiEditPostSave(threadedConnection, item, primaryID);

                        count++;
                    }
                };

                dlg.ProgressTask = Task.Run(saveAction);
                dlg.ShowDialog(PluginEntry.Framework.MainWindow);
            }

            threadedConnection.Close();

            tabSheetTabs.MultiEditSaveSecondaryRecordsFinalizer();

            

            // Here we will need to let the view mark all green controls as not green for the case of Back -> Save, Forwards -> Continue edit
            MultiEditSetInitialStatus();

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "RetailItem", RecordIdentifier.Empty, null);

            
        }

        protected override bool SaveData()
        {
            if (item.Dirty)
            {
                item.Text = tbDescription.Text;

                if ((Guid) item.HeaderItemID != Guid.Empty)
                {
                    item.VariantName = tbVariantName.Text;
                }
            }
            tabSheetTabs.GetData();

            tabSheetTabs.PreSave(PluginEntry.DataModel, item, item.ID);

            Providers.RetailItemData.Save(PluginEntry.DataModel, item);

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "RetailItem",item.ID, null);
            if (originalRetailGroupID != item.RetailGroupMasterID)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "RetailGroup", originalRetailGroupID, null);
            }
            return true;
        }

        protected override bool MultiEditValidateSaveUnknownControls()
        {
            return tabSheetTabs.MultiEditValidateSaveUnknownControls();
        }

        protected override void MultiEditRevertUnknownControl(System.Windows.Forms.Control control, bool isRevertField)
        {
            tabSheetTabs.MultiEditRevertUnknownControl(control, isRevertField);
        }


        public override void SaveUserInterface()
        {
            tabSheetTabs.SaveUserInterface();
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            // We use this one if we want to listen to changes in the Viewstack, like was there a user 
            // changed on a user sheet in the viewstack ? And it matters to our sheet ? if so then no
            // problem we catch it here and react if needed

            switch (objectName)
            {
                case "RetailItem":
                    if((changeHint == DataEntityChangeType.Delete || changeHint == DataEntityChangeType.MultiDelete) && MultiEditMode)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    else if (changeHint == DataEntityChangeType.Delete && changeIdentifier == itemID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    else if (changeHint == DataEntityChangeType.MultiDelete && ((List<RecordIdentifier>)param).Contains(itemID))
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    else if (changeHint == DataEntityChangeType.Undelete)
                    {
                        Attributes &= ~ViewAttributes.Undelete;
                        PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
                    }
                    break;
                case "ItemType":
                    PluginEntry.Framework.ViewController.CloseView(this);
                    PluginEntry.Framework.ViewController.Add(new ItemView(itemID, null));
                    break;
            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);

            

        }

        private void tabSheetTabs_SelectedTabChanged(object sender, EventArgs e)
        {
            /*if (item != null && item.ID != RecordIdentifier.Empty && !initializingView)
            {
                if (ValidateSave())
                {
                    base.Save();
                }
            }*/
        }

        protected override void OnDelete()
        {
            if (MultiEditMode)
            {
                List<SimpleRetailItem> toBeDeleted = new List<SimpleRetailItem>();

                foreach (IDataEntity entity in this.recordBrowsingContext)
                {
                    toBeDeleted.Add(((SimpleRetailItem)entity));
                }

                PluginOperations.DeleteItems(toBeDeleted);
            }
            else
            {
                PluginOperations.DeleteItem(item.ID, item.MasterID);
            }
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }

        private void ShowHeaderItemHandler(object sender, EventArgs args)
        {
            PluginOperations.ShowItemSheet(Providers.RetailItemData.GetItemIDFromMasterID(PluginEntry.DataModel, item.HeaderItemID));
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (!MultiEditMode && (Guid) item.HeaderItemID != Guid.Empty)
            {
                if (arguments.CategoryKey == GetType() + ".Related")
                {
                    arguments.Add(new ContextBarItem(Resources.ViewVariantHeader, ShowHeaderItemHandler), 100);
                }
            }
        }
    }
}
