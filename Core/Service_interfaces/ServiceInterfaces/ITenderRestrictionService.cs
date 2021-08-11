using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    public interface ITenderRestrictionService : IService
    {
        /// <summary>
        /// Displays a dialog with the items that have been excluded from payment due to limitations
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">The current POS settings</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="paymentMethod">The payment method that is to pay for the items</param>
        /// <param name="payableAmount">The total payable amount for the items that fall within the limitations</param>
        /// <returns><see cref="TenderRestrictionResult"/></returns>
        TenderRestrictionResult DisplayTenderRestrictionInformation(IConnectionManager entry, ISettings settings, IPosTransaction retailTransaction, StorePaymentMethod paymentMethod, ref decimal payableAmount);

        /// <summary>
        /// Calculates the amount that can be paid when payment limitations have been taken into account
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">The current POS settings</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="paymentMethod">The payment method that is to pay for the items</param>
        /// <param name="payableAmount">The total payable amount for the items that fall within the limitations</param>
        /// <returns><see cref="TenderRestrictionResult"/></returns>
        TenderRestrictionResult GetTenderRestrictionAmount(IConnectionManager entry, ISettings settings, IPosTransaction retailTransaction, StorePaymentMethod paymentMethod, ref decimal payableAmount);

        /// <summary>
        /// Calculates the amount that can be paid when payment limitations have been taken into account
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">The current POS settings</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="paymentMethod">The payment method that is to pay for the items</param>
        /// <param name="payableAmount">The total payable amount for the items that fall within the limitations</param>
        /// <returns><see cref="TenderRestrictionResult"/></returns>
        TenderRestrictionResult GetUnconfirmedTenderRestrictionAmount(IConnectionManager entry, ISettings settings, IPosTransaction retailTransaction, StorePaymentMethod paymentMethod, ref decimal payableAmount);

        /// <summary>
        /// Deletes restrictions from the tendertype.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        void ClearTenderRestriction(IConnectionManager entry, IPosTransaction retailTransaction);

        /// <summary>
        /// Clears all restrictions from items that were paid for for a specific payment line. If payment line is set to -999 then all limitations that have been set 
        /// but have no payment index yet.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">The current POS settings</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="paymentMethod">The payment method that is to pay for the items</param>
        /// <param name="paymentIndex">The number of the payment line that is to be cleared</param>
        void CancelTenderRestriction(IConnectionManager entry, ISettings settings, IPosTransaction retailTransaction, StorePaymentMethod paymentMethod, int paymentIndex);

        /// <summary>
        /// Clears all restrictions from items that have been prepared but not yet paid for f.ex. when the user clicks Cancel in the payment dialog after restrictions have been displayed
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">The current POS settings</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="paymentMethod">The payment method that is to pay for the items</param>
        void CancelUnconfirmedTenderRestriction(IConnectionManager entry, ISettings settings, IPosTransaction retailTransaction, StorePaymentMethod paymentMethod);

        /// <summary>
        /// Updates the payment index for from items that were paid for using the current payment line
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">The current POS settings</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="paymentMethod">The payment method that is to pay for the items</param>
        /// <param name="paymentIndex">The number of the payment line that is to be cleared</param>
        void UpdatePaymentIndex(IConnectionManager entry, ISettings settings, IPosTransaction retailTransaction, StorePaymentMethod paymentMethod, int paymentIndex);

        /// <summary>
        /// Goes through the limitation list (items/groups/departments) and checks if the item that is being looked at can be included in the payment
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="paymentMethod">The payment method that is to pay for the items</param>
        /// <param name="limitationList">The list of limitations that have been configured</param>
        /// <param name="item">The item that is being checked</param>
        /// <returns></returns>
        PaymentMethodLimitation GetRestrictionForItem(IConnectionManager entry, StorePaymentMethod paymentMethod, List<PaymentMethodLimitation> limitationList, ISaleLineItem item);

        /// <summary>
        /// Returns the text to be displayed in the HTML panel. The text contains a list with all payment types that can be used to pay for a transaction if the payment types contain limitations.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">The current POS settings</param>
        /// <param name="retailTransaction">The retail transaction for which we display the HTML panel</param>
        /// <returns></returns>
        string GetPaymentLimitationsAsHtml(IConnectionManager entry, ISettings settings, IPosTransaction retailTransaction);

        /// <summary>
        /// Returns the text to be displayed in the HTML panel. The text contains a list with all payment types that can be used to refund for a return transaction.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">The current POS settings</param>
        /// <param name="posTransaction">The return retail transaction for which we display the HTML panel</param>
        /// <returns></returns>
        string GetRefundableAmountLimitedToPaymentTypeAsHtml(IConnectionManager entry, ISettings settings, IPosTransaction posTransaction);

        /// <summary>
        /// Goes through the transaction and splits up the limited items that have been paid for with the given tender line. This will add a new sale line to the transaction
        /// which contains the remaining amount if the limited payment does not fully pay for all the applicable items on the transaction.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">The curront POS settings</param>
        /// <param name="posTransaction">The retail transaction to split the lines for</param>
        /// <param name="paymentMethod">The payment method that is to pay for the items</param>
        /// <param name="paymentIndex">The number of the payment line that is to be cleared</param>
        void SplitLines(IConnectionManager entry, ISettings settings, IPosTransaction posTransaction, StorePaymentMethod paymentMethod, int paymentIndex);
    }
}