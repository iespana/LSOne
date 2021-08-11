using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts
{
    public class ItemInPriceDiscountGroup : DataEntity
    {
        public string PriceDiscountGroup { get; set; }
        public string VariantName { get; set; }
    }
}
