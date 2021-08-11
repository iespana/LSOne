namespace LSOne.DataLayer.BusinessObjects.Enums
{
    public enum NumpadEntryTypes
    {
        /// <summary>
        /// 0
        /// </summary>
        BarcodeOrQuantity = 0,
        /// <summary>
        /// 1
        /// </summary>
        Price = 1,
        /// <summary>
        /// 2
        /// </summary>
        Password = 2, // Used in Lock terminal, needs to be removed
        /// <summary>
        /// 3
        /// </summary>
        Numeric = 3,
        /// <summary>
        /// 4
        /// </summary>
        Integer = 4, // Not used
        /// <summary>
        /// 5
        /// </summary>
        CardExpireValidation = 5, 
        /// <summary>
        /// 6
        /// </summary>
        CardValidation = 6,
        /// <summary>
        /// 7
        /// </summary>
        None = 7,  // Not used
        /// <summary>
        /// 8
        /// </summary>
        Quantity = 8,
        /// <summary>
        /// Only positive integers for the input of number of units
        /// </summary>
        IntegerPositive = 9,
        /// <summary>
        /// 10
        /// </summary>
        Date = 10,  // Not used
        /// <summary>
        /// 11
        /// </summary>
        Barcode = 11,
        /// <summary>
        /// 12
        /// </summary>
        OperatorID = 12,  // Not used
        /// <summary>
        /// 13
        /// </summary>
        Amount = 13,
        /// <summary>
        /// 14
        /// </summary>
        QR = 14

    }
}
