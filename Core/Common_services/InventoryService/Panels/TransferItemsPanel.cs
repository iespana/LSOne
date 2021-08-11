using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Controls.Columns;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.POS.Core.Exceptions;
using LSOne.Utilities.ErrorHandling;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.Controls.Rows;
using LSOne.Controls.Cells;
using LSOne.Peripherals;
using LSOne.Controls.Dialogs;
using System.Threading;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.Services.WinFormsTouch;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Controls.SupportClasses;
using LSOne.Controls.Dialogs.SelectionDialog;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Panels
{
    public partial class TransferItemsPanel : UserControl
    {
        private IConnectionManager entry;
        private ISettings settings;
        private readonly SynchronizationContext synchronizationContext;
        private IPosTransaction currentTransaction;
        private OperationInfo operationInfo;
        private StoreTransferWrapper transferWrapper;

        private List<InventoryTransferOrderLine> transferOrderLines;
        private List<InventoryTransferRequestLine> transferRequestLines;

        private bool itemInfoVisible = true;
        private int itemInfoPanelHeight = 0;
        private RecordIdentifier selectedID = RecordIdentifier.Empty;
        private bool suppressKeyPress;
        private bool useScanner;
        private bool showDifference;

        public enum Buttons
        {
            PageUp,
            ArrowUp,
            ArrowDown,
            PageDown,
            ItemSearch,
            EditQty,
            EditUnit,
            DeleteLine,
            AutoPopulate,
            Difference,
            HeaderDetails,
            Send,
            Cancel
        }

        public TransferItemsPanel(IConnectionManager entry, StoreTransferWrapper transferWrapper, IPosTransaction transaction, OperationInfo operationInfo)
        {
            InitializeComponent();

            this.entry = entry;
            settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            synchronizationContext = SynchronizationContext.Current;
            this.transferWrapper = transferWrapper;
            currentTransaction = transaction;
            this.operationInfo = operationInfo;
            SetStyle(ControlStyles.Selectable, true);
            DoubleBuffered = true;

            useScanner = false;
            suppressKeyPress = false;
            showDifference = false;

            lvItems.SetSortColumn(0, true);
            itemInfoPanelHeight = pnlItemInfo.Size.Height;

            btnPanel.ButtonHeight = 50;
            AddButtons();
        }

        private void btnItemInfo_Click(object sender, EventArgs e)
        {
            SetItemInfoVisibility(!itemInfoVisible);
            ntbBarCode.Focus();
        }

        private void SetItemInfoVisibility(bool visible)
        {
            if (itemInfoVisible == visible) return;

            itemInfoVisible = visible;
            btnItemInfo.BackgroundImage = visible ? Properties.Resources.Arrowdownthin_32px : Properties.Resources.Arrowupthin_32px;
            pnlItemInfo.Visible = visible;
            pnlItems.Size = new Size(pnlItems.Size.Width, pnlItems.Size.Height + (visible ? -itemInfoPanelHeight : itemInfoPanelHeight));

            if(visible) //reload item info
            {
                lvItems_SelectionChanged(this, EventArgs.Empty);
            }
        }

        private void TransferItemsPanel_Load(object sender, EventArgs e)
        {
            if(transferWrapper.TransferType == StoreTransferTypeEnum.Request)
            {
                Column sentStatusColumn = new Column
                {
                    HeaderText = Properties.Resources.Sent,
                    AutoSize = true,
                    Clickable = true,
                    DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Center
                };
                lvItems.Columns.Add(sentStatusColumn);

                lvItems.Columns[3].HeaderText = Properties.Resources.Quantity;

                LoadRequestItems();
            }
            else
            {
                if(transferWrapper.TransferDirection == InventoryTransferType.Incoming)
                {
                    lvItems.Columns[3].HeaderText = Properties.Resources.Requested;

                    Column sentStatusColumn = new Column
                    {
                        HeaderText = Properties.Resources.Sent,
                        AutoSize = true,
                        Clickable = true,
                        DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right,
                    };
                    lvItems.Columns.Add(sentStatusColumn);

                    Column receivedStatusColumn = new Column
                    {
                        HeaderText = Properties.Resources.Received,
                        AutoSize = true,
                        Clickable = true,
                        DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right,
                    };
                    lvItems.Columns.Add(receivedStatusColumn);
                }
                else
                {
                    lblFromStore.Text = Properties.Resources.ToStore;
                }

                LoadOrderItems();
            }

            ActiveControl = ntbBarCode;
            ntbBarCode.Focus();
            ntbBarCode.Text = "";
            EnableScanner();
        }

        public void ItemSearch()
        {
            string selectedItemID = "";
            string selectedItemName = "";
            DialogResult result = Interfaces.Services.DialogService(entry).ItemSearch(
                100,
                ref selectedItemID,
                ref selectedItemName,
                Interfaces.Enums.ItemSearchViewModeEnum.Default,
                RetailItemSearchEnum.ItemType,
                (int)ItemTypeEnum.Item,
                currentTransaction,
                operationInfo,
                true
            );

            if (result != DialogResult.Cancel)
            {
                ntbBarCode.Text = selectedItemID;
                LoadItemFromBarcode(selectedItemID);
            }
        }

        public void EditQuantity()
        {
            if(lvItems.Selection.Count == 1)
            {
                using (var inputDialog = new NumpadAmountQtyDialog())
                {
                    inputDialog.HasDecimals = true;
                    inputDialog.NumberOfDecimals = entry.GetDecimalSetting(DecimalSettingEnum.Quantity).Max;
                    inputDialog.AllowNegative = false;
                    inputDialog.PromptText = Properties.Resources.EnterQuantity;
                    inputDialog.GhostText = Properties.Resources.Quantity;
                    inputDialog.SetMaxInputValue((double)settings.FunctionalityProfile.MaximumQTY);
                
                    if (inputDialog.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }

                    decimal quantity = (decimal)inputDialog.Value;

                    SaveTransferOrderLineResult result = SaveTransferOrderLineResult.ErrorSavingTransferOrderLine;
                    if(transferWrapper.TransferType == StoreTransferTypeEnum.Order)
                    {
                        InventoryTransferOrderLine orderLine = (InventoryTransferOrderLine)lvItems.Selection[0].Tag;
                        if (transferWrapper.TransferDirection == InventoryTransferType.Outgoing)
                        {
                            orderLine.QuantitySent = quantity;
                        }
                        else
                        {
                            orderLine.QuantityReceived = quantity;
                        }

                        ServiceCall(() =>
                        {
                            result = Interfaces.Services.SiteServiceService(entry).SaveInventoryTransferOrderLine(entry, settings.SiteServiceProfile, orderLine, transferWrapper.TransferDirection == InventoryTransferType.Incoming, out selectedID, true);
                        });

                        if (result == SaveTransferOrderLineResult.Success)
                        {
                            LoadOrderItems();
                        }
                        else
                        {
                            ShowInventoryTransferLineErrorResultMessage(result);
                        }
                    }
                    else
                    {
                        InventoryTransferRequestLine requestLine = (InventoryTransferRequestLine)lvItems.Selection[0].Tag;
                        requestLine.QuantityRequested = quantity;
                        ServiceCall(() =>
                        {
                            result = Interfaces.Services.SiteServiceService(entry).SaveInventoryTransferRequestLine(entry, settings.SiteServiceProfile, requestLine, out selectedID, true);
                        });

                        if (result == SaveTransferOrderLineResult.Success)
                        {
                            LoadRequestItems();
                        }
                        else
                        {
                            ShowInventoryTransferLineErrorResultMessage(result);
                        }
                    }
                }
            }
        }

        public void DeleteLine()
        {
            if (lvItems.Selection.Count == 1 && Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.RemoveLineQuestion, Properties.Resources.RemoveLine, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes)
            {
                SaveTransferOrderLineResult result = SaveTransferOrderLineResult.ErrorSavingTransferOrderLine;
                if (transferWrapper.TransferType == StoreTransferTypeEnum.Order)
                {
                    InventoryTransferOrderLine orderLine = (InventoryTransferOrderLine)lvItems.Selection[0].Tag;
                    ServiceCall(() =>
                    {
                        result = Interfaces.Services.SiteServiceService(entry).DeleteInventoryTransferOrderLine(entry, settings.SiteServiceProfile, orderLine.ID, true);
                    });

                    if (result == SaveTransferOrderLineResult.Success)
                    {
                        LoadOrderItems();
                    }
                    else
                    {
                        ShowInventoryTransferLineErrorResultMessage(result);
                    }
                }
                else
                {
                    InventoryTransferRequestLine requestLine = (InventoryTransferRequestLine)lvItems.Selection[0].Tag;
                    ServiceCall(() =>
                    {
                        result = Interfaces.Services.SiteServiceService(entry).DeleteInventoryTransferRequestLine(entry, settings.SiteServiceProfile, requestLine.ID, true);
                    });

                    if (result == SaveTransferOrderLineResult.Success)
                    {
                        LoadRequestItems();
                    }
                    else
                    {
                        ShowInventoryTransferLineErrorResultMessage(result);
                    }
                }
            }
        }

        public void EditUnitOfMeasure()
        {
            if(lvItems.Selection.Count == 1)
            {
                InventoryTransferOrderLine orderLine = null;
                InventoryTransferRequestLine requestLine = null;
                RecordIdentifier itemID;
                RecordIdentifier unitID;

                if (transferWrapper.TransferType == StoreTransferTypeEnum.Order)
                {
                    orderLine = (InventoryTransferOrderLine)lvItems.Selection[0].Tag;
                    itemID = orderLine.ItemId;
                    unitID = orderLine.UnitId;
                }
                else
                {
                    requestLine = (InventoryTransferRequestLine)lvItems.Selection[0].Tag;
                    itemID = requestLine.ItemId;
                    unitID = requestLine.UnitId;
                }

                List<UnitConversion> unitConversions = Providers.UnitConversionData.GetAllConversionsForItem(entry, itemID, unitID);

                for (int i = 0; i < unitConversions.Count; i++)
                {
                    if (unitConversions[i].FromUnitID != unitID)
                    {
                        unitConversions[i] = Providers.UnitConversionData.ReverseRule(unitConversions[i]);
                    }
                }

                using (var dialog = new SelectionDialog(new UnitConversionSelectionList(unitConversions), Properties.Resources.PleasePickUnitOfMeasure, false))
                {
                    if (dialog.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }

                    SaveTransferOrderLineResult result = SaveTransferOrderLineResult.ErrorSavingTransferOrderLine;
                    if (transferWrapper.TransferType == StoreTransferTypeEnum.Order)
                    {
                        orderLine.UnitId = ((UnitConversion)dialog.SelectedItem).ToUnitID;
                        ServiceCall(() =>
                        {
                            result = Interfaces.Services.SiteServiceService(entry).SaveInventoryTransferOrderLine(entry, settings.SiteServiceProfile, orderLine, false, out selectedID, true);

                        });

                        if (result == SaveTransferOrderLineResult.Success)
                        {
                            LoadOrderItems();
                        }
                        else
                        {
                            ShowInventoryTransferLineErrorResultMessage(result);
                        }
                    }
                    else
                    {
                        requestLine.UnitId = ((UnitConversion)dialog.SelectedItem).ToUnitID;
                        ServiceCall(() =>
                        {
                            result = Interfaces.Services.SiteServiceService(entry).SaveInventoryTransferRequestLine(entry, settings.SiteServiceProfile, requestLine, out selectedID, true);
                        });

                        if (result == SaveTransferOrderLineResult.Success)
                        {
                            LoadRequestItems();
                        }
                        else
                        {
                            ShowInventoryTransferLineErrorResultMessage(result);
                        }
                    }
                }
            }
        }

        public bool Difference()
        {
            if(transferWrapper.TransferType == StoreTransferTypeEnum.Order && transferWrapper.TransferDirection == InventoryTransferType.Incoming)
            {
                showDifference = !showDifference;
                LoadOrderItems();
            }

            return showDifference;
        }

        public void AutoPopulate()
        {
            if (transferWrapper.TransferType == StoreTransferTypeEnum.Order && transferWrapper.TransferDirection == InventoryTransferType.Incoming 
                && Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.AutoSetQuantityQuestion, Properties.Resources.AutoSetQuantity, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes)
            {
                if(!entry.HasPermission(Permission.AutoSetQuantityOnTransferOrder) && !Interfaces.Services.LoginPanelService(entry).PermissionOverrideDialog(new Interfaces.SupportClasses.PermissionInfo(Permission.AutoSetQuantityOnTransferOrder)))
                {
                    return;
                }

                AutoSetQuantityResult result = AutoSetQuantityResult.ErrorAutoSettingQuantity;

                ServiceCall(() =>
                {
                    result = Interfaces.Services.SiteServiceService(entry).AutoSetQuantityOnTransferOrder(entry, settings.SiteServiceProfile, transferWrapper.ID, true);
                });

                switch (result)
                {
                    case AutoSetQuantityResult.Success:
                        // OK
                        break;
                    case AutoSetQuantityResult.NotFound:
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.TransferOrderNotFound, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        break;
                    case AutoSetQuantityResult.AlreadyReceived:
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.TransferOrderAlreadyReceived, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        break;
                    case AutoSetQuantityResult.ErrorAutoSettingQuantity:
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ErrorAutoSettingQuantity, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        break;
                    default:
                        break;
                }

                LoadOrderItems();
            }
        }

        public void Send()
        {
            if(Interfaces.Services.DialogService(entry).ShowMessage(transferWrapper.TransferType == StoreTransferTypeEnum.Order 
                                                                                ? transferWrapper.TransferDirection == InventoryTransferType.Outgoing ? Properties.Resources.SendTransferOrderQuestion : Properties.Resources.ReceiveTransferOrderQuestion 
                                                                                : Properties.Resources.SendTransferRequestQuestion,
                                                                    transferWrapper.TransferType == StoreTransferTypeEnum.Order 
                                                                                ? transferWrapper.TransferDirection == InventoryTransferType.Outgoing ? Properties.Resources.SendInventoryTransferOrder : Properties.Resources.ReceiveTransferOrder 
                                                                                : Properties.Resources.SendTransferRequest, 
                                                                    MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes)
            {
                if(transferWrapper.TransferDirection == InventoryTransferType.Outgoing)
                {
                    SendTransferOrderResult result = SendTransferOrderResult.ErrorSendingTransferOrder;

                    ServiceCall(() =>
                    {
                        if (transferWrapper.TransferType == StoreTransferTypeEnum.Order)
                        {
                            result = Interfaces.Services.SiteServiceService(entry).SendTransferOrder(entry, settings.SiteServiceProfile, transferWrapper.ID, DateTime.Now, true);
                        }
                        else
                        {
                            result = Interfaces.Services.SiteServiceService(entry).SendInventoryTransferRequest(entry, settings.SiteServiceProfile, transferWrapper.ID, DateTime.Now, true);
                        }
                    });

                    if (result != SendTransferOrderResult.Success)
                    {
                        SendInventoryTransferErrorResultMessage(result);
                    }
                }
                else
                {
                    ReceiveTransferOrderResult result = ReceiveTransferOrderResult.ErrorReceivingTransferOrder;
                    ReceiveTransferOrderResult quantityResult = ReceiveTransferOrderResult.ErrorReceivingTransferOrder;

                    ServiceCall(() => quantityResult = Interfaces.Services.SiteServiceService(entry).ReceiveTransferOrderQuantityIsCorrect(entry, new List<RecordIdentifier>() { transferWrapper.ID }, settings.SiteServiceProfile, true));

                    if(quantityResult != ReceiveTransferOrderResult.Success)
                    {
                        if(Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ReceivedQuantityNotSameAsSentQuantity + " "
                                        + (quantityResult == ReceiveTransferOrderResult.SAPQuantitiesReceivedNotAccurate ? Properties.Resources.SAPReceivingQuantityUsedToAdjustInventory : Properties.Resources.InventoryAdjustmentWillBeCreatedForTheDifference) + "\r\n"
                                        + Properties.Resources.DoYouWantToContinue, Properties.Resources.ReceiveTransferOrder,
                                                                    MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.No)
                        {
                            return;
                        }
                    }

                    ServiceCall(() => result = Interfaces.Services.SiteServiceService(entry).ReceiveTransferOrder(entry, settings.SiteServiceProfile, transferWrapper.ID, DateTime.Now, true));

                    if(result != ReceiveTransferOrderResult.Success)
                    {
                        ReceiveTransferOrderErrorResultMessage(result);
                    }
                }
            }
        }
        private void LoadOrderItems()
        {
            if(transferOrderLines == null)
            {
                transferOrderLines = new List<InventoryTransferOrderLine>();
            }

            RecordIdentifier currentlySelectedID = selectedID;
            lvItems.ClearRows();
            transferOrderLines.Clear();
            selectedID = currentlySelectedID;
            int selectedRowIndex = -1;

            ServiceCall(() =>
            {
                transferOrderLines = Interfaces.Services.SiteServiceService(entry).GetOrderLinesForInventoryTransfer(entry, settings.SiteServiceProfile, transferWrapper.ID, InventoryTransferOrderLineSortEnum.ItemID, false, false, true);
            });

            DecimalLimit quantityLimiter;

            Row row;
            foreach (InventoryTransferOrderLine inventoryTransferOrderLine in transferOrderLines)
            {
                if(showDifference && inventoryTransferOrderLine.QuantityReceived == inventoryTransferOrderLine.QuantitySent)
                {
                    continue;
                }

                quantityLimiter = Providers.UnitData.GetNumberLimitForUnit(entry, Providers.UnitData.GetIdFromDescription(entry, inventoryTransferOrderLine.UnitName), CacheType.CacheTypeApplicationLifeTime);

                row = new Row();

                row.AddText((string)inventoryTransferOrderLine.ItemId);
                row.AddText(inventoryTransferOrderLine.ItemName);
                row.AddText(inventoryTransferOrderLine.VariantName);

                if (transferWrapper.TransferDirection == InventoryTransferType.Outgoing)
                {
                    row.AddCell(new NumericCell(inventoryTransferOrderLine.QuantitySent.FormatWithLimits(quantityLimiter) + " " + inventoryTransferOrderLine.UnitName, inventoryTransferOrderLine.QuantitySent));
                }
                else
                {
                    row.AddCell(new NumericCell(inventoryTransferOrderLine.QuantityRequested.FormatWithLimits(quantityLimiter) + " " + inventoryTransferOrderLine.UnitName, inventoryTransferOrderLine.QuantityRequested));
                    row.AddCell(new NumericCell(inventoryTransferOrderLine.QuantitySent.FormatWithLimits(quantityLimiter) + " " + inventoryTransferOrderLine.UnitName, inventoryTransferOrderLine.QuantitySent));
                    row.AddCell(new NumericCell(inventoryTransferOrderLine.QuantityReceived.FormatWithLimits(quantityLimiter) + " " + inventoryTransferOrderLine.UnitName, inventoryTransferOrderLine.QuantityReceived));
                }

                row.Tag = inventoryTransferOrderLine;
                lvItems.AddRow(row);

                if (selectedID == ((IDataEntity)row.Tag).ID)
                {
                    lvItems.Selection.Set(lvItems.RowCount - 1);
                    selectedRowIndex = lvItems.RowCount - 1;
                }
            }

            lvItems.Sort();
            lvItems.AutoSizeColumns();

            if(selectedRowIndex >= 0)
            {
                lvItems.ScrollRowIntoView(selectedRowIndex);
            }
        }

        private void LoadRequestItems()
        {
            if (transferRequestLines == null)
            {
                transferRequestLines = new List<InventoryTransferRequestLine>();
            }

            RecordIdentifier currentlySelectedID = selectedID;
            lvItems.ClearRows();
            transferRequestLines.Clear();
            selectedID = currentlySelectedID;
            int selectedRowIndex = -1;

            ServiceCall(() =>
            {
                transferRequestLines = Interfaces.Services.SiteServiceService(entry).GetRequestLinesForInventoryTransferAdvanced(entry, settings.SiteServiceProfile, transferWrapper.ID, out int x, new InventoryTransferFilterExtended { RowTo = int.MaxValue }, true);
            });

            DecimalLimit quantityLimiter;

            Row row;
            foreach (InventoryTransferRequestLine inventoryTransferRequestLine in transferRequestLines)
            {
                quantityLimiter = Providers.UnitData.GetNumberLimitForUnit(entry, Providers.UnitData.GetIdFromDescription(entry, inventoryTransferRequestLine.UnitName), CacheType.CacheTypeApplicationLifeTime);

                row = new Row();
                row.AddText((string)inventoryTransferRequestLine.ItemId);
                row.AddText(inventoryTransferRequestLine.ItemName);
                row.AddText(inventoryTransferRequestLine.VariantName);
                row.AddCell(new NumericCell(inventoryTransferRequestLine.QuantityRequested.FormatWithLimits(quantityLimiter) + " " + inventoryTransferRequestLine.UnitName, inventoryTransferRequestLine.QuantityRequested));
                row.AddCell(new ImageCell(inventoryTransferRequestLine.Sent ? Properties.Resources.Checkmark_green_32px : null, lvItems.BackColor.ToArgb(), 0, false));

                row.Tag = inventoryTransferRequestLine;
                lvItems.AddRow(row);

                if (selectedID == ((IDataEntity)row.Tag).ID)
                {
                    lvItems.Selection.Set(lvItems.RowCount - 1);
                    selectedRowIndex = lvItems.RowCount - 1;
                }
            }

            lvItems.Sort();
            lvItems.AutoSizeColumns();

            if (selectedRowIndex >= 0)
            {
                lvItems.ScrollRowIntoView(selectedRowIndex);
            }
        }

        private void LoadItemFromBarcode(string itemBarcode)
        {
            if(string.IsNullOrWhiteSpace(itemBarcode))
            {
                return;
            }

            ntbBarCode.Clear();
            
            IBarcodeService barcodeService = Interfaces.Services.BarcodeService(entry);
            barcodeService.Quantity = 0;
            BarCode barcode;

            // Suppress exceptions from barcode processing unless it is a POS specific exception
            try
            {
                barcode = barcodeService.ProcessBarcode(entry, new ScanInfo(itemBarcode) { EntryType = BarCode.BarcodeEntryType.ManuallyEntered });
            }
            catch (POSException px)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), px);
                Interfaces.Services.DialogService(entry).ShowExceptionMessage(px);
                return;
            }
            catch (Exception)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ItemNotFound);
                return;
            }

            if (barcode == null)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ItemNotFound);
                return;
            }

            RetailItem item = Providers.RetailItemData.Get(entry, barcode.ItemID) ?? Providers.RetailItemData.Get(entry, itemBarcode);

            if (item == null || item.ItemType == ItemTypeEnum.Service)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ItemNotFound);
                return;
            }

            if (item.ItemType == ItemTypeEnum.MasterItem)
            {
                RecordIdentifier selectedVariantID = RecordIdentifier.Empty;

                selectedVariantID = Interfaces.Services.DimensionService(entry).ShowDimensionDialog(entry, item.MasterID, item.Text);
                if (selectedVariantID == RecordIdentifier.Empty)
                {
                    return;
                }

                item = Providers.RetailItemData.Get(entry, selectedVariantID);
            }
            if (item.ItemType == ItemTypeEnum.AssemblyItem)
            {
                SaveAssemblyComponents(item, barcode);
            }
            else
            {
                SaveLine(item, barcode);
            }

            FocusTimer.Start();
        }

        private void SaveAssemblyComponents(RetailItem item, BarCode barcode)
        {

            var assemblyQuantity = Providers.UnitConversionData.ConvertQtyBetweenUnits(entry, item.ID, RecordIdentifier.IsEmptyOrNull(barcode.UnitID) ? item.InventoryUnitID : barcode.UnitID, item.SalesUnitID, barcode.Quantity == 0 ? 1 : barcode.Quantity);

            var assembly = Providers.RetailItemAssemblyData.GetAssemblyForItem(entry, item.ID, entry.CurrentStoreID, false);

            IBarcodeService barcodeService = Interfaces.Services.BarcodeService(entry);

            if (assembly != null)
            {

                foreach (var component in Providers.RetailItemAssemblyComponentData.GetList(entry, assembly.ID))
                {
                    var componentQuantity = component.Quantity * assemblyQuantity;

                    var componentItem = GetRetailItem(component.ItemID);

                    barcode = barcodeService.ProcessBarcode(entry, new ScanInfo((string)componentItem.ID) { EntryType = BarCode.BarcodeEntryType.ManuallyEntered });

                    if (componentItem.IsAssemblyItem)
                    {
                        SaveAssemblyComponents(componentItem, barcode);
                    }
                    else if (componentItem.ItemType != ItemTypeEnum.Service && componentItem.ItemType != ItemTypeEnum.MasterItem)
                    {
                        if (component.UnitID != componentItem.PurchaseUnitID)
                        {
                            barcode.Quantity = Providers.UnitConversionData.ConvertQtyBetweenUnits(
                                entry, componentItem.ID, component.UnitID, componentItem.PurchaseUnitID, componentQuantity);
                        }

                        SaveLine(componentItem, barcode);
                    }
                }
            }
        }

        private RetailItem GetRetailItem(RecordIdentifier itemID)
        {
            RetailItem item = Providers.RetailItemData.Get(entry, itemID);

            if (item == null)
            {
                try
                {
                    item = Interfaces.Services.SiteServiceService(entry).GetRetailItem(entry,
                        settings.SiteServiceProfile,
                        itemID,
                        true);
                }
                catch (Exception px)
                {
                    entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), px);
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CouldNotConnectToStoreServer);
                }
            }

            return item;
        }

        private void SaveLine(RetailItem item, BarCode barcode)
        {
            SaveTransferOrderLineResult result = SaveTransferOrderLineResult.ErrorSavingTransferOrderLine;

            if (transferWrapper.TransferType == StoreTransferTypeEnum.Order)
            {
                InventoryTransferOrderLine orderLine = new InventoryTransferOrderLine
                {
                    Barcode = barcode.ItemBarCode.ToString(),
                    InventoryTransferId = transferWrapper.ID,
                    ItemId = item.ID,
                    QuantitySent = barcode.Quantity == 0 ? 1 : barcode.Quantity,
                    UnitId = RecordIdentifier.IsEmptyOrNull(barcode.UnitID) ? item.InventoryUnitID : barcode.UnitID
                };

                InventoryTransferOrderLine orderLineCheck = Interfaces.Services.SiteServiceService(entry).GetTransferOrderLine(entry, settings.SiteServiceProfile, orderLine, true);

                if (orderLineCheck != null)
                {
                    if (Interfaces.Services.DialogService(entry).ShowMessage(orderLineCheck.ItemName + " " + Properties.Resources.AlreadyIncludedInDocument + "\n\n" + Properties.Resources.AddQuantityToExistingItem, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes)
                    {
                        orderLine.ID = orderLineCheck.ID;
                        orderLine.QuantitySent += orderLineCheck.QuantitySent;
                    }
                    else
                    {
                        return;
                    }
                }

                ServiceCall(() =>
                {
                    result = Interfaces.Services.SiteServiceService(entry).SaveInventoryTransferOrderLine(entry, settings.SiteServiceProfile, orderLine, transferWrapper.TransferDirection == InventoryTransferType.Incoming, out selectedID, true);
                });

                if (result == SaveTransferOrderLineResult.Success)
                {
                    LoadOrderItems();
                }
                else
                {
                    ShowInventoryTransferLineErrorResultMessage(result);
                }
            }
            else
            {
                InventoryTransferRequestLine requestLine = new InventoryTransferRequestLine
                {
                    Barcode = barcode.ItemBarCode.ToString(),
                    InventoryTransferRequestId = transferWrapper.ID,
                    ItemId = item.ID,
                    QuantityRequested = barcode.Quantity == 0 ? 1 : barcode.Quantity,
                    UnitId = RecordIdentifier.IsEmptyOrNull(barcode.UnitID) ? item.InventoryUnitID : barcode.UnitID
                };

                InventoryTransferRequestLine requestLineCheck = Interfaces.Services.SiteServiceService(entry).GetTransferRequestLine(entry, settings.SiteServiceProfile, requestLine, true);

                if (requestLineCheck != null)
                {
                    if (Interfaces.Services.DialogService(entry).ShowMessage(requestLineCheck.ItemName + " " + Properties.Resources.AlreadyIncludedInDocument + "\n\n" + Properties.Resources.AddQuantityToExistingItem, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes)
                    {
                        requestLine.ID = requestLineCheck.ID;
                        requestLine.QuantityRequested += requestLineCheck.QuantityRequested;
                    }
                    else
                    {
                        return;
                    }
                }

                ServiceCall(() =>
                {
                    result = Interfaces.Services.SiteServiceService(entry).SaveInventoryTransferRequestLine(entry, settings.SiteServiceProfile, requestLine, out selectedID, true);
                });

                if (result == SaveTransferOrderLineResult.Success)
                {
                    LoadRequestItems();
                }
                else
                {
                    ShowInventoryTransferLineErrorResultMessage(result);
                }
            }
            FocusTimer.Start();
        }

        private void lvItems_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            EditQuantity();
        }

        private async void lvItems_SelectionChanged(object sender, EventArgs e)
        {
            if(lvItems.Selection.Count > 0)
            {
                selectedID = transferWrapper.TransferType == StoreTransferTypeEnum.Order ? ((InventoryTransferOrderLine)lvItems.Selection[0].Tag).ItemId : ((InventoryTransferRequestLine)lvItems.Selection[0].Tag).ItemId;

                if (itemInfoVisible)
                {
                    ClearItemInfo();
                    await Task.Run(() => LoadItemInformation(selectedID));
                }
            }
            else
            {
                selectedID = RecordIdentifier.Empty;
            }

            SetLineButtonsEnabled(!RecordIdentifier.IsEmptyOrNull(selectedID));
        }

        public void SetLineButtonsEnabled(bool enabled)
        {
            btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.EditQty), enabled);

            if (transferWrapper.TransferDirection == InventoryTransferType.Outgoing)
            {
                btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.EditUnit), enabled);
                btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.DeleteLine), enabled);
            }
        }

        private void AddButtons()
        {
            btnPanel.Clear();

            btnPanel.AddButton("", Buttons.PageUp, Conversion.ToStr((int)Buttons.PageUp), TouchButtonType.Normal, image: Properties.Resources.Doublearrowupthin_32px);
            btnPanel.AddButton("", Buttons.ArrowUp, Conversion.ToStr((int)Buttons.ArrowUp), TouchButtonType.Normal, image: Properties.Resources.Arrowupthin_32px);
            btnPanel.AddButton("", Buttons.ArrowDown, Conversion.ToStr((int)Buttons.ArrowDown), TouchButtonType.Normal, image: Properties.Resources.Arrowdownthin_32px);
            btnPanel.AddButton("", Buttons.PageDown, Conversion.ToStr((int)Buttons.PageDown), TouchButtonType.Normal, image: Properties.Resources.Doublearrowdownthin_32px);

            if (transferWrapper.TransferDirection == InventoryTransferType.Outgoing)
            {
                btnPanel.AddButton(Properties.Resources.ItemSearch, Buttons.ItemSearch, Conversion.ToStr((int)Buttons.ItemSearch), TouchButtonType.Action);
            }
            else
            {
                btnPanel.AddButton(Properties.Resources.FillQuantities, Buttons.AutoPopulate, Conversion.ToStr((int)Buttons.AutoPopulate));
            }

            btnPanel.AddButton(Properties.Resources.Quantity, Buttons.EditQty, Conversion.ToStr((int)Buttons.EditQty), TouchButtonType.Normal, DockEnum.DockNone, Properties.Resources.Edit_16px, ImageAlignment.Left);
            btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.EditQty), false);

            if (transferWrapper.TransferDirection == InventoryTransferType.Outgoing)
            {
                btnPanel.AddButton(Properties.Resources.Unit, Buttons.EditUnit, Conversion.ToStr((int)Buttons.EditUnit), TouchButtonType.Normal, DockEnum.DockNone, Properties.Resources.Edit_16px, ImageAlignment.Left);
                btnPanel.AddButton(Properties.Resources.Delete, Buttons.DeleteLine, Conversion.ToStr((int)Buttons.DeleteLine), TouchButtonType.Normal, DockEnum.DockNone, Properties.Resources.Clear_16px, ImageAlignment.Left);
                btnPanel.AddButton(transferWrapper.TransferRequest != null ? Properties.Resources.RequestDetails : Properties.Resources.OrderDetails, Buttons.HeaderDetails, Conversion.ToStr((int)Buttons.HeaderDetails), dock: DockEnum.DockEnd);

                btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.EditUnit), false);
                btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.DeleteLine), false);
            }
            else
            {
                if (showDifference)
                {
                    btnPanel.AddButton(Properties.Resources.Difference, Buttons.Difference, Conversion.ToStr((int)Buttons.Difference), TouchButtonType.Action);
                }
                else
                {
                    btnPanel.AddButton(Properties.Resources.Difference, Buttons.Difference, Conversion.ToStr((int)Buttons.Difference));
                }
            }

            btnPanel.AddButton(transferWrapper.TransferRequest != null ? Properties.Resources.Send : transferWrapper.TransferDirection == InventoryTransferType.Outgoing ? Properties.Resources.Send : Properties.Resources.Receive, Buttons.Send, Conversion.ToStr((int)Buttons.Send), TouchButtonType.OK, DockEnum.DockEnd);
            btnPanel.AddButton(Properties.Resources.Close, Buttons.Cancel, Conversion.ToStr((int)Buttons.Cancel), TouchButtonType.Cancel, DockEnum.DockEnd);
        }

        private void btnPanel_Click(object sender, ScrollButtonEventArguments args)
        {
            switch ((int)args.Tag)
            {
                case (int)Buttons.PageUp:
                    lvItems.MovePageUp();
                    break;
                case (int)Buttons.ArrowUp:
                    lvItems.MoveSelectionUp();
                    break;
                case (int)Buttons.ArrowDown:
                    lvItems.MoveSelectionDown();
                    break;
                case (int)Buttons.PageDown:
                    lvItems.MovePageDown();
                    break;
                case (int)Buttons.ItemSearch:
                    ItemSearch();
                    break;
                case (int)Buttons.AutoPopulate:
                    AutoPopulate();
                    break;
                case (int)Buttons.EditQty:
                    EditQuantity();
                    break;
                case (int)Buttons.EditUnit:
                    EditUnitOfMeasure();
                    break;
                case (int)Buttons.DeleteLine:
                    DeleteLine();
                    break;
                case (int)Buttons.Difference:
                    Difference();
                    AddButtons();
                    break;
                case (int)Buttons.HeaderDetails:
                    GetParentDialog()?.ShowHeaderPanel();
                    break;
                case (int)Buttons.Send:
                    Send();
                    GetParentDialog()?.Close();
                    break;
                case (int)Buttons.Cancel:
                    GetParentDialog()?.Close();
                    break;
            }
        }

        private StoreTransferDialog GetParentDialog()
        {
            return this.Parent.Parent as StoreTransferDialog;
        }

        private void LoadItemInformation(RecordIdentifier itemID)
        {
            try
            {
                DecimalLimit quantityLimit = entry.GetDecimalSetting(DecimalSettingEnum.Quantity);

                Image img = Providers.RetailItemData.GetDefaultImage(entry, itemID);

                InventoryStatus currentStoreStatus = Interfaces.Services.SiteServiceService(entry).
                    GetInventoryListForItemAndStore(entry, settings.SiteServiceProfile, itemID, entry.CurrentStoreID, null, DataLayer.DataProviders.Inventory.InventorySorting.Store, false, true).FirstOrDefault() ?? new InventoryStatus { ItemID = itemID, StoreID = entry.CurrentStoreID };
                InventoryStatus fromStoreStatus = null;

                if (transferWrapper.TransferType == StoreTransferTypeEnum.Request)
                {
                    fromStoreStatus = Interfaces.Services.SiteServiceService(entry).
                        GetInventoryListForItemAndStore(entry, settings.SiteServiceProfile, itemID, transferWrapper.TransferRequest.ReceivingStoreId, null, DataLayer.DataProviders.Inventory.InventorySorting.Store, false, true).FirstOrDefault() ?? new InventoryStatus { ItemID = itemID, StoreID = transferWrapper.TransferRequest.ReceivingStoreId };
                }
                else
                {
                    RecordIdentifier storeID = transferWrapper.TransferDirection == InventoryTransferType.Incoming ? transferWrapper.TransferOrder.SendingStoreId : transferWrapper.TransferOrder.ReceivingStoreId;
                    fromStoreStatus = Interfaces.Services.SiteServiceService(entry).
                        GetInventoryListForItemAndStore(entry, settings.SiteServiceProfile, itemID, storeID, null, DataLayer.DataProviders.Inventory.InventorySorting.Store, false, true).FirstOrDefault() ?? new InventoryStatus { ItemID = itemID, StoreID = storeID };
                }

                synchronizationContext.Post(new SendOrPostCallback(x =>
                {
                    (Image Image, InventoryStatus CurrentStoreStatus, InventoryStatus FromStoreStatus) CallbackObject = ((Image, InventoryStatus, InventoryStatus))x;

                    pbItemImage.Image = CallbackObject.Image;
                    lblMyStoreInventOnHand.Text = CallbackObject.CurrentStoreStatus.InventoryOnHand.FormatWithLimits(quantityLimit);
                    lblMyStoreOrdered.Text = CallbackObject.CurrentStoreStatus.OrderedQuantity.FormatWithLimits(quantityLimit);
                    lblMyStoreParked.Text = CallbackObject.CurrentStoreStatus.ParkedQuantity.FormatWithLimits(quantityLimit);
                    lblMyStoreReserved.Text = CallbackObject.CurrentStoreStatus.ReservedQuantity.FormatWithLimits(quantityLimit);

                    if(CallbackObject.FromStoreStatus != null)
                    {
                        lblFromStoreInventOnHnad.Text = CallbackObject.FromStoreStatus.InventoryOnHand.FormatWithLimits(quantityLimit);
                        lblFromStoreOrdered.Text = CallbackObject.FromStoreStatus.OrderedQuantity.FormatWithLimits(quantityLimit);
                        lblFromStoreParked.Text = CallbackObject.FromStoreStatus.ParkedQuantity.FormatWithLimits(quantityLimit);
                        lblFromStoreReserved.Text = CallbackObject.FromStoreStatus.ReservedQuantity.FormatWithLimits(quantityLimit);
                    }

                }), (img, currentStoreStatus, fromStoreStatus));
            }
            catch
            {

            }
        }

        private void ClearItemInfo()
        {
            pbItemImage.Image = null;

            lblMyStoreInventOnHand.Text = lblMyStoreOrdered.Text = lblMyStoreParked.Text = lblMyStoreReserved.Text = "";

            if(transferWrapper.TransferType == StoreTransferTypeEnum.Request)
            {
                lblFromStoreInventOnHnad.Text = lblFromStoreOrdered.Text = lblFromStoreParked.Text = lblFromStoreReserved.Text = "";
            }
        }

        private void tnpNumpad_EnterPressed(object sender, EventArgs e)
        {
            if(transferWrapper.TransferDirection == InventoryTransferType.Outgoing)
            {
                if (!string.IsNullOrEmpty(ntbBarCode.Text))
                {
                    LoadItemFromBarcode(ntbBarCode.Text);
                }
            }
        }

        private void tnpNumpad_ClearPressed(object sender, EventArgs e)
        {
            ntbBarCode.Text = "";
        }

        private void ntbBarCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                suppressKeyPress = true;

                if(transferWrapper.TransferDirection == InventoryTransferType.Outgoing)
                {
                    LoadItemFromBarcode(ntbBarCode.Text);
                }
            }
            else if (e.KeyCode == Keys.LineFeed)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            suppressKeyPress = false;
        }

        public void TransferItemsPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            Scanner.ScannerMessageEvent -= ProcessScannedItem;
            Scanner.DisableForScan();
        }

        private void ProcessScannedItem(ScanInfo scanInfo)
        {
            try
            {
                if (useScanner)
                {
                    Scanner.DisableForScan();
                    ntbBarCode.Text = scanInfo.ScanDataLabel;

                    if(transferWrapper.TransferDirection == InventoryTransferType.Outgoing)
                    {
                        LoadItemFromBarcode(ntbBarCode.Text);
                    }
                }
            }
            finally
            {
                if (useScanner)
                {
                    Scanner.ReEnableForScan();
                }
            }
        }

        private void ntbBarCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (suppressKeyPress)
            {
                return;
            }

            if (e.KeyChar == (char)Keys.Return || e.KeyChar == (char)Keys.Enter)
            {
                if(transferWrapper.TransferDirection == InventoryTransferType.Outgoing)
                {
                    LoadItemFromBarcode(ntbBarCode.Text);
                }

                e.Handled = true;
            }
        }

        private void FocusTimer_Tick(object sender, EventArgs e)
        {
            FocusTimer.Stop();

            try
            {
                ActiveControl = ntbBarCode;
                ntbBarCode.Focus();
            }
            catch
            {
                //Retry if the control cannot receive focus (UI can be frozen while loading items)
                FocusTimer.Start();
            }
        }

        private void EnableScanner()
        {
            Scanner.ScannerMessageEvent += ProcessScannedItem;
            Scanner.ReEnableForScan();
            useScanner = true;
        }

        private void ServiceCall(Action action)
        {
            try
            {
                Exception ex = null;
                Interfaces.Services.DialogService(entry).ShowSpinnerDialog(action, "", Properties.Resources.ThisMayTakeAMoment, out ex);

                if(ex != null)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(ex is ClientTimeNotSynchronizedException ? ex.Message : Properties.Resources.CouldNotConnectToStoreServer, Properties.Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                }
            }
            catch(Exception e)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Properties.Resources.CouldNotConnectToStoreServer, Properties.Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
        }

        private void ShowInventoryTransferLineErrorResultMessage(SaveTransferOrderLineResult result)
        {
            switch (result)
            {
                case SaveTransferOrderLineResult.NotFound:
                    Interfaces.Services.DialogService(entry).ShowMessage(transferWrapper.TransferType == StoreTransferTypeEnum.Order ? Properties.Resources.TransferOrderOrLineNotFound : Properties.Resources.TransferRequestOrLineNotFound, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                case SaveTransferOrderLineResult.TransferOrderAlreadySent:
                    Interfaces.Services.DialogService(entry).ShowMessage(transferWrapper.TransferType == StoreTransferTypeEnum.Order ? Properties.Resources.TransferOrderHasAlreadyBeenSent : Properties.Resources.TransferRequestFetchedByStore, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                case SaveTransferOrderLineResult.ErrorSavingTransferOrderLine:
                    Interfaces.Services.DialogService(entry).ShowMessage(transferWrapper.TransferType == StoreTransferTypeEnum.Order ? Properties.Resources.UnableToSaveTransferOrderLine : Properties.Resources.UnableToSaveTransferRequestLine, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                case SaveTransferOrderLineResult.TransferOrderAlreadyReceived:
                    Interfaces.Services.DialogService(entry).ShowMessage(transferWrapper.TransferType == StoreTransferTypeEnum.Order ? Properties.Resources.TransferOrderHasAlreadyBeenSent : Properties.Resources.TransferOrderAlreadyCreated, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                default:
                    break;
            }
        }

        private void SendInventoryTransferErrorResultMessage(SendTransferOrderResult result)
        {
            string CRLF = "\n\r";

            switch (result)
            {
                case SendTransferOrderResult.ErrorSendingTransferOrder:
                    if (transferWrapper.TransferType == StoreTransferTypeEnum.Order)
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.UnableToSendTransferOrder + CRLF + Properties.Resources.TransferOrderRequestMayAlreadyBeSentOrContainNoItems, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    }
                    else
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.UnableToSendTransferRequest, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    }
                    break;
                case SendTransferOrderResult.LinesHaveZeroSentQuantity:
                    if (transferWrapper.TransferType == StoreTransferTypeEnum.Order)
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.UnableToSendTransferOrder + CRLF + Properties.Resources.OneOrMoreLinesHaveSentQuantityAsZero, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    }
                    break;
                case SendTransferOrderResult.FetchedByReceivingStore:
                    Interfaces.Services.DialogService(entry).ShowMessage(transferWrapper.TransferType == StoreTransferTypeEnum.Order ? Properties.Resources.TransferOrderHasBeenOpenedByReceivingStoreAndCannotBeSentAgain : Properties.Resources.TransferRequestHasBeenOpenedByReceivingStoreAndCannotBeSentAgain, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                case SendTransferOrderResult.NotFound:
                    Interfaces.Services.DialogService(entry).ShowMessage(transferWrapper.TransferType == StoreTransferTypeEnum.Order ? Properties.Resources.TransferOrderNotFound : Properties.Resources.TransferRequestNotFound, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                case SendTransferOrderResult.TransferAlreadySent:
                    Interfaces.Services.DialogService(entry).ShowMessage(transferWrapper.TransferType == StoreTransferTypeEnum.Order ? Properties.Resources.UnableToSendTransferOrder + CRLF + Properties.Resources.TransferOrderHasAlreadyBeenSent : Properties.Resources.UnableToSendRequest + CRLF + Properties.Resources.TransferRequestHasAlreadyBeenSent, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                case SendTransferOrderResult.NoItemsOnTransfer:
                    Interfaces.Services.DialogService(entry).ShowMessage(transferWrapper.TransferType == StoreTransferTypeEnum.Order ? Properties.Resources.UnableToSendTransferOrder + CRLF + Properties.Resources.TransferOrderHasNoItemLines : Properties.Resources.UnableToSendTransferOrder + CRLF + Properties.Resources.TransferRequestHasNoItemLines, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                case SendTransferOrderResult.UnitConversionError:
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.UnableToSendTransferOrder + CRLF + Properties.Resources.MissinUnitConversionBetweenUnits, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                case SendTransferOrderResult.TransferOrderIsRejected:
                    Interfaces.Services.DialogService(entry).ShowMessage(transferWrapper.TransferType == StoreTransferTypeEnum.Order ? Properties.Resources.UnableToSendTransferOrder + CRLF + Properties.Resources.RejectedTransferOrderCannotBeSent : Properties.Resources.UnableToSendTransferOrder + CRLF + Properties.Resources.RejectedTransferRequestCannotBeSent, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
            }
        }

        private void ReceiveTransferOrderErrorResultMessage(ReceiveTransferOrderResult result)
        {
            string CRLF = "\n\r";

            switch (result)
            {
                case ReceiveTransferOrderResult.ErrorReceivingTransferOrder:
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.UnabletoReceiveTransferOrder + CRLF + Properties.Resources.ErrorReceivingInventoryTransferOrder, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                case ReceiveTransferOrderResult.Received:
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.UnabletoReceiveTransferOrder + CRLF + Properties.Resources.TransferOrderAlreadyReceived, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                case ReceiveTransferOrderResult.NotFound:
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.UnabletoReceiveTransferOrder + CRLF + Properties.Resources.TransferOrderNotFound, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                case ReceiveTransferOrderResult.UnitConversionError:
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.UnabletoReceiveTransferOrder + CRLF + Properties.Resources.MissinUnitConversionBetweenUnits, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                case ReceiveTransferOrderResult.NoItemsOnTransfer:
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.UnabletoReceiveTransferOrder + CRLF + Properties.Resources.TransferOrderHasNoItemLines, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
            }
        }

        private void pbItemImage_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(ColorPalette.POSControlBorderColor);
            e.Graphics.DrawRectangle(p, 0, 0, pbItemImage.Width - 1, pbItemImage.Height - 1);
            p.Dispose();
        }

        private void tlpInventory_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(ColorPalette.POSControlBorderColor);
            e.Graphics.DrawRectangle(p, 0, 0, tlpInventory.Width - 1, tlpInventory.Height - 1);
            p.Dispose();
        }

        private void tlpInventory_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            Pen p = new Pen(ColorPalette.POSControlBorderColor);
            
            //Draw bottom line
            if(e.Row < tlpInventory.RowCount - 1)
            {
                e.Graphics.DrawLine(p, new Point(e.CellBounds.X, e.CellBounds.Bottom), new Point(e.CellBounds.Right, e.CellBounds.Bottom));
            }

            //Draw right line
            if(e.Row > 0 && e.Column < tlpInventory.ColumnCount - 1)
            {
                e.Graphics.DrawLine(p, new Point(e.CellBounds.Right, e.CellBounds.Y), new Point(e.CellBounds.Right, e.CellBounds.Bottom));
            }

            p.Dispose();
        }
    }
}
