namespace LSOne.DataLayer.DatabaseUtil.Enums
{   

    /// <summary>
    /// The different types of versions available in PosisInfo
    /// </summary>
    public enum VersionType
    {
        /// <summary>
        /// Get database version
        /// </summary>
        Database = 0,
        /// <summary>
        /// Get demo data version
        /// </summary>
        DemoData = 1,
        /// <summary>
        /// Get LS POS version
        /// </summary>
        LSPOS = 2,
        /// <summary>
        /// Get Site Manager version
        /// </summary>
        SC = 3
    }
}
