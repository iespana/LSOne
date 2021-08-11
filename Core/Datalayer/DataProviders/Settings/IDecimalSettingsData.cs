using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Settings
{
    public interface IDecimalSettingsData : IDataProviderBase<DecimalSetting>
    {
        List<DecimalSetting> Get(IConnectionManager entry, int sortColumn, bool backwardsSort);
        DecimalSetting Get(IConnectionManager entry, RecordIdentifier id);
        void Save(IConnectionManager entry, DecimalSetting decimalSetting);
    }
}