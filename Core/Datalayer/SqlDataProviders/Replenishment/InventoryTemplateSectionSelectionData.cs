using System.Collections.Generic;
using System.Data;

using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.DataProviders.Replenishment;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Replenishment
{
	public class InventoryTemplateSectionSelectionData : SqlServerDataProviderBase, IInventoryTemplateSectionSelectionData
	{
		private static void PopulateInventoryTemplateSectionSelection(IDataReader dr, InventoryTemplateSectionSelection templateSectionSelection)
		{
			templateSectionSelection.EntityID = (string)dr["ID"];
			templateSectionSelection.SectionID = (string)dr["SECTIONID"];
			templateSectionSelection.TemplateID = (string)dr["INVENTORYTEMPLATEIDID"];
		}

		/// <summary>
		/// Retrieves the list of selected values for the given template items filter section.
		/// </summary>
		/// <param name="entry">Entry into the database.</param>
		/// <param name="templateID">Inventory template ID.</param>
		/// <param name="sectionID">Items filter section ID for the template.</param>
		/// <note>
		/// <paramref name="sectionID"/> must be one of the defined entries in <see cref="InventoryTemplateSectionType"/>.
		/// </note>
		public virtual List<RecordIdentifier> GetListForFilter(IConnectionManager entry, RecordIdentifier templateID, RecordIdentifier sectionID)
		{
			using (IDbCommand cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText = @"SELECT ID FROM INVENTORYTEMPLATEITEMFILTERSELECTIONS
									WHERE INVENTORYTEMPLATEIDID = @templateID AND SECTIONID = @sectionID AND DATAAREAID = @dataAreaID";

				MakeParam(cmd, "templateID", templateID);
				MakeParam(cmd, "sectionID", sectionID);
				MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

				return Execute(entry, cmd, CommandType.Text, "ID");
			}
		}

		/// <summary>
		/// Retrieves the filter list for the given template items filter section as a <see cref="InventoryTemplateSectionSelection"/>.
		/// </summary>
		/// <param name="entry">Entry into the database.</param>
		/// <param name="templateId">Inventory template ID.</param>
		/// <param name="sectionID"><b>Optional</b> Items filter section ID for the template. If <see langword="null"/> all filter sections will be retrieved.</param>
		/// <note>
		/// <paramref name="sectionID"/> must be one of the defined entries in <see cref="InventoryTemplateSectionType"/>.
		/// </note>
		public virtual List<InventoryTemplateSectionSelection> GetList(IConnectionManager entry, RecordIdentifier templateId, RecordIdentifier sectionID = null)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				string query = @"SELECT i.ID,
										i.INVENTORYTEMPLATEIDID, 
										i.SECTIONID
							   FROM INVENTORYTEMPLATEITEMFILTERSELECTIONS i 
							   WHERE i.DATAAREAID = @dataAreaId and i.INVENTORYTEMPLATEIDID = @templateID";

				if(!RecordIdentifier.IsEmptyOrNull(sectionID))
				{
					query += " and i.SECTIONID = @sectionID";
					MakeParam(cmd, "sectionID", (string)sectionID);
				}

				cmd.CommandText = query;
				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
				MakeParam(cmd, "templateID", (string)templateId);

				return Execute<InventoryTemplateSectionSelection>(entry, cmd, CommandType.Text, PopulateInventoryTemplateSectionSelection);
			}
		}

		/// <summary>
		/// Deletes all filters for the given <paramref name="sectionID"/>.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="templateId">Inventory template ID.</param>
		/// <param name="sectionID">Items filter section ID for the template.</param>
		/// <note>
		/// <paramref name="sectionID"/> must be one of the defined entries in <see cref="InventoryTemplateSectionType"/>.
		/// </note>
		public virtual void Delete(IConnectionManager entry, RecordIdentifier templateId, RecordIdentifier sectionID)
		{
			DeleteRecord(entry, "INVENTORYTEMPLATEITEMFILTERSELECTIONS",new string[]{"INVENTORYTEMPLATEIDID","SECTIONID"}, new RecordIdentifier(templateId,sectionID),
				BusinessObjects.Permission.ManageReplenishment);
		}

		/// <summary>
		/// Inserts a new items filter for the given inventory template.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="templateSectionSelection"></param>
		public virtual void Insert(IConnectionManager entry, InventoryTemplateSectionSelection templateSectionSelection)
		{
			var statement = new SqlServerStatement("INVENTORYTEMPLATEITEMFILTERSELECTIONS");

			templateSectionSelection.Validate();

			statement.StatementType = StatementType.Insert;

			statement.AddKey("ID", (string)templateSectionSelection.EntityID);
			statement.AddKey("SECTIONID", (string)templateSectionSelection.SectionID);
			statement.AddKey("INVENTORYTEMPLATEIDID", (string)templateSectionSelection.TemplateID);
			statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
			

			entry.Connection.ExecuteStatement(statement);
		}
	}
}
