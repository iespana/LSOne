using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.Tax")]
namespace LSOne.DataLayer.BusinessObjects.Tax
{
    /// <summary>
    /// Tax codes are used to calculate tax for everything in the LS POS / Site Manager system. Usually they belong to tax groups and tax codes from two different groups are used 
    /// to calculate the final tax. The value of the tax code is contained in a list of tax code values, but this list usually only contains one item.
    /// </summary>
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class TaxCode : OptimizedUpdateDataEntity
    {
        private decimal taxRoundOff;
        private RoundoffTypeEnum taxRoundOffType;
        private string receiptDisplay;

        /// <summary>
        /// Defines how to sort tax codes in a list
        /// </summary>
        public enum SortEnum
        {
            /// <summary>
            /// Order by tax code ID (Column TAXCODE)
            /// </summary>
            SalesTaxCode,
            /// <summary>
            /// Order by description (Column TAXNAME)
            /// </summary>
            Description,
            /// <summary>
            /// Order by round off (Column TAXROUNDOFF)
            /// </summary>
            RoundOff,
            /// <summary>
            /// Order by rounding type (Column TAXROUNDOFFTYPE)
            /// </summary>
            RoundingType
        }

        /// <summary>
        /// Determines the round off type of the tax code
        /// </summary>
        public enum RoundoffTypeEnum
        {
            Nearest = 0,
            Down = 1,
            Up = 2
        }

        public TaxCode()
            : base()
        {
            Initialize();           
        }

        protected sealed override void Initialize()
        {
            taxRoundOff = 0.0m;
            taxRoundOffType = RoundoffTypeEnum.Nearest;
            receiptDisplay = "";
        }

        /// <summary>
        /// The description of the retail group
        /// </summary>
        [DataMember]
        [StringLength(30)]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                if (base.Text != value)
                {
                    PropertyChanged("TAXNAME", value);
                    base.Text = value;
                }
            }
        }

        /// <summary>
        /// The amount that the tax code value should round to. Usually this amount is 0.1, 0.01, 0.001 , ...
        /// and contains the smallest amount that the tax code should represent.
        /// </summary>
        [DataMember]
        public decimal TaxRoundOff
        {
            get { return taxRoundOff; }
            set 
            {
                if (taxRoundOff != value)
                {
                    PropertyChanged("TAXROUNDOFF", value);
                    taxRoundOff = value;
                }
                NumberOfDecimals = taxRoundOff.DigitsBeforeFirstSignificantDigit();
            }
        }

        /// <summary>
        /// The round off type of the tax code
        /// </summary>
        [DataMember]
        public RoundoffTypeEnum TaxRoundOffType
        {
            get { return taxRoundOffType; }
            set
            {
                if (taxRoundOffType != value)
                {
                    PropertyChanged("TAXROUNDOFFTYPE", value);
                    taxRoundOffType = value;
                }
            }
        }
        
        /// <summary>
        /// A string representation of the round off type of the tax code
        /// </summary>
        public string TaxRoundOffTypeFormatted
        {
            get
            {
                if (taxRoundOffType == RoundoffTypeEnum.Nearest)
                    return Properties.Resources.Nearest;
                else if (taxRoundOffType == RoundoffTypeEnum.Down)
                    return Properties.Resources.Down;
                else
                    return Properties.Resources.Up;
            }
        }

        /// <summary>
        /// Text used on receipts for the tax code. F.x. is used on customer receipt below all the items and below the total payments
        /// </summary>
        [DataMember]
        public string ReceiptDisplay
        {
            get { return receiptDisplay; }
            set
            {
                if (receiptDisplay != value)
                {
                    PropertyChanged("PRINTCODE", value);
                    receiptDisplay = value;
                }
            }
        }

        /// <summary>
        /// Tax code can have 0..* tax code values. Each tax code value has a Value, To date and From date. If you do not have a date range on your tax codes, this should only 
        /// include one tax code value with a Value equal to your tax percentage and to and from dates blank. Because DateTime objects can not be nullable, the value '01.01.1900' 
        /// is considered blank.
        /// </summary>
        public List<TaxCodeValue> TaxCodeLines { get; set; }

        /// <summary>
        /// Number of decimals this tax code should display. This value is based on the TaxRoundOff value and does not have to be set manually.
        /// </summary>
        public int NumberOfDecimals { get; internal set; }

        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            IEnumerable<XElement> elements = element.Elements();
            foreach (XElement current in elements)
            {
                if (!current.IsEmpty)
                {
                    try
                    {
                        switch (current.Name.ToString())
                        {
                            case "text":
                                Text = current.Value;
                                break;
                            case "id":
                                ID = current.Value;
                                break;
                            case "taxRoundOff" :
                                taxRoundOff = XmlConvert.ToDecimal(current.Value);
                                break;
                            case "taxRoundOffType" :
                                taxRoundOffType = (RoundoffTypeEnum) XmlConvert.ToInt32(current.Value);
                                break;
                            case "receiptDisplay" :
                                receiptDisplay = current.Value;
                                break;
                            case "taxCodeLines" :
                                IEnumerable<XElement> taxCodeValueElements = current.Elements();
                                TaxCodeLines = new List<TaxCodeValue>();
                                TaxCodeValue currentValue;
                                foreach (XElement taxCodeValueElement in taxCodeValueElements)
                                {
                                    currentValue = new TaxCodeValue();
                                    currentValue.ToClass(taxCodeValueElement);
                                    TaxCodeLines.Add(currentValue);
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error,
                                                   current.Name.ToString(), ex);
                        }
                    }
                }
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            XElement taxCodeLines = new XElement("taxCodeLines");
            if (TaxCodeLines != null)
            {
                foreach (TaxCodeValue taxCodeValue in TaxCodeLines)
                {
                    taxCodeLines.Add(taxCodeValue.ToXML(errorLogger));
                }
            }
            XElement xml = new XElement("taxCode",
                    new XElement("id", (string)ID),
                    new XElement("text", Text),
                    new XElement("taxRoundOff", XmlConvert.ToString(taxRoundOff)),
                    new XElement("taxRoundOffType", (int)taxRoundOffType),
                    new XElement("receiptDisplay", receiptDisplay),
                    taxCodeLines);
            return xml;
        }
    }
}
