namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    /// <summary>
    /// When searching for items that are in a specific group or are not in a specific group this class is used as a result of those search criteria
    /// </summary>
    public class ItemInGroup : DataEntity
    {
        /// <summary>
        /// Group description
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// The description of the item variant if any
        /// </summary>
        public string VariantName { get; set; }
    }
}
