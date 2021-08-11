using System.Drawing;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

#if !MONO
#endif

namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts
{
    public class DiscountOfferLine : DataEntity
    {
        public enum DiscountOfferTypeEnum
        {
            Item = 0,
            RetailGroup = 1,
            RetailDepartment = 2,
            All = 3,
            BarCodeBasedVariant = 4, // Not supported in Site Manager
            SpecialGroup = 5,
            Variant = 10,
        }

        public enum MixAndMatchDiscountTypeEnum
        {
            DealPrice = 0,
            DiscountPercent = 1
        }

        RecordIdentifier offerID;
        DiscountOfferTypeEnum type;
        RecordIdentifier lineNum;

        public DiscountOfferLine()
        {
            offerID = "";
            type = DiscountOfferTypeEnum.All;
            ItemRelation = "";
            Unit = "";
            StandardPrice = 0.0M;
            DiscountPercent = 0.0M;
            DiscountType = MixAndMatchDiscountTypeEnum.DealPrice;
            LineGroup = "";
            lineNum = 0;
        }

        public override RecordIdentifier ID { get; set; }

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier OfferMasterID { get; set; }
        public RecordIdentifier OfferID
        {
            get { return offerID; }
            set { offerID = value; }
        }
        
        public RecordIdentifier LineID
        {
            get { return lineNum; }
            set { lineNum = value; }
        }

        public DiscountOfferTypeEnum Type
        {
            get { return type; }
            set {type = value;}
        }

        public string TypeText
        {
            get
            {
                switch (type)
                {
                    case DiscountOfferTypeEnum.Item:
                        return Properties.Resources.Item;

                    case DiscountOfferTypeEnum.RetailGroup:
                        return Properties.Resources.RetailGroup;

                    case DiscountOfferTypeEnum.RetailDepartment:
                        return Properties.Resources.RetailDepartment;

                    case DiscountOfferTypeEnum.All:
                        return Properties.Resources.All;

                    case DiscountOfferTypeEnum.SpecialGroup:
                        return Properties.Resources.SpecialGroup;
                        
                    default:
                        return "";
                }
            }
        }

        public RecordIdentifier ItemRelation { get; set; }

        public RecordIdentifier Unit { get; set; }

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier TargetMasterID { get; set; }

        public decimal StandardPrice { get; set; }

        public decimal DiscountPercent { get; set; }

        public MixAndMatchDiscountTypeEnum DiscountType { get; set; }

        public string DiscountTypeText
        {
            get
            {
                return (DiscountType == MixAndMatchDiscountTypeEnum.DealPrice) ? Properties.Resources.DealPrice : Properties.Resources.DiscountPercent;
            }
        }

        public RecordIdentifier LineGroup { get; set; }

#if !MONO
        public Color LineColor { get; set; }
#endif
        public string MMGDescription { get; set; }

        public RecordIdentifier TaxItemGroupID { get; set; }
        
        public string VariantName { get; set; }
    }
}
