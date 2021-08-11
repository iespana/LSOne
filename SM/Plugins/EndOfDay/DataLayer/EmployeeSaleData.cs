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
    public class EmployeeSaleData : SqlServerDataProviderBase
    {
        private string dataFilterString = "";
        private string dataAreaID = "";

        public EmployeeSaleData(string dataAreaID, DateTime from, DateTime to, string statementIDFrom, string statementIDTo, ReportIntervalType intervalType)
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

        public List<EmployeeSaleLine> GetEmployeeSaleLines(IConnectionManager entry, bool includeReportFormatting, string storeId)
        {
            List<EmployeeSaleLine> result;

            StringBuilder sb = new StringBuilder();
            
            sb.Append(" SELECT  ");
            sb.Append(" b.STAFFID AS STAFFID,  ");
            sb.Append(" s.NAME AS NAME,  ");
            sb.Append(" s.NAMEONRECEIPT AS NAMEONRECEIPT,  ");
            sb.Append(" s.FIRSTNAME AS FIRSTNAME,  ");
            sb.Append(" s.LASTNAME AS LASTNAME,  ");
            sb.Append(" u.Login AS LOGIN,  ");
            sb.Append(" SUM(b.QTY) AS QUANTITY,  ");
            sb.Append(" SUM(b.NETAMOUNTINCLTAX) AS NETAMOUNTINCLTAX,   ");
            sb.Append(" SUM(b.WHOLEDISCAMOUNTWITHTAX) AS WHOLEDISCAMOUNTWITHTAX, ");
            sb.Append(" SUM(b.PRICE) AS PRICE, ");
            sb.Append(" SUM(b.COSTAMOUNT) AS COSTAMOUNT ");
            sb.Append(" FROM RBOTRANSACTIONTABLE t, RBOTRANSACTIONSALESTRANS b  ");
            sb.Append(" LEFT OUTER JOIN RBOSTAFFTABLE s ON s.DATAAREAID = b.DATAAREAID AND b.STAFFID = s.STAFFID  ");
            sb.Append(" LEFT OUTER JOIN USERS u ON u.STAFFID = s.STAFFID  ");
            sb.Append(" WHERE t.TRANSACTIONID = b.TRANSACTIONID  ");
            sb.Append(" AND t.DATAAREAID = b.DATAAREAID  ");
            sb.Append(" AND t.STORE = b.STORE  ");
            sb.Append(" AND t.TERMINAL = b.TERMINALID  ");
            sb.Append(" AND t.STORE = @storeid ");
            sb.Append(" AND t.DATAAREAID = @dataAreaID ");
            sb.Append(" AND t.ENTRYSTATUS = 0  ");
            sb.Append(" AND t.TYPE = 2  ");
            sb.Append(" AND b.TRANSACTIONSTATUS = 0  ");
            sb.Append(" AND " + dataFilterString );
            sb.Append(" GROUP BY b.STAFFID, s.LASTNAME, s.NAME, s.NAMEONRECEIPT, s.FIRSTNAME, u.Login  ");
            sb.Append(" HAVING SUM(b.QTY) <> 0  ");
            sb.Append(" ORDER BY u.Login  ");
            
            SqlCommand command = new SqlCommand(sb.ToString());
            MakeParam(command, "dataAreaID", entry.Connection.DataAreaId);
            MakeParam(command, "storeid", storeId);

            result = Execute<EmployeeSaleLine>(entry, command, CommandType.Text, includeReportFormatting, PopulateEmployeeSaleLine);

            if (includeReportFormatting)
            {
                EmployeeSaleLine.PriceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
                EmployeeSaleLine.QuantityLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
            }

            return result;

        }

        private static void PopulateEmployeeSaleLine(IConnectionManager entry, IDataReader dr, EmployeeSaleLine employeeSaleLine, object includeReportFormatting)
        {            
            employeeSaleLine.StaffId = (string)dr["STAFFID"];
            employeeSaleLine.Name = (string)dr["NAME"];
            employeeSaleLine.NameOnReceipt = (string)dr["NAMEONRECEIPT"];
            employeeSaleLine.FirstName = (string)dr["FIRSTNAME"];
            employeeSaleLine.LastName = (string)dr["LASTNAME"];
            employeeSaleLine.Quantity = (decimal)dr["QUANTITY"] *-1;
            employeeSaleLine.NetAmountWithTax = (decimal)dr["NETAMOUNTINCLTAX"] * -1;
            employeeSaleLine.WholeDiscountAmountWithTax = (decimal)dr["WHOLEDISCAMOUNTWITHTAX"];
            employeeSaleLine.Price = (decimal)dr["PRICE"];
            employeeSaleLine.CostAmount = (decimal)dr["COSTAMOUNT"];
            employeeSaleLine.Login = (string)dr["LOGIN"];
        }
    }
}
