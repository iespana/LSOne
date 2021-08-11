namespace LSOne.DataLayer.DatabaseUtil.Enums
{
    /// <summary>
    /// Is the functionality supposed to work for SQL Server or SQL CE 
    /// </summary>
    public enum SQLServerType
    {
        /// <summary>
        /// Either SQL Server 2005 Express or SQL Server 2008 Express
        /// </summary>
        SQLServer = 0,
        /// <summary>
        /// SQL Compact Edition
        /// </summary>
        SQLCompactEdition = 1
    }
}
