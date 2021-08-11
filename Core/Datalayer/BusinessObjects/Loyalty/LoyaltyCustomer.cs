using System;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Loyalty
{
    /// <summary>
    /// A business object that holds information about the loyalty Customer.   
    /// </summary>
    public class LoyaltyCustomer : DataEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoyaltyCustomer" /> class.
        /// </summary>
        public LoyaltyCustomer()
        {
            CustomerID = RecordIdentifier.Empty;
        }

        /// <summary>
        /// Gets or sets the customer ID.
        /// </summary>
        /// <value>The customer ID.</value>
        public RecordIdentifier CustomerID { get; set; }

        [RecordIdentifierValidation(20, Depth = 1)]
        public override RecordIdentifier ID
        {
            get
            {
                return CustomerID;
            }
            set
            {
                CustomerID = value;
            }
        }


        /// <summary>
        /// Gets or sets the customer Name.
        /// </summary>
        /// <value>The Name.</value>
        public string Name { get; set; }


        public enum ErrorCodes
        {
            NoConnectionTried = -2,
            UnknownError = -1,
            NoErrors = 0,
            ErrLoyaltyCardNotFound = 1, 
            ErrLoyaltyCardBlocked = 2, 
            ErrLoyaltyCardIsNoTenderCard = 3, 
            ErrInsertUpdate = 4, 
            ErrNotEnoughLoyaltyPoints = 5, 
            ErrClientNotLoyaltyCustomer = 6, 
            ErrLoyaltyCardAlreadyAssigned = 7, 
            ErrLoyaltyCustomerNotFound = 8, 
            ErrLoyaltyIsNotActivated = 9,
            CouldNotConnectToSiteService = 10,
            ErrCustomerAlreadyAssignedToCard = 11
        }

        public static string AsString(ErrorCodes Value)
        {
            switch (Value)
            {
                case ErrorCodes.ErrLoyaltyCardNotFound: return Properties.Resources.ErrLoyaltyCardNotFound;
                case ErrorCodes.ErrLoyaltyCardBlocked: return Properties.Resources.ErrLoyaltyCardBlocked;
                case ErrorCodes.ErrLoyaltyCardIsNoTenderCard: return Properties.Resources.ErrLoyaltyCardIsNoTenderCard;
                case ErrorCodes.ErrInsertUpdate: return Properties.Resources.ErrInsertUpdate;
                case ErrorCodes.ErrNotEnoughLoyaltyPoints: return Properties.Resources.ErrNotEnoughLoyaltyPoints;
                case ErrorCodes.ErrClientNotLoyaltyCustomer: return Properties.Resources.ErrClientNotLoyaltyCustomer;
                case ErrorCodes.ErrLoyaltyCardAlreadyAssigned: return Properties.Resources.ErrLoyaltyCardAlreadyAssigned;
                case ErrorCodes.ErrLoyaltyCustomerNotFound: return Properties.Resources.ErrLoyaltyCustomerNotFound;
                case ErrorCodes.ErrLoyaltyIsNotActivated: return Properties.Resources.ErrLoyaltyIsNotActivated;
                case ErrorCodes.ErrCustomerAlreadyAssignedToCard: return Properties.Resources.ErrCustomerAlreadyAssignedToCard;
                case ErrorCodes.CouldNotConnectToSiteService:
                    return Properties.Resources.CouldNotConnectToSiteService;
                default: return Enum.GetName(typeof(ErrorCodes), Value);
            }
        }

    }
}
