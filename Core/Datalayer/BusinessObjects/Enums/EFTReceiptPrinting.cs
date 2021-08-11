using System;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    [Flags]
    public enum EFTReceiptPrinting
    {
        None = 0x0,
        CardHoldersReceipt = 0x2,
        StoreReceipt = 0x4,

        All = 0x1000
    }
}
