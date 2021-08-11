using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.TransactionObjects
{
    /// <summary>
    /// A class that holds extra hospitality information gathered from a <see cref="RetailTransaction"/>. This class
    /// is used when a sales transaction is concluded and we are saving information down to the transaction tables.
    /// </summary>    
    public class DiningTableTransaction : DataEntity
    {
        public DiningTableTransaction()
            : base()
        {
            TransactionID = "";
            StoreID = "";
            TerminalID = "";
            HospitalitySalesType = "";
            DiningTableLayoutID = "";
            DiningTableNo = 0;
            NoOfGuests = 0;
        }

        /// <summary>
        /// Gets the ID of the dining table transaction. The id consists of:
        /// * TransactionID
        /// * StoreID
        /// * TerminalID
        /// </summary>
        public override RecordIdentifier ID
        {
            get { return new RecordIdentifier(TransactionID, StoreID, TerminalID); }            
        }


        /// <summary>
        /// Gets or sets the transaction ID
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier TransactionID { get; set; }

        /// <summary>
        /// Gets or sets the store ID
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier StoreID { get; set; }

        /// <summary>
        /// Gets or sets the terminal ID
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier TerminalID { get; set; }

        /// <summary>
        /// Gets or sets the hospitality sales type
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier HospitalitySalesType { get; set; }

        /// <summary>
        /// Gets or sets the dining table layout ID
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier DiningTableLayoutID { get; set; }

        /// <summary>
        /// Gets or sets the table number
        /// </summary>
        public int DiningTableNo { get; set; }

        /// <summary>
        /// Gets or sets the number of guests seated at the table
        /// </summary>
        public int NoOfGuests { get; set; }
    }
}
