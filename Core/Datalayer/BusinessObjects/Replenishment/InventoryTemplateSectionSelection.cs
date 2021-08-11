using System;

using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Replenishment
{
	/// <summary>
	/// Represents one items filter criteria for the current inventory template.
	/// </summary>
	/// <remarks>The inventory template filter accepts one or more criteria (<see cref="SectionID"/>) with one or more values (<see cref="EntityID"/>).</remarks>
	public class InventoryTemplateSectionSelection : DataEntity
	{
		/// <summary>
		/// A complex (3-level) <see cref="RecordIdentifier">ID</see> formed from <see cref="EntityID" />, <see cref="SectionID"/> and <see cref="TemplateID"/>
		/// </summary>
		/// <exception cref="NotImplementedException">Thrown if setter is called by a non-serialization operation.</exception>
		public override RecordIdentifier  ID
		{
			get 
			{
				return new RecordIdentifier(EntityID, SectionID, TemplateID);
			}
			set 
			{
				if (!serializing)
				{
					throw new NotImplementedException();
				}
			}
		}

		/// <summary>
		/// Gets or sets a selected value for the current <see cref="SectionID"/>
		/// </summary>
		/// <remarks>Supports IDs up to 20 characters size.</remarks>
		/// <example>For "Vendor" section, one valid Entity would be 0000001 (= Fashion House)</example>
		[RecordIdentifierValidation(20, Depth = 1)]
		public RecordIdentifier EntityID { get; set; }

		/// <summary>
		/// Gets or sets the template section. Currently supported sections are: RetailGroup, RetailDepartment, SpecialGroup and Vendor.
		/// </summary>
		/// <remarks>Supports IDs up to 20 characters size.</remarks>
		[RecordIdentifierValidation(20, Depth = 1)]
		public RecordIdentifier SectionID { get; set; }

		/// <summary>
		/// Gets or sets the inventory template ID.
		/// </summary>
		/// <remarks>Supports IDs up to 20 characters size.</remarks>
		[RecordIdentifierValidation(20, Depth = 1)]
		public RecordIdentifier TemplateID { get; set; }
	}
}
