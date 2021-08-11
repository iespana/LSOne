using System;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.DataLayer.TransactionObjects
{
    /// <summary>
    /// A business object that holds information about a <see cref="HospitalityTransaction"/>
    /// </summary>
    public class HospitalityTransaction : DataEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HospitalityTransaction" /> class.
        /// </summary>
        public HospitalityTransaction()
            : base()
        {
            StoreID = RecordIdentifier.Empty;
            TerminalID = RecordIdentifier.Empty;
            TableID = RecordIdentifier.Empty;
            Guest = RecordIdentifier.Empty;
            HospitalityType = RecordIdentifier.Empty;
            CreatedBy = 0;
            Transaction = new RetailTransaction("","",false);
            SplitID = RecordIdentifier.Empty;
        }
        /// <summary>
        /// Composed of the following IDs in this order; TransactionID, StoreID, TerminalID, TableID, Guest, HospitalityType
        /// </summary>
        public override RecordIdentifier  ID
        {
            get 
	        {
	            return new RecordIdentifier(TransactionID, StoreID, TerminalID, TableID, Guest, HospitalityType, SplitID);
	        }
	        set 
	        {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public override object Clone()
        {
            var tx = new HospitalityTransaction();
            Populate(tx);
            return tx;
        }

        /// <summary>
        /// Gets or sets the transaction ID
        /// </summary>
        /// <value>The transaction ID.</value>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier TransactionID { get; set; }
        /// <summary>
        /// Gets or sets the store ID.
        /// </summary>
        /// <value>The store ID.</value>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier StoreID { get; set; }
        /// <summary>
        /// Gets or sets the terminal ID.
        /// </summary>
        /// <value>The terminal ID.</value>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier TerminalID { get; set; }
        /// <summary>
        /// Gets or sets the table ID.
        /// </summary>
        /// <value>The table ID.</value>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier TableID { get; set; }
        /// <summary>
        /// Gets or sets the guest ID
        /// </summary>
        /// <value>The guest ID</value>
        public RecordIdentifier Guest { get; set; }
        /// <summary>
        /// Gets or sets the type of the hospitality.
        /// </summary>
        /// <value>The type of the hospitality.</value>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier HospitalityType { get; set; }
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the table transaction.
        /// </summary>
        /// <value>The table transaction</value>
        public IRetailTransaction Transaction { get; set; }

        /// <summary>
        /// A unique ID for the split
        /// </summary>
        public RecordIdentifier SplitID { get; set; }

        protected void Populate(HospitalityTransaction trans)
        {
            base.Populate(trans);

            trans.TransactionID = (RecordIdentifier)TransactionID.Clone();
            trans.StoreID = (RecordIdentifier)StoreID.Clone();
            trans.TerminalID = (RecordIdentifier)TerminalID.Clone();
            trans.TableID = (RecordIdentifier)TableID.Clone();
            trans.Guest = (RecordIdentifier)Guest.Clone();
            trans.HospitalityType = (RecordIdentifier)HospitalityType.Clone();
            trans.CreatedBy = CreatedBy;
            trans.Transaction = Transaction.Clone() as RetailTransaction;
            trans.SplitID = (RecordIdentifier)SplitID.Clone();
        }

        /// <summary>
        /// Populates the specified transaction.
        /// </summary>
        /// <param name="transaction">The transaction</param>
        public void SetFromRetailTransaction(IRetailTransaction transaction)
        {
            Transaction = transaction;
            TransactionID = transaction.TransactionId;
            StoreID = transaction.StoreId;
            TerminalID = transaction.TerminalId;
            TableID = transaction.Hospitality.TableInformation.TableID.ToString();
            HospitalityType = transaction.Hospitality.ActiveHospitalitySalesType;
            SplitID = transaction.SplitID;
        }
    }
}
