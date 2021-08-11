using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Infocodes
{
    public class InfocodeSubcode : DataEntity
    {  
        public InfocodeSubcode()
            : base()
        {
            TriggerFunction = TriggerFunctions.None;
            TriggerCode = "";
            QtyLinkedToTriggerLine = true;
            PriceHandling = PriceHandlings.AlwaysCharge;
            UnitOfMeasure = "";
            QtyPerUnitOfMeasure = 1;
            InfocodePrompt = "";
            MaxSelection = 1;
            SerialLotNeeded = false;
            VariantCode = "";
            SubcodeId = RecordIdentifier.Empty;
            ItemName = "";
            VariantDescription = "";
        }

        private int triggerFunction;
        private int priceHandling;
        private int usageCategory;
        private RecordIdentifier infocodeId;
        private RecordIdentifier subcodeId;
        
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(infocodeId, subcodeId);
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public RecordIdentifier InfocodeId 
        {
            get { return infocodeId; }
            set { infocodeId = value; }
        }
        public RecordIdentifier SubcodeId 
        {
            get { return subcodeId; }
            set { subcodeId = value; }
        }
        public RecordIdentifier TriggerCode { get; set; }
        public PriceTypes PriceType { get; set; }
        public decimal AmountPercent { get; set; }
        public RecordIdentifier VariantCode { get; set; }
        public bool VariantNeeded { get; set; }
        public bool QtyLinkedToTriggerLine { get; set; }
        public RecordIdentifier UnitOfMeasure { get; set; }
        public decimal QtyPerUnitOfMeasure { get; set; }
        public string InfocodePrompt { get; set; }
        public int MaxSelection { get; set; }
        public bool SerialLotNeeded { get; set; }
        //public string Text { get; set; }
        public string ItemName { get; set; }
        public string VariantDescription { get; set; }

        public TriggerFunctions TriggerFunction
        {
            get
            {
                return (TriggerFunctions)triggerFunction;
            }
            set { triggerFunction = (int)value; }
        }

        public PriceHandlings PriceHandling 
        { 
            get
            {
                if (priceHandling > 2)
                {
                    throw new IndexOutOfRangeException();
                }
                return (PriceHandlings)priceHandling;
            }
            set { priceHandling = (int)value; }
        }

        public UsageCategoriesEnum UsageCategory
        {
            get
            {
                if (usageCategory > 2)
                {
                    throw new IndexOutOfRangeException();
                }
                return (UsageCategoriesEnum)usageCategory;
            }
            set { usageCategory = (int)value; }
        }
    }
}
