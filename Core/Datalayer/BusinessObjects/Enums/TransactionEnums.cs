namespace LSOne.DataLayer.BusinessObjects.Enums
{
    public enum TransactionStatus
    {
        /// <summary>
        /// 0
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 1
        /// </summary>
        Voided = 1,
        /// <summary>
        /// 2
        /// </summary>
        OnHold = 2,
        /// <summary>
        /// 3
        /// </summary>
        Concluded = 3,
        /// <summary>
        /// 4
        /// </summary>
        Cancelled = 4,
        /// <summary>
        /// The transaction was created in training mode
        /// </summary>
        Training = 5,
        /// <summary>
        /// Only used for payment lines. A previously paid amount that is for informational purposes
        /// </summary>
        Redeemed = 6
    }

    /// <summary>
    /// <example><code>
    /// typeOfTransaction = TypeOfTransaction.EndOfDay;
    /// </code></example>
    /// </summary>
    public enum TypeOfTransaction
    {
        /// <summary>
        /// 0
        /// </summary>
        LogOff = 0,
        /// <summary>
        /// 1
        /// </summary>
        LogOn = 1,
        /// <summary>
        /// 2
        /// </summary>
        Sales = 2,
        /// <summary>
        /// 3
        /// </summary>
        Payment = 3,
        /// <summary>
        /// 4
        /// </summary>
        RemoveTender = 4,
        /// <summary>
        /// 5
        /// </summary>
        FloatEntry = 5,
        /// <summary>
        /// 6
        /// </summary>
        ChangeTender = 6,
        /// <summary>
        /// 7
        /// </summary>
        TenderDeclaration = 7,
        /// <summary>
        /// 8
        /// </summary>
        Voided = 8,
        /// <summary>
        /// 9
        /// </summary>
        OpenDrawer = 9,
        /// <summary>
        /// 10
        /// </summary>
        NegativeAdjustment = 10,
        /// <summary>
        /// 11
        /// </summary>
        Inventory = 11,
        /// <summary>
        /// 12
        /// </summary>
        EndOfDay = 12,
        /// <summary>
        /// 13
        /// </summary>
        EndOfShift = 13,
        /// <summary>
        /// 14
        /// </summary>
        SalesOrder = 14,
        /// <summary>
        /// 15
        /// </summary>
        SalesInvoice = 15,
        /// <summary>
        /// 16
        /// </summary>
        BankDrop = 16,
        /// <summary>
        /// 17
        /// </summary>
        SafeDrop = 17,
        /// <summary>
        /// 18
        /// </summary>
        IncomeExpense = 18,
        /// <summary>
        /// 19
        /// </summary>
        Internal = 19,
        /// <summary>
        /// 20
        /// </summary>
        SafeDropReversal = 20,
        /// <summary>
        /// 21
        /// </summary>
        BankDropReversal = 21,
        /// <summary>
        /// 22
        /// </summary>
        Deposit = 22,
        /// <summary>
        /// 23
        /// </summary>
        Log = 23
    
    }

    #region Item Class type

    ///// <summary>
    ///// Defines what type of item it is in data i.e. RboTransactionSalesTrans
    ///// </summary>
    //public enum ItemClassTypeEnum
    //{
    //    /// <summary>
    //    /// 0
    //    /// </summary>
    //    SaleLineItem = 0,
    //    /// <summary>
    //    /// 1
    //    /// </summary>
    //    DiscountVoucherItem = 1,
    //    /// <summary>
    //    /// 2
    //    /// </summary>
    //    GiftCertificateItem = 2,
    //    /// <summary>
    //    /// 3
    //    /// </summary>
    //    FuelSalesLineItem = 3,
    //    /// <summary>
    //    /// 4
    //    /// </summary>
    //    PharmacySalesLineItem = 4,
    //    /// <summary>
    //    /// 5
    //    /// </summary>
    //    CreditMemo = 5,
    //    /// <summary>
    //    /// 6
    //    /// </summary>
    //    IncomeExpenseItem = 6,
    //    /// <summary>
    //    /// 7
    //    /// </summary>
    //    SalesOrder = 7,
    //    /// <summary>
    //    /// 8
    //    /// </summary>
    //    SalesInvoice = 8

    //}
#endregion

    #region FuellingPoint type 
    
    /// <summary>
    /// Which type of Fuelling Point transaction is being used for the fuel line item
    /// </summary>
    public enum FuellingPointType
    {
        /// <summary>
        /// 0
        /// </summary>
        ForecourtManager = 0,

        /// <summary>
        /// 1
        /// </summary>
        FpTransaction = 1
    }

    #endregion
}
