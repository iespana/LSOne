using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces.SupportClasses.Price
{
    public class TradeAgreementPriceInfo
    {
        /// <summary>
        /// Type of the trade agreement. BasePrice = 0, SalesPrice = 1, Promotion = 2
        /// </summary>
        public TradeAgreementPriceType PriceType { get; set; }
        /// <summary>
        /// Price of the trade agreement
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// ID generated from number sequence to be sent to ERP systems
        /// </summary>
        public RecordIdentifier PriceID { get; set; }
        /// <summary>
        /// The promotion discount percentage
        /// </summary>
        public decimal? DiscountPercentage { get; set; }

        public bool IsEmpty => Price == null && PriceID == "";

        public TradeAgreementPriceInfo()
        {
            PriceType = TradeAgreementPriceType.BasePrice;
            Price = null;
            PriceID = "";
            DiscountPercentage = null;
        }

        public TradeAgreementPriceInfo Clone()
        {
            TradeAgreementPriceInfo newtradeAgreementPriceInfo = new TradeAgreementPriceInfo();
            Populate(newtradeAgreementPriceInfo, this);
            return newtradeAgreementPriceInfo;
        }

        private void Populate(TradeAgreementPriceInfo newtradeAgreementPriceInfo, TradeAgreementPriceInfo tradeAgreementPriceInfo)
        {
            newtradeAgreementPriceInfo.PriceID = tradeAgreementPriceInfo.PriceID;
            newtradeAgreementPriceInfo.Price = tradeAgreementPriceInfo.Price;
            newtradeAgreementPriceInfo.PriceType = tradeAgreementPriceInfo.PriceType;
            newtradeAgreementPriceInfo.DiscountPercentage = tradeAgreementPriceInfo.DiscountPercentage;
        }        
    }
}
