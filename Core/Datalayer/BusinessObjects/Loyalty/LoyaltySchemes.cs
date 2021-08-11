using System;
using System.Runtime.Serialization;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Loyalty
{
    /// <summary>
    /// A business object that holds information about all Loyalty Schemes
    /// </summary>
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    [KnownType(typeof(Date))]
    public class LoyaltySchemes : DataEntity
    {
        /// <summary>
        /// Determines how to show points on receipt
        /// </summary>
        public enum ShowPointsOnReceiptEnum
        {
            /// <summary>
            /// Do not show points
            /// </summary>
            No = 0,
            /// <summary>
            /// Show only issued points
            /// </summary>
            Issued = 1,
            /// <summary>
            /// Show accumulated points summary
            /// </summary>
            Summary = 2
        }

        /// <summary>
        /// Determines how card is registered
        /// </summary>
        public enum CardRegistrationEnum
        {
            /// <summary>
            /// No registration
            /// </summary>
            None = 0,
            /// <summary>
            /// Register card
            /// </summary>
            Register = 1,
            /// <summary>
            /// Register card and contact
            /// </summary>
            LinkToContact = 2
        }
        

        public static string[] CalculationTypeBaseNames()
        {
            string[] result = Enum.GetNames(typeof(CalculationTypeBase));
            result[0] = Properties.Resources.CalculationTypeBase_Amounts;
            result[1] = Properties.Resources.CalculationTypeBase_Quantity;
            return result;
        }

        public static string AsString(CalculationTypeBase Value)
        {
            switch (Value)
            {
                case CalculationTypeBase.Amounts: return Properties.Resources.CalculationTypeBase_Amounts;
                case CalculationTypeBase.Quantity: return Properties.Resources.CalculationTypeBase_Quantity;
                default: return Enum.GetName(typeof(CalculationTypeBase), Value);
            }
        }

        public static string[] ExpirationTimeUnitEnumNames()
        {
            string[] result = Enum.GetNames(typeof(TimeUnitEnum));
            result[0] = Properties.Resources.ExpirationTimeUnitEnumDay;
            result[1] = Properties.Resources.ExpirationTimeUnitEnumWeek;
            result[2] = Properties.Resources.ExpirationTimeUnitEnumMonth;
            result[3] = Properties.Resources.ExpirationTimeUnitEnumYear;
            return result;
        }

        public static string AsString(TimeUnitEnum Value)
        {
            switch (Value)
            {
                case TimeUnitEnum.Day: return Properties.Resources.ExpirationTimeUnitEnumDay;
                case TimeUnitEnum.Week: return Properties.Resources.ExpirationTimeUnitEnumWeek;
                case TimeUnitEnum.Month: return Properties.Resources.ExpirationTimeUnitEnumMonth;
                case TimeUnitEnum.Year: return Properties.Resources.ExpirationTimeUnitEnumYear;
                default: return Enum.GetName(typeof(TimeUnitEnum), Value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoyaltySchemes" /> class.
        /// </summary>
        public LoyaltySchemes()
        {
            ExpirationTimeUnit = TimeUnitEnum.Day;
            ExpireTimeValue = 0;
        }

        [DataMember]
        [RecordIdentifierValidation(20, Depth = 1)]
        public override RecordIdentifier ID
        {
            get
            {
                return SchemeID;
            }
            set
            {
                SchemeID = value;
            }
        }

        /// <summary>
        /// Gets or sets the expiration time unit such as days, weeks, months or years see more info here <see cref="TimeUnitEnum"/>
        /// </summary>
        /// <value>The expiration time unit.</value>
        [DataMember]
        public TimeUnitEnum ExpirationTimeUnit { get; set; }

        /// <summary>
        /// Gets the expiration time unit as string.
        /// </summary>
        /// <value>The expiration time unit as string.</value>
        public string ExpirationTimeUnitAsString { get { return AsString(ExpirationTimeUnit); } }

        /// <summary>
        /// Gets or sets the expire time value which is the number of <see cref="ExpirationTimeUnit"/> selected i.e. 5 days
        /// </summary>
        /// <value>The expire time value.</value>
        [DataMember]
        public int ExpireTimeValue { get; set; }

        /// <summary>
        /// Gets or sets the loyalty scheme ID.
        /// </summary>
        /// <value>The loyalty scheme ID.</value>
        [DataMember]
        public RecordIdentifier SchemeID { get; set; } 

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        /// <value>The description.</value>
        [DataMember]
        public string Description { get; set; } 

        /// <summary>
        /// Gets or sets the starting card number
        /// </summary>
        /// <value>The starting card number.</value>
        public string StartingCardNumber { get; set; } 

        /// <summary>
        /// Gets or sets the card number length
        /// </summary>
        /// <value>The card number length.</value>
        [DataMember]
        public int CardNumberLength { get; set; } 

        /// <summary>
        /// Gets or sets the setting wether to show message on POS when card is swiped
        /// </summary>
        /// <value>The setting wether to display card message on POS when assigned.</value>
        [DataMember]
        public bool DisplayMessageOnPos { get; set; } 

        /// <summary>
        /// Gets or sets the calculation type
        /// </summary>
        /// <value>The calculation type.</value>
        [DataMember]
        public CalculationTypeBase CalculationType { get; set; }

        /// <summary>
        /// Gets the calculation type as string
        /// </summary>
        /// <value>The calculation type as string.</value>
        public string CalculationTypeAsString { get { return AsString(CalculationType); } }

        /// <summary>
        /// Gets or sets the calculation algorithm Id
        /// </summary>
        /// <value>The calculation algorithm Id.</value>
        [DataMember]
        public int CalculationAlgorithm { get; set; } 

        /// <summary>
        /// Gets or sets the card filter
        /// </summary>
        /// <value>The card filter.</value>
        [DataMember]
        public string CardFilter { get; set; } 

        /// <summary>
        /// Gets or sets the way to show points on receipt
        /// </summary>
        /// <value>The way to show points on receipt.</value>
        [DataMember]
        public ShowPointsOnReceiptEnum ShowPointsOnReceipt { get; set; } 

        /// <summary>
        /// Gets or sets card registration type
        /// </summary>
        /// <value>The card registration type.</value>
        [DataMember]
        public CardRegistrationEnum CardRegistration { get; set; } 

        /// <summary>
        /// Gets or sets days to add to issue date to calculate expire date
        /// </summary>
        /// <value>The expiration calculation in days from issue day.</value>
        [DataMember]
        public Date ExpirationCalculation { get; set; } 

        /// <summary>
        /// Gets or sets loyalty tender type code
        /// </summary>
        /// <value>The loyalty tender type code.</value>
        [DataMember]
        public string LoyaltyTenderType { get; set; } 

        /// <summary>
        /// Gets or sets the points available for use.
        /// </summary>
        /// <value>The points status.</value>
        [DataMember]
        public decimal PointsStatus { get; set; } 

        /// <summary>
        /// Gets or sets the issued points.
        /// </summary>
        /// <value>The issued points.</value>
        [DataMember]
        public decimal IssuedPoints { get; set; } 

        /// <summary>
        /// Gets or sets the used points.
        /// </summary>
        /// <value>The used points.</value>
        [DataMember]
        public decimal UsedPoints { get; set; } 

        /// <summary>
        /// Gets or sets date filter for calculating expired, issued and used points
        /// </summary>
        /// <value>The date filter.</value>
        [DataMember]
        public Date DateFilter { get; set; } 

        /// <summary>
        /// Gets or sets customer number link
        /// </summary>
        /// <value>The customer number link.</value>
        [DataMember]
        public RecordIdentifier CustomerNoLink { get; set; } 

        /// <summary>
        /// Gets or sets the expired points.
        /// </summary>
        /// <value>The expired points (expired never used).</value>
        [DataMember]
        public decimal ExpiredPoints { get; set; } 

        /// <summary>
        /// Gets or sets the date the entry was last time modified
        /// </summary>
        /// <value>The date the entry was last time modified.</value>
        [DataMember]
        public Date ModifiedDate { get; set; } 

        /// <summary>
        /// Gets or sets the time the entry was last time modified
        /// </summary>
        /// <value>The time the entry was last time modified.</value>
        [DataMember]
        public int ModifiedTime { get; set; } 

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        /// <value>The user id who last time modified the entry.</value>
        [DataMember]
        public Guid ModifiedBy { get; set; } 

        /// <summary>
        /// Gets or sets the date the entry was created at
        /// </summary>
        /// <value>The date the entry was created at.</value>
        [DataMember]
        public Date CreatedDate { get; set; } 

        /// <summary>
        /// Gets or sets the time the entry was created at
        /// </summary>
        /// <value>The time the entry was created at.</value>
        [DataMember]
        public int CreatedTime { get; set; } 

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        /// <value>The user id who created the entry.</value>
        [DataMember]
        public Guid CreatedBy { get; set; } 

        /// <summary>
        /// Gets or sets the use limit
        /// </summary>
        /// <value>The use limit.</value>
        [DataMember]
        public int UseLimit { get; set; } 
    }
}
