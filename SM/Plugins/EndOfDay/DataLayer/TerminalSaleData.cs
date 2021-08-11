using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities;

namespace LSOne.ViewPlugins.EndOfDay.DataLayer
{
    public class TerminalSaleData : SqlServerDataProviderBase
    {
        private string dataFilterString = "";
        private string dataAreaID = "";

        internal TerminalSaleData(string dataAreaID, DateTime from, DateTime to, string statementIDFrom, string statementIDTo, ReportIntervalType intervalType)
        {
            this.dataAreaID = dataAreaID;
            switch (intervalType)
            {
                case ReportIntervalType.ByDate:
                    {
                        dataFilterString = " t.transdate >= '" + from.Year + "-" + from.Month + "-" + from.Day + "' ";
                        to = to.AddDays(1);
                        dataFilterString += " and t.transdate < '" + to.Year + "-" + to.Month + "-" + to.Day + "' ";
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
            dataFilterString += " and t.dataareaid = '" + dataAreaID + "' ";
        }

        public List<TerminalSaleLine> GetTerminalSaleLines(IConnectionManager entry, bool includeReportFormatting, string storeId)
        {
            List<TerminalSaleLine> result;

            StringBuilder sb = new StringBuilder();

            sb.Append(" SELECT b.TERMINALID AS TERMINALID, ");
            sb.Append(" SUM(b.QTY) *-1 AS QUANTITY, ");
            sb.Append(" SUM(b.NETAMOUNTINCLTAX) * -1 AS NETAMOUNTINCLTAX,  ");
            sb.Append(" SUM(b.WHOLEDISCAMOUNTWITHTAX) AS WHOLEDISCAMOUNTWITHTAX, ");
            sb.Append(" SUM(b.PRICE) AS PRICE, ");
            sb.Append(" SUM(b.COSTAMOUNT) AS COSTAMOUNT ");
            sb.Append(" FROM RBOTRANSACTIONTABLE t, RBOTRANSACTIONSALESTRANS b ");
            sb.Append(" WHERE t.TRANSACTIONID = b.TRANSACTIONID ");
            sb.Append(" AND t.DATAAREAID = b.DATAAREAID ");
            sb.Append(" AND t.STORE = b.STORE ");
            sb.Append(" AND t.TERMINAL = b.TERMINALID ");
            sb.Append(" AND t.STORE = @storeid");
            sb.Append(" AND t.ENTRYSTATUS = 0 ");
            sb.Append(" AND t.TYPE = 2 ");
            sb.Append(" AND b.TRANSACTIONSTATUS = 0 ");  // 0 == Normal
            sb.Append(" AND " + dataFilterString);
            sb.Append(" GROUP BY b.TERMINALID ");
            sb.Append(" HAVING SUM(b.QTY) <> 0 ");
            sb.Append(" ORDER BY b.TERMINALID ");

            SqlCommand command = new SqlCommand(sb.ToString());
            MakeParam(command, "dataAreaID", entry.Connection.DataAreaId);
            MakeParam(command, "storeid", storeId);

            result = Execute<TerminalSaleLine>(entry, command, CommandType.Text, includeReportFormatting, PopulateTerminalSaleLine);

            if (includeReportFormatting)
            {
                TerminalSaleLine.PriceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
                TerminalSaleLine.QuantityLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
            }

            return result;

        }

        private static void PopulateTerminalSaleLine(IConnectionManager entry, IDataReader dr, TerminalSaleLine terminalSaleLine, object includeReportFormatting)
        {
            terminalSaleLine.TerminalId = (string)dr["TERMINALID"];
           
            terminalSaleLine.Quantity = (decimal)dr["QUANTITY"];
            terminalSaleLine.NetAmountWithTax = (decimal)dr["NETAMOUNTINCLTAX"];
            terminalSaleLine.WholeDiscountAmountWithTax = (decimal)dr["WHOLEDISCAMOUNTWITHTAX"];
            terminalSaleLine.Price = (decimal)dr["PRICE"];
            terminalSaleLine.CostAmount = (decimal)dr["COSTAMOUNT"];
        }
    }
}
