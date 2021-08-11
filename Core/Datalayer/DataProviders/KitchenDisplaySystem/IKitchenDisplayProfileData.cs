using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.DataProviders.KitchenDisplaySystem
{
    public interface IKitchenDisplayProfileData : IDataProvider<KitchenDisplayProfile>
    {
        /// <summary>
        /// Gets all kitchen printers
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of kitchen display profiles</returns>
        List<KitchenDisplayProfile> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a kitchen printer with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the displayprofile to get</param>
        /// <param name="cacheType">Optional parameter to specify if cache may be used</param>
        /// <returns>A Kitchen printer with the given ID</returns>
        KitchenDisplayProfile Get(IConnectionManager entry, RecordIdentifier id);
    }
}
