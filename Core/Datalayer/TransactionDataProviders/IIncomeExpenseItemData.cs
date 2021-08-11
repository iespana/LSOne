using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.IncomeExpenseItem;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface IIncomeExpenseItemData : IDataProviderBase<DataEntity>
    {
        IncomeExpenseItem Get(IConnectionManager entry, RetailTransaction transaction, decimal lineNumber);
        List<IncomeExpenseItem> Get(IConnectionManager entry, RetailTransaction transaction);
        bool Exists(IConnectionManager entry, RecordIdentifier id);
        void Delete(IConnectionManager entry, IncomeExpenseItem item);
        void Save(IConnectionManager entry, IncomeExpenseItem item);
    }
}