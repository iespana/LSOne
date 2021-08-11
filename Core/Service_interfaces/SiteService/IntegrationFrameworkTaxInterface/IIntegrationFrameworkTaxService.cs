using System.ServiceModel;

namespace LSRetail.SiteService.IntegrationFrameworkTaxInterface
{
    //TODO: SessionMode should be required. Changed to Allowed for http binding
    [ServiceContract(SessionMode = SessionMode.Allowed, Namespace = "LSRetail.IntegrationFramework")]
    public partial interface IIntegrationFrameworkTaxService
    {
        [OperationContract]
        bool Ping();
    }
}
