using System.Collections.Generic;
using System.Linq;

namespace LSOne.ViewPlugins.SaleByPaymentMediaReport.Datalayer.DataEntities
{
    public class TerminalPaymentMethods
    {
        public string TerminalId { get; set; }
        public string TerminalName { get; set; }
        public List<PaymentMethodAmount> PaymentTypes { get; set; }
        public decimal TotalPayments
        {
            get { return PaymentTypes.Sum(x => x.Amount); }
        }
    }
}
