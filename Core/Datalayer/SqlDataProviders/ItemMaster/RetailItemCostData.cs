using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace LSOne.DataLayer.SqlDataProviders.ItemMaster
{
    public class RetailItemCostData : SqlServerDataProviderBase, IRetailItemCostData
    {
        private static string CteQuerySimple = ";WITH CTE AS (SELECT *, ROW_NUMBER() OVER (PARTITION BY STOREID ORDER BY ENTRYDATE DESC) AS RN FROM RETAILITEMCOST {0} )";

        private static string CteQuery = @";WITH CTE AS (SELECT *, ROW_NUMBER() OVER (PARTITION BY STOREID ORDER BY ENTRYDATE DESC) AS RN FROM RETAILITEMCOST {0} ),
                                            CTEPAGE AS(SELECT {1}, ROW_NUMBER() OVER({2}) AS ROW FROM CTE C 
                                            {3}
                                            WHERE RN = 1)";

        private static List<TableColumn> CostColumns = new List<TableColumn>
        {
            new TableColumn { ColumnName = "ID", TableAlias = "C" },
            new TableColumn { ColumnName = "ITEMID", TableAlias = "C" },
            new TableColumn { ColumnName = "STOREID", TableAlias = "C" },
            new TableColumn { ColumnName = "NAME", TableAlias = "S", IsNull = true, NullValue = "''", ColumnAlias = "STORENAME" },
            new TableColumn { ColumnName = "UNITID", TableAlias = "C" },
            new TableColumn { ColumnName = "TXT", TableAlias = "U", IsNull = true, NullValue = "''", ColumnAlias = "UNITNAME" },
            new TableColumn { ColumnName = "ISNULL(C.COST, 0) * COALESCE(UCI.FACTOR, 1 / UCIR.FACTOR, UC.FACTOR, 1 / UCR.FACTOR, 1)", ColumnAlias = "COST" },
            new TableColumn { ColumnName = "ISNULL(V.QUANTITY, 0) * COALESCE(1 / UCI.FACTOR, UCIR.FACTOR, 1 / UC.FACTOR, UCR.FACTOR, 1)", ColumnAlias = "QUANTITY" },
            new TableColumn { ColumnName = "ENTRYDATE", TableAlias = "C" },
            new TableColumn { ColumnName = "(ISNULL(C.COST, 0) * COALESCE(UCI.FACTOR, 1 / UCIR.FACTOR, UC.FACTOR, 1 / UCR.FACTOR, 1)) * (ISNULL(V.QUANTITY, 0) * COALESCE(1 / UCI.FACTOR, UCIR.FACTOR, 1 / UC.FACTOR, UCR.FACTOR, 1))", ColumnAlias = "TOTALCOST" },
            new TableColumn { ColumnName = "REASON", TableAlias = "C", IsNull = true, NullValue = "''", ColumnAlias = "REASON" },
            new TableColumn { ColumnName = "USERID", TableAlias = "C", ColumnAlias = "USERID" },
            new TableColumn { ColumnName = "LOGIN", TableAlias = "US", ColumnAlias = "USERLOGIN" },
            new TableColumn { ColumnName = "NAME", TableAlias = "STAFF", ColumnAlias = "USERNAME" }
        };

        public static List<Join> CostJoins = new List<Join>
        {
            new Join { JoinType = "INNER", Table = "RETAILITEM", TableAlias = "R", Condition = "R.ITEMID = C.ITEMID" },
            new Join { JoinType = "LEFT", Table = "UNIT", TableAlias = "U", Condition = "U.UNITID = R.SALESUNITID" },
            new Join { JoinType = "LEFT", Table = "UNITCONVERT", TableAlias = "UCI", Condition = "UCI.FROMUNIT = C.UNITID AND UCI.TOUNIT = R.SALESUNITID AND UCI.ITEMID = C.ITEMID" },
            new Join { JoinType = "LEFT", Table = "UNITCONVERT", TableAlias = "UCIR", Condition = "UCIR.TOUNIT = C.UNITID AND UCIR.FROMUNIT = R.SALESUNITID AND UCIR.ITEMID = C.ITEMID" },
            new Join { JoinType = "LEFT", Table = "UNITCONVERT", TableAlias = "UC", Condition = "UC.FROMUNIT = C.UNITID AND UC.TOUNIT = R.SALESUNITID AND UC.ITEMID = ''" },
            new Join { JoinType = "LEFT", Table = "UNITCONVERT", TableAlias = "UCR", Condition = "UCR.TOUNIT = C.UNITID AND UCR.FROMUNIT = R.SALESUNITID AND UCR.ITEMID = ''" },
            new Join { JoinType = "LEFT", Table = "RBOSTORETABLE", TableAlias = "S", Condition = "C.STOREID = S.STOREID" },
            new Join { JoinType = "LEFT", Table = "VINVENTSUM", TableAlias = "V", Condition = "V.STOREID = C.STOREID AND V.ITEMID = C.ITEMID" },
            new Join { JoinType = "LEFT", Table = "USERS", TableAlias = "US", Condition = "US.GUID = C.USERID" },
            new Join { JoinType = "LEFT", Table = "RBOSTAFFTABLE", TableAlias = "STAFF", Condition = "STAFF.STAFFID = US.STAFFID" }
        };

        public void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            DeleteRecord(entry, "RETAILITEMCOST", new string[] { "ID", "ITEMID", "STOREID" }, ID, "", false);
        }

        public bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            return RecordExists(entry, "RETAILITEMCOST", new string[] { "ID", "ITEMID", "STOREID" }, ID, false);
        }

        public RetailItemCost Get(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "ITEMID = @ITEMID" });
                MakeParam(cmd, "ITEMID", itemID.StringValue);

                if(!RecordIdentifier.IsEmptyOrNull(storeID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "STOREID = @STOREID" });
                    MakeParam(cmd, "STOREID", storeID.StringValue);
                }

                cmd.CommandText = string.Format(CteQuerySimple, QueryPartGenerator.ConditionGenerator(conditions));
                cmd.CommandText += string.Format(QueryTemplates.BaseQuery("CTE", "C"),
                    QueryPartGenerator.InternalColumnGenerator(CostColumns),
                    QueryPartGenerator.JoinGenerator(CostJoins),
                    "WHERE C.RN = 1",
                    "ORDER BY S.NAME ASC");

                try
                {
                    var result = Execute<RetailItemCost>(entry, cmd, CommandType.Text, PopulateCost);
                    if (result == null || result.Count == 0)
                        return null;

                    return RecordIdentifier.IsEmptyOrNull(storeID) ? CalculateAllStoresAverage(result) : result[0];
                }
                catch
                {
                    return null;
                }
            }
        }

        private void PopulateCost(IDataReader dr, RetailItemCost item)
        {
            item.ID = (Guid)dr["ID"];
            item.ItemID = (string)dr["ITEMID"];
            item.StoreID = (string)dr["STOREID"];
            item.StoreName = (string)dr["STORENAME"];
            item.UnitID = (string)dr["UNITID"];
            item.UnitName = (string)dr["UNITNAME"];
            item.Cost = (decimal)dr["COST"];
            item.Quantity = (decimal)dr["QUANTITY"];
            item.TotalCostValue = (decimal)dr["TOTALCOST"];
            item.EntryDate = Date.FromAxaptaDate(dr["ENTRYDATE"]);
            item.RecalculationReason = (string)dr["REASON"];
            item.UserID = (Guid)dr["USERID"];
            item.UserLogin = (string)dr["USERLOGIN"];
            item.UserName = (string)dr["USERNAME"];
        }

        private void PopulateCostWithCount(IConnectionManager entry, IDataReader dr, RetailItemCost item, ref int count)
        {
            PopulateCost(dr, item);
            count = (int)dr["ROW_COUNT"];
        }

        private RetailItemCost CalculateAllStoresAverage(List<RetailItemCost> costs)
        {
            RetailItemCost averageCost = new RetailItemCost();

            averageCost.ItemID = costs[0].ItemID;
            averageCost.Quantity = costs.Sum(x => x.Quantity < 0 ? 0 : x.Quantity);

            if(averageCost.Quantity > 0)
            {
                averageCost.Cost = costs.Sum(x => x.Cost * (x.Quantity < 0 ? 0 : x.Quantity)) / averageCost.Quantity;
            }

            averageCost.TotalCostValue = averageCost.Cost * averageCost.Quantity;

            return averageCost;
        }

        public List<RetailItemCost> GetList(IConnectionManager entry, RecordIdentifier itemID, RetailItemCostFilter filter, out int totalCount)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> cteConditions = new List<Condition>();
                cteConditions.Add(new Condition { Operator = "AND", ConditionValue = "ITEMID = @ITEMID" });
                MakeParam(cmd, "ITEMID", itemID.StringValue);

                List<Condition> innerConditions = new List<Condition>();

                if (!string.IsNullOrWhiteSpace(filter.Description))
                {
                    innerConditions.Add(new Condition { Operator = "AND", ConditionValue = "S.NAME LIKE @DESCRIPTION" });
                    MakeParam(cmd, "DESCRIPTION", PreProcessSearchText(filter.Description, true, filter.DescriptionBeginsWith));
                }

                if (!string.IsNullOrWhiteSpace(filter.City))
                {
                    innerConditions.Add(new Condition { Operator = "AND", ConditionValue = "S.CITY LIKE @CITY" });
                    MakeParam(cmd, "CITY", PreProcessSearchText(filter.City, true, filter.CityBeginsWith));
                }

                if(!RecordIdentifier.IsEmptyOrNull(filter.RegionID))
                {
                    innerConditions.Add(new Condition { Operator = "AND", ConditionValue = "S.REGIONID = @REGIONID" });
                    MakeParam(cmd, "REGIONID", filter.RegionID.StringValue);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.CurrencyID))
                {
                    innerConditions.Add(new Condition { Operator = "AND", ConditionValue = "S.CURRENCY = @CURRENCY" });
                    MakeParam(cmd, "CURRENCY", filter.CurrencyID.StringValue);
                }

                if(filter.CalculationDateFrom.HasValue)
                {
                    innerConditions.Add(new Condition { Operator = "AND", ConditionValue = "C.ENTRYDATE >= @DATEFROM" });
                    MakeParam(cmd, "DATEFROM", filter.CalculationDateFrom.Value, SqlDbType.DateTime);
                }

                if (filter.CalculationDateTo.HasValue)
                {
                    innerConditions.Add(new Condition { Operator = "AND", ConditionValue = "C.ENTRYDATE <= @DATETO" });
                    MakeParam(cmd, "DATETO", filter.CalculationDateTo.Value, SqlDbType.DateTime);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.User))
                {
                    innerConditions.Add(new Condition { Operator = "AND", ConditionValue = "C.USERLOGIN LIKE @STAFFLOGIN" });
                    MakeParam(cmd, "STAFFLOGIN", (string)filter.User);
                }

                if (!(filter.RowFrom == 0 && filter.RowTo == 0))
                {
                    innerConditions.Add(new Condition { Operator = "AND", ConditionValue = "C.ROW BETWEEN @ROWFROM AND @ROWTO" });
                    MakeParam(cmd, "ROWFROM", filter.RowFrom, SqlDbType.Int);
                    MakeParam(cmd, "ROWTO", filter.RowTo, SqlDbType.Int);
                }

                List<Join> joins = new List<Join>();
                joins.Add(new Join { JoinType = "CROSS", Table = "(SELECT Count(*) AS ROW_COUNT FROM CTEPAGE) AS tCount" });

                List<TableColumn> outerColumns = new List<TableColumn>();
                CostColumns.ForEach(x =>
                {
                    outerColumns.Add(new TableColumn
                    {
                        ColumnName = x.ColumnAlias == "" ? x.ColumnName : x.ColumnAlias,
                        TableAlias = "C"
                    });
                });

                outerColumns.Add(new TableColumn { ColumnName = "TOTALCOST", TableAlias = "C" });
                outerColumns.Add(new TableColumn { ColumnName = "ROW_COUNT", TableAlias = "tCount" });

                cmd.CommandText = string.Format(CteQuery, 
                    QueryPartGenerator.ConditionGenerator(cteConditions), 
                    QueryPartGenerator.InternalColumnGenerator(CostColumns),
                    ResolveSort(RetailItemCostSort.Store, true, true),
                    QueryPartGenerator.JoinGenerator(CostJoins));

                cmd.CommandText += string.Format(QueryTemplates.BaseQuery("CTEPAGE", "C"),
                    QueryPartGenerator.InternalColumnGenerator(outerColumns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(innerConditions),
                    ResolveSort(filter.Sort, filter.SortDescending, false));

                totalCount = 0;

                try
                {
                    var result = Execute<RetailItemCost, int>(entry, cmd, CommandType.Text, ref totalCount, PopulateCostWithCount);
                    if (result == null || result.Count == 0)
                    {
                        return new List<RetailItemCost>();
                    }

                    result.Add(CalculateAllStoresAverage(result));
                    return result;
                }
                catch
                {
                    return new List<RetailItemCost>();
                }
            }
        }

        private string ResolveSort(RetailItemCostSort sort, bool sortDescending, bool orderInnerCte)
        {
            string sortString = "ORDER BY ";
            switch (sort)
            {
                case RetailItemCostSort.Store:
                    sortString += orderInnerCte ? "S.NAME" : "C.STORENAME";
                    break;
                case RetailItemCostSort.Price:
                    sortString += "C.COST";
                    break;
                case RetailItemCostSort.CalculationDate:
                    sortString += "C.ENTRYDATE";
                    break;
                case RetailItemCostSort.InventoryQuantity:
                    sortString += orderInnerCte ? "ISNULL(V.QUANTITY, 0)" : "C.QUANTITY";
                    break;
                case RetailItemCostSort.TotalCostValue:
                    sortString += orderInnerCte ? "C.COST * ISNULL(V.QUANTITY, 0)" : "C.TOTALCOST";
                    break;
                case RetailItemCostSort.Reason:
                    sortString += "C.REASON";
                    break;
                case RetailItemCostSort.User:
                    sortString += "C.USERLOGIN";
                    break;
            }

            sortString += sortDescending ? " DESC" : " ASC";

            return sortString;
        }

        public void Save(IConnectionManager entry, RetailItemCost item)
        {
            var statement = new SqlServerStatement("RETAILITEMCOST");

            statement.StatementType = StatementType.Insert;

            if(RecordIdentifier.IsEmptyOrNull(item.ID))
            {
                item.ID = Guid.NewGuid();
            }

            statement.AddKey("ID", (Guid)item.ID, SqlDbType.UniqueIdentifier);
            statement.AddKey("ITEMID", item.ItemID.StringValue, SqlDbType.NVarChar);
            statement.AddKey("STOREID", item.StoreID.StringValue, SqlDbType.NVarChar);
            statement.AddKey("UNITID", item.UnitID.StringValue, SqlDbType.NVarChar);

            statement.AddField("ENTRYDATE", Date.Now.ToAxaptaSQLDate(), SqlDbType.DateTime);
            statement.AddField("COST", item.Cost, SqlDbType.Decimal);
            statement.AddField("REASON", item.RecalculationReason, SqlDbType.NVarChar);
            statement.AddField("USERID", (Guid)item.UserID, SqlDbType.UniqueIdentifier);

            entry.Connection.ExecuteStatement(statement);
        }

        public void ArchiveRecords(IConnectionManager entry)
        {
            using (SqlCommand cmd = new SqlCommand("spINVENTORY_ArchiveItemCosts"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                entry.Connection.ExecuteNonQuery(cmd, false);
            }
        }
    }
}
