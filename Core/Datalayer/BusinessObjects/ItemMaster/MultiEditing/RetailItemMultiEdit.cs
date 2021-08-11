using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster.MultiEditing
{
    public class RetailItemMultiEdit : RetailItem
    {

        public enum FieldSelectionEnum : long
        {
            NoFields = 0x0,
            RetailGroupMasterID = 0x1,
            RetailDivisionMasterID = 0x2,
            RetailDepartmentMasterID = 0x4,
            SalesTaxItemGroupID = 0x8,
            IsFuelItem = 0x10,
            GradeID = 0x20,
            ScaleItem = 0x40,
            MustKeyInComment = 0x80,
            ZeroPriceValid = 0x100,
            QuantityBecomesNegative = 0x200,
            NoDiscountAllowed = 0x400,
            MustSelectUOM = 0x800,
            KeyInPrice = 0x1000,
            KeyInQuantity = 0x2000,
            DateToBeBlocked = 0x4000,
            DateToActivateItem = 0x8000,
            ValidationPeriodID = 0x10000,
            SalesAllowTotalDiscount = 0x20000,
            SalesLineDiscount = 0x40000,
            SalesMultiLineDiscount = 0x80000,
            ExtendedDescription = 0x100000,
            NameAlias = 0x200000,
            SalesMarkup = 0x400000,
            PurchasePrice = 0x800000,
            SalesPrice = 0x1000000,
            SalesPriceIncludingTax = 0x2000000,
            ProfitMargin = 0x4000000,
            KeyInSerialNumber = 0x8000000,
            TareWeight = 0x10000000,
            SearchKeywords = 0x20000000,
            Returnable = 0x40000000,
            CanBeSold = 0x80000000,
            ProductionTime = 0x100000000,
            // Combined macro enums
            PriceFieldsCombined = 0x7800000 // 4 bits set - PurchasePrice - SalesPrice - SalesPriceIncludingTax - ProfitMargin
        };

        public RetailItemMultiEdit() :
            base()
        {
            FieldSelection = FieldSelectionEnum.NoFields;
            HasValidSalesTaxItemGroupID = false;
            MustRecalculatePrices = false;
            OldPrices = null;
        }

        public FieldSelectionEnum FieldSelection;

        public bool HasValidSalesTaxItemGroupID;
        public bool MustRecalculatePrices;

        public RetailItemPrice OldPrices;

        public RecordIdentifier OldSalesUnit;
    }
}
