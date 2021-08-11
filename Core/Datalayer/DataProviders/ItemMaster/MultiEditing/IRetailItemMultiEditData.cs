using LSOne.DataLayer.BusinessObjects.ItemMaster.MultiEditing;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.DataProviders.ItemMaster.MultiEditing
{
    public interface IRetailItemMultiEditData  : IDataProvider<RetailItemMultiEdit>
    {
        void Save(IConnectionManager entry, RecordIdentifier masterID);

        void PrepareStatement(IConnectionManager entry, RetailItemMultiEdit item);

        void UpdatePriceFields(RetailItemMultiEdit item);

    }
}
