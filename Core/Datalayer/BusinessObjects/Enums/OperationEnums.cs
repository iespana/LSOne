namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// Item Operations 1..
    /// Tender Operations 2..
    /// Discount Operations 3..
    /// Transaction Operations 5..
    /// Customer 6..
    /// Operator 7..
    /// Inventory 8..
    /// Internal 9..
    /// No Sale 1000
    /// EOD - EOS Functionality 10..
    /// Special Operations 1100
    /// Cash Management 12..
    /// Hospitality 13..
    /// US Tax 14..
    /// </summary>
    public enum POSOperations
    {
        # region POS Operations

        NoOperation = 0,        
        ItemSale = 100,
        PriceCheck = 101,
        VoidItem = 102,
        ItemComment = 103,
        PriceOverride = 104,
        SetQty = 105,
        ClearQty = 106,
        ClearPriceOverride = 107,        
        ItemSearch = 108,
        ReturnItem = 109,
        WeighItem = 110,
        FuelItemSale = 111,
        LinkedItemsAdd = 112,
        SetDimensions = 113,
        ReturnTransaction = 114,
        ShowJournal = 115,
        SetCostPrice = 116,
        LoyaltyRequest = 117,
        ProcessInput = 118,
        SalespersonBarcode = 119,
        SalespersonCard = 120,
        SalespersonClear = 121,
        InvoiceComment = 122,
        ChangeUnitOfMeasure = 123,
        ItemSaleMulti = 124,
        RFIDSale = 125,
        SalesHistory = 126,
        DiscountVoucherAdd = 127,
        DiscountVoucherRemove = 128,
        InfocodesOnRequest = 129,
        ClearItemComments = 130,
        ChangeItemComments = 131,
        GetLoyaltycardInfo = 132,
        LoyaltyPointDiscount = 133,
        JournalLogExport = 134,
        SetReasonCode = 135,
        // Tender
        PayCash = 200,
        PayCard = 201,
        PayCustomerAccount = 202,
        PayCurrency = 203,
        PayCheque = 204,
        PayTransaction = 205,
        PayCashQuick = 206,
        PayLoyalty = 207,
        PayCorporateCard = 208,        
        ChangeBack = 210,
        VoidPayment = 211,
        FleetCardInfo = 212,
        PayCreditMemo = 213,
        PayGiftCertificate = 214,
        AuthorizeCard = 215,
        AuthorizeCardQuick = 216,
        
        // Discount
        LineDiscountAmount = 300,
        LineDiscountPercent = 301,
        TotalDiscountAmount = 302,
        TotalDiscountPercent = 303,
        ManuallyTriggerPeriodicDisount = 304,
        ClearManuallyTriggeredPeriodicDiscount = 305,
        ClearCoupon = 306,
        ClearLineDiscount = 307,
        ClearAllDiscounts = 308,
        ClearTotalDiscount = 309,

        //Menu operations
        PopupMenu = 400,
        SubMenu = 401,
        
        // Transaction
        VoidTransaction = 500,
        TransactionComment = 501,
        SalesPerson = 502,
        SuspendTransaction = 503,
        RecallTransaction = 504,
        RecallUnconcludedTransaction = 505,
        CardSwipe = 506,
        PharmacyPrescriptionAdd = 507,
        PharmacyPrescriptionCancel = 508,
        PharmacyPrescriptionPaid = 509,
        PharmacyPrescriptions = 510,
        IssueCreditMemo = 511,
        IssueGiftCertificate = 512,
        DisplayTotal = 513,
        CreditMemoBalance = 514,
        SalesOrder = 515,
        SalesInvoice = 516,
        IncomeAccounts = 517,
        ExpenseAccounts = 518,
        ReturnIncomeAccounts = 519,
        ReturnExpenseAccounts = 520,
        GetGiftCardBalance = 521,
        
        // Cash changer
        CashChangerRegisterAmount = 550,
        CashChangerReset = 551,
        CashChangerRegret = 552,
        CashChangerExit = 553,
        CashChangerChange = 554,
        CashChangerLogin = 555,
        CashChangerInit = 556,            
        
        // Customer
        Customer = 600,
        CustomerSearch = 602,
        CustomerClear = 603,
        CustomerCard = 604,
        CustomerTransactions = 609,
        CustomerTransactionsReport = 610,
        CustomerBalanceReport = 611,
        CustomerAdd = 612,
        CustomerBarcode = 613,
        CustomerOrders = 614,
        Quotes = 615,
        RecallCustomerOrders = 616,
        RecallQuotes = 617,
        CustomerEdit = 618,

        // Operator
        LogOn = 700,
        LogOff = 701,
        ChangeUser = 702,
        LockTerminal = 703,
        LogOffForce = 704,
        EmployeeCard = 705,
        EmployeeBarcode = 706,

        // Inventory                
        NegativeAdjustment = 800,
        InventoryLookup = 801,
        TransferRequest = 802,
        TransferOrder = 803,
        
        // Internal
        ApplicationExit = 900,
        InitializeZReport = 902,        
        PrintX = 904,
        PrintZ = 905,
        PrintTaxFree = 906,
        PrintPreviousSlip = 907,
        PrintPreviousInvoice = 908,
        UploadPrinterLogo = 909,
        RestartComputer = 910,
        ShutDownComputer = 911,
        DesignModeEnable = 912,
        DesignModeDisable = 913,
        MinimizePOSWindow = 914, 
        BlankOperation = 915,
        RunExternalCommand = 916,
        ExecutePOSPlugin = 917,
        PrintItemSaleReport = 918,
        ActivateTrainingMode = 919,
        PrintFiscalInfoSlip = 920,

        // No Sale 
        OpenDrawer = 1000,
        
        // EOD - EOS Functionality
        EndOfDay = 1050,
        EndOfShift = 1051,
        TenderDeclaration = 1052,
        
        // Special operations
        CustomerAccountDeposit = 1100,
        AddCustomerToLoyaltyCard = 1101,
        
        // Cash management
        DeclareStartAmount = 1200,
        FloatEntry = 1201,
        TenderRemoval = 1210,
        SafeDrop = 1211,
        BankDrop = 1212,
        SafeDropReversal = 1213,
        BankDropReversal = 1214,
        
        //Hospitality
        SplitBill = 1300,
        ShowHospitality = 1301,
        PrintHospitalityMenuType = 1302,
        SetHospitalityMenuType = 1303,
        ChangeHospitalityMenuType = 1304,
        BumpOrder = 1305,
        
        // US Tax functionality
        TaxExemptTransaction = 1400,
        ClearTransactionTaxExemption = 1401,
        InfocodeTaxGroupChange = 1405,
        OpenMenu = 1500,
        RefreshDisplayStations = 1501,

        TaxRefund = 1600,
        StartOfDay = 1610,

        ScanQR = 1700,
	
	//DDOperation   
        RunJob = 1800,

        ReprintReceipt = 1900,
        EmailReceipt = 1901,
        KeepTime = 2000,

        //OMNI operations
        OmniChangeStore = 2100,
        OmniUpdateMasterData = 2101,
        OmniFullMasterDataUpdate = 2102,
        OmniCustomerVendorFullUpdate = 2103,
        OmniItemLookup = 2104,
        OmniCustomerLookup = 2105,
        OmniVendorLookup = 2106,
        OmniStockCountingTemplate = 2107,
        OmniGoodsReceiving = 2108,
        OmniPostGoodsReceiving = 2109,
        OmniPurchaseWorksheet = 2110,
        OmniAddToPurchaseWorksheet = 2111,
        OmniPostPurchaseWorksheet = 2112,
        OmniUnsentTemplates = 2113,
        OmniUnsentDocuments = 2114,
        OmniReceiving = 2115,
        OmniPicking = 2116,
        OmniWhoAmI = 2117,
        OmniPurchaseOrderTemplate = 2118,
        OmniTransferTemplate = 2119,
        #endregion
    }

}
