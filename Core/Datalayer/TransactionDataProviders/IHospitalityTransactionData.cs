using System.Collections.Generic;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface IHospitalityTransactionData : IDataProviderBase<HospitalityTransaction>
    {
        /// <summary>
        /// Gets the last split transaction created.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The store ID</param>
        /// <param name="terminalID">The terminal ID</param>
        /// <param name="tableID">The table ID</param>
        /// <param name="guest">The guest number</param>
        /// <param name="hospitalityType">The selected hospitality</param>
        /// <param name="cache">The cache selection</param>
        /// <returns>Returns the last split hospitality transaction</returns>
        HospitalityTransaction GetLastSplitTransaction(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier tableID, RecordIdentifier guest, RecordIdentifier hospitalityType, RecordIdentifier splitID, CacheType cache = CacheType.CacheTypeNone);

        /// <summary>
        /// Gets the last split transaction created.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="hospitalityTransactionInfo">The hospitality transaction info</param>
        /// <param name="cache">The cache selection</param>
        /// <returns>Returns the last split hospitality transaction.</returns>
        HospitalityTransaction GetLastSplitTransaction(IConnectionManager entry, RecordIdentifier hospitalityTransactionInfo, CacheType cache = CacheType.CacheTypeNone);

        /// <summary>
        /// Gets a hospitality transaction with a specific combined id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id (TransactionID, StoreID, TerminalID, TableID, Guest, HospitalityType)</param>
        /// <param name="cache">The selected cache</param>
        /// <returns>The <see cref="HospitalityTransaction "/> found</returns>
        HospitalityTransaction Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone);

        /// <summary>
        /// Gets a list of hospitality transactions with a specific combined ID. Any part of the ID can be empty
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The combined ID (TransactionID, StoreID, TerminalID, TableID, Guest, HospitalityType)</param>
        /// <param name="cache">The selected cache</param>
        /// <returns>List of <see cref="HospitalityTransaction"/></returns>
        List<HospitalityTransaction> GetList(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone);

        /// <summary>
        /// Returns true if the records the exists.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The Hospitality transaction to check</param>
        /// <returns><c>true</c> if the record exists, <c>false</c> otherwise</returns>
        bool RecordExists(IConnectionManager entry, HospitalityTransaction transaction);

        /// <summary>
        /// Deletes the specified Hospitality transaction.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The transaction to be deleted</param>
        void Delete(IConnectionManager entry, HospitalityTransaction transaction);

        /// <summary>
        /// Saves the Hospitality transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The transaction to be saved</param>
        void Save(IConnectionManager entry, HospitalityTransaction transaction);
    }
}