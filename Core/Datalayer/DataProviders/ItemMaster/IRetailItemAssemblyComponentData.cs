using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;

namespace LSOne.DataLayer.DataProviders.ItemMaster
{
    public interface IRetailItemAssemblyComponentData : IDataProvider<RetailItemAssemblyComponent>
    {
        /// <summary>
        /// Get a retail item component
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="componentID">ID of the component</param>
        /// <returns></returns>
        RetailItemAssemblyComponent Get(IConnectionManager entry, RecordIdentifier componentID);

        /// <summary>
        /// Get a list of components for an assembly
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="assemblyID">Assembly ID</param>
        /// <returns></returns>
        List<RetailItemAssemblyComponent> GetList(IConnectionManager entry, RecordIdentifier assemblyID);

        /// <summary>
        /// Returns true if an assembly has at least 1 component
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="assemblyID">Assembly ID</param>
        /// <returns></returns>
        bool HasComponents(IConnectionManager entry, RecordIdentifier assemblyID);

        /// <summary>
        /// Get a list of all assembly items that an item is a component of (if any).
        /// Only assemblies that are enabled and not archived are considered.
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="itemId">Item ID</param>
        /// <returns>A list containing the ID and name of all assembly items that contain the item as a component</returns>
        List<DataEntity> GetAssemblyItemsForComponentItem(IConnectionManager entry, RecordIdentifier itemId);

        /// <summary>
        /// Given a list of retail item IDs, returns the the ID and name of the items that are components of assembly items,
        /// where the assembly containing the component is enabled and not archived.
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="itemIds">List of item ID to check</param>
        /// <returns>A list containing the ID and name of all items that are components of active assemblies</returns>
        List<DataEntity> WhichItemsAreComponentsOfAssemblies(IConnectionManager entry, List<RecordIdentifier> itemIds);

        /// <summary>
        /// Check if adding a new component to an assembly will cause a circular reference of assembly items
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="assemblyID">Assembly ID in which the new item will be added</param>
        /// <param name="itemID">Assembly item ID which contains the assembly</param>
        /// <param name="itemIDToCheck">Item ID that will be added to the assembly</param>
        /// <returns>True if the new item will cause a circulare reference, false otherwise.</returns>
        bool AssemblyItemCausesCircularReference(IConnectionManager entry, RecordIdentifier assemblyID, RecordIdentifier itemID, RecordIdentifier itemIDToCheck);
    }
}
