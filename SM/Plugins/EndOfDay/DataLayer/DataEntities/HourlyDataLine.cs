using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities
{
    public class HourlyDataLine : DataEntity
    {
        public static DecimalLimit PriceLimiter { get; set; }
        
        public string Time { get; set; }
        public int NumberOfTransactions { get; set; }
        public decimal NumberOfItemsSold { get; set; }
        public decimal Amount { get; set; }

        public string FormattedNumberOfItemsSold
        {
            get
            {
                return NumberOfItemsSold.ToString("n0");
            }
        }

        public string FormattedAmount
        {
            get
            {
                if (PriceLimiter != null)
                {
                    return Amount.FormatWithLimits(PriceLimiter);
                }
                else
                {
                    return "";
                }
            }
        }        
    }
}
