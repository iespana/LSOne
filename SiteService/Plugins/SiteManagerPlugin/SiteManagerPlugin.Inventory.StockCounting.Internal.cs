using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.FileImport;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;

namespace LSOne.SiteService.Plugins.SiteManager
{
	public partial class SiteManagerPlugin
	{
		/// <summary>
		/// Create a new stock counting journal.
		/// </summary>
		/// <param name="entry">The entry into the database.</param>
		/// <param name="storeID">Store ID for the new journal.</param>
		/// <param name="description">Description of the new journal.</param>
		/// <param name="filter">Container with desired item filters.</param>
		/// <param name="templateID">Optional: The ID of the stock counting template if this is created from a template filter.</param>
		/// <param name="batchSize">Optional: The batch size for processing/generating stock counting items for this new stock counting.</param>
		protected virtual CreateStockCountingContainer CreateNewStockCounting(IConnectionManager entry,
																				RecordIdentifier storeID,
																				string description,
																				InventoryTemplateFilterContainer filter,
																				RecordIdentifier templateID = null,
																				int batchSize = 500)
		{
			InventoryAdjustment stockCounting = new InventoryAdjustment();
			stockCounting.JournalType = InventoryJournalTypeEnum.Counting;
			stockCounting.StoreId = storeID;
			stockCounting.Text = description;
			stockCounting.TemplateID = templateID ?? "";
			stockCounting.CreatedDateTime = DateTime.Now;
			stockCounting.PostedDateTime = Date.Empty;

			Utils.Log(this, "Saving stock counting journal.");

			Providers.InventoryAdjustmentData.Save(entry, stockCounting);

			CreateStockCountingContainer result = new CreateStockCountingContainer(CreateStockCountingResult.Success, stockCounting.ID);

            if(filter.HasFilterCriteria())
            {
                Utils.Log(this, "Saving stock counting lines.");

                try
                {
                    Providers.InventoryJournalTransactionData.CreateStockCountingJournalLinesFromFilter(entry, stockCounting.ID, stockCounting.StoreId, filter);
                }
                catch (Exception innerEx)
                {
                    Utils.LogException(this, innerEx);
                    result.Result = CreateStockCountingResult.ErrorCreatingStockCounting;
                }

                Utils.Log(this, "Checking number of saved lines.");
                int addedNumberOfLines = Providers.InventoryJournalTransactionData.Count(entry, stockCounting.ID) ?? 0;

                if (addedNumberOfLines == 0)
                {
                    Utils.Log(this, "No lines created.");
                    result.Result = CreateStockCountingResult.NoLinesCreated;
                }
            }

			return result;
		}

		//TODO This is duplicate code from ViewPlugins.ExcelFiles.MappingLogic.MapperBase
		//It would be nice to have it in a single place
		protected static bool CheckMandatoryFields(DataTable dt, DataRow row, RecordIdentifier itemID, string[] fieldNames, bool inserting, List<ImportLogItem> importLogItems,
			int? lineNumber = null, string itemDescription = "")
		{
			if (inserting)
			{
				int count = fieldNames.GetUpperBound(0) + 1;

				for (int i = 0; i < count; i++)
				{
					if (row[fieldNames[i]] == DBNull.Value)
					{
						// Mandatory field missing
						if (lineNumber.HasValue)
						{
							importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.MandatoryFieldMissing.Replace("#1", fieldNames[i]),
								lineNumber, itemDescription));
						}
						else
						{
							importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.MandatoryFieldMissing.Replace("#1", fieldNames[i])));
						}
						return false;
					}
				}
			}

			return true;
		}
	}
}