using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;

namespace LSOne.SiteService.Plugins.SiteManager
{
	public partial class SiteManagerPlugin
	{
        /// <summary>
        /// Gets a list with all inventory templates
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="filter"></param>
        /// <returns></returns>
		public virtual List<InventoryTemplateListItem> GetInventoryTemplateList(LogonInfo logonInfo, InventoryTemplateListFilter filter)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, Utils.Starting);

                return Providers.InventoryTemplateData.GetList(entry, filter);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Gets a list with all inventory templates for a given store, matching a given filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="storeID"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
		public virtual List<InventoryTemplateListItem> GetInventoryTemplateListForStore(LogonInfo logonInfo, RecordIdentifier storeID, InventoryTemplateListFilter filter)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, $"{Utils.Starting} {nameof(storeID)}: {storeID}");

                return Providers.InventoryTemplateData.GetListForStore(entry, storeID, filter);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Gets the first store name for the given template ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="templateId"></param>
        /// <returns></returns>
		public virtual string GetInventoryTemplateFirstStoreName(LogonInfo logonInfo, RecordIdentifier templateId)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, $"{Utils.Starting} {nameof(templateId)}: {templateId}");

                return Providers.InventoryTemplateData.GetFirstStoreName(entry, templateId);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Gets the inventory template with the given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="templateID"></param>
        /// <returns></returns>
		public virtual InventoryTemplate GetInventoryTemplate(LogonInfo logonInfo, RecordIdentifier templateID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, $"{Utils.Starting} {nameof(templateID)}: {templateID}");

                return Providers.InventoryTemplateData.Get(entry, templateID);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Saves the given inventory template
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="template"></param>
        /// <returns></returns>
		public virtual RecordIdentifier SaveInventoryTemplate(LogonInfo logonInfo, InventoryTemplate template)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, Utils.Starting);

                Providers.InventoryTemplateData.Save(entry, template);
				return template.ID;
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Inventory templates can be valid at all stores or specific stores referred to as a stored connection. This function saves a connection between a specific store to a specific inventory template.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="template"></param>
		public virtual void SaveInventoryTemplateStoreConnection(LogonInfo logonInfo, InventoryTemplateStoreConnection template)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, Utils.Starting);

                Providers.InventoryTemplateStoreConnectionData.Save(entry, template);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Inventory templates can be valid at all stores or specific stores referred to as a stored connection.
        /// This function deletes a connection between a specific given inventory template and a specific store.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="storeID"></param>
		public virtual void DeleteInventoryTemplateStoreConnection(LogonInfo logonInfo, RecordIdentifier storeID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, $"{Utils.Starting} {nameof(storeID)}: {storeID}");

                Providers.InventoryTemplateStoreConnectionData.DeleteForStore(entry, storeID);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Deletes all store connections for the given inventory template
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="templateID"></param>
		public virtual void DeleteInventoryTemplateTemplateConnection(LogonInfo logonInfo, RecordIdentifier templateID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, $"{Utils.Starting} {nameof(templateID)}: {templateID}");

                Providers.InventoryTemplateStoreConnectionData.DeleteForTemplate(entry, templateID);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Gets a list of the stores the inventory template is valid for
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="templateID"></param>
        /// <returns></returns>
		public virtual List<InventoryTemplateStoreConnection> GetInventoryTemplateStoreConnectionList(LogonInfo logonInfo, RecordIdentifier templateID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, $"{Utils.Starting} {nameof(templateID)}: {templateID}");

                return Providers.InventoryTemplateStoreConnectionData.GetList(entry, templateID);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Retrieves information about the inventory template with the given template ID for display in a list view
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="templateID"></param>
        /// <returns></returns>
		public virtual InventoryTemplateListItem GetInventoryTemplateListItem(LogonInfo logonInfo, RecordIdentifier templateID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, $"{Utils.Starting} {nameof(templateID)}: {templateID}");

                return Providers.InventoryTemplateData.GetListItem(entry, templateID);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Gets a list with all stock counting templates
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="templateEntryType">Type of template</param>
        /// <returns></returns>
		public virtual List<TemplateListItem> GetInventoryTemplatesByType(LogonInfo logonInfo, TemplateEntryTypeEnum templateEntryType)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, Utils.Starting);

                return Providers.InventoryTemplateData.GetInventoryTemplatesByType(entry, templateEntryType);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Deletes the inventory template with the given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="templateID"></param>
        /// <returns></returns>
		public virtual InventoryTemplateDeleteResult DeleteInventoryTemplate(LogonInfo logonInfo, RecordIdentifier templateID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, $"{Utils.Starting} {nameof(templateID)}: {templateID}");

                List<PurchaseWorksheet> worksheets = Providers.PurchaseWorksheetData.GetWorksheetsFromTemplateID(entry, templateID);
				foreach (PurchaseWorksheet worksheet in worksheets)
				{
					List<PurchaseWorksheetLine> lines = Providers.PurchaseWorksheetLineData.GetList(entry, worksheet.ID, true);
					if (lines.Count > 0)
					{
						return InventoryTemplateDeleteResult.OpenWorksheetExists;
					}
				}

				Providers.InventoryTemplateData.Delete(entry, templateID);

				return InventoryTemplateDeleteResult.None;
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Gets the inventory template section list for a given template ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="templateID"></param>
        /// <returns></returns>
		public virtual List<InventoryTemplateSection> GetInventoryTemplateSectionList(LogonInfo logonInfo, RecordIdentifier templateID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, $"{Utils.Starting} {nameof(templateID)}: {templateID}");

                return Providers.InventoryTemplateSectionData.GetList(entry, templateID);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Adds a filter section to a filter saved with the inventory template
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="templateSection"></param>
		public virtual void InsertInventoryTemplateSection(LogonInfo logonInfo, InventoryTemplateSection templateSection)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, Utils.Starting);

                Providers.InventoryTemplateSectionData.Insert(entry, templateSection);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Retrieves information about the filter that is saved with the given template ID and section ID.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="templateID"></param>
        /// <param name="sectionID"></param>
        /// <returns></returns>
		public virtual List<InventoryTemplateSectionSelection> GetInventoryTemplateSectionSelectionList(LogonInfo logonInfo, RecordIdentifier templateID, RecordIdentifier sectionID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, $"{Utils.Starting} {nameof(templateID)}: {templateID}, {nameof(sectionID)}: {sectionID}");

                return Providers.InventoryTemplateSectionSelectionData.GetList(entry, templateID, sectionID);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Adds the value selected in a filter section that is saved with the inventory template
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="templateSectionSelection"></param>
		public virtual void InsertInventoryTemplateSectionSelection(LogonInfo logonInfo, InventoryTemplateSectionSelection templateSectionSelection)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, Utils.Starting);

                Providers.InventoryTemplateSectionSelectionData.Insert(entry, templateSectionSelection);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Gets a list of inventory template section selection identifiers filtered by a given template ID and a section ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="templateID"></param>
        /// <param name="sectionID"></param>
        /// <returns></returns>
		public virtual List<RecordIdentifier> GetInventoryTemplateSectionSelectionListForFilter(LogonInfo logonInfo, RecordIdentifier templateID, RecordIdentifier sectionID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, $"{Utils.Starting} {nameof(templateID)}: {templateID}, {nameof(sectionID)}: {sectionID}");

                return Providers.InventoryTemplateSectionSelectionData.GetListForFilter(entry, templateID, sectionID);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Deletes all inventory template sections for a given template ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="templateID"></param>
		public virtual void DeleteInventoryTemplateSection(LogonInfo logonInfo, RecordIdentifier templateID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, $"{Utils.Starting} {nameof(templateID)}: {templateID}");

                Providers.InventoryTemplateSectionData.Delete(entry, templateID);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

		/// <summary>
		/// Gets a filtered list of inventory template items
		/// </summary>
		/// <param name="logonInfo">Log-on information</param>
		/// <param name="preview">If true, it will return only the first 50 items</param>
		/// <param name="filter">Container with IDs to filter</param>
		/// <returns></returns>
		public virtual List<InventoryTemplateFilterListItem> GetInventoryTemplateItems(LogonInfo logonInfo, InventoryTemplateFilterContainer filter)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, Utils.Starting);

                return Providers.InventoryTemplateItemFilterData.GetInventoryTemplateItems(entry, filter);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);
				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}


        /// <summary>
        /// Returns all items that match the item filter for given inventory template.
        /// </summary>
        /// <param name="logonInfo"></param>
        /// <param name="templateId">Id of the <see cref="InventoryTemplate"/></param>
        /// <param name="storeID">Store ID used in case of filtering by inventory on hand</param>
        /// <param name="getItemsWithNoVendor">If true, items that have no vendor will also be included</param>
        /// <returns></returns>
        public virtual List<InventoryTemplateFilterListItem> GetInventoryTemplateItems(LogonInfo logonInfo, RecordIdentifier templateId, RecordIdentifier storeID, bool getItemsWithNoVendor)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, Utils.Starting);

                return Providers.InventoryTemplateItemFilterData.GetInventoryTemplateItems(entry, templateId, storeID, getItemsWithNoVendor);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);
				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Deletes all filters for the given template ID and a section ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="templateID"></param>
        /// <param name="sectionID"></param>
		public virtual void DeleteInventoryTemplateSectionSelection(LogonInfo logonInfo, RecordIdentifier templateID, RecordIdentifier sectionID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, $"{Utils.Starting} {nameof(templateID)}: {templateID}, {nameof(sectionID)}: {sectionID}");

                Providers.InventoryTemplateSectionSelectionData.Delete(entry, templateID, sectionID);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Gets the inventory area with the given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="areaID"></param>
        /// <param name="usage"></param>
        /// <returns></returns>
		public virtual InventoryArea GetInventoryArea(LogonInfo logonInfo, RecordIdentifier areaID, UsageIntentEnum usage = UsageIntentEnum.Normal)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, $"{Utils.Starting} {nameof(areaID)}: {areaID}, {nameof(usage)}: {usage}");

                return Providers.InventoryAreaData.Get(entry, areaID, usage);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);
				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Gets a list with all available inventory areas
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
		public virtual List<InventoryArea> GetInventoryAreasList(LogonInfo logonInfo)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, Utils.Starting);

                return Providers.InventoryAreaData.GetList(entry);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);
				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Deletes the inventory area line with the given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="lineID"></param>
		public virtual void DeleteInventoryAreaLine(LogonInfo logonInfo, RecordIdentifier lineID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, $"{Utils.Starting} {nameof(lineID)}: {lineID}");

                Providers.InventoryAreaData.DeleteLine(entry, lineID);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);
				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Deletes the inventory area with the given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="areaID"></param>
		public virtual void DeleteInventoryArea(LogonInfo logonInfo, RecordIdentifier areaID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, $"{Utils.Starting} {nameof(areaID)}: {areaID}");

                Providers.InventoryAreaData.Delete(entry, areaID);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);
				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Saves the given inventory area
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="area"></param>
        /// <returns></returns>
		public virtual RecordIdentifier SaveInventoryArea(LogonInfo logonInfo, InventoryArea area)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, Utils.Starting);

                Providers.InventoryAreaData.Save(entry, area);
				return area.ID;
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);
				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Saves the given inventory area line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="line"></param>
        /// <returns></returns>
		public virtual RecordIdentifier SaveInventoryAreaLine(LogonInfo logonInfo, InventoryAreaLine line)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, Utils.Starting);

                Providers.InventoryAreaData.SaveLine(entry, line);
				return line.ID;
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);
				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Returns true if an inventory area is in use; otherwise returns false
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="areaLineID"></param>
        /// <returns></returns>
		public virtual bool AreaInUse(LogonInfo logonInfo, RecordIdentifier areaLineID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, $"{Utils.Starting} {nameof(areaLineID)}: {areaLineID}");

                return Providers.InventoryAreaData.AreaInUse(entry, areaLineID);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);
				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Gets the inventory area line with the given master ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="masterID"></param>
        /// <returns></returns>
		public virtual InventoryAreaLine GetInventoryAreaLine(LogonInfo logonInfo, RecordIdentifier masterID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, $"{Utils.Starting} {nameof(masterID)}: {masterID}");

                return Providers.InventoryAreaData.GetLine(entry, masterID);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);
				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Get a list with the lines of the given inventory area
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="areaID"></param>
        /// <returns></returns>
		public virtual List<InventoryAreaLine> GetInventoryAreaLinesByArea(LogonInfo logonInfo, RecordIdentifier areaID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, $"{Utils.Starting} {nameof(areaID)}: {areaID}");

                return Providers.InventoryAreaData.GetLinesByArea(entry, areaID);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);
				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}

        /// <summary>
        /// Gets a list with all area lines
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
		public virtual List<InventoryAreaLineListItem> GetInventoryAreaLinesListItems(LogonInfo logonInfo)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
                Utils.Log(this, Utils.Starting);

                return Providers.InventoryAreaData.GetAllAreaLines(entry);
			}
			catch (Exception ex)
			{
                Utils.LogException(this, ex);
				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
		}
    }
}