using System.Collections.Generic;

using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Replenishment
{
	/// <summary>
	/// 
	/// </summary>
	public interface IInventoryTemplateSectionSelectionData : IDataProviderBase<InventoryTemplateSectionSelection>
	{
		/// <summary>
		/// Retrieves the list of selected values for the given template items filter section.
		/// </summary>
		/// <param name="entry">Entry into the database.</param>
		/// <param name="templateID">Inventory template ID.</param>
		/// <param name="sectionID">Items filter section ID for the template.</param>
		/// <note>
		/// <paramref name="sectionID"/> must be one of the defined entries in <see cref="InventoryTemplateSectionType"/>.
		/// </note>
		List<RecordIdentifier> GetListForFilter(IConnectionManager entry, RecordIdentifier templateID, RecordIdentifier sectionID);

		/// <summary>
		/// Retrieves the filter list for the given template items filter section as a <see cref="InventoryTemplateSectionSelection"/>.
		/// </summary>
		/// <param name="entry">Entry into the database.</param>
		/// <param name="templateId">Inventory template ID.</param>
		/// <param name="sectionID"><b>Optional</b> Items filter section ID for the template. If <see langword="null"/> all filter sections will be retrieved.</param>
		/// <note>
		/// <paramref name="sectionID"/> must be one of the defined entries in <see cref="InventoryTemplateSectionType"/>.
		/// </note>
		List<InventoryTemplateSectionSelection> GetList(IConnectionManager entry, RecordIdentifier templateId, RecordIdentifier sectionID = null);

		/// <summary>
		/// Deletes all filters for the given <paramref name="sectionID"/>.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="templateId">Inventory template ID.</param>
		/// <param name="sectionID">Items filter section ID for the template.</param>
		/// <note>
		/// <paramref name="sectionID"/> must be one of the defined entries in <see cref="InventoryTemplateSectionType"/>.
		/// </note>
		void Delete(IConnectionManager entry, RecordIdentifier templateId, RecordIdentifier sectionID);

		/// <summary>
		/// Inserts a new items filter for the given inventory template.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="templateSectionSelection"></param>
		void Insert(IConnectionManager entry, InventoryTemplateSectionSelection templateSectionSelection);
	}
}