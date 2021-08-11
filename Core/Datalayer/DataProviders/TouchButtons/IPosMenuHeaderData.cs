using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.TouchButtons
{
    public interface IPosMenuHeaderData : IDataProvider<PosMenuHeader>, ISequenceable
    {
        /// <summary>
        /// Returns a list of menu headers that are using a specific style ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="styleID">The style ID to check</param>
        /// <returns>A list of menu header using the style ID</returns>
        List<PosMenuHeader> AreUsingStyle(IConnectionManager entry, RecordIdentifier styleID);

        /// <summary>
        /// Gets a list of all POS Menu headers
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of PosMenuHeader objects containing all pos menu header records</returns>
        List<PosMenuHeader> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of all POS Menu headers
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="menuType">The type of menu to get</param>
        /// <returns>A list of PosMenuHeader objects containing all pos menu header records</returns>
        List<PosMenuHeader> GetList(IConnectionManager entry, MenuTypeEnum menuType);

        /// <summary>
        /// Gets a list of all POS Menu headers ordered by the specific field
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="filter">The filter options</param>
        /// <returns>A list of PosMenuHeader objects conataining all pos menu header records, ordered by the chosen field</returns>
        List<PosMenuHeader> GetList(IConnectionManager entry, PosMenuHeaderFilter filter);

        /// <summary>
        /// Gets as pos menu header with the given Guid
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="guid">The Guid of the pos menu header to get</param>
        /// <param name="cacheType">The type of cache to be used</param>
        /// <returns>A PosMenuHeader object containing the pos menu header with the given Guid</returns>
        PosMenuHeader GetByGuid(IConnectionManager entry, Guid guid, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Checks if a pos menu header with the given GUID exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="guid">The guid of the restaurant station to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        bool GuidExists(IConnectionManager entry, Guid guid);

        PosMenuHeader Get(IConnectionManager entry, RecordIdentifier ID, CacheType cacheType = CacheType.CacheTypeNone);
    }
}