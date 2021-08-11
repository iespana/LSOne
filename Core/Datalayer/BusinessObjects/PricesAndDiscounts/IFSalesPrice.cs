using LSOne.Utilities.DataTypes;
using System;

namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts
{
    public class IFSalesPrice
    {
        public enum PriceFor
        {
            Item,
            ItemGroup,
            AllItems
        }

        public enum PriceAppliesTo
        {
            SpecificCustomer,
            CustomerGroup,
            AllCustomers
        }

        /// <summary>
        /// Specifies if the price applies to a specific customer, a customer group or all customers
        /// </summary>
        public PriceAppliesTo AppliesTo { get; set; }
        /// <summary>
        /// The value for which the price applies to. Ex: customer ID, customer group ID or empty if it is for all customers
        /// </summary>
        public RecordIdentifier AppliesToValue { get; set; }
        /// <summary>
        /// Specifies if the price is for an item, an item group or all items
        /// </summary>
        public PriceFor PriceIsFor { get; set; }
        /// <summary>
        /// The value for which the price is for. Ex: item ID, item group ID or empty if it is for all items
        /// </summary>
        public RecordIdentifier PriceIsForValue { get; set; }

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
        /// The quantity needed to activate the trade agreement
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// The price of the trade agreement
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// The price of the trade agreement including tax
        /// </summary>
        public decimal PriceWithTax { get; set; }
        /// <summary>
        /// The markup that should be added to the price
        /// </summary>
        public decimal Markup { get; set; }

        public IFSalesPrice()
        {
            FromDate = new Date(DateTime.Now);
            AppliesTo = PriceAppliesTo.SpecificCustomer;
            AppliesToValue = "";
            PriceIsFor = PriceFor.Item;
            PriceIsForValue = "";
            Currency = "";
            UnitID = "";
            Quantity = 0;
            Price = 0;
            PriceWithTax = 0;
            Markup = 0;
            ToDate = Date.Empty;
        }

        public bool Validate()
        {
            if (AppliesTo != PriceAppliesTo.AllCustomers
             && (RecordIdentifier.IsEmptyOrNull(AppliesToValue) || AppliesToValue == ""))
                return false;

            if (PriceIsFor != PriceFor.AllItems
             && (RecordIdentifier.IsEmptyOrNull(PriceIsForValue) || PriceIsForValue == ""))
                return false;

            if (UnitID == "" || Currency == "")
                return false;

            return true;
        }
    }
}
