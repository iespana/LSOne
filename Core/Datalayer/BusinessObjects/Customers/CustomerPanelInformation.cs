using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LSOne.DataLayer.BusinessObjects.Customers
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(CustomerPanelLoyaltyCard))]
    public class CustomerPanelInformation
    {
        public CustomerPanelInformation(RecordIdentifier customerID)
        {
            CustomerID = customerID;
            LastTransactionDate = Date.Empty;
            LoyaltyCards = new List<CustomerPanelLoyaltyCard>();
        }

        /// <summary>
        /// ID of the customer
        /// </summary>
        [DataMember]
        public RecordIdentifier CustomerID { get; set; }

        /// <summary>
        /// Date of the last transaction made by the customer
        /// </summary>
        [DataMember]
        public Date LastTransactionDate { get; set; }
        /// <summary>
        /// Total of the last transaction made by the customer
        /// </summary>
        [DataMember]
        public decimal LastTransactionTotal { get; set; }

        /// <summary>
        /// List of all active loyalty cards belonging to the customer
        /// </summary>
        [DataMember]
        public List<CustomerPanelLoyaltyCard> LoyaltyCards { get; set; }

        /// <summary>
        /// Maximum credit that this customer is allowed to have
        /// </summary>
        [DataMember]
        public decimal MaxCredit { get; set; }
        /// <summary>
        /// Current credit balance for this customer
        /// </summary>
        [DataMember]
        public decimal Balance { get; set; }

        /// <summary>
        /// Total of the last unfinished customer order for this customer
        /// </summary>
        [DataMember]
        public decimal LastCustomerOrderTotal { get; set; }
        /// <summary>
        /// Total amount paid on the last unfinished customer order for this customer
        /// </summary>
        [DataMember]
        public decimal LastCustomerOrderPaidDeposit { get; set; }

        /// <summary>
        /// True if this customer has concluded other transactions
        /// </summary>
        [DataMember]
        public bool HasTransaction { get; set; }
        /// <summary>
        /// True if the customer has any loyalty card
        /// </summary>
        public bool HasLoyaltyCard { get { return LoyaltyCards.Count > 0; } }
        /// <summary>
        /// True if the customer has any unfinished customer order
        /// </summary>
        [DataMember]
        public bool HasCustomerOrder { get; set; }
    }

    [Serializable]
    [DataContract]
    public class CustomerPanelLoyaltyCard
    {
        public CustomerPanelLoyaltyCard()
        {

        }

        /// <summary>
        /// Loyalty card number
        /// </summary>
        [DataMember]
        public string CardNumber { get; set; }
        /// <summary>
        /// Remaining points on the loyalty card
        /// </summary>
        [DataMember]
        public decimal RemainingPoints { get; set; }
        /// <summary>
        /// Remaining value of the loyalty card (points converted into currency based on the card's loyalty scheme)
        /// </summary>
        [DataMember]
        public decimal RemainingValue { get; set; }
        /// <summary>
        /// Percentage of transaction total that the customer is allowed to pay using this loyalty card
        /// </summary>
        [DataMember]
        public int UsageLimit { get; set; }
    }
}
