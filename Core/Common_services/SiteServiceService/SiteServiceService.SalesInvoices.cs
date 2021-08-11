using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Invoice;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.SalesOrder;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        public virtual void GetSalesInvoice(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref bool retVal, ref string comment, ref string invoiceId, ref decimal totalPaidAmount, ref decimal totalInvoiceAmount,
            ref string customerAccount, ref string customerName, ref DateTime creationDate)
        {

        }
        public virtual List<Invoice> GetSalesInvoiceList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, string customerAccount)
        {
            return new List<Invoice>();
        }

        public virtual void PaySalesInvoice(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref bool retVal, ref string comment, string invoiceId, decimal amount, string transactionId)
        {

        }
    }
}
