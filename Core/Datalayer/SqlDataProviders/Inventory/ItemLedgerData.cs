using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Inventory
{
    public class ItemLedgerData : SqlServerDataProviderBase, IItemLedgerData
    {
        private static List<TableColumn> listColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "TIME", TableAlias = "Ledger"},
            new TableColumn {ColumnName = "TYPE", TableAlias = "Ledger"},
            new TableColumn {ColumnName = "STOREID", TableAlias = "Ledger"},
            new TableColumn {ColumnName = "STORENAME", TableAlias = "Ledger"},
            new TableColumn {ColumnName = "TERMINALID", TableAlias = "Ledger"},
            new TableColumn {ColumnName = "TERMINALNAME", TableAlias = "Ledger"},
            new TableColumn {ColumnName = "RECEIPTID",TableAlias = "Ledger", ColumnAlias = "REFERENCE"},
            new TableColumn {ColumnName = "OPERATOR", TableAlias = "Ledger"},
            new TableColumn {ColumnName = "QUANTITY", TableAlias = "Ledger"},
            new TableColumn {ColumnName = "UNITNAME", TableAlias = "Ledger"},
            new TableColumn {ColumnName = "COSTPRICE", TableAlias = "Ledger"},

        };
        private static void PopulateNewItemLedger(IDataReader dr, ItemLedger item)
        {
            item.Time = (DateTime)dr["TIME"];
            
            switch ((int)dr["TYPE"])
            {
                case 0:
                    item.LedgerType = ItemLedgerType.Sale;
                    break;
                case 1:
                    item.LedgerType = ItemLedgerType.VoidedLine;
                    item.Quantity = -item.Quantity;
                    break;
                case 5:
                    item.LedgerType = ItemLedgerType.VoidedSale;
                    break;
                case 16:
                    item.LedgerType = ItemLedgerType.TransferIn;  
                    break;
                case 17:
                    item.LedgerType = ItemLedgerType.TransferOut;  
                    break;
                case 18:
                    item.LedgerType = ItemLedgerType.StockCount;
                    break;
                case 10:
                    item.LedgerType = ItemLedgerType.PostedStatement;  
                    break;
                case 11:
                    item.LedgerType = ItemLedgerType.Purchase;
                    break;
                case 12:
                    item.LedgerType = ItemLedgerType.Adjustment;
                    break;               
                case 14:
                    item.LedgerType = ItemLedgerType.StockReservation;
                    break;
                case 19:
                    item.LedgerType = ItemLedgerType.ParkedInventory;
                    break;
            }
            item.StoreID = (string)dr["STOREID"];
            item.StoreName = (string)dr["STORENAME"];
            item.TerminalID = (string)dr["TERMINALID"];
            item.TerminalName = (string)dr["TERMINALNAME"];
            item.Reference = (string)dr["REFERENCE"];
            item.Operator = (string)dr["OPERATOR"];
            item.Quantity = (decimal)dr["QUANTITY"];
            item.Adjustment = (decimal)dr["ADJUSTMENT"];

            item.UnitName = (string)dr["UNITNAME"];
            item.CostPrice = (decimal)dr["COSTPRICE"];
            item.Compatible = dr["COMPATIBILITY"] == DBNull.Value;
            item.ReasonCode = (string)dr["REASONCODE"];
        }


        private static void PopulateItemLedger(IDataReader dr, ItemLedger item)
        {
            item.ItemID = (string) dr["ITEMID"];
            item.Time = (DateTime) dr["TIME"];
            item.TransactionID = (string) dr["TRANSACTIONID"];
            item.Reference = (string) dr["RECEIPTID"];
            item.StoreID = (string)dr["STOREID"];
            item.StoreName = (string)dr["STORENAME"];
            item.TerminalID = (string)dr["TERMINALID"];
            item.TerminalName = (string)dr["TERMINALNAME"];
            item.Quantity = (decimal)dr["QUANTITY"];
            item.CostPrice = (decimal) dr["COSTPRICE"];
            item.NetPrice = (decimal) dr["NETPRICE"];
            item.Discount = (decimal) dr["DISCOUNTAMOUNT"];
            item.NetDiscount = (decimal) dr["NETDISCOUNTAMOUNT"];
            item.Customer = (string) dr["CUSTOMERID"];
            switch ((int) dr["TYPE"])
            {
                case 0:
                    item.LedgerType = ItemLedgerType.Sale;
                    break;
                case 1:
                    item.LedgerType = ItemLedgerType.VoidedLine;
                    item.Quantity = -item.Quantity;
                    break;
                case 5 : 
                    item.LedgerType = ItemLedgerType.VoidedSale;
                    break;
                case 10:
                    item.LedgerType = ItemLedgerType.PostedStatement;  //Posted Sales
                    break;
                case 11:
                    item.LedgerType = ItemLedgerType.Purchase;
                    break;
                case 12:
                    item.LedgerType = ItemLedgerType.Adjustment;
                    break;
            }
        }

        public virtual List<ItemLedger> GetList(IConnectionManager entry, RecordIdentifier itemLedgerID)
        {
            ItemLedgerSearchParameters parameters = new ItemLedgerSearchParameters
            {
                FromDateTime = (DateTime) itemLedgerID[3],
                ItemID = itemLedgerID[0],
                IncludeVoided = (bool) itemLedgerID[6],
                rowFrom = (int) itemLedgerID[8],
                rowTo = (int) itemLedgerID[9],
                SortAscending = (bool) itemLedgerID[10],
                StoreID = itemLedgerID[1],
                TerminalID = itemLedgerID[2],
                ToDateTime = (DateTime) itemLedgerID[4]
            };
            if (!(bool) itemLedgerID[5] && !(bool) itemLedgerID[7] || (bool)itemLedgerID[5] && (bool)itemLedgerID[7])
            {
                parameters.Source = ItemLedgerSearchOptions.All;
            }
            else if ((bool)itemLedgerID[5])
            {
                parameters.Source = ItemLedgerSearchOptions.Sales;                
            }
            else
            {
                parameters.Source = ItemLedgerSearchOptions.Inventory;
            }
            return GetList(entry,parameters);
            
        }

        private string SalesSource(ItemLedgerSearchParameters itemSearch)
        {
            List<TableColumn> salesColumns = new List<TableColumn>
                    {
                        new TableColumn
                        {
                            ColumnName = @"
                        CASE 
							WHEN ISNULL(H.ENTRYSTATUS, 0) = 1 THEN 5 
							ELSE ISNULL(I.TRANSACTIONSTATUS, 0)
						END",
                            ColumnAlias = "TYPE"
                        },
                        new TableColumn
                        {
                            ColumnName = "TRANSDATE",
                            TableAlias = "I",
                            IsNull = true,
                            ColumnAlias = "TIME",
                            NullValue = "1900-1-1"
                        },
                        new TableColumn
                        {
                            ColumnName = "LINENUM",
                            TableAlias = "I",                            
                        },
                        
                        new TableColumn
                        {
                            ColumnName = "RECEIPTID",
                            TableAlias = "I",
                            IsNull = true,
                            ColumnAlias = "REFERENCE"
                        },
                        new TableColumn
                        {
                            ColumnName = "STORE",
                            TableAlias = "I",
                            ColumnAlias = "STOREID"
                        },
                        new TableColumn
                        {
                            ColumnName = "NAME",
                            TableAlias = "S",
                            IsNull = true,
                            ColumnAlias = "STORENAME"
                        },
                        new TableColumn
                        {
                            ColumnName = "TERMINALID",
                            TableAlias = "I",

                        },
                        new TableColumn
                        {
                            ColumnName = "NAME",
                            TableAlias = "T",
                            IsNull = true,
                            ColumnAlias = "TERMINALNAME"
                        },
                        new TableColumn
                        {
                            ColumnName = "STAFF",
                            TableAlias = "H",
                            IsNull = true,
                            ColumnAlias = "OPERATOR"
                        },
                        new TableColumn
                        {
                            ColumnName = @"        
        CASE 
            WHEN RI.SALESUNITID  =  RI.INVENTORYUNITID THEN 1
            WHEN CSUTIU.FROMUNIT IS NOT NULL THEN CSUTIU.FACTOR
            WHEN CSUTIUGEN.FROMUNIT IS NOT NULL THEN CSUTIUGEN.FACTOR
            ELSE NULL
        END
        *
        I.UNITQTY",
                            IsNull = true,
                            ColumnAlias = "QUANTITY",
                            NullValue = "0"
                        },
                        new TableColumn
                        {
                            ColumnName = "QTY",
                            TableAlias = "I",
                            ColumnAlias = "ADJUSTMENT"
                        },
                        new TableColumn
                        {
                            ColumnName = "TXT",
                            TableAlias = "U",
                            ColumnAlias = "UNITNAME"
                        },
                        new TableColumn
                        {
                            ColumnName = "'SALES'",
                            ColumnAlias = "COMPATIBILITY"
                        },
                        new TableColumn
                        {
                            ColumnName = "ISNULL(R.REASONTEXT, '')",
                            ColumnAlias = "REASONCODE"
                        },
                        new TableColumn
                        {
                            ColumnName = @"		
		 CASE 
            WHEN RI.SALESUNITID  =  RI.INVENTORYUNITID THEN 1
            WHEN CSUTIU.FACTOR IS NOT NULL THEN CSUTIU.FACTOR
            WHEN CSUTIUGEN.FACTOR IS NOT NULL THEN CSUTIUGEN.FACTOR
            ELSE NULL
        END
		*
		 CASE 
            WHEN RI.INVENTORYUNITID = RI.PURCHASEUNITID THEN 1
            WHEN CIUTPU.FACTOR IS NOT NULL THEN CIUTPU.FACTOR
            WHEN CIUTPUGEN.FACTOR IS NOT NULL THEN CIUTPUGEN.FACTOR
            ELSE NULL
        END		
		*
		I.COSTAMOUNT
		*
		I.UNITQTY",
                            IsNull = true,
                            ColumnAlias = "COSTPRICE",
                            NullValue = "0"
                        }
                    };

            List<Join> salesJoins = new List<Join>
                    {

                        new Join
                        {
                            Table = "RETAILITEM",
                            Condition = "I.ITEMID = RI.ITEMID",
                            TableAlias = "RI"
                        },
                         new Join
                        {
                            Table = "UNIT",
                            Condition = "I.UNIT = U.UNITID",
                            TableAlias = "U"
                        },                      
                        new Join
                        {
                            Table = "GETCONVERSIONS(@ITEMID)",
                            Condition = "(RI.SALESUNITID = CSUTIU.TOUNIT AND CSUTIU.FROMUNIT = RI.INVENTORYUNITID)",
                            TableAlias = "CSUTIU",
                            JoinType = "LEFT"
                        },
                        new Join
                        {
                            Table = "GETCONVERSIONS(@ITEMID)",
                            Condition = "(RI.INVENTORYUNITID = CIUTPU.TOUNIT AND CIUTPU.FROMUNIT = RI.PURCHASEUNITID)",
                            TableAlias = "CIUTPU",
                            JoinType = "LEFT"
                        },                     
                        new Join
                        {
                            Table = "GETCONVERSIONS('')",
                            Condition = "(RI.SALESUNITID = CSUTIUGEN.TOUNIT AND CSUTIUGEN.FROMUNIT = RI.INVENTORYUNITID)",
                            TableAlias = "CSUTIUGEN",
                            JoinType = "LEFT"
                        },
                        new Join
                        {
                            Table = "GETCONVERSIONS('')",
                            Condition = "(RI.INVENTORYUNITID = CIUTPUGEN.TOUNIT AND CIUTPUGEN.FROMUNIT = RI.PURCHASEUNITID)",
                            TableAlias = "CIUTPUGEN",
                            JoinType = "LEFT"
                        },
                        new Join
                        {
                            Table = "RBOSTORETABLE",
                            Condition = "I.STORE = S.STOREID AND I.DATAAREAID = S.DATAAREAID",
                            TableAlias = "S",
                            JoinType = "LEFT"
                        },
                        new Join
                        {
                            Table = "RBOTERMINALTABLE",
                            Condition =
                                "I.TERMINALID = T.TERMINALID AND I.DATAAREAID = T.DATAAREAID AND I.STORE = T.STOREID",
                            TableAlias = "T",
                            JoinType = "LEFT"
                        },
                        new Join
                        {
                            Table = "RBOTRANSACTIONTABLE",
                            Condition =
                                "I.TRANSACTIONID = H.TRANSACTIONID AND I.DATAAREAID = H.DATAAREAID AND I.STORE = H.STORE and I.TERMINALID = H.TERMINAL",
                            TableAlias = "H"
                        },
                        new Join
                        {
                            Table = "INVENTTRANSREASON",
                            Condition = "I.REASONCODEID = R.REASONID AND I.DATAAREAID = R.DATAAREAID",
                            TableAlias = "R",
                            JoinType = "LEFT"
                        }

                    };

            List<Condition> salesConditions = new List<Condition>
            {
                new Condition
                {
                    ConditionValue = " I.ITEMID = @ITEMID ",
                    Operator = "AND"

                },
                new Condition
                {
                    ConditionValue = " I.DATAAREAID = @DATAAREAID ",
                    Operator = "AND"
                },
                new Condition
                {
                    ConditionValue = " H.ENTRYSTATUS <> 5 ",
                    Operator = "AND"
                },                

            };
            if (itemSearch.FromDateTime != null)
            {
                salesConditions.Add(new Condition
                {
                    ConditionValue = " I.TRANSDATE > @FROMDATE ",
                    Operator = "AND"
                });
            }
            if (itemSearch.ToDateTime != null)
            {
                salesConditions.Add(new Condition
                {
                    ConditionValue = " I.TRANSDATE < @TODATE ",
                    Operator = "AND"
                });
            }
            if (!RecordIdentifier.IsEmptyOrNull(itemSearch.StoreID))
            {
                salesConditions.Add(new Condition
                {
                    ConditionValue = " I.STORE = @STOREID ",
                    Operator = "AND"
                });
            }
            if (!RecordIdentifier.IsEmptyOrNull(itemSearch.TerminalID))
            {
                salesConditions.Add(new Condition
                {
                    ConditionValue = " I.TERMINALID = @TERMINALID ",
                    Operator = "AND"
                });
            }
            if (!itemSearch.IncludeVoided)
            {
                salesConditions.Add(new Condition
                {
                    ConditionValue = " H.ENTRYSTATUS <> 1 ",
                    Operator = "AND"
                });
                salesConditions.Add(new Condition
                {
                    ConditionValue = " I.TRANSACTIONSTATUS <> 1 ",
                    Operator = "AND"
                });
            }
            if (!itemSearch.DoNotAggregatePostedSales)
            {
                salesConditions.Add(new Condition
                {
                    ConditionValue = " I.STATEMENTID =  '0' ",
                    Operator = "AND"
                });
               
            }
            return string.Format(QueryTemplates.BaseQuery("RBOTRANSACTIONSALESTRANS", "I"),
                QueryPartGenerator.InternalColumnGenerator(salesColumns),
                QueryPartGenerator.JoinGenerator(salesJoins),
                QueryPartGenerator.ConditionGenerator(salesConditions),
                string.Empty);
        }

        private string ParkedInventorySource(ItemLedgerSearchParameters itemSearch)
        {
            List<TableColumn> parkedSalesColumns = new List<TableColumn>
                    {
                        new TableColumn
                        {
                            ColumnName = "19",
                            ColumnAlias = "TYPE"
                        },
                        new TableColumn
                        {
                            ColumnName = "TRANSDATE",
                            TableAlias = "I",
                            IsNull = true,
                            ColumnAlias = "TIME",
                            NullValue = "1900-1-1"
                        },
                        new TableColumn
                        {
                            ColumnName = "LINENUM",
                            TableAlias = "I",
                        },
                        new TableColumn
                        {
                            ColumnName = "RECEIPTID",
                            TableAlias = "I",
                            IsNull = true,
                            ColumnAlias = "REFERENCE"
                        },
                        new TableColumn
                        {
                            ColumnName = "STORE",
                            TableAlias = "I",
                            ColumnAlias = "STOREID"
                        },
                        new TableColumn
                        {
                            ColumnName = "NAME",
                            TableAlias = "S",
                            IsNull = true,
                            ColumnAlias = "STORENAME"
                        },
                        new TableColumn
                        {
                            ColumnName = "TERMINALID",
                            TableAlias = "I",

                        },
                        new TableColumn
                        {
                            ColumnName = "NAME",
                            TableAlias = "T",
                            IsNull = true,
                            ColumnAlias = "TERMINALNAME"
                        },
                        new TableColumn
                        {
                            ColumnName = "STAFF",
                            TableAlias = "H",
                            IsNull = true,
                            ColumnAlias = "OPERATOR"
                        },
                        new TableColumn
                        {
                            ColumnName = @"        
        CASE 
            WHEN RI.SALESUNITID  =  RI.INVENTORYUNITID THEN 1
            WHEN CSUTIU.FROMUNIT IS NOT NULL THEN CSUTIU.FACTOR
            WHEN CSUTIUGEN.FROMUNIT IS NOT NULL THEN CSUTIUGEN.FACTOR
            ELSE NULL
        END
        *
        I.UNITQTY * -1",
                            IsNull = true,
                            ColumnAlias = "QUANTITY",
                            NullValue = "0"
                        },
                        new TableColumn
                        {
                            ColumnName = "QTY * -1",
                            TableAlias = "I",
                            ColumnAlias = "ADJUSTMENT"
                        },
                        new TableColumn
                        {
                            ColumnName = "TXT",
                            TableAlias = "U",
                            ColumnAlias = "UNITNAME"
                        },
                        new TableColumn
                        {
                            ColumnName = "'SALES'",
                            ColumnAlias = "COMPATIBILITY"
                        },
                        new TableColumn
                        {
                            ColumnName = "'Returned to parked inventory'",
                            ColumnAlias = "REASONCODE"
                        },
                        new TableColumn
                        {
                            ColumnName = @"		
		 CASE 
            WHEN RI.SALESUNITID  =  RI.INVENTORYUNITID THEN 1
            WHEN CSUTIU.FACTOR IS NOT NULL THEN CSUTIU.FACTOR
            WHEN CSUTIUGEN.FACTOR IS NOT NULL THEN CSUTIUGEN.FACTOR
            ELSE NULL
        END
		*
		 CASE 
            WHEN RI.INVENTORYUNITID = RI.PURCHASEUNITID THEN 1
            WHEN CIUTPU.FACTOR IS NOT NULL THEN CIUTPU.FACTOR
            WHEN CIUTPUGEN.FACTOR IS NOT NULL THEN CIUTPUGEN.FACTOR
            ELSE NULL
        END		
		*
		I.COSTAMOUNT
		*
		I.UNITQTY",
                            IsNull = true,
                            ColumnAlias = "COSTPRICE",
                            NullValue = "0"
                        }
                    };

            List<Join> parkedSalesJoins = new List<Join>
                    {

                        new Join
                        {
                            Table = "RETAILITEM",
                            Condition = "I.ITEMID = RI.ITEMID",
                            TableAlias = "RI"
                        },
                         new Join
                        {
                            Table = "UNIT",
                            Condition = "I.UNIT = U.UNITID",
                            TableAlias = "U"
                        },
                        new Join
                        {
                            Table = "GETCONVERSIONS(@ITEMID)",
                            Condition = "(RI.SALESUNITID = CSUTIU.TOUNIT AND CSUTIU.FROMUNIT = RI.INVENTORYUNITID)",
                            TableAlias = "CSUTIU",
                            JoinType = "LEFT"
                        },
                        new Join
                        {
                            Table = "GETCONVERSIONS(@ITEMID)",
                            Condition = "(RI.INVENTORYUNITID = CIUTPU.TOUNIT AND CIUTPU.FROMUNIT = RI.PURCHASEUNITID)",
                            TableAlias = "CIUTPU",
                            JoinType = "LEFT"
                        },
                        new Join
                        {
                            Table = "GETCONVERSIONS('')",
                            Condition = "(RI.SALESUNITID = CSUTIUGEN.TOUNIT AND CSUTIUGEN.FROMUNIT = RI.INVENTORYUNITID)",
                            TableAlias = "CSUTIUGEN",
                            JoinType = "LEFT"
                        },
                        new Join
                        {
                            Table = "GETCONVERSIONS('')",
                            Condition = "(RI.INVENTORYUNITID = CIUTPUGEN.TOUNIT AND CIUTPUGEN.FROMUNIT = RI.PURCHASEUNITID)",
                            TableAlias = "CIUTPUGEN",
                            JoinType = "LEFT"
                        },
                        new Join
                        {
                            Table = "RBOSTORETABLE",
                            Condition = "I.STORE = S.STOREID AND I.DATAAREAID = S.DATAAREAID",
                            TableAlias = "S",
                            JoinType = "LEFT"
                        },
                        new Join
                        {
                            Table = "RBOTERMINALTABLE",
                            Condition =
                                "I.TERMINALID = T.TERMINALID AND I.DATAAREAID = T.DATAAREAID AND I.STORE = T.STOREID",
                            TableAlias = "T",
                            JoinType = "LEFT"
                        },
                        new Join
                        {
                            Table = "RBOTRANSACTIONTABLE",
                            Condition =
                                "I.TRANSACTIONID = H.TRANSACTIONID AND I.DATAAREAID = H.DATAAREAID AND I.STORE = H.STORE and I.TERMINALID = H.TERMINAL",
                            TableAlias = "H"
                        },
                        new Join
                        {
                            Table = "INVENTTRANSREASON",
                            Condition = "I.REASONCODEID = R.REASONID AND I.DATAAREAID = R.DATAAREAID",
                            TableAlias = "R",
                            JoinType = "LEFT"
                        }

                    };

            List<Condition> parkedSalesConditions = new List<Condition>
            {
                new Condition
                {
                    ConditionValue = " I.ITEMID = @ITEMID ",
                    Operator = "AND"

                },
                new Condition
                {
                    ConditionValue = " I.DATAAREAID = @DATAAREAID ",
                    Operator = "AND"
                },
                new Condition
                {
                    ConditionValue = " H.ENTRYSTATUS <> 5 ",
                    Operator = "AND"
                },
                new Condition
                {
                    ConditionValue = " ISNULL(R.ACTION, 0) = 1 ",
                    Operator = "AND"
                }
            };
            if (itemSearch.FromDateTime != null)
            {
                parkedSalesConditions.Add(new Condition
                {
                    ConditionValue = " I.TRANSDATE > @FROMDATE ",
                    Operator = "AND"
                });
            }
            if (itemSearch.ToDateTime != null)
            {
                parkedSalesConditions.Add(new Condition
                {
                    ConditionValue = " I.TRANSDATE < @TODATE ",
                    Operator = "AND"
                });
            }
            if (!RecordIdentifier.IsEmptyOrNull(itemSearch.StoreID))
            {
                parkedSalesConditions.Add(new Condition
                {
                    ConditionValue = " I.STORE = @STOREID ",
                    Operator = "AND"
                });
            }
            if (!RecordIdentifier.IsEmptyOrNull(itemSearch.TerminalID))
            {
                parkedSalesConditions.Add(new Condition
                {
                    ConditionValue = " I.TERMINALID = @TERMINALID ",
                    Operator = "AND"
                });
            }
            if (!itemSearch.IncludeVoided)
            {
                parkedSalesConditions.Add(new Condition
                {
                    ConditionValue = " H.ENTRYSTATUS <> 1 ",
                    Operator = "AND"
                });
                parkedSalesConditions.Add(new Condition
                {
                    ConditionValue = " I.TRANSACTIONSTATUS <> 1 ",
                    Operator = "AND"
                });
            }
            if (!itemSearch.DoNotAggregatePostedSales)
            {
                parkedSalesConditions.Add(new Condition
                {
                    ConditionValue = " I.STATEMENTID =  '0' ",
                    Operator = "AND"
                });

            }
            return string.Format(QueryTemplates.BaseQuery("RBOTRANSACTIONSALESTRANS", "I"),
                QueryPartGenerator.InternalColumnGenerator(parkedSalesColumns),
                QueryPartGenerator.JoinGenerator(parkedSalesJoins),
                QueryPartGenerator.ConditionGenerator(parkedSalesConditions),
                string.Empty);
        }

        private string InventorySource(ItemLedgerSearchParameters itemSearch)
        {
            List<TableColumn> inventoryColumns = new List<TableColumn>
                    {
                        new TableColumn
                        {
                            ColumnName = "ISNULL(I.TYPE, 0) + 10",
                            ColumnAlias = "TYPE"
                        },
                        new TableColumn
                        {
                            ColumnName = "POSTINGDATE",
                            TableAlias = "I",
                            IsNull = true,
                            ColumnAlias = "TIME",
                            NullValue = "1900-1-1"
                        },
                         new TableColumn
                        {
                             ColumnName = "'0.00'",
                            ColumnAlias = "LINENUM",
                            
                        },
                        new TableColumn
                        {
                            ColumnName = "REFERENCE"
                            
                        },
                        new TableColumn
                        {
                            ColumnName = "STOREID",
                            TableAlias = "I",
                            IsNull = true,
                            ColumnAlias = "STOREID"
                        },
                        new TableColumn
                        {
                            ColumnName = "NAME",
                            TableAlias = "S",
                            IsNull = true,
                            ColumnAlias = "STORENAME"
                        },
                        new TableColumn
                        {
                            ColumnName = "''",
                            ColumnAlias = "TERMINALID"
                        },
                        new TableColumn
                        {
                            ColumnName = "''",
                            ColumnAlias = "TERMINALNAME"
                        },
                        new TableColumn
                        {
                            ColumnName = "''",
                            ColumnAlias = "OPERATOR"
                        },
                        new TableColumn
                        {
                            ColumnName = "ADJUSTMENTININVENTORYUNIT",
                            TableAlias = "I",
                            IsNull = true,
                            ColumnAlias = "QUANTITY",
                            NullValue = "0"
                        },
                        new TableColumn
                        {
                            ColumnName = "ADJUSTMENT",
                            TableAlias = "I",
                            ColumnAlias = "ADJUSTMENT",
                        },
                        new TableColumn
                        {
                            ColumnName = "TXT",
                            ColumnAlias = "UNITNAME",
                            TableAlias = "U"
                        },
                          new TableColumn
                        {
                            ColumnName = "COMPATIBILITY",
                            TableAlias = "I"
                        },
                        new TableColumn
                        {
                            ColumnName = "REASONTEXT",
                            TableAlias = "R",
                            IsNull = true,
                            NullValue = "''",
                            ColumnAlias = "REASONCODE"
                        }
           
                    };
            if (itemSearch.ShowObsoleteEntries)
            {
                inventoryColumns.Add(new TableColumn
                {
                    ColumnName = @"		                   
                            ISNULL(I.COSTPRICEPERITEM, 0) 
		                    *
		                    ISNULL(I.ADJUSTMENTININVENTORYUNIT, 0)",
                    ColumnAlias = "COSTPRICE",
                });                

            }
            else
            {
                inventoryColumns.Add(new TableColumn
                {
                    ColumnName = @"
                    CASE 
	                    WHEN I.COMPATIBILITY IS NOT NULL 
		                    THEN
                            ISNULL(I.COSTPRICEPERITEM, 0) 
		                    *
		                    ISNULL(I.ADJUSTMENTININVENTORYUNIT, 0)
	                    ELSE 0
                    END",
                    ColumnAlias = "COSTPRICE",
                });
            }
            List<Condition> inventoryConditions = new List<Condition>
                    {
                        new Condition
                        {
                            ConditionValue = " I.ITEMID = @ITEMID ",
                            Operator = "AND"

                        },
                        new Condition
                        {
                            ConditionValue = " I.DATAAREAID = @DATAAREAID ",
                            Operator = "AND"
                        }

                    };

            if (itemSearch.FromDateTime != null)
            {
                inventoryConditions.Add(new Condition
                {
                    ConditionValue = " I.POSTINGDATE > @FROMDATE ",
                    Operator = "AND"
                });
            }
            if (itemSearch.ToDateTime != null)
            {
                inventoryConditions.Add(new Condition
                {
                    ConditionValue = " I.POSTINGDATE < @TODATE ",
                    Operator = "AND"
                });
            }
            if (!RecordIdentifier.IsEmptyOrNull(itemSearch.StoreID))
            {
                inventoryConditions.Add(new Condition
                {
                    ConditionValue = "  I.STOREID = @STOREID  ",
                    Operator = "AND"
                });
            }
           
            if (itemSearch.DoNotAggregatePostedSales)
            {
                inventoryConditions.Add(new Condition
                {
                    ConditionValue = " ISNULL(I.TYPE, 0) !=  0 ",
                    Operator = "AND"
                });

            }
           
            List<Join> inventoryJoins = new List<Join>
            {

                new Join
                {
                    Table = "RBOSTORETABLE",
                    Condition = "I.STOREID = S.STOREID AND I.DATAAREAID = S.DATAAREAID",
                    TableAlias = "S",
                    JoinType = "LEFT"
                },
                new Join
                {
                    Table = "UNIT",
                    Condition = "I.UNITID = U.UNITID",
                    TableAlias = "U"
                },
                new Join
                {
                    Table = "INVENTTRANSREASON",
                    Condition = "I.REASONCODE = R.REASONID AND I.DATAAREAID = R.DATAAREAID",
                    TableAlias = "R",
                    JoinType = "LEFT"
                }
            };

            return string.Format(QueryTemplates.BaseQuery("INVENTTRANS", "I"),
                QueryPartGenerator.InternalColumnGenerator(inventoryColumns),
                QueryPartGenerator.JoinGenerator(inventoryJoins),
                QueryPartGenerator.ConditionGenerator(inventoryConditions),
                string.Empty);
        }

     
        public virtual List<ItemLedger> GetList(IConnectionManager entry, ItemLedgerSearchParameters itemSearch)
        {
            
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ITEMID", itemSearch.ItemID);

                if (!RecordIdentifier.IsEmptyOrNull(itemSearch.StoreID))
                {
                    MakeParam(cmd, "STOREID", itemSearch.StoreID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(itemSearch.TerminalID))
                {
                    MakeParam(cmd, "TERMINALID", itemSearch.TerminalID);
                }

                if (itemSearch.FromDateTime != null)
                {
                    MakeParam(cmd, "FROMDATE", (DateTime)itemSearch.FromDateTime, SqlDbType.DateTime);
                }

                if (itemSearch.ToDateTime != null)
                {
                    MakeParam(cmd, "TODATE", (DateTime)itemSearch.ToDateTime, SqlDbType.DateTime);
                }

                MakeParam(cmd, "FROMNUMBER", itemSearch.rowFrom, SqlDbType.Int);
                MakeParam(cmd, "TONUMBER", itemSearch.rowTo <= 0? int.MaxValue: itemSearch.rowTo, SqlDbType.Int);
                 

                List<string> sources = new List<string>();
                string sales = string.Empty;
                string inventory = string.Empty;
                if (itemSearch.Source == ItemLedgerSearchOptions.All ||
                    itemSearch.Source == ItemLedgerSearchOptions.Sales)
                {
                    sources.Add(SalesSource(itemSearch));
                }
                if (itemSearch.Source == ItemLedgerSearchOptions.All ||
                    itemSearch.Source == ItemLedgerSearchOptions.Inventory)
                {
                    sources.Add(InventorySource(itemSearch));
                    sources.Add(ParkedInventorySource(itemSearch));
                }

                string ledgerSource = string.Empty;

                for(int i = 0; i < sources.Count; i++)
                {
                    if(i != 0)
                    {
                        ledgerSource += Environment.NewLine + "UNION ALL";
                    }

                    ledgerSource += Environment.NewLine + sources[i];
                }

                ledgerSource = $"({ledgerSource})";

                List <TableColumn> ledgerColumns = new List<TableColumn>
                {
                    new TableColumn
                    {
                        ColumnName = "TYPE",
                        TableAlias = "SOURCE"
                    },
                    new TableColumn
                    {
                        ColumnName = "TIME",
                        TableAlias = "SOURCE"
                    },
                    new TableColumn
                    {
                        ColumnName = "LINENUM",
                        TableAlias = "SOURCE"
                    },
                    new TableColumn
                    {
                        ColumnName = "REFERENCE",
                        TableAlias = "SOURCE"
                    },
                    new TableColumn
                    {
                        ColumnName = "STOREID",
                        TableAlias = "SOURCE"
                    },
                    new TableColumn
                    {
                        ColumnName = "STORENAME",
                        TableAlias = "SOURCE"
                    },
                    new TableColumn
                    {
                        ColumnName = "TERMINALID",
                        TableAlias = "SOURCE"
                    },
                    new TableColumn
                    {
                        ColumnName = "TERMINALNAME",
                        TableAlias = "SOURCE"
                    },
                    new TableColumn
                    {
                        ColumnName = "OPERATOR",
                        TableAlias = "SOURCE"
                    },
                    new TableColumn
                    {
                        ColumnName = "QUANTITY",
                        TableAlias = "SOURCE"
                    },
                     new TableColumn
                    {
                        ColumnName = "ADJUSTMENT",
                        TableAlias = "SOURCE"
                    },
                    new TableColumn
                    {
                        ColumnName = "UNITNAME",
                        TableAlias = "SOURCE"
                    },
                    new TableColumn
                    {
                        ColumnName = "COSTPRICE",
                        TableAlias = "SOURCE"
                    },
                    new TableColumn
                    {
                        ColumnName = "COMPATIBILITY",
                        TableAlias = "SOURCE"
                    },
                    new TableColumn
                    {
                        ColumnName = "REASONCODE",
                        TableAlias = "SOURCE"
                    }
                };
                string sortString = itemSearch.SortAscending ? "ORDER BY  TIME ASC" : "ORDER BY TIME DESC";
                TableColumn rowColumn = new TableColumn
                {
                    ColumnName = $"ROW_NUMBER() OVER( {sortString} ) AS ROWNUM"
                };

                Condition pagingCondition = new Condition
                {
                    ConditionValue = "ROWNUM BETWEEN @FROMNUMBER AND @TONUMBER"
                };


                cmd.CommandText = string.Format(QueryTemplates.PagingQueryWithSepparateRowColumn(ledgerSource, "SOURCE", "LEDGER"),
                        QueryPartGenerator.ExternalColumnGenerator(ledgerColumns, "LEDGER"),
                        QueryPartGenerator.InternalColumnGenerator(ledgerColumns),
                        rowColumn,
                        string.Empty,
                        string.Empty,
                        QueryPartGenerator.ConditionGenerator(pagingCondition),
                        sortString);
                
              
                return Execute<ItemLedger>(entry, cmd, CommandType.Text, PopulateNewItemLedger);
            }
        }

        public virtual int GetLedgerEntryCountForItem(IConnectionManager entry, RecordIdentifier itemID)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ITEMID", itemID);


                List<string> sources = new List<string>();
                string sales = string.Empty;
                string inventory = string.Empty;
   
                sources.Add(SalesSource(new ItemLedgerSearchParameters() { ItemID =  itemID}));
                sources.Add(ParkedInventorySource(new ItemLedgerSearchParameters() { ItemID = itemID }));
                sources.Add(InventorySource(new ItemLedgerSearchParameters() { ItemID = itemID }));

                string ledgerSource = string.Empty;

                for (int i = 0; i < sources.Count; i++)
                {
                    if (i != 0)
                    {
                        ledgerSource += Environment.NewLine + "UNION ALL";
                    }

                    ledgerSource += Environment.NewLine + sources[i];
                }

                ledgerSource = $"({ledgerSource})";

                List<TableColumn> externalColumns = new List<TableColumn>
                {
                    new TableColumn
                    {
                        CustomText = "COUNT(*)",
                        IsCustomExternalColumn = true
                    }
                };

                List<TableColumn> ledgerColumns = new List<TableColumn>
                {
                    new TableColumn
                    {
                        ColumnName = "DISTINCT *"
                    }                    
                };               

                cmd.CommandText = string.Format(QueryTemplates.InternalQuery(ledgerSource, "SOURCE", "LEDGER"),
                        QueryPartGenerator.ExternalColumnGenerator(externalColumns, ""),
                        QueryPartGenerator.InternalColumnGenerator(ledgerColumns),
                        "",
                        "",
                        "",
                        "",
                        "",
                        "");


                return (int) entry.Connection.ExecuteScalar(cmd);
            }            
        }
    }
}
