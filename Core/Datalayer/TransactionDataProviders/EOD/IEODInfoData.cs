using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Enums;
using LSOne.DataLayer.TransactionObjects.EOD;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders.EOD
{
    public interface IEODInfoData : IDataProviderBase<EODInfo>
    {
        EODInfo GetReportData(IConnectionManager entry, EODInfo reportInfo, IPosTransaction transaction,
            ReportType repType, RecordIdentifier currencyCode);
    }
}