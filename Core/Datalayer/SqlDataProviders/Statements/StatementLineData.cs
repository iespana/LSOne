using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders.Statements;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Statements
{
    /// <summary>
    /// Data provider class for statements
    /// </summary>
    public class StatementLineData : SqlServerDataProviderBase, IStatementLineData
    {
        private string BaseSql
        {
            get
            {
                return @"SELECT DISTINCT r.STATEMENTID, r.LINENUMBER , 
                        ISNULL(r.STAFFID,'') AS STAFFID, 
                        ISNULL(r.TERMINALID,'') AS TERMINALID, 
                        ISNULL(r.TENDERID,'') AS TENDERID, 
                        ISNULL(r.TRANSACTIONAMOUNT,'0') AS TRANSACTIONAMOUNT, 
                        ISNULL(r.BANKEDAMOUNT,'0') AS BANKEDAMOUNT, 
                        ISNULL(r.SAFEAMOUNT,'0') AS SAFEAMOUNT, 
                        ISNULL(r.COUNTEDAMOUNT,'0') AS COUNTEDAMOUNT, 
                        ISNULL(r.DIFFERENCE,'0') AS DIFFERENCE, 
                        COALESCE(t.NAME, rs.NAME,'') AS TENDERNAME, 
                        ISNULL(r.CURRENCYCODE,'') AS CURRENCYCODE,
                        r.STATEMENTSTATUS
                        FROM RBOSTATEMENTLINE r 
                        Left outer join RBOTENDERTYPETABLE t on r.DATAAREAID = t.DATAAREAID and r.TENDERID = t.TENDERTYPEID 
                        Left outer join RBOSTORETENDERTYPETABLE rs on rs.DATAAREAID = t.DATAAREAID and r.TENDERID = rs.TENDERTYPEID ";
            }
        }

        private static void PopulateStatmentLine(IDataReader dr, StatementLine statementLine)
        {
            statementLine.StatementID = (string)dr["STATEMENTID"];
            statementLine.LineNumber = (string)dr["LINENUMBER"];
            //statementLine.StoreID = (string)dr["STOREID"];
            statementLine.TerminalID = (string)dr["TERMINALID"];
            statementLine.StaffID = (string)dr["STAFFID"];
            statementLine.TransactionAmount = (decimal)dr["TRANSACTIONAMOUNT"];
            statementLine.BankedAmount = (decimal)dr["BANKEDAMOUNT"];
            statementLine.SafeAmount = (decimal)dr["SAFEAMOUNT"];
            statementLine.CountedAmount = (decimal)dr["COUNTEDAMOUNT"];
            statementLine.Difference = (decimal)dr["DIFFERENCE"];
            statementLine.StaffID = (string)dr["STAFFID"];
            statementLine.TenderID = (string)dr["TENDERID"];
            statementLine.TenderName = (string)dr["TENDERNAME"];
            statementLine.CurrencyCode = (string)dr["CURRENCYCODE"];
            statementLine.StatementStatus = (AllowEODEnums)dr["STATEMENTSTATUS"];
        }

        public virtual void DeleteLines(IConnectionManager entry, RecordIdentifier statementInfoID)
        {
            DeleteRecord(entry, "RBOSTATEMENTLINE", "STATEMENTID", statementInfoID, BusinessObjects.Permission.RunEndOfDay);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "RBOStatementLine", new[]{"STATEMENTID", "LINENUMBER"}, id);
        }

        public virtual StatementLine Get(IConnectionManager entry, RecordIdentifier statementID, RecordIdentifier lineNumber)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + 
                                "WHERE r.DATAAREAID = @dataareaid AND r.STATEMENTID = @statementID and r.LINENUMBER = @lineNumber";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "statementID", (string)statementID);
                MakeParam(cmd, "lineNumber", (string)lineNumber);

                var result = Execute<StatementLine>(entry, cmd, CommandType.Text, PopulateStatmentLine);
                return (result.Count > 0) ? result[0] : null;
            }            
        }

        public virtual List<StatementLine> GetStatementLines(IConnectionManager entry, RecordIdentifier statementID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql +
                                " WHERE r.DATAAREAID = @dataareaid AND r.STATEMENTID = @statementID" +
                                " ORDER BY  ISNULL(r.TERMINALID,''), ISNULL(r.TENDERID,'')";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "statementID", (string)statementID);

                return Execute<StatementLine>(entry, cmd, CommandType.Text, PopulateStatmentLine);
            }
        }
       
        public virtual void Save(IConnectionManager entry, StatementLine statementLine)
        {
            var statement = new SqlServerStatement("RBOSTATEMENTLINE");

            ValidateSecurity(entry, BusinessObjects.Permission.RunEndOfDay);

            if (!Exists(entry, statementLine.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("STATEMENTID", (string)statementLine.StatementID);
                statement.AddKey("LINENUMBER", (string)statementLine.LineNumber);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("STATEMENTID", (string)statementLine.StatementID);
                statement.AddCondition("LINENUMBER", (string)statementLine.LineNumber);
            }

            statement.AddField("STAFFID", statementLine.StaffID);
            statement.AddField("TERMINALID", statementLine.TerminalID);
            statement.AddField("CURRENCYCODE", statementLine.CurrencyCode);
            statement.AddField("TENDERID", statementLine.TenderID);
            statement.AddField("TRANSACTIONAMOUNT", statementLine.TransactionAmount, SqlDbType.Decimal);
            statement.AddField("BANKEDAMOUNT", statementLine.BankedAmount, SqlDbType.Decimal);
            statement.AddField("SAFEAMOUNT", statementLine.SafeAmount, SqlDbType.Decimal);
            statement.AddField("COUNTEDAMOUNT", statementLine.CountedAmount, SqlDbType.Decimal);
            statement.AddField("DIFFERENCE", statementLine.Difference, SqlDbType.Decimal);
            statement.AddField("STATEMENTSTATUS", statementLine.StatementStatus, SqlDbType.Int);
            
            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "StatementLine"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOSTATEMENTLINE", "STATEMENTID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion


    }
}
