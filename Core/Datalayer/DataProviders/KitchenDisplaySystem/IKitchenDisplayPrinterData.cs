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
    public interface IKitchenDisplayPrinterData : IDataProvider<KitchenDisplayPrinter>, ISequenceable
    {
        /// <summary>
        /// Gets all kitchen printers
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of kitchen display printers</returns>
        List<KitchenDisplayPrinter> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets all kitchen printers ordered by the specified field
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted </param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns>A list of all kitchen printers, ordered by a chosen field</returns>
        List<KitchenDisplayPrinter> GetList(IConnectionManager entry, KitchenPrinterSortingEnum sortBy, bool sortBackwards);

        /// <summary>
        /// Gets a kitchen printer with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the kitchen printer to get</param>
        /// <param name="cacheType">Optional parameter to specify if cache may be used</param>
        /// <returns>A Kitchen printer with the given ID</returns>
        KitchenDisplayPrinter Get(IConnectionManager entry, RecordIdentifier id, CacheType cacheType = CacheType.CacheTypeNone);
    }
}
