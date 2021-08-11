using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;
using System.Collections.Generic;

namespace LSOne.DataLayer.DataProviders.Omni
{
    [LSOneUsage(CodeUsage.LSCommerce)]
    public interface IOmniJournalData : IDataProviderBase<OmniJournal>, ISequenceable
    {
        /// <summary>
        /// Saves an instance of an LS Commerce journal
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="journal">The journal to be saved</param>
        void Save(IConnectionManager entry, OmniJournal journal);

        /// <summary>
        /// Saves an instance of an LS Commerce journal
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="journal">The journal to be updated</param>
        void UpdateOmniJournalStatus(IConnectionManager entry, RecordIdentifier journalID, OmniJournalStatus status);

        /// <summary>
        /// Retrieves information about a specific journal. If the journal doesn't exist an empty object is returned
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="journalID">The journal ID to retrieve</param>
        /// <returns></returns>
        OmniJournal Get(IConnectionManager entry, RecordIdentifier journalID);

        /// <summary>
        /// Returns true if the journal exists
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="journalID">The journal ID to check</param>
        /// <returns></returns>
        bool Exists(IConnectionManager entry, RecordIdentifier journalID);

        /// <summary>
        /// Import the document lines from an XML attached to an LS Commerce journal
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="omniJournalID">LS Commerce journal ID</param>
        /// <returns></returns>
        int ImportOmniLinesFromXML(IConnectionManager entry, RecordIdentifier omniJournalID);

        /// <summary>
        /// Import the goods receiving lines from an XML attached to an LS Commerce journal
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="omniJournalID">LS Commerce journal ID</param>
        /// <returns></returns>
        int ImportOmniGoodsReceivingLinesFromXML(IConnectionManager entry, RecordIdentifier omniJournalID);

        /// <summary>
        /// Gets a list of LS Commerce journals filtered by status
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="status">Journal status</param>
        /// <returns></returns>
        List<OmniJournal> GetOmniJournals(IConnectionManager entry, OmniJournalStatus status);

        /// <summary>
        /// Delete an LS Commerce journal
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="omniJournalID">Journal ID to delete</param>
        void Delete(IConnectionManager entry, RecordIdentifier omniJournalID);

        /// <summary>
        /// Increment the retry counter of an omni journal
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="omniJournalID">ID of the LS Commerce journal</param>
        void IncrementRetryCounter(IConnectionManager entry, RecordIdentifier omniJournalID);
    }
}
