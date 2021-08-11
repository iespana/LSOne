using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.DataLayer.SqlConnector.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Interfaces;
using System.Data;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;

namespace LSOne.DataLayer.SqlDataProviders.Profiles
{
    /// <summary>
    /// Encapsulates methods to manipulate import profiles.
    /// </summary>
    public class ImportProfileData : SqlServerDataProviderBase, IImportProfileData
    {
        private static List<TableColumn> importProfileColumns = new List<TableColumn>()
        {
            new TableColumn { ColumnName = "MASTERID", TableAlias = "A" },
            new TableColumn { ColumnName = "ID", TableAlias = "A" },
            new TableColumn { ColumnName = "DESCRIPTION", TableAlias = "A" },
            new TableColumn { ColumnName = "IMPORTTYPE", TableAlias = "A" },
            new TableColumn { ColumnName = "[DEFAULT]", TableAlias = "A" },
            new TableColumn { ColumnName = "HASHEADERS", TableAlias = "A" },
        };

        public RecordIdentifier SequenceID
        {
            get
            {
                return "IMPORTPROFILE";
            }
        }

        private static void PopulateImportProfile(IDataReader dr, ImportProfile importProfile)
        {
            importProfile.MasterID = (Guid)dr["MASTERID"];
            importProfile.ID = (string)dr["ID"];
            importProfile.Description = (string)dr["DESCRIPTION"];
            importProfile.ImportType = (ImportType)dr["IMPORTTYPE"];
            importProfile.Default = (byte)dr["DEFAULT"];
            importProfile.HasHeaders = (bool)dr["HASHEADERS"];
        }

        /// <summary>
        /// Get the list of all ImportProflie instances.
        /// </summary>
        public virtual List<ImportProfile> GetSelectList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("IMPORTPROFILE", "A"),
                    QueryPartGenerator.InternalColumnGenerator(importProfileColumns),
                    string.Empty,
                    string.Empty,
                    string.Empty);

                return Execute<ImportProfile>(entry, cmd, CommandType.Text, PopulateImportProfile);
            }
        }

        /// <summary>
        /// Get the list of all ImportProflie instances.
        /// </summary>
        public virtual List<ImportProfile> GetSelectListOfNonEmptyProfiles(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Join> joins = new List<Join>();
                joins.Add(new Join
                {
                    Condition = "A.MASTERID = LINECOUNTS.IMPORTPROFILEMASTERID",
                    JoinType = "LEFT OUTER",
                    Table = "(SELECT count(*) AS LINECOUNT, IMPORTPROFILEMASTERID FROM IMPORTPROFILELINES GROUP BY IMPORTPROFILEMASTERID)",
                    TableAlias = "LINECOUNTS"
                });

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "LINECOUNT > 0"
                });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("IMPORTPROFILE", "A"),
                    QueryPartGenerator.InternalColumnGenerator(importProfileColumns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

                return Execute<ImportProfile>(entry, cmd, CommandType.Text, PopulateImportProfile);
            }
        }

        /// <summary>
        /// Deletes the ImportProflie instance having the given id.
        /// </summary>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            ValidateSecurity(entry, Permission.ManageImportProfiles);

            SqlServerStatement statement = new SqlServerStatement("IMPORTPROFILE", StatementType.Delete);
            statement.AddCondition("MASTERID", (string)id);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Saves the given ImportProflie instance.
        /// </summary>
        public virtual void Save(IConnectionManager entry, ImportProfile item)
        {
            ValidateSecurity(entry, Permission.ManageImportProfiles);

            SqlServerStatement statement = new SqlServerStatement("IMPORTPROFILE");
            if (item.MasterID == RecordIdentifier.Empty)
            {
                statement.StatementType = StatementType.Insert;
                item.ID = DataProviderFactory.Instance.GenerateNumber<IImportProfileData, ImportProfile>(entry);
                item.MasterID = (RecordIdentifier)Guid.NewGuid();
                statement.AddKey("ID", (string)item.ID);
                statement.AddKey("MASTERID", (string)item.MasterID);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("MASTERID", (string)item.MasterID);
            }
            statement.AddField("DESCRIPTION", item.Description);
            statement.AddField("IMPORTTYPE", ((int)item.ImportType).ToString());
            statement.AddField("HASHEADERS", item.HasHeaders, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Get the ImportProfile instance having the given id.
        /// </summary>
        public virtual ImportProfile Get(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "A.MASTERID = @masterId"
                });

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("IMPORTPROFILE", "A"),
                    QueryPartGenerator.InternalColumnGenerator(importProfileColumns),
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );

                MakeParam(cmd, "masterId", (string)id);

                var importProfiles = Execute<ImportProfile>(entry, cmd, CommandType.Text, PopulateImportProfile);

                return (importProfiles.Count > 0) ? importProfiles[0] : null;
            }
        }

        /// <summary>
        /// Check weather there exists an import profile having the given id.
        /// </summary>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "IMPORTPROFILE", "ID", id, false);
        }

        /// <summary>
        /// Check weather there exists an import profile having the given sequence id.
        /// </summary>
        public bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "IMPORTPROFILE", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        /// <summary>
        /// Set the default import profile.
        /// </summary>
        public void SetAsDefault(IConnectionManager entry, RecordIdentifier id)
        {
            ValidateSecurity(entry);

            ImportProfile importProfile = Get(entry, id);

            string protectedUpdateSql = "UPDATE {0} SET [{1}]={2} WHERE [{3}]={4} AND {5} = {6}";
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = string.Format(
                    protectedUpdateSql,
                    "IMPORTPROFILE",
                    "DEFAULT",
                    "0",
                    "DEFAULT",
                    "1",
                    "IMPORTTYPE",
                    (int)importProfile.ImportType);
                Execute<ImportProfile>(entry, cmd, CommandType.Text, PopulateImportProfile);

                cmd.CommandText = string.Format(
                    protectedUpdateSql,
                    "IMPORTPROFILE",
                    "DEFAULT",
                    "1",
                    "MASTERID",
                    "'" + (string)id + "'",
                    "IMPORTTYPE",
                    (int)importProfile.ImportType);
                Execute<ImportProfile>(entry, cmd, CommandType.Text, PopulateImportProfile);
            }
        }

        public ImportProfile GetDefaultImportProfile(IConnectionManager entry, ImportType importType)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>()
                {
                    new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "A.[DEFAULT] = 1"
                    },
                    new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "A.[IMPORTTYPE] = " + ((int)importType).ToString()
                    }
                };

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("IMPORTPROFILE", "A"),
                    QueryPartGenerator.InternalColumnGenerator(importProfileColumns),
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

                List<ImportProfile> importProfiles = Execute<ImportProfile>(entry, cmd, CommandType.Text, PopulateImportProfile);
                if (importProfiles.Count == 0)
                {
                    return new ImportProfile();
                }
                else
                {
                    return importProfiles[0];
                }
            }
        }
    }
}