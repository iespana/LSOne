using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders.EOD;
using LSOne.DataLayer.TransactionObjects.EOD;
using LSOne.DataLayer.SqlConnector.QueryHelpers;

namespace LSOne.DataLayer.SqlTransactionDataProviders.EOD
{
    public class ItemReportInfoData : SqlServerDataProviderBase, IItemReportInfoData
    {
        public virtual List<ItemSaleReportLine> GetItemSaleReportLines(IConnectionManager entry, DateTime dtFrom, DateTime dtTo, string storeId, string terminalId, SalesOrReturnsEnum salesOrReturns, ItemSaleReportGroupEnum printGroup)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                int groupIndex = (int)printGroup;
                string groupBy = "GROUP BY TS.STORE, TS.TERMINALID, UN.TXT, TS.ITEMID, I.VARIANTNAME";

                List<Condition> conditions = new List<Condition>
                {
                    new Condition { ConditionValue = "T.TYPE = 2", Operator = "AND" },
                    new Condition { ConditionValue = "T.RECEIPTID <> ''", Operator = "AND" },
                    new Condition { ConditionValue = "T.ENTRYSTATUS = 0", Operator = "AND" },
                    new Condition { ConditionValue = "T.STORE = @storeId", Operator = "AND" },
                    new Condition { ConditionValue = "T.TERMINAL = @terminalId", Operator = "AND" },
                    new Condition { ConditionValue = "T.TRANSDATE >= @dtFrom and t.TRANSDATE <= @dtTo", Operator = "AND" },
                    new Condition { ConditionValue = "TS.TRANSACTIONSTATUS = 0", Operator = "AND" },
                    new Condition { ConditionValue = salesOrReturns == SalesOrReturnsEnum.Sales ? "TS.QTY < 0" : "TS.QTY > 0", Operator = "AND" },
                };

                List<Join> joins = new List<Join>
                {
                    new Join { Condition = "TS.TRANSACTIONID = T.TRANSACTIONID AND TS.RECEIPTID = T.RECEIPTID AND TS.STORE = T.STORE AND TS.TERMINALID = T.TERMINAL", Table = "RBOTRANSACTIONSALESTRANS", TableAlias = "TS" },
                    new Join { Condition = "I.ITEMID = TS.ITEMID", Table = "RETAILITEM", TableAlias = "I" },
                    new Join { Condition = "UN.UNITID = TS.UNIT", JoinType = "LEFT", Table = "UNIT", TableAlias = "UN" }
                };

                List<TableColumn> columns = new List<TableColumn>
                {
                    new TableColumn { ColumnName = "MAX(I.ITEMNAME)", ColumnAlias = "ITEMNAME" },
                    new TableColumn { ColumnName = "-SUM(TS.NETAMOUNTINCLTAX)", ColumnAlias = "AMOUNT" },
                    new TableColumn { ColumnName = "-SUM(TS.QTY)", ColumnAlias = "QTY" },
                    new TableColumn { ColumnName = groupIndex == (int)ItemSaleReportGroupEnum.RetailItem ? "ISNULL(UN.TXT, '')" : "''", ColumnAlias = "UNIT" },
                    new TableColumn { ColumnName = groupIndex == (int)ItemSaleReportGroupEnum.RetailItem ? "ISNULL(i.VARIANTNAME, '')" : "''", ColumnAlias = "VARIANTNAME" }
                };

                MakeParam(cmd, "dtFrom", dtFrom, SqlDbType.DateTime);
                MakeParam(cmd, "dtTo", dtTo, SqlDbType.DateTime);
                MakeParam(cmd, "storeid", storeId);
                MakeParam(cmd, "terminalId", terminalId);

                if(groupIndex == (int)ItemSaleReportGroupEnum.SpecialGroup)
                {
                    joins.Add(new Join { Condition = "SG.ITEMID = I.ITEMID", Table = "SPECIALGROUPITEMS", TableAlias = "SG" });
                    joins.Add(new Join { Condition = "SG.GROUPMASTERID = S.MASTERID", Table = "SPECIALGROUP", TableAlias = "S" });
                    groupBy = "GROUP BY TS.STORE, TS.TERMINALID, S.MASTERID";
                    columns[0].ColumnName = "MAX(S.NAME)";
                }
                else
                {
                    if(groupIndex >= (int)ItemSaleReportGroupEnum.RetailGroup)
                    {
                        joins.Add(new Join { Condition = "I.RETAILGROUPMASTERID = G.MASTERID", Table = "RETAILGROUP", TableAlias = "G" });
                        groupBy = "GROUP BY TS.STORE, TS.TERMINALID, G.MASTERID";
                        columns[0].ColumnName = "MAX(G.NAME)";
                    }

                    if (groupIndex >= (int)ItemSaleReportGroupEnum.RetailDepartment)
                    {
                        joins.Add(new Join { Condition = "G.DEPARTMENTMASTERID = DEP.MASTERID", Table = "RETAILDEPARTMENT", TableAlias = "DEP" });
                        groupBy = "GROUP BY TS.STORE, TS.TERMINALID, DEP.MASTERID";
                        columns[0].ColumnName = "MAX(DEP.NAME)";
                    }

                    if (groupIndex >= (int)ItemSaleReportGroupEnum.RetailDivision)
                    {
                        joins.Add(new Join { Condition = "DEP.DIVISIONMASTERID = DIV.MASTERID", Table = "RETAILDIVISION", TableAlias = "DIV" });
                        groupBy = "GROUP BY TS.STORE, TS.TERMINALID, DIV.MASTERID";
                        columns[0].ColumnName = "MAX(DIV.NAME)";
                    }
                }

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RBOTRANSACTIONTABLE", "T"),
                                                QueryPartGenerator.InternalColumnGenerator(columns),
                                                QueryPartGenerator.JoinGenerator(joins),
                                                QueryPartGenerator.ConditionGenerator(conditions),
                                                groupBy);

                return Execute<ItemSaleReportLine>(entry, cmd, CommandType.Text, PopulateItemSaleReportLines);
            }
        }

        private static void PopulateItemSaleReportLines(IDataReader dr, ItemSaleReportLine reportInfo)
        {
            reportInfo.ItemDescription = (string) dr["ITEMNAME"];
            reportInfo.Quantity = (decimal) dr["QTY"];
            reportInfo.Amount = (decimal) dr["AMOUNT"];
            reportInfo.VariantName = (string)dr["VARIANTNAME"];
            reportInfo.Unit = (string)dr["UNIT"]; 
        }
    }
}
