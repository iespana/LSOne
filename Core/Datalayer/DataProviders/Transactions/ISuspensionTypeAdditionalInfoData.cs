using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Transactions
{
    public interface ISuspensionTypeAdditionalInfoData : IDataProvider<SuspensionTypeAdditionalInfo>
    {
        List<SuspensionTypeAdditionalInfo> GetList(IConnectionManager entry, RecordIdentifier suspensionTypeID);
        SuspensionTypeAdditionalInfo Get(IConnectionManager entry, RecordIdentifier id);
    }
}