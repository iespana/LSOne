namespace LSOne.Services.Interfaces.Enums
{
    /// <summary>
    /// Enum specifying the type of a discount. Values stored in the table PRICEDISCTABLE, column 'RELATION'.
    /// </summary>
    public enum PriceDiscType
    {
        /// <summary>
        /// currently not used
        /// </summary>
        PricePurch = 0,             //Innkaupsverð
        /// <summary>
        /// currently not used
        /// </summary>
        LineDiscPurch = 1,          //Innkaups línuafsláttarflokkur
        /// <summary>
        /// currently not used
        /// </summary>
        MultiLineDiscPurch = 2,     //Innkaups samvalsafsláttur
        /// <summary>
        /// currently not used
        /// </summary>
        EndDiscPurch = 3,           //Innkaups lokaafsláttur
        /// <summary>
        /// usual netto price. currently not used
        /// </summary>
        PriceSales = 4,             //Söluverð
        /// <summary>
        /// Predefined line discount for an item.
        /// Line Discount = 5
        /// </summary>
        LineDiscSales = 5,          //Sölu línuafsláttarflokkur
        /// <summary>
        /// Predefined discount for the multiple of a certain item.
        /// Multiline Discount = 6
        /// </summary>
        MultiLineDiscSales = 6,     //Sölu samvalsafsláttarflokkur
        /// <summary>
        /// Total discount value granted at the end of a transaction (globally) retroactive to each item.
        /// Total Discount = 7
        /// </summary>
        EndDiscSales = 7            //Sölu lokaafsláttur
    }

    /// <summary>
    /// Enables to grant a discount with regard to an item, a superordinate item group or to all items.
    /// Refers to table PriceDiscTable.ItemCode. The itemcode can have three values:
    /// </summary>
    public enum PriceDiscItemCode
    {
        /// <summary>
        /// 0 (Item)
        /// </summary>
        Table = 0,
        /// <summary>
        /// 1 (Group)
        /// </summary>
        GroupId = 1,
        /// <summary>
        /// 2, (All)
        /// </summary>
        All = 2
    }

    /// <summary>
    /// Enables to grant a discount with regard to a customer, a superordinate customer group or to all customers.
    /// Refers to table PriceDiscTable.AccountCode. The field can have three values:
    /// </summary>
    public enum PriceDiscAccountCode
    {
        /// <summary>
        /// 0 (Customer)
        /// </summary>
        Table = 0,
        /// <summary>
        /// 1 (Group)
        /// </summary>
        GroupId = 1,
        /// <summary>
        /// 2 (All customers)
        /// </summary>
        All = 2
    }

    public enum CustomerDiscountTypes
    {
        LineDiscount = 0,
        MultiLineDiscount = 1,
        TotalDiscount = 2
    }

    public enum DiscountTypes
    {
        /// <summary>
        /// 0
        /// </summary>
        Customer = 0,
        /// <summary>
        /// 1
        /// </summary>
        Employee = 1,
        /// <summary>
        /// 2
        /// </summary>
        Periodic = 2,
        /// <summary>
        /// 3
        /// </summary>
        Infocode = 3,
        /// <summary>
        /// 4
        /// </summary>
        Manual = 4,
        Coupon = 5
    }
}
