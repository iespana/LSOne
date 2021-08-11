using System.Drawing;
using LSOne.Utilities.DataTypes;
#if !MONO
#endif


namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts
{
    public class DiscountOfferLineWithPrice 
    {
        public DiscountOfferLine DiscountOfferLine { get; set; }

        public decimal DiscountPriceWithTax { get; set; }
        
        public decimal OfferPrice { get; set; }
        
        public decimal TaxAmount { get; set; }
        
        public decimal OfferPriceWithTax { get; set; }
        
        public decimal StandardPriceWithTax { get; set; }
        
        public decimal DiscountAmountWithTax { get; set; }
       
    }
}
