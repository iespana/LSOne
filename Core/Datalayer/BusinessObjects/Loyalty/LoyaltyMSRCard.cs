using System;
using System.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Loyalty
{
    /// <summary>
    /// A business object that holds information about each loyalty card issued
    /// </summary>
    public class LoyaltyMSRCard : DataEntity
    {
        /// <summary>
        /// Determines MSR Card's type
        /// </summary>
        public enum LinkTypeEnum
        {
            /// <summary>
            /// Card is cashier card
            /// </summary>
            Cashier = 0,
            /// <summary>
            /// card is customer card (company card)
            /// </summary>
            Customer = 1,
            /// <summary>
            /// card is contact card (person in a company)
            /// </summary>
            Contact = 2,
        }

        /// <summary>
        /// Determines MSR Card's tender type
        /// </summary>
        public enum TenderTypeEnum
        {
            /// <summary>
            /// CardTender
            /// </summary>
            CardTender = 0,
            /// <summary>
            /// ContactTender
            /// </summary>
            ContactTender = 1,
            /// <summary>
            /// NoTender
            /// </summary>
            NoTender = 2,
            /// <summary>
            /// Blocked
            /// </summary>
            Blocked = 3
        }

        public static string[] TenderTypeEnumNames()
        {
            string[] result = Enum.GetNames(typeof(TenderTypeEnum));
            result[0] = Properties.Resources.TenderTypeEnum_CardTender;
            result[1] = Properties.Resources.TenderTypeEnum_ContactTender;
            result[2] = Properties.Resources.TenderTypeEnum_NoTender;
            result[3] = Properties.Resources.TenderTypeEnum_Blocked;
            //return result;
            var tmpList = result.ToList();
            tmpList.RemoveAt(1);

            return tmpList.ToArray();
        }

        public static string AsString(TenderTypeEnum Value)
        {
            switch (Value)
            {
                case TenderTypeEnum.CardTender: return Properties.Resources.TenderTypeEnum_CardTender;
                case TenderTypeEnum.ContactTender: return Properties.Resources.TenderTypeEnum_ContactTender;
                case TenderTypeEnum.NoTender: return Properties.Resources.TenderTypeEnum_NoTender;
                case TenderTypeEnum.Blocked: return Properties.Resources.TenderTypeEnum_Blocked;
                default: return Enum.GetName(typeof(TenderTypeEnum), Value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoyaltyMSRCard" /> class.
        /// </summary>
        public LoyaltyMSRCard()
            : base()
        {
            CardNumber = "";
            SchemeID = RecordIdentifier.Empty;
            LinkType = LinkTypeEnum.Customer;
            LinkID = RecordIdentifier.Empty;
        }

        [RecordIdentifierValidation(30, Depth = 1)]
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(CardNumber);
            }
            set
            {
                if (value == null)
                {
                    CardNumber = "";
                }
                else
                {
                    CardNumber = value.ToString();
                }
            }
        }

        /// <summary>
        /// Gets or sets the card number.
        /// </summary>
        /// <value>The card number.</value>
        public string CardNumber { get; set; } 

        /// <summary>
        /// Gets or sets the loyalty scheme ID.
        /// </summary>
        /// <value>The loyalty scheme ID.</value>
        public RecordIdentifier SchemeID { get; set; } 

        /// <summary>
        /// The description of the loyalty scheme that is attached to the card
        /// </summary>
        /// <value>The loyalty scheme description.</value>
        public string SchemeDescription { get; set; }

        /// <summary>
        /// The Use points % limit on the scheme on the card
        /// </summary>
        public int UsePointsLimit { get; set; }
        
        /// <summary>
        /// Gets or sets the loyalty customer ID.
        /// </summary>
        /// <value>The loyalty customer ID.</value>
        public RecordIdentifier CustomerID { get; set; } 

        private LinkTypeEnum linkType;
        /// <summary>
        /// Gets or sets the link type.
        /// </summary>
        /// <value>The link type.</value>
        public LinkTypeEnum LinkType 
        {
            get
            {
                return linkType;
            }
            set
            {
                linkType = value;
                if (linkType == LinkTypeEnum.Customer)
                {
                    CustomerID = linkID;
                }
                else
                {
                    CustomerID = RecordIdentifier.Empty;
                }
            }
        }

        private RecordIdentifier linkID;

        /// <summary>
        /// Gets or sets the link ID.
        /// </summary>
        /// <value>The link ID.</value>
        public RecordIdentifier LinkID 
        {
            get
            {
                return linkID;
            }
            set
            {
                linkID = value;
                if (LinkType == LinkTypeEnum.Customer)
                {
                    CustomerID = linkID;
                }
            }
        }

        /// <summary>
        /// Gets or sets the customer name
        /// </summary>
        /// <value>The customer name.</value>
        public string CustomerName { get; set; } 

        /// <summary>
        /// Gets or sets the tender type.
        /// </summary>
        /// <value>The tender type.</value>
        public TenderTypeEnum TenderType { get; set; } 

        /// <summary>
        /// Gets tender type string representation.
        /// </summary>
        /// <value>The tender type as string.</value>
        public string TenderTypeAsString { get { return AsString(TenderType); } }

        /// <summary>
        /// Gets or sets the points available for use.
        /// </summary>
        /// <value>The points status.</value>
        public decimal PointStatus { get; set; } 

        /// <summary>
        /// Gets or sets the issued points.
        /// </summary>
        /// <value>The issued points.</value>
        public decimal IssuedPoints { get; set; } 

        /// <summary>
        /// Gets or sets the used points.
        /// </summary>
        /// <value>The used points.</value>
        public decimal UsedPoints { get; set; } 

        /// <summary>
        /// Gets or sets the expired points.
        /// </summary>
        /// <value>The expired points (expired never used).</value>
        public decimal ExpiredPoints { get; set; } 

        /// <summary>
        /// Gets or sets the starting points.
        /// </summary>
        /// <value>The starting points.</value>
        public decimal StartingPoints { get; set; } 

        /// <summary>
        /// Gets or sets the date the entry was last time modified
        /// </summary>
        /// <value>The date the entry was last time modified.</value>
        public Date ModifiedDate { get; set; } 

        /// <summary>
        /// Gets or sets the time the entry was last time modified
        /// </summary>
        /// <value>The time the entry was last time modified.</value>
        public int ModifiedTime { get; set; } 

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        /// <value>The user id who last time modified the entry.</value>
        public Guid ModifiedBy { get; set; } 

        /// <summary>
        /// Gets or sets the date the entry was created at
        /// </summary>
        /// <value>The date the entry was created at.</value>
        public Date CreatedDate { get; set; } 

        /// <summary>
        /// Gets or sets the time the entry was created at
        /// </summary>
        /// <value>The time the entry was created at.</value>
        public int CreatedTime { get; set; } 

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        /// <value>The user id who created the entry.</value>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Dictates whether this card is used for mobile app loyalty
        /// </summary>
        public bool MobileCard { get; set; }
    }
}
