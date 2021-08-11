using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Services.Datalayer.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.SqlDataProviders
{
    public class InventoryMigrators : SqlServerDataProviderBase, IInventoryMigrators
    {
        private static List<TableColumn> inventoryTransactionColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "GUID ", TableAlias = "A"},
            new TableColumn {ColumnName = "VARIANTNAME", TableAlias = "IT"},
            new TableColumn {ColumnName = "ISNULL(A.POSTINGDATE,'01.01.1900')", ColumnAlias = "POSTINGDATE"},
            new TableColumn {ColumnName = "ISNULL(A.ITEMID,'')", ColumnAlias = "ITEMID"},
            new TableColumn {ColumnName = "ISNULL(A.STOREID,'')", ColumnAlias = "STOREID"},
            new TableColumn {ColumnName = "ISNULL(A.ADJUSTMENT,'0')", ColumnAlias = "ADJUSTMENT"},
            new TableColumn {ColumnName = "ISNULL(A.UNITID,'')", ColumnAlias = "UNITID"},
            new TableColumn {ColumnName = "ISNULL(A.TYPE,'0')", ColumnAlias = "TYPE"},
            new TableColumn {ColumnName = "ISNULL(A.OFFERID,'')", ColumnAlias = "OFFERID"},
            new TableColumn {ColumnName = "ISNULL(A.COSTPRICEPERITEM,'0')", ColumnAlias = "COSTPRICEPERITEM"},
            new TableColumn
            {
                ColumnName = "ISNULL(A.SALESPRICEWITHOUTTAXPERITEM,'0')",
                ColumnAlias = "SALESPRICEWITHOUTTAXPERITEM"
            },
            new TableColumn
            {
                ColumnName = "ISNULL(A.SALESPRICEWITHTAXPERITEM,'0')",
                ColumnAlias = "SALESPRICEWITHTAXPERITEM"
            },
            new TableColumn {ColumnName = "ISNULL(A.REASONCODE,'')", ColumnAlias = "REASONCODE"},
            new TableColumn {ColumnName = "ISNULL(A.DISCOUNTAMOUNTPERITEM,'0')", ColumnAlias = "DISCOUNTAMOUNTPERITEM"},
            new TableColumn
            {
                ColumnName = "ISNULL(A.OFFERDISCOUNTAMOUNTPERITEM,'0')",
                ColumnAlias = "OFFERDISCOUNTAMOUNTPERITEM"
            },
            new TableColumn {ColumnName = "ISNULL(IT.ITEMTYPE, 0)", ColumnAlias = "ITEMTYPE"}
        };

        private static void PopulateInventoryTransaction(IDataReader dr, InventoryTransaction inventoryTransaction)
        {
            inventoryTransaction.Initialize((string)dr["ITEMID"], (ItemTypeEnum)((byte)dr["ITEMTYPE"]));
            inventoryTransaction.Guid = Guid.NewGuid();
            inventoryTransaction.PostingDate = (DateTime) dr["POSTINGDATE"];
            //inventoryTransaction.ItemID = (string) dr["ITEMID"];
            inventoryTransaction.VariantName = (string) dr["VARIANTNAME"];
            inventoryTransaction.StoreID = (string) dr["STOREID"];
            inventoryTransaction.Type = (InventoryTypeEnum) dr["TYPE"];
            inventoryTransaction.OfferID = (string) dr["OFFERID"];
            inventoryTransaction.AdjustmentUnitID = (string) dr["UNITID"];
            inventoryTransaction.Adjustment = (decimal) dr["ADJUSTMENT"];
            inventoryTransaction.CostPricePerItem = (decimal) dr["COSTPRICEPERITEM"];
            inventoryTransaction.SalesPriceWithTaxPerItem = (decimal) dr["SALESPRICEWITHTAXPERITEM"];
            inventoryTransaction.SalesPriceWithoutTaxPerItem = (decimal) dr["SALESPRICEWITHOUTTAXPERITEM"];
            inventoryTransaction.DiscountAmountPerItem = (decimal) dr["DISCOUNTAMOUNTPERITEM"];
            inventoryTransaction.OfferDiscountAmountPerItem = (decimal) dr["OFFERDISCOUNTAMOUNTPERITEM"];
            inventoryTransaction.ReasonCode = (string) dr["REASONCODE"];
        }


        private static List<Join> inventoryTransactionJoins = new List<Join>
        {
            new Join
            {
                Condition = " A.ITEMID = IT.ITEMID",
                JoinType = "LEFT OUTER",
                Table = "RETAILITEM",
                TableAlias = "IT"
            },
        };

        private static List<TableColumn> journalColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "JOURNALID ", TableAlias = "A"},
            new TableColumn {ColumnName = "LINENUM", TableAlias = "A"},
            new TableColumn {ColumnName = "TRANSDATE ", TableAlias = "A"},
            new TableColumn {ColumnName = "ITEMID ", TableAlias = "A"},
            new TableColumn {ColumnName = "ADJUSTMENT ", TableAlias = "A"},
            new TableColumn {ColumnName = "COSTPRICE ", TableAlias = "A"},
            new TableColumn {ColumnName = "PRICEUNIT", TableAlias = "A"},
            new TableColumn {ColumnName = "COSTMARKUP ", TableAlias = "A"},
            new TableColumn {ColumnName = "COSTAMOUNT ", TableAlias = "A"},
            new TableColumn {ColumnName = "SALESAMOUNT ", TableAlias = "A"},
            new TableColumn {ColumnName = "INVENTONHAND ", TableAlias = "A"},
            new TableColumn {ColumnName = "COUNTED ", TableAlias = "A"},
            new TableColumn {ColumnName = "POSTED ", TableAlias = "A"},
            new TableColumn {ColumnName = "POSTEDDATETIME ", TableAlias = "A",},
            new TableColumn {ColumnName = "ISNULL(rbo.NAME, '')  ", ColumnAlias = "STORENAME"},
            new TableColumn {ColumnName = "REASONREFRECID ", TableAlias = "A"},
            new TableColumn {ColumnName = "ISNULL(itr.REASONTEXT,'') ", ColumnAlias = "REASONTEXT"},
            new TableColumn {ColumnName = "ISNULL(a.UNITID,'')  ", ColumnAlias = "UNITID"},
            new TableColumn {ColumnName = "ISNULL(u.TXT,'')  ", ColumnAlias = "UNITDESCRIPTION"},
            new TableColumn {ColumnName = "ISNULL(u.UNITDECIMALS,0) ", ColumnAlias = "UNITDECIMALSMAX"},
            new TableColumn {ColumnName = "ISNULL(u.MINUNITDECIMALS,0) ", ColumnAlias = "UNITDECIMALSMIN"},
            new TableColumn {ColumnName = "ISNULL(inventUnit.UNITID,'')", ColumnAlias = "INVENTUNITID"},
            new TableColumn {ColumnName = "ISNULL(inventUnit.TXT,'')", ColumnAlias = "INVENTUNITDESCRIPTION"},
            new TableColumn {ColumnName = "ISNULL(it.ITEMNAME,'') ", ColumnAlias = "ITEMNAME"},
            new TableColumn {ColumnName = "STOREID", TableAlias = "IJT"},
            new TableColumn {ColumnName = "VARIANTNAME", TableAlias = "IT"},


        };

        private static void PopulateJournalInfo(IDataReader dr,
            InventoryJournalTransaction inventoryJournalTransactionInfo)
        {
            inventoryJournalTransactionInfo.JournalId = (string) dr["JOURNALID"];
            inventoryJournalTransactionInfo.LineNum = (string) dr["LINENUM"];
            inventoryJournalTransactionInfo.CostAmount = (decimal) dr["COSTAMOUNT"];
            inventoryJournalTransactionInfo.CostMarkup = (decimal) dr["COSTMARKUP"];
            inventoryJournalTransactionInfo.CostPrice = (decimal) dr["COSTPRICE"];
            inventoryJournalTransactionInfo.Counted = (decimal) dr["COUNTED"];
            inventoryJournalTransactionInfo.InventOnHandInInventoryUnits = (decimal) dr["INVENTONHAND"];
            inventoryJournalTransactionInfo.ItemId = (string) dr["ITEMID"];

            inventoryJournalTransactionInfo.PriceUnit = (decimal) dr["PRICEUNIT"];
            inventoryJournalTransactionInfo.Adjustment = (decimal) dr["ADJUSTMENT"];
            inventoryJournalTransactionInfo.ReasonId = (string) dr["REASONREFRECID"];
            inventoryJournalTransactionInfo.SalesAmount = (decimal) dr["SALESAMOUNT"];
            inventoryJournalTransactionInfo.TransDate = (DateTime) dr["TRANSDATE"];
            inventoryJournalTransactionInfo.Posted = Convert.ToBoolean(dr["POSTED"]);
            inventoryJournalTransactionInfo.PostedDateTime = (DateTime) dr["POSTEDDATETIME"];
            inventoryJournalTransactionInfo.UnitID = (string) dr["UNITID"];
            inventoryJournalTransactionInfo.InventoryUnitID = (string) dr["INVENTUNITID"];
            inventoryJournalTransactionInfo.VariantName = (string) dr["VARIANTNAME"];

            var unitDecimalsMax = (int) dr["UNITDECIMALSMAX"];
            var unitDecimalsMin = (int) dr["UNITDECIMALSMIN"];
            inventoryJournalTransactionInfo.UnitQuantityLimiter = new DecimalLimit(unitDecimalsMin, unitDecimalsMax);
        }


        private static List<Join> journaJoins = new List<Join>
        {
            new Join
            {
                Condition = "A.JOURNALID = IJT.JOURNALID",
                Table = "INVENTJOURNALTABLE",
                TableAlias = "IJT"
            },
            new Join
            {
                Condition = "A.REASONREFRECID = ITR.REASONID",
                JoinType = "LEFT OUTER",
                Table = "INVENTTRANSREASON",
                TableAlias = "ITR"
            },
            new Join
            {
                Condition = "IJT.STOREID = RBO.STOREID",
                JoinType = "LEFT OUTER",
                Table = "RBOSTORETABLE",
                TableAlias = "RBO"
            },
            new Join
            {
                Condition = "U.UNITID = A.UNITID",
                JoinType = "LEFT OUTER",
                Table = "UNIT",
                TableAlias = "U"
            },

            new Join
            {
                Condition = "IT.ITEMID = A.ITEMID",
                JoinType = "LEFT OUTER",
                Table = "RETAILITEM",
                TableAlias = "IT"
            },
            new Join
            {
                Condition = "INVENTUNIT.UNITID = IT.INVENTORYUNITID",
                JoinType = "LEFT OUTER",
                Table = "UNIT",
                TableAlias = "INVENTUNIT"
            },
            new Join
            {
                Condition = "ISUM.ITEMID = A.ITEMID AND ISUM.STOREID = IJT.STOREID",
                JoinType = "LEFT OUTER",
                Table = "INVENTSUM",
                TableAlias = "ISUM"
            },
        };

        private static List<TableColumn> inventoryTransferOrderColumns = new List<TableColumn>
        {

            new TableColumn {ColumnName = "ID ", TableAlias = "itl"},
            new TableColumn {ColumnName = "INVENTORYTRANSFERORDERID ", TableAlias = "itl"},
            new TableColumn {ColumnName = "ITEMID ", TableAlias = "itl"},
            new TableColumn {ColumnName = "ISNULL(it.ITEMNAME,'')", ColumnAlias = "ITEMNAME"},
            new TableColumn {ColumnName = "ISNULL(it.VARIANTNAME,'')", ColumnAlias = "VARIANTNAME"},
            new TableColumn {ColumnName = "UNITID ", TableAlias = "itl"},
            new TableColumn {ColumnName = "ISNULL(u.TXT, '')", ColumnAlias = "UNITNAME"},
            new TableColumn {ColumnName = "SENT ", TableAlias = "itl"},
            new TableColumn {ColumnName = "ISNULL(itl.QUANTITYSENT, 0)", ColumnAlias = "QUANTITYSENT"},
            new TableColumn {ColumnName = "ISNULL(itl.QUANTITYRECEIVED, 0)", ColumnAlias = "QUANTITYRECEIVED"},
            new TableColumn {ColumnName = "ISNULL(itrl.QUANTITYREQUESTED, 0)", ColumnAlias = "QUANTITYREQUESTED"}

        };

        private static List<Join> inventoryTransferOrderJoins = new List<Join>
        {
            new Join
            {
                Condition = " ITL.ITEMID = IT.ITEMID",
                JoinType = "LEFT OUTER",
                Table = "RETAILITEM",
                TableAlias = "IT"
            },
            new Join
            {
                Condition = "ITL.UNITID = U.UNITID",
                JoinType = "LEFT OUTER",
                Table = "UNIT",
                TableAlias = "U"
            },
            new Join
            {
                Condition = " ITL.INVENTORYTRANSFERORDERID = ITT.ID ",
                JoinType = "LEFT OUTER",
                Table = "INVENTORYTRANSFERORDER",
                TableAlias = "ITT"
            },
            new Join
            {
                Condition = "ITR.ID = ITT.INVENTORYTRANSFERREQUESTID",
                JoinType = "LEFT OUTER",
                Table = "INVENTORYTRANSFERREQUEST",
                TableAlias = "ITR"
            },
            new Join
            {
                Condition =
                    "ITRL.INVENTORYTRANSFERREQUESTID = ITR.ID AND ITL.ITEMID = ITRL.ITEMID AND ITRL.UNITID = ITL.UNITID",
                JoinType = "LEFT OUTER",
                Table = "INVENTORYTRANSFERREQUESTLINE",
                TableAlias = "ITRL"
            },
        };

        private static void PopulateTransferLine(IConnectionManager entry, IDataReader dr,
            InventoryTransferOrderLine inventoryTransferOrderLine, object param)
        {
            inventoryTransferOrderLine.ID = (Guid) dr["ID"];
            inventoryTransferOrderLine.InventoryTransferId = (string) dr["INVENTORYTRANSFERORDERID"];
            inventoryTransferOrderLine.ItemId = (string) dr["ITEMID"];
            inventoryTransferOrderLine.ItemName = (string) dr["ITEMNAME"];
            inventoryTransferOrderLine.VariantName = (string) dr["VARIANTNAME"];
            inventoryTransferOrderLine.UnitId = (string) dr["UNITID"];
            inventoryTransferOrderLine.UnitName = (string) dr["UNITNAME"];
            inventoryTransferOrderLine.Sent = (bool) dr["SENT"];
            inventoryTransferOrderLine.QuantitySent = (decimal) dr["QUANTITYSENT"];
            inventoryTransferOrderLine.QuantityReceived = (decimal) dr["QUANTITYRECEIVED"];
            inventoryTransferOrderLine.QuantityRequested = (decimal) dr["QUANTITYREQUESTED"];

        }

        private static List<TableColumn> inventoryTransferRequestOrderColumns = new List<TableColumn>
        {

            new TableColumn {ColumnName = "ID ", TableAlias = "itrl"},
            new TableColumn {ColumnName = "INVENTORYTRANSFERREQUESTID ", TableAlias = "itrl"},
            new TableColumn {ColumnName = "ITEMID ", TableAlias = "itrl"},
            new TableColumn {ColumnName = "ISNULL(it.ITEMNAME,'')", ColumnAlias = "ITEMNAME"},
            new TableColumn {ColumnName = "ISNULL(it.VARIANTNAME,'')", ColumnAlias = "VARIANTNAME"},
            new TableColumn {ColumnName = "UNITID ", TableAlias = "itrl"},
            new TableColumn {ColumnName = "ISNULL(u.TXT, '')", ColumnAlias = "UNITNAME"},
            new TableColumn {ColumnName = "QUANTITYREQUESTED ", TableAlias = "itrl"},
            new TableColumn {ColumnName = "SENT", TableAlias = "itrl"},

        };

        private static List<Join> inventoryTransferRequestOrderJoins = new List<Join>
        {
            new Join
            {
                Condition = " ITRL.ITEMID = it.ITEMID ",
                JoinType = "LEFT OUTER",
                Table = "RETAILITEM",
                TableAlias = "IT"
            },
            new Join
            {
                Condition = "ITRL.UNITID = U.UNITID",
                JoinType = "LEFT OUTER",
                Table = "UNIT",
                TableAlias = "U"
            },
        };

        private static void PopulateTransferRequestLine(IConnectionManager entry, IDataReader dr,
            InventoryTransferRequestLine inventoryTransferRequestLine, object param)
        {
            inventoryTransferRequestLine.ID = (Guid) dr["ID"];
            inventoryTransferRequestLine.InventoryTransferRequestId = (string) dr["INVENTORYTRANSFERREQUESTID"];
            inventoryTransferRequestLine.ItemId = (string) dr["ITEMID"];
            inventoryTransferRequestLine.ItemName = (string) dr["ITEMNAME"];
            inventoryTransferRequestLine.VariantName = (string) dr["VARIANTNAME"];
            inventoryTransferRequestLine.UnitId = (string) dr["UNITID"];
            inventoryTransferRequestLine.UnitName = (string) dr["UNITNAME"];
            inventoryTransferRequestLine.QuantityRequested = (decimal) dr["QUANTITYREQUESTED"];
            inventoryTransferRequestLine.Sent = (bool) dr["SENT"];


        }

        private static List<TableColumn> purchaseOrderColumns = new List<TableColumn>
        {

            new TableColumn {ColumnName = "PURCHASEORDERID ", TableAlias = "t"},
            new TableColumn {ColumnName = "LINENUMBER ", TableAlias = "t"},
            new TableColumn {ColumnName = "RETAILITEMID ", TableAlias = "t"},
            new TableColumn {ColumnName = "VENDORITEMID ", TableAlias = "v"},
            new TableColumn {ColumnName = "UNITID ", TableAlias = "t"},
            new TableColumn {ColumnName = "ISNULL(u.TXT, '') as UNITNAME "},
            new TableColumn {ColumnName = "UNITDECIMALS", TableAlias = "u", ColumnAlias = "MAXUNITDECIMALS"},
            new TableColumn {ColumnName = "ISNULL(u.MINUNITDECIMALS,0)", ColumnAlias = "MINUNITDECIMALS"},
            new TableColumn {ColumnName = "QUANTITY", TableAlias = "t"},
            new TableColumn {ColumnName = "PRICE", TableAlias = "t"},
            new TableColumn {ColumnName = "ISNULL(a.ITEMNAME,'')", ColumnAlias = "ITEMNAME"},
            new TableColumn {ColumnName = "ISNULL(a.VARIANTNAME,'')", ColumnAlias = "VARIANTNAME"},
            new TableColumn {ColumnName = "ISNULL(t.DISCOUNTAMOUNT,0)", ColumnAlias = "DISCOUNTAMOUNT"},
            new TableColumn {ColumnName = "ISNULL(t.DISCOUNTPERCENTAGE,0)", ColumnAlias = "DISCOUNTPERCENTAGE"},
            new TableColumn {ColumnName = "ISNULL(t.TAXAMOUNT,0)", ColumnAlias = "TAXAMOUNT"},
            new TableColumn {ColumnName = "ISNULL(t.TAXCALCULATIONMETHOD,0)", ColumnAlias = "TAXCALCULATIONMETHOD"}

        };

        private static List<Join> purchaseOrderJoins = new List<Join>
        {
            new Join
            {
                Condition = "p.PURCHASEORDERID = t.PURCHASEORDERID",
                JoinType = "LEFT OUTER",
                Table = "PURCHASEORDERS",
                TableAlias = "P"
            },
            new Join
            {
                Condition = "T.UNITID = U.UNITID",
                JoinType = "LEFT OUTER",
                Table = "UNIT",
                TableAlias = "U"
            },
            new Join
            {
                Condition = " T.RETAILITEMID = V.RETAILITEMID and P.VENDORID = V.VENDORID ",
                JoinType = "LEFT OUTER",
                Table = "VENDORITEMS",
                TableAlias = "V"
            },
            new Join
            {
                Condition = "A.ITEMID = T.RETAILITEMID",
                JoinType = "LEFT OUTER",
                Table = "RETAILITEM",
                TableAlias = "A"
            },
        };

        private static void PopulatePurchaseOrderLine(IDataReader dr, PurchaseOrderLine purchaseOrderLine)
        {
            purchaseOrderLine.PurchaseOrderID = (string) dr["PURCHASEORDERID"];
            purchaseOrderLine.LineNumber = (string) dr["LINENUMBER"];
            purchaseOrderLine.ItemID = (string) dr["RETAILITEMID"];
            purchaseOrderLine.ItemName = (string) dr["ITEMNAME"];
            purchaseOrderLine.VendorItemID = dr["VARIANTNAME"] == DBNull.Value || (string) dr["VARIANTNAME"] == ""
                ? purchaseOrderLine.ItemID
                : (string) dr["VARIANTNAME"];
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


        }

        private static List<TableColumn> vendorColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "INTERNALID" , TableAlias = "A"},
            new TableColumn {ColumnName = "ISNULL(b.VARIANTNAME,'')" , ColumnAlias = "VARIANTNAME"},
            new TableColumn {ColumnName = "ITEMPRICE" , TableAlias = "A"},
            new TableColumn {ColumnName = "VENDORITEMID" , TableAlias = "A"},
            new TableColumn {ColumnName = "ISNULL(b.ITEMID,'')" , ColumnAlias = "RETAILITEMID"},
            new TableColumn {ColumnName = "UNITID" , TableAlias = "A"},
            new TableColumn {ColumnName = "VENDORID" , TableAlias = "A"},
            new TableColumn {ColumnName = "ISNULL(a.LASTORDERDATE,'01.01.1900')" , ColumnAlias = "LASTORDERDATE"},


        };

        private static List<Join> vendorJoins = new List<Join>
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
                Condition = "V.ACCOUNTNUM= A.VENDORID",
                JoinType = "LEFT OUTER",
                Table = "VENDTABLE",
                TableAlias = "V"
            },


        };

        private static void PopulateMinimumVendorItem(IDataReader dr, VendorItem vendorItem)
        {
            vendorItem.ID = (string)dr["INTERNALID"];
            vendorItem.VendorItemID = (string)dr["VENDORITEMID"];
            vendorItem.RetailItemID = (string)dr["RETAILITEMID"];
            vendorItem.VariantName = (string)dr["VARIANTNAME"];

            vendorItem.UnitID = (string)dr["UNITID"];
            vendorItem.VendorID = (string)dr["VENDORID"];
            vendorItem.LastItemPrice = (decimal)dr["ITEMPRICE"];
            vendorItem.LastOrderDate = Date.FromAxaptaDate(dr["LASTORDERDATE"]);
        }

        public virtual List<InventoryJournalTransaction> GetJournalTransactionListForVariant(IConnectionManager entry,
            RecordIdentifier variantID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition {Operator = "AND", ConditionValue = "A.VARIANTID = @variantID"});

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("INVENTJOURNALTRANS", "A"),
                    QueryPartGenerator.InternalColumnGenerator(journalColumns),
                    QueryPartGenerator.JoinGenerator(journaJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );

                MakeParam(cmd, "variantID", (string) variantID);

                return Execute<InventoryJournalTransaction>(entry, cmd, CommandType.Text, PopulateJournalInfo);
            }
        }

        public List<InventoryTransaction> GetInventoryTransactionsForVariant(IConnectionManager entry,
            RecordIdentifier variantID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition {Operator = "AND", ConditionValue = "A.VARIANTID = @variantID"});



                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("INVENTTRANS", "A"),
                    QueryPartGenerator.InternalColumnGenerator(inventoryTransactionColumns),
                    QueryPartGenerator.JoinGenerator(inventoryTransactionJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );




                MakeParam(cmd, "variantID", (string) variantID);

                //MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
                //MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);


                return Execute<InventoryTransaction>(entry, cmd, CommandType.Text, PopulateInventoryTransaction);
            }
        }

        public List<InventoryTransferOrderLine> GetInventoryTransferOrderForVariant(
            IConnectionManager entry,
            RecordIdentifier variantID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition {Operator = "AND", ConditionValue = "ITL.VARIANTID = @variantID"});


                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("INVENTORYTRANSFERORDERLINE", "ITL"),
                    QueryPartGenerator.InternalColumnGenerator(inventoryTransferOrderColumns),
                    QueryPartGenerator.JoinGenerator(inventoryTransferOrderJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );

                MakeParam(cmd, "variantID", (string) variantID);


                return Execute<InventoryTransferOrderLine>(entry, cmd, CommandType.Text, null, PopulateTransferLine);
            }

        }

        public List<InventoryTransferRequestLine> GetInventoryTransferRequestListForVariant(
            IConnectionManager entry,
            RecordIdentifier variantID)
        {
            List<InventoryTransferRequestLine> result;

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition {Operator = "AND", ConditionValue = "ITRL.VARIANTID = @variantID"});

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("INVENTORYTRANSFERREQUESTLINE", "ITRL"),
                    QueryPartGenerator.InternalColumnGenerator(inventoryTransferRequestOrderColumns),
                    QueryPartGenerator.JoinGenerator(inventoryTransferRequestOrderJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );


                MakeParam(cmd, "variantID", (string) variantID);

                result = Execute<InventoryTransferRequestLine>(entry, cmd, CommandType.Text, null,
                    PopulateTransferRequestLine);
            }

            return result;
        }

        public virtual List<PurchaseOrderLine> GetPurchaseOrderLinesForVariant(IConnectionManager entry, RecordIdentifier variantID)
        {
            ValidateSecurity(entry);

            List<PurchaseOrderLine> result;
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition {Operator = "AND", ConditionValue = "t.VARIANTID = @variantID"});

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("PURCHASEORDERLINE", "t"),
                    QueryPartGenerator.InternalColumnGenerator(purchaseOrderColumns),
                    QueryPartGenerator.JoinGenerator(purchaseOrderJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );


                MakeParam(cmd, "variantID", (string) variantID.PrimaryID);



                result = Execute<PurchaseOrderLine>(entry, cmd, CommandType.Text, PopulatePurchaseOrderLine);
            }

            return result;
        }

        public virtual void DropVariantIDColumnFromInventSum(IConnectionManager entry)
        {
            ValidateSecurity(entry);


            using (var cmd = entry.Connection.CreateCommand())
            {


                cmd.CommandText = @"
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'INVENTSUM' AND  COLUMN_NAME  = 'VARIANTID')
BEGIN
	Delete from INVENTSUM where Variantid <> ''
END";

                entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
            }


        }

        public void SaveInventoryTransaction(IConnectionManager entry, InventoryTransaction inventoryTransaction)
        {
            var statement = new SqlServerStatement("INVENTTRANS");

            // Add every permission to the array that gives the user access to this operation.
            ValidateSecurity(entry,
                new[]
                {
                    LSOne.DataLayer.BusinessObjects.Permission.PostStatement,
                    DataLayer.BusinessObjects.Permission.EditInventoryAdjustments
                });


            statement.StatementType = StatementType.Insert;
            statement.AddField("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddKey("GUID", (Guid) inventoryTransaction.Guid, SqlDbType.UniqueIdentifier);

            statement.AddField("POSTINGDATE", inventoryTransaction.PostingDate, SqlDbType.DateTime);
            statement.AddField("ITEMID", (string) inventoryTransaction.ItemID);
            statement.AddField("VARIANTID", string.Empty);
            statement.AddField("STOREID", (string) inventoryTransaction.StoreID);
            statement.AddField("TYPE", (int) inventoryTransaction.Type, SqlDbType.Int);
            statement.AddField("OFFERID", inventoryTransaction.OfferID);
            statement.AddField("ADJUSTMENT", inventoryTransaction.Adjustment, SqlDbType.Decimal);
            statement.AddField("ADJUSTMENTININVENTORYUNIT", inventoryTransaction.AdjustmentInInventoryUnit,
                SqlDbType.Decimal);
            statement.AddField("COSTPRICEPERITEM", inventoryTransaction.CostPricePerItem, SqlDbType.Decimal);
            statement.AddField("SALESPRICEWITHTAXPERITEM", inventoryTransaction.SalesPriceWithTaxPerItem,
                SqlDbType.Decimal);
            statement.AddField("SALESPRICEWITHOUTTAXPERITEM", inventoryTransaction.SalesPriceWithoutTaxPerItem,
                SqlDbType.Decimal);
            statement.AddField("DISCOUNTAMOUNTPERITEM", inventoryTransaction.DiscountAmountPerItem, SqlDbType.Decimal);
            statement.AddField("OFFERDISCOUNTAMOUNTPERITEM", inventoryTransaction.OfferDiscountAmountPerItem,
                SqlDbType.Decimal);
            statement.AddField("REASONCODE", (string) inventoryTransaction.ReasonCode);
            statement.AddField("UNITID", (string) inventoryTransaction.AdjustmentUnitID);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void SaveInventoryJournal(IConnectionManager entry,
            InventoryJournalTransaction inventoryJournalTrans)
        {
            var statement = new SqlServerStatement("INVENTJOURNALTRANS");


            if (inventoryJournalTrans.LineNum == RecordIdentifier.Empty)
            {
                inventoryJournalTrans.LineNum =
                    DataProviderFactory.Instance
                        .GenerateNumber<IInventoryJournalTransactionData, InventoryJournalTransaction>(entry);
            }


            statement.StatementType = StatementType.Update;
            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("JOURNALID", (string) inventoryJournalTrans.JournalId);
            statement.AddCondition("LINENUM", (string) inventoryJournalTrans.LineNum);

            statement.AddField("POSTED", inventoryJournalTrans.Posted, SqlDbType.Int);
            statement.AddField("POSTEDDATETIME", DateTime.Now, SqlDbType.DateTime);
            statement.AddField("TRANSDATE", inventoryJournalTrans.TransDate, SqlDbType.DateTime);
            statement.AddField("ITEMID", (string) inventoryJournalTrans.ItemId);
            statement.AddField("COSTPRICE", inventoryJournalTrans.CostPrice, SqlDbType.Decimal);
            statement.AddField("PRICEUNIT", inventoryJournalTrans.PriceUnit, SqlDbType.Decimal);
            statement.AddField("COSTMARKUP", inventoryJournalTrans.CostMarkup, SqlDbType.Decimal);
            statement.AddField("COSTAMOUNT", inventoryJournalTrans.CostAmount, SqlDbType.Decimal);
            statement.AddField("SALESAMOUNT", inventoryJournalTrans.SalesAmount, SqlDbType.Decimal);
            statement.AddField("VARIANTID", string.Empty);

            statement.AddField("INVENTONHAND", inventoryJournalTrans.InventOnHandInInventoryUnits, SqlDbType.Decimal);
            statement.AddField("COUNTED", inventoryJournalTrans.Counted, SqlDbType.Decimal);
            statement.AddField("ADJUSTMENT", inventoryJournalTrans.Adjustment, SqlDbType.Decimal);

            statement.AddField("REASONREFRECID", (string) inventoryJournalTrans.ReasonId);
            statement.AddField("UNITID", (string) inventoryJournalTrans.UnitID);

            entry.Connection.ExecuteStatement(statement);

            //List<InventoryJournalTransaction> journalTransLines = GetJournalTransactionList(entry, inventoryJournalTrans.ID);

            var currentJournal = Providers.InventoryAdjustmentData.Get(entry, inventoryJournalTrans.ID);
            Providers.InventoryAdjustmentData.Save(entry, currentJournal);
        }

        public virtual void SaveInventoryTransferOrder(IConnectionManager entry,
            InventoryTransferOrderLine inventoryTransferOrderLine)
        {
            var statement = new SqlServerStatement("INVENTORYTRANSFERORDERLINE", false);
            // Dont' create replication actions because replication is handled through Site Service

            if (inventoryTransferOrderLine.ID == "" || inventoryTransferOrderLine.ID.IsEmpty)
            {
                inventoryTransferOrderLine.ID = Guid.NewGuid();
            }


            statement.StatementType = StatementType.Update;
            statement.AddCondition("ID", (Guid) inventoryTransferOrderLine.ID, SqlDbType.UniqueIdentifier);
            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);

            statement.AddField("INVENTORYTRANSFERORDERID", (string) inventoryTransferOrderLine.InventoryTransferId);
            statement.AddField("ITEMID", (string) inventoryTransferOrderLine.ItemId);
            statement.AddField("UNITID", (string) inventoryTransferOrderLine.UnitId);
            statement.AddField("SENT", inventoryTransferOrderLine.Sent, SqlDbType.Bit);
            statement.AddField("QUANTITYSENT", inventoryTransferOrderLine.QuantitySent, SqlDbType.Decimal);
            statement.AddField("QUANTITYRECEIVED", inventoryTransferOrderLine.QuantityReceived, SqlDbType.Decimal);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void SaveInventoryTransferRequest(IConnectionManager entry,
            InventoryTransferRequestLine inventoryTransferRequestLine)
        {
            var statement = new SqlServerStatement("INVENTORYTRANSFERREQUESTLINE", false);
            // Dont' create replication actions because replication is handled through Site Service

            if (inventoryTransferRequestLine.ID == "" || inventoryTransferRequestLine.ID.IsEmpty)
            {
                inventoryTransferRequestLine.ID = Guid.NewGuid();
            }


            statement.StatementType = StatementType.Update;
            statement.AddCondition("ID", (Guid) inventoryTransferRequestLine.ID, SqlDbType.UniqueIdentifier);
            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);


            statement.AddField("INVENTORYTRANSFERREQUESTID",
                (string) inventoryTransferRequestLine.InventoryTransferRequestId);
            statement.AddField("ITEMID", (string) inventoryTransferRequestLine.ItemId);
            statement.AddField("UNITID", (string) inventoryTransferRequestLine.UnitId);
            statement.AddField("QUANTITYREQUESTED", inventoryTransferRequestLine.QuantityRequested, SqlDbType.Decimal);
            statement.AddField("SENT", inventoryTransferRequestLine.Sent, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void SavePurchaseOrderLines(IConnectionManager entry, PurchaseOrderLine purchaseOrderLine)
        {
            var statement = new SqlServerStatement("PURCHASEORDERLINE");

            statement.StatementType = StatementType.Update;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("PURCHASEORDERID", (string) purchaseOrderLine.PurchaseOrderID);
            statement.AddCondition("LINENUMBER", (string) purchaseOrderLine.LineNumber);

            statement.AddField("RETAILITEMID", purchaseOrderLine.ItemID);
            statement.AddField("VENDORITEMID", purchaseOrderLine.VendorItemID);
            statement.AddField("UNITID", purchaseOrderLine.UnitID);
            statement.AddField("QUANTITY", purchaseOrderLine.Quantity, SqlDbType.Decimal);
            statement.AddField("PRICE", purchaseOrderLine.UnitPrice, SqlDbType.Decimal);
            statement.AddField("DISCOUNTAMOUNT", purchaseOrderLine.DiscountAmount, SqlDbType.Decimal);
            statement.AddField("DISCOUNTPERCENTAGE", purchaseOrderLine.DiscountPercentage, SqlDbType.Decimal);
            statement.AddField("TAXAMOUNT", purchaseOrderLine.TaxAmount, SqlDbType.Decimal);
            statement.AddField("TAXCALCULATIONMETHOD", (int) purchaseOrderLine.TaxCalculationMethod, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }


        /// <summary>
        /// Gets all vendor items for a given vendor ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="variantID">ID of the variant to fetch records for</param>
        /// <returns>All vendors in the database</returns>
        public virtual List<VendorItem> GetVendorItemsForVariant(IConnectionManager entry, RecordIdentifier variantID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>
                {
                    new Condition {ConditionValue = " A.VARIANTID = @variantID ", Operator = "AND"},
                };

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("VENDORITEMS", "A"),
                    QueryPartGenerator.InternalColumnGenerator(vendorColumns),
                    QueryPartGenerator.JoinGenerator(vendorJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );

                MakeParam(cmd, "variantID", (string)variantID);

                return Execute<VendorItem>(entry, cmd, CommandType.Text, PopulateMinimumVendorItem);
            }
        }

        /// <summary>
        /// Saves a vendor item to the database. If the record does not exist then a Insert is done, else it executes a update statement.
        /// </summary>
        /// <remarks>Edit vendor permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorItem">The vendor item to be saved</param>
        public virtual void SaveVendorItem(IConnectionManager entry, VendorItem vendorItem)
        {
            var statement = new SqlServerStatement("VENDORITEMS");

            statement.StatementType = StatementType.Update;

            statement.AddCondition("INTERNALID", (string) vendorItem.ID);

            statement.AddField("VENDORITEMID", (string) vendorItem.VendorItemID);
            statement.AddField("RETAILITEMID", (string) vendorItem.RetailItemID);
            statement.AddField("UNITID", (string) vendorItem.UnitID);
            statement.AddField("VENDORID", (string) vendorItem.VendorID);
            statement.AddField("ITEMPRICE", vendorItem.LastItemPrice, SqlDbType.Decimal);
            statement.AddField("LASTORDERDATE", vendorItem.LastOrderDate.ToAxaptaSQLDate(), SqlDbType.DateTime);

            entry.Connection.ExecuteStatement(statement);
        }


    }

}
