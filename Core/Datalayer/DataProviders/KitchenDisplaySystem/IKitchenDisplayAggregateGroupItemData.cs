using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;


namespace LSOne.DataLayer.DataProviders.KitchenDisplaySystem
{
    public interface IKitchenDisplayAggregateGroupItemData : IDataProvider<AggregateGroupItem>
    {
        /// <summary>
        /// Get the aggregate group item with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the aggregate group item</param>
        /// <returns>The aggregate group item with the given ID</returns>
        AggregateGroupItem Get(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Get a list of all aggregate group items
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of all aggregate group items</returns>
        List<AggregateGroupItem> GetList(IConnectionManager entry);

        /// <summary>
        /// Get a list of all aggregate group items belonging to a certain aggregate group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="aggregateGroupID">The ID of the aggregate group</param>
        /// <returns>A list of all aggregate group items from the aggregate group</returns>
        List<AggregateGroupItem> GetList(IConnectionManager entry, RecordIdentifier aggregateGroupID);

        /// <summary>
        /// Get list of all items connected to an aggregate group.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="aggregateGroupID">The ID of the aggregate group</param>
        List<DataEntity> ItemsConnected(IConnectionManager entry, RecordIdentifier aggregateGroupID);

        /// <summary>
        /// Get list of all retail groups connected to an aggregate group.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="aggregateGroupID">The ID of the aggregate group</param>
        List<DataEntity> RetailGroupsConnected(IConnectionManager entry, RecordIdentifier aggregateGroupID);

        /// <summary>
        /// Get list of all special groups connected to an aggregate group.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="aggregateGroupID">The ID of the aggregate group</param>
        List<DataEntity> SpecialGroupsConnected(IConnectionManager entry, RecordIdentifier aggregateGroupID);

        /// <summary>
        /// Delete all items from a aggreagate group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="aggregateGroupID">The ID of the aggregate group</param>
        void DeleteByGroup(IConnectionManager entry, RecordIdentifier aggregateGroupID);

        /// <summary>
        /// Update the item connection
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="aggregateGroupItem">The new aggregate group item</param>
        /// <param name="oldItemID">ID of the old connection item</param>
        void UpdateItem(IConnectionManager entry, AggregateGroupItem aggregateGroupItem, RecordIdentifier oldItemID);

        /// <summary>
        /// Get a list of all aggregate group items for kds. All rows have been converted to items.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        List<AggregateGroupItem> GetForKds(IConnectionManager entry);
    }
}