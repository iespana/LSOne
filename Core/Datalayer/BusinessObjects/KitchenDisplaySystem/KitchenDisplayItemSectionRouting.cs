using LSOne.Utilities.DataTypes;


namespace LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem
{
    public class KitchenDisplayItemSectionRouting : DataEntity
    {
        /// <summary>
        /// Routing can be from retail group, special group or item
        /// </summary>
        public ItemTypeEnum ItemType { get; set; }

        /// <summary>
        /// Master ID of the item/group
        /// </summary>
        public RecordIdentifier ItemMasterId { get; set; }

        /// <summary>
        /// ID of the item/group
        /// </summary>
        public RecordIdentifier ItemId { get; set; }

        /// <summary>
        /// Description of the item/group
        /// </summary>
        public string ItemDescription { get; set; }

        /// <summary>
        /// ID of the production section
        /// </summary>
        public RecordIdentifier SectionId { get; set; }

        /// <summary>
        /// Code of the production section
        /// </summary>
        public RecordIdentifier SectionCode { get; set; }

        /// <summary>
        /// Description of the production section
        /// </summary>
        public string SectionDescription { get; set; }

        public KitchenDisplayItemSectionRouting()
        {
            ID = RecordIdentifier.Empty;
            ItemMasterId = RecordIdentifier.Empty;
            ItemId = RecordIdentifier.Empty;
            ItemDescription = string.Empty;
            SectionId = RecordIdentifier.Empty;
            SectionCode = RecordIdentifier.Empty;
            SectionDescription = string.Empty;
            ItemType = ItemTypeEnum.RetailGroup;
        }

        public enum ItemTypeEnum
        {
            RetailGroup,
            SpecialGroup,
            Item
        }

        public static string ItemTypeToString(ItemTypeEnum itemType)
        {
            switch (itemType)
            {
                case ItemTypeEnum.RetailGroup:
                    return Properties.Resources.RetailGroup;
                case ItemTypeEnum.SpecialGroup:
                    return Properties.Resources.SpecialGroup;
                case ItemTypeEnum.Item:
                    return Properties.Resources.Item;
                default:
                    return string.Empty;
            }
        }
    }
}