using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Transactions
{
    [Serializable()]
    public abstract class BaseTransaction : DataEntity
    {
        RecordIdentifier storeID;
        RecordIdentifier terminalID;
        RecordIdentifier transactionID;

        public BaseTransaction()
            : base()
        {
            storeID = "";
            terminalID = "";
            transactionID = "";
        }

        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(transactionID, new RecordIdentifier(storeID, terminalID));
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public RecordIdentifier StoreID 
        {
            get { return storeID; }
            set {storeID = value;}
        }

        public RecordIdentifier TerminalID
        {
            get { return terminalID; }
            set { terminalID = value; }
        }

        public RecordIdentifier TransactionID
        {
            get { return transactionID; }
            set { transactionID = value; }
        }
    }
}
