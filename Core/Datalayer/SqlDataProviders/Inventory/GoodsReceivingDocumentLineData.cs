using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Expressions;

namespace LSOne.DataLayer.SqlDataProviders.Inventory
{
    /// <summary>
    /// Data provider class for goods receiving document lines
    /// </summary>
    public class GoodsReceivingDocumentLineData : SqlServerDataProviderBase, IGoodsReceivingDocumentLineData
    {
        private static List<TableColumn> goodsReceivingColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "GOODSRECEIVINGID " , TableAlias = "a"},
            new TableColumn {ColumnName = "LINENUMBER " , TableAlias = "a"},
            new TableColumn {ColumnName = "RECEIVEDQUANTITY " , TableAlias = "a"},
            new TableColumn {ColumnName = "POSTED " , TableAlias = "a"},
            new TableColumn {ColumnName = "STOREID " , TableAlias = "a"},
            new TableColumn {ColumnName = "RECEIVEDDATE " , TableAlias = "a"},
            new TableColumn {ColumnName = "PURCHASEORDERID " , TableAlias = "b"},
            new TableColumn {ColumnName = "ISNULL(c.NAME,'') " , ColumnAlias = "STORENAME"},

            new TableColumn {ColumnName = "RETAILITEMID " , TableAlias = "PL"},
            new TableColumn {ColumnName = "ISNULL(v.VENDORITEMID,'') " ,ColumnAlias = "VENDORITEMID"},
            new TableColumn {ColumnName = "UNITID " , TableAlias =  "PL"},
            new TableColumn {ColumnName = "ISNULL(u.TXT, '') ", ColumnAlias = "UNITNAME" },
            new TableColumn {ColumnName = "UNITDECIMALS" , TableAlias = "u",ColumnAlias = "MAXUNITDECIMALS"},
            new TableColumn {ColumnName = "ISNULL(u.MINUNITDECIMALS,0)" , ColumnAlias = "MINUNITDECIMALS"},
            new TableColumn {ColumnName = "QUANTITY" , TableAlias =  "PL"},
            new TableColumn {ColumnName = "PRICE" , TableAlias =  "PL"},
            new TableColumn {ColumnName = "ISNULL(IT.ITEMNAME,'')" , ColumnAlias = "ITEMNAME"},
            new TableColumn {ColumnName = "ISNULL(IT.ITEMID,'')" , ColumnAlias = "ITEMID"},
            new TableColumn {ColumnName = "IT.DELETED" , ColumnAlias = "ITEMDELTED"},
            new TableColumn {ColumnName = "IT.ITEMTYPE" , ColumnAlias = "ITEMTYPE"},
            new TableColumn {ColumnName = "ISNULL(IT.VARIANTNAME,'')" , ColumnAlias = "VARIANTNAME"},
            new TableColumn {ColumnName = "ISNULL(PL.DISCOUNTAMOUNT,0)" , ColumnAlias = "DISCOUNTAMOUNT"},
            new TableColumn {ColumnName = "ISNULL(PL.DISCOUNTPERCENTAGE,0)" , ColumnAlias = "DISCOUNTPERCENTAGE"},
            new TableColumn {ColumnName = "ISNULL(PL.TAXAMOUNT,0)" , ColumnAlias = "TAXAMOUNT"},
            new TableColumn {ColumnName = "ISNULL(PL.TAXCALCULATIONMETHOD,0)", ColumnAlias  = "TAXCALCULATIONMETHOD"},

