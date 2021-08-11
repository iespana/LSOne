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
    public interface IKitchenDisplayFunctionalProfileData : IDataProvider<KitchenDisplayFunctionalProfile>
    {
        List<KitchenDisplayFunctionalProfile> GetList(IConnectionManager entry);

        KitchenDisplayFunctionalProfile Get(IConnectionManager entry, RecordIdentifier ID);
    }
}
