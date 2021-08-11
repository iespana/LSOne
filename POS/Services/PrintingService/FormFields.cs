namespace LSOne.Services
{
    /// <summary>
    /// Form serialization uses these enum fields
    /// Older forms serialized a string key, which was then used while printing. Those older codes where mapped to this
    /// </summary>
    public enum FormFields
    {
        // Credit memo fields
        CreditMemoAmount = 100,
        CreditMemoNumber = 105,
        CreditMemoSerialNumber = 110,
        CreditMemoBarCode = 115,

        // Gift card fields
        GiftCardAmount = 150,
        GiftCardNumber = 155,
        GiftCardSerialNumber = 160,
        GiftCardBarCode = 165,
        GiftCardBalance = 170,

        // Customer fields
        CustomerAccountNumber = 200,
        CustomerAddress = 205,
        CustomerBillingAddress = 206,
        CustomerShippingAddress = 207,
        CustomerAmount = 210,
        CustomerName = 215,
        CustomerTaxOffice = 220,
        CustomerVAT = 225,
        CustomerEmail = 226,

        CustomerAddressLine1 = 230,
        CustomerAddressLine2 = 231,
        CustomerAddressZip = 232,
        CustomerAddressCity = 233,
        CustomerAddressState = 234,
        CustomerAddressCounty = 235,
        CustomerAddressCountry = 236,

        CustomerAddressBillingLine1 = 240,
        CustomerAddressBillingLine2 = 241,
        CustomerAddressBillingZip = 242,
        CustomerAddressBillingCity = 243,
        CustomerAddressBillingState = 244,
        CustomerAddressBillingCounty = 245,
        CustomerAddressBillingCountry = 246,

        CustomerAddressShippingLine1 = 250,
        CustomerAddressShippingLine2 = 251,
        CustomerAddressShippingZip = 252,
        CustomerAddressShippingCity = 253,
        CustomerAddressShippingState = 254,
        CustomerAddressShippingCounty = 255,
        CustomerAddressShippingCountry = 256,

        CustomerText = 260,
        CustomerBalance = 270,
        CustomerCreditLimit = 275,

        // Discount fields
        LineDiscountName = 300,
        SumLineDiscount = 305,
        SumTotalDiscount = 310,
        TotalDiscountName = 315,
        TotalTransactionDiscountName = 320,
        SumTotalTransactionDiscount = 325,

        // EFT fields
        AcquirerName = 400,
        AuthSourceCode = 405,
        AuthorizationCode = 410,
        CardAmount = 415,
        CardAuthNumber = 420,
        CardExpireDate = 425,
        CardTransNumber = 430,
        CardType = 435,
        CardNumber = 440,
        CardNumberPartlyHidden = 445,
        EFTInfoMessage = 450,
        EFTMessage = 455,
        EFTStoreCode = 460,
        EFTTerminalID = 465,
        EFTTerminalNumber = 470,
        EntrySourceCode = 475,
        EuroAuthCode = 480,
        SequenceCode = 485,
        VisaAuthCode = 490,

        // Forecourt fields
        OiltaxAmount = 500,
        OiltaxText = 505,

        // Item fields
        AllItemComments = 600,
        NumberOfSaleLines = 601,
        NumberOfSaleItems = 602,

        // Loyalty fields
        AccumulatedLoyaltyPoints = 700,
        IssuedLoyaltyPoints = 705,
        LoyaltyCardNumber = 710,
        LoyaltyText = 715,
        UsedLoyaltyPoints = 720,

        // Float entry
        PreviouslyTendered = 790,
        AddedAmount = 791,
        Reason = 792,

        // Other fields
        BankDropBagID = 800,
        Date = 805,
        ExtraInfo1 = 810,
        ExtraInfo2 = 811,
        ExtraInfo3 = 812,
        ExtraInfo4 = 813,
        ExtraInfo5 = 814,
        ExtraInfo6 = 815,
        ExtraInfo7 = 816,
        ExtraInfo8 = 817,
        ExtraInfo9 = 818,
        ExtraInfo10 = 819,
        ExtraInfo11 = 820,
        ExtraInfo12 = 821,
        ExtraInfo13 = 822,
        ExtraInfo14 = 823,
        ExtraInfo15 = 824,
        ExtraInfo16 = 825,
        ExtraInfo17 = 826,
        ExtraInfo18 = 827,
        ExtraInfo19 = 828,
        ExtraInfo20 = 829,
        Logo = 835,
        ReprintMessage = 840,
        Text = 845,
        Time12H = 850,
        Time24H = 855,

        // Pharmacy fields
        BatchCode = 900,

        // Return sale fields
        ReturnSaleText = 990,
        ReturnSaleTextCopy = 991,

        // Sale fields
        CurrencySymbol = 1000,
        InvoiceComment = 1005,
        TransactionComment = 1006,
        MarkupAmount = 1010,
        MarkupDescription = 1015,
        OperatorID = 1020,
        OperatorName = 1025,
        OperatorNameOnReceipt = 1030,
        PriceWithoutTax = 1035,
        ReceiptNumber = 1040,
        ReceiptNumberBarcode = 1045,
        SalespersonName = 1050,
        SalespersonNameOnReceipt = 1055,
        SaleText = 1057,
        SaleTextCopy = 1058,
        TaxAmount = 1060,
        TaxGroup = 1065,
        TaxID = 1070,
        TaxPercentage = 1075,
        TaxTotal = 1080,
        Total = 1085,
        TotalWithoutTax = 1090,
        TransactionDescription = 1095,
        TransactionNumber = 1100,
        TransactionSequence = 1101,
		TotalPayment = 1102,

        // Tender fields
        AllTenderComments = 1103,
        TenderAmount = 1105,
        TenderComment = 1110,
        TenderName = 1115,
        TenderRounding = 1120,
        TenderRoundingText = 1125,
        TenderDate = 1130,
        TenderDetails = 1135,

        //Tender Removal fields
        TenderRemovalAmount = 1150,
        TenderRemovalReason = 1151,

        // Store/Terminal fields
        StoreAddress = 1200,
        StoreID = 1205,
        StoreName = 1210,
        TerminalID = 1215,
        StoreFormInfo1 = 1220,
        StoreFormInfo2 = 1221,
        StoreFormInfo3 = 1222,
        StoreFormInfo4 = 1223,
        TerminalFormInfo1 = 1225,
        TerminalFormInfo2 = 1226,
        TerminalFormInfo3 = 1227,
        TerminalFormInfo4 = 1228,

        // Discount fields
        LineDiscountAmount = 1300,
        LineDiscountPercent = 1305,
        LineDiscountText = 1310,
        PeriodicDiscountAmount = 1315,
        PeriodicDiscountName = 1320,
        PeriodicDiscountPercent = 1325,
        PeriodicDiscountText = 1330,
        TotalDiscountAmount = 1335,
        TotalDiscountPercent = 1340,
        TotalDiscountText = 1345,

        // Item fields
        //DimensionColorID = 1400,
        //DimensionColorValue = 1405,
        //DimensionSizeID = 1410,
        //DimensionSizeValue = 1415,
        //DimensionStyleID = 1420,
        //DimensionStyleValue = 1425,
        //DimensionID = 1426,
        //DimensionText = 1430,
        VariantDescription = 1400,
        ItemBarcode = 1435,
        ItemComment = 1440,
        ItemCommentText = 1445,
        ItemGroup = 1450,
        ItemID = 1455,
        ItemName = 1460,
        ItemNameAlias = 1465,
        ItemUnitID = 1470,
        ItemUnitIDName = 1475,
        RFID = 1480,
        SerialID = 1485,
        ReturnReceiptNumber = 1486,
        ExtendedDescription = 1487,

        // Pharmacy fields
        BatchExpDate = 1500,
        BatchID = 1505,
        BatchText = 1510,
        PharmacyDosageQty = 1515,
        PharmacyDosageStrength = 1520,
        PharmacyDosageStrengthUnit = 1525,
        PharmacyDosageType = 1530,
        PharmacyDosageUnitQty = 1535,
        PharmacyPrescriptionNumber = 1540,
        PharmacyText = 1545,

        // Sale fields
        ManuallyEnteredWeight = 1600,
        Qty = 1605,
        SalespersonID = 1610,
        ScaleInfo = 1615,
        TaxPercent = 1620,
        TotalPrice = 1625,
        TotalPriceWithoutTax = 1630,
        UnitPrice = 1635,
        UnitPriceWithoutTax = 1640,
        NetTotalPrice = 1645,
        NetTotalPriceWithoutTax = 1650,

        // Suspended transaction fields
        SuspendedDestination = 1660,
        SuspendedAnswer = 1661,
        SuspendedIdBarcode = 1662,

        //Customer orders
        OrderType = 1700,
        Reference = 1701,
        ReferenceBarcode = 1702,
        Delivery = 1703,
        Source = 1704,
        DeliveryLocation = 1705,
        ExpirationDate = 1706,
        CustomerOrderComment = 1707,
        DepositAmount = 1708,
        RemainingAmount = 1709,

        Ordered = 1750,
        OrderedText = 1751,
        Received = 1752,
        ReceivedText = 1753,
        ForDelivery = 1754,
        ForDeliveryText = 1755,
        FullyReceivedText = 1756,
        DepositsPaid = 1757,
        DepositsPaidText = 1758,

        //Hospitality 
        TableDescription = 1770,
        OrderNumber = 1771,
        MenuTypes = 1772,
        ChangedItem = 1773,
        VoidedItem = 1774,
        RestaurantStation = 1775,

        // Fiscalization fields
        TransactionFiscalSignature = 1800,
        FiscalInfo1 = 1801,
        FiscalInfo2 = 1802,
        FiscalInfo3 = 1803,
        FiscalInfo4 = 1804,


        // Partner customization should come here and have a number greater than 10000
        // PartnerField = 10000
    }
}
