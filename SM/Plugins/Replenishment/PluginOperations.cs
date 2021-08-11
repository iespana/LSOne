using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.Replenishment.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Replenishment
{
    internal class PluginOperations
    {
        private static SiteServiceProfile siteServiceProfile;

        public static bool TestSiteService(bool displayMsg = true)
        {
            bool serviceConfigured = false;
            bool serviceActivce = false;
            bool serviceValid;

            SiteServiceProfile siteServiceProfile = GetSiteServiceProfile();
            serviceConfigured = siteServiceProfile != null;

            if (siteServiceProfile != null)
            {

                IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                ConnectionEnum result = service.TestConnection(PluginEntry.DataModel,
                    siteServiceProfile.SiteServiceAddress,
                    (ushort)siteServiceProfile.SiteServicePortNumber);

                PluginEntry.Framework.ViewController.CurrentView.HideProgress();

                if (result != ConnectionEnum.Success)
                {
                    if (displayMsg)
                    {
                        MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                    }
                }
                else
                {
                    serviceActivce = true;
                }
            }
            else
            {
                if (displayMsg)
                {
                    MessageDialog.Show(Resources.NoStoreServerIsSetUp);
                }
            }

            serviceValid = serviceActivce && serviceConfigured;
            if (!serviceValid && displayMsg)
            {
                IPlugin plugin = PluginEntry.Framework.FindImplementor(null, "CanViewAdministrationTab", null);

                if (plugin != null)
                {
                    plugin.Message(null, "ViewSiteServiceTab", null);
                }
            }
            return serviceValid;
        }


        public static SiteServiceProfile GetSiteServiceProfile()
        {
            if (siteServiceProfile == null)
            {
                Parameters parameters = Providers.ParameterData.Get(PluginEntry.DataModel);
                if (parameters != null)
                {
                    siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, parameters.SiteServiceProfile);
                }
            }

            return siteServiceProfile;
        }
        internal static void ShowInventoryTemplates(object sender, EventArgs args)
        {
            if(TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.InventoryTemplatesView());
            }
        }

        internal static void ShowInventoryTemplates(RecordIdentifier selectedId)
        {
            if (TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.InventoryTemplatesView(selectedId));
            }
        }

        internal static void ShowPurchaseWorksheets(object sender, EventArgs args)
        {
            if(TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.PurchaseWorksheetsView());
            }
        }

        internal static void ShowPurchaseWorksheet(RecordIdentifier purchaseWorksheetId)
        {
            if (TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.PurchaseWorksheetView(purchaseWorksheetId));
            }
        }

        internal static void ShowPurchaseWorksheet(object sender, EventArgs args)
        {
            if (args is PluginOperationArguments && TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.PurchaseWorksheetView(((PluginOperationArguments)args).ID));
            }
        }

        internal static void ShowPurchaseWorksheet(RecordIdentifier purchaseWorksheetId, RecordIdentifier selectedLineId)
        {
            if(TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.PurchaseWorksheetView(purchaseWorksheetId, selectedLineId));
            }
        }

        internal static bool DeletePurchaseWorksheetLine(RecordIdentifier purchaseWorksheetLineId)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageReplenishment))
            {
                if (QuestionDialog.Show(Properties.Resources.DeletePurchaseWorksheetLineQuestion,
                                        Properties.Resources.DeletePurchaseWorksheetLine) == DialogResult.Yes)
                {
                    try
                    {
                        var line =
                            Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                                .GetPurchaseWorksheetLine(PluginEntry.DataModel, siteServiceProfile,
                                    purchaseWorksheetLineId, false);



                        line.Deleted = true;
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).SaveInventoryWorksheetLine(PluginEntry.DataModel, siteServiceProfile, line, true);

                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                        return false;
                    }
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "PurchaseWorksheet", purchaseWorksheetLineId, null);

                    return true;
                }
            }

            return false;
        }

        internal static bool DeletePurchaseWorksheetLines(List<RecordIdentifier> purchaseWorksheetLineIds)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageReplenishment))
            {
                if (QuestionDialog.Show(Properties.Resources.DeletePurchaseWorksheetLinesQuestion,
                                        Properties.Resources.DeletePurchaseWorksheetLines) == DialogResult.Yes)
                {
                    foreach (var purchaseWorksheetLineId in purchaseWorksheetLineIds)
                    {
                        try
                        {
                            var line = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetPurchaseWorksheetLine(PluginEntry.DataModel, siteServiceProfile, purchaseWorksheetLineId, false);
                            line.Deleted = true;
                            Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).SaveInventoryWorksheetLine(PluginEntry.DataModel, siteServiceProfile, line, true);


                        }
                        catch (Exception ex)
                        {
                            MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                            return false;
                        }

                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "PurchaseWorksheet", null, null);

                    return true;
                }
            }

            return false;
        }

        internal static void DeleteInventoryTemplate(RecordIdentifier templateID)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageInventoryTemplates))
            {
                if (QuestionDialog.Show(Resources.DeleteInventoryTemplateQuestion,
                                        Resources.DeleteInventoryTemplate) == DialogResult.Yes)
                {
                    try
                    {
                        if (Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).DeleteInventoryTemplate(PluginEntry.DataModel, siteServiceProfile, templateID, true) == InventoryTemplateDeleteResult.OpenWorksheetExists)
                        {
                            MessageDialog.Show(Resources.TemplateCannotBeDeletedWhenOpenWorksheetsAreAttached);
                        }
                        else
                        {
                            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "InventoryTemplate", templateID, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                    }
                }
            }
        }

        internal static bool DeleteInventoryTemplates(List<RecordIdentifier> templateIDs)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageInventoryTemplates))
            {
                if (QuestionDialog.Show(Resources.DeleteInventoryTemplatesQuestion,
                                        Resources.DeleteInventoryTemplates) == DialogResult.Yes)
                {
                    foreach (var templateID in templateIDs)
                    {
                        try
                        {

                            Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).DeleteInventoryTemplate(PluginEntry.DataModel, siteServiceProfile, templateID, true);


                        }
                        catch (Exception ex)
                        {
                            MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);

                        }

                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTemplate", null, null);

                    return true;
                }
            }

            return false;
        }

        internal static RecordIdentifier CreateInventoryTemplate(InventoryTemplate inventoryTemplate,
            bool allStoresChecked, List<DataEntity> storeList, RecordIdentifier copyExistingTemplateID)
        {
            InventoryTemplate existingTemplate = null;
            try
            {
                if (copyExistingTemplateID != "")
                {

                    existingTemplate = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                        .GetInventoryTemplate(PluginEntry.DataModel, siteServiceProfile, copyExistingTemplateID, false);


                    existingTemplate.Text = inventoryTemplate.Text;
                    existingTemplate.AllStores = inventoryTemplate.AllStores;
                    existingTemplate.ID = RecordIdentifier.Empty;
                    existingTemplate.TemplateEntryType = inventoryTemplate.TemplateEntryType;

                    inventoryTemplate = existingTemplate;
                }

                inventoryTemplate.ID = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                    .SaveInventoryTemplate(PluginEntry.DataModel, siteServiceProfile, inventoryTemplate, false);

                var newTemplateId = inventoryTemplate.ID;
                if (!allStoresChecked)
                {
                    InventoryTemplateStoreConnection connection;
                    foreach (var entity in storeList)
                    {
                        connection = new InventoryTemplateStoreConnection();
                        connection.TemplateID = inventoryTemplate.ID;
                        connection.StoreID = entity.ID;
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                            .SaveInventoryTemplateStoreConnection(PluginEntry.DataModel, siteServiceProfile, connection,
                                false);

                    }
                }
                else
                {
                    InventoryTemplateStoreConnection connection;
                    storeList = Providers.StoreData.GetList(PluginEntry.DataModel);
                    foreach (var entity in storeList)
                    {
                        connection = new InventoryTemplateStoreConnection();
                        connection.TemplateID = inventoryTemplate.ID;
                        connection.StoreID = entity.ID;
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                            .SaveInventoryTemplateStoreConnection(PluginEntry.DataModel, siteServiceProfile, connection,
                                false);

                    }
                }

                if (inventoryTemplate.TemplateEntryType == TemplateEntryTypeEnum.PurchaseOrder)
                {
                    // Create purchase worksheets
                    List<InventoryTemplateStoreConnection> storesConnectedToTemplate = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                            .GetInventoryTemplateStoreConnectionList(PluginEntry.DataModel, siteServiceProfile, inventoryTemplate.ID, false);

                    foreach (var storeConnection in storesConnectedToTemplate)
                    {
                        var pw = new PurchaseWorksheet();
                        pw.StoreId = storeConnection.StoreID;
                        pw.InventoryTemplateID = newTemplateId;
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).SaveInventoryWorksheet(PluginEntry.DataModel, siteServiceProfile, pw, false);
                    }
                }
                    

                if (copyExistingTemplateID != "")
                {
                    var templateSections =
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                            .GetInventoryTemplateSectionList(PluginEntry.DataModel, siteServiceProfile,
                                copyExistingTemplateID,
                                false);


                    foreach (var templateSection in templateSections)
                    {
                        templateSection.TemplateID = newTemplateId;

                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                            .InsertInventoryTemplateSection(PluginEntry.DataModel, siteServiceProfile, templateSection,
                                false);

                        var templateSectionSelections =
                            Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                                .GetInventoryTemplateSectionSelectionList(PluginEntry.DataModel, siteServiceProfile,
                                    copyExistingTemplateID, templateSection.SectionID,
                                    false);

                        foreach (var templateSectionSelection in templateSectionSelections)
                        {
                            templateSectionSelection.TemplateID = newTemplateId;

                            Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                                .InsertInventoryTemplateSectionSelection(PluginEntry.DataModel, siteServiceProfile,
                                    templateSectionSelection,
                                    false);
                        }
                    }
                }

                return newTemplateId;
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return null;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }

        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Resources.StoreSetup, "Store setup", null), 75);
            args.Add(new Category(Resources.GeneralSetup, "General setup", null), 100);

        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {

            if (args.CategoryKey == "General setup")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageReplenishment))
                {
                    args.Add(new Item(Resources.Inventory, "Inventory", null), 300);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "General setup" && args.ItemKey == "Inventory")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageReplenishment))
                {                    
                    args.Add(new ItemButton(Resources.PurchaseWorksheets, Resources.PurchaseWorksheetsDescription, ShowPurchaseWorksheets), 810);
                }
                if (PluginEntry.DataModel.HasPermission(Permission.ManageInventoryTemplates))
                {
                    args.Add(new ItemButton(Resources.InventoryTemplates, Resources.InventoryTemplatesDescription, ShowInventoryTemplates), 800);                 
                }
            }
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Resources.StoreSetup, "Inventory"), 150);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Inventory")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageReplenishment) || PluginEntry.DataModel.HasPermission(Permission.ManageInventoryTemplates))
                {
                    args.Add(new PageCategory(Resources.Purchase, "Purchase"), 100);
                    args.Add(new PageCategory(Resources.General, "General"), 500);
                }
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Inventory" && args.CategoryKey == "Purchase")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageReplenishment))
                {
                    args.Add(new CategoryItem(
                                 Resources.PurchaseWorksheet,
                                 Resources.PurchaseWorksheets,
                                 Resources.PurchaseWorksheetsTooltipDescription,
                                 CategoryItem.CategoryItemFlags.Button,
                                 Resources.worksheets_16,
                                 null,
                                 ShowPurchaseWorksheets,
                                 "PurchaseWorksheets"),
                             40);
                }
            }

            if(args.PageKey == "Inventory" && args.CategoryKey == "General")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageInventoryTemplates))
                {
                    args.Add(new CategoryItem(
                             Resources.Templates,
                             Resources.InventoryTemplates,
                             Resources.InventoryTemplatesTooltipDescription,
                             CategoryItem.CategoryItemFlags.Button,
                             null,
                             Resources.templates_32,
                             ShowInventoryTemplates,
                             "ShowInventoryTemplates"),
                         10);
                }
            }
        }

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {

        }

        internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            TabControl.Tab tab;
            switch (args.ContextName)
            {
                case "LSOne.ViewPlugins.RetailItems.Views.ItemView":
                    if (PluginEntry.DataModel.HasPermission(Permission.ManageReplenishment))
                    {
                        tab = new TabControl.Tab(Resources.Replenishment, ItemTabKey.Replenishment.ToString(), ViewPages.RetailItemReplenishmentPage.CreateInstance);
                        if (((RetailItem)args.InternalContext).ItemType == ItemTypeEnum.Service || ((RetailItem)args.InternalContext).ItemType == ItemTypeEnum.MasterItem || ((RetailItem)args.InternalContext).ItemType == ItemTypeEnum.AssemblyItem)
                        {
                            tab.Enabled = false;
                        }
                        args.Add(tab, 130);
                    }
                    break;
            }
        }

        internal static void PostPurchaseWorksheet(RecordIdentifier purchaseWorksheetID)
        {
            if (TestSiteService())
            {
                if (QuestionDialog.Show(Resources.PostPurchaseWorksheetQuestion, Resources.PostPurchaseWorksheet) ==
                    DialogResult.Yes)
                {
                    try
                    {
                        IInventoryService inventoryService = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel);
                        

                        if (inventoryService.PurchaseWorksheetHasInventoryExcludedItems(PluginEntry.DataModel, siteServiceProfile, purchaseWorksheetID, true)
                            && QuestionDialog.Show(Resources.PostPurchaseWorksheetNonInventoryItemsQuestion, Resources.PostPurchaseWorksheet) == DialogResult.No)
                        {
                            return;
                        }

                        PostPurchaseWorksheetContainer result = inventoryService.PostPurchaseWorksheet(PluginEntry.DataModel, siteServiceProfile, purchaseWorksheetID, true);

                        if(result.Result == PostPurchaseWorksheetResult.Success)
                        {
                            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "PurchaseWorksheet", purchaseWorksheetID, null);
                            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "PurchaseOrder", RecordIdentifier.Empty, null);

                            // Show purchase order(s) if the user wishes
                            if (result.CreatedPurchaseOrderIDs.Count == 1)
                            {
                                var purchaseOrderPlugin = PluginEntry.Framework.FindImplementor(null, "CanViewPurchaseOrder", result.CreatedPurchaseOrderIDs[0]);
                                if (purchaseOrderPlugin != null && QuestionDialog.Show(Resources.PurchaseOrderCreatedQuestion, Resources.PurchaseOrderCreated) == DialogResult.Yes)
                                {
                                    purchaseOrderPlugin.Message(null, "ViewPurchaseOrder", result.CreatedPurchaseOrderIDs[0]);
                                }
                            }
                            else
                            {
                                var purchaseOrderPlugin = PluginEntry.Framework.FindImplementor(null, "CanViewPurchaseOrders", null);

                                if (purchaseOrderPlugin != null && QuestionDialog.Show(Resources.PurchaseOrdersCreatedQuestion.Replace("#1", result.CreatedPurchaseOrderIDs.Count.ToString()), Resources.PurchaseOrdersCreated) == DialogResult.Yes)
                                {
                                    purchaseOrderPlugin.Message(null, "ViewPurchaseOrders", RecordIdentifier.Empty);
                                }
                            }
                        }
                        else
                        {
                            switch (result.Result)
                            {
                                case PostPurchaseWorksheetResult.NoItems:
                                    MessageDialog.Show(Resources.PostPurchaseWorksheetNoItems, Resources.PostPurchaseWorksheet);
                                    break;
                                case PostPurchaseWorksheetResult.NonVendorItems:
                                    MessageDialog.Show(Resources.PostPurchaseWorksheetNonVendorItems, Resources.PostPurchaseWorksheet);
                                    break;
                                case PostPurchaseWorksheetResult.TemplateNotFound:
                                    MessageDialog.Show(Resources.PostPurchaseWorksheetTemplateNotFound, Resources.PostPurchaseWorksheet);
                                    break;
                                case PostPurchaseWorksheetResult.Error:
                                    MessageDialog.Show(Resources.PostPurchaseWorksheetError, Resources.PostPurchaseWorksheet);
                                    break;
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        PluginEntry.Framework?.LogMessage(Utilities.ErrorHandling.LogMessageType.Error, ex.Message, ex);
                        MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                        return;
                    }
                    finally
                    {
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
                    }
                }
            }
        }

        internal static void ResetPurchaseWorksheetLines(RecordIdentifier purchaseWorksheetId)
        {
            Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).DeleteInventoryWorksheetLineForPurchaseWorksheet(PluginEntry.DataModel, siteServiceProfile, purchaseWorksheetId, true);
            CreatePurchaseWorksheetLines(purchaseWorksheetId);
        }

        internal static void EnteringPurchaseWorksheetView(RecordIdentifier purchaseWorksheetId)
        {
            List<PurchaseWorksheetLine> currentLines =
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                    .GetInventoryWorksheetLineListSimple(PluginEntry.DataModel, siteServiceProfile, purchaseWorksheetId, false, true);

            if (currentLines.Count == 0)
            {
                CreatePurchaseWorksheetLines(purchaseWorksheetId);
            }
        }

        internal static void RefreshPurchaseWorksheetLines(RecordIdentifier inventoryTemplateId, RecordIdentifier purchaseWorksheetId)
        {
            Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).RefreshPurchaseWorksheetLines(PluginEntry.DataModel, siteServiceProfile, purchaseWorksheetId, true);
        }

        internal static void CreatePurchaseWorksheetLines(RecordIdentifier purchaseWorksheetId)
        {
            Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).CreatePurchaseWorksheetLinesFromFilter(PluginEntry.DataModel, siteServiceProfile, purchaseWorksheetId, true);
        }

        internal static decimal GetEffectiveInventory(PurchaseWorksheetLine purchaseWorksheetItem)
        {
            try
            {
                return Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetEffectiveInventoryForItem(PluginEntry.DataModel, siteServiceProfile, purchaseWorksheetItem.Item.ID, purchaseWorksheetItem.StoreID, true);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return 0;
            }
        }

        internal static decimal GetSuggestedQuantity(RecordIdentifier itemId, RecordIdentifier storeId, RecordIdentifier unitId)
        {
            try
            {
                return Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).CalculateSuggestedQuantity(PluginEntry.DataModel, siteServiceProfile, itemId, storeId, unitId, true);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return 0;
            }
        }

        internal static void DeleteReplenishmentSetting(RecordIdentifier selectedId)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageReplenishment))
            {
                if (QuestionDialog.Show(Resources.DeleteReplenishmentSettingQuestion, Resources.DeleteReplenishmentSetting) == DialogResult.Yes)
                {
                    try
                    {
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).DeleteItemReplenishmentSetting(PluginEntry.DataModel, siteServiceProfile, selectedId,
                                true);

                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete,
                            "ReplenishmentSetting", selectedId, null);
                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                        return;
                    }
                }
            }
        }

        internal static void DeleteReplenishmentSettings(List<RecordIdentifier> selectedIDs)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageReplenishment))
            {
                if (QuestionDialog.Show(Resources.DeleteReplenishmentSettingsQuestion, Resources.DeleteReplenishmentSettings) == DialogResult.Yes)
                {
                    try
                    {
                        foreach (var id in selectedIDs)
                        {
                            Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).DeleteItemReplenishmentSetting(PluginEntry.DataModel, siteServiceProfile, id,
                                false);
                        }

                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "ReplenishmentSetting", null, selectedIDs);

                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                        return;
                    }
                    finally
                    {
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
                    }
                }
            }
        }

        internal static bool UnitConversionWithInventoryUnitExists(DataEntity itemDataEntity, RecordIdentifier unitId)
        {
            RecordIdentifier itemInventoryUnit = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, itemDataEntity.ID, RetailItem.UnitTypeEnum.Inventory);
            while (!Providers.UnitConversionData.UnitConversionRuleExists(PluginEntry.DataModel, itemDataEntity.ID, unitId, itemInventoryUnit))
            {
                IPlugin unitConversionAdder = PluginEntry.Framework.FindImplementor(null, "CanAddUnitConversionsSiteService", null);

                if (unitConversionAdder != null)
                {

                    if (QuestionDialog.Show(
                        Resources.UnitConversionQuestion,
                        Resources.UnitConversionRuleMissing) == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (!(bool)unitConversionAdder.Message(null, "AddUnitConversionIncludeSiteService", new object[] { itemDataEntity, itemInventoryUnit, unitId }))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    MessageDialog.Show(Resources.UnitConversionRuleMissingAlert);
                    return false;
                }
            }

            return true;
        }

        internal static DataEntity GetInventoryTemplatesSelectedVendor(RecordIdentifier templateId)
        {
            try
            {
                var vendorList = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryTemplateSectionSelectionList(PluginEntry.DataModel, siteServiceProfile, templateId, "Vendor",
                                false);
                DataEntity selectedVendorEntity = new DataEntity();
                if (vendorList.Count > 0)
                {
                    var selectedVendor = vendorList[0];
                    selectedVendorEntity =
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                            .GetVendor(PluginEntry.DataModel, siteServiceProfile, selectedVendor.EntityID,
                                true);
                }
                return selectedVendorEntity;
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);

            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }

            return null;
        }

        public static void BlockItemReplenishmentChangeItemType(RecordIdentifier itemID)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageReplenishment)
                 || PluginEntry.DataModel.HasPermission(Permission.ManageItemTypes))
            {
                try
                {
                    var service = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel);
                    var serviceProfile = PluginOperations.GetSiteServiceProfile();

                    var setting = service.GetItemReplenishmentSettingForItem(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),
                                                                            itemID,
                                                                            true);
                    if (setting == null)
                    {
                        setting = new ItemReplenishmentSetting
                        {
                            ItemId = itemID,
                            ReplenishmentMethod = ReplenishmentMethodEnum.StockLevel,
                            BlockedForReplenishment = BlockedForReplenishmentEnum.BlockedForReplenishment,
                            PurchaseOrderMultipleRounding = PurchaseOrderMultipleRoundingEnum.Nearest,
                            ReorderPoint = 0,
                            MaximumInventory = 0,
                            PurchaseOrderMultiple = 0,
                            BlockingDate = DateTime.Today
                        };
                    }
                    else
                    {
                        setting.BlockedForReplenishment = BlockedForReplenishmentEnum.BlockedForReplenishment;
                        setting.BlockingDate = DateTime.Today;
                    }
                    service.SaveItemReplenishmentSetting(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), setting, true);

                    var storeSettings = service.GetItemReplenishmentSettingListForStores(PluginEntry.DataModel, serviceProfile, true, itemID);
                    if (storeSettings != null && storeSettings.Count > 0)
                    {
                        for (int i = 0; i < storeSettings.Count; i++)
                        {
                            storeSettings[i].BlockedForReplenishment = BlockedForReplenishmentEnum.BlockedForReplenishment;
                            storeSettings[i].BlockingDate = DateTime.Now;
                            service.SaveItemReplenishmentSetting(PluginEntry.DataModel, serviceProfile, storeSettings[i], false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                }
            }
        }

        public static void BlockItemReplenishmentNewServiceItem(RecordIdentifier itemID)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageReplenishment)
                 || PluginEntry.DataModel.HasPermission(Permission.ManageItemTypes))
            {
                try
                {
                    var setting = Providers.ItemReplenishmentSettingData.GetForItem(PluginEntry.DataModel, itemID, true);
                    if (setting == null)
                    {
                        setting = new ItemReplenishmentSetting
                        {
                            ItemId = itemID,
                            ReplenishmentMethod = ReplenishmentMethodEnum.StockLevel,
                            BlockedForReplenishment = BlockedForReplenishmentEnum.BlockedForReplenishment,
                            PurchaseOrderMultipleRounding = PurchaseOrderMultipleRoundingEnum.Nearest,
                            ReorderPoint = 0,
                            MaximumInventory = 0,
                            PurchaseOrderMultiple = 0,
                            BlockingDate = DateTime.Today
                        };
                    }
                    else
                    {
                        setting.BlockedForReplenishment = BlockedForReplenishmentEnum.BlockedForReplenishment;
                        setting.BlockingDate = DateTime.Today;
                    }
                    Providers.ItemReplenishmentSettingData.Save(PluginEntry.DataModel, setting);
                    
                    var storeSettings = Providers.ItemReplenishmentSettingData.GetListForStores(PluginEntry.DataModel, itemID, true);
                    if (storeSettings != null && storeSettings.Count > 0)
                    {
                        for (int i = 0; i < storeSettings.Count; i++)
                        {
                            storeSettings[i].BlockedForReplenishment = BlockedForReplenishmentEnum.BlockedForReplenishment;
                            storeSettings[i].BlockingDate = DateTime.Now;
                            Providers.ItemReplenishmentSettingData.Save(PluginEntry.DataModel, storeSettings[i]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                }
            }
        }
    }
}
