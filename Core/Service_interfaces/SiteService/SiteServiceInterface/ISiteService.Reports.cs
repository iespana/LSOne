using System.ServiceModel;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.DTO;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {
        [OperationContract]
        bool ReportExists(RecordIdentifier reportID, LogonInfo logonInfo);

        [OperationContract]
        ReportResult ReportRun(ReportManifest manifest, LogonInfo logonInfo);

        [OperationContract]
        ReportResult ReportRunSubReport(ReportManifest manifest, LogonInfo logonInfo);

    }
}
