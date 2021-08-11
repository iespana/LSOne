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
    public interface IKitchenDisplayTimeStyleData : IDataProvider<KitchenDisplayTimeStyle>
    {
        List<KitchenDisplayTimeStyle> GetList(IConnectionManager entry);
        List<KitchenDisplayTimeStyle> GetList(IConnectionManager entry, RecordIdentifier kdStyleProfileId);
        void Save(IConnectionManager entry, KitchenDisplayTimeStyle newKitchenDisplayTimeStyle, RecordIdentifier oldKitchenDisplayTimeStyleId);
        KitchenDisplayTimeStyle Get(IConnectionManager entry, RecordIdentifier id);        
    }
}
