using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.POS;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.POS
{
    public interface IPOSKeyboardButtonControlData : IDataProvider<POSKeyboardButtonControl>
    {
        void PopulateKeyboardButtonControl(IDataReader dr, POSKeyboardButtonControl posKeyboardButtonControl);
        List<POSKeyboardButtonControl> GetList(IConnectionManager entry);
        List<POSKeyboardButtonControl> GetButtonControlList(IConnectionManager entry, RecordIdentifier buttonControlID);
    }
}