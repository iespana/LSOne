using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;


namespace LSOne.DataLayer.DataProviders.KitchenDisplaySystem
{
    public interface IKitchenDisplayAggregateProfileData : IDataProvider<AggregateProfile>
    {
        /// <summary>
        /// Get the aggregate profile with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the aggregate profile</param>
        /// <returns>The aggregate profile with the given ID</returns>
        AggregateProfile Get(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Get a list of all aggregate profiles
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of all aggregate profiles</returns>
        List<AggregateProfile> GetList(IConnectionManager entry);
    }
}