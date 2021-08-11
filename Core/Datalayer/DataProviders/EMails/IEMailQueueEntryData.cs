using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.EMails;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.EMails
{
    public interface IEMailQueueEntryData : IDataProviderBase<EMailQueueEntry>
    {
        /// <summary>
        /// Gets the specified entry.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="unsentOnly">If true, only fetch unsent items</param>
        /// <returns>An instance of <see cref="EMailQueueEntry"/></returns>
        int Count(IConnectionManager entry, bool unsentOnly);

        /// <summary>
        /// Gets the specified entry.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="unsentOnly">If true, only fetch unsent items</param>
        /// <param name="topEntries">Maximum number of entries to fetch</param>
        /// <param name="maxAttempts">Only fetch items where Attempts is less than maxAttempts</param>
        /// <returns>An instance of <see cref="EMailQueueEntry"/></returns>
        List<EMailQueueEntry> GetList(IConnectionManager entry, bool unsentOnly, int topEntries, int maxAttempts);

        /// <summary>
        /// Gets the specified entry.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="ID">ID of email entry</param>
        /// <returns>An instance of <see cref="EMailQueueEntry"/></returns>
        EMailQueueEntry Get(IConnectionManager entry, int ID);

        /// <summary>
        /// Gets the specified entry.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="unsentOnly">If true, only fetch unsent items</param>
        /// <param name="index">Index of first entry</param>
        /// <param name="maxEntries">Maximum number of entries to fetch</param>
        /// <param name="sort">Sorting to perform</param>
        /// <returns>An instance of <see cref="EMailQueueEntry"/></returns>
        List<EMailQueueEntry> GetIndexedList(IConnectionManager entry, bool unsentOnly, int index, int maxEntries, EMailSortEnum sort);

        void Delete (IConnectionManager entry, int emailID);

        void Update(IConnectionManager entry, RecordIdentifier emailQueueEntryID, 
            DateTime sent, int attempts, string lastError);

        void Save(IConnectionManager entry, EMailQueueEntry emailQueueEntry);

        void Truncate(IConnectionManager entry, DateTime created);
    }
}