using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Dialogs.SelectionDialog;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.DiscountItems;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    public partial class DiscountService : IDiscountService
    {
        #region Structs and enums
        public enum DiscountType
        {
            DealPrice = 0,
            DiscountPct = 1,
            DiscountAmount = 2,
            LeastExpensive = 3,
            LineSpecific = 4,
            MultiplyDealPrice = 5,
            MultiplyDiscountPct = 6,
            None = 7
        }

        protected struct MultiBuyLine
        {
            public decimal minQuantity;
            public decimal unitPriceOrDiscPct;
        }

        private struct LeastExpensiveLines
        {
            public int SaleLineId;
            public decimal PriceWithTax;
        }

        #endregion

        private DiscountAndPriceActivation discountAndPriceActivation;
        private List<PeriodicDiscount> periodicDiscountData;
        private DateTime periodicDiscountCacheReset;
        private ISettings settings;

        List<StoreInPriceGroup> priceGroupLines;

        #region Member variables
        private DataTable multiLineDiscTable;

        private DataTable activeOffers = new DataTable("ActivePeriodicOffers");
        private DataTable activeOfferLines = new DataTable("ActivePeriodicOfferLines");
        private DataTable tmpMMOffer = new DataTable("tmpMMOffer");

        private object threadLock = new object();
        private object activeOffersLock = new object();
        private object activeOfferLinesLock = new object();
        private object tmpMMOfferLock = new object();
        #endregion

        private ISettings GetSettings(IConnectionManager entry)
        {
            if (settings == null)
            {
                settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            }

            return settings;
        }

        /// <summary>
        /// Finds all discounts to be added to the transaction. Calculate Periodic Discounts and Calculate Customer Discount configurations controls 
        /// if the discounts are added to the transaction always or on payment.
        /// </summary>
        /// <param name="retailTransaction">The current transaction</param>
        /// <returns>A transaction with discount information if any are found</returns>
        public IRetailTransaction CalculateDiscount(IConnectionManager entry, IRetailTransaction retailTransaction, bool CalculateNow = false)
        {
            retailTransaction = CalcPeriodicDisc(entry, retailTransaction, CalculateNow);  //Calculation of periodic offers                            

            retailTransaction = CalcCustomerDiscount(entry, retailTransaction, CalculateNow); //Calculation of customer discount      

            return retailTransaction;
        }

        #region CustomerDiscount

        public void AddTotalDiscountAmount(IConnectionManager entry, IRetailTransaction retailTransaction, decimal amountValue)
        {
            retailTransaction.SetTotalDiscAmount(amountValue);
        }

        public bool AuthorizeTotalDiscountAmount(IConnectionManager entry, IRetailTransaction rt, decimal amountValue, decimal maxAmountValue)
        {
            ISettings settings = GetSettings(entry);

            bool suppressUI = settings.SuppressUI;

            //If the transaction has a loyalty points discount then a normal manual total discount is not permitted
            if (rt.LoyaltyItem.Relation == LoyaltyPointsRelation.Discount)
            {
                if (!suppressUI)
                {
                    Interfaces.Services.DialogService(entry)
                        .ShowMessage(Properties.Resources.TotalDiscountLoyaltyPointDiscountAlreadyOnItem, MessageBoxButtons.OK, MessageDialogType.Generic);
                }
                return false;
            }

            bool returnValue = true;
            decimal tempGrossAmount = rt.GrossAmountWithTax - rt.LineDiscountWithTax - rt.PeriodicDiscountWithTax;

            //If the POS is configured to calculate discounts from prices without tax then get the grossamount from price exluding tax
            if (rt.CalculateDiscountFrom == Store.CalculateDiscountsFromEnum.Price)
            {
                tempGrossAmount = rt.GrossAmount - rt.LineDiscount - rt.PeriodicDiscountAmount;
            }            

            if (tempGrossAmount > decimal.Zero && ((Math.Abs(amountValue) > Math.Abs(tempGrossAmount)) || Math.Abs(amountValue) > Math.Abs(maxAmountValue)))
            {
                IRoundingService rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

                decimal discountLimit = Math.Min(tempGrossAmount, maxAmountValue);
                string discountLimitRounded = rounding.RoundString(entry, discountLimit, settings.Store.Currency, true, CacheType.CacheTypeTransactionLifeTime);
                decimal maximumDiscountPct = (100 * discountLimit / tempGrossAmount);

                //The discount amount is to high. The discount limit is set to xxxx %.
                if (!suppressUI)
                {
                    string message = Properties.Resources.EnteredDiscountAmountIsTooHigh.Replace("#1",
                        string.Format("{0} ({1} %)",
                            discountLimitRounded,
                            maximumDiscountPct.ToString("n2")));
                    Interfaces.Services.DialogService(entry).ShowMessage(message, MessageBoxButtons.OK, MessageDialogType.Generic);
                }

                returnValue = false;
            }

            return returnValue;
        }

        public void AddTotalDiscountPercent(IConnectionManager entry, IRetailTransaction retailTransaction, decimal percentValue)
        {
            retailTransaction.SetTotalDiscPercent(percentValue);
        }

        public bool AuthorizeTotalDiscountPercent(IConnectionManager entry, IRetailTransaction rt, decimal percentValue, decimal maxPercentValue)
        {
            ISettings settings = GetSettings(entry);

            bool suppressUI = settings.SuppressUI;
            //If the transaction has a loyalty points discount then a normal manual total discount is not permitted
            if (rt.LoyaltyItem.Relation == LoyaltyPointsRelation.Discount)
            {
                if (!suppressUI)
                {
                    Interfaces.Services.DialogService(entry)
                        .ShowMessage(Properties.Resources.TotalDiscountLoyaltyPointDiscountAlreadyOnItem, MessageBoxButtons.OK, MessageDialogType.Generic);
                }
                return false;
            }

            bool returnValue = true;

            if ((percentValue > 100) || (Math.Abs(percentValue) > Math.Abs(maxPercentValue)))
            {
                //The discount percentage is too high. The maximum limit is set to xxx %.
                if (!suppressUI)
                {
                    string message = Properties.Resources.EnteredDiscountPercentageIsTooHigh.Replace("#1",
                        string.Format("{0} %", maxPercentValue.ToString("n2")));

                    Interfaces.Services.DialogService(entry).ShowMessage(message, MessageBoxButtons.OK, MessageDialogType.Generic);
                }
                returnValue = false;
            }
            return returnValue;
        }

        public void AddLineDiscountAmount(IConnectionManager entry, ISaleLineItem lineItem, ILineDiscountItem discountItem)
        {
            lineItem.Add(discountItem);
        }

        public bool AuthorizeLineDiscountAmount(IConnectionManager entry, ISaleLineItem lineItem, ILineDiscountItem discountItem, decimal maximumDiscountAmt)
        {
            ISettings settings = GetSettings(entry);
            bool suppressUI = settings.SuppressUI;

            //If the item has a loyalty points discount then a normal manua line discount is not permitted
            if (lineItem.LoyaltyPoints.Relation == LoyaltyPointsRelation.Discount)
            {
                if (!suppressUI)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.LineDiscountLoyaltyPointDiscountAlreadyOnItem, MessageBoxButtons.OK, MessageDialogType.Generic);
                }
                return false;
            }

            bool returnValue = true;

            decimal itemPriceWithoutDiscount = lineItem.PriceWithTax * lineItem.Quantity;

            //If the POS is configured to calculate discounts from prices without tax then get the grossamount from price exluding tax
            if (((RetailTransaction)lineItem.Transaction).CalculateDiscountFrom == Store.CalculateDiscountsFromEnum.Price)
            {
                itemPriceWithoutDiscount = lineItem.Price * lineItem.Quantity;
            }

            maximumDiscountAmt *= lineItem.Quantity;            

            if ((Math.Abs(discountItem.Amount) > Math.Abs(itemPriceWithoutDiscount)) || (Math.Abs(discountItem.Amount) > Math.Abs(maximumDiscountAmt)))
            {
                IRoundingService rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

                decimal discountLimit = Math.Min(itemPriceWithoutDiscount, maximumDiscountAmt);
                string discountLimitRounded = rounding.RoundString(entry, discountLimit, settings.Store.Currency, true, CacheType.CacheTypeTransactionLifeTime);
                decimal maximumDiscountPct = (100 * discountLimit / itemPriceWithoutDiscount);

                //The discount amount is too high. The discount limit is set to xxxx %.
                string message = Properties.Resources.EnteredDiscountAmountIsTooHigh.Replace("#1",
                    string.Format("{0} ({1} %)",
                        discountLimitRounded,
                        maximumDiscountPct.ToString("n2")));

                if (!suppressUI)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(message, MessageBoxButtons.OK, MessageDialogType.Generic);
                }

                returnValue = false;
            }

            return returnValue;
        }

        public void AddLineDiscountPercent(IConnectionManager entry, ISaleLineItem lineItem, ILineDiscountItem discountItem)
        {
            lineItem.Add(discountItem);
        }

        public bool AuthorizeLineDiscountPercent(IConnectionManager entry, ISaleLineItem lineItem, ILineDiscountItem discountItem, decimal maximumDiscountPct)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            //If the item has a loyalty points discount then a normal manual line discount is not permitted
            if (lineItem.LoyaltyPoints.Relation == LoyaltyPointsRelation.Discount)
            {
                if (!settings.SuppressUI)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.LineDiscountLoyaltyPointDiscountAlreadyOnItem, MessageBoxButtons.OK, MessageDialogType.Generic);
                }
                return false;
            }

            bool returnValue = true;
            decimal itemPriceWithoutDiscount = lineItem.PriceWithTax * lineItem.Quantity;

            //If the POS is configured to calculate discounts from prices without tax then get the grossamount from price exluding tax
            if (((RetailTransaction)lineItem.Transaction).CalculateDiscountFrom == Store.CalculateDiscountsFromEnum.Price)
            {
                itemPriceWithoutDiscount = lineItem.Price * lineItem.Quantity;
            }

            if (discountItem.Percentage > 100)
            {
                if (!settings.SuppressUI)
                {
                    string message = Properties.Resources.TheDiscountPercentageIsTooHigh.Replace("#1", maximumDiscountPct.ToString("n2") + "%"); //The discount percentage is to high. The limit is xxx %.
                    Interfaces.Services.DialogService(entry).ShowMessage(message, MessageBoxButtons.OK, MessageDialogType.Generic);
                }
                returnValue = false;
            }
            else if (Math.Abs(discountItem.Percentage) > Math.Abs(maximumDiscountPct))
            {
                if (!settings.SuppressUI)
                {
                    string message = Properties.Resources.TheDiscountPercentageIsTooHigh.Replace("#1", maximumDiscountPct.ToString("n2") + "%"); //The discount percentage is to high. The limit is xxx %.
                    Interfaces.Services.DialogService(entry).ShowMessage(message, MessageBoxButtons.OK, MessageDialogType.Generic);
                }
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Calculate the customer discount.
        /// </summary>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <returns>The retail transaction</returns>
        public IRetailTransaction CalcCustomerDiscount(IConnectionManager entry, IRetailTransaction retailTransaction, bool CalculateNow)
        {
            if ((CalculateNow || retailTransaction.CalcCustomerDiscounts == CalculateCustomerDiscountEnums.AlwaysCalculate) &&
                retailTransaction.Customer.AllowCustomerDiscounts)
            {
                //Calc line discount
                retailTransaction = CalcLineDiscount(entry, retailTransaction);

                //Calc multiline discount
                retailTransaction = CalcMultiLineDiscount(entry, retailTransaction);

                //Calc total discount
                retailTransaction = CalcTotalDiscount(entry, retailTransaction);
            }
            return retailTransaction;
        }

        private bool DoResetCache(IConnectionManager entry)
        {
            ISettings settings = GetSettings(entry);

            if (settings == null)
            {
                return true;
            }

            if (settings.DiscountCalculation.ClearPeriodicDiscountCache == ClearPeriodicDiscountCacheEnum.ClearAfter)
            {
                if (periodicDiscountCacheReset.AddMinutes(settings.DiscountCalculation.ClearPeriodicDiscountAfterMinutes) >= DateTime.Now)
                {
                    return false;
                }
            }

            periodicDiscountCacheReset = DateTime.Now;

            return true;
        }

        public void ResetDiscountService(IConnectionManager entry)
        {
            lock (threadLock)
            {
                if (DoResetCache(entry))
                {
                    discountAndPriceActivation = Providers.DiscountAndPriceActivationData.Get(entry) ?? new DiscountAndPriceActivation();                    

                    activeOfferLines.Clear();
                    activeOffers.Clear();
                    tmpMMOffer.Clear();
                    multiLineDiscTable.Clear();

                    periodicDiscountData = new List<PeriodicDiscount>();
                }
            }
        }

        private void GetLineDiscountLines(IConnectionManager entry, IRetailTransaction retailTransaction, ISaleLineItem saleItem, string currencyCode, ref decimal absQty, ref decimal discountAmount, ref decimal percent1, ref decimal percent2, ref decimal minQty)
        {
            IDiscountData discountData = DataProviderFactory.Instance.Get<IDiscountData, DataEntity>();

            int idx = 0;
            while (idx < 9)
            {
                int itemCode = idx % 3;
                int accountCode = idx / 3;

                string accountRelation =
                    accountCode == (int)PriceDiscAccountCode.Table
                        ? (string)retailTransaction.Customer.ID
                        : accountCode == (int)PriceDiscAccountCode.GroupId
                            ? (string)retailTransaction.Customer.LineDiscountID
                            : "";
                string itemRelation =
                    itemCode == (int)PriceDiscItemCode.Table
                        ? saleItem.ItemId
                        : itemCode == (int)PriceDiscItemCode.GroupId
                            ? saleItem.LineDiscountGroup
                            : "";

                if (accountRelation == null)
                {
                    accountRelation = "";
                }
                if (itemRelation == null)
                {
                    itemRelation = "";
                }

                PriceDiscType relation = PriceDiscType.LineDiscSales; //Sales line discount - 5

                if (Activation(relation, (PriceDiscAccountCode)accountCode, (PriceDiscItemCode)itemCode) || true)
                {
                    if (ValidRelation((PriceDiscAccountCode)accountCode, accountRelation) &&
                        ValidRelation((PriceDiscItemCode)itemCode, itemRelation))
                    {
                        DataTable dimensionPriceDiscTable = discountData.GetPriceDiscData(entry,
                                                                                              relation,
                                                                                              itemRelation,
                                                                                              accountRelation,
                                                                                              itemCode,
                                                                                              accountCode,
                                                                                              absQty,
                                                                                              currencyCode,
                                                                                              saleItem.MasterID,
                                                                                              saleItem.SalesOrderUnitOfMeasure,
                                                                                              CacheType.CacheTypeTransactionLifeTime);

                        foreach (DataRow row in dimensionPriceDiscTable.Rows)
                        {
                            percent1 += (decimal)row["Percent1"];
                            percent2 += (decimal)row["Percent2"];
                            discountAmount += (decimal)row["Amount"];
                            minQty += (decimal)row["QUANTITYAMOUNT"];

                            if ((byte)row["SearchAgain"] != 1)
                            {
                                idx = 9;
                            }
                        }
                    }
                }

                idx++;
            }
        }

        /// <summary>
        /// The calculation of a customer line discount.
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <returns>The retail transaction.</returns>
        private IRetailTransaction CalcLineDiscount(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            //Loop trough all items all calc line discount
            foreach (SaleLineItem saleItem in retailTransaction.ISaleItems.Where(x => !x.ReceiptReturnItem))
            {
                if (saleItem.LoyaltyPoints.Relation == LoyaltyPointsRelation.Discount || saleItem.NoDiscountAllowed)
                {
                    continue;
                }

                decimal absQty = Math.Abs(saleItem.Quantity);
                decimal discountAmount = 0;
                decimal percent1 = 0;
                decimal percent2 = 0;
                decimal minQty = 0;

                GetLineDiscountLines(entry, retailTransaction, saleItem, retailTransaction.StoreCurrencyCode, ref absQty, ref discountAmount, ref percent1, ref percent2, ref minQty);

                ISettings settings = GetSettings(entry);

                if (percent1 == 0M && percent2 == 0M && discountAmount == 0M && (settings.Store.Currency != (string)settings.CompanyInfo.CurrencyCode))
                {
                    GetLineDiscountLines(entry, retailTransaction, saleItem, (string)settings.CompanyInfo.CurrencyCode, ref absQty, ref discountAmount, ref percent1, ref percent2, ref minQty);
                    discountAmount = Interfaces.Services.CurrencyService(entry).CurrencyToCurrency(
                        entry,
                        settings.CompanyInfo.CurrencyCode,
                        settings.Store.Currency,
                        settings.CompanyInfo.CurrencyCode,
                        discountAmount);
                }

                decimal totalPercentage = (1 - (1 - (percent1 / 100)) * (1 - (percent2 / 100))) * 100;

                if (((totalPercentage != 0) || (discountAmount != 0)) && (!saleItem.DiscountsWereRemoved))
                {
                    CustomerDiscountItem discountItem = new CustomerDiscountItem();
                    discountItem.LineDiscountType = DiscountTypes.Customer;
                    discountItem.CustomerDiscountType = CustomerDiscountTypes.LineDiscount;
                    discountItem.Percentage = totalPercentage;
                    discountItem.Amount = discountAmount;

                    UpdateDiscountLines(saleItem, discountItem);
                }
            }

            return retailTransaction;
        }

        private DataRow GetMultiLineDiscountLine(IConnectionManager entry, IRetailTransaction retailTransaction, DataRow mlRow, string currencyCode)
        {
            PriceDiscType relation = PriceDiscType.MultiLineDiscSales; //Sales multiline discount - 6                
            Dimension dimension = new Dimension();

            IDiscountData discountData = DataProviderFactory.Instance.Get<IDiscountData, DataEntity>();

            int idx = 0;
            while (idx < 9)
            {
                int itemCode = idx % 3; //Mod divsion
                int accountCode = idx / 3;

                string itemRelation = (itemCode == (int)PriceDiscItemCode.GroupId) ? (string)mlRow["MultiLineGroup"] : "";
                string accountRelation = (accountCode == (int)PriceDiscAccountCode.Table) ? (string)retailTransaction.Customer.ID : (accountCode == (int)PriceDiscAccountCode.GroupId) ? (string)retailTransaction.Customer.MultiLineDiscountID : "";

                if (Activation(relation, (PriceDiscAccountCode)accountCode, (PriceDiscItemCode)itemCode))
                {
                    if (ValidRelation((PriceDiscAccountCode)accountCode, accountRelation) &&
                        ValidRelation((PriceDiscItemCode)itemCode, itemRelation))
                    {
                        DataTable priceDiscTable = discountData.GetPriceDiscData(entry, relation, itemRelation, accountRelation, itemCode, accountCode, (decimal)mlRow["Quantity"], currencyCode, RecordIdentifier.Empty, RecordIdentifier.Empty, CacheType.CacheTypeTransactionLifeTime);

                        foreach (DataRow row in priceDiscTable.Rows)
                        {
                            mlRow["Percent1"] = (decimal)mlRow["Percent1"] + (decimal)row["Percent1"];
                            mlRow["Percent2"] = (decimal)mlRow["Percent2"] + (decimal)row["Percent2"];
                            mlRow["Amount"] = (decimal)mlRow["Amount"] + (decimal)row["Amount"];
                            mlRow["MinQty"] = (decimal)mlRow["MinQty"] + (decimal)row["QuantityAmount"];

                            if ((byte)row["SearchAgain"] != 1)
                            {
                                idx = 9;
                            }
                        }
                    }
                }
                idx++;
            }

            return mlRow;
        }

        /// <summary>
        /// The calculation of a customer multiline discount.
        /// </summary>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <returns>The retail transaction.</returns>
        private IRetailTransaction CalcMultiLineDiscount(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            lock (threadLock)
            {
                multiLineDiscTable.Clear();
            }

            //Sum up all the linegroup discount lines in the same group
            foreach (SaleLineItem saleItem in retailTransaction.ISaleItems.Where(x => !x.ReceiptReturnItem))
            {
                if (saleItem.LoyaltyPoints.Relation == LoyaltyPointsRelation.Discount || saleItem.NoDiscountAllowed)
                {
                    continue;
                }
                if (((saleItem.MultiLineDiscountGroup != null) && (saleItem.MultiLineDiscountGroup != "")) && (!saleItem.DiscountsWereRemoved) && !saleItem.Voided)
                {
                    UpdateMultiLineDiscTable(saleItem.MultiLineDiscountGroup, saleItem.Quantity);
                }
            }

            ISettings settings = GetSettings(entry);

            //Find discounts for the different multiline discount groups
            #region Find discounts

            lock (threadLock)
            {
                foreach (DataRow nextRow in multiLineDiscTable.Rows)
                {
                    DataRow mlRow = GetMultiLineDiscountLine(entry, retailTransaction, nextRow, retailTransaction.StoreCurrencyCode);

                    if ((decimal)mlRow["Percent1"] == 0M && (decimal)mlRow["Percent2"] == 0M && (decimal)mlRow["Amount"] == 0M &&
                        (settings.Store.Currency != (string)settings.CompanyInfo.CurrencyCode))
                    {
                        mlRow = GetMultiLineDiscountLine(entry, retailTransaction, nextRow, (string)settings.CompanyInfo.CurrencyCode);
                        mlRow["Amount"] = Services.Interfaces.Services.CurrencyService(entry).CurrencyToCurrency(
                            entry,
                            settings.CompanyInfo.CurrencyCode,
                            settings.Store.Currency,
                            settings.CompanyInfo.CurrencyCode,
                            Conversion.ToDecimal(mlRow["Amount"]));
                    }

                    //Update the sale items.
                    foreach (SaleLineItem saleItem in retailTransaction.ISaleItems)
                    {
                        if (!saleItem.NoDiscountAllowed 
                            &&
                            (saleItem.MultiLineDiscountGroup == (string)mlRow["MultiLineGroup"] &&
                            ((decimal)mlRow["Percent1"] > 0M || (decimal)mlRow["Percent2"] > 0M || (decimal)mlRow["Amount"] > 0M)))
                        {
                            CustomerDiscountItem discountItem = new CustomerDiscountItem();
                            discountItem.LineDiscountType = DiscountTypes.Customer;
                            discountItem.CustomerDiscountType = CustomerDiscountTypes.MultiLineDiscount;
                            discountItem.Percentage = (1 - (1 - ((decimal)mlRow["Percent1"] / 100)) * (1 - ((decimal)mlRow["Percent2"] / 100))) * 100;
                            discountItem.Amount = (decimal)mlRow["Amount"];

                            UpdateDiscountLines(saleItem, discountItem);
                        }
                    }
                }
            }

            #endregion

            return retailTransaction;
        }

        private bool DiscountLineFound(SaleLineItem saleItem)
        {
            foreach (DiscountItem discLine in saleItem.DiscountLines)
            {
                if (discLine is LineDiscountItem || discLine is TotalDiscountItem)
                    return true;
            }

            return false;
        }
        /// <summary>
        /// The calculation of the total customer discount.
        /// </summary>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <returns>The retail transaction.</returns>
        private IRetailTransaction CalcTotalDiscount(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            decimal totalAmount = 0;

            //Find the total amount as a basis for the total discount
            foreach (SaleLineItem saleItem in retailTransaction.ISaleItems.Where(x => !x.ReceiptReturnItem))
            {
                if (saleItem.LoyaltyPoints.Relation == LoyaltyPointsRelation.Discount || saleItem.NoDiscountAllowed)
                {
                    continue;
                }
                if (saleItem.IncludedInTotalDiscount && !saleItem.DiscountsWereRemoved && !saleItem.Voided
                    && (!BanCompoundDiscounts || !DiscountLineFound(saleItem) || saleItem.PriceOverridden))
                {
                    if (saleItem.TaxIncludedInItemPrice || retailTransaction.CalculateDiscountFrom == Store.CalculateDiscountsFromEnum.PriceWithTax)
                    {
                        totalAmount += saleItem.PriceWithTax * saleItem.Quantity;

                    }
                    else
                    {
                        totalAmount += saleItem.Price * saleItem.Quantity;
                    }
                }
            }

            decimal absTotalAmount = Math.Abs(totalAmount);

            //Find the total discounts.
            PriceDiscType relation = PriceDiscType.EndDiscSales; //Total sales discount - 7
            int itemCode = (int)PriceDiscItemCode.All; //All items - 2
            int accountCode = 0;
            string itemRelation = "";
            decimal percent1 = 0;
            decimal percent2 = 0;
            decimal discountAmount = 0;
            Dimension dimension = new Dimension();

            IDiscountData discountData = DataProviderFactory.Instance.Get<IDiscountData, DataEntity>();

            int idx = 0;
            while (idx < 3)
            {
                accountCode = idx;

                string accountRelation = (accountCode == (int)PriceDiscAccountCode.Table) ? (string)retailTransaction.Customer.ID : (accountCode == (int)PriceDiscAccountCode.GroupId) ? (string)retailTransaction.Customer.FinalDiscountID : "";

                if (Activation(relation, (PriceDiscAccountCode)accountCode, PriceDiscItemCode.All))
                {
                    DataTable priceDiscTable = discountData.GetPriceDiscData(entry, relation, itemRelation, accountRelation, itemCode, accountCode, absTotalAmount, retailTransaction.StoreCurrencyCode, RecordIdentifier.Empty, RecordIdentifier.Empty, CacheType.CacheTypeTransactionLifeTime);

                    foreach (DataRow row in priceDiscTable.Rows)
                    {
                        percent1 += (decimal)row["Percent1"];
                        percent2 += (decimal)row["Percent2"];
                        discountAmount += (decimal)row["Amount"];

                        if ((byte)row["SearchAgain"] != 1)
                        {
                            idx = 3;
                        }
                    }
                }

                idx++;
            }

            ISettings settings = GetSettings(entry);

            if (percent1 == 0M && percent2 == 0M && discountAmount == 0M && (string)settings.CompanyInfo.CurrencyCode != settings.Store.Currency)
            {
                idx = 0;
                while (idx < 3)
                {
                    accountCode = idx;

                    string accountRelation = (accountCode == (int)PriceDiscAccountCode.Table) ? (string)retailTransaction.Customer.ID : (accountCode == (int)PriceDiscAccountCode.GroupId) ? (string)retailTransaction.Customer.FinalDiscountID : "";

                    if (Activation(relation, (PriceDiscAccountCode)accountCode, PriceDiscItemCode.All))
                    {
                        DataTable priceDiscTable = discountData.GetPriceDiscData(entry, relation,
                                                                                 itemRelation, accountRelation, itemCode,
                                                                                 accountCode, absTotalAmount,
                                                                                 (string)
                                                                                 settings.CompanyInfo.CurrencyCode,
                                                                                 RecordIdentifier.Empty,
                                                                                 RecordIdentifier.Empty,
                                                                                 CacheType.CacheTypeTransactionLifeTime);

                        foreach (DataRow row in priceDiscTable.Rows)
                        {
                            percent1 += (decimal)row["Percent1"];
                            percent2 += (decimal)row["Percent2"];
                            discountAmount += (decimal)row["Amount"];

                            if ((byte)row["SearchAgain"] != 1)
                            {
                                idx = 3;
                            }
                        }
                    }

                    idx++;
                }
                discountAmount = Interfaces.Services.CurrencyService(entry).CurrencyToCurrency(
                    entry,
                    settings.CompanyInfo.CurrencyCode,
                    settings.Store.Currency,
                    settings.CompanyInfo.CurrencyCode,
                    discountAmount);
            }

            decimal totalPercentage = (1 - (1 - (percent1 / 100)) * (1 - (percent2 / 100))) * 100;
            decimal totalAmountDiscountPercentage = totalAmount == 0 ? 0 : discountAmount / totalAmount;
            if (totalPercentage != 0 || discountAmount != 0)
            {
                //Update the sale items.
                foreach (SaleLineItem saleItem in retailTransaction.ISaleItems)
                {
                    if (saleItem.IncludedInTotalDiscount 
                        && !saleItem.DiscountsWereRemoved 
                        && !saleItem.Voided 
                        && saleItem.LoyaltyPoints.Relation != LoyaltyPointsRelation.Discount
                        && (!BanCompoundDiscounts || !DiscountLineFound(saleItem) || saleItem.PriceOverridden)
                        && !saleItem.NoDiscountAllowed)
                    {
                        CustomerDiscountItem discountItem = new CustomerDiscountItem();
                        discountItem.LineDiscountType = DiscountTypes.Customer;
                        discountItem.CustomerDiscountType = CustomerDiscountTypes.TotalDiscount;
                        discountItem.Percentage = totalPercentage;

                        decimal discount = 0;
                        if (saleItem.TaxIncludedInItemPrice || retailTransaction.CalculateDiscountFrom == Store.CalculateDiscountsFromEnum.PriceWithTax)
                        {
                            discount = totalAmountDiscountPercentage * saleItem.PriceWithTax;
                        }
                        else
                        {
                            discount = totalAmountDiscountPercentage * saleItem.Price;
                        }

                        discountItem.Amount = discount;

                        if ((string)retailTransaction.Customer.FinalDiscountID != "")
                        {
                            RecordIdentifier priceDiscGroupID = new RecordIdentifier(1, 3, retailTransaction.Customer.FinalDiscountID);
                            discountItem.DiscountName = Providers.PriceDiscountGroupData.Get(entry, priceDiscGroupID).Text;
                        }

                        UpdateDiscountLines(saleItem, discountItem);
                    }
                }
            }

            return retailTransaction;
        }

        /// <summary>
        /// Updates the multilineGroup memory table, that is used in the customer multiline calculation.
        /// </summary>
        /// <param name="multiLineGroup">The multiline group.</param>
        /// <param name="quantity">The item quantity.</param>
        private void UpdateMultiLineDiscTable(string multiLineGroup, decimal quantity)
        {
            lock (threadLock)
            {
                bool rowfound = false;

                foreach (DataRow tableRow in multiLineDiscTable.Rows)
                {
                    if (multiLineGroup == (string)tableRow["MultiLineGroup"])
                    {
                        tableRow["Quantity"] = (decimal)tableRow["Quantity"] + quantity;
                        rowfound = true;
                    }
                }
                //If multiline group is not found then add a new row.
                if (!rowfound)
                {
                    DataRow row;
                    row = multiLineDiscTable.NewRow();
                    row["MultiLineGroup"] = multiLineGroup;
                    row["Quantity"] = quantity;
                    row["Percent1"] = 0M;
                    row["Percent2"] = 0M;
                    row["Amount"] = 0M;
                    row["MinQty"] = 0M;
                    multiLineDiscTable.Rows.Add(row);
                }
            }
        }

        /// <summary>
        /// Update the discount items.
        /// </summary>
        /// <param name="saleItem">The item line that the discount line is added to.</param>
        /// <param name="discountItem">The new discount item</param>
        private void UpdateDiscountLines(SaleLineItem saleItem, CustomerDiscountItem discountItem)
        {
            //Check if line discount is found, if so then update
            bool discountLineFound = false;
            bool lineDiscountFound = false;
            bool totalDiscountFound = false;
            foreach (DiscountItem discLine in saleItem.DiscountLines)
            {
                if (discLine is CustomerDiscountItem)
                {
                    CustomerDiscountItem customerDiscLine = (CustomerDiscountItem)discLine;
                    //If found then update
                    if ((customerDiscLine.LineDiscountType == discountItem.LineDiscountType) &&
                        (customerDiscLine.CustomerDiscountType == discountItem.CustomerDiscountType))
                    {
                        customerDiscLine.Percentage = discountItem.Percentage;
                        customerDiscLine.Amount = discountItem.Amount;
                        discountLineFound = true;
                    }
                }
                else if (discLine is LineDiscountItem)
                    lineDiscountFound = true;
                else if (discLine is TotalDiscountItem)
                    totalDiscountFound = true;
            }
            //If line discount is not found then add it.
            if (!discountLineFound)
            {
                if (BanCompoundDiscounts && (lineDiscountFound || totalDiscountFound) && !saleItem.PriceOverridden)
                {
                    ; // Ignore
                }
                else
                    saleItem.Add(discountItem);
            }

            saleItem.WasChanged = true;
        }

        /// <summary>
        /// Create the MultiLineDiscTable in memory
        /// </summary>
        private void MakeMultiLineDiscTable()
        {
            lock (threadLock)
            {
                // Create a new DataTable.
                multiLineDiscTable = new DataTable("MultiLineDiscTable");
                DataColumn column;

                //Adding MultiLineGrup
                column = new DataColumn();
                column.DataType = typeof(string);
                column.ColumnName = "MultiLineGroup";
                column.AutoIncrement = false;
                column.Caption = "MultiLineGroup";
                column.ReadOnly = false;
                column.Unique = true;
                multiLineDiscTable.Columns.Add(column);

                //Adding Quantity
                column = new DataColumn();
                column.DataType = typeof(decimal);
                column.ColumnName = "Quantity";
                column.AutoIncrement = false;
                column.Caption = "Quantity";
                column.ReadOnly = false;
                column.Unique = false;
                multiLineDiscTable.Columns.Add(column);

                //Adding Percent1
                column = new DataColumn();
                column.DataType = typeof(decimal);
                column.ColumnName = "Percent1";
                column.AutoIncrement = false;
                column.Caption = "Percent1";
                column.ReadOnly = false;
                column.Unique = false;
                multiLineDiscTable.Columns.Add(column);

                //Adding Percent2
                column = new DataColumn();
                column.DataType = typeof(decimal);
                column.ColumnName = "Percent2";
                column.AutoIncrement = false;
                column.Caption = "Percent2";
                column.ReadOnly = false;
                column.Unique = false;
                multiLineDiscTable.Columns.Add(column);

                //Adding Amount
                column = new DataColumn();
                column.DataType = typeof(decimal);
                column.ColumnName = "Amount";
                column.AutoIncrement = false;
                column.Caption = "Amount";
                column.ReadOnly = false;
                column.Unique = false;
                multiLineDiscTable.Columns.Add(column);

                //Adding min Quantity for activation
                column = new DataColumn();
                column.DataType = typeof(decimal);
                column.ColumnName = "MinQty";
                column.AutoIncrement = false;
                column.Caption = "MinQty";
                column.ReadOnly = false;
                column.Unique = false;
                multiLineDiscTable.Columns.Add(column);
            }
        }

        /// <summary>
        /// Is there a valid relation between the itemcode and relation?
        /// </summary>
        /// <param name="itemCode">The item code (table,group,all)</param>
        /// <param name="relation">The item relation</param>
        /// <returns>Returns true if the relation ok, else false.</returns>
        private bool ValidRelation(PriceDiscItemCode itemCode, string relation)
        {
            return (relation == "" && itemCode == PriceDiscItemCode.All)
                   || (relation != "" && itemCode != PriceDiscItemCode.All);
        }

        /// <summary>
        /// Is there a valid relation between the accountcode and relation?
        /// </summary>
        /// <param name="accountCode">The account code (table,group,all).</param>
        /// <param name="relation">The account relation.</param>
        /// <returns></returns>
        private bool ValidRelation(PriceDiscAccountCode accountCode, string relation)
        {
            return (relation == "" && accountCode == PriceDiscAccountCode.All)
                   || (relation != "" && accountCode != PriceDiscAccountCode.All);
        }

        /// <summary>
        /// Returns true or false, whether a certain relation is active for a discount search.
        /// </summary>
        /// <param name="relation">The discount relation(line,multiline,total)</param>
        /// <param name="accountCode">The account code(table,group,all)</param>
        /// <param name="itemCode">The item coude(table,group,all)</param>
        /// <returns>Returns true if the relation is active, else false.</returns>
        private bool Activation(PriceDiscType relation, PriceDiscAccountCode accountCode, PriceDiscItemCode itemCode)
        {
            switch (accountCode)
            {
                case PriceDiscAccountCode.Table:
                    switch (itemCode)
                    {
                        case PriceDiscItemCode.Table:
                            switch (relation)
                            {
                                case PriceDiscType.LineDiscSales:
                                    return discountAndPriceActivation.LineDiscountCustomerItem;
                                default:
                                    return false;
                            }
                        case PriceDiscItemCode.GroupId:
                            switch (relation)
                            {
                                case PriceDiscType.LineDiscSales:
                                    return discountAndPriceActivation.LineDiscountCustomerItemGroup;
                                case PriceDiscType.MultiLineDiscSales:
                                    return discountAndPriceActivation.MultilineDiscountCustomerItemGroup;
                                default:
                                    return false;
                            }
                        case PriceDiscItemCode.All:
                            switch (relation)
                            {
                                case PriceDiscType.LineDiscSales:
                                    return discountAndPriceActivation.LineDiscountCustomerAllItems;
                                case PriceDiscType.MultiLineDiscSales:
                                    return discountAndPriceActivation.MultilineDiscountCustomerAllItems;
                                case PriceDiscType.EndDiscSales:
                                    return discountAndPriceActivation.TotalDiscountCustomerAllItems;
                                default:
                                    return false;
                            }
                    }
                    break;

                case PriceDiscAccountCode.GroupId:
                    switch (itemCode)
                    {
                        case PriceDiscItemCode.Table:
                            switch (relation)
                            {
                                case PriceDiscType.LineDiscSales:
                                    return discountAndPriceActivation.LineDiscountCustomerGroupItem;
                                default:
                                    return false;
                            }
                        case PriceDiscItemCode.GroupId:
                            switch (relation)
                            {
                                case PriceDiscType.LineDiscSales:
                                    return discountAndPriceActivation.LineDiscountCustomerGroupItemGroup;
                                case PriceDiscType.MultiLineDiscSales:
                                    return discountAndPriceActivation.MultilineDiscountCustomerGroupItemGroup;
                                default:
                                    return false;
                            }
                        case PriceDiscItemCode.All:
                            switch (relation)
                            {
                                case PriceDiscType.LineDiscSales:
                                    return discountAndPriceActivation.LineDiscountCustomerGroupAllItems;
                                case PriceDiscType.MultiLineDiscSales:
                                    return discountAndPriceActivation.MultilineDiscountCustomerGroupAllItems;
                                case PriceDiscType.EndDiscSales:
                                    return discountAndPriceActivation.TotalDiscountCustomerGroupAllItems;
                                default:
                                    return false;
                            }
                    }
                    break;

                case PriceDiscAccountCode.All:
                    switch (itemCode)
                    {
                        case PriceDiscItemCode.Table:
                            switch (relation)
                            {
                                case PriceDiscType.LineDiscSales:
                                    return discountAndPriceActivation.LineDiscountAllCustomersItem;
                                default:
                                    return false;
                            }
                        case PriceDiscItemCode.GroupId:
                            switch (relation)
                            {
                                case PriceDiscType.LineDiscSales:
                                    return discountAndPriceActivation.LineDiscountAllCustomersItemGroup;
                                case PriceDiscType.MultiLineDiscSales:
                                    return discountAndPriceActivation.MultilineDiscountAllCustomersItemGroup;
                                default:
                                    return false;
                            }
                        case PriceDiscItemCode.All:
                            switch (relation)
                            {
                                case PriceDiscType.LineDiscSales:
                                    return discountAndPriceActivation.LineDiscountAllCustomersAllItems;
                                case PriceDiscType.MultiLineDiscSales:
                                    return discountAndPriceActivation.MultilineDiscountAllCustomersAllItems;
                                case PriceDiscType.EndDiscSales:
                                    return discountAndPriceActivation.TotalDiscountAllCustomersAllItems;
                                default:
                                    return false;
                            }
                    }
                    break;
            }

            return false;
        }

        public void CustomersDiscountedPurchasesStatus(
            IConnectionManager entry,
            string customerID,
            out decimal maxDiscountedPurchases,
            out decimal currentPeriodDiscountedPurchases)
        {
            CustomerGroup customerGroup = Providers.CustomerGroupData.GetDefaultCustomerGroup(entry, customerID);
            if (customerGroup != null && customerGroup.UsesDiscountedPurchaseLimit)
            {
                maxDiscountedPurchases = customerGroup.MaxDiscountedPurchases;
                currentPeriodDiscountedPurchases = DiscountedAmountForPeriod(entry, customerID, customerGroup.CurrentPeriodStart, customerGroup.CurrentPeriodEnd);
            }
            else
            {
                maxDiscountedPurchases = 0;
                currentPeriodDiscountedPurchases = 0;
            }
        }

        private decimal DiscountedAmountForPeriod(
            IConnectionManager entry,
            RecordIdentifier customerID,
            DateTime start,
            DateTime end)
        {
            List<SalesTransaction> transactionsWithinPeriod = Providers.SalesTransactionData.GetDiscountedItemsForCustomer(
                entry,
                (string)customerID,
                start,
                end);

            return transactionsWithinPeriod.Sum(x => x.Price - x.DiscountAmount);
        }

        #endregion CustomerDiscount

        #region PeriodicDiscount

        private void MakeActiveOfferTables()
        {
            //Adding colums to activeOffers
            activeOffers.Columns.Add("OfferId", typeof(string));
            activeOffers.Columns.Add("Description", typeof(string));
            activeOffers.Columns.Add("Status", typeof(int));
            activeOffers.Columns.Add("PDType", typeof(int));
            activeOffers.Columns.Add("Priority", typeof(int));
            activeOffers.Columns.Add("DiscValidPeriodId", typeof(string));
            activeOffers.Columns.Add("DiscountType", typeof(int));
            activeOffers.Columns.Add("SameDiffMMLines", typeof(int));
            activeOffers.Columns.Add("NoOfLinesToTrigger", typeof(int));
            activeOffers.Columns.Add("DealPriceValue", typeof(decimal));
            activeOffers.Columns.Add("DiscountPctValue", typeof(decimal));
            activeOffers.Columns.Add("DiscountAmountValue", typeof(decimal));
            activeOffers.Columns.Add("NoOfLeastExpensiveItems", typeof(int));
            //activeOffers.Columns.Add("NoOfTimesApplicable", typeof(int));
            activeOffers.Columns.Add("NoLinesTriggered", typeof(int));      // The number of lines that have been triggerd in an mix and match offer. Shoulb be equal or less than NoOfLinesToTrigger.
            activeOffers.Columns.Add("NoOfTimesActivated", typeof(int));    // The number times the offer has been activated. Should be equal or less than NoOfTimesApplicable
            DataColumn[] primaryKey = new DataColumn[3];
            primaryKey[0] = activeOffers.Columns["OfferId"];
            primaryKey[1] = activeOffers.Columns["PDType"];
            primaryKey[2] = activeOffers.Columns["Priority"];
            activeOffers.PrimaryKey = primaryKey;

            //Adding columns to activeOfferLines
            activeOfferLines.Columns.Add("OfferId", typeof(string));            // The offer id
            activeOfferLines.Columns.Add("LineId", typeof(int));                // The offer line id
            activeOfferLines.Columns.Add("Id", typeof(string));                 // The item/itemgroup id
            activeOfferLines.Columns.Add("SaleLinePosition", typeof(int));      // The 1-based index of the sale line in the transaction. 
            activeOfferLines.Columns.Add("SaleLineId", typeof(Guid));           // The unique ID of the sale line
            activeOfferLines.Columns.Add("Quantity", typeof(decimal));          // The item quantity
            activeOfferLines.Columns.Add("DealPriceOrDiscPct", typeof(decimal));// The deal price or discount percentage
            activeOfferLines.Columns.Add("LineGroup", typeof(string));
            activeOfferLines.Columns.Add("DiscType", typeof(int));
            activeOfferLines.Columns.Add("Status", typeof(int));
            activeOfferLines.Columns.Add("NoOfItemsNeeded", typeof(int));
            activeOfferLines.Columns.Add("MixMatchPriority", typeof(decimal)); // The order priority for the mix and match lines, higher values give higher priority
            activeOfferLines.Columns.Add("ProductType", typeof(int));
            DataColumn[] primaryLineKey = new DataColumn[4];
            primaryLineKey[0] = activeOfferLines.Columns["OfferId"];
            primaryLineKey[1] = activeOfferLines.Columns["Id"];
            primaryLineKey[2] = activeOfferLines.Columns["SaleLineId"];
            primaryLineKey[3] = activeOfferLines.Columns["DealPriceOrDiscPct"];
            activeOfferLines.PrimaryKey = primaryLineKey;
        }

        private void MakeTmpOfferTable()
        {
            tmpMMOffer.Columns.Add("SaleLinePosition", typeof(int));
            tmpMMOffer.Columns.Add("SaleLineId", typeof(Guid));
            tmpMMOffer.Columns.Add("ItemsTriggered", typeof(decimal));
            tmpMMOffer.Columns.Add("DiscType", typeof(int));
            tmpMMOffer.Columns.Add("DealPriceOrDiscPct", typeof(decimal));
            DataColumn[] pk = new DataColumn[1];
            pk[0] = tmpMMOffer.Columns["SaleLineId"];
            tmpMMOffer.PrimaryKey = pk;
        }

        private bool IsDiscountable(ISaleLineItem saleLineItem)
        {
            return saleLineItem.ShouldCalculateAndDisplayAssemblyPrice() &&
                   !saleLineItem.NoDiscountAllowed &&
                   !saleLineItem.Voided &&
                   saleLineItem.LoyaltyPoints.Relation != LoyaltyPointsRelation.Discount &&
                   !saleLineItem.ReceiptReturnItem;
        }

        /// <summary>
        /// Loops through the transaction to find offers that the items are in.
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="retailTransaction"></param>
        private void FindPeriodicOffers(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            lock (threadLock)
            {
                priceGroupLines = Providers.PriceDiscountGroupData.GetPriceGroupsForStore(entry, entry.CurrentStoreID, CacheType.CacheTypeTransactionLifeTime);

                activeOffers.Clear();
                activeOfferLines.Clear();

                //Find all the active periodic offers 
                foreach (SaleLineItem saleItem in retailTransaction.SaleItems.Where(w => IsDiscountable(w)))
                {
                    GetDiscountsFromDataBase(entry, retailTransaction, saleItem);

                    //Loop trough the offers found for the item that should be triggered automatically
                    foreach (PeriodicDiscount pd in periodicDiscountData.Where(w => w.TransactionID == retailTransaction.TransactionId
                                                                                    && ((w.OfferId != "" && w.Triggering == DiscountOffer.TriggeringEnum.Automatic)
                                                                                        ||
                                                                                        (retailTransaction.ManualPeriodicDiscounts.Select(e => e.ID).Contains(w.OfferId) &&
                                                                                         w.Triggering == DiscountOffer.TriggeringEnum.Manual))))
                    {
                        if (PeriodIsValid(entry, pd, retailTransaction))
                        {
                            if (DiscountIsValidForItem(entry, pd, saleItem, retailTransaction))
                            {
                                if (DiscountIsValidForCustomer(entry, pd, retailTransaction))
                                {
                                    if (priceGroupLines.Exists(p => p.PriceGroupID == pd.PriceGroup) || pd.PriceGroup == "")
                                    {
                                        AddToActiveOffers(entry, pd, saleItem);                                       
                                    }
                                }
                            }
                        }

                        // If we are dealing with variant items, we need to check if we've already added the header item. We need to prune the list
                        // so we don't double-count the item 
                        if (saleItem.HeaderItemID != Guid.Empty && activeOfferLines.Rows.Count > 1)
                        {
                            RecordIdentifier headerItemID = Providers.RetailItemData.GetHeaderItemID(entry, saleItem.ItemId, CacheType.CacheTypeTransactionLifeTime);

                            string filterExpr = "OfferId='" + pd.OfferId + "' AND Id='" + (string)headerItemID
                                                 + "' OR Id='" + saleItem.ItemId
                                                 + "' AND SaleLineId='" + saleItem.ID.ToString()
                                                 + "' AND DealPriceOrDiscPct='" + pd.DealPriceOrDiscPct + "'";

                            DataRow[] offerLines = activeOfferLines.Select(filterExpr);                            

                            // The lines contain both the item and it's header item, we remove the header item so that the item-specific discount overrides the header item discount
                            if (offerLines.Length > 1 && offerLines.Any(p => (string)p["Id"] == (string)headerItemID) && offerLines.Any(p => (string)p["Id"] == saleItem.ItemId))
                            {
                                var line = offerLines.Single(p => (string)p["Id"] == (string)headerItemID);

                                activeOfferLines.Rows.Remove(line);
                            }
                        }
                    }                    
                }
            }
        }

        private void AddToActiveOffers(IConnectionManager entry, PeriodicDiscount pd, ISaleLineItem saleItem)
        {
            string filterExpr = "OfferId='" + pd.OfferId + "'";
            try
            {
                lock (activeOffersLock)
                {
                    DataRow[] dr = activeOffers.Select(filterExpr);
                    // If has not been added yet to active offers
                    if (dr.Length == 0)
                    {
                        DataRow offerRow;
                        offerRow = activeOffers.NewRow();
                        offerRow["OfferId"] = pd.OfferId; // row["OfferId"];
                        offerRow["Description"] = pd.Description; // row["Description"];
                        offerRow["PDType"] = pd.PDType; // row["PDType"];
                        offerRow["Priority"] = pd.Priority; // row["Priority"];
                        offerRow["DiscValidPeriodId"] = pd.DiscValidPeriodId; // row["DiscValidPeriodId"];
                        offerRow["DiscountType"] = pd.DiscountType; // row["DiscountType"];
                        //   offerRow["SameDiffMMLines"] = pd.SameDiffMMLines; // row["SameDiffMMLines"];
                        offerRow["NoOfLinesToTrigger"] = pd.NoOfLinesToTrigger; //row["NoOfLinesToTrigger"];
                        offerRow["DealPriceValue"] = pd.DealPriceValue; // row["DealPriceValue"];
                        offerRow["DiscountPctValue"] = pd.DiscountPctValue; // row["DiscountPctValue"];
                        offerRow["DiscountAmountValue"] = pd.DiscountAmountValue; // row["DiscountAmountValue"];
                        offerRow["NoOfLeastExpensiveItems"] = pd.NoOfLeastExpItems; // row["NoOfLeastExpItems"];
                        //offerRow["NoOfTimesApplicable"] = pd.NoOfTimesApplicable; // row["NoOfTimesApplicable"];
                        offerRow["NoLinesTriggered"] = 0;
                        offerRow["NoOfTimesActivated"] = 0;
                        activeOffers.Rows.Add(offerRow);
                    }
                }

                string id = pd.GetID();

                lock (activeOfferLinesLock)
                {
                    filterExpr = "OfferId='" + pd.OfferId + "' AND Id='" + id                                 
                                 + "' AND SaleLineId='" + saleItem.ID.ToString()                                 
                                 + "' AND DealPriceOrDiscPct='" + pd.DealPriceOrDiscPct + "'";
                    DataRow[] aol = activeOfferLines.Select(filterExpr);
                    if (aol.Length == 0)
                    {
                        DataRow offerLineRow;
                        offerLineRow = activeOfferLines.NewRow();
                        offerLineRow["OfferId"] = pd.OfferId; // row["OfferId"];
                        offerLineRow["LineId"] = pd.LineId; // row["LineId"];
                        offerLineRow["Id"] = id; // row["Id"];                                 //ItemID or GroupId or BarcodeId
                        offerLineRow["SaleLineId"] = saleItem.ID;
                        offerLineRow["SaleLinePosition"] = saleItem.LineId;
                        offerLineRow["ProductType"] = pd.ProductType; // productType;
                        offerLineRow["Quantity"] = saleItem.Quantity;
                        offerLineRow["DealPriceOrDiscPct"] = pd.DealPriceOrDiscPct; // row["DealPriceOrDiscPct"];
                        offerLineRow["LineGroup"] = pd.LineGroup; // row["LineGroup"];
                        offerLineRow["DiscType"] = pd.DiscType; // row["DiscType"];
                        offerLineRow["Status"] = 1; // row["Status"];
                        //int noOfItemsNeeded = 0;
                        //if (row["NoOfItemsNeeded"] == System.DBNull.Value) { noOfItemsNeeded = 0; } else { noOfItemsNeeded = (int)row["NoOfItemsNeeded"]; }                                            
                        decimal priority = saleItem.PriceWithTax * pd.NoOfItemsNeeded; // noOfItemsNeeded;
                        offerLineRow["NoOfItemsNeeded"] = pd.NoOfItemsNeeded; // noOfItemsNeeded;
                        offerLineRow["MixMatchPriority"] = priority;
                        activeOfferLines.Rows.Add(offerLineRow);
                    }
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw ex;
            }
        }

        private bool DiscountIsValidForCustomer(IConnectionManager entry, PeriodicDiscount pd, IRetailTransaction retailTransaction)
        {
            if (pd.AccountCode == DiscountOffer.AccountCodeEnum.None)
                return true;

            if ((string)retailTransaction.Customer.ID != "")
            {
                switch (pd.AccountCode)
                {
                    case DiscountOffer.AccountCodeEnum.Customer:
                        return pd.AccountRelation == retailTransaction.Customer.ID;
                    case DiscountOffer.AccountCodeEnum.CustomerGroup:
                        return Providers.PriceDiscountGroupData.CustomerExistsInGroup(entry, PriceDiscGroupEnum.LineDiscountGroup, pd.AccountRelation, retailTransaction.Customer.ID);
                }
            }

            return false;
        }

        private bool DiscountIsValidForItem(IConnectionManager entry, PeriodicDiscount pd, ISaleLineItem saleItem, IRetailTransaction retailTransaction)
        {
            // When we have a mix of sales and return items on a transaction we exclude the return items from the calculations.             
            if(pd.DiscountType == DiscountType.LeastExpensive)
            {
                if(saleItem.Quantity < 0 && retailTransaction.ISaleItems.Any(p => !p.Voided && p.Quantity > 0))
                {
                    return false;
                }
            }

            switch (pd.PDType)
            {
                case PeriodicDiscOfferType.MixAndMatch:
                case PeriodicDiscOfferType.Multibuy:
                case PeriodicDiscOfferType.Offer:
                    switch ((DiscountOfferLine.DiscountOfferTypeEnum)pd.ProductType)
                    {
                        case DiscountOfferLine.DiscountOfferTypeEnum.All:
                            return true;
                        //TODO: deal with dimensions
                        //case DiscountOfferLine.DiscountOfferTypeEnum.Variant:
                        //    return pd.VariationNumber == saleItem.Dimension.VariantNumber;
                        case DiscountOfferLine.DiscountOfferTypeEnum.Item:
                            bool unitTriggered = true;
                            if (pd.PDType == PeriodicDiscOfferType.Multibuy)
                            {
                                unitTriggered = pd.UnitId.Equals(saleItem.SalesOrderUnitOfMeasure, StringComparison.CurrentCultureIgnoreCase);
                            }
                            bool headerItemTriggered = pd.TargetMasterID == saleItem.HeaderItemID;
                            bool itemTriggered = pd.ItemId.Equals(saleItem.ItemId, StringComparison.CurrentCultureIgnoreCase);
                            return (headerItemTriggered || itemTriggered) && unitTriggered;
                        case DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment:
                            return (pd.RetailDepartmentId.Equals(saleItem.ItemDepartmentId, StringComparison.CurrentCultureIgnoreCase));
                        case DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup:
                            return (pd.RetailGroupId.Equals(saleItem.RetailItemGroupId, StringComparison.CurrentCultureIgnoreCase));
                        case DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup:
                            return Providers.SpecialGroupData.ItemInSpecialGroup(entry, pd.SpecialGroup, saleItem.ItemId);
                    }
                    break;
            }
            return false;
        }

        public void ClearManuallyTriggeredDiscount(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            //If no discounts are on the transaction there is no need to ask which one to clear
            if (retailTransaction.ManualPeriodicDiscounts.Any())
            {
                using (SelectionDialog dlg = new SelectionDialog(new DataEntitySelectionList(retailTransaction.ManualPeriodicDiscounts), Properties.Resources.SelectPeriodicDiscountToClear, false, false))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        DataEntity selectedEntity = (DataEntity)dlg.SelectedItem;
                        retailTransaction.ManualPeriodicDiscounts.Remove(selectedEntity);
                    }
                }
            }
        }

        public void ClearAllDiscounts(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            //This function clears out all discounts of any type
            retailTransaction.ClearAllDiscountLines();            
        }

        public void ManuallyTriggerPeriodicDiscount(IConnectionManager entry, IRetailTransaction retailTransaction, string offerID)
        {
            retailTransaction.ClearPeriodicDiscountLines();

            FindManuallyTriggeredPeriodicDiscounts(entry, retailTransaction, offerID);
            FindPeriodicOffers(entry, retailTransaction);
            RegisterPeriodicDisc(entry, retailTransaction);
        }

        private void FindManuallyTriggeredPeriodicDiscounts(IConnectionManager entry,
            IRetailTransaction retailTransaction, string offerID)
        {
            foreach (ISaleLineItem item in retailTransaction.SaleItems)
            {
                GetDiscountsFromDataBase(entry, retailTransaction, item);
            }

            if (offerID == "")
            {
                List<DataEntity> triggerablePeriodicDiscounts = new List<DataEntity>();
                priceGroupLines = Providers.PriceDiscountGroupData.GetPriceGroupsForStore(entry, entry.CurrentStoreID);

                foreach (PeriodicDiscount pd in periodicDiscountData.Where(
                                w =>
                                w.TransactionID == retailTransaction.TransactionId && w.OfferId != "" &&
                                w.Triggering == DiscountOffer.TriggeringEnum.Manual))
                {
                    if (PeriodIsValid(entry, pd, retailTransaction))
                    {
                        if (DiscountIsValidForCustomer(entry, pd, retailTransaction))
                        {
                            if (!triggerablePeriodicDiscounts.Select(e => e.ID).Contains(pd.OfferId) && !retailTransaction.ManualPeriodicDiscounts.Select(de => de.ID).Contains(pd.OfferId))
                            {
                                triggerablePeriodicDiscounts.Add(new DataEntity(pd.OfferId, pd.Description));
                            }
                        }
                    }
                }
                if (triggerablePeriodicDiscounts.Count == 0)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.NoPeriodicDiscountsToTrigger);
                    return;
                }

                using (SelectionDialog dlg = new SelectionDialog(new DataEntitySelectionList(triggerablePeriodicDiscounts), Properties.Resources.SelectPeriodicDiscountToTrigger, false, false))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        DataEntity selectedEntity = (DataEntity)dlg.SelectedItem;

                        if (!retailTransaction.ManualPeriodicDiscounts.Select(e => e.ID).Contains(selectedEntity.ID))
                        {
                            retailTransaction.ManualPeriodicDiscounts.Add(selectedEntity);
                        }
                    }
                }
            }
            else
            {
                PeriodicDiscount selectedDiscount = periodicDiscountData.FirstOrDefault(p => p.OfferId == offerID);
                if (selectedDiscount != null)
                {
                    if (selectedDiscount.Triggering == DiscountOffer.TriggeringEnum.Automatic)
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.DiscountNotManuallyTriggerable.Replace("#1", selectedDiscount.Description));
                        return;
                    }
                    if (!retailTransaction.ManualPeriodicDiscounts.Select(e => e.ID).Contains(selectedDiscount.ID))
                    {
                        retailTransaction.ManualPeriodicDiscounts.Add(new DataEntity(selectedDiscount.OfferId, selectedDiscount.Description));
                    }
                }
            }
        }

        private bool PeriodIsValid(IConnectionManager entry, PeriodicDiscount pd, IRetailTransaction retailTransaction)
        {
            PeriodStatus periodStatus = retailTransaction.Period.IsValid(pd.DiscValidPeriodId);
            if (periodStatus == PeriodStatus.NotFoundInMemoryTable)
            {
                bool discountIsValid = Providers.DiscountPeriodData.IsDiscountPeriodValid(entry, pd.DiscValidPeriodId, DateTime.Now);
                periodStatus = discountIsValid ? PeriodStatus.IsValid : PeriodStatus.IsInvalid;
                retailTransaction.Period.Add(pd.DiscValidPeriodId, (periodStatus == PeriodStatus.IsValid));
            }

            return periodStatus == PeriodStatus.IsValid;
        }

        private void GetDiscountsFromDataBase(IConnectionManager entry, IRetailTransaction retailTransaction, ISaleLineItem saleItem)
        {
            //Try to receive the data from the memory table else get it from the database.
            //Check for the item and retail item group separately if either one returns 0 then get the 
            //data from the database.
            int itemCount = periodicDiscountData.Count(c => c.ItemId == saleItem.ItemId && c.TransactionID == retailTransaction.TransactionId);
            int groupCount = periodicDiscountData.Count(c => c.RetailGroupId == saleItem.RetailItemGroupId && c.TransactionID == retailTransaction.TransactionId);
            int barcodeCount = periodicDiscountData.Count(c => c.BarcodeId == saleItem.BarcodeId && !string.IsNullOrEmpty(saleItem.BarcodeId) && c.TransactionID == retailTransaction.TransactionId);
            int departmentCount = periodicDiscountData.Count(c => c.RetailDepartmentId == saleItem.ItemDepartmentId && c.TransactionID == retailTransaction.TransactionId);
            // TODO: deal with dimensions
            //int variantCount = 0;
            //int variantCount = periodicDiscountData.Count(c => c.VariationNumber == (string)saleItem.Dimension.VariantNumber && c.TransactionID == retailTransaction.TransactionId);

            if (itemCount == 0 || groupCount == 0 || barcodeCount == 0 || departmentCount == 0)
            {
                //Only get data that we need
                string itemId = saleItem.ItemId != "" ? saleItem.ItemId : "";
                string unitId = saleItem.SalesOrderUnitOfMeasure != "" ? saleItem.SalesOrderUnitOfMeasure : "";
                string groupId = saleItem.RetailItemGroupId != "" ? saleItem.RetailItemGroupId : "";
                string barcodeId = saleItem.BarcodeId != "" ? saleItem.BarcodeId : "";
                string departmentId = saleItem.ItemDepartmentId != "" ? saleItem.ItemDepartmentId : "";
                //TODO: deal with dimensions
                //string variantNumber = (string)saleItem.Dimension.VariantNumber;
                //string variantNumber = "";

                if (itemCount > 0)
                { itemId = ""; }
                if (groupCount > 0)
                { groupId = ""; }
                if (barcodeCount > 0)
                { barcodeId = ""; }
                if (departmentCount > 0)
                { departmentId = ""; }
                // if (variantCount > 0) { variantNumber = ""; }

                if (barcodeId == saleItem.ItemId)
                { barcodeId = ""; }

                List<PeriodicDiscount> discountList =
                    DataProviderFactory.Instance.Get<IDiscountData, DataEntity>()
                        .GetPeriodicDiscountList(entry, itemId, unitId, groupId, departmentId, retailTransaction.TransactionId,
                            CacheType.CacheTypeTransactionLifeTime);
                if (discountList.Count == 0)
                {
                    PeriodicDiscount pd = new PeriodicDiscount(retailTransaction.TransactionId);
                    bool addToList = false;
                    //Add an row for items that have no discountoffer so that no need to to query the database
                    if (itemId.Trim() != "")
                    {
                        pd.AddItem(saleItem.ItemId);
                        addToList = true;
                        //retailTransaction.PeriodicDiscount.AddItem(saleItem.ItemId);
                    }
                    if (groupId.Trim() != "")
                    {
                        pd.AddGroup(saleItem.RetailItemGroupId);
                        addToList = true;
                        //retailTransaction.PeriodicDiscount.AddRetailGroup(saleItem.RetailItemGroupId);
                    }
                    if (barcodeId.Trim() != "")
                    {
                        pd.AddBarcode(barcodeId);
                        addToList = true;
                        //retailTransaction.PeriodicDiscount.AddBarcode(saleItem.BarcodeId);
                    }
                    if (departmentId.Trim() != "")
                    {
                        pd.AddDepartment(saleItem.ItemDepartmentId);
                        addToList = true;
                        //retailTransaction.PeriodicDiscount.AddDepartment(saleItem.ItemDepartmentId);
                    }

                    if (addToList)
                    {
                        periodicDiscountData.Add(pd);
                    }
                }
                else
                {
                    foreach (PeriodicDiscount periodicDiscount in discountList)
                    {
                        // Discount Offer dicounts with type ALL can get repeatedly added. We want to make sure that this does not happen                    
                        if (periodicDiscountData.Exists(p => p.OfferId == periodicDiscount.OfferId && p.PDType == PeriodicDiscOfferType.Offer && (DiscountOfferLine.DiscountOfferTypeEnum)p.ProductType == DiscountOfferLine.DiscountOfferTypeEnum.All))
                        {
                            continue;
                        }

                        periodicDiscountData.Add(periodicDiscount);
                    }
                }
            }
        }

        /// <summary>
        /// Loops through the active offers in priority order.  
        /// Starting with offers with the highest order(lowest number) first. 
        /// </summary>
        /// <param name="retailTransaction">The retailtransaction.</param>
        private IRetailTransaction RegisterPeriodicDisc(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            lock (threadLock)
            {
                foreach (DataRow row in activeOffers.Select("", "Priority ASC"))
                {
                    string offerId = (string)row["OfferId"];
                    string offerName = (string)row["Description"];
                    DiscountType discType = (DiscountType)row["DiscountType"];
                    PeriodicDiscOfferType discOfferType = (PeriodicDiscOfferType)row["PDType"];
                    switch (discOfferType)
                    {
                        case PeriodicDiscOfferType.MixAndMatch:
                            retailTransaction = CalcMixMatch(entry, offerId, offerName, row, retailTransaction);
                            break;
                        case PeriodicDiscOfferType.Multibuy:
                            retailTransaction = CalcMultiBuy(entry, offerId, offerName, retailTransaction, discType);
                            break;
                        case PeriodicDiscOfferType.Offer:
                            retailTransaction = CalcDiscountOffer(offerId, offerName, retailTransaction, discType);
                            break;
                            //case (int)PeriodicDiscType.Promotion: 
                            //If an item is in a promotion is should be worked with as an active price 
                            //to be used in other discounts. This is dealt with in Price.cs 
                    }
                }
            }

            return retailTransaction;
        }

        /// <summary>
        /// Calculate the periodic discounts for the transation.
        /// </summary>
        /// <param name="retailtransaction"></param>
        public IRetailTransaction CalcPeriodicDisc(IConnectionManager entry, IRetailTransaction retailTransaction, bool CalculateNow)
        {
            if (CalculateNow || retailTransaction.CalcPeriodicDiscounts == CalculatePeriodicDiscountEnums.AlwaysCalculate)
            {
                //Clear all the periodic discounts 
                retailTransaction.ClearPeriodicDiscountLines();
                //Clear Customer discounts

                retailTransaction.ClearCustomerDiscountLines();
                //Find all possible offfers
                FindPeriodicOffers(entry, retailTransaction);
                //Calculate the periodic offers
                retailTransaction = RegisterPeriodicDisc(entry, retailTransaction);
            }

            return retailTransaction;
        }

        private IRetailTransaction CalcDiscountOffer(string offerId, string offerName, IRetailTransaction retailTransaction, DiscountType discType)
        {
            //Loop through all the lines in a specific offer
            foreach (DataRow row in activeOfferLines.Select("OfferId='" + offerId + "'", "OfferId ASC, DealPriceOrDiscPct DESC"))
            {
                ISaleLineItem saleItem = retailTransaction.GetItem((Guid)row["SaleLineId"]);

                bool variantCanContinue = !((int)row["ProductType"] == (int)DiscountOfferLine.DiscountOfferTypeEnum.BarCodeBasedVariant && saleItem.BarcodeId != (string)row["Id"]);
                if ((saleItem.PeriodicDiscountOfferId == "" || saleItem.PeriodicDiscountOfferId == null) && variantCanContinue)
                {
                    if (saleItem.Quantity != 0)
                    {
                        PeriodicDiscountItem discountItem = new PeriodicDiscountItem();
                        discountItem.LineDiscountType = DiscountTypes.Periodic;
                        discountItem.PeriodicDiscountType = PeriodicDiscOfferType.Offer;

                        discountItem.Percentage = (decimal)row["DealPriceOrDiscPct"];
                        discountItem.Amount = 0;

                        saleItem.PeriodicDiscountOfferId = offerId;
                        saleItem.PeriodicDiscountOfferName = offerName;
                        saleItem.QuantityDiscounted = saleItem.Quantity;
                        saleItem.PeriodicDiscType = LineEnums.PeriodicDiscountType.DiscountOffer;
                        saleItem.WasChanged = true;

                        UpdatePeriodicDiscountLines(saleItem, discountItem);
                        //saleItem.CalculateLine(); -- Not needed here - calculate line called later after all discounts are found
                    }
                }
                else //Discount has been calculated in a mixmatch offer for some of the line items
                {
                    if ((saleItem.Quantity - saleItem.QuantityDiscounted) > 0)
                    {
                        DataRow newRow = SplitLine(ref retailTransaction, saleItem.LineId, (saleItem.Quantity - saleItem.QuantityDiscounted));
                        saleItem = retailTransaction.GetItem(retailTransaction.SaleItems.Count);
                        if (saleItem.Quantity != 0)
                        {
                            PeriodicDiscountItem discountItem = new PeriodicDiscountItem();
                            discountItem.LineDiscountType = DiscountTypes.Periodic;
                            discountItem.PeriodicDiscountType = PeriodicDiscOfferType.Offer;

                            discountItem.Percentage = (decimal)row["DealPriceOrDiscPct"];
                            discountItem.Amount = 0;

                            saleItem.PeriodicDiscountOfferId = offerId;
                            saleItem.PeriodicDiscountOfferName = offerName;
                            saleItem.QuantityDiscounted = saleItem.Quantity;
                            saleItem.PeriodicDiscType = LineEnums.PeriodicDiscountType.DiscountOffer;
                            saleItem.WasChanged = true;

                            UpdatePeriodicDiscountLines(saleItem, discountItem);
                            //saleItem.CalculateLine(); -- Not needed here - calculate line called later after all discounts are found
                        }
                    }
                }
            }

            return retailTransaction;
        }

        /// <summary>
        /// Goes through the transaction and sums up the total quantity of items that match the given offer ID
        /// </summary>
        /// <param name="offerId">The id of the offer.</param>
        /// <param name="retailTransaction">The retail transaction</param>
        /// <returns>The total quantity of the items in the transaction.</returns>
        private decimal MultibuyLineQty(string offerId, IRetailTransaction retailTransaction)
        {
            decimal result = 0;

            //Loop through all the lines in a specific offer to find the totals for that item.
            foreach (DataRow row in activeOfferLines.Select("OfferId='" + offerId + "'", "OfferId ASC, LineId ASC"))
            {
                SaleLineItem saleItem = (SaleLineItem)retailTransaction.GetItem((Guid)row["SaleLineId"]);
                if(!saleItem.NoDiscountAllowed && !saleItem.Voided)
                {
                    //If the item doesn't have a periodic discount or has been added to the same offer we are looking at
                    //add the total quantity of the item to the result
                    if (saleItem.PeriodicDiscountOfferId == "" || saleItem.PeriodicDiscountOfferId == offerId)
                    {
                        result += saleItem.Quantity;
                    }
                    //If the item is in another offer then get the possible number of items that have not been discounted yet
                    else if (saleItem.PeriodicDiscountOfferId != "" && saleItem.PeriodicDiscountOfferId != offerId)
                    {
                        result += (saleItem.Quantity - saleItem.QuantityDiscounted);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Calculate the periodic multibuy discount.
        /// </summary>
        /// <param name="offerId">The id of the offer</param>
        /// <param name="retailTransaction">The retail transaction</param>
        /// <param name="discType"></param>
        /// <returns>The retail transaction</returns>
        private IRetailTransaction CalcMultiBuy(IConnectionManager entry, string offerId, string offerName, IRetailTransaction retailTransaction, DiscountType discType)
        {
            decimal totQtyForMultiBuyLine = 0;

            ISettings settings = GetSettings(entry);

            //Loop through all the lines in a specific offer to calculate the discount
            foreach (DataRow row in activeOfferLines.Select("OfferId='" + offerId + "'", "OfferId ASC, LineId ASC"))
            {
                ISaleLineItem saleItem = retailTransaction.GetItem((Guid)row["SaleLineId"]);
                //bool isUsingItemId = true;
                bool variantCanContinue = !((int)row["ProductType"] == (int)DiscountOfferLine.DiscountOfferTypeEnum.BarCodeBasedVariant && saleItem.BarcodeId != (string)row["id"]);

                /*bool variantCanContinue = true;
                if ((int)row["ProductType"] == (int)DiscountOfferLine.DiscountOfferTypeEnum.BarCodeBasedVariant && saleItem.BarcodeId != (string)row["id"])
                {
                    variantCanContinue = false;
                }
                else if ((int)row["ProductType"] == (int)DiscountOfferLine.DiscountOfferTypeEnum.BarCodeBasedVariant && saleItem.BarcodeId == (string)row["id"])
                {
                    //isUsingItemId = false;
                }*/

                if (variantCanContinue)
                {
                    totQtyForMultiBuyLine = MultibuyLineQty(offerId, retailTransaction);

                    MultibuyDiscountLine multibuyDiscountLine = Providers.MultibuyDiscountLineData.GetMultibuyOfferForQuantity(entry, offerId, Math.Abs(totQtyForMultiBuyLine));

                    if (multibuyDiscountLine != null && (decimal)multibuyDiscountLine.MinQuantity > 0)
                    {
                        if (saleItem.PeriodicDiscountOfferId == "" || saleItem.PeriodicDiscountOfferId == null)
                        {
                            saleItem.PeriodicDiscType = LineEnums.PeriodicDiscountType.Multibuy;
                            saleItem.PeriodicDiscountOfferId = offerId;
                            saleItem.PeriodicDiscountOfferName = offerName;

                            if (discType == DiscountType.MultiplyDealPrice || discType == DiscountType.MultiplyDiscountPct)
                            {
                                if ((totQtyForMultiBuyLine % (decimal)multibuyDiscountLine.MinQuantity) > 0M)
                                {
                                    totQtyForMultiBuyLine = totQtyForMultiBuyLine - (totQtyForMultiBuyLine % (decimal)multibuyDiscountLine.MinQuantity);
                                }
                            }

                            if (Math.Abs(saleItem.Quantity) < Math.Abs(totQtyForMultiBuyLine))
                            {
                                saleItem.QuantityDiscounted = saleItem.Quantity;
                            }
                            else
                            {
                                saleItem.QuantityDiscounted = totQtyForMultiBuyLine;
                            }

                            if (saleItem.Price != 0 || saleItem.PriceWithTax != 0)
                            {
                                if (saleItem.Quantity != 0)
                                {
                                    saleItem.PeriodicDiscountOfferId = offerId;
                                    saleItem.PeriodicDiscountOfferName = offerName;
                                    //saleItem.QuantityDiscounted = saleItem.Quantity;
                                    saleItem.WasChanged = true;

                                    PeriodicDiscountItem discountItem = new PeriodicDiscountItem();
                                    discountItem.LineDiscountType = DiscountTypes.Periodic;
                                    discountItem.PeriodicDiscountType = PeriodicDiscOfferType.Multibuy;

                                    if ((discType == DiscountType.DealPrice) || (discType == DiscountType.MultiplyDealPrice))
                                    {
                                        //if the company currency is not the same as the store currency 
                                        //then convert the unit price to the current store currency.
                                        if ((string)settings.CompanyInfo.CurrencyCode != settings.Store.Currency)
                                        {
                                            multibuyDiscountLine.PriceOrDiscountPercent = Services.Interfaces.Services.CurrencyService(entry).CurrencyToCurrency(
                                                entry,
                                                settings.CompanyInfo.CurrencyCode,
                                                settings.Store.Currency,
                                                settings.CompanyInfo.CurrencyCode,
                                                multibuyDiscountLine.PriceOrDiscountPercent);
                                        }
                                        discountItem.Percentage = 0;

                                        //The store can be configured to calculate discounts from price with tax or price excl tax
                                        if (retailTransaction.CalculateDiscountFrom == Store.CalculateDiscountsFromEnum.PriceWithTax)
                                        {
                                            if (multibuyDiscountLine.PriceOrDiscountPercent < saleItem.PriceWithTax)
                                            {
                                                discountItem.Amount = saleItem.PriceWithTax - multibuyDiscountLine.PriceOrDiscountPercent;
                                            }
                                        }
                                        else if (retailTransaction.CalculateDiscountFrom == Store.CalculateDiscountsFromEnum.Price)
                                        {
                                            if (multibuyDiscountLine.PriceOrDiscountPercent < saleItem.Price)
                                            {
                                                discountItem.Amount = saleItem.Price - multibuyDiscountLine.PriceOrDiscountPercent;
                                            }
                                        }
                                    }
                                    else if (discType == DiscountType.DiscountAmount)
                                    {
                                        if ((string)settings.CompanyInfo.CurrencyCode != settings.Store.Currency)
                                        {
                                            multibuyDiscountLine.PriceOrDiscountPercent = Services.Interfaces.Services.CurrencyService(entry).CurrencyToCurrency(
                                                entry,
                                                settings.CompanyInfo.CurrencyCode,
                                                settings.Store.Currency,
                                                settings.CompanyInfo.CurrencyCode,
                                                multibuyDiscountLine.PriceOrDiscountPercent);
                                        }
                                        discountItem.Percentage = 0;
                                        discountItem.Amount = multibuyDiscountLine.PriceOrDiscountPercent;
                                    }
                                    else
                                    {
                                        discountItem.Percentage = multibuyDiscountLine.PriceOrDiscountPercent;
                                        discountItem.Amount = 0;
                                    }
                                    UpdatePeriodicDiscountLines(saleItem, discountItem);
                                }
                            }
                        }
                        else //Discount has been calculated in a mixmatch offer for some of the line items
                        {
                            if ((saleItem.Quantity - saleItem.QuantityDiscounted) > 0)
                            {
                                DataRow newRow = SplitLine(ref retailTransaction, saleItem.LineId, (saleItem.Quantity - saleItem.QuantityDiscounted));
                                //Get the last sale line item that was just created
                                saleItem = retailTransaction.GetItem(retailTransaction.SaleItems.Count);
                                if (saleItem.Quantity != 0)
                                {
                                    saleItem.PeriodicDiscType = LineEnums.PeriodicDiscountType.Multibuy;
                                    saleItem.PeriodicDiscountOfferId = offerId;
                                    saleItem.PeriodicDiscountOfferName = offerName;
                                    saleItem.QuantityDiscounted = saleItem.Quantity;
                                    saleItem.WasChanged = true;

                                    PeriodicDiscountItem discountItem = new PeriodicDiscountItem();
                                    discountItem.LineDiscountType = DiscountTypes.Periodic;
                                    discountItem.PeriodicDiscountType = PeriodicDiscOfferType.Multibuy;

                                    if (discType == DiscountType.DealPrice || discType == DiscountType.MultiplyDealPrice)
                                    {
                                        //if the company currency is not the same as the store currency 
                                        //then convert the unit price to the current store currency.
                                        if ((string)settings.CompanyInfo.CurrencyCode != settings.Store.Currency)
                                        {
                                            multibuyDiscountLine.PriceOrDiscountPercent = Services.Interfaces.Services.CurrencyService(entry).CurrencyToCurrency(
                                                entry,
                                                settings.CompanyInfo.CurrencyCode,
                                                settings.Store.Currency,
                                                settings.CompanyInfo.CurrencyCode,
                                                multibuyDiscountLine.PriceOrDiscountPercent);
                                        }
                                        discountItem.Percentage = 0;
                                        discountItem.Amount = saleItem.PriceWithTax - multibuyDiscountLine.PriceOrDiscountPercent;
                                    }
                                    else
                                    {
                                        discountItem.Percentage = multibuyDiscountLine.PriceOrDiscountPercent;
                                        discountItem.Amount = 0;
                                    }

                                    UpdatePeriodicDiscountLines(saleItem, discountItem);
                                }
                            }
                        }
                    }
                }
            }
            return retailTransaction;
        }

        private DataTable CompressActiveOfferLines(DataTable offerLines)
        {
            bool negQtyExists = false;
            bool posQtyExists = false;

            foreach (DataRow row in offerLines.Select())
            {
                if ((decimal)row["Quantity"] > 0)
                { posQtyExists = true; }
                if ((decimal)row["Quantity"] < 0)
                { negQtyExists = true; }
            }

            if (posQtyExists && negQtyExists)
            {
                foreach (DataRow row in offerLines.Select("", "SaleLinePosition ASC"))
                {
                    foreach (DataRow row2 in offerLines.Select("Id='" + ((string)row["Id"]) + "'", "SaleLinePosition DESC"))
                    {
                        if ((int)row2["SaleLinePosition"] > (int)row["SaleLinePosition"])
                        {
                            if ((decimal)row["Quantity"] > 0 && (decimal)row2["Quantity"] < 0)
                            {
                                if ((decimal)row["Quantity"] + (decimal)row2["Quantity"] >= 0)
                                {
                                    row["Quantity"] = (decimal)row["Quantity"] + (decimal)row2["Quantity"];
                                    row2["Quantity"] = 0;
                                }
                                else
                                {
                                    row["Quantity"] = 0;
                                    row2["Quantity"] = (decimal)row["Quantity"] + (decimal)row2["Quantity"];
                                    ;
                                }
                            }
                            if ((decimal)row["Quantity"] < 0 && (decimal)row2["Quantity"] > 0)
                            {
                                if ((decimal)row["Quantity"] + (decimal)row2["Quantity"] <= 0)
                                {
                                    row["Quantity"] = (decimal)row["Quantity"] + (decimal)row2["Quantity"];
                                    row2["Quantity"] = 0;
                                }
                                else
                                {
                                    row["Quantity"] = 0;
                                    row2["Quantity"] = (decimal)row["Quantity"] + (decimal)row2["Quantity"];
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return offerLines;
        }

        private DataTable GetMixMatchOfferLines(string offerId)
        {
            //Get a copy of active offerLines for this offer ordered by priority
            DataView tmpOffer = new DataView(activeOfferLines);
            tmpOffer.RowFilter = "OfferId='" + offerId + "'";
            tmpOffer.Sort = ("MixMatchPriority ASC");

            //Create a new datatable with the Mix & Match information sorted by the Mix & Match priority
            DataTable offerLines = tmpOffer.ToTable();
            //Set the primary key as M&M priority + Sale line id
            DataColumn[] pk = new DataColumn[3];
            pk[0] = offerLines.Columns["MixMatchPriority"];
            pk[1] = offerLines.Columns["SaleLineId"];
            pk[2] = offerLines.Columns["Id"];
            offerLines.PrimaryKey = pk;

            return offerLines;
        }

        private IRetailTransaction CalcMixMatch(IConnectionManager entry, string offerId, string offerName, DataRow activeOffer, IRetailTransaction retailTransaction)
        {
            //Get all offerlines for the offer in question
            DataTable offerLines;
            //Must compress check because of minus quantities
            Dictionary<MixAndMatchLineGroup, decimal> mmGroups = Providers.MixAndMatchLineGroupData.GetGroups(entry, offerId, 0, false).ToDictionary(item => item, i => 0M);

            if (mmGroups.Count <= 0)
            {
                return retailTransaction;
            }
            //Initialize
            tmpMMOffer.Clear();
            do
            {
                //If the criteria for the offer has been fulfilled then update the itmes.
                if (ALLGroupsTriggered(mmGroups))
                {
                    foreach (DataRow tmpMMRow in tmpMMOffer.Select())
                    {
                        ISaleLineItem saleItem = retailTransaction.GetItem((Guid)tmpMMRow["SaleLineId"]);
                        saleItem.QuantityDiscounted += (decimal)tmpMMRow["ItemsTriggered"];
                    }

                    //Calculate discount and update all saleitems
                    RegisterMixMatch(entry, offerId, offerName, activeOffer, tmpMMOffer, retailTransaction);

                    tmpMMOffer.Clear();

                    //N?llstillum mmLineGroups
                    for (int index = 0; index < mmGroups.Keys.Count; index++)
                    {
                        mmGroups[mmGroups.Keys.ElementAt(index)] = 0;
                    }
                }

                //Get all offerlines for the offer in question
                offerLines = GetMixMatchOfferLines(offerId);

                //Must compress check because of minus quantities
                offerLines = CompressActiveOfferLines(offerLines);

                decimal totQuantityDiscounted = decimal.Zero;
                foreach (DataRow row in offerLines.Select("", "MixMatchPriority DESC"))
                {
                    if (ALLGroupsTriggered(mmGroups))
                    {
                        break;
                    }

                    ISaleLineItem saleItem = retailTransaction.GetItem((Guid)row["SaleLineId"]);
                    bool variantCanContinue = !((int)row["ProductType"] == (int)DiscountOfferLine.DiscountOfferTypeEnum.BarCodeBasedVariant && saleItem.BarcodeId != (string)row["Id"]);
                    bool saleItemCanContinue = ((saleItem.PeriodicDiscountOfferId == "") || (saleItem.PeriodicDiscountOfferId != "" && Math.Abs(saleItem.Quantity) > Math.Abs(saleItem.QuantityDiscounted)));

                    //Only look at sale items that haven't already been picked up for an offer
                    if (variantCanContinue && saleItemCanContinue)
                    {
                        DataRow newRow = row;
                        if (saleItem.PeriodicDiscountOfferId != "")
                        {
                            if ((Math.Abs(saleItem.Quantity) - Math.Abs(saleItem.QuantityDiscounted)) > 0)
                            {
                                decimal qtyNewLine = (Math.Abs(saleItem.Quantity) - Math.Abs(saleItem.QuantityDiscounted));
                                newRow = SplitLine(ref retailTransaction, saleItem.LineId, qtyNewLine);
                                saleItem = retailTransaction.GetItem(retailTransaction.SaleItems.Count);
                                if (saleItem.Quantity == 0)
                                {
                                    continue;
                                }
                            }
                        }
                        decimal leftToDiscount = Math.Abs((decimal)newRow["Quantity"]) - saleItem.QuantityDiscounted;
                        totQuantityDiscounted += leftToDiscount;
                        if (leftToDiscount > 0 && (saleItem.NoDiscountAllowed == false))
                        {
                            saleItem.PeriodicDiscType = LineEnums.PeriodicDiscountType.MixAndMatch;

                            //Go through each instance of the sale item and check if it can be in the promotion.
                            for (int i = 0; i < (int)leftToDiscount; i++)
                            {
                                string lineGroup = (string)newRow["LineGroup"];
                                for (int index = 0; index < mmGroups.Keys.Count; index++)
                                {
                                    if (mmGroups.Keys.ElementAt(index).LineGroup == lineGroup)
                                    {
                                        if ((Math.Abs(totQuantityDiscounted) >= Math.Abs(mmGroups[mmGroups.Keys.ElementAt(index)])) && Math.Abs(mmGroups.Keys.ElementAt(index).NumberOfItemsNeeded) > Math.Abs(mmGroups[mmGroups.Keys.ElementAt(index)]))
                                        {
                                            bool found = false;

                                            foreach (DataRow mmOldRow in tmpMMOffer.Select("SaleLineId='" + saleItem.ID.ToString() + "'"))
                                            {
                                                mmOldRow["ItemsTriggered"] = (decimal)mmOldRow["ItemsTriggered"] + (saleItem.Quantity < 1M ? -1 : 1);
                                                //mmOldRow["ItemsTriggered"] = (decimal) mmOldRow["ItemsTriggered"] + (saleItem.Quantity < 1M ? saleItem.Quantity : 1);
                                                found = true;
                                            }

                                            if (!found)
                                            {
                                                DataRow tmpMMRow;
                                                tmpMMRow = tmpMMOffer.NewRow();
                                                tmpMMRow["SaleLineId"] = saleItem.ID;
                                                tmpMMRow["SaleLinePosition"] = saleItem.LineId;
                                                tmpMMRow["ItemsTriggered"] = saleItem.Quantity < 1M ? -1 : 1;
                                                //tmpMMRow["ItemsTriggered"] = saleItem.Quantity < 1M ? saleItem.Quantity : 1;
                                                tmpMMRow["DiscType"] = row["DiscType"];
                                                tmpMMRow["DealPriceOrDiscPct"] = row["DealPriceOrDiscPct"];
                                                tmpMMOffer.Rows.Add(tmpMMRow);
                                            }

                                            mmGroups[mmGroups.Keys.ElementAt(index)] += saleItem.Quantity < 1M ? -1 : 1M;
                                            //mmGroups[mmGroups.Keys.ElementAt(index)] += saleItem.Quantity < 1M ? saleItem.Quantity : 1M;
                                        }
                                    }
                                }
                            }
                        }
                        else if (leftToDiscount > 0 && saleItem.NoDiscountAllowed)
                        {
                            saleItem.DiscountUnsuccessfullyApplied = true;
                        }
                    }
                }
            } while (ALLGroupsTriggered(mmGroups)); //&& triedAll == true); //TODO e?a NoOfTimeAppl.

            return retailTransaction;
        }

        private void RegisterMixMatch(IConnectionManager entry, string offerId, string offerName, DataRow activeOffer, DataTable tmpMMOffer, IRetailTransaction retailTransaction)
        {
            bool sameDiffMMlines = Conversion.ToInt(activeOffer["SameDiffMMLines"]) == 1;
            DiscountType discountType = (DiscountType)activeOffer["DiscountType"];
            decimal dealPrice = Conversion.ToDecimal(activeOffer["DealPriceValue"]);
            decimal discountAmount = Conversion.ToDecimal(activeOffer["DiscountAmountValue"]);
            decimal discountDiff = 0M;
            decimal leastExpensiveDiscount = 0M;
            int firstLineInDisc = -1;

            ISettings settings = GetSettings(entry);

            //if the company currency is not the same as the store currency 
            //then convert the unit price to the current store currency.
            if ((string)settings.CompanyInfo.CurrencyCode != settings.Store.Currency)
            {
                dealPrice = Interfaces.Services.CurrencyService(entry).CurrencyToCurrency(
                    entry,
                    settings.CompanyInfo.CurrencyCode,
                    settings.Store.Currency,
                    settings.CompanyInfo.CurrencyCode,
                    dealPrice);

                discountAmount = Interfaces.Services.CurrencyService(entry).CurrencyToCurrency(
                    entry,
                    settings.CompanyInfo.CurrencyCode,
                    settings.Store.Currency,
                    settings.CompanyInfo.CurrencyCode,
                    discountAmount);
            }

            decimal totalAmount = 0;

            List<LeastExpensiveLines> LeastExpLines = new List<LeastExpensiveLines>();

            foreach (DataRow row in tmpMMOffer.Select())
            {
                ISaleLineItem saleItem = retailTransaction.GetItem((Guid)row["SaleLineId"]);
                LeastExpensiveLines expLines = new LeastExpensiveLines();

                switch (retailTransaction.CalculateDiscountFrom)
                {
                    case Store.CalculateDiscountsFromEnum.PriceWithTax:
                        totalAmount += saleItem.PriceWithTax * (decimal)row["ItemsTriggered"];
                        expLines.PriceWithTax = saleItem.PriceWithTax;
                        break;
                    case Store.CalculateDiscountsFromEnum.Price:
                        totalAmount += saleItem.Price * (decimal)row["ItemsTriggered"];
                        expLines.PriceWithTax = saleItem.Price;
                        break;
                }

                expLines.SaleLineId = saleItem.LineId;
                LeastExpLines.Add(expLines);

                //Store the first sale line in the offer to use later on
                if (saleItem.Price != 0)
                {
                    int saleLinePosition = (int)row["SaleLinePosition"];

                    if ((firstLineInDisc == -1) || ((saleLinePosition > 0) && (saleLinePosition < firstLineInDisc)))
                    {
                        firstLineInDisc = saleLinePosition;
                    }
                }
            }

            //Retrieving the original discount difference amount depending on what type of discount it is
            //Used to make sure that the discount is divided between the lines accurately
            if (discountType == DiscountType.DealPrice)
            {
                discountDiff = Math.Abs(totalAmount) - dealPrice;
            }
            else if (discountType == DiscountType.DiscountAmount)
            {
                discountDiff = discountAmount;
            }
            else if (discountType == DiscountType.LeastExpensive)
            {
                leastExpensiveDiscount = GetLeastExpensiveAmount(offerId, (int)activeOffer["NoOfLeastExpensiveItems"], retailTransaction, LeastExpLines);
                discountDiff = leastExpensiveDiscount;
            }

            IRoundingService rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

            foreach (DataRow row in tmpMMOffer.Select())
            {
                ISaleLineItem saleItem = retailTransaction.GetItem((Guid)row["SaleLineId"]);
                saleItem.PeriodicDiscountOfferId = offerId;
                saleItem.PeriodicDiscountOfferName = offerName;
                saleItem.WasChanged = true;

                decimal orgQuantity = saleItem.Quantity;
                saleItem.Quantity = saleItem.QuantityDiscounted;

                decimal percentage = 0;
                decimal amount = 0;

                if (saleItem.Quantity != 0 && totalAmount != 0)
                {

                    decimal calcDiscountFromPrice = decimal.Zero;
                    switch (retailTransaction.CalculateDiscountFrom)
                    {
                        case Store.CalculateDiscountsFromEnum.PriceWithTax:
                            calcDiscountFromPrice = saleItem.PriceWithTax;
                            break;
                        case Store.CalculateDiscountsFromEnum.Price:
                            calcDiscountFromPrice = saleItem.Price;
                            break;
                    }

                    switch (discountType)
                    {
                        case DiscountType.DealPrice:
                            {
                                totalAmount = Math.Abs(totalAmount);
                                percentage = 0;
                                amount = (totalAmount - dealPrice) * Math.Abs((calcDiscountFromPrice * saleItem.QuantityDiscounted / totalAmount));

                                amount = rounding.Round(
                                    entry,
                                    amount / Math.Abs(saleItem.QuantityDiscounted),
                                    settings.Store.Currency,
                                    CacheType.CacheTypeTransactionLifeTime); //Discount amount per pcs.

                                discountDiff -= (amount * Math.Abs(saleItem.QuantityDiscounted));
                                if (saleItem.Quantity < 0)
                                    amount *= -1;
                            }
                            break;
                        case DiscountType.DiscountPct:
                            {
                                percentage = (decimal)activeOffer["DiscountPctValue"] * Math.Abs((saleItem.QuantityDiscounted / (saleItem.Quantity)));
                                amount = 0;
                            }
                            break;
                        case DiscountType.DiscountAmount:
                            {
                                percentage = 0;
                                amount = discountAmount * Math.Abs((calcDiscountFromPrice * saleItem.QuantityDiscounted / totalAmount));
                                amount = rounding.Round(
                                    entry,
                                    amount / Math.Abs(saleItem.QuantityDiscounted),
                                    settings.Store.Currency,
                                    CacheType.CacheTypeTransactionLifeTime); //Discount amount per pcs.
                                discountDiff -= (amount * Math.Abs(saleItem.QuantityDiscounted));
                                if (saleItem.Quantity < 0)
                                    amount *= -1;
                            }
                            break;
                        case DiscountType.LeastExpensive:
                            {
                                percentage = 0;
                                amount = leastExpensiveDiscount * Math.Abs((calcDiscountFromPrice * saleItem.QuantityDiscounted / totalAmount));
                                amount = rounding.Round(
                                    entry,
                                    amount / Math.Abs(saleItem.QuantityDiscounted),
                                    settings.Store.Currency,
                                    CacheType.CacheTypeTransactionLifeTime); //Discount amount per pcs.

                                discountDiff -= (amount * Math.Abs(saleItem.QuantityDiscounted));
                                if (saleItem.Quantity < 0)
                                    amount *= -1;
                            }
                            break;
                        case DiscountType.LineSpecific:
                            {
                                if ((int)row["DiscType"] == (int)DiscountService.DiscountType.DealPrice)
                                {
                                    percentage = 0;
                                    decimal lineSpecDealPrice = (decimal)row["DealPriceOrDiscPct"];

                                    if ((string)settings.CompanyInfo.CurrencyCode != settings.Store.Currency)
                                    {
                                        lineSpecDealPrice = Interfaces.Services.CurrencyService(entry).CurrencyToCurrency(
                                            entry,
                                            settings.CompanyInfo.CurrencyCode,
                                            settings.Store.Currency,
                                            settings.CompanyInfo.CurrencyCode,
                                            lineSpecDealPrice);
                                    }

                                    amount = ((calcDiscountFromPrice * saleItem.QuantityDiscounted) - lineSpecDealPrice);
                                    amount = rounding.Round(
                                        entry,
                                        amount / Math.Abs(saleItem.QuantityDiscounted),
                                        settings.Store.Currency,
                                        CacheType.CacheTypeTransactionLifeTime); //Discount amount per pcs.

                                    if (saleItem.Quantity < 0)
                                        amount *= -1;
                                }
                                else
                                {
                                    percentage = (decimal)row["DealPriceOrDiscPct"] * Math.Abs((saleItem.QuantityDiscounted / (saleItem.Quantity)));
                                    amount = 0;
                                }
                            }
                            break;
                    }

                    PeriodicDiscountItem discountItem = new PeriodicDiscountItem();
                    discountItem.LineDiscountType = DiscountTypes.Periodic;
                    discountItem.PeriodicDiscountType = PeriodicDiscOfferType.MixAndMatch;
                    discountItem.Percentage = percentage;
                    discountItem.SameDifferentMMLines = sameDiffMMlines;
                    discountItem.Amount = amount;
                    saleItem.QuantityDiscounted *= saleItem.Quantity >= 0 ? 1 : -1;

                    UpdatePeriodicDiscountLines(saleItem, discountItem);

                    saleItem.Quantity = orgQuantity;
                    saleItem.ExcludedActions |= DataLayer.BusinessObjects.Transactions.Line.ExcludedActions.PriceOverride;
                }
            }

            int loopCount = 0;

            while ( // If the difference is zero, then looping will discontinue
                (discountDiff != decimal.Zero)
                && // If the difference is less than 0,005, then looping will discontinue
                (rounding.Round(entry, discountDiff, 2, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime) != decimal.Zero)
                && // If looping has been 10 times or more, looping will discontinue
                (loopCount < 10)
                )
            {
                //Update the periodic discount line on the first item in the discount to round out the deal price/disc amount
                ISaleLineItem firstLine = retailTransaction.GetItem(firstLineInDisc);
                PeriodicDiscountItem firstDiscItem = firstLine.DiscountLines.Count <= 0 ? null : (PeriodicDiscountItem)firstLine.DiscountLines[firstLine.DiscountLines.Count - 1];
                if (firstLine != null)
                {
                    if (firstLine.Quantity < 0)
                    {
                        firstDiscItem.Amount -= (discountDiff / firstLine.QuantityDiscounted);
                    }
                    else
                    {
                        firstDiscItem.Amount += (discountDiff / firstLine.QuantityDiscounted);
                    }
                }

                decimal totalDiscountAmt = 0M;

                //Make sure that the deal price difference doesn't still exist after the previous changes.
                foreach (DataRow row in tmpMMOffer.Select())
                {
                    ISaleLineItem saleItem = retailTransaction.GetItem((Guid)row["SaleLineId"]);
                    PeriodicDiscountItem discItem = saleItem.DiscountLines.Count <= 0 ? null : (PeriodicDiscountItem)saleItem.DiscountLines[saleItem.DiscountLines.Count - 1];
                    if (discItem != null)
                    {
                        totalDiscountAmt += (discItem.Amount * saleItem.QuantityDiscounted);
                    }
                }

                if (discountType == DiscountType.DealPrice)
                {
                    discountDiff = (totalAmount - dealPrice - Math.Abs(totalDiscountAmt));
                }
                else if (discountType == DiscountType.DiscountAmount)
                {
                    discountDiff = (discountAmount - totalDiscountAmt);
                }
                else if (discountType == DiscountType.LeastExpensive)
                {
                    discountDiff = Math.Abs(totalDiscountAmt) - leastExpensiveDiscount;
                }
                loopCount++;
            }
        }

        /// <summary>
        /// Returns the discount amount for the least exepnsive items.
        /// </summary>
        /// <param name="retailTransaction">The retailtransaction</param>
        /// <param name="noLeastExpensiveItems">The number of least expensive items that are free</param>
        /// <returns></returns>
        private decimal GetLeastExpensiveAmount(string offerId, int noLeastExpensiveItems, IRetailTransaction retailTransaction, List<LeastExpensiveLines> LeastExpLines)
        {
            decimal discountAmount = 0;

            int items = 0;
            foreach (LeastExpensiveLines expLine in LeastExpLines.OrderBy(expLine => expLine.PriceWithTax))
            {
                ISaleLineItem saleItem = retailTransaction.GetItem(expLine.SaleLineId);
                if (Math.Abs(saleItem.Quantity) + items <= noLeastExpensiveItems)
                {
                    items += Math.Abs((int)saleItem.Quantity);

                    switch (retailTransaction.CalculateDiscountFrom)
                    {
                        case Store.CalculateDiscountsFromEnum.PriceWithTax:
                            discountAmount += Math.Abs(saleItem.Quantity) * saleItem.PriceWithTax;
                            break;
                        case Store.CalculateDiscountsFromEnum.Price:
                            discountAmount += Math.Abs(saleItem.Quantity) * saleItem.Price;
                            break;
                    }
                }
                else
                {
                    switch (retailTransaction.CalculateDiscountFrom)
                    {
                        case Store.CalculateDiscountsFromEnum.PriceWithTax:
                            discountAmount += (noLeastExpensiveItems - items) * saleItem.PriceWithTax;
                            break;
                        case Store.CalculateDiscountsFromEnum.Price:
                            discountAmount += (noLeastExpensiveItems - items) * saleItem.Price;
                            break;
                    }
                    items += noLeastExpensiveItems;
                }

                if (items == noLeastExpensiveItems)
                {
                    return discountAmount;
                }
            }

            return discountAmount;
        }

        private bool ALLGroupsTriggered(Dictionary<MixAndMatchLineGroup, decimal> mmLineGroups)
        {
            //The Mix&Match cannot be triggered unless all the items have negative or positive qty.
            //If one of the items is being returned and the other sold - then the M&M should not be triggered
            if (mmLineGroups.All(a => a.Value > 0))
            {
                return mmLineGroups.All(keyValuePair => keyValuePair.Key.NumberOfItemsNeeded <= keyValuePair.Value);
            }
            else if (mmLineGroups.All(a => a.Value < 0))
            {
                return mmLineGroups.All(keyValuePair => keyValuePair.Key.NumberOfItemsNeeded <= Math.Abs(keyValuePair.Value));
            }

            return false;
        }

        /// <summary>
        /// Used to split a item line if needed in a periodicdiscount.  A splitting of a line is needed if part of the 
        /// quantity has been used in another (mix&match)offer.
        /// </summary>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <param name="lineId">The id of the line that will be splited.</param>
        /// <param name="qtyNewLine">The quantity that will be taken taken from one line and put into a new line</param>
        private DataRow SplitLine(ref IRetailTransaction retailTransaction, int lineId, decimal qtyNewLine)
        {
            if (qtyNewLine == 0M)
            {
                return null;
            }

            bool newLineAdded = false;
            string offerId = "";
            Guid newLineID = Guid.Empty;
            int newLinePosition = -1;

            //Create a list for item?s to be removed
            LinkedList<SaleLineItem> newSaleLinesList;
            newSaleLinesList = new LinkedList<SaleLineItem>();

            foreach (SaleLineItem saleItem in retailTransaction.SaleItems)
            {
                if (saleItem.LineId == lineId)
                {
                    //Create the dublicate sale line                    
                    SaleLineItem newSaleItem1 = new SaleLineItem(retailTransaction);
                    newSaleItem1 = saleItem.Clone(retailTransaction);
                    newSaleItem1.ID = Guid.NewGuid();
                    if (saleItem.Quantity > 0)
                    {
                        newSaleItem1.Quantity = qtyNewLine;
                        newSaleItem1.UnitQuantity = qtyNewLine;
                    }
                    else
                    {
                        newSaleItem1.Quantity = -qtyNewLine;
                        newSaleItem1.UnitQuantity = -qtyNewLine;
                    }
                    newSaleItem1.QuantityDiscounted = 0;                    
                    newSaleItem1.DiscountLines.Clear();
                    offerId = saleItem.PeriodicDiscountOfferId;
                    newSaleItem1.BlockDiscountLinkItem = true;
                    newSaleLinesList.AddLast(newSaleItem1);
                    newLineID = newSaleItem1.ID;
                    newLineAdded = true;

                    //Set the new quantity on the orgininal sale line item
                    if(saleItem.Quantity > 0)
                    {
                        saleItem.Quantity += -qtyNewLine;
                        saleItem.UnitQuantity += -qtyNewLine;
                    }
                    else
                    {
                        saleItem.Quantity += qtyNewLine;
                        saleItem.UnitQuantity += qtyNewLine;
                    }

                }
            }

            foreach (SaleLineItem item in newSaleLinesList)
            {
                if (item.IsLinkedItem && item.LinkedToLineId != -1)
                {
                    ISaleLineItem lastItem = retailTransaction.SaleItems.LastOrDefault(p => p.LinkedToLineId == item.LinkedToLineId);
                    if (lastItem != null)
                    {
                        newLinePosition = lastItem.LineId + 1;
                        retailTransaction.Insert(lastItem, item);
                    }
                    else
                    {                        
                        retailTransaction.Add(item);
                        newLinePosition = retailTransaction.SaleItems.Count;
                    }
                }
                else
                {
                    retailTransaction.Add(item);
                    newLinePosition = retailTransaction.SaleItems.Count;
                }
            }

            DataRow offerLineRow = null;

            //Refresh the offer information after adding a sales line
            if (newLineAdded)
            {
                string filterExpr = "SaleLinePosition ='" + lineId.ToString() + "' AND OfferId='" + offerId + "'";

                foreach (DataRow row in activeOfferLines.Select(filterExpr))
                {
                    offerLineRow = activeOfferLines.NewRow();
                    offerLineRow["OfferId"] = row["OfferId"];
                    offerLineRow["LineId"] = row["LineId"];
                    offerLineRow["Id"] = row["Id"];                                 //ItemID or GroupId
                    offerLineRow["SaleLinePosition"] = newLinePosition;
                    offerLineRow["SaleLineId"] = newLineID;
                    offerLineRow["ProductType"] = row["ProductType"];
                    offerLineRow["Quantity"] = qtyNewLine;
                    offerLineRow["DealPriceOrDiscPct"] = row["DealPriceOrDiscPct"];
                    offerLineRow["LineGroup"] = row["LineGroup"];
                    offerLineRow["DiscType"] = row["DiscType"];
                    offerLineRow["Status"] = row["Status"];
                    offerLineRow["NoOfItemsNeeded"] = row["NoOfItemsNeeded"];
                    offerLineRow["MixMatchPriority"] = row["MixMatchPriority"];
                    activeOfferLines.Rows.Add(offerLineRow);
                }

                //Doing the same for the tmpMMOffer rows
                foreach (DataRow row in tmpMMOffer.Select("", "SaleLinePosition DESC"))
                {
                    if ((int)row["SaleLinePosition"] == lineId)
                    {
                        DataRow tmpMMRow;
                        tmpMMRow = tmpMMOffer.NewRow();
                        tmpMMRow["SaleLinePosition"] = newLinePosition;
                        tmpMMRow["SaleLineId"] = newLineID;
                        tmpMMRow["ItemsTriggered"] = row["ItemsTriggered"];
                        tmpMMRow["DiscType"] = row["DiscType"];
                        tmpMMRow["DealPriceOrDiscPct"] = row["DealPriceOrDiscPct"];
                        tmpMMOffer.Rows.Add(tmpMMRow);
                        break;
                    }
                }
            }

            return offerLineRow;
        }

        /// <summary>
        /// Update the periodic discount items.
        /// </summary>
        /// <param name="saleItem">The item line that the discount line is added to.</param>
        /// <param name="discountItem">The new discount item</param>
        private void UpdatePeriodicDiscountLines(ISaleLineItem saleItem, PeriodicDiscountItem discountItem)
        {
            //Check if line discount is found, if so then update
            bool discountLineFound = false;
            foreach (DiscountItem discountLine in saleItem.DiscountLines)
            {
                if (discountLine is PeriodicDiscountItem)
                {
                    //If found then update
                    PeriodicDiscountItem periodicDiscLine = (PeriodicDiscountItem)discountLine;
                    if ((periodicDiscLine.LineDiscountType == discountItem.LineDiscountType) &&
                        (periodicDiscLine.PeriodicDiscountType == discountItem.PeriodicDiscountType))
                    {
                        periodicDiscLine.Percentage = discountItem.Percentage;
                        periodicDiscLine.Amount = discountItem.Amount;
                        discountLineFound = true;
                    }
                }
            }
            //If line discount is not found then add it.
            if (!discountLineFound)
            {
                saleItem.Add(discountItem);
            }
        }
        #endregion PeriodicDiscount

        public IErrorLog ErrorLog
        {
            set
            {

            }
        }

        public void Init(IConnectionManager entry)
        {
#pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
#pragma warning restore 0612, 0618

            discountAndPriceActivation = Providers.DiscountAndPriceActivationData.Get(entry) ?? new DiscountAndPriceActivation();

            MakeMultiLineDiscTable();       //Table for customer multiline discount calculation
            MakeActiveOfferTables();        //Tables for periodic discount calculation
            MakeTmpOfferTable();            //Temporary table used to for mix match calculation            

            periodicDiscountData = new List<PeriodicDiscount>();

            periodicDiscountCacheReset = DateTime.Now;

            // Register local data providers
            DataProviderFactory.Instance.Register<IDiscountData, DiscountData, DataEntity>();
        }
    }
}