            new TableColumn {ColumnName = "LINENUMBER " , ColumnAlias = "PURCHASEORDERLINENUMBER " , TableAlias = "PL"},
        };

        private static List<Join> listJoins = new List<Join>
        {
            new Join
            {
                Condition = "a.DATAAREAID = b.DATAAREAID AND a.GOODSRECEIVINGID = b.GOODSRECEIVINGID",
                Table = "GOODSRECEIVING",
                TableAlias = "b"
            },
            new Join
            {
                Condition = "a.DATAAREAID = c.DATAAREAID AND a.STOREID = c.STOREID",
                Table = "RBOSTORETABLE",
                TableAlias = "c"
            },
            new Join
            {
                Condition = "p.PURCHASEORDERID = b.PURCHASEORDERID",
                Table = "PURCHASEORDERS",
                TableAlias = "P"
            },
            new Join
            {
                Condition = "p.PURCHASEORDERID = PL.PURCHASEORDERID AND A.PURCHASEORDERLINENUMBER = PL.LINENUMBER",
                Table = "PURCHASEORDERLINE",
                TableAlias = "PL"
            },
            new Join
            {
                Condition = "PL.UNITID = U.UNITID",
                JoinType = "LEFT OUTER",
                Table = "UNIT",
                TableAlias = "U"
            },
            new Join
            {
                Condition = " PL.RETAILITEMID = V.RETAILITEMID AND P.VENDORID = V.VENDORID AND V.UNITID = PL.UNITID ",
                JoinType = "LEFT OUTER",
                Table = "VENDORITEMS",
                TableAlias = "V"
            },
            new Join
            {
                Condition = "IT.ITEMID = PL.RETAILITEMID",
                JoinType = "LEFT OUTER",
                Table = "RETAILITEM",
                TableAlias = "IT"
            },
        };

        private static TableColumn ResolveSort(GoodsReceivingDocumentLineSorting sort, bool sortBackwards)
        {
            // I was going to use dynamic linq but didnt because its not in the base class library
            switch (sort)
            {
                case GoodsReceivingDocumentLineSorting.ReceiveDate:
                    return new TableColumn
                    {
                        ColumnName = "RECEIVEDDATE",
                        SortDescending = sortBackwards,
                        TableAlias = "a"
                    };
                case GoodsReceivingDocumentLineSorting.ReceiveQuantity:
                    return new TableColumn
                    {
                        ColumnName = "RECEIVEDQUANTITY",
                        SortDescending = sortBackwards,
                        TableAlias = "a"
                    };
                case GoodsReceivingDocumentLineSorting.Posted:
                    return new TableColumn
                    {
                        ColumnName = "POSTED",
                        SortDescending = sortBackwards,
                        TableAlias = "a"
                    };
                case GoodsReceivingDocumentLineSorting.ItemName:
                    return new TableColumn
                    {
                        ColumnName = "ITEMNAME",
                        SortDescending = sortBackwards,
                        TableAlias = "IT"
                    };
                case GoodsReceivingDocumentLineSorting.ItemID:
                    return new TableColumn
                    {
                        ColumnName = "ITEMID",
                        SortDescending = sortBackwards,
                        TableAlias = "IT"
                    };
                case GoodsReceivingDocumentLineSorting.Variant:
                    return new TableColumn
                    {
                        ColumnName = "VARIANTNAME",
                        SortDescending = sortBackwards,
                        TableAlias = "IT"
                    };
                case GoodsReceivingDocumentLineSorting.OrderedQuantity:
                    return new TableColumn
                    {
                        ColumnName = "QUANTITY",
                        SortDescending = sortBackwards,
                        TableAlias = "PL"
                    };
                case GoodsReceivingDocumentLineSorting.StoreName:
                    return new TableColumn
                    {
                        ColumnName = "NAME",
                        ColumnAlias = "STORENAME",
                        TableAlias = "c"
                    };
            }

            return null;
        }

        private static List<GoodsReceivingDocumentLine> ResolveSortOLD(GoodsReceivingDocumentLineSorting sort, bool sortBackwards, List<GoodsReceivingDocumentLine> grdls)
        {
            // I was going to use dynamic linq but didn't because it's not in the base class library
            switch (sort)
            {
                case GoodsReceivingDocumentLineSorting.ReceiveDate:
                    grdls = (from g in grdls
                             orderby g.ReceivedDate
                             select g).ToList();
                    break;
                case GoodsReceivingDocumentLineSorting.ReceiveQuantity:
                    grdls = (from g in grdls
                             orderby g.ReceivedQuantity
                             select g).ToList();
                    break;
                case GoodsReceivingDocumentLineSorting.Posted:
                    grdls = (from g in grdls
                             orderby g.Posted
                             select g).ToList();
                    break;
                case GoodsReceivingDocumentLineSorting.ItemName:
                    grdls = (from g in grdls
                             orderby g.purchaseOrderLine.ItemName
                             select g).ToList();
                    break;
                case GoodsReceivingDocumentLineSorting.Variant:
                    grdls = (from g in grdls
                             orderby g.purchaseOrderLine.VariantName
                             select g).ToList();
                    break;
                case GoodsReceivingDocumentLineSorting.OrderedQuantity:
                    grdls = (from g in grdls
                             orderby g.purchaseOrderLine.Quantity
                             select g).ToList();
                    break;
                case GoodsReceivingDocumentLineSorting.StoreName:
                    grdls = (from g in grdls
                             orderby g.StoreName
                             select g).ToList();
                    break;
            }

            if (sortBackwards)
            {
                grdls.Reverse();
            }

            return grdls;
        }

        protected virtual void PopulateGoodsReceivingDocumentLineWithCount(IConnectionManager entry, IDataReader dr, GoodsReceivingDocumentLine goodsReceivingDocumentLine,
            ref int rowCount, object includeReportFormatting)
        {
            PopulateGoodsReceivingDocumentLine(entry,dr, goodsReceivingDocumentLine,includeReportFormatting);

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

        private static void PopulateGoodsReceivingDocumentLine(IConnectionManager entry, IDataReader dr, GoodsReceivingDocumentLine goodsReceivingDocumentLine, object includeReportFormatting)
        {
            goodsReceivingDocumentLine.GoodsReceivingDocumentID = (string)dr["GOODSRECEIVINGID"];
            goodsReceivingDocumentLine.PurchaseOrderID = (string)dr["PURCHASEORDERID"];
            goodsReceivingDocumentLine.PurchaseOrderLineNumber = (string)dr["PURCHASEORDERLINENUMBER"];
            goodsReceivingDocumentLine.LineNumber = (string)dr["LINENUMBER"];
            goodsReceivingDocumentLine.ReceivedQuantity = (decimal)dr["RECEIVEDQUANTITY"];
            goodsReceivingDocumentLine.ReceivedDate = (DateTime)dr["RECEIVEDDATE"];
            goodsReceivingDocumentLine.Posted = ((byte)dr["POSTED"] == 1);
            goodsReceivingDocumentLine.StoreID = (string)dr["STOREID"];
            goodsReceivingDocumentLine.StoreName = (string)dr["STORENAME"];
            goodsReceivingDocumentLine.purchaseOrderLine = new PurchaseOrderLine();
            PopulatePurchaseOrderLine(entry,dr, goodsReceivingDocumentLine.purchaseOrderLine,includeReportFormatting);
        }

        private static void PopulatePurchaseOrderLine(IConnectionManager entry, IDataReader dr, PurchaseOrderLine purchaseOrderLine, object includeReportFormatting)
        {
            purchaseOrderLine.PurchaseOrderID = (string) dr["PURCHASEORDERID"];
            purchaseOrderLine.LineNumber = (string) dr["PURCHASEORDERLINENUMBER"];
            purchaseOrderLine.ItemID = (string) dr["RETAILITEMID"];
            purchaseOrderLine.ItemName = (string) dr["ITEMNAME"];
            if (dr["ITEMDELTED"] != null && dr["ITEMDELTED"] is bool)
            {
                purchaseOrderLine.ItemDeleted = (bool)dr["ITEMDELTED"];
            }
            if (dr["ITEMTYPE"] != null && dr["ITEMTYPE"] is byte)
            {
                purchaseOrderLine.ItemType = (ItemTypeEnum)((byte)dr["ITEMTYPE"]);
            }
            purchaseOrderLine.VariantName = (string) dr["VARIANTNAME"];
            purchaseOrderLine.VendorItemID = (string) dr["VENDORITEMID"] == ""
                ? purchaseOrderLine.ItemID
                : (string) dr["VENDORITEMID"];

            purchaseOrderLine.UnitID = (string) dr["UNITID"];
            purchaseOrderLine.UnitName = (string) dr["UNITNAME"];
            purchaseOrderLine.Quantity = (decimal) dr["QUANTITY"];
            purchaseOrderLine.UnitPrice = (decimal) dr["PRICE"];
            purchaseOrderLine.DiscountAmount = (decimal) dr["DISCOUNTAMOUNT"];
            purchaseOrderLine.DiscountPercentage = (decimal) dr["DISCOUNTPERCENTAGE"];
            purchaseOrderLine.TaxAmount = (decimal) dr["TAXAMOUNT"];
            purchaseOrderLine.TaxCalculationMethod = (TaxCalculationMethodEnum) dr["TAXCALCULATIONMETHOD"];

            if ((bool) includeReportFormatting)
            {
                if (dr["MAXUNITDECIMALS"] == DBNull.Value)
                {
                    purchaseOrderLine.QuantityLimiter = entry.GetDecimalSetting(DecimalSettingEnum.Quantity);
                }
                else
                {
                    var maxDecimals = (int) dr["MAXUNITDECIMALS"];
                    var minDecimals = (int) dr["MINUNITDECIMALS"];

                    purchaseOrderLine.QuantityLimiter = new DecimalLimit(minDecimals, maxDecimals);
                }

                purchaseOrderLine.PriceLimiter = entry.GetDecimalSetting(DecimalSettingEnum.Prices);
                purchaseOrderLine.PercentageLimiter = entry.GetDecimalSetting(DecimalSettingEnum.DiscountPercent);
            }
        }

        public virtual RecordIdentifier GetPurchaseOrderIDForLine(IConnectionManager entry, RecordIdentifier lineID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"
                       SELECT PURCHASEORDERID FROM PURCHASEORDERLINE
                            WHERE LINENUMBER = @LINEID";

                MakeParam(cmd, "LINEID", (string)lineID);

                return (string)entry.Connection.ExecuteScalar(cmd);
            }
        }

        public virtual List<GoodsReceivingDocumentLine> GetGoodsReceivingDocumentLines(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID, bool includeReportFormatting = true)
        {
            return GetGoodsReceivingDocumentLines(entry, goodsReceivingDocumentID, GoodsReceivingDocumentLineSorting.ItemName, false, includeReportFormatting);
        }

        /// <summary>
        /// Gets a list of goods receiving document lines for a given goods receiving document ID. The list is sorted by the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="goodsReceivingDocumentID">The goods receiving document ID to get goods receiving document lines by</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>A list of goods receiving document lines for a given goods receiving document ID</returns>
        public List<GoodsReceivingDocumentLine> GetGoodsReceivingDocumentLines(
            IConnectionManager entry, 
            RecordIdentifier goodsReceivingDocumentID,
            GoodsReceivingDocumentLineSorting sortBy, 
            bool sortBackwards, 
            bool includeReportFormatting = true)
        {
            ValidateSecurity(entry);

            List<GoodsReceivingDocumentLine> result;
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                Condition condition = new Condition
                {
                    ConditionValue = "a.GOODSRECEIVINGID = @GOODSRECEIVINGID",
                    Operator = "AND"
                };
                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("GOODSRECEIVINGLINE", "a"),
                    QueryPartGenerator.InternalColumnGenerator(goodsReceivingColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(condition),
                    string.Empty);

                MakeParam(cmd, "GOODSRECEIVINGID", (string)goodsReceivingDocumentID);

                result = Execute<GoodsReceivingDocumentLine>(entry, cmd, CommandType.Text, includeReportFormatting, PopulateGoodsReceivingDocumentLine);
            }

            return result;
        }

        public List<GoodsReceivingDocumentLine> AdvancedSearch(IConnectionManager entry, GoodsReceivingDocumentLineSearch searchCriteria, GoodsReceivingDocumentLineSorting sortBy, bool sortBackwards, out int totalCount)
        {
            ValidateSecurity(entry);
            List<TableColumn> columns = new List<TableColumn>(goodsReceivingColumns);

            List<Condition> externalConditions = new List<Condition>();
            List<Condition> conditions = new List<Condition>();

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                columns.Add(new TableColumn
                {
                    ColumnName = $"ROW_NUMBER() OVER(ORDER BY { ResolveSort(sortBy, sortBackwards)})",
                    ColumnAlias = "ROW"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = $"COUNT(1) OVER ( ORDER BY { ResolveSort(sortBy, sortBackwards)} RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )",
                    ColumnAlias = "ROW_COUNT"
                });
                externalConditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "ss.ROW BETWEEN @ROWFROM AND @ROWTO"
                });
                MakeParam(cmd, "ROWFROM", searchCriteria.RecordsFrom, SqlDbType.Int);
                MakeParam(cmd, "ROWTO", searchCriteria.RecordsTo, SqlDbType.Int);

                if (!RecordIdentifier.IsEmptyOrNull(searchCriteria.DocumentID))
                {
                    conditions.Add(new Condition {Operator = "AND", ConditionValue = "A.GOODSRECEIVINGID = @GOODSRECEIVINGDOCUMENTID" });
                    MakeParam(cmd, "GOODSRECEIVINGDOCUMENTID", (string) searchCriteria.DocumentID);
                }
                if (searchCriteria.ItemNameSearch != null && searchCriteria.ItemNameSearch.Count == 1 && !string.IsNullOrEmpty(searchCriteria.ItemNameSearch[0]))
                {
                    string searchString = PreProcessSearchText(searchCriteria.ItemNameSearch[0], true, searchCriteria.DescriptionBeginsWith);
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue =
                            " (IT.ITEMNAME LIKE @DESCRIPTION OR IT.ITEMID LIKE @DESCRIPTION OR IT.VARIANTNAME LIKE @DESCRIPTION OR IT.NAMEALIAS LIKE @DESCRIPTION OR V.VENDORITEMID LIKE @DESCRIPTION) "
                    });

                    MakeParam(cmd, "DESCRIPTION", searchString);
                }
                else if (searchCriteria.ItemNameSearch != null && searchCriteria.ItemNameSearch.Count > 1)
                {
                    List<Condition> searchConditions = new List<Condition>();
                    for (int index = 0; index < searchCriteria.ItemNameSearch.Count; index++)
                    {
                        var searchToken = PreProcessSearchText(searchCriteria.ItemNameSearch[index], true, searchCriteria.DescriptionBeginsWith);

                        if (!string.IsNullOrEmpty(searchToken))
                        {
                            searchConditions.Add(new Condition
                            {
                                ConditionValue =
                                    $@" (IT.ITEMNAME LIKE @DESCRIPTION{index
                                        } 
                                        OR IT.ITEMID LIKE @DESCRIPTION{index
                                        } 
                                        OR IT.VARIANTNAME LIKE @DESCRIPTION{
                                        index
                                        } 
                                        OR IT.NAMEALIAS LIKE @DESCRIPTION{
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
               
                if (searchCriteria.VariantSearch != null && searchCriteria.VariantSearch.Count == 1 && !string.IsNullOrEmpty(searchCriteria.VariantSearch[0]))
                {
                    string searchString = PreProcessSearchText(searchCriteria.VariantSearch[0], true, searchCriteria.VariantSearchBeginsWith);
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue =
                            " IT.VARIANTNAME LIKE @VARIANT "
                    });

                    MakeParam(cmd, "VARIANT", searchString);
                }
                else if (searchCriteria.VariantSearch != null && searchCriteria.VariantSearch.Count > 1)
                {
                    List<Condition> searchConditions = new List<Condition>();
                    for (int index = 0; index < searchCriteria.VariantSearch.Count; index++)
                    {
                        var searchToken = PreProcessSearchText(searchCriteria.VariantSearch[index], true, searchCriteria.VariantSearchBeginsWith);

                        if (!string.IsNullOrEmpty(searchToken))
                        {
                            searchConditions.Add(new Condition
                            {
                                ConditionValue =
                                    $" IT.VARIANTNAME LIKE @VARIANT{index} ",
                                Operator = "AND"
                            });

                            MakeParam(cmd, $"VARIANT{index}", searchToken);
                        }
                    }
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue =
                            $" ({QueryPartGenerator.ConditionGenerator(searchConditions, true)}) "
                    });

                }
                if (searchCriteria.ReceivedTo != null && searchCriteria.ReceivedTo  != Date.Empty && searchCriteria.ReceivedFrom != null && searchCriteria.ReceivedFrom != Date.Empty)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = $"A.RECEIVEDDATE BETWEEN @DATAFROM AND @DATETO "
                    });

                    MakeParam(cmd, "DATEFROM", searchCriteria.ReceivedFrom.DateTime, SqlDbType.DateTime);
                    MakeParam(cmd, "DATETO", searchCriteria.ReceivedTo.DateTime, SqlDbType.DateTime);
                }
                else if (searchCriteria.ReceivedTo != null &&  searchCriteria.ReceivedTo != Date.Empty)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = $"A.RECEIVEDDATE < @DATETO "
                    });

                    MakeParam(cmd, "DATETO", searchCriteria.ReceivedTo.DateTime, SqlDbType.DateTime);
                }
                else if (searchCriteria.ReceivedFrom != null && searchCriteria.ReceivedFrom != Date.Empty)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = $"A.RECEIVEDDATE > @DATEFROM "
                    });

                    MakeParam(cmd, "DATEFROM", searchCriteria.ReceivedFrom.DateTime, SqlDbType.DateTime);
                }
                if (searchCriteria.Posted != null)
                {
                    if ((bool)searchCriteria.Posted)
                    {
                        conditions.Add(new Condition
                        {
                            Operator = "AND",
                            ConditionValue = $"A.POSTED = 1 "
                        });
                    }
                    else
                    {
                        conditions.Add(new Condition
                        {
                            Operator = "AND",
                            ConditionValue = $"A.POSTED = 0 "
                        });
                    }
                }
                if (searchCriteria.ReceivedQuantity != null)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = $"A.RECEIVEDQUANTITY {GetComparisonOperator(searchCriteria.ReceivedQuantityOperator) }  @RECEIVED "
                    });

                    MakeParam(cmd, "RECEIVED", searchCriteria.ReceivedQuantity, SqlDbType.Decimal);
                }
                if (searchCriteria.OrderedQuantity != null)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = $"PL.QUANTITY {GetComparisonOperator(searchCriteria.OrderedQuantityOperator) }  @ORDERED "
                    });

                    MakeParam(cmd, "ORDERED", searchCriteria.OrderedQuantity, SqlDbType.Decimal);
                }

                cmd.CommandText = string.Format(
                    QueryTemplates.PagingQuery("GOODSRECEIVINGLINE", "a","ss", searchCriteria.LimitResultTo),
                    QueryPartGenerator.ExternalColumnGenerator(columns),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    QueryPartGenerator.ConditionGenerator(externalConditions),
                    "ORDER BY " + ResolveSort(sortBy, sortBackwards).ToSortString(true)
                    );
                totalCount = 0;

                return Execute<GoodsReceivingDocumentLine, int>(entry, cmd, CommandType.Text, ref totalCount, true, PopulateGoodsReceivingDocumentLineWithCount);
            }
        }

        private string GetComparisonOperator(DoubleValueOperator doubleOperator)
        {
            switch (doubleOperator)
            {
                case DoubleValueOperator.Equals:
                    return "=";
                case DoubleValueOperator.GreaterThan:
                    return ">";
                case DoubleValueOperator.LessThan:
                    return "<";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Checks if a goods receiving document line with a given ID exists in the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="goodsReceivingDocumentLineID">The ID of the goods receiving document line to check for</param>
        /// <returns>Whether a goods receiving document line with a given ID exists in the database</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentLineID)
        {
            return RecordExists(entry, "GOODSRECEIVINGLINE", new[] { "GOODSRECEIVINGID", "PURCHASEORDERLINENUMBER", "LINENUMBER" }, goodsReceivingDocumentLineID);
        }

        private static bool LineNumberExists(IConnectionManager entry, RecordIdentifier lineNumber)
        {
            return RecordExists(entry, "GOODSRECEIVINGLINE", "LINENUMBER", lineNumber);
        }

        /// <summary>
        /// Deletes a goods receiving document line with a given ID
        /// </summary>
        /// <remarks>Requires the 'ManageGoodsReceivingDocuments' permission</remarks>
        /// <param name="entry">The entry into the database</param>
        /// <param name="goodsReceivingDocumentLineID">The ID of the goods receiving document line to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentLineID)
        {
            DeleteRecord(entry, "GOODSRECEIVINGLINE", new[] { "GOODSRECEIVINGID", "PURCHASEORDERLINENUMBER", "LINENUMBER" }, goodsReceivingDocumentLineID, BusinessObjects.Permission.ManageGoodsReceivingDocuments);      
        }

        /// <summary>
        /// Gets a goods receiving document line with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="goodsReceivingDocumentLineID">The ID of the goods receiving document line to get</param>
        /// <param name="storeID">The ID of the store that we are receiving at</param>
        /// <returns>A goods receiving document line with a given ID</returns>
        public virtual GoodsReceivingDocumentLine Get(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentLineID, RecordIdentifier storeID)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>
                {
                    new Condition
                    {
                        ConditionValue = "a.GOODSRECEIVINGID = @GOODSRECEIVINGID",
                        Operator = "AND"
                    },
                    new Condition
                    {
                        ConditionValue = "PURCHASEORDERLINENUMBER = @PURCHASEORDERLINENUMBER",
                        Operator = "AND"
                    },
                    new Condition
                    {
                        ConditionValue = "a.LINENUMBER = @LINENUMBER",
                        Operator = "AND"
                    },
                };
                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("GOODSRECEIVINGLINE", "a"),
                    QueryPartGenerator.InternalColumnGenerator(goodsReceivingColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);
           
                MakeParam(cmd, "GOODSRECEIVINGID", (string)goodsReceivingDocumentLineID[0]);
                MakeParam(cmd, "PURCHASEORDERLINENUMBER", (string)goodsReceivingDocumentLineID[1]);
                MakeParam(cmd, "LINENUMBER", (string)goodsReceivingDocumentLineID[2]);

                List<GoodsReceivingDocumentLine> result = Execute<GoodsReceivingDocumentLine>(entry, cmd, CommandType.Text, true ,PopulateGoodsReceivingDocumentLine);

                if (result.Count > 0)
                {
                    return result[0];
                }
            }

            return null;
        }

        /// <summary>
        /// Saves a given goods receiving document line into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="goodsReceivingDocumentLine">The goods receiving document line to save</param>
        public virtual void Save(IConnectionManager entry, GoodsReceivingDocumentLine goodsReceivingDocumentLine)
        {
            SqlServerStatement statement = new SqlServerStatement("GOODSRECEIVINGLINE", false);

            ValidateSecurity(entry, BusinessObjects.Permission.ManageGoodsReceivingDocuments);

            GoodsReceivingDocumentLine grdl = goodsReceivingDocumentLine;
            if (RecordIdentifier.IsEmptyOrNull(goodsReceivingDocumentLine.LineNumber) || !Exists(entry, goodsReceivingDocumentLine.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("GOODSRECEIVINGID", (string)grdl.GoodsReceivingDocumentID);
                statement.AddKey("PURCHASEORDERLINENUMBER", (string)grdl.PurchaseOrderLineNumber);
                if (RecordIdentifier.IsEmptyOrNull(goodsReceivingDocumentLine.LineNumber))
                {
                    goodsReceivingDocumentLine.LineNumber = DataProviderFactory.Instance.GenerateNumber<IGoodsReceivingDocumentLineData, GoodsReceivingDocumentLine>(entry);
                }
                statement.AddKey("LINENUMBER", (string)grdl.LineNumber);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("GOODSRECEIVINGID", (string)grdl.GoodsReceivingDocumentID);
                statement.AddCondition("PURCHASEORDERLINENUMBER", (string)grdl.PurchaseOrderLineNumber);
                statement.AddCondition("LINENUMBER", (string)grdl.LineNumber);
            }

            statement.AddField("RECEIVEDQUANTITY", grdl.ReceivedQuantity, SqlDbType.Decimal);
            statement.AddField("RECEIVEDDATE", grdl.ReceivedDate, SqlDbType.DateTime);
            statement.AddField("POSTED", grdl.Posted ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("STOREID", (string)grdl.StoreID);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual bool IsPosted(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentLineID)
        {
            GoodsReceivingDocumentLine grdl = Get(entry, goodsReceivingDocumentLineID, RecordIdentifier.Empty);

            return grdl.Posted;
        }

        public virtual void CreateGoodsReceivingDocumentLinesFromPurchaseOrder(IConnectionManager entry, RecordIdentifier purchaseOrderID)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.ManageGoodsReceivingDocuments);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO GOODSRECEIVINGLINE (GOODSRECEIVINGID, PURCHASEORDERLINENUMBER, LINENUMBER, STOREID, RECEIVEDQUANTITY, RECEIVEDDATE, POSTED, DATAAREAID)
SELECT P.PURCHASEORDERID, PL.LINENUMBER, PL.LINENUMBER, P.STOREID, PL.QUANTITY, GETDATE(), 0, PL.DATAAREAID FROM PURCHASEORDERS P
INNER JOIN PURCHASEORDERLINE PL ON P.PURCHASEORDERID = PL.PURCHASEORDERID
INNER JOIN RETAILITEM R ON R.ITEMID = PL.RETAILITEMID
WHERE P.PURCHASEORDERID = @PURCHASEORDERID AND P.DATAAREAID = @DATAAREAID AND R.DELETED = 0 AND R.ITEMTYPE != 2";

                MakeParam(cmd, "PURCHASEORDERID", (string)purchaseOrderID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
            }
        }

        public decimal GetReceivedItemQuantity(
            IConnectionManager entry,
            RecordIdentifier purchaseOrderID,  
            RecordIdentifier purchaseOrderLineID, 
            bool includeReportFormatting)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>
                {
                    new Condition
                    {
                        ConditionValue = "b.PURCHASEORDERID = @PURCHASEORDERID",
                        Operator = "AND"
                    },
                    new Condition
                    {
                        ConditionValue = "a.PURCHASEORDERLINENUMBER = @PURCHASEORDERLINEID",
                        Operator = "AND"
                    },
                    new Condition
                    {
                        ConditionValue = "a.POSTED = 1",
                        Operator = "AND"
                    }
                };
                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("GOODSRECEIVINGLINE", "a"),
                    QueryPartGenerator.InternalColumnGenerator(goodsReceivingColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

                MakeParam(cmd, "PURCHASEORDERID", (string)purchaseOrderID);
                MakeParam(cmd, "PURCHASEORDERLINEID", (string)purchaseOrderLineID);

                List<GoodsReceivingDocumentLine> lines = Execute<GoodsReceivingDocumentLine>(entry, cmd, CommandType.Text, true,PopulateGoodsReceivingDocumentLine);

                return lines.Sum(line => line.ReceivedQuantity);
            }
        }

        public List<InventoryTotals> GetReceivedTotals(IConnectionManager entry, int numberOfDocuments)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> totalColumns = new List<TableColumn>();
                totalColumns.Add(new TableColumn {ColumnName = "GOODSRECEIVINGID", TableAlias = "a"});
                totalColumns.Add(new TableColumn {ColumnName = "SUM(a.RECEIVEDQUANTITY)", ColumnAlias = "RECEIVEDQUANTITY" });

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("GOODSRECEIVINGLINE", "a", numberOfDocuments),
                    QueryPartGenerator.InternalColumnGenerator(totalColumns),
                    QueryPartGenerator.JoinGenerator(new List<Join>()),
                    QueryPartGenerator.ConditionGenerator(new Condition {ConditionValue = "a.DATAAREAID = @DATAAREAID", Operator = "AND"}),
                    "GROUP BY GOODSRECEIVINGID");

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Execute<InventoryTotals>(entry, cmd, CommandType.Text, PopulateReceivedTotals);
            }
        }

        private static void PopulateReceivedTotals(IDataReader dr, InventoryTotals total)
        {
            total.ID = (string)dr["GOODSRECEIVINGID"];
            total.Quantity = (decimal)dr["RECEIVEDQUANTITY"];
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return LineNumberExists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "GoodsReceivingLine"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "GOODSRECEIVINGLINE", "LINENUMBER", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}