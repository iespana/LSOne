using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Triggers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IItemTriggers
    {

        #region ItemSale
        
        /// <summary>
        /// Triggered prior to adding the item to the transaction, but after all item properties have been fetched from the database.
        /// Note that the item's dimensions have not been checked.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="results"></param>
        /// <param name="saleLineItem"></param>
        /// <param name="posTransaction"></param>
        void PreSale(IConnectionManager entry, PreTriggerResults results, ISaleLineItem saleLineItem, IPosTransaction posTransaction);

        /// <summary>
        /// Triggered after adding the item to the transaction.
        /// Prices and discounts have been calculated but the event is triggered before 
        /// processing any infocodes or linked items.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction</param>
        void PostSale(IConnectionManager entry, IPosTransaction posTransaction);

        /// <summary>
        /// Triggered if the sale was cancelled within the Item sale operation. The reason for the cancelation is detailed in the <seealso cref="ItemSaleCancelledEnum"/> parameter.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="cancelledReason">The reason for the cancellation of the item sale</param>
        void PostSaleCancelled(IConnectionManager entry, IPosTransaction posTransaction, ItemSaleCancelledEnum cancelledReason);

        /// <summary>
        /// Triggered prior to adding the item to the transaction, but after all item properties have been fetched from the database.
        /// Note that the item's dimensions have not been checked.
        /// WARNING - this sale is triggered through a forecourt sale. 
        /// In most countries, the manipulation of such a sale item is considered illegal. 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="results"></param>
        /// <param name="saleLineItem"></param>
        /// <param name="posTransaction"></param>
        void PreForecourtSale(IConnectionManager entry, PreTriggerResults results, IBaseSaleItem saleLineItem, IPosTransaction posTransaction);

        /// <summary>
        /// Triggered after adding the item to the transaction.
        /// Prices and discounts have been calculated but the event is triggered before 
        /// processing any infocodes or linked items.
        /// WARNING - this sale is triggered through a forecourt sale. 
        /// In most countries, the manipulation of such a sale item is considered illegal. 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        void PostForecourtSale(IConnectionManager entry, IPosTransaction posTransaction);

        #endregion

        #region ReturnItem
       
        /// <summary>
        /// Note!
        /// The return operation only sets the till to a "return state".  It is the item sale operation that really 
        /// returns the item (sells it with a negative qty).  Programming the "return triggers" therefore only affects
        /// whether the till can enter the so called "return state".
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="results"></param>
        /// <param name="posTransaction"></param>
        void PreReturnItem(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction);

        /// <summary>
        /// Triggered after an item has been returned.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        void PostReturnItem(IConnectionManager entry, IPosTransaction posTransaction);

        #endregion

        #region VoidItem
        /// <summary>
        /// Summary pending.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="results"></param>
        /// <param name="posTransaction"></param>
        /// <param name="lineId"></param>
        void PreVoidItem(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction, int lineId);
        
        /// <summary>
        /// Summary pending.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        /// <param name="lineId"></param>
        void PostVoidItem(IConnectionManager entry, IPosTransaction posTransaction, int lineId);

        #endregion

        #region Set Quantity

        /// <summary>
        /// Summary pending.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="results"></param>
        /// <param name="saleLineItem"></param>
        /// <param name="posTransaction"></param>
        /// <param name="lineId"></param>
        void PreSetQty(IConnectionManager entry, PreTriggerResults results, IBaseSaleItem saleLineItem, IPosTransaction posTransaction, int lineId);

        /// <summary>
        /// Summary pending.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        /// <param name="saleLineItem"></param>
        void PostSetQty(IConnectionManager entry, IPosTransaction posTransaction, IBaseSaleItem saleLineItem);

        #endregion

        #region Price override

        /// <summary>
        /// Summary pending.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="results"></param>
        /// <param name="posTransaction"></param>
        /// <param name="lineId"></param>
        void PrePriceOverride(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction, int lineId);

        /// <summary>
        /// Summary pending.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        void PostPriceOverride(IConnectionManager entry, IPosTransaction posTransaction);

        #endregion

        #region Unit of measure

        /// <summary>
        /// Summary pending.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="results"></param>
        /// <param name="posTransaction"></param>
        /// <param name="lineId"></param>
        void PreChangeUnitOfMeasure(IConnectionManager entry, PreTriggerResults results, IPosTransaction posTransaction, int lineId);

        /// <summary>
        /// Summary pending.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        void PostChangeUnitOfMeasure(IConnectionManager entry, IPosTransaction posTransaction);

        #endregion Unit of measure
    }
}
