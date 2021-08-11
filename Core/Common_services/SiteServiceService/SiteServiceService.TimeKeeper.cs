using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Terminals;
using LSOne.DataLayer.BusinessObjects.TimeKeeper;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        public void KeepTime(IConnectionManager entry, SiteServiceProfile siteServiceProfile, TimeKept timeToKeep)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.KeepTime(timeToKeep, CreateLogonInfo(entry)), false);

        }

        public TimeKept GetLastTimeKept(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier userGuid)
        {
            TimeKept result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result =  server.GetLastTimeKept(userGuid, CreateLogonInfo(entry)), false);
            return result;
        }
    }
}
