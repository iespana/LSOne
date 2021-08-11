using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Services.Datalayer.BusinessObjects;
using LSOne.Services.Datalayer.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.SqlDataProviders
{
    public class OldItemLedgerData : SqlServerDataProviderBase, IOldItemLedgerData
    {
        private static void PopulateItemLedger(IDataReader dr, OldItemLedger item)
        {
            item.ItemID = (string) dr["ITEMID"];
            item.Time = (DateTime) dr["TIME"];
            item.TransactionID = (string) dr["TRANSACTIONID"];
            item.RecieptNumber = (string) dr["RECEIPTID"];
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
                    item.LedgerType = OldItemLedgerType.Sale;
                    break;
                case 1:
                    item.LedgerType = OldItemLedgerType.VoidedLine;
                    item.Quantity = -item.Quantity;
                    break;
                case 5 :
                    item.LedgerType = OldItemLedgerType.VoidedSale;
                    break;
                case 10:
                    item.LedgerType = OldItemLedgerType.Adjustment;  //Posted Sales
                    break;
                case 11:
                    item.LedgerType = OldItemLedgerType.Purchase;
                    break;
                case 12:
                    item.LedgerType = OldItemLedgerType.Adjustment;
                    break;
            }
        }

        public virtual List<OldItemLedger> GetList(IConnectionManager entry, RecordIdentifier itemLedgerID)
        {
            bool includeSales = (bool) itemLedgerID[5] || (bool) itemLedgerID[6];
            bool noneSelected = !(bool) itemLedgerID[5] && !(bool) itemLedgerID[6] && !(bool) itemLedgerID[7];
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT DISTINCT ITEMID, TYPE, TIME, TRANSACTIONID, RECEIPTID,STOREID, STORENAME, TERMINALID, TERMINALNAME, QUANTITY, COSTPRICE, NETPRICE,
                                     DISCOUNTAMOUNT, NETDISCOUNTAMOUNT,CUSTOMERID  
                                     FROM (
                                     SELECT TOP 2147483647 ITEMID, TYPE, TIME, TRANSACTIONID, RECEIPTID,STOREID, STORENAME, TERMINALID, TERMINALNAME, QUANTITY, COSTPRICE, NETPRICE,
                                     DISCOUNTAMOUNT, NETDISCOUNTAMOUNT,CUSTOMERID, ROW_NUMBER() over(order by TIME " + ((bool)itemLedgerID[10] ? "ASC" : "DESC") + @") as rownum
                                     FROM ( " +
                                  (includeSales || noneSelected ?
                                       @"SELECT ISNULL(I.ITEMID, '') AS ITEMID,
                                     (CASE WHEN ISNULL(H.ENTRYSTATUS, 0) = 1 THEN 5 
                                     ELSE 
                                     ISNULL(I.TRANSACTIONSTATUS, 0)
                                     END) AS TYPE, 
                                     ISNULL(I.TRANSDATE, '1900-1-1') AS TIME, I.TRANSACTIONID, ISNULL(I.RECEIPTID, '') AS RECEIPTID, 
                                     I.STORE AS STOREID, ISNULL(S.NAME, '') AS STORENAME, I.TERMINALID, ISNULL(T.NAME, '') AS TERMINALNAME,
                                     ISNULL(I.QTY, 0) AS QUANTITY,
                                     ISNULL(I.COSTAMOUNT, 0) AS COSTPRICE, ISNULL(I.NETPRICE, 0) AS NETPRICE,
                                     ISNULL(I.WHOLEDISCAMOUNTWITHTAX, 0) AS DISCOUNTAMOUNT, ISNULL(I.DISCAMOUNT, 0) AS NETDISCOUNTAMOUNT,  
                                     ISNULL(H.CUSTACCOUNT, '') AS CUSTOMERID
                                     FROM RBOTRANSACTIONSALESTRANS I
                                     LEFT JOIN RBOSTORETABLE S ON I.STORE = S.STOREID AND I.DATAAREAID = S.DATAAREAID
                                     LEFT JOIN RBOTERMINALTABLE T ON I.TERMINALID = T.TERMINALID AND I.DATAAREAID = T.DATAAREAID AND I.STORE = T.STOREID
                                     LEFT JOIN RBOTRANSACTIONTABLE H ON I.TRANSACTIONID = H.TRANSACTIONID AND I.DATAAREAID = H.DATAAREAID AND I.STORE = H.STORE and I.TERMINALID = H.TERMINAL
                                     WHERE I.ITEMID = @itemID 
                                     AND I.DATAAREAID = @dataAreaID 
                                     AND I.TRANSDATE > @fromDate 
                                     AND I.TRANSDATE < @toDate 
                                     AND H.ENTRYSTATUS <> 5
                                     AND (I.STORE = @storeID OR @storeID = '')
                                     AND (I.TERMINALID = @terminalID OR @terminalID = '')" +
                                       (includeSales ? "AND (" : "") +
                                       ((bool)itemLedgerID[5] ? "(I.TRANSACTIONSTATUS = 0 AND H.ENTRYSTATUS = 0)" + ((bool)itemLedgerID[6] ? "OR " : "") : "") +
                                       ((bool) itemLedgerID[6] ? "I.TRANSACTIONSTATUS = 1 OR H.ENTRYSTATUS = 1" : "") +
                                       (includeSales ? ")" : "") : "") +
                                       ((bool) itemLedgerID[7] || noneSelected ? (includeSales || noneSelected ? @"UNION ALL " : "") +
                                       @"SELECT I.ITEMID,
	                                  ISNULL(I.TYPE, 0) + 10 AS TYPE,
                                      ISNULL(I.POSTINGDATE, '1900-1-1') AS TIME,
                                      '' AS TRANSACTIONID,
                                      '' AS RECEIPTID,
                                      ISNULL(I.STOREID, '') AS STOREID,
                                      ISNULL(S.NAME, '') AS STORENAME,
	                                  '' AS TERMINALID,
	                                  '' AS TERMINALNAME,
	                                  ISNULL(I.ADJUSTMENT, 0) AS QUANTITY,
                                      ISNULL(I.COSTPRICEPERITEM, 0) AS COSTPRICE,
                                      ISNULL(I.SALESPRICEWITHOUTTAXPERITEM, 0) AS NETPRICE,
                                      ISNULL(I.OFFERDISCOUNTAMOUNTPERITEM, 0) AS DISCOUNTAMOUNT,
                                      ISNULL(I.DISCOUNTAMOUNTPERITEM, 0) AS NETDISCOUNTAMOUNT,
                                      '' AS CUSTOMERID
                                      FROM INVENTTRANS I
                                      LEFT JOIN RBOSTORETABLE S ON I.STOREID = S.STOREID AND I.DATAAREAID = S.DATAAREAID
                                      WHERE I.ITEMID = @itemID 
                                      AND I.DATAAREAID = @dataAreaID
                                      AND I.POSTINGDATE > @fromDate 
                                      AND I.POSTINGDATE < @toDate 
                                      AND I.TYPE <> 0
                                      AND (I.STOREID = @storeID OR @storeID = '')" : "") +
                                      @") AS query) as query2 where rownum between @fromNumber and @toNumber order by TIME DESC";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", itemLedgerID[0]);
                MakeParam(cmd, "storeID", itemLedgerID[1]);
                MakeParam(cmd, "terminalID", itemLedgerID[2]);
                MakeParam(cmd, "fromDate", (DateTime)itemLedgerID[3], SqlDbType.DateTime);
                MakeParam(cmd, "toDate", (DateTime)itemLedgerID[4], SqlDbType.DateTime);
                MakeParam(cmd, "fromNumber", (int)itemLedgerID[8], SqlDbType.Int);
                MakeParam(cmd, "toNumber", (int) itemLedgerID[9], SqlDbType.Int);
                return Execute<OldItemLedger>(entry, cmd, CommandType.Text, PopulateItemLedger);
            }
        }
    }
}
