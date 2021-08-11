#if !MONO
#endif
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Validation;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.Currencies")]
namespace LSOne.DataLayer.BusinessObjects.Currencies
{
    /// <summary>
    /// Data entity class for a currencies
    /// </summary>
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class Currency : OptimizedUpdateDataEntity
    {
        private string currencyPrefix;        
        private string currencySuffix;
        private string symbol;
        private decimal roundOffPurchase;
        private decimal roundOffAmount;
        private decimal roundOffSales;
        private int roundOffTypeAmount;
        private int roundOffTypeSales;
        private int roundOffTypePurchase;

        public Currency() 
            : this("","")
        {
            
        }

        public Currency(string currencyCode, string txt) 
            : base(currencyCode, txt)
        {            
            Initialize();
        }

        protected override void Initialize()
        {
            currencyPrefix = "";
            currencySuffix = "";
            symbol = "";
            roundOffPurchase = 0;
            roundOffAmount = 0;
            roundOffSales = 0;
            roundOffTypeAmount = 0;
            roundOffTypeSales = 0;
            roundOffTypePurchase = 0;
        }

        /// <summary>
        /// ID of the currency. Can only be 3 letters (f.x. EUR, USD ...)
        /// </summary>
        [RecordIdentifierValidation(3)]
        public override RecordIdentifier ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        /// <summary>
        /// The description of the currency
        /// </summary>
        [DataMember]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                if (base.Text != value)
                {
                    PropertyChanged("TXT", value: value);
                    base.Text = value;
                }
            }
        }

        [DataMember]
        public decimal RoundOffPurchase
        {
            get { return roundOffPurchase; }
            set
            {
                if (roundOffPurchase != value)
                {
                    PropertyChanged("ROUNDOFFPURCH", value: value);
                    roundOffPurchase = value;
                }
            }
        }

        [DataMember]
        public decimal RoundOffAmount
        {
            get { return roundOffAmount; }
            set
            {
                if (roundOffAmount != value)
                {
                    PropertyChanged("ROUNDOFFAMOUNT", value: value);
                    roundOffAmount = value;
                }
            }
        }

        [DataMember]
        public decimal RoundOffSales
        {
            get { return roundOffSales; }
            set
            {
                if (roundOffSales != value)
                {
                    PropertyChanged("ROUNDOFFSALES", value: value);
                    roundOffSales = value;
                }
            }
        }

        [DataMember]
        public int RoundOffTypeAmount
        {
            get { return roundOffTypeAmount; }
            set
            {
                if (roundOffTypeAmount != value)
                {
                    PropertyChanged("ROUNDOFFTYPEAMOUNT", value: value);
                    roundOffTypeAmount = value;
                }
            }
        }

        [DataMember]
        public int RoundOffTypeSales
        {
            get { return roundOffTypeSales; }
            set
            {
                if (roundOffTypeSales != value)
                {
                    PropertyChanged("ROUNDOFFTYPESALES", value: value);
                    roundOffTypeSales = value;
                }
            }
        }

        [DataMember]
        public int RoundOffTypePurchase
        {
            get { return roundOffTypePurchase; }
            set
            {
                if (roundOffTypePurchase != value)
                {
                    PropertyChanged("ROUNDOFFTYPEPURCH", value: value);
                    roundOffTypePurchase = value;
                }
            }
        }

        /// <summary>
        /// Prefix of the currency. Examples include $, £ etc
        /// </summary>
        [StringLength(10)]
        [DataMember]
        public string CurrencyPrefix
        {
            get { return currencyPrefix; }
            set
            {
                if (currencyPrefix != value)
                {
                    PropertyChanged("CURRENCYPREFIX", value: value);
                    currencyPrefix = value;
                }
            }
        }

        /// <summary>
        /// Suffix of the currency. Examples include $, £ etc
        /// </summary>
        [StringLength(10)]
        [DataMember]
        public string CurrencySuffix
        {
            get { return currencySuffix; }
            set
            {
                if (currencySuffix != value)
                {
                    PropertyChanged("CURRENCYSUFFIX", value: value);
                    currencySuffix = value;
                }
            }
        }

        [StringLength(5)]
        [DataMember]
        public string Symbol
        {
            get { return symbol; }
            set
            {
                if (symbol != value)
                {
                    PropertyChanged("SYMBOL", value: value);
                    symbol = value;
                }
            }
        }

        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            var currencyElements = element.Elements();
            foreach (XElement storeElem in currencyElements)
            {
                if (!storeElem.IsEmpty)
                {
                    try
                    {
                        switch (storeElem.Name.ToString())
                        {
                            case "name":
                                Text = storeElem.Value;
                                break;
                            case "currencyCode":
                                ID = storeElem.Value;
                                break;
                            case "roundOffPurchase":
                                RoundOffPurchase = Convert.ToDecimal(storeElem.Value, XmlCulture);
                                break;
                            case "roundOffAmount":
                                RoundOffAmount = Convert.ToDecimal(storeElem.Value, XmlCulture);
                                break;
                            case "roundOffSales":
                                RoundOffSales = Convert.ToDecimal(storeElem.Value, XmlCulture);
                                break;
                            case "roundOffTypeAmount":
                                RoundOffTypeAmount = Convert.ToInt32(storeElem.Value);
                                break;
                            case "roundOffTypePurchase":
                                RoundOffTypePurchase = Convert.ToInt32(storeElem.Value);
                                break;
                            case "roundOffTypeSales":
                                RoundOffTypeSales = Convert.ToInt32(storeElem.Value);
                                break;
                            case "currencyPrefix":
                                CurrencyPrefix = storeElem.Value;
                                break;
                            case "currencySuffix":
                                CurrencySuffix = storeElem.Value;
                                break;
                            case "symbol":
                                Symbol = storeElem.Value;
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error,
                                                   storeElem.Name.ToString(), ex);
                        }
                    }
                }
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            XElement xml = new XElement("currency",
                    new XElement("currencyCode", (string)ID),
                    new XElement("name", Text),
                    new XElement("roundOffPurchase", RoundOffPurchase.ToString(XmlCulture)),
                    new XElement("roundOffAmount", RoundOffAmount.ToString(XmlCulture)),
                    new XElement("roundOffSales", RoundOffSales.ToString(XmlCulture)),
                    new XElement("roundOffTypeAmount", RoundOffTypeAmount),
                    new XElement("roundOffTypePurchase", RoundOffTypePurchase),
                    new XElement("roundOffTypeSales", RoundOffTypeSales),
                    new XElement("currencyPrefix", CurrencyPrefix),
                    new XElement("currencySuffix", CurrencySuffix),
                    new XElement("symbol", Symbol));
            return xml;
        }

        public override object Clone()
        {
            var o = new Currency();
            Populate(o);
            return o;
        }

        protected void Populate(Currency o)
        {
            o.ID = (RecordIdentifier)ID.Clone();
            o.Text = Text;
            o.RoundOffPurchase = RoundOffPurchase;
            o.RoundOffAmount = RoundOffAmount;
            o.RoundOffSales = RoundOffSales;
            o.RoundOffTypeAmount = RoundOffTypeAmount;
            o.RoundOffTypePurchase = RoundOffTypePurchase;
            o.RoundOffTypeSales = RoundOffTypeSales;
            o.CurrencyPrefix = CurrencyPrefix;
            o.CurrencySuffix = CurrencySuffix;
            o.Symbol = Symbol;
        }
    }
}
