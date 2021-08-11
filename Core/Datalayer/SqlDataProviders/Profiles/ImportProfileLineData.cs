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

namespace LSOne.DataLayer.SqlDataProviders.Profiles
{
    /// <summary>
    /// Encapsulates methods to manipulate import profile lines.
    /// </summary>
    public class ImportProfileLineData : SqlServerDataProviderBase, IImportProfileLineData
    {
        private static List<TableColumn> importProfileLineColumns = new List<TableColumn>()
        {
            new TableColumn { ColumnName = "MASTERID", TableAlias = "A" },
            new TableColumn { ColumnName = "IMPORTPROFILEMASTERID", TableAlias = "A" },
            new TableColumn { ColumnName = "FIELD", TableAlias = "A" },
            new TableColumn { ColumnName = "FIELDTYPE", TableAlias = "A" },
            new TableColumn { ColumnName = "SEQUENCE", TableAlias = "A" }
        };

        private static void PopulateImportProfileLine(IDataReader dr, ImportProfileLine importProfileLine)
        {
            importProfileLine.MasterId = (Guid)dr["MASTERID"];
            importProfileLine.ImportProfileMasterId = (Guid)dr["IMPORTPROFILEMASTERID"];
            importProfileLine.Field = (Field)dr["FIELD"];
            importProfileLine.FieldType = (FieldType)dr["FIELDTYPE"];
            importProfileLine.Sequence = (int)dr["SEQUENCE"];
        }

        /// <summary>
        /// Get the list of ImportProflieLine instances associated with the given id.
        /// </summary>
        public virtual List<ImportProfileLine> GetSelectList(IConnectionManager entry, RecordIdentifier importProfileMasterId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "A.IMPORTPROFILEMASTERID = @importProfileMasterId"
                });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("IMPORTPROFILELINES", "A"),
                    QueryPartGenerator.InternalColumnGenerator(importProfileLineColumns),
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "ORDER BY A.[SEQUENCE] ASC");

                MakeParam(cmd, "importProfileMasterId", (string)importProfileMasterId);

                return Execute<ImportProfileLine>(entry, cmd, CommandType.Text, PopulateImportProfileLine);
            }
        }

        /// <summary>
        /// Deletes the ImportProflieLine record having the given id.
        /// </summary>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            ValidateSecurity(entry, Permission.ManageImportProfiles);

            SqlServerStatement statement = new SqlServerStatement("IMPORTPROFILELINES", StatementType.Delete);
            statement.AddCondition("MASTERID", (string)id);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Saves the given ImportProflieLine instance.
        /// </summary>
        public virtual void Save(IConnectionManager entry, ImportProfileLine item)
        {
            ValidateSecurity(entry, Permission.ManageImportProfiles);

            SqlServerStatement statement = new SqlServerStatement("IMPORTPROFILELINES");

            if (item.MasterId == RecordIdentifier.Empty)
            {
                statement.StatementType = StatementType.Insert;
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("MASTERID", (string)item.MasterId);
            }

            statement.AddKey("IMPORTPROFILEMASTERID", (string)item.ImportProfileMasterId);
            statement.AddField("FIELD", ((int)item.Field).ToString());
            statement.AddField("FIELDTYPE", ((int)item.FieldType).ToString());
            statement.AddField("SEQUENCE", item.Sequence.ToString());
            
            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Get the instance of ImportProfileLine having the given id.
        /// </summary>
        public virtual ImportProfileLine Get(IConnectionManager entry, RecordIdentifier id)
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
                    QueryTemplates.BaseQuery("IMPORTPROFILELINES", "A"),
                    QueryPartGenerator.InternalColumnGenerator(importProfileLineColumns),
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );

                MakeParam(cmd, "masterId", (string)id);

                var importProfileLines = Execute<ImportProfileLine>(entry, cmd, CommandType.Text, PopulateImportProfileLine);

                return (importProfileLines.Count > 0) ? importProfileLines[0] : null;
            }
        }

        /// <summary>
        /// Check weather there exists an import profile having the given id.
        /// </summary>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "IMPORTPROFILELINES", "MASTERID", id);
        }

        /// <summary>
        /// Get the list of Field values that are unavailable for a given id, as they are already in use.
        /// </summary>
        public virtual List<Field> GetUnavailableFieldList(IConnectionManager entry, RecordIdentifier importProfileMasterId)
        {
            return GetSelectList(entry, importProfileMasterId)
                .Select(a => a.Field)
                .ToList();
        }

        /// <summary>
        /// Swap the sequence values for two ImportProfileLine items having the given ids.
        /// </summary>
        public virtual void SwapSequenceValues(IConnectionManager entry, RecordIdentifier firstRowId, RecordIdentifier secondRowId)
        {
            ImportProfileLine firstLine = Get(entry, firstRowId);
            ImportProfileLine secondLine = Get(entry, secondRowId);

            int aux = firstLine.Sequence;
            firstLine.Sequence = secondLine.Sequence;
            secondLine.Sequence = aux;

            Save(entry, firstLine);
            Save(entry, secondLine);
        }
    }
}
