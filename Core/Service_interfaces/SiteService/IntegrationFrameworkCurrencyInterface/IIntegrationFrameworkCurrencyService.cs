using System.ServiceModel;

namespace LSRetail.SiteService.IntegrationFrameworkCurrencyInterface
{
    //TODO: SessionMode should be required. Changed to Allowed for http binding
    [ServiceContract(SessionMode = SessionMode.Allowed, Namespace = "LSRetail.IntegrationFramework")]
    public partial interface IIntegrationFrameworkCurrencyService
    {
        [OperationContract]
        bool Ping();
    }
}
