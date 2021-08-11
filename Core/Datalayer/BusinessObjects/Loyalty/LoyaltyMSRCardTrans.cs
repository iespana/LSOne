using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Loyalty
{
	/// <summary>
	/// A business object that holds information about operations/transactions with loyalty points
	/// </summary>
	public class LoyaltyMSRCardTrans : DataEntity
	{
		/// <summary>
		/// Determines the type of loyalty points entry
		/// </summary>
		public enum EntryTypeEnum
		{
			/// <summary>
			/// entry for valid points
			/// </summary>
			None = 0,
			/// <summary>
			/// entry for voided points
			/// </summary>
			Voided = 1,
		}

		/// <summary>
		/// Determines the type of loyalty points entry
		/// </summary>
		public enum TypeEnum
		{
			/// <summary>
			/// entry about issuing/accumulating points
			/// </summary>
			IssuePoints = 0,
			/// <summary>
			/// entry about using/spending points
			/// </summary>
			UsePoints = 1,
			/// <summary>
			/// entry about expiring points that were not used in time
			/// </summary>
			ExpirePoints = 2
		}

		/// <summary>
		/// Determines the status of loyalty points entry
		/// </summary>
		public enum StatusEnum
		{
			/// <summary>
			/// entry has no points for use (all points are used and/or expired)
			/// </summary>
			Closed = 0,
			/// <summary>
			/// entry has points available for use
			/// </summary>
			Open = 1,
		}

		public static string[] TypeEnumNames()
		{
			string[] result = Enum.GetNames(typeof(TypeEnum));
			result[0] = Properties.Resources.TypeEnumIssued;
            result[1] = Properties.Resources.TypeEnumUsed;
            result[2] = Properties.Resources.TypeEnumExpired;
			return result;
		}

		public static string AsString(TypeEnum Value)
		{
			switch (Value)
			{
                case TypeEnum.IssuePoints: return Properties.Resources.TypeEnumIssued;
                case TypeEnum.UsePoints: return Properties.Resources.TypeEnumUsed;
                case TypeEnum.ExpirePoints: return Properties.Resources.TypeEnumExpired;
				default: return Enum.GetName(typeof(TypeEnum), Value);
			}
		}

		public static string AsString(EntryTypeEnum Value)
		{
			switch (Value)
			{
                case EntryTypeEnum.None: return Properties.Resources.EntryTypeEnumNone;
                case EntryTypeEnum.Voided: return Properties.Resources.EntryTypeEnumVoided;
				default: return Enum.GetName(typeof(EntryTypeEnum), Value);
			}
		}

		public static string AsString(StatusEnum Value)
		{
			switch (Value)
			{
                case StatusEnum.Closed: return Properties.Resources.StatusEnumClosed;
                case StatusEnum.Open: return Properties.Resources.StatusEnumOpen;
				default: return Enum.GetName(typeof(StatusEnum), Value);
			}
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="LoyaltyMSRCardTrans" /> class.
		/// </summary>
		public LoyaltyMSRCardTrans()
		{
			CardNumber = "";
			LineNumber = 0;
            SchemeID = "";
            CustomerID = "";
		}

		/// <summary>
		/// Gets or sets the card number.
		/// </summary>
		/// <value>The card number.</value>
		public string CardNumber { get; set; } //[nvarchar](30)

		/// <summary>
		/// Gets or sets the line number.
		/// </summary>
		/// <value>The line number.</value>
		public decimal LineNumber { get; set; } //[numeric](28, 12)

		/// <summary>
		/// Gets or sets the Transaction ID.
		/// </summary>
		/// <value>The Transaction ID.</value>
		public RecordIdentifier TransactionID { get; set; } //[nvarchar](20)

		/// <summary>
		/// Gets or sets the Receipt ID.
		/// </summary>
		/// <value>The Receipt ID.</value>
		public RecordIdentifier ReceiptID { get; set; } //[nvarchar](20)

		/// <summary>
		/// Gets or sets the Points.
		/// </summary>
		/// <value>The Points.</value>
		public decimal Points { get; set; } //[numeric](28, 12)

		/// <summary>
		/// Gets or sets the date of issue of the points in question.
		/// </summary>
		/// <value>The date when points are issued.</value>
		public Date DateOfIssue { get; set; } //[datetime]

		/// <summary>
		/// Gets or sets the Store ID
		/// </summary>
		/// <value>The Store ID.</value>
		public RecordIdentifier StoreID { get; set; } //[nvarchar](20)

		/// <summary>
		/// Gets or sets the Store name
		/// </summary>
		/// <value>The Store name.</value>
		public string StoreName { get; set; } //RBOSTORETABLE.NAME

		/// <summary>
		/// Gets or sets the Terminal ID
		/// </summary>
		/// <value>The Terminal ID.</value>
		public RecordIdentifier TerminalID { get; set; } //[nvarchar](20)

		/// <summary>
		/// Gets or sets the Terminal name
		/// </summary>
		/// <value>The Terminal name.</value>
		public string TerminalName { get; set; } //RBOTERMINALTABLE.NAME

		/// <summary>
		/// Gets or sets the sequence number.
		/// </summary>
		/// <value>The sequence number.</value>
		public int SequenceNumber { get; set; } //[int]

		/// <summary>
		/// Gets or sets the loyalty customer ID.
		/// </summary>
		/// <value>The loyalty customer ID.</value>
		public RecordIdentifier CustomerID { get; set; } //[nvarchar](20)

		/// <summary>
		/// Gets or sets the customer name
		/// </summary>
		/// <value>The customer name.</value>
		public string CustomerName { get; set; } //CUSTOMER.NAME

		/// <summary>
		/// Gets or sets the Entry Type.
		/// </summary>
		/// <value>The Entry Type.</value>
		public EntryTypeEnum EntryType { get; set; } //[int]

		/// <summary>
		/// Gets the Entry Type as string.
		/// </summary>
		/// <value>The Entry Type as string.</value>
		public string EntryTypeAsString { get { return AsString(EntryType); } }

		/// <summary>
		/// Gets or sets the Expiration Date.
		/// </summary>
		/// <value>The Expiration Date of issued points.</value>
		public Date ExpirationDate { get; set; } //[datetime]

		/// <summary>
		/// Gets or sets the loyalty scheme ID.
		/// </summary>
		/// <value>The loyalty scheme ID.</value>
		public RecordIdentifier SchemeID { get; set; } //[nvarchar](20)

		/// <summary>
		/// Gets or sets the Scheme description
		/// </summary>
		/// <value>The Scheme description.</value>
		public string SchemeDescription { get; set; } //RBOLOYALTYSCHEMESTABLE.DESCRIPTION

		/// <summary>
		/// Gets or sets the Statement ID.
		/// </summary>
		/// <value>The Statement ID.</value>
		public RecordIdentifier StatementID { get; set; } //[nvarchar](20)

		/// <summary>
		/// Gets or sets the Statement Code.
		/// </summary>
		/// <value>The Statement Code.</value>
		public string StatementCode { get; set; } //[nvarchar](10)

		/// <summary>
		/// Gets or sets the Staff ID.
		/// </summary>
		/// <value>The Staff ID who processed the transaction on POS.</value>
		public RecordIdentifier StaffID { get; set; } //[nvarchar](20)

		/// <summary>
		/// Gets or sets the Staff Name.
		/// </summary>
		/// <value>The Staff Name of who processed the transaction on POS.</value>
		public RecordIdentifier StaffName { get; set; } //[nvarchar](20)

		/// <summary>
		/// Gets or sets the loyalty points transaction line number.
		/// </summary>
		/// <value>The loyalty points transaction line number.</value>
		public decimal LoyPointsTransLineNumber { get; set; } //[numeric](28, 12)

		/// <summary>
		/// Gets or sets the date the entry was last time modified
		/// </summary>
		/// <value>The date the entry was last time modified.</value>
		public Date ModifiedDate { get; set; } //[datetime]

		/// <summary>
		/// Gets or sets the time the entry was last time modified
		/// </summary>
		/// <value>The time the entry was last time modified.</value>
		public int ModifiedTime { get; set; } //[int]

		/// <summary>
		/// Gets or sets the user id
		/// </summary>
		/// <value>The user id who last time modified the entry.</value>
		public Guid ModifiedBy { get; set; } //uniqueidentifier

		/// <summary>
		/// Gets or sets the date the entry was created at
		/// </summary>
		/// <value>The date the entry was created at.</value>
		public Date CreatedDate { get; set; } //[datetime]

		/// <summary>
		/// Gets or sets the time the entry was created at
		/// </summary>
		/// <value>The time the entry was created at.</value>
		public int CreatedTime { get; set; } //[int]

		/// <summary>
		/// Gets or sets the user id
		/// </summary>
		/// <value>The user id who created the entry.</value>
		public Guid CreatedBy { get; set; } //uniqueidentifier

		/// <summary>
		/// Gets or sets the Type.
		/// </summary>
		/// <value>The Type.</value>
		public TypeEnum Type { get; set; } //[int]

		/// <summary>
		/// Gets the Type as string.
		/// </summary>
		/// <value>The Type as string.</value>
		public string TypeAsString { get { return AsString(Type); } }

		/// <summary>
		/// Gets or sets the Remaining Points.
		/// </summary>
		/// <value>The Remaining (not-expired and not-used) Points.</value>
		public decimal RemainingPoints { get; set; } //[numeric](28, 12)

		/// <summary>
		/// Gets or sets the open status.
		/// </summary>
		/// <value>The Open status shows if there are remaining points left.</value>
		public bool Open { get; set; } //[tinyint]

		/// <summary>
		/// Gets open status as string.
		/// </summary>
		/// <value>The Open status shows if there are remaining points left.</value>
		public string StatusAsString { 
			get
			{
				if (Open)
				{
					return AsString(StatusEnum.Open);
				}
				else
				{
					return AsString(StatusEnum.Closed);
				}
			}
		}

	}
}
