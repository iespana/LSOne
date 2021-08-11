using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects.DiscountItems;
using LSOne.DataLayer.TransactionObjects.Line.Loyalty;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.POS.Processes.Common;
using LSOne.Services.Dialogs;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Properties;
using LSOne.Controls.SupportClasses;

namespace LSOne.Services
{
    /// <summary>
    /// The Loyalty service calculates loyalty points and authorizes loyalty card payments. 
    /// Possible customization includes a calculation of awarded loyalty points, rounding and local/external loyalty payment authorization.     
    /// </summary>
    public partial class LoyaltyService : ILoyaltyService
    {
        private CalculationHelper calculationHelper;
        private LoyaltyPointStatus pointStatus;
    
        public LoyaltyService()
        {
            calculationHelper = null;
            pointStatus = new LoyaltyPointStatus();
        }

        public virtual IErrorLog ErrorLog
        {
            set
            {
            }
        }

        public void Init(IConnectionManager entry)
        {
            #pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
            DLLEntry.Settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            #pragma warning restore 0612, 0618
        }

        #region ILoyalty Members

        /// <summary>
        /// This function adds a loyalty card and the customer attached to the loyalty card to the transaction. 
        /// This function also calculates the points that are on the transaction (if the loyalty card was added 
        /// after some items were added to the sale). 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cardInfo">Loyalty card information to be added to the transaction</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <returns><c>true</c> if the card is not added to the transaction, <c>false</c> otherwise</returns>
        public virtual bool AddLoyaltyCardToTransaction(IConnectionManager entry, CardInfo cardInfo, IRetailTransaction retailTransaction)
        {            
            try
            {
                pointStatus.Clear();
                
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Adding a loyalty record to the transaction...", "Loyalty.AddLoyaltyItem");

                UpdateCalculatePoints(retailTransaction, LoyaltyPointsRelation.Item);

                if (retailTransaction.SaleIsReturnSale && retailTransaction.LoyaltyItem.Empty)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.LoyaltyCardCannotBeAddedToReturnSale);
                    return false;
                }

                if (!retailTransaction.CustomerOrder.Empty() && retailTransaction.CustomerOrder.OrderType == CustomerOrderType.Quote )
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(string.Format(Resources.LoyaltyCardCannotBeAddedToAQuote, retailTransaction.CustomerOrder.OrderType == CustomerOrderType.Quote ? Resources.QuoteLowCase : Resources.CustomerOrderLowCase));
                    return false;
                }

                // Add the loyalty item to the transaction
                GetLoyaltyInfoForTransaction(entry, cardInfo, retailTransaction, cardInfo == null || cardInfo.CardNumber == "", true);

                if (!retailTransaction.LoyaltyItem.Empty)
                {
                    calculationHelper.AddCustomerToTransaction(entry, retailTransaction);
                    return true;
                }
                
