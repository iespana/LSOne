using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Currencies
{
    public interface ICashDenominatorData : IDataProvider<CashDenominator>
    {
        /// <summary>
        /// Get a list of all cash declarations for a given currency. The list is sorted by a column index, and ordered ascending or descending based on the sortedBackwards parameter.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="currencyCode">The currency code to get cash declarations for</param>
        /// <param name="sortColumn">The column index to sort by. The columns are ["TYPE", "AMOUNT"]</param>
        /// <param name="sortedBackwards">Wheter to sort by ascending order or not</param>
        /// <returns>A list of all cash declarations for a given currency</returns>
        List<CashDenominator> GetCashDenominators(IConnectionManager entry, RecordIdentifier currencyCode, int sortColumn, bool sortedBackwards);

        List<CashDenominator> GetBills(IConnectionManager entry, RecordIdentifier currencyCode);
    }
}