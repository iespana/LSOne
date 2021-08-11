namespace LSOne.Services.Interfaces.Enums
{
    public enum ExcelStandardFormats
    {
        /// <summary>
        /// "# ##0,00" + CurrencySymbol of CurrentCulture. 
        /// </summary>
        Currency,        
        /// <summary>
        ///  "MM.dd". 
        /// </summary>
        DateMonthDayOne,   
        /// <summary>
        ///  "MMM.dd". 
        /// </summary>
        DateMonthDayTwo,    
        /// <summary>
        /// "MM.dd.yy".
        /// </summary>
        DateFullOne,         
        /// <summary>
        /// "MM.dd.yyyy
        /// </summary>
        DateFullTwo,        
        /// <summary>
        /// "MMM.dd.yyyy". 
        /// </summary>
        DateFullThree,      
        /// <summary>
        /// "MM.DD.YY h:mm". 
        /// </summary>
        DateTime,   
        /// <summary>
        ///  "0,00". 
        /// </summary>
        Numerical,
        /// <summary>
        /// "0%". 
        /// </summary>
        Percentage,
        /// <summary>
        /// Short date format for the current locale
        /// </summary>
        CurrentLocaleShortDate,
        /// <summary>
        /// Text
        /// </summary>
        Textual

    }
}
