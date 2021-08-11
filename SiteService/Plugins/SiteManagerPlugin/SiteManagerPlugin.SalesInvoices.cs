using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.SalesOrder;
using LSOne.Services.Interfaces.SupportClasses;
using LSRetail.SiteService.SiteServiceInterface.DTO;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual void GetSalesInvoice(ref bool retVal, ref string comment, ref string invoiceId, ref decimal totalPaidAmount, ref decimal totalInvoiceAmount, ref string customerAccount, ref string customerName, ref DateTime creationDate, LogonInfo logonInfo)
        {
            throw new NotImplementedException();
        }

        public virtual void GetSalesInvoiceList(ref bool retVal, ref string comment, ref DataTable salesInvoices, string customerAccount, LogonInfo logonInfo)
        {
            throw new NotImplementedException();
        }
    }
}
