using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts
{
    public class CouponCustomerLink : DataEntity
    {
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(CouponID, CustomerID);
            }
            set
            {
                CouponID = value.PrimaryID;
                CustomerID = value.SecondaryID;
            }
        }

        public RecordIdentifier CouponID { get; set; }
        public RecordIdentifier CustomerID { get; set; }
        public string CustomerDescription { get; set; }
        public int Usages { get; set; }
    }
}
