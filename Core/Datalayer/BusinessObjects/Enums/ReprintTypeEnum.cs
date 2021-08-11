using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    public enum ReprintTypeEnum
    {
        ReceiptCopy,
        Invoice,
        Email
    }

    public enum ReprintReceiptEnum
    {
        GiftReceipt = 1,
        CopyLastReceipt = 2,
        TaxFreeReceipt = 3,
        CustomReceipt = 4
    }
}
