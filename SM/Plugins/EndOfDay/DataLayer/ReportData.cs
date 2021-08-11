using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities;

namespace LSOne.ViewPlugins.EndOfDay.DataLayer
{
    public class ReportData : SqlServerDataProviderBase, IReportData
    {
        private static Report GenerateReportFromReportLines(List<ReportLine> reportLines)
        {
            Report report = new Report();
            if (reportLines == null) return report;
            
            foreach (var reportLine in reportLines)
            {
                if (reportLine.Type == 2)
                {
                    report.HeildarInnst += reportLine.GrossAmount + reportLine.LineDiscAmount + reportLine.TotalDiscAmount;
                    report.HeildarInnstAfgreidslur += 1;
                    report.HeildarInnstEiningar += reportLine.NumberOfItems;
                    if (reportLine.EntryStatus == 1)
                    {
                        report.HaettVid += reportLine.GrossAmount;
                        report.HaettVidAfgreidslur += 1;
                        report.HaettVidEiningar += reportLine.NumberOfItems;
                    }
                    else
                    {
                        if (reportLine.GrossAmount < 0)
                            report.FjoldiAfgrIMinus += 1;
                        report.DagsSala += reportLine.GrossAmount;
                        report.DagsSalaAfgreidslur += 1;
                        report.DagsSalaEiningar += reportLine.NumberOfItems;
                        report.InnsleginnFjoldi += reportLine.NumberOfItems;
                        if (reportLine.LineDiscAmount != 0)
                        {
                            report.LinuAfsl += reportLine.LineDiscAmount;
                            report.LinuAfslFjVid += 1;
                        }
                        if (reportLine.TotalDiscAmount != 0)
                        {
                            report.HeildAfsl += reportLine.TotalDiscAmount;
                            report.HeildAfslFjVid += 1;
                        }
                    }
                }
            }
            report.HeildarSummur += report.DagsSala;
            return report;
        }

        private static void PopulateReport(IDataReader dr, ReportLine reportLine)
        {
            reportLine.EntryStatus = (int)dr["EntryStatus"];
            reportLine.Type = (int)dr["Type"];
            reportLine.NumberOfItems = (decimal)dr["numberOfItems"];
            reportLine.GrossAmount = ((decimal)dr["GrossAmount"] * -1);
            reportLine.LineDiscAmount = (decimal)dr["LineDisc"];
            reportLine.TotalDiscAmount = (decimal)dr["TotalDisc"];
            
        }

        public Report GetReport(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType)
        {
            ValidateSecurity(entry);
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT t.transactionid, t.type, t.EntryStatus, SUM(s.QTY)*(-1) as numberOfItems, t.GrossAmount,
                                    sum(s.linediscamountwithtax + s.PERIODICDISCAMOUNTWITHTAX) as LineDisc, sum(s.TOTALDISCAMOUNTWITHTAX) as TotalDisc,
                                    t.StatementId, t.TransDate 
                                    from rbotransactiontable t                                     
                                    left join  rbotransactionsalestrans s on t.transactionid = s.transactionid and t.terminal = s.TERMINALID and t.store = s.store and t.dataareaid = s.dataareaid 
                                    where s.transactionstatus = 0 
                                    AND t.ENTRYSTATUS <> 5
                                    AND t.STORE = @storeID 
                                    AND t.DATAAREAID = @dataAreaID ";
                //Entry status 5 are transactions done in training mode
                MakeParam(cmd, "storeID", storeID);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                switch (intervalType)
                {
                    case ReportIntervalType.ByDate:
                        {
                            cmd.CommandText += " AND t.TRANSDATE >= @fromDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "fromDate", from.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);

                            if (statementID != RecordIdentifier.Empty)
                            {
                                cmd.CommandText += "AND t.STATEMENTID = @statementID";
                                MakeParam(cmd, "statementID", statementID);
                            }

                            break;
                        }
                    case ReportIntervalType.CurrentSaleOnly:
                        {
                            cmd.CommandText += " AND t.TRANSDATE >= @nowDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "nowDate", DateTime.Now.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);
                            break;
                        }
                }
                cmd.CommandText += @" group by t.transactionid, t.[type], t.EntryStatus, t.NUMBEROFITEMLINES, t.GrossAmount, t.StatementId, t.TransDate ";
                var reportLines =  Execute<ReportLine>(entry, cmd, CommandType.Text, PopulateReport);
                return GenerateReportFromReportLines(reportLines);
            }
        }

