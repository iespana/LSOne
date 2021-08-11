using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.DataLayer.BusinessObjects.StoreManagement.Validity;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;


namespace LSOne.DataLayer.DataProviders.StoreManagement
{
    public interface ITerminalData : IDataProvider<Terminal>, ISequenceable
    {
        List<TerminalListItem> GetList(IConnectionManager entry);
        string GetName(IConnectionManager entry, RecordIdentifier terminalID);
        List<TerminalListItem> GetList(IConnectionManager entry, RecordIdentifier storeID);

        List<TerminalListItem> GetAvailableList(IConnectionManager entry, RecordIdentifier storeID);
        List<DataEntity> GetHospitalityTerminalList(IConnectionManager entry, RecordIdentifier storeID);

        List<TerminalListItem> Search(IConnectionManager entry, RecordIdentifier id, string description,
            int maxCount);

        List<TerminalListItem> GetTerminals(IConnectionManager entry, RecordIdentifier storeid);

        /// <summary>
        /// Gets all terminals populated depending on the given <see cref="UsageIntentEnum"/>. Note that this populates a lot of data so do not use this
        /// unless you actually need fully populated terminal objects.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="usageIntent">Specifies how much extra data should be loaded</param>
        /// <returns></returns>
        List<Terminal> GetAllTerminals(IConnectionManager entry, UsageIntentEnum usageIntent);

        List<TerminalListItem> GetAllTerminals(IConnectionManager entry, bool sortAscending,
            TerminalListItem.SortEnum sortEnum);

        Terminal Get(IConnectionManager entry, RecordIdentifier terminalID, RecordIdentifier storeID, CacheType cache = CacheType.CacheTypeNone, UsageIntentEnum usageIntent = UsageIntentEnum.Normal);
        void PopulateTerminalData(IDataReader dr, Terminal terminal);
        void PopulateMinimal(IDataReader dr, Terminal terminal);
        SuspendedTransactionsStatementPostingEnum TerminalAllowsEOD(IConnectionManager entry, RecordIdentifier terminalID, RecordIdentifier storeID);

        /// <summary>
        /// Checks wether a terminal exists with the given terminal and store combination
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="terminalID">The terminal ID to look for</param>
        /// <param name="storeID">The store ID to look for</param>
        /// <returns></returns>
        bool Exists(IConnectionManager entry, RecordIdentifier terminalID, RecordIdentifier storeID);

        void MarkAsActivated(IConnectionManager entry, RecordIdentifier terminalID, RecordIdentifier storeID);

        void MarkAsActivated(IConnectionManager entry, Terminal terminal);

        void SetHardwareProfile(IConnectionManager entry, RecordIdentifier terminalID, RecordIdentifier storeID, RecordIdentifier profileID);

        void SetHardwareProfile(IConnectionManager entry, Terminal terminal, RecordIdentifier profileID);

        /// <summary>
        /// Checks if all terminals are valid, returning list of all terminal IDs and their validity report.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>list of all terminal IDs and their validity report</returns>
        List<TerminalValidity> CheckTerminalValidity(IConnectionManager entry);
    }
}