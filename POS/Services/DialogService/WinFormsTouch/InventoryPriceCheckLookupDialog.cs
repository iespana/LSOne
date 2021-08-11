using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Peripherals;
using LSOne.POS.Core.Exceptions;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.ListViewExtensions;
using LSOne.Services.Properties;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ItemSale = LSOne.POS.Processes.Operations.ItemSale;

namespace LSOne.Services.WinFormsTouch
{
    public partial class InventoryPriceCheckLookupDialog : TouchBaseForm
    {
        private IPosTransaction posTransaction;
        private bool useScanner;
        private bool suppressKeyPress;
        private ISession session;

        private RetailItem item;
        private RetailItem linkedItem;
        private RetailItem componentItem;
        private RecordIdentifier regionID;
        private string regionDescription;
        private bool expandRegions;
        private bool expandLinkedItems;
        private bool expandComponents;
        private bool hasLinkedItems;
        private bool hasComponents;
        private Timer focusTimer;

        private List<ISaleLineItem> linkedItems;
        private List<ISaleLineItem> ComponentItems;
        private ISaleLineItem saleLineItem;

        public enum Buttons
        {
            MyRegion,
            Regions,
            AllRegions,
            LinkedItems,
            Components,
            Search,
            SellItem,
            Cancel
        }

        private bool isPriceCheckOperation;
        private bool showDataFromAllOperations;

        private RetailItem Item
        {
            get { return item; }
            set
            {
                item = value;

                tbItem.Text = item == null ? "" : item?.ID.StringValue + " - " + GetFullItemName(item);
            }
        }

        private RetailItem LinkedItem
        {
            get { return linkedItem; }
            set
            {
                linkedItem = value;

                if (linkedItem == null)
                {
                    tbItem.Text = item == null ? "" : item?.ID.StringValue + " - " + GetFullItemName(item);
                }
                else
                {
                    tbItem.Text = linkedItem.ID.StringValue + " - " + GetFullItemName(linkedItem);
                }
            }
        }

        private RetailItem ComponentItem
        {
            get { return componentItem; }
            set
            {
                componentItem = value;

                if (componentItem == null)
                {
                    tbItem.Text = item == null ? "" : item?.ID.StringValue + " - " + GetFullItemName(item);
                }
                else
                {
                    tbItem.Text = componentItem.ID.StringValue + " - " + GetFullItemName(componentItem);
                }
            }
        }

        /// <summary>
        /// Barcode object representing the scanned or searched item.
        /// Saved as barcode to allow the sale operation to save the item barcode number in the case item is scanned instead of searched.
        /// If the price/inventory for a linked item is checked, this property still holds the parent item information.
        /// </summary>
        public BarCode BarCode { get; private set; }

        public InventoryPriceCheckLookupDialog(ISession session, IPosTransaction posTransaction, bool isPriceCheckOperation, bool showDataFromAllOperations)
        {
            InitializeComponent();

            this.session = session;
            this.posTransaction = posTransaction;
            this.isPriceCheckOperation = isPriceCheckOperation;
            this.showDataFromAllOperations = showDataFromAllOperations;

            RetailTransaction retailTransaction = (RetailTransaction)posTransaction;

            if (retailTransaction.Customer != null && !RecordIdentifier.IsEmptyOrNull(retailTransaction.Customer.ID))
            {
                tbCustomer.Text = (string)retailTransaction.Customer.ID + " - " + retailTransaction.Customer.GetFormattedName(DLLEntry.DataModel.Settings.NameFormatter);
            }
            else
            {
                lblCustomer.Visible = tbCustomer.Visible = false;
                Size = new System.Drawing.Size(Width, Height - lblCustomer.Height - tbCustomer.Height - 13);
            }

            tbBarCode.Text = "";
            useScanner = false;
            suppressKeyPress = false;
            linkedItem = null;
            componentItem = null;

            regionID = DLLEntry.Settings.Store.RegionID;
            regionDescription = RecordIdentifier.IsEmptyOrNull(DLLEntry.Settings.Store.RegionID) ? Resources.AllRegions : DLLEntry.Settings.Store.RegionDescription;
            tbRegion.Text = regionDescription;

            AddButtons(false, false, false);

            focusTimer = new Timer();
            focusTimer.Interval = 1000;
            focusTimer.Tick += FocusTimer_Tick;

            touchDialogBanner.BannerText = isPriceCheckOperation ? Resources.PriceCheck : Resources.InventoryLookup;

            lvInventoryStatuses.HeaderHeight = 30;

            // Let the store column fill remaining width of the list view after resizing to content
            // Except when the store column is not shown (price check without inventory status)
            // in that case let the price column fill the width of the list
            clmStore.FillRemainingWidth = true;
            clmPrice.FillRemainingWidth = false;

            if (!showDataFromAllOperations)
            {
                if(isPriceCheckOperation)
                {
                    lblRegion.Visible = tbRegion.Visible = false;
                    tbItem.Width = tbItem.Width + tbRegion.Width + 5;

                    lvInventoryStatuses.Columns.RemoveAt(2);
                    lvInventoryStatuses.Columns.RemoveAt(0);
                    clmPrice.FillRemainingWidth = true;
                }
                else
                {
                    lvInventoryStatuses.Columns.RemoveAt(1);
                }
            }

            lvInventoryStatuses.AutoSizeColumns();
        }

