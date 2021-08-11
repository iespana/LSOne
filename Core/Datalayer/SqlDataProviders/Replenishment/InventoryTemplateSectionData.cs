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
	public class InventoryTemplateSectionData : SqlServerDataProviderBase, IInventoryTemplateSectionData
	{
		private static void PopulateInventoryTemplateSection(IDataReader dr, InventoryTemplateSection templateSection)
		{
			templateSection.SectionID = (string)dr["ID"];
			templateSection.TemplateID = (string)dr["INVENTORYTEMPLATEIDID"];
			templateSection.SortRank = (int)dr["SORTRANK"];
		}

		public virtual List<InventoryTemplateSection> GetList(IConnectionManager entry, RecordIdentifier templateId)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				string sql = @"SELECT i.ID,i.INVENTORYTEMPLATEIDID, i.SORTRANK
							   FROM INVENTORYTEMPLATEITEMFILTERSECTIONS i 
							   WHERE i.DATAAREAID = @dataAreaId and i.INVENTORYTEMPLATEIDID = @templateID
							   ORDER BY SORTRANK";

				cmd.CommandText = sql;

				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
				MakeParam(cmd, "templateID", (string)templateId);

				return Execute<InventoryTemplateSection>(entry, cmd, CommandType.Text, PopulateInventoryTemplateSection);
			}
		}

		/// <summary>
		/// Deletes all template sections for a given template
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="templateId"></param>
		public virtual void Delete(IConnectionManager entry, RecordIdentifier templateId)
		{
			DeleteRecord(entry, "INVENTORYTEMPLATEITEMFILTERSECTIONS", "INVENTORYTEMPLATEIDID", templateId, BusinessObjects.Permission.ManageReplenishment);
			DeleteRecord(entry, "INVENTORYTEMPLATEITEMFILTERSELECTIONS", "INVENTORYTEMPLATEIDID", templateId, BusinessObjects.Permission.ManageReplenishment);
		}

		/// <summary>
		/// Inserts a given template section to the database.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="templateSection"></param>
		/// <returns></returns>
		public virtual void Insert(IConnectionManager entry, InventoryTemplateSection templateSection)
		{
			var statement = new SqlServerStatement("INVENTORYTEMPLATEITEMFILTERSECTIONS");

			templateSection.Validate();

			statement.StatementType = StatementType.Insert;

			statement.AddKey("ID", (string)templateSection.SectionID);
			statement.AddKey("INVENTORYTEMPLATEIDID", (string)templateSection.TemplateID);
			statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

			statement.AddField("SORTRANK", templateSection.SortRank, SqlDbType.Int);
			

			entry.Connection.ExecuteStatement(statement);
		}
	}
}