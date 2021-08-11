using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.Services.Interfaces.Enums
{
    public enum AddCustomerResultEnum
    {
        Success,
        SuccessButInvoiceBlockedChargingNotAllowed,
        CustomerIDNotValid,
        CustomerBlockedNoTransactionAllowed,
        InvoicedCustomerBlockedSalesNotAllowed
    };
}
