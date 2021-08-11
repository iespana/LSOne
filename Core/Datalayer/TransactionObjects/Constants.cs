namespace LSOne.DataLayer.TransactionObjects
{
    public static class Constants
    {
        /// <summary>
        /// Each RetailTransaction.SaleLineItem has a property called PaymentIndex which stores the payment tender line index used to pay the current item
        /// if a payment with limitation is used. In order to know what sale line items can be paid with the current restricted payment, 
        /// all eligible sale line items to be paid with the current payment are initially set to -999.
        /// </summary>
        public const int PaymentIndexToBeUpdated = -999;
        /// <summary>
        /// Each RetailTransaction.SaleLineItem has a property called PaymentIndex which stores the payment tender line index used to pay the current item
        /// if a payment with limitation is used. The initial value of this property is -1. Also, if the restriction are removed or cancelled, its value is reset to -1.
        /// This means that no payment with restriction was used to pay the current line.
        /// </summary>
        public const int EmptyPaymentIndex = -1;
    }
}