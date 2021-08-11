using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Infocodes
{
    public class Infocode : DataEntity
    {
        public Infocode() : base()
        {
            Prompt = "";
            OncePerTransaction = false;
            ValueIsAmountOrQuantity = false;
            PrintPromptOnReceipt = false;
            PrintInputOnReceipt = false;
            PrintInputNameOnReceipt = false;
            InputType = InputTypesEnum.SubCodeList;
            MinimumValue = 0.0m;
            MaximumValue = 0.0m;
            MinimumLength = 0;
            MaximumLength = 0;
            InputRequired = false;
            RandomFactor = 100;
            RandomCounter = 0;
            AdditionalCheck = false;
            LinkItemLinesToTriggerLine = false;
            LinkedInfocodeId = "";
            MultipleSelection = false;
            MinSelection = 0;
            MaxSelection = 0;
            CreateInfocodeTransEntries = true;
            ExplanatoryHeaderText = "";
        }

        private int inputType;
        private int triggering;
        private UsageCategoriesEnum usageCategory;

        //public string InfocodeId { get; set; }
        //public string Description { get; set; }
        public string Prompt { get; set; }
        public bool OncePerTransaction { get; set; }
        public bool ValueIsAmountOrQuantity { get; set; }
        public bool PrintPromptOnReceipt { get; set; }
        public bool PrintInputOnReceipt { get; set; }
        public bool PrintInputNameOnReceipt { get; set; }
        public decimal MinimumValue { get; set; }
        public decimal MaximumValue { get; set; }
        public int MinimumLength { get; set; }
        public int MaximumLength { get; set; }
        public bool InputRequired { get; set; }
        public decimal RandomFactor { get; set; }
        public decimal RandomCounter { get; set; }
        public bool AdditionalCheck { get; set; }
        public DisplayOptions DisplayOption { get; set; }
        public bool LinkItemLinesToTriggerLine { get; set; }
        public RecordIdentifier LinkedInfocodeId { get; set; }
        public bool MultipleSelection { get; set; }
        public int MinSelection { get; set; }
        public int MaxSelection { get; set; }
        public bool CreateInfocodeTransEntries { get; set; }
        public string ExplanatoryHeaderText { get; set; }
        public OKPressedActions OkPressedAction { get; set; }

        public InputTypesEnum InputType
        {
            get
            {
                return (InputTypesEnum)inputType;
            }
            set { inputType = (int)value; }
        }

        public TriggeringEnum Triggering
        {
            get
            {
                if (triggering > 1)
                {
                    throw new IndexOutOfRangeException();
                }

                return (TriggeringEnum)triggering;
            }
            set { triggering = (int)value; }
        }

        public UsageCategoriesEnum UsageCategory
        {
            get
            {
                if ((int)usageCategory > 2)
                {
                    return UsageCategoriesEnum.ItemModifier;
                }

                return usageCategory;
            }
            set { usageCategory = value; }
        }
    }
}