        //Pending further refactoring
        public DataTable GetTenderTable(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT b.TENDERTYPE, c.NAME, COUNT(*) AS SALESNUMBERS, sum(AMOUNTTENDERED) AS AMOUNT 
                                         FROM RBOTRANSACTIONPAYMENTTRANS b, RBOTENDERTYPETABLE c, RBOTRANSACTIONTABLE t 
                                         where c.TENDERTYPEID = b.TENDERTYPE AND t.TRANSACTIONID = b.TRANSACTIONID 
                                         AND t.STORE = b.STORE 
                                         AND t.TERMINAL = b.TERMINAL 
                                         AND t.DATAAREAID = b.DATAAREAID AND t.DATAAREAID = c.DATAAREAID 
                                         AND t.ENTRYSTATUS = 0 
                                         AND t.STORE = @storeid 
                                         AND b.TRANSACTIONSTATUS = 0 
                                         AND c.DEFAULTFUNCTION NOT IN (4,5) and ";  

                switch (intervalType)
                {
                    case ReportIntervalType.ByDate:
                        {
                            cmd.CommandText += " t.TRANSDATE >= @fromDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "fromDate", from.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);

                            if (statementID != RecordIdentifier.Empty)
                            {
                                cmd.CommandText += "AND t.STATEMENTID = @statementID";
                                MakeParam(cmd, "statementID", statementID);
                            }

                            break;
                        }
                    case ReportIntervalType.CurrentSaleOnly:
                        {

                            cmd.CommandText += " t.TRANSDATE >= @nowDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "nowDate", DateTime.Now.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);
                            break;
                        }
                }
                cmd.CommandText += " GROUP BY b.TENDERTYPE, c.NAME ORDER BY b.TENDERTYPE ASC ";

                MakeParam(cmd, "storeid", storeID);

                IDataReader reader = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                DataTable tenderTable = new DataTable();
                tenderTable.Load(reader);
                reader.Close();
                return tenderTable;
            }
        }

        public DataTable GetTenderDetailTable(IConnectionManager entry, RecordIdentifier paymentTypeId, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType)
        {
            using(IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT b.TENDERTYPE, 
                                    b.CARDTYPEID, 
                                    ISNULL(d.NAME, ISNULL(b.CARDTYPEID,'<unknown>')) as NAME,  
                                    COUNT(*) AS SALESNUMBERS, 
                                    SUM(b.AMOUNTTENDERED) AS AMOUNT
                                    FROM RBOTRANSACTIONTABLE t
                                    JOIN RBOTRANSACTIONPAYMENTTRANS b on t.TRANSACTIONID = b.TRANSACTIONID AND t.DATAAREAID = b.DATAAREAID  AND t.STORE = b.STORE   AND t.TERMINAL = b.TERMINAL  
                                    LEFT OUTER JOIN RBOSTORETENDERTYPECARDTABLE d on b.TENDERTYPE = d.TENDERTYPEID AND b.CARDTYPEID = d.CARDTYPEID AND t.STORE = d.STOREID  AND t.DATAAREAID = d.DATAAREAID  
                                    WHERE  
                                    b.TENDERTYPE = @paymentTypeId
                                    AND t.STORE = @storeid  
                                    AND t.DATAAREAID = @dataAreaID 
                                    AND t.ENTRYSTATUS = 0  
                                    AND b.TRANSACTIONSTATUS = 0
                                    AND ";

                switch (intervalType)
                {
                    case ReportIntervalType.ByDate:
                        {
                            cmd.CommandText += " t.TRANSDATE >= @fromDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "fromDate", from.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);

                            if (statementID != RecordIdentifier.Empty)
                            {
                                cmd.CommandText += "AND t.STATEMENTID = @statementID";
                                MakeParam(cmd, "statementID", statementID);
                            }

                            break;
                        }
                    case ReportIntervalType.CurrentSaleOnly:
                        {

                            cmd.CommandText += " t.TRANSDATE >= @nowDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "nowDate", DateTime.Now.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);
                            break;
                        }
                }
                cmd.CommandText += @" GROUP BY b.TENDERTYPE, b.CARDTYPEID, d.NAME  
                                      ORDER BY b.TENDERTYPE, b.CARDTYPEID ";

                MakeParam(cmd, "storeid", storeID);
                MakeParam(cmd, "paymentTypeId", (string)paymentTypeId);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                IDataReader reader = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                DataTable tenderDetailTable = new DataTable();
                tenderDetailTable.Load(reader);
                reader.Close();
                return tenderDetailTable;
            }
        }

        public DataTable GetTaxData(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @" SELECT b.TAXGROUP AS TAXGROUP, tax.TAXVALUE AS PERCENTAGE, 
                         (b.NETAMOUNT)*(-1) AS NETAMOUNT, 
                         (b.TAXAMOUNT)*(-1) AS TAXAMOUNT, 
                         (b.NETAMOUNT + b.TAXAMOUNT)*(-1) AS BRUTTOAMOUNT 
                         FROM RBOTRANSACTIONTABLE t, RBOTRANSACTIONSALESTRANS b, TAXONITEM s, TAXDATA tax 
                         WHERE t.TRANSACTIONID = b.TRANSACTIONID 
                         AND b.DATAAREAID = s.DATAAREAID 
                         AND s.DATAAREAID = tax.DATAAREAID 
                         AND t.DATAAREAID = b.DATAAREAID 
                         AND s.TAXCODE = tax.TAXCODE 
                         AND s.TAXITEMGROUP = b.TAXGROUP 
                         AND t.TERMINAL = b.TERMINALID 
                         AND t.STORE = b.STORE 
                         AND t.ENTRYSTATUS = 0 
                         AND t.STORE = @storeid  
                         AND t.RECEIPTID != '' 
                         AND t.DATAAREAID = @dataAreaID 
                         AND b.TRANSACTIONSTATUS = 0 and ";

                switch (intervalType)
                {
                    case ReportIntervalType.ByDate:
                        {
                            cmd.CommandText += " t.TRANSDATE >= @fromDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "fromDate", from.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);

                            if (statementID != RecordIdentifier.Empty)
                            {
                                cmd.CommandText += "AND t.STATEMENTID = @statementID";
                                MakeParam(cmd, "statementID", statementID);
                            }

                            break;
                        }
                    case ReportIntervalType.CurrentSaleOnly:
                        {

                            cmd.CommandText += " t.TRANSDATE >= @nowDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "nowDate", DateTime.Now.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);
                            break;
                        }
                }

                cmd.CommandText += " ORDER BY TAXGROUP ";

                MakeParam(cmd, "storeid", storeID);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                IDataReader reader = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                DataTable taxTable = new DataTable();
                taxTable.Load(reader);
                reader.Close();
                return taxTable;
            }
        }

        public decimal GetNumberOfManuallyEnteredItems(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT (SUM(b.QTY)*-1) AS MANUALENTERED FROM RBOTRANSACTIONTABLE t, RBOTRANSACTIONSALESTRANS b 
                WHERE t.DATAAREAID = @dataAreaID AND b.ITEMIDSCANNED = 0 
                AND t.TRANSACTIONID = b.TRANSACTIONID 
                AND t.STORE = b.STORE AND t.TERMINAL = b.TERMINALID 
                AND t.ENTRYSTATUS = 0 
                AND t.STORE = @storeid 
                AND b.TRANSACTIONSTATUS = 0 and "; //in case of a voided sales item; 0 == Norma

                switch (intervalType)
                {
                    case ReportIntervalType.ByDate:
                        {
                            cmd.CommandText += " t.TRANSDATE >= @fromDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "fromDate", from.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);

                            if (statementID != RecordIdentifier.Empty)
                            {
                                cmd.CommandText += "AND t.STATEMENTID = @statementID";
                                MakeParam(cmd, "statementID", statementID);
                            }

                            break;
                        }
                    case ReportIntervalType.CurrentSaleOnly:
                        {

                            cmd.CommandText += " t.TRANSDATE >= @nowDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "nowDate", DateTime.Now.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);
                            break;
                        }
                }

                MakeParam(cmd, "storeid", storeID);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                object result = entry.Connection.ExecuteScalar(cmd);

                if (result == DBNull.Value)
                {
                    return 0;
                }
                else
                {
                    return (decimal) result;
                }
            }
        }

        public decimal GetNumberOfScannedItems(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType)
        {
            using(IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT (SUM(b.QTY) * -1) AS MANUALENTERED FROM RBOTRANSACTIONTABLE t, RBOTRANSACTIONSALESTRANS b 
                                    WHERE t.DATAAREAID = @dataAreaID AND b.ITEMIDSCANNED = 1 
                                    AND t.TRANSACTIONID = b.TRANSACTIONID 
                                    AND t.STORE = b.STORE AND t.TERMINAL = b.TERMINALID 
                                    AND t.ENTRYSTATUS = 0 
                                    AND t.STORE = @storeid 
                                    AND b.TRANSACTIONSTATUS = 0  and ";  //in case of a voided sales item; 0 == Normal

                switch (intervalType)
                {
                    case ReportIntervalType.ByDate:
                        {
                            cmd.CommandText += " t.TRANSDATE >= @fromDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "fromDate", from.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);

                            if (statementID != RecordIdentifier.Empty)
                            {
                                cmd.CommandText += "AND t.STATEMENTID = @statementID";
                                MakeParam(cmd, "statementID", statementID);
                            }

                            break;
                        }
                    case ReportIntervalType.CurrentSaleOnly:
                        {

                            cmd.CommandText += " t.TRANSDATE >= @nowDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "nowDate", DateTime.Now.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);
                            break;
                        }
                }

                MakeParam(cmd, "storeid", storeID);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                object result = entry.Connection.ExecuteScalar(cmd);
                if (result == DBNull.Value)
                {
                    return 0;
                }
                else
                {
                    return (decimal)result;
                }
            }
        }

        public int GetNumOfNegative(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType)
        {
            using(IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @" SELECT COUNT(*) AS NUMOFNEGATIVE FROM RBOTRANSACTIONTABLE t 
                                     WHERE t.GROSSAMOUNT > 0 
                                     AND t.ENTRYSTATUS = 0 
                                     AND t.STORE = @storeid 
                                     AND t.DATAAREAID = @dataAreaID 
                                     and ";
                switch (intervalType)
                {
                    case ReportIntervalType.ByDate:
                        {
                            cmd.CommandText += " t.TRANSDATE >= @fromDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "fromDate", from.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);

                            if (statementID != RecordIdentifier.Empty)
                            {
                                cmd.CommandText += "AND t.STATEMENTID = @statementID";
                                MakeParam(cmd, "statementID", statementID);
                            }

                            break;
                        }
                    case ReportIntervalType.CurrentSaleOnly:
                        {

                            cmd.CommandText += " t.TRANSDATE >= @nowDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "nowDate", DateTime.Now.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);
                            break;
                        }
                }

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeid", storeID);

                object result = entry.Connection.ExecuteScalar(cmd);

                if (result == DBNull.Value)
                {
                    return 0;
                }
                else
                {
                    return (int)result;
                }
                
            }
        }

        public int GetNumOfTransactions(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType)
        {
            using(IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @" SELECT COUNT(*) NUMOFTRANSACTIONS FROM RBOTRANSACTIONTABLE t 
                                     WHERE t.RECEIPTID > 0 
                                     AND t.ENTRYSTATUS = 0 
                                     AND t.STORE = @storeid 
                                     AND t.DATAAREAID = @dataAreaID AND ";
                switch (intervalType)
                {
                    case ReportIntervalType.ByDate:
                        {
                            cmd.CommandText += " t.TRANSDATE >= @fromDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "fromDate", from.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);

                            if (statementID != RecordIdentifier.Empty)
                            {
                                cmd.CommandText += "AND t.STATEMENTID = @statementID";
                                MakeParam(cmd, "statementID", statementID);
                            }

                            break;
                        }
                    case ReportIntervalType.CurrentSaleOnly:
                        {

                            cmd.CommandText += " t.TRANSDATE >= @nowDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "nowDate", DateTime.Now.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);
                            break;
                        }
                }

                MakeParam(cmd, "storeid", storeID);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                object result = entry.Connection.ExecuteScalar(cmd);

                if (result == DBNull.Value)
                {
                    return 0;
                }
                else
                {
                    return (int)result;
                }
            }
        }

        public int GetNumberOfOpenedSales(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @" SELECT COUNT(*) AS OPENEDNOSALES FROM RBOTRANSACTIONTABLE t
                                     WHERE OPENDRAWER = 1 
                                     AND ENTRYSTATUS <> 5 
                                     AND ENTRYSTATUS = 0
                                     AND t.STORE = @storeid 
                                     AND t.DATAAREAID = @dataAreaID
                                     and ";
                //Entry status 5 are transactions done in training mode
                switch (intervalType)
                {
                    case ReportIntervalType.ByDate:
                        {
                            cmd.CommandText += " t.TRANSDATE >= @fromDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "fromDate", from.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);

                            if (statementID != RecordIdentifier.Empty)
                            {
                                cmd.CommandText += "AND t.STATEMENTID = @statementID";
                                MakeParam(cmd, "statementID", statementID);
                            }

                            break;
                        }
                    case ReportIntervalType.CurrentSaleOnly:
                        {

                            cmd.CommandText += " t.TRANSDATE >= @nowDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "nowDate", DateTime.Now.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);
                            break;
                        }
                }

                MakeParam(cmd, "storeid", storeID);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                object result = entry.Connection.ExecuteScalar(cmd);

                if (result == DBNull.Value)
                {
                    return 0;
                }
                else
                {
                    return (int)result;
                }
            }
        }

        public decimal GetNumberOfItemsSold(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType)
        {
            using(IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText  = @" SELECT SUM(b.QTY)*(-1) AS NUMOFITEMSSOLD FROM RBOTRANSACTIONTABLE t, RBOTRANSACTIONSALESTRANS b 
                                      WHERE t.TRANSACTIONID = b.TRANSACTIONID AND t.DATAAREAID = b.DATAAREAID 
                                      AND t.STORE = b.STORE 
                                      AND t.TERMINAL = b.TERMINALID 
                                      AND t.ENTRYSTATUS = 0
                                      AND t.STORE = @storeid 
                                      AND t.DATAAREAID = @dataAreaID 
                                      AND b.TRANSACTIONSTATUS = 0 and ";  //in case of a voided sales item; 0 == Normal

                switch (intervalType)
                {
                    case ReportIntervalType.ByDate:
                        {
                            cmd.CommandText += " t.TRANSDATE >= @fromDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "fromDate", from.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);

                            if (statementID != RecordIdentifier.Empty)
                            {
                                cmd.CommandText += "AND t.STATEMENTID = @statementID";
                                MakeParam(cmd, "statementID", statementID);
                            }

                            break;
                        }
                    case ReportIntervalType.CurrentSaleOnly:
                        {

                            cmd.CommandText += " t.TRANSDATE >= @nowDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "nowDate", DateTime.Now.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);
                            break;
                        }
                }

                MakeParam(cmd, "storeid", storeID);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                object result = entry.Connection.ExecuteScalar(cmd);
                if (result == DBNull.Value)
                {
                    return 0;
                }
                else
                {
                    return (decimal)result;
                }
            }
        }

        public decimal GetSumOfNegative(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType)
        {
            using(IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @" SELECT SUM(t.GROSSAMOUNT) AS SUMOFNEGATIVE FROM RBOTRANSACTIONTABLE t
                                     WHERE t.GROSSAMOUNT > 0
                                     AND t.ENTRYSTATUS = 0
                                     AND t.DATAAREAID = @dataAreaID
                                     AND t.STORE = @storeid and";

                switch (intervalType)
                {
                    case ReportIntervalType.ByDate:
                        {
                            cmd.CommandText += " t.TRANSDATE >= @fromDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "fromDate", from.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);

                            if (statementID != RecordIdentifier.Empty)
                            {
                                cmd.CommandText += "AND t.STATEMENTID = @statementID";
                                MakeParam(cmd, "statementID", statementID);
                            }

                            break;
                        }
                    case ReportIntervalType.CurrentSaleOnly:
                        {

                            cmd.CommandText += " t.TRANSDATE >= @nowDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "nowDate", DateTime.Now.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);
                            break;
                        }
                }

                MakeParam(cmd, "storeid", storeID);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                object result = entry.Connection.ExecuteScalar(cmd);
                if (result == DBNull.Value)
                {
                    return 0;
                }
                else
                {
                    return (decimal)result;
                }
            }
        }

        public List<HourlyDataLine> GetHourlyDataLines(IConnectionManager entry, bool includeReportFormatting, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType)
        {
            using(IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<HourlyDataLine> result;

                cmd.CommandText = @" SELECT CASE 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 0 THEN '00:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 1 THEN '01:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 2 THEN '02:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 3 THEN '03:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 4 THEN '04:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 5 THEN '05:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 6 THEN '06:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 7 THEN '07:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 8 THEN '08:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 9 THEN '09:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 10 THEN '10:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 11 THEN '11:00'
                                    WHEN DATEPART(hour,t.TRANSDATE) = 12 THEN '12:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 13 THEN '13:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 14 THEN '14:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 15 THEN '15:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 16 THEN '16:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 17 THEN '17:00'
                                    WHEN DATEPART(hour,t.TRANSDATE) = 18 THEN '18:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 19 THEN '19:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 20 THEN '20:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 21 THEN '21:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 22 THEN '22:00' 
                                    WHEN DATEPART(hour,t.TRANSDATE) = 23 THEN '23:00' 
                                    END AS TIME,  count(distinct t.transactionid) AS NUMBEROFTRANSACTIONS, 
                                    SUM(s.NETAMOUNTINCLTAX)*(-1) AS AMOUNT, 
                                    SUM(s.QTY)*(-1) AS NUMOFITEMSSOLD
                                    FROM RBOTRANSACTIONTABLE t
                                    join rbotransactionsalestrans s on 
                                     t.transactionid = s.transactionid AND 
                                     t.STORE = s.STORE AND 
                                     t.TERMINAL = s.TERMINALID AND 
                                     t.dataareaid = s.dataareaid 
                                    WHERE ";
                
                switch (intervalType)
                {
                    case ReportIntervalType.ByDate:
                        {
                            cmd.CommandText += " t.TRANSDATE >= @fromDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "fromDate", from.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);

                            if (statementID != RecordIdentifier.Empty)
                            {
                                cmd.CommandText += "AND t.STATEMENTID = @statementID";
                                MakeParam(cmd, "statementID", statementID);
                            }

                            break;
                        }
                    case ReportIntervalType.CurrentSaleOnly:
                        {

                            cmd.CommandText += " t.TRANSDATE >= @nowDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "nowDate", DateTime.Now.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);
                            break;
                        }
                }

                cmd.CommandText += @" AND t.RECEIPTID > 0 
                                      AND t.ENTRYSTATUS = 0
                                      AND t.STORE = @storeid 
                                      AND s.TRANSACTIONSTATUS = 0
                                      GROUP BY DATEPART(hh,t.TRANSDATE)
                                      ORDER BY DATEPART(hh,t.TRANSDATE) ASC "; 
                                      
                MakeParam(cmd, "storeid", storeID);

                result = Execute<HourlyDataLine>(entry, cmd, CommandType.Text, includeReportFormatting, PopulateHourlyDataLine);


                if (includeReportFormatting)
                {
                    HourlyDataLine.PriceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
                }
                return result;
            }
        }

        private static void PopulateHourlyDataLine(IConnectionManager entry, IDataReader dr, HourlyDataLine hourlyLine, object includeReportFormatting)
        {
            hourlyLine.Time = (string)dr["TIME"];           
            hourlyLine.NumberOfTransactions = (int)dr["NUMBEROFTRANSACTIONS"];
            hourlyLine.NumberOfItemsSold = (decimal)dr["NUMOFITEMSSOLD"];
            hourlyLine.Amount = (decimal)dr["AMOUNT"];           
        }

        public List<CurrencyDataLine> GetCurrencyDataLines(IConnectionManager entry, bool includeReportFormatting, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<CurrencyDataLine> result;

                ValidateSecurity(entry);


                cmd.CommandText = @"SELECT b.CURRENCY, 
                     COUNT(*) AS SALESNUMBERS,  
                     SUM(b.AMOUNTTENDERED) AS AMOUNT 
                     FROM  
                     RBOTRANSACTIONPAYMENTTRANS b  
                     Join RBOTRANSACTIONTABLE t on b.STORE = t.STORE AND t.TERMINAL = b.TERMINAL AND t.TRANSACTIONID = b.TRANSACTIONID AND b.DATAAREAID = t.DATAAREAID 
                     Join RBOSTORETABLE s on s.STOREID = t.STORE AND t.DATAAREAID = s.DATAAREAID  
                     Join RBOSTORETENDERTYPETABLE stt on b.TENDERTYPE = stt.TENDERTYPEID AND b.DATAAREAID = stt.DATAAREAID
                     WHERE 
                     b.CURRENCY != s.CURRENCY 
                     AND t.ENTRYSTATUS = 0
                     AND b.TRANSACTIONSTATUS = 0 
                     AND s.STOREID = @storeID  
                     AND b.DATAAREAID = @dataAreaID and ";
                switch (intervalType)
                {
                    case ReportIntervalType.ByDate:
                        {
                            cmd.CommandText += " t.TRANSDATE >= @fromDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "fromDate", from.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);

                            if (statementID != RecordIdentifier.Empty)
                            {
                                cmd.CommandText += "AND t.STATEMENTID = @statementID";
                                MakeParam(cmd, "statementID", statementID);
                            }
                            break;
                        }
                    case ReportIntervalType.CurrentSaleOnly:
                        {

                            cmd.CommandText += " t.TRANSDATE >= @nowDate and t.TRANSDATE < @toDate ";
                            
                            MakeParam(cmd, "nowDate", DateTime.Now.Date, SqlDbType.DateTime);
                            MakeParam(cmd, "toDate", to, SqlDbType.DateTime);
                            break;
                        }
                }
                cmd.CommandText += " GROUP BY b.CURRENCY, s.STOREID, s.CURRENCY  ";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", storeID);


                result = Execute<CurrencyDataLine>(entry, cmd, CommandType.Text, includeReportFormatting, PopulateCurrencyDataLine);
                return result;
            }
        }

        private static void PopulateCurrencyDataLine(IConnectionManager entry, IDataReader dr, CurrencyDataLine currencyLine, object includeReportFormatting)
        {
            currencyLine.Currency = (string)dr["CURRENCY"];
            currencyLine.SalesNumbers = (int)dr["SALESNUMBERS"];
            currencyLine.Amount = (decimal)dr["AMOUNT"];
        }
    }
}
