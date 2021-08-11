using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LSOne.DataLayer.SqlDataProviders.ItemMaster
{
    public class RetailItemAssemblyData : SqlServerDataProviderBase, IRetailItemAssemblyData
    {
        private static List<TableColumn> AssemblyColumns = new List<TableColumn>
        {
            new TableColumn { ColumnName = "ID", TableAlias = "A" },
            new TableColumn { ColumnName = "ITEMID", TableAlias = "A" },
            new TableColumn { ColumnName = "DESCRIPTION", TableAlias = "A" },
            new TableColumn { ColumnName = "ENABLED", TableAlias = "A" },
            new TableColumn { ColumnName = "CALCULATEPRICEFROMCOMPS", TableAlias = "A" },
            new TableColumn { ColumnName = "STOREID", TableAlias = "A" },
            new TableColumn { ColumnName = "ISNULL(S.NAME, '')", ColumnAlias = "STORENAME" },
            new TableColumn { ColumnName = "PRICE", TableAlias = "A" },
            new TableColumn { ColumnName = "EXPANDASSEMBLY", TableAlias = "A" },
            new TableColumn { ColumnName = "SENDCOMPONENTSTOKDS", TableAlias = "A" },
            new TableColumn { ColumnName = "STARTINGDATE", TableAlias = "A" },
            new TableColumn { ColumnName = $"{AssemblyItemQueries.AssemblyTotalCostQuery}", ColumnAlias = "COST" },
        };

        private static List<TableColumn> AssemblyColumnsWithCalculatedPrice = new List<TableColumn>(AssemblyColumns)
        {
            new TableColumn { ColumnName = "SALESPRICE", IsNull = true, ColumnAlias = "SALESPRICE", NullValue = "0", TableAlias = "P" },
        };

        public static List<Join> AssemblyJoins = new List<Join>
        {
            new Join { JoinType = "LEFT", Table = "RBOSTORETABLE", TableAlias = "S", Condition = "A.STOREID = S.STOREID" },
            new Join { JoinType = "OUTER", UseApply = true,
                Condition = @"(SELECT [Value] AS COSTCALCULATION FROM SYSTEMSETTINGS SS WHERE SS.GUID = '2BAB2653-C366-480E-8DC2-99107BC03D5F') CC" },
        };

        public static List<Join> AssemblyJoinsWithCalculatedPrice = new List<Join>(AssemblyJoins)
        {
            new Join { JoinType = "OUTER", UseApply = true,
                Condition = @"(SELECT SUM(C.SALESPRICE * COALESCE(UNIT1.FACTOR, 1 / UNIT1R.FACTOR, UNIT2.FACTOR, 1 / UNIT2R.FACTOR, 1) * C.QUANTITY) AS SALESPRICE FROM COMPS C 
                            LEFT JOIN UNITCONVERT UNIT1 ON UNIT1.FROMUNIT = C.SALESUNITID AND UNIT1.TOUNIT = C.UNITID AND UNIT1.ITEMID = C.ITEMID
                            LEFT JOIN UNITCONVERT UNIT1R ON UNIT1R.TOUNIT = C.SALESUNITID AND UNIT1R.FROMUNIT = C.UNITID AND UNIT1R.ITEMID = C.ITEMID
                            LEFT JOIN UNITCONVERT UNIT2 ON UNIT2.FROMUNIT = C.SALESUNITID AND UNIT2.TOUNIT = C.UNITID AND UNIT2.ITEMID = ''
                            LEFT JOIN UNITCONVERT UNIT2R ON UNIT2R.TOUNIT = C.SALESUNITID AND UNIT2R.FROMUNIT = C.UNITID AND UNIT2R.ITEMID = ''
                            WHERE C.ASSEMBLYID = A.ID) AS P" },
        };

        protected void PopulateAssembly(IDataReader dr, RetailItemAssembly assembly)
        {
            assembly.ID = (Guid)dr["ID"];
            assembly.Text = (string)dr["DESCRIPTION"];
            assembly.ItemID = (string)dr["ITEMID"];
            assembly.StoreID = (string)dr["STOREID"];
            assembly.StoreName = (string)dr["STORENAME"];
            assembly.Enabled = (bool)dr["ENABLED"];
            assembly.CalculatePriceFromComponents = (bool)dr["CALCULATEPRICEFROMCOMPS"];
            assembly.Price = (decimal)dr["PRICE"];
            assembly.ExpandAssembly = (ExpandAssemblyLocation)dr["EXPANDASSEMBLY"];
            assembly.SendAssemblyComponentsToKds = (KitchenDisplayAssemblyComponentType)dr["SENDCOMPONENTSTOKDS"];
            assembly.StartingDate = Date.FromAxaptaDate(dr["STARTINGDATE"]);
            assembly.TotalCost = (decimal)dr["COST"];

            decimal price = assembly.CalculatePriceFromComponents ? 0 : assembly.Price;
            if (price > 0)
            {
                assembly.Margin = (price - assembly.TotalCost) / price * 100;
            }
        }

        protected void PopulateAssemblyWithCalculatedPrice(IDataReader dr, RetailItemAssembly assembly)
        {
            PopulateAssembly(dr, assembly);
            assembly.TotalSalesPrice = (decimal)dr["SALESPRICE"];

            decimal price = assembly.CalculatePriceFromComponents ? assembly.TotalSalesPrice : assembly.Price;
            if (price > 0)
            {
                assembly.Margin = (price - assembly.TotalCost) / price * 100;
            }
        }

        public void Delete(IConnectionManager entry, RecordIdentifier assemblyID)
        {
            DeleteRecord(entry, "RETAILITEMASSEMBLY", "ID", assemblyID, "", false);
            DeleteRecord(entry, "RETAILITEMASSEMBLYCOMPONENTS", "ASSEMBLYID", assemblyID, "", false);
        }

        public bool Exists(IConnectionManager entry, RecordIdentifier assemblyID)
        {
            return RecordExists(entry, "RETAILITEMASSEMBLY", "ID", assemblyID, false);
        }

        public RetailItemAssembly Get(IConnectionManager entry, RecordIdentifier assemblyID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.ID = @ASSEMBLYID" });
                MakeParam(cmd, "ASSEMBLYID", (Guid)assemblyID, SqlDbType.UniqueIdentifier);

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEMASSEMBLY", "A"),
                    QueryPartGenerator.InternalColumnGenerator(AssemblyColumns),
                    QueryPartGenerator.JoinGenerator(AssemblyJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "");

                try
                {
                    var result = Execute<RetailItemAssembly>(entry, cmd, CommandType.Text, PopulateAssembly);
                    if (result == null || result.Count == 0)
                        return null;

                    return result[0];
                }
                catch
                {
                    return null;
                }
            }
        }

        public List<RetailItemAssembly> GetList(IConnectionManager entry, RecordIdentifier itemID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.ITEMID = @ITEMID" });
                MakeParam(cmd, "ITEMID", (string)itemID, SqlDbType.NVarChar);

                cmd.CommandText = AssemblyItemQueries.AssemblyTotalSalesPriceQuery + Environment.NewLine + string.Format(QueryTemplates.BaseQuery("RETAILITEMASSEMBLY", "A"),
                    QueryPartGenerator.InternalColumnGenerator(AssemblyColumnsWithCalculatedPrice),
                    QueryPartGenerator.JoinGenerator(AssemblyJoinsWithCalculatedPrice),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "");

                try
                {
                    return Execute<RetailItemAssembly>(entry, cmd, CommandType.Text, PopulateAssemblyWithCalculatedPrice);
                }
                catch
                {
                    return null;
                }
            }
        }

        public List<DataEntity> GetAll(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT ID, DESCRIPTION FROM RETAILITEMASSEMBLY ORDER BY DESCRIPTION ASC";
                return Execute<DataEntity>(entry, cmd, CommandType.Text, "DESCRIPTION", "ID");
            }
        }

        public void Save(IConnectionManager entry, RetailItemAssembly assembly)
        {
            var statement = new SqlServerStatement("RETAILITEMASSEMBLY");

            if (RecordIdentifier.IsEmptyOrNull(assembly.ID))
            {
                assembly.ID = Guid.NewGuid();
            }

            if (Exists(entry, assembly.ID))
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ID", (Guid)assembly.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("ID", (Guid)assembly.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("DESCRIPTION", assembly.Text, SqlDbType.NVarChar);
            statement.AddField("ITEMID", assembly.ItemID.StringValue, SqlDbType.NVarChar);
            statement.AddField("STOREID", assembly.StoreID.StringValue, SqlDbType.NVarChar);
            statement.AddField("PRICE", assembly.Price, SqlDbType.Decimal);
            statement.AddField("EXPANDASSEMBLY", assembly.ExpandAssembly, SqlDbType.Int);
            statement.AddField("SENDCOMPONENTSTOKDS", assembly.SendAssemblyComponentsToKds, SqlDbType.Int);
            statement.AddField("ENABLED", assembly.Enabled, SqlDbType.Bit);
            statement.AddField("CALCULATEPRICEFROMCOMPS", assembly.CalculatePriceFromComponents, SqlDbType.Bit);
            statement.AddField("STARTINGDATE", assembly.StartingDate.ToAxaptaSQLDate(), SqlDbType.DateTime);

            entry.Connection.ExecuteStatement(statement);
        }

        public List<RetailItemAssembly> Search(IConnectionManager entry, RetailItemAssemblySearchFilter searchFilter)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.ITEMID = @ITEMID" });
                MakeParam(cmd, "ITEMID", (string)searchFilter.ItemID, SqlDbType.NVarChar);

                if(!searchFilter.AllStores && !RecordIdentifier.IsEmptyOrNull(searchFilter.StoreID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.STOREID = @STOREID" });
                    MakeParam(cmd, "STOREID", (string)searchFilter.StoreID, SqlDbType.NVarChar);
                }

                if(searchFilter.StartingDateFrom.HasValue)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "CAST(A.STARTINGDATE AS DATE) >= @DATEFROM" });
                    MakeParam(cmd, "DATEFROM", searchFilter.StartingDateFrom.Value.Date, SqlDbType.Date);
                }

                if (searchFilter.StartingDateTo.HasValue)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "CAST(A.STARTINGDATE AS DATE) <= @DATETO" });
                    MakeParam(cmd, "DATETO", searchFilter.StartingDateTo.Value.Date.AddDays(1).AddMilliseconds(-1), SqlDbType.Date);
                }

                if(searchFilter.AssemblyStatus.HasValue)
                {
                    switch (searchFilter.AssemblyStatus)
                    {
                        case RetailItemAssemblyStatus.Disabled:
                            conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.ENABLED = 0" });
                            break;
                        case RetailItemAssemblyStatus.Enabled:
                            conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.ENABLED = 1" });
                            break;
                        case RetailItemAssemblyStatus.EnabledNotStarted:
                            conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.ENABLED = 1 AND CAST(A.STARTINGDATE AS DATE) > CAST(GETDATE() AS DATE)" });
                            break;
                        case RetailItemAssemblyStatus.Archived:
                            conditions.Add(new Condition { Operator = "AND", ConditionValue = "CAST(A.STARTINGDATE AS DATE) <= CAST(GETDATE() AS DATE)" });
                            break;
                        default:
                            break;
                    }
                }

                cmd.CommandText = AssemblyItemQueries.AssemblyTotalSalesPriceQuery + Environment.NewLine + string.Format(QueryTemplates.BaseQuery("RETAILITEMASSEMBLY", "A"),
                    QueryPartGenerator.InternalColumnGenerator(AssemblyColumnsWithCalculatedPrice),
                    QueryPartGenerator.JoinGenerator(AssemblyJoinsWithCalculatedPrice),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "");

                try
                {
                    return FilterSearchList(Execute<RetailItemAssembly>(entry, cmd, CommandType.Text, PopulateAssemblyWithCalculatedPrice), searchFilter.AssemblyStatus);
                }
                catch
                {
                    return null;
                }
            }
        }

        private List<RetailItemAssembly> FilterSearchList(List<RetailItemAssembly> retailItemAssemblies, RetailItemAssemblyStatus? status)
        {
            List<RetailItemAssembly> result = new List<RetailItemAssembly>();

            foreach(var group in retailItemAssemblies.OrderByDescending(x => x.StartingDate).GroupBy(x => x.StoreID))
            {
                RetailItemAssembly activeAssembly = null;

                foreach(var assembly in group)
                {
                    if(assembly.StartingDate.DateTime.Date > DateTime.Now.Date)
                    {
                        assembly.Status = assembly.Enabled ? RetailItemAssemblyStatus.EnabledNotStarted : RetailItemAssemblyStatus.Disabled;

                        if(status == null || status == assembly.Status || (assembly.Status == RetailItemAssemblyStatus.EnabledNotStarted && status == RetailItemAssemblyStatus.Enabled))
                        {
                            result.Add(assembly);
                        }
                    }
                    else
                    {
                        if(!assembly.Enabled)
                        {
                            assembly.Status = RetailItemAssemblyStatus.Disabled;

                            if (status == null || status == assembly.Status)
                            {
                                result.Add(assembly);
                            }

                            continue;
                        }

                        if(activeAssembly == null)
                        {
                            activeAssembly = assembly;
                            assembly.Status = RetailItemAssemblyStatus.Enabled;

                            if (status == null || status == assembly.Status)
                            {
                                result.Add(assembly);
                            }
                        }
                        else
                        {
                            assembly.Status = RetailItemAssemblyStatus.Archived;

                            if(status.HasValue && status.Value == RetailItemAssemblyStatus.Archived)
                            {
                                result.Add(assembly);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public void SetEnabled(IConnectionManager entry, RecordIdentifier assemblyID, bool enabled)
        {
            var statement = new SqlServerStatement("RETAILITEMASSEMBLY", StatementType.Update);

            statement.AddCondition("ID", (Guid)assemblyID, SqlDbType.UniqueIdentifier);
            statement.AddField("ENABLED", enabled, SqlDbType.Bit);
            entry.Connection.ExecuteStatement(statement);
        }

        public RetailItemAssembly GetAssemblyForItem(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier storeID, bool onlyGetActive = true)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.ITEMID = @ITEMID" });
                MakeParam(cmd, "ITEMID", (string)itemID, SqlDbType.NVarChar);

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "(A.STOREID = @STOREID OR A.STOREID = '')" });
                MakeParam(cmd, "STOREID", (string)storeID, SqlDbType.NVarChar);

                if (onlyGetActive)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.ENABLED = 1 AND CAST(A.STARTINGDATE AS DATE) <= CAST(GETDATE() AS DATE)" });
                }

                cmd.CommandText = AssemblyItemQueries.AssemblyTotalSalesPriceQuery + Environment.NewLine + string.Format(QueryTemplates.BaseQuery("RETAILITEMASSEMBLY", "A"),
                    QueryPartGenerator.InternalColumnGenerator(AssemblyColumnsWithCalculatedPrice),
                    QueryPartGenerator.JoinGenerator(AssemblyJoinsWithCalculatedPrice),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "ORDER BY STARTINGDATE DESC");

                List<RetailItemAssembly> results = Execute<RetailItemAssembly>(entry, cmd, CommandType.Text, PopulateAssemblyWithCalculatedPrice);

                return results.Count > 0 ? results.FirstOrDefault(x => x.StoreID == storeID) ?? results.FirstOrDefault(x => x.StoreID == "") : null;
            }
        }
    }
}
