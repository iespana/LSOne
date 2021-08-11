using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Inventory
{
    public class InventoryTransferOrderLineData : SqlServerDataProviderBase, IInventoryTransferOrderLineData
    {

        private SqlServerStatement updateStatment = null;
        private SqlServerStatement insertStatement = null;

        //Used for the transfer request ID which created a transfer order to avoid lots of joins
        private static string transferRequestIdDeclaration = "DECLARE @TRANSFERREQUESTID NVARCHAR(40) = (SELECT INVENTORYTRANSFERREQUESTID FROM INVENTORYTRANSFERORDER WHERE DATAAREAID = @DATAAREAID AND ID = @TRANSFERORDERID);";

        private static List<TableColumn> inventoryTransferOrderColumns = new List<TableColumn>
        {

            new TableColumn {ColumnName = "ID " , TableAlias = "itl"},
            new TableColumn {ColumnName = "INVENTORYTRANSFERREQUESTLINEID " , TableAlias = "itl"},
            new TableColumn {ColumnName = "INVENTORYTRANSFERORDERID " , TableAlias = "itl"},
            new TableColumn {ColumnName = "ITEMID " , TableAlias = "itl"},
            new TableColumn {ColumnName = "ISNULL(it.ITEMNAME,'')" , ColumnAlias = "ITEMNAME"},
            new TableColumn {ColumnName = "ISNULL(it.VARIANTNAME,'')" , ColumnAlias = "VARIANTNAME"},
            new TableColumn {ColumnName = "UNITID " , TableAlias = "itl"},
            new TableColumn {ColumnName = "ISNULL(u.TXT, '')", ColumnAlias  = "UNITNAME"},
            new TableColumn {ColumnName = "SENT " , TableAlias = "itl"},
            new TableColumn {ColumnName = "ISNULL(itl.QUANTITYSENT, 0)", ColumnAlias  = "QUANTITYSENT"},
            new TableColumn {ColumnName = "ISNULL(itl.QUANTITYRECEIVED, 0)", ColumnAlias  = "QUANTITYRECEIVED"},
            new TableColumn {ColumnName = "ISNULL(itrl.QUANTITYREQUESTED, 0)", ColumnAlias  = "QUANTITYREQUESTED"},
            new TableColumn {ColumnName = "ISNULL(itl.PICTUREID, '')", ColumnAlias = "PICTUREID"},
            new TableColumn {ColumnName = "ISNULL(itl.OMNILINEID, '')", ColumnAlias = "OMNILINEID"},
            new TableColumn {ColumnName = "ISNULL(itl.OMNITRANSACTIONID, '')", ColumnAlias = "OMNITRANSACTIONID"},
            new TableColumn {ColumnName = "ISNULL(itl.COSTPRICE, '0')", ColumnAlias = "COSTPRICE"},

        };

        private static List<Join> listJoins = new List<Join>
        {
            new Join
            {
                Condition = " ITL.ITEMID = IT.ITEMID",
                JoinType = "LEFT OUTER",
                Table = "RETAILITEM",
                TableAlias = "IT"
            },
            new Join
            {
                Condition = "ITL.UNITID = U.UNITID",
                JoinType = "LEFT OUTER",
                Table = "UNIT",
                TableAlias = "U"
            },
            new Join
            {
                Condition = " ITL.INVENTORYTRANSFERORDERID = ITT.ID ",
                JoinType = "LEFT OUTER",
                Table = "INVENTORYTRANSFERORDER",
                TableAlias = "ITT"
            },
            new Join
            {
                Condition = "ITR.ID = ITT.INVENTORYTRANSFERREQUESTID",
                JoinType = "LEFT OUTER",
                Table = "INVENTORYTRANSFERREQUEST",
                TableAlias = "ITR"
            },
            new Join
            {
                Condition = "ITRL.INVENTORYTRANSFERREQUESTID = ITR.ID AND ITL.ITEMID = ITRL.ITEMID AND ITRL.UNITID = ITL.UNITID",
                JoinType = "LEFT OUTER",
                Table = "INVENTORYTRANSFERREQUESTLINE",
                TableAlias = "ITRL"
            }
        };

        /// <summary>
        /// Minimal joins that assumes the query has declared a @TRANSFERREQUESTID
        /// </summary>
        private static List<Join> minimalJoins = new List<Join>
        {
            new Join
            {
                Condition = " ITL.ITEMID = IT.ITEMID",
                JoinType = "LEFT OUTER",
                Table = "RETAILITEM",
                TableAlias = "IT"
            },
            new Join
            {
                Condition = "ITL.UNITID = U.UNITID",
                JoinType = "LEFT OUTER",
                Table = "UNIT",
                TableAlias = "U"
            },
            new Join
            {
                Condition = "ITRL.INVENTORYTRANSFERREQUESTID = @TRANSFERREQUESTID AND ITL.ITEMID = ITRL.ITEMID AND ITRL.UNITID = ITL.UNITID",
                JoinType = "LEFT OUTER",
                Table = "INVENTORYTRANSFERREQUESTLINE",
                TableAlias = "ITRL"
            }
        };

        private static void PopulateTransferLine(IDataReader dr, InventoryTransferOrderLine inventoryTransferOrderLine)
        {
            inventoryTransferOrderLine.ID = (Guid)dr["ID"];
            inventoryTransferOrderLine.InventoryTransferRequestLineID = dr["INVENTORYTRANSFERREQUESTLINEID"] == DBNull.Value ? Guid.Empty : (Guid)dr["INVENTORYTRANSFERREQUESTLINEID"];
            inventoryTransferOrderLine.InventoryTransferId = (string)dr["INVENTORYTRANSFERORDERID"];
            inventoryTransferOrderLine.ItemId = (string)dr["ITEMID"];
            inventoryTransferOrderLine.ItemName = (string)dr["ITEMNAME"];
            inventoryTransferOrderLine.VariantName = (string)dr["VARIANTNAME"];
            inventoryTransferOrderLine.UnitId = (string)dr["UNITID"];
            inventoryTransferOrderLine.UnitName = (string)dr["UNITNAME"];
            inventoryTransferOrderLine.Sent = (bool)dr["SENT"];
            inventoryTransferOrderLine.QuantitySent = (decimal)dr["QUANTITYSENT"];
            inventoryTransferOrderLine.QuantityReceived = (decimal)dr["QUANTITYRECEIVED"];
            inventoryTransferOrderLine.QuantityRequested = (decimal)dr["QUANTITYREQUESTED"];
            inventoryTransferOrderLine.PictureID = (string)dr["PICTUREID"];
            inventoryTransferOrderLine.OmniLineID = (string)dr["OMNILINEID"];
            inventoryTransferOrderLine.OmniTransactionID = (string)dr["OMNITRANSACTIONID"];
            inventoryTransferOrderLine.CostPrice = (decimal)dr["COSTPRICE"];

        }

        private static void PopulateTransferLineWithItemsAndRowCount(IConnectionManager entry, IDataReader dr, InventoryTransferOrderLine inventoryTransferOrderLine, ref int rowCount)
        {
            PopulateTransferLine(dr, inventoryTransferOrderLine);
            inventoryTransferOrderLine.Barcode = dr["ITEMBARCODE"] == DBNull.Value ? "" : (string)dr["ITEMBARCODE"];
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

        private static string ResolveSortPaged(InventoryTransferOrderLineSortEnum sortEnum, bool sortBackwards, bool useAlias = false)
        {
            var sortString = " ORDER BY " + GetSortColumn(sortEnum, useAlias) + " ASC";

            if (sortBackwards)
            {
                sortString = sortString.Replace("ASC", "DESC");
            }

            return sortString;
        }

        private static string GetSortColumn(InventoryTransferOrderLineSortEnum sortEnum, bool useAlias = false)
        {
            string sortColumn;

            switch (sortEnum)
            {
                case InventoryTransferOrderLineSortEnum.ItemName:
                    sortColumn = useAlias ? "it.ITEMNAME" : "ITEMNAME";
                    break;
                case InventoryTransferOrderLineSortEnum.SentQuantity:
                    sortColumn = useAlias ? "itl.QUANTITYSENT" : "QUANTITYSENT";
                    break;
                case InventoryTransferOrderLineSortEnum.ReceivedQuantity:
                    sortColumn = useAlias ? "itl.QUANTITYRECEIVED" : "QUANTITYRECEIVED";
                    break;
                case InventoryTransferOrderLineSortEnum.RequestedQuantity:
                    sortColumn = useAlias ? "itrl.QUANTITYREQUESTED" : "QUANTITYREQUESTED";
                    break;
                case InventoryTransferOrderLineSortEnum.Sent:
                    sortColumn = useAlias ? "itl.SENT" : "SENT";
                    break;
                case InventoryTransferOrderLineSortEnum.ItemID:
                    sortColumn = useAlias ? "it.ITEMID" : "ITEMID";
                    break;
                case InventoryTransferOrderLineSortEnum.VariantName:
                    sortColumn = useAlias ? "it.VARIANTNAME" : "VARIANTNAME";
                    break;
                case InventoryTransferOrderLineSortEnum.Barcode:
                    sortColumn = useAlias ? "itb.ITEMBARCODE" : "ITEMBARCODE";
                    break;
                default:
                    sortColumn = useAlias ? "itl.ITEMID" : "ITEMID";
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
                case InventoryTransferOrderLineSortEnum.ReceivedQuantity:
                    sortString += "QUANTITYRECEIVED";
                    break;
                case InventoryTransferOrderLineSortEnum.SentQuantity:
                    sortString += "QUANTITYSENT";
                    break;
                case InventoryTransferOrderLineSortEnum.RequestedQuantity:
                    sortString += "QUANTITYREQUESTED";
                    break;
                case InventoryTransferOrderLineSortEnum.Sent:
                    sortString += "itl.SENT";
                    break;                
                default:
                    sortString += "ITEMNAME";
                    break;
            }

            sortString += (sortBackwards) ? " DESC" : " ASC";

            return sortString;
        }

        public virtual InventoryTransferOrderLine Get(IConnectionManager entry, RecordIdentifier transferLineId)
        {
            List<InventoryTransferOrderLine> result;
            SqlCommand cmd;

            ValidateSecurity(entry);

            using (cmd = new SqlCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "itl.ID = @TRANSFERLINEID " });

                cmd.CommandText = string.Format(
                 QueryTemplates.BaseQuery("INVENTORYTRANSFERORDERLINE", "ITL"),
                 QueryPartGenerator.InternalColumnGenerator(inventoryTransferOrderColumns),
                 QueryPartGenerator.JoinGenerator(listJoins),
                 QueryPartGenerator.ConditionGenerator(conditions),
                 string.Empty
                 );

                MakeParam(cmd, "TRANSFERLINEID", (Guid)transferLineId);

                result = Execute<InventoryTransferOrderLine>(entry, cmd, CommandType.Text, PopulateTransferLine);
            }

            return (result.Count > 0) ? result[0] : null;
        }

        public virtual InventoryTransferOrderLine GetLine(IConnectionManager entry, InventoryTransferOrderLine line)
        {
            List<InventoryTransferOrderLine> result;
            SqlCommand cmd;

            ValidateSecurity(entry);

            using (cmd = new SqlCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "itl.INVENTORYTRANSFERORDERID = @INVENTORYTRANSFERORDERID " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "itl.ITEMID = @ITEMID " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "itl.UNITID = @UNITID " });

                cmd.CommandText = string.Format(
                 QueryTemplates.BaseQuery("INVENTORYTRANSFERORDERLINE", "ITL"),
                 QueryPartGenerator.InternalColumnGenerator(inventoryTransferOrderColumns),
                 QueryPartGenerator.JoinGenerator(listJoins),
                 QueryPartGenerator.ConditionGenerator(conditions),
                 string.Empty
                 );

                MakeParam(cmd, "INVENTORYTRANSFERORDERID", (string)line.InventoryTransferId);
                MakeParam(cmd, "ITEMID", (string)line.ItemId);
                MakeParam(cmd, "UNITID", (string)line.UnitId);

                result = Execute<InventoryTransferOrderLine>(entry, cmd, CommandType.Text, PopulateTransferLine);
            }

            return (result.Count > 0) ? result[0] : null;
        }

        public List<InventoryTransferOrderLine> GetListForInventoryTransfer(
            IConnectionManager entry, 
            RecordIdentifier inventoryTransferId, 
            InventoryTransferOrderLineSortEnum sortBy, 
            bool sortBackwards,
            bool getUnsentItemsOnly = false)
        {
            List<InventoryTransferOrderLine> result;
            SqlCommand cmd;

            ValidateSecurity(entry);

            using (cmd = new SqlCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "itl.INVENTORYTRANSFERORDERID = @TRANSFERORDERID " });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "itl.DATAAREAID = @DATAAREAID " });
                if (getUnsentItemsOnly)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itl.SENT = 0 " });
                }

                cmd.CommandText = transferRequestIdDeclaration +
                    string.Format(
                 QueryTemplates.BaseQuery("INVENTORYTRANSFERORDERLINE", "ITL"),
                 QueryPartGenerator.InternalColumnGenerator(inventoryTransferOrderColumns),
                 QueryPartGenerator.JoinGenerator(minimalJoins),
                 QueryPartGenerator.ConditionGenerator(conditions),
                    ResolveSort(sortBy, sortBackwards)
                 );

                MakeParam(cmd, "TRANSFERORDERID", (string)inventoryTransferId);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);


                result = Execute<InventoryTransferOrderLine>(entry, cmd, CommandType.Text, PopulateTransferLine);
            }

            return result;
        }

        public List<InventoryTransferOrderLine> GetOrderLinesForInventoryTransferAdvanced(IConnectionManager entry, RecordIdentifier transferOrderID, out int totalRecordsMatching, InventoryTransferFilterExtended filter)
        {
            List<InventoryTransferOrderLine> result;

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>();
                columns.AddRange(inventoryTransferOrderColumns);
                columns.Add(new TableColumn { ColumnName = "ITEMBARCODE", ColumnAlias = "ITEMBARCODE", TableAlias = "ITB" });
                columns.Add(new TableColumn { ColumnName = string.Format("ROW_NUMBER() OVER({0})", ResolveSortPaged(filter.TransferOrderSortBy, filter.SortDescending, true)), ColumnAlias = "ROW" });


                List<Condition> externalConditions = new List<Condition>();
                externalConditions.Add(new Condition { Operator = "AND", ConditionValue = "CTE.ROW BETWEEN @ROWFROM AND @ROWTO" });

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "ITL.INVENTORYTRANSFERORDERID = @TRANSFERORDERID " });
                MakeParam(cmd, "TRANSFERORDERID", transferOrderID);

                if (!string.IsNullOrEmpty(filter.DescriptionOrID))
                {
                    string searchString = PreProcessSearchText(filter.DescriptionOrID, true, filter.DescriptionOrIDBeginsWith);
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "(IT.ITEMNAME LIKE @DESCRIPTION OR IT.ITEMID LIKE @DESCRIPTION OR IT.VARIANTNAME LIKE @DESCRIPTION OR ITB.ITEMBARCODE LIKE @DESCRIPTION)"
                    });
                    MakeParam(cmd, "DESCRIPTION", searchString);
                }

                if (!string.IsNullOrEmpty(filter.Barcode))
                {
                    string searchString = PreProcessSearchText(filter.Barcode, true, filter.BarcodeBeginsWith);
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "(ITB.ITEMBARCODE LIKE @BARCODE)"
                    });
                    MakeParam(cmd, "BARCODE", searchString);
                }

                if (filter.RequestedQuantityFrom != null)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "ITRL.QUANTITYREQUESTED >= @RECEIVEDQUANTITYFROM "
                    });
                    MakeParam(cmd, "RECEIVEDQUANTITYFROM", filter.RequestedQuantityFrom.Value, SqlDbType.Decimal);
                }

                if (filter.RequestedQuantityTo != null)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "ITRL.QUANTITYREQUESTED <= @RECEIVEDQUANTITYTO "
                    });
                    MakeParam(cmd, "RECEIVEDQUANTITYTO", filter.RequestedQuantityTo.Value, SqlDbType.Decimal);
                }

                if (filter.SentQuantityFrom != null)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "ITL.QUANTITYSENT >= @SENTQUANTITYFROM "
                    });
                    MakeParam(cmd, "SENTQUANTITYFROM", filter.SentQuantityFrom.Value, SqlDbType.Decimal);
                }

                if (filter.SentQuantityTo != null)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "ITL.QUANTITYSENT <= @SENTQUANTITYTO "
                    });
                    MakeParam(cmd, "SENTQUANTITYTO", filter.SentQuantityTo.Value, SqlDbType.Decimal);
                }

                if (filter.ReceivedQuantityFrom != null)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "ITL.QUANTITYRECEIVED >= @RECEIVEDQUANTITYFROM "
                    });
                    MakeParam(cmd, "RECEIVEDQUANTITYFROM", filter.ReceivedQuantityFrom.Value, SqlDbType.Decimal);
                }

                if (filter.ReceivedQuantityTo != null)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "ITL.QUANTITYRECEIVED <= @RECEIVEDQUANTITYTO "
                    });
                    MakeParam(cmd, "RECEIVEDQUANTITYTO", filter.ReceivedQuantityTo.Value, SqlDbType.Decimal);
                }

                if (filter.Sent != null)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "ITL.SENT = @SENT "
                    });
                    MakeParam(cmd, "SENT", filter.Sent, SqlDbType.Bit);
                }

                conditions.Add(new Condition{ Operator = "AND", ConditionValue = "ITL.DATAAREAID = @DATAAREAID"});

                string joinCondition = QueryPartGenerator.JoinGenerator(minimalJoins);
                joinCondition += Environment.NewLine + "OUTER APPLY (SELECT TOP 1 ITEMBARCODE FROM INVENTITEMBARCODE IB WHERE IB.DATAAREAID = @DATAAREAID AND IB.ITEMID = IT.ITEMID AND IB.BLOCKED != 1 AND IB.DELETED = 0 ORDER BY IB.RBOSHOWFORITEM DESC, IB.QTY ASC) itb";
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                string baseQuery = string.Format(QueryTemplates.BaseQuery("INVENTORYTRANSFERORDERLINE", "ITL"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    joinCondition,
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

                cmd.CommandText = transferRequestIdDeclaration +
                    $";WITH CTE AS ({baseQuery}) " +
                    $"SELECT {QueryPartGenerator.ExternalColumnGenerator(columns, "CTE")}, ROW_COUNT = tCountTransactions.ROW_COUNT " +
                    $"FROM CTE CROSS JOIN (SELECT Count(*) AS ROW_COUNT FROM CTE) AS tCountTransactions " +
                    $"{QueryPartGenerator.ConditionGenerator(externalConditions)}" +
                    $"{ResolveSortPaged(filter.TransferOrderSortBy, filter.SortDescending)}";

                MakeParam(cmd, "ROWFROM", filter.RowFrom, SqlDbType.Int);
                MakeParam(cmd, "ROWTO", filter.RowTo, SqlDbType.Int);
                totalRecordsMatching = 0;
                result = Execute<InventoryTransferOrderLine, int>(entry, cmd, CommandType.Text, ref totalRecordsMatching, PopulateTransferLineWithItemsAndRowCount);
            }

            return result;
        }

        public virtual void AutoSetQuantityOnTransferOrderLines(IConnectionManager entry, RecordIdentifier tranferOrderID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = $"UPDATE INVENTORYTRANSFERORDERLINE SET QUANTITYRECEIVED = QUANTITYSENT WHERE INVENTORYTRANSFERORDERID = '{tranferOrderID.StringValue}' AND DATAAREAID = '{entry.Connection.DataAreaId}'";
                entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
            }
        }

        public virtual void SendTransferOrderLines(IConnectionManager entry, RecordIdentifier transferOrderID, bool populateReceivingQuantity)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = $@"UPDATE ITR  SET SENT = 1, COSTPRICE = IIF(CC.COSTCALCULATION = 2, R.PURCHASEPRICE, ISNULL(C.COST, 0)) {(populateReceivingQuantity ? ", QUANTITYRECEIVED = QUANTITYSENT" : "")}
                FROM INVENTORYTRANSFERORDERLINE ITR
                INNER JOIN INVENTORYTRANSFERORDER ITO ON ITO.ID = ITR.INVENTORYTRANSFERORDERID
                INNER JOIN RETAILITEM R ON R.ITEMID = ITR.ITEMID
                OUTER APPLY (SELECT TOP 1 COST FROM RETAILITEMCOST C WHERE C.STOREID = ITO.SENDINGSTOREID AND C.ITEMID = ITR.ITEMID ORDER BY ENTRYDATE DESC) C
                OUTER APPLY (SELECT [Value] AS COSTCALCULATION FROM SYSTEMSETTINGS SS WHERE SS.GUID = '2BAB2653-C366-480E-8DC2-99107BC03D5F') CC
                WHERE ITR.INVENTORYTRANSFERORDERID = '{transferOrderID.StringValue}' AND ITR.DATAAREAID = '{entry.Connection.DataAreaId}' AND ITR.SENT = 0";

                entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
            }
        }

        public virtual void CalculateReceivingItemCosts(IConnectionManager entry, RecordIdentifier transferOrderID)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand("spINVENTORY_CalculateReceivingTransferOrderCosts"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;

                MakeParam(cmd, "TRANSFERORDERID", (string)transferOrderID);
                MakeParam(cmd, "USERID", entry.CurrentUser.ID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                entry.Connection.ExecuteNonQuery(cmd, false);
            }
        }

        public virtual bool ItemWithSameParametersExists(IConnectionManager entry, InventoryTransferOrderLine line)
        {
            return RecordExists(entry, "INVENTORYTRANSFERORDERLINE", new[] { "INVENTORYTRANSFERORDERID", "ITEMID",  "UNITID" }, new RecordIdentifier(line.InventoryTransferId, line.ItemId,  line.UnitId));
        }
 
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "INVENTORYTRANSFERORDERLINE", "ID", id);
        }

        private bool Exists(IConnectionManager entry, RecordIdentifier orderID, RecordIdentifier lineID, List<InventoryTransferOrderLine> cachedLines)
        {
            if (cachedLines == null || !cachedLines.Any())
            {
                cachedLines = GetListForInventoryTransfer(entry, orderID, InventoryTransferOrderLineSortEnum.ItemName, false);
            }

            if (cachedLines == null || !cachedLines.Any())
            {
                return false;
            }

            return cachedLines.FirstOrDefault(f => f.ID == lineID) != null;
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "INVENTORYTRANSFERORDERLINE", "ID", id, BusinessObjects.Permission.EditInventoryTransfersOrders);
        }
        

        public virtual void Save(IConnectionManager entry, InventoryTransferOrderLine inventoryTransferOrderLine)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.EditInventoryTransfersOrders);
            var statement = new SqlServerStatement("INVENTORYTRANSFERORDERLINE", false); // Dont' create replication actions because replication is handled through Site Service

            if (inventoryTransferOrderLine.ID == "" || inventoryTransferOrderLine.ID.IsEmpty)
            {
                inventoryTransferOrderLine.ID = Guid.NewGuid();
            }

            if (RecordIdentifier.IsEmptyOrNull(inventoryTransferOrderLine.InventoryTransferRequestLineID))
            {
                inventoryTransferOrderLine.InventoryTransferRequestLineID = Guid.Empty;
            }

            if (!Exists(entry, inventoryTransferOrderLine.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("ID", (Guid)inventoryTransferOrderLine.ID, SqlDbType.UniqueIdentifier);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ID", (Guid)inventoryTransferOrderLine.ID, SqlDbType.UniqueIdentifier);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("INVENTORYTRANSFERORDERID", (string)inventoryTransferOrderLine.InventoryTransferId);
            statement.AddField("ITEMID", (string)inventoryTransferOrderLine.ItemId);
            statement.AddField("UNITID", (string)inventoryTransferOrderLine.UnitId);
            statement.AddField("SENT", inventoryTransferOrderLine.Sent, SqlDbType.Bit);
            statement.AddField("QUANTITYSENT", inventoryTransferOrderLine.QuantitySent, SqlDbType.Decimal);
            statement.AddField("QUANTITYRECEIVED", inventoryTransferOrderLine.QuantityReceived, SqlDbType.Decimal);
            statement.AddField("PICTUREID", (string)inventoryTransferOrderLine.PictureID);
            statement.AddField("OMNILINEID", inventoryTransferOrderLine.OmniLineID);
            statement.AddField("OMNITRANSACTIONID", inventoryTransferOrderLine.OmniTransactionID);
            statement.AddField("COSTPRICE", inventoryTransferOrderLine.CostPrice, SqlDbType.Decimal);
            statement.AddField("INVENTORYTRANSFERREQUESTLINEID", (Guid)inventoryTransferOrderLine.InventoryTransferRequestLineID, SqlDbType.UniqueIdentifier);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void SaveIfChanged(IConnectionManager entry, InventoryTransferOrderLine inventoryTransferOrderLine)
        {
            var previousLine = Get(entry, inventoryTransferOrderLine.ID);

            if (previousLine != inventoryTransferOrderLine)
            {
                Save(entry, inventoryTransferOrderLine);
            }
        }

        protected virtual SqlServerStatement PrepareStatement(SqlServerStatement statement, StatementType statementType)
        {
            if (statement == null)
            {
                statement = new SqlServerStatement("INVENTORYTRANSFERORDERLINE", false);
                statement.StatementType = statementType;

                if (statementType == StatementType.Insert)
                {
                    statement.AddKey("ID", Guid.Empty, SqlDbType.UniqueIdentifier);
                    statement.AddKey("DATAAREAID", "");
                }
                else
                {
                    statement.AddCondition("ID", Guid.Empty, SqlDbType.UniqueIdentifier);
                    statement.AddCondition("DATAAREAID", "");
                }

                statement.AddField("INVENTORYTRANSFERORDERID", "");
                statement.AddField("ITEMID", "");
                statement.AddField("UNITID", "");
                statement.AddField("SENT", 0, SqlDbType.Bit);
                statement.AddField("QUANTITYSENT", decimal.Zero, SqlDbType.Decimal);
                statement.AddField("QUANTITYRECEIVED", decimal.Zero, SqlDbType.Decimal);                             
                statement.AddField("INVENTORYTRANSFERREQUESTLINEID", Guid.Empty, SqlDbType.UniqueIdentifier);                             
            }

            return statement;
        }

        protected virtual SqlServerStatement Save(IConnectionManager entry, InventoryTransferOrderLine line, SqlServerStatement statement, StatementType statementType)
        {
            statement = PrepareStatement(statement, statementType);

            if (line.ID == "" || line.ID.IsEmpty)
            {
                line.ID = Guid.NewGuid();
            }

            if (line.InventoryTransferRequestLineID == "" || line.InventoryTransferRequestLineID.IsEmpty)
            {
                line.InventoryTransferRequestLineID = Guid.NewGuid();
            }

            if (statementType == StatementType.Insert)
            {
                statement.UpdateField("ID", (Guid)line.ID);
                statement.UpdateField("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.UpdateCondition(0, "ID", (Guid)line.ID);
                statement.UpdateCondition(1, "DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.UpdateField("INVENTORYTRANSFERORDERID", (string)line.InventoryTransferId);
            statement.UpdateField("ITEMID", (string)line.ItemId);
            statement.UpdateField("UNITID", (string)line.UnitId);
            statement.UpdateField("SENT", line.Sent);
            statement.UpdateField("QUANTITYSENT", line.QuantitySent);
            statement.UpdateField("QUANTITYRECEIVED", line.QuantityReceived);
            statement.UpdateField("INVENTORYTRANSFERREQUESTLINEID", (Guid)line.InventoryTransferRequestLineID);

            entry.Connection.ExecuteStatement(statement);

            return statement;

        }        

        public virtual void SaveLines(IConnectionManager entry, List<InventoryTransferOrderLine> lines)
        {
            if (lines == null || !lines.Any())
            {
                return;
            }            

            RecordIdentifier transferOrderID = lines[0].InventoryTransferId;
            List<InventoryTransferOrderLine> existingLines = new List<InventoryTransferOrderLine>();

            ValidateSecurity(entry, Permission.EditInventoryTransfersOrders);
            
            foreach (InventoryTransferOrderLine line in lines)
            {
                if (!Exists(entry, transferOrderID, line.ID, existingLines))
                {
                    insertStatement = Save(entry, line, insertStatement, StatementType.Insert);
                    existingLines.Add(line);
                }
                else
                {
                    updateStatment = Save(entry, line, updateStatment, StatementType.Update);
                }
            }            
        }

        public virtual void CopyLines(IConnectionManager entry, RecordIdentifier copyFrom, RecordIdentifier copyTo)
        {
            ValidateSecurity(entry, Permission.EditInventoryTransfersOrders);

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText =
                    @" INSERT INTO INVENTORYTRANSFERORDERLINE
                       (ID, INVENTORYTRANSFERORDERID, ITEMID, UNITID, QUANTITYSENT, QUANTITYRECEIVED, SENT, DATAAREAID)
                       SELECT NEWID(), @COPYTO, ITEMID, UNITID, QUANTITYSENT, 0, 0, DATAAREAID FROM INVENTORYTRANSFERORDERLINE WHERE INVENTORYTRANSFERORDERID = @COPYFROM ";

                MakeParam(cmd, "COPYTO", (string)copyTo);
                MakeParam(cmd, "COPYFROM", (string)copyFrom);

                entry.Connection.ExecuteScalar(cmd);
            }
        }  
        
        public virtual void CopyLinesFromRequest(IConnectionManager entry, RecordIdentifier copyFromRequest, RecordIdentifier copyToOrder)
        {
            ValidateSecurity(entry, Permission.EditInventoryTransfersOrders);

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText =
                    @" INSERT INTO INVENTORYTRANSFERORDERLINE
                       (ID, INVENTORYTRANSFERORDERID, ITEMID, UNITID, QUANTITYSENT, QUANTITYRECEIVED, SENT, DATAAREAID, INVENTORYTRANSFERREQUESTLINEID)                       
                       select NEWID(), @COPYTO, ITEMID, UNITID, QUANTITYREQUESTED, 0, 0, DATAAREAID, ID from INVENTORYTRANSFERREQUESTLINE WHERE INVENTORYTRANSFERREQUESTID = @COPYFROM ";

                MakeParam(cmd, "COPYTO", (string)copyToOrder);
                MakeParam(cmd, "COPYFROM", (string)copyFromRequest);

                entry.Connection.ExecuteScalar(cmd);
            }
        }

        public virtual int LineCount(IConnectionManager entry, RecordIdentifier orderID)
        {
            return Count(entry, "INVENTORYTRANSFERORDERLINE", "INVENTORYTRANSFERORDERID", orderID);            
        }

        /// <summary>
		/// Imports transfer order lines from an xml file
		/// </summary>
		/// <param name="entry">The entry into the database</param>
        /// <param name="transferOrderID">ID of the transfer order in which to import lines</param>
		/// <param name="xmlData">XML data to import</param>
		public virtual int ImportTransferOrderLinesFromXML(IConnectionManager entry, RecordIdentifier transferOrderID, string xmlData)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand("spINVENTORY_ImportTransferOrderLinesFromXML"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;

                MakeParam(cmd, "TRANSFERORDERID", (string)transferOrderID);
                MakeParam(cmd, "XMLDATA", xmlData, SqlDbType.Xml);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                SqlParameter insertedRows = MakeParam(cmd, "INSERTEDRECORDS", "", SqlDbType.Int, ParameterDirection.Output, 4);

                entry.Connection.ExecuteNonQuery(cmd, false);

                return (int)insertedRows.Value;
            }
        }

        /// <summary>
        /// Updates a single line with a picture ID based on the transaction ID and line IDs from the inventory app
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="omniTransactionID">The ID of the transaction in the inventory app that this line was created on</param>
        /// <param name="omniLineID">The ID of the line that was assigned to it by the inventory app</param>
        /// <param name="pictureID">The ID of the picture to set on the line</param>
        public virtual void SetPictureIDForOmniLine(IConnectionManager entry, string omniTransactionID, string omniLineID, RecordIdentifier pictureID)
        {
            var statement = new SqlServerStatement("INVENTORYTRANSFERORDERLINE");

            statement.StatementType = StatementType.Update;
            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("OMNITRANSACTIONID", omniTransactionID);
            statement.AddCondition("OMNILINEID", omniLineID);

            statement.AddField("PICTUREID", (string)pictureID);

            entry.Connection.ExecuteStatement(statement);

        }
    }
}
