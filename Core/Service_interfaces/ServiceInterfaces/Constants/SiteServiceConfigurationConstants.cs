namespace LSOne.Services.Interfaces.Constants
{
	/// <summary>
	/// Site Service settings names (defined as string constants).
	/// </summary>
	public static class SiteServiceConfigurationConstants
	{
		public const string DatabaseServer = "DatabaseServer";
		public const string DatabaseWindowsAuthentication = "DatabaseWindowsAuthentication";
		public const string DatabaseUser = "DatabaseUser";
		public const string DatabasePassword = "DatabasePassword";
		public const string DatabaseName = "DatabaseName";
		public const string DatabaseConnectionType = "DatabaseConnectionType";
		public const string SiteManagerUser = "SiteManagerUser";
		public const string SiteManagerPassword = "SiteManagerPassword";
		public const string DataAreaID = "DataAreaID";
		public const string Port = "Port";
		public const string EMailSendInterval = "EMailSendInterval";
		public const string EMailMaximumBatch = "EMailMaximumBatch";
		public const string EMailMaximumAttempts = "EMailMaximumAttempts";
		public const string EMailTruncateQueue = "EMailTruncateQueue";
		public const string PrivateHashKey = "PrivateHashKey";
		public const string MaxCount = "MaxCount";
		public const string MaxUserConnectionCount = "MaxUserConnectionCount";
		public const string ExternalAddress = "ExternalAddress";
		public const string IsCloud = "IsCloud";

		/// <summary>
		/// Send timeout setting name for <see cref="System.ServiceModel.NetTcpBinding"/>
		/// </summary>
		public const string NetTcpTimeout = "NetTcpTimeout";

		/// <summary>
		/// Administrative password setting name.
		/// </summary>
		public const string Administration = "Administration";
		
		/// <summary>
		/// Administrative session timeout setting name.
		/// </summary>
		public const string AdministrationTimeout = "AdministrationTimeout";

		/// <summary>
		/// Integration framework setting name for enabling service discovery (mex binding).
		/// </summary>
		public const string IFEnableMex = "EnableServiceDiscovery";

		/// <summary>
		/// Integration framework setting name for enabling web http/https endpoint (default is net/tcp).
		/// </summary>
		public const string IFEnableHttpEndpoint = "EnableHttpEndpoint";

		/// <summary>
		/// Integration framework setting name for http/https port.
		/// </summary>
		public const string IFHttpPort = "HttpPort";

		/// <summary>
		/// Integration framework setting name for enabling net/tcp endpoint.
		/// </summary>
		public const string IFEnableNetTcpEndpoint = "EnableNetTcpEndpoint";

		/// <summary>
		/// Integration framework setting name for net/tcp port.
		/// </summary>
		public const string IFNetTcpPort = "NetTcpPort";

		/// <summary>
		/// Integration framework setting name for enforcing all communication to be done on https.
		/// </summary>
		public const string IFEnforceHttps = "EnforceHttps";
		/// <summary>
		/// Integration framework setting name for the ssl certificate thumbnail required by https binding.
		/// </summary>
		public const string IFCertificateThumbnail = "SslCertificateThumbnail";
		/// <summary>
		/// Integration framework setting name for the ssl certificate store location (e.g. LocalMachine)
		/// </summary>
		public const string IFCertificateStoreLocation = "SslStoreLocation";
		/// <summary>
		/// Integration framework setting name for the ssl certificate store name (e.g. TrustedPeople)
		/// </summary>
		public const string IFCertificateStoreName = "SslStoreName";

		/// <summary>
		/// Setting that controls which site service plugin will start. Only one instance of site service can be active at a time.
		/// </summary>
		public const string SiteServicePluginOverride = "SiteServicePluginOverride";

		/// <summary>
		/// How many days should site service logs be kept. Any log files older that this setting will be deleted at regular intervals.
		/// </summary>
		public const string DaysToKeepLogs = "DaysToKeepLogs";
	}
}
