using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms.VisualStyles;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Fiscal;
using LSOne.DataLayer.DataProviders.Fiscal;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Fiscal
{
    public class FiscalLogData : SqlServerDataProviderBase, IFiscalLogData
    {

        private string BaseSql
        {
            get
            {
                return "Select ID, ENTRYDATE, ISNULL(PRINTSTRING, '') AS PRINTSTRING, ISNULL(OPERATIONID, 0) AS OPERATIONID FROM RBOTRANSACTIONFISCALLOG ";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="id">RECEIPTID, Store, Terminal</param>
        /// <returns></returns>
        public bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return true;
        }


        /// <summary>
        /// Gets a list of all tax codes as data entities
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<FiscalLogEntity> GetList(IConnectionManager entry, DateTime fromDate, DateTime toDate)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ID, EntryDate, ISNULL(PrintString, '') AS PrintString, ISNULL(OperationID, 0) AS OperationID from RBOTRANSACTIONFISCALLOG";
                cmd.CommandText += " WHERE EntryDate >= @fromDate AND EntryDate <= @toDate";

                MakeParam(cmd, "fromDate", fromDate, SqlDbType.DateTime);
                MakeParam(cmd, "toDate", toDate, SqlDbType.DateTime);
                return Execute<FiscalLogEntity>(entry, cmd, CommandType.Text, PopulateFiscalLog);
            }
        }

        private void PopulateFiscalLog(IDataReader dr, FiscalLogEntity log)
        {
            log.ID = new RecordIdentifier((int)dr["ID"]);
            log.EntryDate = dr["OperationID"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["EntryDate"];
            log.PrintString = (string)dr["PrintString"];
            log.Operation = dr["OperationID"] == DBNull.Value ? POSOperations.NoOperation : (POSOperations)(int)dr["OperationID"];
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string[] GetTransactionData(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented - use the other Save function
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="log"></param>
        public void Save(IConnectionManager entry, FiscalLogEntity log)
        {
            var statement = new SqlServerStatement("RBOTRANSACTIONFISCALLOG");

            statement.AddField("EntryDate", DateTime.Now, SqlDbType.DateTime);
            statement.AddField("PrintString", log.PrintString);
            statement.AddField("OperationID", (int)log.Operation, SqlDbType.Int);

            statement.StatementType = StatementType.Insert;

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="ID"></param>
        public void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            throw new NotImplementedException();
        }
    }
}
