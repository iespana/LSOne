using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        public virtual void UpdateCouponCustomerLink(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier couponID, RecordIdentifier customerID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.UpdateCouponCustomerLink(couponID, customerID, CreateLogonInfo(entry)), closeConnection);
        }
    }
}
