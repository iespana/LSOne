using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Financials
{
    public interface IIncomeExpenseAccountData : IDataProvider<IncomeExpenseAccount>, ISequenceable
    {
        List<DataEntity> GetCopyList(IConnectionManager entry);

        IncomeExpenseAccount GetLists(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Gets all income-expense accounts
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>All income-expense accounts</returns>
        List<IncomeExpenseAccount> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets all income-expense accounts of the given type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="accountType">The type of account to get</param>
        /// <returns>All income-expense accounts</returns>
        List<IncomeExpenseAccount> GetList(IConnectionManager entry, IncomeExpenseAccount.AccountTypeEnum accountType);

        List<IncomeExpenseAccount> GetListForStore(IConnectionManager entry, IncomeExpenseAccount.AccountTypeEnum accountType, RecordIdentifier storeid);
        void Save(IConnectionManager entry, IncomeExpenseAccount incomeExpenseAccount, bool isNew);

        IncomeExpenseAccount Get(IConnectionManager entry, RecordIdentifier incomeExpenseAccountID);
    }
}