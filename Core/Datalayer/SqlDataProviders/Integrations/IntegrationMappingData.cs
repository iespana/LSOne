using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Integrations;
using LSOne.DataLayer.DataProviders.Integrations;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Integrations
{
    /// <summary>
    /// Data provider class for integration mappings
    /// </summary>
    public class IntegrationMappingData : SqlServerDataProviderBase, IIntegrationMappingData
    {
        private static void PopulateIntegrationMapping(IDataReader dr, IntegrationMapping integrationMapping)
        {
            integrationMapping.ID = (string)dr["EXTERNALID"];
            integrationMapping.Text = (string)dr["TABLENAME"];
            integrationMapping.InternalID = (string)dr["INTERNALID"];
            integrationMapping.Created = (DateTime)dr["CREATED"];
            object updated = dr["UPDATED"];
            integrationMapping.Updated = updated == null || updated == DBNull.Value ? DateTime.MinValue : (DateTime)updated;
        }

        private static void PopulateMappingRecords(IDataReader dr, MappingRecords mappingRecords)
        {
            mappingRecords.Id = (Guid)dr["ID"];
            mappingRecords.MappingEntity = (Guid)dr["MAPPINGENTITY"];
            mappingRecords.InternalId = (Guid)dr["INTERNALID"];
            mappingRecords.ExternalId = (string)dr["EXTERNALID"];
        }

        /// <summary>
        /// Gets an integration mapping by the table and external id
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="tableName"></param>
        /// <param name="externalID">The ID of the integration mapping to get</param>
        /// <returns>An integration mapping for the table and ID, or null</returns>
        public virtual IntegrationMapping Get(IConnectionManager entry, IntegrationMapping.MappingEnum tableName, RecordIdentifier externalID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select TABLENAME,EXTERNALID,INTERNALID,CREATED,UPDATED " +
                    "from RBOINTEGRATIONMAPPINGS " +
                    "where TABLENAME = @tableName AND EXTERNALID = @externalID AND DATAAREAID = @dataAreaId";

                MakeParam(cmd, "tableName", tableName.ToString());
                MakeParam(cmd, "externalID", (string)externalID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var result = Execute<IntegrationMapping>(entry, cmd, CommandType.Text, PopulateIntegrationMapping);
                return (result.Count > 0) ? result[0] : null;
            }
        }

        /// <summary>
        /// Gets a list of data entities containing IDs and names of all integration mappings for a specific table
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="tableName">The name of the table to get entries for</param>
        /// <returns>A list of integration mappings for the table</returns>
        public virtual List<IntegrationMapping> GetList(IConnectionManager entry, string tableName)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                     "Select TABLENAME,EXTERNALID,INTERNALID,CREATED,UPDATED " +
                     "from RBOINTEGRATIONMAPPINGS " +
                     "where TABLENAME = @tableName AND DATAAREAID = @dataAreaId ORDER BY EXTERNALID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "tableName", tableName);

                return Execute<IntegrationMapping>(entry, cmd, CommandType.Text, PopulateIntegrationMapping);
            }
        }

        /// <summary>
        /// Checks if an external id mapping exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="tableName">The table name for integration</param>
        /// <param name="externalID">The external ID</param>
        /// <returns>Whether an integration mapping exists in the database</returns>
        public virtual bool Exists(IConnectionManager entry, string tableName, RecordIdentifier externalID)
        {
            return RecordExists(entry, "RBOINTEGRATIONMAPPINGS", new[] { "TABLENAME", "EXTERNALID" }, new RecordIdentifier(tableName) { SecondaryID = externalID });
        }

        /// <summary>
        /// Saves a given integration mapping to the database
        /// </summary>
        /// <remarks>Requires the 'Edit integration mapping' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="integrationMapping">The integration mappping to save</param>
        public virtual void Save(IConnectionManager entry, IntegrationMapping integrationMapping)
        {
            var statement = new SqlServerStatement("RBOINTEGRATIONMAPPINGS");

            ValidateSecurity(entry, BusinessObjects.Permission.EditIntegrationMapping);

            if (!Exists(entry, integrationMapping.Text, integrationMapping.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("EXTERNALID", (string)integrationMapping.ID);
                statement.AddKey("TABLENAME", integrationMapping.Text);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("EXTERNALID", (string)integrationMapping.ID);
                statement.AddCondition("TABLENAME", integrationMapping.Text);
            }

            statement.AddField("INTERNALID", (string)integrationMapping.InternalID);
            statement.AddField("CREATED", integrationMapping.Created, SqlDbType.DateTime);
            if (integrationMapping.Updated != DateTime.MinValue)
                statement.AddField("UPDATED", integrationMapping.Updated, SqlDbType.DateTime);

            entry.Connection.ExecuteStatement(statement);
        }


        /// <summary>
        /// Touches the integration mapping in the database, i.e. updates the UPDATED field
        /// </summary>
        /// <remarks>Requires the 'Edit integration mapping' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="integrationMapping">The integration mapping to touch</param>
        public virtual void Touch(IConnectionManager entry, IntegrationMapping integrationMapping)
        {
            var statement = new SqlServerStatement("RBOINTEGRATIONMAPPINGS");

            ValidateSecurity(entry, BusinessObjects.Permission.EditIntegrationMapping);

            statement.StatementType = StatementType.Update;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("EXTERNALID", (string)integrationMapping.ID);
            statement.AddCondition("TABLENAME", integrationMapping.Text);

            integrationMapping.Updated = DateTime.Now;
            statement.AddField("UPDATED", integrationMapping.Updated, SqlDbType.DateTime);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Deletes an integration mapping
        /// </summary>
        /// <remarks>Requires the 'Edit integration mapping' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="integrationMapping">The mapping to delete</param>
        public virtual void Delete(IConnectionManager entry, IntegrationMapping integrationMapping)
        {
            DeleteRecord(entry, "RBOINTEGRATIONMAPPINGS", new[] { "TABLENAME", "EXTERNALID" },
                new RecordIdentifier(integrationMapping.Text) { SecondaryID = integrationMapping.ID }, BusinessObjects.Permission.EditIntegrationMapping);
        }


        public virtual List<MappingRecords> GetMappings(IConnectionManager entry, RecordIdentifier mappingEntity)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"
                                    SELECT  
	                                    ID,
	                                    MAPPINGENTITY,
	                                    INTERNALID,
	                                    EXTERNALID
                                      FROM 
                                        MAPPINGINTERNALEXTERNALENTITIES
                                      WHERE 	                                    
	                                    MAPPINGENTITY = @MappedEntity ";

                MakeParam(cmd, "MappedEntity", (Guid)mappingEntity);

                return Execute<MappingRecords>(entry, cmd, CommandType.Text, PopulateMappingRecords);
            }
        }

        public virtual MappingRecords GetMapping(IConnectionManager entry, RecordIdentifier MappingId, string entityIdString)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"
                                    SELECT  
	                                    ID,
	                                    MAPPINGENTITY,
	                                    INTERNALID,
	                                    EXTERNALID
                                      FROM 
                                        MAPPINGINTERNALEXTERNALENTITIES
                                      WHERE 
	                                    EXTERNALID = @ExternalID AND 
	                                    MAPPINGENTITY = @MappedEntity ";

                MakeParam(cmd, "ExternalID", entityIdString);
                MakeParam(cmd, "MappedEntity", (Guid)MappingId);

                var records = Execute<MappingRecords>(entry, cmd, CommandType.Text, PopulateMappingRecords);
                if (records.Count > 0)
                {
                    return records[0];
                }
                return null;
            }
        }

        public virtual void MapToExternalEntity(IConnectionManager entry, RecordIdentifier internalID, RecordIdentifier mappingId, string entityIdString)
        {
            var statement = new SqlServerStatement("MAPPINGINTERNALEXTERNALENTITIES");

            MappingRecords mapping = GetMapping(entry, mappingId, entityIdString);

            if (mapping == null)
            {
                statement.StatementType = StatementType.Insert;
                mapping.Id = Guid.NewGuid();
                statement.AddKey("Id", (Guid)mapping.Id, SqlDbType.UniqueIdentifier);
            }
            else if (mapping.InternalId == internalID)
            {
                return;
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("Id", (Guid)mapping.Id, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("MAPPINGENTITY", (Guid)mappingId, SqlDbType.UniqueIdentifier);
            statement.AddField("INTERNALID", (Guid)internalID, SqlDbType.UniqueIdentifier);
            statement.AddField("EXTERNALID", entityIdString, SqlDbType.SmallInt);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual Guid? GetMappingEntity(IConnectionManager entry, RecordIdentifier systemId, string entityId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"
                                SELECT  
                                    ID,
                                    TABLENAME,
                                    MAPPINGSYSTEM,
                                    EXTERNALENTITY
                                FROM MAPPINGENTITY 
                    WHERE 
                        MAPPINGSYSTEM = @MappingSystem AND 
                        EXTERNALENTITY = @externalEntity";

                MakeParam(cmd, "MappingSystem", (Guid)systemId);
                MakeParam(cmd, "externalEntity", entityId);

                var records = Execute(entry, cmd, CommandType.Text, "Id");
                if (records.Count > 0)
                {
                    return (Guid)records[0];
                }
                return null;
            }
        }

        public virtual Guid CreateMappingEntity(IConnectionManager entry, RecordIdentifier systemId, string entityId, string tableName = "")
        {
            var statement = new SqlServerStatement("MAPPINGENTITY");

            RecordIdentifier mappingId = GetMappingEntity(entry, systemId, entityId);

            if (mappingId == null || mappingId.IsEmpty)
            {
                statement.StatementType = StatementType.Insert;
                mappingId = Guid.NewGuid();
                statement.AddKey("Id", (Guid)mappingId, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("Id", (Guid)mappingId, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("TABLENAME", tableName);
            statement.AddField("MAPPINGSYSTEM", (Guid)systemId, SqlDbType.UniqueIdentifier);
            statement.AddField("EXTERNALENTITY", entityId);

            entry.Connection.ExecuteStatement(statement);

            return (Guid)mappingId;
        }


    }
}
