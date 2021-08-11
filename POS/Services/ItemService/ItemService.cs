using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.SerialNumbers;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.POS.Core;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Services.WinFormsTouch;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static LSOne.DataLayer.BusinessObjects.ItemMaster.RetailItem;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.ItemMaster;

namespace LSOne.Services
{
    public partial class ItemService : IItemService
    {
        private const string Key = "IcelandPos1944";

        public ItemService()
        {


        }

        #region IItem Members

        public void Init(IConnectionManager entry)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);


#pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
            DLLEntry.Settings = settings;
#pragma warning restore 0612, 0618

            // You can get pharmacy host and pharmacy port from:
            // settings.HardwareProfile.PharmacyHost
            // settings.HardwareProfile.PharmacyPort
        }

        public virtual IErrorLog ErrorLog
        {
            set
            {

            }
        }

        /// <summary>
        /// Add all information about the item.
        /// </summary>
        /// <param name="entry">Entry to the database</param>
        /// <param name="saleLineItem"></param>
        /// <param name="transaction"></param>
        public virtual IItemSale ProcessItem(IConnectionManager entry, ISaleLineItem saleLineItem, IPosTransaction transaction)
        {
            try
            {
                if (saleLineItem.ItemId != "")
                {
                    ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                    string culture = settings.CompanyInfo.LanguageCode != settings.CultureName && settings.CompanyInfo.LanguageCode != "" ? settings.CultureName : "";
                    ItemSale sale = TransactionProviders.SaleLineItemData.GetAdditionalItemInfo(entry, saleLineItem, culture);
                    if (saleLineItem.Found)
                    {
                        if (string.IsNullOrEmpty(saleLineItem.SalesOrderUnitOfMeasureName))
                        {
                            Unit unit = Providers.UnitData.Get(entry, saleLineItem.SalesOrderUnitOfMeasure, CacheType.CacheTypeApplicationLifeTime);
                            saleLineItem.SalesOrderUnitOfMeasureName = (unit != null) ? unit.Text : "";
                        }

                        RecordIdentifier itemSalesUnitID = Providers.RetailItemData.GetSalesUnitID(entry, saleLineItem.ItemId);

                        if (itemSalesUnitID != null && itemSalesUnitID.StringValue != saleLineItem.SalesOrderUnitOfMeasure)
                        {
                            //Sales order unit of measure changed by a linked item or assembly component
                            saleLineItem.OrgUnitOfMeasure = itemSalesUnitID.StringValue;
                        }

                        saleLineItem.UnitQuantityFactor = Providers.UnitConversionData.GetUnitOfMeasureConversionFactor(entry, saleLineItem.OrgUnitOfMeasure, saleLineItem.SalesOrderUnitOfMeasure, saleLineItem.ItemId);

                        if(saleLineItem.UnitQuantityFactor != 1)
                        {
                            saleLineItem.Price = saleLineItem.Price * saleLineItem.UnitQuantityFactor;
                            saleLineItem.PriceWithTax = saleLineItem.PriceWithTax * saleLineItem.UnitQuantityFactor;
                        }

                        if(!saleLineItem.UsedForPriceCheck)
                        {
                            ManageSerialInfo(entry, settings, saleLineItem, sale, transaction);
                        }

                        // ONE-8779: if the item is marked as ScaleItem, then we are good; if it is not, check whether the sales unit is a scale unit and if it is, mark the item accordingly
                        Parameters parameters = Providers.ParameterData.Get(entry);
                        saleLineItem.ScaleItem = saleLineItem.ScaleItem || parameters.IsScaleUnit(saleLineItem.SalesOrderUnitOfMeasure);
                    }

                    return sale;
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), ex);
                throw;
            }
            return null;
        }

        public virtual bool ItemSearch(IConnectionManager entry, ref string selectedItemId, int numberOfDisplayedRows, ItemSearchViewModeEnum viewMode, RecordIdentifier retailGroupID, IPosTransaction posTransaction, OperationInfo operationInfo = null)
        {
            DialogResult dialogResult = Interfaces.Services.DialogService(entry).ItemSearch(numberOfDisplayedRows,
                ref selectedItemId, viewMode, retailGroupID, posTransaction, operationInfo);

            // Quit if cancel is pressed...
            return dialogResult != DialogResult.Cancel;
        }

        public virtual bool BarcodeSelect(IConnectionManager entry, List<BarCode> barcodes, ref BarCode selectedBarcode, string itemName)
        {
            DialogResult dialogResult = Interfaces.Services.DialogService(entry).BarcodeSelect(barcodes, ref selectedBarcode, itemName);
            return dialogResult == DialogResult.OK;
        }

        public virtual string GetScaleDisplayInformation(IConnectionManager entry, ISaleLineItem saleLineItem, int columnLength)
        {
            var rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            string qty = rounding.RoundQuantity(
                entry,
                saleLineItem.Quantity,
                saleLineItem.SalesOrderUnitOfMeasure,
                false,
                settings.Store.Currency,
                true,
                CacheType.CacheTypeTransactionLifeTime);


            string totalAmount = rounding.RoundForDisplay(entry, settings.Store.DisplayAmountsWithTax ? saleLineItem.NetAmountWithTax : saleLineItem.NetAmount, true, false, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
            totalAmount = totalAmount.Replace(" ", "");

            string retString;
            if (qty.Length + totalAmount.Length > columnLength)
            {
                retString = qty.Substring(0, columnLength - totalAmount.Length) + totalAmount;
            }
            else
                retString = qty + totalAmount.PadLeft(columnLength - (qty).Length);

            return retString;
        }

        public virtual string Validate(IConnectionManager entry, string validation)
        {
            return HMAC_SHA1.GetValue(validation, Key);
        }

        public bool UseSerialNumbers(IConnectionManager entry, IPosTransaction transaction)
        {
            try
            {
                if (transaction is RetailTransaction)
                {
                    RetailTransaction retailTransaction = transaction as RetailTransaction;
                    ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
 
                    List<SerialNumber> concludedItems = new List<SerialNumber>();
                    List<SerialNumber> clearedItems = new List<SerialNumber>();

                    retailTransaction.ISaleItems.ToList().ForEach(si =>
                    {
                        SerialNumber sn = new SerialNumber();
                        sn.ItemMasterID = si.MasterID;
                        sn.SerialNo = string.IsNullOrEmpty(si.SerialId) ? si.RFIDTagId : si.SerialId;
                        sn.ManualEntry = si.SerialIdManualInput;
                        sn.Reference = retailTransaction.ReceiptId;
                        if (!string.IsNullOrWhiteSpace(sn.SerialNo))
                        {
                            if (si.Quantity < 0)
                            {
                                //If we return a serial number and we void it, do not do anything to the serial number
                                if (!(si.Voided || retailTransaction.EntryStatus == DataLayer.BusinessObjects.Enums.TransactionStatus.Voided))
                                    clearedItems.Add(sn);
                            }
                            else
                            {
                                if (si.Voided || retailTransaction.EntryStatus == DataLayer.BusinessObjects.Enums.TransactionStatus.Voided)
                                    clearedItems.Add(sn);
                                else
                                    concludedItems.Add(sn);
                            }
                        }
                    });

                    if (settings.SiteServiceProfile.UseSerialNumbers)
                    {
                        Interfaces.Services.SiteServiceService(entry).UseSerialNumbers(entry, settings.SiteServiceProfile, concludedItems);
                        Interfaces.Services.SiteServiceService(entry).ClearReservedSerialNumbers(entry, settings.SiteServiceProfile, clearedItems);

                        entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Use serial numbers ...", "SiteServiceService.UseSerialNumbers");
                    }
                    return true;
                }
                return true;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                return false;
            }
        }

        /// <summary>
        /// Get the purchase price for an item and store
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="itemID">ID of the item for which to retrieve the cost</param>
        /// <param name="storeID">ID of the store for which to retrieve the cost. Empty ID will return an average cost of all stores</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public RetailItemCost GetRetailItemCost(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, RecordIdentifier storeID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetRetailItemCost(entry, siteServiceProfile, itemID, storeID, closeConnection);
        }

        /// <summary>
        /// Get a list purchase prices for an item, for each store including an average for all stores
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="itemID">ID of the item for which to retrieve the cost</param>
        /// <param name="filter">Search filter</param>
        /// <param name="totalCount">Total items found</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public List<RetailItemCost> GetRetailItemCostList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, RetailItemCostFilter filter, out int totalCount, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetRetailItemCostList(entry, siteServiceProfile, itemID, filter, out totalCount, closeConnection);
        }

        ///<inheritdoc cref="IItemService"/>
        public void InsertRetailItemCosts(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RetailItemCost> itemCosts, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).InsertRetailItemCosts(entry, siteServiceProfile, itemCosts, closeConnection);
        }

        #endregion

        #region Private methods

        private bool ItemHasSerialNumbers(IConnectionManager entry, ISettings settings, ISaleLineItem item, out bool couldNotConnectToSiteService)
        {
            couldNotConnectToSiteService = false;
            try
            {
                int rows = 0;
                ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                List<SerialNumber> list = service.GetActiveSerialNumbersByItem(entry, settings.SiteServiceProfile, item.MasterID, string.Empty, 1, 2, DataLayer.BusinessObjects.Enums.SerialNumberSorting.SerialNumber, true, out rows, true);
                return rows > 0;
            }
            catch (Exception e)
            {
                if (!settings.SuppressUI)
                {
                    Interfaces.Services.DialogService(entry)
                        .ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Properties.Resources.CouldNotConnectToStoreServer, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                }
                couldNotConnectToSiteService = true;
                return false;
            }
            finally
            {
                if (!settings.SuppressUI)
                {
                    Interfaces.Services.DialogService(entry).CloseStatusDialog();
                }
            }
        }

        protected virtual void ManageSerialInfo(IConnectionManager entry, ISettings settings, ISaleLineItem saleLineItem, ItemSale sale, IPosTransaction transaction)
        {
            //Decide if serial numbers should be used
            if (saleLineItem == null || sale == null || sale.KeyInSerialNumber == KeyInSerialNumberEnum.Never || !settings.SiteServiceProfile.UseSerialNumbers)
            {
                return;
            }

            try
            {
                bool allowBlank = sale.KeyInSerialNumber == KeyInSerialNumberEnum.NotMandatory;

                //If item has serial numbers assigned then show the serial  number selection list, otherwise show the keyboard input so that the use can type in the serial number
                bool couldNotConnectToSiteService = false;
                bool itemHasSerialNumbers = ItemHasSerialNumbers(entry, settings, saleLineItem, out couldNotConnectToSiteService);
                if (couldNotConnectToSiteService)
                {
                    return;
                }
                if (itemHasSerialNumbers)
                {
                    List<string> serialNumbersInSale = new List<string>();

                    if (transaction is RetailTransaction retailTrans && retailTrans.SaleIsReturnSale)
                    {
                        serialNumbersInSale.AddRange(retailTrans.SaleItems.Where(x => x.SerialId != "" && x.Quantity < 0).Select(x => x.SerialId));
                    }
                    
                    var searchDialog = new SerialIDDialog(entry, saleLineItem, serialNumbersInSale);

                    // Show the search dialog
                    if (settings.VisualProfile.TerminalType == VisualProfile.HardwareTypes.Touch)
                    {
                        searchDialog.ShowDialog();

                        // Quit if cancel is pressed...
                        if (searchDialog.DialogResult == DialogResult.Cancel && allowBlank)
                        {
                            searchDialog.Dispose();
                            return;
                        }

                        if (searchDialog.DialogResult == DialogResult.Cancel && !allowBlank && !searchDialog.ForceClose)
                        {
                            searchDialog.Dispose();
                            sale.ItemSaleCancelledReason = DataLayer.BusinessObjects.Enums.ItemSaleCancelledEnum.MustKeyInSerialNumber;
                            return;
                        }

                        if (searchDialog.SelectedSerialNumber != null && !searchDialog.ManuallyEntered)
                        {
                            if (searchDialog.SelectedSerialNumber.SerialType == DataLayer.BusinessObjects.Enums.TypeOfSerial.SerialNumber)
                            {
                                saleLineItem.SerialId = searchDialog.SelectedSerialNumber.SerialNo;
                            }
                            else
                            {
                                saleLineItem.RFIDTagId = searchDialog.SelectedSerialNumber.SerialNo;
                            }
                            saleLineItem.SerialIdManualInput = false;
                        }
                        if (searchDialog.ManuallyEntered)
                        {
                            saleLineItem.SerialId = searchDialog.ManualSerialNumber;
                            saleLineItem.SerialIdManualInput = true;
                        }
                        searchDialog.Dispose();
                    }

                }
                else
                {
                    string dialogText = string.Empty;

                    DialogResult result = Interfaces.Services.DialogService(entry).KeyboardInput(ref dialogText, Resources.EnterSerialNumber, Resources.SerialNumber, 300, InputTypeEnum.Normal);

                    if (result == DialogResult.Cancel && allowBlank)
                    {
                        return;
                    }

                    if (result == DialogResult.Cancel && !allowBlank && string.IsNullOrWhiteSpace(dialogText))
                    {
                        sale.ItemSaleCancelledReason = DataLayer.BusinessObjects.Enums.ItemSaleCancelledEnum.MustKeyInSerialNumber;
                        return;
                    }

                    saleLineItem.SerialId = dialogText;
                    saleLineItem.SerialIdManualInput = true;
                }

                if (string.IsNullOrEmpty(saleLineItem.SerialId))
                {
                    return;
                }

                ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);

                //For return transaction, don't check the serial
                //Check is this item and serial are unique in the transaction. Same item with same serial number should not exist twice.
                RetailTransaction retailTransaction = transaction is RetailTransaction ? (transaction as RetailTransaction) : null;
                if (retailTransaction != null && !retailTransaction.SaleIsReturnSale)
                {
                     if(retailTransaction.SaleItems.Any(i => i.MasterID == saleLineItem.MasterID && i.SerialId == saleLineItem.SerialId))
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(string.Format(Resources.DuplicateSerialNumber, saleLineItem.SerialId, saleLineItem.Description + " " + saleLineItem.VariantName));
                        sale.ItemSaleCancelledReason = DataLayer.BusinessObjects.Enums.ItemSaleCancelledEnum.DuplicateSerialNumber;
                        return;
                    }

                    SerialNumber sn = service.GetSerialNumber(entry, settings.SiteServiceProfile, saleLineItem.MasterID, saleLineItem.SerialId);
                    //Check if the item is not reserved or used.
                    if ((saleLineItem.SerialIdManualInput && sn != null) || (sn != null && !sn.ManualEntry && (sn.Reserved || sn.UsedDate.HasValue)))
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(string.Format(Resources.SerialNumberAlreadyUsed, sn.SerialNo, sn.ItemDescription + " " + sn.ItemVariant));
                        sale.ItemSaleCancelledReason = DataLayer.BusinessObjects.Enums.ItemSaleCancelledEnum.SerialNumberUsed;
                        return;
                    }

                    if (saleLineItem.SerialIdManualInput)
                    {
                        sn = new SerialNumber();
                        sn.ItemMasterID = saleLineItem.MasterID;
                        sn.ManualEntry = true;
                        sn.SerialNo = saleLineItem.SerialId;
                        sn.SerialType = DataLayer.BusinessObjects.Enums.TypeOfSerial.SerialNumber;
                    }

                    //Reserve serial number
                    service.ReserveSerialNumber(entry, settings.SiteServiceProfile, sn);
                }
                else
                {
                    //just verify the serial exists in the DB
                    SerialNumber sn = service.GetSerialNumber(entry, settings.SiteServiceProfile, saleLineItem.MasterID, saleLineItem.SerialId);
                    if (sn == null)
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(Resources.EnterSerialNumber);
                        sale.ItemSaleCancelledReason = DataLayer.BusinessObjects.Enums.ItemSaleCancelledEnum.SerialNumberUsed;
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), ex);
                throw;
            }
        }

        // Displays the Item Search dialog.  
        // Returns false if the user pressed cancel.  
        // Returns true if the user did choose to sell a selected item.  In this case the selecedItemId contains the item id of the item being sold.

        # endregion
    }
}
