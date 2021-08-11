using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.Contacts
{
    /// <summary>
    /// A enum that defines sorting for the contacts
    /// </summary>
    public enum ContactSorting
    {
        /// <summary>
        /// Sort by Name
        /// </summary>
        Name,
        /// <summary>
        /// Sort by company name
        /// </summary>
        CompanyName,
        /// <summary>
        /// Sort by phone
        /// </summary>
        Phone,
        /// <summary>
        /// Sort by address
        /// </summary>
        Address
    };
}