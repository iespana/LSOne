using System;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts
{
    public class DiscountOffer : DataEntity
    {
        private int discountType;

        public enum PeriodicDiscountOfferTypeEnum
        {
            MultiBuy = 0,
            MixMatch = 1,
            Offer = 2,
            Promotion = 3,
            All = 4
        };

        public enum PeriodicDiscountDiscountTypeEnum
        {
            UnitPrice = 0,
            DiscountPercent = 1
        };

        public enum MixAndMatchDiscountTypeEnum
        {
            DealPrice = 0,
            DiscountPercent = 1,
            DiscountAmount = 2,
            LeastExpensive = 3,
            LineSpecific = 4
        }

        public enum AccountCodeEnum
        {
            None = 0,
            Customer = 1,
            CustomerGroup = 2
        }

        public enum TriggeringEnum
        {
            Automatic = 0,
            Manual = 1
        }

        public DiscountOffer()
        {
            AccountCode = AccountCodeEnum.None;
            AccountRelation = RecordIdentifier.Empty;
            ValidationPeriod = "";
            OfferType = PeriodicDiscountOfferTypeEnum.MultiBuy;
            DiscountType = PeriodicDiscountDiscountTypeEnum.DiscountPercent;
            PriceGroup = "";
            PriceGroupName = "";
            CustomerName = "";
            CustomerGroupName = "";
            Triggering = TriggeringEnum.Automatic;
            BarCode = "";

            discountType = 1;
        }

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier MasterID { get; set; }

        public AccountCodeEnum AccountCode { get; set; }
        public RecordIdentifier AccountRelation { get; set; }
        public string CustomerName { get; set; }
        public string CustomerGroupName { get; set; }
        public bool Enabled { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DealPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public int NumberOfLeastExpensiveLines { get; set; }
        public RecordIdentifier ValidationPeriod { get; set; }
        public string ValidationPeriodDescription { get; set; }
        public Date StartingDate {get ; set;}
        public Date EndingDate { get; set; }
        public int Priority { get; set; }
        public PeriodicDiscountOfferTypeEnum OfferType { get; set; }
        public int NumberOfItemsNeeded { get; set; }

        public RecordIdentifier PriceGroup { get; set; }
        public string PriceGroupName { get; set; }

        public TriggeringEnum Triggering { get; set; }
        public string BarCode { get; set; }

        public int DiscountTypeValue
        {
            get { return discountType; }
            set { discountType = value; }
        }

        public PeriodicDiscountDiscountTypeEnum DiscountType
        {
            get 
            {
                if (discountType > 1)
                {
                    throw new IndexOutOfRangeException();
                }

                return (PeriodicDiscountDiscountTypeEnum)discountType; 
            }
            set { discountType = (int)value;}
        }

        public MixAndMatchDiscountTypeEnum MixAndMatchDiscountType
        {
            get { return (MixAndMatchDiscountTypeEnum)discountType; }
            set { discountType = (int)value; }
        }

        public string DiscountTypeText
        {
            get
            {
                return (DiscountType == PeriodicDiscountDiscountTypeEnum.UnitPrice) ? Properties.Resources.UnitPrice : Properties.Resources.DiscountPercent;
            }
        }

        public string MixAndMatchDiscountTypeText
        {
            get
            {
                switch ((MixAndMatchDiscountTypeEnum)discountType)
                {
                    case MixAndMatchDiscountTypeEnum.DealPrice:
                        return Properties.Resources.DealPrice;

                    case MixAndMatchDiscountTypeEnum.DiscountPercent:
                        return Properties.Resources.DiscountPercent;

                    case MixAndMatchDiscountTypeEnum.DiscountAmount:
                        return Properties.Resources.DiscountAmount;

                    case MixAndMatchDiscountTypeEnum.LeastExpensive:
                        return Properties.Resources.LeastExpensive;

                    case MixAndMatchDiscountTypeEnum.LineSpecific:
                        return Properties.Resources.LineSpecific;

                    default:
                        return "";
                }
            }
        }

        public string OfferTypeText
        {
            get
            {
                switch (OfferType)
                {
                    case PeriodicDiscountOfferTypeEnum.MultiBuy:
                        return Properties.Resources.Multibuy;
                        
                    case PeriodicDiscountOfferTypeEnum.MixMatch:
                        return Properties.Resources.MixMatch;
                        
                    case PeriodicDiscountOfferTypeEnum.Offer:
                        return Properties.Resources.DiscountOffer;
                        
                    case PeriodicDiscountOfferTypeEnum.Promotion:
                        return Properties.Resources.PromotionOffer;
                        
                    case PeriodicDiscountOfferTypeEnum.All:
                        return Properties.Resources.All;
                        
                    default:
                        return "";
                }
            }
        }

        public string TriggeringText
        {
            get
            {
                switch (Triggering)
                {
                    case TriggeringEnum.Automatic:
                        return Properties.Resources.Automatic;
                    case TriggeringEnum.Manual:
                        return Properties.Resources.Manual;
                    default:
                        return "";
                }
            }
        }
    }
}
