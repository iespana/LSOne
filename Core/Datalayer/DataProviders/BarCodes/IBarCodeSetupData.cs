using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.BarCodes
{
    public interface IBarCodeSetupData : IDataProvider<BarCodeSetup>, ISequenceable
    {
        List<DataEntity> GetList(IConnectionManager entry);
        List<BarCodeSetup> GetBarcodes(IConnectionManager entry);
        bool BarCodeSetupInUse(IConnectionManager entry, RecordIdentifier barCodeSetupID);
        BarCodeSetup Get(IConnectionManager entry, RecordIdentifier barCodeSetupID);
    }
}