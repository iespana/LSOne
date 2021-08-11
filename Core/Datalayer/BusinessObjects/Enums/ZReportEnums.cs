using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    public enum ZReportOtherInfoEnum
    {
        ReceiptCopy,
        Invoice,
        Training,
        Deposits,
        None
    }

    public enum GrandTotalAmtDisplay
    {
        WithTax,
        WithoutTax,
        Both
    }

    public enum SalesReportAmtdisplay
    {
        WithTax,
        WithoutTax
    }

    public enum XZDisplayAmounts
    {
        Combined = 0,
        OnlyReturn = 1,
        OnlySale = 2
    }

    public enum DepositOrderBy
    {
        Account = 0,
        Amount = 1,
        Name = 2
    }
}
