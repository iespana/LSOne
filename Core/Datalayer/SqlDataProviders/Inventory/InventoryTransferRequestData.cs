using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.SqlDataProviders.Inventory
{
    public class InventoryTransferRequestData : SqlServerDataProviderBase, IInventoryTransferRequestData
    {
        private static List<TableColumn> transferColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "ID " , TableAlias = "itr"},
            new TableColumn {ColumnName = "ISNULL(itr.DESCRIPTION, '') " , ColumnAlias = "DESCRIPTION"},
            new TableColumn {ColumnName = "ISNULL(itr.INVENTORYTRANSFERORDERID, '') " ,ColumnAlias = "INVENTORYTRANSFERORDERID"},
            new TableColumn {ColumnName = "SENDINGSTOREID " , TableAlias = "itr"},
            new TableColumn {ColumnName = "ISNULL(st1.NAME,'') " ,ColumnAlias = "SENDINGSTORENAME"},
            new TableColumn {ColumnName = "RECEIVINGSTOREID " , TableAlias = "itr"},
            new TableColumn {ColumnName = "ISNULL(st2.NAME,'') " , ColumnAlias = "RECEIVINGSTORENAME"},
            new TableColumn {ColumnName = "CREATIONDATE ", TableAlias = "itr"},
            new TableColumn {ColumnName = "INVENTORYTRANSFERCREATED  " , TableAlias = "itr"},
            new TableColumn {ColumnName = "ISNULL(itr.SENTDATE, '01.01.1900') " , ColumnAlias = "SENTDATE"},
            new TableColumn {ColumnName = "SENT ", TableAlias = "itr"},
            new TableColumn {ColumnName = "REJECTED ", TableAlias = "itr"},
            new TableColumn {ColumnName = "FETCHEDBYRECEIVINGSTORE ", TableAlias = "itr"},
            new TableColumn {ColumnName = "CREATEDBY ", TableAlias = "itr"},
            new TableColumn {ColumnName = "ISNULL(itr.EXPECTEDDELIVERY, '01.01.1900') ", ColumnAlias = "EXPECTEDDELIVERY"},
            new TableColumn {ColumnName = "CREATEDFROMOMNI ", ColumnAlias = "CREATEDFROMOMNI", TableAlias = "itr"},
            new TableColumn {ColumnName = "TEMPLATEID ", ColumnAlias = "TEMPLATEID", IsNull = true, NullValue = "''", TableAlias = "itr"}
        };

        private static List<Join> listJoins = new List<Join>
        {
            new Join
            {
                Condition = "st1.STOREID = itr.SENDINGSTOREID and st1.DATAAREAID = itr.DATAAREAID",
                JoinType = "LEFT OUTER",
                Table = "RBOSTORETABLE",
                TableAlias = "st1"
            },
            new Join
            {
                Condition = "st2.STOREID = itr.RECEIVINGSTOREID and st2.DATAAREAID = itr.DATAAREAID",
                JoinType = "LEFT OUTER",
                Table = "RBOSTORETABLE",
                TableAlias = "st2"
            }
        };

        private static List<TableColumn> quantityColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "ISNULL(lines.ITEMLINES, 0) ", ColumnAlias = "ITEMLINES"},
        };

        private static List<Join> quantityJoins = new List<Join>
        {
            new Join
            {
                Condition = "itr.ID = lines.INVENTORYTRANSFERREQUESTID",
                JoinType = "LEFT",
                Table = "(SELECT INVENTORYTRANSFERREQUESTID, COUNT(*) AS ITEMLINES FROM INVENTORYTRANSFERREQUESTLINE GROUP BY INVENTORYTRANSFERREQUESTID)",
                TableAlias = "lines"
            },
        };

        private static void PopulateTransferRequest(IDataReader dr, InventoryTransferRequest inventoryTransferRequest)
        {
            inventoryTransferRequest.ID = (string)dr["ID"];
            inventoryTransferRequest.Text = (string)dr["DESCRIPTION"];
            inventoryTransferRequest.SendingStoreId = (string)dr["SENDINGSTOREID"];
            inventoryTransferRequest.SendingStoreName = (string)dr["SENDINGSTORENAME"];
            inventoryTransferRequest.ReceivingStoreId = (string)dr["RECEIVINGSTOREID"];
            inventoryTransferRequest.ReceivingStoreName = (string)dr["RECEIVINGSTORENAME"];
            inventoryTransferRequest.CreationDate = (DateTime)dr["CREATIONDATE"];
            inventoryTransferRequest.SentDate = (DateTime)dr["SENTDATE"];
            inventoryTransferRequest.Sent = (bool)dr["SENT"];
            inventoryTransferRequest.Rejected = (bool)dr["REJECTED"];
            inventoryTransferRequest.FetchedByReceivingStore = (bool)dr["FETCHEDBYRECEIVINGSTORE"];
            inventoryTransferRequest.InventoryTransferOrderCreated = (bool)dr["INVENTORYTRANSFERCREATED"];
            inventoryTransferRequest.CreatedBy = (string)dr["CREATEDBY"];
            inventoryTransferRequest.InventoryTransferOrderId = (string)dr["INVENTORYTRANSFERORDERID"];
            inventoryTransferRequest.ExpectedDelivery = (DateTime)dr["EXPECTEDDELIVERY"];
            inventoryTransferRequest.CreatedFromOmni = (bool)dr["CREATEDFROMOMNI"];
            inventoryTransferRequest.TemplateID = (string)dr["TEMPLATEID"];
        }

        protected static void PopulateRowCount(IConnectionManager entry, IDataReader dr, ref int rowCount)
        {
            if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 ||
                entry.Connection.DatabaseVersion == ServerVersion.Unknown)
            {
                rowCount = (int)dr["Row_Count"];
            }
        }

        private static void PopulateTransferWithItemsAndRowCount(IConnectionManager entry, IDataReader dr, InventoryTransferRequest inventoryTransferRequest, ref int rowCount)
        {
            PopulateTransferRequest(dr, inventoryTransferRequest);
            PopulateRowCount(entry, dr, ref rowCount);
            inventoryTransferRequest.NoOfItems = AsDecimal(dr["ITEMLINES"]);
        }

        private static void PopulateTransferRequestWithItemsCount(IDataReader dr, InventoryTransferRequest inventoryTransferRequest)
        {
            PopulateTransferRequest(dr, inventoryTransferRequest);

            inventoryTransferRequest.NoOfItems = AsDecimal(dr["ITEMLINES"]);
        }

        private static string ResolveSort(InventoryTransferOrderSortEnum sort, bool sortBackwards)
        {
            string sortString = " ORDER BY ";
            switch (sort)
            {
                case InventoryTransferOrderSortEnum.Id:
                    sortString += "itr.Id";
                    break;
                case InventoryTransferOrderSortEnum.CreatedDate:
                    sortString += "itr.CREATIONDATE";
                    break;
                case InventoryTransferOrderSortEnum.SentDate:
                    sortString += "ISNULL(itr.SENTDATE, '01.01.1900')";
                    break;
                case InventoryTransferOrderSortEnum.SendingStore:
                    sortString += "SENDINGSTORENAME";
                    break;
                case InventoryTransferOrderSortEnum.ReceivingStore:
                    sortString += "RECEIVINGSTORENAME";
                    break;
                case InventoryTransferOrderSortEnum.Sent:
                    sortString += " itr.SENT";
                    break;
                case InventoryTransferOrderSortEnum.Fetched:
                    sortString += " itr.FETCHEDBYRECEIVINGSTORE";
                    break;
                case InventoryTransferOrderSortEnum.InventoryTransferCreated:
                    sortString += " itr.INVENTORYTRANSFERCREATED";
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
                case InventoryTransferOrderSortEnum.ReceivedDate:
                    sortString += "itr.Id"; //Requests do not have received date
                    break;

            }

            sortString += (sortBackwards) ? " DESC" : " ASC";

            return sortString;
        }

        public virtual InventoryTransferRequest Get(IConnectionManager entry, RecordIdentifier transferRequestId)
        {
            List<InventoryTransferRequest> result = new List<InventoryTransferRequest>();

            if (string.IsNullOrEmpty((string)transferRequestId))
            {
                return null;
            }

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.DATAAREAID = @DATAAREAID " });
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                if (!string.IsNullOrEmpty((string)transferRequestId))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.ID = @TRANSFERID " });
                    MakeParam(cmd, "TRANSFERID", (string)transferRequestId);
                }

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("INVENTORYTRANSFERREQUEST", "itr", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(transferColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );

                result = Execute<InventoryTransferRequest>(entry, cmd, CommandType.Text, PopulateTransferRequest);
            }

            return (result.Count > 0) ? result[0] : null;
        }

        public List<InventoryTransferRequest> GetFromList(
            IConnectionManager entry,
            List<RecordIdentifier> requestIds,
            InventoryTransferOrderSortEnum orderSortBy,
            bool sortBackwards)
        {
            if (requestIds == null || requestIds.Count == 0)
            {
                return new List<InventoryTransferRequest>();
            }

            List<InventoryTransferRequest> result;

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.DATAAREAID = @DATAAREAID " });
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                string transferParams = "";
                for (int i = 0; i < requestIds.Count; i++)
                {
                    if (i != 0)
                    {
                        transferParams += " OR ";
                    }

                    transferParams += "itr.ID = @TRANSFERID" + i;
                    MakeParam(cmd, "TRANSFERID" + i, (string)requestIds[i]);
                }

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "(" + transferParams + ") " });

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("INVENTORYTRANSFERREQUEST", "itr", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(transferColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    ResolveSort(orderSortBy, sortBackwards)
                    );

                result = Execute<InventoryTransferRequest>(entry, cmd, CommandType.Text, PopulateTransferRequest);
            }

            return result;
        }

        public List<InventoryTransferRequest> GetListForStore(
            IConnectionManager entry,
            List<RecordIdentifier> storeIds, 
            InventoryTransferType transferRequestType, 
            InventoryTransferOrderSortEnum sortBy, 
            bool sortBackwards)
        {
            List<InventoryTransferRequest> result;

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.DATAAREAID = @DATAAREAID " });
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
                        switch (transferRequestType)
                        {
                            case InventoryTransferType.Outgoing:
                                storeParams += "(itr.SENDINGSTOREID = @STOREID" + i + ")";
                                break;
                            case InventoryTransferType.Incoming:
                                storeParams += "(itr.RECEIVINGSTOREID = @STOREID" + i + " and itr.SENT = 1)";
                                break;
                        }
                        MakeParam(cmd, "STOREID" + i, (string)storeIds[i]);
                    }

                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "(" + storeParams + ") " });
                }

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("INVENTORYTRANSFERREQUEST", "itr", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(transferColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    ResolveSort(sortBy, sortBackwards)
                    );

                result = Execute<InventoryTransferRequest>(entry, cmd, CommandType.Text, PopulateTransferRequest);
            }

            return result;
        }

        public virtual List<InventoryTransferRequest> Search(IConnectionManager entry, InventoryTransferFilter filter)
        {
            List<InventoryTransferRequest> result;

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.DATAAREAID = @DATAAREAID " });
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                if (!string.IsNullOrEmpty(filter.DescriptionOrID))
                {
                    string searchString = PreProcessSearchText(filter.DescriptionOrID, true, filter.DescriptionOrIDBeginsWith);
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "(itr.DESCRIPTION LIKE @DESCRIPTION OR itr.ID LIKE @DESCRIPTION)" });
                    MakeParam(cmd, "DESCRIPTION", searchString);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.SendingStoreID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.RECEIVINGSTOREID = @RECEIVINGSTOREID " });
                    MakeParam(cmd, "RECEIVINGSTOREID", (string)filter.SendingStoreID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.ReceivingStoreID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.SENDINGSTOREID = @SENDINGSTOREID " });
                    MakeParam(cmd, "SENDINGSTOREID", (string)filter.ReceivingStoreID);
                }

                if (filter.Status != null)
                {
                    switch (filter.Status.Value)
                    {
                        case TransferOrderStatusEnum.New:
                            conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.SENT = 1 AND itr.REJECTED = 0 " });
                            break;
                        case TransferOrderStatusEnum.Sent:
                            conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.SENT = 1 AND itr.FETCHEDBYRECEIVINGSTORE = 0 AND itr.REJECTED = 0 " });
                            break;
                        case TransferOrderStatusEnum.Received:
                            conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.FETCHEDBYRECEIVINGSTORE = 1 AND itr.REJECTED = 0 " });
                            break;
                        case TransferOrderStatusEnum.Rejected:
                            conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.REJECTED = 1 " });
                            break;
                        case TransferOrderStatusEnum.Closed:
                            conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.INVENTORYTRANSFERCREATED = 1 " });
                            break;
                        default:
                            break;
                    }
                }

                if (filter.FromDate != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "(itr.CREATIONDATE >= @FROMDATE OR itr.SENTDATE >= @FROMDATE OR itr.EXPECTEDDELIVERY >= @FROMDATE) " });
                    MakeParam(cmd, "FROMDATE", filter.FromDate.Value.Date, SqlDbType.DateTime);
                }

                if (filter.ToDate != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "(itr.CREATIONDATE <= @TODATE OR itr.SENTDATE <= @TODATE OR itr.EXPECTEDDELIVERY <= @TODATE) " });
                    MakeParam(cmd, "TODATE", filter.ToDate.Value.Date.AddDays(1).AddSeconds(-1), SqlDbType.DateTime);
                }

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("INVENTORYTRANSFERREQUEST", "itr", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(transferColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    ResolveSort(filter.SortBy, filter.SortDescending)
                    );

                result = Execute<InventoryTransferRequest>(entry, cmd, CommandType.Text, PopulateTransferRequest);
            }

            return result;
        }

        public List<InventoryTransferRequest> Search(IConnectionManager entry, InventoryTransferFilterExtended filter)
        {
            List<InventoryTransferRequest> result;

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>();
                columns.AddRange(transferColumns);
                List<Join> joins = new List<Join>();
                joins.AddRange(listJoins);
                
                joins.AddRange(quantityJoins);
                columns.AddRange(quantityColumns);

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.DATAAREAID = @DATAAREAID " });
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                if (!string.IsNullOrEmpty(filter.DescriptionOrID))
                {
                    string searchString = PreProcessSearchText(filter.DescriptionOrID, true, filter.DescriptionOrIDBeginsWith);
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "(itr.DESCRIPTION LIKE @DESCRIPTION OR itr.ID LIKE @DESCRIPTION)" });
                    MakeParam(cmd, "DESCRIPTION", searchString);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.StoreID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "(itr.SENDINGSTOREID = @STOREID OR itr.RECEIVINGSTOREID = @STOREID) " });
                    MakeParam(cmd, "STOREID", (string)filter.StoreID);
                }
                
                if (!RecordIdentifier.IsEmptyOrNull(filter.SendingStoreID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.RECEIVINGSTOREID = @RECEIVINGSTOREID " });
                    MakeParam(cmd, "RECEIVINGSTOREID", (string)filter.SendingStoreID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.ReceivingStoreID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.SENDINGSTOREID = @SENDINGSTOREID " });
                    MakeParam(cmd, "SENDINGSTOREID", (string)filter.ReceivingStoreID);
                }

                if (filter.SentFrom != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.SENTDATE >= @SENTFROM " });
                    MakeParam(cmd, "SENTFROM", filter.SentFrom.Value.Date, SqlDbType.DateTime);
                }

                if (filter.SentTo != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.SENTDATE <= @SENTTO " });
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

                if (filter.ExpectedFrom != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.EXPECTEDDELIVERY >= @EXPECTEDFROM " });
                    MakeParam(cmd, "EXPECTEDFROM", filter.ExpectedFrom.Value.Date, SqlDbType.DateTime);
                }

                if (filter.ExpectedTo != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.EXPECTEDDELIVERY <= @EXPECTEDTO " });
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
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.SENT = 0 " });
                                    break;
                                case TransferOrderStatusEnum.Sent:
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.SENT = 1 AND itr.FETCHEDBYRECEIVINGSTORE = 0 " });
                                    break;
                                case TransferOrderStatusEnum.Received:
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.FETCHEDBYRECEIVINGSTORE = 1 " });
                                    break;
                                default:
                                    break;
                            }
                        }

                        conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.INVENTORYTRANSFERCREATED = 0 AND itr.REJECTED = 0 " });
                        break;
                    case InventoryTransferType.Incoming:
                        if (filter.Status != null)
                        {
                            switch (filter.Status.Value)
                            {
                                case TransferOrderStatusEnum.Sent:
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.SENT = 1 AND itr.FETCHEDBYRECEIVINGSTORE = 0 " });
                                    break;
                                case TransferOrderStatusEnum.Received:
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.FETCHEDBYRECEIVINGSTORE = 1 " });
                                    break;
                                default:
                                    break;
                            }
                        }

                        conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.INVENTORYTRANSFERCREATED = 0 AND itr.SENT = 1 AND itr.REJECTED = 0 " });                        

                        break;
                    case InventoryTransferType.SendingAndReceiving:
                        break;
                    case InventoryTransferType.Finished:
                        if (filter.Status != null)
                        {
                            switch (filter.Status.Value)
                            {
                                case TransferOrderStatusEnum.Closed:
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.INVENTORYTRANSFERCREATED = 1 " });
                                    break;
                                case TransferOrderStatusEnum.Rejected:
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.REJECTED = 1 " });
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            conditions.Add(new Condition { Operator = "AND", ConditionValue = "(itr.INVENTORYTRANSFERCREATED = 1 OR itr.REJECTED = 1) " });
                        }
                        break;
                    default:
                        break;
                }

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("INVENTORYTRANSFERREQUEST", "itr", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    ResolveSort(filter.SortBy, filter.SortDescending)
                    );

                result = Execute<InventoryTransferRequest>(entry, cmd, CommandType.Text, PopulateTransferRequestWithItemsCount);
            }

            return result;
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
                    sortColumn = tableAlias + "Id"; //Requests do not have a received date - filter by id
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

        public List<InventoryTransferRequest> AdvancedSearch(IConnectionManager entry, out int totalRecordsMatching, InventoryTransferFilterExtended filter)
        {
            List<InventoryTransferRequest> result;

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
                    ColumnName = string.Format("ROW_NUMBER() OVER({0})", ResolveSortPaged(filter.SortBy, filter.SortDescending, "itr")),
                    ColumnAlias = "ROW"
                });
                columns.Add(new TableColumn
                {
                    ColumnName =
                        string.Format("COUNT(1) OVER ( {0} RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )",
                            ResolveSortPaged(filter.SortBy, filter.SortDescending, "itr")),
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

                joins.AddRange(quantityJoins);
                columns.AddRange(quantityColumns);

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.DATAAREAID = @DATAAREAID " });
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                if (!string.IsNullOrEmpty(filter.DescriptionOrID))
                {
                    string searchString = PreProcessSearchText(filter.DescriptionOrID, true, filter.DescriptionOrIDBeginsWith);
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "(itr.DESCRIPTION LIKE @DESCRIPTION OR itr.ID LIKE @DESCRIPTION)" });
                    MakeParam(cmd, "DESCRIPTION", searchString);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.StoreID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "(itr.SENDINGSTOREID = @STOREID OR itr.RECEIVINGSTOREID = @STOREID) " });
                    MakeParam(cmd, "STOREID", (string)filter.StoreID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.SendingStoreID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.RECEIVINGSTOREID = @RECEIVINGSTOREID " });
                    MakeParam(cmd, "RECEIVINGSTOREID", (string)filter.SendingStoreID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.ReceivingStoreID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.SENDINGSTOREID = @SENDINGSTOREID " });
                    MakeParam(cmd, "SENDINGSTOREID", (string)filter.ReceivingStoreID);
                }

                if (filter.SentFrom != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.SENTDATE >= @SENTFROM " });
                    MakeParam(cmd, "SENTFROM", filter.SentFrom.Value.Date, SqlDbType.DateTime);
                }

                if (filter.SentTo != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.SENTDATE <= @SENTTO " });
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

                switch (filter.TransferFilterType)
                {
                    case InventoryTransferType.Outgoing:
                        if (filter.Status != null)
                        {
                            switch (filter.Status.Value)
                            {
                                case TransferOrderStatusEnum.New:
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.SENT = 0 " });
                                    break;
                                case TransferOrderStatusEnum.Sent:
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.SENT = 1 AND itr.FETCHEDBYRECEIVINGSTORE = 0 " });
                                    break;
                                case TransferOrderStatusEnum.Received:
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.FETCHEDBYRECEIVINGSTORE = 1 " });
                                    break;
                                default:
                                    break;
                            }
                        }

                        conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.INVENTORYTRANSFERCREATED = 0 AND itr.REJECTED = 0 " });
                        break;
                    case InventoryTransferType.Incoming:
                        if (filter.Status != null)
                        {
                            switch (filter.Status.Value)
                            {
                                case TransferOrderStatusEnum.Sent:
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.SENT = 1 AND itr.FETCHEDBYRECEIVINGSTORE = 0 " });
                                    break;
                                case TransferOrderStatusEnum.Received:
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.FETCHEDBYRECEIVINGSTORE = 1 " });
                                    break;
                                default:
                                    break;
                            }
                        }

                        conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.INVENTORYTRANSFERCREATED = 0 AND itr.SENT = 1 AND itr.REJECTED = 0 " });

                        if (filter.ExpectedFrom != null)
                        {
                            conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.EXPECTEDDELIVERY >= @EXPECTEDFROM " });
                            MakeParam(cmd, "EXPECTEDFROM", filter.ExpectedFrom.Value.Date, SqlDbType.DateTime);
                        }

                        if (filter.ExpectedTo != null)
                        {
                            conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.EXPECTEDDELIVERY <= @EXPECTEDTO " });
                            MakeParam(cmd, "EXPECTEDTO", filter.ExpectedTo.Value.Date.AddDays(1).AddSeconds(-1), SqlDbType.DateTime);
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
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.INVENTORYTRANSFERCREATED = 1 " });
                                    break;
                                case TransferOrderStatusEnum.Rejected:
                                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "itr.REJECTED = 1 " });
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            conditions.Add(new Condition { Operator = "AND", ConditionValue = "(itr.INVENTORYTRANSFERCREATED = 1 OR itr.REJECTED = 1) " });
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

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("INVENTORYTRANSFERREQUEST", "itr", "S"),
                    QueryPartGenerator.ExternalColumnGenerator(columns, "S"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    QueryPartGenerator.ConditionGenerator(externalConditions),
                    sorting);

                MakeParam(cmd, "ROWFROM", filter.RowFrom, SqlDbType.Int);
                MakeParam(cmd, "ROWTO", filter.RowTo, SqlDbType.Int);
                totalRecordsMatching = 0;
                result = Execute<InventoryTransferRequest, int>(entry, cmd, CommandType.Text, ref totalRecordsMatching, PopulateTransferWithItemsAndRowCount);
            }

            return result;
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "INVENTORYTRANSFERREQUEST", "ID", id);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "INVENTORYTRANSFERREQUEST", "ID", id, Permission.EditInventoryTransferRequests);
            DeleteRecord(entry, "INVENTORYTRANSFERREQUESTLINE", "INVENTORYTRANSFERREQUESTID", id, Permission.EditInventoryTransferRequests);
        }

        public virtual void Save(IConnectionManager entry, InventoryTransferRequest inventoryTransferRequest)
        {
            ValidateSecurity(entry, Permission.EditInventoryTransferRequests);
            var statement = new SqlServerStatement("INVENTORYTRANSFERREQUEST", false); // Dont' create replication actions because replication is handled through Site Service

            if (inventoryTransferRequest.ID == "" || inventoryTransferRequest.ID.IsEmpty)
            {
                inventoryTransferRequest.ID = DataProviderFactory.Instance.GenerateNumber<IInventoryTransferOrderData, InventoryTransferOrder>(entry);
            }
            if (!Exists(entry, inventoryTransferRequest.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("ID", (string)inventoryTransferRequest.ID);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ID", (string)inventoryTransferRequest.ID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("DESCRIPTION", inventoryTransferRequest.Text);
            statement.AddField("SENDINGSTOREID", (string)inventoryTransferRequest.SendingStoreId);
            statement.AddField("RECEIVINGSTOREID", (string)inventoryTransferRequest.ReceivingStoreId);
            statement.AddField("CREATIONDATE", inventoryTransferRequest.CreationDate, SqlDbType.DateTime);
            statement.AddField("SENTDATE", inventoryTransferRequest.SentDate, SqlDbType.DateTime);
            statement.AddField("SENT", inventoryTransferRequest.Sent, SqlDbType.Bit);
            statement.AddField("REJECTED", inventoryTransferRequest.Rejected, SqlDbType.Bit);
            statement.AddField("FETCHEDBYRECEIVINGSTORE", inventoryTransferRequest.FetchedByReceivingStore, SqlDbType.Bit);
            statement.AddField("INVENTORYTRANSFERCREATED", inventoryTransferRequest.InventoryTransferOrderCreated, SqlDbType.Bit);
            statement.AddField("CREATEDBY", (string)inventoryTransferRequest.CreatedBy);
            statement.AddField("INVENTORYTRANSFERORDERID", (string)inventoryTransferRequest.InventoryTransferOrderId);
            statement.AddField("EXPECTEDDELIVERY", inventoryTransferRequest.ExpectedDelivery, SqlDbType.DateTime);
            statement.AddField("CREATEDFROMOMNI", inventoryTransferRequest.CreatedFromOmni, SqlDbType.Bit);
            statement.AddField("TEMPLATEID", (string)inventoryTransferRequest.TemplateID);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual List<string> CheckUnitConversionsForTransferRequest(IConnectionManager entry, RecordIdentifier transferRequestID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = $@"SELECT RI.ITEMID, RI.INVENTORYUNITID, TOR.UNITID FROM INVENTORYTRANSFERREQUESTLINE TOR
                    INNER JOIN RETAILITEM RI ON RI.ITEMID = TOR.ITEMID
                    WHERE TOR.INVENTORYTRANSFERREQUESTID = '{transferRequestID.StringValue}' AND TOR.DATAAREAID = '{entry.Connection.DataAreaId}' AND RI.INVENTORYUNITID != TOR.UNITID
                    AND NOT EXISTS (SELECT 1 FROM UNITCONVERT WHERE (ITEMID = RI.ITEMID OR ITEMID = '') AND ((FROMUNIT = RI.INVENTORYUNITID AND TOUNIT = TOR.UNITID) OR (TOUNIT = RI.INVENTORYUNITID AND FROMUNIT = TOR.UNITID)))";

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

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "INVENTTRANSFERREQ"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "INVENTORYTRANSFERREQUEST", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}