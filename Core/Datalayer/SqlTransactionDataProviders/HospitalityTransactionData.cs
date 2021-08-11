using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    /// <summary>
    /// A data provider for the <see cref="HospitalityTransaction"/> business object
    /// </summary>
    public class HospitalityTransactionData : SqlServerDataProviderBase, IHospitalityTransactionData
    {
        /// <summary>
        /// Gets the last split transaction created.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The store ID</param>
        /// <param name="terminalID">The terminal ID</param>
        /// <param name="tableID">The table ID</param>
        /// <param name="guest">The guest number</param>
        /// <param name="hospitalityType">The selected hospitality</param>
        /// <param name="cache">The cache selection</param>
        /// <returns>Returns the last split hospitality transaction</returns>
        public virtual HospitalityTransaction GetLastSplitTransaction(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier tableID, RecordIdentifier guest, RecordIdentifier hospitalityType, RecordIdentifier splitID, CacheType cache = CacheType.CacheTypeNone)
        {
            RecordIdentifier hospInfo = new RecordIdentifier("", storeID, terminalID, tableID, guest, hospitalityType, splitID);
            return GetLastSplitTransaction(entry, hospInfo, cache);
        }

        /// <summary>
        /// Gets the last split transaction created.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="hospitalityTransactionInfo">The hospitality transaction info</param>
        /// <param name="cache">The cache selection</param>
        /// <returns>Returns the last split hospitality transaction.</returns>
        public virtual HospitalityTransaction GetLastSplitTransaction(IConnectionManager entry, RecordIdentifier hospitalityTransactionInfo, CacheType cache = CacheType.CacheTypeNone)        
        {            
            var hospTransactions = GetHospitalityTransactions(entry, hospitalityTransactionInfo, cache);

            if (hospTransactions == null)
            {
                return null;
            }

            var transaction = hospTransactions.OrderByDescending(o => o.TransactionID).FirstOrDefault();

            if (transaction != null)
            {
                return transaction;
            }

            return null;
        }

        /// <summary>
        /// Gets a hospitality transaction with a specific combined id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id (TransactionID, StoreID, TerminalID, TableID, Guest, HospitalityType)</param>
        /// <param name="cache">The selected cache</param>
        /// <returns>The <see cref="HospitalityTransaction "/> found</returns>
        public virtual HospitalityTransaction Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            var transactions = GetHospitalityTransactions(entry, id, cache);
            return (transactions == null) ? null : (transactions.Count > 0 ? transactions[0] : null);
        }

        /// <summary>
        /// Gets a list of hospitality transactions with a specific combined ID. Any part of the ID can be empty
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The combined ID (TransactionID, StoreID, TerminalID, TableID, Guest, HospitalityType)</param>
        /// <param name="cache">The selected cache</param>
        /// <returns>List of <see cref="HospitalityTransaction"/></returns>
        public virtual List<HospitalityTransaction> GetList(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            var transactionID = id[0];
            var storeID = id[1];
            var tableID = id[3];
            var guest = id[4];
            var hospitalityType = id[5];
            var splitID = id[6];

            return GetHospitalityTransactions(
                entry, 
                transactionID, 
                storeID, 
                tableID, 
                guest, 
                hospitalityType,
                splitID,
                cache);            
        }

        /// <summary>
        /// Gets the hospitality transactions.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="hospitalityTransactionInfo">The hospitality transaction info.</param>
        /// <param name="cache">The cache.</param>
        /// <returns>List{HospitalityTransaction}.</returns>
        private static List<HospitalityTransaction> GetHospitalityTransactions(IConnectionManager entry, RecordIdentifier hospitalityTransactionInfo, CacheType cache = CacheType.CacheTypeNone)
        {
            var transactionID = hospitalityTransactionInfo[0];
            var storeID = hospitalityTransactionInfo[1];
            var tableID = hospitalityTransactionInfo[3];
            var guest = hospitalityTransactionInfo[4];
            var hospitalityType = hospitalityTransactionInfo[5];
            var splitID = hospitalityTransactionInfo[6];
            
            return GetHospitalityTransactions(entry, 
                transactionID, 
                storeID,                 
                tableID, 
                guest, 
                hospitalityType, 
                splitID,
                cache);
        }

        /// <summary>
        /// Returns hospitality transactions using the parameters provided
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">Store id to look for - if empty then it's ignored and not added to SQL statement</param>
        /// <param name="tableID">Table id to look for - if empty then it's ignored and not added to SQL statement</param>
        /// <param name="hospitalityType">Hospitality type to look for - if empty then it's ignored and not added to SQL statement</param>
        /// <param name="transactionID">Transaction id to look for - if empty then it's ignored and not added to SQL statement</param>
        /// <param name="guest">Guest id to look for - if set to less than zero then it's ignored and not added to SQL statement</param>   
        /// <param name="cache">The type of cache that should be used</param>
        /// <returns>List of <see cref="HospitalityTransaction"/>s found.</returns>
        private static List<HospitalityTransaction> GetHospitalityTransactions(IConnectionManager entry, 
                                                                               RecordIdentifier transactionID, 
                                                                               RecordIdentifier storeID,                                                                                
                                                                               RecordIdentifier tableID, 
                                                                               RecordIdentifier guest, 
                                                                               RecordIdentifier hospitalityType, 
                                                                               RecordIdentifier splitID,
                                                                               CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT TRANSACTIONID, STOREID, TERMINALID, TABLEID, GUEST, HOSPITALITYTYPE, ISNULL(SPLITID, '00000000-0000-0000-0000-000000000000') SPLITID,
                                    ISNULL(TRANSACTIONXML, '') AS TRANSACTIONXML, ISNULL(CREATEDBY, 0) AS CREATEDBY
                                    FROM POSISHOSPITALITYTRANSTABLE 
                                    WHERE DATAAREAID = @dataAreaID ";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                if (splitID.StringValue != "")
                {
                    cmd.CommandText += " AND SPLITID = @SPLITID ";
                    MakeParam(cmd, "SPLITID", (Guid)splitID, SqlDbType.UniqueIdentifier);
                }

                if (transactionID.StringValue != "")
                {
                    cmd.CommandText += " AND TRANSACTIONID = @transactionID ";
                    MakeParam(cmd, "transactionID", transactionID);
                }

                if (storeID.StringValue != "")
                {
                    cmd.CommandText += " AND STOREID = @storeID ";
                    MakeParam(cmd, "storeID", storeID);
                }

                if ((tableID.IsInteger && (int)tableID > 0) || (!tableID.IsInteger && Conversion.ToInt(tableID.StringValue) > 0))
                {
                    cmd.CommandText += " AND TABLEID = @tableID ";
                    MakeParam(cmd, "tableID", tableID);
                }

                if (hospitalityType.StringValue != "")
                {
                    cmd.CommandText += " AND HOSPITALITYTYPE = @hospitalityType ";
                    MakeParam(cmd, "hospitalityType", hospitalityType);
                }

                if ((int)guest >= 0)
                {
                    cmd.CommandText += " AND GUEST = @guest ";
                    MakeParam(cmd, "guest", guest);
                }

                var result = Execute<HospitalityTransaction>(entry, cmd, CommandType.Text, PopulateTransaction);

                return result.Count > 0 ? result : null;
            }
        }

        private static void PopulateTransaction(IDataReader dr, HospitalityTransaction transaction)
        {
            transaction.TransactionID = (string)dr["TRANSACTIONID"];
            transaction.StoreID = (string)dr["STOREID"];
            transaction.TerminalID = (string)dr["TERMINALID"];
            transaction.TableID = (string)dr["TABLEID"];
            transaction.Guest = (int)dr["GUEST"];
            transaction.HospitalityType = (string)dr["HOSPITALITYTYPE"];
            transaction.SplitID = (Guid)dr["SPLITID"];
            transaction.Transaction.ToClass(XDocument.Parse((string)dr["TRANSACTIONXML"]).Root);
        }

        /// <summary>
        /// Returns true if the records the exists.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The Hospitality transaction to check</param>
        /// <returns><c>true</c> if the record exists, <c>false</c> otherwise</returns>
        public virtual bool RecordExists(IConnectionManager entry, HospitalityTransaction transaction)
        {
            var entity = (HospitalityTransaction)entry.Cache.GetEntityFromCache(typeof(HospitalityTransaction), transaction.ID);
            if (entity != null)
            {
                return true;
            }
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);
                cmd.CommandText = @"SELECT TRANSACTIONID 
                                    FROM POSISHOSPITALITYTRANSTABLE 
                                    WHERE TRANSACTIONID = @TransactionID 
                                    AND STOREID = @StoreID 
                                    AND TERMINALID = @TerminalID 
                                    AND GUEST = @Guest 
                                    AND HOSPITALITYTYPE = @Hospitalitytype 
                                    AND SPLITID = @splitID 
                                    AND DATAAREAID = @dataAreaId";

                MakeParam(cmd, "TransactionID", transaction.TransactionID);
                MakeParam(cmd, "StoreID", transaction.StoreID);
                MakeParam(cmd, "TerminalID", transaction.TerminalID);
                MakeParam(cmd, "Guest", (int)transaction.Guest, SqlDbType.Int);
                MakeParam(cmd, "Hospitalitytype", transaction.HospitalityType);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "splitID", (Guid)transaction.SplitID, SqlDbType.UniqueIdentifier);
                using (var dr = entry.Connection.ExecuteReader(cmd, CommandType.Text))
                {
                    return dr.Read();
                }
            }
        }

        /// <summary>
        /// Deletes the specified Hospitality transaction.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The transaction to be deleted</param>
        public virtual void Delete(IConnectionManager entry, HospitalityTransaction transaction)
        {
            entry.Cache.DeleteEntityFromCache(typeof(HospitalityTransaction), transaction.ID);

            var statement = new SqlServerStatement("POSISHOSPITALITYTRANSTABLE", StatementType.Delete);
            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);            
            statement.AddCondition("STOREID", (string)transaction.StoreID);            
            statement.AddCondition("TABLEID", (string)transaction.TableID);
            statement.AddCondition("GUEST", (int)transaction.Guest, SqlDbType.Int);
            statement.AddCondition("HOSPITALITYTYPE", (string)transaction.HospitalityType);
            statement.AddCondition("SPLITID", (Guid)transaction.SplitID, SqlDbType.UniqueIdentifier);
            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Saves the Hospitality transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The transaction to be saved</param>
        public virtual void Save(IConnectionManager entry, HospitalityTransaction transaction)
        {
            IConnectionManager dbTransaction;
            if (!(entry is IConnectionManagerTransaction))
            {
                // If we already have a transaction then we may not create transactions since SqlServer does not support that.
                dbTransaction = entry.CreateTransaction();
            }
            else
            {
                dbTransaction = entry;
            }

            try
            {
                var statement = new SqlServerStatement("POSISHOSPITALITYTRANSTABLE");

                bool isNew = transaction.ID == RecordIdentifier.Empty;

                if (isNew || !RecordExists(dbTransaction, transaction))
                {
                    if (transaction.TransactionID == RecordIdentifier.Empty)
                    {
                        transaction.TransactionID = transaction.Transaction.TransactionId;
                    }
                    statement.StatementType = StatementType.Insert;
                    statement.AddKey("TRANSACTIONID", (string) transaction.TransactionID);
                    statement.AddKey("STOREID", (string) transaction.StoreID);
                    statement.AddKey("TERMINALID", (string) transaction.TerminalID);
                    statement.AddKey("TABLEID", (string) transaction.TableID);
                    statement.AddKey("GUEST", (int) transaction.Guest, SqlDbType.Int);
                    statement.AddKey("HOSPITALITYTYPE", (string) transaction.HospitalityType);
                    statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                }
                else
                {
                    statement.StatementType = StatementType.Update;
                    statement.AddCondition("TRANSACTIONID", (string) transaction.TransactionID);
                    statement.AddCondition("STOREID", (string) transaction.StoreID);
                    statement.AddCondition("TERMINALID", (string) transaction.TerminalID);
                    statement.AddCondition("TABLEID", (string) transaction.TableID);
                    statement.AddCondition("GUEST", (int) transaction.Guest, SqlDbType.Int);
                    statement.AddCondition("HOSPITALITYTYPE", (string) transaction.HospitalityType);
                    statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                }
                statement.AddField("TRANSACTIONXML", transaction.Transaction.ToXML().ToString(), SqlDbType.Xml);
                statement.AddField("CREATEDBY", transaction.CreatedBy, SqlDbType.Int);
                statement.AddField("SPLITID", (Guid)transaction.SplitID, SqlDbType.UniqueIdentifier);

                Save(dbTransaction, transaction, statement);
                if (!(entry is IConnectionManagerTransaction))
                {
                    ((IConnectionManagerTransaction)dbTransaction).Commit();
                }
            }
            catch (Exception ex)
            {
                if (!(entry is IConnectionManagerTransaction))
                {
                    ((IConnectionManagerTransaction) dbTransaction).Rollback();
                }

                throw ex;
            }
        }
    }
}