                return false;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        /// <summary>
        /// Displays a dialog where the user must enter a loyalty card number and information about the loyalty card is retrieved including point status, 
        /// point value and etc. It depends on the context of what operation and/or functionality is calling this function how the dialog is displayed. 
        /// The same dialog is used to view loyalty card status, pay with loyalty cards and add a loyalty point discount.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="cardInfo"></param>
        /// <param name="retailTransaction"></param>
        public virtual void GetLoyaltyCardInfo(IConnectionManager entry, CardInfo cardInfo, IRetailTransaction retailTransaction)
        {
            try
            {
                pointStatus.Clear();

                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Retrieving loyalty information", "Loyalty.LoyaltyCardInfo");

                // Add the loyalty item to the transaction
                GetLoyaltyInfoForTransaction(entry, cardInfo, retailTransaction, true, false);
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        protected virtual decimal GetLoyaltyInfoForPayment(
            IConnectionManager entry, 
            CardInfo cardInfo, 
            IRetailTransaction retailTransaction, 
            decimal balanceAmount, 
            RecordIdentifier tenderTypeID, 
            UseDialogEnum useDlg)
        {
            return GetLoyaltyItem(entry, cardInfo, retailTransaction, balanceAmount, tenderTypeID, true, true, useDlg);
        }

        protected virtual decimal GetLoyaltyInfoForTransaction(
            IConnectionManager entry, 
            CardInfo cardInfo, 
            IRetailTransaction retailTransaction, 
            bool displayDialog, 
            bool displayOKCancel)
        {
            return GetLoyaltyItem(entry, cardInfo, retailTransaction, decimal.Zero, RecordIdentifier.Empty, displayDialog, displayOKCancel, UseDialogEnum.GetInformation);
        }

        /// <summary>
        /// Gets information about a loyalty card
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cardInfo">Loyalty card information</param>
        /// <param name="displayDialog">If true the Get loyalty card info dialog is displayed. If false the loyalty information is retrieved without displaying the dialog</param>
        /// <param name="displayOKCancel"></param>
        /// <param name="useDlg"></param>
        /// <returns>The LoyaltyItem found</returns>
        protected virtual decimal GetLoyaltyItem(IConnectionManager entry, CardInfo cardInfo, IRetailTransaction retailTransaction, decimal balanceAmount, RecordIdentifier tenderTypeID, 
            bool displayDialog, bool displayOKCancel, UseDialogEnum useDlg)
        {
            decimal toReturn = decimal.Zero;

            bool getLoyaltyInfo = true;

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (cardInfo == null || displayDialog)
            {
                cardInfo = cardInfo ?? new CardInfo();
                GetLoyaltyCardInfo dlg = new GetLoyaltyCardInfo(entry, cardInfo, balanceAmount, tenderTypeID, displayOKCancel, useDlg, retailTransaction);
                
                DialogResult result = dlg.ShowDialog(settings.POSApp.POSMainWindow);

                if (result == DialogResult.Cancel)
                {                    
                    return toReturn;                    
                }

                if (result == DialogResult.Abort)
                {
                    return decimal.MaxValue; // the user selected to clear all the discounts
                }

                pointStatus = dlg.PointStatus;
                
                calculationHelper.GetLoyaltyInfo(entry, retailTransaction.LoyaltyItem);
                
                getLoyaltyInfo = !dlg.LoyaltyInfoRetrieved;
                toReturn = dlg.PaidAmount;
            }

            if (getLoyaltyInfo)
            {
                pointStatus.ResultCode = LoyaltyCustomer.ErrorCodes.UnknownError;
                pointStatus.CardNumber = cardInfo.CardNumber;
                
                GetPointStatus(entry);

                if (pointStatus.ResultCode == LoyaltyCustomer.ErrorCodes.NoErrors)
                {
                    retailTransaction.LoyaltyItem.AccumulatedPoints = pointStatus.Points;
                    retailTransaction.LoyaltyItem.CurrentValue = pointStatus.CurrentValue;
                    retailTransaction.LoyaltyItem.CardNumber = (string)pointStatus.CardNumber;
                    calculationHelper.GetLoyaltyInfo(entry, retailTransaction.LoyaltyItem);
                }
            }

            POSFormsManager.ShowPOSStatusBarInfo(Resources.CardID + " " + retailTransaction.LoyaltyItem.CardNumber, null, TaskbarSection.Message);

            return toReturn;
        }

        /// <summary>
        /// Gets the point status using the Store Server
        /// </summary>
        protected virtual void GetPointStatus(IConnectionManager entry)
        {
            try
            {
                try
                {
                    Interfaces.Services.DialogService(entry).UpdateStatusDialog(Resources.RetrievingLoyaltyStatus);

                    ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                    ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                    pointStatus = service.GetLoyaltyPointsStatus(entry, settings.SiteServiceProfile, pointStatus);
                }
                finally
                {
                    Interfaces.Services.DialogService(entry).CloseStatusDialog();
                }
            }
            catch (Exception)
            {
                pointStatus.ResultCode = LoyaltyCustomer.ErrorCodes.UnknownError;
            }

            if (pointStatus.ResultCode != 0)
            {
                pointStatus.Comment = (pointStatus.Comment == "") ? Resources.ErrorConnectingToLoyaltyServer : pointStatus.Comment;
                Interfaces.Services.DialogService(entry).ShowMessage(pointStatus.Comment, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
        }

        /// <summary>                
        /// This function calculates the points on each item and the total number of points on the transaction.        
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        public virtual void AddLoyaltyPoints(IConnectionManager entry, IRetailTransaction retailTransaction)
        {            
            try
            {
                pointStatus.Clear();

                //If any loyalty tenderlines are on the transaction then no points should be added
                if (retailTransaction.TenderLines.Count(c => c is LoyaltyTenderLineItem) > 0)
                {
                    return;
                }

                if (retailTransaction.Training)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.NoLoyaltyPointsOnTraining);
                    entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Transaction is a training transaction - no loyalty points added to card", "Loyalty.AddLoyaltyPoints");
                    return;
                }
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Adding loyalty points...", "Loyalty.AddLoyaltyPoints");                

                CalculateLoyaltyPoints(entry, retailTransaction);
            }
            catch(Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            } 
        }
        
        protected virtual bool PaymentLineAllowed(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            if (retailTransaction.TenderLines.Count(c => !c.Voided && c is LoyaltyTenderLineItem) == 0)
            {
                return true;
            }

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            decimal previousPaidAmt = retailTransaction.TenderLines.Where(w => w is LoyaltyTenderLineItem && w.Voided == false).Sum(s => s.Amount);

            LoyaltyMSRCard card = calculationHelper.GetLoyaltyInfo(entry, retailTransaction.LoyaltyItem);

            decimal maxPayment = (retailTransaction.NetAmountWithTax * card.UsePointsLimit / 100);

            maxPayment = Interfaces.Services.RoundingService(entry).Round(entry, maxPayment, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);

            if (card.UsePointsLimit > decimal.Zero && (Math.Abs(previousPaidAmt) >= Math.Abs(maxPayment)))
            {
                return false;
            }
            if (retailTransaction.Customer != null && retailTransaction.Customer.Blocked != BlockedEnum.Nothing)
            {
                return false;
            }

            return true;
        }

        /// <summary>        
        /// Retrieves loyalty card information from the user and the number of points that should be used for payment. 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cardInfo">The card info</param>
        /// <param name="amount">The amount</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <returns>TenderLineItem.</returns>
        public virtual ITenderLineItem AddLoyaltyPayment(IConnectionManager entry, CardInfo cardInfo, decimal amount, IRetailTransaction retailTransaction, RecordIdentifier tenderTypeID)
        {
            TenderLineItem tenderItem = null;
            try
            {
                pointStatus.Clear();

                calculationHelper = null;
                UpdateCalculatePoints(retailTransaction, LoyaltyPointsRelation.Tender);

                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                if (retailTransaction.SaleIsReturnSale && retailTransaction.LoyaltyItem.Empty)
                {
                    if (retailTransaction.Payment == 0)
                        Interfaces.Services.DialogService(entry).ShowMessage(Resources.ConcludeTransactionWithCash);
                    else
                        Interfaces.Services.DialogService(entry).ShowMessage(Resources.LoyaltyCardCannotBeAddedToReturnSale);
                    return null;
                }

                if (retailTransaction.SaleItems.Count(c => c.DiscountLines.Any(b => b.DiscountType == DiscountTransTypes.LoyaltyDisc)) > 0)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.TransactionAlreadyHasLoyaltyPoints);
                    return null;
                }

                if (!PaymentLineAllowed(entry, retailTransaction))
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.NoMoreLoyaltyPaymentLinesAllowed);
                    return null;
                }

