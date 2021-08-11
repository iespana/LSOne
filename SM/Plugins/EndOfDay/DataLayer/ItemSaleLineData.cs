using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities;

namespace LSOne.ViewPlugins.EndOfDay.DataLayer
{
    class ItemSaleLineData : SqlServerDataProviderBase
    {
        private string dataFilterString = "";
        private string dataAreaID = "";
        private bool dateSpecified = false;
        private DateTime fromDate;
        private DateTime toDate;

        public ItemSaleLineData(string dataAreaID, DateTime from, DateTime to, string statementIDFrom, string statementIDTo, ReportIntervalType intervalType)
        {
            this.dataAreaID = dataAreaID;
            switch (intervalType)
            {
                case ReportIntervalType.ByDate:
                {
                    dateSpecified = true;
                        dataFilterString = " t.transdate >= @fromDate and t.transdate < @toDate";
                        toDate = to.AddDays(1);
                    fromDate = from;
                        break;
                    }
                case ReportIntervalType.ByStatementID:
                    {
                        dataFilterString += " t.statementID >= '" + statementIDFrom + "' ";
                        dataFilterString += " and t.statementID <= '" + statementIDTo + "' ";
                        break;
                    }
                case ReportIntervalType.CurrentSaleOnly:
                    {
                        dataFilterString += " t.statementID = '' ";
                        break;
                    }
            }
            dataFilterString += " and t.DATAAREAID = '" + dataAreaID + "' ";
        }

        public List<ItemSaleLine> GetItemSaleLines(IConnectionManager entry, string storeId, bool includeReportFormatting)
        {
            List<ItemSaleLine> result;

            //ValidateSecurity(entry);

            StringBuilder sb = new StringBuilder();

            sb.Append(" SELECT ");
            sb.Append(" b.ITEMID, ");
            sb.Append(" b.DESCRIPTION, ");
            sb.Append(" b.VARIANTID, ");
            sb.Append(" ISNULL(co.NAME,'') AS COLOR,  ");
            sb.Append(" ISNULL(sz.NAME,'') AS SIZE,  ");
            sb.Append(" ISNULL(st.NAME,'') AS STYLE, ");
            sb.Append(" ISNULL(u.TXT, '') AS UNITNAME, ");
            sb.Append(" u.UNITDECIMALS AS DECIMALSMAX, ");
            sb.Append(" ISNULL(u.MINUNITDECIMALS, 0) AS DECIMALSMIN, ");
            //sb.Append(" --ISNULL(m.MODULETYPE, '') AS MODULETYPE, ");
            sb.Append(" SUM(b.QTY) AS QUANTITY,  ");
            sb.Append(" SUM(b.NETAMOUNTINCLTAX) AS NETAMOUNTINCLTAX , ");
            sb.Append(" SUM(b.COSTAMOUNT) AS COSTAMOUNT,   ");
            sb.Append(" b.PRICE AS PRICE, ");
            sb.Append(" SUM(b.UNITPRICE) AS UNITPRICE, ");
            sb.Append(" SUM(b.PRICEUNIT) AS PRICEUNIT, ");
            sb.Append(" SUM(b.UNITQTY) AS UNITQTY, ");
            sb.Append(" SUM(b.WHOLEDISCAMOUNTWITHTAX) AS WHOLEDISCAMOUNTWITHTAX ");
            sb.Append(" FROM RBOTRANSACTIONTABLE t, RBOTRANSACTIONSALESTRANS b  ");
            sb.Append(" LEFT OUTER JOIN INVENTTABLEMODULE m on b.ITEMID = m.ITEMID AND b.DATAAREAID = m.DATAAREAID ");
            sb.Append(" LEFT OUTER JOIN UNIT u on m.UNITID = u.UNITID AND m.DATAAREAID = u.DATAAREAID  ");
            sb.Append(" LEFT OUTER JOIN INVENTDIMCOMBINATION i ON b.VARIANTID = i.RBOVARIANTID AND b.DATAAREAID = i.DATAAREAID ");
            sb.Append(" LEFT OUTER JOIN RBOCOLORS co ON i.INVENTCOLORID = co.COLOR AND i.DATAAREAID = co.DATAAREAID   ");
            sb.Append(" LEFT OUTER JOIN RBOSIZES sz ON i.INVENTSIZEID = sz.SIZE_ AND i.DATAAREAID = sz.DATAAREAID   ");
            sb.Append(" LEFT OUTER JOIN RBOSTYLES st ON i.INVENTSTYLEID = st.STYLE AND i.DATAAREAID = st.DATAAREAID   ");
            sb.Append(" WHERE t.TRANSACTIONID = b.TRANSACTIONID  ");
            sb.Append(" AND t.DATAAREAID = b.DATAAREAID  ");
            sb.Append(" AND t.STORE = b.STORE  ");
            sb.Append(" AND t.TERMINAL = b.TERMINALID  ");
            sb.Append(" AND t.ENTRYSTATUS = 0  ");
            sb.Append(" AND t.STORE = @storeid  ");
            sb.Append(" AND t.TYPE = 2  ");
            sb.Append(" AND m.MODULETYPE = 2 ");
            sb.Append(" AND b.TRANSACTIONSTATUS = 0   "); // 0 == Normal
            sb.Append(" AND " + dataFilterString );
            sb.Append(" GROUP BY b.ITEMID, b.VARIANTID, b.DESCRIPTION, u.TXT, co.NAME, st.NAME, sz.NAME, u.MINUNITDECIMALS, u.UNITDECIMALS, b.PRICE  ");
            sb.Append(" HAVING SUM(b.QTY) <> 0  ");
            sb.Append(" ORDER BY b.ITEMID ");

            SqlCommand command = new SqlCommand(sb.ToString());

            MakeParam(command, "storeid", storeId);
            if (dateSpecified)
            {
                MakeParam(command, "fromDate", fromDate, SqlDbType.DateTime);
                MakeParam(command, "toDate", toDate, SqlDbType.DateTime);
            }
            result = Execute<ItemSaleLine>(entry, command, CommandType.Text, includeReportFormatting, PopulateItemSaleLine);

            if (includeReportFormatting)
            {
                ItemSaleLine.PriceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            }

            return result;

        }

