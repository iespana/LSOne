using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Enums;
using System.Data.SqlClient;
using LSOne.Utilities.ErrorHandling;
using System.Linq;

namespace LSOne.DataLayer.SqlDataProviders.Inventory
{
    public class InventoryTransferOrderData : SqlServerDataProviderBase, IInventoryTransferOrderData
    {
        private static List<TableColumn> transferColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "ID " , TableAlias = "it"},
            new TableColumn {ColumnName = "ISNULL(it.DESCRIPTION, '') " , ColumnAlias = "DESCRIPTION"},
            new TableColumn {ColumnName = "ISNULL(it.INVENTORYTRANSFERREQUESTID, '') " ,ColumnAlias = "INVENTORYTRANSFERREQUESTID"},
            new TableColumn {ColumnName = "SENDINGSTOREID " , TableAlias = "it"},
            new TableColumn {ColumnName = "ISNULL(st1.NAME,'') " ,ColumnAlias = "SENDINGSTORENAME"},
            new TableColumn {ColumnName = "RECEIVINGSTOREID " , TableAlias = "it"},
            new TableColumn {ColumnName = "ISNULL(st2.NAME,'') " , ColumnAlias = "RECEIVINGSTORENAME"},
            new TableColumn {ColumnName = "CREATIONDATE ", TableAlias = "it"},
            new TableColumn {ColumnName = "ISNULL(it.RECEIVINGDATE, '01.01.1900')  " , ColumnAlias = "RECEIVINGDATE"},
            new TableColumn {ColumnName = "ISNULL(it.SENTDATE, '01.01.1900') " , ColumnAlias = "SENTDATE"},
            new TableColumn {ColumnName = "RECEIVED ", TableAlias = "it"},
            new TableColumn {ColumnName = "SENT ", TableAlias = "it"},
            new TableColumn {ColumnName = "REJECTED ", TableAlias = "it"},
            new TableColumn {ColumnName = "FETCHEDBYRECEIVINGSTORE ", TableAlias = "it"},
            new TableColumn {ColumnName = "CREATEDBY ", TableAlias = "it"},
            new TableColumn {ColumnName = "ISNULL(it.EXPECTEDDELIVERY, '01.01.1900') ", ColumnAlias = "EXPECTEDDELIVERY"},
            new TableColumn {ColumnName = "CREATEDFROMOMNI ", ColumnAlias = "CREATEDFROMOMNI", TableAlias = "it"},
            new TableColumn {ColumnName = "TEMPLATEID ", ColumnAlias = "TEMPLATEID", IsNull = true, NullValue = "''", TableAlias = "it"},
            new TableColumn {ColumnName = "PROCESSINGSTATUS ", ColumnAlias = "PROCESSINGSTATUS", TableAlias = "it"}
        };

        private static List<Join> listJoins = new List<Join>
        {
            new Join
            {
                Condition = "st1.STOREID = it.SENDINGSTOREID and st1.DATAAREAID = it.DATAAREAID",
                JoinType = "LEFT OUTER",
                Table = "RBOSTORETABLE",
                TableAlias = "st1"
            },
            new Join
            {
                Condition = "st2.STOREID = it.RECEIVINGSTOREID and st2.DATAAREAID = it.DATAAREAID",
                JoinType = "LEFT OUTER",
                Table = "RBOSTORETABLE",
                TableAlias = "st2"
            }
        };

        private static List<TableColumn> quantityColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "ISNULL(lines.ITEMLINES, 0) ", ColumnAlias = "ITEMLINES"}
        };

        private static List<Join> quantityJoins = new List<Join>
        {
            new Join
            {
                Condition = "it.ID = lines.INVENTORYTRANSFERORDERID",
                JoinType = "LEFT",
                Table = "(SELECT INVENTORYTRANSFERORDERID, COUNT(*) AS ITEMLINES FROM INVENTORYTRANSFERORDERLINE GROUP BY INVENTORYTRANSFERORDERID)",
                TableAlias = "lines"
            },
        };

        private static void PopulateTransfer(IDataReader dr, InventoryTransferOrder inventoryTransferOrder)
        {
            inventoryTransferOrder.ID = (string)dr["ID"];
            inventoryTransferOrder.Text = (string)dr["DESCRIPTION"];
            inventoryTransferOrder.InventoryTransferRequestId = (string)dr["INVENTORYTRANSFERREQUESTID"];
            inventoryTransferOrder.SendingStoreId = (string)dr["SENDINGSTOREID"];
            inventoryTransferOrder.SendingStoreName = (string)dr["SENDINGSTORENAME"];
            inventoryTransferOrder.ReceivingStoreId = (string)dr["RECEIVINGSTOREID"];
            inventoryTransferOrder.ReceivingStoreName = (string)dr["RECEIVINGSTORENAME"];
            inventoryTransferOrder.CreationDate = (DateTime)dr["CREATIONDATE"];
            inventoryTransferOrder.ReceivingDate = (DateTime)dr["RECEIVINGDATE"];
            inventoryTransferOrder.SentDate = (DateTime)dr["SENTDATE"];
            inventoryTransferOrder.Received = (bool)dr["RECEIVED"];
            inventoryTransferOrder.Sent = (bool)dr["SENT"];
            inventoryTransferOrder.Rejected = (bool)dr["REJECTED"];
            inventoryTransferOrder.FetchedByReceivingStore = (bool)dr["FETCHEDBYRECEIVINGSTORE"];
            inventoryTransferOrder.CreatedBy = (string)dr["CREATEDBY"];
            inventoryTransferOrder.ExpectedDelivery = (DateTime)dr["EXPECTEDDELIVERY"];
            inventoryTransferOrder.CreatedFromOmni = (bool)dr["CREATEDFROMOMNI"];
            inventoryTransferOrder.TemplateID = (string)dr["TEMPLATEID"];
            inventoryTransferOrder.ProcessingStatus = (InventoryProcessingStatus)dr["PROCESSINGSTATUS"];
        }

        protected static void PopulateRowCount(IConnectionManager entry, IDataReader dr, ref int rowCount)
        {
            if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 ||
                entry.Connection.DatabaseVersion == ServerVersion.Unknown)
            {
                rowCount = (int)dr["Row_Count"];
            }
        }

        private static void PopulateTransferWithItemsCount(IDataReader dr, InventoryTransferOrder inventoryTransferOrder)
        {
            PopulateTransfer(dr, inventoryTransferOrder);
            inventoryTransferOrder.NoOfItems = AsDecimal(dr["ITEMLINES"]);
        }

        private static void PopulateTransferWithItemsAndRowCount(IConnectionManager entry, IDataReader dr, InventoryTransferOrder inventoryTransferOrder, ref int rowCount)
        {
            PopulateTransfer(dr, inventoryTransferOrder);
            PopulateRowCount(entry, dr, ref rowCount);
            inventoryTransferOrder.NoOfItems = AsDecimal(dr["ITEMLINES"]);
        }

        private static string ResolveSortPaged(InventoryTransferOrderSortEnum sortEnum, bool sortBackwards, string tableAlias = "")
        {
            var sortString = " ORDER BY " + GetSortColumn(sortEnum, tableAlias) + " ASC";

            if (sortBackwards)
            {
                sortString = sortString.Replace("ASC", "DESC");
            }

            return sortString;
        }

        private static string GetSortColumn(InventoryTransferOrderSortEnum sortEnum, string tableAlias = "")
        {
            var sortColumn = "";
            tableAlias = tableAlias == "" ? tableAlias : tableAlias + ".";

            switch (sortEnum)
            {
                case InventoryTransferOrderSortEnum.Id:
                    sortColumn = tableAlias + "Id";
                    break;
                case InventoryTransferOrderSortEnum.CreatedDate:
                    sortColumn = tableAlias + "CREATIONDATE";
                    break;
                case InventoryTransferOrderSortEnum.ReceivedDate:
                    sortColumn = tableAlias + "RECEIVINGDATE";
                    break;
                case InventoryTransferOrderSortEnum.SentDate:
                    sortColumn = tableAlias + "SENTDATE";
                    break;
                case InventoryTransferOrderSortEnum.SendingStore:
                    sortColumn = "st1.NAME";
                    break;
                case InventoryTransferOrderSortEnum.ReceivingStore:
                    sortColumn = "st2.NAME";
                    break;
                case InventoryTransferOrderSortEnum.Received:
                    sortColumn = tableAlias + "RECEIVED";
                    break;
                case InventoryTransferOrderSortEnum.Sent:
                    sortColumn = tableAlias + "SENT";
                    break;
                case InventoryTransferOrderSortEnum.Fetched:
                    sortColumn = tableAlias + "FETCHEDBYRECEIVINGSTORE";
                    break;
                case InventoryTransferOrderSortEnum.Description:
                    sortColumn = tableAlias + "DESCRIPTION";
                    break;
                case InventoryTransferOrderSortEnum.SentQuantity:
                    sortColumn = tableAlias + "SENTQUANTITY";
                    break;
                case InventoryTransferOrderSortEnum.ReceivedQuantity:
                    sortColumn = tableAlias + "RECEIVEDQUANTITY";
                    break;
                case InventoryTransferOrderSortEnum.ItemLines:
                    sortColumn = "ITEMLINES";
                    break;
                case InventoryTransferOrderSortEnum.ExpectedDelivery:
                    sortColumn = tableAlias + "EXPECTEDDELIVERY";
                    break;
            }
            return sortColumn;
        }

        private static string ResolveSort(InventoryTransferOrderSortEnum orderSort, bool sortBackwards)
        {
            string sortString = " ORDER BY ";
            switch (orderSort)
            {
                case InventoryTransferOrderSortEnum.Id:
                    sortString += "it.Id";
                    break;
                case InventoryTransferOrderSortEnum.CreatedDate:
                    sortString += "it.CREATIONDATE" ;
                    break;
                case InventoryTransferOrderSortEnum.ReceivedDate:
                    sortString += "RECEIVINGDATE";
                    break;
                case InventoryTransferOrderSortEnum.SentDate:
                    sortString += "SENTDATE";
                    break;
                case InventoryTransferOrderSortEnum.SendingStore:
                    sortString += "SENDINGSTORENAME";
                    break;
                case InventoryTransferOrderSortEnum.ReceivingStore:
                    sortString += "RECEIVINGSTORENAME";
                    break;
                case InventoryTransferOrderSortEnum.Sent:
                    sortString += " it.SENT";
                    break;
                case InventoryTransferOrderSortEnum.Received:
                    sortString += " it.RECEIVED";
                    break;
                case InventoryTransferOrderSortEnum.Fetched:
                    sortString += " it.FETCHEDBYRECEIVINGSTORE";
                    break;
                case InventoryTransferOrderSortEnum.Description:
                    sortString += " DESCRIPTION";
                    break;
                case InventoryTransferOrderSortEnum.SentQuantity:
                    sortString += " sent.SENTQUANTITY";
                    break;
                case InventoryTransferOrderSortEnum.ReceivedQuantity:
                    sortString += " received.RECEIVEDQUANTITY";
                    break;
            }

            sortString += (sortBackwards) ? " DESC" : " ASC";

            return sortString;
        }

        public virtual InventoryTransferOrder Get(IConnectionManager entry, RecordIdentifier transferId)
        {
            List<InventoryTransferOrder> result = new List<InventoryTransferOrder>();

            if (string.IsNullOrEmpty((string) transferId))
            {
                return null;
            }

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.DATAAREAID = @DATAAREAID " });
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                if (!string.IsNullOrEmpty((string)transferId))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.ID = @TRANSFERID " });
                    MakeParam(cmd, "TRANSFERID", (string)transferId);
                }

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("INVENTORYTRANSFERORDER", "it", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(transferColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );

                result = Execute<InventoryTransferOrder>(entry, cmd, CommandType.Text, PopulateTransfer);
            }

            return (result.Count > 0) ? result[0] : null;
        }

        public List<InventoryTransferOrder> GetFromList(
            IConnectionManager entry, 
            List<RecordIdentifier> transferIds,
            InventoryTransferOrderSortEnum orderSortBy,
            bool sortBackwards)
        {
            if (transferIds == null || transferIds.Count == 0)
            {
                return new List<InventoryTransferOrder>();
            }

            List<InventoryTransferOrder> result;

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.DATAAREAID = @DATAAREAID " });
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                
                string transferParams = "";
                for (int i = 0; i < transferIds.Count; i++)
                {
                    if (i != 0)
                    {
                        transferParams += " OR ";
                    }

                    transferParams += "it.ID = @TRANSFERID" + i;
                    MakeParam(cmd, "TRANSFERID" + i, (string)transferIds[i]);
                }

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "(" + transferParams + ") " });
                
                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("INVENTORYTRANSFERORDER", "it", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(transferColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    ResolveSort(orderSortBy, sortBackwards)
                    );

                result = Execute<InventoryTransferOrder>(entry, cmd, CommandType.Text, PopulateTransfer);
            }

            return result;
        }

        public List<InventoryTransferOrder> GetListForStore(
            IConnectionManager entry, 
            List<RecordIdentifier> storeIds, 
            InventoryTransferType transferType, 
            InventoryTransferOrderSortEnum orderSortBy, 
            bool sortBackwards)
        {
            List<InventoryTransferOrder> result;

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.DATAAREAID = @DATAAREAID " });
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);


                if (storeIds != null && storeIds.Count != 0)
                {
                    cmd.CommandText += "AND (";

                    string storeParams = "";
                    for (int i = 0; i < storeIds.Count; i++)
                    {
                        if (i > 0)
                        {
                            storeParams += " OR ";
                        }
                        switch (transferType)
                        {
                            case InventoryTransferType.Outgoing:
                                storeParams += "(it.SENDINGSTOREID = @STOREID" + i + " and it.RECEIVED = 0)";
                                break;
                            case InventoryTransferType.Incoming:
                                storeParams += "(it.RECEIVINGSTOREID = @STOREID" + i + " and it.SENT = 1 and it.RECEIVED = 0)";
                                break;
                            case InventoryTransferType.SendingAndReceiving:
                                storeParams += "(it.RECEIVINGSTOREID = @STOREID" + i + " OR it.SENDINGSTOREID = @STOREID" + i + ")";
                                break;
                            case InventoryTransferType.Finished:
                                storeParams += "((it.RECEIVINGSTOREID = @STOREID" + i + " or it.SENDINGSTOREID = @STOREID" + i + ") and it.RECEIVED = 1)";
                                break;
                        }
                        MakeParam(cmd, "STOREID" + i, (string)storeIds[i]);
                    }

                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "(" + storeParams + ") " });
                }

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("INVENTORYTRANSFERORDER", "it", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(transferColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    ResolveSort(orderSortBy, sortBackwards)
                    );

                result = Execute<InventoryTransferOrder>(entry, cmd, CommandType.Text, PopulateTransfer);
            }

            return result;
        }

        public Dictionary<RecordIdentifier, decimal> GetTotalUnreceivedItemForTransferOrders(IConnectionManager entry, List<RecordIdentifier> transferOrderIds)
        {
            Dictionary<RecordIdentifier, decimal> results = new Dictionary<RecordIdentifier, decimal>();

            if(transferOrderIds.Count == 0)
            {
                return results;
            }
            
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = string.Format(@"SELECT ID, SUM(QTY) AS QTY FROM
                            (SELECT ITR.ID, IIF(TOL.QUANTITYSENT > TOL.QUANTITYRECEIVED, TOL.QUANTITYSENT - TOL.QUANTITYRECEIVED, IIF(ITR.RECEIVED = 0, TOL.QUANTITYSENT, 0)) AS QTY FROM INVENTORYTRANSFERORDER ITR
                            LEFT JOIN INVENTORYTRANSFERORDERLINE TOL ON ITR.ID = TOL.INVENTORYTRANSFERORDERID
                            WHERE ITR.ID IN({0}) AND TOL.DATAAREAID = '{1}') AS X
                            GROUP BY ID", string.Join(",", transferOrderIds.Select(t => $"'{t.StringValue}'")), entry.Connection.DataAreaId);
                
                using (IDataReader dr = entry.Connection.ExecuteReader(cmd, CommandType.Text))
                {
                    while (dr.Read())
                    {
                        results.Add((string)dr["ID"], (decimal)dr["QTY"]);
                    }
                }
            }

            return results;
        }

        public List<InventoryTransferOrder> GetInventoryInTransit(
            IConnectionManager entry,
            InventoryTransferOrderSortEnum orderSortBy,
            bool sortBackwards,
            RecordIdentifier storeId = null)
        {
            List<InventoryTransferOrder> result;

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Join> joins = new List<Join>(listJoins);
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.DATAAREAID = @DATAAREAID " });
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.SENT = 1 "});
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.RECEIVED = 0 "});
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.REJECTED = 0 " });

                if (!RecordIdentifier.IsEmptyOrNull(storeId))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = $"(it.SENDINGSTOREID = '{storeId.StringValue}' OR it.RECEIVINGSTOREID = '{storeId.StringValue}') " });
                }

                joins.Add(
                        new Join
                        {
                            Condition = "itl.INVENTORYTRANSFERORDERID = it.ID and itl.DATAAREAID = it.DATAAREAID",
                            JoinType = "LEFT OUTER",
                            Table = "INVENTORYTRANSFERORDERLINE",
                            TableAlias = "itl"
                        }
                    );

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("INVENTORYTRANSFERORDER", "it", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(transferColumns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    ResolveSort(orderSortBy, sortBackwards)
                    );

                result = Execute<InventoryTransferOrder>(entry, cmd, CommandType.Text, PopulateTransfer);
            }

            return result;
        }

        public virtual List<InventoryTransferOrder> Search(IConnectionManager entry, InventoryTransferFilter filter)
        {
            List<InventoryTransferOrder> result;

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.DATAAREAID = @DATAAREAID " });
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                if(!string.IsNullOrEmpty(filter.DescriptionOrID))
                {
                    string searchString = PreProcessSearchText(filter.DescriptionOrID, true, filter.DescriptionOrIDBeginsWith);
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "(it.DESCRIPTION LIKE @DESCRIPTION OR it.ID LIKE @DESCRIPTION)" });
                    MakeParam(cmd, "DESCRIPTION", searchString);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.StoreID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.SENDINGSTOREID = @STOREID OR it.RECEIVINGSTOREID = @STOREID " });
                    MakeParam(cmd, "STOREID", (string)filter.StoreID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.SendingStoreID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.SENDINGSTOREID = @SENDINGSTOREID " });
                    MakeParam(cmd, "SENDINGSTOREID", (string)filter.SendingStoreID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.ReceivingStoreID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.RECEIVINGSTOREID = @RECEIVINGSTOREID " });
                    MakeParam(cmd, "RECEIVINGSTOREID", (string)filter.ReceivingStoreID);
                }

                if(filter.Status != null)
                {
                    switch (filter.Status.Value)
                    {
                        case TransferOrderStatusEnum.New:
                            conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.SENT = 0 AND it.REJECTED = 0 " });
                            break;
                        case TransferOrderStatusEnum.Sent:
                            conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.SENT = 1 AND it.FETCHEDBYRECEIVINGSTORE = 0 AND it.REJECTED = 0 " });
                            break;
                        case TransferOrderStatusEnum.Received:
                            conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.FETCHEDBYRECEIVINGSTORE = 1 AND it.REJECTED = 0 " });
                            break;
                        case TransferOrderStatusEnum.Rejected:
                            conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.REJECTED = 1 " });
                            break;
                        case TransferOrderStatusEnum.Closed:
                            conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.RECEIVED = 1 " });
                            break;
                        default:
                            break;
                    }
                }

                if(filter.FromDate != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "(it.CREATIONDATE >= @FROMDATE OR it.RECEIVINGDATE >= @FROMDATE OR it.SENTDATE >= @FROMDATE OR it.EXPECTEDDELIVERY >= @FROMDATE) " });
                    MakeParam(cmd, "FROMDATE", filter.FromDate.Value.Date, SqlDbType.DateTime);
                }

                if (filter.ToDate != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "(it.CREATIONDATE <= @TODATE OR it.RECEIVINGDATE <= @TODATE OR it.SENTDATE <= @TODATE OR it.EXPECTEDDELIVERY <= @TODATE) " });
                    MakeParam(cmd, "TODATE", filter.ToDate.Value.Date.AddDays(1).AddSeconds(-1), SqlDbType.DateTime);
                }

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("INVENTORYTRANSFERORDER", "it", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(transferColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    ResolveSort(filter.SortBy, filter.SortDescending)
                    );

                result = Execute<InventoryTransferOrder>(entry, cmd, CommandType.Text, PopulateTransfer);
            }

            return result;
        }

        public List<InventoryTransferOrder> Search(IConnectionManager entry, InventoryTransferFilterExtended filter)
        {
            List<InventoryTransferOrder> result;

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>();
                columns.AddRange(transferColumns);
                List<Join> joins = new List<Join>();
                joins.AddRange(listJoins);
                
                //Number of items
                joins.AddRange(quantityJoins);
                columns.AddRange(quantityColumns);

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.DATAAREAID = @DATAAREAID " });
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                if (!string.IsNullOrEmpty(filter.DescriptionOrID))
                {
                    string searchString = PreProcessSearchText(filter.DescriptionOrID, true, filter.DescriptionOrIDBeginsWith);
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "(it.DESCRIPTION LIKE @DESCRIPTION OR it.ID LIKE @DESCRIPTION)" });
                    MakeParam(cmd, "DESCRIPTION", searchString);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.StoreID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "(it.SENDINGSTOREID = @STOREID OR it.RECEIVINGSTOREID = @STOREID) " });
                    MakeParam(cmd, "STOREID", (string)filter.StoreID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.SendingStoreID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.SENDINGSTOREID = @SENDINGSTOREID " });
                    MakeParam(cmd, "SENDINGSTOREID", (string)filter.SendingStoreID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.ReceivingStoreID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.RECEIVINGSTOREID = @RECEIVINGSTOREID " });
                    MakeParam(cmd, "RECEIVINGSTOREID", (string)filter.ReceivingStoreID);
                }
                
                if (filter.SentFrom != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.SENTDATE >= @SENTFROM " });
                    MakeParam(cmd, "SENTFROM", filter.SentFrom.Value.Date, SqlDbType.DateTime);
                }

                if (filter.SentTo != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.SENTDATE <= @SENTTO " });
                    MakeParam(cmd, "SENTTO", filter.SentTo.Value.Date.AddDays(1).AddSeconds(-1), SqlDbType.DateTime);
                }

                if (filter.ItemsFrom != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "ISNULL(lines.ITEMLINES, 0) >= @ITEMSFROM " });
                    MakeParam(cmd, "ITEMSFROM", filter.ItemsFrom.Value, SqlDbType.Decimal);
                }

                if (filter.ItemsTo != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "ISNULL(lines.ITEMLINES, 0) <= @ITEMSTO " });
                    MakeParam(cmd, "ITEMSTO", filter.ItemsTo.Value, SqlDbType.Decimal);
                }

                if (filter.FromDate != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "(it.CREATIONDATE >= @FROMDATE) " });
                    MakeParam(cmd, "FROMDATE", filter.FromDate.Value.Date, SqlDbType.DateTime);
                }

                if (filter.ToDate != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "(it.CREATIONDATE <= @TODATE) " });
                    MakeParam(cmd, "TODATE", filter.ToDate.Value.Date.AddDays(1).AddSeconds(-1), SqlDbType.DateTime);
                }

                if (filter.ExpectedFrom != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.EXPECTEDDELIVERY >= @EXPECTEDFROM " });
                    MakeParam(cmd, "EXPECTEDFROM", filter.ExpectedFrom.Value.Date, SqlDbType.DateTime);
                }

                if (filter.ExpectedTo != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.EXPECTEDDELIVERY <= @EXPECTEDTO " });
                    MakeParam(cmd, "EXPECTEDTO", filter.ExpectedTo.Value.Date.AddDays(1).AddSeconds(-1), SqlDbType.DateTime);
                }

                switch (filter.TransferFilterType)
                {
                    case InventoryTransferType.Outgoing:
                        if(filter.Status != null)
                        {
                            switch (filter.Status.Value)
                            {
                                case TransferOrderStatusEnum.New:
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.SENT = 0 " });
                                    break;
                                case TransferOrderStatusEnum.Sent:
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.SENT = 1 AND it.FETCHEDBYRECEIVINGSTORE = 0 " });
                                    break;
                                case TransferOrderStatusEnum.Received:
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.FETCHEDBYRECEIVINGSTORE = 1 " });
                                    break;
                                default:
                                    break;
                            }
                        }

                        conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.RECEIVED = 0  AND it.REJECTED = 0" });
                        
                        break;
                    case InventoryTransferType.Incoming:
                        if (filter.Status != null)
                        {
                            switch (filter.Status.Value)
                            {
                                case TransferOrderStatusEnum.Sent:
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.SENT = 1 AND it.FETCHEDBYRECEIVINGSTORE = 0 " });
                                    break;
                                case TransferOrderStatusEnum.Received:
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.FETCHEDBYRECEIVINGSTORE = 1 " });
                                    break;
                                default:
                                    break;
                            }
                        }

                        conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.RECEIVED = 0 AND it.SENT = 1 AND it.REJECTED = 0 " });
                        
                        break;
                    case InventoryTransferType.SendingAndReceiving:

                        break;
                    case InventoryTransferType.Finished:
                        if (filter.Status != null)
                        {
                            switch (filter.Status.Value)
                            {
                                case TransferOrderStatusEnum.Closed:
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.RECEIVED = 1 " });
                                    break;
                                case TransferOrderStatusEnum.Rejected:
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.REJECTED = 1 " });
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            conditions.Add(new Condition { Operator = "AND", ConditionValue = "(it.RECEIVED = 1 OR it.REJECTED = 1) " });
                        }
                        break;
                    default:
                        break;
                }

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("INVENTORYTRANSFERORDER", "it", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    ResolveSort(filter.SortBy, filter.SortDescending)
                    );

                result = Execute<InventoryTransferOrder>(entry, cmd, CommandType.Text, PopulateTransferWithItemsCount);
            }

            return result;
        }

        public List<InventoryTransferOrder> AdvancedSearch(IConnectionManager entry, out int totalRecordsMatching, InventoryTransferFilterExtended filter)
        {
            List<InventoryTransferOrder> result;

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>();
                foreach (var transferColumn in transferColumns)
                {
                    columns.Add(transferColumn);
                }
                columns.Add(new TableColumn
                {
                    ColumnName = string.Format("ROW_NUMBER() OVER({0})", ResolveSortPaged(filter.SortBy, filter.SortDescending, "it")),
                    ColumnAlias = "ROW"
                });
                columns.Add(new TableColumn
                {
                    ColumnName =
                        string.Format("COUNT(1) OVER ( {0} RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )",
                            ResolveSortPaged(filter.SortBy, filter.SortDescending, "it")),
                    ColumnAlias = "ROW_COUNT"
                });

                List<Condition> externalConditions = new List<Condition>();
                externalConditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "S.ROW BETWEEN @ROWFROM AND @ROWTO"
                });

                List<Join> joins = new List<Join>();
                joins.AddRange(listJoins);

                //Number of items
                joins.AddRange(quantityJoins);
                columns.AddRange(quantityColumns);

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition {Operator = "AND", ConditionValue = "it.DATAAREAID = @DATAAREAID "});
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                if (!string.IsNullOrEmpty(filter.DescriptionOrID))
                {
                    string searchString =
                        PreProcessSearchText(filter.DescriptionOrID, true, filter.DescriptionOrIDBeginsWith);
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "(it.DESCRIPTION LIKE @DESCRIPTION OR it.ID LIKE @DESCRIPTION)"
                    });
                    MakeParam(cmd, "DESCRIPTION", searchString);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.StoreID))
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "(it.SENDINGSTOREID = @STOREID OR it.RECEIVINGSTOREID = @STOREID) "
                    });
                    MakeParam(cmd, "STOREID", (string) filter.StoreID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.SendingStoreID))
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "it.SENDINGSTOREID = @SENDINGSTOREID "
                    });
                    MakeParam(cmd, "SENDINGSTOREID", (string) filter.SendingStoreID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.ReceivingStoreID))
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "it.RECEIVINGSTOREID = @RECEIVINGSTOREID "
                    });
                    MakeParam(cmd, "RECEIVINGSTOREID", (string) filter.ReceivingStoreID);
                }

                if (filter.SentFrom != null)
                {
                    conditions.Add(new Condition {Operator = "AND", ConditionValue = "it.SENTDATE >= @SENTFROM " });
                    MakeParam(cmd, "SENTFROM", filter.SentFrom.Value.Date, SqlDbType.DateTime);
                }

                if (filter.SentTo != null)
                {
                    conditions.Add(new Condition {Operator = "AND", ConditionValue = "it.SENTDATE <= @SENTTO "});
                    MakeParam(cmd, "SENTTO", filter.SentTo.Value.Date.AddDays(1).AddSeconds(-1), SqlDbType.DateTime);
                }

                if (filter.ItemsFrom != null)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "ISNULL(lines.ITEMLINES, 0) >= @ITEMSFROM "
                    });
                    MakeParam(cmd, "ITEMSFROM", filter.ItemsFrom.Value, SqlDbType.Decimal);
                }

                if (filter.ItemsTo != null)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "ISNULL(lines.ITEMLINES, 0) <= @ITEMSTO "
                    });
                    MakeParam(cmd, "ITEMSTO", filter.ItemsTo.Value, SqlDbType.Decimal);
                }

                switch (filter.TransferFilterType)
                {
                    case InventoryTransferType.Outgoing:
                        if (filter.Status != null)
                        {
                            switch (filter.Status.Value)
                            {
                                case TransferOrderStatusEnum.New:
                                    conditions.Add(new Condition {Operator = "AND", ConditionValue = "it.SENT = 0 "});
                                    break;
                                case TransferOrderStatusEnum.Sent:
                                    conditions.Add(new Condition
                                    {
                                        Operator = "AND",
                                        ConditionValue = "it.SENT = 1 AND it.FETCHEDBYRECEIVINGSTORE = 0 "
                                    });
                                    break;
                                case TransferOrderStatusEnum.Received:
                                    conditions.Add(new Condition
                                    {
                                        Operator = "AND",
                                        ConditionValue = "it.FETCHEDBYRECEIVINGSTORE = 1 "
                                    });
                                    break;
                            }
                        }

                        conditions.Add(new Condition
                        {
                            Operator = "AND",
                            ConditionValue = "it.RECEIVED = 0  AND it.REJECTED = 0"
                        });

                        break;
                    case InventoryTransferType.Incoming:
                        if (filter.Status != null)
                        {
                            switch (filter.Status.Value)
                            {
                                case TransferOrderStatusEnum.Sent:
                                    conditions.Add(new Condition
                                    {
                                        Operator = "AND",
                                        ConditionValue = "it.SENT = 1 AND it.FETCHEDBYRECEIVINGSTORE = 0 "
                                    });
                                    break;
                                case TransferOrderStatusEnum.Received:
                                    conditions.Add(new Condition
                                    {
                                        Operator = "AND",
                                        ConditionValue = "it.FETCHEDBYRECEIVINGSTORE = 1 "
                                    });
                                    break;
                            }
                        }

                        conditions.Add(new Condition
                        {
                            Operator = "AND",
                            ConditionValue = "it.RECEIVED = 0 AND it.SENT = 1 AND it.REJECTED = 0 "
                        });

                        if (filter.ExpectedFrom != null)
                        {
                            conditions.Add(new Condition
                            {
                                Operator = "AND",
                                ConditionValue = "it.EXPECTEDDELIVERY >= @EXPECTEDFROM "
                            });
                            MakeParam(cmd, "EXPECTEDFROM", filter.ExpectedFrom.Value.Date, SqlDbType.DateTime);
                        }

                        if (filter.ExpectedTo != null)
                        {
                            conditions.Add(new Condition
                            {
                                Operator = "AND",
                                ConditionValue = "it.EXPECTEDDELIVERY <= @EXPECTEDTO "
                            });
                            MakeParam(cmd, "EXPECTEDTO", filter.ExpectedTo.Value.Date.AddDays(1).AddSeconds(-1),
                                SqlDbType.DateTime);
                        }
                        break;
                    case InventoryTransferType.SendingAndReceiving:

                        break;
                    case InventoryTransferType.Finished:
                        if (filter.Status != null)
                        {
                            switch (filter.Status.Value)
                            {
                                case TransferOrderStatusEnum.Closed:
                                    conditions.Add(
                                        new Condition {Operator = "AND", ConditionValue = "it.RECEIVED = 1 "});
                                    break;
                                case TransferOrderStatusEnum.Rejected:
                                    conditions.Add(
                                        new Condition {Operator = "AND", ConditionValue = "it.REJECTED = 1 "});
                                    break;
                            }
                        }
                        else
                        {
                            conditions.Add(new Condition
                            {
                                Operator = "AND",
                                ConditionValue = "(it.RECEIVED = 1 OR it.REJECTED = 1) "
                            });
                        }
                        break;
                }

                string ascDesc = filter.SortDescending ? " ASC" : " DESC";
                string sorting = ResolveSortPaged(filter.SortBy, filter.SortDescending, "S");
                if (filter.SortBy == InventoryTransferOrderSortEnum.SendingStore)
                {
                    sorting = "ORDER BY SENDINGSTORENAME" + ascDesc;
                }
                else if (filter.SortBy == InventoryTransferOrderSortEnum.ReceivingStore)
                {
                    sorting = "ORDER BY RECEIVINGSTORENAME" + ascDesc;
                }

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("INVENTORYTRANSFERORDER", "it", "S"),
                    QueryPartGenerator.ExternalColumnGenerator(columns, "S"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    QueryPartGenerator.ConditionGenerator(externalConditions),
                    sorting);

                MakeParam(cmd, "ROWFROM", filter.RowFrom, SqlDbType.Int);
                MakeParam(cmd, "ROWTO", filter.RowTo, SqlDbType.Int);
                totalRecordsMatching = 0;
                result = Execute<InventoryTransferOrder, int>(entry, cmd, CommandType.Text, ref totalRecordsMatching, PopulateTransferWithItemsAndRowCount);
            }

            return result;
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "INVENTORYTRANSFERORDER", "ID", id);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "INVENTORYTRANSFERORDER", "ID", id, BusinessObjects.Permission.EditInventoryTransfersOrders);
            DeleteRecord(entry, "INVENTORYTRANSFERORDERLINE", "INVENTORYTRANSFERORDERID", id, BusinessObjects.Permission.EditInventoryTransfersOrders);
        }

        public virtual void Save(IConnectionManager entry, InventoryTransferOrder inventoryTransferOrder)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.EditInventoryTransfersOrders);
            var statement = new SqlServerStatement("INVENTORYTRANSFERORDER", false); // Dont' create replication actions because replication is handled through Site Service

            if (inventoryTransferOrder.ID == "" || inventoryTransferOrder.ID.IsEmpty)
            {
                inventoryTransferOrder.ID = DataProviderFactory.Instance.GenerateNumber<IInventoryTransferOrderData, InventoryTransferOrder>(entry);
            }

            if (!Exists(entry, inventoryTransferOrder.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("ID", (string)inventoryTransferOrder.ID);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ID", (string)inventoryTransferOrder.ID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            bool tranferRequestIdEmpty = (inventoryTransferOrder.InventoryTransferRequestId == RecordIdentifier.Empty);
            if (!tranferRequestIdEmpty)
            {
                statement.AddField("INVENTORYTRANSFERREQUESTID", (string)inventoryTransferOrder.InventoryTransferRequestId);
            }
            else
            {
                statement.AddField("INVENTORYTRANSFERREQUESTID", DBNull.Value, SqlDbType.NVarChar);
            }

            statement.AddField("DESCRIPTION", inventoryTransferOrder.Text);
            statement.AddField("SENDINGSTOREID", (string)inventoryTransferOrder.SendingStoreId);
            statement.AddField("RECEIVINGSTOREID", (string)inventoryTransferOrder.ReceivingStoreId);
            statement.AddField("CREATIONDATE", inventoryTransferOrder.CreationDate, SqlDbType.DateTime);
            statement.AddField("RECEIVINGDATE", inventoryTransferOrder.ReceivingDate, SqlDbType.DateTime);
            statement.AddField("SENTDATE", inventoryTransferOrder.SentDate, SqlDbType.DateTime);
            statement.AddField("RECEIVED", inventoryTransferOrder.Received, SqlDbType.Bit);
            statement.AddField("SENT", inventoryTransferOrder.Sent, SqlDbType.Bit);
            statement.AddField("REJECTED", inventoryTransferOrder.Rejected, SqlDbType.Bit);
            statement.AddField("FETCHEDBYRECEIVINGSTORE", inventoryTransferOrder.FetchedByReceivingStore, SqlDbType.Bit);
            statement.AddField("CREATEDBY", (string)inventoryTransferOrder.CreatedBy);
            statement.AddField("EXPECTEDDELIVERY", inventoryTransferOrder.ExpectedDelivery, SqlDbType.DateTime);
            statement.AddField("CREATEDFROMOMNI", inventoryTransferOrder.CreatedFromOmni, SqlDbType.Bit);
            statement.AddField("TEMPLATEID", (string)inventoryTransferOrder.TemplateID);
            statement.AddField("PROCESSINGSTATUS", (int)inventoryTransferOrder.ProcessingStatus, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual List<string> CheckUnitConversionsForTransferOrder(IConnectionManager entry, RecordIdentifier transferOrderID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = $@"SELECT RI.ITEMID, RI.INVENTORYUNITID, TOL.UNITID FROM INVENTORYTRANSFERORDERLINE TOL
                    INNER JOIN RETAILITEM RI ON RI.ITEMID = TOL.ITEMID
                    WHERE TOL.INVENTORYTRANSFERORDERID = '{transferOrderID.StringValue}' AND TOL.DATAAREAID = '{entry.Connection.DataAreaId}' AND RI.INVENTORYUNITID != TOL.UNITID
                    AND NOT EXISTS (SELECT 1 FROM UNITCONVERT WHERE (ITEMID = RI.ITEMID OR ITEMID = '') AND ((FROMUNIT = RI.INVENTORYUNITID AND TOUNIT = TOL.UNITID) OR (TOUNIT = RI.INVENTORYUNITID AND FROMUNIT = TOL.UNITID)))";

                List<string> results = new List<string>();

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    while (dr.Read())
                    {
                        results.Add("No unit conversion rule exists between " + (string)dr["INVENTORYUNITID"] + " and " + (string)dr["UNITID"] + " on item " + (string)dr["ITEMID"]);
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }
                return results;
            }
        }

        public virtual bool UpdateInventoryForTransferOrder(IConnectionManager entry, RecordIdentifier transferOrderID, TransferOrderUpdateInventoryAction action, bool adjustmentNeeded, RecordIdentifier adjustmentReasonCode, RecordIdentifier adjustmentJournalID)
        {
            //Create a separate connection to run this because it can take a long time
            IConnectionManagerTemporary updateLinesEntry = entry as IConnectionManagerTemporary;
            try
            {
                if(updateLinesEntry == null)
                {
                    updateLinesEntry = entry.CreateTemporaryConnection();
                }

                using (SqlCommand cmd = new SqlCommand("spINVENTORY_UpdateInventoryForTransferOrder"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 3600; //1 hour timeout, this operation can take a long time

                    MakeParam(cmd, "TRANSFERORDERID", transferOrderID.StringValue);
                    MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                    MakeParam(cmd, "ACTIONTYPE", (int)action, SqlDbType.Int);
                    MakeParam(cmd, "ADJUSTMENTNEEDED", adjustmentNeeded, SqlDbType.Bit);
                    MakeParam(cmd, "ADJUSTMENTREASONCODE", adjustmentReasonCode.StringValue);
                    MakeParam(cmd, "ADJUSTMENTJOURNALID", adjustmentJournalID.StringValue);
                    SqlParameter updateResult = MakeParam(cmd, "RESULT", "", SqlDbType.Bit, ParameterDirection.Output, 4);

                    updateLinesEntry.Connection.ExecuteNonQuery(cmd, false);

                    return (bool)updateResult.Value;
                }
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
            finally
            {
                updateLinesEntry?.Close();
            }
        }

        public virtual void SetTransferOrderProcessingStatus(IConnectionManager entry, RecordIdentifier transferOrderID, InventoryProcessingStatus status)
        {
            var statement = new SqlServerStatement("INVENTORYTRANSFERORDER");

            statement.StatementType = StatementType.Update;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("ID", (string)transferOrderID);

            statement.AddField("PROCESSINGSTATUS", (int)status, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual InventoryTransferOrder GetOmniTransferOrderByTemplate(IConnectionManager entry, RecordIdentifier templateID, RecordIdentifier storeID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>
                {
                    new Condition { ConditionValue = "it.DATAAREAID = @DATAAREAID", Operator = "AND" },
                    new Condition { ConditionValue = "it.CREATEDFROMOMNI = 1", Operator = "AND" },
                    new Condition { ConditionValue = "it.TEMPLATEID = @TEMPLATEID", Operator = "AND" },
                    new Condition { ConditionValue = "it.SENDINGSTOREID = @STOREID", Operator = "AND" },
                    new Condition { ConditionValue = "it.SENT = 0", Operator = "AND" },
                };

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("INVENTORYTRANSFERORDER", "it"),
                    QueryPartGenerator.InternalColumnGenerator(transferColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    " ORDER BY it.CREATIONDATE DESC"
                    );

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "TEMPLATEID", (string)templateID);
                MakeParam(cmd, "STOREID", (string)storeID);

                var result = Execute<InventoryTransferOrder>(entry, cmd, CommandType.Text, PopulateTransfer);

                return (result.Count > 0) ? result[0] : null;
            }
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "INVENTTRANSFER"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "INVENTORYTRANSFERORDER", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion

    }
}