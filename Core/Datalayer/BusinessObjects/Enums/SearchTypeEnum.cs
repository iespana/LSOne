namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum SearchTypeEnum
    {
        /// <summary>
        /// 0 = Retail items
        /// </summary>
        RetailItems = 0,
        /// <summary>
        /// 1 = Retail groups
        /// </summary>
        RetailGroups = 1,
        /// <summary>
        /// 2 = Special groups
        /// </summary>
        SpecialGroups = 2,
        /// <summary>
        /// 3 = Retail departments
        /// </summary>
        RetailDepartments = 3,
        /// <summary>
        /// 4 = Customers
        /// </summary>
        Customers = 4,
        /// <summary>
        /// 5 = Loyalty cards
        /// </summary>
        LoyaltyCards = 5,
        /// <summary>
        /// Retail division
        /// </summary>
        RetailDivisions = 6,
        /// <summary>
        /// Customer groups
        /// </summary>
        CustomerGroups = 7,
        /// <summary>
        /// Sales tax group
        /// </summary>
        SalesTaxGroup = 8,
        /// <summary>
        /// Price group
        /// </summary>
        PriceGroup = 9,
        /// <summary>
        /// Line discount group
        /// </summary>
        LineDiscountGroup = 10,
        /// <summary>
        /// Total discount group
        /// </summary>
        TotalDiscountGroup = 11,
        /// <summary>
        /// Blocked
        /// </summary>
        Blocked = 12,
        /// <summary>
        /// 12 = Retail groups where the ID behind is MasterID
        /// </summary>
        RetailGroupsMasterID = 12,
        /// <summary>
        /// 13 = Retail departments ID behind is MasterID
        /// </summary>
        RetailDepartmentsMasterID = 13,
        /// <summary>
        /// 14 = Retail divisions ID behind is MasterID
        /// </summary>
        RetailDivisionsMasterID = 14,
        /// <summary>
        /// 15 = Retail item ID behind is MasterID
        /// </summary>
        RetailItemsMasterID = 15,
        /// <summary>
        /// 16 = Special Group ID behind is MasterID
        /// </summary>
        SpecialGroupsMasterID = 16,
        /// <summary>
        /// 17 = Reail item variant behind is MasterID
        /// </summary>
        RetailItemVariantMasterID = 17,
        /// <summary>
        /// 18 = Stock counting journals (not posted)
        /// </summary>
        StockCountingJournal = 18,
        /// <summary>
        /// 19 = Retail items that are not deleted and not service items
        /// </summary>
        InventoryItems = 19,
        /// <summary>
        /// 20 = Identical with RetailItemsMasterID except it does not contain non-inventory items
        /// </summary>
        InventoryItemsMasterID = 20,
        /// <summary>
        /// 21 = Identical with RetailItemVariantMasterID but contains a readable ID instead of master ID
        /// </summary>
        RetailItemVariantReadableID = 21,
        /// <summary>
        /// Custom data source - requires event handling
        /// </summary>
        Custom = 99,
    }
}
