namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// Shows when a terminal allows a statement to be posted
    /// </summary>
    public enum AllowTerminalStatementPostingEnum
    {
        /// <summary>
        /// This terminal always allows statements to be posted
        /// </summary>
        Always,
        /// <summary>
        /// This terminal only allows statement posting if it's last transaction is an EOD transaction
        /// </summary>
        IfLastTransactionIsEod
    }
}
