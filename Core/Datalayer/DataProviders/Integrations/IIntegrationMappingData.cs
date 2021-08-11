using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Integrations;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Integrations
{
    public interface IIntegrationMappingData : IDataProviderBase<IntegrationMapping>
    {
        /// <summary>
        /// Gets an integration mapping by the table and external id
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="tableName"></param>
        /// <param name="externalID">The ID of the integration mapping to get</param>
        /// <returns>An integration mapping for the table and ID, or null</returns>
        IntegrationMapping Get(IConnectionManager entry, IntegrationMapping.MappingEnum tableName, RecordIdentifier externalID);

        /// <summary>
        /// Gets a list of data entities containing IDs and names of all integration mappings for a specific table
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="tableName">The name of the table to get entries for</param>
        /// <returns>A list of integration mappings for the table</returns>
        List<IntegrationMapping> GetList(IConnectionManager entry, string tableName);

        /// <summary>
        /// Checks if an external id mapping exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="tableName">The table name for integration</param>
        /// <param name="externalID">The external ID</param>
        /// <returns>Whether an integration mapping exists in the database</returns>
        bool Exists(IConnectionManager entry, string tableName, RecordIdentifier externalID);

        /// <summary>
        /// Touches the integration mapping in the database, i.e. updates the UPDATED field
        /// </summary>
        /// <remarks>Requires the 'Edit integration mapping' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="integrationMapping">The integration mapping to touch</param>
        void Touch(IConnectionManager entry, IntegrationMapping integrationMapping);

        /// <summary>
        /// Deletes an integration mapping
        /// </summary>
        /// <remarks>Requires the 'Edit integration mapping' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="integrationMapping">The mapping to delete</param>
        void Delete(IConnectionManager entry, IntegrationMapping integrationMapping);

        void Save(IConnectionManager entry, IntegrationMapping mapping);

        List<MappingRecords> GetMappings(IConnectionManager entry, RecordIdentifier mappingEntity);
        MappingRecords GetMapping(IConnectionManager entry, RecordIdentifier MappingId, string entityIdString);
        void MapToExternalEntity(IConnectionManager entry, RecordIdentifier internalID, RecordIdentifier mappingId, string entityIdString);
        Guid? GetMappingEntity(IConnectionManager entry, RecordIdentifier systemId, string entityId);
        Guid CreateMappingEntity(IConnectionManager entry, RecordIdentifier systemId, string entityId, string tableName = "");
    }
}