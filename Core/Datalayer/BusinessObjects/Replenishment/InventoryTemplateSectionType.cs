using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;

namespace LSOne.DataLayer.BusinessObjects.Replenishment
{
	/// <summary>
	/// Available filter sections for <see cref="InventoryTemplateFilterContainer"/> and <see cref="InventoryTemplateSectionSelection"/>
	/// </summary>
	public enum InventoryTemplateSectionType
	{
		/// <summary>
		/// No section type or unknown / undefined.
		/// </summary>
		Unset = 0,
		/// <summary>
		/// Retail group section.
		/// </summary>
		RetailGroup = 1,
		/// <summary>
		/// Retail department section.
		/// </summary>
		RetailDepartment = 2,
		/// <summary>
		/// Vendor section.
		/// </summary>
		Vendor = 3,
		/// <summary>
		/// Special group section.
		/// </summary>
		SpecialGroup = 4,
        /// <summary>
        /// Inventory on hand section
        /// </summary>
        InventoryOnHand = 5
	}
}
