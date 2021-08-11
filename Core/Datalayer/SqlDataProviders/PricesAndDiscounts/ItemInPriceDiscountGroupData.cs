using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;
using Permission = LSOne.DataLayer.BusinessObjects.Permission;

namespace LSOne.DataLayer.SqlDataProviders.PricesAndDiscounts
{
    public class ItemInPriceDiscountGroupData : SqlServerDataProviderBase, IItemInPriceDiscountGroupData
    {
        private static void PopulateItem(IDataReader dr, ItemInPriceDiscountGroup item)
        {
            item.ID = (string)dr["ITEMID"];
            item.Text = (string)dr["ITEMNAME"];
            item.PriceDiscountGroup = (string)dr["GROUPNAME"];
        }
        private static void PopulateMinimalItem(IDataReader dr, ItemInPriceDiscountGroup item)
        {
            item.ID = (string)dr["ITEMID"];
            item.Text = (string)dr["ITEMNAME"];
            item.VariantName = (string)dr["VARIANTNAME"];
        }
        protected virtual void PopulateMinimalItemWithCount(IConnectionManager entry, IDataReader dr, ItemInPriceDiscountGroup item,
     ref int rowCount)
        {
            PopulateMinimalItem(dr, item);

            PopulateRowCount(entry, dr, ref rowCount);

        }
        protected virtual void PopulateRowCount(IConnectionManager entry, IDataReader dr, ref int rowCount)
        {
            if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 ||
                entry.Connection.DatabaseVersion == ServerVersion.Unknown)
            {
                rowCount = (int)dr["ROW_COUNT"];
            }
        }
        public List<ItemInPriceDiscountGroup> GetItemList(IConnectionManager entry,
            PriceDiscGroupEnum type,
            RecordIdentifier discountGroup,
            int? RecordFrom,
            int? RecordTo,
            out int count)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "ITEMID", TableAlias = "RI"},
                    new TableColumn {ColumnName = "ITEMNAME", TableAlias = "RI"},
                    new TableColumn {ColumnName = "VARIANTNAME", TableAlias = "RI"},
                    new TableColumn {ColumnName = "COUNT(1) OVER ( ORDER BY  RI.ITEMID RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING ) ", ColumnAlias = "ROW_COUNT"}
                };
               
                List<Condition> joinConditions = new List<Condition>
                {
                    new Condition{ConditionValue = "PDG.MODULE = 0", Operator = "AND"},
                    new Condition{ConditionValue = "PDG.TYPE = @type", Operator = "AND"},
                    new Condition{ConditionValue = "PDG.GROUPID = @discountGroup", Operator = "AND"}
                };
                switch (type)
                {
                    case PriceDiscGroupEnum.LineDiscountGroup:
                        joinConditions.Add(new Condition
                        {
                            ConditionValue = "PDG.GROUPID = RI.SALESLINEDISC",
                            Operator = "AND"
                        });
                        break;
                    case PriceDiscGroupEnum.MultilineDiscountGroup:
                        joinConditions.Add(new Condition
                        {
                            ConditionValue = "pdg.GROUPID = RI.SALESMULTILINEDISC",
                            Operator = "AND"
                        });
                        break;
                    default:
                        count = 0;
                        return new List<ItemInPriceDiscountGroup>();
                }

                Join join = new Join
                {
                    Table = "PRICEDISCGROUP",
                    Condition = QueryPartGenerator.ConditionGenerator(joinConditions,true),
                    TableAlias = "PDG"
                };
                if (RecordFrom.HasValue && RecordTo.HasValue)
                {
                    Condition pagingCondition = new Condition
                    {
                        ConditionValue = " ITEMS.ROWNR between @recordFrom and @recordTo",
                        Operator = "AND"
                    };
                    columns.Add(new TableColumn
                    {
                        ColumnAlias = "ROWNR",
                        ColumnName = "ROW_NUMBER() OVER(ORDER BY RI.ITEMNAME) "
                    });
                    cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RETAILITEM", "RI", "ITEMS"),
                        QueryPartGenerator.ExternalColumnGenerator(columns, "ITEMS"),
                        QueryPartGenerator.InternalColumnGenerator(columns),
                        join,
                        string.Empty,
                        QueryPartGenerator.ConditionGenerator(pagingCondition),
                        string.Empty
                        );

                    MakeParam(cmd, "recordFrom", RecordFrom.Value, SqlDbType.Int);
                    MakeParam(cmd, "recordTo", RecordTo.Value, SqlDbType.Int);
                }
                else
                {
                    cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "RI"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    join,
                    string.Empty,
                    string.Empty);
                }
               
                MakeParam(cmd, "type", (int) type, SqlDbType.Int);
                MakeParam(cmd, "discountGroup", (string) discountGroup);
                
                count = 0;
                var result = Execute<ItemInPriceDiscountGroup,int>(entry, cmd, CommandType.Text, ref count, PopulateMinimalItemWithCount);

                return result;
            }
        }
        public virtual List<ItemInPriceDiscountGroup> SearchItemsNotInGroup(IConnectionManager entry, string searchText, int numberOfRecords, int type, string excludedGroupId)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "ITEMID", TableAlias = "RI"},
                    new TableColumn {ColumnName = "ITEMNAME", TableAlias = "RI"},
                    new TableColumn {ColumnName = "ISNULL(pdg.NAME,'')", ColumnAlias = "GROUPNAME"},
                    new TableColumn {ColumnName = "ISNULL(pdg.GROUPID,'')", ColumnAlias = "GROUPID"},
                };
                List<Condition> joinConditions = new List<Condition>
                {
                    new Condition{ConditionValue = "PDG.MODULE = 0", Operator = "AND"},
                    new Condition{ConditionValue = "PDG.TYPE = @type", Operator = "AND"}
                };
                List<Condition> conditions = new List<Condition>
                {
                    new Condition{ConditionValue = "(RI.ITEMID like @searchString or RI.ITEMNAME like @searchString) ", Operator = "AND"}

                };
                switch (type)
                {
                    case 1:
                        joinConditions.Add(new Condition
                        {
                            ConditionValue = "PDG.GROUPID = RI.SALESLINEDISC",
                            Operator = "AND"
                        });
                        conditions.Add(new Condition
                        {
                            ConditionValue = "RI.SALESLINEDISC <>  @excludedGroupId",
                            Operator = "AND"
                        });
                        break;
                    case 2:
                        joinConditions.Add(new Condition
                        {
                            ConditionValue = "pdg.GROUPID = RI.SALESMULTILINEDISC",
                            Operator = "AND"
                        });
                        conditions.Add(new Condition
                        {
                            ConditionValue = "RI.SALESMULTILINEDISC <>  @excludedGroupId",
                            Operator = "AND"
                        });
                        break;
                    default:
                        return new List<ItemInPriceDiscountGroup>();
                }

                Join join = new Join
                {
                    Table = "PRICEDISCGROUP",
                    Condition = QueryPartGenerator.ConditionGenerator(joinConditions, true),
                    TableAlias = "PDG",
                    JoinType = "LEFT OUTER"
                };
                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "RI"),
                   QueryPartGenerator.InternalColumnGenerator(columns),
                   join,
                   QueryPartGenerator.ConditionGenerator(conditions),
                   "Order by ITEMNAME ");

                MakeParam(cmd, "type", type, SqlDbType.Int);
                MakeParam(cmd, "excludedGroupId", excludedGroupId);
                MakeParam(cmd, "searchString", PreProcessSearchText(searchText, false, false) + "%");


                return Execute<ItemInPriceDiscountGroup>(entry, cmd, CommandType.Text, PopulateItem);
            }
        }



        public virtual bool ItemIsInGroup(IConnectionManager entry, RecordIdentifier itemID, int type, string excludedGroupId)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "ITEMID", TableAlias = "RI"},
                    new TableColumn {ColumnName = "ITEMNAME", TableAlias = "RI"},
                    new TableColumn {ColumnName = "ISNULL(pdg.NAME,'')", ColumnAlias = "GROUPNAME"},
                    new TableColumn {ColumnName = "ISNULL(pdg.GROUPID,'')", ColumnAlias = "GROUPID"},
                };
                List<Condition> joinConditions = new List<Condition>
                {
                    new Condition{ConditionValue = "PDG.MODULE = 0", Operator = "AND"},
                    new Condition{ConditionValue = "PDG.TYPE = @type", Operator = "AND"}
                };
                List<Condition> conditions = new List<Condition>();

                if (itemID.IsGuid)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "RI.MASTERID = @itemID" });
                    MakeParam(cmd, "itemID", (Guid)itemID, SqlDbType.UniqueIdentifier);
                }
                else
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "RI.ITEMID = @itemID" });
                    MakeParam(cmd, "itemID", (string)itemID);
                }
                switch (type)
                {
                    case 1:
                        joinConditions.Add(new Condition
                        {
                            ConditionValue = "PDG.GROUPID = RI.SALESLINEDISC",
                            Operator = "AND"
                        });
                        conditions.Add(new Condition
                        {
                            ConditionValue = "RI.SALESLINEDISC =  @excludedGroupId",
                            Operator = "AND"
                        });
                        break;
                    case 2:
                        joinConditions.Add(new Condition
                        {
                            ConditionValue = "pdg.GROUPID = RI.SALESMULTILINEDISC",
                            Operator = "AND"
                        });
                        conditions.Add(new Condition
                        {
                            ConditionValue = "RI.SALESMULTILINEDISC =  @excludedGroupId",
                            Operator = "AND"
                        });
                        break;
                    default:
                        return true;
                }

                Join join = new Join
                {
                    Table = "PRICEDISCGROUP",
                    Condition = QueryPartGenerator.ConditionGenerator(joinConditions, true),
                    TableAlias = "PDG",
                    JoinType = "LEFT OUTER"
                };
                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "RI"),
                   QueryPartGenerator.InternalColumnGenerator(columns),
                   join,
                   QueryPartGenerator.ConditionGenerator(conditions),
                   "Order by ITEMNAME ");

                MakeParam(cmd, "type", type, SqlDbType.Int);
                MakeParam(cmd, "excludedGroupId", excludedGroupId);


                var rows =  Execute<ItemInPriceDiscountGroup>(entry, cmd, CommandType.Text, PopulateItem);
                return rows.Count > 0;
            }
        }


        private static bool ItemExists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "RETAILITEM", "ITEMID",id,false);
        }

        public virtual void RemoveItemFromGroup(IConnectionManager entry, RecordIdentifier itemId, PriceDiscGroupEnum type)
        {
            ValidateSecurity(entry, Permission.ItemsEdit);

            var masterId = GetMasterID(entry, itemId, "RETAILITEM", "ITEMID");
            SqlServerStatement statement = new SqlServerStatement("RETAILITEM");

            string groupFieldinDB = "";

            switch (type)
            {
                case PriceDiscGroupEnum.LineDiscountGroup:
                    groupFieldinDB = "SALESLINEDISC";
                    break;
                case PriceDiscGroupEnum.MultilineDiscountGroup:
                    groupFieldinDB = "SALESMULTILINEDISC";
                    break;
                default:
                    return;
            }

            if (ItemExists(entry, itemId))
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("MASTERID", (Guid)masterId, SqlDbType.UniqueIdentifier);
                statement.AddField(groupFieldinDB, "");
                
                entry.Connection.ExecuteStatement(statement);

            }
        }

        public virtual void RemoveAllItemsFromGroup(IConnectionManager entry, RecordIdentifier groupId, PriceDiscGroupEnum type)
        {
            ValidateSecurity(entry, Permission.ItemsEdit);
            int count = 0;
            var itemsInGroup = GetItemList(entry, type, groupId, null, null, out count);
            foreach (var ItemInPriceDiscountGroup in itemsInGroup)
            {
                RemoveItemFromGroup(entry,ItemInPriceDiscountGroup.ID, type);
            }
        } 

        public virtual void AddItemToGroup(IConnectionManager entry, RecordIdentifier itemId, PriceDiscGroupEnum type, string groupId)
        {
            ValidateSecurity(entry, Permission.ItemsEdit);

            var masterId = GetMasterID(entry, itemId, "RETAILITEM", "ITEMID");
            SqlServerStatement statement = new SqlServerStatement("RETAILITEM");

            string groupFieldinDB = "";

            switch (type)
            {
                case PriceDiscGroupEnum.LineDiscountGroup:
                    groupFieldinDB = "SALESLINEDISC";
                    break;
                case PriceDiscGroupEnum.MultilineDiscountGroup:
                    groupFieldinDB = "SALESMULTILINEDISC";
                    break;
            }

            if (ItemExists(entry, itemId))
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("MASTERID", (Guid)masterId, SqlDbType.UniqueIdentifier);
                statement.AddField(groupFieldinDB, groupId);

                entry.Connection.ExecuteStatement(statement);

            }
        }
    }
}
