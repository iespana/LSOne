using System.Collections.Generic;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface IForecourtSoundData : IDataProviderBase<ForecourtSound>
    {
        ForecourtSound Get(IConnectionManager entry, RecordIdentifier id);
        List<ForecourtSound> GetList(IConnectionManager entry);
        void Delete(IConnectionManager entry, ForecourtSound sound);
        bool RecordExists(IConnectionManager entry, ForecourtSound sound);
        void Save(IConnectionManager entry, ForecourtSound sound);
    }
}