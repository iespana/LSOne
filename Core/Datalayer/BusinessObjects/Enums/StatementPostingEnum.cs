namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// Tells if and when a terminal is included in statement posting
    /// </summary>
    public enum StatementPostingEnum
    {
        /// <summary>
        /// The statement can be posted for this terminal at any time
        /// </summary>
        Always = 0,
        /// <summary>
        /// The terminal will not be included in statement posting
        /// </summary>
        Never = 1,
        /// <summary>
        /// The terminal will not be included in statements until an EOD transaction has been retrieved from the POS
        /// </summary>
        AfterEODOnTerminal = 2
    }
}
