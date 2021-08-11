using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;
using System.Data.SqlClient;

namespace LSOne.DataLayer.SqlDataProviders.Inventory
{
    public class InventoryTransferRequestLineData : SqlServerDataProviderBase, IInventoryTransferRequestLineData
    {

        private static List<TableColumn> inventoryTransferRequestOrderColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "ID " , TableAlias = "itrl"},
            new TableColumn {ColumnName = "INVENTORYTRANSFERREQUESTID " , TableAlias = "itrl"},
            new TableColumn {ColumnName = "ITEMID " , TableAlias = "itrl"},
            new TableColumn {ColumnName = "ISNULL(it.ITEMNAME,'')", ColumnAlias  = "ITEMNAME"},
            new TableColumn {ColumnName = "ISNULL(it.VARIANTNAME,'')", ColumnAlias  = "VARIANTNAME"},
            new TableColumn {ColumnName = "UNITID " , TableAlias = "itrl"},
            new TableColumn {ColumnName = "ISNULL(u.TXT, '')", ColumnAlias  = "UNITNAME"},
            new TableColumn {ColumnName = "QUANTITYREQUESTED " , TableAlias = "itrl"},
            new TableColumn {ColumnName = "SENT" , TableAlias = "itrl"},
        };

        private static List<Join> listJoins = new List<Join>
        {
            new Join
            {
                Condition = " ITRL.ITEMID = it.ITEMID ",
                JoinType = "LEFT OUTER",
                Table = "RETAILITEM",
                TableAlias = "IT"
            },
            new Join
            {
                Condition = "ITRL.UNITID = U.UNITID",
                JoinType = "LEFT OUTER",
                Table = "UNIT",
                TableAlias = "U"
            },
        };

        private static void PopulateTransferRequestLine(IConnectionManager entry, IDataReader dr, InventoryTransferRequestLine inventoryTransferRequestLine, object param)
        {
            inventoryTransferRequestLine.ID = (Guid)dr["ID"];
            inventoryTransferRequestLine.InventoryTransferRequestId = (string)dr["INVENTORYTRANSFERREQUESTID"];
            inventoryTransferRequestLine.ItemId = (string)dr["ITEMID"];
            inventoryTransferRequestLine.ItemName = (string)dr["ITEMNAME"];
            inventoryTransferRequestLine.VariantName = (string)dr["VARIANTNAME"];
            inventoryTransferRequestLine.UnitId = (string)dr["UNITID"];
            inventoryTransferRequestLine.UnitName = (string)dr["UNITNAME"];
            inventoryTransferRequestLine.QuantityRequested = (decimal)dr["QUANTITYREQUESTED"];
            inventoryTransferRequestLine.Sent = (bool)dr["SENT"];
        }

        private static void PopulateTransferLineWithItemsAndRowCount(IConnectionManager entry, IDataReader dr, InventoryTransferRequestLine inventoryTransferRequestLine, ref int rowCount)
        {
            PopulateTransferRequestLine(entry, dr, inventoryTransferRequestLine, null);
            inventoryTransferRequestLine.Barcode = dr["ITEMBARCODE"] == DBNull.Value ? "" : (string)dr["ITEMBARCODE"];
            PopulateRowCount(entry, dr, ref rowCount);
        }

        protected static void PopulateRowCount(IConnectionManager entry, IDataReader dr, ref int rowCount)
        {
            if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 ||
                entry.Connection.DatabaseVersion == ServerVersion.Unknown)
            {
                rowCount = (int)dr["Row_Count"];
            }
        }

        private static string ResolveSortPaged(InventoryTransferRequestLineSortEnum sortEnum, bool sortBackwards, bool useAlias = false)
        {
            var sortString = " ORDER BY " + GetSortColumn(sortEnum, useAlias) + " ASC";

            if (sortBackwards)
            {
                sortString = sortString.Replace("ASC", "DESC");
            }

            return sortString;
        }

        private static string GetSortColumn(InventoryTransferRequestLineSortEnum sortEnum, bool useAlias = false)
        {
            string sortColumn;

            switch (sortEnum)
            {
                case InventoryTransferRequestLineSortEnum.ItemName:
                    sortColumn = useAlias ? "it.ITEMNAME" : "ITEMNAME";
                    break;
                case InventoryTransferRequestLineSortEnum.RequestedQuantity:
                    sortColumn = useAlias ? "itrl.QUANTITYREQUESTED" : "QUANTITYREQUESTED";
                    break;
                case InventoryTransferRequestLineSortEnum.Sent:
                    sortColumn = useAlias ? "itrl.SENT" : "SENT";
                    break;
                case InventoryTransferRequestLineSortEnum.VariantName:
                    sortColumn = useAlias ? "it.VARIANTNAME" : "VARIANTNAME";
                    break;
                case InventoryTransferRequestLineSortEnum.Barcode:
                    sortColumn = useAlias ? "itb.ITEMBARCODE" : "ITEMBARCODE";
                    break;
                default:
                    sortColumn = useAlias ? "itrl.ITEMID" : "ITEMID";
                    break;
            }
            return sortColumn;
        }

        private static string ResolveSort(InventoryTransferOrderLineSortEnum sort, bool sortBackwards)
        {
            string sortString = " ORDER BY ";
            switch (sort)
            {
                case InventoryTransferOrderLineSortEnum.ItemName:
                    sortString += "ITEMNAME";
                    break;
                case InventoryTransferOrderLineSortEnum.RequestedQuantity:
                    sortString += "QUANTITYREQUESTED";
                    break;
                case InventoryTransferOrderLineSortEnum.Sent:
                    sortString += "itrl.SENT";
                    break;
            }

            sortString += (sortBackwards) ? " DESC" : " ASC";

            return sortString;
        }

        public virtual InventoryTransferRequestLine Get(IConnectionManager entry, RecordIdentifier transferRequestLineId)
        {
            List<InventoryTransferRequestLine> result;

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "itrl.ID = @TRANSFERREQUESTLINEID " });

                cmd.CommandText = string.Format(
                 QueryTemplates.BaseQuery("INVENTORYTRANSFERREQUESTLINE", "ITRL"),
                 QueryPartGenerator.InternalColumnGenerator(inventoryTransferRequestOrderColumns),
                 QueryPartGenerator.JoinGenerator(listJoins),
                 QueryPartGenerator.ConditionGenerator(conditions),
                 string.Empty
                 );
              
                MakeParam(cmd, "TRANSFERREQUESTLINEID", (Guid)transferRequestLineId);

                result = Execute<InventoryTransferRequestLine>(entry, cmd, CommandType.Text, null, PopulateTransferRequestLine);
            }

            return (result.Count > 0) ? result[0] : null;
        }

        public virtual InventoryTransferRequestLine GetLine(IConnectionManager entry, InventoryTransferRequestLine line)
        {
            List<InventoryTransferRequestLine> result;
            SqlCommand cmd;

            ValidateSecurity(entry);

            using (cmd = new SqlCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "itrl.INVENTORYTRANSFERREQUESTID = @INVENTORYTRANSFERREQUESTID " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "itrl.ITEMID = @ITEMID " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "itrl.UNITID = @UNITID " });

                cmd.CommandText = string.Format(
                 QueryTemplates.BaseQuery("INVENTORYTRANSFERREQUESTLINE", "itrl"),
                 QueryPartGenerator.InternalColumnGenerator(inventoryTransferRequestOrderColumns),
                 QueryPartGenerator.JoinGenerator(listJoins),
                 QueryPartGenerator.ConditionGenerator(conditions),
                 string.Empty
                 );

                MakeParam(cmd, "INVENTORYTRANSFERREQUESTID", (string)line.InventoryTransferRequestId);
                MakeParam(cmd, "ITEMID", (string)line.ItemId);
                MakeParam(cmd, "UNITID", (string)line.UnitId);

                result = Execute<InventoryTransferRequestLine>(entry, cmd, CommandType.Text, null, PopulateTransferRequestLine);
            }

            return (result.Count > 0) ? result[0] : null;
        }

        public List<InventoryTransferRequestLine> GetRequestLinesForInventoryTransferAdvanced(IConnectionManager entry, RecordIdentifier transferRequestID, out int totalRecordsMatching, InventoryTransferFilterExtended filter)
        {
            List<InventoryTransferRequestLine> result;

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>();
                columns.AddRange(inventoryTransferRequestOrderColumns);
                columns.Add(new TableColumn { ColumnName = "ITEMBARCODE", ColumnAlias = "ITEMBARCODE", TableAlias = "ITB" });
                columns.Add(new TableColumn { ColumnName = string.Format("ROW_NUMBER() OVER({0})", ResolveSortPaged(filter.TransferRequestSortBy, filter.SortDescending, true)), ColumnAlias = "ROW" });
                

                List<Condition> externalConditions = new List<Condition>();
                externalConditions.Add(new Condition { Operator = "AND", ConditionValue = "CTE.ROW BETWEEN @ROWFROM AND @ROWTO" });

                List<Join> joins = new List<Join>();
                joins.AddRange(listJoins);

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "itrl.INVENTORYTRANSFERREQUESTID = @TRANSFERREQUESTID " });
                MakeParam(cmd, "TRANSFERREQUESTID", transferRequestID);

                if (!string.IsNullOrEmpty(filter.DescriptionOrID))
                {
                    string searchString = PreProcessSearchText(filter.DescriptionOrID, true, filter.DescriptionOrIDBeginsWith);
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "(it.ITEMNAME LIKE @DESCRIPTION OR it.ITEMID LIKE @DESCRIPTION OR it.VARIANTNAME LIKE @DESCRIPTION OR itb.ITEMBARCODE LIKE @DESCRIPTION)"
                    });
                    MakeParam(cmd, "DESCRIPTION", searchString);
                }

                if (!string.IsNullOrEmpty(filter.Barcode))
                {
                    string searchString = PreProcessSearchText(filter.Barcode, true, filter.BarcodeBeginsWith);
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "(itb.ITEMBARCODE LIKE @BARCODE)"
                    });
                    MakeParam(cmd, "BARCODE", searchString);
                }

                if (filter.RequestedQuantityFrom != null)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "itrl.QUANTITYREQUESTED >= @RECEIVEDQUANTITYFROM "
                    });
                    MakeParam(cmd, "RECEIVEDQUANTITYFROM", filter.RequestedQuantityFrom.Value, SqlDbType.Decimal);
                }

                if (filter.RequestedQuantityTo != null)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "itrl.QUANTITYREQUESTED <= @RECEIVEDQUANTITYTO "
                    });
                    MakeParam(cmd, "RECEIVEDQUANTITYTO", filter.RequestedQuantityTo.Value, SqlDbType.Decimal);
                }

                if (filter.Sent != null)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "itrl.SENT = @SENT "
                    });
                    MakeParam(cmd, "SENT", filter.Sent, SqlDbType.Bit);
                }

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "itrl.DATAAREAID = @DATAAREAID" });

                string joinCondition = QueryPartGenerator.JoinGenerator(listJoins);
                joinCondition += Environment.NewLine + "OUTER APPLY (SELECT TOP 1 ITEMBARCODE FROM INVENTITEMBARCODE IB WHERE IB.DATAAREAID = @DATAAREAID AND IB.ITEMID = IT.ITEMID AND IB.BLOCKED != 1 AND IB.DELETED = 0 ORDER BY IB.RBOSHOWFORITEM DESC, IB.QTY ASC) itb";
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                string baseQuery = string.Format(QueryTemplates.BaseQuery("INVENTORYTRANSFERREQUESTLINE", "itrl"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    joinCondition,
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

                cmd.CommandText = $";WITH CTE AS ({baseQuery}) " +
                    $"SELECT {QueryPartGenerator.ExternalColumnGenerator(columns, "CTE")}, ROW_COUNT = tCountTransactions.ROW_COUNT " +
                    $"FROM CTE CROSS JOIN (SELECT Count(*) AS ROW_COUNT FROM CTE) AS tCountTransactions " +
                    $"{QueryPartGenerator.ConditionGenerator(externalConditions)}" +
                    $"{ResolveSortPaged(filter.TransferRequestSortBy, filter.SortDescending)}";

                MakeParam(cmd, "ROWFROM", filter.RowFrom, SqlDbType.Int);
                MakeParam(cmd, "ROWTO", filter.RowTo, SqlDbType.Int);
                totalRecordsMatching = 0;
                result = Execute<InventoryTransferRequestLine, int>(entry, cmd, CommandType.Text, ref totalRecordsMatching, PopulateTransferLineWithItemsAndRowCount);
            }

            return result;
        }

        public virtual void MarkTransferRequestLinesAsSent(IConnectionManager entry, RecordIdentifier transferRequestID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = $"UPDATE INVENTORYTRANSFERREQUESTLINE SET SENT = 1 WHERE INVENTORYTRANSFERREQUESTID = '{transferRequestID.StringValue}' AND DATAAREAID = '{entry.Connection.DataAreaId}' AND SENT = 0";
                entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
            }
        }

        public List<InventoryTransferRequestLine> GetListForInventoryTransferRequest(
            IConnectionManager entry,
            RecordIdentifier inventoryTransferRequestId,
            InventoryTransferOrderLineSortEnum sortBy,
            bool sortBackwards)
        {
            List<InventoryTransferRequestLine> result;

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "ITRL.INVENTORYTRANSFERREQUESTID = @INVENTORYTRANSFERREQUESTID"
                });

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("INVENTORYTRANSFERREQUESTLINE", "ITRL"),
                    QueryPartGenerator.InternalColumnGenerator(inventoryTransferRequestOrderColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    ResolveSort(sortBy, sortBackwards)
                    );


                MakeParam(cmd, "INVENTORYTRANSFERREQUESTID", (string) inventoryTransferRequestId);

                result = Execute<InventoryTransferRequestLine>(entry, cmd, CommandType.Text, null,
                    PopulateTransferRequestLine);
            }

            return result;
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "INVENTORYTRANSFERREQUESTLINE", "ID", id);
        }

        public virtual bool ItemWithSameParametersExists(IConnectionManager entry, InventoryTransferRequestLine line)
        {
            return RecordExists(entry, "INVENTORYTRANSFERREQUESTLINE", new[] { "INVENTORYTRANSFERREQUESTID", "ITEMID", "UNITID" }, new RecordIdentifier(line.InventoryTransferRequestId, line.ItemId, line.UnitId));
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "INVENTORYTRANSFERREQUESTLINE", "ID", id, BusinessObjects.Permission.EditInventoryTransferRequests);
        }

        public virtual void Save(IConnectionManager entry, InventoryTransferRequestLine inventoryTransferRequestLine)
        {
            ValidateSecurity(entry, Permission.EditInventoryTransferRequests);
            var statement = new SqlServerStatement("INVENTORYTRANSFERREQUESTLINE", false); // Dont' create replication actions because replication is handled through Site Service

            if (inventoryTransferRequestLine.ID == "" || inventoryTransferRequestLine.ID.IsEmpty)
            {
                inventoryTransferRequestLine.ID = Guid.NewGuid();
            }

            if (!Exists(entry, inventoryTransferRequestLine.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("ID", (Guid)inventoryTransferRequestLine.ID, SqlDbType.UniqueIdentifier);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ID", (Guid)inventoryTransferRequestLine.ID, SqlDbType.UniqueIdentifier);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("INVENTORYTRANSFERREQUESTID", (string)inventoryTransferRequestLine.InventoryTransferRequestId);
            statement.AddField("ITEMID", (string)inventoryTransferRequestLine.ItemId);
            statement.AddField("UNITID", (string)inventoryTransferRequestLine.UnitId);
            statement.AddField("QUANTITYREQUESTED", inventoryTransferRequestLine.QuantityRequested, SqlDbType.Decimal);
            statement.AddField("SENT", inventoryTransferRequestLine.Sent, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void SaveIfChanged(IConnectionManager entry, InventoryTransferRequestLine inventoryTransferRequestLine)
        {
            var previousLine = Get(entry, inventoryTransferRequestLine.ID);

            if (previousLine != inventoryTransferRequestLine)
            {
                Save(entry, inventoryTransferRequestLine);
            }

        }

        public virtual void CopyLines(IConnectionManager entry, RecordIdentifier copyFrom, RecordIdentifier copyTo)
        {
            ValidateSecurity(entry, Permission.EditInventoryTransferRequests);

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText =
                    @" INSERT INTO INVENTORYTRANSFERREQUESTLINE
                       (ID, INVENTORYTRANSFERREQUESTID, ITEMID, UNITID, QUANTITYREQUESTED, SENT, DATAAREAID)
                       SELECT NEWID(), @COPYTO, ITEMID, UNITID, QUANTITYREQUESTED, 0, DATAAREAID FROM INVENTORYTRANSFERREQUESTLINE WHERE INVENTORYTRANSFERREQUESTID = @COPYFROM ";

                MakeParam(cmd, "COPYTO", (string)copyTo);
                MakeParam(cmd, "COPYFROM", (string)copyFrom);

                entry.Connection.ExecuteScalar(cmd);
            }
        }

        public virtual void CopyLinesFromOrder(IConnectionManager entry, RecordIdentifier copyFromOrder, RecordIdentifier copyToRequest)
        {
            ValidateSecurity(entry, Permission.EditInventoryTransferRequests);

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText =
                    @" INSERT INTO INVENTORYTRANSFERREQUESTLINE
                       (ID, INVENTORYTRANSFERREQUESTID, ITEMID, UNITID, QUANTITYREQUESTED, SENT, DATAAREAID)
                       select NEWID(), @COPYTO, ITEMID, UNITID, QUANTITYSENT, 0, DATAAREAID from INVENTORYTRANSFERORDERLINE WHERE INVENTORYTRANSFERORDERID = @COPYFROM ";

                MakeParam(cmd, "COPYTO", (string)copyToRequest);
                MakeParam(cmd, "COPYFROM", (string)copyFromOrder);

                entry.Connection.ExecuteScalar(cmd);
            }
        }

        public virtual int LineCount(IConnectionManager entry, RecordIdentifier requestID)
        {
            return Count(entry, "INVENTORYTRANSFERREQUESTLINE", "INVENTORYTRANSFERREQUESTID", requestID);
        }
    }
}