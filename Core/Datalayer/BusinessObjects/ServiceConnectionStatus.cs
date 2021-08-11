namespace LSOne.DataLayer.BusinessObjects
{
    /// <summary>
    /// Represents the success state of NetTCP and HTTP protocols
    /// </summary>
    public class ServiceConnectionStatus
    {
        public bool NetTcpConnectionSuccesfull { get; set; }
        public bool HttpConnectionSuccesfull { get; set; }
    }
}
