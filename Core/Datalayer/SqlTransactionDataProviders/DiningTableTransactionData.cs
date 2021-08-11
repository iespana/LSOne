using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    /// <summary>
    /// A data provider for the <see cref="DiningTableTransaction"/> business object
    /// </summary>
    public class DiningTableTransactionData : SqlServerDataProviderBase, IDiningTableTransactionData
    {
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier diningTableTransactionID)
        {
            return RecordExists(entry,
                                "RBOTRANSACTIONDININGTABLE",
                                new[] {"TRANSACTIONID", "STORE", "TERMINAL"},
                                diningTableTransactionID);
        }

        /// <summary>
        /// Saves the dining table transaction into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The <see cref="DiningTableTransaction"/> business object to save</param>
        public virtual void Save(IConnectionManager entry, DiningTableTransaction transaction)
        {
            SqlServerStatement statement = new SqlServerStatement("RBOTRANSACTIONDININGTABLE", StatementType.Insert, false);            

            if (Exists(entry, transaction.ID))
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("TRANSACTIONID", (string)transaction.TransactionID);
                statement.AddCondition("STORE", (string)transaction.StoreID);
                statement.AddCondition("TERMINAL", (string)transaction.TerminalID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("TRANSACTIONID", (string)transaction.TransactionID);
                statement.AddKey("STORE", (string)transaction.StoreID);
                statement.AddKey("TERMINAL", (string)transaction.TerminalID);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("SALESTYPE", (string)transaction.HospitalitySalesType);
            statement.AddField("DININGTABLELAYOUTID", (string)transaction.DiningTableLayoutID);
            statement.AddField("DINEINTABLENO", transaction.DiningTableNo, SqlDbType.Int);
            statement.AddField("NOOFGUESTS", transaction.NoOfGuests, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
