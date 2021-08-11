using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.TouchButtons
{
    public interface IPosMenuLineData : IDataProvider<PosMenuLine>, ISequenceable
    {
        /// <summary>
        /// Gets a list of all pos menu lines.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>List of all pos menu lines</returns>
        List<PosMenuLine> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of pos menu line list items for the given pos menu ID. This is intended for displaying pos menu lines only.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuID">The ID of the menu to get the menu lines for</param>
        /// <returns></returns>
        List<PosMenuLineListItem> GetListItems(IConnectionManager entry, RecordIdentifier posMenuID);

        /// <summary>
        /// Returns a list of menu buttons that are using a specific style
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="styleID">The style ID to look for</param>
        /// <returns>A list of menu buttons that are using the style ID</returns>
        List<PosMenuLine> AreUsingStyle(IConnectionManager entry, RecordIdentifier styleID);

        /// <summary>
        /// Gets a list of all pos menu lines for a given pos menu ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuID">The ID of the menu to get the menu lines for</param>
        /// <param name="cacheType">The type of cache to be used</param>
        /// <returns>A list of PosMenuLine objects containing all menu lines for the given menu ID</returns>
        List<PosMenuLine> GetList(IConnectionManager entry, RecordIdentifier posMenuID, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Gets the next KeyNo value for the given menu ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuID">The id of the pos menu (the header ID)</param>
        /// <returns>The next KeyNo, which is the highest key number + 1</returns>
        int GetNextKeyNo(IConnectionManager entry, RecordIdentifier posMenuID);

        /// <summary>
        /// Deletes all pos menu lines for a given menu header ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuHeaderID">The header ID</param>
        void DeleteForHeaderID(IConnectionManager entry, RecordIdentifier posMenuHeaderID);

        void SaveOrder(IConnectionManager entry, PosMenuLineListItem posMenuLineListOrder);

        PosMenuLine Get(IConnectionManager entry, RecordIdentifier ID);

        /// <summary>
        /// Get all operations available for a pos menu header. Note that those can be both POS operations and KDS bump-bar operations.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuHeaderID">The header ID</param>
        List<int> GetOperations(IConnectionManager entry, RecordIdentifier posMenuHeaderID);
    }
}