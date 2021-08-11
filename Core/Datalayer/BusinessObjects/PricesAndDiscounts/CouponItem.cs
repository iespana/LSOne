using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts
{
    public class CouponItem : DataEntity
    {
        public enum TypeEnum
        {
            Item = 0,
            RetailGroup = 1
        }

        public CouponItem()
        {
            DiscountedQuantity = 0;
            SaleItemLineIDs = new List<int>();
        }

        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(CouponID, ItemRelation);
            }
            set
            {
                CouponID = value.PrimaryID;
                ItemRelation = value.SecondaryID;
            }
        }

        public RecordIdentifier CouponID { get; set; }
        public RecordIdentifier ItemRelation { get; set; }

        public int LinkedToCouponLineID { get; set; }
        public List<int> SaleItemLineIDs { get; set; } 
        public decimal DiscountedQuantity { get; set; }
        public int ItemQuantity { get; set; }
        public TypeEnum Type { get; set; }

        public string TypeText
        {
            get
            {
                switch (Type)
                {
                    case TypeEnum.Item:
                        return Properties.Resources.RetailItem;
                    case TypeEnum.RetailGroup:
                        return Properties.Resources.RetailGroup;
                    default:
                        return "";
                }
            }
        }

        public override object Clone()
        {
            CouponItem cloned = new CouponItem();
            Populate(cloned);
            return cloned;
        }

        private void Populate(CouponItem couponItem)
        {
            couponItem.ID = ID;
            couponItem.Text = Text;
            couponItem.CouponID = CouponID;
            couponItem.ItemRelation = ItemRelation;
            couponItem.ItemQuantity = ItemQuantity;
            couponItem.Type = Type;
            couponItem.LinkedToCouponLineID = LinkedToCouponLineID;
            couponItem.DiscountedQuantity = DiscountedQuantity;
            couponItem.SaleItemLineIDs = SaleItemLineIDs;
        }
    }
}
