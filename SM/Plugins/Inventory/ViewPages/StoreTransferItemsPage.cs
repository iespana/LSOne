using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.ViewCore.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Controls.EventArguments;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.Controls;
using LSOne.ViewPlugins.Inventory.Properties;
using LSOne.ViewCore.Dialogs;
using LSOne.Services.Interfaces;
using LSOne.ViewPlugins.Inventory.Dialogs;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.Controls.Columns;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.DataProviders;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    public partial class StoreTransferItemsPage : Controls.ContainerControl, ITabViewV2
    {
        private static Guid OrdersSendingSearchBarSettingID = new Guid("7080679A-DC36-4E0A-B81C-AB9A4BCB47C7");
        private static Guid OrdersToReceiveSearchBarSettingID = new Guid("f0d698aa-b55d-4243-8005-b583b95360fc");
        private static Guid OrdersSendingAndReceivingSearchBarSettingID = new Guid("c104555d-0a4e-42be-846a-ff648f75984d");
        private static Guid OrdersFinishedSearchBarSettingID = new Guid("ddc36fe5-7d1a-4ce2-a492-f434e630adf6");
        private static Guid RequestsSendingSearchBarSettingID = new Guid("e928fef0-beaa-4e6d-9359-d950cdd5dc35");
        private static Guid RequestsToReceiveSearchBarSettingID = new Guid("94b31f99-ce37-4a89-a305-0e05b2765e7a");
        private static Guid RequestsSendingAndReceivingSearchBarSettingID = new Guid("13a8074d-68f9-4ffd-be65-05a53261db7d");
        private static Guid RequestsFinishedSearchBarSettingID = new Guid("1fc337e7-f2a3-439c-8772-69b2dc19659a");
        private RecordIdentifier selectedItemID = "";
        private bool initialized;
        private StoreTransferTypeEnum storeTransferType;
        private InventoryTransferType inventoryTransferType;
        private InventoryTransferOrder transferOrder;
        private InventoryTransferRequest transferRequest;
        private Setting searchBarSetting;
        private bool refreshContextBar;
        private bool lockEvent;
        private bool canEditImages = false;
        private bool canDeleteImages = false;
        public delegate void RefreshContextBarHandler();
        public RefreshContextBarHandler OnRefreshContextBar;

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new StoreTransferItemsPage();
        }

        public StoreTransferItemsPage()
        {
            InitializeComponent();

            searchBar.BuddyControl = lvItems;

            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            lvItems.SetSortColumn(0, true);
            itemDataScroll.PageSize = PluginEntry.DataModel.PageSize;
            itemDataScroll.Reset();

            canEditImages = PluginEntry.Framework.CanRunOperation("AddEditImage");
            canDeleteImages = PluginEntry.Framework.CanRunOperation("DeleteImage");
        }

        public bool HasRows()
        {
            return lvItems.RowCount > 0;
        }

        public bool HasUnsentRows()
        {
            return storeTransferType == StoreTransferTypeEnum.Request && lvItems.Rows.Any(x => !((InventoryTransferRequestLine)x.Tag).Sent);
        }

        public bool DataIsModified()
        {
            return false;
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
            
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void InitializeView(RecordIdentifier context, object internalContext)
        {
            
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            (object internalObject, StoreTransferTypeEnum StoreTransferType, InventoryTransferType InventoryTransferType) info = (ValueTuple<object, StoreTransferTypeEnum, InventoryTransferType>)internalContext;

            storeTransferType = info.StoreTransferType;
            inventoryTransferType = info.InventoryTransferType;

            if (storeTransferType == StoreTransferTypeEnum.Order)
            {
                transferOrder = (InventoryTransferOrder)info.internalObject;
                btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders) && !transferOrder.Rejected && !transferOrder.Sent;
            }
            else
            {
                transferRequest = (InventoryTransferRequest)info.internalObject;
                btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransferRequests) && !transferRequest.Rejected &&
                                                       (!transferRequest.Sent ||
                                                            (!transferRequest.FetchedByReceivingStore && inventoryTransferType == InventoryTransferType.Outgoing &&
                                                                (PluginEntry.DataModel.IsHeadOffice || PluginEntry.DataModel.CurrentStoreID == transferRequest.SendingStoreId)
                                                            )
                                                       );
            }

            if (inventoryTransferType == InventoryTransferType.Incoming)
            {
                IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                if (storeTransferType == StoreTransferTypeEnum.Order && !transferOrder.FetchedByReceivingStore)
                {
                    transferOrder.FetchedByReceivingStore = true;
                    service.SaveInventoryTransferOrder(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), transferOrder, true);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferOrder", RecordIdentifier.Empty, null);
                }

                if (storeTransferType == StoreTransferTypeEnum.Request && !transferRequest.FetchedByReceivingStore)
                {
                    transferRequest.FetchedByReceivingStore = true;
                    service.SaveInventoryTransferRequest(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), transferRequest, true);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferRequest", RecordIdentifier.Empty, null);
                }
            }

            LoadItems();
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if ((storeTransferType == StoreTransferTypeEnum.Order && objectName == "InventoryTransferOrderLine" && changeIdentifier == transferOrder.ID)
                 || (storeTransferType == StoreTransferTypeEnum.Request && objectName == "InventoryTransferRequestLine" && changeIdentifier == transferRequest.ID))
            {
                LoadItems();
                OnRefreshContextBar?.Invoke();
            }

            if ((storeTransferType == StoreTransferTypeEnum.Order && objectName == "InventoryTransferOrder" && changeIdentifier == transferOrder.ID)
                || (storeTransferType == StoreTransferTypeEnum.Request && objectName == "InventoryTransferRequest" && changeIdentifier == transferRequest.ID))
            {
                if (storeTransferType == StoreTransferTypeEnum.Order)
                {
                    if (param != null)
                    {
                        transferOrder = (InventoryTransferOrder)param;
                        btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders) && !transferOrder.Rejected && !transferOrder.Sent;
                    }
                }
                else
                {
                    if (param != null)
                    {
                        transferRequest = (InventoryTransferRequest)param;
                        btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransferRequests) && !transferRequest.Rejected &&
                                                    (!transferRequest.Sent ||
                                                         (!transferRequest.FetchedByReceivingStore && inventoryTransferType == InventoryTransferType.Outgoing &&
                                                             (PluginEntry.DataModel.IsHeadOffice || PluginEntry.DataModel.CurrentStoreID == transferRequest.SendingStoreId)
                                                         )
                                                    );
                    }
                }

                searchBar_SearchClicked(null, EventArgs.Empty);
                OnRefreshContextBar?.Invoke();
            }
        }

        public bool SaveData()
        {
            return true;
        }

        public void SaveUserInterface()
        {
            
        }

        private void LoadItems()
        {
            InventoryTransferFilterExtended filter = GetSearchFilter();

            ViewBase parentView = (ViewBase)Parent.Parent.Parent;

            try
            {
                switch (storeTransferType)
                {
                    case StoreTransferTypeEnum.Order: LoadTransferOrders(filter); break;
                    case StoreTransferTypeEnum.Request: LoadTransferRequests(filter); break;
                }

                if (refreshContextBar)
                {
                    OnRefreshContextBar?.Invoke();
                }
            }
            finally
            {
                refreshContextBar = true;
            }
        }

        private void LoadTransferOrders(InventoryTransferFilterExtended filter)
        {
            if (!initialized)
            {
                Column sendingQtyColumn = new Column
                {
                    HeaderText = Resources.SendingQuantity,
                    AutoSize = true,
                    Clickable = true
                };
                lvItems.Columns.Add(sendingQtyColumn);

                Column requestedQtyColumn = new Column
                {
                    HeaderText = Resources.RequestedQuantidy,
                    AutoSize = true,
                    Clickable = true
                };
                lvItems.Columns.Add(requestedQtyColumn);

                Column receivedQtyColumn = new Column
                {
                    HeaderText = Resources.ReceivedQuantity,
                    AutoSize = true,
                    Clickable = true
                };
                lvItems.Columns.Add(receivedQtyColumn);

                Column sentStatusColumn = new Column
                {
                    HeaderText = Resources.Sent,
                    AutoSize = true,
                    Clickable = true
                };
                lvItems.Columns.Add(sentStatusColumn);

                Column deleteButtonColumn = new Column
                {
                    AutoSize = true,
                    Clickable = false
                };
                lvItems.Columns.Add(deleteButtonColumn);

                Column imageColumn = new Column
                {
                    AutoSize = true,
                    Clickable = false
                };

                lvItems.Columns.Add(imageColumn);
            }

            initialized = true;
            lockEvent = false;
            lvItems.Columns[2].Clickable = false;

            RecordIdentifier currentlySelectedID = selectedItemID;
            lvItems.ClearRows();
            selectedItemID = currentlySelectedID;

            try
            {
                int sortColumnIndex = lvItems.Columns.IndexOf(lvItems.SortColumn);

                if (lvItems.SortColumn != null)
                {
                    filter.TransferOrderSortBy = GetTransferOrderSort(sortColumnIndex);
                }

                ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
                List<InventoryTransferOrderLine> inventoryTransferOrderLines = new List<InventoryTransferOrderLine>();

                int itemCount = 0;
                
                inventoryTransferOrderLines = service.GetOrderLinesForInventoryTransferAdvanced(PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    transferOrder.ID,
                    out itemCount,
                    filter,
                    true);

                Row row;

                itemDataScroll.RefreshState(inventoryTransferOrderLines, itemCount);

                foreach (InventoryTransferOrderLine inventoryTransferOrderLine in inventoryTransferOrderLines)
                {
                    DecimalLimit quantityLimit = Providers.UnitData.GetNumberLimitForUnit(PluginEntry.DataModel, Providers.UnitData.GetIdFromDescription(PluginEntry.DataModel, inventoryTransferOrderLine.UnitName), CacheType.CacheTypeApplicationLifeTime);

                    row = new Row();
                    row.AddText((string)inventoryTransferOrderLine.ItemId);
                    if (string.IsNullOrEmpty(inventoryTransferOrderLine.ItemName))
                    {
                        RetailItem retailItem = PluginOperations.GetRetailItem(inventoryTransferOrderLine.ItemId, false);
                        inventoryTransferOrderLine.ItemName = retailItem == null ? Resources.NotAvailable : retailItem.Text;
                        inventoryTransferOrderLine.VariantName = retailItem == null ? Resources.NotAvailable : retailItem.VariantName;
                    }
                    row.AddText(inventoryTransferOrderLine.ItemName);
                    row.AddText(inventoryTransferOrderLine.VariantName);
                    row.AddText(inventoryTransferOrderLine.Barcode);
                    row.AddCell(new NumericCell(inventoryTransferOrderLine.QuantitySent.FormatWithLimits(quantityLimit) + " " + inventoryTransferOrderLine.UnitName, inventoryTransferOrderLine.QuantitySent));
                    row.AddCell(new NumericCell(inventoryTransferOrderLine.QuantityRequested.FormatWithLimits(quantityLimit) + " " + inventoryTransferOrderLine.UnitName, inventoryTransferOrderLine.QuantityRequested));
                    row.AddCell(new NumericCell(inventoryTransferOrderLine.QuantityReceived.FormatWithLimits(quantityLimit) + " " + inventoryTransferOrderLine.UnitName, inventoryTransferOrderLine.QuantityReceived));
                    row.AddCell(new CheckBoxCell(inventoryTransferOrderLine.Sent, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                    IconButton button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Resources.Delete, !transferOrder.Sent && !transferOrder.Rejected);
                    IconButtonCell icbDelete = new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", true);
                    icbDelete.Tag = "Delete";
                    row.AddCell(icbDelete);                    
                    
                    if (!RecordIdentifier.IsEmptyOrNull(inventoryTransferOrderLine.PictureID) && canEditImages)
                    {
                        IconButton imageButton = new IconButton(Resources.Camera24, Properties.Resources.EditImage);
                        row.AddCell(new IconButtonCell(imageButton, IconButtonCell.IconButtonIconAlignmentEnum.HorizontalCenter | IconButtonCell.IconButtonIconAlignmentEnum.VerticalCenter, ""));
                    }

                    row.Tag = inventoryTransferOrderLine;

                    if (!string.IsNullOrEmpty(inventoryTransferOrderLine.VariantName) && !lockEvent)
                    {
                        lvItems.Columns[2].Clickable = true;
                        lockEvent = true;
                    }

                    lvItems.AddRow(row);

                    if (selectedItemID == ((IDataEntity)row.Tag).ID)
                    {
                        lvItems.Selection.Set(lvItems.RowCount - 1);
                        lvItems.ScrollRowIntoView(lvItems.RowCount - 1);
                    }
                }
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }

            lvItems.AutoSizeColumns();
        }

        private InventoryTransferOrderLineSortEnum GetTransferOrderSort(int sortColumnIndex)
        {
            switch (sortColumnIndex)
            {
                case 0:
                    return InventoryTransferOrderLineSortEnum.ItemID;
                case 1:
                    return InventoryTransferOrderLineSortEnum.ItemName;
                case 2:
                    return InventoryTransferOrderLineSortEnum.VariantName;
                case 3:
                    return InventoryTransferOrderLineSortEnum.Barcode;
                case 4:
                    return InventoryTransferOrderLineSortEnum.SentQuantity;
                case 5:
                    return InventoryTransferOrderLineSortEnum.RequestedQuantity;
                case 6:
                    return InventoryTransferOrderLineSortEnum.ReceivedQuantity;
                case 7:
                    return InventoryTransferOrderLineSortEnum.Sent;
            }
            return InventoryTransferOrderLineSortEnum.ItemID;
        }

        private void LoadTransferRequests(InventoryTransferFilterExtended filter)
        {
            if (!initialized)
            {
                Column requestedQtyColumn = new Column
                {
                    HeaderText = Resources.RequestedQuantity,
                    AutoSize = true,
                    Clickable = true,
                };
                lvItems.Columns.Add(requestedQtyColumn);

                Column sentStatusColumn = new Column
                {
                    HeaderText = Resources.Sent,
                    AutoSize = true,
                    Clickable = true,
                };
                lvItems.Columns.Add(sentStatusColumn);
            }

            initialized = true;
            lockEvent = false;
            lvItems.Columns[2].Clickable = false;

            RecordIdentifier currentlySelectedID = selectedItemID;
            lvItems.ClearRows();
            selectedItemID = currentlySelectedID;

            try
            {
                int sortColumnIndex = lvItems.Columns.IndexOf(lvItems.SortColumn);

                if (lvItems.SortColumn != null)
                {
                    filter.TransferRequestSortBy = GetTransferRequestSort(sortColumnIndex);
                }

                ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

                List<InventoryTransferRequestLine> inventoryTransferRequestLines = new List<InventoryTransferRequestLine>();

                int itemCount = 0;

                inventoryTransferRequestLines = service.GetRequestLinesForInventoryTransferAdvanced(PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    transferRequest.ID,
                    out itemCount,
                    filter,
                    true);

                Row row;

                itemDataScroll.RefreshState(inventoryTransferRequestLines, itemCount);

                foreach (InventoryTransferRequestLine inventoryTransferRequestLine in inventoryTransferRequestLines)
                {
                    DecimalLimit quantityLimit = Providers.UnitData.GetNumberLimitForUnit(PluginEntry.DataModel, Providers.UnitData.GetIdFromDescription(PluginEntry.DataModel, inventoryTransferRequestLine.UnitName), CacheType.CacheTypeApplicationLifeTime);

                    row = new Row();
                    row.AddText((string)inventoryTransferRequestLine.ItemId);
                    if (string.IsNullOrEmpty(inventoryTransferRequestLine.ItemName))
                    {
                        RetailItem retailItem = PluginOperations.GetRetailItem(inventoryTransferRequestLine.ItemId, false);
                        inventoryTransferRequestLine.ItemName = retailItem == null ? Resources.NotAvailable : retailItem.Text;
                        inventoryTransferRequestLine.VariantName = retailItem == null ? Resources.NotAvailable : retailItem.VariantName;
                    }

                    row.AddText(inventoryTransferRequestLine.ItemName);
                    row.AddText(inventoryTransferRequestLine.VariantName);
                    row.AddText(inventoryTransferRequestLine.Barcode);
                    row.AddCell(new NumericCell(inventoryTransferRequestLine.QuantityRequested.FormatWithLimits(quantityLimit) + " " + inventoryTransferRequestLine.UnitName, inventoryTransferRequestLine.QuantityRequested));
                    row.AddCell(new CheckBoxCell(inventoryTransferRequestLine.Sent, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                    IconButton button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Resources.Delete, !transferRequest.Sent && !transferRequest.Rejected);
                    IconButtonCell icbDelete = new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", true);
                    icbDelete.Tag = "Delete";
                    row.AddCell(icbDelete);

                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", true));

                    row.Tag = inventoryTransferRequestLine;

                    if (!string.IsNullOrEmpty(inventoryTransferRequestLine.VariantName) && !lockEvent)
                    {
                        lvItems.Columns[2].Clickable = true;
                        lockEvent = true;
                    }

                    lvItems.AddRow(row);

                    if (selectedItemID == ((IDataEntity)row.Tag).ID)
                    {
                        lvItems.Selection.Set(lvItems.RowCount - 1);
                        lvItems.ScrollRowIntoView(lvItems.RowCount - 1);
                    }
                }
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }

            lvItems.AutoSizeColumns();
        }

        private InventoryTransferRequestLineSortEnum GetTransferRequestSort(int sortColumnIndex)
        {
            switch (sortColumnIndex)
            {
                case 0:
                    return InventoryTransferRequestLineSortEnum.ItemID;
                case 1:
                    return InventoryTransferRequestLineSortEnum.ItemName;
                case 2:
                    return InventoryTransferRequestLineSortEnum.VariantName;
                case 3:
                    return InventoryTransferRequestLineSortEnum.Barcode;
                case 4:
                    return InventoryTransferRequestLineSortEnum.RequestedQuantity;
                case 5:
                    return InventoryTransferRequestLineSortEnum.Sent;
            }
            return InventoryTransferRequestLineSortEnum.ItemID;
        }

        private void lvItems_SelectionChanged(object sender, EventArgs e)
        {
            btnsEditAddRemove.EditButtonEnabled = false;
            btnsEditAddRemove.RemoveButtonEnabled = false;

            int rowsSelected = lvItems.Selection.Count;

            switch (storeTransferType)
            {
                case StoreTransferTypeEnum.Order:
                    if (!transferOrder.Rejected && PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders))
                    {
                        btnsEditAddRemove.EditButtonEnabled = rowsSelected == 1 && (inventoryTransferType == InventoryTransferType.Outgoing ? !transferOrder.Sent : !transferOrder.Received);
                        btnsEditAddRemove.RemoveButtonEnabled = !transferOrder.Sent && rowsSelected >= 1;
                    }
                    break;
                case StoreTransferTypeEnum.Request:
                    var selectedTransferRequestLines = new List<InventoryTransferRequestLine>();

                    for (int i = 0; i < rowsSelected; i++)
                    {
                        selectedTransferRequestLines.Add((InventoryTransferRequestLine)lvItems.Selection[i].Tag);
                    }

                    if (rowsSelected >= 1 && PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransferRequests)
                        && CanDeleteOrEditTransferRequestLines(selectedTransferRequestLines))
                    {
                        btnsEditAddRemove.EditButtonEnabled = rowsSelected == 1;
                        btnsEditAddRemove.RemoveButtonEnabled = true;
                    }
                    break;
            }

            if (rowsSelected <= 0)
            {
                selectedItemID = "";
            }
            else if (rowsSelected == 1)
            {
                selectedItemID = ((IDataEntity)lvItems.Selection[0].Tag).ID;
            }
        }

        private bool CanDeleteOrEditTransferRequestLines(IEnumerable<InventoryTransferRequestLine> transferRequestLines)
        {
            return !transferRequest.Rejected && (!transferRequest.Sent || (!transferRequest.FetchedByReceivingStore && transferRequestLines.Any(x => !x.Sent)));
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            if (!PluginOperations.TestSiteService())
            {
                return;
            }

            if (storeTransferType == StoreTransferTypeEnum.Order)
            {
                if (!transferOrder.Sent && !transferOrder.Rejected)
                {
                    StoreTransferSendingItemDialog dlg = new StoreTransferSendingItemDialog((InventoryTransferOrderLine)lvItems.Selection[0].Tag, transferOrder);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        LoadItems();
                    }
                }
                else if (!transferOrder.Received && !transferOrder.Rejected)
                {
                    bool canReceiveNextRow = false;
                    int rowIndex = lvItems.Rows.IndexOf(lvItems.Selection[0]);
                    InventoryTransferOrderLine orderLine = (InventoryTransferOrderLine)lvItems.Selection[0].Tag;
                    StoreTransferReceivingItemDialog dlg = new StoreTransferReceivingItemDialog(orderLine);

                    do
                    {
                        dlg.ShowDialog();

                        if (dlg.DialogResult == DialogResult.OK && dlg.ReceiveAnother)
                        {
                            try
                            {
                                InventoryTransferOrderLine nextLine = (InventoryTransferOrderLine)lvItems.Rows[++rowIndex].Tag;
                                lvItems.Selection.Set(rowIndex);
                                dlg.OrderLine = nextLine;
                                canReceiveNextRow = true;
                            }
                            catch
                            {
                                //no row found
                                canReceiveNextRow = false;
                            }
                        }
                        else
                        {
                            canReceiveNextRow = false;
                        }

                    } while (dlg.ReceiveAnother && canReceiveNextRow);

                    LoadItems();
                }
            }
            else
            {
                if (!transferRequest.FetchedByReceivingStore && !transferRequest.Rejected)
                {
                    StoreTransferSendingItemDialog dlg = new StoreTransferSendingItemDialog((InventoryTransferRequestLine)lvItems.Selection[0].Tag, transferRequest);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        LoadItems();
                    }
                }
            }
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            if (!PluginOperations.TestSiteService())
            {
                return;
            }

            refreshContextBar = lvItems.RowCount <= 0;
            StoreTransferSendingItemDialog dlg = new StoreTransferSendingItemDialog(storeTransferType,
                storeTransferType == StoreTransferTypeEnum.Order ? transferOrder.ID : transferRequest.ID,
                storeTransferType == StoreTransferTypeEnum.Order ? transferOrder.SendingStoreId : transferRequest.ReceivingStoreId);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems();
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferOrder", RecordIdentifier.Empty, null);
            }
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if(!PluginOperations.TestSiteService())
            {
                return;
            }

            if (storeTransferType == StoreTransferTypeEnum.Order)
            {
                if (lvItems.Selection.Count == 1)
                {
                    RemoveOrderLine(((InventoryTransferOrderLine)lvItems.Selection[0].Tag).ID);
                }
                else
                {
                    List<RecordIdentifier> selectedIDsToBeRemoved = new List<RecordIdentifier>();

                    for (int i = 0; i < lvItems.Selection.Count; i++)
                    {
                        selectedIDsToBeRemoved.Add(((InventoryTransferOrderLine)lvItems.Selection[i].Tag).ID);
                    }

                    RemoveOrderLines(selectedIDsToBeRemoved);
                }
            }
            else
            {
                if (lvItems.Selection.Count == 1)
                {
                    RemoveRequestLine(((InventoryTransferRequestLine)lvItems.Selection[0].Tag).ID);
                }
                else
                {
                    var unableToRemoveSomeLines = false;

                    List<RecordIdentifier> selectedIDsToBeRemoved = new List<RecordIdentifier>();

                    for (int i = 0; i < lvItems.Selection.Count; i++)
                    {
                        var transferRequestLine = (InventoryTransferRequestLine)lvItems.Selection[i].Tag;

                        if (CanDeleteOrEditTransferRequestLines(new[] { transferRequestLine }))
                        {
                            selectedIDsToBeRemoved.Add(transferRequestLine.ID);
                        }
                        else
                        {
                            unableToRemoveSomeLines = true;
                        }
                    }

                    RemoveRequestLines(selectedIDsToBeRemoved);

                    if (unableToRemoveSomeLines)
                    {
                        MessageDialog.Show(Resources.ErrorDeletingInventoryTransferRequestLines, MessageBoxIcon.Error);
                    }
                }
            }
            refreshContextBar = lvItems.RowCount <= 0;

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferOrder", RecordIdentifier.Empty, null);
        }

        private void lvItems_CellAction(object sender, CellEventArgs args)
        {
            if (args.Cell is IconButtonCell)
            {
                string cellTag = ((IconButtonCell)args.Cell).Tag == null ? "" : (string)((IconButtonCell)args.Cell).Tag;

                if (cellTag == "Delete" && btnsEditAddRemove.RemoveButtonEnabled)
                {                    
                    btnsEditAddRemove_RemoveButtonClicked(null, EventArgs.Empty);
                }
                else if (canEditImages && storeTransferType == StoreTransferTypeEnum.Order)
                {
                    RecordIdentifier pictureID = ((InventoryTransferOrderLine)lvItems.Row(args.RowNumber).Tag).PictureID;

                    if (!Providers.ImageData.Exists(PluginEntry.DataModel, pictureID))
                    {
                        MessageDialog.Show(Properties.Resources.CannotEditImageLocally);
                        return;
                    }

                    PluginEntry.Framework.RunOperation("AddEditImage", this, new ViewCore.EventArguments.PluginOperationArguments(pictureID, null));
                }                
            }
            
        }

        private void RemoveOrderLine(RecordIdentifier lineID)
        {
            if (QuestionDialog.Show(Resources.RemoveLineQuestion, Resources.RemoveLine) == DialogResult.Yes)
            {
                try
                {
                    IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                    SaveTransferOrderLineResult result = service.DeleteInventoryTransferOrderLine(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), lineID, true);

                    if (result != SaveTransferOrderLineResult.Success)
                    {
                        PluginOperations.ShowInventoryTransferLineErrorResultMessage(result, true);
                    }
                }
                catch
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                }

                LoadItems();
            }
        }

        private void RemoveOrderLines(List<RecordIdentifier> lineIDs)
        {
            if (QuestionDialog.Show(Resources.RemoveLinesQuestion, Resources.RemoveLines) == DialogResult.Yes)
            {
                try
                {
                    IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                    SaveTransferOrderLineResult result = service.DeleteInventoryTransferOrderLines(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), lineIDs, true);

                    if (result != SaveTransferOrderLineResult.Success)
                    {
                        PluginOperations.ShowInventoryTransferLineErrorResultMessage(result, true);
                    }
                }
                catch
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                }

                LoadItems();
            }
        }

        private void RemoveRequestLine(RecordIdentifier lineID)
        {
            if (QuestionDialog.Show(Resources.RemoveLineQuestion, Resources.RemoveLine) == DialogResult.Yes)
            {
                try
                {
                    IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                    SaveTransferOrderLineResult result = service.DeleteInventoryTransferRequestLine(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), lineID, true);

                    if (result != SaveTransferOrderLineResult.Success)
                    {
                        PluginOperations.ShowInventoryTransferLineErrorResultMessage(result, false);
                    }
                }
                catch
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                }

                LoadItems();
            }
        }

        private void RemoveRequestLines(List<RecordIdentifier> lineIDs)
        {
            if (QuestionDialog.Show(Resources.RemoveLinesQuestion, Resources.RemoveLines) == DialogResult.Yes)
            {
                try
                {
                    IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                    SaveTransferOrderLineResult result = service.DeleteInventoryTransferRequestLines(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), lineIDs, true);

                    if (result != SaveTransferOrderLineResult.Success)
                    {
                        PluginOperations.ShowInventoryTransferLineErrorResultMessage(result, false);
                    }
                }
                catch
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                }

                LoadItems();
            }
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvItems.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                    Resources.Edit,
                    100,
                    btnsEditAddRemove_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Resources.Add,
                   200,
                   btnsEditAddRemove_AddButtonClicked);

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Delete,
                    300,
                    btnsEditAddRemove_RemoveButtonClicked);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            if (canDeleteImages)
            {
                menu.Items.Add(new ExtendedMenuItem("-", 600));

                item = new ExtendedMenuItem(Resources.DeleteImages, 650, DeleteImageHandler);
                item.Enabled = btnsEditAddRemove.RemoveButtonEnabled && LinesWithImagesSelected();
                menu.Items.Add(item);
            }

            PluginEntry.Framework.ContextMenuNotify("StoreTransferItems", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvItems_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            switch (storeTransferType)
            {
                case StoreTransferTypeEnum.Order:
                    searchBar.AddCondition(new ConditionType(Resources.DescriptionAndID, "DescriptionAndID", ConditionType.ConditionTypeEnum.Text));
                    searchBar.AddCondition(new ConditionType(Resources.SearchBar_Barcode, "Barcode", ConditionType.ConditionTypeEnum.Text));
                    searchBar.AddCondition(new ConditionType(Resources.SendingQuantity, "SendingQty", ConditionType.ConditionTypeEnum.NumericRange));
                    searchBar.AddCondition(new ConditionType(Resources.RequestedQuantity, "RequestedQty", ConditionType.ConditionTypeEnum.NumericRange));
                    searchBar.AddCondition(new ConditionType(Resources.ReceivedQuantity, "ReceivingQty", ConditionType.ConditionTypeEnum.NumericRange));
                    searchBar.AddCondition(new ConditionType(Resources.Sent, "Sent", ConditionType.ConditionTypeEnum.Checkboxes));
                    break;
                case StoreTransferTypeEnum.Request:
                    searchBar.AddCondition(new ConditionType(Resources.DescriptionAndID, "DescriptionAndID", ConditionType.ConditionTypeEnum.Text));
                    searchBar.AddCondition(new ConditionType(Resources.SearchBar_Barcode, "Barcode", ConditionType.ConditionTypeEnum.Text));
                    searchBar.AddCondition(new ConditionType(Resources.RequestedQuantity, "RequestedQty", ConditionType.ConditionTypeEnum.NumericRange));
                    searchBar.AddCondition(new ConditionType(Resources.Sent, "Sent", ConditionType.ConditionTypeEnum.Checkboxes));
                    break;
            }
            searchBar_LoadDefault(this, EventArgs.Empty);
        }

        public List<IDataEntity> GetSelectedItems()
        {
            var res = new List<IDataEntity>();
            for (int i = 0; i < lvItems.Selection.Count; i++)
            {
                InventoryTransferOrderLine itol = (InventoryTransferOrderLine)lvItems.Selection[i].Tag;
                RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, itol.ItemId);
                res.Add(item);
                int salesUnitQuantity = (int)Providers.UnitConversionData.ConvertQtyBetweenUnits(PluginEntry.DataModel, item.ID, item.PurchaseUnitID, item.SalesUnitID, itol.QuantityReceived);
                res[i].ID.SecondaryID = salesUnitQuantity;
            }
            return res;
        }

        private InventoryTransferFilterExtended GetSearchFilter()
        {
            InventoryTransferFilterExtended filter = new InventoryTransferFilterExtended();
            filter.RowFrom = itemDataScroll.StartRecord;
            filter.RowTo = itemDataScroll.EndRecord + 1;
            filter.SortDescending = !lvItems.SortedAscending;

            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "DescriptionAndID":
                        filter.DescriptionOrID = result.StringValue;
                        filter.DescriptionOrIDBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "SendingQty":
                        filter.SentQuantityFrom = (decimal)result.FromValue;
                        filter.SentQuantityTo = (decimal)result.ToValue;
                        break;
                    case "ReceivingQty":
                        filter.ReceivedQuantityFrom = (decimal)result.FromValue;
                        filter.ReceivedQuantityTo = (decimal)result.ToValue;
                        break;
                    case "RequestedQty":
                        filter.RequestedQuantityFrom = (decimal)result.FromValue;
                        filter.RequestedQuantityTo = (decimal)result.ToValue;
                        break;
                    case "Sent":
                        filter.Sent = result.CheckedValues[0];
                        break;
                    case "Barcode":
                        filter.Barcode = result.StringValue;
                        filter.BarcodeBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                }
            }

            return filter;
        }

        private void searchBar_SaveAsDefault(object sender, EventArgs e)
        {
            string layoutString = searchBar.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;
                switch (storeTransferType)
                {
                    case StoreTransferTypeEnum.Order:
                        switch (inventoryTransferType)
                        {
                            case InventoryTransferType.Outgoing:
                                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, OrdersSendingSearchBarSettingID, SettingsLevel.User, searchBarSetting);
                                break;
                            case InventoryTransferType.Incoming:
                                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, OrdersToReceiveSearchBarSettingID, SettingsLevel.User, searchBarSetting);
                                break;
                            case InventoryTransferType.SendingAndReceiving:
                                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, OrdersSendingAndReceivingSearchBarSettingID, SettingsLevel.User, searchBarSetting);
                                break;
                            case InventoryTransferType.Finished:
                                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, OrdersFinishedSearchBarSettingID, SettingsLevel.User, searchBarSetting);
                                break;
                        }
                        break;
                    case StoreTransferTypeEnum.Request:
                        switch (inventoryTransferType)
                        {
                            case InventoryTransferType.Outgoing:
                                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, RequestsSendingSearchBarSettingID, SettingsLevel.User, searchBarSetting);
                                break;
                            case InventoryTransferType.Incoming:
                                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, RequestsToReceiveSearchBarSettingID, SettingsLevel.User, searchBarSetting);
                                break;
                            case InventoryTransferType.SendingAndReceiving:
                                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, RequestsSendingAndReceivingSearchBarSettingID, SettingsLevel.User, searchBarSetting);
                                break;
                            case InventoryTransferType.Finished:
                                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, RequestsFinishedSearchBarSettingID, SettingsLevel.User, searchBarSetting);
                                break;
                        }
                        break;
                }
            }
        }

        private void searchBar_LoadDefault(object sender, EventArgs e)
        {
            switch (storeTransferType)
            {
                case StoreTransferTypeEnum.Order:
                    switch (inventoryTransferType)
                    {
                        case InventoryTransferType.Outgoing:
                            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, OrdersSendingSearchBarSettingID, SettingType.UISetting, "");
                            break;
                        case InventoryTransferType.Incoming:
                            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, OrdersToReceiveSearchBarSettingID, SettingType.UISetting, "");
                            break;
                        case InventoryTransferType.SendingAndReceiving:
                            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, OrdersSendingAndReceivingSearchBarSettingID, SettingType.UISetting, "");
                            break;
                        case InventoryTransferType.Finished:
                            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, OrdersFinishedSearchBarSettingID, SettingType.UISetting, "");
                            break;
                    }
                    break;
                case StoreTransferTypeEnum.Request:
                    switch (inventoryTransferType)
                    {
                        case InventoryTransferType.Outgoing:
                            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, RequestsSendingSearchBarSettingID, SettingType.UISetting, "");
                            break;
                        case InventoryTransferType.Incoming:
                            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, RequestsToReceiveSearchBarSettingID, SettingType.UISetting, "");
                            break;
                        case InventoryTransferType.SendingAndReceiving:
                            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, RequestsSendingAndReceivingSearchBarSettingID, SettingType.UISetting, "");
                            break;
                        case InventoryTransferType.Finished:
                            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, RequestsFinishedSearchBarSettingID, SettingType.UISetting, "");
                            break;
                    }
                    break;
            }

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            itemDataScroll.Reset();
            LoadItems();
        }

        private void searchBar_SearchOptionChanged(object sender, EventArgs e)
        {
            LoadItems();
        }

        private void itemDataScroll_PageChanged(object sender, EventArgs e)
        {
            LoadItems();
        }

        private void lvItems_HeaderClicked(object sender, ColumnEventArgs args)
        {
            if (lvItems.SortColumn == args.Column)
            {
                lvItems.SetSortColumn(args.Column, !lvItems.SortedAscending);
            }
            else
            {
                lvItems.SetSortColumn(args.Column, true);
            }

            itemDataScroll.Reset();

            LoadItems();
        }

        /// <summary>
        /// Checks if any of the selected lines has a picture ID
        /// </summary>
        /// <returns></returns>
        private bool LinesWithImagesSelected()
        {
            if (storeTransferType == StoreTransferTypeEnum.Order)
            {

                if (lvItems.Selection.Count == 0)
                {
                    return false;
                }

                for (int i = 0; i < lvItems.Selection.Count; i++)
                {
                    if (!RecordIdentifier.IsEmptyOrNull(((InventoryTransferOrderLine)lvItems.Selection[i].Tag).PictureID))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void DeleteImageHandler(object sender, EventArgs args)
        {        
            List<InventoryTransferOrderLine> modifiedTransferOrderLines = new List<InventoryTransferOrderLine>();

            if (lvItems.Selection.Count == 1)
            {
                // We don't have to check for an image here since the "Delete images" button is only enabled for a single line if it has a picture ID
                if (QuestionDialog.Show(Properties.Resources.DeleteImageQuestion) == DialogResult.Yes)
                {
                    InventoryTransferOrderLine transferOrderLine = ((InventoryTransferOrderLine)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag);
                    modifiedTransferOrderLines.Add(transferOrderLine);

                    Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).DeleteImage(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), transferOrderLine.PictureID, false);
                }
            }
            else
            {
                List<InventoryTransferOrderLine> transferOrderLinesWithPictureIDs = new List<InventoryTransferOrderLine>();

                for (int i = 0; i < lvItems.Selection.Count; i++)
                {
                    InventoryTransferOrderLine transferOrderLine = ((InventoryTransferOrderLine)lvItems.Selection[i].Tag);
                    
                    if (!RecordIdentifier.IsEmptyOrNull(transferOrderLine.PictureID))
                    {
                        transferOrderLinesWithPictureIDs.Add(transferOrderLine);
                    }
                }

                if (QuestionDialog.Show(Properties.Resources.DeleteImagesQuestion) == DialogResult.Yes)
                {
                    var pictureIDs = from transferOrderLine in transferOrderLinesWithPictureIDs select transferOrderLine.PictureID;
                    Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).DeleteImageList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), pictureIDs.ToList(), false);

                    modifiedTransferOrderLines = transferOrderLinesWithPictureIDs;
                }
            }

            int totalCount = modifiedTransferOrderLines.Count;
            using (ProgressDialog dlg = new ProgressDialog(Resources.SavingTransferOrderLines, Resources.SavingCounter, totalCount))
            {
                Action saveAction = () =>
                {
                    int count = 1;
                    RecordIdentifier newLineID = RecordIdentifier.Empty;
                    foreach (InventoryTransferOrderLine modifiedTransferOrderline in modifiedTransferOrderLines)
                    {
                        modifiedTransferOrderline.PictureID = RecordIdentifier.Empty;
                        SaveTransferOrderLineResult result = Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).SaveInventoryTransferOrderLine(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), modifiedTransferOrderline, false, out newLineID, false);

                        dlg.Report(count, totalCount);
                        count++;
                    }

                    Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
                };

                dlg.ProgressTask = Task.Run(saveAction);
                dlg.ShowDialog(PluginEntry.Framework.MainWindow);
            }
            
            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferOrderLine", transferOrder.ID, null);
        }
    }
}