using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.RetailItems.Dialogs;
using ListView = System.Windows.Forms.ListView;
using LSOne.ViewCore;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.ViewPlugins.RetailItems.Properties;
using LSOne.Services.Interfaces;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using System.Linq;

namespace LSOne.ViewPlugins.RetailItems
{
    internal class PluginOperations
    {
        private static SiteServiceProfile siteServiceProfile;

        private static ViewBase orderView;

        public static bool TestSiteService(IConnectionManager entry)
        {
            SiteServiceProfile siteServiceProfile = GetSiteServiceProfile();

            if (siteServiceProfile != null)
            {
                IInventoryService service = (IInventoryService)entry.Service(ServiceType.InventoryService);
                ConnectionEnum result = service.TestConnection(entry, siteServiceProfile.SiteServiceAddress,
                                                                      (ushort)siteServiceProfile.SiteServicePortNumber);

                return (result == ConnectionEnum.Success);
            }

            return false;
        }

        public static void ShowItemsSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ItemsView());
        }

        public static void ShowItemsSheet(RecordIdentifier args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ItemsView());
        }

        public static void ShowItemSheet(RecordIdentifier itemID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ItemView(itemID,null));
        }

        public static void BulkEditItems(IEnumerable<IDataEntity> items)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ItemView( items));
        }

        public static void ShowItemSheet(RecordIdentifier itemID, IEnumerable<IDataEntity> context)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ItemView(itemID, context));
        }

        public static void ShowItemSheetContextHandler(object sender, EventArgs args)
        {
            ListView lv = (ListView)PluginEntry.Framework.GetContextMenuContext();

            ShowItemSheet((RecordIdentifier)lv.SelectedItems[0].Tag, null);
        }

        public static void ShowRetailGroupListView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.RetailGroupsView());
        }

        public static void ShowRetailDivisionListView(RecordIdentifier retailDivisionID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.RetailDivisionsView(retailDivisionID));
        }

        public static void ShowRetailDivisionListView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.RetailDivisionsView());
        }

        public static void ShowRetailGroupListView(RecordIdentifier retailGroupID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.RetailGroupsView(retailGroupID));
        }

        public static void ShowSpecialGroupListView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.SpecialGroupsView());
        }

        public static void ShowSpecialGroupListView(RecordIdentifier specialGroupID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.SpecialGroupsView(specialGroupID));
        }

        public static void EditRetailDivision(RecordIdentifier retailDivisionId)
        {
            using (RetailDivisionDialog dialog = new RetailDivisionDialog(retailDivisionId))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    RecordIdentifier selectedID = dialog.retailDivision.ID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "RetailDivision", selectedID, null);
                }
            }
        }

        public static void ShowRetailGroupView(RecordIdentifier retailGroupID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.RetailGroupView(retailGroupID));
        }

        public static void ShowRetailDepartmentListView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.RetailDepartmentsView());
        }

        public static void ShowRetailDepartmentListView(RecordIdentifier retailDepartmentID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.RetailDepartmentsView(retailDepartmentID));
        }

        public static void NewItem(object sender, EventArgs args)
        {
            NewItem();
        }

        public static void DeleteItems(List<SimpleRetailItem> items)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit))
            {
                List<DataEntity> componentItems = Providers.RetailItemAssemblyComponentData.WhichItemsAreComponentsOfAssemblies(
                    PluginEntry.DataModel, 
                    items.Select(i => i.ID).ToList());

                if (componentItems.Count > 0)
                {
                    string message = Properties.Resources.CannotDeleteItemsPartOfAssemblies + Environment.NewLine;

                    foreach (var item in componentItems)
                    {
                        message += String.Format("{0}   - {1} - {2}", Environment.NewLine, item.ID, item.Text);
                    }

                    MessageDialog.Show(message);
                }
                else if (QuestionDialog.Show(Properties.Resources.DeleteRetailItemsQuestion, Properties.Resources.DeleteRetailItems) == DialogResult.Yes)
                {
                    List<RecordIdentifier> itemIDs = new List<RecordIdentifier>();

                    foreach (SimpleRetailItem item in items)
                    {
                        Providers.RetailItemData.Delete(PluginEntry.DataModel, item.MasterID);
                        Providers.LinkedItemData.Delete(PluginEntry.DataModel, item.ID);
                        Providers.BarCodeData.DeleteWithItemID(PluginEntry.DataModel, item.ID);
                        itemIDs.Add(item.ID);
                    }
                    PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.RetailItemsDashboardItemID);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "RetailItem", null, itemIDs);
                }
            }
        }

        public static void UndeleteItems(List<SimpleRetailItem> items)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit))
            {
                if (QuestionDialog.Show(Resources.UndeleteRetailItemsQuestion, Properties.Resources.DeleteRetailItems) == DialogResult.Yes)
                {
                    List<RecordIdentifier> itemIDs = new List<RecordIdentifier>();

                    foreach (SimpleRetailItem item in items)
                    {
                        Providers.RetailItemData.Undelete(PluginEntry.DataModel, item.MasterID);
                        Providers.BarCodeData.UndeleteBarcodeWithItemID(PluginEntry.DataModel, item.ID);

                        itemIDs.Add(item.ID);
                    }
                    PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.RetailItemsDashboardItemID);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "RetailItem", null, itemIDs);
                }
            }
        }

        public static bool DeleteItem(RecordIdentifier itemID, RecordIdentifier itemMasterID)
        {
            bool retValue = false;

            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit))
            {
                List<DataEntity> assemblyItems = Providers.RetailItemAssemblyComponentData.GetAssemblyItemsForComponentItem(PluginEntry.DataModel, itemID);
                if (assemblyItems.Count > 0)
                {
                    string message = assemblyItems.Count > 1 
                        ? Properties.Resources.CannotDeleteItemPartOfAssemblies
                        : Properties.Resources.CannotDeleteItemPartOfAssembly;

                    message += Environment.NewLine;

                    foreach (var assemblyItem in assemblyItems)
                    {
                        message += String.Format("{0}   - {1} - {2}", Environment.NewLine, assemblyItem.ID, assemblyItem.Text);
                    }

                    MessageDialog.Show(message);
                }
                else if (Providers.RetailItemData.IsDeleted(PluginEntry.DataModel, itemMasterID))
                {
                    if (QuestionDialog.Show(Properties.Resources.UndeleteRetailItemQuestion, Properties.Resources.UndeleteRetailItem) == DialogResult.Yes)
                    {
                        Providers.RetailItemData.Undelete(PluginEntry.DataModel, itemMasterID);
                        Providers.BarCodeData.UndeleteBarcodeWithItemID(PluginEntry.DataModel, itemID);
                        PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.RetailItemsDashboardItemID);

                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Undelete, "RetailItem", itemID, null);
                    }
                }
                else
                {
                    if (QuestionDialog.Show(Properties.Resources.DeleteRetailItemQuestion, Properties.Resources.DeleteRetailItem) == DialogResult.Yes)
                    {
                        Providers.RetailItemData.Delete(PluginEntry.DataModel, itemMasterID);
                        Providers.LinkedItemData.Delete(PluginEntry.DataModel, itemID);
                        Providers.BarCodeData.DeleteWithItemID(PluginEntry.DataModel, itemID);
                        PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.RetailItemsDashboardItemID);

                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "RetailItem", itemID, null);

                        retValue = true;
                    }
                }
            }

            return retValue;
        }

        public static void DeleteItemContextHandler(object sender, EventArgs args)
        {
            ListView lv = (ListView)PluginEntry.Framework.GetContextMenuContext();

            DeleteItem(((SearchListViewItem)lv.SelectedItems[0]).ID, ((SearchListViewItem)lv.SelectedItems[0]).MasterID);
        }

        public static RetailDivision NewRetailDivision(object sender, EventArgs args)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailDivisions))
            {
                using (var dialog = new RetailDivisionDialog())
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        // We put null in sender so that we also get our own change hint sent.
                        RecordIdentifier selectedID = dialog.retailDivision.ID;
                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "RetailDivision", selectedID, null);

                        return dialog.retailDivision;
                    }
                }
            }

            return null;
        }

        public static void NewRetailGroup(object sender, EventArgs args)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailGroups))
            {
                using (NewRetailGroupDialog dlg = new NewRetailGroupDialog())
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        // We put null in sender so that we also get our own change hint sent.
                        RecordIdentifier selectedID = dlg.retailGroup.ID;
                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "RetailGroup", selectedID, null);

                        ShowRetailGroupView(selectedID);
                    }
                }
            }
        }

        public static void NewRetailDepartment(object sender, EventArgs args)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailDepartments))
            {
                try
                {

                
                using (RetailDepartmentDialog dlg = new RetailDepartmentDialog())
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        // We put null in sender so that we also get our own change hint sent.
                        RecordIdentifier selectedID = dlg.RetailDepartmentID;
                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "RetailDepartment", selectedID, null);
                    }
                }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public static void EditRetailDepartment(RecordIdentifier departmentID)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailDepartments))
            {
                RetailDepartmentDialog dlg = new RetailDepartmentDialog(departmentID, false);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    RecordIdentifier selectedID = dlg.RetailDepartmentID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "RetailDepartment", selectedID, null);
                }
            }
        }

        public static void DeleteRetailDivision(RecordIdentifier id)
        {
            Providers.RetailDivisionData.Delete(PluginEntry.DataModel, id);
        }

        public static void DeleteRetailGroup(RecordIdentifier id)
        {
            Providers.RetailGroupData.Delete(PluginEntry.DataModel, id);
        }

        public static void DeleteRetailDepartment(RecordIdentifier id)
        {
            Providers.RetailDepartmentData.Delete(PluginEntry.DataModel, id);
        }

        public static RecordIdentifier NewItem()
        {
            return NewItem(true, true);
        }

        public static RecordIdentifier NewItem(bool showItemSheet, bool allowCreateAnother)
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit) || PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ManagePurchaseOrders))
            {
                using (NewRetailItemDialog dlg = new NewRetailItemDialog(allowCreateAnother))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        // We put null in sender so that we also get our own change hint sent.
                        selectedID = dlg.ItemID;
                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "RetailItem", selectedID, null);

                        if (showItemSheet)
                        {
                            ShowItemSheet(selectedID, null);
                        }
                    }
                }
            }

            return selectedID;
        }

        internal static void EditRetailItem(object sender, PluginOperationArguments args)
        {
            if (args.WantsViewReturned)
            {
                args.ResultView = new Views.ItemView(args.ID, null);
            }
        }

        internal static void EditRetailItems(object sender, PluginOperationArguments args)
        {
            if (args.WantsViewReturned)
            {
                args.ResultView = new Views.ItemView((IEnumerable<IDataEntity>)args.Param);
            }
        }

        internal static void EditSpecialGroups(object sender, PluginOperationArguments args)
        {        
            PluginEntry.Framework.ViewController.Add(new Views.SpecialGroupsView(args.ID));
        }

        internal static void ImportImages(object sender, EventArgs args)
        {
            ImportImagesDialog dlg = new ImportImagesDialog(PluginEntry.DataModel);
            dlg.ShowDialog(PluginEntry.Framework.MainWindow);
        }

        

        public static void AddSearchHandler(object sender, SearchBarConstructionArguments args)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit))
            {
                PluginEntry.retailItemSearchProvider = new RetailItemSearchPanelFactory();

                args.AddItem(Properties.Resources.RetailItems, -1, PluginEntry.retailItemSearchProvider, 100);
            }
        }
        /// <summary>
        /// Returns the selected site service profile for the Site Manager. If no site service profile has been selected then the function returns null
        /// </summary>
        /// <returns>The Site managers site service profile. Returns null if no site service profile has been selected</returns>
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
        private static void SearchItems (object sender, SearchEventArgs args)
        {
            PluginEntry.Framework.Search(PluginEntry.retailItemSearchProvider, args.Text);
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Properties.Resources.Item, "Item"), 50);
        }



        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Item")
            {
                args.Add(new PageCategory(Properties.Resources.Items, "Items"), 100);
                args.Add(new PageCategory(Properties.Resources.Hierarchy, "Hierarchy"), 200);
            }

        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Item" && args.CategoryKey == "Items")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.New_RetailItem,
                            Properties.Resources.RetailItemNew,
                            Properties.Resources.NewRetailItemDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            null,
                            Properties.Resources.new_retail_item_32,
                            new EventHandler(NewItem),
                            "NewRetailItem"), 10);
                }

                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsView))
                {
                    args.Add(new CategoryItem(
                        Properties.Resources.Items,
                        Properties.Resources.ViewItems,
                        Properties.Resources.ViewItemsDescription,
                        CategoryItem.CategoryItemFlags.Button | CategoryItem.CategoryItemFlags.BeginOfGroup,
                        Properties.Resources.item_16,
                        new EventHandler(ShowItemsSheet),
                        "ViewRetailItems"), 20);
                }
            }
            if (args.PageKey == "Item" && args.CategoryKey == "Hierarchy")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailDivisions))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.Divisions,
                            Properties.Resources.ViewRetailDivisions,
                            Properties.Resources.ViewRetailDivisionDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            Properties.Resources.divisions_16,
                            new EventHandler(ShowRetailDivisionListView),
                            "Divisions"), 10);
                }



                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailDepartments))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.Departments,
                            Properties.Resources.ViewRetailDepartments,
                            Properties.Resources.ViewRetailDepartmentsDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            Properties.Resources.departments_16,
                            new EventHandler(ShowRetailDepartmentListView),
                            "Departments"), 20);
                }

                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailGroups))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.Groups_Retail,
                            Properties.Resources.ViewRetailGroup,
                            Properties.Resources.ViewRetailGroupsDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            Properties.Resources.groups_16,
                            new EventHandler(ShowRetailGroupListView),
                            "Groups"), 30);
                }

                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageSpecialGroups))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.SpecialGroups,
                            Properties.Resources.ViewSpecialGroups,
                            Properties.Resources.ViewSpecialGroupsDescription,
                            CategoryItem.CategoryItemFlags.Button | CategoryItem.CategoryItemFlags.BeginOfGroup,
                            null,
                            Properties.Resources.special_groups_32,
                            new EventHandler(ShowSpecialGroupListView),
                            "SpecialGroup"), 40);
                }
            }

        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Properties.Resources.Retail, "Retail", Properties.Resources.retail_green_16), 50);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "Retail")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsView) ||
                    PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailGroups) ||
                    PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailDepartments) ||
                    PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageSpecialGroups))
                {
                    args.Add(new Item(Properties.Resources.RetailItems, "RetailItems", null), 100);
                }
            }

        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Retail" && args.ItemKey == "RetailItems")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit))
                {
                    args.Add(new ItemButton(Properties.Resources.NewRetailItem, Properties.Resources.NewRetailItemDescription, new EventHandler(NewItem)), 50);
                }

                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsView))
                {
                    args.Add(new ItemButton(Properties.Resources.RetailItems, Properties.Resources.ViewItemsDescription, new EventHandler(ShowItemsSheet)), 100);
                }

                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailGroups))
                {
                    args.Add(new ItemButton(Properties.Resources.RetailGroups, Properties.Resources.ViewRetailGroupsDescription, new EventHandler(ShowRetailGroupListView)), 200);
                }

                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailDepartments))
                {
                    args.Add(new ItemButton(Properties.Resources.RetailDepartments, Properties.Resources.ViewRetailDepartmentsDescription, new EventHandler(ShowRetailDepartmentListView)), 250);
                }

                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailDivisions))
                {
                    args.Add(new ItemButton(Properties.Resources.RetailDivisions, Properties.Resources.ViewRetailDivisionDescription, new EventHandler(ShowRetailDivisionListView)), 300);
                }

                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageSpecialGroups))
                {
                    args.Add(new ItemButton(Properties.Resources.SpecialGroups, Properties.Resources.ViewSpecialGroupsDescription, new EventHandler(ShowSpecialGroupListView)), 350);
                }
            }
        }

        internal static bool ListViewItemIsCustomer(ListViewItem lvItem)
        {
            return (((TradeAgreementEntry)lvItem.Tag).AccountCode == TradeAgreementEntryAccountCode.Table &&
                    (((TradeAgreementEntry)lvItem.Tag).AccountRelation != ""));
        }

        internal static bool HasUnitConversion(DataEntity itemDataEntity, RecordIdentifier targetUnitId)
        {
            IPlugin unitConversionAdder = PluginEntry.Framework.FindImplementor(null, "CanEnforceInventoryUnitConversion", null);

            if (unitConversionAdder != null)
            {
                bool conversionRuleExists =
                    (bool)unitConversionAdder.Message(null, "EnforceInventoryUnitConversion",
                                            new object[] { itemDataEntity, targetUnitId });
                if (!conversionRuleExists)
                {
                    MessageDialog.Show(Properties.Resources.UnitConversionRuleMissingAlert);
                    return false;
                }
            }

            return true;
        }

        public static void EditItemType(RecordIdentifier originalItemID, ItemTypeEnum itemType)
        {
            ItemTypeDialog dlg = new ItemTypeDialog(PluginEntry.DataModel, originalItemID, itemType);

            if(dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                RecordIdentifier newItemTypeID = dlg.NewItemTypeId;
                if (newItemTypeID != RecordIdentifier.Empty)
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "ItemType", originalItemID, newItemTypeID);
                }
            }
        }

        public static void TaskBarCategoryCallback(object sender, ContextBarHeaderConstructionArguments arguments)
        {
            if ((arguments.Key == "LSOne.ViewPlugins.Inventory.Views.PurchaseOrderView") || (arguments.Key == "LSOne.ViewPlugins.Inventory.Views.GoodsReceivingDocumentView"))
            {
                orderView = arguments.View;

                ExtendedMenuItem item = new ExtendedMenuItem(
                   Resources.OpenInItemView,
                   400,
                   ViewItemClickedFromMenuItems);

                orderView.AddExtendedMenuItem(item);
            }
        }

        private static void ViewItemClickedFromMenuItems(object sender, EventArgs args)
        {
            List<IDataEntity> selectedEntities = orderView.GetListSelection();

            if(selectedEntities.Count == 0)
            {
                MessageDialog.Show(Resources.NoItemSelected + ".",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            else if(selectedEntities.Count == 1)
			{
                RecordIdentifier itemId = selectedEntities[0].ID;
                ShowItemSheet(itemId);
            }
			else
			{
                BulkEditItems(selectedEntities);
			}
        }
    }
}
