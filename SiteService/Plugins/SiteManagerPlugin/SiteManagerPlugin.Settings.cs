using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.DTO;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual RecordIdentifier GetDefaultTaxStoreID(LogonInfo logonInfo)
        {
            RecordIdentifier result = RecordIdentifier.Empty;
            IConnectionManager entry = GetConnectionManager(logonInfo);

            DoWork(entry, () => result = Providers.StoreData.GetDefaultStoreID(entry), MethodBase.GetCurrentMethod().Name);

            return result;
        }
    }
}
