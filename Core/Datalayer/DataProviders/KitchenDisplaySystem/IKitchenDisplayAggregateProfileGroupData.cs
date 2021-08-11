using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;


namespace LSOne.DataLayer.DataProviders.KitchenDisplaySystem
{
    public interface IKitchenDisplayAggregateProfileGroupData : IDataProvider<AggregateProfileGroup>
    {
        /// <summary>
        /// Get the aggregate profile group with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="groupID">The group ID of the aggregate profile group</param>
        /// <returns>The aggregate profile group with the given ID</returns>
        AggregateProfileGroup Get(IConnectionManager entry, RecordIdentifier groupID);

        /// <summary>
        /// Get a list of all aggregate profile groups
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of all aggregate profile groups</returns>
        List<AggregateProfileGroup> GetList(IConnectionManager entry);

        /// <summary>
        /// Get a list of all aggregate profile groups belonging to a certain aggregate profile
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="aggregateProfileId">The ID of the aggregate profile</param>
        /// <returns>A list of all aggregate profile groups in the aggregate profile</returns>
        List<AggregateProfileGroup> GetList(IConnectionManager entry, RecordIdentifier aggregateProfileId);

        /// <summary>
        /// Delete all aggregate groups connected to a certain aggregate profile
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="aggregateProfileId">The ID of the aggregate profile</param>
        void DeleteByAggregateProfile(IConnectionManager entry, RecordIdentifier aggregateProfileId);

        /// <summary>
        /// Remove a group from profile
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the aggregate group</param>
        void RemoveGroupFromProfile(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Check if a connection exists between a profile and group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the aggregate group</param>
        /// <returns></returns>
        bool RelationExists(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Creates a profile to group relation
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="aggregateProfileGroup">Aggregate group to use</param>
        void CreateGroupRelation(IConnectionManager entry, AggregateProfileGroup aggregateProfileGroup);

        /// <summary>
        /// Get all aggregate groups, formatted for the KDS
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        List<AggregateProfileGroup> GetForKDS(IConnectionManager entry);
    }
}