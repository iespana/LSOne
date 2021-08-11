using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {

        #region Tax refund

        public virtual void SaveTaxRefund(IConnectionManager entry, SiteServiceProfile siteServiceProfile, TaxRefund refund, bool closeConnection = true)
        {
            try
            {
                if (refund == null)
                {
                    return;
                }

                if (!isClosed)
                {
                    Disconnect(entry);
                }

                if (isClosed)
                {
                    if (siteServiceProfile.UseCentralReturns)
                    {
                        Connect(entry, siteServiceProfile);
                    }
                }

                if (siteServiceProfile.UseCentralReturns)
                {
                    server.SaveTaxRefund(refund, CreateLogonInfo(entry));
                }
                else
                {
                    server.SaveTaxRefund(refund, CreateLogonInfo(entry));
                }

                if (closeConnection)
                {
                    Disconnect(entry);
                }

            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }

        public virtual TaxRefund GetTaxRefund(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier id, bool closeConnection = true)
        {
            TaxRefund result = null;
            try
            {
                if (id == null || id.IsEmpty)
                {
                    return null;
                }
                

                if (!isClosed)
                {
                    Disconnect(entry);
                }

                if (isClosed)
                {
                    if (siteServiceProfile.UseCentralReturns)
                    {
                        Connect(entry, siteServiceProfile);
                    }
                }

                result = siteServiceProfile.UseCentralReturns ?
                        server.GetTaxRefund(id, CreateLogonInfo(entry)) :
                        Providers.TaxRefundData.Get(entry, id);

                if (closeConnection)
                {
                    Disconnect(entry);
                }

            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
            return result;
        }


        #endregion

    }
}
