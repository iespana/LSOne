using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Inventory
{
    /// <summary>
    /// Data provider class for purchase orders
    /// </summary>
    public class PurchaseOrderData : SqlServerDataProviderBase, IPurchaseOrderData
    {
        private static List<TableColumn> purchaseOrdersColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "PURCHASEORDERID ", TableAlias = "p"},
            new TableColumn {ColumnName = "VENDORID ", TableAlias = "p"},
            new TableColumn {ColumnName = "PURCHASESTATUS ", TableAlias = "p"},
            new TableColumn {ColumnName = "DELIVERYDATE ", TableAlias = "p"},
            new TableColumn {ColumnName = "CREATEDDATE ", TableAlias = "p"},
            new TableColumn {ColumnName = "ORDERINGDATE ", TableAlias = "p"},
            new TableColumn {ColumnName = "STOREID ", TableAlias = "p"},
            new TableColumn {ColumnName = "ORDERER ", TableAlias = "p"},
            new TableColumn {ColumnName = "NAME ", ColumnAlias = "STORENAME", IsNull = true, NullValue = "''", TableAlias = "s"},
            new TableColumn {ColumnName = "NAME ", ColumnAlias = "VENDORNAME", IsNull = true, NullValue = "''", TableAlias = "v"},
            new TableColumn {ColumnName = "DESCRIPTION ", ColumnAlias = "DESCRIPTION", IsNull = true, NullValue = "''", TableAlias = "p"},
            new TableColumn {ColumnName = "CURRENCYCODE ", ColumnAlias = "CURRENCYCODE", IsNull = true, NullValue = "''", TableAlias = "p"},
            new TableColumn {ColumnName = "TXT ", ColumnAlias = "CURRENCYDESCRIPTION", IsNull = true, NullValue = "''", TableAlias = "c"},
            new TableColumn {ColumnName = "DEFAULTDISCOUNTPERCENTAGE ", ColumnAlias = "DEFAULTDISCOUNTPERCENTAGE", IsNull = true, NullValue = "0", TableAlias = "p"},
            new TableColumn {ColumnName = "DEFAULTDISCOUNTAMOUNT ", ColumnAlias = "DEFAULTDISCOUNTAMOUNT", IsNull = true, NullValue = "0", TableAlias = "p"},
            new TableColumn {ColumnName = "FirstName ", ColumnAlias = "OrdererFirstName", IsNull = true, NullValue = "''", TableAlias = "u"},
            new TableColumn {ColumnName = "MiddleName ", ColumnAlias = "OrdererMiddleName", IsNull = true, NullValue = "''", TableAlias = "u"},
            new TableColumn {ColumnName = "LastName ", ColumnAlias = "OrdererLastName", IsNull = true, NullValue = "''", TableAlias = "u"},
            new TableColumn {ColumnName = "NamePrefix ", ColumnAlias = "OrdererNamePrefix", IsNull = true, NullValue = "''", TableAlias = "u"},
            new TableColumn {ColumnName = "NameSuffix ", ColumnAlias = "OrdererNameSuffix", IsNull = true, NullValue = "''", TableAlias = "u"},
            new TableColumn {ColumnName = "ADDRESS ", ColumnAlias = "STOREADDRESS", IsNull = true, NullValue = "''", TableAlias = "s"},
            new TableColumn {ColumnName = "STREET ", ColumnAlias = "STORESTREET", IsNull = true, NullValue = "''", TableAlias = "s"},
            new TableColumn {ColumnName = "ZIPCODE ", ColumnAlias = "STOREZIPCODE", IsNull = true, NullValue = "''", TableAlias = "s"},
            new TableColumn {ColumnName = "CITY ", ColumnAlias = "STORECITY", IsNull = true, NullValue = "''", TableAlias = "s"},
            new TableColumn {ColumnName = "COUNTRY ", ColumnAlias = "STORECOUNTRY", IsNull = true, NullValue = "''", TableAlias = "s"},
            new TableColumn {ColumnName = "STATE ", ColumnAlias = "STORESTATE", IsNull = true, NullValue = "''", TableAlias = "s"},
            new TableColumn {ColumnName = "ADDRESS ", ColumnAlias = "VENDORADDRESS", IsNull = true, NullValue = "''", TableAlias = "v"},
            new TableColumn {ColumnName = "STREET ", ColumnAlias = "VENDORSTREET", IsNull = true, NullValue = "''", TableAlias = "v"},
            new TableColumn {ColumnName = "ZIPCODE ", ColumnAlias = "VENDORZIPCODE", IsNull = true, NullValue = "''", TableAlias = "v"},
            new TableColumn {ColumnName = "CITY ", ColumnAlias = "VENDORCITY", IsNull = true, NullValue = "''", TableAlias = "v"},
            new TableColumn {ColumnName = "COUNTRY ", ColumnAlias = "VENDORCOUNTRY", IsNull = true, NullValue = "''", TableAlias = "v"},
            new TableColumn {ColumnName = "STATE ", ColumnAlias = "VENDORSTATE", IsNull = true, NullValue = "''", TableAlias = "v"},
            new TableColumn {ColumnName = "CREATEDFROMOMNI ", ColumnAlias = "CREATEDFROMOMNI", TableAlias = "p"},
            new TableColumn {ColumnName = "TEMPLATEID ", ColumnAlias = "TEMPLATEID", IsNull = true, NullValue = "''", TableAlias = "p"},
            new TableColumn {ColumnName = "PROCESSINGSTATUS ", ColumnAlias = "PROCESSINGSTATUS", TableAlias = "p"}
        };

        private static List<Join> listJoins = new List<Join>
        {
            new Join
            {
                Condition = "c.CURRENCYCODE = p.CURRENCYCODE",
                JoinType = "LEFT OUTER",
                Table = "CURRENCY",
                TableAlias = "c"
            },
            new Join
            {
                Condition = "s.STOREID = p.STOREID",
                JoinType = "LEFT OUTER",
                Table = "RBOSTORETABLE",
                TableAlias = "s"
            },
            new Join
            {
                Condition = "v.ACCOUNTNUM = p.VENDORID",
                JoinType = "LEFT OUTER",
                Table = "VENDTABLE",
                TableAlias = "v"
            },
            new Join
            {
                Condition = "u.GUID = p.ORDERER",
                JoinType = "LEFT OUTER",
                Table = "USERS",
                TableAlias = "u"
            }
        };
        
        private static string ResolveSort(PurchaseOrderSorting sort)
        {
            switch (sort)
            {
                case PurchaseOrderSorting.PurchaseOrderID:
                    return "p.PURCHASEORDERID";
                case PurchaseOrderSorting.VendorDescription:
                    return "v.NAME";
                case PurchaseOrderSorting.Status:
                    return "p.PURCHASESTATUS";
                case PurchaseOrderSorting.DeliveryDate:
                    return "p.DELIVERYDATE";
                case PurchaseOrderSorting.StoreName:
                    return "STORENAME";
            }

            return "";
        }

        private static void PopulatePurchaseOrder(IConnectionManager entry, IDataReader dr, PurchaseOrder purchaseOrder, object includeReportFormatting)
        {
            purchaseOrder.PurchaseOrderID = (string)dr["PURCHASEORDERID"];
            purchaseOrder.VendorID = (string)dr["VENDORID"];
            purchaseOrder.VendorName = (string)dr["VENDORNAME"];
            purchaseOrder.Description = (dr["DESCRIPTION"] != DBNull.Value) ? (string)dr["DESCRIPTION"] : string.Empty;
            purchaseOrder.PurchaseStatus = (PurchaseStatusEnum)dr["PURCHASESTATUS"];
            purchaseOrder.DeliveryDate = (DateTime)dr["DELIVERYDATE"];
            purchaseOrder.CurrencyCode = (string)dr["CURRENCYCODE"];
            purchaseOrder.CurrencyDescription = (string)dr["CURRENCYDESCRIPTION"];
            purchaseOrder.CreatedDate = (DateTime)dr["CREATEDDATE"];
            purchaseOrder.Orderer = (dr["ORDERER"] != DBNull.Value) ? (Guid)dr["ORDERER"] : Guid.Empty;
            purchaseOrder.StoreID = (string)dr["STOREID"];
            purchaseOrder.StoreName = (string)dr["STORENAME"];
            purchaseOrder.OrderingDate = Date.FromAxaptaDate(dr["ORDERINGDATE"]);
            purchaseOrder.DefaultDiscountPercentage = (decimal)dr["DEFAULTDISCOUNTPERCENTAGE"];
            purchaseOrder.DefaultDiscountAmount = (decimal)dr["DEFAULTDISCOUNTAMOUNT"];
            purchaseOrder.CreatedFromOmni = (bool)dr["CREATEDFROMOMNI"];
            purchaseOrder.TemplateID = (string)dr["TEMPLATEID"];
            purchaseOrder.ProcessingStatus = (InventoryProcessingStatus)dr["PROCESSINGSTATUS"];

            var ordererName = new Name
                {
                    First = (string) dr["ORDERERFIRSTNAME"],
                    Middle = (string) dr["ORDERERMIDDLENAME"],
                    Last = (string) dr["ORDERERLASTNAME"],
                    Prefix = (string) dr["ORDERERNAMEPREFIX"],
                    Suffix = (string) dr["ORDERERNAMESUFFIX"]
                };
            purchaseOrder.OrdererName = ordererName;

            var deliveryAddress = new Address
                {
                    Address1 = (string) dr["STORESTREET"],
                    Address2 = (string) dr["STOREADDRESS"],
                    Zip = (string) dr["STOREZIPCODE"],
                    City = (string) dr["STORECITY"],
                    State = (string) dr["STORESTATE"],
                    Country = (string) dr["STORECOUNTRY"]
                };
            purchaseOrder.DeliveryAddress = deliveryAddress;

            var vendorAddress = new Address
                {
                    Address1 = (string) dr["VENDORSTREET"],
                    Address2 = (string) dr["VENDORADDRESS"],
                    Zip = (string) dr["VENDORZIPCODE"],
                    City = (string) dr["VENDORCITY"],
                    State = (string) dr["VENDORSTATE"],
                    Country = (string) dr["VENDORCOUNTRY"]
                };
            purchaseOrder.VendorAddress = vendorAddress;

            if ((bool)includeReportFormatting)
            {
                purchaseOrder.DeliveryAddressFormatted = entry.Settings.LocalizationContext.FormatMultipleLines(deliveryAddress, entry.Cache, "\n");
                purchaseOrder.VendorAddressFormatted = entry.Settings.LocalizationContext.FormatMultipleLines(vendorAddress, entry.Cache, "\n");
                purchaseOrder.OrdererNameFormatted = entry.Settings.LocalizationContext.NameFormatter.Format(ordererName);
            }
        }

        private static void PopulatePurchaseOrderWithLineTotals(IConnectionManager entry, IDataReader dr, PurchaseOrder purchaseOrder, object includeReportFormatting)
        {
            PopulatePurchaseOrder(entry, dr, purchaseOrder, includeReportFormatting);

            purchaseOrder.TotalQuantity = AsDecimal(dr["TOTALQUANTITY"]);
            purchaseOrder.NumberOfLines = AsInt(dr["NUMBEROFLINES"]);
        }

        protected virtual void PopulatePurchaseOrderWithCount(IConnectionManager entry, IDataReader dr, PurchaseOrder purchaseOrder, ref int rowCount)

        {
            PopulatePurchaseOrder(entry, dr, purchaseOrder, false);
            PopulateRowCount(entry, dr, ref rowCount);
        }

        protected virtual void PopulateRowCount(IConnectionManager entry, IDataReader dr, ref int rowCount)
        {
            if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 ||
                entry.Connection.DatabaseVersion == ServerVersion.Unknown)
            {
                rowCount = (int)dr["Row_Count"];
            }
        }

        private static string ResolveSort(InventoryPurchaseOrderSortEnums sort, string alias, bool isInternalSort)
        {
            switch (sort)
            {
                case InventoryPurchaseOrderSortEnums.ID:
                    return alias + ".PURCHASEORDERID";
                case InventoryPurchaseOrderSortEnums.Description:
                    return alias + ".DESCRIPTION";
                case InventoryPurchaseOrderSortEnums.Status:
                    return alias + ".PURCHASESTATUS";
                case InventoryPurchaseOrderSortEnums.Store:
                    if (isInternalSort)
                    {
                        return "s.NAME";
                    }
                    return alias + ".STORENAME";
                case InventoryPurchaseOrderSortEnums.Vendor:
                    if (isInternalSort)
                    {
                        return "v.NAME";
                    }
                    return alias + ".VENDORNAME";
                case InventoryPurchaseOrderSortEnums.DeliveryDate:
                    return alias + ".DELIVERYDATE";
                case InventoryPurchaseOrderSortEnums.CreationDate:
                    return alias + ".CREATEDDATE";
                case InventoryPurchaseOrderSortEnums.None:
                    return alias + ".PURCHASEORDERID";
                default:
                    return alias + ".PURCHASEORDERID";
            }
        }

        /// <summary>
        /// Searches for purchase orders
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="rowFrom">The starting row number to return</param>
        /// <param name="rowTo">The ending row number to return</param>
        /// <param name="sortBy">The column to sort by</param>
        /// <param name="sortBackwards">Indicates wether the results should be sorted backwards</param>
        /// <param name="itemCount">The number of results that the given parameters yielded</param>
        /// <param name="idOrDescription">A list of IDs and/or descriptions that the search should look for</param>
        /// <param name="idOrDescriptionBeginsWith">Indicates wether the search should look for records that begin with the values in <paramref name="idOrDescription"/></param>
        /// <param name="storeID">The store ID of the store that the purchase orders should belong to</param>
        /// <param name="vendorID">The vendor ID that the purchase orders should belong to</param>
        /// <param name="status">The status that the purchase orders should be in</param>
        /// <param name="deliveryDateFrom">Starting delivery date</param>
        /// <param name="deliveryDateTo">Ending delivery date</param>
        /// <param name="creationDateFrom">Starting creation date</param>
        /// <param name="creationDateTo">Ending creation date</param>
        /// <param name="onlySearchOpenAndNoGoodsReceivingDocument">If true then the search will only return purchase orders that are open and do note have a goods receiving document attached to them</param>
        /// <returns></returns>
        public virtual List<PurchaseOrder> AdvancedSearch(
            IConnectionManager entry, 
            int rowFrom, 
            int rowTo,
            InventoryPurchaseOrderSortEnums sortBy, 
            bool sortBackwards,
            out int itemCount,
            List<string> idOrDescription = null, 
            bool idOrDescriptionBeginsWith = true, 
            RecordIdentifier storeID = null,
            RecordIdentifier vendorID = null, 
            PurchaseStatusEnum? status = null, 
            Date deliveryDateFrom = null,
            Date deliveryDateTo = null, 
            Date creationDateFrom = null, 
            Date creationDateTo = null,
            bool onlySearchOpenAndNoGoodsReceivingDocument = false)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                itemCount = 0;

                string defaultSort = "PURCHASEORDERID";

                List<TableColumn> externalColumns = new List<TableColumn>(purchaseOrdersColumns);

                externalColumns.Add(new TableColumn
                {
                    ColumnName = "ROW"
                });

                externalColumns.Add(new TableColumn
                {
                    ColumnName = "ROW_COUNT"
                });

                List<Condition> externalConditions = new List<Condition>();
                externalConditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "ex.ROW BETWEEN @ROWFROM AND @ROWTO"
                });
                MakeParam(cmd, "ROWFROM", rowFrom, SqlDbType.Int);
                MakeParam(cmd, "ROWTO", rowTo, SqlDbType.Int);

                List<TableColumn> internalColumns = new List<TableColumn>(purchaseOrdersColumns);

                internalColumns.Add(new TableColumn
                {
                    ColumnName =
                        string.Format("ROW_NUMBER() OVER(ORDER BY {0})",
                            sortBy == InventoryPurchaseOrderSortEnums.None
                                ? defaultSort
                                : ResolveSort(sortBy, "p", true)),
                    ColumnAlias = "ROW"
                });

                internalColumns.Add(new TableColumn
                {
                    ColumnName =
                        string.Format(
                            "COUNT(1) OVER ( ORDER BY {0} RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )",
                            sortBy == InventoryPurchaseOrderSortEnums.None
                                ? defaultSort
                                : ResolveSort(sortBy, "p", true)),
                    ColumnAlias = "ROW_COUNT"
                });

                List<Condition> internalConditions = new List<Condition>();

                if (storeID != null)
                {
                    internalConditions.Add(new Condition {Operator = "AND", ConditionValue = "p.STOREID = @STOREID"});
                    MakeParam(cmd, "STOREID", storeID);
                }

                if (vendorID != null)
                {
                    internalConditions.Add(new Condition {Operator = "AND", ConditionValue = "p.VENDORID = @VENDORID"});
                    MakeParam(cmd, "VENDORID", vendorID);
                }

                if (idOrDescription?.Count > 0)
                {
                    List<Condition> searchConditions = new List<Condition>();
                    for (int index = 0; index < idOrDescription.Count; index++)
                    {
                        var searchToken = PreProcessSearchText(idOrDescription[index], true, idOrDescriptionBeginsWith);

                        if (!string.IsNullOrEmpty(searchToken))
                        {
                            searchConditions.Add(new Condition
                            {
                                ConditionValue =
                                    $@" (p.PURCHASEORDERID LIKE @ITEMNAME{index
                                        } 
                                        OR p.DESCRIPTION LIKE @ITEMNAME{
                                        index} 
                                        OR v.NAME LIKE @ITEMNAME{index
                                        } 
                                        OR s.NAME LIKE @ITEMNAME{index}) ",
                                Operator = "AND"
                            });

                            MakeParam(cmd, $"ITEMNAME{index}", searchToken);
                        }
                    }
                    internalConditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = $" ({QueryPartGenerator.ConditionGenerator(searchConditions, true)}) "
                    });
                }

                if (status != null)
                {
                    internalConditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "p.PURCHASESTATUS = @PURCHASESTATUS"
                    });
                    MakeParam(cmd, "PURCHASESTATUS", (int) status);
                }

                if (deliveryDateFrom != null && deliveryDateFrom != Date.Empty)
                {
                    internalConditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "p.DELIVERYDATE >= CONVERT(datetime, @DELIVERYDATEFROM, 103)"
                    });
                    MakeParam(cmd, "DELIVERYDATEFROM", deliveryDateFrom.DateTime.Date, SqlDbType.DateTime);
                }

                if (deliveryDateTo != null && deliveryDateTo != Date.Empty)
                {
                    internalConditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "p.DELIVERYDATE <= CONVERT(datetime, @DELIVERYDATETO, 103)"
                    });
                    MakeParam(cmd, "DELIVERYDATETO", deliveryDateTo.DateTime.Date, SqlDbType.DateTime);
                }

                if (creationDateFrom != null && creationDateFrom != Date.Empty)
                {
                    internalConditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "p.CREATEDDATE >= CONVERT(datetime, @CREATEDDATEFROM, 103)"
                    });
                    MakeParam(cmd, "CREATEDDATEFROM", creationDateFrom.DateTime.Date, SqlDbType.DateTime);
                }

                if (creationDateTo != null && creationDateTo != Date.Empty)
                {
                    internalConditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "p.CREATEDDATE <= CONVERT(datetime, @CREATEDDATETO, 103)"
                    });
                    MakeParam(cmd, "CREATEDDATETO", creationDateTo.DateTime.Date, SqlDbType.DateTime);
                }

                if (onlySearchOpenAndNoGoodsReceivingDocument)
                {
                    internalConditions.Add(new Condition()
                    {
                        Operator = "AND",
                        ConditionValue = "p.PURCHASEORDERID NOT IN (SELECT g.PURCHASEORDERID FROM GOODSRECEIVING g)"
                    });
                }

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("PURCHASEORDERS", "p", "ex", 0, true),
                    QueryPartGenerator.ExternalColumnGenerator(externalColumns, "ex"),
                    QueryPartGenerator.InternalColumnGenerator(internalColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(internalConditions),
                    QueryPartGenerator.ConditionGenerator(externalConditions),
                    "ORDER BY " + ResolveSort(sortBy, "ex", false) + (sortBackwards ? " DESC" : " ASC"));

                return Execute<PurchaseOrder, int>(entry, cmd, CommandType.Text, ref itemCount, PopulatePurchaseOrderWithCount);
            }
        }

        /// <summary>
        /// Gets a list of all PurchaseOrders . The list is sorted by the the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <param name="includeReportFormatting">Set to true if you want address and name formatting, usually for reports</param>
        /// <returns>A list of all PurchaseOrders</returns>
        public virtual List<PurchaseOrder> GetPurchaseOrders(IConnectionManager entry, PurchaseOrderSorting sortBy, bool sortBackwards, bool includeReportFormatting)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>
                {
                    new Condition
                    {
                        ConditionValue = "p.DATAAREAID = @DATAAREAID", Operator = "AND"
                    },
                };

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PURCHASEORDERS", "p"), QueryPartGenerator.InternalColumnGenerator(purchaseOrdersColumns), QueryPartGenerator.JoinGenerator(listJoins), QueryPartGenerator.ConditionGenerator(conditions), "Order by " + ResolveSort(sortBy) + (sortBackwards ? " DESC" : " ASC"));

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Execute<PurchaseOrder>(entry, cmd, CommandType.Text, includeReportFormatting, PopulatePurchaseOrder);
            }
        }

        /// <summary>
        /// Gets a list of purchase orders for a specific vendor. The list is sorted by the the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="vendorID">The ID of the vendor</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>A list of all PurchaseOrders for a specific vendor</returns>
        public virtual List<PurchaseOrder> GetPurchaseOrdersForVendor(IConnectionManager entry, RecordIdentifier vendorID, PurchaseOrderSorting sortBy, bool sortBackwards)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>
                {
                    new Condition
                    {
                        ConditionValue = "p.DATAAREAID = @DATAAREAID", Operator = "AND"
                    },
                    new Condition
                    {
                        ConditionValue = "  p.VENDORID = @VENDORID", Operator = "AND"
                    }
                };

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PURCHASEORDERS", "p"), QueryPartGenerator.InternalColumnGenerator(purchaseOrdersColumns), QueryPartGenerator.JoinGenerator(listJoins), QueryPartGenerator.ConditionGenerator(conditions), "Order by " + ResolveSort(sortBy) + (sortBackwards ? " DESC" : " ASC"));

                MakeParam(cmd, "VENDORID", (string) vendorID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Execute<PurchaseOrder>(entry, cmd, CommandType.Text, false, PopulatePurchaseOrder);
            }
        }

        /// <summary>
        /// Gets a list of purchase orders for a specific store. The list is sorted by the the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The ID of the store</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>A list of all PurchaseOrders for a specific store</returns>
        public virtual List<PurchaseOrder> GetPurchaseOrdersForStore(IConnectionManager entry, RecordIdentifier storeID, PurchaseOrderSorting sortBy, bool sortBackwards)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>
                {
                    new Condition
                    {
                        ConditionValue = "p.DATAAREAID = @DATAAREAID", Operator = "AND"
                    },
                    new Condition
                    {
                        ConditionValue = " p.STOREID = @STOREID", Operator = "AND"
                    }
                };

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PURCHASEORDERS", "p"), QueryPartGenerator.InternalColumnGenerator(purchaseOrdersColumns), QueryPartGenerator.JoinGenerator(listJoins), QueryPartGenerator.ConditionGenerator(conditions), "Order by " + ResolveSort(sortBy) + (sortBackwards ? " DESC" : " ASC"));

                MakeParam(cmd, "STOREID", (string) storeID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Execute<PurchaseOrder>(entry, cmd, CommandType.Text, false, PopulatePurchaseOrder);
            }
        }

        /// <summary>
        /// Gets a list of purchase orders for a specific store and a specific vendor. The list is sorted by the the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The ID of the store</param>
        /// <param name="vendorID">The ID of the vendor</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>A list of all PurchaseOrders for a specific store and a specific vendor</returns>
        public virtual List<PurchaseOrder> GetPurchaseOrdersForStoreAndVendor(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier vendorID, PurchaseOrderSorting sortBy, bool sortBackwards)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>
                {
                    new Condition
                    {
                        ConditionValue = "p.DATAAREAID = @DATAAREAID", Operator = "AND"
                    },
                    new Condition
                    {
                        ConditionValue = " p.STOREID = @STOREID", Operator = "AND"
                    },
                    new Condition
                    {
                        ConditionValue = "p.VENDORID = @VENDORID ", Operator = "AND"
                    },
                };

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PURCHASEORDERS", "p"), QueryPartGenerator.InternalColumnGenerator(purchaseOrdersColumns), QueryPartGenerator.JoinGenerator(listJoins), QueryPartGenerator.ConditionGenerator(conditions), "Order by " + ResolveSort(sortBy) + (sortBackwards ? " DESC" : " ASC"));

                MakeParam(cmd, "STOREID", (string) storeID);
                MakeParam(cmd, "VENDORID", (string) vendorID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Execute<PurchaseOrder>(entry, cmd, CommandType.Text, false, PopulatePurchaseOrder);
            }
        }

        /// <summary>
        /// Gets a list of all purchase orders for a store that are not linked to a goods receiving document. The list is sorted by the the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The store ID</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <param name="includeLineTotals">True if the total quantity of items and total number of items should be included in the query. Used in OMNI</param>
        /// <returns>A list of all purchase orders for a store that are not linked to a goods receiving document</returns>
        public virtual List<PurchaseOrder> GetPurchaseOrdersWithNoGoodsReceivingDocumentForStore(IConnectionManager entry, RecordIdentifier storeID, PurchaseOrderSorting sortBy, bool sortBackwards, bool includeLineTotals)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>(purchaseOrdersColumns);
                List<Join> joins = new List<Join>(listJoins);

                string groupSortString = "";
                DataPopulatorWithEntry<PurchaseOrder> populator = PopulatePurchaseOrder;

                if (includeLineTotals)
                {
                    groupSortString += "GROUP BY " + QueryPartGenerator.GroupByColumnGenerator(columns);

                    columns.Add(new TableColumn { ColumnName = "SUM(pwl.QUANTITY)", ColumnAlias = "TOTALQUANTITY" });
                    columns.Add(new TableColumn { ColumnName = "COUNT(pwl.PURCHASEORDERID)", ColumnAlias = "NUMBEROFLINES" });
                    joins.Add(new Join { Table = "PURCHASEORDERLINE", TableAlias = "pwl", JoinType = "LEFT", Condition = "p.PURCHASEORDERID = pwl.PURCHASEORDERID" });
                    populator = PopulatePurchaseOrderWithLineTotals;
                }

                groupSortString += " ORDER BY " + ResolveSort(sortBy) + (sortBackwards ? " DESC" : " ASC");

                List<Condition> conditions = new List<Condition>
                {
                    new Condition
                    {
                        ConditionValue = "p.DATAAREAID = @DATAAREAID", Operator = "AND"
                    },
                    new Condition
                    {
                        ConditionValue = "p.PURCHASESTATUS = 0", Operator = "AND"
                    },
                    new Condition
                    {
                        ConditionValue = "p.PURCHASEORDERID NOT IN " + "(SELECT g.PURCHASEORDERID FROM GOODSRECEIVING g)", Operator = "AND"
                    },
                };
                if (storeID != RecordIdentifier.Empty && storeID != "")
                {
                    conditions.Add(new Condition
                    {
                        ConditionValue = " p.STOREID = @STOREID", Operator = "AND"
                    });

                    MakeParam(cmd, "STOREID", (string) storeID);
                }

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PURCHASEORDERS", "p"), 
                    QueryPartGenerator.InternalColumnGenerator(columns), 
                    QueryPartGenerator.JoinGenerator(joins), 
                    QueryPartGenerator.ConditionGenerator(conditions), 
                    groupSortString);

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Execute<PurchaseOrder>(entry, cmd, CommandType.Text, false, populator);
            }
        }

        /// <summary>
        /// Checks if a purchase order with a given ID exists in the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderID">The ID of the purchase order to check for</param>
        /// <returns>Whether a purchase order with a given ID exists in the database</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier purchaseOrderID)
        {
            return RecordExists(entry, "PURCHASEORDERS", "PURCHASEORDERID", purchaseOrderID);
        }

        /// <summary>
        /// Deletes a purchase order with a given ID
        /// </summary>
        /// <remarks>Requires the 'ManagePurchaseOrders' permission</remarks>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderID">The ID of the purchase order to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier purchaseOrderID)
        {
            DeleteRecord(entry, "PURCHASEORDERLINE", "PURCHASEORDERID", purchaseOrderID, Permission.ManagePurchaseOrders);
            DeleteRecord(entry, "PURCHASEORDERS", "PURCHASEORDERID", purchaseOrderID, Permission.ManagePurchaseOrders);
        }

        /// <summary>
        /// Gets a purchase order with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderID">The ID of the purchase order to get</param>
        /// <param name="includeReportFormatting">Set to true if you want address and name formatting, usually for reports</param>
        /// <returns>A purchase order with a given ID</returns>
        public virtual PurchaseOrder Get(IConnectionManager entry, RecordIdentifier purchaseOrderID, bool includeReportFormatting)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>
                {
                    new Condition
                    {
                        ConditionValue = "p.DATAAREAID = @DATAAREAID", Operator = "AND"
                    },
                    new Condition
                    {
                        ConditionValue = "p.PURCHASEORDERID = @PURCHASEORDERID", Operator = "AND"
                    }
                };

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PURCHASEORDERS", "p"), QueryPartGenerator.InternalColumnGenerator(purchaseOrdersColumns), QueryPartGenerator.JoinGenerator(listJoins), QueryPartGenerator.ConditionGenerator(conditions), string.Empty);

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "PURCHASEORDERID", (string) purchaseOrderID);

                var result = Execute<PurchaseOrder>(entry, cmd, CommandType.Text, includeReportFormatting, PopulatePurchaseOrder);

                return (result.Count > 0) ? result[0] : null;
            }
        }

        /// <summary>
        /// Saves a given purchase order into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrder">The Purchase order to save</param>
        public virtual void Save(IConnectionManager entry, PurchaseOrder purchaseOrder)
        {
            var statement = new SqlServerStatement("PURCHASEORDERS", false);

            ValidateSecurity(entry, Permission.ManagePurchaseOrders);

            if (RecordIdentifier.IsEmptyOrNull(purchaseOrder.PurchaseOrderID))
            {
                purchaseOrder.PurchaseOrderID = DataProviderFactory.Instance.GenerateNumber<IPurchaseOrderData, PurchaseOrder>(entry);
            }

            purchaseOrder.Validate();

            if (!Exists(entry, purchaseOrder.PurchaseOrderID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("PURCHASEORDERID", (string) purchaseOrder.PurchaseOrderID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("PURCHASEORDERID", (string) purchaseOrder.PurchaseOrderID);
            }

            statement.AddField("VENDORID", purchaseOrder.VendorID);
            statement.AddField("PURCHASESTATUS", (int) purchaseOrder.PurchaseStatus, SqlDbType.Int);
            statement.AddField("DELIVERYDATE", purchaseOrder.DeliveryDate, SqlDbType.DateTime);
            statement.AddField("CREATEDDATE", purchaseOrder.CreatedDate, SqlDbType.DateTime);
            statement.AddField("CURRENCYCODE", (string) purchaseOrder.CurrencyCode);
            statement.AddField("ORDERER", purchaseOrder.Orderer, SqlDbType.UniqueIdentifier);
            statement.AddField("STOREID", (string) purchaseOrder.StoreID);
            statement.AddField("ORDERINGDATE", purchaseOrder.OrderingDate.ToAxaptaSQLDate(false), SqlDbType.DateTime);
            statement.AddField("DEFAULTDISCOUNTPERCENTAGE", purchaseOrder.DefaultDiscountPercentage, SqlDbType.Decimal);
            statement.AddField("DEFAULTDISCOUNTAMOUNT", purchaseOrder.DefaultDiscountAmount, SqlDbType.Decimal);
            statement.AddField("DESCRIPTION", purchaseOrder.Description);
            statement.AddField("CREATEDFROMOMNI", purchaseOrder.CreatedFromOmni, SqlDbType.Bit);
            statement.AddField("TEMPLATEID", (string)purchaseOrder.TemplateID);
            statement.AddField("PROCESSINGSTATUS", (int)purchaseOrder.ProcessingStatus, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);

            purchaseOrder.Dirty = false;
        }

        /// <summary>
        /// Saves a given purchase order into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrder">The Purchase order to save</param>
        /// <returns>Purchase order</returns>
        public virtual PurchaseOrder SaveAndReturnPurchaseOrder(IConnectionManager entry, PurchaseOrder purchaseOrder)
        {
            Save(entry, purchaseOrder);
            return purchaseOrder;
        }

        public virtual List<DataEntity> SearchItemsInPurchaseOrder(IConnectionManager entry, RecordIdentifier purchaserOrderID, string searchString, int rowFrom, int rowTo, bool beginsWith)
        {
            string modifiedSearchString = PreProcessSearchText(searchString, true, beginsWith);

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "ISNULL (B.ITEMID, T.RETAILITEMID)", ColumnAlias = "RETAILITEMID"}, new TableColumn {ColumnName = "COALESCE(B.ITEMNAME,A.ITEMNAME,'')", ColumnAlias = "DESCRIPTION"},
                };
                List<TableColumn> allColumns = new List<TableColumn>(columns)
                {
                    new TableColumn {ColumnName = "ROW_NUMBER() OVER(order by COALESCE(B.ITEMNAME,A.ITEMNAME,''))", ColumnAlias = "ROW"}
                };

                List<Join> joins = new List<Join>
                {
                    new Join
                    {
                        Condition = "a.ITEMID = t.RETAILITEMID", JoinType = "Left Outer", Table = "RETAILITEM", TableAlias = "A"
                    },
                    new Join
                    {
                        Table = "RETAILITEM", TableAlias = "B", Condition = " A.HEADERITEMID = B.MASTERID   ", JoinType = "LEFT OUTER"
                    }
                };

                List<Condition> conditions = new List<Condition>
                {
                    new Condition {ConditionValue = "T.PURCHASEORDERID = @PURCHASEORDERID", Operator = "AND"},
                    new Condition {ConditionValue = @" ((A.ITEMNAME LIKE @SEARCHSTRING OR A.ITEMID LIKE @SEARCHSTRING) 
                                                        OR (B.ITEMNAME Like @SEARCHSTRING OR B.ITEMID LIKE @SEARCHSTRING))", Operator = "AND"},
                    new Condition {ConditionValue = "A.DELETED = 0", Operator = "AND"},
                    new Condition {ConditionValue = "A.ITEMTYPE != 2", Operator = "AND"}
                };

                Condition pagingCondition = new Condition
                {
                    ConditionValue = "s.ROW BETWEEN @ROWFROM AND @ROWTO"
                };

                cmd.CommandText = string.Format(QueryTemplates.PagingQueryWithGroup("PURCHASEORDERLINE", "T", "S"), QueryPartGenerator.ExternalColumnGenerator(allColumns, "S"), QueryPartGenerator.InternalColumnGenerator(allColumns), QueryPartGenerator.JoinGenerator(joins), QueryPartGenerator.ConditionGenerator(conditions), QueryPartGenerator.GroupByColumnGenerator(columns), QueryPartGenerator.ConditionGenerator(pagingCondition), "ORDER BY DESCRIPTION");

                MakeParam(cmd, "PURCHASEORDERID", (string) purchaserOrderID.PrimaryID);
                MakeParam(cmd, "SEARCHSTRING", modifiedSearchString);
                MakeParam(cmd, "ROWFROM", rowFrom);
                MakeParam(cmd, "ROWTO", rowTo);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "DESCRIPTION", "RETAILITEMID");
            }
        }

        public virtual List<Unit> GetUnitsForPurchaserOrderItemVariant(IConnectionManager entry, RecordIdentifier purchaseOrderID, RecordIdentifier itemID)
        {
            // Get data entities containing unitIDs for the purchaser order, item and variant
            ValidateSecurity(entry);
            List<DataEntity> unitDataEntities;

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT DISTINCT p.UNITID FROM PURCHASEORDERLINE p WHERE p.PURCHASEORDERID = @PURCHASEORDERID AND p.RETAILITEMID = @ITEMID AND p.DATAAREAID = @DATAAREAID ORDER BY p.UNITID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "PURCHASEORDERID", (string) purchaseOrderID);
                MakeParam(cmd, "ITEMID", (string) itemID);

                unitDataEntities = Execute<DataEntity>(entry, cmd, CommandType.Text, "UNITID", "UNITID");
            }

            var units = new List<Unit>();
            foreach (var unitData in unitDataEntities)
            {
                units.Add(Providers.UnitData.Get(entry, unitData.ID));
            }

            return units;
        }

        public virtual bool HasPostedGoodsReceivingDocument(IConnectionManager entry, RecordIdentifier purchaseOrderID)
        {
            if (Providers.GoodsReceivingDocumentData.Exists(entry, purchaseOrderID) && Providers.GoodsReceivingDocumentData.HasPostedLines(entry, purchaseOrderID))
            {
                return true;
            }

            return false;
        }

        public virtual bool HasGoodsReceivingDocument(IConnectionManager entry, RecordIdentifier purchaseOrderID)
        {
            return Providers.GoodsReceivingDocumentData.Exists(entry, purchaseOrderID);
        }

        public virtual bool HasPurchaseOrderLines(IConnectionManager entry, RecordIdentifier purchaseOrderID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT 'X' FROM PURCHASEORDERLINE WHERE PURCHASEORDERID = @PURCHASEORDERID AND DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "PURCHASEORDERID", (string) purchaseOrderID);

                var value = entry.Connection.ExecuteScalar(cmd);

                return (value != null);
            }
        }

        /// <summary>
        /// Returns the total number of purchase orders
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>The total number of purchase orders</returns>
        public virtual int CountOrders(IConnectionManager entry)
        {
            ValidateSecurity(entry);

            return Count(entry, "PURCHASEORDERS");
        }

        public virtual int CreatePurchaseOrderLinesFromFilter(IConnectionManager entry, RecordIdentifier purchaseOrderID, InventoryTemplateFilterContainer filter)
        {
            using (SqlCommand cmd = new SqlCommand("spINVENTORY_CreatePurchaseOrderLinesFromFilter"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                string filterDelimiter = ";";

                MakeParam(cmd, "PURCHASEORDERID", purchaseOrderID);
                MakeParam(cmd, "RETAILGROUPS", filter.RetailGroups.Count == 0 ? "" : filter.RetailGroups.Select(x => x.StringValue).Aggregate((x, y) => x + filterDelimiter + y), SqlDbType.VarChar);
                MakeParam(cmd, "RETAILDEPARTMENTS", filter.RetailDepartments.Count == 0 ? "" : filter.RetailDepartments.Select(x => x.StringValue).Aggregate((x, y) => x + filterDelimiter + y), SqlDbType.VarChar);
                MakeParam(cmd, "VENDORS", filter.Vendors.Count == 0 ? "" : filter.Vendors.Select(x => x.StringValue).Aggregate((x, y) => x + filterDelimiter + y));
                MakeParam(cmd, "SPECIALGROUPS", filter.SpecialGroups.Count == 0 ? "" : filter.SpecialGroups.Select(x => x.StringValue).Aggregate((x, y) => x + filterDelimiter + y), SqlDbType.VarChar);
                MakeParam(cmd, "FILTERDELIMITER", filterDelimiter, SqlDbType.VarChar);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                SqlParameter insertedRows = MakeParam(cmd, "INSERTEDRECORDS", "", SqlDbType.Int, ParameterDirection.Output, 4);

                entry.Connection.ExecuteNonQuery(cmd, false);

                return (int)insertedRows.Value;
            }
        }

        public virtual void SetPurchaseOrderProcessingStatus(IConnectionManager entry, RecordIdentifier purchaseOrderID, InventoryProcessingStatus status)
        {
            var statement = new SqlServerStatement("PURCHASEORDERS");

            statement.StatementType = StatementType.Update;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("PURCHASEORDERID", (string)purchaseOrderID);

            statement.AddField("PROCESSINGSTATUS", (int)status, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual PurchaseOrder GetOmniPurchaseOrderByTemplate(IConnectionManager entry, RecordIdentifier templateID, RecordIdentifier storeID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>
                {
                    new Condition { ConditionValue = "p.DATAAREAID = @DATAAREAID", Operator = "AND" },
                    new Condition { ConditionValue = "p.CREATEDFROMOMNI = 1", Operator = "AND" },
                    new Condition { ConditionValue = "p.TEMPLATEID = @TEMPLATEID", Operator = "AND" },
                    new Condition { ConditionValue = "p.STOREID = @STOREID", Operator = "AND" },
                    new Condition { ConditionValue = "p.PURCHASESTATUS = 0", Operator = "AND" },
                };

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PURCHASEORDERS", "p"), 
                    QueryPartGenerator.InternalColumnGenerator(purchaseOrdersColumns), 
                    QueryPartGenerator.JoinGenerator(listJoins), 
                    QueryPartGenerator.ConditionGenerator(conditions), " ORDER BY p.CREATEDDATE DESC");

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "TEMPLATEID", (string)templateID);
                MakeParam(cmd, "STOREID", (string)storeID);

                var result = Execute<PurchaseOrder>(entry, cmd, CommandType.Text, false, PopulatePurchaseOrder);

                return (result.Count > 0) ? result[0] : null;
            }
        }

        #region ISequenceable Members

        /// <summary>
        /// Checks if a sequence with a given ID exists for a purchase order
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">ID to check for</param>
        /// <returns>True if it exists, else false</returns>
        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        /// <summary>
        /// ID into the sequence manager.
        /// </summary>
        public RecordIdentifier SequenceID
        {
            get { return "PurchaseOrders"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "PURCHASEORDERS", "PURCHASEORDERID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}