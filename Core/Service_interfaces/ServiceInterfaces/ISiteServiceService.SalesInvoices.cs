using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Invoice;

namespace LSOne.Services.Interfaces
{
    public partial interface ISiteServiceService
    {
        void GetSalesInvoice(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref bool retVal, ref string comment, ref string invoiceId, ref decimal totalPaidAmount, ref decimal totalInvoiceAmount,
                             ref string customerAccount, ref string customerName, ref DateTime creationDate);

        List<Invoice> GetSalesInvoiceList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, string customerAccount);

        void PaySalesInvoice(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref bool retVal, ref string comment, string invoiceId, decimal amount, string transactionId);
    }
}
