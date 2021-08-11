namespace LSOne.Services.Interfaces.Constants
{
    public static class KDSLSOneWebServiceConstants
    {
        /// <summary>
        /// The wcf endpoint name
        /// </summary>
        public const string EndpointName = "Server/WS/Company/Codeunit/Dyn365BCWebServiceForKDS";
        
        /// <summary>
        /// KDS Web service setting name for http/https port.
        /// </summary>
        public const string KDSHttpPort = "HttpPort";

        /// <summary>
        /// KDS web service setting name for the ssl certificate thumbnail required by https binding.
        /// </summary>
        public const string KDSCertificateThumbnail = "SslCertificateThumbnail";
        
        /// <summary>
        /// KDS web service setting name for the ssl certificate store location (e.g. LocalMachine)
        /// </summary>
        public const string KDSCertificateStoreLocation = "SslStoreLocation";
        
        /// <summary>
        /// KDS web service setting name for the ssl certificate store name (e.g. TrustedPeople)
        /// </summary>
        public const string KDSCertificateStoreName = "SslStoreName";
    }
}