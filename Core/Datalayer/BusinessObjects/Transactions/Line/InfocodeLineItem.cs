using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.Transactions.Line
{
    /// <summary>
    /// The infocode items that can be added either to a retailtransaction,sale or tender item.
    /// </summary>
    [Serializable]
    public class InfoCodeLineItem : LineItem
    {

        #region Enums

        public enum Triggering
        {
            Automatic = 0,
            OnRequest = 1
        }

        public enum InputTypes
        {
            General = 0,
            /// <summary>
            /// If a subcode is connected to the infocode that will be presented as a list
            /// </summary>
            SubCodeList = 1,
            /// <summary>
            /// If the input is date.
            /// </summary>
            Date = 2,
            /// <summary>
            /// If the input is numeric.
            /// </summary>
            Numeric = 3,
            /// <summary>
            /// If the input is an item.
            /// </summary>
            Item = 4,
            /// <summary>
            /// If the input is a customer.
            /// </summary>
            Customer = 5,
            /// <summary>
            /// If the input is employee.
            /// </summary>
            Operator = 6,
            /// <summary>
            /// If the input is a text.
            /// </summary>
            Text = 9,
            /// <summary>
            /// If a subcode is connected to the infocode that will be presented as buttons
            /// </summary>
            SubCodeButtons = 10,
            /// <summary>
            /// Determines what is the minimum age that the customer needs to have.
            /// </summary>
            AgeLimit = 11,
            /// <summary>
            /// Infocode that groups other infocodes together. The selection codes of the infocode are the infocodes themselves.
            /// </summary>
            Group = 12,
            Selection = 13

        }
        public enum InputRequriedTypes
        {
            /// <summary>
            /// Always = 0
            /// </summary>
            Always = 0,
            /// <summary>
            /// PositiveAndNegative = 1
            /// </summary>
            PositiveAndNegative = 1,
            /// <summary>
            /// Positive = 2
            /// </summary>
            Positive = 2,
            /// <summary>
            /// Negative = 3
            /// </summary>
            Negative = 3
        }

        public enum TableRefId
        {
            None = 0,
            /// <summary>
            /// RetailItem = 1
            /// </summary>
            Item = 1,
            /// <summary>
            /// CustomerTable = 2
            /// </summary>
            Customer = 2,
            /// <summary>
            /// RboStoreTenderTypeTable = 3
            /// </summary>
            Tender = 3,
            /// <summary>
            /// RboStoreTenderTypeCardTable = 4
            /// </summary>
            CreditCard = 4,
            /// <summary>
            /// RboIncomeExpenseAccountTable = 5
            /// </summary>
            IncomeExpense = 5,
            /// <summary>
            /// RboInventItemDepartment = 6
            /// </summary>
            ItemDepartment = 6,
            /// <summary>
            /// RboInventItemGroup = 7
            /// </summary>
            ItemGroup = 7,
            /// <summary>
            /// PosFunctionalityProfile
            /// </summary>
            FunctionalityProfile = 8,
            /// <summary>
            /// PosFunctionalityProfile
            /// </summary>
            PreItem = 9,
            /// <summary>
            /// Infocodes that are manually triggered on the entire sale
            /// </summary>
            Transaction = 10

        }

        public enum InfocodeType
        {
            /// <summary>
            /// Header = 0
            /// </summary>
            Header = 0,
            /// <summary>
            /// Sales = 1
            /// </summary>
            Sales = 1,
            /// <summary>
            /// Payment = 2
            /// </summary>
            Payment = 2,
            /// <summary>
            /// IncomeExpense = 3
            /// </summary>
            IncomeExpense = 3
        }
        #endregion

        #region Member variables
        //Infocode info
        private string infocodeId;              //The id of the infocode.
        private string description;             //Description of the infocode.
        private string prompt;                  //The text that is prompted when the user is prompted for the input of the infocode information.
        private Boolean oncePerTransaction;     //Should the user only be asked once in each transaction.
        private Boolean valueIsAmountOrQuantity;//Is set true if the value is amount or quantity.
        private Boolean printPromptOnReceipt;   //Print what is prompted on the receipt
        private Boolean printInputOnReceipt;    //Print what is inputed on the receipt
        private Boolean printInputNameOnReceipt;//Print the input name on the receipt
        private InputTypes inputType;           //The infocode input type.
        private InputRequriedTypes inputRequriedType; //When is the input required, alway,when postive or negative
        private decimal minimumValue;             //The minimum value that is allowed as a input.
        private decimal maximumValue;             //The maximum value that is allowed as a input.
        private int minimumLength;              //The minimum length of the input.
        private int maximumLength;              //The maximum lenght of the input.
        private int minSelection;
        private int maxSelection;               
        private Boolean inputRequired;          //Is the the input required or can it be skipped
        private Boolean standardValueIsOne;     //The standard value equal to 1.
        private string linkedInfoCodeId;        //The id of the linked infocode.
        private decimal randomFactor;           //The random factor;
        private InfocodeType infocodeType;      //Where does the infocode originate from
        private string refRelation;             //First field in the primary key of what activated the infocode.
        private string refRelation2;            //Second field in the primary key of what activated the infocode.
        private string refRelation3;            //Third field in the primary key of what activated the infocode.
        private Triggering triggered;           //The field indicates how the infocode on the line is triggered. The options are Automatic and On Request
        private bool createTransactionEntries;  //Indicates whether the ifnocode creates transaction infocode entries when the POS transaction is posted into a completed transaction
        private bool linkItemLinesToTriggerLine;//Indicates whether the subcode should be linked to a single item line
        private string unitOfMeasure;           //Is the infocode valid only for a specific unit of measure?
        private string salesTypeFilter;         //Is the infocode valid only for a specific sales type

        //The input
        private string information;     //The information retreived.
        private string information2;    //Second line of information retrieved.
        private string subcode;         //The subcode id if it selected, i.e gender.
        private int subcodeSaleLineId; //The line id of the item that was sold through the subcode
        private string variantcode;     //The variant code.
        //Amount        
        private decimal amount;         //The item price (negative)
        //Timestamps
        private DateTime beginDateTime = DateTime.ParseExact("01.01.1900", "dd.MM.yyyy", null); //The start date and time of the infocode entry
        private DateTime endDateTime = DateTime.ParseExact("01.01.1900", "dd.MM.yyyy", null);   //The end date and time of the infocode entry

        //Additional info for the service
        private int additionalCheck; //Can be any value. Only to be used in the Infocode Service
        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether the ifnocode creates transaction infocode entries when the POS transaction is posted into a completed transaction
        /// </summary>
        public bool CreateTransactionEntries
        {
            get { return createTransactionEntries; }
            set { createTransactionEntries = value; }
        }

        /// <summary>
        /// The field indicates how the infocode on the line is triggered. The options are Automatic and On Request
        /// </summary>
        public Triggering Triggered
        {
            get { return triggered; }
            set { triggered = value; }
        }

        /// <summary>
        /// The id of the infocode.
        /// </summary>
        public string InfocodeId
        {
            get { return infocodeId; }
            set { infocodeId = value; }
        }
        ///// <summary>
        ///// Description of the infocode.
        ///// </summary>
        //public string Description
        //{
        //    get { return description; }
        //    set { description = value; }
        //}
        /// <summary>
        /// The text that is prompted when the user is prompted for the input of the infocode information.
        /// </summary>
        public string Prompt
        {
            get { return prompt; }
            set { prompt = value; }
        }
        /// <summary>
        /// Should the user only be asked once in each transaction.
        /// </summary>
        public Boolean OncePerTransaction
        {
            get { return oncePerTransaction; }
            set { oncePerTransaction = value; }
        }
        /// <summary>
        /// Is set true if the value is amount or quantity.
        /// </summary>
        public Boolean ValueIsAmountOrQuantity
        {
            get { return valueIsAmountOrQuantity; }
            set { valueIsAmountOrQuantity = value; }
        }
        /// <summary>
        /// Print what is prompted on the receipt
        /// </summary>
        public Boolean PrintPromptOnReceipt
        {
            get { return printPromptOnReceipt; }
            set { printPromptOnReceipt = value; }
        }
        /// <summary>
        /// Print what is inputed on the receipt
        /// </summary>
        public Boolean PrintInputOnReceipt
        {
            get { return printInputOnReceipt; }
            set { printInputOnReceipt = value; }
        }
        /// <summary>
        /// Print the input name on the receipt
        /// </summary>
        public Boolean PrintInputNameOnReceipt
        {
            get { return printInputNameOnReceipt; }
            set { printInputNameOnReceipt = value; }
        }
        /// <summary>
        /// The infocode input type.
        /// </summary>
        public InputTypes InputType
        {
            get { return inputType; }
            set { inputType = value; }
        }
        /// <summary>
        /// When is the input required
        /// </summary>
        public InputRequriedTypes InputRequriedType
        {
            get { return inputRequriedType; }
            set { inputRequriedType = value; }
        }
        /// <summary>
        /// The minimum value that is allowed as a input.
        /// </summary>
        public decimal MinimumValue
        {
            get { return minimumValue; }
            set { minimumValue = value; }
        }
        /// <summary>
        /// The maximum value that is allowed as a input.
        /// </summary>
        public decimal MaximumValue
        {
            get { return maximumValue; }
            set { maximumValue = value; }
        }
        /// <summary>
        /// The minimum length of the input.
        /// </summary>
        public int MinimumLength
        {
            get { return minimumLength; }
            set { minimumLength = value; }
        }
        /// <summary>
        /// The maximum lenght of the input.
        /// </summary>
        public int MaximumLength
        {
            get { return maximumLength; }
            set { maximumLength = value; }
        }

        /// <summary>
        /// Minimum times the infocode can be selected - used for cross selling and item modifiers
        /// </summary>
        public int MinSelection
        {
            get { return minSelection; }
            set { minSelection = value; }
        }

        /// <summary>
        /// Maximum times the infocode can be selected - used for cross selling and item modifiers
        /// </summary>
        public int MaxSelection
        {
            get { return maxSelection; }
            set { maxSelection = value; }
        }

        /// <summary>
        /// Is the the input required or can it be skipped
        /// </summary>
        public Boolean InputRequired
        {
            get { return inputRequired; }
            set { inputRequired = value; }
        }
        /// <summary>
        /// The standard value equal to 1.
        /// </summary>
        public Boolean StandardValueIsOne
        {
            get { return standardValueIsOne; }
            set { standardValueIsOne = value; }
        }
        /// <summary>
        /// The id of the linked infocode.
        /// </summary>
        public string LinkedInfoCodeId
        {
            get { return linkedInfoCodeId; }
            set { linkedInfoCodeId = value; }
        }
        /// <summary>
        /// The random factor;
        /// </summary>
        public decimal RandomFactor
        {
            get { return randomFactor; }
            set { randomFactor = value; }
        }
        /// <summary>
        /// The information retreived.
        /// </summary>
        [StringLength(120)]
        public string Information
        {
            get { return information; }
            set { information = value; }
        }

        public string Information2
        {
            get { return information2; }
            set { information2 = value; }
        }

        /// <summary>
        /// The subcode if it selected, i.e gender.
        /// </summary>
        public string Subcode
        {
            get { return subcode; }
            set { subcode = value; }
        }

        /// <summary>
        /// The variant code.
        /// </summary>
        public string Variantcode
        {
            get { return variantcode; }
            set { variantcode = value; }
        }
        /// <summary>
        /// First field in the primary key of what activated the infocode.
        /// </summary>
        public string RefRelation
        {
            get { return refRelation; }
            set { refRelation = value; }
        }
        /// <summary>
        /// Second field in the primary key of what activated the infocode.
        /// </summary>
        public string RefRelation2
        {
            get { return refRelation2; }
            set { refRelation2 = value; }
        }
        /// <summary>
        /// Third field in the primary key of what activated the infocode.
        /// </summary>
        public string RefRelation3
        {
            get { return refRelation3; }
            set { refRelation3 = value; }
        }
        /// <summary>
        /// The item price (negative)
        /// </summary>
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        /// <summary>
        /// Where does the infocode originate from (sales, payment, etc)
        /// </summary>
        public InfocodeType Type
        {
            get { return infocodeType; }
            set { infocodeType = value; }
        }

        /// <summary>
        /// Can be any value. Only to be used in the Infocode Service 
        /// </summary>
        public int AdditionalCheck
        {
            get { return additionalCheck; }
            set { additionalCheck = value; }
        }


        /// <summary>
        /// Indicates whether the subcode should be linked to a single item line
        /// </summary>
        public bool LinkItemLinesToTriggerLine
        {
            get { return linkItemLinesToTriggerLine; }
            set { linkItemLinesToTriggerLine = value; }
        }

        /// <summary>
        /// Is the infocode valid only for a specific unit of measure?        
        /// </summary>
        public string UnitOfMeasure
        {
            get { return unitOfMeasure; }
            set { unitOfMeasure = value; }
        }

        /// <summary>
        /// Is the infocode valid only for a specific sales type
        /// </summary>
        public string SalesTypeFilter
        {
            get { return salesTypeFilter; }
            set { salesTypeFilter = value; }
        }

        #endregion


        public override object Clone()
        {
            InfoCodeLineItem item = new InfoCodeLineItem();
            Populate(item);
            return item;
        }

        protected virtual void Populate(InfoCodeLineItem item)
        {
            base.Populate(item);
            item.infocodeId = infocodeId;
            item.description = description;
            item.prompt = prompt;
            item.oncePerTransaction = oncePerTransaction;
            item.valueIsAmountOrQuantity = valueIsAmountOrQuantity;
            item.printPromptOnReceipt = printPromptOnReceipt;
            item.printInputNameOnReceipt = printInputNameOnReceipt;
            item.printInputOnReceipt = printInputOnReceipt;
            item.inputType = inputType;
            item.inputRequriedType = inputRequriedType;
            item.minimumValue = minimumValue;
            item.maximumValue = maximumValue;
            item.minimumLength = minimumLength;
            item.maximumLength = maximumLength;
            item.minSelection = minSelection;
            item.maxSelection = maxSelection;
            item.inputRequired = inputRequired;
            item.standardValueIsOne = standardValueIsOne;
            item.linkedInfoCodeId = linkedInfoCodeId;
            item.randomFactor = randomFactor;
            item.infocodeType = infocodeType;
            item.refRelation = refRelation;
            item.refRelation2 = refRelation2;
            item.refRelation3 = refRelation3;
            item.information = information;
            item.information2 = information2;
            item.subcode = subcode;
            item.subcodeSaleLineId = subcodeSaleLineId;
            item.variantcode = variantcode;
            item.amount = amount;
            item.beginDateTime = beginDateTime;
            item.endDateTime = endDateTime;
            item.additionalCheck = additionalCheck;
            item.triggered = triggered;
            item.createTransactionEntries = createTransactionEntries;
            item.linkItemLinesToTriggerLine = linkItemLinesToTriggerLine;
            item.unitOfMeasure = unitOfMeasure;
            item.salesTypeFilter = salesTypeFilter;
        }
        /// <summary>
        /// sets the member begindatetime to datetime.now
        /// </summary>
        public InfoCodeLineItem()
        {
            this.BeginDateTime = DateTime.Now;
            information = string.Empty;
            information2 = string.Empty;
            refRelation = string.Empty;
            refRelation2 = string.Empty;
            refRelation3 = string.Empty;
            maximumLength = 120;
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            try
            {
                /*
                * Strings      added as is
                * Int          added as is
                * Bool         added as is
                * 
                * Decimal      added with ToString()
                * DateTime     added with DevUtilities.Utility.DateTimeToXmlString()
                * 
                * Enums        added with an (int) cast   
                * 
               */
                XElement xInfocode = new XElement("InfoCodeLineItem",
                    new XElement("infocodeId", infocodeId),
                    new XElement("description", description),
                    new XElement("prompt", prompt),
                    new XElement("oncePerTransaction", Conversion.ToXmlString(oncePerTransaction)),
                    new XElement("valueIsAmountOrQuantity", Conversion.ToXmlString(valueIsAmountOrQuantity)),
                    new XElement("printPromptOnReceipt", Conversion.ToXmlString(printPromptOnReceipt)),
                    new XElement("printInputOnReceipt", Conversion.ToXmlString(printInputOnReceipt)),
                    new XElement("printInputNameOnReceipt", Conversion.ToXmlString(printInputNameOnReceipt)),
                    new XElement("inputType", Conversion.ToXmlString((int)inputType)),
                    new XElement("inputRequriedType", Conversion.ToXmlString((int)inputRequriedType)),
                    new XElement("minimumValue", Conversion.ToXmlString(minimumValue)),
                    new XElement("maximumValue", Conversion.ToXmlString(maximumValue)),
                    new XElement("minimumLength", Conversion.ToXmlString(minimumLength)),
                    new XElement("maximumLength", Conversion.ToXmlString(maximumLength)),
                    new XElement("minSelection", Conversion.ToXmlString(minSelection)),
                    new XElement("maxSelection", Conversion.ToXmlString(maxSelection)),
                    new XElement("inputRequired", Conversion.ToXmlString(inputRequired)),
                    new XElement("standardValueIsOne", Conversion.ToXmlString(standardValueIsOne)),
                    new XElement("linkedInfoCodeId", linkedInfoCodeId),
                    new XElement("randomFactor", Conversion.ToXmlString(randomFactor)),
                    new XElement("infocodeType", Conversion.ToXmlString((int)infocodeType)),
                    new XElement("refRelation", refRelation),
                    new XElement("refRelation2", refRelation2),
                    new XElement("refRelation3", refRelation3),
                    new XElement("information", information),
                    new XElement("information2", information2),
                    new XElement("subcode", subcode),
                    new XElement("subcodeSaleLineId", Conversion.ToXmlString(subcodeSaleLineId)),
                    new XElement("variantcode", variantcode),
                    new XElement("amount", Conversion.ToXmlString(amount)),
                    new XElement("beginDateTime", Conversion.ToXmlString(beginDateTime)),
                    new XElement("endDateTime", Conversion.ToXmlString(endDateTime)),
                    new XElement("additionalCheck", Conversion.ToXmlString(additionalCheck)),
                    new XElement("triggered", Conversion.ToXmlString((int)triggered)),
                    new XElement("createTransactionEntries", Conversion.ToXmlString(createTransactionEntries)),
                    new XElement("linkItemLinesToTriggerLine", Conversion.ToXmlString(linkItemLinesToTriggerLine)),
                    new XElement("unitOfMeasure", unitOfMeasure),
                    new XElement("salesTypeFilter", salesTypeFilter)
                );

                xInfocode.Add(base.ToXML(errorLogger));
                return xInfocode;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "InfoCodeLineItem.ToXml", ex);

                throw;
            }
        }

        public override void ToClass(XElement xItem, IErrorLog errorLogger = null)
        {
            try
            {
                if (xItem.HasElements)
                {
                    IEnumerable<XElement> classVariables = xItem.Elements();
                    foreach (XElement xVariable in classVariables)
                    {
                        if (!xVariable.IsEmpty)
                        {
                            try
                            {
                                switch (xVariable.Name.ToString())
                                {
                                    case "infocodeId":
                                        infocodeId = xVariable.Value;
                                        break;
                                    case "description":
                                        description = xVariable.Value;
                                        break;
                                    case "prompt":
                                        prompt = xVariable.Value;
                                        break;
                                    case "oncePerTransaction":
                                        oncePerTransaction = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "valueIsAmountOrQuantity":
                                        valueIsAmountOrQuantity = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "printPromptOnReceipt":
                                        printPromptOnReceipt = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "printInputOnReceipt":
                                        printInputOnReceipt = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "printInputNameOnReceipt":
                                        printInputNameOnReceipt = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "inputType":
                                        inputType = (InputTypes)Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "inputRequriedType":
                                        inputRequriedType = (InputRequriedTypes)Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "minimumValue":
                                        minimumValue = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "maximumValue":
                                        maximumValue = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "minSelection":
                                        minSelection = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "maxSelection":
                                        maxSelection = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "minimumLength":
                                        minimumLength = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "maximumLength":
                                        maximumLength = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "inputRequired":
                                        inputRequired = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "standardValueIsOne":
                                        standardValueIsOne = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "linkedInfoCodeId":
                                        linkedInfoCodeId = xVariable.Value;
                                        break;
                                    case "randomFactor":
                                        randomFactor = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "infocodeType":
                                        infocodeType = (InfocodeType)Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "refRelation":
                                        refRelation = xVariable.Value;
                                        break;
                                    case "refRelation2":
                                        refRelation2 = xVariable.Value;
                                        break;
                                    case "refRelation3":
                                        refRelation3 = xVariable.Value;
                                        break;
                                    case "information":
                                        information = xVariable.Value;
                                        break;
                                    case "information2":
                                        information2 = xVariable.Value;
                                        break;
                                    case "subcode":
                                        subcode = xVariable.Value;
                                        break;
                                    case "subcodeSaleLineId":
                                        subcodeSaleLineId = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "variantcode":
                                        variantcode = xVariable.Value;
                                        break;
                                    case "amount":
                                        amount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "beginDateTime":
                                        beginDateTime = Conversion.XmlStringToDateTime(xVariable.Value);
                                        break;
                                    case "endDateTime":
                                        endDateTime = Conversion.XmlStringToDateTime(xVariable.Value);
                                        break;
                                    case "additionalCheck":
                                        additionalCheck = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "triggered":
                                        triggered = (Triggering)Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "createTransactionEntries":
                                        createTransactionEntries = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "linkItemLinesToTriggerLine":
                                        linkItemLinesToTriggerLine = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "unitOfMeasure":
                                        unitOfMeasure = xVariable.Value;
                                        break;
                                    case "salesTypeFilter":
                                        salesTypeFilter = xVariable.Value;
                                        break;
                                    default:
                                        base.ToClass(xVariable,errorLogger);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                errorLogger?.LogMessage(LogMessageType.Error, "InfoCodeLineItem:" + xVariable.Name, ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "InfoCodeLineItem.ToClass", ex);
                throw;
            }
        }

    }
}
