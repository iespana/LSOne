namespace LSOne.DataLayer.DatabaseUtil.Enums
{
    /// <summary>
    /// The result of setting the Database connection
    /// </summary>
    public enum UtilityResult
    {
        /// <summary>
        /// The SQL connection has not been validated
        /// </summary>
        NotValidated = 0,
        /// <summary>
        /// The SQL Server in the connection can not be found
        /// </summary>
        SQLServerNotFound = 1,
        /// <summary>
        /// The Database in the connection can not be found
        /// </summary>
        DatabaseNotFound = 2,
        /// <summary>
        /// Both SQL Server and Database in the connection exist and connection has been made
        /// </summary>
        Validated = 3,
        /// <summary>
        /// If not using Windows Authentication this result may happen. Then either the user name or password are not correct
        /// </summary>
        LogonInformationNotValid = 4
    }   
    
}
