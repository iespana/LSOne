using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Fiscal;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Fiscal
{
    public interface IFiscalData : IDataProvider<FiscalTrans>
    {
        /// <summary>
        /// Get signature of last transaction if it is for new transaction or signature of previous transaction if it is to verify signature of current transaction.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="store">ID of the store</param>
        /// <param name="terminal">ID of the POS terminal</param>
        /// <param name="transactionId">ID of the transaction</param>
        /// <param name="newTrans">Indicator if it is for creating new transaction or verify signature of existing transaction</param>
        /// <returns>Signature of last transaction if it is for new transasction or signature of previous last transaction if it is to verify signature of the selected transaction</returns>
        string GetLastSignature(IConnectionManager entry, string store, string terminal, string transactionId, bool newTrans);

        /// <summary>
        /// Get transaction signature and related details from RBOTRANSACTIONFISCALTRANS table
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">Unique ID of RBOTRANSACTIONFISCALTRANS table record</param>
        /// <returns>Transaction signature details from RBOTRANSACTIONFISCALTRANS table</returns>
        FiscalTrans Get(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Get the list of transaction signature from RBOTRANSACTIONFISCALTRANS
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>List of transaction signature details from RBOTRANSACTIONFISCALTRANS table</returns>
        List<FiscalTrans> GetList(IConnectionManager entry);

        /// <summary>
        /// Get record of transaction signature from RBOTRANSACTIONFISCALTRANS table based on the filtering
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchFilter">Search filter</param>
        /// <param name="totalReceiptsMatching">Total number of matching records</param>
        /// <returns>List of transaction signature details from RBOTRANSACTIONFISCALTRANS table based on the filtering</returns>
        List<FiscalTrans> Find(IConnectionManager entry, FiscalTransSearchFilter searchFilter, out int totalReceiptsMatching);
    }
}
