namespace LSOne.DataLayer.DatabaseUtil.Enums
{
    /// <summary>
    /// Should the SQL Express 2005 or SQL Express 2008 be installed
    /// </summary>
    public enum SQLInstall
    {
        /// <summary>
        /// Install SQL Express 2005
        /// </summary>
        SQL2005Expr = 0,
        /// <summary>
        /// Install SQL Express 2008
        /// </summary>
        SQL2008Expr = 1,
        /// <summary>
        /// Install SQL Compact Edition
        /// </summary>
        SQLCompactEdition = 2,
        /// <summary>
        /// Install SQL Express 2017
        /// </summary>
        SQL2017Expr = 3
    }
}
