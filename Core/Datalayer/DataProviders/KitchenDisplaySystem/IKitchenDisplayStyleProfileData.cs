using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.DataProviders.KitchenDisplaySystem
{
    public interface IKitchenDisplayStyleProfileData : IDataProvider<LSOneKitchenDisplayStyleProfile>
    {
        LSOneKitchenDisplayStyleProfile Get(IConnectionManager entry, RecordIdentifier profileId,
            bool includeDetails = false);

        List<LSOneKitchenDisplayStyleProfile> GetList(IConnectionManager entry);
        List<KdsButtonStyleProfile> GetButtonStyleList(IConnectionManager entry);
    }
}
