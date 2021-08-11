using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Reports
{
    public interface IReportData : IDataProviderBase<ReportListItem>
    {
        Report Get(IConnectionManager entry, RecordIdentifier id);
        List<ReportListItem> GetList(IConnectionManager entry);
        void InsertSQLProcedures(IConnectionManager entry, Report report);
        List<ReportListItem> GetReportContextItems(IConnectionManager entry, ReportContextEnum context, CacheType cacheType = CacheType.CacheTypeNone);
        void InvalidateReportContextCache(IConnectionManager entry);
        void Delete(IConnectionManager entry, RecordIdentifier id);
        bool Exists(IConnectionManager entry, RecordIdentifier id);
        void Save(IConnectionManager entry, Report report);
    }
}