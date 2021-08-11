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

namespace LSOne.DataLayer.SqlDataProviders.Inventory
{
    public partial class InventoryData : SqlServerDataProviderBase, IInventoryData
    {

        private static List<TableColumn> inventoryColumns = new List<TableColumn>
        {

            new TableColumn {ColumnName = "ITEMID " , TableAlias = "IT"},
            new TableColumn {ColumnName = "ITEMNAME " , TableAlias = "IT"},
            new TableColumn {ColumnName = "VARIANTNAME " , TableAlias = "IT"},
            new TableColumn {ColumnName = "STOREID " , TableAlias = "RST"},
            new TableColumn {ColumnName = "NAME " , TableAlias = "RST", ColumnAlias = "STORENAME"},

            new TableColumn {ColumnName = "ISNULL(INS.QUANTITY,0)" , ColumnAlias = "QUANTITY"},
            new TableColumn {ColumnName = "ISNULL(U.MINUNITDECIMALS,0)" , ColumnAlias = "MINUNITDECIMALS"},
            new TableColumn {ColumnName = "ISNULL(U.UNITDECIMALS,0)" , ColumnAlias = "MAXUNITDECIMALS"},
            new TableColumn {ColumnName = "ISNULL(U.UNITID,'')" , ColumnAlias = "UNITID"},
            new TableColumn {ColumnName = "ISNULL(U.TXT,'')" , ColumnAlias = "UNITTEXT"},

        };

        private static List<Join> listJoins = new List<Join>
        {
            new Join
            {
                JoinType = "CROSS",
                Table = "RBOSTORETABLE",
                TableAlias = "RST"
            },
            new Join
            {
                Condition = "INS.ITEMID = IT.ITEMID AND INS.STOREID = RST.STOREID",
                JoinType = "LEFT OUTER",
                Table = "VINVENTSUM",
                TableAlias = "INS"
            },
            new Join
            {
                Condition = " IT.INVENTORYUNITID = U.UNITID ",
                JoinType = "LEFT OUTER",
                Table = "UNIT",
                TableAlias = "U"
            },
            
        };

        private static string ResolveSort(InventorySorting sort, bool sortBackwards)
        {
            string sortString = ""; 
            switch (sort)
            {
                case InventorySorting.Quantity:
                    sortString = " ORDER BY QUANTITY";
                    break;
                case InventorySorting.Store:
                    sortString = " ORDER BY RST.NAME";
                    break;
             
            }

            if (sortBackwards) 
            { 
                sortString = sortString + " DESC"; 
            }
            else
            {
                sortString = sortString + " ASC";
            }
                
            return sortString;
        }       
        

        private static void PopulateInventoryStatus(IDataReader dr, InventoryStatus inventoryStatus)
        {
            inventoryStatus.ItemID = (string)dr["ITEMID"];
            inventoryStatus.ItemName = (string)dr["ITEMNAME"];
            inventoryStatus.InventoryQuantity = (decimal)dr["QUANTITY"];
            inventoryStatus.StoreID = (string)dr["STOREID"];
            inventoryStatus.StoreName = (string)dr["STORENAME"];
            inventoryStatus.VariantName = (string)dr["VARIANTNAME"];
            inventoryStatus.InventoryUnitId = (string)dr["UNITID"];
            inventoryStatus.InventoryUnitDescription = (string)dr["UNITTEXT"];

            var minUnitDecimals = (int)dr["MINUNITDECIMALS"];
            var maxUnitDecimals = (int)dr["MAXUNITDECIMALS"];
            inventoryStatus.QuantityLimiter = new DecimalLimit(minUnitDecimals, maxUnitDecimals);
        }

        protected virtual void PopulateInventoryStatusWithCount(IConnectionManager entry, IDataReader dr,
        InventoryStatus inventoryStatus, ref int rowCount)
        {
            PopulateInventoryStatus(dr, inventoryStatus);
            rowCount = (int)dr["Row_Count"];

        }

