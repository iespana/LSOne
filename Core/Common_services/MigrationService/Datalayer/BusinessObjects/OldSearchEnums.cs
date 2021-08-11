namespace LSOne.Services.Datalayer.BusinessObjects
{
    /// <summary>
    /// This enum is used when poviding search options for retail items (i.e the Retail items list view)
    /// </summary>
    public enum OldRetailItemSearchEnum
    {
        /// <summary>
        /// Searches by Retail group
        /// </summary>
        RetailGroup,

        /// <summary>
        /// Searches by Retail department
        /// </summary>
        RetailDepartment,

        /// <summary>
        /// Searches by Tax group
        /// </summary>
        TaxGroup,

        /// <summary>
        /// Searches by Variant group
        /// </summary>
        VariantGroup,

        /// <summary>
        /// Searches by default Vendor
        /// </summary>
        Vendor,

        /// <summary>
        /// Search by default bar code
        /// </summary>
        BarCode,

        /// <summary>
        /// Search by special group
        /// </summary>
        SpecialGroup
    }
}
