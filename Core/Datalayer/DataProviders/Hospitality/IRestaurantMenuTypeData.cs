using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Hospitality
{
    public interface IRestaurantMenuTypeData : IDataProvider<RestaurantMenuType>
    {
        /// <summary>
        /// Gets a list of all restaurant menu types
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of RestaurantMenuType objects containing all restaurant menu types</returns>
        List<RestaurantMenuType> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of all restaurant menu types for the given restaurant id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The ID of the restaurant</param>
        /// <returns>A list of RestaurantMenuType objects containing all restaurant menu types for the given restaurant id</returns>
        List<RestaurantMenuType> GetList(IConnectionManager entry, RecordIdentifier restaurantID);

        /// <summary>
        /// Cheks if a given restaurant menu type and codeOnPos combination exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantMenuTypeID">The id of the restaurant meny type to check for</param>
        /// <param name="codeOnPos">The code on pos value to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        bool Exists(IConnectionManager entry, RecordIdentifier restaurantMenuTypeID, string codeOnPos);

        RestaurantMenuType Get(IConnectionManager entry, RecordIdentifier restaurantMenuTypeID);
    }
}