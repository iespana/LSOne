namespace LSOne.Services.Interfaces.Enums
{
    /// <summary>
    /// Rounding method for Tax and Currency calculations
    /// </summary>
    public enum RoundMethod
    {
        /// <summary>
        /// 0
        /// </summary>
        RoundNearest = 0,
        /// <summary>
        /// 1
        /// </summary>
        RoundDown = 1,
        /// <summary>
        /// 2
        /// </summary>
        RoundUp = 2
    }

    /// <summary>
    /// Rounding method for Tender calculations
    /// </summary>
    public enum TenderRoundMethod
    {
        /// <summary>
        /// 0
        /// </summary>
        None = 0,
        /// <summary>
        /// 1
        /// </summary>
        RoundNearest = 1,
        /// <summary>
        /// 2
        /// </summary>
        RoundUp = 2,
        /// <summary>
        /// 3
        /// </summary>
        RoundDown = 3

    }
    /// <summary>
    /// Rounding method for Unit Conversions
    /// </summary>
    public enum UnitRoundOff
    {
        /// <summary>
        /// 0
        /// </summary>
        RoundOff = 0,
        /// <summary>
        /// 1
        /// </summary>
        Up = 1,
        /// <summary>
        /// 2
        /// </summary>
        Decrease = 2
    }
}