                cardInfo = cardInfo ?? (new CardInfo());

                decimal paidAmount = GetLoyaltyInfoForPayment(entry, cardInfo, retailTransaction, amount, tenderTypeID, UseDialogEnum.PointsPayment);


                if (!retailTransaction.LoyaltyItem.Empty && paidAmount != decimal.Zero)
                {
                    cardInfo.TenderTypeId = tenderTypeID;
                    calculationHelper.AddCustomerToTransaction(entry, retailTransaction);

                    // If the paid amount is higher than the "new" NetAmountWithTax, then it is acceptable to lower the paid amount
                    if (paidAmount > retailTransaction.TransSalePmtDiff)
                    {
                        paidAmount = retailTransaction.TransSalePmtDiff;
                    }

                    decimal totalNumberOfPoints = 0;

                    LoyaltyItem calculatedPoints = calculationHelper.GetPointsForAllLoyaltyTenderLines(entry, ref totalNumberOfPoints, tenderTypeID, paidAmount);

                    if (calculationHelper.TenderRuleFound)
                    {
                        GetPointStatus(entry);

                        if ((pointStatus.ResultCode == LoyaltyCustomer.ErrorCodes.NoErrors) &&
                            (pointStatus != null && pointStatus.LoyaltyTenderType != LoyaltyMSRCard.TenderTypeEnum.NoTender))
                        {
                            if (pointStatus.Points >= (paidAmount < decimal.Zero ? totalNumberOfPoints*-1 : totalNumberOfPoints))
                            {
                                // If this is a return payment then both paidAmount and CalculatedPoints need to be set as negative numbers
                                if (retailTransaction.TransSalePmtDiff < decimal.Zero)
                                {
                                    paidAmount *= -1;
                                    calculatedPoints.CalculatedPoints *= -1;
                                }

                                // If so create a loyalty card payment with the card number
                                tenderItem = new LoyaltyTenderLineItem();
                                ((LoyaltyTenderLineItem) tenderItem).CardNumber = retailTransaction.LoyaltyItem.CardNumber;

                                // Gathering tender information
                                StorePaymentMethod tenderInfo = Providers.StorePaymentMethodData.Get(entry, new RecordIdentifier(entry.CurrentStoreID,cardInfo.TenderTypeId), CacheType.CacheTypeApplicationLifeTime);
                                tenderItem.Amount = paidAmount;

                                tenderItem.Description = tenderInfo.Text;
                                tenderItem.TenderTypeId = (string) cardInfo.TenderTypeId;
                                ((LoyaltyTenderLineItem) tenderItem).Points = calculatedPoints.CalculatedPoints;
                                tenderItem.LoyaltyPoints = (LoyaltyItem) calculatedPoints.Clone();
                                tenderItem.OpenDrawer = tenderItem.OpenDrawer;

                                // Convert from the store-currency to the company-currency...
                                tenderItem.CompanyCurrencyAmount = Interfaces.Services.CurrencyService(entry).CurrencyToCurrency(
                                    entry, settings.Store.Currency, settings.CompanyInfo.CurrencyCode, settings.CompanyInfo.CurrencyCode, paidAmount);

                                // The exchange rate between the store and the company currency
                                tenderItem.ExchrateMST = Interfaces.Services.CurrencyService(entry).ExchangeRate(entry, settings.Store.Currency)*100;

                                retailTransaction.Add(tenderItem);
                            }
                            else
                            {
                                Interfaces.Services.DialogService(entry).ShowMessage(Resources.NotEnoughPointsOnCard, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                            }
                        }
                        else
                        {
                            pointStatus.Comment = pointStatus.LoyaltyTenderType == LoyaltyMSRCard.TenderTypeEnum.NoTender ? 
                                Resources.PaymentNotAllowedWithCard : 
                                (pointStatus.Comment == "") ? Resources.ErrorConnectingToLoyaltyServer : pointStatus.Comment;

                            Interfaces.Services.DialogService(entry).ShowMessage(pointStatus.Comment,
                                MessageBoxButtons.OK,
                                pointStatus.LoyaltyTenderType == LoyaltyMSRCard.TenderTypeEnum.NoTender ? MessageDialogType.Generic : MessageDialogType.ErrorWarning);
                        }
                    }
                    else
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(Resources.NoPointDeductionRuleExists, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    }
                }
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }

            return tenderItem;
        }

        protected virtual void UpdateCalculatePoints(IRetailTransaction retailTransaction, LoyaltyPointsRelation loyaltyUsage)
        {
            if (calculationHelper == null)
            {
                calculationHelper = new CalculationHelper(retailTransaction, loyaltyUsage == LoyaltyPointsRelation.Tender ? UseDialogEnum.PointsPayment : loyaltyUsage == LoyaltyPointsRelation.Discount ? UseDialogEnum.PointsDiscount :  UseDialogEnum.CardRequest);
                return;
            }

            calculationHelper.Transaction = retailTransaction;
        }

        /// <summary>
        /// Voids the loyalty points tender line and clears the point status on the tender line. The actual point status will not be updated
        /// untill the sale is concluded
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cardInfo">The card info.</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <param name="tenderLine">The tenderline being voided</param>
        public virtual void VoidLoyaltyPayment(IConnectionManager entry, CardInfo cardInfo, IRetailTransaction retailTransaction, ILoyaltyTenderLineItem tenderLine)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Voiding loyalty payment...", "Loyalty.VoidLoyaltyPayment");

                pointStatus.Clear();

                retailTransaction.VoidPaymentLine(tenderLine.LineId);
                tenderLine.Points = decimal.Zero;
                tenderLine.LoyaltyPoints.Clear();
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        #endregion

        protected virtual void CalculateLoyaltyPoints(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            try
            {
                if (retailTransaction.SaleItems.Any(p => p.Quantity == 0))
                {
                    return;
                }

                decimal totalNumberOfPoints = 0;
                // Get the table containing the point logic
                List<LoyaltyPoints> loyaltyPoints = Providers.LoyaltyPointsData.GetList(entry, retailTransaction.LoyaltyItem.SchemeID);

                // Loop through the transaction and calculate the acquired loyalty points. 
                if (loyaltyPoints != null)
                {
                    totalNumberOfPoints = GetTransactionPoints(entry, retailTransaction, loyaltyPoints);
                }
                
                retailTransaction.LoyaltyItem.CalculatedPoints = totalNumberOfPoints;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }
       
        protected virtual decimal GetPoints(IConnectionManager entry, IRetailTransaction retailtransaction, LoyaltyPoints points)
        {
            try
            {
                switch (points.Type)
                {
                    case LoyaltyPointTypeBase.Item:
                        {
                            foreach (SaleLineItem saleLineItem in retailtransaction.SaleItems.Where(w => w.LoyaltyPoints.RuleID == RecordIdentifier.Empty && !w.Voided && w.Quantity > 0))
                            {
                                if (saleLineItem.ItemId.Equals(points.SchemeRelation.ToString(), StringComparison.CurrentCultureIgnoreCase))
                                {
                                    saleLineItem.LoyaltyPoints = SetLoyaltyItem(entry, retailtransaction, saleLineItem, points);
                                }
                            }
                            break;
                        }
                    case LoyaltyPointTypeBase.Tender:
                        {
                            foreach (TenderLineItem tenderLineItem in retailtransaction.TenderLines.Where(w => w.LoyaltyPoints.RuleID == RecordIdentifier.Empty && !w.Voided))
                            {
                                if (tenderLineItem.TenderTypeId.Equals(points.SchemeRelation.ToString(), StringComparison.CurrentCultureIgnoreCase))
                                {
                                    tenderLineItem.LoyaltyPoints = SetLoyaltyItem(entry, retailtransaction, tenderLineItem, points);
                                }
                            }
                            break;
                        }
                    case LoyaltyPointTypeBase.Discount:
                        {
                            break;
                        }
                    case LoyaltyPointTypeBase.Promotion:
                        {
                            break;
                        }
                    case LoyaltyPointTypeBase.RetailDepartment:
                        {
                            foreach (SaleLineItem saleLineItem in retailtransaction.SaleItems.Where(w => w.LoyaltyPoints.RuleID == RecordIdentifier.Empty && !w.Voided))
                            {
                                if (saleLineItem.ItemDepartmentId.Equals(points.SchemeRelation.ToString(), StringComparison.CurrentCultureIgnoreCase))
                                {
                                    saleLineItem.LoyaltyPoints = SetLoyaltyItem(entry, retailtransaction, saleLineItem, points);
                                }
                            }
                            break;
                        }
                    case LoyaltyPointTypeBase.RetailGroup:
                        {
                            foreach (SaleLineItem saleLineItem in retailtransaction.SaleItems.Where(w => w.LoyaltyPoints.RuleID == RecordIdentifier.Empty && !w.Voided && w.Quantity > 0))
                            {
                                if (saleLineItem.RetailItemGroupId.Equals(points.SchemeRelation.ToString(), StringComparison.CurrentCultureIgnoreCase))
                                {
                                    saleLineItem.LoyaltyPoints = SetLoyaltyItem(entry, retailtransaction, saleLineItem, points);
                                }
                            }
                            break;
                        }
                }

                if (points.QtyAmountLimit > 0)
                {
                    decimal itemPoints = retailtransaction.SaleItems.Where(w => w.LoyaltyPoints.RuleID == points.RuleID && w.LoyaltyPoints.PointsAdded)
                                                                    .Sum(s => s.LoyaltyPoints.CalculatedPoints);

                    decimal tenderPoints = retailtransaction.TenderLines.Where(w => w.LoyaltyPoints.RuleID == points.RuleID && w.LoyaltyPoints.PointsAdded)
                                                                        .Sum(s => s.LoyaltyPoints.CalculatedPoints);

                    return itemPoints + tenderPoints;
                }

                return 0;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        protected virtual LoyaltyItem SetLoyaltyItem(IConnectionManager entry, IRetailTransaction retailTransaction, LoyaltyPoints points, decimal quantity, decimal amount, LoyaltyPointsRelation relation)
        {
            UpdateCalculatePoints(retailTransaction, relation);

            LoyaltyItem calculatedPoints = (LoyaltyItem)retailTransaction.LoyaltyItem.CopyForPointsRelation(relation);

            if (points.QtyAmountLimit > 0)
            {
                // In previous implementations the user was allowed to enter both negative and positive points values in rules setup
                // Current SM implementation doesn't allow this, but the data could possibly still be like this so here we need to make sure the points are positive.
                points.Points = Math.Abs(points.Points);

                if ((Math.Abs(amount) >= points.QtyAmountLimit) && (points.BaseCalculationOn == CalculationTypeBase.Amounts))
                {
                    calculationHelper.SetLoyaltyPoints(entry, calculatedPoints, amount/points.QtyAmountLimit, points, quantity);
                    return calculatedPoints;
                }

                if ((Math.Abs(quantity) >= points.QtyAmountLimit) && (points.BaseCalculationOn == CalculationTypeBase.Quantity))
                {
                    calculationHelper.SetLoyaltyPoints(entry, calculatedPoints, quantity/points.QtyAmountLimit, points, quantity);
                    return calculatedPoints;
                }
            }

            return calculatedPoints;
        }
        
        protected virtual LoyaltyItem SetLoyaltyItem(IConnectionManager entry, IRetailTransaction transaction, ITenderLineItem tenderLineItem, LoyaltyPoints points)
        {
            bool aggregateTenders = false;
            decimal totalAmount = tenderLineItem.Amount;

            decimal noOfTenderLines = transaction.TenderLines.Count(c => c.TenderTypeId == tenderLineItem.TenderTypeId && c.Voided == false);

            if (noOfTenderLines > 1)
            {
                aggregateTenders = true;
                totalAmount = transaction.TenderLines.Where(w => w.TenderTypeId == tenderLineItem.TenderTypeId && w.Voided == false).Sum(s => s.Amount);
            }

            LoyaltyItem calculatedPoints = SetLoyaltyItem(entry, transaction, points, decimal.Zero, totalAmount, LoyaltyPointsRelation.Tender);

            if (!aggregateTenders)
            {
                return calculatedPoints;
            }

            foreach (TenderLineItem lineItem in transaction.TenderLines.Where(w => w.TenderTypeId == tenderLineItem.TenderTypeId && w.Voided == false && w.LineId != tenderLineItem.LineId))
            {
                lineItem.LoyaltyPoints.CalculatedPoints = decimal.Zero;
                lineItem.LoyaltyPoints.RuleID = points.RuleID;
                lineItem.LoyaltyPoints.AggregatedItemQuantity = noOfTenderLines;
                lineItem.LoyaltyPoints.PartOfAggregatedItemPoints = true;
            }

            return calculatedPoints;
        }

        protected virtual LoyaltyItem SetLoyaltyItem(IConnectionManager entry, IRetailTransaction transaction, ISaleLineItem saleLineItem, LoyaltyPoints points)
        {
            bool aggregateItems = false;
            decimal totalQty = saleLineItem.Quantity;
            decimal totalAmt = transaction.CalculateDiscountFrom == Store.CalculateDiscountsFromEnum.PriceWithTax ? saleLineItem.NetAmountWithTax : saleLineItem.NetAmount;

            if (transaction.SaleItems.Count(c => c.ItemId == saleLineItem.ItemId && c.Voided == false) > 1)
            {
                aggregateItems = true;
                totalQty = transaction.SaleItems.Where(w => w.ItemId == saleLineItem.ItemId && w.Voided == false).Sum(s => s.Quantity);
                totalAmt = transaction.SaleItems.Where(w => w.ItemId == saleLineItem.ItemId && w.Voided == false).Sum(s => transaction.CalculateDiscountFrom == Store.CalculateDiscountsFromEnum.PriceWithTax ?  s.NetAmountWithTax : saleLineItem.NetAmount);
            }
            
            LoyaltyItem calculatedPoints = SetLoyaltyItem(entry, transaction, points, totalQty, totalAmt, LoyaltyPointsRelation.Item);

            if (!aggregateItems)
            {
                return calculatedPoints;
            }

            foreach (SaleLineItem sli in transaction.SaleItems.Where(w => w.ItemId == saleLineItem.ItemId && w.Voided == false && w.LineId != saleLineItem.LineId))
            {
                sli.LoyaltyPoints.CalculatedPoints = decimal.Zero;
                sli.LoyaltyPoints.RuleID = points.RuleID;
                sli.LoyaltyPoints.AggregatedItemQuantity = decimal.Zero;
                sli.LoyaltyPoints.PartOfAggregatedItemPoints = true;
            }

            return calculatedPoints;
        }

        protected virtual decimal GetTransactionPoints(IConnectionManager entry, IRetailTransaction transaction, List<LoyaltyPoints> loyaltyPointsList)
        {
            decimal totalCollectedPoints = 0;

            foreach (LoyaltyPoints points in loyaltyPointsList)
            {
                Date nowDate = Date.Now;
                Date emptyDate = Date.Empty; //new Date(1900, 1, 1); //01.01.1900, is an empty date             

                if ((points.StartingDate.DateTime.Date <= nowDate.DateTime.Date) && (nowDate.DateTime.Date <= points.EndingDate.DateTime.Date)
                    || ((points.StartingDate.DateTime.Date <= nowDate.DateTime.Date) && (points.EndingDate.DateTime.Date == emptyDate.DateTime.Date))
                    || ((points.StartingDate.DateTime.Date == emptyDate.DateTime.Date) && (points.EndingDate.DateTime.Date == emptyDate.DateTime.Date))
                   )
                {
                    totalCollectedPoints += GetPoints(entry, transaction, points);
                }
            }

            return totalCollectedPoints;
        }

        /// <summary>
        /// Updates the central database with the loyalty points that have been issued (accrued) during the sale or when a sale with a loyalty points tender line is being returned.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="loyaltyItem">The current loyalty information</param>
        /// <param name="lineID"></param>
        /// <param name="cardNumber">The loyalty card number</param>
        /// <param name="calculatedPoints">How many points were issued</param>
        public virtual void UpdateIssuedLoyaltyPoints(IConnectionManager entry, IRetailTransaction retailTransaction, ILoyaltyItem loyaltyItem, decimal lineID, RecordIdentifier cardNumber, decimal calculatedPoints)
        {
            if (retailTransaction.Training)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.NoLoyaltyPointsOnTraining);
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Transaction is a training transaction - no loyalty points issued", "Loyalty.UpdateIssuedLoyaltyPoints");
                return;
            }

            pointStatus.Clear();

            LoyaltyCustomer.ErrorCodes valid = LoyaltyCustomer.ErrorCodes.NoErrors;
            string comment = "";
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            try
            {
                try
                {
                    Interfaces.Services.DialogService(entry).UpdateStatusDialog(Properties.Resources.UpdatingIssuedLoyaltyPoints);

                    pointStatus.CardNumber = retailTransaction.LoyaltyItem.CardNumber;

                    ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                    service.UpdateIssuedLoyaltyPoints(
                        entry,
                        settings.SiteServiceProfile,
                        ref valid,
                        ref comment,
                        retailTransaction.TransactionId,
                        lineID,
                        (string)cardNumber,
                        retailTransaction.BeginDateTime,
                        calculatedPoints,
                        retailTransaction.ReceiptId);

                    pointStatus.ResultCode = valid;
                    pointStatus.Comment = comment;

                    if (valid == LoyaltyCustomer.ErrorCodes.NoErrors)
                    {
                        GetPointStatus(entry);
                        Interfaces.Services.DialogService(entry).UpdateStatusDialog(Resources.UpdatingIssuedLoyaltyPoints);
                        if (valid == LoyaltyCustomer.ErrorCodes.NoErrors)
                        {
                            loyaltyItem.AccumulatedPoints = pointStatus.Points;
                        }
                    }
                    else // error
                    {
                        entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), LoyaltyCustomer.AsString(valid));
                        comment = LoyaltyCustomer.AsString(valid);
                    }

                }
                finally
                {
                    Interfaces.Services.DialogService(entry).CloseStatusDialog();
                }
            }
            catch (Exception)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), LoyaltyCustomer.AsString(valid));
                valid = LoyaltyCustomer.ErrorCodes.UnknownError;
            }

            if (valid != LoyaltyCustomer.ErrorCodes.NoErrors)
            {
                comment = (comment == "") ? Resources.ErrorConnectingToLoyaltyServer : comment;
                Interfaces.Services.DialogService(entry).ShowMessage(comment, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
        }

        /// <summary>
        /// Updates the central database with the loyalty points that have been issued (accrued) during the sale or when a sale with a loyalty points tender line is being returned.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <param name="loyaltyItem">The loyalty item.</param>
        public virtual void UpdateIssuedLoyaltyPoints(IConnectionManager entry, IRetailTransaction retailTransaction, ILoyaltyItem loyaltyItem)
        {
            UpdateIssuedLoyaltyPoints(entry, retailTransaction, loyaltyItem, loyaltyItem.LineNum, loyaltyItem.CardNumber, loyaltyItem.CalculatedPoints);
        }

        /// <summary>
        /// Updates the central database with the loyalty points that have been used as a discount during the sale or when a sale is being paid for with loyalty points tender line
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <param name="lineID"></param>
        /// <param name="points">The points that were being used</param>
        /// <param name="voided"></param>
        public virtual void UpdateUsedLoyaltyPoints(IConnectionManager entry, IRetailTransaction retailTransaction, int lineID, decimal points, bool voided)
        {
            if (retailTransaction.LoyaltyItem == null)
            {
                return;
            }

            if (retailTransaction.Training)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.NoLoyaltyPointsOnTraining);
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Transaction is a training transaction - no loyalty points added to card", "Loyalty.UpdateIssuedLoyaltyPoints");
                return;
            }

            pointStatus.Clear();

            try
            {
                try
                {
                    Interfaces.Services.DialogService(entry).UpdateStatusDialog(Resources.UpdatingUsedLoyaltyPoints);

                    pointStatus.CardNumber = retailTransaction.LoyaltyItem.CardNumber;
                    ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                    ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                    pointStatus = service.UpdateUsedLoyaltyPoints(
                        entry,
                        settings.SiteServiceProfile,
                        pointStatus,
                        retailTransaction.TransactionId,
                        lineID,
                        retailTransaction.BeginDateTime,
                        points,
                        retailTransaction.ReceiptId,
                        voided
                        );

                    if ((pointStatus.ResultCode == LoyaltyCustomer.ErrorCodes.NoErrors) && (!retailTransaction.LoyaltyItem.Empty))
                    {
                        GetPointStatus(entry);

                        Interfaces.Services.DialogService(entry).UpdateStatusDialog(Resources.UpdatingUsedLoyaltyPoints);
                        if (pointStatus.ResultCode == LoyaltyCustomer.ErrorCodes.NoErrors)
                        {
                            retailTransaction.LoyaltyItem.AccumulatedPoints = pointStatus.Points;
                        }
                        else // error
                        {
                            pointStatus.Comment = LoyaltyCustomer.AsString(pointStatus.ResultCode);
                        }
                    }
                }
                finally
                {
                    Interfaces.Services.DialogService(entry).CloseStatusDialog();
                }
            }
            catch (Exception)
            {
                pointStatus.ResultCode = LoyaltyCustomer.ErrorCodes.UnknownError;
            }

            if (pointStatus.ResultCode != LoyaltyCustomer.ErrorCodes.NoErrors)
            {
                pointStatus.Comment = (pointStatus.Comment == "") ? Resources.ErrorConnectingToLoyaltyServer : pointStatus.Comment;
                Interfaces.Services.DialogService(entry).ShowMessage(pointStatus.Comment, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
        }        

        /// <summary>
        /// Updates the central database with the loyalty points that have been used as payment with a loyalty points tender line
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <param name="loyaltyTenderItem">The loyalty tender item.</param>
        public virtual void UpdateUsedLoyaltyPoints(IConnectionManager entry, IRetailTransaction retailTransaction, ILoyaltyTenderLineItem loyaltyTenderItem)
        {
            UpdateUsedLoyaltyPoints(entry, retailTransaction, loyaltyTenderItem.LineId, loyaltyTenderItem.Points, loyaltyTenderItem.Voided);

            if (pointStatus.ResultCode != LoyaltyCustomer.ErrorCodes.NoErrors)
            {
                loyaltyTenderItem.Points = 0;
            }
        }

        /// <summary>
        /// Function should update RBOLOYALTYMSRCARDTABLE table: LOYALTYCUSTID=CustomerID
        /// </summary>
        protected virtual bool UpdateLoyaltyCardCustomerID(IConnectionManager entry, RecordIdentifier loyaltyCardID, RecordIdentifier customerID)
        {
            LoyaltyCustomer.ErrorCodes valid = LoyaltyCustomer.ErrorCodes.UnknownError;
            string comment = "";

            try
            {
                try
                {
                    Interfaces.Services.DialogService(entry).UpdateStatusDialog(Resources.UpdateLoyaltyCardCustomerID);

                    ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                    ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                    service.UpdateLoyaltyCardCustomerID(entry, settings.SiteServiceProfile, ref valid, ref comment, loyaltyCardID, customerID);

                    if (valid != 0)
                    {
                        comment = LoyaltyCustomer.AsString(valid);
                    }
                }
                finally
                {
                    Interfaces.Services.DialogService(entry).CloseStatusDialog();
                }
            }
            catch (Exception)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), LoyaltyCustomer.AsString(LoyaltyCustomer.ErrorCodes.UnknownError));
                valid = LoyaltyCustomer.ErrorCodes.UnknownError;
            }

            if (valid != 0)
            {
                comment = (comment == "") ? Resources.ErrorConnectingToLoyaltyServer : comment;
                Interfaces.Services.DialogService(entry).ShowMessage(comment, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }

            return true;
        }

        /// <summary>
        /// Connects a customer to a loyalty card
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        /// <param name="posTransaction">The current transaction </param>
        /// <param name="customer">The current customer on the transaction (if any)</param>
        public virtual void AddCustomerToLoyaltyCard(IConnectionManager entry, IPosTransaction posTransaction, Customer customer)
        {                      
            pointStatus.Clear();

            AddCustomerToLoyaltoCardDialog dlg = new AddCustomerToLoyaltoCardDialog(entry, customer, posTransaction);

            if (posTransaction.Training)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.NoCustomerConnectionOnTraining);
                return;
            }

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            if (dlg.ShowDialog(settings.POSApp.POSMainWindow) == DialogResult.OK)
            {
                UpdateLoyaltyCardCustomerID(entry, dlg.LoyaltyCardNumer, dlg.CustomerID);                
            }            
        }

        /// <summary>
        /// Uses GetLoyaltyCardInfo to retrieve information about a loyalty card and the number of points that should be used for the discount. The points are then turned into a discount and divided between all the items on the transaction.
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="tenderTypeID">The tender type ID that should be used to calculate the point values</param>
        /// <returns></returns>
        public virtual bool AddLoyaltyPointsDiscount(IConnectionManager entry, IRetailTransaction retailTransaction, RecordIdentifier tenderTypeID)
        {
            try
            {
                pointStatus.Clear();

                calculationHelper = null;
                UpdateCalculatePoints(retailTransaction, LoyaltyPointsRelation.Discount);

                if (retailTransaction.SaleIsReturnSale)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.ReturnSaleCannotUpdateLoyaltyPointDisc);
                    return false;
                }

                if (retailTransaction.SaleItems.Count(c => c.Voided == false) == 0)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.NoItemsOnSale);
                    return false;
                }

                if (retailTransaction.TenderLines.Count(c => !c.Voided && c is LoyaltyTenderLineItem) > 0)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.TransactionAlreadyHasLoyaltyPayment);
                    return false;                    
                }

                if (calculationHelper.GetTotalAmtUsedForDiscount(retailTransaction, true) == decimal.Zero)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.NoItemsToDiscount);
                    return false;
                }

                CardInfo cardInfo = new CardInfo();

                decimal discountAmt = GetLoyaltyInfoForPayment(entry, cardInfo, retailTransaction, retailTransaction.TransSalePmtDiff, tenderTypeID, UseDialogEnum.PointsDiscount);

                if (discountAmt == decimal.MaxValue)
                {
                    retailTransaction.ClearLoyaltyDiscountLines();

                    retailTransaction.LoyaltyItem.CalculatedPointsAmount = decimal.Zero;
                    retailTransaction.LoyaltyItem.Relation = LoyaltyPointsRelation.Header;
                    retailTransaction.LoyaltyItem.CalculatedPoints = decimal.Zero;
                    retailTransaction.LoyaltyItem.RuleID = RecordIdentifier.Empty;
                    retailTransaction.LoyaltyItem.AggregatedItemQuantity = 1M;
                    retailTransaction.LoyaltyItem.RecalculateDiscount = false;

                    Interfaces.Services.DiscountService(entry).CalculateDiscount(entry, retailTransaction, true);
                    Interfaces.Services.CalculationService(entry).CalculateTotals(entry, retailTransaction);

                    return false;
                }

                discountAmt = discountAmt > 0 ? discountAmt * -1 : discountAmt; // the discount should always be a negative number

                if (!retailTransaction.LoyaltyItem.Empty && discountAmt != decimal.Zero)
                {
                    LoyaltyItem calculatedPoints = calculationHelper.GetPointsforLoyaltyDiscount(entry, tenderTypeID, discountAmt);

                    retailTransaction.LoyaltyItem.Relation = LoyaltyPointsRelation.Discount;
                    retailTransaction.LoyaltyItem.CalculatedPointsAmount = discountAmt * -1;
                    retailTransaction.LoyaltyItem.CalculatedPoints = calculatedPoints.CalculatedPoints *-1;
                    retailTransaction.LoyaltyItem.RuleID = calculatedPoints.RuleID;
                    retailTransaction.LoyaltyItem.AggregatedItemQuantity = calculatedPoints.AggregatedItemQuantity;
                    retailTransaction.LoyaltyItem.RecalculateDiscount = false;

                    calculationHelper.AddCustomerToTransaction(entry, retailTransaction);

                    AddLoyaltyDiscountLines(retailTransaction);

                    return true;
                }
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }

            return false;
        }

        protected virtual void AddLoyaltyDiscountLines(IRetailTransaction retailTransaction)
        {
            retailTransaction.ClearLoyaltyDiscountLines();

            decimal totalAmt = calculationHelper.GetTotalAmtUsedForDiscount(retailTransaction, false);

            // The percentage discount
            if (totalAmt != 0)
            {
                decimal discPct = (1 - ((Math.Abs(totalAmt) - Math.Abs(retailTransaction.LoyaltyItem.CalculatedPointsAmount)) / Math.Abs(totalAmt))) * 100;
                
                // Add the total discount to each item.
                foreach (ISaleLineItem saleItem in retailTransaction.SaleItems.Where(saleItem => saleItem.Voided == false && !saleItem.DiscountUnsuccessfullyApplied))
                {
                    if (saleItem.NoDiscountAllowed == false && saleItem.Quantity != 0)
                    {
                        // Add a new loyalty discount
                        LoyaltyDiscountItem disc = new LoyaltyDiscountItem();
                        disc.Percentage = discPct;
                        saleItem.Add(disc);
                    }
                }
            }
        }

        public virtual bool LoyaltyDiscountRecalculationNeeded(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            if (retailTransaction.LoyaltyItem.Relation != LoyaltyPointsRelation.Discount)
            {
                return false;
            }

            UpdateCalculatePoints(retailTransaction, LoyaltyPointsRelation.Discount);

            decimal totalAmt = calculationHelper.GetTotalAmtUsedForDiscount(retailTransaction, true);
            LoyaltyMSRCard card = calculationHelper.GetLoyaltyInfo(entry, retailTransaction.LoyaltyItem);

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            decimal maxDiscount = Interfaces.Services.RoundingService(entry).Round(entry, (totalAmt * ((decimal)card.UsePointsLimit / 100)), settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
            if (!retailTransaction.SaleIsReturnSale && (Math.Abs(retailTransaction.LoyaltyItem.CalculatedPointsAmount) > Math.Abs(maxDiscount)) || retailTransaction.LoyaltyItem.RecalculateDiscount)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.RecalculationOfLoyaltyDiscount);
                return true;
            }

            return false;
        }
     
        public virtual bool LoyaltyCardExistsForLoyaltyScheme(IConnectionManager entry, RecordIdentifier loyaltySchemeID)
        {
            try
            {
                ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                return service.LoyaltyCardExistsForLoyaltyScheme(entry, settings.SiteServiceProfile, loyaltySchemeID,true);
            }
            finally
            {
                Interfaces.Services.DialogService(entry).CloseStatusDialog();
            }
        }

        public virtual bool SiteServiceIsNeeded(IConnectionManager entry, IPosTransaction transaction)
        {
            return true;
        }
    }
}