        private void FocusTimer_Tick(object sender, EventArgs e)
        {
            focusTimer.Stop();

            try
            {
                ActiveControl = tbBarCode;
                tbBarCode.Focus();
            }
            catch
            {
                //ONE-8004 - Retry if the control cannot receive focus (UI can be frozen while loading items)
                focusTimer.Start();
            }
        }

        public void AddItem(RetailItem item)
        {
            Item = item;
            LoadItemInformation("", false, false);
            Invalidate();
        }

        public void EnableScanner()
        {
            Scanner.ScannerMessageEvent += ProcessScannedItem;
            Scanner.ReEnableForScan();
            useScanner = true;
        }

        private string GetFullItemName(RetailItem item)
        {
            if(item != null)
            {
                return item.IsVariantItem ? string.Format("{0} - {1}", item.Text, item.VariantName) : item.Text;
            }

            return "";
        }

        private void ProcessScannedItem(ScanInfo scanInfo)
        {
            try
            {
                ClearItemInfo();

                if (useScanner)
                {
                    Scanner.DisableForScan();
                }

                tbBarCode.Text = scanInfo.ScanDataLabel;
                LoadItemInformation(tbBarCode.Text, false, false);
            }
            finally
            {
                if (useScanner)
                {
                    Scanner.ReEnableForScan();
                }
            }
        }

        private void ClearItemInfo()
        {
            lvInventoryStatuses.ClearRows();
        }

        private void SetItemHeader(string itemName)
        {
            string header = isPriceCheckOperation ? Resources.PriceCheck : Resources.InventoryLookup;

            if(!string.IsNullOrEmpty(itemName))
            {
                header += " - " + itemName;
            }

            touchDialogBanner.BannerText = header;
        }

        private void LoadItemInformation(string itemBarcode, bool loadLinkedItem, bool loadComponent)
        {
            BarCode barCode = null;
            lvInventoryStatuses.ClearRows();
            hasLinkedItems = false;

            //Get item by barcode
            if (!string.IsNullOrEmpty(itemBarcode))
            {
                IBarcodeService barcodeService = Interfaces.Services.BarcodeService(DLLEntry.DataModel);
                barcodeService.Quantity = 0;

                // Suppress exceptions from barcode processing unless it is a POS specific exception
                try
                {
                    barCode = barcodeService.ProcessBarcode(DLLEntry.DataModel, new ScanInfo(itemBarcode) { EntryType = BarCode.BarcodeEntryType.ManuallyEntered });
                }
                catch (POSException px)
                {
                    DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), px);
                    Interfaces.Services.DialogService(DLLEntry.DataModel).ShowExceptionMessage(px);
                    return;
                }
                catch (Exception)
                {
                    Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Resources.ItemNotFound);
                    return;
                }

