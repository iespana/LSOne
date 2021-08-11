namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// Time unit enum used f.ex. in Loyalty schemes
    /// </summary>
    public enum TimeUnitEnum
    {
        /// <summary>
        /// No time unit set
        /// </summary>
        None = -1,
        /// <summary>
        /// add days to date 
        /// </summary>
        Day = 0,
        /// <summary>
        /// add weeks to date
        /// </summary>
        Week = 1,
        /// <summary>
        /// add months to date
        /// </summary>
        Month = 2,
        /// <summary>
        /// add years to date
        /// </summary>
        Year = 3
    }
}
