using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.BusinessObjects.Terminals;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        public bool ReportExists(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier reportID, bool closeConnection)
        {
            bool result = false;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.ReportExists(reportID, CreateLogonInfo(entry)), closeConnection);

            return result;
        }


        public ReportResult ReportRun(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ReportManifest manifest, bool closeConnection)
        {
            ReportResult result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.ReportRun(manifest, CreateLogonInfo(entry)), closeConnection);
            
            return result;
        }
    
    }
}
