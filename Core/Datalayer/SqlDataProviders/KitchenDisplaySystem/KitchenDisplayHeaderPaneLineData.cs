using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using static LSOne.Utilities.DataTypes.RecordIdentifier;

namespace LSOne.DataLayer.SqlDataProviders.KitchenDisplaySystem
{
    public class KitchenDisplayHeaderPaneLineData : SqlServerDataProviderBase, IKitchenDisplayHeaderPaneLineData
    {
        private string BaseSelectString
        {
            get
            {
                return @"SELECT 
                            ID, 
                            HEADERPANEID, 
                            LINENUMBER
					     FROM 
                            KITCHENDISPLAYHEADERPANELINE ";
            }
        }

        public virtual HeaderPaneLine Get(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = 
                    BaseSelectString + 
                    "WHERE ID = @id";

                MakeParam(cmd, "id", (Guid)id);

                List<HeaderPaneLine> result = Execute<HeaderPaneLine>(entry, cmd, CommandType.Text, PopulateHeaderPane);
                return (result.Count == 1) ? result[0] : null;
            }
        }

        public virtual List<HeaderPaneLine> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseSelectString;

                return Execute<HeaderPaneLine>(entry, cmd, CommandType.Text, PopulateHeaderPane);
            }
        }

        public virtual List<HeaderPaneLine> GetList(IConnectionManager entry, RecordIdentifier headerPaneID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = 
                    BaseSelectString +
                    "WHERE HEADERPANEID = @headerPaneID";

                MakeParam(cmd, "headerPaneID", (Guid)headerPaneID, SqlDbType.UniqueIdentifier);

                return Execute<HeaderPaneLine>(entry, cmd, CommandType.Text, PopulateHeaderPane);
            }
        }

        public virtual void DeleteByHeaderPane(IConnectionManager entry, RecordIdentifier headerPaneID)
        {
            DeleteRecord(entry, "KITCHENDISPLAYHEADERPANELINE", "HEADERPANEID", headerPaneID, Permission.ManageKitchenDisplayStations, false);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "KITCHENDISPLAYHEADERPANELINE", "ID", id, Permission.ManageKitchenDisplayStations, false);
            Providers.KitchenDisplayHeaderPaneLineColumnData.DeleteByHeaderPaneLine(entry, id);
        }

        private void RestoreLineNumberSequence(IConnectionManager entry, RecordIdentifier id)
        {
            ValidateSecurity(entry, Permission.ManageKitchenDisplayStations);

            Dictionary<int, int> lineNumberPairs = new Dictionary<int, int>();

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>
                {
                    new TableColumn { ColumnName = "LINENUMBER" },
                    new TableColumn { ColumnName = "CAST(ROW_NUMBER() OVER (ORDER BY LINENUMBER) AS INT) LINENUMBER_New" },
                };

                List<Condition> conditions = new List<Condition>()
                {
                    new Condition { Operator = "AND", ConditionValue = "HEADERPANEID = @HEADERPANEID" }
                };

                MakeParam(cmd, "HEADERPANEID", (Guid)id.SecondaryID, SqlDbType.UniqueIdentifier);

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("KITCHENDISPLAYHEADERPANELINECOLUMN", ""),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    "",
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "");


                using (IDataReader dr = entry.Connection.ExecuteReader(cmd, CommandType.Text))
                {
                    while (dr.Read())
                    {
                        int oldValue = (int)dr["LINENUMBER"];
                        int newValue = (int)dr["LINENUMBER_New"];
                        lineNumberPairs.Add(oldValue, newValue);
                    }
                }
            }

            foreach (var columnNumberPair in lineNumberPairs)
            {
                var statement = new SqlServerStatement("KITCHENDISPLAYHEADERPANELINECOLUMN", StatementType.Update);

                statement.AddCondition("COLUMNNUMBER", columnNumberPair.Key, SqlDbType.Int);
                statement.AddCondition("LINEID", (Guid)id.SecondaryID, SqlDbType.UniqueIdentifier);
                statement.AddField("COLUMNNUMBER", columnNumberPair.Value, SqlDbType.Int);

                entry.Connection.ExecuteStatement(statement);
            }
        }


        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "KITCHENDISPLAYHEADERPANELINE", "ID", id, false);
        }

        public virtual void Save(IConnectionManager entry, HeaderPaneLine headerPaneLine)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYHEADERPANELINE");

            ValidateSecurity(entry, Permission.ManageKitchenDisplayStations);

            if (!Exists(entry, headerPaneLine.ID))
            {
                statement.StatementType = StatementType.Insert;
                if (RecordIdentifier.IsEmptyOrNull(headerPaneLine.ID))
                {
                    headerPaneLine.ID = Guid.NewGuid();
                }
                statement.AddKey("ID", (Guid)headerPaneLine.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ID", (Guid)headerPaneLine.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("HEADERPANEID", (Guid)headerPaneLine.HeaderProfileId, SqlDbType.UniqueIdentifier);
            statement.AddField("LINENUMBER", headerPaneLine.LineNumber, SqlDbType.Int);

            Save(entry, headerPaneLine, statement);
        }

        public virtual void SaveLineNumber(IConnectionManager entry, HeaderPaneLine headerPaneLine)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYHEADERPANELINE", StatementType.Update);

            statement.AddCondition("ID", (string)headerPaneLine.ID);
            statement.AddField("LINENUMBER", headerPaneLine.LineNumber, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        private void PopulateHeaderPane(IDataReader dr, HeaderPaneLine headerPaneLine)
        {
            headerPaneLine.ID = headerPaneLine.LineId = (Guid)dr["ID"];
            headerPaneLine.HeaderProfileId = (Guid)dr["HEADERPANEID"];
            headerPaneLine.HeaderProfileId.SerializationType = RecordIdentifierType.String;
            headerPaneLine.LineNumber = (int)dr["LINENUMBER"];
        }
    }
}