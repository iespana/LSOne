using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// The default implementation has not been finished yet. 
    /// Still, the framework is implemented and ready to handle loyalty cards.
    /// The LoyaltyItem (used as private member in an implementation class, i.e. Loyalty.cs) 
    /// is instantiated at the initialization of the RetailTransaction.
    /// The properties for the LoyaltyItem are accessible.
    /// 
    /// </summary>
    public interface ILoyaltyService : IService
    {
        /// <summary>
        /// If a previous loyalty item exists on the transaction, the system should prompt the user whether to
        /// overwrite the existing loyalty item or cancel the operation.
        /// Otherwise, a new LoyaltyItem is instantiated, the properties are set and the LoyaltyItem is assigned
        /// to the current RetailTransaction.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cardInfo">Loyalty card information to be added to the transaction</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <returns><c>true</c> if the card is not added to the transaction, <c>false</c> otherwise</returns>
        bool AddLoyaltyCardToTransaction(IConnectionManager entry, CardInfo cardInfo, IRetailTransaction retailTransaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cardInfo">Loyalty card information to be added to the transaction</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        void GetLoyaltyCardInfo(IConnectionManager entry, CardInfo cardInfo, IRetailTransaction retailTransaction);

        /// <summary>        
        /// If there is a loyalty record in the transaction, the ILoyalty.AddLoyaltyPoints() operation
        /// is called to add the calculated loyalty points to the transaction.
        /// The calculation method is not yet implemented.
        /// Afterwards, the transaction has to be updated with the latest status.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        void AddLoyaltyPoints(IConnectionManager entry, IRetailTransaction retailTransaction);

        /// <summary>        
        /// The adding of a loyalty tender line to the transaction has not yet been implemented; except for a stub in Loyalty.cs.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cardInfo">The card info.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <param name="tenderTypeID">The tender type</param>
        /// <returns>TenderLineItem.</returns>
        ITenderLineItem AddLoyaltyPayment(IConnectionManager entry, CardInfo cardInfo, decimal amount, IRetailTransaction retailTransaction, RecordIdentifier tenderTypeID);

        /// <summary>
        /// Voids the selected loyalty points tender line
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cardInfo">The card info.</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <param name="tenderLine">The tenderline being voided</param>
        void VoidLoyaltyPayment(IConnectionManager entry, CardInfo cardInfo, IRetailTransaction retailTransaction, ILoyaltyTenderLineItem tenderLine);

        /// <summary>
        /// Called when issued points are being confirmed
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <param name="loyaltyItem">The loyalty item.</param>
        void UpdateIssuedLoyaltyPoints(IConnectionManager entry, IRetailTransaction retailTransaction, ILoyaltyItem loyaltyItem);

        /// <summary>
        /// Called when issued points are being confirmed
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <param name="loyaltyItem">The loyalty item.</param>
        /// <param name="lineID">The line id of the tender line</param>
        /// <param name="cardNumber">The loyalty card number being updated</param>
        /// <param name="calculatedPoints">The points that are being issued</param>
        void UpdateIssuedLoyaltyPoints(IConnectionManager entry, IRetailTransaction retailTransaction, ILoyaltyItem loyaltyItem, decimal lineID, RecordIdentifier cardNumber, decimal calculatedPoints);

        /// <summary>
        /// Called when used points are being confirmed
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <param name="loyaltyTenderItem">The loyalty tender item.</param>
        void UpdateUsedLoyaltyPoints(IConnectionManager entry, IRetailTransaction retailTransaction, ILoyaltyTenderLineItem loyaltyTenderItem);

        /// <summary>
        /// Called when used points are being confirmed or returned
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <param name="lineID">The line id of the tender line</param>
        /// <param name="points">The points that are being used/returned</param>
        /// <param name="voided">Is the tender line voided</param>
        void UpdateUsedLoyaltyPoints(IConnectionManager entry, IRetailTransaction retailTransaction, int lineID, decimal points, bool voided);

        ///// <summary>
        ///// Called when used discount loyalty points are being confirmed or returned
        ///// </summary>
        ///// <param name="entry">The entry into the database</param>
        ///// <param name="retailTransaction">The retail transaction.</param>
        ///// <param name="loyaltyDiscountItem">The loyalty discount item that was added to the transaction</param>
        //void UpdateUsedLoyaltyPoints(IConnectionManager entry, RetailTransaction retailTransaction, LoyaltyDiscountItem loyaltyDiscountItem);

        /// <summary>
        /// Attaches a specific customer to a loyalty card
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="customer">Customer for which to add a loyalty card. Null if you want to search for a customer</param>
        void AddCustomerToLoyaltyCard(IConnectionManager entry, IPosTransaction posTransaction, Customer customer);


        /// <summary>
        /// Adds a loyalty point discount line to the sale
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction in the POS</param>
        /// <param name="tenderTypeID">The tender type selected on the operation</param>
        /// <returns>If true then the discount line was added</returns>
        bool AddLoyaltyPointsDiscount(IConnectionManager entry, IRetailTransaction retailTransaction, RecordIdentifier tenderTypeID);

        /// <summary>
        /// Returns true if the loyalty discount needs to be recalculated due to the total amount having changed
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction in the POS</param>
        /// <returns>If true then the loyalty discount needs to be recaluclated</returns>
        bool LoyaltyDiscountRecalculationNeeded(IConnectionManager entry, IRetailTransaction retailTransaction);

        /// <summary>
        /// Checks if the schem is in use by any card
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="loyaltySchemeID">The scheme</param>
        /// <returns>if it exists</returns>
        bool LoyaltyCardExistsForLoyaltyScheme(IConnectionManager entry, RecordIdentifier loyaltySchemeID);

        /// <summary>
        /// Returns true if the Site service is needed to conclude a transaction that includes loyalty information
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction</param>
        /// <returns></returns>
        bool SiteServiceIsNeeded(IConnectionManager entry, IPosTransaction transaction);

    }
}
