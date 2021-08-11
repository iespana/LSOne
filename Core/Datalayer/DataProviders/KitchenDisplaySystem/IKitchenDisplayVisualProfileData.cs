using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.DataLayer.KDSBusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.DataProviders.KitchenDisplaySystem
{
    public interface IKitchenDisplayVisualProfileData : IDataProvider<KitchenDisplayVisualProfile>
    {
        List<KitchenDisplayVisualProfile> GetList(IConnectionManager entry);

        KitchenDisplayVisualProfile Get(IConnectionManager entry, RecordIdentifier ID, RecordIdentifier displayProfileID);

        List<KitchenDisplayVisualProfile> GetList(IConnectionManager entry, RecordIdentifier headerPaneID);
    }
}
