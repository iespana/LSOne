using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities
{
    public class CurrencyDataLine : DataEntity
    {
        public string Currency { get; set; }
        public int SalesNumbers { get; set; }
        public decimal Amount { get; set; }        

        public string FormattedAmount
        {
            get
            {
                return Amount.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
            }
        }
    }
}
