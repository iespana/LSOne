namespace LSOne.DataLayer.BusinessObjects.Enums
{

    #region BarcodeType
    /// <summary>
    /// The barcode type i.e. Code39, Code128, EAN128 and etc
    /// </summary>
    public enum BarcodeType
    {
        /// <summary>
        /// NoBarcode
        /// </summary>
        NoBarcode = 0,
        /// <summary>
        /// EAN128
        /// </summary>
        EAN128 = 1,
        /// <summary>
        /// Code39
        /// </summary>
        Code39 = 2,
        /// <summary>
        /// Interleved2of5
        /// </summary>
        Interleaved2of5 = 3,
        /// <summary>
        /// Code128
        /// </summary>        
        Code128 = 4,
        /// <summary>
        /// UPCA
        /// </summary>
        UPCA = 5,
        /// <summary>
        /// UPCE
        /// </summary>
        UPCE = 6,
        /// <summary>
        /// EAN13
        /// </summary>
        EAN13 = 7,
        /// <summary>
        /// EAN8
        /// </summary>
        EAN8 = 8,
        /// <summary>
        /// PDF417
        /// </summary>
        PDF417 = 9,
        /// <summary>
        /// MaxiCode
        /// </summary>
        MaxiCode = 10,
        /// <summary>
        /// QR code
        /// </summary>
        QRCode = 11
    }
    #endregion

    #region BarcodeInternalType
    /// <summary>
    /// What type of barcode is this i.e. Item, Customer, Employee and etc.
    /// </summary>
    public enum BarcodeInternalType
    {
        /// <summary>
        /// Is the same as settingthe barcode type to Item
        /// </summary>
        None = 0,
        /// <summary>
        /// The scanned barcode is an item barcode
        /// </summary>
        Item = 1,
        /// <summary>
        /// The scanned barcode is a customer ID
        /// </summary>
        Customer = 2,
        /// <summary>
        /// The scanned barcode is an employee ID
        /// </summary>
        Employee = 3,
        /// <summary>
        /// Not supported by LS One, has to be customized. If found the POS will call function ProcessCustomizedCoupons
        /// </summary>
        Coupon = 4,
        /// <summary>
        /// Not supported by LS One, has to be customized. If found the POS will call function ProcessCustomizedBarcode
        /// </summary>
        DataEntry = 5,
        /// <summary>
        /// The scanned barcode is an employee ID and will result in the Add sales person operation to be run
        /// </summary>
        SalesPerson = 6,
        /// <summary>
        /// Not supported by LS One, has to be customized. If found the POS will call Pharmacy service
        /// </summary>
        Pharmacy = 7,
        /// <summary>
        /// Not used by LS One, has to be customized. Partners can use this for any customized barcode handling
        /// </summary>
        Customized = 8,
        /// <summary>
        /// The receipt barcode that is printed on the customer's receipt
        /// </summary>
        ReceiptBarcode = 9,
        /// <summary>
        /// The Discount barcode activates a specific periodic discount
        /// </summary>
        Discount = 10,
        /// <summary>
        /// The Credit memo barcode is scanned into the credit memo lookup dialog in the POS
        /// </summary>
        CreditMemo = 11,
        /// <summary>
        /// The gift card barcode is scanned into the gift card lookup dialog in the POS
        /// </summary>
        GiftCard = 12,
        /// <summary>
        /// The loyalty card barcode is scanned into the pay loyalty points dialog in the POS
        /// </summary>
        Loyalty = 13,
        /// <summary>
        /// Not supported by LS One, has to be customized. If found the POS will call function ProcessQRCode
        /// </summary>
        /// Since it's not supported by LS One, we give it a higher index to allow adding new barcode types without having to change this value
        QR = 100,
    }
    #endregion

    #region BarcodeSegmentType

    /// <summary>
    /// Describes what each segment of the barcode holds i.e. item id, weight, quantity, price and etc.
    /// </summary>
    public enum BarcodeSegmentType
    {
        /// <summary>
        /// The segment is the item ID
        /// </summary>
        Item = 0,
        /// <summary>
        /// The segment can be any number
        /// </summary>
        AnyNumber = 1,
        /// <summary>
        /// The segment is the check digit
        /// </summary>
        CheckDigit = 2,
        /// <summary>
        /// The segment is part of the variant; the size information
        /// </summary>
        SizeDigit = 3,
        /// <summary>
        /// The segment is part of the variant; the color information
        /// </summary>
        ColorDigit = 4,
        /// <summary>
        /// The segment is part of the variant; the style information
        /// </summary>
        StyleDigit = 5,
        /// <summary>
        /// Not supported
        /// </summary>
        EANLicenseCode = 6,
        /// <summary>
        /// The segment is the price of the item
        /// </summary>
        Price = 7,
        /// <summary>
        /// The segment is the quantity (or weight) of the item
        /// </summary>
        Quantity = 8,
        /// <summary>
        /// The segment is an employee ID
        /// </summary>
        Employee = 9,
        /// <summary>
        /// The segment is a customer ID
        /// </summary>
        Customer = 10,
        /// <summary>
        /// Not supported
        /// </summary>
        DataEntry = 11,
        /// <summary>
        /// The segment is an employee ID for sales person functionality
        /// </summary>
        SalesPerson = 12,
        /// <summary>
        /// The segment is prescription information
        /// </summary>
        Pharmacy = 13,
        /// <summary>
        /// The segment is the store ID
        /// </summary>
        Store = 14,
        /// <summary>
        /// The segment is the terminal ID
        /// </summary>
        Terminal = 15,
        /// <summary>
        /// The segment is the receipt number
        /// </summary>
        Receipt = 16
    }
    #endregion

    public enum BarcodePrintTypeEnum
    {
        Receipt = 0,
        SuspendedSale = 1,
        CustomerOrder = 2,
        CreditMemo = 3,
        GiftCard = 4
    }

    #region OPOSScanDataType

    /// <summary>
    /// Data type that is scanned.
    /// </summary>
    public enum OPOSScanDataType
    {
        /// <summary>
        /// Digits
        /// </summary>
        SCAN_SDT_UPCA = 101,
        /// <summary>
        /// Digits
        /// </summary>
        SCAN_SDT_UPCE = 102,
        /// <summary>
        /// EAN 8
        /// </summary>
        SCAN_SDT_JAN8 = 103,
        /// <summary>
        /// JAN 8 (added in 1.2)
        /// </summary>
        SCAN_SDT_EAN8 = 103,
        /// <summary>
        /// EAN 13
        /// </summary>
        SCAN_SDT_JAN13 = 104,
        /// <summary>
        /// JAN 13 (added in 1.2)
        /// </summary>
        SCAN_SDT_EAN13 = 104,
        /// <summary>
        /// (Discrete 2 of 5) Digits
        /// </summary>
        SCAN_SDT_TF = 105,
        /// <summary>
        /// (Interleaved 2 of 5) Digits
        /// </summary>
        SCAN_SDT_ITF = 106,
        /// <summary>
        /// Digits, -, $, :, /, ., +;
        /// 4 start/stop characters (a, b, c, d)
        /// </summary>
        SCAN_SDT_Codabar = 107,
        /// <summary>
        /// Alpha, Digits, Space, -, ., $, /, +, %; start/stop (*). 
        /// Also has Full ASCII feature
        /// </summary>
        SCAN_SDT_Code39 = 108,
        /// <summary>
        /// Same characters as Code 39
        /// </summary>
        SCAN_SDT_Code93 = 109,
        /// <summary>
        /// 128 data characters
        /// </summary>
        SCAN_SDT_Code128 = 110,
        /// <summary>
        /// UPC-A with supplemental barcode
        /// </summary>
        SCAN_SDT_UPCA_S = 111,
        /// <summary>
        /// UPC-E with supplemental barcode
        /// </summary>
        SCAN_SDT_UPCE_S = 112,
        /// <summary>
        /// UPC-D1
        /// </summary>
        SCAN_SDT_UPCD1 = 113,
        /// <summary>
        /// UPC-D2
        /// </summary>
        SCAN_SDT_UPCD2 = 114,
        /// <summary>
        /// UPC-D3
        /// </summary>
        SCAN_SDT_UPCD3 = 115,
        /// <summary>
        /// UPC-D4
        /// </summary>
        SCAN_SDT_UPCD4 = 116,
        /// <summary>
        /// UPC-D5
        /// </summary>
        SCAN_SDT_UPCD5 = 117,
        /// <summary>
        /// EAN 8 with supplemental barcode
        /// </summary>
        SCAN_SDT_EAN8_S = 118,
        /// <summary>
        /// EAN 13 with supplemental barcode
        /// </summary>
        SCAN_SDT_EAN13_S = 119,
        /// <summary>
        /// EAN 128
        /// </summary>
        SCAN_SDT_EAN128 = 120,
        /// <summary>
        /// OCR "A"
        /// </summary>
        SCAN_SDT_OCRA = 121,
        /// <summary>
        /// OCR "B"
        /// </summary>
        SCAN_SDT_OCRB = 122,


        // Two dimensional symbologies
        /// <summary>
        /// Two-dimensional symbology: 201 
        /// </summary>
        SCAN_SDT_PDF417 = 201,
        /// <summary>
        /// Two-dimensional symbology: 202
        /// </summary>
        SCAN_SDT_MAXICODE = 202,


        // Special cases
        /// <summary>
        /// Special case: start of Scanner-Specific bar
        /// </summary>
        SCAN_SDT_OTHER = 501,


        //   code symbologies
        /// <summary>
        /// Special cases: cannot determine the barcode
        /// </summary>
        SCAN_SDT_UNKNOWN = 0
    }


    #endregion

    #region KeyInPrices
    /// <summary>
    /// Options for enum values:  NotMandatory = 0, MustKeyInNewPrice = 1, MustKeyInEqualHigerPrice = 2,  MustKeyInEqualLowerPrice = 3,
    /// EnteringPriceNotAllowed = 4
    /// </summary>
    public enum KeyInPrices
    {
        /// <summary>
        /// Default setting. When item is sold the user is not asked for a price
        /// </summary>
        NotMandatory = 0,
        /// <summary>
        /// The user must enter a price when the item is sold
        /// </summary>
        MustKeyInNewPrice = 1,
        /// <summary>
        /// The user must enter a higher or equal price to the one already on the item when changing the price
        /// </summary>
        MustKeyInEqualHigerPrice = 2,
        /// <summary>
        /// The user must enter a lower or equal price to the one already on the item when changing the price
        /// </summary>
        MustKeyInEqualLowerPrice = 3,
        /// <summary>
        /// The user cannot change the price on the item
        /// </summary>
        EnteringPriceNotAllowed = 4
    }


    /// <summary>
    /// Options for enum values:   NotMandatory = 0, MustKeyInQuantity = 1, EnteringQuantityNotAllowed = 2.
    /// </summary>
    public enum KeyInQuantities
    {
        /// <summary>
        /// Defautl setting. When item is sold the user is not asked for the quantity
        /// </summary>
        NotMandatory = 0,
        /// <summary>
        /// The user must enter a quantity when the item is sold
        /// </summary>
        MustKeyInQuantity = 1,
        /// <summary>
        /// The user cannot change the quantity of the item
        /// </summary>
        EnteringQuantityNotAllowed = 2
    }
    #endregion

    #region RFID
    /// <summary>
    /// Options for enum values:  None = 0, Found = 1, NotExists = 2
    /// </summary>
    public enum RFIDType
    {
        /// <summary>
        /// Tag is not an RFID
        /// </summary>
        None = 0,
        /// <summary>
        /// RFID Tag has been found
        /// </summary>
        Found = 1,
        /// <summary>
        /// RFID tag has been identified but cannot be found in data
        /// </summary>
        NotExists = 2
    }
    #endregion


}