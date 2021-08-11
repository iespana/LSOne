using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Reports
{
    public interface IReportContextData : IDataProviderBase<ReportContext>
    {
        List<ReportContext> GetList(IConnectionManager entry, RecordIdentifier reportGUID);
        void DeleteAllForReport(IConnectionManager entry, RecordIdentifier reportID,string languageID);
        bool Exists(IConnectionManager entry, RecordIdentifier id);
        void Save(IConnectionManager entry, ReportContext context);
    }
}