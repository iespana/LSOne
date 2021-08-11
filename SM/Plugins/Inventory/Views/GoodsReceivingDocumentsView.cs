using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Inventory.Properties;
using LSOne.DataLayer.BusinessObjects.StoreManagement;

namespace LSOne.ViewPlugins.Inventory.Views
{
    public partial class GoodsReceivingDocumentsView : ViewBase
    {
        private static Guid BarSettingID = new Guid("567979FB-EB02-4583-8789-2F07E51EF070");

        private RecordIdentifier selectedID = "";
        private GoodsReceivingDocumentSearch searchCriteria;
        private Setting searchBarSetting;
        private List<InventoryTotals> receivedQuantity;
        private List<InventoryTotals> orderedQuantity;
        private List<DataEntity> vendorsList;
        private Store defaultStore;
        private int totalNumberOfDocuments;
        private int totalNumberOfOrders;

        public GoodsReceivingDocumentsView(RecordIdentifier selectedID)
            : this()
        {
            this.selectedID = selectedID;
        }

        public GoodsReceivingDocumentsView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help;

            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += new CancelEventHandler(lvItems_Opening);
            searchBar1.BuddyControl = lvItems;

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageGoodsReceivingDocuments);

            receivedQuantity = new List<InventoryTotals>();
            orderedQuantity = new List<InventoryTotals>();

