using System;
using System.Drawing;
using System.Runtime.Serialization;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts
{
    public class Coupon : DataEntity
    {
        public Coupon()
        {
            DiscountPercent = 0;
            Details = "";
            ValidationPeriodID = "";
            MaxUsages = 0;
        }
        [DataMember]
        public override string Text { get { return base.Text; } }
        [DataMember]
        public decimal DiscountPercent { get; set; }
        public string Details { get; set; }
        public bool GiveToNewCustomers { get; set; }
        public RecordIdentifier ValidationPeriodID { get; set; }
        public string ValidationPeriod { get; set; } 
        public int MaxUsages { get; set; }
        public Image Image { get; set; }
        public int LineId { get; set; }

        public override object Clone()
        {
            Coupon cloned = new Coupon();
            Populate(cloned);
            return cloned;
        }

        private void Populate(Coupon coupon)
        {
            coupon.ID = ID;
            coupon.LineId = LineId;
            coupon.Text = Text;
            coupon.DiscountPercent = DiscountPercent;
            coupon.ValidationPeriod = ValidationPeriod;
            coupon.ValidationPeriodID = ValidationPeriodID;
            coupon.MaxUsages = MaxUsages;
            coupon.Image = Image;
            coupon.Details = Details;
            coupon.GiveToNewCustomers = GiveToNewCustomers;
        }
    }
}
