using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.Customers
{
    /// <summary>
    /// A enum that defines sorting for the customers
    /// </summary>
    public enum CustomerSorting
    {
        /// <summary>
        /// Sort by customer ID
        /// </summary>
        ID,
        /// <summary>
        /// Sort by customer name
        /// </summary>
        Name,
        CashCustomer,
        SalesTaxGroup,
        PriceGroup,
        LineDiscountGroup,
        TotalDiscountGroup,
        CreditLimit,
        Blocked,
        /// <summary>
        /// Sort by invoice account
        /// </summary>
        InvoiceAccount,
        /// <summary>
        /// Sort by address
        /// </summary>
        Address,
        FirstName,
        LastName,
    };

    /// <summary>
    /// A enum that defines sorting for the customer groups
    /// </summary>
    public enum CustomerGroupSorting
    {
        /// <summary>
        /// Sort by customer group ID
        /// </summary>
        ID,
        /// <summary>
        /// Sort by customer group name
        /// </summary>
        Name,
    }
}