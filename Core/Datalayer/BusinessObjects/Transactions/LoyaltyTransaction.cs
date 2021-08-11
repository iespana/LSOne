namespace LSOne.DataLayer.BusinessObjects.Transactions
{
    //[Serializable()]
    //public class LoyaltyTransaction : BaseTransaction
    //{
    //    public LoyaltyTransaction()
    //        : base()
    //    {
    //        SchemeID = "";
    //        CustomerID = "";
    //        LoyaltyCustomerID = "";
    //    }

    //    /// <summary>
    //    /// The loyalty card number
    //    /// </summary>
    //    public string LoyaltyCardNumber { get; set; }

    //    /// <summary>
    //    /// The number of loyalty points for this transaction
    //    /// </summary>
    //    public decimal CalculatedLoyaltyPoints { get; set; }

    //    /// <summary>
    //    /// The total number of accumulated loyalty points for this card
    //    /// </summary>
    //    public decimal AccumulatedLoyaltyPoints { get; set; }

    //    /// <summary>
    //    /// Have we added points for this transaction. E.g. communicated with an external service.
    //    /// </summary>
    //    public bool LoyaltyPointsAdded { get; set; }

    //    /// <summary>
    //    /// The card's scheme ID that determines the rule to follow to get loyalty points
    //    /// </summary>
    //    public RecordIdentifier SchemeID { get; set; }

    //    /// <summary>
    //    /// The card's customer ID that showing which customer owns the card, if any.
    //    /// </summary>
    //    public RecordIdentifier CustomerID { get; set; }

    //    /// <summary>
    //    /// The loyalty customer ID.
    //    /// </summary>
    //    public RecordIdentifier LoyaltyCustomerID { get; set; }

    //    /// <summary>
    //    /// The expire unit of the scheme being used.
    //    /// </summary>
    //    public int ExpireUnit { get; set; }

    //    /// <summary>
    //    /// The expire value of the scheme being used.
    //    /// </summary>
    //    public int ExpireValue { get; set; }


    //    //NVB001_CustomerLoyalty+

    //    /// <summary>
    //    /// The loyalty line number
    //    /// </summary>
    //    public decimal LineNum { get; set; }

    //    /// <summary>
    //    /// Determines the type of loyalty points entry
    //    /// </summary>
    //    public enum EntryTypeEnum
    //    {
    //        /// <summary>
    //        /// entry for valid points
    //        /// </summary>
    //        None = 0,
    //        /// <summary>
    //        /// entry for voided points
    //        /// </summary>
    //        Voided = 1,
    //    }

    //    public RecordIdentifier ReceiptID { get; set; }

    //    public decimal Points { get; set; }

    //    public DateTime DateOfIssue { get; set; }

    //    public string CardNumber { get; set; }

    //    public int SequenceNumber { get; set; }

    //    public RecordIdentifier LoyaltyCustID { get; set; }

    //    /// <summary>
    //    /// Gets or sets the Entry Type.
    //    /// </summary>
    //    /// <value>The Entry Type.</value>
    //    public EntryTypeEnum EntryType { get; set; } //[int]

    //    public DateTime ExpirationDate { get; set; } //[int]

    //    public RecordIdentifier StatementID { get; set; }

    //    public string StatementCode { get; set; }

    //    public RecordIdentifier StaffID { get; set; }

    //    /// <summary>
    //    /// Gets or sets the date the entry was last time modified
    //    /// </summary>
    //    /// <value>The date the entry was last time modified.</value>
    //    public Date ModifiedDate { get; set; } //[datetime]

    //    /// <summary>
    //    /// Gets or sets the time the entry was last time modified
    //    /// </summary>
    //    /// <value>The time the entry was last time modified.</value>
    //    public int ModifiedTime { get; set; } //[int]

    //    /// <summary>
    //    /// Gets or sets the user id
    //    /// </summary>
    //    /// <value>The user id who last time modified the entry.</value>
    //    public Guid ModifiedBy { get; set; } //[uniqueidentifier]

    //    /// <summary>
    //    /// Gets or sets the date the entry was created at
    //    /// </summary>
    //    /// <value>The date the entry was created at.</value>
    //    public Date CreatedDate { get; set; } //[datetime]

    //    /// <summary>
    //    /// Gets or sets the time the entry was created at
    //    /// </summary>
    //    /// <value>The time the entry was created at.</value>
    //    public int CreatedTime { get; set; } //[int]

    //    /// <summary>
    //    /// Gets or sets the user id
    //    /// </summary>
    //    /// <value>The user id who created the entry.</value>
    //    public Guid CreatedBy { get; set; } //[uniqueidentifier]

    //    public int Replicated { get; set; }

    //    public int ReplicationCounter { get; set; }

    //    public decimal AccumulatedPoints { get; set; }

    //}
}
