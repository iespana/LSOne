namespace LSOne.DataLayer.GenericConnector.Enums
{
    public enum ConnectionType
    {
        SharedMemory,
        TCP_IP,
        NamedPipes
    }

    public enum ConnectionUsageType
    {
        UsageNormalClient,
        UsageService
    }
    
    public enum ConnectionAuthenticationType
    {
        WindowsAuthentication,
        SQLServerAuthentication
    }
}