        private static void PopulateItemSaleLine(IConnectionManager entry, IDataReader dr, ItemSaleLine itemSaleLine, object includeReportFormatting)
        {
            itemSaleLine.ItemID = (string)dr["ITEMID"];
            itemSaleLine.ItemName = (string)dr["DESCRIPTION"];
            itemSaleLine.VariantID = (string)dr["VARIANTID"];
            itemSaleLine.ColorName = (string)dr["COLOR"];
            itemSaleLine.SizeName = (string)dr["SIZE"];
            itemSaleLine.StyleName = (string)dr["STYLE"];
            //itemSaleLine.UnitID = (string)dr["UNITID"]; //not fetched
            itemSaleLine.UnitName = (string)dr["UNITNAME"];
            itemSaleLine.Quantity = (decimal)dr["QUANTITY"] * -1;
            itemSaleLine.NetAmountWithTax = (decimal)dr["NETAMOUNTINCLTAX"] * -1;
            itemSaleLine.CostAmount = (decimal)dr["COSTAMOUNT"];
            itemSaleLine.Price = (decimal)dr["PRICE"];
            itemSaleLine.UnitPrice = (decimal)dr["UNITPRICE"];
            itemSaleLine.PriceUnit = (decimal)dr["PRICEUNIT"];
            itemSaleLine.UnitQuantity = (decimal)dr["UNITQTY"];
            itemSaleLine.WholeDiscountAmountWithTax = (decimal)dr["WHOLEDISCAMOUNTWITHTAX"];

            if ((bool)includeReportFormatting)
            {
                if (dr["DECIMALSMAX"] == DBNull.Value)
                    itemSaleLine.QuantityLimiter = entry.GetDecimalSetting(DecimalSettingEnum.Quantity);
                else
                    itemSaleLine.QuantityLimiter = new DecimalLimit((int)dr["DECIMALSMIN"], (int)dr["DECIMALSMAX"]);
            }

        }

        public List<SalesTotalAmounts> GetTotals(IConnectionManager entry, string storeId, bool includeReportFormatting)
        {
            List<SalesTotalAmounts> totals;

            StringBuilder sb = new StringBuilder();

            sb.Append(" SELECT ");
            sb.Append(" SUM(b.QTY) AS QUANTITY,  ");
            sb.Append(" SUM(b.NETAMOUNTINCLTAX) AS NETAMOUNTINCLTAX , ");
            sb.Append(" SUM(b.COSTAMOUNT) AS COSTAMOUNT,   ");
            sb.Append(" SUM(b.PRICE) AS PRICE, ");
            sb.Append(" SUM(b.WHOLEDISCAMOUNTWITHTAX) AS WHOLEDISCAMOUNTWITHTAX ");
            sb.Append(" FROM RBOTRANSACTIONTABLE t, RBOTRANSACTIONSALESTRANS b  ");
            sb.Append(" WHERE t.TRANSACTIONID = b.TRANSACTIONID  ");
            sb.Append(" AND t.DATAAREAID = b.DATAAREAID  ");
            sb.Append(" AND t.STORE = b.STORE  ");
            sb.Append(" AND t.TERMINAL = b.TERMINALID  ");
            sb.Append(" AND t.ENTRYSTATUS = 0  ");
            sb.Append(" AND t.STORE = @storeid  ");
            sb.Append(" AND t.TYPE = 2  ");
            sb.Append(" AND b.TRANSACTIONSTATUS = 0   "); // 0 == Normal
            sb.Append(" AND " + dataFilterString);
            sb.Append(" AND t.DATAAREAID = @dataAreaID ");
            sb.Append(" HAVING SUM(b.QTY) <> 0  ");


            SqlCommand command = new SqlCommand(sb.ToString());

            MakeParam(command, "dataAreaID", entry.Connection.DataAreaId);
            MakeParam(command, "storeid", storeId);

            if (dateSpecified)
            {
                MakeParam(command, "fromDate", fromDate, SqlDbType.DateTime);
                MakeParam(command, "toDate", toDate, SqlDbType.DateTime);
            }

            totals = Execute<SalesTotalAmounts>(entry, command, CommandType.Text, includeReportFormatting,
                PopulateTotals);

            if (includeReportFormatting)
            {
                SalesTotalAmounts.PriceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            }
            return totals;
        }

        private static void PopulateTotals(IConnectionManager entry, IDataReader dr, SalesTotalAmounts totalsLine, object includeReportFormatting)
        {
            totalsLine.TotalQuantity = (decimal)dr["QUANTITY"] * -1;
            totalsLine.TotalNetAmountWithTax = (decimal)dr["NETAMOUNTINCLTAX"] * -1;
            totalsLine.TotalCostAmount = (decimal)dr["COSTAMOUNT"];
            totalsLine.TotalPrice = (decimal)dr["PRICE"];
            totalsLine.TotalWholeDiscountAmountWithTax = (decimal)dr["WHOLEDISCAMOUNTWITHTAX"];
        }
    }
}
