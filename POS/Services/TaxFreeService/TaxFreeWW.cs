using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
 #if TAXFREEWW
    public partial class TaxFreeService : ITaxFreeService
    {
        #region IService members

        public IErrorLog ErrorLog { set; private get; }

        public void Init(IConnectionManager entry)
        {
            var settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            DLLEntry.Init(entry, settings);
        }

        #endregion

        #region ITaxFree Members

        public bool CancelTaxRefund(DataLayer.GenericConnector.Interfaces.IConnectionManager entry, Utilities.DataTypes.RecordIdentifier refundID)
        {
            throw new NotImplementedException();
        }

        public void CaptureSale(IConnectionManager entry, IPosTransaction transaction)
        {
            if (!(transaction is IRetailTransaction))
            {
                
            }
            TaxRefund refund = new TaxRefund();
            refund.Created = DateTime.Now;
        }

        public bool ShowInJournal
        {
            get { return true; }
        }

        public void TaxRefundMulti(Interfaces.SupportInterfaces.IPosTransaction transaction)
        {
            throw new NotImplementedException();
        }

        //public Utilities.ErrorHandling.IErrorLog ErrorLog
        //{
        //    set { throw new NotImplementedException(); }
        //}

        #endregion
    }

#endif
}
