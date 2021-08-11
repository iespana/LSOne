using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;

namespace LSOne.DataLayer.DataProviders.ItemMaster
{
    public interface IRetailItemAssemblyData : IDataProvider<RetailItemAssembly>
    {
        /// <summary>
        /// Get an assembly (regardless whether it is active or not)
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="assemblyID">ID of the item for which to get the assembly</param>
        /// <returns></returns>
        RetailItemAssembly Get(IConnectionManager entry, RecordIdentifier assemblyID);

        /// <summary>
        /// Get a list of assemblies for an item
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="itemID">Item ID</param>
        /// <returns></returns>
        List<RetailItemAssembly> GetList(IConnectionManager entry, RecordIdentifier itemID);

        /// <summary>
        /// Search for retail item assemblies
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="searchFilter">Search filter</param>
        /// <returns></returns>
        List<RetailItemAssembly> Search(IConnectionManager entry, RetailItemAssemblySearchFilter searchFilter);

        /// <summary>
        /// Set an assembly to be enabled or disabled
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="assemblyID">Assembly ID for which to change status</param>
        /// <param name="enabled">Enabled or disabled status for the assembly</param>
        void SetEnabled(IConnectionManager entry, RecordIdentifier assemblyID, bool enabled);

        /// <summary>
        /// Get an active assembly for an item
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="itemID">ID of the item for which to get the assembly</param>
        /// <param name="storeID">ID of the store for which to get the assembly</param>
        /// <returns></returns>
        RetailItemAssembly GetAssemblyForItem(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier storeID, bool onlyGetActive = true);

        /// <summary>
        /// Get all assemblies
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <returns></returns>
        List<DataEntity> GetAll(IConnectionManager entry);
    }
}
