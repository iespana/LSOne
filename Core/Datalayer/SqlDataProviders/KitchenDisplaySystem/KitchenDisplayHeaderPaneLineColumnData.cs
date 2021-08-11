using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
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
    public class KitchenDisplayHeaderPaneLineColumnData : SqlServerDataProviderBase, IKitchenDisplayHeaderPaneLineColumnData
    {
        private string BaseSelectString
        {
            get
            {
                return @" SELECT COLUMNNUMBER,
                                 LINEID,
                                 HEADERPANEID,
                                 COLUMNTYPE,
                                 DESCRIPTION,
                                 COLUMNALIGNMENT,
                                 STYLEID
					      FROM KITCHENDISPLAYHEADERPANELINECOLUMN ";
            }
        }

        public virtual LSOneHeaderPaneLineColumn Get(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = 
                    BaseSelectString + 
                    "WHERE LINEID = @lineID AND COLUMNNUMBER = @columnNumber";

                MakeParam(cmd, "columnNumber", (int)id.PrimaryID, SqlDbType.Int);
                MakeParam(cmd, "lineID", (Guid)id.SecondaryID, SqlDbType.UniqueIdentifier);

                List<LSOneHeaderPaneLineColumn> result = Execute<LSOneHeaderPaneLineColumn>(entry, cmd, CommandType.Text, PopulateColumn);

                if (result.Count == 1)
                {
                    AddStyle(entry, result[0]);
                    return result[0];
                }

                return null;
            }
        }

        public virtual List<LSOneHeaderPaneLineColumn> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseSelectString;

                List<LSOneHeaderPaneLineColumn> result = Execute<LSOneHeaderPaneLineColumn>(entry, cmd, CommandType.Text, PopulateColumn);
                result.ForEach(r => AddStyle(entry, r));
                return result;
            }
        }

        public virtual List<LSOneHeaderPaneLineColumn> GetList(IConnectionManager entry, RecordIdentifier headerPaneLineID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = 
                    BaseSelectString +
                    "WHERE LINEID = @lineID";

                MakeParam(cmd, "lineID", (Guid)headerPaneLineID, SqlDbType.UniqueIdentifier);

                List<LSOneHeaderPaneLineColumn> result = Execute<LSOneHeaderPaneLineColumn>(entry, cmd, CommandType.Text, PopulateColumn);
                result.ForEach(r => AddStyle(entry, r));
                return result;
            }
        }

        public int GetNextColumnNumber(IConnectionManager entry, RecordIdentifier lineID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @" SELECT COALESCE(MAX(COLUMNNUMBER), 0) AS NEXTCOLUMNNUMBER
					                 FROM KITCHENDISPLAYHEADERPANELINECOLUMN
                                     WHERE LINEID = @lineID";

                MakeParam(cmd, "lineID", (Guid)lineID, SqlDbType.UniqueIdentifier);

                List<RecordIdentifier> result = Execute(entry, cmd, CommandType.Text, "NEXTCOLUMNNUMBER");

                return (int)result[0] + 1;
            }
        }

        public virtual void DeleteByHeaderPaneLine(IConnectionManager entry, RecordIdentifier lineID)
        {
            DeleteRecord(entry, "KITCHENDISPLAYHEADERPANELINECOLUMN", "LINEID", lineID, Permission.ManageKitchenDisplayStations, false);
        }

        public virtual void DeleteByHeaderPane(IConnectionManager entry, RecordIdentifier headerPaneID)
        {
            DeleteRecord(entry, "KITCHENDISPLAYHEADERPANELINECOLUMN", "HEADERPANEID", headerPaneID, Permission.ManageKitchenDisplayStations, false);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "KITCHENDISPLAYHEADERPANELINECOLUMN", new[] { "COLUMNNUMBER", "LINEID" }, id, Permission.ManageKitchenDisplayStations, false);
            RestoreColumnNumberSequence(entry, id);
        }

        private void RestoreColumnNumberSequence(IConnectionManager entry, RecordIdentifier id)
        {
            ValidateSecurity(entry, Permission.ManageKitchenDisplayStations);

            Dictionary<int, int> columnNumberPairs = new Dictionary<int, int>();

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>
                {
                    new TableColumn { ColumnName = "COLUMNNUMBER" },
                    new TableColumn { ColumnName = "CAST(ROW_NUMBER() OVER (ORDER BY COLUMNNUMBER) AS INT) COLUMNNUMBER_New" },
                };

                List<Condition> conditions = new List<Condition>()
                {
                    new Condition { Operator = "AND", ConditionValue = "LINEID = @LINEID" }
                };

                MakeParam(cmd, "LINEID", (Guid)id.SecondaryID, SqlDbType.UniqueIdentifier);

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
                        int oldValue = (int)dr["COLUMNNUMBER"];
                        int newValue = (int)dr["COLUMNNUMBER_NEW"];
                        columnNumberPairs.Add(oldValue, newValue);
                    }
                }
            }

            foreach (var columnNumberPair in columnNumberPairs)
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
            return RecordExists(entry, "KITCHENDISPLAYHEADERPANELINECOLUMN", new[] { "COLUMNNUMBER", "LINEID" }, id, false);
        }

        public virtual void Save(IConnectionManager entry, LSOneHeaderPaneLineColumn headerPaneLineColumn)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYHEADERPANELINECOLUMN");

            ValidateSecurity(entry, Permission.ManageKitchenDisplayStations);

            if (!Exists(entry, new RecordIdentifier(headerPaneLineColumn.ColumnNumber, headerPaneLineColumn.LineId)))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("COLUMNNUMBER", (int)headerPaneLineColumn.ColumnNumber, SqlDbType.Int);
                statement.AddKey("LINEID", (Guid)headerPaneLineColumn.LineId, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("COLUMNNUMBER", (int)headerPaneLineColumn.ColumnNumber, SqlDbType.Int);
                statement.AddCondition("LINEID", (Guid)headerPaneLineColumn.LineId, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("HEADERPANEID", (Guid)headerPaneLineColumn.HeaderProfileId, SqlDbType.UniqueIdentifier);
            statement.AddField("COLUMNTYPE", (byte)headerPaneLineColumn.ColumnType, SqlDbType.TinyInt);
            statement.AddField("DESCRIPTION", headerPaneLineColumn.Text);
            statement.AddField("COLUMNALIGNMENT", (byte)headerPaneLineColumn.ColumnAlignment, SqlDbType.TinyInt);
            statement.AddField("STYLEID", (string)headerPaneLineColumn.Style.ID);

            Save(entry, headerPaneLineColumn, statement);
        }

        private void PopulateColumn(IDataReader dr, LSOneHeaderPaneLineColumn headerPaneLineColumn)
        {
            headerPaneLineColumn.ColumnNumber = (int)dr["COLUMNNUMBER"];
            headerPaneLineColumn.LineId = (Guid)dr["LINEID"];
            headerPaneLineColumn.HeaderProfileId = (Guid)dr["HEADERPANEID"];
            headerPaneLineColumn.ColumnType = (HdrPnColumnTypeEnum)(byte)dr["COLUMNTYPE"];
            headerPaneLineColumn.Text = (string)dr["DESCRIPTION"];
            headerPaneLineColumn.ColumnAlignment = (HdrPnColumnAlignmentEnum)(byte)dr["COLUMNALIGNMENT"];
            headerPaneLineColumn.Style.ID = (string)dr["STYLEID"];
        }

        private void AddStyle(IConnectionManager entry, LSOneHeaderPaneLineColumn headerPaneLineColumn)
        {
            headerPaneLineColumn.Style = Providers.PosStyleData.Get(entry, headerPaneLineColumn.Style.ID) ?? new PosStyle();
        }
    }
}