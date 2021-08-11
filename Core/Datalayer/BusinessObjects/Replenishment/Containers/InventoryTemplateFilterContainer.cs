using System;
using System.Collections.Generic;

using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Expressions;

namespace LSOne.DataLayer.BusinessObjects.Replenishment.Containers
{
	/// <summary>
	/// 
	/// </summary>
	public class InventoryTemplateFilterContainer
	{
		/// <summary>
		/// Gets or sets the retail groups filter section that is used for selecting <see cref="LSOne.DataLayer.BusinessObjects.ItemMaster.RetailItem"/>.
		/// </summary>
		public List<RecordIdentifier> RetailGroups { get; set; }

		/// <summary>
		/// Gets or sets the retail departments filter section that is used for selecting <see cref="LSOne.DataLayer.BusinessObjects.ItemMaster.RetailItem"/>.
		/// </summary>
		public List<RecordIdentifier> RetailDepartments { get; set; }

		/// <summary>
		/// Gets or sets the special groups filter section that is used for selecting <see cref="LSOne.DataLayer.BusinessObjects.ItemMaster.RetailItem"/>.
		/// </summary>
		public List<RecordIdentifier> SpecialGroups { get; set; }

		/// <summary>
		/// Gets or sets the vendors filter section that is used for selecting <see cref="LSOne.DataLayer.BusinessObjects.ItemMaster.RetailItem"/>.
		/// </summary>
		public List<RecordIdentifier> Vendors { get; set; }

        /// <summary>
        /// If true, creating a journal will filter lines by inventory on hand.
        /// </summary>
        public bool FilterByInventoryOnHand { get; set; }

        /// <summary>
        /// The inventory on hand value to filter by.
        /// </summary>
        public int InventoryOnHand { get; set; }

        /// <summary>
        /// Comparison type for the inventory on hand filter.
        /// </summary>
        public DoubleValueOperator InventoryOnHandComparison;

		/// <summary>
		/// Gets or sets the starting index that is used for selecting <see cref="LSOne.DataLayer.BusinessObjects.ItemMaster.RetailItem"/>.
		/// </summary>
		public int RowFrom { get; set; }

		/// <summary>
		/// Gets or sets the ending index that is used for selecting <see cref="LSOne.DataLayer.BusinessObjects.ItemMaster.RetailItem"/>.
		/// </summary>
		public int RowTo { get; set; }

        /// <summary>
        /// Include RowFrom and RowTo in the filter of the query. If this flag is not set to true, the filter won't apply.
        /// </summary>
        public bool LimitRows { get; set; }

        /// <summary>
        /// Use this property if you only need the first 50 rows. This allows the method using this parameter to execute an optimized query, which runs very fast.
        /// </summary>
        public bool LimitToFirst50Rows { get; set; }

		/// <summary>
		/// Initializes a default <see cref="InventoryTemplateFilterContainer" /> with:
		/// <list type="bullet">
		/// <item>empty <see cref="RetailGroups">RetailGroup</see> section,</item>
		/// <item>empty <see cref="RetailDepartments">RetailDepartment</see> section,</item>
		/// <item>empty <see cref="SpecialGroups">SpecialGroup</see> section,</item>
		/// <item>empty <see cref="Vendors">Vendor</see> section,</item>
		/// <item><see cref="RowFrom"/> set to 1,</item>
		/// <item><see cref="RowTo"/> set to 51,</item>
		/// <item><see cref="LimitRows"/> set to false</item>
		/// </list>
		/// </summary>
		public InventoryTemplateFilterContainer()
		{
			RetailGroups = new List<RecordIdentifier>();
			RetailDepartments = new List<RecordIdentifier>();
			SpecialGroups = new List<RecordIdentifier>();
			Vendors = new List<RecordIdentifier>();
			RowFrom = 1;
			RowTo = 51;
			LimitRows = false;
            FilterByInventoryOnHand = false;
            InventoryOnHand = 0;
            InventoryOnHandComparison = DoubleValueOperator.Equals;
		}

		/// <summary>
		/// Initializes a <see cref="InventoryTemplateFilterContainer" /> with the given list of <see cref="InventoryTemplateSectionSelection">section values</see>.
		/// </summary>
		/// <param name="filters"></param>
		public InventoryTemplateFilterContainer(List<InventoryTemplateSectionSelection> filters)
			: this()
		{
			if (filters == null || filters.Count == 0) return;

			//if using LINQ we'll have to scan the collection 4 times so a simple foreach would be better from performance point of view
			foreach (var filter in filters)
			{
				if (Enum.TryParse((string)filter.SectionID, out InventoryTemplateSectionType sectionType))
				{
					switch (sectionType)
					{
						case InventoryTemplateSectionType.RetailGroup:
							RetailGroups.Add(filter.EntityID);
							break;
						case InventoryTemplateSectionType.RetailDepartment:
							RetailDepartments.Add(filter.EntityID);
							break;
						case InventoryTemplateSectionType.Vendor:
							Vendors.Add(filter.EntityID);
							break;
						case InventoryTemplateSectionType.SpecialGroup:
							SpecialGroups.Add(filter.EntityID);
							break;
                        case InventoryTemplateSectionType.InventoryOnHand:
                            string[] inventOnHandSplit = filter.EntityID.StringValue.Split('|');

                            FilterByInventoryOnHand = true;
                            InventoryOnHandComparison = (DoubleValueOperator)Convert.ToInt32(inventOnHandSplit[0]);
                            InventoryOnHand = Convert.ToInt32(inventOnHandSplit[1]);
                            break;
						case InventoryTemplateSectionType.Unset:
						default:
							// unknown filters will not be added to the container
							break;
					}
				}
			}
		}

        /// <summary>
        /// True if any filters are selected
        /// </summary>
        /// <returns></returns>
        public bool HasFilterCriteria()
        {
            return RetailGroups.Count > 0 || RetailDepartments.Count > 0 || Vendors.Count > 0 || SpecialGroups.Count > 0 || FilterByInventoryOnHand;
        }
	}
}
