using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface ITaxTransactionData : IDataProviderBase<TaxItem>
    {
        void Insert(IConnectionManager entry, ISaleLineItem saleLineItem, TaxItem taxItem, int counter);

        /// <summary>
        /// Get all tax items connected to a transaction id for a given line number
        /// </summary>
        /// <param name="entry">Entry into the database</param>     
        /// <param name="transactionId">The transaction id on the tax items </param> 
        /// <param name="lineNum">The line number to be fetched</param>  
        /// <param name="trans">The current transaction - information regarding store and terminal used</param>
        /// <param name="sumOfTaxRatePercent">Returns sum of all tax rates</param>
        /// <returns></returns>   
        List<TaxItem> GetTaxItems(IConnectionManager entry, RecordIdentifier transactionId, int lineNum, IRetailTransaction trans, out decimal sumOfTaxRatePercent);

        void PopulateTaxLinesForTransaction(RetailTransaction trans);
    }
}