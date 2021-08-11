using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders.Financials;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Financial
{
    public class FinancialReportData : SqlServerDataProviderBase, IFinancialReportData
    {
        private static void PopulateReportTaxGroupLine(IConnectionManager entry, IDataReader dr, FinancialReportTaxGroupLine reportTaxGroupLine, object param)
        {
            reportTaxGroupLine.SalesTaxGroupName = (string)dr["TAXGROUPNAME"];
            //reportTaxGroupLine.SalesTaxGroupReceiptDisplay = (string)dr[""];

            var grossAmount = (decimal)dr["GROSSAMOUNT"];
            var netAmount = (decimal)dr["NETAMOUNT"];
            var taxAmount = (decimal)dr["TAXAMOUNT"];

            reportTaxGroupLine.GrossAmount = grossAmount;
            reportTaxGroupLine.NetAmount = netAmount;
            reportTaxGroupLine.TaxAmount = taxAmount;

            var priceLimiter = entry.GetDecimalSetting(DecimalSettingEnum.Prices);

            reportTaxGroupLine.FormattedGrossAmount = grossAmount.FormatWithLimits(priceLimiter);
            reportTaxGroupLine.FormattedNetAmount = netAmount.FormatWithLimits(priceLimiter);
            reportTaxGroupLine.FormattedTaxAmount = taxAmount.FormatWithLimits(priceLimiter);
        }

        public virtual List<FinancialReportTaxGroupLine> GetTaxGroupLines(IConnectionManager entry, RecordIdentifier storeID, DateTime fromDate, DateTime toDate)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select  " +
                    "(-1)*SUM((ISNULL(b.taxamount,0))) as TAXAMOUNT,  " +
                    "(-1)*SUM((ISNULL(b.netamount,0))) as NETAMOUNT, " +
                    "(-1)*SUM((ISNULL(b.NETAMOUNT,0) + ISNULL(b.TAXAMOUNT,0))) AS GROSSAMOUNT, " +
                    "ISNULL(tg.NAME,'') as TAXGROUPNAME " +
                    "From  " +
                    "RBOTRANSACTIONSALESTRANS b " +
                    "Join RBOTRANSACTIONTABLE t on  t.TRANSACTIONID = b.TRANSACTIONID AND t.DATAAREAID = b.DATAAREAID  " +
	                "    AND t.TERMINAL = b.TERMINALID AND t.STORE = b.STORE " +
                    "Left outer join TAXITEMGROUPHEADING tg on tg.TAXITEMGROUP = b.TAXGROUP AND tg.DATAAREAID = b.DATAAREAID  " +
                    "Where  " +
                    "b.TRANSACTIONSTATUS = 0 " +
                    "AND t.ENTRYSTATUS = 0 " +
                    "AND t.STORE = @storeID " +
                    "AND t.TRANSDATE >= @fromDate " +
                    "AND t.TRANSDATE <= @toDate  " +
                    "AND t.DATAAREAID = @dataAreaID " +
                    "GROUP BY tg.NAME";

                MakeParam(cmd, "storeID", (string)storeID);
                MakeParam(cmd, "fromDate", fromDate, SqlDbType.DateTime);
                MakeParam(cmd, "toDate", toDate, SqlDbType.DateTime);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                var result = Execute<FinancialReportTaxGroupLine>(entry, cmd, CommandType.Text, null,PopulateReportTaxGroupLine);
                return result;
            }
        }
    }
}
