using System;
using System.Reflection;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual void SaveTerminaldData(LogonInfo logonInfo, Terminal terminal)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(terminal)}: {terminal}");
                Providers.TerminalData.Save(entry, terminal);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);
                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);
                Utils.Log(this, Utils.Done);
            }
        }
    }
}
