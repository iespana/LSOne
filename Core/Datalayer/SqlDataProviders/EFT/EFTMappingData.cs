using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.EFT;
using LSOne.DataLayer.BusinessObjects.Sequencable;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.EFT;
using LSOne.DataLayer.DataProviders.Sequences;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.EFT
{
    public class EFTMappingData : SqlServerDataProviderBase, IEFTMappingData
    {
        private static string BaseSelectString
        {
            get
            {
                return
                    "SELECT m.MAPPINGID," +
                    "ISNULL(m.TENDERTYPEID,'') as TENDERTYPEID," +
                    "ISNULL(t.NAME,'') as TENDERNAME," +
                    "ISNULL(m.CARDTYPEID,'') as CARDTYPEID," +
                    "ISNULL(C.NAME,'') as CARDTYPENAME," +
                    "m.SCHEMENAME," +
                    "m.ENABLED, " +
                    "m.CREATED, " +
                    "ISNULL(M.LOOKUPORDER,0) as LOOKUPORDER " +
                    "FROM RBOEFTMAPPING m " +
                    "LEFT OUTER JOIN RBOTENDERTYPETABLE T on m.TENDERTYPEID=T.TENDERTYPEID " +
                    "LEFT OUTER JOIN RBOTENDERTYPECARDTABLE C on m.CARDTYPEID=C.CARDTYPEID " +
                    "WHERE m.DATAAREAID = @dataAreaId ";
            }
        }

        private static void PopulateEFTMapping(IDataReader dr, EFTMapping mapping)
        {
            mapping.MappingID = (string)dr["MAPPINGID"];
            mapping.SchemeName = (string)dr["SCHEMENAME"];
            mapping.TenderTypeID = (string)dr["TENDERTYPEID"];
            mapping.CardTypeID = (string)dr["CARDTYPEID"];
            mapping.CardTypeName = (string)dr["CARDTYPENAME"];
            mapping.TenderTypeName = (string)dr["TENDERNAME"];
            mapping.Enabled = (Convert.ToInt16(dr["ENABLED"]) != 0);
            mapping.LookupOrder = (short)AsInt(dr["LOOKUPORDER"]);

            var created = dr["CREATED"];
            if (created == null || created == DBNull.Value)
            {
                mapping.Created = DateTime.MinValue;
            }
            else
            {
                mapping.Created = (DateTime) created;
            }
        }

        public virtual List<EFTMapping> GetList(IConnectionManager entry, bool includeDisabled = false)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSelectString;

                if (!includeDisabled)
                {
                    cmd.CommandText += "AND Enabled = 1 ";
                }

                cmd.CommandText += " ORDER BY LOOKUPORDER,SCHEMENAME";
                
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<EFTMapping>(entry, cmd, CommandType.Text, PopulateEFTMapping);
            }
        }

        /// <summary>
        /// Gets a mapping with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="mappingID">The ID of the mapping to get</param>
        /// <returns>The mapping with the given ID</returns>
        public virtual EFTMapping Get(IConnectionManager entry, RecordIdentifier mappingID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    " AND m.MAPPINGID = @mappingId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "mappingId", (string)mappingID);

                var res = Execute<EFTMapping>(entry, cmd, CommandType.Text, PopulateEFTMapping);
                if (res == null || res.Count == 0)
                    return null;

                return res[0];
            }
        }

        /// <summary>
        /// Gets a mapping for the specified scheme name (only if ENABLED = 1)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="schemeName">The name of the scheme to search for</param>
        /// <returns>The mapping with the given scheme name</returns>
        public virtual EFTMapping GetForScheme(IConnectionManager entry, string schemeName)
        {
            if (string.IsNullOrEmpty(schemeName))
                return null;

            using (var cmd = entry.Connection.CreateCommand())
            {

                cmd.CommandText =
                    BaseSelectString +
                    " AND m.SCHEMENAME = @schemeName AND m.ENABLED = 1";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "schemeName", schemeName);

                var res = Execute<EFTMapping>(entry, cmd, CommandType.Text, PopulateEFTMapping);
                if (res == null || res.Count == 0)
                    return null;

                return res[0];
            }
        }

        /// <summary>
        /// Gets a mapping for the specified scheme. If no such mapping is found, but a mapping for a '*' scheme
        /// is found, then copy that scheme, save it with the new name and return that mapping
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="schemeName">The name of the scheme to search for</param>
        /// <returns>The mapping with the given scheme name</returns>
        public virtual EFTMapping GetForSchemeWithFallback(IConnectionManager entry, string schemeName)
        {
            var mapping = GetForScheme(entry, schemeName);
            if (mapping == null)
            {
                mapping = GetForScheme(entry, "*");
                if (mapping != null)
                {
                    var newMapping = new EFTMapping
                        {
                            TenderTypeID = mapping.TenderTypeID,
                            CardTypeID = mapping.CardTypeID,
                            SchemeName = schemeName,
                            Enabled = true
                        };

                    // Don't require permissions for this method !
                    Save(entry, newMapping, false);
                    return newMapping;
                }
            }
            return mapping;
        }

        public virtual EFTMapping GetWhereSchemeIsInCardName(IConnectionManager entry, string cardName)
        {
            if (string.IsNullOrEmpty(cardName))
                return null;

            var mappings = Providers.EFTMappingData.GetList(entry);
            foreach (var eftMapping in mappings)
            {
                if (eftMapping.SchemeName == "*" || !eftMapping.Enabled)
                    continue;

                if (cardName.IndexOf(eftMapping.SchemeName, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    return eftMapping;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if an infocode with the given Id exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the infocode to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "RBOEFTMAPPING", "MAPPINGID", id);
        }

        /// <summary>
        /// Deletes the infocode with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the infocode to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "RBOEFTMAPPING", "MAPPINGID", id, BusinessObjects.Permission.EFTMappingEdit);
        }
        
        /// <summary>
        /// Saves an Infocode object to the database. If the record does not exist then a Insert is done, else it executes a update statement.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="mapping">The Infocode to be saved</param>
        public virtual void Save(IConnectionManager entry, EFTMapping mapping)
        {
            Save(entry, mapping, true);
        }

        /// <summary>
        /// Saves an Infocode object to the database. If the record does not exist then a Insert is done, else it executes a update statement.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="mapping">The Infocode to be saved</param>
        /// <param name="validateSecurity">If true, then edit permissions will be validated</param>
        public virtual void Save(IConnectionManager entry, EFTMapping mapping, bool validateSecurity)
        {
            var statement = new SqlServerStatement("RBOEFTMAPPING", true);

            if (validateSecurity)
                ValidateSecurity(entry, BusinessObjects.Permission.EFTMappingEdit);

            var isNew = false;
            if (mapping.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                mapping.ID = DataProviderFactory.Instance.
                    Get<INumberSequenceData, NumberSequence>()
                    .GenerateNumberFromSequence(entry, new EFTMappingData()); 
            }

            if (isNew || !Exists(entry, mapping.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("MAPPINGID", (string)mapping.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("MAPPINGID", (string)mapping.ID);
            }

            statement.AddField("TENDERTYPEID", (string)mapping.TenderTypeID);
            statement.AddField("CARDTYPEID", (string)mapping.CardTypeID);
            statement.AddField("SCHEMENAME", mapping.SchemeName);
            statement.AddField("ENABLED", mapping.Enabled ? 1 : 0, SqlDbType.Bit);
            statement.AddField("LOOKUPORDER", mapping.LookupOrder, SqlDbType.SmallInt);

            if (isNew)
                statement.AddField("CREATED", DateTime.Now, SqlDbType.DateTime);

            entry.Connection.ExecuteStatement(statement);
        }
        
        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "EFTMAPPING"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOEFTMAPPING", "MAPPINGID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
