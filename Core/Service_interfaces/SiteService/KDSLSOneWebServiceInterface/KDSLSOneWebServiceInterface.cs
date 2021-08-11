using LSRetail.SiteService.SiteServiceInterface;
using System.ServiceModel;


namespace LSRetail.SiteService.KDSLSOneWebServiceInterface
{
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IKDSLSOneWebService : ISiteServicePlugin, Dyn365BCWebServiceForKDS_Port
    {
        //[OperationContract]
        //bool Ping();
    }
}