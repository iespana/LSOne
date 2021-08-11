using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.TouchButtons
{
    public interface ITouchLayoutData : IDataProvider<TouchLayout>, ISequenceable
    {
        List<DataEntity> GetList(IConnectionManager entry);
        List<DataEntity> GetList(IConnectionManager entry,string sort);

        /// <summary>
        /// Returns a list of all touch layouts ordered by the specified field
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns>An ordered list of touch layouts</returns>
        List<TouchLayout> GetTouchLayouts(IConnectionManager entry, TouchLayoutSorting sortBy, bool sortBackwards);

        TouchLayout GetByGuid(IConnectionManager entry, Guid guid, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Returns a lit of the first 5 button grids
        /// </summary>
        /// <returns></returns>
        List<DataEntity> GetButtonGrids();

        /// <summary>
        /// Gets the ID of the touch layout that should be used. The hierarchy of the layouts is as follows:
        ///  1. POS user
        ///  2. Active hospitality type
        ///  3. Terminal
        ///  4. Store        
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posUserID">The pos user</param>        
        /// <param name="hospitalitySalesTypeID">The ID of the sales type that is in use by the current hospitality type</param>
        /// <param name="terminalID">The terminal</param>
        /// <param name="storeID">The store</param>
        /// <returns>RecordIdentifier.Empty if no touch layout is defined, the ID of the correct touch layout otherwise</returns>
        RecordIdentifier GetPOSTouchLayoutID(IConnectionManager entry,RecordIdentifier posUserID, RecordIdentifier hospitalitySalesTypeID, RecordIdentifier terminalID, RecordIdentifier storeID);

        bool GuidExists(IConnectionManager entry, Guid guid);

        /// <summary>
        /// Copies the given TouchLayout and returns the ID of the newly created layout
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="newLayout">The layout to copy from</param>
        /// <param name="copyFromID">The ID of the layout to copy from</param>
        /// <returns></returns>
        RecordIdentifier CreateNewAndCopyFrom(IConnectionManager entry, TouchLayout newLayout, RecordIdentifier copyFromID);

        /// <summary>
        /// Used to save only the header information. This is used when the TillLayoutDesigner has handled saving
        /// the rest of the layout information and we only need to handle the saving of header information.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="layout">The layout to save</param>
        void SaveHeader(IConnectionManager entry, TouchLayout layout);

        TouchLayout Get(IConnectionManager entry, RecordIdentifier ID, CacheType cache = CacheType.CacheTypeNone);
    }
}