                if (barCode == null)
                {
                    Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Resources.ItemNotFound);
                    return;
                }

                if(barCode.Found)
                {
                    barCode.InternalType = BarcodeInternalType.Item;
                }

                Item = Providers.RetailItemData.Get(DLLEntry.DataModel, barCode.ItemID) ?? Providers.RetailItemData.Get(DLLEntry.DataModel, itemBarcode);

                if(Item == null) //Reset view
                {
                    LinkedItem = null;
                    hasLinkedItems = false;
                    hasComponents = false;
                    saleLineItem = null;
                    BarCode = null;
                    SetItemHeader("");
                    AddButtons(expandRegions, false, false);
                    Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Resources.ItemNotFound);
                    return;
                }
            }
            else if(Item != null && barCode == null)
            {
                barCode = new BarCode()
                {
                    InternalType = BarcodeInternalType.Item,
                    ItemID = Item.ID,
                };
            }

            if (!loadLinkedItem)
            {
                //If we load a component item, make sure that setting the linked item doesn't override the item names on the UI
                if (loadComponent)
                {
                    linkedItem = null;
                }
                else
                {
                    LinkedItem = null;
                }
            }

            if(!loadComponent)
            {
                //If we load a linked item, make sure that setting the component item doesn't override the item names on the UI
                if(loadLinkedItem)
                {
                    componentItem = null;
                }
                else
                {
                    ComponentItem = null;
                }
            }

            if (!loadLinkedItem && !loadComponent)
            {
                linkedItems = new List<ISaleLineItem>();
                expandLinkedItems = LinkedItem != null;
                hasLinkedItems = false;

                ComponentItems = new List<ISaleLineItem>();
                expandComponents = ComponentItem != null;
                hasComponents = false;
            }

            //Process item sale
            ICloneTransactions cloning = Interfaces.Services.TransactionService(DLLEntry.DataModel).CreateCloneTransactions();
            IPosTransaction tempTransaction = cloning.CloneTransaction(DLLEntry.DataModel, posTransaction);

            OperationInfo operationInfo = new OperationInfo();
            operationInfo.TenderLineId = -1;
            operationInfo.ItemLineId = -1;
            operationInfo.PriceCheck = true;

            var itemSale = new ItemSale(session)
            {
                POSTransaction = (PosTransaction)tempTransaction,
                OperationID = POSOperations.ItemSale,
                OperationInfo = operationInfo,
                BarCode = loadLinkedItem || loadComponent ? null : barCode,
                Barcode = loadLinkedItem ? LinkedItem.ID.StringValue : loadComponent ? ComponentItem.ID.StringValue : Item.ID.StringValue,
            };

            int prevItemCount = ((RetailTransaction)tempTransaction).SaleItems.Count;
            itemSale.RunOperation();

            string variantName = string.Empty;
            bool inventoryStatusHasHeaderItem = false;

            if (((RetailTransaction)tempTransaction).SaleItems.Count > prevItemCount)
            {
                ISaleLineItem lineItem = ((RetailTransaction)tempTransaction).SaleItems.Last.Value;

                foreach (ISaleLineItem addedItem in ((RetailTransaction)tempTransaction).SaleItems.Where(x => x.LineId > prevItemCount))
                {
                    //If we check an item with infocodes, the last item can be an infocode
                    if (addedItem.IsLinkedItem && !addedItem.IsAssemblyComponent)
                    {
                        hasLinkedItems = true;
                        lineItem = ((RetailTransaction)tempTransaction).GetItem(addedItem.LinkedToLineId);
                    }

                    if (addedItem.IsAssemblyComponent)
                    {
                        hasComponents = true;
                        lineItem = ((RetailTransaction)tempTransaction).GetItem(addedItem.LinkedToLineId);
                    }

                    //If we load a linked item, keep the original item for sale
                    if (!loadLinkedItem && !loadComponent)
                    {
                        saleLineItem = lineItem;
                        BarCode = barCode;

                        if (Item.IsHeaderItem)
                        {
                            Item = Providers.RetailItemData.Get(DLLEntry.DataModel, saleLineItem.ItemId);
                        }

                        //Add linked and component items
                        linkedItems = ((RetailTransaction)tempTransaction).SaleItems.Where(x => x.IsLinkedItem && !x.IsAssemblyComponent && x.LinkedToLineId == saleLineItem.LineId).ToList();
                        ComponentItems = ((RetailTransaction)tempTransaction).SaleItems.Where(x => x.IsAssemblyComponent && x.AssemblyParentLineID == saleLineItem.LineId).ToList();
                    }
                }

                RetailItem lookupItem = loadLinkedItem ? LinkedItem : 
                                        loadComponent ? ComponentItem : Item;
                                        
                if (lookupItem.IsHeaderItem)
                {
                    lookupItem = Providers.RetailItemData.Get(DLLEntry.DataModel, lineItem.ItemId);
                }

                AddButtons(expandRegions, expandLinkedItems, expandComponents);

                #region Inventory status
                //Load inventory status
                List<InventoryStatus> listOfInventoryStatusesForItem = new List<InventoryStatus>();

                if (!isPriceCheckOperation || showDataFromAllOperations)
                {
                    tbRegion.Text = regionDescription;

                    ISiteServiceService service = (ISiteServiceService)DLLEntry.DataModel.Service(ServiceType.SiteServiceService);

                    try
                    {
                        ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowStatusDialog(Properties.Resources.RetrievingAvailableInventory);

                        if(!loadLinkedItem && lookupItem.ItemType == ItemTypeEnum.AssemblyItem)
                        {
                            listOfInventoryStatusesForItem = service.GetInventoryListForAssemblyItemAndStore(DLLEntry.DataModel, DLLEntry.Settings.SiteServiceProfile, lookupItem.ID, RecordIdentifier.Empty, regionID, DataLayer.DataProviders.Inventory.InventorySorting.Store, false, true).OrderBy(x => x.StoreName).ToList();
                        }
                        else
                        {
                            listOfInventoryStatusesForItem = service.GetInventoryStatus(DLLEntry.DataModel, DLLEntry.Settings.SiteServiceProfile, loadLinkedItem ? LinkedItem.ID : lookupItem.ID, RecordIdentifier.Empty, regionID, true).OrderBy(x => x.StoreName).ToList();
                        }
                    }                    
                    catch (Exception e)
                    {
                        ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Properties.Resources.CouldNotConnectToSiteService, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        return;
                    }
                    finally
                    {
                        ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).CloseStatusDialog();
                    }

                    if (!isPriceCheckOperation && !showDataFromAllOperations)
                    {
                        foreach (var inventoryStatus in listOfInventoryStatusesForItem)
                        {
                            Row row = new Row();
                            row.AddText(inventoryStatus.StoreName);

                            if (inventoryStatus.HasHeaderItem)
                            {
                                row.AddText("N/A");
                                inventoryStatusHasHeaderItem = true;
                            }
                            else
                            {
                                row.AddText((loadLinkedItem ? LinkedItem.IsServiceItem : lookupItem.IsServiceItem) ? "-" : inventoryStatus.InventoryQuantityFormatted);
                            }

                            lvInventoryStatuses.AddRow(row);
                            variantName = inventoryStatus.VariantName;
                            if (inventoryStatus.StoreID == DLLEntry.DataModel.CurrentStoreID)
                            {
                                lvInventoryStatuses.Selection.Set(lvInventoryStatuses.RowCount - 1);
                            }
                        }
                    }
                }
                #endregion Inventory status

                #region Price check
                if (isPriceCheckOperation || showDataFromAllOperations)
                {
                    if (((RetailTransaction)tempTransaction).SaleItems.Count > prevItemCount)
                    {
                        IRoundingService roundingService = Interfaces.Services.RoundingService(DLLEntry.DataModel);

                        Row row;
                        if (showDataFromAllOperations)
                        {
                            foreach (var inventoryStatus in listOfInventoryStatusesForItem)
                            {
                                row = new Row();
                                row.AddText(inventoryStatus.StoreName);
                                row.AddText(roundingService.RoundForDisplay(DLLEntry.DataModel, lineItem.GetCalculatedNetAmount(withTax: true), false, true, DLLEntry.Settings.Store.Currency));

                                if (inventoryStatus.HasHeaderItem)
                                {
                                    row.AddText("N/A");
                                    inventoryStatusHasHeaderItem = true;
                                }
                                else
                                {
                                    row.AddText((loadLinkedItem ? LinkedItem.IsServiceItem : lookupItem.IsServiceItem) ? "-" : inventoryStatus.InventoryQuantityFormatted);
                                }

                                lvInventoryStatuses.AddRow(row);
                                variantName = inventoryStatus.VariantName;
                                if (inventoryStatus.StoreID == DLLEntry.DataModel.CurrentStoreID)
                                {
                                    lvInventoryStatuses.Selection.Set(lvInventoryStatuses.RowCount - 1);
                                }
                            }
                        }
                        else
                        {
                            row = new Row();
                            row.AddText(roundingService.RoundForDisplay(DLLEntry.DataModel, lineItem.GetCalculatedNetAmount(withTax: true), false, true, DLLEntry.Settings.Store.Currency));
                            variantName = lineItem.VariantName;
                            lvInventoryStatuses.AddRow(row);
                        }

                        //Add detail information for item
                        ItemDetailRow detailRow;
                        string detailText = "";

                        //Show periodic discount if needed
                        if ((lineItem.PeriodicDiscountWithTax != 0) || (lineItem.PeriodicDiscount != 0))
                        {
                            //Changed because of U.S. tax implementation because there the discounts are calculated on the grossamount and the tax comes later (Tobias, 24.04.2011)
                            if (((RetailTransaction)lineItem.Transaction).DisplayAmountsIncludingTax)
                            {
                                detailText = lineItem.PeriodicDiscountOfferName + ": " +
                                    Properties.Resources.Discount + " " +
                                    Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, lineItem.PeriodicDiscountWithTax, DLLEntry.Settings.Store.Currency, true) + " (" +
                                    Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, lineItem.PeriodicPctDiscount, DLLEntry.Settings.Store.Currency, false) + " %)"; //Discount

                                detailRow = new ItemDetailRow(detailText);
                                lvInventoryStatuses.AddRow(detailRow);
                            }
                            else
                            {
                                detailText = lineItem.PeriodicDiscountOfferName + ": " +
                                    Properties.Resources.Discount + " " +
                                    Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, lineItem.PeriodicDiscount, DLLEntry.Settings.Store.Currency, true) + " (" +
                                    Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, lineItem.PeriodicPctDiscount, DLLEntry.Settings.Store.Currency, false) + " %)"; //Discount

                                detailRow = new ItemDetailRow(detailText);
                                lvInventoryStatuses.AddRow(detailRow);
                            }
                        }

                        if ((lineItem.LineDiscountWithTax != 0) || (lineItem.LineDiscount != 0))
                        {
                            string discountName = Properties.Resources.LineDiscount;

                            var lineDiscountItem = lineItem.DiscountLines.FirstOrDefault(f => f.DiscountType == DiscountTransTypes.LineDisc && f.Origin == DiscountOrigin.POS);
                            if (lineDiscountItem != null && lineDiscountItem.DiscountName != "")
                            {
                                discountName = lineDiscountItem.DiscountName;
                            }
                            else
                            {
                                var customerDiscountItem = lineItem.DiscountLines.FirstOrDefault(f => f.DiscountType == DiscountTransTypes.Customer && f.Origin == DiscountOrigin.Custom);
                                if (customerDiscountItem != null && customerDiscountItem.DiscountName != "")
                                {
                                    discountName = customerDiscountItem.DiscountName;
                                }
                            }

                            var partnerDiscExist = lineItem.DiscountLines.FirstOrDefault(f => (f.DiscountType == DiscountTransTypes.Customer || f.DiscountType == DiscountTransTypes.LineDisc) && f.Origin == DiscountOrigin.Custom);
                            var normalDiscExist = lineItem.DiscountLines.FirstOrDefault(f => (f.DiscountType == DiscountTransTypes.Customer || f.DiscountType == DiscountTransTypes.LineDisc) && f.Origin == DiscountOrigin.POS);

                            //Changed because of U.S. tax implementation because there the discounts are calculated on the grossamount and the tax comes later (Tobias, 24.04.2011)
                            if (((RetailTransaction)lineItem.Transaction).DisplayAmountsIncludingTax)
                            {
                                if (normalDiscExist != null)
                                {

                                    detailText = discountName + ": " +
                                        Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, lineItem.LineDiscountWithTax, DLLEntry.Settings.Store.Currency, true) + " (" +
                                        Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, lineItem.LinePctDiscount, DLLEntry.Settings.Store.Currency, false) + " %)"; //Line discount

                                    detailRow = new ItemDetailRow(detailText);
                                    lvInventoryStatuses.AddRow(detailRow);
                                }

                                if (partnerDiscExist != null)
                                {
                                    foreach (IDiscountItem di in lineItem.DiscountLines.Where(w => w.Origin == DiscountOrigin.Custom))
                                    {
                                        detailText = di.DiscountName + ": " +
                                            Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, di.AmountWithTax, DLLEntry.Settings.Store.Currency, true) + " (" +
                                            Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, di.Percentage, DLLEntry.Settings.Store.Currency, false) + " %)"; //Line discount

                                        detailRow = new ItemDetailRow(detailText);
                                        lvInventoryStatuses.AddRow(detailRow);
                                    }
                                }
                            }
                            else
                            {
                                if (normalDiscExist != null)
                                {
                                    detailText = discountName + ": " +
                                        Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, lineItem.LineDiscount, DLLEntry.Settings.Store.Currency, true) + " (" +
                                        Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, lineItem.LinePctDiscount, DLLEntry.Settings.Store.Currency, false) + " %)"; //Line discount                                

                                    detailRow = new ItemDetailRow(detailText);
                                    lvInventoryStatuses.AddRow(detailRow);
                                }

                                if (partnerDiscExist != null)
                                {
                                    foreach (IDiscountItem di in lineItem.DiscountLines.Where(w => w.Origin == DiscountOrigin.Custom))
                                    {
                                        detailText = di.DiscountName + ": " +
                                            Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, di.Amount, DLLEntry.Settings.Store.Currency, true) + " (" +
                                            Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, di.Percentage, DLLEntry.Settings.Store.Currency, false) + " %)"; //Line discount

                                        detailRow = new ItemDetailRow(detailText);
                                        lvInventoryStatuses.AddRow(detailRow);
                                    }
                                }
                            }
                        }

                        //Show total discount if needed
                        if ((lineItem.TotalDiscountWithTax != 0) || (lineItem.TotalDiscount != 0))
                        {
                            string discountName = Properties.Resources.TotalDiscount;
                            var periodDiscountItem = lineItem.DiscountLines.FirstOrDefault(f => f.DiscountType == DiscountTransTypes.TotalDisc);

                            if (periodDiscountItem != null && periodDiscountItem.DiscountName != "")
                            {
                                discountName = periodDiscountItem.DiscountName;
                            }

                            //Changed because of U.S. tax implementation because there the discounts are calculated on the grossamount and the tax comes later (Tobias, 24.04.2011)
                            if (((RetailTransaction)lineItem.Transaction).DisplayAmountsIncludingTax)
                            {
                                detailText = discountName + ": " +
                                    Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, lineItem.TotalDiscountWithTax, DLLEntry.Settings.Store.Currency, true) + " (" +
                                    Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, lineItem.TotalPctDiscount, DLLEntry.Settings.Store.Currency, false) + " %)"; //Total discount

                                detailRow = new ItemDetailRow(detailText);
                                lvInventoryStatuses.AddRow(detailRow);
                            }
                            else
                            {
                                detailText = discountName + ": " +
                                    Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, lineItem.TotalDiscount, DLLEntry.Settings.Store.Currency, true) + " (" +
                                    Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, lineItem.TotalPctDiscount, DLLEntry.Settings.Store.Currency, false) + " %)"; //Total discount  

                                detailRow = new ItemDetailRow(detailText);
                                lvInventoryStatuses.AddRow(detailRow);
                            }
                        }

                        //Show loyalty discount if needed
                        if ((lineItem.LoyaltyDiscountWithTax != 0) || (lineItem.LoyaltyDiscount != 0))
                        {
                            string discountName = Properties.Resources.LoyaltyDiscount;
                            var periodDiscountItem = lineItem.DiscountLines.FirstOrDefault(f => f.DiscountType == DiscountTransTypes.LoyaltyDisc);

                            if (periodDiscountItem != null && periodDiscountItem.DiscountName != "")
                            {
                                discountName = periodDiscountItem.DiscountName;
                            }

                            //Changed because of U.S. tax implementation because there the discounts are calculated on the grossamount and the tax comes later (Tobias, 24.04.2011)
                            if (((RetailTransaction)lineItem.Transaction).DisplayAmountsIncludingTax)
                            {
                                detailText = discountName + ": " +
                                    Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, lineItem.LoyaltyDiscountWithTax, DLLEntry.Settings.Store.Currency, true) + " (" +
                                    Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, lineItem.LoyaltyPctDiscount, DLLEntry.Settings.Store.Currency, false) + " %)";

                                detailRow = new ItemDetailRow(detailText);
                                lvInventoryStatuses.AddRow(detailRow);
                            }
                            else
                            {
                                detailText = discountName + ": " +
                                    Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, lineItem.LoyaltyDiscount, DLLEntry.Settings.Store.Currency, true) + " (" +
                                    Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, lineItem.LoyaltyPctDiscount, DLLEntry.Settings.Store.Currency, false) + " %)";

                                detailRow = new ItemDetailRow(detailText);
                                lvInventoryStatuses.AddRow(detailRow);
                            }
                        }
                    }
                }
                #endregion Price check

                if (lookupItem.ItemType == ItemTypeEnum.AssemblyItem && (showDataFromAllOperations || !isPriceCheckOperation))
                {
                    lvInventoryStatuses.Rows.Add(new ItemDetailRow(Properties.Resources.AssemblyInventoryOnHandInfo, true));

                    if (inventoryStatusHasHeaderItem)
                    {
                        lvInventoryStatuses.Rows.Add(new ItemDetailRow(Properties.Resources.AssemblyHeaderItemInfo, true));
                    }
                }

                if (hasLinkedItems)
                {
                    lvInventoryStatuses.Rows.Add(new ItemDetailRow(Properties.Resources.HasLinkedItems, true));
                }
            }

            tbBarCode.Clear();
            tbBarCode.Text = "";
            lvInventoryStatuses.AutoSizeColumns();
            string itemText = Item.Text + (string.IsNullOrEmpty(variantName) ? string.Empty : " - " + variantName);

            if (!loadLinkedItem && !loadComponent)
            {
                SetItemHeader(itemText);
            }

            focusTimer.Start();
        }

        private void SearchClicked()
        {
            string itemId = "";
            DialogResult result = ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ItemSearch(100, ref itemId,
                ItemSearchViewModeEnum.Default, null, posTransaction);
            if (result != DialogResult.Cancel)
            {
                Item = Providers.RetailItemData.Get(DLLEntry.DataModel, itemId);

                if (tbBarCode.Text == "")
                {
                    tbBarCode.Text = itemId;
                }

                LoadItemInformation("", false, false);
            }
        }

        private void SellItem()
        {
            if(saleLineItem == null || BarCode == null)
            {
                return;
            }
            
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnPanel_Click(object sender, ScrollButtonEventArguments args)
        {
            if (args.Tag is RecordIdentifier)
            {
                if(args.Key == "Region")
                {
                    regionID = (RecordIdentifier)args.Tag;
                    regionDescription = args.Text;

                    if (Item != null)
                    {
                        LoadItemInformation("", LinkedItem != null, ComponentItem != null);
                    }
                }
                else if(args.Key == "Item")
                {
                    if ((RecordIdentifier)args.Tag == Item.ID)
                    {
                        if (LinkedItem != null || ComponentItem != null)
                        {
                            LoadItemInformation("", false, false);
                        }
                    }
                }
                else if(args.Key == "LinkedItem")
                {
                    if (LinkedItem == null || LinkedItem.ID != (RecordIdentifier)args.Tag)
                    {
                        LinkedItem = Providers.RetailItemData.Get(DLLEntry.DataModel, (RecordIdentifier)args.Tag);
                        LoadItemInformation("", true, false);
                    }
                }
                else if(args.Key == "ComponentItem")
                {
                    if (ComponentItem == null || ComponentItem.ID != (RecordIdentifier)args.Tag)
                    {
                        ComponentItem = Providers.RetailItemData.Get(DLLEntry.DataModel, (RecordIdentifier)args.Tag);
                        LoadItemInformation("", false, true);
                    }
                }
                
                return;
            }

            switch ((int)args.Tag)
            {
                case (int)Buttons.MyRegion:
                    regionID = DLLEntry.Settings.Store.RegionID;
                    if (Item != null)
                    {
                        regionDescription = RecordIdentifier.IsEmptyOrNull(DLLEntry.Settings.Store.RegionID) ? Resources.AllRegions : DLLEntry.Settings.Store.RegionDescription;
                        LoadItemInformation("", LinkedItem != null, ComponentItem != null);
                    }
                    break;
                case (int)Buttons.AllRegions:
                    regionID = RecordIdentifier.Empty;
                    if (Item != null)
                    {
                        regionDescription = Resources.AllRegions;
                        LoadItemInformation("", LinkedItem != null, ComponentItem != null);
                    }
                    break;
                case (int)Buttons.Regions:
                    AddButtons(!expandRegions, expandLinkedItems, expandComponents);
                    break;
                case (int)Buttons.LinkedItems:
                    AddButtons(expandRegions, !expandLinkedItems, expandComponents);
                    break;
                case (int)Buttons.Components:
                    AddButtons(expandRegions, expandLinkedItems, !expandComponents);
                    break;
                case (int)Buttons.Search:
                    SearchClicked();
                    break;
                case (int)Buttons.SellItem:
                    SellItem();
                    break;
                case (int)Buttons.Cancel:
                    Close();
                    break;
            }
        }

        private void AddButtons(bool expandRegions, bool expandLinkedItems, bool expandComponents)
        {
            btnPanel.Clear();
            this.expandRegions = expandRegions;
            this.expandLinkedItems = expandLinkedItems;
            this.expandComponents = expandComponents;

            if(!isPriceCheckOperation || showDataFromAllOperations)
            {
                List<DataLayer.BusinessObjects.StoreManagement.Region> regions = Providers.RegionData.GetList(DLLEntry.DataModel, DataLayer.BusinessObjects.StoreManagement.Region.SortEnum.Description, false);

                if (regions.Count > 0)
                {
                    btnPanel.AddButton(Resources.MyRegion, Buttons.MyRegion, Conversion.ToStr((int)Buttons.MyRegion));
                    btnPanel.AddButton(Resources.AllRegions, Buttons.AllRegions, Conversion.ToStr((int)Buttons.AllRegions));

                    if (regions.Count > 1)
                    {
                        btnPanel.AddButton(Resources.Regions, Buttons.Regions, Conversion.ToStr((int)Buttons.Regions), TouchButtonType.Action, DockEnum.DockNone, expandRegions ? Resources.white_line_arrow_up_16 : Resources.white_line_arrow_down_16, ImageAlignment.Left);

                        if (expandRegions)
                        {
                            foreach (var region in regions)
                            {
                                if(region.ID == regionID)
                                {
                                    btnPanel.AddButton(region.Text, region.ID, "Region", TouchButtonType.None);
                                }
                                else
                                {
                                    btnPanel.AddButton(region.Text, region.ID, "Region");
                                }
                            }
                        }
                    }
                }
            }

            if(hasLinkedItems)
            {
                btnPanel.AddButton(Resources.LinkedItems, Buttons.LinkedItems, Conversion.ToStr((int)Buttons.LinkedItems), TouchButtonType.Action, DockEnum.DockNone, expandLinkedItems ? Resources.white_line_arrow_up_16 : Resources.white_line_arrow_down_16, ImageAlignment.Left);

                if(expandLinkedItems)
                {
                    btnPanel.AddButton(Item.Text, Item.ID, "Item", TouchButtonType.None);

                    foreach (ISaleLineItem linkedItem in linkedItems)
                    {
                        if(LinkedItem != null && LinkedItem.ID == linkedItem.ItemId)
                        {
                            btnPanel.AddButton(linkedItem.Description, (RecordIdentifier)linkedItem.ItemId, "LinkedItem", TouchButtonType.None);
                        }
                        else
                        {
                            btnPanel.AddButton(linkedItem.Description, (RecordIdentifier)linkedItem.ItemId, "LinkedItem");
                        }
                    }
                }
            }

            if (hasComponents)
            {
                btnPanel.AddButton(Resources.Components, Buttons.Components, Conversion.ToStr((int)Buttons.Components), TouchButtonType.Action, DockEnum.DockNone, expandComponents ? Resources.white_line_arrow_up_16 : Resources.white_line_arrow_down_16, ImageAlignment.Left);

                if (expandComponents)
                {
                    btnPanel.AddButton(Item.Text, Item.ID, "Item", TouchButtonType.None);

                    foreach (ISaleLineItem component in ComponentItems)
                    {
                        if (ComponentItem != null && ComponentItem.ID == component.ItemId)
                        {
                            btnPanel.AddButton(component.Description, (RecordIdentifier)component.ItemId, "ComponentItem", TouchButtonType.None);
                        }
                        else
                        {
                            btnPanel.AddButton(component.Description, (RecordIdentifier)component.ItemId, "ComponentItem");
                        }
                    }
                }
            }

            btnPanel.AddButton(Resources.Search, Buttons.Search, Conversion.ToStr((int)Buttons.Search), TouchButtonType.Action, DockEnum.DockEnd);
            btnPanel.AddButton(Resources.SellItem, Buttons.SellItem, Conversion.ToStr((int)Buttons.SellItem), TouchButtonType.OK, DockEnum.DockEnd);
            btnPanel.AddButton(Resources.Cancel, Buttons.Cancel, Conversion.ToStr((int)Buttons.Cancel), TouchButtonType.Cancel, DockEnum.DockEnd);

            btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.SellItem), BarCode != null);
        }

        private void InventoryPriceCheckLookupDialog_Load(object sender, EventArgs e)
        {
            ActiveControl = tbBarCode;
            tbBarCode.Focus();
        }

        private void tnpNumpad_EnterPressed(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(tbBarCode.Text))
            {
                LoadItemInformation(tbBarCode.Text, false, false);
            }
        }

        private void tnpNumpad_ClearPressed(object sender, EventArgs e)
        {
            tbBarCode.Text = "";
        }

        private void tbBarCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                suppressKeyPress = true;

                LoadItemInformation(tbBarCode.Text, false, false);
            }
            else if (e.KeyCode == Keys.LineFeed)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            suppressKeyPress = false;
        }

        private void InventoryPriceCheckLookupDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            Scanner.ScannerMessageEvent -= ProcessScannedItem;
            Scanner.DisableForScan();
        }

        private void tbBarCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (suppressKeyPress)
            {
                return;
            }

            if (e.KeyChar == (char)Keys.Return || e.KeyChar == (char)Keys.Enter)
            {
                LoadItemInformation(tbBarCode.Text, false, false);
                e.Handled = true;
            }
        }
    }
}
