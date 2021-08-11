namespace LSOne.DataLayer.BusinessObjects.Enums
{
    public enum FormSystemType
    {
        /// <summary>
        /// 0
        /// </summary>
        UserDefinedType = 0,
        /// <summary>
        /// 1
        /// </summary>
        Receipt = 1,
        /// <summary>
        /// 2
        /// </summary>
        CardReceiptForShop = 2,
        /// <summary>
        /// 3
        /// </summary>
        CardReceiptForCust = 3,
        /// <summary>
        /// 4
        /// </summary>
        CardReceiptForShopReturn = 4,
        /// <summary>
        /// 5
        /// </summary>
        CardReceiptForCustReturn = 5,
        /// <summary>
        /// 6
        /// </summary>
        EFTMessage = 6,
        /// <summary>
        /// 7
        /// </summary>
        CustAcntReceiptForShop = 7,
        /// <summary>
        /// 8
        /// </summary>
        CustAcntReceiptForCust = 8,
        /// <summary>
        /// 9
        /// </summary>
        CustAcntReceiptForShopReturn = 9,
        /// <summary>
        /// 10
        /// </summary>
        CustAcntReceiptForCustReturn = 10,
        /// <summary>
        /// 11
        /// </summary>
        TenderDeclaration = 11,
        /// <summary>
        /// 12
        /// </summary>
        Invoice = 12,
        /// <summary>
        /// 13
        /// </summary>
        RemoveTender = 13,
        /// <summary>
        /// 14
        /// </summary>
        CustomerAccountDeposit = 14,
        /// <summary>
        /// 15
        /// </summary>
        CreditMemo = 15,
        /// <summary>
        /// 16
        /// </summary>
        CreditMemoBalance = 16,
        /// <summary>
        /// 17
        /// </summary>
        FloatEntry = 17,
        /// <summary>
        /// 18
        /// </summary>
        SalesOrderReceipt = 18,
        /// <summary>
        /// 19
        /// </summary>
        SalesInvoiceReceipt = 19,
        /// <summary>
        /// 20
        /// </summary>
        GiftCertificate = 20,
        /// <summary>
        /// A receipt printed when a transaction is suspended
        /// </summary>
        SuspendedTransaction = 21,
        /// <summary>
        /// A receipt printed when a transaction is vioded
        /// </summary>
        VoidedTransaction = 22,
        ///  <summary>
        ///  23
        ///  </summary>
        CardInfo = 23,
        /// <summary>
        /// A receipt printed when performing safe drop
        /// </summary>
        SafeDrop = 24,
        /// <summary>
        /// A receipt printed when performing safe drop reversal
        /// </summary>
        SafeDropReversal = 25,
        /// <summary>
        /// A receipt printed when performing bank drop
        /// </summary>
        BankDrop = 26,
        /// <summary>
        /// A receipt printed when performing bank drop reversal
        /// </summary>
        BankDropReversal = 27,
        /// <summary>
        /// A receipt printed when loyalty points are used for payment.
        /// </summary>
        LoyaltyPaymentReceipt = 28,
        /// <summary>
        /// A receipt prefix printed when a transaction is suspended
        /// </summary>
        SuspendedTransactionPrefix = 29,
        /// <summary>
        /// A gift receipt used for returns of gifts
        /// </summary>
        GiftReceipt = 30,
        /// <summary>
        /// A receipt printed out when a deposit is paid to a customer order
        /// </summary>
        CustomerOrderDeposit = 31,
        /// <summary>
        /// A receipt printed for a customer order detailing the items on order and amounts paid
        /// </summary>
        CustomerOrderInformation = 32,
        /// <summary>
        /// A picking list printout for a customer order
        /// </summary>
        CustomerOrderPickingList = 33,
        /// <summary>
        /// The text for the body of the receipt email
        /// </summary>
        ReceiptEmailBody = 34,
        /// <summary>
        /// The text to be used as a subject for the receipt email
        /// </summary>
        ReceiptEmailSubject = 35,
        /// <summary>
        /// A receipt printed for a quote detailing the items included in the quote and the total
        /// </summary>
        QuoteInformation = 36,
        /// <summary>
        /// Receipt printed after the open drawer operation is concluded with any sale
        /// </summary>
        OpenDrawer = 37,
        /// <summary>
        /// Receipt printed when paying with gift cards, showing the remaining amount of the gift card
        /// </summary>
        GiftCardBalance = 38,
        /// <summary>
        /// Slip printed in the kitchen when an order is created in the hospitality functionality
        /// </summary>
        KitchenSlip = 39,
        /// <summary>
        /// Slip printed from POS containing fiscal info like terminal id and other specific fiscal info
        /// </summary>
        FiscalInfoSlip = 40,
    }
}
