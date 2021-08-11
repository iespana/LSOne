using System;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;
using LSOne.Services.Interfaces.SupportClasses;

namespace LSOne.Triggers
{
    /// <summary>
    /// <example><code>
    /// // In order to get a copy of the last item added to the transaction, use the following code:
    /// LinkedListNode<SaleLineItem> saleItem = ((RetailTransaction)posTransaction).SaleItems.Last;
    /// // To remove the last line use:
    /// ((RetailTransaction)posTransaction).SaleItems.RemoveLast();
    /// </code></example>
    /// </summary>
    public class ItemTriggers : IItemTriggers
    {
        #region Constructor - Destructor

        public ItemTriggers(IConnectionManager entry)
        {
            
            // Get all text through the Translation function in the ApplicationLocalizer
            // TextID's for ItemTriggers are reserved at 50350 - 50399            
        }

        ~ItemTriggers()
        {

        }

        #endregion

        #region IItemTriggers Members

        public void PreSale(IConnectionManager entry, PreTriggerResults results, ISaleLineItem saleLineItem, IPosTransaction posTransaction)
        {
            try
            {
                if(saleLineItem.UsedForPriceCheck) //This should always return valid trigger results
                {
                    results.RunOperation = true;
                    entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Prior to the price check of an item...", "ItemTriggers.PreSale");
                }
                else
                {
                    entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Prior to the sale of an item...", "ItemTriggers.PreSale");
                }
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PostSale(IConnectionManager entry, IPosTransaction posTransaction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "After the sale of an item...", "ItemTriggers.PostSale");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        /// <summary>
        /// Triggered if the sale was cancelled within the Item sale operation. The reason for the cancelation is detailed in the <seealso cref="ItemSaleCancelledEnum"/> parameter.
        /// </summary>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="cancelledReason">The reason for the cancellation of the item sale</param>
        public void PostSaleCancelled(IConnectionManager entry, IPosTransaction posTransaction, ItemSaleCancelledEnum cancelledReason)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "After the sale of an item was cancelled: " + Enum.GetName(typeof(ItemSaleCancelledEnum), cancelledReason), "ItemTriggers.PostSaleCancelled");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PreForecourtSale(IConnectionManager entry, PreTriggerResults results, IBaseSaleItem baseSaleItem, IPosTransaction posTransaction)
        {
            try
            {

                // *************************
                // WARNING
                // This sale is triggered through a forecourt sale. 
                // In most countries, the manipulation of such a sale item is considered illegal. 
                // *************************

                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Prior to the forecourt sale of an item...", "ItemTriggers.PreForecourtSale");
                //N.B. the item is a base sale item - cast it into a fuel sale line item.
                //LSRetailPosis.Transaction.Line.SaleItem.FuelSalesLineItem item = (LSRetailPosis.Transaction.Line.SaleItem.FuelSalesLineItem)baseSaleItem;


            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PostForecourtSale(IConnectionManager entry, IPosTransaction posTransaction)
        {
            try
            {

                // *************************
                // WARNING
                // This sale is triggered through a forecourt sale. 
                // In most countries, the manipulation of such a sale item is considered illegal. 
                // *************************

                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "After the forecourt sale of an forecourt item...", "ItemTriggers.PostForecourtSale");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PreReturnItem(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Prior to entering return state...", "ItemTriggers.PreReturnItem");                
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            } 


        }

        public void PostReturnItem(IConnectionManager entry, IPosTransaction posTransaction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "After entering return state", "ItemTriggers.PostReturnItem");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            } 
        }


        public void PreVoidItem(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction, int lineId)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Before voiding an item", "ItemTriggers.PreVoidItem");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            } 
        }

        public void PostVoidItem(IConnectionManager entry, IPosTransaction posTransaction, int lineId)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "After voiding an item", "ItemTriggers.PostVoidItem");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            } 
        }

        public void PreSetQty(IConnectionManager entry, PreTriggerResults results, IBaseSaleItem saleLineItem, IPosTransaction posTransaction, int lineId)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Before setting the qty for an item", "ItemTriggers.PreSetQty");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PostSetQty(IConnectionManager entry, IPosTransaction posTransaction, IBaseSaleItem saleLineItem)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "After setting the qty from an item", "ItemTriggers.PostSetQty");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PrePriceOverride(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction, int lineId)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Before overriding the price on an item", "ItemTriggers.PrePriceOverride");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PostPriceOverride(IConnectionManager entry, IPosTransaction posTransaction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "After overriding the price of an item", "ItemTriggers.PostPriceOverride");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PreChangeUnitOfMeasure(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction, int lineId)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Before changing the unit of measure of an item", "ItemTriggers.PreChangeUnitOfMeasure");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public void PostChangeUnitOfMeasure(IConnectionManager entry, IPosTransaction posTransaction)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "After changing the unit of measure of an item", "ItemTriggers.PostChangeUnitOfMeasure");

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        #endregion                          
    }
}
