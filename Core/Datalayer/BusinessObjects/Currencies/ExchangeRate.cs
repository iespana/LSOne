using System;
using System.Runtime.Serialization;
using LSOne.Utilities.DataTypes;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.Currencies")]
namespace LSOne.DataLayer.BusinessObjects.Currencies
{
    /// <summary>
    /// Represents an exchange rate for a currency code starting from a specific date
    /// </summary>
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class ExchangeRate : OptimizedUpdateDataEntity
    {
        private RecordIdentifier fromDate;
        private RecordIdentifier currencyCode;
        private decimal exchangeRateValue;
        private decimal posExchangeRateValue;

        public ExchangeRate()
        {
            FromDate = DateTime.MinValue;
            CurrencyCode = RecordIdentifier.Empty;
            ExchangeRateValue = 0;
            POSExchangeRateValue = 0;
        }

        /// <summary>
        /// The ID of the exchange rate, composed of the Currency Code and the starting date of the exchange rate
        /// </summary>
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(CurrencyCode, FromDate);
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// The starting date of the exchange rate
        /// </summary>
        [DataMember]
        public RecordIdentifier FromDate
        {
            get
            {
                return fromDate;
            }
            set
            {
                if (fromDate != value)
                {
                    fromDate = value;
                    PropertyChanged("FROMDATE", value);
                }
            }
        }

        /// <summary>
        /// The currency code of the exchange rate
        /// </summary>
        [DataMember]
        public RecordIdentifier CurrencyCode
        {
            get
            {
                return currencyCode;
            }
            set
            {
                if (currencyCode != value)
                {
                    currencyCode = value;
                    PropertyChanged("CURRENCYCODE", value);
                }
            }
        }

        /// <summary>
        /// The exchange rate value
        /// </summary>
        [DataMember]
        public decimal ExchangeRateValue
        {
            get
            {
                return exchangeRateValue;
            }
            set
            {
                if (exchangeRateValue != value)
                {
                    exchangeRateValue = value;
                    PropertyChanged("EXCHRATE", value);
                }
            }
        }

        /// <summary>
        /// The exchange rate value for the POS. If this is not set, the <see cref="ExchangeRateValue"/> is used.
        /// </summary>
        [DataMember]
        public decimal POSExchangeRateValue
        {
            get
            {
                return posExchangeRateValue;
            }
            set
            {
                if (posExchangeRateValue != value)
                {
                    posExchangeRateValue = value;
                    PropertyChanged("POSEXCHRATE", value);
                }
            }
        }

        /// <summary>
        /// The Text property is used for internal use only [eg: for logging errors information].
        /// </summary>
        public override string Text
        {
            get { return $"{CurrencyCode.StringValue},  FromDate: {FromDate.StringValue}"; }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}