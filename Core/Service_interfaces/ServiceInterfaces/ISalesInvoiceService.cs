using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISalesInvoiceService : IService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction</param>
        void SalesInvoices(IConnectionManager entry, ref IPosTransaction posTransaction);

        /// <summary>
        /// Returns true if the Site service is needed to conclude a transaction that includes sales invoice item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction</param>
        /// <returns></returns>
        bool SiteServiceIsNeeded(IConnectionManager entry, IPosTransaction transaction);
    }
}
