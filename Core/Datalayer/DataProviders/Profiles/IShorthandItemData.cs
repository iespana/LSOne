using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Profiles
{
    public interface IShorthandItemData : IDataProvider<ShorthandItem>
    {
        bool Exists(IConnectionManager entry, RecordIdentifier ID, string shortHand);

        List<ShorthandItem> GetList(IConnectionManager entry, RecordIdentifier profileID);
    }
}
