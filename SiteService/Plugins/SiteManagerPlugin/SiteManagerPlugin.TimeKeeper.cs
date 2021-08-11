using System;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.TimeKeeper;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual void KeepTime(TimeKept timeToKeep, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                Providers.TimeKeeperData.Save(dataModel, timeToKeep);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual TimeKept GetLastTimeKept(RecordIdentifier userGuid, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                return Providers.TimeKeeperData.GetLastTimeKept(dataModel, userGuid);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }
    }
}