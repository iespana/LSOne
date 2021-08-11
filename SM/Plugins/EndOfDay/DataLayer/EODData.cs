using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using LSRetail.StoreController.SharedDatabase.Interfaces;
using LSRetail.StoreController.SharedDatabase.DataProviders;
using LSRetail.StoreController.SharedDatabase;

namespace LSRetail.StoreController.EndOfDay.DataLayer
{
    public class EODData : DataProviderBase
    {
        public EODData()
        {
        }

        public string GetNextStatementId(IConnectionManager entry)
        {
            SqlDataReader reader = null;

            try
            {
                SqlCommand cmd = new SqlCommand();

                ValidateSecurity(entry, BusinessObjects.Permission.RunEndOfDay);

                cmd.CommandText = "SELECT TOP 1 StatementId FROM RBOTRANSACTIONTABLE WHERE DATAAREAID = @DataAreaId AND StatementId <> @EmptyString ORDER BY TransactionId DESC";

                MakeParam(cmd, "DataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "EmptyString", string.Empty);
                reader = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                string result = string.Empty;
                if (reader.HasRows)
                {
                    result = reader.GetString(0);
                    if (result != string.Empty)
                        result = Convert.ToString(Convert.ToInt32(result) + 1);
                }

                return result == string.Empty ? "1" : result;
            }
            catch (Exception)
            {
                return "0";
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public DataTable GetTransactions(IConnectionManager entry)
        {
            DataTable resultTable = new DataTable();
            SqlDataReader reader = null;
            try
            {
                SqlCommand cmd = new SqlCommand();

                ValidateSecurity(entry, BusinessObjects.Permission.RunEndOfDay);

                cmd.CommandText = "Select Store,Terminal,Type,ReceiptId,TransactionId,Staff,TransDate,OpenDrawer,SaleIsReturnSale,CustAccount,ToAccount,EntryStatus ";
                cmd.CommandText += "from RBOTransactionTable where StatementId = @StatementID and DataAreaId = @DataAreaID order by Terminal,TransactionId";

                MakeParam(cmd, "StatementID", "");
                MakeParam(cmd, "DataAreaID", entry.Connection.DataAreaId);

                reader = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                if (reader.HasRows)
                {
                    resultTable.Load(reader);
                }
                
            }
            catch (Exception x)
            {
                throw x;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return resultTable;
        }

        public void MarkAllTransactionWithOutStatementId(IConnectionManager entry, string statementId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                
                cmd.CommandText = "UPDATE RBOTRANSACTIONTABLE ";
                cmd.CommandText += "SET StatementId = @StatementId WHERE StatementId = @EmptyStatement AND DataAreaId = @DataAreaId";

                MakeParam(cmd, "StatementId", statementId);
                MakeParam(cmd, "DataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "EmptyStatement", string.Empty);

                entry.Connection.ExecuteNonQuery(cmd, false, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
