using System.ServiceModel;
using LSOne.DataLayer.BusinessObjects.TimeKeeper;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {   
        [OperationContract]
        void KeepTime(TimeKept timeToKeep, LogonInfo logonInfo);

        [OperationContract]
        TimeKept GetLastTimeKept(RecordIdentifier userGuid, LogonInfo logonInfo);
    }
}
