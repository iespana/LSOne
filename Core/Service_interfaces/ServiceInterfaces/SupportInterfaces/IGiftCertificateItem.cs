using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface IGiftCertificateItem : ISaleLineItem
    {
        string SerialNumber
        {
            get;
            set;
        }

        decimal Amount
        {
            get;
            set;
        }

        string StoreId
        {
            get;
            set;
        }

        string TerminalId
        {
            get;
            set;
        }

        string StaffId
        {
            get;
            set;
        }

        string TransactionId
        {
            get;
            set;
        }

        string ReceiptId
        {
            get;
            set;
        }

        DateTime Date
        {
            get;
            set;
        }
    }
}
