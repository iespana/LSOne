using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Dialogs;
using LSOne.Controls.Enums;
using LSOne.Controls.EventArguments;
using LSOne.Controls.OperationButtons;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.Hospitality.ListItems;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.DataLayer.KDSBusinessObjects.Enums;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.KitchenDisplaySystem.KdsClient;
using LSOne.KitchenDisplaySystem.KdsCommon;
using LSOne.Peripherals;
using LSOne.POS.Core;
using LSOne.POS.Processes.Common;
using LSOne.Services.Dialogs;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Delegates;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Services.SplitBill;
using LSOne.Services.StationPrinting;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Enums;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    public partial class HospitalityService : IHospitalityService, IDisposable
    {
        public event RunOperationDelegate RunOperation;
        public event SetMainViewIndexDelegate SetMainViewIndex;
        public event SetTransactionDelegate SetTransactionEvent;
        public event SetInputAbilityDelegate SetInputAbilityEvent;
        public event LoadPosDesignDelegate LoadPosDesignEvent;
        public event LogOffUserDelegate LogOffUserEvent;

        //private SqlConnection connection;
        private string dataAreaId;
        private Layout.HospitalityLayoutContainer layoutContainer;

        private ISiteServiceService siteService;
        private List<DiningTableList> DiningTableLists;
        private OperationButtons opHospitalityTypes;
        private HospitalityType activeHospitalityType;
        private bool isRunningTransferOperation;
        private DiningTable selectedTable;
        private DiningTable transferToTable;
        private OperationButtons opOperations;
        private DateTime lastClick;
        private DateTime resetLastClick;
        private Timer hospitalityTimer;
        private System.Timers.Timer kdsTryToReconnectTimer;
        private System.Timers.Timer kdsCheckConnectionTimer;
        private volatile bool kdsConnected;
        private volatile bool tableStatusUpdateFromKds;
        private Timer kdsConnectionStatusUiTimer;
        private bool lastKdsConnectionStatus;
        private bool justOneTerminal;
        private bool checkTableIsLocked;
        private bool posOperationCancelled;
        private RecordIdentifier currentWaiter;

        private List<HospitalityType> loadedHospitalityTypes;
        private List<SalesType> loadedSalesTypes;
        private HospitalitySetup hospitalitySetup;

        private MainViewEnum mainViewIndex;
        private bool runFirstUpdate;

        private LSOne.KitchenDisplaySystem.KdsClient.Client kdsConnection;
        private bool reloadPOSDesign;
        private int currentGestNumber = 1;
        private Dictionary<Guid, KitchenDisplayOrder> kdsOrders;
        private List<KitchenDisplayOrder> stagedOrders;
        private Object stagedOrdersLock = new Object();
        private bool usingKds;

        private RecordIdentifier orgDefaultCustomerID;
        private bool orgUseDefaultCustomer;

        private bool paymentStarted;
        private IConnectionManager dataModel; // NOTE: Only use this for error logging and basic operations. We should always pass the entry as a parameter to functions

        protected virtual void CheckHospitalityTerminalList(IConnectionManager entry)
        {
            List<DataEntity> list = Providers.TerminalData.GetHospitalityTerminalList(entry,
                                                                              entry.CurrentStoreID);

            justOneTerminal = !(list.FirstOrDefault(f => f.ID != entry.CurrentTerminalID) != null);

        }

        protected virtual void SetMainView(MainViewEnum index)
        {
            mainViewIndex = index;
            if (SetMainViewIndex != null)
            {
                SetMainViewIndex((int)index);
            }
        }

        #region IHospitality Members

        #region Transfer table

        public virtual HospitalityResult TransferTable(IConnectionManager entry, IRetailTransaction FromTransaction,
                                               IRetailTransaction ToTransaction, int FromTableId, int ToTableId)
        {
            HospitalityResult OperationResult = new HospitalityResult();

            if (((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).VisualProfile.TerminalType ==
                VisualProfile.HardwareTypes.Touch)
            {
                SplitBillDialog frmSplit = new SplitBillDialog(entry, 1M, FromTransaction, ToTransaction, FromTableId, ToTableId,
                                                         Enums.SplitAction.TransferLines,
                                                         activeHospitalityType.TransferLinesLookupID.ToString(),
                                                         activeHospitalityType,
                                                         selectedTable);
                POSFormsManager.ShowPOSForm(frmSplit);
                OperationResult = frmSplit.OperationResult;
                frmSplit.Dispose();

                //Update the transaction to the current waiter before sending the transfer ticket to the kitchen so
                //that the correct operator is on the ticket and transaction
                UpdateOperatorInformationOnTransaction(entry, currentWaiter, ToTransaction);
                ToTransaction.UseTaxGroupFrom = FromTransaction.UseTaxGroupFrom;

                HospitalityType fromHospitalityType = Providers.HospitalityTypeData.Get(entry,
                                                                              entry.CurrentStoreID,
                                                                              FromTransaction.Hospitality.ActiveHospitalitySalesType);

                PrintTransferSlip(entry, ToTransaction, FromTransaction.Hospitality.TableInformation.TableID, fromHospitalityType);
                TransferOrderOnKDS(entry, FromTransaction, ToTransaction, FromTableId, ToTableId);

                if (FromTransaction.SaleItems.Count == 0)
                {
                    FromTransaction = null;
                }

                if (ToTransaction.SaleItems.Count == 0)
                {
                    ToTransaction = null;
                }
            }

            return OperationResult;
        }

        private void TransferOrderOnKDS(IConnectionManager entry, IRetailTransaction FromTransaction, IRetailTransaction ToTransaction, int FromTableId, int ToTableId)
        {
            if (!usingKds || ToTransaction.SaleItems.Count == 0) return;

            string oldTableNumber = "";
            if(FromTransaction.SaleItems.Count == 0 && !kdsOrders.ContainsKey(ToTransaction.KDSOrderID)) //Full table transfer to an empty table
            {
                if(kdsOrders.ContainsKey(FromTransaction.KDSOrderID))
                {
                    ToTransaction.KDSOrderID = FromTransaction.KDSOrderID;
                    kdsOrders[FromTransaction.KDSOrderID].TableNumber = GetTableNumberOrOrderNumber(entry, ToTransaction);
                    oldTableNumber = GetTableNumberOrOrderNumber(entry, FromTransaction);
                }
            }
            else //Partial transfer or transfer to an existing order
            {
                if (kdsOrders.ContainsKey(FromTransaction.KDSOrderID))
                {
                    KitchenDisplayOrder oldOrder = kdsOrders[FromTransaction.KDSOrderID];
                    KitchenDisplayOrder order;

                    if(kdsOrders.ContainsKey(ToTransaction.KDSOrderID))
                    {
                        order = kdsOrders[ToTransaction.KDSOrderID];
                    }
                    else
                    {
                        order = CreateKDSOrder(entry, ToTransaction);
                    }

                    foreach(SaleLineItem item in ToTransaction.SaleItems)
                    {
                        KitchenDisplayItem kdsItem = oldOrder.Items.Find(x => (Guid)x.KdsId == item.KdsId);

                        if(kdsItem != null)
                        {
                            oldOrder.Items.Remove(kdsItem);
                            kdsItem.LineId = item.LineId;
                            order.Items.Add(kdsItem);
                        }
                    }

                    foreach (SaleLineItem item in FromTransaction.SaleItems)
                    {
                        KitchenDisplayItem kdsItem = oldOrder.Items.Find(x => (Guid)x.KdsId == item.KdsId);

                        if (kdsItem != null)
                        {
                            kdsItem.LineId = item.LineId;
                        }
                    }

                    if(FromTransaction.SaleItems.Count == 0)
                    {
                        kdsOrders.Remove(FromTransaction.KDSOrderID);
                        //Remove old order from kds - waiting for KDS team to find a solution to this
                    }
                    else
                    {
                        SendToKitchenStations(entry, FromTransaction);
                    }
                }
            }

            if(kdsOrders.ContainsKey(ToTransaction.KDSOrderID))
            {
                KitchenDisplayOrder kdsOrder = kdsOrders[ToTransaction.KDSOrderID];

                foreach (KitchenDisplayItem kdsItem in kdsOrder.Items)
                {
                    KitchenDisplayItemModifier transferModifier = kdsItem.ItemModifiers.Find(x => x.ModifierType == ModifierTypeEnum.Comment && x.Text.StartsWith(Resources.TableTransfer));

                    if(transferModifier != null)
                    {
                        transferModifier.Text += $" > {kdsOrder.TableNumber}";
                    }
                    else
                    {
                        transferModifier = new KitchenDisplayItemModifier
                        {
                            ID = kdsItem.ID,
                            LineID = kdsItem.LineId,
                            ModifierType = ModifierTypeEnum.Comment,
                            Text = Resources.TableTransfer + string.Format(" {0} > {1}", oldTableNumber, kdsOrder.TableNumber)
                        };

                        kdsItem.ItemModifiers.Add(transferModifier);
                    }
                }

                SendToKitchenStations(entry, ToTransaction);
            }
        }

        #endregion

        #region Station printer

        public virtual void PrintTransferSlip(IConnectionManager entry, IRetailTransaction retailTransaction, int fromTableID, HospitalityType fromHospitalityType)
        {
            HospitalityType currentHospitalityType = GetHospitalityType(entry, retailTransaction.Hospitality.ActiveHospitalitySalesType);

            if (currentHospitalityType != null && !currentHospitalityType.SendTransfersToStation)
            {
                return;
            }

            if (currentHospitalityType != null && currentHospitalityType.StationPrinting == HospitalityType.StationPrintingEnum.Manual)
            {
                return;
            }

            // No items to print
            if (retailTransaction.SaleItems.Count == 0)
            {
                return;
            }

            //No items have been printed
            if (AreAnyItemsPrinted(retailTransaction) == false)
            {
                return;
            }

            List<(StationSelection StationSelection, StationPrinter StationPrinter)> printerRoutes = GetStationRoutings(entry);

            //This should only retrieve items that have been printed from split bill so the filter should only look for Printed items
            FindItemsToPrint(entry, w => w.PrintingStatus == SalesTransaction.PrintStatus.Printed, retailTransaction.Hospitality.ActiveHospitalitySalesType, retailTransaction.SaleItems, printerRoutes);

            foreach (StationPrinter printer in printerRoutes.Select(x => x.StationPrinter).Where(x => x.ItemsToPrint.Count != 0))
            {
                printer.PrintTransferSlip(entry, retailTransaction, fromTableID, fromHospitalityType);
            }
        }

        private List<(StationSelection StationSelection, StationPrinter StationPrinter)> GetStationRoutings(IConnectionManager entry)
        {
            //// Get all the available restaurant stations for the restaurant
            List<PrintingStation> printingStations = Providers.PrintingStationData.GetList(entry);

            // Get all the available station selections for the restaurant
            List<StationSelection> stationSelection = Providers.StationSelectionData.GetList(entry).OrderByDescending(p => p.Type).ToList();

            // Create station printer for each station selection
            List<(StationSelection StationSelection, StationPrinter StationPrinter)> printerRoutes = new List<(StationSelection StationSelection, StationPrinter StationPrinter)>();

            foreach (StationSelection station in stationSelection)
            {
                StationPrinter printer = new StationPrinter(entry);
                printer.RestaurantStation = printingStations.FirstOrDefault(x => x.ID == station.StationID);

                printerRoutes.Add((station, printer));
            }

            return printerRoutes;
        }

        protected virtual Func<ISaleLineItem, bool> GetPrintFilter()
        {
            if (activeHospitalityType.SendVoidedItemsToStation)
            {
                return w => w is SaleLineItem;
            }
            else
            {
                return w => w.Voided == false;
            }
        }

        protected virtual bool FindItemsToPrint(IConnectionManager entry, Func<ISaleLineItem, bool> filter, RecordIdentifier salesType, IEnumerable<ISaleLineItem> items, List<(StationSelection StationSelection, StationPrinter StationPrinter)> printerRoutes)
        {
            foreach (SaleLineItem item in items.Where(filter))
            {
                foreach (
                    (StationSelection StationSelection, StationPrinter StationPrinter) station in
                        printerRoutes.Where(
                            w =>
                            w.StationSelection.RestaurantID.StringValue == entry.CurrentStoreID.StringValue &&
                            w.StationSelection.SalesType.StringValue == salesType.StringValue))
                {
                    bool include = false;
                    if ((StationSelection.TypeEnum)(int)station.StationSelection.Type == StationSelection.TypeEnum.Item)
                    {
                        include = (item.ItemId == station.StationSelection.Code.ToString());
                    }
                    else if ((StationSelection.TypeEnum)(int)station.StationSelection.Type == StationSelection.TypeEnum.RetailGroup)
                    {
                        include = (item.RetailItemGroupId == station.StationSelection.Code.ToString());
                    }
                    else if ((StationSelection.TypeEnum)(int)station.StationSelection.Type ==
                             StationSelection.TypeEnum.SpecialGroup)
                    {
                        include = Providers.SpecialGroupData.ItemInSpecialGroup(entry,
                                                                      station.StationSelection.Code.ToString(), item.ItemId);
                    }
                    else if ((StationSelection.TypeEnum)(int)station.StationSelection.Type == StationSelection.TypeEnum.All)
                    {
                        include = true;
                    }

                    //If printers is null then we're just checking if there are any items to print on the transaction
                    if (station.StationPrinter == null && include)
                    {
                        return true;
                    }

                    if (station.StationPrinter != null && include)
                    {
                        //Need to check if the item has already been added to the station print list, 
                        //f.ex. if routing is setup to have All items and items in Retail group sent to a specific printer
                        //if this check is not done then the same item is sent twice to the station printer
                        if (station.StationPrinter.ItemsToPrint.FirstOrDefault(f => f.ItemId == item.ItemId && f.LineId == item.LineId) == null)
                        {
                            station.StationPrinter.ItemsToPrint.Add(item);
                        }
                    }
                }
            }
            return false;
        }

        public virtual bool PreparationForPayment(IConnectionManager entry, IPosTransaction posTransaction)
        {
            if (!(posTransaction is RetailTransaction))
            {
                return true;
            }

            if (!((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.IsHospitality)
            {
                return true;
            }

            if(!CheckSiteServiceConnectionWithCancel(entry))
            {
                return false;
            }

            if (posTransaction.HasBeenSentToStation)
            {
                return true;
            }

            paymentStarted = true;
            posTransaction.HasBeenSentToStation = ((RetailTransaction)posTransaction).SaleItems.Count(p => p.PrintingStatus == SalesTransaction.PrintStatus.Printable) == 0;
            return true;
        }

        public virtual bool PreparationForVoid(IConnectionManager entry, IPosTransaction posTransaction)
        {
            if (!(posTransaction is RetailTransaction))
            {
                return true;
            }

            if (!((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.IsHospitality)
            {
                return true;
            }

            if (!CheckSiteServiceConnectionWithCancel(entry))
            {
                return false;
            }

            return true;
        }

        public virtual bool SendToStationPrinter(IConnectionManager entry, IRetailTransaction transaction, bool sendAllRemainingItems, bool isPaymentOperation)
        {
            if (transaction == null || (transaction.HasBeenSentToStation && AreAllItemsPrinted(transaction)))
            {
                return false;
            }

            HospitalityType defaultHospitalityType = GetHospitalityType(entry, ((RetailTransaction)transaction).Hospitality.ActiveHospitalitySalesType);


            if (!((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.IsHospitality ||
                ((RetailTransaction)transaction).SaleItems.Count == 0 ||
                AreAllItemsPrinted((RetailTransaction)transaction))
            {
                return false;
            }


            if (defaultHospitalityType == null ||
                !defaultHospitalityType.SendSuspensionsToStation &&
                transaction.EntryStatus == TransactionStatus.OnHold)
            {
                return false;
            }

            List<SaleLineItem> itemsToPrint = new List<SaleLineItem>();
            List<ISaleLineItem> itemsToKitchenDisplayStation = new List<ISaleLineItem>();

            string printAll = Resources.PrintAll;
            List<string> menuTypeNames = CreateMenuTypeDisplay(entry, (RetailTransaction)transaction, defaultHospitalityType, printAll);

            if (defaultHospitalityType.StationPrinting == HospitalityType.StationPrintingEnum.AlwaysPrintAll || menuTypeNames == null ||
                (sendAllRemainingItems && defaultHospitalityType.StationPrinting == HospitalityType.StationPrintingEnum.AtItemAddedOneDelay))
            {
                // Send all items belonging to the selected menu type to the station printer and mark them as printed...
                foreach (SaleLineItem lineItem in ((RetailTransaction)transaction).SaleItems)
                {
                    itemsToKitchenDisplayStation.Add(lineItem);

                    // Items are only sent to the station printer once!
                    if (lineItem.PrintingStatus == SalesTransaction.PrintStatus.Printable)
                    {
                        // Send the item to the station printer and mark it as printed.
                        itemsToPrint.Add(lineItem);
                        lineItem.PrintingStatus = SalesTransaction.PrintStatus.Printed;
                        lineItem.LastPrintedQty = lineItem.Quantity;
                    }
                }

                SendItemsToStationPrinter(entry, itemsToPrint, (RetailTransaction)transaction);
                SendToKitchenStations(entry, (RetailTransaction)transaction, itemsToKitchenDisplayStation);
            }
            else if (defaultHospitalityType.StationPrinting == HospitalityType.StationPrintingEnum.AtItemAdded)
            {
                // Should always be the last item
                ISaleLineItem lineItem = transaction.GetItem(transaction.SaleItems.Count);

                if (lineItem.PrintingStatus == SalesTransaction.PrintStatus.Printable)
                {
                    lineItem.PrintingStatus = SalesTransaction.PrintStatus.Printed;
                    lineItem.LastPrintedQty = lineItem.Quantity;
                }

                SendItemsToStationPrinter(entry, new List<SaleLineItem>() { (SaleLineItem)lineItem }, transaction);
                SendToKitchenStations(entry, transaction);
            }
            else if (defaultHospitalityType.StationPrinting == HospitalityType.StationPrintingEnum.AtItemAddedOneDelay)
            {
                if (transaction.SaleItems.Count(x => !x.IsLinkedItem) < 2)
                {
                    return false;
                }

                ISaleLineItem lineItem = transaction.SaleItems.Last(x => !x.IsLinkedItem);

                if (lineItem.PrintingStatus == SalesTransaction.PrintStatus.Printable)
                {
                    lineItem.PrintingStatus = SalesTransaction.PrintStatus.Printed;
                    lineItem.LastPrintedQty = lineItem.Quantity;
                }

                SendItemsToStationPrinter(entry, new List<SaleLineItem>() { (SaleLineItem)lineItem }, transaction);

                // Exclude the newest item
                List<ISaleLineItem> excludeList = transaction.ISaleItems.Where(x => !(x.LineId == lineItem.LineId || x.LinkedToLineId == lineItem.LineId)).ToList();
                SendToKitchenStations(entry, transaction, excludeList);
            }
            else if (defaultHospitalityType.StationPrinting == HospitalityType.StationPrintingEnum.Manual && transaction.LastRunOperation != POSOperations.PrintHospitalityMenuType)
            {
                return false;
            }
            else if (defaultHospitalityType.StationPrinting == HospitalityType.StationPrintingEnum.AtPosPayment && !isPaymentOperation)
            {
                return false;
            }
            else
            {
                List<string> selectedMenuTypes = new List<string>();
                DialogResult dlgResult = DialogResult.Cancel;

                if (transaction.EntryStatus == TransactionStatus.Normal || transaction.EntryStatus == TransactionStatus.OnHold)
                {
                    if (menuTypeNames.Count == 0)
                    {
                        dlgResult = Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.SendAllItemsToStationQuestion, Properties.Resources.SendToStation, MessageBoxButtons.YesNo);
                        selectedMenuTypes.Add(printAll);
                    }
                    else
                    {
                        using (var dlg = new MenuTypeSelectionDialog(menuTypeNames, Properties.Resources.SendToStation, allowMultiSelection: true))
                        {
                            dlgResult = dlg.ShowDialog();
                            selectedMenuTypes = dlg.SelectedMenuTypes;
                        }
                    }
                }

                if (dlgResult == DialogResult.OK || dlgResult == DialogResult.Yes)
                {
                    foreach (var selectedMenuType in selectedMenuTypes)
                    {
                        // Send all items belonging to the selected menu type to the station printer and mark them as printed...
                        foreach (SaleLineItem lineItem in ((RetailTransaction)transaction).SaleItems)
                        {
                            if (selectedMenuType == printAll ||
                                lineItem.MenuTypeItem.DisplayText == selectedMenuType ||
                                lineItem.PrintingStatus == SalesTransaction.PrintStatus.Printed)
                            {
                                itemsToKitchenDisplayStation.Add(lineItem);

                                // Items are only sent to the station printer once!
                                if (lineItem.PrintingStatus == SalesTransaction.PrintStatus.Printable)
                                {
                                    // Send the item to the station printer and mark it as printed.
                                    itemsToPrint.Add(lineItem);
                                    lineItem.PrintingStatus = SalesTransaction.PrintStatus.Printed;
                                    lineItem.LastPrintedQty = lineItem.Quantity;
                                }
                            }
                        }
                    }

                    SendToKitchenStations(entry, (RetailTransaction)transaction, itemsToKitchenDisplayStation);
                }
                else if (dlgResult == DialogResult.Cancel)
                {
                    return false;
                }
                else if (transaction.EntryStatus == TransactionStatus.Voided || transaction.EntryStatus == TransactionStatus.OnHold)
                {
                    foreach (SaleLineItem lineItem in ((RetailTransaction)transaction).SaleItems.Where(w => w.PrintingStatus == SalesTransaction.PrintStatus.Printable))
                    {
                        itemsToPrint.Add(lineItem);
                        lineItem.PrintingStatus = SalesTransaction.PrintStatus.Printed;
                    }
                }
                SendItemsToStationPrinter(entry, itemsToPrint, (RetailTransaction)transaction);
            }

            transaction.HasBeenSentToStation = AreAllItemsPrinted((RetailTransaction)transaction);
            return true;
        }

        protected virtual List<string> CreateMenuTypeDisplay(IConnectionManager entry, RetailTransaction transaction, HospitalityType defaultHospitalityType, string printAll)
        {
            //If everything should always be printed out there is no need to create this display data
            if (defaultHospitalityType.StationPrinting == HospitalityType.StationPrintingEnum.AlwaysPrintAll)
            {
                return null;
            }

            List<RestaurantMenuType> restaurantMenuTypes = Providers.RestaurantMenuTypeData.GetList(entry, entry.CurrentStoreID);

            List<string> menuTypeNames = new List<string>();
            foreach (RestaurantMenuType menuType in restaurantMenuTypes)
            {
                if (transaction.SaleItems.Count(
                        c => c.PrintingStatus == SalesTransaction.PrintStatus.Printable && c.MenuTypeItem.Exists && c.MenuTypeItem.CodeOnPos == menuType.CodeOnPos) > 0)
                {
                    menuTypeNames.Add(menuType.DisplayText);
                }
            }

            return menuTypeNames;
        }

        protected virtual void SendToKitchenStations(IConnectionManager entry, IRetailTransaction retailTransaction, List<ISaleLineItem> itemsToExclude = null)
        {
            if (!usingKds) return;

            foreach (var pair in kdsOrders)
            {
                var order = pair.Value;

                if (VoidedAndNotSentToKds(order))
                {
                    continue;
                }

                OrderSent(order);

                UpdateCustomer(order, retailTransaction);
                AddItemModifiersToOrder(order, retailTransaction);

                // Break order down to individual orders, one for each guest
                var gestsIds = order.Items.Select(x => x.DealId).Distinct();
                foreach (var guestId in gestsIds)
                {
                    var clonedOrder = order.DeepClone();
                    clonedOrder.ID = order.ID + "-" + guestId;
                    clonedOrder.Items = order.Items.Where(x => x.DealId == guestId.ToString()).ToList();

                    if (itemsToExclude != null)
                    {
                        clonedOrder.Items.RemoveAll(item => !itemsToExclude.ConvertAll(i => i.KdsId).Contains((Guid)item.KdsId));
                    }

                    if (clonedOrder.Items.Count(c => !c.Voided) > 0)
                    {
                        HospitalityType currentHospitalityType = GetHospitalityType(entry, retailTransaction.Hospitality.ActiveHospitalitySalesType);
                        if (!currentHospitalityType.SendVoidedItemsToStation)
                        {
                            clonedOrder.Items.RemoveAll(item => item.Voided);
                        }
						lock (stagedOrdersLock)
						{
                            if (kdsConnected)
                            {
                                try
                                {
                                    kdsConnection.SendOrder(clonedOrder);
                                }
                                catch
                                {
                                    stagedOrders.Add(clonedOrder);
                                }
                            }
                            else
                            {
                                stagedOrders.Add(clonedOrder);
                            }
                        }
                    }
                }
            }

            if (selectedTable != null)
            {
                selectedTable.Transaction = retailTransaction;
            }

            MarkTableAsSent(entry);
        }

        protected virtual void AddItemModifiersToOrder(KitchenDisplayOrder order, IRetailTransaction retailTransaction)
        {
            // LS One stores item modfiers as linked items on sale line items.
            // Kds items store Ls One item modifiers with type Increase.
            // All the modifiers are thrown out and rebuilt because we don't get a call when an item modifier is added
            foreach (var kdsItem in order.Items)
            {
                kdsItem.ItemModifiers.RemoveAll(
                    mod => mod.ModifierType == ModifierTypeEnum.IncreaseModifier);
            }

            HospitalityType currentHospitalityType = GetHospitalityType(dataModel, retailTransaction.Hospitality.ActiveHospitalitySalesType);

            var saleItemList = retailTransaction.SaleItems.ToList();
            for (int i = 0; i < retailTransaction.SaleItems.Count; i++)
            {
                var saleItem = saleItemList[i];

                if (saleItem.IsAssemblyComponent && saleItem.ParentAssembly.SendComponentsToKdsAsItemModifiers)
                {
                    if (!currentHospitalityType.SendVoidedItemsToStation && saleItem.Voided)
                    {
                        continue;
                    }

                    var kdsItem = GetKdsItem(retailTransaction.GetItem(saleItem.AssemblyParentLineID).KdsId);

                    if (kdsItem == null)
                    {
                        continue;
                    }

                    var kdsItemModifier = new KitchenDisplayItemModifier();
                    kdsItemModifier.ID = saleItem.ItemId;
                    kdsItemModifier.LineID = i;
                    kdsItemModifier.Text = saleItem.Description;
                    kdsItemModifier.ModifierType = ModifierTypeEnum.IncreaseModifier;
                    kdsItemModifier.Voided = saleItem.Voided;
                    kdsItemModifier.Quantity = saleItem.Quantity;
                    kdsItem.ItemModifiers.Add(kdsItemModifier);
                }
            }
        }

        private void UpdateCustomer(KitchenDisplayOrder order, IRetailTransaction retailTransaction)
        {
            if (order.GetDataFieldValue("CustomerName") != (retailTransaction.Customer?.FirstName ?? ""))
            {
                order.SetDataField("CustomerName", (retailTransaction.Customer != null) ? retailTransaction.Customer.FirstName : "");
            }
        }

        protected virtual bool VoidedAndNotSentToKds(KitchenDisplayOrder order)
        {
            return (order.Items.All(item => item.Voided)) && !order.RushOrder; // RushOrder flag is set when order is sent.
        }

        protected virtual void OrderSent(KitchenDisplayOrder order)
        {
            order.RushOrder = true; // Used to maintain which orders have been sent
        }

        protected virtual void MarkTableAsSent(IConnectionManager entry)
        {
            if (selectedTable != null &&
                (selectedTable.Details.DiningTableStatus != DiningTableStatus.OrderSent ||
                selectedTable.Details.DiningTableStatus != DiningTableStatus.OrderStarted))
            {
                selectedTable.Details.DiningTableStatus = DiningTableStatus.OrderSent;
                selectedTable.Details.KitchenStatus = KitchenOrderStatusEnum.None;
                selectedTable.Save(entry, siteService, (string)entry.CurrentTerminalID, "");
            }
        }

        protected virtual void SendItemsToStationPrinter(IConnectionManager entry, List<SaleLineItem> items, IRetailTransaction retailTransaction)
        {
            List<(StationSelection StationSelection, StationPrinter StationPrinter)> printerRoutes = GetStationRoutings(entry);

            FindItemsToPrint(entry, GetPrintFilter(), retailTransaction.Hospitality.ActiveHospitalitySalesType, items, printerRoutes);

            // Loop through all the stations and print the items going to each station
            foreach (StationPrinter temp in printerRoutes.Select(x => x.StationPrinter).Where(x => x.ItemsToPrint.Count != 0))
            {
                if (!temp.Print(entry, retailTransaction))
                {
                    foreach (SaleLineItem sli in temp.ItemsToPrint.Where(w => w.PrintingStatus == SalesTransaction.PrintStatus.Printed))
                    {
                        sli.PrintingStatus = SalesTransaction.PrintStatus.Printable;
                    }
                }
            }
        }

        /// <summary>
        /// Indicates wether any of the sale items in the given transactions will be sent to a printing station for the given hospitality type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The transaction containing the items to check for</param>
        /// <returns></returns>
        protected virtual bool TransactionContainsItemsToPrint(IConnectionManager entry, RetailTransaction transaction)
        {
            List<StationSelection> stationSelections = Providers.StationSelectionData.GetList(entry);
            List<(StationSelection StationSelection, StationPrinter StationPrinter)> printerRoutes = new List<(StationSelection StationSelection, StationPrinter StationPrinter)>();
            stationSelections.ForEach(x => printerRoutes.Add((x, null)));

            return FindItemsToPrint(entry, GetPrintFilter(), transaction.Hospitality.ActiveHospitalitySalesType, transaction.SaleItems, printerRoutes);
        }

        public virtual void SetPrintingStatus(IConnectionManager entry, IPosTransaction posTransaction)
        {
            //if Hospitality is not active then don't continue
            if (((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.IsHospitality == false)
                return;

            if (posTransaction == null)
            {
                return;
            }

            if (!(posTransaction is RetailTransaction))
            {
                return;
            }

            foreach (SaleLineItem sli in ((RetailTransaction)posTransaction).SaleItems.Where(w => w.PrintingStatus != SalesTransaction.PrintStatus.Unprintable))
            {
                SetPrintingStatus(entry, posTransaction, sli, true);
            }
        }

        /// <summary>
        /// Sets the printing status on the items depending on actions that have been taken
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="lineItem">The currently selected item</param>
        /// /// <param name="linkedItemsGetChangedStatus">If the item has linked items should the linked items get a "Changed" status with the header item?</param>
        public void SetPrintingStatus(IConnectionManager entry, IPosTransaction posTransaction, IBaseSaleItem lineItem,
                                      bool linkedItemsGetChangedStatus)
        {
            //If the item is not a SaleLineItem then don't continue. Only the SaleLineItem has the printing status property
            if (!(lineItem is SaleLineItem))
                return;

            //if Hospitality is not active then don't continue
            if (((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.IsHospitality == false)
                return;

            SaleLineItem saleLineItem = (SaleLineItem)lineItem;

            // If the item has been printed before, it will become printable again
            if (saleLineItem.PrintingStatus == SalesTransaction.PrintStatus.Printed && saleLineItem.LastPrintedQty != saleLineItem.Quantity)
            {
                saleLineItem.PrintingStatus = SalesTransaction.PrintStatus.Printable;
                saleLineItem.ChangedForPreparation = true;

                SetLinkedItemPrintStatus(entry, posTransaction, saleLineItem, linkedItemsGetChangedStatus);
            }

            //If an item is already printed and then the table is opened, item qty is changed and then changed back to the same qty it was when printed then 
            //we don't want the item to be sent to the printer again
            else if (saleLineItem.PrintingStatus == SalesTransaction.PrintStatus.Printable &&
                     saleLineItem.LastPrintedQty == saleLineItem.Quantity)
            {
                saleLineItem.PrintingStatus = SalesTransaction.PrintStatus.Printed;
                saleLineItem.ChangedForPreparation = false;
                SetLinkedItemPrintStatus(entry, posTransaction, saleLineItem, false);
                return;
            }

            // if the item void status is toggled after the item has been printed once, then the item will be marked as "changed"
            if (lineItem.ChangedForPreparation == false)
            {
                if ((saleLineItem.PrintingStatus == SalesTransaction.PrintStatus.Printable) &&
                    (lineItem.Voided || posTransaction.EntryStatus == TransactionStatus.Voided))
                {
                    saleLineItem.PrintingStatus = SalesTransaction.PrintStatus.Unprintable;
                    SetLinkedItemPrintStatus(entry, posTransaction, saleLineItem, false);
                }
                else
                {
                    saleLineItem.PrintingStatus = SalesTransaction.PrintStatus.Printable;
                    SetLinkedItemPrintStatus(entry, posTransaction, saleLineItem, false);

                }
            }
        }

        /// <summary>
        /// Sets the printing status on the items depending on changes in item comments
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="lineItem">The currently selected item</param>
        /// <param name="newComment">The item line comments after the user has edited them</param>
        /// <param name="originalComment">The item line comments before the user edited them</param>
        public void SetPrintingStatus(IConnectionManager entry, IPosTransaction posTransaction, IBaseSaleItem lineItem,
                                      string originalComment, string newComment)
        {
            //If the item is not a SaleLineItem then don't continue. Only the SaleLineItem has the printing status property
            if (!(lineItem is SaleLineItem))
                return;

            //If the comments are not the same then set the printing status
            if (newComment != originalComment)
            {
                SetPrintingStatus(entry, posTransaction, lineItem, false);
            }
        }

        /// <summary>
        /// Sets the printing status on the items depending on changes in item quantity
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="lineItem">The currently selected item</param>
        /// <param name="newQty">The item line quantity after the user has edited them</param>
        /// <param name="originalQty">The item line quantity before the user edited them</param>
        public void SetPrintingStatus(IConnectionManager entry, IPosTransaction posTransaction, IBaseSaleItem lineItem,
                                      decimal originalQty, decimal newQty)
        {
            //If the item is not a SaleLineItem then don't continue. Only the SaleLineItem has the printing status property
            if (!(lineItem is SaleLineItem))
                return;

            //If the quantity is not the same then set the printing status
            if (newQty != originalQty)
            {
                SetPrintingStatus(entry, posTransaction, lineItem, true);
            }
        }

        /// <summary>
        /// If the selected item has linked item or is a linked item the status needs to be
        /// reflected on other items
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">the current transaction</param>
        /// <param name="lineItem">the selected item</param>
        private void SetLinkedItemPrintStatus(IConnectionManager entry, IPosTransaction posTransaction,
                                              SaleLineItem lineItem, bool linkedItemsGetChangedStatus)
        {
            //If the item is a linked item then get the header item and set the print status on that as well
            if (lineItem.IsLinkedItem || lineItem.IsInfoCodeItem)
            {
                ((RetailTransaction)posTransaction).GetItem(lineItem.LinkedToLineId).PrintingStatus = SalesTransaction.PrintStatus.Printable;
            }
            //If the item is a header item then get the linked items and set their status too
            else if (lineItem.IsReferencedByLinkItems)
            {
                foreach (
                    SaleLineItem sli in
                        (((RetailTransaction)posTransaction).SaleItems.Where(
                            w => (w.IsLinkedItem || w.IsInfoCodeItem) && w.LinkedToLineId == lineItem.LineId)))
                {
                    if ((lineItem.PrintingStatus == SalesTransaction.PrintStatus.Unprintable) &&
                        (lineItem.Voided || posTransaction.EntryStatus == TransactionStatus.Voided))
                    {
                        sli.PrintingStatus = SalesTransaction.PrintStatus.Unprintable;
                        sli.ChangedForPreparation = linkedItemsGetChangedStatus;
                    }
                    else if (sli.LastPrintedQty == sli.Quantity)
                    {
                        sli.ChangedForPreparation = false;
                        sli.PrintingStatus = SalesTransaction.PrintStatus.Printed;
                    }
                    else if (sli.LastPrintedQty != sli.Quantity)
                    {
                        sli.ChangedForPreparation = linkedItemsGetChangedStatus;
                        sli.PrintingStatus = SalesTransaction.PrintStatus.Printable;
                    }

                }
            }
        }

        #endregion

        #region Table layout panel

        public TableLayoutPanel GetHospPanel(
            IConnectionManager entry,
            SetMainViewIndexDelegate setMainViewIndexHandler,
            RunOperationDelegate runOperationHandler,
            SetTransactionDelegate setTransactionHandler,
            SetInputAbilityDelegate setInputAbilityHandler,
            LoadPosDesignDelegate loadPosDesignHandler,
            LogOffUserDelegate logOffUserHandler)
        {
            if (((((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.IsHospitality) &&
                 ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.SkipHospitalityTableView == false))
            {
                mainViewIndex = MainViewEnum.Hospitality;
                layoutContainer.HospitalityVisibleChanged += HospitalityLayoutContainer_VisibleChanged;
                SetTransactionEvent += setTransactionHandler;
                SetInputAbilityEvent += setInputAbilityHandler;
                LoadPosDesignEvent += loadPosDesignHandler;

                SetMainViewIndex += setMainViewIndexHandler;
                RunOperation += runOperationHandler;
                LogOffUserEvent += logOffUserHandler;
                return layoutContainer.HospPanel;
            }

            return null;
        }

        protected virtual void HospitalityLayoutContainer_VisibleChanged(IConnectionManager entry)
        {
            //When the table view is displayed for the first time the table status has not been 
            //drawn completely - this is necessary to get the correct status and colors on the tables
            //the first time the table view is displayed
            if (runFirstUpdate)
            {
                runFirstUpdate = false;
                UpdateTableStatus(entry);
            }
            //We need to display the "Welcome" msg on the line display when the user is in the table view
            LineDisplay.DisplayWelcomeMessage(((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Terminal.CustomerDisplayText1, ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Terminal.CustomerDisplayText2);
        }

        public virtual void SetHospitalityTypeText(IConnectionManager entry, string text)
        {
            layoutContainer.HospitalityTypeText = text;
        }

        public virtual void SetSelectedTableText(IConnectionManager entry, TableInfo details)
        {
            if (details != null)
            {
                DisplayHospitalityStatusText(details.Description, GetDiningTableStatusText(details));
                layoutContainer.SelectedTableText = details.Description;
            }
            else
            {
                layoutContainer.SelectedTableText = "-";
            }
        }

        public virtual string GetDiningTableStatusText(TableInfo details)
        {
            switch (details.DiningTableStatus)
            {
                case DiningTableStatus.Available:
                    return Resources.Available;
                case DiningTableStatus.GuestsSeated:
                    return Resources.GuestsSeated;
                case DiningTableStatus.OrderStarted:
                    return Resources.OrderStarted;
                case DiningTableStatus.OrderFinished:
                    return Resources.OrderFinished;
                case DiningTableStatus.Unavailable:
                    return Resources.Unavailable;
                case DiningTableStatus.OrderPrinted:
                    return Resources.OrderPrinted;
                case DiningTableStatus.OrderPartiallyPrinted:
                    return Resources.OrderPartiallyPrinted;
                case DiningTableStatus.OrderNotPrinted:
                    return Resources.OrderNotPrinted;
                case DiningTableStatus.OrderSent:
                    return Resources.OrderSent;
                case DiningTableStatus.OrderPartiallySent:
                    return Resources.OrderPartiallySent;
                case DiningTableStatus.OrderNotSent:
                    return Resources.OrderNotSent;
                default:
                    return Resources.UnknownState;
            }
        }

        #endregion

        #region Load Hospitality types

        protected virtual void LoadHospitalityTypes(IConnectionManager entry)
        {
            try
            {
                string[] split = ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Terminal.SalesTypeFilter.Split(new char[] { '|' });

                if (split.Length == 0)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.MustBeHospitalityType,
                                                            MessageBoxButtons.RetryCancel, MessageDialogType.Attention);
                    return;
                }

                // Get all the available hospitality types for the restaurant
                List<HospitalityTypeListItem> restaurantHospitalityTypes
                    = Providers.HospitalityTypeData.GetHospitalityTypesForRestaurant(entry,
                                                                           entry.CurrentStoreID);

                // and cross-reference them with the ones that should be available on this till
                List<HospitalityTypeListItem> terminalHospitalityTypes = new List<HospitalityTypeListItem>();
                foreach (HospitalityTypeListItem restaurantType in restaurantHospitalityTypes)
                {
                    for (int i = 0; i <= split.Length - 1; i++)
                    {
                        if (restaurantType.SalesType == split[i])
                        {
                            terminalHospitalityTypes.Add(restaurantType);
                            break;
                        }
                    }
                }

                if (terminalHospitalityTypes.Count == 0)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.NoHospitalityTypesFound, MessageBoxButtons.OK, MessageDialogType.Attention);
                    opHospitalityTypes = new OperationButtons(entry, layoutContainer.OperationsPanel, OperationButtonClick);
                    // Only displays operation panel with a single log off button
                    opHospitalityTypes.SetOperationButtons();
                    return;
                }

                POSMenuInfo menuHeader = new POSMenuInfo(entry);

                menuHeader.MenuHeader.ID = new RecordIdentifier("TOPMENUID");
                menuHeader.MenuId = "";
                menuHeader.MenuHeader.Columns = terminalHospitalityTypes.Count;
                menuHeader.MenuHeader.Rows = 1;

                // and the lines...

                int keyNum = 1;
                bool first = true;

                loadedHospitalityTypes.Clear();
                loadedSalesTypes.Clear();

                foreach (HospitalityTypeListItem terminalHospitalityType in terminalHospitalityTypes)
                {
                    // Get the hospitality type info for the selected hospitality type
                    HospitalityType selectedHospitalityType = Providers.HospitalityTypeData.Get(entry,
                                                                                      entry.CurrentStoreID,
                                                                                      terminalHospitalityType.SalesType);
                    SalesType selectedSalesType = Providers.SalesTypeData.Get(entry,
                                                                    terminalHospitalityType.SalesType);

                    if (selectedHospitalityType != null)
                    {
                        loadedHospitalityTypes.Add(selectedHospitalityType);

                        if (selectedSalesType != null)
                        {
                            loadedSalesTypes.Add(selectedSalesType);
                        }

                        // We use cachetype none in this case since we are editing business objects, and fetching them from the cache
                        // would affect the existing objects.
                        POSMenuInfo TopPosMenuId = new POSMenuInfo(entry,
                                                                   (string)selectedHospitalityType.TopPosMenuID,
                                                                   CacheType.CacheTypeNone);

                        DataLayer.BusinessObjects.TouchButtons.PosMenuLine button = TopPosMenuId.MenuLine.FirstOrDefault();
                        //PosMenuLine hospitalityTypeButton = new PosMenuLine();

                        // Get a copy of the first "Dining table layout ID"
                        if (first)
                        {
                            activeHospitalityType = selectedHospitalityType;
                            first = false;
                        }

                        if (button != null)
                        {
                            button.Parameter = (string)selectedHospitalityType.DiningTableLayoutID;
                            button.Sequence = selectedHospitalityType.Sequence;
                            button.KeyNo = keyNum++;
                            button.Text = selectedHospitalityType.Text;
                            button.Operation = 0;
                            button.Disabled = false;

                            // We need to manually handle "Use header configuration" since we are mixing and matching
                            // different pos menu lines from different pos menu headers
                            if (button.UseHeaderAttributes)
                            {
                                button.FontName = TopPosMenuId.MenuHeader.FontName;
                                button.FontSize = TopPosMenuId.MenuHeader.FontSize;
                                button.FontBold = TopPosMenuId.MenuHeader.FontBold;
                                button.FontItalic = TopPosMenuId.MenuHeader.FontItalic;
                                button.FontCharset = TopPosMenuId.MenuHeader.FontCharset;
                                button.ForeColor = TopPosMenuId.MenuHeader.ForeColor;
                                button.BackColor = TopPosMenuId.MenuHeader.BackColor;
                                button.BackColor2 = TopPosMenuId.MenuHeader.BackColor2;
                                button.GradientMode = TopPosMenuId.MenuHeader.GradientMode;
                                button.Shape = TopPosMenuId.MenuHeader.Shape;
                            }

                            button.UseHeaderAttributes = false;
                        }
                        else
                        {
                            button = new DataLayer.BusinessObjects.TouchButtons.PosMenuLine();
                            button.MenuID = "";
                            button.Sequence = selectedHospitalityType.Sequence;
                            button.KeyNo = keyNum++;
                            button.Text = selectedHospitalityType.Text;
                            button.Operation = 0;
                            button.ColumnSpan = 1;
                            button.RowSpan = 1;
                            button.Parameter = (string)selectedHospitalityType.DiningTableLayoutID;
                            button.Disabled = false;
                            button.BackColor = 0;
                            button.FontSize = 16;
                            button.FontBold = true;
                            button.ForeColor = -16777216;
                            button.BackColor = -16711808;
                            button.BackColor2 = -16744448;
                            button.GradientMode = GradientModeEnum.Vertical;
                            button.Shape = ShapeEnum.RoundRectangle;
                            button.UseHeaderAttributes = false;
                            button.UseHeaderFont = true;
                        }

                        menuHeader.MenuLine.Add(button);

                        // Get the Dining table layout for the selected hospitality type
                        RecordIdentifier tempIdentifier = new RecordIdentifier(selectedHospitalityType.RestaurantID,
                                                                               selectedHospitalityType.Sequence,
                                                                               selectedHospitalityType.SalesType,
                                                                               selectedHospitalityType.DiningTableLayoutID);

                        DiningTableLayout diningTableLayout = Providers.DiningTableLayoutData.Get(entry, tempIdentifier);

                        // Continue if there is any data
                        if (diningTableLayout != null)
                        {
                            // Create a table list
                            DiningTableList tableList = new DiningTableList();
                            tableList.DiningTableLayoutId = selectedHospitalityType.DiningTableLayoutID.ToString();
                            tableList.ColumnCount = diningTableLayout.DiningTableColumns;
                            tableList.RowCount = diningTableLayout.DiningTableRows;
                            tableList.HospitalitySalesType = diningTableLayout.SalesType.ToString();
                            tableList.Description = selectedHospitalityType.Text;


                            // Get the RestaurantDiningTableData for the Dining table layout
                            List<RestaurantDiningTable> restaurantDiningTables =
                                Providers.RestaurantDiningTableData.GetList(entry, diningTableLayout.RestaurantID,
                                                                  diningTableLayout.Sequence,
                                                                  diningTableLayout.SalesType,
                                                                  diningTableLayout.LayoutID);

                            DiningTable diningTable;
                            foreach (RestaurantDiningTable tempTable in restaurantDiningTables)
                            {
                                diningTable = new DiningTable(entry);
                                diningTable.Details.TableID = Convert.ToInt32(tempTable.DineInTableNo.ToString());
                                diningTable.Details.StoreID = (string)entry.CurrentStoreID;
                                diningTable.Details.SalesType = diningTableLayout.SalesType.ToString();
                                diningTable.Details.Sequence = Convert.ToInt32(diningTableLayout.Sequence.ToString());
                                diningTable.Details.DiningTableLayoutID = selectedHospitalityType.DiningTableLayoutID.ToString();
                                diningTable.Details.Description = tempTable.GetDescription(selectedHospitalityType.TableButtonDescription);
                                diningTable.TableDescription = diningTable.Details.Description;

                                if (tempTable.Availability == RestaurantDiningTable.AvailabilityEnum.NotAvailable)
                                {
                                    diningTable.Details.DiningTableStatus = DiningTableStatus.Unavailable;
                                }

                                UpdateDiningTableStatus(diningTable);

                                tableList.TableList.Add(diningTable);

                            }

                            DiningTableLists.Add(tableList);

                        }
                    }
                }

                opHospitalityTypes = new OperationButtons(entry, layoutContainer.HospTypesPanel,
                                                          LoadTableLayout);
                opHospitalityTypes.SetOperationButtons(menuHeader);
                opHospitalityTypes.SetDimTextWhenDisabled(false);

                // Load the table layout for the first dining table found above
                LoadHospitalityLayout(entry);
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), ex);
                throw;
            }

        }

        protected virtual string GetStaffDescriptionForButton(IConnectionManager entry, DiningTable diningTable, string LockedTxt)
        {
            string staff = "";
            if ((diningTable.Transaction != null) && (((RetailTransaction)diningTable.Transaction).Hospitality.ActiveHospitalitySalesType != null))
            {
                HospitalityType hospitalityType = GetHospitalityType(entry, ((RetailTransaction)diningTable.Transaction).Hospitality.ActiveHospitalitySalesType);

                if (hospitalityType != null)
                {
                    staff = GetStaffDescription(entry, hospitalityType, (RetailTransaction)diningTable.Transaction);
                }
            }

            if (diningTable.Details.Locked &&
                (Conversion.ToStr(diningTable.Details.TerminalID) != "" &&
                 diningTable.Details.TerminalID != (string)entry.CurrentTerminalID))
            {
                if (staff != "")
                {
                    return LockedTxt + " - " + staff;
                }
                else
                {
                    return LockedTxt;
                }
            }

            return staff;
        }

        protected virtual string GetStaffDescription(IConnectionManager entry, HospitalityType hospitalityType, POSUser posUser)
        {
            try
            {
                if (posUser == null)
                    return "";

                if (hospitalityType.TableButtonStaffDescription == HospitalityType.TableButtonStaffDescriptionEnum.ReceiptName)
                {
                    return posUser.NameOnReceipt == ""
                               ? entry.Settings.NameFormatter.Format(posUser.Name)
                               : posUser.NameOnReceipt;
                }
                else
                {
                    return (string)posUser.ID;
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), ex);
            }

            return "";
        }

        protected virtual string GetStaffDescription(IConnectionManager entry, HospitalityType hospitalityType, IRetailTransaction retailTransaction)
        {
            try
            {
                if (retailTransaction == null)
                {
                    return "";
                }

                if (hospitalityType.TableButtonStaffDescription == HospitalityType.TableButtonStaffDescriptionEnum.ReceiptName)
                {
                    return retailTransaction.Cashier.NameOnReceipt == ""
                               ? retailTransaction.Cashier.Name
                               : retailTransaction.Cashier.NameOnReceipt;
                }
                else
                {
                    return (string)retailTransaction.Cashier.Login;
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), ex);
            }

            return "";
        }

        #endregion

        #region Load operations

        public virtual void LoadOperationsMenu(IConnectionManager entry, HospitalityType selectedHospitalityType)
        {
            try
            {
                opOperations = new OperationButtons(entry, layoutContainer.OperationsPanel,
                                                    OperationButtonClick);
                opOperations.SetOperationButtons(selectedHospitalityType.PosLogonMenuID.ToString());
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), ex);
            }

        }

        protected virtual void OperationButtonClick(object sender, IConnectionManager entry, OperationBtnArgs e)
        {
            try
            {
                try
                {
                    switch ((HospitalityOperations)(int)e.OperationButtonInfo.Operation)
                    {
                        case HospitalityOperations.SeatGuests:
                            OperationSeatGuests(entry, true);
                            break;
                        case HospitalityOperations.SplitBill:
                            OperationSplitBill(entry, true, MainViewEnum.Hospitality);
                            break;
                        case HospitalityOperations.Transfer:
                            OperationTransfer(entry);
                            break;
                        case HospitalityOperations.ChangeStaff:
                            OperationChangeStaff(entry);
                            break;
                        case HospitalityOperations.RunPos:
                            OperationRunPOS(entry);
                            break;
                        case HospitalityOperations.PrintPreReceipt:
                            OperationPrintPreReceipt(entry);
                            break;
                        case HospitalityOperations.LogOff:
                            OperationLogOff(entry);
                            break;
                        case HospitalityOperations.StationPrint:
                            OperationStationPrint(entry);
                            break;
                        case HospitalityOperations.UnlockTable:
                            OperationUnlockTable(entry);
                            break;
                        default:
                            Interfaces.Services.DialogService(entry).ShowMessage(Resources.SelectedOperationNotAllowedInTableView);
                            break;
                    }
                }
                finally
                {
                    if (selectedTable != null && selectedTable.TableButton != null)
                    {
                        selectedTable.TableButton.Select();
                    }
                }
            }
            catch (Exception x)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(x is ClientTimeNotSynchronizedException ? x.Message : Resources.OperationCannotBePerformed, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                entry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), x);
            }
        }

        protected virtual void OperationSeatGuests(IConnectionManager entry, bool shouldClearTableLock)
        {
            // Check if any table is selected
            if (IsTableSelected(entry) == false)
                return;

            // Check if the selected table is available
            if (IsTableAvailable(entry, selectedTable) == false)
                return;

            // Check if the selected table is locked to another staff member
            if (IsTableAvailableToOperator(entry, selectedTable) == false)
                return;

            if (!TryToLockTable(entry, selectedTable))
                return;

            // Seating guests is only possible when the table is in the "available" state
            if (selectedTable.Details.DiningTableStatus == DiningTableStatus.Available)
            {
                DiningTable tempTable = new DiningTable(entry, selectedTable);

                bool customerAdded = activeHospitalityType.PromptForCustomer && AssignCustomerToTable(entry, tempTable);
                bool guestsAdded = activeHospitalityType.RequestNoOfGuests && AddNoOfGuestsToTable(entry, tempTable, false);

                if (customerAdded || guestsAdded)
                {
                    tempTable.Details.DiningTableStatus = DiningTableStatus.GuestsSeated;
                }

                if (!activeHospitalityType.PromptForCustomer && !activeHospitalityType.RequestNoOfGuests)
                {
                    tempTable.Details.DiningTableStatus = DiningTableStatus.GuestsSeated;
                }

                if (shouldClearTableLock)
                {
                    tempTable.Save(entry, siteService, "", "");
                }

                UpdateTableStatus(entry);
            }
            else if (selectedTable.Details.DiningTableStatus == DiningTableStatus.GuestsSeated)
            {
                // toggle to an available state
                DiningTable tempTable = new DiningTable(entry, selectedTable);
                tempTable.Details.DiningTableStatus = DiningTableStatus.Available;

                if (tempTable.Transaction != null && ((RetailTransaction)tempTable.Transaction).NoOfItems == 0)
                {
                    tempTable.Transaction = null;
                }

                tempTable.Clear();

                tempTable.Save(entry, siteService, "", "");
                UpdateTableStatus(entry);
                SetSelectedTableText(entry, tempTable.Details);
            }
            else
            {
                DiningTable tempTable = new DiningTable(entry, selectedTable);
                if (AddNoOfGuestsToTable(entry, tempTable, true))
                {
                    tempTable.Save(entry, siteService, "", "");
                    UpdateTableStatus(entry);
                }
                else if (!activeHospitalityType.RequestNoOfGuests)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.SeatingGuestsNotPossibleNow, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    //Seating guests is not possible on this table at this time.
                }
            }
        }

        protected virtual bool AddNoOfGuestsToTable(IConnectionManager entry, DiningTable table, bool overrideCurrentValue)
        {
            if (!activeHospitalityType.RequestNoOfGuests)
            {
                return false;
            }

            if (!overrideCurrentValue && table.Details.NumberOfGuests > 0)
            {
                return false;
            }

            using (var inputDialog = new NumpadAmountQtyDialog())
            {
                inputDialog.HasDecimals = false;
                inputDialog.PromptText = Resources.NoOfGuests;
                inputDialog.GhostText = Resources.Guests;

                if (inputDialog.ShowDialog() == DialogResult.OK)
                {
                    int guests = (int)inputDialog.Value;
                    if (guests > activeHospitalityType.MaxGuestsPerTable)
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(Resources.MaximumNoOfGuestsExceeded.Replace("#1", Conversion.ToStr(activeHospitalityType.MaxGuestsPerTable)));
                        return false;
                    }

                    table.Details.NumberOfGuests = guests;
                    selectedTable.Details.NumberOfGuests = guests;
                    return true;
                }
            }

            return false;
        }

        protected virtual bool AssignCustomerToTable(IConnectionManager entry, DiningTable table)
        {
            if (!activeHospitalityType.PromptForCustomer)
            {
                return false;
            }

            // Seating guests is only possible when the table is the available or guests seated state
            if (table.Details.DiningTableStatus == DiningTableStatus.Available ||
                (table.Details.DiningTableStatus == DiningTableStatus.GuestsSeated && table.Details.CustomerID == RecordIdentifier.Empty && table.Transaction == null))
            {
                Customer selectedCustomer = Interfaces.Services.CustomerService(entry).Search(entry, true, table.Transaction);

                if (selectedCustomer != null && selectedCustomer.ID != RecordIdentifier.Empty)
                {
                    table.Details.CustomerID = selectedCustomer.ID;
                    table.Customer = selectedCustomer;

                    if (table.Transaction != null)
                    {
                        ((RetailTransaction)table.Transaction).Customer.ID = selectedCustomer.ID;
                    }

                    return true;
                }
            }

            return false;
        }

        protected virtual void OperationTransfer(IConnectionManager entry)
        {
            bool unlockTerminals = false;
            bool clearTableInformation = false;

            if (activeHospitalityType.TransferLinesLookupID == RecordIdentifier.Empty || activeHospitalityType.TransferLinesLookupID == "")
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.TransferNotConfigured.Replace("#1", activeHospitalityType.Text), MessageBoxButtons.OK, MessageDialogType.Attention);
                return;
            }

            // Check if any table is selected
            if (IsTableSelected(entry) == false)
                return;

            // Check if the selected table is available
            if (IsTableAvailable(entry, selectedTable) == false)
                return;

            // Check if the selected table is locked to another staff member
            if (IsTableAvailableToOperator(entry, selectedTable) == false)
                return;

            if (IsActiveTransactionOnTable(true, selectedTable) == false)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.NoItemsOnThisTableToTransfer);
                return;
            }

            if (isRunningTransferOperation == false)
            {
                isRunningTransferOperation = true;
                SetSelectedTableText(entry, null);
            }
            else
            {
                if (transferToTable == null)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.SelectTableToTransferTo, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    return;
                }

                if (transferToTable == selectedTable)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.SameFromAndToTable, MessageBoxButtons.OK,
                                                                          MessageDialogType.ErrorWarning);
                    // The same table cannot be selected as from and to table
                    isRunningTransferOperation = false;
                    return;
                }

                try
                {
                    if (IsTableAvailable(entry, transferToTable) == false)
                    {
                        selectedTable.TableButton.Select();
                        SetSelectedTableText(entry, selectedTable.Details);
                        return;
                    }

                    if (IsTableAvailableToOperator(entry, transferToTable) == false)
                    {
                        return;
                    }

                    //At this point we know that the terminals were available
                    //so the finally code can unlock them even if something goes wrong in the transfer
                    unlockTerminals = true;
                    clearTableInformation = true;

                    //Make sure that the other terminals can't do anything to the tables during the transfer operation                    
                    if (!TryToLockTable(entry, selectedTable) && !TryToLockTable(entry, transferToTable))
                        return;

                    UpdateOperatorIDForTable(entry, HospitalityOperations.Transfer);

                    RetailTransaction selectedTableTrans = (RetailTransaction)selectedTable.GetTransaction();
                    RetailTransaction transferToTableTrans = (RetailTransaction)transferToTable.GetTransaction();

                    // Call the operation
                    HospitalityResult result = TransferTable(entry, selectedTableTrans,
                                                                    transferToTableTrans,
                                                                    selectedTable.Details.TableID,
                                                                    transferToTable.Details.TableID);

                    if (result.OperationResult == HospitalityOperationResult.SaveAndExit)
                    {

                        selectedTable.Transaction = selectedTableTrans;
                        transferToTable.Transaction = transferToTableTrans;

                        if (selectedTable.Transaction != null && ((RetailTransaction)selectedTable.Transaction).SaleItems.Count == 0)
                        {
                            selectedTable.Transaction = null;
                        }

                        if (transferToTable.Transaction != null && ((RetailTransaction)transferToTable.Transaction).SaleItems.Count == 0)
                        {
                            transferToTable.Transaction = null;
                        }

                        //If all the items were moved from the original table - then also move the customer
                        if (selectedTable.Transaction == null)
                        {
                            transferToTable.Customer = selectedTable.Customer;
                            transferToTable.Details.CustomerID = selectedTable.Details.CustomerID;

                            selectedTable.ClearCustomer();
                            selectedTable.Details.Description = selectedTable.TableDescription;

                            GetCustomerDescriptionOnTable(entry, transferToTable);

                            transferToTable.Details.NumberOfGuests = selectedTable.Details.NumberOfGuests;
                            selectedTable.Details.NumberOfGuests = 0;
                        }

                        // Transfer the status and number of guests of the source table to the destination field
                        transferToTable.Details.DiningTableStatus = selectedTable.Details.DiningTableStatus;
                        transferToTable.Details.KitchenStatus = selectedTable.Details.KitchenStatus;
                        UpdateDiningTableStatus(transferToTable);
                        UpdateDiningTableStatus(selectedTable);
                    }
                }
                catch (Exception x)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.ErrorTransferingBetweenTables, MessageBoxButtons.OK,
                                                                          MessageDialogType.ErrorWarning);
                    entry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), x);
                }
                finally
                {
                    isRunningTransferOperation = false;

                    if (unlockTerminals)
                    {
                        UnlockTable(entry, selectedTable);
                        UnlockTable(entry, transferToTable);
                        UpdateTableStatus(entry);
                    }

                    if (clearTableInformation)
                    {
                        DeselectTable();
                        SetSelectedTableText(entry, null);
                    }

                    transferToTable = null;
                }
            }
        }

        protected virtual void OperationChangeStaff(IConnectionManager entry)
        {
            // Check if any table is selected
            if (IsTableSelected(entry) == false)
                return;

            // Check if the selected table is available
            if (IsTableAvailable(entry, selectedTable) == false)
                return;

            // Check if there is a transaction on the table, that we can change the staff member on.
            if (selectedTable.Transaction == null)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.NoTransactionAailableOnTable, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                //No transaction is available on this table.
                return;
            }

            //if the table is locked by a terminal i.e. being used at another terminal then nothing can be done
            if (Conversion.ToStr(selectedTable.Details.TerminalID) != "" &&
                selectedTable.Details.TerminalID != (string)entry.CurrentTerminalID)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.TableLockedByAnotherTerminal, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                //The selected table is locked by another terminal
                return;
            }

            //If the staff member already owns the transaction then we don't need this operation
            if (selectedTable.Transaction.Cashier.ID == ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).POSUser.ID)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.SelectedTableBelongsToCurrentStaff, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                //The selected table already belongs to the current staff member.
            }
            else
            {
                DialogResult result = System.Windows.Forms.DialogResult.Yes;
                HospitalitySetup hospSetup = GetCurrentHospitalitySetup(entry);
                if (hospSetup.DineInTableLocking == HospitalitySetup.SetupDineInTableLocking.ByStaff)
                {
                    result = Interfaces.Services.DialogService(entry).ShowMessage(Resources.OverrideTheStaffMemberOnThisTransaction, MessageBoxButtons.YesNo,
                                                                                   MessageDialogType.Attention);
                    //Do you want to overwrite the staff member on this transaction?               
                }
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    //Set the table with the current staff logged on
                    UpdateOperatorInformationOnTransaction(entry, ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).POSUser, selectedTable.Transaction);
                    selectedTable.Save(entry, siteService, "", "");

                    UpdateTableStatus(entry);
                }
            }
        }

        protected virtual void UpdateOperatorInformationOnTransaction(IConnectionManager entry, RecordIdentifier operatorID, IPosTransaction transaction)
        {
            if (transaction == null || operatorID == RecordIdentifier.Empty)
            {
                return;
            }

            POSUser posUser = Providers.POSUserData.Get(entry, operatorID, UsageIntentEnum.Normal,
                                              CacheType.CacheTypeTransactionLifeTime);

            UpdateOperatorInformationOnTransaction(entry, posUser, transaction);
        }

        protected virtual void UpdateOperatorInformationOnTransaction(IConnectionManager entry, POSUser posUser, IPosTransaction transaction)
        {
            transaction.Cashier.ID = posUser.ID;
            transaction.Cashier.Name = entry.Settings.NameFormatter.Format(posUser.Name);
            transaction.Cashier.NameOnReceipt = posUser.NameOnReceipt;
            transaction.Cashier.Login = posUser.Login;
        }

        protected virtual void OperationRunPOS(IConnectionManager entry)
        {
            CloseCashDrawer(entry);

            if (!IsTableSelected(entry))
                return;

            // Check if the selected table is available
            if (!IsTableAvailable(entry, selectedTable))
                return;

            // Check if the selected table is locked to another staff member
            if (!IsTableAvailableToOperator(entry, selectedTable))
                return;

            if (((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Terminal.SwitchUserWhenEnteringPOS)
            {
                ILoginPanelService service = (ILoginPanelService)entry.Service(ServiceType.LoginPanelService);
                if (!service.SwitchUser())
                {
                    return;
                }
            }

            if (!TryToLockTable(entry, selectedTable))
                return;

            checkTableIsLocked = true;
            posOperationCancelled = false;

            if (selectedTable.Transaction != null)
            {
                ((RetailTransaction)selectedTable.Transaction).SalesPerson.ID = selectedTable.Details.StaffID;
                selectedTable.Transaction.ReceiptId = "";

                if(selectedTable.Transaction.TerminalId != (string)entry.CurrentTerminalID || selectedTable.Transaction.StoreId != (string)((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.ID)
                {
                    //The selected table was opened from another POS, we need to generate a new transaction ID from the number sequence of the current DB
                    selectedTable.Transaction.TransactionId = (string)DataProviderFactory.Instance.GenerateNumber<IPosTransactionData, PosTransaction>(entry);
                }

                selectedTable.Transaction.TerminalId = (string)entry.CurrentTerminalID;
                selectedTable.Transaction.StoreId = (string)((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.ID;

                UpdateOperatorIDForTable(entry, HospitalityOperations.RunPos);

                ((RetailTransaction)selectedTable.Transaction).ReceiptIdNumberSequence = (string)((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Terminal.ReceiptIDNumberSequence;

                SalesType salesType = GetSalesType(entry, ((RetailTransaction)selectedTable.Transaction).Hospitality.ActiveHospitalitySalesType);

                if (selectedTable.Details.CustomerID != RecordIdentifier.Empty && ((RetailTransaction)selectedTable.Transaction).Customer.ID != selectedTable.Customer.ID)
                {
                    ((RetailTransaction)selectedTable.Transaction).Add(selectedTable.Customer);
                }

                if (TransactionNeedsRecalculation(entry, salesType))
                {
                    foreach (SaleLineItem sli in ((RetailTransaction)selectedTable.Transaction).SaleItems.Where(w => w.TaxGroupId != salesType.TaxGroupID))
                    {
                        sli.WasChanged = true; //this will trigger recalculation of tax on the items 
                    }
                    ((ITransactionService)entry.Service(ServiceType.TransactionService)).CalculatePriceTaxDiscount(entry, selectedTable.Transaction);
                    ((ICalculationService)entry.Service(ServiceType.CalculationService)).CalculateTotals(entry, selectedTable.Transaction, ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.Currency);
                }
                if (entry.ServiceIsLoaded(ServiceType.DualDisplayService))
                {
                    ((IDualDisplayService)entry.Service(ServiceType.DualDisplayService)).ShowTransaction(selectedTable.Transaction);
                }
            }
            else
            {
                if (selectedTable.Details.DiningTableStatus == DiningTableStatus.Available && (activeHospitalityType.PromptForCustomer || activeHospitalityType.RequestNoOfGuests))
                {
                    OperationSeatGuests(entry, false);
                }

                if (selectedTable.Details.CustomerID != RecordIdentifier.Empty && activeHospitalityType.PromptForCustomer)
                {
                    ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.UseDefaultCustomerAccount = true;
                    ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.DefaultCustomerAccount = selectedTable.Details.CustomerID;
                }
                else
                {
                    ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.UseDefaultCustomerAccount = orgUseDefaultCustomer;
                    ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.DefaultCustomerAccount = orgDefaultCustomerID;
                }
            }


            if (reloadPOSDesign)
            {
                reloadPOSDesign = false;
                LoadPosDesignEvent();
            }

            SetMainView(MainViewEnum.POS);


            RunOperationFromHospitality(entry, null, selectedTable);
        }

        protected virtual void CloseCashDrawer(IConnectionManager entry)
        {
            if (!((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.AllowSalesIfDrawerIsOpen)
            {
                if (CashDrawer.CapStatus())
                {
                    if (CashDrawer.DrawerOpen())
                    {
                        Interfaces.Services.DialogService(entry).ShowCloseDrawerMessage();
                    }
                }
            }
        }

        protected virtual bool TransactionNeedsRecalculation(IConnectionManager entry, SalesType salesType)
        {
            if (salesType == null)
            {
                return false;
            }

            //If currently the transaction is using Hospitality tax group...
            if (((RetailTransaction)selectedTable.Transaction).UseTaxGroupFrom == UseTaxGroupFromEnum.SalesType)
            {
                //..and the currently selected hospitality type has a taxgroup then check to make sure that it's the same as is on existing items on the transaction
                if (salesType.TaxGroupID != "")
                {
                    return (GetFirstItemTaxGroup(entry) != salesType.TaxGroupID);
                }
                //.. and the currently selected hospitality type does NOT have a taxgroup then reset the UseTaxGroupFrom to system settings and recalculate the transaction
                else if (salesType.TaxGroupID == "")
                {
                    //the transaction was previously opened in a hospitality type with a tax group - we need to calculate the transaction
                    //using POS tax group and set the UseTaxGroupFrom to system settings
                    ((RetailTransaction)selectedTable.Transaction).UseTaxGroupFrom = ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.UseTaxGroupFrom;
                    return true;
                }
            }
            //If use tax group is either store or customer....
            else if (((RetailTransaction)selectedTable.Transaction).UseTaxGroupFrom !=
                     UseTaxGroupFromEnum.SalesType)
            {
                //...and the current hospitality type has a tax group then set the use tax group as sales type and reclaculate
                if (salesType.TaxGroupID != "")
                {
                    ((RetailTransaction)selectedTable.Transaction).UseTaxGroupFrom = UseTaxGroupFromEnum.SalesType;
                    return true;
                }
            }

            return false;
        }

        protected virtual RecordIdentifier GetFirstItemTaxGroup(IConnectionManager entry)
        {
            if (selectedTable.Transaction == null)
            {
                return RecordIdentifier.Empty;
            }

            Store currentStore = ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store;
            Customer customer = ((RetailTransaction)selectedTable.Transaction).Customer;
            ISaleLineItem sli = ((RetailTransaction)selectedTable.Transaction).SaleItems.FirstOrDefault();

            //if using customer tax group and there is a customer on the sale
            if (currentStore.UseTaxGroupFrom == UseTaxGroupFromEnum.Customer && customer != null && !RecordIdentifier.IsEmptyOrNull(customer.ID))
            {
                return sli == null
                    ? (RecordIdentifier.IsEmptyOrNull(customer.TaxGroup) && customer.TaxExempt == TaxExemptEnum.No
                        ? currentStore.TaxGroup
                        : customer.TaxGroup)
                    : sli.SalesTaxGroupId;
            }
            else if ((currentStore.UseTaxGroupFrom == UseTaxGroupFromEnum.Customer && RecordIdentifier.IsEmptyOrNull(customer.ID))
                    || currentStore.UseTaxGroupFrom == UseTaxGroupFromEnum.Store)
            {
                return sli == null ? currentStore.TaxGroup : sli.SalesTaxGroupId;
            }

            return RecordIdentifier.Empty;
        }

        protected virtual void UpdateOperatorIDForTable(IConnectionManager entry, HospitalityOperations origin)
        {
            //Default should be the user that is logged on - is used for splitt bill transactions to make sure that the operator is the "correct" one
            //when the split bill transactions are created for print outs and to be mark the tables with waiter name
            currentWaiter = ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).POSUser.ID;

            if (selectedTable.Transaction.Cashier.ID == ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).POSUser.ID)
            {
                return;
            }

            string confirmationMsg = origin == HospitalityOperations.Transfer
                                         ? Resources.ConfirmChangingUserOnTableTransferTable
                                         : origin == HospitalityOperations.SplitBill
                                               ? Resources.ConfirmChangingUserOnTableSplitBill
                                               : Resources.ConfirmChangingUserOnTable;

            switch (activeHospitalityType.StaffTakeOverInTrans)
            {
                case HospitalityType.StaffTakeOverInTransEnum.Never:
                    currentWaiter = selectedTable.Transaction.Cashier.ID;
                    return;

                case HospitalityType.StaffTakeOverInTransEnum.WithConfirmation:
                    if (
                        Interfaces.Services.DialogService(entry)
                                .ShowMessage(confirmationMsg.Replace("#1", GetStaffDescription(entry, activeHospitalityType, (RetailTransaction)selectedTable.Transaction)).Replace("#2", GetStaffDescription(entry, activeHospitalityType, ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).POSUser)), MessageBoxButtons.YesNo, MessageDialogType.Attention) ==
                        DialogResult.Yes)
                    {
                        currentWaiter = ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).POSUser.ID;
                        UpdateOperatorInformationOnTransaction(entry, currentWaiter, selectedTable.Transaction);
                    }
                    else
                    {
                        currentWaiter = selectedTable.Transaction.Cashier.ID;
                        UpdateOperatorInformationOnTransaction(entry, currentWaiter, selectedTable.Transaction);
                    }
                    break;

                default:
                    currentWaiter = ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).POSUser.ID;
                    UpdateOperatorInformationOnTransaction(entry, currentWaiter, selectedTable.Transaction);
                    break;
            }
        }

        protected virtual void OperationPrintPreReceipt(IConnectionManager entry)
        {
            // Check if any table is selected
            if (IsTableSelected(entry) == false)
                return;

            // Check if the selected table is available
            if (IsTableAvailable(entry, selectedTable) == false)
                return;

            // Check if there is a transaction on the table
            if (IsActiveTransactionOnTable(true, selectedTable) == false)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.NoTransactionAailableOnTable, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                //No transaction is available on this table.
                return;
            }

            // Check if the selected table is locked to another staff member
            if (IsTableAvailableToOperator(entry, selectedTable) == false)
                return;

            LSOne.Services.Interfaces.Services.PrintingService(entry).PrintReceipt(entry, FormSystemType.Receipt, selectedTable.Transaction,
                                                                     true);

        }

        #region Logon Screen

        protected virtual void ShowOperatorLogOn(IConnectionManager entry)
        {
            Form form = Form.ActiveForm;
            while (form != null && form.Owner != null)
            {
                form = form.Owner;
            }
            if (form != null && form.OwnedForms != null)
            {
                foreach (Form current in form.OwnedForms)
                {
                    if (current != null)
                    {
                        current.DialogResult = DialogResult.Cancel;
                        current.Close();
                        current.Dispose();
                    }
                }
            }

            if (((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).HardwareProfile.DualDisplayConnected)
            {
                Interfaces.Services.DualDisplayService(entry).ShowTransaction(null);
            }

            LSRetailPosis.ApplicationTriggers.IApplicationTriggers.LoginWindowVisible(entry);

            try
            {
                if (((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).VisualProfile.TerminalType ==
                    VisualProfile.HardwareTypes.Touch)
                {
                    mainViewIndex = MainViewEnum.Logon;
                    //TODO this needs to be vired to the new forms somehow ?
                    if (LogOffUserEvent != null)
                    {
                        LogOffUserEvent();

                        //LogOffUserEvent -= (LogOffUserDelegate)LogOffUserEvent.GetInvocationList()[0];
                    }

                    //frmLogOnTouch logonForm = new frmLogOnTouch(false);
                    //logonForm.ShowDialog();

                    //logonStatus = logonForm.Status; //this is the only time when the property "Status" is called
                    //logonForm.Dispose();
                }
            }
            finally
            {
                mainViewIndex = MainViewEnum.Hospitality;
                ForecourtManager.ForecourtManagerClient?.EnableTopMost();
            }
        }

        protected virtual void OperationLogOff(IConnectionManager entry)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            try
            {
                //siteService.SetAddress(settings.HospitalitySiteServiceProfile);
                siteService.ClearTerminalLocks(entry, settings.HospitalitySiteServiceProfile, (string)entry.CurrentTerminalID, false);
            }
            catch
            {
                //Doesn't matter if this doesn't work - the user should be able to logoff regardless
            }

            ((Settings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).LoadPOSUser(entry, null, null, false);

            ShowOperatorLogOn(entry);

            LineDisplay.DisplayText(settings.HardwareProfile.DisplayClosedLine1, settings.HardwareProfile.DisplayClosedLine2);
        }

        protected virtual void OperationStationPrint(IConnectionManager entry)
        {
            // Check if any table is selected
            if (IsTableSelected(entry) == false)
                return;

            if ((selectedTable != null) && (selectedTable.Transaction != null))
            {
                RetailTransaction trans = (RetailTransaction)selectedTable.Transaction;
                SendToStationPrinter(entry, trans, sendAllRemainingItems: true, isPaymentOperation: false);
                selectedTable.Transaction = trans;

                UpdateDiningTableStatus(selectedTable);

                DiningTable tempTable = new DiningTable(entry, selectedTable);
                tempTable.Save(entry, siteService, "", "");
                UpdateTableStatus(entry);

            }
        }

        protected virtual void OperationUnlockTable(IConnectionManager entry)
        {
            // Check if any table is selected
            if (!IsTableSelected(entry))
			{
                return;
            }

            // Check if the selected table is available
            if (!IsTableAvailable(entry, selectedTable))
			{
                return;
            }

            // Check if there is a transaction on the table, that we can change the staff member on.
            if (selectedTable.Transaction == null)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.NoTransactionAailableOnTable, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                //No transaction is available on this table.
                return;
            }

            if (selectedTable.Transaction.Cashier.ID != ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).POSUser.ID)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.TheSelectedTableIsLockedToAnotherStaffMember, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                //The selected table already belongs to another staff member.

                return;
            }

            //if the table is locked by a terminal i.e. being used at another terminal then we want to unlock it
            if (Conversion.ToStr(selectedTable.Details.TerminalID) != "" &&
                selectedTable.Details.TerminalID != (string)entry.CurrentTerminalID)
            {
                if (entry.HasPermission(Permission.HospitalityUnlockTable))
                {
                    if (Interfaces.Services.DialogService(entry).ShowMessage(Resources.ConfirmUnlockTable, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes)
                    {
                        //Unlock the table, update status and open the POS for the table
                        UnlockTable(entry, selectedTable);
                        UpdateTableStatus(entry);
                        OperationRunPOS(entry);
                        selectedTable.SaveUnlockedTransaction(entry, siteService, selectedTable.Transaction.ID);
                    }
					else
					{
                        return;
					}
                }
				else
				{
                    ILoginPanelService loginPanelService = Services.Interfaces.Services.LoginPanelService(entry);
                    if (loginPanelService != null)
                    {
                        //Display manager permission override dialog
                        if(loginPanelService.PermissionOverrideDialog(new PermissionInfo(Permission.HospitalityUnlockTable)))
						{
                            Guid permissionContext = Guid.NewGuid();

                            if (Interfaces.Services.DialogService(entry).ShowMessage(Resources.ConfirmUnlockTable, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes)
                            {
                                entry.BeginPermissionOverride(permissionContext, new HashSet<string> { Permission.HospitalityUnlockTable });
                                UnlockTable(entry, selectedTable);
                                UpdateTableStatus(entry);
                                OperationRunPOS(entry);
                                selectedTable.SaveUnlockedTransaction(entry, siteService, selectedTable.Transaction.ID);
                                entry.EndPermissionOverride(permissionContext);
                            }
                            else
                            {
                                return;
                            }
                        }
						else
						{
                            return;
						}
                    }
                }
            }
			else
			{
                if (Interfaces.Services.DialogService(entry).ShowMessage(Resources.TableNotLocked, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes)
                {
                    OperationRunPOS(entry);
                }
                else
                {
                    return;
                }
            }
        }

        #endregion

        #endregion

        #region Load table layout

        protected virtual void LoadHospitalityLayout(IConnectionManager entry)
        {
            LoadTableLayout(entry, activeHospitalityType.DiningTableLayoutID.ToString());

            LoadOperationsMenu(entry, activeHospitalityType);

            CheckHospitalityTerminalList(entry);

            hospitalityTimer.Enabled = true;
        }

        protected virtual void LoadTableLayout(object sender, IConnectionManager entry, OperationBtnArgs e)
        {
            LoadTableLayout(entry, e.OperationButtonInfo.Parameter);
        }

        protected virtual void LoadTableLayout(IConnectionManager entry, string tableLayoutId)
        {
            try
            {
                // Start by initializing the table layout panel
                layoutContainer.DiningTableLayoutPanel.Controls.Clear();

                // Filter out the tables in the "DiningTableLists" where p.DiningTableLayoutId matches the selected tableLayoutID
                DiningTableList diningTableListForSelectedLayoutID = DiningTableLists.Find(p => p.DiningTableLayoutId == tableLayoutId);
                //Check if any data exists
                if (diningTableListForSelectedLayoutID == null)
                {
                    return;
                }

                // Set the hospitlaity type text
                layoutContainer.HospitalityTypeText = diningTableListForSelectedLayoutID.Description;
                opHospitalityTypes.SetSelectedButton(tableLayoutId);

                if (isRunningTransferOperation == false)
                {
                    // Clear selected table stuff if Transfer operation is running
                    layoutContainer.SelectedTableText = "-";
                    DeselectTable();
                }

                SetActiveHospitalityType(entry, diningTableListForSelectedLayoutID.HospitalitySalesType);

                //Check if there are any tables
                if (diningTableListForSelectedLayoutID.TableList.Count == 0)
                {
                    return;
                }

                //Suspend the layout
                layoutContainer.DiningTableLayoutPanel.SuspendLayout();

                //Clear the format for the buttons
                layoutContainer.DiningTableLayoutPanel.RowStyles.Clear();
                layoutContainer.DiningTableLayoutPanel.ColumnStyles.Clear();
                layoutContainer.DiningTableLayoutPanel.Controls.Clear();

                layoutContainer.DiningTableLayoutPanel.RowCount = diningTableListForSelectedLayoutID.RowCount;
                layoutContainer.DiningTableLayoutPanel.ColumnCount = diningTableListForSelectedLayoutID.ColumnCount;

                #region Row and Column styles

                for (int i = 0; i < layoutContainer.DiningTableLayoutPanel.RowCount; i++)
                {
                    //Create new row styles
                    layoutContainer.DiningTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent,
                                                                                         (100 /
                                                                                          diningTableListForSelectedLayoutID
                                                                                              .RowCount)));
                }

                for (int i = 0; i < layoutContainer.DiningTableLayoutPanel.ColumnCount; i++)
                {
                    //Create new column styles                
                    layoutContainer.DiningTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,
                                                                                               (100 /
                                                                                                diningTableListForSelectedLayoutID
                                                                                                    .ColumnCount)));
                }

                #endregion

                int tabIndex = 0;
                string LockedTxt = Resources.Locked;

                //Add the buttons to the panel
                foreach (DiningTable diningTable in diningTableListForSelectedLayoutID.TableList)
                {

                    MenuButton btn = new MenuButton();

                    btn.Dock = DockStyle.Fill;
                    btn.BorderColor = ColorPalette.POSControlBorderColor;
                    btn.Margin = new Padding(0);
                    btn.Location = new System.Drawing.Point(0, 0);
                    btn.Name = "btn" + diningTable.Details.TableID;
                    btn.Tag = diningTable.Details.TableID;
                    btn.Size = new System.Drawing.Size(42, 43);
                    btn.TabIndex = tabIndex;
                    btn.PushEffect = ButtonPushEffect.Darken;
                    btn.PushEffectStrength = 0;
                    btn.Text = diningTable.Details.Description;
                    btn.FocusHighlighting = false;
                    btn.DimColorWhenDisabled = false;
                    btn.DimTextWhenDisabled = false;
                    btn.Click += TableLayoutButtonClick;
                    btn.DoubleClick += TableLayoutDoubleClick;

                    int glyphFontSize = 7;
                    int tableDescriptionFontSize = 10;
                    if ((layoutContainer.HospPanel.Width > 1024) && (layoutContainer.HospPanel.Width <= 1280))
                    {
                        glyphFontSize = 10;
                        tableDescriptionFontSize = 12;
                    }

                    else if (layoutContainer.HospPanel.Width > 1280)
                    {
                        glyphFontSize = 12;
                        tableDescriptionFontSize = 14;
                    }

                    btn.Glyph1Font = new System.Drawing.Font("Segoe UI", glyphFontSize);
                    btn.Glyph2Font = new System.Drawing.Font("Segoe UI", glyphFontSize);
                    btn.Glyph3Font = new System.Drawing.Font("Segoe UI", glyphFontSize);
                    btn.Glyph4Font = new System.Drawing.Font("Segoe UI", glyphFontSize);
                    btn.Font = new System.Drawing.Font("Segoe UI", tableDescriptionFontSize, FontStyle.Bold);
                    btn.ForeColor = diningTable.Forecolor();
                    btn.Glyph1Color = btn.Glyph2Color = btn.Glyph3Color = btn.Glyph4Color = diningTable.Forecolor();

                    btn.Glyph1Text = "";
                    btn.Glyph2Text = "";
                    btn.Glyph3Text = "";
                    btn.Glyph4Text = "";

                    if (usingKds)
                    {
                        btn.Image = diningTable.Image();
                    }

                    //Get the lock status of the table
                    diningTable.Details.Locked = IsTableLocked(entry, diningTable);

                    btn.Glyph4Text = GetDiningTableStatusText(diningTable.Details);

                    if (diningTable.Details.NumberOfGuests != 0)
                        btn.Glyph2Text = diningTable.Details.NumberOfGuests.ToString();

                    btn.Glyph1Text = GetStaffDescriptionForButton(entry, diningTable, LockedTxt);
                    if (diningTable.Transaction != null)
                    {
                        btn.Glyph3Text = diningTable.Transaction.BeginDateTime.ToShortTimeString();
                    }

                    btn.GradientMode = GradientModeEnum.None;
                    btn.ButtonColor = diningTable.Color1();
                    btn.ButtonColor2 = diningTable.Color2();
                    btn.Shape = ShapeEnum.Rectangle;

                    layoutContainer.DiningTableLayoutPanel.Controls.Add(btn);
                    layoutContainer.DiningTableLayoutPanel.SetColumnSpan(btn, 1);
                    layoutContainer.DiningTableLayoutPanel.SetRowSpan(btn, 1);

                    tabIndex++;
                }

                layoutContainer.DiningTableLayoutPanel.ResumeLayout();

                UpdateTableStatus(entry);

            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), ex);
            }
        }


        #endregion

        public virtual int GetSelectedTableId(IConnectionManager entry)
        {
            return (selectedTable != null) ? selectedTable.Details.TableID : 0;
        }

        public virtual string GetSelectedTableDescription(IConnectionManager entry)
        {
            return (selectedTable != null) ? selectedTable.Details.Description : "";
        }

        public virtual int GetNoOfGuests(IConnectionManager entry)
        {
            return (selectedTable != null) ? selectedTable.Details.NumberOfGuests : 0;
        }

        public virtual bool Initialize(IConnectionManager entry)
        {
            if (((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).HospitalitySiteServiceProfile.SiteServiceAddress == "" ||
                ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).HospitalitySiteServiceProfile.SiteServicePortNumber == 0)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.NoSiteServiceProfileForHospitality);
                return false;
            }

            hospitalityTimer.Enabled = true;

            LoadHospitalityTypes(entry);

            RecallUnconcludedTransaction(entry);

            return true;
        }

        private void kdsConnectionStatusUiTimer_Tick(object sender, EventArgs e)
        {
            if ((hospitalityTimer.Interval > 1000) && ((kdsConnected != lastKdsConnectionStatus) || tableStatusUpdateFromKds))
            {
                hospitalityTimer.Interval = 1000;
                lastKdsConnectionStatus = kdsConnected;
                tableStatusUpdateFromKds = false;
            }
        }

        protected virtual void hospitalityTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                hospitalityTimer.Enabled = false;

                //If the main view is not displaying hospitality then there is no need to run the updates
                if (mainViewIndex != MainViewEnum.Hospitality)
                {
                    return;
                }

                UpdateTableStatus((IConnectionManager)hospitalityTimer.Tag);
            }
            finally
            {
                if (hospitalitySetup == null && (dataModel == null || dataModel.Connection == null)) //User logged out
                {
                    hospitalityTimer.Interval = 30000;
                }
                else
                {
                    HospitalitySetup hospSetup = GetCurrentHospitalitySetup(dataModel);
                    hospitalityTimer.Interval = hospSetup.TableUpdateTimerInterval == 0
                                                    ? 30000
                                                    : (hospSetup.TableUpdateTimerInterval * 1000);
                }

                // The default value for a timer is 30 seconds            
                hospitalityTimer.Enabled = true;
            }
        }

        #region Helpers

        protected virtual void UpdateDiningTableStatus(DiningTable updateTable)
        {
            if (updateTable.Transaction == null)
            {
                if (updateTable.Details.DiningTableStatus != DiningTableStatus.GuestsSeated &&
                    updateTable.Details.DiningTableStatus != DiningTableStatus.Unavailable
                    )
                {
                    updateTable.Details.DiningTableStatus = DiningTableStatus.Available;
                }
                return;
            }

            bool someItemsPrinted = AreAnyItemsPrinted(updateTable);
            bool allItemsPrinted = AreAllItemsPrinted(updateTable);

            if (updateTable.Details.DiningTableStatus == DiningTableStatus.Unavailable && ((RetailTransaction)updateTable.Transaction).NoOfItemLines > 0)
            {
                updateTable.Details.DiningTableStatus = DiningTableStatus.Available;
            }

            // There is a transaction for the table so we upgrade the status appropriately
            switch (updateTable.Details.DiningTableStatus)
            {
                case DiningTableStatus.Available:
                case DiningTableStatus.GuestsSeated:
                    if (((RetailTransaction)updateTable.Transaction).NoOfItemLines > 0)
                    {
                        //If all the items are voided then set OrderNotPrinted status
                        if (((RetailTransaction)updateTable.Transaction).NoOfItemLines ==
                            ((RetailTransaction)updateTable.Transaction).SaleItems.Count(c => c.Voided))
                        {
                            updateTable.Details.DiningTableStatus = usingKds ? DiningTableStatus.OrderNotSent : DiningTableStatus.OrderNotPrinted;
                        }
                        else if (allItemsPrinted)
                        {
                            updateTable.Details.DiningTableStatus = usingKds ? DiningTableStatus.OrderSent : DiningTableStatus.OrderPrinted;
                        }
                        else if (someItemsPrinted)
                        {
                            updateTable.Details.DiningTableStatus = usingKds ? DiningTableStatus.OrderPartiallySent : DiningTableStatus.OrderPartiallyPrinted;
                        }
                        else
                        {
                            updateTable.Details.DiningTableStatus = usingKds ? DiningTableStatus.OrderNotSent : DiningTableStatus.OrderNotPrinted;
                        }
                    }
                    else if (activeHospitalityType.PromptForCustomer && updateTable.Details.CustomerID != RecordIdentifier.Empty && ((RetailTransaction)updateTable.Transaction).NoOfItemLines == 0)
                    {
                        updateTable.Details.DiningTableStatus = DiningTableStatus.GuestsSeated;
                    }
                    break;

                case DiningTableStatus.OrderSent:
                case DiningTableStatus.OrderPartiallySent:
                case DiningTableStatus.OrderNotSent:
                case DiningTableStatus.OrderPrinted:
                case DiningTableStatus.OrderPartiallyPrinted:
                case DiningTableStatus.OrderNotPrinted:

                    //If all the items are voided then keep that status as it was
                    if (((RetailTransaction)updateTable.Transaction).NoOfItemLines ==
                        ((RetailTransaction)updateTable.Transaction).SaleItems.Count(c => c.Voided))
                    {
                        break;
                    }
                    else if (allItemsPrinted)
                    {
                        updateTable.Details.DiningTableStatus = usingKds ? DiningTableStatus.OrderSent : DiningTableStatus.OrderPrinted;
                    }
                    else if (someItemsPrinted)
                    {
                        updateTable.Details.DiningTableStatus = usingKds ? DiningTableStatus.OrderPartiallySent : DiningTableStatus.OrderPartiallyPrinted;
                    }
                    else
                    {
                        updateTable.Details.DiningTableStatus = usingKds ? DiningTableStatus.OrderNotSent : DiningTableStatus.OrderNotPrinted;
                    }

                    break;

                case DiningTableStatus.OrderFinished:
                case DiningTableStatus.OrderStarted:

                    //If all the items are voided then keep that status as it was
                    if (((RetailTransaction)updateTable.Transaction).NoOfItemLines ==
                        ((RetailTransaction)updateTable.Transaction).SaleItems.Count(c => c.Voided))
                    {
                        break;
                    }
                    else if (allItemsPrinted)
                    {
                        break;
                    }
                    else if (someItemsPrinted)
                    {
                        updateTable.Details.DiningTableStatus = usingKds ? DiningTableStatus.OrderPartiallySent : DiningTableStatus.OrderPartiallyPrinted;
                    }
                    else
                    {
                        updateTable.Details.DiningTableStatus = usingKds ? DiningTableStatus.OrderNotSent : DiningTableStatus.OrderNotPrinted;
                    }

                    break;
            }
        }

        protected virtual bool TryToLockTable(IConnectionManager entry, DiningTable tableToLock)
        {
            if (tableToLock == null) return false;

            TableInfo currentTableInfo = tableToLock.Save(entry, siteService, (string)entry.CurrentTerminalID, (string)entry.CurrentStaffID);

            if (currentTableInfo != null && currentTableInfo.TerminalID != (string)entry.CurrentTerminalID)
            {
                tableToLock.Load(currentTableInfo);
                UpdateTableButton(entry, tableToLock);
                ShowTableLockedMessage(entry);
                return false;
            }

            return true;
        }

        protected virtual void UnlockTable(IConnectionManager entry, DiningTable tableToUnlock)
        {
            if (tableToUnlock == null) return;

            tableToUnlock.Save(entry, siteService, "", "");
        }

        protected virtual bool IsTableSelected(IConnectionManager entry)
        {
            if (selectedTable == null)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.PleaseSelectTableBeforeRunningTheOperation, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return false;
            }

            return true;
        }

        protected virtual bool IsTableAvailable(IConnectionManager entry, DiningTable tableToCheck)
        {
            if (tableToCheck.Details.DiningTableStatus == DiningTableStatus.Unavailable)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.TheSelectedTableIsUnavalible, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return false;
            }

            return true;
        }

        protected virtual bool IsTableAvailableToOperator(IConnectionManager entry, DiningTable tableToCheck)
        {
            tableToCheck.Details.Locked = IsTableLocked(entry, tableToCheck, true);

            if (tableToCheck.Details.Locked)
            {
                ShowTableLockedMessage(entry);
                return false;
            }

            return true;
        }

        protected virtual void ShowTableLockedMessage(IConnectionManager entry)
        {
            HospitalitySetup hospSetup = GetCurrentHospitalitySetup(entry);
            IDialogService dialogService = Interfaces.Services.DialogService(entry);

            if (hospSetup.DineInTableLocking == HospitalitySetup.SetupDineInTableLocking.ByStaff)
            {
                dialogService.ShowMessage(Resources.TheSelectedTableIsLockedToAnotherStaffMember, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
            else if (hospSetup.DineInTableLocking == HospitalitySetup.SetupDineInTableLocking.ByPOS)
            {
                dialogService.ShowMessage(Resources.TableLockedByAnotherTerminal, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
        }

        protected virtual bool IsActiveTransactionOnTable(bool CountOnlyActiveItems, DiningTable tableToCheck)
        {

            if (tableToCheck.Transaction == null)
                return false;

            if (((RetailTransaction)tableToCheck.Transaction).SaleItems.Count == 0)
            {
                return false;
            }

            if (CountOnlyActiveItems)
            {
                return (((RetailTransaction)tableToCheck.Transaction).SaleItems.Count(f => f.Voided == false) > 0);
            }

            return true;
        }

        protected virtual bool IsTableLocked(IConnectionManager entry, DiningTable table, bool loadEachTable = false)
        {
            // if the table's terminal ID is "something" and it does not match the local terminal id, the table is considered to be locked
            if (Conversion.ToStr(table.Details.TerminalID) != "" &&
                table.Details.TerminalID != (string)entry.CurrentTerminalID)
                return true;

            if (loadEachTable)
            {
                //if the table terminals ID is a match then double check to make sure that another terminal hasn't locked it
                TableInfo statusNow = siteService.LoadSpecificTableState(entry, ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).HospitalitySiteServiceProfile, CreateDineInTable(table), false);

                if (statusNow != null)
                {
                    table.Load(statusNow);

                    if (statusNow.TerminalID != "" && statusNow.TerminalID != (string)entry.CurrentTerminalID)
                    {
                        table.Details.TerminalID = statusNow.TerminalID;
                        UpdateTableButton(entry, table);
                        return true;
                    }
                }
            }

            // No transaction exists, the table is not locked
            if (table.Transaction == null)
            {
                return false;
            }

            HospitalitySetup hospSetup = GetCurrentHospitalitySetup(entry);

            //If the tables should be locked by staff 
            if (hospSetup.DineInTableLocking == HospitalitySetup.SetupDineInTableLocking.ByStaff)
            {
                // and the current staff id doesn't match the staff id on the table return true
                if ((string)table.Transaction.Cashier.ID != "" && table.Transaction.Cashier.ID != ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).POSUser.ID)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool IsTableUnlocked(IConnectionManager entry)
        {
            DiningTable table = selectedTable;

            if(table != null)
			{
                if (mainViewIndex == MainViewEnum.POS)
				{
                    if (table.Transaction != null && table.ExistsUnlockedTransaction(entry, siteService, table.Transaction.ID))
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(Resources.TableUnlocked, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        SetMainView(MainViewEnum.Hospitality);
                        return true;
                    }
                }
            }

            return false;
        }

        protected virtual bool AreAnyItemsPrinted(IRetailTransaction transaction, bool includeUnprintables = false)
        {
            int count = 0;
            if (activeHospitalityType.SendVoidedItemsToStation)
            {
                count = transaction.SaleItems.Count(c => c.PrintingStatus == SalesTransaction.PrintStatus.Printed);
                count += includeUnprintables ? transaction.SaleItems.Count(c => c.PrintingStatus == SalesTransaction.PrintStatus.Unprintable) : 0;
            }
            else
            {
                count = transaction.SaleItems.Count(c => c.PrintingStatus == SalesTransaction.PrintStatus.Printed && c.Voided == false);
                count += includeUnprintables ? transaction.SaleItems.Count(c => c.PrintingStatus == SalesTransaction.PrintStatus.Unprintable && c.Voided == false) : 0;
            }

            return count > 0;
        }

        protected virtual bool AreAnyItemsPrinted(DiningTable checkTable)
        {
            return AreAnyItemsPrinted((RetailTransaction)checkTable.Transaction);
        }

        protected virtual bool AreAllItemsPrinted(DiningTable checkTable)
        {
            return AreAllItemsPrinted((RetailTransaction)checkTable.Transaction);
        }

        protected virtual bool AreAllItemsPrinted(IRetailTransaction transaction)
        {
            if (activeHospitalityType.SendVoidedItemsToStation)
            {
                return
                    transaction.SaleItems.Count(
                        c =>
                        (c.PrintingStatus == SalesTransaction.PrintStatus.Printable)) == 0;
            }
            else
            {
                if (transaction.EntryStatus == TransactionStatus.Voided)
                {
                    return true;
                }

                return
                    transaction.SaleItems.Count(
                        c =>
                        (c.PrintingStatus == SalesTransaction.PrintStatus.Printable && c.Voided == false)) == 0;

            }
        }

        #endregion

        protected virtual void DisplayHospitalityStatusText(string tableDescription, string tableStatusText)
        {
            POSFormsManager.ShowPOSStatusPanelText("");
            POSFormsManager.ShowPOSStatusBarFreeInfo(tableDescription, null, 1);
            POSFormsManager.ShowPOSStatusBarFreeInfo(tableStatusText, null, 2);
        }

        private void RunOperationFromHospitality(IConnectionManager entry, object extraInfo, DiningTable table,
                                                 POSOperations operation = POSOperations.NoOperation)
        {
            // When opening a table with an existing transaction we can get a situation where the transaction id is not
            // valid anymore
            if (table.Transaction != null && TransactionProviders.PosTransactionData.Exists(entry, table.Transaction))
            {
                table.Transaction.TransactionId = (string)DataProviderFactory.Instance.GenerateNumber<IPosTransactionData, PosTransaction>(entry);

                foreach (ISaleLineItem saleLineItem in ((RetailTransaction)table.Transaction).SaleItems)
                {
                    saleLineItem.Transaction = (RetailTransaction)table.Transaction;
                }
            }
            DisplayHospitalityStatusText(table.Details.Description, GetDiningTableStatusText(table.Details));

            if (SetTransactionEvent != null)
                SetTransactionEvent(table.Transaction);

            if (SetInputAbilityEvent != null)
                SetInputAbilityEvent(true);

            if (RunOperation != null)
            {
                if (((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).HospitalitySiteServiceProfile != ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).SiteServiceProfile)
                {
                    //siteService.SetAddress(null, 0);
                    siteService.Disconnect(entry);
                }
                OperationInfo opInfo = new OperationInfo();
                opInfo.HospitalityTableId = table.Details.TableID;
                RunOperation(operation, extraInfo, opInfo);
            }
        }

        protected virtual TableInfo CreateDineInTable(DiningTable table)
        {
            TableInfo currentTable = new TableInfo();
            currentTable.StoreID = table.Details.StoreID;
            currentTable.SalesType = table.Details.SalesType;
            currentTable.Sequence = table.Details.Sequence;
            currentTable.DiningTableLayoutID = table.Details.DiningTableLayoutID;
            currentTable.TableID = table.Details.TableID;
            return currentTable;
        }

        public virtual bool IsTableLockedByCurrentTerminal(IConnectionManager entry)
        {
            /*
             * 
             * If for some reason the Hospitality is allowing two or more terminals to get into one table then this code here below should be activated
             * It will do a separate check when the user adds the first item to the table to double-check that the terminal has the table locked
             * This requires an additional call to the site siteService which can slow down the first item sale on each table which is why this code
             * is commented out as the normal checks should be working fine in all normal circumstances
             * 
             */

            if (selectedTable != null && checkTableIsLocked && !justOneTerminal && ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.SkipHospitalityTableView == false)
            {
                //    DineInTable statusNow = iStoreServer.LoadSpecificTableState(CreateDineInTable(selectedTable));

                //    if (statusNow.TerminalID != (string)entry.CurrentTerminalID)
                //    {
                //        Services.DialogService(entry).ShowMessage(Resources.TableOpenInAnotherTerminal, MessageBoxButtons.OK, MessageDialogType.Attention);
                //        ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).ForceHospitalityExit = true;
                //        posOperationCancelled = true;
                //        checkTableIsLocked = false;
                //        return true;
                //    }
            }
            checkTableIsLocked = false;
            return false;
        }

        public virtual void UpdateSelectedTableTransaction(IConnectionManager entry, IPosTransaction posTransaction, bool useTableUpdate = false, double secondsIdle = 0)
        {
            if (posTransaction is CustomerPaymentTransaction)
            {
                return;
            }
            if (selectedTable != null && mainViewIndex != MainViewEnum.Hospitality)
            {
                //How many sales lines are on the selected table
                int noOfLinesOnSelectedTable = selectedTable.Transaction == null
                                                   ? 0
                                                   : ((RetailTransaction)selectedTable.Transaction).NumberOfLines();

                //How many sales lines are on the transaction that is to be updated
                int noOfLines = posTransaction == null
                                    ? 0
                                    : ((RetailTransaction)posTransaction).NumberOfLines();

                //If there are the same number of lines on the table and in the POS then we don't need to update anything
                //this is total number of lines - including voided lines and tender lines
                // NOTE - if we do not keep track of the lines the autologg off timer will update the table
                //        every second untill the user does something on the POS
                // NOTE - each operation in the POS will call this update f.ex. Display Total, Show journal and etc. 
                //        which is why we are keeping track of the lines to decide if we should update or not
                if (noOfLines == noOfLinesOnSelectedTable)
                {
                    return;
                }

                bool updateAfterEachOp = activeHospitalityType != null && activeHospitalityType.UpdateTableFromPOS;

                //useTableUpdate is true if called from the Autologoff timer - if DineInTableRequired is true then 
                //each user operation will update the table and there is no need to use the autologg off timer
                if (useTableUpdate && !updateAfterEachOp)
                {
                    HospitalitySetup hospSetup = GetCurrentHospitalitySetup(entry);
                    double seconds = hospSetup.TableUpdateTimerInterval == 0
                                         ? 30
                                         : (hospSetup.TableUpdateTimerInterval);

                    // The default value for a timer is 30 seconds and the number of sales lines has changed
                    if (seconds <= secondsIdle)
                    {
                        selectedTable.Transaction = posTransaction;
                        selectedTable.Save(entry, siteService, (string)entry.CurrentTerminalID, (string)entry.CurrentStaffID);
                        return;
                    }
                }

                //if this call is not coming from Autloggoff timer and the number of sales lines has changed
                if (!useTableUpdate && updateAfterEachOp)
                {
                    selectedTable.Transaction = posTransaction;
                    selectedTable.Save(entry, siteService, (string)entry.CurrentTerminalID, (string)entry.CurrentStaffID);
                }
            }
        }

        private bool CheckSiteServiceConnectionWithCancel(IConnectionManager entry)
        {
            bool cancelled = false;
            IDialogService dialogService = Interfaces.Services.DialogService(entry);
            ConnectionEnum result = ConnectionEnum.Success;
            Exception e;
            Action ssCheck = new Action(() => result = siteService.TestConnection(entry, ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).HospitalitySiteServiceProfile));

            dialogService.ShowSpinnerDialog(ssCheck, Resources.SiteService, Resources.CheckingSiteServiceConnection, out e);
            
            while(result != ConnectionEnum.Success && !cancelled)
            {
                if (Interfaces.Services.DialogService(entry).ShowMessage(Resources.CouldNotConnectToSiteService + " " + Resources.WouldYouLikeToRetry, MessageBoxButtons.RetryCancel, MessageDialogType.ErrorWarning) == DialogResult.Retry)
                {
                    dialogService.ShowSpinnerDialog(ssCheck, Resources.SiteService, Resources.CheckingSiteServiceConnection, out e);
                }
                else
                {
                    cancelled = true;
                }
            }

            return !cancelled;
        }

        public virtual void RunHospitalityPart(IConnectionManager entry, IPosTransaction posTransaction, bool forceLogoffUser, bool cancelStationPrinting, bool autoLogOff = false)
        {
            bool discardChanges = false;
            if(!CheckSiteServiceConnectionWithCancel(entry))
            {
                if(posTransaction == null || Interfaces.Services.DialogService(entry).ShowMessage(Resources.NoConnectionDiscardChanges, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes)
                {
                    discardChanges = true;
                }
                else
                {
                    return;
                }
            }

            ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).ForceHospitalityExit = false;

            if (!discardChanges && !cancelStationPrinting)
            {
                SendToStationPrinter(entry, (IRetailTransaction)posTransaction, sendAllRemainingItems: true, isPaymentOperation: false);
            }

            //If we are already in the hospitality view then we cannot update the transaction of the selected table
            //as this comes from AutoLogoff and the transaction is null and will clear out the selected table
            //HAS TO BE DONE before SetMainView is called
            bool updateTableTransaction = !discardChanges && !(selectedTable != null && (mainViewIndex == MainViewEnum.Hospitality && forceLogoffUser));

            SetMainView(MainViewEnum.Hospitality);

            try
            {
                if (SetInputAbilityEvent != null)
                    SetInputAbilityEvent(false);

                lastClick = resetLastClick;

                if (updateTableTransaction && selectedTable != null)
                {
                    //If logoff force is being used and the transaction has no items then the posTransaction is a LogonLogoffTransaction
                    //if that is added to the table then UpdateDiningTableStatus goes a little bit crazy
                    selectedTable.Transaction = posTransaction is RetailTransaction ? posTransaction : null;
                }

                // There is no table to update if no table is selected...
                if (selectedTable == null)
                {
                    if (forceLogoffUser)
                    {
                        OperationLogOff(entry);
                    }
                    return;
                }

                selectedTable.TableButton.Select();

                if(!discardChanges)
                {
                    CheckForSplitTransaction(entry, posTransaction as RetailTransaction, true, selectedTable.Transaction?.TransactionId);

                    if (selectedTable.Transaction == null)
                    {
                        /*******************
                         * 1) Table has guests seated status, table is opened, items added and paid for => table should be available with no specific information
                         * 2) Table has guests seated status, table is opened, nothing done in POS, go back into table view => table should still have status Guests seated
                         * paymentStarted is set in PrepareForPayment which is called when the POS is about to pay for a transaction
                         * => we need to use that variable to know if the transaction is null because it was just paid for or because there never was one on the table
                         *******************/
                        if ((selectedTable.Details.DiningTableStatus != DiningTableStatus.GuestsSeated) ||
                            (selectedTable.Details.DiningTableStatus == DiningTableStatus.GuestsSeated && paymentStarted))
                        {
                            selectedTable.Details.DiningTableStatus = DiningTableStatus.Available;
                            selectedTable.Clear();
                        }

                        paymentStarted = false;
                    }
                    else
                    {
                        UpdateCustomerOnTable(selectedTable);
                        UpdateDiningTableStatus(selectedTable);
                    }
                }

                if (SetTransactionEvent != null)
                {
                    SetTransactionEvent(null);
                }

                if (!posOperationCancelled) //This is only set if a table was opened simultaniously in two terminals. Can only happen if two people open a table at the exact same time
                {
                    if(!discardChanges)
                    {
                        // Removing the terminal ID of the table so that other terminals can access it
                        selectedTable.Save(entry, siteService, "", (string)entry.CurrentStaffID);
                    }

                    // Remove the seralized transaction, since it has now been saved to the Site Services's database. 
                    TransactionProviders.SerializedTransactionData.DropSerializedTransactions(entry, entry.CurrentStoreID, entry.CurrentTerminalID);
                }

                UpdateTableButton(entry, selectedTable);

                SetSelectedTableText(entry, selectedTable.Details);

                HospitalitySetup hospSetup = GetCurrentHospitalitySetup(entry);
                if ((((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Terminal.ExitAfterEachTransaction || hospSetup.AutoLogoffAtPOSExit) || forceLogoffUser)
                {
                    OperationLogOff(entry);
                }
            }
            catch (Exception x)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(x is ClientTimeNotSynchronizedException ? x.Message : Resources.OperationCannotBePerformed, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
            }

        }

        protected virtual void UpdateCustomerOnTable(DiningTable selectedTable)
        {
            if (activeHospitalityType.PromptForCustomer)
            {
                if (((RetailTransaction)selectedTable.Transaction).Customer.ID != selectedTable.Customer.ID)
                {
                    selectedTable.Customer = ((RetailTransaction)selectedTable.Transaction).Customer;
                    selectedTable.Details.CustomerID = ((RetailTransaction)selectedTable.Transaction).Customer.ID;
                }
            }
        }

        public virtual void CheckForSplitTransaction(IConnectionManager entry, IPosTransaction posTransaction, bool combineSplitTransactions, RecordIdentifier overrideTransactionID = null)
        {
            if (posTransaction == null)
            {
                return;
            }

            //Find the guest no on the transaction that was just paid
            int guest = 0;
            var sli = ((RetailTransaction)posTransaction).SaleItems.FirstOrDefault();
            guest = sli != null ? sli.CoverNo : guest; //TODO: when guest handling is added this needs to be changed

            //Create a hospitality transaction from the current transaction
            HospitalityTransaction transaction = new HospitalityTransaction();
            transaction.SetFromRetailTransaction((RetailTransaction)posTransaction);
            transaction.Guest = guest;

            RecordIdentifier transactionID = RecordIdentifier.Empty;
            if (((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.SkipHospitalityTableView)
            {
                transactionID = posTransaction.TransactionId;
            }
            else
            {
                transactionID = overrideTransactionID ?? posTransaction.TransactionId;
            }
            transaction.TransactionID = transactionID;

            //Delete the split that was added to the POS as we already have that
            TransactionProviders.HospitalityTransactionData.Delete(entry, transaction);

            //If there are split parts remaining then get them and add to the transaction
            List<HospitalityTransaction> transactions = TransactionProviders.HospitalityTransactionData.GetList(entry, RunSplitBill.CombinedHospTransID(posTransaction, transactionID, -1));

            if (combineSplitTransactions)
            {
                //If no transactions are left then no need to continue and clear out the split ID
                if (transactions == null || transactions.Count == 0)
                {
                    ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).SplitBillID = RecordIdentifier.Empty;
                    return;
                }

                //Check if there are any split payments left and add them to the current transaction
                foreach (HospitalityTransaction hosp in transactions.Where(w => w.Guest != guest))
                {
                    foreach (SaleLineItem splitLine in hosp.Transaction.SaleItems)
                    {
                        ((RetailTransaction)posTransaction).Add(splitLine);
                    }

                    foreach (var tenderLine in hosp.Transaction.TenderLines)
                    {
                        ((RetailTransaction)posTransaction).Add(tenderLine);
                    }

                    TransactionProviders.HospitalityTransactionData.Delete(entry, hosp);
                }

                ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).SplitBillID = RecordIdentifier.Empty;
                ((ICalculationService)entry.Service(ServiceType.CalculationService)).CalculateTotals(
                    entry, posTransaction, ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.Currency);
            }
            else
            {
                ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FinalizeSplitBill = (transactions != null && transactions.Count > 0);
                ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).SplitBillID = ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FinalizeSplitBill ? ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).SplitBillID : RecordIdentifier.Empty;

                HospitalityType transHospitalityType = GetHospitalityType(entry, ((RetailTransaction)posTransaction).Hospitality.ActiveHospitalitySalesType);

                if (transHospitalityType != null && transHospitalityType.StayInPosAfterTrans == false &&
                    (((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.SkipHospitalityTableView == false))
                {
                    ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).ForceHospitalityExit = !((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FinalizeSplitBill;
                }
            }
        }

        #region Split Bill

        public virtual void SplitBill(IConnectionManager entry, ref IPosTransaction posTransaction, MainViewEnum origin)
        {
            if (!((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.IsHospitality)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.OnlyAllowedInHospitalityMode, MessageBoxButtons.OK, MessageDialogType.Generic);
                return;
            }

            //If table view is being used then the table needs to be updated with the transaction
            if (((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.SkipHospitalityTableView && selectedTable == null)
            {
                selectedTable = new DiningTable(entry);
            }

            selectedTable.Transaction = posTransaction;

            posTransaction = OperationSplitBill(entry, false, origin);
        }

        protected virtual IPosTransaction OperationSplitBill(IConnectionManager entry, bool lockTable, MainViewEnum origin)
        {
            if (activeHospitalityType.SplitBillLookupID == RecordIdentifier.Empty || activeHospitalityType.SplitBillLookupID == "")
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.SplitBillNotConfigured.Replace("#1", activeHospitalityType.Text), MessageBoxButtons.OK, MessageDialogType.Attention);
                return selectedTable.Transaction;
            }

            if (lockTable)
            {
                CloseCashDrawer(entry);

                // Check if any table is selected
                if (!IsTableSelected(entry))
                    return null;

                // Check if the selected table is available
                if (!IsTableAvailable(entry, selectedTable))
                    return null;

                // Check if the selected table is locked to another staff member
                if (!IsTableAvailableToOperator(entry, selectedTable))
                    return null;

                if (!IsActiveTransactionOnTable(true, selectedTable))
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.ThereAreNoItemsToSplit);
                    return null;
                }

                if (!TryToLockTable(entry, selectedTable))
                    return null;
            }

            if (origin == MainViewEnum.Hospitality) //if the split bill is being called from POS then this has already been done
            {
                UpdateOperatorIDForTable(entry, HospitalityOperations.SplitBill);
            }

            if (!PreparationForPayment(entry, selectedTable.Transaction)) return null;

            RunSplitBill rsb = new RunSplitBill();
            HospitalityOperationResult result = rsb.Execute(entry, selectedTable, activeHospitalityType, origin == MainViewEnum.Hospitality);

            if (result != HospitalityOperationResult.Cancel)
            {
                foreach (var saleLineItem in ((RetailTransaction)selectedTable.Transaction).SaleItems)
                {
                    saleLineItem.PrintingStatus = SalesTransaction.PrintStatus.Unprintable;
                }
            }

            if (result == HospitalityOperationResult.Pay)
            {
                SetMainView(MainViewEnum.POS);
                IPosTransaction transaction = rsb.SplitBillInfo.CurrentGuest == 2 ? rsb.SplitBillInfo.SplitTransaction : rsb.SplitBillInfo.TableTransaction;
                Interfaces.Services.TransactionService(entry).LoadTransactionStatus(entry, transaction, false, true);
                UpdateOperatorInformationOnTransaction(entry, currentWaiter, (PosTransaction)transaction);
                ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).POSApp.RunOperation(POSOperations.NoOperation, null, ref transaction);
                return (PosTransaction)transaction;
            }
            else
            {
                //If staff takeover is never or should not be done then we need to update the transaction that the split bill might have created
                //before the user selected cancel
                if (result == HospitalityOperationResult.Cancel)
                {
                    UpdateOperatorInformationOnTransaction(entry, currentWaiter, selectedTable.Transaction);
                }

                if (result == HospitalityOperationResult.Cancel && origin == MainViewEnum.Hospitality)
                {
                    //if Cancel is pressed in Split Bill that comes from the table view
                    //ForceHospitalityExit has to be set to false otherwise when the next operation
                    //is run in the POS the POS will go into the table view
                    ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).ForceHospitalityExit = false;
                    UnlockTable(entry, selectedTable);
                    SetMainView(MainViewEnum.Hospitality);
                }
                return selectedTable.Transaction;
            }
        }

        #endregion

        #region UpdateTableUI


        protected virtual void UpdateTableStatus(IConnectionManager entry)
        {
            try
            {
                if (((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.IsHospitality && entry.Connection != null)
                {
                    IConnectionManagerTemporary tempConnection = entry.CreateTemporaryConnection();

                    try
                    {

                        // Get the Dining table layout for the selected hospitality type
                        RecordIdentifier tempIdentifier =
                            new RecordIdentifier(activeHospitalityType.RestaurantID,
                                                 new RecordIdentifier(activeHospitalityType.Sequence,
                                                     new RecordIdentifier(activeHospitalityType.SalesType, activeHospitalityType.DiningTableLayoutID)));

                        DiningTableLayout diningTableLayout = Providers.DiningTableLayoutData.Get(tempConnection, tempIdentifier, CacheType.CacheTypeApplicationLifeTime);

                        // Check if there is any data
                        if (diningTableLayout == null)
                        {
                            return;
                        }

                        // Get the RestaurantDiningTableData for the Dining table layout
                        List<RestaurantDiningTable> restaurantDiningTables =
                            Providers.RestaurantDiningTableData.GetList(tempConnection, diningTableLayout.RestaurantID,
                                                              diningTableLayout.Sequence, diningTableLayout.SalesType,
                                                              diningTableLayout.LayoutID, CacheType.CacheTypeApplicationLifeTime);

                        layoutContainer.DiningTableLayoutPanel.SuspendLayout();

                        DiningTableLayout dinTableLayoutDTO = (DiningTableLayout)diningTableLayout.Clone();

                        //siteService.SetAddress(((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).HospitalitySiteServiceProfile.SiteServiceAddress, (ushort)((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).HospitalitySiteServiceProfile.SiteServicePortNumber);
                        List<TableInfo> currentTableStatusList = siteService.LoadHospitalityTableState(entry, ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).HospitalitySiteServiceProfile, dinTableLayoutDTO, false);

                        DiningTableList tempTableList = DiningTableLists.Find(p => p.DiningTableLayoutId == diningTableLayout.LayoutID.ToString());

                        foreach (RestaurantDiningTable tempTable in restaurantDiningTables)
                        {
                            DiningTable diningTable = new DiningTable(entry);

                            diningTable.Details.TableID = (int)tempTable.DineInTableNo;
                            diningTable.Details.StoreID = (string)entry.CurrentStoreID;
                            diningTable.Details.SalesType = diningTableLayout.SalesType.ToString();
                            diningTable.Details.Sequence = (int)diningTableLayout.Sequence;
                            diningTable.Details.DiningTableLayoutID = diningTableLayout.LayoutID.ToString();
                            diningTable.Details.Description = tempTable.GetDescription(activeHospitalityType.TableButtonDescription);
                            diningTable.TableDescription = diningTable.Details.Description;

                            diningTable.Load(currentTableStatusList);

                            if (tempTable.Availability == RestaurantDiningTable.AvailabilityEnum.NotAvailable)
                            {
                                diningTable.Details.DiningTableStatus = DiningTableStatus.Unavailable;
                            }

                            UpdateDiningTableStatus(diningTable);
                            UpdateTableButton(entry, diningTable);

                            // find this table in DiningTableLists and update it                            
                            DiningTable thisTable = tempTableList.TableList.Find(p => p.Details.TableID == diningTable.Details.TableID);
                            thisTable.Transaction = diningTable.Transaction;

                            if (diningTable.Transaction != null)
                            {
                                ((RetailTransaction)thisTable.Transaction).Hospitality.TableInformation.TableID = diningTable.Details.TableID;
                            }

                            thisTable.Details.TerminalID = diningTable.Details.TerminalID;
                            thisTable.Details.StaffID = diningTable.Details.StaffID;
                            thisTable.Details.DiningTableStatus = diningTable.Details.DiningTableStatus;
                            thisTable.Details.KitchenStatus = diningTable.Details.KitchenStatus;
                            thisTable.Details.CustomerID = (string)diningTable.Details.CustomerID != "" ? diningTable.Details.CustomerID : RecordIdentifier.Empty;

                            if (thisTable.Details.CustomerID != RecordIdentifier.Empty && thisTable.Customer.FirstName == "")
                            {
                                thisTable.Customer = Providers.CustomerData.Get(entry, diningTable.Details.CustomerID, UsageIntentEnum.Normal, CacheType.CacheTypeApplicationLifeTime);
                                thisTable.Customer = thisTable.Customer ?? new Customer();
                            }
                            else if (thisTable.Details.CustomerID == RecordIdentifier.Empty)
                            {
                                thisTable.ClearCustomer();
                                thisTable.Details.Description = diningTable.Details.Description;
                            }

                            GetCustomerDescriptionOnTable(entry, thisTable);

                            UpdateDiningTableStatus(thisTable);
                        }

                        layoutContainer.DiningTableLayoutPanel.ResumeLayout();
                    }
                    finally
                    {
                        tempConnection.Close();
                    }

                    layoutContainer.ErrorMessageLeft = "";
                    ShowOrHideKdsConnectionErrorMessage();

                    if (selectedTable != null)
                    {
                        DisplayHospitalityStatusText(selectedTable.Details.Description,
                                                     GetDiningTableStatusText(selectedTable.Details));
                    }
                }
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Error updating the table layout: " + x.Message, x);
                layoutContainer.DiningTableLayoutPanel.ResumeLayout();
                layoutContainer.ErrorMessageLeft = Resources.TableLayoutNotUpdated;
                ShowOrHideKdsConnectionErrorMessage();
            }
        }

        private void ShowOrHideKdsConnectionErrorMessage()
        {
            if (usingKds)
            {
                layoutContainer.ErrorMessageRight = kdsConnected ? "" : Resources.UnableToConnectToKDSTryingToReconnect;
            }
        }

        protected virtual string GetCustomerDescriptionOnTable(IConnectionManager entry, DiningTable thisTable)
        {
            if (thisTable.Customer == null)
            {
                return thisTable.Details.Description;
            }

            //If asking for customer then check if the customer name should be displayed on the table
            if (activeHospitalityType.PromptForCustomer && (thisTable.Details.CustomerID != RecordIdentifier.Empty))
            {
                //It depends on when this is being called if the customer information is already on the table or not
                if (thisTable.Customer.FirstName == "")
                {
                    thisTable.Customer = Providers.CustomerData.Get(entry, thisTable.Details.CustomerID, UsageIntentEnum.Normal, CacheType.CacheTypeApplicationLifeTime);
                    thisTable.Customer = thisTable.Customer ?? new Customer();
                }

                string name = thisTable.Customer.GetFormattedName(entry.Settings.NameFormatter);
                name = name != "" ? name : (string)thisTable.Details.CustomerID;

                if (activeHospitalityType.DisplayCustomerOnTable == HospitalityType.CustomerOnTable.CustomerOnly)
                {
                    thisTable.Details.Description = name;
                }
                else if (activeHospitalityType.DisplayCustomerOnTable == HospitalityType.CustomerOnTable.CustomerPlusButtonDescription)
                {
                    thisTable.Details.Description = name + " - " + thisTable.TableDescription;
                }
            }

            return thisTable.Details.Description;

        }


        protected virtual void UpdateTableButton(IConnectionManager entry, DiningTable tableToUpdate)
        {
            // If the table to be updated is in the selected table list then we need to update the button
            if (activeHospitalityType.DiningTableLayoutID == tableToUpdate.Details.DiningTableLayoutID)
            {
                Control[] controls = layoutContainer.DiningTableLayoutPanel.Controls.Find("btn" + tableToUpdate.Details.TableID, true);

                MenuButton btn = (MenuButton)controls[0];

                //If the customer name should be visible on the button - the description is created here
                btn.Text = GetCustomerDescriptionOnTable(entry, tableToUpdate);

                btn.Glyph4Text = GetDiningTableStatusText(tableToUpdate.Details);

                btn.Glyph2Text = "";
                if (tableToUpdate.Details.NumberOfGuests != 0)
                {
                    btn.Glyph2Text = Conversion.ToStr(tableToUpdate.Details.NumberOfGuests);
                }

                if (tableToUpdate.Transaction == null)
                {
                    btn.Glyph1Text = "";
                    btn.Glyph3Text = "";
                }
                else
                {
                    btn.Glyph3Text = tableToUpdate.Transaction.BeginDateTime.ToShortTimeString();
                }

                //Get the current lock status on the table
                tableToUpdate.Details.Locked = IsTableLocked(entry, tableToUpdate);

                btn.Glyph1Text = GetStaffDescriptionForButton(entry, tableToUpdate, Resources.Locked);
                btn.ForeColor = tableToUpdate.Forecolor();
                btn.Glyph1Color = btn.Glyph2Color = btn.Glyph3Color = btn.Glyph4Color = tableToUpdate.Forecolor();
                btn.ButtonColor = tableToUpdate.Color1();
                btn.ButtonColor2 = tableToUpdate.Color2();

                if (usingKds)
                {
                    btn.Image = tableToUpdate.Image();
                }

                if (selectedTable != null && selectedTable.Details.TableID == tableToUpdate.Details.TableID)
                {
                    btn.BorderWidth = 2;
                    btn.BorderColor = ColorPalette.POSBlack;
                }
                else
                {
                    btn.BorderWidth = 1;
                    btn.BorderColor = ColorPalette.POSControlBorderColor;
                }

                btn.Enabled = tableToUpdate.Details.DiningTableStatus != DiningTableStatus.Unavailable;
            }
        }

        #endregion

        #region Table Layout

        protected virtual void TableLayoutDoubleClick(object sender, EventArgs e)
        {
            try
            {
                HospitalitySetup hospSetup = GetCurrentHospitalitySetup(dataModel);

                if (hospSetup.DineInTableSelection != HospitalitySetup.SetupDineInTableSelection.DoubleClick)
                {
                    return;
                }

                TableLayoutButtonClick(dataModel, sender, true);
            }
            catch (Exception ex)
            {
                dataModel.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), ex);
            }
        }

        protected virtual void TableLayoutButtonClick(object sender, EventArgs e)
        {
            try
            {
                TableLayoutButtonClick(dataModel, sender, false);
            }
            catch (Exception ex)
            {
                dataModel.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), ex);
            }
        }

        protected virtual void TableLayoutButtonClick(IConnectionManager entry, object Sender, bool RunPOS)
        {
            try
            {
                MenuButton btn = (MenuButton)Sender;

                DiningTableList tempTableList = DiningTableLists.Find(p => p.DiningTableLayoutId == activeHospitalityType.DiningTableLayoutID);
                DiningTable tempTable = tempTableList.TableList.Find(p => p.Details.TableID == Convert.ToInt32(btn.Tag));

                if (isRunningTransferOperation)
                {
                    transferToTable = tempTable;
                    SetSelectedTableText(entry, transferToTable.Details);
                    OperationTransfer(entry);
                    return;
                }

                HospitalitySetup hospSetup = GetCurrentHospitalitySetup(entry);

                if (tempTable == selectedTable &&
                    tempTable.Details.DiningTableStatus != DiningTableStatus.Unavailable
                    && hospSetup.DineInTableSelection == HospitalitySetup.SetupDineInTableSelection.ClickTwice)
                {
                    TimeSpan timeLapsed = DateTime.Now.Subtract(lastClick);
                    if (timeLapsed.Seconds <= 2)
                    {
                        RunPOS = true;
                    }
                }

                lastClick = DateTime.Now;

                if (tempTable.Details.DiningTableStatus == DiningTableStatus.Unavailable &&
                    selectedTable != null)
                {
                    //If the table is unavailable then keep the table that had the focus as the selected table
                    //and keep the visible focus on it still
                    selectedTable.TableButton.Select();

                }
                else if (tempTable.Details.DiningTableStatus != DiningTableStatus.Unavailable)
                {
                    tempTable.TableButton = btn;
                    SelectTable(tempTable);
                    SetSelectedTableText(entry, selectedTable.Details);

                    if (RunPOS)
                    {
                        OperationRunPOS(entry);
                    }
                }
            }
            catch (Exception x)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(x is ClientTimeNotSynchronizedException ? x.Message : Resources.TableCannotBeOpened, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                entry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), x);
            }
        }

        #endregion

        #region Finalize Split Bill

        public virtual void FinalizeSplitBill(IConnectionManager entry, IPosTransaction posTransaction, bool cancelStationPrinting)
        {
            if (!((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.IsHospitality)
            {
                return;
            }

            decimal transSalePmtDiff = 0;
            if (posTransaction.GetType() == typeof(RetailTransaction))
                transSalePmtDiff = ((RetailTransaction)posTransaction).TransSalePmtDiff;
            else if (posTransaction.GetType() == typeof(CustomerPaymentTransaction))
                transSalePmtDiff = ((CustomerPaymentTransaction)posTransaction).TransSalePmtDiff;

            bool okToFinalize;

            int noOfPaymentLines = 0;

            if (posTransaction is RetailTransaction)
            {
                noOfPaymentLines = (int)((RetailTransaction)posTransaction).NoOfPaymentLines;
            }
            else if (posTransaction is CustomerPaymentTransaction)
            {
                noOfPaymentLines = ((CustomerPaymentTransaction)posTransaction).TenderLines.Count;
            }

            okToFinalize = (noOfPaymentLines != 0 && transSalePmtDiff == 0);

            if ((transSalePmtDiff == 0 && okToFinalize) || posTransaction.EntryStatus == TransactionStatus.Voided)
            {

                //Make sure that FinalizeSplitBill and ForceHospitalityExit are always false except if the conditions in the below functions are met
                ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FinalizeSplitBill = false;
                ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).ForceHospitalityExit = false;

                //No point checking if the transaction isn't a retail transaction
                if (!(posTransaction is RetailTransaction))
                {
                    return;
                }

                if (!cancelStationPrinting)
                {
                    SendToStationPrinter(entry, (IRetailTransaction)posTransaction, sendAllRemainingItems: true, isPaymentOperation: false);
                }

                CheckForSplitTransaction(entry, posTransaction, false, selectedTable?.Transaction?.TransactionId);
            }
        }

        #endregion

        #region Active Hospitality Data

        public virtual string GetActiveHospSalesType(IConnectionManager entry)
        {
            return (activeHospitalityType != null) ? activeHospitalityType.SalesType.ToString() : "";
        }

        public virtual void SetActiveHospitalityType(IConnectionManager entry, string salesType)
        {
            RecordIdentifier prevHospitalityTypeID = new RecordIdentifier(activeHospitalityType.RestaurantID,
                                                                          activeHospitalityType.SalesType);

            HospitalityType loadedType = loadedHospitalityTypes.FirstOrDefault(f => f.SalesType == salesType);
            if (loadedType != null)
            {
                activeHospitalityType = loadedType;
            }
            else
            {
                activeHospitalityType = Providers.HospitalityTypeData.Get(entry, entry.CurrentStoreID, salesType);
                if (activeHospitalityType != null)
                {
                    //Add it to the list so that it will be found in the list next time
                    loadedHospitalityTypes.Add(activeHospitalityType);
                }
            }

            RecordIdentifier currentHospitalityTypeID = new RecordIdentifier(activeHospitalityType.RestaurantID,
                                                                             activeHospitalityType.SalesType);

            if ((string)prevHospitalityTypeID.PrimaryID != (string)currentHospitalityTypeID.PrimaryID ||
                (string)prevHospitalityTypeID.SecondaryID != (string)currentHospitalityTypeID.SecondaryID)
            {
                LoadOperationsMenu(entry, activeHospitalityType);
                reloadPOSDesign = true;
            }
        }

        protected virtual HospitalitySetup GetCurrentHospitalitySetup(IConnectionManager entry)
        {
            return hospitalitySetup ?? (hospitalitySetup = Providers.HospitalitySetupData.Get(entry));
        }

        protected virtual HospitalityType GetHospitalityType(IConnectionManager entry, RecordIdentifier salesType)
        {
            if ((string)salesType == "")
            {
                return null;
            }

            HospitalityType loadedType = loadedHospitalityTypes.FirstOrDefault(f => f.SalesType == salesType);
            if (loadedType != null)
            {
                return loadedType;
            }
            else
            {
                loadedType = Providers.HospitalityTypeData.Get(entry, entry.CurrentStoreID, salesType);
                loadedHospitalityTypes.Add(loadedType); //Add it to the list so that it will be found in the list next time
                return loadedType;
            }
        }

        protected virtual SalesType GetSalesType(IConnectionManager entry, RecordIdentifier salesType)
        {
            if ((string)salesType == "")
            {
                return null;
            }

            SalesType loadedType = loadedSalesTypes.FirstOrDefault(f => f.ID == salesType);
            if (loadedType != null)
            {
                return loadedType;
            }
            else
            {
                loadedType = Providers.SalesTypeData.Get(entry, salesType);
                loadedSalesTypes.Add(loadedType); //Add it to the list so that it will be found in the list next time
                return loadedType;
            }
        }

        public virtual UseTaxGroupFromEnum GetHospitalityTaxGroupFrom(IConnectionManager entry)
        {
            if (activeHospitalityType == null || (string)activeHospitalityType.SalesType == "")
            {
                return ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.UseTaxGroupFrom;
            }

            SalesType loadedType = GetSalesType(entry, activeHospitalityType.SalesType);
            if (loadedType != null && loadedType.TaxGroupID != "")
            {
                return UseTaxGroupFromEnum.SalesType;
            }

            return ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.UseTaxGroupFrom;
        }

        /// <summary>
        /// Called when an item has been added to the sales transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">Current retail transaction</param>
        public virtual void ItemAddedToSale(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            if (retailTransaction.SaleItems.Last.Value.UsedForPriceCheck)
            {
                return;
            }

            HospitalityType defaultHospitalityType = GetHospitalityType(entry, retailTransaction.Hospitality.ActiveHospitalitySalesType);
            retailTransaction.HasBeenSentToStation = false;

            if (usingKds)
            {
                KitchenDisplayOrder order;
                if (!kdsOrders.ContainsKey(retailTransaction.KDSOrderID))
                {
                    order = CreateKDSOrder(entry, retailTransaction);
                }
                else
                {
                    order = kdsOrders[retailTransaction.KDSOrderID];
                }

                SaleLineItem newItem = retailTransaction.SaleItems.Cast<SaleLineItem>().ToList().Last();

                var existingKdsItem = GetKdsItem(newItem.KdsId);
                if (existingKdsItem != null)
                {
                    existingKdsItem.Quantity = newItem.Quantity;
                    HandleModifiedTransaction(entry, retailTransaction);
                }
                else 
                {
                    AddItemToKdsOrder(order, newItem);
                }
            }

            if (defaultHospitalityType != null && (defaultHospitalityType.StationPrinting == HospitalityType.StationPrintingEnum.AtItemAdded || defaultHospitalityType.StationPrinting == HospitalityType.StationPrintingEnum.AtItemAddedOneDelay))
            {
                SendToStationPrinter(entry, retailTransaction, sendAllRemainingItems: false, isPaymentOperation: false);
            }
        }

        private KitchenDisplayOrder CreateKDSOrder(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            KitchenDisplayOrder order;
            Guid newKDSOrderID = Guid.NewGuid();
            order = new KitchenDisplayOrder();
            order.ID = newKDSOrderID.ToString();
            order.TableNumber = GetTableNumberOrOrderNumber(entry, retailTransaction);
            order.OrderingTime = DateTime.Now;
            order.SetDataField("EmployeeName", retailTransaction.Cashier.NameOnReceipt);
            HospitalityType currentHospitalityType = GetHospitalityType(entry, retailTransaction.Hospitality.ActiveHospitalitySalesType);
            order.HospitalityTypeId = (string)currentHospitalityType.RestaurantID + '-' + (string)currentHospitalityType.SalesType;
            order.SetDataField("HospitalityTypeText", currentHospitalityType.Text);
            order.SetDataField("CustomerName", (retailTransaction.Customer != null) ? retailTransaction.Customer.FirstName : "");
            order.PosId = $"{retailTransaction.TerminalId}-{retailTransaction.StoreId}";
            retailTransaction.KDSOrderID = newKDSOrderID;
            kdsOrders.Add(newKDSOrderID, order);
            return order;
        }

        private string GetTableNumberOrOrderNumber(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            if (((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.SkipHospitalityTableView)
            {
                return retailTransaction.TransactionId.Substring(retailTransaction.TransactionId.Length - 3, 3);
            }

            return (retailTransaction.Hospitality.TableInformation.TableID != 0) ? Convert.ToString(retailTransaction.Hospitality.TableInformation.TableID) : "";
        }

        public void ClearItemComment(
            IConnectionManager entry,
            IRetailTransaction retailTransaction,
            ISaleLineItem saleItem,
            string comment)
        {
            var kdsItem = GetKdsItem(saleItem.KdsId);
            if (kdsItem != null)
            {
                kdsItem.ItemModifiers.RemoveAll(mod => mod.ID == comment);
            }
        }

        protected virtual KitchenDisplayItem GetKdsItem(Guid kdsItemId)
        {
            if (kdsOrders != null)
            {
                foreach (var kdsOrder in kdsOrders.Values)
                {
                    foreach (var item in kdsOrder.Items)
                    {
                        if (item.KdsId == kdsItemId) return item;
                    }
                }
            }
            return null;
        }

        protected virtual void HandleModifiedTransaction(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            HospitalityType currentHospitalityType = GetHospitalityType(entry, retailTransaction.Hospitality.ActiveHospitalitySalesType);
            if (currentHospitalityType.StationPrinting == HospitalityType.StationPrintingEnum.AtItemAdded)
            {
                SendToKitchenStations(entry, retailTransaction);
            }
        }

        public virtual void ItemCommentAdded(IConnectionManager entry, IRetailTransaction retailTransaction, ISaleLineItem saleItem, string comment)
        {
            ItemCommentsAdded(entry, retailTransaction, saleItem, new List<string> { comment });
        }

        public virtual void ItemCommentsAdded(IConnectionManager entry, IRetailTransaction retailTransaction, ISaleLineItem saleItem, IEnumerable<string> comments)
        {
            var kdsItem = GetKdsItem(saleItem.KdsId);
            if (kdsItem != null)
            {
                foreach (var comment in comments)
                {
                    AddItemModifierToKdsItem(kdsItem, comment);
                    HandleModifiedTransaction(entry, retailTransaction);
                }

                if (comments.Any())
                {
                    SetPrintingStatus(entry, retailTransaction, saleItem, false);
                }
            }
        }

        protected virtual void AddItemModifierToKdsItem(KitchenDisplayItem kdsItem, string comment)
        {
            var commentItemModifier = new KitchenDisplayItemModifier();

            commentItemModifier.ID = comment;
            commentItemModifier.Text = comment;
            commentItemModifier.ModifierType = ModifierTypeEnum.Comment;
            commentItemModifier.Voided = false;

            kdsItem.ItemModifiers.Add(commentItemModifier);
        }

        protected virtual void AddItemToKdsOrder(KitchenDisplayOrder order, SaleLineItem newItem)
        {
            if (newItem.IsInfoCodeItem)
            {
                return;
            }

            if((newItem.IsAssemblyComponent && !newItem.ParentAssembly.SendComponentsToKdsAsSeparateItems) ||
                newItem.IsAssembly && newItem.ItemAssembly.SendComponentsToKdsAsSeparateItems)
			{
                return;
			}

            var kdItem = new KitchenDisplayItem();

            if (newItem.Voided)
            {
                kdItem.Voided = true;
            }

            kdItem.ID = newItem.ItemId;
            kdItem.KdsId = newItem.KdsId;
            kdItem.Text = newItem.Description;
            kdItem.ItemGroupIds.Add(newItem.RetailItemGroupId);
            kdItem.Quantity = newItem.Quantity;
            kdItem.CookingTime = newItem.ProductionTime * 60;
            kdItem.DealId = currentGestNumber.ToString(); // Since we don't have deals in LS One we store the gest number in the deal id field
            kdItem.LineId = order.Items.Count;

            newItem.InfoCodeLines.ForEach(infoCode => AddItemModifierToKdsItem(kdItem, infoCode.Information));

            if (!string.IsNullOrEmpty(newItem.VariantName))
            {
                AddItemModifierToKdsItem(kdItem, newItem.VariantName);
            }

            if (newItem.Dimension.Exists())
            {
                kdItem.Text = newItem.Description + " - " + newItem.Dimension.Description;
            }

            order.Items.Add(kdItem);
        }

        /// <summary>
        /// Called when the transaction has been voided
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The voided transaction</param>
        public virtual void TransactionVoided(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            if (usingKds)
            {
                foreach (var item in retailTransaction.SaleItems)
                {
                    ItemVoided(entry, retailTransaction, item);
                }

                HospitalityType currentHospitalityType = GetHospitalityType(entry, retailTransaction.Hospitality.ActiveHospitalitySalesType);
                SendToKitchenStations(entry, retailTransaction);

            }

            if (((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.IsHospitality)
            {
                SetPrintingStatus(entry, retailTransaction);
                SendToStationPrinter(entry, retailTransaction, sendAllRemainingItems: true, isPaymentOperation: false);
                FinalizeSplitBill(entry, retailTransaction, false);
            }
        }

        public virtual void ItemVoided(IConnectionManager entry, IRetailTransaction retailTransaction, ISaleLineItem item)
        {
            if (usingKds)
            {
                var kdsItem = GetKdsItem(((SaleLineItem)item).KdsId);
                if (kdsItem != null)
                {
                    bool voided = item.Voided;
                    kdsItem.Voided = voided;
                    kdsItem.ItemModifiers.ForEach(mod => mod.Voided = voided);

                    HandleModifiedTransaction(entry, retailTransaction);
                }
            }
        }

        public virtual void ItemQuantityChanged(IConnectionManager entry, IRetailTransaction retailTransaction, ISaleLineItem item)
        {
            if (usingKds)
            {
                var kdsItem = GetKdsItem(((SaleLineItem)item).KdsId);
                if (kdsItem != null)
                {
                    kdsItem.Quantity = item.Quantity;
                    HandleModifiedTransaction(entry, retailTransaction);
                }
            }
        }

        #endregion

        public virtual void ClearTableSelection()
        {
            DeselectTable();
        }

        private void SelectTable(DiningTable table)
        {
            DeselectTable();

            if (table != null)
            {
                selectedTable = table;

                if (selectedTable.TableButton != null)
                {
                    selectedTable.TableButton.BorderColor = ColorPalette.POSBlack;
                    selectedTable.TableButton.BorderWidth = 2;
                }
            }
        }

        private void DeselectTable()
        {
            if (selectedTable != null && selectedTable.TableButton != null)
            {
                selectedTable.TableButton.BorderColor = ColorPalette.POSControlBorderColor;
                selectedTable.TableButton.BorderWidth = 1;
            }

            selectedTable = null;
        }

        public virtual void StopHospitalityTimers()
        {
            hospitalityTimer.Enabled = false;
        }

        public virtual void ConcludeTransaction(IConnectionManager entry, IPosTransaction posTransaction)
        {
            if (((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).FunctionalityProfile.IsHospitality == false)
            {
                return;
            }

            if (selectedTable != null
                && selectedTable.Details.TerminalID != ""
                && !selectedTable.Details.Locked
                && (posTransaction is EndOfDayTransaction || posTransaction is EndOfShiftTransaction || posTransaction is LogOnOffTransaction))
            {
                selectedTable.Transaction = null;

                if(CheckSiteServiceConnectionWithCancel(entry))
                {
                    selectedTable.Save(entry, siteService, "", (string)entry.CurrentStaffID);
                }
            }

            if (usingKds && posTransaction is RetailTransaction retailTransaction && kdsOrders.ContainsKey(retailTransaction.KDSOrderID))
            {
                kdsOrders.Remove(retailTransaction.KDSOrderID);
            }
        }

        public virtual void PrintHospitalityMenuType(IConnectionManager entry, IPosTransaction retailTransaction)
        {
            if (!(retailTransaction is RetailTransaction)) return;
            HospitalityType transHospitalityType = GetHospitalityType(entry, ((RetailTransaction)retailTransaction).Hospitality.ActiveHospitalitySalesType);
            if (transHospitalityType.StationPrinting == HospitalityType.StationPrintingEnum.AlwaysPrintAll)
            {
                currentGestNumber++;
            }

            if(!CheckSiteServiceConnectionWithCancel(entry))
            {
                return;
            }

            SendToStationPrinter(entry, (IRetailTransaction)retailTransaction, sendAllRemainingItems: true, isPaymentOperation: false);
        }

        #endregion

        protected virtual DiningTable GetDiningTable(IConnectionManager entry, string kdsOrderId)
        {
            foreach (var diningTableList in DiningTableLists)
            {
                foreach (var diningTable in diningTableList.TableList)
                {
                    if (diningTable.Transaction == null) continue;

                    string expectedOrderId = (string)entry.CurrentTerminalID + "-" +
                                             diningTable.Transaction.TransactionId;
                    bool rightTable = expectedOrderId == kdsOrderId.Substring(0, expectedOrderId.Count());
                    if (rightTable)
                    {
                        return diningTable;
                    }
                }
            }
            return null;
        }

        protected virtual void KdsConnectionOnOrderUpdate(KitchenDisplayEvent e)
        {
            if (e.OrderId == "") return;
            var diningTable = GetDiningTable(dataModel, e.OrderId);
            if (diningTable == null) return;
            switch (e.OrderStatus)
            {
                case KitchenOrderStatusEnum.Started:
                    if (diningTable.Details.DiningTableStatus != DiningTableStatus.OrderStarted)
                    {
                        diningTable.Details.DiningTableStatus = DiningTableStatus.OrderStarted;
                        diningTable.Details.KitchenStatus = KitchenOrderStatusEnum.Started;
                        diningTable.Save(dataModel, siteService, "", "");
                    }

                    tableStatusUpdateFromKds = true;
                    break;
                case KitchenOrderStatusEnum.Done:
                    if (AreAllItemsPrinted(diningTable))
                    {
                        diningTable.Details.DiningTableStatus = DiningTableStatus.OrderFinished;
                    }
                    diningTable.Details.KitchenStatus = KitchenOrderStatusEnum.Done;
                    diningTable.Save(dataModel, siteService, "", "");

                    tableStatusUpdateFromKds = true;
                    break;
            }
        }

        public virtual void BumpOrder(RecordIdentifier transactionId, RecordIdentifier orderId)
        {
            if (!kdsConnected) return;

            kdsConnection.ChangeOrderStatus((string)orderId, KitchenDisplayStatus.Bumped, DateTime.Now);
        }

        public virtual void RefreshKitchenService()
        {
            if (kdsConnected)
            {
                kdsConnection.RefreshKitchenService();
            }
        }

        public virtual void PostPayment(IConnectionManager entry, IRetailTransaction transaction)
        {
            if (!usingKds || !kdsConnected || !kdsOrders.ContainsKey(transaction.KDSOrderID)) return;

            bool orderPayed = transaction.TransSalePmtDiff <= 0;

            if (orderPayed)
            {
                kdsOrders[transaction.KDSOrderID].SetDataField("POSOrderStatus", Properties.Resources.Paid);
            }

            SendToStationPrinter(entry, transaction, sendAllRemainingItems: false, isPaymentOperation: true);
        }

        public virtual void Dispose()
        {
            if (kdsConnected)
            {
                kdsConnection.Disconnect();
            }
        }

        void processStagedOrders()
		{
            if (stagedOrders.Count > 0)
            {
				lock (stagedOrdersLock)
				{
                    foreach (KitchenDisplayOrder order in stagedOrders)
                    {
                        kdsConnection.SendOrder(order);
                    }
                    stagedOrders.Clear();
				}
            }
        }

        protected virtual void KdsPeriodicallyTryToReconnect()
        {
            kdsTryToReconnectTimer = new System.Timers.Timer(10000);
            kdsTryToReconnectTimer.Elapsed += KdsTryToReconnectTimerTick;
            kdsTryToReconnectTimer.AutoReset = true;
            kdsTryToReconnectTimer.Enabled = true;
        }

        void KdsTryToReconnectTimerTick(object sender, EventArgs e)
        {
            kdsConnected = KdsTryToConnect(dataModel);
            if (kdsConnected)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("KDS connected");
#endif
                processStagedOrders();
                kdsTryToReconnectTimer.Enabled = false;
                KdsPeriodicallyCheckConnection();
            }
        }

        protected virtual bool KdsTryToConnect(IConnectionManager entry)
        {
            bool connectionSuccessful;

            try
            {
                connectionSuccessful = kdsConnection.Connect(Dns.GetHostName(), (string)entry.CurrentTerminalID, DataLayer.KDSBusinessObjects.DeviceTypeEnum.POS, (string)entry.CurrentStoreID);
            }
            catch (Exception e)
            {
                connectionSuccessful = false;
                entry.ErrorLogger.LogMessage(LogMessageType.Error, "TryToConnectToKM: Error connecting to Kitchen Service", e);
            }

            return connectionSuccessful;
        }

        protected virtual void KdsPeriodicallyCheckConnection()
        {
            kdsCheckConnectionTimer = new System.Timers.Timer(10000);
            kdsCheckConnectionTimer.Elapsed += KdsCheckConnectionTimerTick;
            kdsCheckConnectionTimer.AutoReset = true;
            kdsCheckConnectionTimer.Enabled = true;
        }

        void KdsCheckConnectionTimerTick(object sender, EventArgs e)
        {
            // We check the connection to the KM every 10 seconds. However we do not want to check the connection if we know the connection is down            
            if (kdsConnected && !kdsConnection.PingKitchenService())
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("KDS connection lost");
#endif
                kdsConnected = false;
                kdsConnection.Disconnect();
                kdsCheckConnectionTimer.Enabled = false;
                KdsPeriodicallyTryToReconnect();
            }
        }

        public void Init(IConnectionManager entry)
        {
            this.dataAreaId = entry.Connection.DataAreaId;
            dataModel = entry;

            opOperations = null;
            opHospitalityTypes = null;
            DiningTableLists = new List<DiningTableList>();
            selectedTable = null;
            lastClick = DateTime.Now;
            resetLastClick = lastClick;

            layoutContainer = new Layout.HospitalityLayoutContainer(entry);
            hospitalitySetup = null;

            hospitalityTimer = new Timer();

            hospitalityTimer.Interval = 10000;
            hospitalityTimer.Tag = entry;
            hospitalityTimer.Tick += hospitalityTimer_Tick;
            runFirstUpdate = true;

            activeHospitalityType = new HospitalityType();

            loadedHospitalityTypes = new List<HospitalityType>();
            loadedSalesTypes = new List<SalesType>();

            checkTableIsLocked = true;
            posOperationCancelled = false;
            currentWaiter = RecordIdentifier.Empty;

            orgDefaultCustomerID = ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.DefaultCustomerAccount;
            orgUseDefaultCustomer = ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.UseDefaultCustomerAccount;

            paymentStarted = false;

            usingKds = ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).HardwareProfile.UseKitchenDisplay && ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).KitchenServiceProfile.KitchenServiceAddress != "";

            siteService = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);

            if (usingKds)
            {
                kdsConnectionStatusUiTimer = new Timer();
                kdsConnectionStatusUiTimer.Interval = 1000;
                kdsConnectionStatusUiTimer.Tick += kdsConnectionStatusUiTimer_Tick;
                kdsConnectionStatusUiTimer.Enabled = true;

                kdsOrders = new Dictionary<Guid, KitchenDisplayOrder>();
                stagedOrders = new List<KitchenDisplayOrder>();
                var kitchenServiceProfile = ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).KitchenServiceProfile;

                try
                {
                    kdsConnection = new Client(kitchenServiceProfile.KitchenServiceAddress, kitchenServiceProfile.KitchenServicePort, 5);
                    kdsConnected = KdsTryToConnect(entry);
                    if (!kdsConnected)
                    {
                        KdsPeriodicallyTryToReconnect();
                    }
                    else
                    {
                        KdsPeriodicallyCheckConnection();
                    }

                    kdsConnection.OnOrderUpdate += KdsConnectionOnOrderUpdate;
                }
                catch (Exception ex)
                {
                    entry.ErrorLogger.LogMessage(LogMessageType.Error, "Error connection to the Kitchen Manager siteService", ex);
                }
            }
        }

        public void SetDataModel(IConnectionManager entry)
        {
            dataModel = entry;
        }

        /// <summary>
        /// Checks if there are any unconcluded transactions that match the current store/terminal and tries to recall it. The transaction will be sent to the corresponding table via Site Service.
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        protected virtual void RecallUnconcludedTransaction(IConnectionManager entry)
        {
            PosTransaction tempTrans = TransactionProviders.SerializedTransactionData.GetSerializedTransaction(entry, entry.CurrentStoreID, entry.CurrentTerminalID, null);

            if (tempTrans == null)
            {
                return;
            }

            if (tempTrans is IRetailTransaction && ((RetailTransaction)tempTrans).SplitTransaction)
            {
                CheckForSplitTransaction(entry, tempTrans, true);
            }

            DiningTable diningTable = new DiningTable(entry);
            diningTable.Details = tempTrans.Hospitality.TableInformation;
            diningTable.Transaction = tempTrans;

            diningTable.Save(entry, Services.Interfaces.Services.SiteServiceService(entry), (string)entry.CurrentTerminalID, (string)entry.CurrentStaffID);

            TransactionProviders.SerializedTransactionData.DropSerializedTransactions(entry, entry.CurrentStoreID, entry.CurrentTerminalID);
        }

        public HospitalityType GetActiveHospitalityType(IConnectionManager entry)
        {
            return activeHospitalityType;
        }

        public TableInfo GetDiningTableInfo(IConnectionManager entry, RecordIdentifier diningTableLayoutID, int tableID)
        {
            DiningTableList tempTableList = DiningTableLists.Find(p => p.DiningTableLayoutId == diningTableLayoutID);
            DiningTable tempTable = tempTableList.TableList.FirstOrDefault(p => p.Details.TableID == tableID);

            return tempTable != null ? tempTable.Details : null;
        }

        public TableInfo GetDiningTableInfo(IConnectionManager entry)
        {
            return GetDiningTableInfo(entry, GetActiveHospitalityType(entry).DiningTableLayoutID, GetSelectedTableId(entry));
        }

        public virtual IErrorLog ErrorLog { set; private get; }

        public string SelectMenuType(List<string> menuTypeNames, string caption)
        {
            using (var dlg = new MenuTypeSelectionDialog(menuTypeNames, caption, allowMultiSelection: false))
            {
                DialogResult dlgResult = dlg.ShowDialog();
                List<string> selectedMenuTypes = dlg.SelectedMenuTypes;

                if (dlgResult == DialogResult.OK)
                {
                    return selectedMenuTypes.FirstOrDefault();
                }
            }

            return "";
        }
    }
}
