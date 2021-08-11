using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.DTO;
using LSOne.DataLayer.BusinessObjects.SalesOrder;
using System.Data;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {
        [OperationContract]
        void GetSalesInvoice(ref bool retVal, ref string comment, ref string invoiceId, ref decimal totalPaidAmount, ref decimal totalInvoiceAmount, ref string customerAccount, ref string customerName, ref DateTime creationDate, LogonInfo logonInfo);
        [OperationContract]
        void GetSalesInvoiceList(ref bool retVal, ref string comment, ref DataTable salesInvoices, string customerAccount, LogonInfo logonInfo);

        [OperationContract]
        void PaySalesInvoice(ref bool retVal, ref string comment, string invoiceId, decimal amount, string posID, string storeId, string transactionId, LogonInfo logonInfo);
    }
}
