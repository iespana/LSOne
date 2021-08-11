namespace LSOne.DataLayer.BusinessObjects.TouchButtons
{
    public enum Position
    {
        Center,
        Left,
        Right,
        Top,
        Bottom,
        Fill
    }

    public enum MenuTypeEnum
    {
        Hospitality = 0,
        POSButtonGrid = 1,
        KitchenDisplay = 2
    }

    public enum DeviceTypeEnum
    {
        POS = 0,
        MobileInventory = 1
    }

    /// <summary>
    /// Used for PosOperation to indicate if the operation requires a lookup for a parameter value
    /// </summary>
    public enum LookupTypeEnum
    {
        /// <summary>
        /// No parameter lookup
        /// </summary>
        None = 0,

        /// <summary>
        /// Look up a retail item ID or barcode
        /// </summary>
        RetailItems = 1,

        /// <summary>
        /// Look up payment types for the current store
        /// </summary>
        StorePaymentTypes = 2,

        /// <summary>
        /// Looks up POS button grid menus
        /// </summary>
        PosMenu = 3,

        /// <summary>
        /// Looks up Infocodes that have tax codes attached. Used to change tax group on the transaction
        /// </summary>
        TaxGroupInfocodes = 4,

        /// <summary>
        /// Looks up a combination of POS button grid menu and button grid (1 - 5)
        /// </summary>
        PosMenuAndButtonGrid = 5,

        /// <summary>
        /// Looks up suspended transaction types
        /// </summary>
        SuspendedTransactionTypes = 6,

        /// <summary>
        /// Looks up pre defined blank operations
        /// </summary>
        BlankOperations = 7,

        /// <summary>
        /// Looks up income accounts
        /// </summary>
        IncomeAccounts = 8,

        /// <summary>
        /// Looks up expense accounts
        /// </summary>
        ExpenseAccounts = 9,

        /// <summary>
        /// Provides text input for parameter
        /// </summary>
        TextInput = 10,

        /// <summary>
        /// Provides numeric input for parameter
        /// </summary>
        NumericInput = 11,

        /// <summary>
        /// Looks up a combination of store payment types and a numeric input parameter for amount
        /// </summary>
        StorePaymentTypeAndAmount = 12,

        /// <summary>
        /// Provides numeric input for parameter, and a lookup into cash denominations to select known amounts
        /// for the company or store currency depending on if you are using the Site Manager or the POS.
        /// Site Manager uses company currency, and the POS uses store currency.
        /// </summary>
        Amount = 13,

        /// <summary>
        /// Provides a dropdown for search mode and a lookup editor for retail group selection
        /// </summary>
        ItemSearch = 14,

        /// <summary>
        /// Looks up periodic discounts that are set to be triggered manually
        /// </summary>
        ManuallyTriggerPeriodicDiscount = 15,

        /// <summary>
        /// Provides a dropdown for sales person selection and a yes/no option to limit the selection to staff by store
        /// </summary>
        SalesPerson = 16,

        /// <summary>
        /// Looks up a Job to run 
        /// </summary>
        LookupJob = 18,

        /// <summary>
        /// Provides a boolean yes/no option. Each operation that uses this option must provide a localized description for the option.
        /// </summary>
        Boolean = 19,

        /// <summary>
        /// Provides options for reprinting such as tax free, gift receipt etc
        /// </summary>
        ReprintReceipt = 20,

        /// <summary>
        /// An option of "Last sale" or to enter a specific receipt ID
        /// </summary>
        LastSaleOrReceiptID = 21,

        /// <summary>
        /// Provides dropdowns to set the behaviour of the reason codes when returning items or transactions
        /// </summary>
        ReasonCode = 22,

        /// <summary>
        /// Provides drop downs to set the behaviour of the Infocode on request operation
        /// </summary>
        InfocodeOnRequest = 23,

        /// <summary>
        /// Provides a drop down to set the grouping depth when printings a sales report
        /// </summary>
        PrintGroup = 24,

        /// <summary>
        /// Provides a drop down to select ether incoming or outgoing transfer requests
        /// </summary>
        TransferRequests = 25,

        /// <summary>
        /// Provides a drop down to select ether incoming or outgoing transfer orders
        /// </summary>
        TransferOrders = 26,

        /// <summary>
        /// Provides a drop down to select one of the menu types available for the current store
        /// </summary>
        MenuTypes = 27

    }

    public enum ButtonGridsEnum
    {
        ButtonGrid1 = 1,
        ButtonGrid2 = 2,
        ButtonGrid3 = 3,
        ButtonGrid4 = 4,
        ButtonGrid5 = 5
    }

    public enum OperationTypeEnum
    {
        All = 0,
        SystemOperation = 1,
        POSOperation = 2,
        OMNIInventoryOperation = 3
    }
}
