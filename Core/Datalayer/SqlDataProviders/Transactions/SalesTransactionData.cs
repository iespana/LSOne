using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Transactions
{
    public class SalesTransactionData : SqlServerDataProviderBase, ISalesTransactionData
    {
        private const string BaseSql =
            @"SELECT DISTINCT t.TRANSACTIONID, t.STORE, t.TERMINALID,t.ITEMTYPE,ISNULL(t.GIFTCARD,0) as GIFTCARD,
            ISNULL(t.PRESCRIPTIONID,'') as PRESCRIPTIONID,ISNULL(t.PUMPID,0) as PUMPID,ISNULL(t.SOURCELINENUM,0) as SOURCELINENUM,
            ISNULL(t.ITEMID,'') as ITEMID,t.LINENUM,ISNULL(t.CREDITMEMONUMBER,'') as CREDITMEMONUMBER,ISNULL(t.PRICE,0.0) as PRICE,
            ISNULL(t.COMMENT,'') as COMMENT,ISNULL(t.DOSAGETYPE,'') as DOSAGETYPE,ISNULL(t.DOSAGESTRENGTHUNIT,'') as DOSAGESTRENGTHUNIT,
            ISNULL(t.DOSAGESTRENGTH,0.0) as DOSAGESTRENGTH,ISNULL(t.DOSAGEUNITQUANTITY,0.0) as DOSAGEUNITQUANTITY, ISNULL(t.TRANSACTIONSTATUS,0) as TRANSACTIONSTATUS, 
            ISNULL(t.BARCODE,'') as BARCODE,ISNULL(t.TAXGROUP,'') as TAXGROUP,ISNULL(t.TAXAMOUNT,0.0) as TAXAMOUNT,ISNULL(t.NETPRICE,0.0) as NETPRICE, 
            ISNULL(t.COSTAMOUNT,0) as COSTAMOUNT, ISNULL(rtt.CUSTACCOUNT,'') as CUSTACCOUNT, 
            ISNULL(t.NETAMOUNT,0.0) as NETAMOUNT,t.NETAMOUNTINCLTAX,ISNULL(t.QTY,0.0) as QTY,t.TRANSDATE,ISNULL(t.ITEMIDSCANNED,0) as ITEMIDSCANNED, 
            ISNULL(t.STAFF,'') as STAFF,ISNULL(t.UNIT,'') as UNIT,ISNULL(t.UNITQTY,1.0) as UNITQTY,ISNULL(t.INVENTSERIALID,'') as INVENTSERIALID, 
            ISNULL(t.RFIDTAGID,'') as RFIDTAGID,t.TAXINCLINPRICE,ISNULL(t.VARIANTID,'') as VARIANTID,ISNULL(t.INVENTBATCHID,'') as INVENTBATCHID, 
            t.INVENTBATCHEXPDATE,ISNULL(t.OILTAX,0.0) as OILTAX,ISNULL(t.RETURNLINEID,0.0) as RETURNLINEID,ISNULL(t.RETURNQTY,0.0) as RETURNQTY, 
            ISNULL(t.LINKEDITEMNOTORIGINAL,0) as LINKEDITEMNOTORIGINAL,ISNULL(t.ORIGINALOFLINKEDITEMLIST,0.0) as ORIGINALOFLINKEDITEMLIST, 
            ISNULL(t.RETURNTRANSACTIONID,'') as RETURNTRANSACTIONID,ISNULL(t.ISINFOCODEITEM,0) as ISINFOCODEITEM, ISNULL(t.ITEMDEPARTMENTID,'') as ITEMDEPARTMENTID, 
            ISNULL(t.ITEMGROUPID,'') as ITEMGROUPID,ISNULL(t.PRICEINBARCODE,0) as PRICEINBARCODE,ISNULL(t.PRICECHANGE,0) as PRICECHANGE, 
            ISNULL(t.WEIGHTMANUALLYENTERED,0) as WEIGHTMANUALLYENTERED, ISNULL(t.LINEWASDISCOUNTED,0) as LINEWASDISCOUNTED,ISNULL(t.SCALEITEM,0) as SCALEITEM,
            ISNULL(t.WEIGHTITEM,0) as WEIGHTITEM, t.LIMITATIONMASTERID, ISNULL(t.PRICEUNIT,0.0) as PRICEUNIT, 
            ISNULL(t.DISCAMOUNT,0) as DISCAMOUNT, ISNULL(t.WHOLEDISCAMOUNTWITHTAX,0) as WHOLEDISCAMOUNTWITHTAX, 
            ISNULL(t.TOTALDISCAMOUNT,0.0) as TOTALDISCAMOUNT,ISNULL(t.TOTALDISCPCT,0.0) as TOTALDISCPCT,ISNULL(TOTALDISCAMOUNTWITHTAX,0.0) as TOTALDISCAMOUNTWITHTAX, 
            ISNULL(t.LINEDSCAMOUNT,0.0) as LINEDSCAMOUNT,ISNULL(t.LINEDISCPCT,0.0) as LINEDISCPCT,ISNULL(t.LINEDISCAMOUNTWITHTAX,0.0) as LINEDISCAMOUNTWITHTAX,
            ISNULL(t.PERIODICDISCTYPE,0) as PERIODICDISCTYPE, ISNULL(t.PERIODICDISCAMOUNT,0.0) as PERIODICDISCAMOUNT, 
            ISNULL(t.PERIODICDISCAMOUNTWITHTAX,0.0) as PERIODICDISCAMOUNTWITHTAX, ISNULL(t.DISCOFFERID,'') as DISCOFFERID, ISNULL(t.DESCRIPTION,'') as DESCRIPTION 
            FROM RBOTRANSACTIONSALESTRANS t 
            Join RBOTRANSACTIONTABLE rtt on t.TRANSACTIONID = rtt.TRANSACTIONID and t.STORE = rtt.STORE AND t.TERMINALID = rtt.TERMINAL AND t.DATAAREAID = rtt.DATAAREAID ";

        public virtual List<SalesTransaction> GetRetailTransactionItems(IConnectionManager entry, RecordIdentifier transactionId, RecordIdentifier storeId, RecordIdentifier terminalId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                //TODO WTF??
                cmd.CommandText =
                       BaseSql +
                       "Left outer join retailitem i on t.ITEMID = i.ITEMID " +
                        "where t.DATAAREAID = @dataAreaID and t.TRANSACTIONID = @transactionID and t.STORE = @storeID and t.TERMINALID = @terminalID and t.TRANSACTIONSTATUS = 0 " +
                        "order by t.LINENUM";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "transactionID", (string)transactionId);
                MakeParam(cmd, "storeID", (string)storeId);
                MakeParam(cmd, "terminalID", (string)terminalId);

                return Execute<SalesTransaction>(entry, cmd, CommandType.Text, PopulateTransaction);
            }
        }

        public List<SalesTransaction> GetList(IConnectionManager entry, RecordIdentifier itemID,
            int rowFrom, int rowTo,
            string storeID, DateTime startDate, DateTime endDate)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                var sqlCmd = BaseSql +
                             "WHERE t.DATAAREAID = @dataAreaId AND t.ITEMID = @itemId AND t.TRANSDATE >= @startDate AND t.TRANSDATE <= @endDate";
                if (!string.IsNullOrEmpty(storeID))
                    sqlCmd += " AND t.STORE = @storeID ";
                sqlCmd += " ORDER BY (t.TRANSDATE)";

                cmd.CommandText = sqlCmd;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemId", (string)itemID);
                MakeParam(cmd, "startDate", startDate < new DateTime(1900, 1, 1) ? new DateTime(1900, 1, 1) : startDate, SqlDbType.DateTime);
                MakeParam(cmd, "endDate", endDate, SqlDbType.DateTime);

                //MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
                //MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);


                if (!string.IsNullOrEmpty(storeID))
                    MakeParam(cmd, "storeID", storeID);

                return Execute<SalesTransaction>(entry, cmd, CommandType.Text, PopulateTransaction);
            }
        }

        public List<SalesTransaction> GetDiscountedItemsForCustomer(
            IConnectionManager entry, 
            string customerId, 
            DateTime startDate, 
            DateTime endDate)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                var sqlCmd = BaseSql +
                             @"JOIN RBOTRANSACTIONDISCOUNTTRANS rt on rt.TRANSACTIONID = t.TRANSACTIONID and 
                                rt.STORE = t.STORE and rt.TERMINAL = t.TERMINALID and rt.LINENUM = t.LINENUM and rt.DISCOUNTTYPE = @customerDiscountType
                             WHERE t.DATAAREAID = @dataAreaId AND t.TRANSDATE >= @startDate AND t.TRANSDATE <= @endDate
                                and rtt.CUSTACCOUNT = @customerId
                             ORDER BY (t.TRANSDATE)";

                cmd.CommandText = sqlCmd;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "customerId", customerId);
                MakeParam(cmd, "customerDiscountType", 1, SqlDbType.Int);
                MakeParam(cmd, "startDate", startDate < new DateTime(1900, 1, 1) ? new DateTime(1900, 1, 1) : startDate, SqlDbType.DateTime);
                MakeParam(cmd, "endDate", endDate, SqlDbType.DateTime);

                return Execute<SalesTransaction>(entry, cmd, CommandType.Text, PopulateTransaction);
            }
        }

        private static void PopulateTransaction(IDataReader dr, SalesTransaction transaction)
        {
            transaction.TransactionID = (string)dr["TRANSACTIONID"];
            transaction.StoreID = (string)dr["STORE"];
            transaction.TerminalID = (string)dr["TERMINALID"];

            if (dr["ITEMTYPE"] == DBNull.Value)
            {
                transaction.HasItemType = false;
                transaction.ItemType = 0;
            }
            else
            {
                transaction.HasItemType = true;
                transaction.ItemType = (SalesTransaction.ItemClassTypeEnum)dr["ITEMTYPE"];
            }

            transaction.GiftCard = ((byte)dr["GIFTCARD"] != 0);
            transaction.PrescriptionId = (string)dr["PRESCRIPTIONID"];
            transaction.PumpID = (int)dr["PUMPID"];
            transaction.SourceLineNum = (int)dr["SOURCELINENUM"];
            transaction.ItemID = (string)dr["ITEMID"];
            transaction.LineNum = (decimal)dr["LINENUM"];
            transaction.CreditMemoNumber = (string)dr["CREDITMEMONUMBER"];
            transaction.Price = (decimal)dr["PRICE"];
            transaction.Comment = (string)dr["COMMENT"];
            transaction.DosageType = (string)dr["DOSAGETYPE"];
            transaction.DosageStrengthUnit = (string)dr["DOSAGESTRENGTHUNIT"];
            transaction.DosageStrength = (decimal)dr["DOSAGESTRENGTH"];
            transaction.DosageUnitQuantity = (decimal)dr["DOSAGEUNITQUANTITY"];
            transaction.EntryStatus = (TransactionStatus)dr["TRANSACTIONSTATUS"];
            transaction.BarCode = (string)dr["BARCODE"];
            transaction.TaxGroup = (string)dr["TAXGROUP"];
            transaction.TaxAmount = (decimal)dr["TAXAMOUNT"];
            transaction.NetPrice = (decimal)dr["NETPRICE"];
            transaction.NetAmount = (decimal)dr["NETAMOUNT"];

            if (dr["NETAMOUNTINCLTAX"] == DBNull.Value)
            {
                transaction.HasNetAmountIncludeTax = false;
                transaction.NetAmountIncludeTax = 0.0M;
            }
            else
            {
                transaction.HasNetAmountIncludeTax = true;
                transaction.NetAmountIncludeTax = (decimal)dr["NETAMOUNTINCLTAX"];
            }

            transaction.Quantity = (decimal)dr["QTY"];
            transaction.TransactionDate = (DateTime)(dr["TRANSDATE"]);
            transaction.ItemIDScanned = ((byte)dr["ITEMIDSCANNED"] != 0);
            transaction.SalesPersonID = (string)dr["STAFF"];
            transaction.Unit = (string)dr["UNIT"];
            transaction.UnitQuantity = (decimal)dr["UNITQTY"];
            transaction.InventSerialID = (string)dr["INVENTSERIALID"];
            transaction.RFIDTagID = (string)dr["RFIDTAGID"];

            if(dr["TAXINCLINPRICE"] == DBNull.Value)
            {
                transaction.HasTaxIncludedInPrice = false;
                transaction.TaxIncludedInPrice = false;
            }
            else
            {
                transaction.HasTaxIncludedInPrice = true;
                transaction.TaxIncludedInPrice = ((byte)dr["TAXINCLINPRICE"] != 0);
            }

            transaction.VariantID = (string)dr["VARIANTID"];
            transaction.BatchID = (string)dr["INVENTBATCHID"];

            if (dr["INVENTBATCHEXPDATE"] == DBNull.Value)
            {
                transaction.HasBatchExpDate = false;
                transaction.BatchExpDate = new Date(DateTime.MinValue);
            }
            else
            {
                transaction.HasBatchExpDate = true;
                transaction.BatchExpDate = Date.FromAxaptaDate(dr["INVENTBATCHEXPDATE"]);
            }

            transaction.OilTax = (decimal)dr["OILTAX"];
            transaction.ReturnLineID = (decimal)dr["RETURNLINEID"];
            transaction.ReturnQuantity = (decimal)dr["RETURNQTY"];
            transaction.IsLinkedItem = ((byte)dr["LINKEDITEMNOTORIGINAL"] != 0);
            transaction.IsAssembly = ((byte)dr["ISASSEMBLY"] != 0);
            transaction.IsAssemblyComponent = ((byte)dr["ISASSEMBLYCOMPONENT"] != 0);
            transaction.AssemblyParentLineID = (decimal)dr["ASSEMBLYPARENTLINEID"];
            transaction.LinkedToLineID = (decimal)dr["ORIGINALOFLINKEDITEMLIST"];
            transaction.ReturnTransActionID = (string)dr["RETURNTRANSACTIONID"];
            transaction.IsInfoCodeItem = ((byte)dr["ISINFOCODEITEM"] != 0);
            transaction.ItemDepartmentID = (string)dr["ITEMDEPARTMENTID"];
            transaction.ItemGroupID = (string)dr["ITEMGROUPID"];
            transaction.PriceInBarCode = ((byte)dr["PRICEINBARCODE"] != 0);
            transaction.PriceOverridden = ((byte)dr["PRICECHANGE"] != 0);
            transaction.WeightManuallyEntered = ((byte)dr["WEIGHTMANUALLYENTERED"] != 0);
            transaction.LineWasDiscounted = ((byte)dr["LINEWASDISCOUNTED"] != 0);
            transaction.ScaleItem = ((byte)dr["SCALEITEM"] != 0);
            transaction.WeightInBarcode = ((byte)dr["WEIGHTITEM"] != 0);
            transaction.LimitationMasterID = dr["LIMITATIONMASTERID"] == DBNull.Value ? Guid.Empty : (Guid)dr["LIMITATIONMASTERID"];
            transaction.PriceUnit = (decimal)dr["PRICEUNIT"];
            transaction.TotalDiscountAmount = (decimal)dr["TOTALDISCAMOUNT"];
            transaction.TotalDiscountPercent = (decimal)dr["TOTALDISCPCT"];
            transaction.TotalDiscountWithTax = (decimal)dr["TOTALDISCAMOUNTWITHTAX"];
            transaction.LineDiscount = (decimal)dr["LINEDSCAMOUNT"];
            transaction.LineDiscountPercent = (decimal)dr["LINEDISCPCT"];
            transaction.LineDiscountAmountWithTax = (decimal)dr["LINEDISCAMOUNTWITHTAX"];
            transaction.PeriodicDiscountType = (SalesTransaction.PeriodicDiscountTypeEnum)dr["PERIODICDISCTYPE"];
            transaction.PeriodicDiscountAmount = (decimal)dr["PERIODICDISCAMOUNT"];
            transaction.PeriodicDiscountAmountWithTax = (decimal)dr["PERIODICDISCAMOUNTWITHTAX"];
            transaction.DiscountOfferID = (string)dr["DISCOFFERID"];
            transaction.Text = (string)dr["DESCRIPTION"];

            transaction.CostAmount = (decimal) dr["COSTAMOUNT"];
            transaction.DiscountAmount = (decimal)dr["DISCAMOUNT"];
            transaction.WholeDiscountAmountWithTax = (decimal)dr["WHOLEDISCAMOUNTWITHTAX"];
            transaction.CustomerAccountID = (string)dr["CUSTACCOUNT"];
        }
    }
}
