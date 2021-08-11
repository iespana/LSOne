using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.IO;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class SerializedTransactionData : SqlServerDataProviderBase, ISerializedTransactionData
    {
        
        private static object lockObject = new object();

        private static FolderItem GetTransactionFile(IConnectionManager entry)
        {
            FolderItem transactionFile;
            FolderItem appData = FolderItem.GetSpecialFolder(Environment.SpecialFolder.LocalApplicationData);

            if (appData.Exists)
            {
                appData = appData.CreateChildDirectory("LS Retail");
                appData = appData.CreateChildDirectory("LS One POS");
                #pragma warning disable 618
                string fileName = ((SqlConnection)entry.Connection.NativeConnection).Database + ";" + (string)entry.CurrentStoreID + ";" + (string)entry.CurrentTerminalID + ".xml";
                #pragma warning restore 618
                transactionFile = appData.Child(fileName);
            }
            else
            {
                transactionFile = FolderItem.FromPath(string.Empty);
            }

            return transactionFile;
        }

        public virtual PosTransaction GetSerializedTransaction(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier terminalID, PosTransaction transaction)
        {
            lock (lockObject)
            {
                if (GetTransactionFile(entry).Exists)
                {
                    string xml = File.ReadAllText(GetTransactionFile(entry).AbsolutePath);
                    return PosTransaction.CreateTransFromXML(xml, transaction, null);
                }
                
                return null;
            }
        }

        public virtual bool UnconcludedTransactionExists(IConnectionManager entry)
        {
            return GetTransactionFile(entry).Exists;
        }

        public virtual void DropSerializedTransactions(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier terminalID)
        {
            lock (lockObject)
            {
                FolderItem transactionFile = GetTransactionFile(entry);
                if (transactionFile.Exists)
                {
                    transactionFile.Delete();
                }
            }
        }

        public virtual void SaveSerializedTransaction(IConnectionManager entry, PosTransaction transaction)
        {
            ThreadStart start = () =>
            {
                lock (lockObject)
                {
                    if (FolderItem.GetSpecialFolder(Environment.SpecialFolder.LocalApplicationData).Exists)
                    {
                        transaction.CreateXmlTransaction().Save(GetTransactionFile(entry).AbsolutePath);
                    }
                }
            };

            new Thread(start).Start();
        }
    }
}
