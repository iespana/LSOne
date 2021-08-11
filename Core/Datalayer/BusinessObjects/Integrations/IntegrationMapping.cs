using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Integrations
{
    /// <summary>
    /// IntegrationMapping allows for mapping between internal ids and external ids
    /// </summary>
    public class IntegrationMapping : DataEntity
    {
        /// <summary>
        /// Used to determine which table the mapping is for
        /// </summary>
        public enum MappingEnum
        {
            Customer,
            RetailItem,
            RetailItemDivision,
            RetailItemDepartment,
            RetailItemGroup,
            SpecialGroup,
            SalesTaxGroup,
            ItemSalesTaxGroup,
            Unit,
            PriceDiscountGroup,
            CustomerPriceDiscountGroup
        }

        public IntegrationMapping()
            : base()
        {
            InternalID = "";
            Created = DateTime.Now;
        }

        /// <summary>
        /// Internal Id that maps to the external id
        /// </summary>
        public RecordIdentifier InternalID { get; set; }

        /// <summary>
        /// A timestamp of when the entry was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// A timestamp of when the entry was last updated
        /// </summary>
        public DateTime Updated { get; set; }

        // Accessors
        public RecordIdentifier ExternalID { get { return ID; } set { ID = value; }}
        public MappingEnum TableName { get { return (MappingEnum)Enum.Parse(typeof(MappingEnum), Text); } set { base.Text = value.ToString(); }}
    }
}
