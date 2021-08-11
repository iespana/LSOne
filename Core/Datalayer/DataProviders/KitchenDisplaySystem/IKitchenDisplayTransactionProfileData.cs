using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.KitchenDisplaySystem
{
    public interface IKitchenDisplayTransactionProfileData : IDataProvider<KitchenServiceProfile>
    {
        /// <summary>
        /// Gets a list of all profiles
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all profiles</returns>
        /// 
        List<KitchenServiceProfile> GetList(IConnectionManager entry);

        KitchenServiceProfile Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone);
    }
}
