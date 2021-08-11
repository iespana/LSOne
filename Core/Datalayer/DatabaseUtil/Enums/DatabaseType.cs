namespace LSOne.DataLayer.DatabaseUtil.Enums
{
    /// <summary>
    /// What type of database should be updated and/or installed
    /// </summary>
    public enum DatabaseType : int
    {        
        /// <summary>
        /// Update should apply to the database that either the Site Manager or LS POS use as their main database
        /// </summary>
        Normal = 0x2,
        /// <summary>
        /// Update should apply to the audit database used by the Site Manager
        /// </summary>
        Audit = 0x4,
        /// <summary>
        /// Update should apply to both Normal and Audit databases
        /// </summary>
        All = 0x6
    }
}
