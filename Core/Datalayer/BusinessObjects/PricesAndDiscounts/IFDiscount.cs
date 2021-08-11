using LSOne.Utilities.DataTypes;
using System;

namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts
{
    public class IFDiscount
    {
        public enum DiscountFor
        {
            Item,
            ItemGroup,
            AllItems
        }

        public enum DiscountAppliesTo
        {
            SpecificCustomer,
            CustomerGroup,
            AllCustomers
        }

        /// <summary>
        /// Specifies if the discount applies to a specific customer, a customer group or all customers
        /// </summary>
        public DiscountAppliesTo AppliesTo { get; set; }
        /// <summary>
        /// The value for which the discount applies to. Ex: customer ID, customer group ID or empty if it is for all customers
        /// </summary>
        public RecordIdentifier AppliesToValue { get; set; }
        /// <summary>
        /// Specifies if the discount is for an item, an item group or all items
        /// </summary>
        public DiscountFor DiscountIsFor { get; set; }
        /// <summary>
        /// The value for which the discount is for. Ex: item ID, item group ID or empty if it is for all items
        /// </summary>
        public RecordIdentifier DiscountIsForValue { get; set; }

        /// <summary>
        /// ID of the currency code used in trade agreement
        /// </summary>
        public RecordIdentifier Currency { get; set; }
        /// <summary>
        /// What unit does this trade agreement apply to.
        /// </summary>
        public RecordIdentifier UnitID { get; set; }
        /// <summary>
        /// Start of trade agreement valid date.A blank value means the trade agreement has no start date (is always valid until the ToDate). 
        /// Because DateTime objects can not be nullable, the value '01.01.1900' is considered blank
        /// </summary>
        public Date FromDate { get; set; }
        /// <summary>
        /// End of trade agreement valid date. A blank value means the trade agreement has no end date (is valid from the FromDate). 
        /// Because DateTime objects can not be nullable, the value '01.01.1900' is considered blank
        /// </summary>
        public Date ToDate { get; set; }
        /// <summary>
        /// The quantity needed to activate the trade agreement. When using total discount trade agreement this field refers to a minimum amount of purchase to activate the discount.
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Discount amount of the trade agreement
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// First percentage discount of the trade agreement
        /// </summary>
        public decimal Percentage1 { get; set; }
        /// <summary>
        /// Second percentage discount of the trade agreement
        /// </summary>
        /// <remarks>
        /// Percentage 2 will be calculated after the percentage 1 has been calculated.
        /// Example: the item costs € 10,00, percentage 1 is set to 10%, percentage 2 is set
        /// to 20%. The amount to be paid is calculated as follows: 10 * 0,9 = 9; 9 * 0,8 =
        /// 7,20.
        /// </remarks>
        public decimal Percentage2 { get; set; }

        public IFDiscount()
        {
            FromDate = new Date(DateTime.Now);
            AppliesTo = DiscountAppliesTo.SpecificCustomer;
            AppliesToValue = "";
            DiscountIsFor = DiscountFor.Item;
            DiscountIsForValue = "";
            Currency = "";
            UnitID = "";
            Quantity = 0;
            Amount = 0;
            Percentage1 = 0;
            Percentage2 = 0;
            ToDate = Date.Empty;
        }

        public bool Validate()
        {
            if (AppliesTo != DiscountAppliesTo.AllCustomers
             && (RecordIdentifier.IsEmptyOrNull(AppliesToValue) || AppliesToValue == ""))
                return false;

            if (DiscountIsFor != DiscountFor.AllItems
             && (RecordIdentifier.IsEmptyOrNull(DiscountIsForValue) || DiscountIsForValue == ""))
                return false;

            if (UnitID == "" || Currency == "")
                return false;

            return true;
        }
    }
}