        /// <summary>
        /// Gets inventory status for an item in a store (or all stores)
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The item's ID</param>
        /// <param name="storeID">The store's ID. Note that if this is RecordIdentifier.Empty then results for all stores will be returned</param>
        /// <param name="sort">Sort enum that determines in which order the results will be returned</param>
        /// <param name="backwardsSort">Results will be returned in backwards order</param>
        /// <returns>Inventory status for an item in a store (or all stores)</returns>
        public List<InventoryStatus> GetInventoryListForItemAndStore(
            IConnectionManager entry,
            RecordIdentifier itemID,
            RecordIdentifier storeID,
            RecordIdentifier regionID,
            InventorySorting sort,
            bool backwardsSort)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "IT.ITEMID = @ITEMID " });

                if (!RecordIdentifier.IsEmptyOrNull(storeID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = " RST.STOREID = @STOREID " });
                    MakeParam(cmd, "STOREID", storeID);
                }
                else if (!RecordIdentifier.IsEmptyOrNull(regionID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = " RST.REGIONID = @REGIONID " });
                    MakeParam(cmd, "REGIONID", (string)regionID);
                }

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RETAILITEM", "IT"),
                    QueryPartGenerator.InternalColumnGenerator(inventoryColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    sort != InventorySorting.Ordered
                        ? ResolveSort(sort, backwardsSort)
                        : string.Empty
                    );

                MakeParam(cmd, "ITEMID", (string)itemID);

                return Execute<InventoryStatus>(entry, cmd, CommandType.Text, PopulateInventoryStatus);
            }
        }

        /// <summary>
        /// Gets inventory status for all items in a store (or all stores)
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The store's ID. Note that if this is RecordIdentifier.Empty then results for all stores will be returned</param>
        /// <param name="sort">Sort enum that determines in which order the results will be returned</param>
        /// <param name="backwardsSort">Results will be returned in backwards order</param>
        /// <returns>Inventory status for an item in a store (or all stores)</returns>
        public List<InventoryStatus> GetInventoryListForStore(
            IConnectionManager entry,
            RecordIdentifier storeID,
            InventorySorting sort,
            bool backwardsSort)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();

                if (!(storeID == RecordIdentifier.Empty || storeID == ""))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = " RST.STOREID = @STOREID " });
                    MakeParam(cmd, "STOREID", (string)storeID);

                }

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RETAILITEM", "IT"),
                    QueryPartGenerator.InternalColumnGenerator(inventoryColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    ResolveSort(sort, backwardsSort));

                return Execute<InventoryStatus>(entry, cmd, CommandType.Text, PopulateInventoryStatus);
            }
        }

        /// <summary>
        /// Gets inventory status for all items in a store (or all stores)
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The store's ID. Note that if this is RecordIdentifier.Empty then results for all stores will be returned</param>
        /// <param name="sort">Sort enum that determines in which order the results will be returned</param>
        /// <param name="backwardsSort">Results will be returned in backwards order</param>
        /// <param name="rowFrom">Start index</param>
        /// <param name="rowTo">End index</param>
        /// <param name="total">Total number of records</param>
        /// <returns>Inventory status for an item in a store (or all stores)</returns>
        public List<InventoryStatus> GetInventoryListForStore(
            IConnectionManager entry,
            RecordIdentifier storeID,
            InventorySorting sort,
            bool backwardsSort,
            int rowFrom,
            int rowTo,
            out int total)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                List<TableColumn> columns = new List<TableColumn>(inventoryColumns);

                List<Condition> externalConditions = new List<Condition>();

                columns.Add(new TableColumn
                {
                    ColumnName = $"ROW_NUMBER() OVER( { ResolveSort(sort, backwardsSort)} )",
                    ColumnAlias = "ROW"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = $"COUNT(1) OVER ( { ResolveSort(sort, backwardsSort)} RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )",

                    ColumnAlias = "ROW_COUNT"
                });
                externalConditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "ss.ROW BETWEEN @ROWFROM AND @ROWTO"
                });
                MakeParam(cmd, "ROWFROM", rowFrom, SqlDbType.Int);
                MakeParam(cmd, "ROWTO", rowTo, SqlDbType.Int);

                if (!(storeID == RecordIdentifier.Empty || storeID == ""))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = " RST.STOREID = @STOREID " });
                    MakeParam(cmd, "STOREID", (string)storeID);

                }

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RETAILITEM", "IT", "ss"),
                       QueryPartGenerator.ExternalColumnGenerator(columns, "ss"),
                       QueryPartGenerator.InternalColumnGenerator(columns),
                       QueryPartGenerator.JoinGenerator(listJoins),
                       QueryPartGenerator.ConditionGenerator(conditions),
                       QueryPartGenerator.ConditionGenerator(externalConditions),
                       "");

                total = 0;

                return Execute<InventoryStatus, int>(entry, cmd, CommandType.Text, ref total, PopulateInventoryStatusWithCount);
            }
        }

        public virtual decimal GetInventoryOnHand(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "IT.ITEMID = @ITEMID " });

                if (!(storeID == RecordIdentifier.Empty || storeID == ""))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = " RST.STOREID = @STOREID " });
                    MakeParam(cmd, "STOREID", storeID);
                }

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RETAILITEM", "IT"),
                    QueryPartGenerator.InternalColumnGenerator(inventoryColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );

                MakeParam(cmd, "ITEMID", (string)itemID);

                var listOfStatuses = Execute<InventoryStatus>(entry, cmd, CommandType.Text, PopulateInventoryStatus);

                return (listOfStatuses.Count > 0) ? listOfStatuses[0].InventoryQuantity : 0;
            }
        }

        public virtual void UpdateInventoryUnit(IConnectionManager entry, RecordIdentifier itemID, decimal conversionFactor)
        {
            var inventoryList = GetInventoryStatusesForItem(entry, itemID, RecordIdentifier.Empty);

            foreach (var inventoryStatus in inventoryList)
            {
                inventoryStatus.InventoryQuantity /= conversionFactor;

                #pragma warning disable 0612, 0618
                Save(entry, inventoryStatus);
                #pragma warning restore 0612, 0618
            }
        }

        public virtual List<InventoryStatus> GetInventoryStatusesForItem(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier regionID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "IT.ITEMID = @ITEMID " });

                if (!RecordIdentifier.IsEmptyOrNull(regionID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "RST.REGIONID = @REGIONID" });
                    MakeParam(cmd, "REGIONID", (string)regionID);
                }

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RETAILITEM", "IT"),
                    QueryPartGenerator.InternalColumnGenerator(inventoryColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    " ORDER BY RST.STOREID "
                    );

                MakeParam(cmd, "ITEMID", (string)itemID);

                return Execute<InventoryStatus>(entry, cmd, CommandType.Text, PopulateInventoryStatus);
            }
        }
        
        [Obsolete("This function should only be used after careful consideration because the INVENTTRANS table should be responsible for updating INVENTSUM")]
        private void Save(IConnectionManager entry, InventoryStatus inventoryStatus)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.EditInventoryAdjustments);

            var statement = new SqlServerStatement("INVENTSUM");

            if (!Exists(entry, inventoryStatus.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("ITEMID", (string)inventoryStatus.ItemID);
                statement.AddKey("STOREID", (string)inventoryStatus.StoreID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ITEMID", (string)inventoryStatus.ItemID);
                statement.AddCondition("STOREID", (string)inventoryStatus.StoreID);
            }

            statement.AddField("QUANTITY", inventoryStatus.InventoryQuantity, SqlDbType.Decimal);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "INVENTSUM", new[]{"ITEMID", "STOREID"}, id,false);
        }

        /// <summary>
        /// Gets inventory statuses for items in the selected inventory group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The ID of the store to get inventory status for. RecordIdentifier.Empty and you get statuses for all stores</param>
        /// <param name="inventoryGroup">By which group do we wish to filter on</param>
        /// <param name="inventoryGroupID">The ID of the group to filter on</param>
        /// <returns>Inventory statues for items in the selected inventory group</returns>
        public List<InventoryStatus> GetInventoryStatuses(
            IConnectionManager entry,
            RecordIdentifier storeID,
            InventoryGroup inventoryGroup,
            RecordIdentifier inventoryGroupID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();

                if (!(storeID == RecordIdentifier.Empty || storeID == ""))
                {
                    conditions.Add(new Condition {Operator = "AND", ConditionValue = " RST.STOREID = @STOREID "});
                    MakeParam(cmd, "STOREID", storeID);

                }
                List<Join> joins = new List<Join>(listJoins);
               

                switch (inventoryGroup)
                {
                    case InventoryGroup.RetailGroup:
                        conditions.Add(new Condition
                        {
                            Operator = "AND",
                            ConditionValue = "RG.GROUPID = @INVENTORYGROUPID "
                        });
                        joins.Add(new Join
                        {
                            Condition = "RG.MASTERID = IT.RETAILGROUPMASTERID",
                            Table = "RETAILGROUP",
                            JoinType = "LEFT OUTER",
                            TableAlias = "RG"});
                        break;
                    case InventoryGroup.RetailDepartment:
                        conditions.Add(new Condition
                        {
                            Operator = "AND",
                            ConditionValue = "RD.DEPARTMENTID = @INVENTORYGROUPID "
                        });
                        joins.Add(new Join
                        {
                            Condition = "RG.MASTERID = IT.RETAILGROUPMASTERID",
                            Table = "RETAILGROUP",
                            JoinType = "LEFT OUTER",
                            TableAlias = "RG"
                        });
                        joins.Add(new Join
                        {
                            Condition = "RD.MASTERID = RG.DEPARTMENTMASTERID",
                            Table = "RETAILDEPARTMENT",
                            JoinType = "LEFT OUTER",
                            TableAlias = "RD"
                        });
                        break;
                    case InventoryGroup.Vendor:
                        conditions.Add(new Condition
                        {
                            Operator = "AND",
                            ConditionValue = "VI.VENDORID = @INVENTORYGROUPID "
                        });
                        joins.Add(new Join
                        {
                            Condition = "VI.RETAILITEMID = IT.ITEMID",
                            Table = "VENDORITEMS",
                            JoinType = "LEFT OUTER",
                            TableAlias = "VI"
                        });
                        break;
                }

                MakeParam(cmd, "INVENTORYGROUPID", inventoryGroupID);

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RETAILITEM", "IT"),
                    QueryPartGenerator.InternalColumnGenerator(inventoryColumns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );

                return Execute<InventoryStatus>(entry, cmd, CommandType.Text, PopulateInventoryStatus);
            }
        }

        public virtual RecordIdentifier GetInventoryUnitId(IConnectionManager entry, RecordIdentifier itemId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "IT.ITEMID = @ITEMID " });

                

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RETAILITEM", "IT"),
                    QueryPartGenerator.InternalColumnGenerator(inventoryColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );
                
                MakeParam(cmd, "ITEMID", (string)itemId);

                var list = Execute<InventoryStatus>(entry, cmd, CommandType.Text, PopulateInventoryStatus);

                return (list.Count > 0) ? list[0].InventoryUnitId : "";
            }
        }
    }
}
