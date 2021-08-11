using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Fiscal;
using LSOne.DataLayer.DataProviders.Fiscal;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Fiscal
{
    public partial class FiscalData : SqlServerDataProviderBase, IFiscalData
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="id">RECEIPTID, Store, Terminal</param>
        /// <returns></returns>
        public bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            string[] fields = { "RECEIPTID", "STORE", "TERMINAL" };
            return RecordExists(entry, "RBOTRANSACTIONFISCALTRANS", fields, id);
        }

        public void Save(IConnectionManager entry, List<object> fiscalObjList)
        {
            //List 0 - ReceiptID
            //List 1 - StoreID
            //List 2 - TerminalID
            //List 3 - FiscalUnitID
            //List 4 - FiscalControlID
            //List 5 - TransactionID

            var statement = new SqlServerStatement("RBOTRANSACTIONFISCALTRANS");
            RecordIdentifier id = new RecordIdentifier(fiscalObjList[0].ToString(),
                                                   new RecordIdentifier(fiscalObjList[1].ToString(), fiscalObjList[2].ToString()));
            statement.AddField("FISCALUNITID", fiscalObjList[3].ToString());
            statement.AddField("FISCALCONTROLID", fiscalObjList[4].ToString());
            statement.AddField("TRANSACTIONID", fiscalObjList[5].ToString());

            if (Exists(entry, id))
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("RECEIPTID", (string)id[0]);
                statement.AddCondition("STORE", (string)id[1]);
                statement.AddCondition("TERMINAL", (string)id[2]);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("RECEIPTID", (string)id[0]);
                statement.AddKey("STORE", (string)id[1]);
                statement.AddKey("TERMINAL", (string)id[2]);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Save fiscal transaction details to db
        /// </summary>
        /// <param name="entry">The connection manager</param>
        /// <param name="fiscalTrans">The fiscal transaction</param>
		public void Save(IConnectionManager entry, FiscalTrans fiscalTrans)
        {
            var statement = new SqlServerStatement("RBOTRANSACTIONFISCALTRANS");
            RecordIdentifier id = new RecordIdentifier(fiscalTrans.ReceiptId, fiscalTrans.Store, fiscalTrans.Terminal);

            statement.AddField("TRANSACTIONID", fiscalTrans.TransactionId);
            statement.AddField("FISCALUNITID", fiscalTrans.FiscalUnitId);
            statement.AddField("FISCALCONTROLID", fiscalTrans.FiscalControlId);
            statement.AddField("TYPE", fiscalTrans.Type, SqlDbType.Int);
            statement.AddField("TRANSDATE", fiscalTrans.TransDate, SqlDbType.DateTime);
            statement.AddField("GROSSAMOUNT", fiscalTrans.GrossAmount, SqlDbType.Decimal);
            statement.AddField("NETAMOUNT", fiscalTrans.NetAmount, SqlDbType.Decimal);
            statement.AddField("PRIVATEKEYVER", fiscalTrans.PrivateKeyVersion);
            statement.AddField("SIGNATURE", fiscalTrans.Signature);

            statement.StatementType = StatementType.Insert;
            statement.AddKey("RECEIPTID", (string)id[0]);
            statement.AddKey("STORE", (string)id[1]);
            statement.AddKey("TERMINAL", (string)id[2]);
            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Get signature of last transaction if it is for new transaction or signature of previous transaction if it is to verify signature of current transaction.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="store">ID of the store</param>
        /// <param name="termianl">ID of the POS terminal</param>
        /// <param name="transactionId">ID of the transaction</param>
        /// <param name="newTrans">Indicator if it is for creating new transaction or verify signature of existing transaction</param>
        /// <returns>Signature of last transaction if it is for new transasction or signature of previous last transaction if it is to verify signature of the selected transaction</returns>
		public string GetLastSignature(IConnectionManager entry, string store, string terminal, string transactionId, bool newTrans)
		{
			FiscalTrans fiscalTrans = new FiscalTrans();

			using (SqlCommand cmd = new SqlCommand())
			{
				cmd.CommandText =
						  @"Select
	                            Top 1 ISNULL(SIGNATURE,'') AS SIGNATURE
                            From RBOTRANSACTIONFISCALTRANS
                            Where STORE = @store and TERMINAL = @terminal
                                and DATAAREAID = @dataAreaId ";

				if (newTrans == false)
				{
					cmd.CommandText = cmd.CommandText + "and TRANSACTIONID < @transId "; // If it is to verify the signature, get the previous last one
					MakeParam(cmd, "transId", transactionId);
				}

				cmd.CommandText = cmd.CommandText + "Order by TRANSACTIONID DESC";

				MakeParam(cmd, "store", store);
				MakeParam(cmd, "terminal", terminal);
				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

				var result = Execute<FiscalTrans>(entry, cmd, CommandType.Text, PopulateLastSignature);
				fiscalTrans = (result.Count > 0) ? result[0] : null;
			}

			if (fiscalTrans != null && fiscalTrans.Signature != "")
				return fiscalTrans.Signature;

			return EncodeBase64("0");
		}

		public void Save(IConnectionManager entry, BusinessObjects.Fiscal.FiscalLogEntity item)
        {
            throw new NotImplementedException();
        }

        public void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            throw new NotImplementedException();
        }
    }
}
