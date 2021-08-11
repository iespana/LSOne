using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.DataLayer.KDSBusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.DataProviders.KitchenDisplaySystem
{
    public interface IKitchenDisplayStationData : IDataProvider<KitchenDisplayStation>, ISequenceable
    {
        /// <summary>
        /// Gets all kitchen display stations
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of kitchen display station objects containing all kitchen display stations station</returns>
        List<KitchenDisplayStation> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets all kitchen display stations ordered by the specified field
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted </param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns>A list of all kitchen display stations, ordered by a chosen field</returns>
        List<KitchenDisplayStation> GetList(IConnectionManager entry, KitchenDisplayStationSortingEnum sortBy, bool sortBackwards);

        /// <summary>
        /// Gets a kitchen display station with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the kitchen display station to get</param>
        /// <param name="cacheType">Optional parameter to specify if cache may be used</param>
        /// <returns>A KitchenDisplayStation object containing the kitchen display station with the given ID</returns>
        KitchenDisplayStation Get(IConnectionManager entry, RecordIdentifier id, CacheType cacheType = CacheType.CacheTypeNone);

        List<KitchenDisplayStation> GetList(IConnectionManager entry, string storeId);

        void ClearRoutingTo(IConnectionManager entry);
    }
}
