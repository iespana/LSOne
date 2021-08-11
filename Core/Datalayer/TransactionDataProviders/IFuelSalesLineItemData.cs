using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface IFuelSalesLineItemData : IDataProviderBase<DataEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="id">TransactionID, LineNumber</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        FuelSalesLineItem Get(IConnectionManager entry, RecordIdentifier id, RetailTransaction transaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">TransactionID, LineNumber, Terminal, Store</param>
        /// <returns></returns>
        bool Exists(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="item">TransactionID, LineNumber,Terminal, Store</param>
        void Delete(IConnectionManager entry, FuelSalesLineItem item);

        void Save(IConnectionManager entry, FuelSalesLineItem item, RetailTransaction transaction);
    }
}