using LSOne.Services.Interfaces.Constants;
using LSOne.SiteService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


namespace LSOne.SiteService.Plugins.KDSLSOneWebServicePlugin
{
    internal class KDSWcfServiceBuilder
    {
		public static ServiceHostBase CreateKDSHost(object singletonInstance,
																Dictionary<string, string> configurations,
																string endpointName, Type serviceType,
																string hostName)
		{
			int httpPort = int.Parse(configurations[KDSLSOneWebServiceConstants.KDSHttpPort]);
			bool enableServiceDiscovery = true;

			Uri httpAddress = new UriBuilder(Uri.UriSchemeHttp, hostName, httpPort, endpointName).Uri;
			ServiceHost wcfHost = new ServiceHost(singletonInstance, httpAddress);

			WcfServiceBuilderBase.AddDebugBehaviour(wcfHost);

			WcfServiceBuilderBase.AddHttpEndpoint(wcfHost, serviceType.FullName, false,
							configurations[KDSLSOneWebServiceConstants.KDSCertificateStoreLocation],
							configurations[KDSLSOneWebServiceConstants.KDSCertificateStoreName],
							configurations[KDSLSOneWebServiceConstants.KDSCertificateThumbnail],
							true);

			if (enableServiceDiscovery)
			{
				WcfServiceBuilderBase.AddMexEndpointAndBehaviour(wcfHost, false);
			}
			
			return wcfHost;
		}
	}
}