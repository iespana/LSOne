using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.SqlDataProviders.Inventory
{
    public class VendorItemData : SqlServerDataProviderBase, IVendorItemData
    {
        private static List<TableColumn> vendorColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "INTERNALID" , TableAlias = "A"},
            new TableColumn {ColumnName = "ISNULL(b.ITEMNAME,'')" , ColumnAlias = "ITEMNAME"},
            new TableColumn {ColumnName = "ISNULL(b.VARIANTNAME,'')" , ColumnAlias = "VARIANTNAME"},
            new TableColumn {ColumnName = "ITEMPRICE" , TableAlias = "A"},
            new TableColumn {ColumnName = "DEFAULTPURCHASEPRICE" , TableAlias = "A"},
            new TableColumn {ColumnName = "VENDORITEMID" , TableAlias = "A"},
            new TableColumn {ColumnName = "ISNULL(b.ITEMID,'')" , ColumnAlias = "RETAILITEMID"},
            new TableColumn {ColumnName = "UNITID" , TableAlias = "A"},
            new TableColumn {ColumnName = "VENDORID" , TableAlias = "A"},
            new TableColumn {ColumnName = "ISNULL(u.TXT,'')" , ColumnAlias = "UNITDESCRIPTION"},
            new TableColumn {ColumnName = "ISNULL(a.LASTORDERDATE,'01.01.1900')" , ColumnAlias = "LASTORDERDATE"},
            new TableColumn {ColumnName = "ISNULL(v.NAME,'') " , ColumnAlias = "VENDORDESCRIPTION"},
            new TableColumn {ColumnName = "ISNULL(b.ITEMTYPE, 0)", ColumnAlias = "ITEMTYPE" },
        };

        private static List<Join> listJoins = new List<Join>
        {

            new Join
            {
                Condition = "A.RETAILITEMID = B.ITEMID",
                JoinType = "LEFT OUTER",
                Table = "RETAILITEM",
                TableAlias = "B"
            },
            new Join
            {
                Condition = "A.UNITID = U.UNITID",
                JoinType = "LEFT OUTER",
                Table = "UNIT",
                TableAlias = "U"
            },
            new Join
            {
                Condition = "V.ACCOUNTNUM = A.VENDORID",
                JoinType = "LEFT OUTER",
                Table = "VENDTABLE",
                TableAlias = "V"
            }


        };
        

        private static Dictionary<VendorItemSorting, TableColumn> SortColumns = new Dictionary<VendorItemSorting, TableColumn>
        {
            {VendorItemSorting.ID, new TableColumn { ColumnName = "INTERNALID", TableAlias = "A" }},
            {VendorItemSorting.Description, new TableColumn {ColumnName = "ITEMNAME", TableAlias = "B"}},
            {VendorItemSorting.VendorItemID, new TableColumn {ColumnName = "VENDORITEMID", TableAlias = "A"}},
            {VendorItemSorting.SizeDescription, new TableColumn {ColumnName = "NAME", TableAlias = "S"}},
            {VendorItemSorting.ColorDescription, new TableColumn {ColumnName = "NAME", TableAlias = "C"}},
            {VendorItemSorting.StyleDescription, new TableColumn {ColumnName = "NAME", TableAlias = "T"}},
            {VendorItemSorting.UnitDescription, new TableColumn {ColumnName = "TXT", TableAlias = "U", ColumnAlias = "UNITDESCRIPTION"}},
            {VendorItemSorting.ItemPrice, new TableColumn {ColumnName = "ITEMPRICE", TableAlias = "A"}},
            {VendorItemSorting.LastOrderDate, new TableColumn {ColumnName = "LASTORDERDATE", TableAlias = "A"}},

            {VendorItemSorting.VendorName, new TableColumn {ColumnName = "NAME", TableAlias = "V", ColumnAlias = "VENDORDESCRIPTION"}},
            {VendorItemSorting.DefaultPurchasePrice, new TableColumn {ColumnName = "DEFAULTPURCHASEPRICE", TableAlias = "A"}},
            {VendorItemSorting.Variant, new TableColumn {ColumnName = "VARIANTNAME", TableAlias = "B"}}
        };

        private static string ResolveSort(VendorItemSorting sort, bool backwards, bool externalColumn = false)
        {
            string sortString = "";
            sortString = SortColumns[sort].ToSortString(externalColumn);            

            if (backwards)
            {
                sortString = sortString.Replace("ASC", "DESC");
            }

            return sortString;
        }

        private static void PopulateMinimumVendorItem(IDataReader dr, VendorItem vendorItem)
        {
            vendorItem.ID = (string)dr["INTERNALID"];
            vendorItem.VendorItemID = (string)dr["VENDORITEMID"];
            vendorItem.RetailItemID = (string)dr["RETAILITEMID"];
            vendorItem.VariantName = dr["VARIANTNAME"] == DBNull.Value ? "" : (string)dr["VARIANTNAME"];

            vendorItem.UnitID = (string)dr["UNITID"];
            vendorItem.VendorID = (string)dr["VENDORID"];
            vendorItem.LastItemPrice = (decimal)dr["ITEMPRICE"];
            vendorItem.DefaultPurchasePrice = (decimal)dr["DEFAULTPURCHASEPRICE"];
            vendorItem.LastOrderDate = Date.FromAxaptaDate(dr["LASTORDERDATE"]);
        }

        private static void PopulateVendorItem(IDataReader dr, VendorItem vendorItem)
        {
            PopulateMinimumVendorItem(dr, vendorItem);

            vendorItem.Text = (string)dr["ITEMNAME"];
            vendorItem.UnitDescription = (string)dr["UNITDESCRIPTION"];
            vendorItem.VendorDescription = (string)dr["VENDORDESCRIPTION"];

            vendorItem.RetailItemType = (ItemTypeEnum)((byte)dr["ITEMTYPE"]);
        }

        private static void PopulateVendorItemWithSecondaryID(IDataReader dr, DataEntity dataEntity)
        {
            dataEntity.Text = (string)dr["DESCRIPTION"];
            dataEntity.ID = new RecordIdentifier((string)dr["RETAILITEMID"], new RecordIdentifier((string)dr["VENDORITEMID"]));
        }

        protected virtual void PopulateVendorItemWithCount(IConnectionManager entry, IDataReader dr, VendorItem vendorItem, ref int rowCount)

        {
            PopulateVendorItem(dr, vendorItem);
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

        private static void PopulateItemAsMasterIDEntity(IDataReader dr, MasterIDEntity vendorItem)
        {
            vendorItem.ID = dr["MASTERID"] == DBNull.Value ? Guid.Empty : (Guid)dr["MASTERID"];
            vendorItem.ReadadbleID = (string)dr["RETAILITEMID"];
            vendorItem.Text = (string)dr["ITEMNAME"];
            vendorItem.ExtendedText = (string)dr["VARIANTNAME"];
        }

        /// <summary>
        /// Gets a list of DataEntity that contains Vendor item ID and Item Description for a given vendorID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="vendorID">ID of the vendor to get the list for</param>
        /// <returns>List of the vendor items from a given vendor</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry, RecordIdentifier vendorID)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "INTERNALID", TableAlias =  "A"},
                    new TableColumn {ColumnName = "ISNULL(B.ITEMNAME,'')", ColumnAlias =  "ITEMNAME"},

                };

                List<Join> joins = new List<Join>
                {
                    new Join
                    {
                        Table = "RETAILITEM",
                        TableAlias = "B",
                        Condition = "A.RETAILITEMID = B.ITEMID",
                        JoinType = "LEFT OUTER"
                    }
                };
                List<Condition> conditions = new List<Condition>
                {
                    new Condition { ConditionValue = " a.VENDORID = @VENDORID ", Operator = "AND"},
                };

                cmd.CommandText = string.Format(
                 QueryTemplates.BaseQuery("VENDORITEMS", "A"),
                 QueryPartGenerator.InternalColumnGenerator(columns),
                 QueryPartGenerator.JoinGenerator(joins),
                 QueryPartGenerator.ConditionGenerator(conditions),
                 "ORDER BY B.ITEMNAME"
                 );

                MakeParam(cmd, "VENDORID", (string)vendorID);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMNAME", "INTERNALID");
            }
        }

        /// <summary>
        /// Gets a list of DataEntities that contains distinct list of retail item ID's and Item Description for a given vendorID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="vendorID">ID of the vendor to get the list for</param>
        /// <returns>List of the retail items from a given vendor</returns>
        public virtual List<MasterIDEntity> GetDistinctRetailItemsForVendor(IConnectionManager entry, RecordIdentifier vendorID)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "RETAILITEMID", TableAlias =  "A"},
                    new TableColumn {ColumnName = "MASTERID", TableAlias =  "B"},
                    new TableColumn {ColumnName = "ISNULL(B.ITEMNAME,'')", ColumnAlias =  "ITEMNAME"},
                    new TableColumn {ColumnName = "ISNULL(B.VARIANTNAME,'')", ColumnAlias =  "VARIANTNAME"}
            };

                List<Join> joins = new List<Join>
                {
                    new Join
                    {
                        Table = "RETAILITEM",
                        TableAlias = "B",
                        Condition = "A.RETAILITEMID = B.ITEMID",
                        JoinType = "LEFT OUTER"
                    }
                };
                List<Condition> conditions = new List<Condition>
                {
                    new Condition { ConditionValue = " a.VENDORID = @VENDORID ", Operator = "AND"},
                    new Condition { ConditionValue = " b.ITEMTYPE <> 2 ", Operator = "AND"},
                };

                cmd.CommandText = string.Format(
                 QueryTemplates.BaseQuery("VENDORITEMS", "A", 0, true),
                 QueryPartGenerator.InternalColumnGenerator(columns),
                 QueryPartGenerator.JoinGenerator(joins),
                 QueryPartGenerator.ConditionGenerator(conditions),
                 "ORDER BY ISNULL(B.ITEMNAME,'')"
                 );

                MakeParam(cmd, "VENDORID", (string)vendorID);

                return Execute<MasterIDEntity>(entry, cmd, CommandType.Text, PopulateItemAsMasterIDEntity);
            }
        }


        public virtual List<DataEntity> SearchRetailItemsForVendor(IConnectionManager entry, RecordIdentifier vendorID, string searchString, RecordIdentifier itemGroupId, int rowFrom, int rowTo, bool beginsWith)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                string modifiedSearchString = PreProcessSearchText(searchString, true, beginsWith);
                List<TableColumn> columns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "ISNULL (C.ITEMID, A.RETAILITEMID)", ColumnAlias =  "RETAILITEMID"},
                    new TableColumn {ColumnName = "A.VENDORITEMID", ColumnAlias =  "VENDORITEMID"},
                    new TableColumn {ColumnName = "COALESCE(C.ITEMNAME,B.ITEMNAME,'')", ColumnAlias =  "DESCRIPTION"}

                };
                List<TableColumn> externalColumns = new List<TableColumn>(columns);
                columns.Add(new TableColumn
                {
                    ColumnName = "ROW_NUMBER() OVER(order by b.ITEMNAME)",
                    ColumnAlias = "ROW"
                });

                List<Join> joins = new List<Join>
                {
                    new Join
                    {
                        Table = "RETAILITEM",
                        TableAlias = "B",
                        Condition = "A.RETAILITEMID = B.ITEMID",
                        JoinType = "LEFT OUTER"
                    },
                    new Join
                    {
                        Table = "RETAILITEM",
                        TableAlias = "C",
                        Condition = " B.HEADERITEMID = C.MASTERID   ",
                        JoinType = "LEFT OUTER"
                    }
                };
                List<Condition> conditions = new List<Condition>
                {
                    new Condition { ConditionValue = " a.VENDORID = @VENDORID ", Operator = "AND"},
                    new Condition { ConditionValue = @" ((b.ITEMNAME LIKE @SEARCHSTRING OR b.ITEMID LIKE @SEARCHSTRING) 
                                                        OR (c.ITEMNAME LIKE @SEARCHSTRING OR c.ITEMID LIKE @SEARCHSTRING)
                                                        OR (a.VENDORITEMID LIKE @SEARCHSTRING))", Operator = "AND"},
                    new Condition { ConditionValue = "b.DELETED = 0", Operator = "AND"},
                    new Condition { ConditionValue = "b.ITEMTYPE != 2", Operator = "AND"}
                };

                List<Condition> externalConditions = new List<Condition>();
                externalConditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "S.ROW BETWEEN @ROWFROM AND @ROWTO"
                });

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("VENDORITEMS", "A", "S", 0, true),
                   QueryPartGenerator.ExternalColumnGenerator(externalColumns, "S"),
                   QueryPartGenerator.InternalColumnGenerator(columns),
                   QueryPartGenerator.JoinGenerator(joins),
                   QueryPartGenerator.ConditionGenerator(conditions),
                   QueryPartGenerator.ConditionGenerator(externalConditions),
                   "ORDER BY DESCRIPTION");


                MakeParam(cmd, "ROWFROM", rowFrom, SqlDbType.Int);
                MakeParam(cmd, "ROWTO", rowTo, SqlDbType.Int);
                MakeParam(cmd, "VENDORID", (string)vendorID);
                MakeParam(cmd, "SEARCHSTRING", modifiedSearchString);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, PopulateVendorItemWithSecondaryID);
            }
        }

        private static void PopulateUnit(IDataReader dr, Unit unit)
        {
            unit.ID = (string)dr["UNITID"];
            unit.Text = (string)dr["DESCRIPTION"];
            unit.MaximumDecimals = (int)dr["UNITDECIMALS"];
        }

        public virtual List<Unit> GetDistinctUnitsForVendorItem(IConnectionManager entry, RecordIdentifier vendorId,
            RecordIdentifier retailItemId)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "UNITID", TableAlias = "A"},
                    new TableColumn {ColumnName = "ISNULL(u.TXT,'') ", ColumnAlias = "DESCRIPTION"},
                    new TableColumn {ColumnName = "ISNULL(u.UNITDECIMALS,0) ", ColumnAlias = "UnitDecimals"},

                };

                List<Join> joins = new List<Join>
                {
                    new Join
                    {
                        Table = "UNIT",
                        TableAlias = "U",
                        Condition = "A.UNITID = U.UNITID",
                        JoinType = "LEFT OUTER"
                    }
                };
                List<Condition> conditions = new List<Condition>
                {
                    new Condition {ConditionValue = " A.VENDORID = @VENDORID ", Operator = "AND"},
                    new Condition {ConditionValue = " A.RETAILITEMID = @RETAILITEMID ", Operator = "AND"},
                };

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("VENDORITEMS", "A"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "ORDER BY u.TXT"
                    );

                MakeParam(cmd, "VENDORID", (string)vendorId);
                MakeParam(cmd, "RETAILITEMID", (string)retailItemId);

                return Execute<Unit>(entry, cmd, CommandType.Text, PopulateUnit);
            }
        }

        /// <summary>
        /// Gets a single vendor item by a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">ID of record to fetch (the internal sequence id)</param>
        /// <returns>The requested vendor item or null if not found</returns>
        public virtual VendorItem Get(IConnectionManager entry, RecordIdentifier id)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>
                {
                    new Condition {ConditionValue = " a.INTERNALID = @INTERNALID ", Operator = "AND"},
                };

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("VENDORITEMS", "A"),
                    QueryPartGenerator.InternalColumnGenerator(vendorColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                   string.Empty
                    );

                MakeParam(cmd, "INTERNALID", (string)id);

                var list = Execute<VendorItem>(entry, cmd, CommandType.Text, PopulateVendorItem);
                return list.Count > 0 ? list[0] : null;
            }
        }

        /// <summary>
        /// Gets a single vendor item by a given vendor ID and given vendor item ID (External Vendor item ID)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="vendorID">ID of the Vendor</param>
        /// <param name="vendorItemID">ID of item as the vendor knows it (External Vendor item id)</param>
        /// <returns>The requested vendor item or null if not found</returns>
        public virtual VendorItem Get(IConnectionManager entry, RecordIdentifier vendorID, RecordIdentifier vendorItemID)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>
                {
                    new Condition {ConditionValue = " A.VENDORID = @VENDORID ", Operator = "AND"},
                    new Condition {ConditionValue = " A.VENDORITEMID = @VENDORITEMID ", Operator = "AND"},
                };

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("VENDORITEMS", "A"),
                    QueryPartGenerator.InternalColumnGenerator(vendorColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                   string.Empty
                    );

                MakeParam(cmd, "VENDORID", (string)vendorID);
                MakeParam(cmd, "VENDORITEMID", (string)vendorItemID);

                var list = Execute<VendorItem>(entry, cmd, CommandType.Text, PopulateVendorItem);
                return list.Count > 0 ? list[0] : null;
            }
        }

        /// <summary>
        /// Gets a single vendor item by a given vendorID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="vendorID">ID of the vendor</param>
        /// <param name="retailItemID">Item of the retail item</param>
        /// <param name="unitID">Unit ID of the item</param>
        /// <returns>The requested vendor item or null if not found</returns>
        public virtual VendorItem Get(IConnectionManager entry, RecordIdentifier vendorID, RecordIdentifier retailItemID, RecordIdentifier unitID)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>
                {
                    new Condition {ConditionValue = " A.VENDORID = @VENDORID ", Operator = "AND"},
                    new Condition {ConditionValue = " A.RETAILITEMID = @RETAILITEMID ", Operator = "AND"},
                    new Condition {ConditionValue = " A.UNITID = @UNITID ", Operator = "AND"},
                };

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("VENDORITEMS", "A"),
                    QueryPartGenerator.InternalColumnGenerator(vendorColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                   string.Empty
                    );

                MakeParam(cmd, "VENDORID", (string)vendorID);
                MakeParam(cmd, "RETAILITEMID", (string)retailItemID);
                MakeParam(cmd, "UNITID", (string)unitID);

                var list = Execute<VendorItem>(entry, cmd, CommandType.Text, PopulateVendorItem);
                return list.Count > 0 ? list[0] : null;
            }
        }

        /// <summary>
        /// Gets all vendor items for a given vendor ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorID">ID of the vendor to fetch records for</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>All vendors in the database</returns>
        public virtual List<VendorItem> GetItemsForVendor(IConnectionManager entry, RecordIdentifier vendorID, VendorItemSorting sortBy, bool sortBackwards)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>
                {
                    new Condition {ConditionValue = " A.VENDORID = @VENDORID ", Operator = "AND"},
                    new Condition {ConditionValue = " B.DELETED = 0 ", Operator = "AND"},
                };

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("VENDORITEMS", "A"),
                    QueryPartGenerator.InternalColumnGenerator(vendorColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "ORDER BY " + ResolveSort(sortBy, sortBackwards)
                    );

                MakeParam(cmd, "VENDORID", (string)vendorID);

                return Execute<VendorItem>(entry, cmd, CommandType.Text, PopulateVendorItem);
            }
        }

        /// <summary>
        /// Gets paginated vendor items for a given vendor ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorID">ID of the vendor to fetch records for</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <param name="startRecord">Pagination start index</param>
        /// <param name="endRecord">Pagination end index</param>
        /// <param name="totalRecords">Total number of records for the given vendor</param>
        /// <returns>All vendors in the database</returns>
        public virtual List<VendorItem> GetItemsForVendor(IConnectionManager entry,
                                                          RecordIdentifier vendorID,
                                                          VendorItemSorting sortBy, bool sortBackwards,
                                                          int startRecord, int endRecord,
                                                          out int totalRecords)
        {
            ValidateSecurity(entry);

            List<TableColumn> columns = new List<TableColumn>(vendorColumns);
            string sortCondition = ResolveSort(sortBy, sortBackwards);

            columns.Add(new TableColumn
            {
                ColumnName = $"ROW_NUMBER() OVER(ORDER BY {sortCondition})",
                ColumnAlias = "ROW"
            });
            columns.Add(new TableColumn
            {
                ColumnName = $"COUNT(1) OVER ( ORDER BY {sortCondition} RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )",
                ColumnAlias = "ROW_COUNT"
            });

            List<Condition> conditions = new List<Condition>
                {
                    new Condition {ConditionValue = " A.VENDORID = @VENDORID ", Operator = "AND"},
                    new Condition {ConditionValue = " B.DELETED = 0 ", Operator = "AND"},
                };

            List<Condition> externalConditions = new List<Condition>();
            externalConditions.Add(new Condition
            {
                Operator = "AND",
                ConditionValue = "S.ROW BETWEEN @ROWFROM AND @ROWTO"
            });

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("VENDORITEMS", "A", "S", 0, true),
                                                QueryPartGenerator.ExternalColumnGenerator(columns, "S"),
                                                QueryPartGenerator.InternalColumnGenerator(columns),
                                                QueryPartGenerator.JoinGenerator(listJoins),
                                                QueryPartGenerator.ConditionGenerator(conditions),
                                                QueryPartGenerator.ConditionGenerator(externalConditions),
                                                string.Empty);

                MakeParam(cmd, "VENDORID", (string)vendorID);
                MakeParam(cmd, "ROWFROM", startRecord, SqlDbType.Int);
                MakeParam(cmd, "ROWTO", endRecord, SqlDbType.Int);
                int matchingRecords = 0;

                var result = Execute<VendorItem, int>(entry, cmd, CommandType.Text, ref matchingRecords, PopulateVendorItemWithCount);
                totalRecords = matchingRecords;

                return result;
            }
        }

        /// <summary>
        /// Gets a vendor item with given retailItemId and vendorId with minimal data (no derrived descriptions).
        /// This function is should be used to get vendor from a given default vendor ID on a item.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="retailItemID">ID of the retail item</param>
        /// <param name="vendorID">ID of the vendor or default vendor</param>
        /// <returns>The requested vendor or null if not found</returns>
        public virtual VendorItem GetVendorForItem(IConnectionManager entry, RecordIdentifier retailItemID, RecordIdentifier vendorID)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> colmns = new List<TableColumn>
                {
                    new TableColumn{ColumnName = "INTERNALID"},
                    new TableColumn{ColumnName = "VENDORITEMID"},
                    new TableColumn{ColumnName = "RETAILITEMID"},
                    new TableColumn{ColumnName = "UNITID"},
                    new TableColumn{ColumnName = "VENDORID"},
                    new TableColumn{ColumnName = "ITEMPRICE"},
                    new TableColumn{ColumnName = "DEFAULTPURCHASEPRICE"},
                    new TableColumn{ColumnName = "LASTORDERDATE"},
                    new TableColumn{ColumnName = "VARIANTNAME", IsNull = true, NullValue = "''", ColumnAlias = "VARIANTNAME", TableAlias = "B"},
                };
                List<Condition> conditions = new List<Condition>
                {
                    new Condition {ConditionValue = " A.RETAILITEMID = @RETAILITEMID ", Operator = "AND"},
                    new Condition {ConditionValue = " A.VENDORID = @VENDORID ", Operator = "AND"},
                };

                List<Join> joins = new List<Join>
                {
                    new Join
                    {
                        Condition = "A.RETAILITEMID = B.ITEMID",
                        JoinType = "LEFT OUTER",
                        Table = "RETAILITEM",
                        TableAlias = "B"
                    }
                };

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("VENDORITEMS", "A"),
                    QueryPartGenerator.InternalColumnGenerator(colmns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );

                MakeParam(cmd, "RETAILITEMID", (string)retailItemID);
                MakeParam(cmd, "VENDORID", (string)vendorID);

                var result = Execute<VendorItem>(entry, cmd, CommandType.Text, PopulateMinimumVendorItem);

                return (result.Count > 0) ? result[0] : null;
            }


        }

        public virtual List<VendorItem> GetVendorsForItem(IConnectionManager entry, RecordIdentifier retailItemID, VendorItemSorting sortBy, bool sortBackwards)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>
                {
                    new Condition {ConditionValue = " A.RETAILITEMID = @RETAILITEMID ", Operator = "AND"},
                    new Condition {ConditionValue = " V.DELETED = 0 ", Operator = "AND"},
                };

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("VENDORITEMS", "A"),
                    QueryPartGenerator.InternalColumnGenerator(vendorColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "ORDER BY " + ResolveSort(sortBy, sortBackwards)
                    );

                MakeParam(cmd, "RETAILITEMID", (string)retailItemID);

                return Execute<VendorItem>(entry, cmd, CommandType.Text, PopulateVendorItem);
            }
        }

        /// <summary>
        /// Gets the latest purchase price of an item
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        /// <param name="itemID">The item we want the purchase price for</param>
        /// <param name="vendorID">ID of the vendor</param>
        /// <param name="unitID">The unit of the item we want the purchase price for</param>
        /// <returns>The default cost price of an item</returns>
        public virtual decimal GetDefaultPurchasePrice(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier vendorID, RecordIdentifier unitID)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"SELECT ISNULL(DEFAULTPURCHASEPRICE, 0) AS DEFAULTPURCHASEPRICE 
                                    FROM VENDORITEMS 
                                    WHERE RETAILITEMID = @ITEMID 
                                    AND VENDORID = @VENDORID 
                                    AND UNITID = @UNITID 
                                    AND DATAAREAID = @DATAAREAID ";

                MakeParam(cmd, "ITEMID", (string)itemID);
                MakeParam(cmd, "VENDORID", (string)vendorID);
                MakeParam(cmd, "UNITID", (string)unitID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                object result = entry.Connection.ExecuteScalar(cmd);

                if (result == null)
                {
                    return decimal.Zero;
                }

                return (decimal)result;
            }
        }

        public decimal GetLatestPurchasePrice(
            IConnectionManager entry,
            RecordIdentifier retailItemID,
            RecordIdentifier vendorID,
            RecordIdentifier unitID)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT ISNULL(MAX(ITEMPRICE), 0) " +
                                    "FROM VENDORITEMS " +
                                    "WHERE RETAILITEMID = @RETAILITEMID " +
                                    "AND UNITID = @UNITID " +
                                    "AND VENDORID = @VENDORID " +
                                    "AND DATAAREAID = @DATAAREAID " +
                                    "GROUP BY ITEMPRICE";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "RETAILITEMID", (string)retailItemID);
                MakeParam(cmd, "UNITID", (string)unitID);
                MakeParam(cmd, "VENDORID", (string)vendorID);

                object result = entry.Connection.ExecuteScalar(cmd);
                return (result != null) ? (decimal)entry.Connection.ExecuteScalar(cmd) : 0;
            }
        }

        /// <summary>
        /// Returns true if there's a link between the given vendorID and itemID in the VENDORITEMS table.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="retailItemID">Unique ID of the item</param>
        /// <param name="vendorID">Unique ID of the vendor</param>
        /// <returns></returns>
        public virtual bool VendorHasItem(IConnectionManager entry, RecordIdentifier retailItemID, RecordIdentifier vendorID)
        {
            var vi = GetVendorForItem(entry, retailItemID, vendorID);
            return vi != null;
        }

        /// <summary>
        /// Returns true if there's at least one item linked to the given vendorID in VENDORITEMS table.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorID">Unique ID for the vendor</param>
        /// <returns></returns>
        public virtual bool VendorHasItems(IConnectionManager entry, RecordIdentifier vendorID)
        {
            return RecordExists(entry, "VENDORITEMS", "VENDORID", vendorID);
        }

        /// <summary>
        /// Returns true if there is at least one item that has the given vendorID as default vendor.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="vendorID">The unique ID of the vendor</param>
        /// <returns></returns>
        public virtual bool VendorIsDefaultVendor(IConnectionManager entry, RecordIdentifier vendorID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);
                List<TableColumn> columns = new List<TableColumn>();
                columns.Add(
                    new TableColumn { ColumnName = "INTERNALID", TableAlias = "A" }
                    );

                List<Join> joins = new List<Join>();
                joins.Add(
                    new Join
                    {
                        Condition = " RI.DEFAULTVENDORID = A.INTERNALID",
                        JoinType = "INNER",
                        Table = "RETAILITEM",
                        TableAlias = "RI"
                    });

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.VENDORID = @VENDORID" });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("VENDORITEMS", "A"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

                MakeParam(cmd, "VENDORID", (string)vendorID);

                var links = Execute(entry, cmd, CommandType.Text, "INTERNALID");
                return links != null && links.Count > 0;
            }
        }

        /// <summary>
        /// Checks if a vendor item by a given ID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="internalID">ID of the vendor item to check for</param>
        /// <returns>True if the vendor item exists, else false</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier internalID)
        {
            return RecordExists(entry, "VENDORITEMS", "INTERNALID", internalID);
        }

        /// <summary>
        /// Checks if a vendor item by a given Vendor ID and vendor item ID exists, excluding current record from the check
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorID">ID of the vendor to check for</param>
        /// <param name="vendorItemID">ID of the vendor item to check for (this is external vendor specific id)</param>
        /// <param name="oldRecordID">ID of the old record which we skip comparing to</param>
        /// <returns>True if the vendor item exists, else false</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier vendorID, RecordIdentifier vendorItemID, RecordIdentifier oldRecordID)
        {
            return RecordExists(entry, "VENDORITEMS",
                new[] { "VENDORID", "VENDORITEMID" },
                new RecordIdentifier(vendorID, vendorItemID),
                new[] { "INTERNALID" },
                oldRecordID);
        }

        /// <summary>
        /// Checks if a vendor item by a given Vendor ID, retailID, variantID and unitID, excluding current record from the check
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorID">ID of the vendor to check for</param>
        /// <param name="retaiID">ID of the retail item</param>
        /// <param name="unitID">ID of the variant</param>
        /// <param name="oldRecordID">ID of the old record which we skip comparing to</param>
        /// <returns>True if the vendor item exists, else false</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier vendorID, RecordIdentifier retaiID, RecordIdentifier unitID, RecordIdentifier oldRecordID)
        {
            return RecordExists(entry, "VENDORITEMS",
                new[] { "VENDORID", "RETAILITEMID", "UNITID" },
                new RecordIdentifier(vendorID, new RecordIdentifier(retaiID, unitID)),
                new[] { "INTERNALID" },
                oldRecordID);
        }

        /// <summary>
        /// Checks if a vendor item by a given Vendor ID, retailID, variantID and unitID, excluding current record from the check
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorID">ID of the vendor to check for</param>
        /// <param name="retaiID">ID of the retail item</param>
        /// <param name="unitID">ID of the variant</param>
        /// <param name="vendorItemID"></param>
        /// <param name="oldRecordID">ID of the old record which we skip comparing to</param>
        /// <returns>True if the vendor item exists, else false</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier vendorID, RecordIdentifier retaiID, RecordIdentifier unitID, RecordIdentifier vendorItemID, RecordIdentifier oldRecordID)
        {
            return RecordExists(entry, "VENDORITEMS",
                new[] { "VENDORID", "RETAILITEMID", "UNITID", "VENDORITEMID" },
                new RecordIdentifier(vendorID, new RecordIdentifier(retaiID, unitID, vendorItemID)),
                new[] { "INTERNALID" },
                oldRecordID);
        }

        /// <summary>
        /// Deletes a vendor item by a given ID.
        /// </summary>
        /// <remarks>Edit vendor permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="internalID">The ID of the vendor item to be deleted</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier internalID)
        {
            DeleteRecord(entry, "VENDORITEMS", "INTERNALID", internalID, BusinessObjects.Permission.VendorEdit);
        }

        /// <summary>
        /// Deletes all vendor items for the given retail item ID.
        /// </summary>
        /// <remarks>Edit vendor permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="retailItemID">The ID of the retail item</param>
        /// <param name="vendorID">The ID of the vendor</param>
        public virtual void DeleteByRetailItemID(IConnectionManager entry, RecordIdentifier retailItemID, RecordIdentifier vendorID)
        {
            DeleteRecord(entry, "VENDORITEMS", new[] { "RETAILITEMID", "VENDORID" }, new RecordIdentifier(retailItemID, vendorID), Permission.VendorEdit);
        }

        /// <summary>
        /// Deletes all vendor items for the given retail item ID.
        /// </summary>
        /// <remarks>Edit vendor permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="retailItemID">The ID of the retail item</param>
        /// <param name="vendorID">The ID of the vendor</param>
        /// <param name="unitID">The ID of the unit</param>
        public virtual void DeleteByRetailItemID(IConnectionManager entry, RecordIdentifier retailItemID, RecordIdentifier vendorID, RecordIdentifier unitID)
        {
            DeleteRecord(entry, "VENDORITEMS", new[] { "RETAILITEMID", "VENDORID", "UNITID" }, new RecordIdentifier(retailItemID, vendorID, unitID), Permission.VendorEdit);
        }

        /// <summary>
        /// Saves a vendor item to the database. If the record does not exist then a Insert is done, else it executes a update statement.
        /// </summary>
        /// <remarks>Edit vendor permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorItem">The vendor item to be saved</param>
        public virtual void Save(IConnectionManager entry, VendorItem vendorItem)
        {
            SqlServerStatement statement = new SqlServerStatement("VENDORITEMS");

            ValidateSecurity(entry, BusinessObjects.Permission.VendorEdit);

            if (vendorItem.ID == RecordIdentifier.Empty)
            {
                vendorItem.ID = DataProviderFactory.Instance.GenerateNumber<IVendorItemData, VendorItem>(entry);
            }

            vendorItem.Validate();

            if (!Exists(entry, vendorItem.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("INTERNALID", (string)vendorItem.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("INTERNALID", (string)vendorItem.ID);
            }

            statement.AddField("VENDORITEMID", (string)vendorItem.VendorItemID);
            statement.AddField("RETAILITEMID", (string)vendorItem.RetailItemID);
            statement.AddField("UNITID", (string)vendorItem.UnitID);
            statement.AddField("VENDORID", (string)vendorItem.VendorID);
            statement.AddField("ITEMPRICE", vendorItem.LastItemPrice, SqlDbType.Decimal);
            statement.AddField("DEFAULTPURCHASEPRICE", vendorItem.DefaultPurchasePrice, SqlDbType.Decimal);
            statement.AddField("LASTORDERDATE", vendorItem.LastOrderDate.ToAxaptaSQLDate(), SqlDbType.DateTime);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Searches the vendor item for the given criteria
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchCriteria">Criterias for search</param>
        /// <param name="sortBy">Sort field for returned vendor items</param>
        /// <param name="sortBackwards">Sort direction of the returned vendor items</param>
        /// <param name="startRecord">Pagination start index</param>
        /// <param name="endRecord">Pagination end index</param>
        /// <param name="totalRecords">Total number of records that match the search criteria</param>
        /// <returns></returns>
        public List<VendorItem> AdvancedSearch(IConnectionManager entry, VendorItemSearch searchCriteria,
                                        VendorItemSorting sortBy, bool sortBackwards,
                                        int startRecord, int endRecord,
                                        out int totalRecords)
        {
            ValidateSecurity(entry);

            List<TableColumn> columns = new List<TableColumn>(vendorColumns);
            string sortCondition = ResolveSort(sortBy, sortBackwards);

            columns.Add(new TableColumn
            {
                ColumnName = $"ROW_NUMBER() OVER(ORDER BY {sortCondition})",
                ColumnAlias = "ROW"
            });
            columns.Add(new TableColumn
            {
                ColumnName = $"COUNT(1) OVER ( ORDER BY {sortCondition} RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )",
                ColumnAlias = "ROW_COUNT"
            });

            List<Condition> conditions = new List<Condition>();
            conditions.Add(new Condition { ConditionValue = " B.DELETED = 0 ", Operator = "AND" });

            List<Condition> externalConditions = new List<Condition>();
            externalConditions.Add(new Condition
            {
                Operator = "AND",
                ConditionValue = "S.ROW BETWEEN @ROWFROM AND @ROWTO"
            });

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                if (searchCriteria != null)
                {
                    if(!RecordIdentifier.IsEmptyOrNull(searchCriteria.VendorID))
                    {
                        conditions.Add(new Condition { ConditionValue = " A.VENDORID = @VENDORID ", Operator = "AND" });
                        MakeParam(cmd, "VENDORID", (string)searchCriteria.VendorID);
                    }

                    if(!RecordIdentifier.IsEmptyOrNull(searchCriteria.UnitID))
                    {
                        conditions.Add(new Condition { Operator = "AND", ConditionValue = "U.UNITID = @UNITID" });
                        MakeParam(cmd, "UNITID", (string)searchCriteria.UnitID);
                    }

                    if (searchCriteria.LastOrderingDateFrom != Date.Empty || searchCriteria.LastOrderingDateTo != Date.Empty)
                    {
                        conditions.Add(new Condition
                        {
                            Operator = "AND",
                            ConditionValue = $"A.LASTORDERDATE BETWEEN @ORDERDATEFROM AND @ORDERDATETO "
                        });

                        MakeParam(cmd, "ORDERDATEFROM", searchCriteria.LastOrderingDateFrom.DateTime, SqlDbType.DateTime);
                        MakeParam(cmd, "ORDERDATETO", searchCriteria.LastOrderingDateTo.DateTime, SqlDbType.DateTime);
                    }

                    if (searchCriteria.Description != null && searchCriteria.Description.Count > 0)
                    {
                        List<Condition> searchConditions = new List<Condition>();
                        for (int index = 0; index < searchCriteria.Description.Count; index++)
                        {
                            if (string.IsNullOrWhiteSpace(searchCriteria.Description[index]))
                                continue;

                            var searchToken = PreProcessSearchText(searchCriteria.Description[index], true, searchCriteria.DescriptionBeginsWith);
                            if (!string.IsNullOrWhiteSpace(searchToken))
                            {
                                searchConditions.Add(new Condition
                                {
                                    ConditionValue =
                                        $@" (B.ITEMNAME LIKE @DESCRIPTION{index
                                            } 
                                        OR A.RETAILITEMID LIKE @DESCRIPTION{index
                                            } 
                                        OR B.VARIANTNAME LIKE @DESCRIPTION{
                                            index
                                            } 
                                        OR A.VENDORITEMID LIKE @DESCRIPTION{
                                            index}) ",
                                    Operator = "AND"

                                });

                                MakeParam(cmd, $"DESCRIPTION{index}", searchToken);
                            }
                        }
                        conditions.Add(new Condition
                        {
                            Operator = "AND",
                            ConditionValue =
                                $" ({QueryPartGenerator.ConditionGenerator(searchConditions, true)}) "
                        });
                    }
                }

                string sortstring = ResolveSort(sortBy, sortBackwards, externalColumn: true);

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("VENDORITEMS", "A", "S", 0, true),
                                            QueryPartGenerator.ExternalColumnGenerator(columns, "S"),
                                            QueryPartGenerator.InternalColumnGenerator(columns),
                                            QueryPartGenerator.JoinGenerator(listJoins),
                                            QueryPartGenerator.ConditionGenerator(conditions),
                                            QueryPartGenerator.ConditionGenerator(externalConditions),
                                            string.Format("ORDER BY {0}", sortstring));

                MakeParam(cmd, "ROWFROM", startRecord, SqlDbType.Int);
                MakeParam(cmd, "ROWTO", endRecord, SqlDbType.Int);
                int matchingRecords = 0;

                var result = Execute<VendorItem, int>(entry, cmd, CommandType.Text, ref matchingRecords, PopulateVendorItemWithCount);
                totalRecords = matchingRecords;

                return result;
            }
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "VendorItems"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "VENDORITEMS", "INTERNALID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
