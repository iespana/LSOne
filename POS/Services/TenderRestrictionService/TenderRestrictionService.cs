using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    public partial class TenderRestrictionService : ITenderRestrictionService
    {
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
#pragma warning restore 0612, 0618
        }

        protected virtual TenderRestrictionResult InitialCheck(IConnectionManager entry, ISettings settings, IPosTransaction posTransaction, StorePaymentMethod paymentMethod)
        {
            if (!(posTransaction is RetailTransaction))
            {
                return TenderRestrictionResult.NoTenderRestrictions;
            }

            if (paymentMethod == null)
            {
                return TenderRestrictionResult.NoTenderRestrictions;
            }

            var retailTransaction = (RetailTransaction)posTransaction;

            if (!paymentMethod.PaymentLimitations.Any())
            {
                return TenderRestrictionResult.NoTenderRestrictions;
            }

            if (retailTransaction.SaleItems.Count == 0)
            {
                return TenderRestrictionResult.NoTenderRestrictions;
            }

            if (Interfaces.Services.RoundingService(entry)
                .Round(entry, retailTransaction.TransSalePmtDiff, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime)
                == decimal.Zero)
            {
                return TenderRestrictionResult.NoTenderRestrictions;
            }

            return TenderRestrictionResult.Continue;
        }

        public virtual TenderRestrictionResult GetUnconfirmedTenderRestrictionAmount(IConnectionManager entry, ISettings settings, IPosTransaction retailTransaction, StorePaymentMethod paymentMethod, ref decimal payableAmount)
        {
            payableAmount = decimal.Zero; //the return value of total amount paid with tender 

            if (InitialCheck(entry, settings, retailTransaction, paymentMethod) == TenderRestrictionResult.NoTenderRestrictions)
            {
                return TenderRestrictionResult.NoTenderRestrictions;
            }

            // Get the list of items/retail groups/departments/special groups and etc that are part of the limitation on the payment method
            List<PaymentMethodLimitation> limitations = new List<PaymentMethodLimitation>();
            foreach (StorePaymentLimitation limitation in paymentMethod.PaymentLimitations)
            {
                limitations.AddRange(Providers.PaymentLimitationsData.GetListForRestrictionCode(entry, limitation.LimitationMasterID, paymentMethod.PaymentTypeID, CacheType.CacheTypeTransactionLifeTime) ?? new List<PaymentMethodLimitation>());
            }

            if (!limitations.Any())
            {
                return TenderRestrictionResult.NoTenderRestrictions;
            }

            // Calculating how much amount of the original amount can be paid
            foreach (SaleLineItem lineItem in ((RetailTransaction)retailTransaction).SaleItems.Where(x => !x.Voided && x.PaymentIndex == Constants.PaymentIndexToBeUpdated))
            {
                payableAmount += settings.Store.StorePriceSetting == Store.StorePriceSettingsEnum.PricesIncludeTax ? lineItem.NetAmountWithTax : lineItem.NetAmount;
            }

            // If nothing can be paid, then it can be concluded that this card is prohibited
            return payableAmount == 0 ? TenderRestrictionResult.NothingCanBePaidFor : TenderRestrictionResult.Continue;
        }

        public virtual TenderRestrictionResult GetTenderRestrictionAmount(IConnectionManager entry, ISettings settings, IPosTransaction posTransaction, StorePaymentMethod paymentMethod, ref decimal payableAmount)
        {
            payableAmount = decimal.Zero;

            if (InitialCheck(entry, settings, posTransaction, paymentMethod) == TenderRestrictionResult.NoTenderRestrictions)
            {
                return TenderRestrictionResult.NoTenderRestrictions;
            }

            //Get the list of items/retail groups/departments/special groups and etc that are part of the limitation on the payment method
            List<PaymentMethodLimitation> limitations = new List<PaymentMethodLimitation>();
            foreach (StorePaymentLimitation limitation in paymentMethod.PaymentLimitations)
            {
                limitations.AddRange(Providers.PaymentLimitationsData.GetListForRestrictionCode(entry, limitation.LimitationMasterID, paymentMethod.PaymentTypeID, CacheType.CacheTypeTransactionLifeTime) ?? new List<PaymentMethodLimitation>());
            }

            RetailTransaction retailTransaction = (RetailTransaction)posTransaction;
            if (!retailTransaction.IsReturnTransaction && !limitations.Any())
            {
                return TenderRestrictionResult.NoTenderRestrictions;
            }

            foreach (StorePaymentLimitation limitation in paymentMethod.PaymentLimitations)
            {
                if (limitations.Count(c => c.Type == LimitationType.Everything && c.LimitationMasterID == limitation.LimitationMasterID) == 0)
                {
                    PaymentMethodLimitation everything = new PaymentMethodLimitation();
                    everything.LimitationMasterID = limitation.LimitationMasterID;
                    everything.RestrictionCode = limitation.RestrictionCode;
                    everything.TenderID = limitation.TenderTypeID;
                    limitations.Add(everything);
                }
            }

            // The flow is very important:
            // 1. Process all lines and call CalculateTax() for each line that needs it
            // 2. Call CalculateTotals() for the transaction only after all lines were already processed by CalculateTax()
            // 3. Calculate payableAmount for each line only after the transaction and all its lines were processed with CalculateTotals()
            var saleLineItemsToProcess = new List<SaleLineItem>();

            // Calculating how much amount of the original amount can be paid.
            foreach (SaleLineItem lineItem in retailTransaction.SaleItems.Where(x => !x.Voided && !x.IsAssemblyComponent))
            {
                PaymentMethodLimitation restriction = GetRestrictionForItem(entry, paymentMethod, limitations, lineItem);

                // Check new items on the sale or items that have not been restricted yet
                if ((!retailTransaction.IsReturnTransaction || (retailTransaction.IsReturnTransaction && !lineItem.ReceiptReturnItem)) && 
                    (RecordIdentifier.IsEmptyOrNull(lineItem.LimitationMasterID) || (!RecordIdentifier.IsEmptyOrNull(lineItem.LimitationMasterID) && !retailTransaction.CustomerOrder.Empty())) &&
                    (!restriction.Empty() && restriction.Include))
                {
                    lineItem.LimitationMasterID = restriction.LimitationMasterID;
                    lineItem.LimitationCode = (string)restriction.RestrictionCode;
                    lineItem.PaymentIndex = Constants.PaymentIndexToBeUpdated;

                    if(restriction.TaxExempt)                            
                    {
                        // Only recalculate if we need to update the totals. In this case we need to do that if
                        // the transaction or line item wasn't tax exempted already
                        bool recalculateTransaction = !lineItem.TaxExempt;

                        lineItem.TaxExempt = restriction.TaxExempt;
                        lineItem.TaxExemptionCode = (string)restriction.RestrictionCode;

                        if (lineItem.IsAssembly && !lineItem.IsAssemblyComponent)
                        {
                            foreach (ISaleLineItem sl in retailTransaction.SaleItems.Where(x => x.IsAssemblyComponent && x.LinkedToLineId == lineItem.LineId))
                            {
                                sl.TaxExempt = restriction.TaxExempt;
                                sl.TaxExemptionCode = (string)restriction.RestrictionCode;

                                if (!recalculateTransaction)
                                {
                                    // Set tax exemption code on all components
                                    sl.TaxLines[0].TaxExemptionCode = sl.TaxExemptionCode;
                                }
                                else if(lineItem.ItemAssembly.CalculatePriceFromComponents)
                                {
                                    Interfaces.Services.TaxService(entry).CalculateTax(entry, retailTransaction, sl);
                                }
                            }
                        }

                        if (recalculateTransaction)
                        {
                            Interfaces.Services.TaxService(entry).CalculateTax(entry, retailTransaction, lineItem);
                        }
                        else
                        {
                            // The item was already tax exempt, but we still need to make sure it has the correct exemption code
                            lineItem.TaxLines[0].TaxExemptionCode = lineItem.TaxExemptionCode;
                        }
                    }

                    saleLineItemsToProcess.Add(lineItem);
                }
                else if (retailTransaction.IsReturnTransaction)
                {
                    CalculatePayableAmountForReturnSale(entry, settings, posTransaction, retailTransaction, lineItem, restriction, paymentMethod.PaymentTypeID, ref payableAmount);
                }
            }

            Interfaces.Services.CalculationService(entry).CalculateTotals(entry, posTransaction);

            if (!retailTransaction.CustomerOrder.Empty() && retailTransaction.CustomerOrder.Status == CustomerOrderStatus.New && retailTransaction.SaleItems.Any(x => !RecordIdentifier.IsEmptyOrNull(x.LimitationMasterID)))
            {
                retailTransaction.SaleItems.Where(x => !x.Voided).ToList().ForEach(x => x.Order.Deposits.Clear());

                // UpdatePaymentInformation(), among other things, recalculates the min deposit to be paid for each item
                // We want to do this because if we're paying deposits, we want to calculate the restrictions in regards of the min deposits, not in regards of NetAmount[WithTax]
                Interfaces.Services.CustomerOrderService(entry).UpdatePaymentInformation(entry, retailTransaction);
            }

            decimal payableAmountForSaleLineItemsToProcess = 0;
            bool isPayingDeposit = !retailTransaction.CustomerOrder.Empty() && retailTransaction.CustomerOrder.CurrentAction == CustomerOrderAction.PayDeposit;
            saleLineItemsToProcess.ForEach(
                x => 
                    payableAmountForSaleLineItemsToProcess +=
                        isPayingDeposit ?
                        x.Order.TotalDepositAmount() :
                        x.GetCalculatedNetAmount(settings.Store.StorePriceSetting == Store.StorePriceSettingsEnum.PricesIncludeTax));
            payableAmount += payableAmountForSaleLineItemsToProcess;

            var paymentHasRestrictions = paymentMethod.PaymentLimitations.Any();
            if ((retailTransaction.IsReturnTransaction || !retailTransaction.CustomerOrder.Empty()) && paymentHasRestrictions)
            {
                decimal alreadyPayed = retailTransaction.ITenderLines.Where(x => x.TenderTypeId == paymentMethod.PaymentTypeID && !x.Voided).Sum(x => x.Amount);
                payableAmount -= alreadyPayed;
            }

            if (!retailTransaction.IsReturnTransaction && retailTransaction.CustomerOrder.Empty())
            {
                decimal payableAmountWithSpillover = CalculatePayableAmountWithSpillover(entry, settings, retailTransaction, saleLineItemsToProcess, paymentMethod);

                if(payableAmountWithSpillover != 0)
                {
                    payableAmount = payableAmountWithSpillover;
                }
            }

            payableAmount = Services.Interfaces.Services.RoundingService(entry).Round(entry, payableAmount, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
            // If nothing can be paid, then it can be concluded that this card is prohibited
            return payableAmount == 0 ? TenderRestrictionResult.NothingCanBePaidFor : TenderRestrictionResult.Continue;
        }

        /// <summary>
        /// Calculates if there is a spillover and how much is paybable with the spillover taken into account
        /// </summary>
        /// <param name="entry">The entry into to the database</param>
        /// <param name="settings">The current POS settings</param>
        /// <param name="posTransaction">The transaction to examine</param>
        /// <param name="saleLineItemsToProcess">The list of items that the current payment restriction applies for</param>
        /// <param name="paymentMethod">The current payment method being examined for restrictions</param>
        /// <returns>True and a non-zero value if there is spillover, false and zero otherwise</returns>
        private decimal CalculatePayableAmountWithSpillover(IConnectionManager entry, ISettings settings, IRetailTransaction retailTransaction, List<SaleLineItem> saleLineItemsToProcess, StorePaymentMethod paymentMethod)
        {
            // Check if an existing non-limited payment is covering part of our limited items. In that case the total payable amount with limitation
            // needs to be adjusted so that we don't overpay with the limited payment
            Func<ITenderLineItem, bool> paymentLineHasRegisteredLimitations = tenderLine => retailTransaction.ISaleItems.Any(x => x.PaymentIndex == tenderLine.LineId);

            decimal alreadyPayedByNonLimitedPayments = retailTransaction.ITenderLines.Where(p => !p.Voided && p.TenderTypeId != paymentMethod.PaymentTypeID && !paymentLineHasRegisteredLimitations(p)).Sum(p => p.Amount);
            decimal spillOverAmount = 
                alreadyPayedByNonLimitedPayments == 0 ?
                0 :
                alreadyPayedByNonLimitedPayments - retailTransaction.ISaleItems.Where(p => !p.Voided && RecordIdentifier.IsEmptyOrNull(p.LimitationMasterID) && !p.IsAssemblyComponent).Sum(p => p.GetCalculatedNetAmount(true));            

            // If we detect a spillover, we need to virtually split the items to get an accurate amount left to pay. 
            if (spillOverAmount > 0)
            {
                IRetailTransaction clonedTransaction = (IRetailTransaction)retailTransaction.Clone();
                Guid dummyRestrictionID = Guid.NewGuid();

                clonedTransaction.SaleItems.Clear();
                clonedTransaction.TenderLines.Clear();

                List<ISaleLineItem> itemsToSplit = new List<ISaleLineItem>();
                saleLineItemsToProcess.ForEach(x => itemsToSplit.Add((ISaleLineItem)x.Clone()));

                foreach (ISaleLineItem item in itemsToSplit)
                {
                    ClearRestrictionFromItem(item, clonedTransaction);
                    item.LimitationMasterID = dummyRestrictionID;
                    item.LimitationCode = "dummyCode";
                    item.PaymentIndex = 1;

                    clonedTransaction.Add(item);
                }

                // Add dummy restriction information and tender line
                TenderLineItem tenderLine = new TenderLineItem
                {
                    Amount = spillOverAmount
                };

                clonedTransaction.LastRunOperationIsValidPayment = true;
                clonedTransaction.Add(tenderLine);

                Interfaces.Services.TransactionService(entry).RecalculatePriceTaxDiscount(entry, clonedTransaction, true);
                Interfaces.Services.CalculationService(entry).CalculateTotals(entry, clonedTransaction);

                StorePaymentMethod dummyPaymentMethod = new StorePaymentMethod();
                dummyPaymentMethod.PaymentLimitations.Add(new StorePaymentLimitation());

                SplitLines(entry, settings, clonedTransaction, dummyPaymentMethod, 1);

                // Now we have a transaction where:
                // * The already paid for items are now marked with a dummy limitation code
                // * The remaining items that have no dummy limitation code are actually the items that we can pay for with our current limited payment.
                // Now we can calculate the the correct amount we can pay for using the unmarked items                
                decimal payableAmountWithSpillover = 0;
                GetTenderRestrictionAmount(entry, settings, clonedTransaction, (StorePaymentMethod)paymentMethod.Clone(), ref payableAmountWithSpillover);

                retailTransaction.TransSalePmtDiffForCurrentPaymentOperation = clonedTransaction.TransSalePmtDiff;

                return payableAmountWithSpillover;
            }
            else
            {
                return 0;
            }            
        }

        public virtual TenderRestrictionResult DisplayTenderRestrictionInformation(IConnectionManager entry, ISettings settings, IPosTransaction posTransaction, StorePaymentMethod paymentMethod, ref decimal payableAmount)
        {
            if (InitialCheck(entry, settings, posTransaction, paymentMethod) == TenderRestrictionResult.NoTenderRestrictions)
            {
                return TenderRestrictionResult.NoTenderRestrictions;
            }

            //Get the list of items/retail groups/departments/special groups and etc that are part of the limitation on the payment method
            List<PaymentMethodLimitation> limitations = new List<PaymentMethodLimitation>();
            foreach (StorePaymentLimitation limitation in paymentMethod.PaymentLimitations)
            {
                limitations.AddRange(Providers.PaymentLimitationsData.GetListForRestrictionCode(entry, limitation.LimitationMasterID, paymentMethod.PaymentTypeID, CacheType.CacheTypeTransactionLifeTime) ?? new List<PaymentMethodLimitation>());
            }

            RetailTransaction retailTransaction = (RetailTransaction)posTransaction;
            if (!retailTransaction.IsReturnTransaction && !limitations.Any())
            {
                return TenderRestrictionResult.NoTenderRestrictions;
            }

            if (payableAmount == decimal.Zero)
            {
                foreach (StorePaymentLimitation limitation in paymentMethod.PaymentLimitations)
                {
                    if (limitations.Count(c => c.Type == LimitationType.Everything && c.LimitationMasterID == limitation.LimitationMasterID) == 0)
                    {
                        PaymentMethodLimitation everything = new PaymentMethodLimitation();
                        everything.LimitationMasterID = limitation.LimitationMasterID;
                        everything.RestrictionCode = limitation.RestrictionCode;
                        everything.TenderID = limitation.TenderTypeID;
                        limitations.Add(everything);
                    }
                }

                // Calculating how much amount of the original amount can be paid.
                foreach (SaleLineItem lineItem in retailTransaction.SaleItems.Where(x => !x.Voided && !x.IsAssemblyComponent))
                {
                    PaymentMethodLimitation restriction = GetRestrictionForItem(entry, paymentMethod, limitations, lineItem);

                    // Add to payableAmount all prices of those items that can be payed with the selected payment type
                    // Exclude items that have already been paid for
                    if ((!retailTransaction.IsReturnTransaction || (retailTransaction.IsReturnTransaction && !lineItem.ReceiptReturnItem)) &&
                        lineItem.PaymentIndex < 0 &&
                        (!restriction.Empty() && restriction.Include))
                    {
                        payableAmount += lineItem.GetCalculatedNetAmount(settings.Store.StorePriceSetting == Store.StorePriceSettingsEnum.PricesIncludeTax);
                    }
                    else if (retailTransaction.IsReturnTransaction)
                    {
                        CalculatePayableAmountForReturnSale(entry, settings, posTransaction, retailTransaction, lineItem, restriction, paymentMethod.PaymentTypeID, ref payableAmount);
                    }
                }
            }

            if (payableAmount == decimal.Zero)
            {
                return TenderRestrictionResult.NothingCanBePaidFor;
            }

            if (!retailTransaction.IsReturnTransaction && 
                (payableAmount != (settings.Store.StorePriceSetting == Store.StorePriceSettingsEnum.PricesIncludeTax ? retailTransaction.NetAmountWithTax : retailTransaction.NetAmount)) && 
                (settings.FunctionalityProfile.DialogLimitationDisplayType != FunctionalityProfile.LimitationDisplayType.NotDisplayed))
            {
                TenderRestrictionDialog tenderRestrictionDialog = new TenderRestrictionDialog(entry, (RetailTransaction)posTransaction, paymentMethod);

                if (tenderRestrictionDialog.ShowDialog() == DialogResult.Cancel)
                {
                    CancelTenderRestriction(entry, settings, posTransaction, paymentMethod, Constants.PaymentIndexToBeUpdated);                    
                    return TenderRestrictionResult.CancelledByUser;
                }
            }

            return TenderRestrictionResult.Continue;
        }

        public virtual void ClearTenderRestriction(IConnectionManager entry, IPosTransaction posTransaction)
        {            
            foreach (ISaleLineItem item in ((RetailTransaction)posTransaction).SaleItems.Where(x => !RecordIdentifier.IsEmptyOrNull(x.LimitationMasterID)))
            {
                ClearRestrictionFromItem(item, posTransaction);                
            }

            Interfaces.Services.TransactionService(entry).RecalculatePriceTaxDiscount(entry, posTransaction, true);
            Interfaces.Services.CalculationService(entry).CalculateTotals(entry, posTransaction);
        }

        public virtual void CancelTenderRestriction(IConnectionManager entry, ISettings settings, IPosTransaction posTransaction, StorePaymentMethod paymentMethod, int paymentIndex)
        {
            //If the transaction isn't a retail transaction then no need to go further
            if (!(posTransaction is RetailTransaction))
            {
                return;
            }            

            if (paymentMethod != null && InitialCheck(entry, settings, posTransaction, paymentMethod) == TenderRestrictionResult.NoTenderRestrictions)
            {
                return;
            }

            bool recalculateTransaction = false;                        
            
            List<ISaleLineItem> itemsToRemove = new List<ISaleLineItem>();
            List<ISaleLineItem> cancelledItems = new List<ISaleLineItem>();

            IRetailTransaction retailTransaction = (IRetailTransaction)posTransaction;

            foreach (ISaleLineItem item in retailTransaction.SaleItems.Where(x => !RecordIdentifier.IsEmptyOrNull(x.LimitationMasterID) && x.PaymentIndex == paymentIndex && !x.IsAssemblyComponent))
            {
                bool prevTaxExempt = item.TaxExempt;

                ClearRestrictionFromItem(item, retailTransaction);                
                
                if (prevTaxExempt != item.TaxExempt)
                {                                        
                    recalculateTransaction = true;
                }
                
                if(!recalculateTransaction && item.TaxExempt)
                {
                    // The item was already tax exempt, but we still need to make sure it has the correct exemption code
                    item.TaxLines[0].TaxExemptionCode = item.TaxExemptionCode;
                }

                cancelledItems.Add(item);

                if (item.IsAssembly && !item.IsAssemblyComponent)
                {
                    foreach (ISaleLineItem sl in retailTransaction.SaleItems.Where(x => x.IsAssemblyComponent && x.LinkedToLineId == item.LineId))
                    {
                        if (!recalculateTransaction && sl.TaxExempt)
                        {
                            // The item was already tax exempt, but we still need to make sure it has the correct exemption code
                            sl.TaxLines[0].TaxExemptionCode = sl.TaxExemptionCode;
                        }

                        cancelledItems.Add(sl);
                    }
                }
            }

            if (paymentIndex > 0)
            {
                foreach (ISaleLineItem item in cancelledItems.Where(p => p.LimitationSplitChildLineId > 0 || p.LimitationSplitParentLineId > 0))
                {
                    ISaleLineItem parentItem = retailTransaction.GetTopMostLimitationSplitParentItem(item);
                    CompressSplitItems(parentItem, retailTransaction, itemsToRemove);

                    if (itemsToRemove.Count > 0)
                    {
                        recalculateTransaction = true;
                    }
                }

                if (itemsToRemove.Count > 0)
                {
                    itemsToRemove.ForEach((item) => retailTransaction.SaleItems.Remove(item));

                    // Items have been removed from the tail end of the transaction so we need to fix the line IDs of the limitation split items
                    int count = 1;
                    foreach(ISaleLineItem item in retailTransaction.SaleItems)
                    {
                        if (item.LimitationSplitParentLineId > 0)
                        {
                            item.LineId = count;
                            retailTransaction.GetItem(item.LimitationSplitParentLineId).LimitationSplitChildLineId = item.LineId;

                            if(item.LimitationSplitChildLineId > 0)
                            {
                                retailTransaction.GetItem(item.LimitationSplitChildLineId).LimitationSplitParentLineId = item.LineId;
                            }
                        }

                        count++;
                    }
                }                
            }

            
            if (recalculateTransaction)
            {
                Interfaces.Services.TransactionService(entry).RecalculatePriceTaxDiscount(entry, posTransaction, true);
                Interfaces.Services.CalculationService(entry).CalculateTotals(entry, posTransaction);                
            }
        }

        public virtual void CancelUnconfirmedTenderRestriction(IConnectionManager entry, ISettings settings, IPosTransaction retailTransaction, StorePaymentMethod paymentMethod)
        {
            CancelTenderRestriction(entry, settings, retailTransaction, paymentMethod, Constants.PaymentIndexToBeUpdated);
        }

        // TODO: Do we use a IPosTransaction or a retailTransaction?
        public virtual void UpdatePaymentIndex(IConnectionManager entry, ISettings settings, IPosTransaction retailTransaction, StorePaymentMethod paymentMethod, int paymentIndex)
        {
            if (InitialCheck(entry, settings, retailTransaction, paymentMethod) == TenderRestrictionResult.NoTenderRestrictions)
            {
                return;
            }

            foreach (ISaleLineItem item in ((RetailTransaction)retailTransaction).SaleItems.Where(x => !RecordIdentifier.IsEmptyOrNull(x.LimitationMasterID) && x.PaymentIndex == Constants.PaymentIndexToBeUpdated))
            {
                item.PaymentIndex = paymentIndex;
            }
        }

        public virtual PaymentMethodLimitation GetRestrictionForItem(IConnectionManager entry, StorePaymentMethod paymentMethod, List<PaymentMethodLimitation> limitationList, ISaleLineItem item)
        {
            //If no restrictions were found
            if (!limitationList.Any())
            {
                return new PaymentMethodLimitation();
            }

            //Check if there is a limitation specific for the item
            PaymentMethodLimitation codeForItem = limitationList.FirstOrDefault(a => a.Type == LimitationType.Item && a.TenderID == paymentMethod.PaymentTypeID && a.RelationMasterID == item.MasterID);
            if (codeForItem != null)
            {
                return codeForItem;
            }

            //Check if there is a limitation specific for the retail group
            codeForItem = limitationList.FirstOrDefault(a => a.Type == LimitationType.RetailGroup && a.TenderID == paymentMethod.PaymentTypeID && a.RelationReadableID == item.RetailItemGroupId);
            if (codeForItem != null)
            {
                return codeForItem;
            }

            //Check if there is a limitation specific for a special group
            List<DataEntity> specialGroups = Providers.SpecialGroupData.GetSpecialGroupsForItem(entry, item.MasterID);
            foreach (DataEntity groupItem in specialGroups)
            {
                codeForItem = limitationList.FirstOrDefault(a => a.Type == LimitationType.SpecialGroup && a.TenderID == paymentMethod.PaymentTypeID && a.RelationReadableID == groupItem.ID);
                if (codeForItem != null)
                {
                    return codeForItem;
                }
            }

            //Check if there is a limitation specific for the retail department
            codeForItem = limitationList.FirstOrDefault(a => a.Type == LimitationType.RetailDepartment && a.TenderID == paymentMethod.PaymentTypeID && a.RelationReadableID == item.ItemDepartmentId);
            if (codeForItem != null)
            {
                return codeForItem;
            }

            //Check if there is a limitation for everything
            codeForItem = limitationList.FirstOrDefault(a => a.Type == LimitationType.Everything && a.TenderID == paymentMethod.PaymentTypeID);
            if (codeForItem != null)
            {
                return codeForItem;
            }

            return new PaymentMethodLimitation();
        }

        public virtual string GetPaymentLimitationsAsHtml(IConnectionManager entry, ISettings settings, IPosTransaction posTransaction)
        {
            var retailTransaction = posTransaction as RetailTransaction;

            if (retailTransaction == null)
            {
                return string.Empty;
            }

            Func<ISaleLineItem, decimal> valueThatCanBePaid = sli => sli.GetCalculatedNetAmount(settings.Store.DisplayBalanceWithTax);

            var validSaleItems = retailTransaction.ISaleItems.Where(x => !x.Voided && !x.IsAssemblyComponent);

            var retailItemIDs = validSaleItems.Select(x => x.ItemId).Distinct().ToList();
            var retailGroupIDs = validSaleItems.Select(x => x.RetailItemGroupId).Distinct().ToList();
            var specialGroupIDs = validSaleItems.SelectMany(x => Providers.SpecialGroupData.GetSpecialGroupsForItem(entry, x.MasterID).Select(sgd => (string)sgd.ID)).Distinct().ToList();
            var retailDepartmentIDs = validSaleItems.Select(x => x.ItemDepartmentId).Distinct().ToList();

            var limitations = Providers.PaymentLimitationsData.GetList(entry, retailItemIDs, retailGroupIDs, specialGroupIDs, retailDepartmentIDs);

            var paymentTypeIDsThatWeHaveLimitationsFor = limitations.Select(x => x.TenderID).Distinct();
            var paymentTypeSaleLineItemPairs = paymentTypeIDsThatWeHaveLimitationsFor.SelectMany(x => validSaleItems, (paymentTypeId, saleLineItem) => new { paymentTypeId, saleLineItem });
            var allRestrictionsForEachSaleLineItem = paymentTypeSaleLineItemPairs
                                                            .Select(x => new
                                                                    {
                                                                        x.saleLineItem,
                                                                        restriction = GetRestrictionForItem(entry, new StorePaymentMethod { PaymentTypeID = x.paymentTypeId }, limitations, x.saleLineItem)
                                                                    });
            var validRestrictionsForEachSaleLineItem = allRestrictionsForEachSaleLineItem.Where(x => x.restriction.ID != Guid.Empty);
            var paymentTypesWithSaleLineItems = validRestrictionsForEachSaleLineItem.GroupBy(
                                                            x => x.restriction.TenderID, 
                                                            x => x.saleLineItem,
                                                            (tenderID, sli) => new { paymentTypeName = Providers.PaymentMethodData.Get(entry, tenderID)?.Text, saleLineItems = sli })
                                                            .Where(x => !string.IsNullOrEmpty(x.paymentTypeName));
            var paymentTypesWithMaximumPayableAmount = paymentTypesWithSaleLineItems.Select(x => new { x.paymentTypeName, value = x.saleLineItems.Distinct().Sum(valueThatCanBePaid) });

            // "Combined" is the total amount that can be payed with any payment type with limitations (ONE-10096)
            var combined = new
            {
                paymentTypeName = Resources.Combined,
                value = paymentTypesWithSaleLineItems.SelectMany(x => x.saleLineItems).Distinct().Sum(valueThatCanBePaid)
            };

            var roundingService = Interfaces.Services.RoundingService(entry);
            Func<decimal, string> formatValue = value => roundingService.RoundForDisplay(entry, value, true, true, retailTransaction.StoreCurrencyCode, CacheType.CacheTypeTransactionLifeTime);

            var titleAsHtml = $"<h1 style='font-size: 18px; font-style: bold; '>{Resources.MaximumAmountForPaymentType}</h1>";
            var paymentTypesWithMaximumPayableAmountAsHtml = paymentTypesWithMaximumPayableAmount
                                                .ToList()
                                                .Union(new[] { combined })
                                                .Select(x => $"<tr><td width='100px'><b>{x.paymentTypeName}</b>:</td><td align='right'> {formatValue(x.value)}</td></tr>")
                                                .Aggregate((c, n) => c + n);

            return $"<html><head></head><body style='font-family: Helvetica; font-size: 16px; color: #37474f;'><div class='TenderRestrictions'>{titleAsHtml}<table style='border: 0px;'>{paymentTypesWithMaximumPayableAmountAsHtml}</table></div></body></html>";
        }

        public virtual string GetRefundableAmountLimitedToPaymentTypeAsHtml(IConnectionManager entry, ISettings settings, IPosTransaction posTransaction)
        {
            var retailTransaction = posTransaction as RetailTransaction;

            if (retailTransaction == null)
            {
                return string.Empty;
            }

            Func<ISaleLineItem, decimal> valueThatCanBeRefund = sli => -sli.GetCalculatedNetAmount(settings.Store.DisplayBalanceWithTax);

            var validSaleItems = retailTransaction.ISaleItems.Where(x => !x.Voided && !x.IsAssemblyComponent);

            var paymentTransactionsOfTheOriginalReceipt = Providers.PaymentTransactionData.GetAll(entry, retailTransaction.OrgTransactionId);
            var tenderTypeIDsUsedInTheOriginalReceipt = paymentTransactionsOfTheOriginalReceipt.Select(x => x.TenderType).Distinct();
            var storePaymentsThatSupportRefundOnlyWithTheSamePaymentType = 
                tenderTypeIDsUsedInTheOriginalReceipt
                .SelectMany(x => Providers.StorePaymentMethodData.GetRecords(entry, new RecordIdentifier(retailTransaction.OrgStore, x), true, CacheType.CacheTypeTransactionLifeTime))
                .Where(x => x.ForceRefundToThisPaymentType);
            // storePaymentLimitations contains limitations attached to those payment types supporting refund only with the same payment type (let's call it "restricted refund")
            var storePaymentLimitations = storePaymentsThatSupportRefundOnlyWithTheSamePaymentType.SelectMany(x => x.PaymentLimitations);

            // Each line item that has been payed with a payment type that has limitations has its property LimitationMasterID set (aka it is not Guid.Empty)
            // We can use this information to determine which item lines were payed with payments with limitations
            // We only need the lines that were payed with payment types whose refund is limited to the same payment type
            // So we join the payable line items (validSaleItems) with the limitations used in the receipt (storePaymentLimitation), we group the resulted data by tender type name and
            // we summarize the value that can be paid for each pair (line item, payment limitation) aka (line item, payment type with limitation that only support refund with the same payment type)
            var refundableAmountLimitedToPaymentType = 
                validSaleItems
                .Join(  storePaymentLimitations, 
                        saleItem => saleItem.LimitationMasterID, 
                        storePaymentLimitation => storePaymentLimitation.LimitationMasterID, 
                        (saleItem, storePaymentLimitation) => new { saleItem, storePaymentLimitation })
                .Select(x => new
                {
                    InheritedName = (string)storePaymentsThatSupportRefundOnlyWithTheSamePaymentType.First(y => y.StoreAndTenderTypeID == new RecordIdentifier(retailTransaction.OrgStore, x.storePaymentLimitation.TenderTypeID)).InheritedName,
                    Value = valueThatCanBeRefund(x.saleItem)
                })
                .GroupBy(x => x.InheritedName)
                .Select(x => new { TenderTypeName = x.Key, Value = x.Sum(group => group.Value) });

            var other = new
            {
                TenderTypeName = Resources.Other,
                Value = validSaleItems.Sum(valueThatCanBeRefund) - refundableAmountLimitedToPaymentType.Sum(x => x.Value)
            };

            var roundingService = Interfaces.Services.RoundingService(entry);
            Func<decimal, string> formatValue = value => roundingService.RoundForDisplay(entry, value, true, true, retailTransaction.StoreCurrencyCode, CacheType.CacheTypeTransactionLifeTime);

            var refundableAmountLimitedToPaymentTypeAsHtml = refundableAmountLimitedToPaymentType
                                                                .ToList()
                                                                .Union(new[] { other })
                                                                .Select(x => $"<tr><td width='100px'><b>{x.TenderTypeName}</b>:</td><td align='right'> {formatValue(x.Value)}</td></tr>")
                                                                .Aggregate((c, n) => c + n);

            var titleAsHtml = $"<h1 style='font-size: 18px; font-style: bold; '><b>{Resources.RefundableAmountLimitedToPaymentType}</h1>";

            return $"<html><head></head><body style='font-family: Helvetica; font-size: 16px; color: #37474f;'>{titleAsHtml}<table style='border: 0px;'>{refundableAmountLimitedToPaymentTypeAsHtml}</table></body></html>";
        }

        private void CalculatePayableAmountForReturnSale(IConnectionManager entry, ISettings settings, IPosTransaction posTransaction, RetailTransaction retailTransaction, SaleLineItem lineItem, PaymentMethodLimitation restriction, RecordIdentifier tenderTypeID, ref decimal payableAmount)
        {
            if (!RecordIdentifier.IsEmptyOrNull(restriction.LimitationMasterID) && restriction.LimitationMasterID == lineItem.LimitationMasterID)
            {
                payableAmount += lineItem.GetCalculatedNetAmount(settings.Store.StorePriceSetting == Store.StorePriceSettingsEnum.PricesIncludeTax);
            }
            else if (RecordIdentifier.IsEmptyOrNull(restriction.LimitationMasterID) && !RecordIdentifier.IsEmptyOrNull(lineItem.LimitationMasterID))
            {
                var forceRefundWithTheSamePaymentType = Providers.StorePaymentMethodData.GetForceRefundToThisPaymentType(entry, entry.CurrentStoreID, lineItem.LimitationMasterID);

                if (!forceRefundWithTheSamePaymentType)
                {
                    payableAmount += lineItem.GetCalculatedNetAmount(settings.Store.StorePriceSetting == Store.StorePriceSettingsEnum.PricesIncludeTax);
                }
            }
        }

        public virtual void SplitLines(IConnectionManager entry, ISettings settings, IPosTransaction posTransaction, StorePaymentMethod paymentMethod, int paymentIndex)
        {
            // Each item that hasn't been paid for already is marked with PaymentIndexToBeUpdated. We must see how many items can be fully paid with the given amount
            // and split the last remaining item.
            if (InitialCheck(entry, settings, posTransaction, paymentMethod) == TenderRestrictionResult.NoTenderRestrictions)
            {
                return;
            }            

            IRetailTransaction retailTransaction = (IRetailTransaction)posTransaction;

            // We shouldn't split lines for customer orders
            if(!retailTransaction.CustomerOrder.Empty())
            {
                return;
            }

            decimal currentTotalAmount = 0m;
            decimal paidAmount = retailTransaction.GetTenderItem(paymentIndex).Amount;
            
            bool shouldClearRestriction = false;
            List<ISaleLineItem> originalItems = retailTransaction.SaleItems.Where(w => !RecordIdentifier.IsEmptyOrNull(w.LimitationMasterID) && w.PaymentIndex == paymentIndex && !w.IsAssemblyComponent).OrderBy(p => p.NetAmountWithTax).ToList();
            for (int i = 0; i < originalItems.Count; i++)
            {
                ISaleLineItem item = originalItems[i];

                // This means that this is the item that breaks the threshold for the payment and the total sum of the items is exactly the same as the paid amount.
                // In that case we don't want to split them up but we want to unmark the rest of the items.
                decimal roundedNetAmountWithTax = Services.Interfaces.Services.RoundingService(entry).Round(entry, item.GetCalculatedNetAmount(true), settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                if (!shouldClearRestriction && (paidAmount == currentTotalAmount + roundedNetAmountWithTax))
                {
                    shouldClearRestriction = true;
                    continue;
                }

                //This means that this is the item that breaks the threshold for the payment but the current item costs more than the paid amount can cover. This means that we have to:
                // 1: Split the item in two
                // 2: Unmark the rest of the items since the amount tendered only covers all items up to this point.
                if (!shouldClearRestriction && (paidAmount < currentTotalAmount + roundedNetAmountWithTax))
                {
                    ISaleLineItem newItem = (SaleLineItem)item.Clone();
                    retailTransaction.Add(newItem);

                    decimal splitFactor = (item.PaymentAmount - currentTotalAmount) / roundedNetAmountWithTax;

                    item.Quantity = item.Quantity * splitFactor;
                    item.UnitQuantity = item.Quantity * item.UnitQuantityFactor;
                    newItem.Quantity = newItem.Quantity - item.Quantity;
                    newItem.UnitQuantity = newItem.Quantity * item.UnitQuantityFactor;
                    newItem.LimitationSplitParentLineId = item.LineId;
                    item.LimitationSplitChildLineId = newItem.LineId;

                    if (item.IsAssembly && !item.IsAssemblyComponent)
                    {
                        List<ISaleLineItem> componentsToSplit = retailTransaction.SaleItems.Where(x => x.IsAssemblyComponent && x.LinkedToLineId == item.LineId).ToList();
                        for(int j = 0; j < componentsToSplit.Count; j++)
                        {
                            ISaleLineItem sl = componentsToSplit[j];
                            ISaleLineItem newComponentItem = (SaleLineItem)sl.Clone();
                            retailTransaction.Add(newComponentItem);

                            sl.Quantity = sl.Quantity * splitFactor;
                            sl.UnitQuantity = sl.Quantity * sl.UnitQuantityFactor;
                            newComponentItem.Quantity = newComponentItem.Quantity - sl.Quantity;
                            newComponentItem.UnitQuantity = newComponentItem.Quantity * sl.UnitQuantityFactor;
                            newComponentItem.LimitationSplitParentLineId = sl.LineId;
                            sl.LimitationSplitChildLineId = newComponentItem.LineId;
                            newComponentItem.LinkedToLineId = newItem.LineId; //Link to split parent line
                        }
                    }

                    // Clear tender restriction information from the item
                    bool prevTaxExempt = newItem.TaxExempt;

                    ClearRestrictionFromItem(newItem, posTransaction);

                    if (prevTaxExempt && !newItem.TaxExempt)
                    {
                        if (settings.Store.KeyedInPriceIncludesTax && newItem.PriceOverridden)
                        {
                            // The original item was tax exempted AND the original price was keyed in manually. This means we need
                            // to set the originally keyed in price so that we get the correct tax calculations
                            newItem.PriceWithTax = newItem.PriceOverrideAmount;
                        }
                    }                    

                    shouldClearRestriction = true;                             
                }
                else
                {
                    if (shouldClearRestriction)
                    {
                        ClearRestrictionFromItem(item, posTransaction);
                    }
                    else
                    {
                        // Keep looking for items
                        currentTotalAmount += roundedNetAmountWithTax;
                    }
                }                                
            }

            if(shouldClearRestriction)
            {
                Interfaces.Services.TransactionService(entry).RecalculatePriceTaxDiscount(entry, posTransaction, true);
                Interfaces.Services.CalculationService(entry).CalculateTotals(entry, posTransaction);
            }
        }

        private void ClearRestrictionFromItem(ISaleLineItem item, IPosTransaction posTranasction)
        {
            IRetailTransaction retailTransaction = (IRetailTransaction)posTranasction;

            item.LimitationMasterID = RecordIdentifier.Empty;
            item.LimitationCode = "";
            item.PaymentIndex = Constants.EmptyPaymentIndex;
            item.TaxExempt = retailTransaction.TaxExempt;
            item.TaxExemptionCode = retailTransaction.TransactionTaxExemptionCode;

            if(item.IsAssembly && !item.IsAssemblyComponent)
            {
                foreach (ISaleLineItem sl in retailTransaction.SaleItems.Where(x => x.IsAssemblyComponent && x.LinkedToLineId == item.LineId))
                {
                    sl.LimitationMasterID = RecordIdentifier.Empty;
                    sl.LimitationCode = "";
                    sl.PaymentIndex = Constants.EmptyPaymentIndex;
                    sl.TaxExempt = retailTransaction.TaxExempt;
                    sl.TaxExemptionCode = retailTransaction.TransactionTaxExemptionCode;
                }
            }
        }

        /// <summary>
        /// Compresses a given sale item and its child items if they can be merged together. Recursively continues down the parent/child chain if we encounter a child
        /// item that has payment on it. If a child item can be merged into its parent item it is added to the <paramref name="itemsToRemove"/> list.
        /// </summary>
        /// <param name="currentParentItem">The current parent item</param>
        /// <param name="posTransaction">The transaction that the item belongs to</param>
        /// <param name="itemsToRemove">The list of merged child items that should be removed</param>
        private void CompressSplitItems(ISaleLineItem currentParentItem, IPosTransaction posTransaction, List<ISaleLineItem> itemsToRemove)
        {
            IRetailTransaction retailTransaction = (IRetailTransaction)posTransaction;

            // This is the final item in the chain. This happens if the second-to-last item has a payment
            if (currentParentItem.LimitationSplitChildLineId < 0)
            {
                return;
            }

            if (currentParentItem.PaymentIndex > 0)
            {
                if (currentParentItem.LimitationSplitChildLineId > 0)
                {
                    // Current item cannot be merged, continue down the chain
                    CompressSplitItems(retailTransaction.GetItem(currentParentItem.LimitationSplitChildLineId), retailTransaction, itemsToRemove);
                }
            }
            else
            {                    
                ISaleLineItem splitChildItem = retailTransaction.GetItem(currentParentItem.LimitationSplitChildLineId);

                // Current parent and the child item can be merged
                while(splitChildItem.PaymentIndex < 0)
                {
                    currentParentItem.Quantity += splitChildItem.Quantity;
                    splitChildItem.Comment = "";
                    itemsToRemove.Add(splitChildItem);

                    // Check if we can continue down the list
                    if (splitChildItem.LimitationSplitChildLineId > 0)
                    {
                        splitChildItem = retailTransaction.GetItem(splitChildItem.LimitationSplitChildLineId);
                        splitChildItem.LimitationSplitParentLineId = currentParentItem.LineId;
                        splitChildItem.LineId--;
                        currentParentItem.LimitationSplitChildLineId = splitChildItem.LineId;                            
                    }
                    else
                    {
                        //We've reached the end because the child item was the last in the chain
                        currentParentItem.LimitationSplitChildLineId = -1;
                        currentParentItem.Comment = "";

                        return;
                    }
                }
                // We continue down the chain with the child as the new parent item                
                CompressSplitItems(splitChildItem, retailTransaction, itemsToRemove);                    
            }
        }
    }
}