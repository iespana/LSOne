using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Fiscal
{
    public class FiscalTransSearchFilter
    {
        public Date DateFrom;
        public Date DateTo;
        public string IdOrReceiptNumber;
        public bool IdOrReceiptNumberBeginsWith;
        public RecordIdentifier EmployeeID;
        public RecordIdentifier StoreID;
        public RecordIdentifier TerminalID;
        public RecordIdentifier Currency;
        public decimal PaidAmount;
        public int RecordFrom;
        public int RecordTo;
        public FiscalSort Sort;
        public bool SortBackwards;
        public string Signature;
        public bool SignatureBeginsWith;

        public FiscalTransSearchFilter()
        {
            DateFrom = Date.Empty;
            DateTo = Date.Empty;
            IdOrReceiptNumber = "";
            IdOrReceiptNumberBeginsWith = true;
            EmployeeID = RecordIdentifier.Empty;
            StoreID = RecordIdentifier.Empty;
            TerminalID = RecordIdentifier.Empty;
            Currency = RecordIdentifier.Empty;
            PaidAmount = 0;
            RecordFrom = 0;
            RecordTo = 0;
            Signature = "";
            SignatureBeginsWith = true;
            Sort = FiscalSort.TransDate;
            SortBackwards = false;
        }
    }
}
