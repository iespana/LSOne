
namespace LSOne.DataLayer.BusinessObjects.BarCodes
{
    public class BarcodeReceiptParseInfo
    {
        public string StoreID { get; set; }
        public string TerminalID { get; set; }
        public string ReceiptID { get; set; }

        public BarcodeReceiptParseInfo(string storeID, string terminalID, string receiptID)
        {
            StoreID = storeID;
            TerminalID = terminalID;
            ReceiptID = receiptID;
        }

        public BarcodeReceiptParseInfo()
        {
            StoreID = string.Empty;
            TerminalID = string.Empty;
            ReceiptID = string.Empty;
        }
    }
}
