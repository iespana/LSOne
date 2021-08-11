using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Ledger
{
    public class CustomerLedgerFilter
    {
        public CustomerLedgerFilter()
        {
            StoreID = null;
            TerminalID = null;
            FromDate = null;
            ToDate = null;
            Types = 0;
            Receipt = string.Empty;
            RowFrom = 0;
            RowTo = 100;
            SortBackwards = true;
        }
        
        public RecordIdentifier StoreID { get; set; }
        public RecordIdentifier TerminalID { get; set; }
        public Date FromDate { get; set; }
        public Date ToDate { get; set; }
        public byte Types { get; set; }
        public string Receipt { get; set; }
        public int RowFrom { get; set; }
        public int RowTo { get; set; }
        public bool SortBackwards { get; set; }
    }
}