            searchCriteria = new GoodsReceivingDocumentSearch {Status = GoodsReceivingStatusEnum.Active, LimitResultTo = PluginEntry.DataModel.PageSize };
            defaultStore = null;
            if (!string.IsNullOrWhiteSpace((string)PluginEntry.DataModel.CurrentStoreID))
            {
                searchCriteria.StoreID = PluginEntry.DataModel.CurrentStoreID;
                searchBar1.DefaultNumberOfSections = 2;
                defaultStore = Providers.StoreData.Get(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
            }

            try
            {
                vendorsList = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetVendorList(
                    PluginEntry.DataModel, 
                    PluginOperations.GetSiteServiceProfile(), 
                    false);

                totalNumberOfOrders = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetTotalNumberOfProductOrders(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    false);

                totalNumberOfDocuments = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetTotalNumberOfGRDocuments(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    true);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("GoodsReceivingDocuments", RecordIdentifier.Empty, Resources.GoodsReceivingDocuments, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.GoodsReceiving;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return RecordIdentifier.Empty;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            if (isRevert)
            {
                
            }
            HeaderText = Resources.GoodsReceivingDocuments;

            LoadItems(searchCriteria);
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PluginOperations.NewGoodReceivingDocument(this, EventArgs.Empty);

            LoadItems();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            GoodsReceivingDocument grd = (GoodsReceivingDocument) lvItems.Rows[lvItems.Selection.FirstSelectedRow].Tag;

            PluginOperations.ShowGoodsReceivingDocument(grd.GoodsReceivingID);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            GoodsReceivingDocument grd = (GoodsReceivingDocument)lvItems.Rows[lvItems.Selection.FirstSelectedRow].Tag;

            if (PluginOperations.DeleteGoodsReceivingDocument(grd.GoodsReceivingID))
            {
                LoadItems();
            }
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "GoodsReceivingDocument")
            {
                LoadItems();
            }
        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            GoodsReceivingDocument grd = lvItems.Selection.Count > 0 ? (GoodsReceivingDocument)lvItems.Rows[lvItems.Selection.FirstSelectedRow].Tag : new GoodsReceivingDocument();

            selectedID = (lvItems.Selection.Count > 0) ? grd.GoodsReceivingID : RecordIdentifier.Empty;

            var hasPermissionManageGoodsReceivingDocuments = PluginEntry.DataModel.HasPermission(Permission.ManageGoodsReceivingDocuments);

            btnsEditAddRemove.EditButtonEnabled = lvItems.Selection.Count == 1 && hasPermissionManageGoodsReceivingDocuments;
            btnsEditAddRemove.RemoveButtonEnabled = lvItems.Selection.Count != 0 && hasPermissionManageGoodsReceivingDocuments;
        }

        private void lvItems_DoubleClick(object sender, EventArgs e)
        {
            if (lvItems.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ManageGoodsReceivingDocuments))
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        void lvItems_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvItems.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Resources.Edit,
                    100,
                    new EventHandler(btnEdit_Click));

            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsEditAddRemove.EditButtonEnabled;
            item.Default = true;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Add,
                    200,
                    new EventHandler(btnAdd_Click));

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;

            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Delete,
                    300,
                    new EventHandler(btnRemove_Click));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("GoodsReceivingDocuments", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        private decimal ReceivedTotal(RecordIdentifier documentID)
        {
            try
            {
                int interval = PluginEntry.DataModel.PageSize;
                bool checkAgain = true;
                int count = receivedQuantity.Count == 0 ? interval : receivedQuantity.Count + interval;
                do
                {
                    InventoryTotals received = receivedQuantity.FirstOrDefault(f => f.ID == documentID);
                    if (received != null)
                    {
                        return received.Quantity;
                    }
                    else if (totalNumberOfDocuments > receivedQuantity.Count)
                    {
                        receivedQuantity = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetReceivedTotals(
                            PluginEntry.DataModel,
                            PluginOperations.GetSiteServiceProfile(),
                            count, 
                            true);

                        // If the number of documents retrieved is less than the total count then there aren't any more to get
                        // so set the total count to the current count
                        if (receivedQuantity.Count < count)
                        {
                            totalNumberOfDocuments = receivedQuantity.Count;
                        }

                        if (totalNumberOfDocuments > receivedQuantity.Count)
                        {
                            count += interval;
                        }
                    }
                    else
                    {
                        checkAgain = false;
                    }

                } while (checkAgain);

                return decimal.Zero;
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return decimal.Zero;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        private decimal OrderedTotal(RecordIdentifier documentID)
        {
            try
            {
                int interval = PluginEntry.DataModel.PageSize;
                bool checkAgain = true;
                int count = orderedQuantity.Count == 0 ? interval : orderedQuantity.Count + interval;
                do
                {
                    InventoryTotals received = orderedQuantity.FirstOrDefault(f => f.ID == documentID);
                    if (received != null)
                    {
                        return received.Quantity;
                    }
                    else if (totalNumberOfOrders > orderedQuantity.Count)
                    {
                        orderedQuantity = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetOrderedTotals(
                            PluginEntry.DataModel,
                            PluginOperations.GetSiteServiceProfile(),
                            count,
                            true);

                        // If the number of documents retrieved is less than the total count then there aren't any more to get
                        // so set the total count to the current count
                        if (orderedQuantity.Count < count)
                        {
                            totalNumberOfOrders = orderedQuantity.Count;
                        }

                        if (totalNumberOfOrders > orderedQuantity.Count)
                        {
                            count += interval;
                        }
                    }
                    else
                    {
                        checkAgain = false;
                    }

                } while (checkAgain);

                return decimal.Zero;
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return decimal.Zero;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        private void LoadItems(GoodsReceivingDocumentSearch searchCriteria = null)
        {
            lvItems.ClearRows();

            if (searchCriteria == null)
            {
                searchCriteria = GetSearchBarResults();
            }

            if (!PluginEntry.DataModel.HasPermission(Permission.ManageGoodsReceivingForAllStores) && !PluginEntry.DataModel.IsHeadOffice)
            {
                searchCriteria.StoreID = PluginEntry.DataModel.CurrentStoreID;
            }

            List<GoodsReceivingDocument> goodsReceivingDocuments = new List<GoodsReceivingDocument>();
            int totalSearchResult = 0;
            try
            {
                goodsReceivingDocuments = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetGoodsReceivingDocuments(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    searchCriteria,
                    true);

                // If the number of rows returned is the same as the page size limit then we need to check for the total number
                // of results possible using the same search parameters.
                if (goodsReceivingDocuments.Count == searchCriteria.LimitResultTo)
                {
                    totalSearchResult = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).CountGoodsReceivingDocumentsSearchResults(
                        PluginEntry.DataModel,
                        PluginOperations.GetSiteServiceProfile(),
                        searchCriteria,
                        true);
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }

            Row row;
            DecimalLimit quantityLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);

            if (totalSearchResult > searchCriteria.LimitResultTo)
            {
                lblMsg.Text = Resources.NumberOfDocsReturned.Replace("#1", Conversion.ToStr(PluginEntry.DataModel.PageSize)).Replace("#2", Conversion.ToStr(totalSearchResult));
            }

            foreach (GoodsReceivingDocument doc in goodsReceivingDocuments)
            {
                row = new Row();

                decimal receivedQuantity = ReceivedTotal(doc.GoodsReceivingID);
                decimal orderedQuantity = OrderedTotal(doc.PurchaseOrderID);

                Bitmap statusImage = null;
                switch (doc.Status)
                {
                    case GoodsReceivingStatusEnum.Active:
                        statusImage = receivedQuantity == 0 ? Resources.dot_grey_16 
                                        :(receivedQuantity >= orderedQuantity ? Resources.dot_green_16 : Resources.dot_yellow_16);
                        break;
                    case GoodsReceivingStatusEnum.Posted:
                        statusImage = Resources.dot_finished_16;
                        break;
                    default:
                        statusImage = Resources.dot_grey_16;
                        break;
                }
                row.AddCell(new ExtendedCell(string.Empty, statusImage));                

                row.AddText((string)doc.GoodsReceivingID);
                row.AddText(doc.Description);
                row.AddText(doc.VendorName);
                row.AddText(doc.StoreName);
                row.AddText(doc.StatusText);
                row.AddCell(new NumericCell(orderedQuantity.FormatWithLimits(quantityLimiter), orderedQuantity));
                row.AddCell(new NumericCell(receivedQuantity.FormatWithLimits(quantityLimiter), receivedQuantity));

                if (doc.CreatedDate != null)
                {
                    row.AddCell(new DateCell(doc.CreatedDate.ToShortDateString(), doc.CreatedDate));
                }
                else
                {
                    row.AddText("");
                }
                if (doc.PostedDate != null)
                {
                    row.AddCell(new DateCell(doc.PostedDate.ToShortDateString(), doc.PostedDate));
                }
                else
                {
                    row.AddText("");
                }

                row.Tag = doc;
                lvItems.AddRow(row);
            }

            lvItems.Sort(lvItems.SortColumn, lvItems.SortedAscending);
            lvItems.AutoSizeColumns();
            lvItems_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private GoodsReceivingDocumentSearch GetSearchBarResults()
        {
            List<SearchParameterResult> results = searchBar1.SearchParameterResults;

            searchCriteria = new GoodsReceivingDocumentSearch(PluginEntry.DataModel.PageSize);

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Status":
                        switch (result.ComboSelectedIndex)
                        {
                            case 1:
                                searchCriteria.Status = GoodsReceivingStatusEnum.Active;
                                break;
                            case 2:
                                searchCriteria.Status = GoodsReceivingStatusEnum.Posted;
                                break;
                            default:
                                searchCriteria.Status = null;
                                break;
                        }
                        break;
                    case "Description":
                        searchCriteria.Description = result.StringValue.Tokenize();
                        searchCriteria.DescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Store":
                        searchCriteria.StoreID = ((DualDataComboBox) result.UnknownControl).SelectedData.ID ?? RecordIdentifier.Empty;
                        break;
                    case "Vendor":
                        searchCriteria.VendorID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "CreatedDate":
                        searchCriteria.CreatedFrom = new Date(!result.Date.Checked, result.Date.Value).GetDateWithoutTime();
                        searchCriteria.CreatedTo = new Date(!result.DateTo.Checked, result.DateTo.Value.AddDays(1)).GetDateWithoutTime();
                        break;
                    case "PostedDate":
                        searchCriteria.PostedFrom = new Date(!result.Date.Checked, result.Date.Value).GetDateWithoutTime();
                        searchCriteria.PostedTo = new Date(!result.DateTo.Checked, result.DateTo.Value.AddDays(1)).GetDateWithoutTime();
                        break;
                }
            }

            return searchCriteria;
        }

        private void searchBar1_SetupConditions(object sender, EventArgs e)
        {
            List<object> statusList = new List<object>();
            statusList.Add(Resources.AllStatuses);
            statusList.Add(Resources.SearchBar_Value_Active);
            statusList.Add(Resources.SearchBar_Value_Posted);

            searchBar1.AddCondition(new ConditionType(Resources.Status, "Status", ConditionType.ConditionTypeEnum.ComboBox, statusList, 1, 0, false));
            searchBar1.AddCondition(new ConditionType(Resources.Store, "Store", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.Vendor, "Vendor", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar1.AddCondition(new ConditionType(Resources.SearchBar_CreatedDate, "CreatedDate", ConditionType.ConditionTypeEnum.DateRange));
            searchBar1.AddCondition(new ConditionType(Resources.SearchBar_PostedDate, "PostedDate", ConditionType.ConditionTypeEnum.DateRange));

            searchBar1_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar1_SaveAsDefault(object sender, EventArgs e)
        {
            string layoutString = searchBar1.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, BarSettingID, SettingsLevel.User, searchBarSetting);
            }

            ShowTimedProgress(searchBar1.GetLocalizedSavingText());
        }

        private void searchBar1_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar1.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private void searchBar1_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = defaultStore == null ? new DataEntity(null, Resources.AllStores) : new DataEntity(defaultStore.ID, defaultStore.Text);
                    ((DualDataComboBox)args.UnknownControl).RequestData += Store_RequestData;
                    break;
                case "Vendor":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");
                    ((DualDataComboBox)args.UnknownControl).RequestData += Vendor_RequestData;
                    break;
            }
        }

        void Store_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            List<DataEntity> stores = new List<DataEntity>();

            var hasAccessToAllStores = GetHasAccessToAllStores();
            if (hasAccessToAllStores)
            {
                stores = Providers.StoreData.GetList(PluginEntry.DataModel);
                stores.Insert(0, new DataEntity(null, Resources.AllStores));
            }
            else
            {
                stores.Add(Providers.StoreData.Get(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID));
            }

            ((DualDataComboBox)sender).SetData(stores, null);
        }

        void Vendor_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            ((DualDataComboBox)sender).SetData(vendorsList, null);
        }

        private void searchBar1_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= Store_RequestData;
                    break;
                case "Vendor":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= Vendor_RequestData;
                    break;
            }
        }

        private void searchBar_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                case "Vendor":
                    args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
                    break;
            }
        }

        private void searchBar_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                case "Vendor":
                    args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
                    break;
            }
        }

        private void searchBar_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            try
            {
                DataEntity entity = null;
                switch (args.TypeKey)
                {
                    case "Store":
                        var hasAccessToAllStores = GetHasAccessToAllStores();
                        entity = hasAccessToAllStores ? Providers.StoreData.Get(PluginEntry.DataModel, args.Selection) ?? new DataEntity(null, Resources.AllStores) :
                                                        new DataEntity(defaultStore.ID, defaultStore.Text);
                        break;
                    case "Vendor":
                        entity = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetVendor(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), args.Selection, true);
                        break;
                }
                ((DualDataComboBox) args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private void searchBar1_SearchClicked(object sender, EventArgs e)
        {
            LoadItems();
        }

        private bool GetHasAccessToAllStores()
        {
            return PluginEntry.DataModel.IsHeadOffice || PluginEntry.DataModel.HasPermission(Permission.ManageGoodsReceivingForAllStores);
        }
    }
}