using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities
{
    internal class ReceiptListItem : DataEntity
    {
        string receiptID;
        Date transactionDate;

        RecordIdentifier storeID;
        RecordIdentifier terminalID;
        RecordIdentifier employeeID;

        string storeDescription;
        string terminalDescription;
        string employeeDescription;

        public ReceiptListItem() 
            : base()
        {
            receiptID = "";
            transactionDate = Date.Empty;
            storeID = RecordIdentifier.Empty;
            terminalID = RecordIdentifier.Empty;
            employeeID = RecordIdentifier.Empty;
            storeDescription = "";
            terminalDescription = "";
            employeeDescription = "";
        }

        public string ReceiptID
        {
            get { return receiptID; }
            set { receiptID = value; }
        }

        public Date TransactionDate
        {
            get { return transactionDate; }
            set { transactionDate = value; }
        }

        public RecordIdentifier StoreID
        {
            get { return storeID; }
            set { storeID = value; }
        }

        public RecordIdentifier TerminalID
        {
            get { return terminalID; }
            set { terminalID = value; }
        }

        public RecordIdentifier EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }

        public string StoreDescription
        {
            get { return storeDescription; }
            set { storeDescription = value; }
        }

        public string TerminalDescription
        {
            get { return terminalDescription; }
            set { terminalDescription = value; }
        }

        public string EmployeeDescription
        {
            get { return employeeDescription; }
            set { employeeDescription = value; }
        }
    }
}
