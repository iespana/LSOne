using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Hospitality
{
    public interface IDiningTableLayoutData : IDataProvider<DiningTableLayout>, ISequenceable
    {
        /// <summary>
        /// Gets a list of all Dining Table Layouts
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of all DiningTableLayouts</returns>
        List<DiningTableLayout> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets all DiningTablesLayouts for the specific Restaurant and Sales Type combination
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The id for the Restaurant</param>
        /// <param name="salesTypeID">The id for the Sales Type</param>
        /// <returns>A list of all DiningTableLayouts for the specidfic Restaurant and Sales Type</returns>
        List<DiningTableLayout> GetList(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier salesTypeID);

        /// <summary>
        /// Gets a specific DiningTableLayout with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the diningTableLayout to get</param>
        /// <param name="cacheType"></param>
        /// <returns>The DiningTableLayout matching the given ID</returns>
        DiningTableLayout Get(IConnectionManager entry, RecordIdentifier diningTableLayoutID, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Gets the maximum number of tables (diningtablerows * diningtablecolumns) that is allowed for the given dining table layout ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the dining table layout</param>
        /// <returns>The maximum number of tables</returns>
        int GetMaximumNumberOfTables(IConnectionManager entry, RecordIdentifier diningTableLayoutID);

        /// <summary>
        /// Checks if a DiningTableLayout with a given LayoutID exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="layoutID">The LayoutID to check for</param>
        /// <returns>True if the DiningTableLayout exists, false otherwise</returns>
        bool LayoutIDExists(IConnectionManager entry, RecordIdentifier layoutID);

        /// <summary>
        /// Deletes all dining table layouts belonging to a specific hospitality type (restaurantid + salestype)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The ID of the restaurant</param>
        /// <param name="salesType">The ID of the sales type</param>
        void DeleteForHospitalityType(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier salesType);

        /// <summary>
        /// Checks if a dining table layout with the given restaurant id and sales type combination exists.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The id of the restaurant</param>
        /// <param name="salesTypeID">The id of the sales type</param>
        /// <returns>True if the given combination exists, false otherwise</returns>
        bool RestaurantSalesTypeCombinationExists(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier salesTypeID);

        /// <summary>
        /// Copies information from another dining table layout into the DiningTableLayout object.
        /// The fields that are copied are: NoOfScreens, StartingTableNo, NoOfDiningTables, EndingTableNo, DiningTableRows, DiningTableColumns, CurrentLayout
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the dining table layout to copy from</param>
        /// <param name="diningTableLayout">The DininTableLayout object to copy the data into</param>
        void CopyDataFromLayout(IConnectionManager entry, RecordIdentifier diningTableLayoutID, DiningTableLayout diningTableLayout);
    }
}