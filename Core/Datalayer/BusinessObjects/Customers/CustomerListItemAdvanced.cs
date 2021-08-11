using System;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.BusinessObjects.Customers
{
    [Serializable]
    public class CustomerListItemAdvanced : CustomerListItem
    {
        public CustomerListItemAdvanced()
        {
            SalesTaxGroupName = "";
            PriceGroupName = "";
            LineDiscountGroupName = "";
            TotalDiscountGroupName = "";
            SalesTaxGroupID = RecordIdentifier.Empty;
            Blocked = BlockedEnum.Nothing;
            CashCustomer = false;
            PriceGroupID = RecordIdentifier.Empty;
            LineDiscountGroupID = RecordIdentifier.Empty;
            TotalDiscountGroupID = RecordIdentifier.Empty;
            CreditLimit = 0;
        }

        public TaxExemptEnum TaxExempt { get; set; }
        public RecordIdentifier SalesTaxGroupID { get; set; }

        public BlockedEnum Blocked { get; set; }

        public bool CashCustomer { get; set; }

        public RecordIdentifier PriceGroupID { get; set; }

        public RecordIdentifier LineDiscountGroupID { get; set; }

        public RecordIdentifier TotalDiscountGroupID { get; set; }

        public decimal CreditLimit { get; set; }

        public string SalesTaxGroupName { get; set; }

        public string PriceGroupName { get; set; }

        public string LineDiscountGroupName { get; set; }

        public string TotalDiscountGroupName { get; set; }
    }
}
