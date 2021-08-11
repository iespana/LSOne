using System;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Interfaces;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    /// <summary>
    /// Data entity class for a purchase order lines
    /// </summary>
    public class PurchaseOrderLine : DataEntity, ITaxAmount
    {
        public PurchaseOrderLine()
        {
            PictureID = RecordIdentifier.Empty;
            OmniLineID = "";
            OmniTransactionID = "";
        }

        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(PurchaseOrderID, LineNumber);
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public DecimalLimit PriceLimiter { get; set; }
        public DecimalLimit PercentageLimiter { get; set; }
        public DecimalLimit QuantityLimiter { get; set; }
        public RecordIdentifier PurchaseOrderID { get; set; }
        public RecordIdentifier LineNumber { get; set; }
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string VendorItemID { get; set; }
        public string VariantName { get; set; }
        //public string ColorName { get; set; }
        //public string SizeName { get; set; }
        //public string StyleName { get; set; }
        public string UnitID { get; set; }
        public string UnitName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal TotalPrice => FinalUnitPrice() * Quantity;
        public bool ItemDeleted { get; set; }
        public ItemTypeEnum ItemType { get; set; }
        public bool ItemInventoryExcluded
        {
            get
            {
                return ItemDeleted || ItemType == ItemTypeEnum.Service;
            }
        }
        
        public TaxCalculationMethodEnum TaxCalculationMethod { get; set; }

        /// <summary>
        /// The ID of the image associated with this line
        /// </summary>               
        public RecordIdentifier PictureID { get; set; }

        /// <summary>
        /// The ID of the line that was assigned to it by the inventory app.         
        /// </summary>        
        public string OmniLineID { get; set; }

        /// <summary>
        /// The ID of the transaction in the inventory app that this line was created on
        /// </summary>        
        public string OmniTransactionID { get; set; }


        public string FormattedQuantity
        {
            get
            {
                if (QuantityLimiter != null)
                {
                    return Quantity.FormatWithLimits(QuantityLimiter);
                }
                else
                {
                    return "";
                }
            }
        }
        public string FormattedUnitPrice
        {
            get
            {
                if (PriceLimiter != null)
                {
                    return UnitPrice.FormatWithLimits(PriceLimiter);
                }
                else
                {
                    return "";
                }
            }
        }
        public string FormattedDiscountPercentage
        {
            get
            {
                if (PercentageLimiter != null)
                {
                    return DiscountPercentage.FormatWithLimits(PercentageLimiter) + " %";
                }
                else
                {
                    return "";
                }
            }
        }
        public string FormattedDiscountAmount
        {
            get
            {
                if (PriceLimiter != null)
                {
                    return DiscountAmount.FormatWithLimits(PriceLimiter);
                }
                else
                {
                    return "";
                }
            }
        }
        public string FormattedTaxAmount
        {
            get
            {
                if (PriceLimiter != null)
                {
                    string formattedTaxAmount = TaxAmount.FormatWithLimits(PriceLimiter);
                    return formattedTaxAmount != "0" ? formattedTaxAmount : "-";
                }
                else
                {
                    return "";
                }
            }
        }
        public string FormattedTotalPrice
        {
            get
            {
                if (PriceLimiter != null)
                {
                    return TotalPrice.FormatWithLimits(PriceLimiter);
                }
                else
                {
                    return "";
                }
            }
        }
        public string FormattedFinalUnitPrice
        {
            get
            {
                if (PriceLimiter != null)
                {
                    return (FinalUnitPrice()).FormatWithLimits(PriceLimiter);
                }
                else
                {
                    return "";
                }
            }
        }

        public string FormattedCalculatedDiscount
        {
            get
            {
                if (PriceLimiter != null)
                {
                    return (CalculatedDiscount()).FormatWithLimits(PriceLimiter);
                }
                else
                {
                    return "";
                }
            }
        }

        public void SetReportFormatting(DecimalLimit priceLimiter, DecimalLimit percentageLimiter, DecimalLimit quantityLimiter)
        {
            PriceLimiter = priceLimiter;
            PercentageLimiter = percentageLimiter;
            QuantityLimiter = quantityLimiter;
        }

        public decimal FinalUnitPrice()
        {
            decimal finalUnitPrice = decimal.Zero;

            switch (TaxCalculationMethod)
            {
                case TaxCalculationMethodEnum.AddTax:
                    finalUnitPrice = ((UnitPrice - DiscountAmount) * (1 - DiscountPercentage / 100)) + TaxAmount;
                    break;
                case TaxCalculationMethodEnum.IncludeTax:
                case TaxCalculationMethodEnum.NoTax:
                    finalUnitPrice = ((UnitPrice - DiscountAmount) * (1 - DiscountPercentage / 100));
                    break;
            }
            return (finalUnitPrice >= decimal.Zero) ? finalUnitPrice : decimal.Zero;
            
        }

        public decimal GetDiscountedPrice()
        {
            return ((UnitPrice - DiscountAmount) * (1 - DiscountPercentage / 100));
        }

        public decimal CalculatedDiscount()
        {
            decimal finalUnitPrice = FinalUnitPrice();
            decimal unitPrice = decimal.Zero;

            switch (TaxCalculationMethod)
            {
                case TaxCalculationMethodEnum.AddTax:
                    unitPrice = UnitPrice + TaxAmount;
                    break;
                case TaxCalculationMethodEnum.IncludeTax:
                case TaxCalculationMethodEnum.NoTax:
                    unitPrice = UnitPrice;
                    break;
            }

            return (unitPrice - finalUnitPrice)*-1;
        }
    }
}
 