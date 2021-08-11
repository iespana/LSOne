namespace LSOne.DataLayer.BusinessObjects.Enums
{    
    /// <summary>
    /// The different types of loyalty points calculations
    /// </summary>
    public enum LoyaltyPointTypeBase
    {        
        Item = 0,     
        RetailGroup = 1,        
        RetailDepartment = 2,        
        Promotion = 3,        
        Discount = 4,        
        Tender = 5, 
        SpecialGroup = 6
    }

    /// <summary>
    /// The base loyalty point calculations types
    /// </summary>
    public enum CalculationTypeBase
    {        
        Amounts = 0,        
        Quantity = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public enum LoyaltyTenderTypeBase
    {
        AsCardTender = 0,
        AsContactTender = 1,
        NoTender = 2,
        Blocked = 3
    }

    public enum LoyaltyPointsRelation
    {
        Header = 0,
        Item = 1,
        Tender = 2,
        Discount = 3
    }

    public enum UseDialogEnum
    {
        GetInformation = 0,
        CardRequest = 1,
        PointsPayment = 2,
        PointsDiscount = 3
    }
    
}
