namespace LSOne.DataLayer.BusinessObjects.Currencies
{
    /// <summary>
    /// Enum to distinguish between banknotes and coins.
    /// </summary>
    public class CurrencyEnums
    {
        
            /// <summary>
            /// Enum to distinguish between banknotes and coins.
            /// </summary>
            public enum CurrType
            {
                /// <summary>
                /// 0, Hard money (coins).
                /// </summary>
                Coin = 0,

                /// <summary>
                /// 1, Paper money.
                /// </summary>
                Paperbill = 1,

                /// <summary>
                /// 2, Foreign Currency
                /// </summary>
                ForeignCurrency = 2

            }
        
    }
}
