namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts
{
    public class DiscountAndPriceActivation : DataEntity
    {
        public DiscountAndPriceActivation()
        {
            PriceCustomerItem = true;
            PriceCustomerGroupItem = true;
            PriceAllCustomersItem = true;

            LineDiscountCustomerItem = true;
            LineDiscountCustomerGroupItem = true;
            LineDiscountAllCustomersItem = false;
            LineDiscountCustomerItemGroup = true;
            LineDiscountCustomerGroupItemGroup = true;
            LineDiscountAllCustomersItemGroup = false;
            LineDiscountCustomerAllItems = false;
            LineDiscountCustomerGroupAllItems = false;
            LineDiscountAllCustomersAllItems = false;

            MultilineDiscountCustomerItemGroup = true;
            MultilineDiscountCustomerGroupItemGroup = true;
            MultilineDiscountAllCustomersItemGroup = false;
            MultilineDiscountCustomerAllItems = false;
            MultilineDiscountCustomerGroupAllItems = false;
            MultilineDiscountAllCustomersAllItems = false;

            TotalDiscountCustomerAllItems = true;
            TotalDiscountCustomerGroupAllItems = true;
            TotalDiscountAllCustomersAllItems = false;
        }

        public bool PriceCustomerItem { get; set; }
        public bool PriceCustomerGroupItem { get; set; }
        public bool PriceAllCustomersItem { get; set; }

        public bool LineDiscountCustomerItem { get; set; }
        public bool LineDiscountCustomerGroupItem { get; set; }
        public bool LineDiscountAllCustomersItem { get; set; }
        public bool LineDiscountCustomerItemGroup { get; set; }
        public bool LineDiscountCustomerGroupItemGroup { get; set; }
        public bool LineDiscountAllCustomersItemGroup { get; set; }
        public bool LineDiscountCustomerAllItems { get; set; }
        public bool LineDiscountCustomerGroupAllItems { get; set; }
        public bool LineDiscountAllCustomersAllItems { get; set; }

        public bool MultilineDiscountCustomerItemGroup { get; set; }
        public bool MultilineDiscountCustomerGroupItemGroup { get; set; }
        public bool MultilineDiscountAllCustomersItemGroup { get; set; }
        public bool MultilineDiscountCustomerAllItems { get; set; }
        public bool MultilineDiscountCustomerGroupAllItems { get; set; }
        public bool MultilineDiscountAllCustomersAllItems { get; set; }

        public bool TotalDiscountCustomerAllItems { get; set; }
        public bool TotalDiscountCustomerGroupAllItems { get; set; }
        public bool TotalDiscountAllCustomersAllItems { get; set; }
    }
}

