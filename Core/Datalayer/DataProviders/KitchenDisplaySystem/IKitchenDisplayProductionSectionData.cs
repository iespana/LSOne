using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;


namespace LSOne.DataLayer.DataProviders.KitchenDisplaySystem
{
    public interface IKitchenDisplayProductionSectionData : IDataProvider<KitchenDisplayProductionSection>
    {
        /// <summary>
        /// Get the production section with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the production section</param>
        /// <returns>The production section with the given ID</returns>
        KitchenDisplayProductionSection Get(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Get a list of all production sections
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of all production sections</returns>
        List<KitchenDisplayProductionSection> GetList(IConnectionManager entry);

        /// <summary>
        /// Get a list of all production sections, represented as MasterID objects
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of all production sections as MasterID objects</returns>
        List<MasterIDEntity> GetMasterIDList(IConnectionManager entry);

        /// <summary>
        /// Check if a production section with the given code exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="code">The code of production section</param>
        /// <returns>True if a production sections with the given code exists</returns>
        bool Exists(IConnectionManager entry, string code);
    }
}