using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Reports
{
    public interface IReportEnumValueData : IDataProviderBase<ReportEnumValue>
    {
        List<ReportEnumValue> GetEnumValues(IConnectionManager entry, RecordIdentifier reportID, string enumName );
        bool Exists(IConnectionManager entry, RecordIdentifier id);
        bool Exists(IConnectionManager entry, RecordIdentifier reportID, string languageID, string enumName, int enumValue);
        void Delete(IConnectionManager entry, RecordIdentifier reportID, string languageID);
        void Save(IConnectionManager entry, ReportEnumValue value);
    }
}