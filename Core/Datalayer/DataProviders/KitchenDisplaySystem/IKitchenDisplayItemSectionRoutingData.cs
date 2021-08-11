using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;


namespace LSOne.DataLayer.DataProviders.KitchenDisplaySystem
{
    public interface IKitchenDisplayItemSectionRoutingData : IDataProvider<KitchenDisplayItemSectionRouting>
    {
        /// <summary>
        /// Get the item section routing with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the item section routing</param>
        /// <returns>The item section routing with the given ID</returns>
        KitchenDisplayItemSectionRouting Get(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Get a list of all item section routing
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of all item section routings</returns>
        List<KitchenDisplayItemSectionRouting> GetList(IConnectionManager entry);

        /// <summary>
        /// Checks if a item routing section with the given arguements exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="type">The item type</param>
        /// <param name="itemId">The item ID</param>
        /// <param name="sectionId">The production secion ID</param>
        /// <returns>True if an item section routing with the given attributes exists</returns>
        bool Exists(IConnectionManager entry, KitchenDisplayItemSectionRouting.ItemTypeEnum type, RecordIdentifier itemId, RecordIdentifier sectionId);

        /// <summary>
        /// Remove a production section from item section routings
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sectionId">ID of the production section to remove</param>
        void RemoveSection(IConnectionManager entry, RecordIdentifier sectionId);
    }
}