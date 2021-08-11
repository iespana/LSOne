using System.ServiceModel;

namespace LSRetail.SiteService.IntegrationFrameworkRetailItemInterface
{
    //TODO: SessionMode should be required. Changed to Allowed for http binding
    [ServiceContract(SessionMode = SessionMode.Allowed, Namespace = "LSRetail.IntegrationFramework")]
    public partial interface IIntegrationFrameworkRetailItemService
    {
        [OperationContract]
        bool Ping();
    }
}
