using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.Hospitality.ListItems;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Hospitality
{
    public interface IHospitalityTypeData : IDataProvider<HospitalityType>
    {
        /// <summary>
        /// Gets all HospitalityTypes
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of HospitalityListItems</returns>
        List<HospitalityTypeListItem> GetHospitalityTypes(IConnectionManager entry);

        /// <summary>
        /// Gets the default hospitality type for the given store
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The id of the store</param>
        /// <returns>A list of HospitalityListItems</returns>
        string GetDefaultHospitalitySalesTypes(IConnectionManager entry, string storeID);

        /// <summary>
        /// Returns a list if all hospitality types
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of HospitalityType objects</returns>
        List<HospitalityType> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets all HospitalityTypes with sorting
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">Which field to sort by</param>
        /// <param name="sortBackwards">Sort ascending or descending</param>
        /// <returns>A list of HospitalityListItems</returns>
        List<HospitalityTypeListItem> GetHospitalityTypes(IConnectionManager entry, HospitalityTypeSorting sortBy, bool sortBackwards);

        HospitalityType Get(IConnectionManager entry, RecordIdentifier hospitalityTypeID);

        /// <summary>
        /// Gets a hospitality type for a given restaurant and sales type ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The ID of the restaurant</param>
        /// <param name="salesTypeID">The ID of the sales type</param>
        /// <returns>A HospitalityType object for the given restaurant and sales type combination</returns>
        HospitalityType Get(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier salesTypeID);

        /// <summary>
        /// Returns the next Sequence value for the given hospitality type.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="hospitalityTypeID">The ID of the hospitality type. Note that only the Restaurant ID field is requiered</param>
        /// <returns>The next Sequence number</returns>
        int GetNextSequence(IConnectionManager entry, RecordIdentifier hospitalityTypeID);

        /// <summary>
        /// Gets a list of hospitality types for a given restaurant
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The id of the restaurant</param>
        /// <returns>A list of all hospitality types for the given restaurant</returns>
        List<HospitalityTypeListItem> GetHospitalityTypesForRestaurant(IConnectionManager entry, RecordIdentifier restaurantID);

        /// <summary>
        /// Gets a lit of hospitality types for a given restaurant and overview combination. This is commonly used when 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The id of the restaurant</param>
        /// <param name="overview">The type of overview to get</param>
        /// <returns>A list of all hospitality types for the </returns>
        List<HospitalityTypeListItem> GetHostpitalityTypesForRestaurant(IConnectionManager entry, RecordIdentifier restaurantID, HospitalityType.OverviewEnum overview = HospitalityType.OverviewEnum.ButtonFormat);

        /// <summary>
        /// Checks if a hospitality type with the given restaurant id and sales type combination exists.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The id of the restaurant</param>
        /// <param name="salesTypeID">The id of the sales type</param>
        /// <returns>True if the given combination exists, false otherwise</returns>
        bool RestaurantSalesTypeCombinationExists(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier salesTypeID);
    }
}