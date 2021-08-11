using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using ListView = System.Windows.Forms.ListView;
using LSOne.ViewCore.Controls;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.RetailItems
{
    public class PluginEntry : IPlugin, IPluginDashboardProvider
    {
        internal static Guid RetailItemsDashboardItemID = new Guid("d326af60-b1b5-4dd0-bf9a-bd9483a585b1");
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;
        internal static int RetailItemImageIndex;
        internal static int MasterItemImageIndex;
        internal static int VariantItemImageIndex;
        internal static int ServiceItemImageIndex;
        internal static int AssemblyItemImageIndex;

        internal static RetailItemSearchPanelFactory retailItemSearchProvider = null;

        //TODO replace the menu handlers with something nice on the ribbon bar
        private void ConstructMenus(object sender, MenuConstructionArguments args)
        {
            ExtendedMenuItem item;
            SearchListViewItem selectedItem;

            switch (args.Key)
            {
                case "RetailImportButton":
                    item = new ExtendedMenuItem(Properties.Resources.Images, 5, PluginOperations.ImportImages)
                    {
                        Image = Properties.Resources.ImageImport
                    };

                    args.AddMenu(item);
                    break;

                case "Insert":
                    if (DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit))
                    {

                        item = new ExtendedMenuItem(
                            Properties.Resources.NewRetailItem + "...",
                            Properties.Resources.new_retail_item_16,
                            50,
                            new EventHandler(PluginOperations.NewItem));

                        args.AddMenu(item);
                    }

                    break;

                case "RetailItemSearchList":
                case "AllSearchList":
                    if (args.Key != "AllSearchList")
                    {
                        if (DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit))
                        {

                            item = new ExtendedMenuItem(
                           Properties.Resources.NewRetailItem + "...",
                           null,
                           50,
                           new EventHandler(PluginOperations.NewItem));

                            args.AddMenu(item);
                        }
                    }

                    if (((ListView)args.Context).SelectedItems.Count > 0)
                    {
                        selectedItem = ((SearchListViewItem)((ListView)args.Context).SelectedItems[0]);

                        if (selectedItem.Key == "RetailItem")
                        {
                            if (DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsView))
                            {

                                item = new ExtendedMenuItem(
                                    Properties.Resources.EditRetailItem + "...",
                                    ContextButtons.GetEditButtonImage(),
                                    50,
                                    new EventHandler(PluginOperations.ShowItemSheetContextHandler));

                                item.Default = true;

                                args.AddMenu(item);
                            }


                            if (DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsView))
                            {

                                item = new ExtendedMenuItem(
                                    Properties.Resources.DeleteRetailItem + "...",
                                    ContextButtons.GetRemoveButtonImage(),
                                    150,
                                    new EventHandler(PluginOperations.DeleteItemContextHandler));

                                args.AddMenu(item);
                            }
                        }

                    }

                    break;
                
                case "RibbonGroups":
                    if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailGroups))
                    {
                        args.AddMenu(new ExtendedMenuItem(
                                Properties.Resources.ViewRetailGroups + "...",
                                null,
                                20,
                                new EventHandler(PluginOperations.ShowRetailGroupListView)));
                    }

                    if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailDepartments))
                    {
                        args.AddMenu(new ExtendedMenuItem(
                                Properties.Resources.ViewRetailDepartments + "...",
                                null,
                                30,
                                new EventHandler(PluginOperations.ShowRetailDepartmentListView)));
                    }

                    if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailDivisions))
                    {
                        args.AddMenu(new ExtendedMenuItem(
                                Properties.Resources.ViewRetailDivisions + "...",
                                null,
                                40,
                                new EventHandler(PluginOperations.ShowRetailDivisionListView)));
                    }

                    if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageSpecialGroups))
                    {
                        args.AddMenu(new ExtendedMenuItem(
                                Properties.Resources.ViewSpecialGroups + "...",
                                null,
                                50,
                                new EventHandler(PluginOperations.ShowSpecialGroupListView)));
                    }
                    break;
                
            }
        }

        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.RetailItems; }
        }


        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            if (message == "CanViewRetailItem")
            {
                return PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsView);
            }
            else if (message == "CanCreateRetailItem")
            {
                return PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit);
            }
            else if (message == "RetailItemNotifications")
            {
                return true;
            }
            else if (message == "CanManagePurchaseOrders")
            {
                return true;
            }
            return false;
        }

        public object Message(object sender, string message, object parameters)
        {
            if (message == "ViewItem")
            {
                if (parameters is RecordIdentifier)
                {
                    RecordIdentifier itemID = (RecordIdentifier)parameters;

                    PluginOperations.ShowItemSheet(itemID, null);
                }
                else
                {
                    RecordIdentifier itemID = (RecordIdentifier)((object[])parameters)[0];

                    PluginOperations.ShowItemSheet(itemID, (IEnumerable<IDataEntity>)((object[])parameters)[1]);
                }
            }
            else if (message == "NotifyRetailItem")
            {
                if(((DataEntityChangeType)((object[])parameters)[0]) == DataEntityChangeType.MultiAdd)
                {
                    // Some other plugin was adding some retail items so we need to make the dashboard item dirty.
                    PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.RetailItemsDashboardItemID);
                }
            }
            else if (message == "CreateRetailItem")
            {
                bool showItemSheet = (bool)(((object[])parameters)[0]);
                bool allowCreateAnother = (bool)(((object[])parameters)[1]);
                
                return PluginOperations.NewItem(showItemSheet, allowCreateAnother);
            }

            return null;
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            DataModel = dataModel;
            Framework = frameworkCallbacks;


            ImageList iml = frameworkCallbacks.GetImageList();

            iml.Images.Add(Properties.Resources.item_16);
            RetailItemImageIndex = iml.Images.Count - 1;

            iml.Images.Add(Properties.Resources.itemVariant_16);
            VariantItemImageIndex = iml.Images.Count - 1;

            iml.Images.Add(Properties.Resources.header_item);
            MasterItemImageIndex = iml.Images.Count - 1;

            iml.Images.Add(Properties.Resources.service_items_16);
            ServiceItemImageIndex = iml.Images.Count - 1;

            iml.Images.Add(Properties.Resources.itemassembly_16px);
            AssemblyItemImageIndex = iml.Images.Count - 1;

            // We want to be able to register items to the main application menu
            frameworkCallbacks.AddMenuConstructionConstructionHandler(ConstructMenus);

            frameworkCallbacks.AddSearchBarConstructionHandler(PluginOperations.AddSearchHandler);

            frameworkCallbacks.AddRibbonPageConstructionHandler(PluginOperations.AddRibbonPages);
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(PluginOperations.AddRibbonPageCategories);
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(PluginOperations.AddRibbonPageCategoryItems);

            frameworkCallbacks.ViewController.AddContextBarCategoryConstructionHandler(PluginOperations.TaskBarCategoryCallback);

            frameworkCallbacks.AddOperationCategoryConstructionHandler(PluginOperations.AddOperationCategoryHandler);
            frameworkCallbacks.AddOperationItemConstructionHandler(PluginOperations.AddOperationItemHandler);
            frameworkCallbacks.AddOperationButttonConstructionHandler(PluginOperations.AddOperationButtonHandler);
        }

        public void Dispose()
        {
            
        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation(Properties.Resources.NewRetailItem, PluginOperations.NewItem, LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit);
            operations.AddOperation(Properties.Resources.ViewItems, PluginOperations.ShowItemsSheet, LSOne.DataLayer.BusinessObjects.Permission.ItemsView);
            operations.AddOperation(Properties.Resources.ViewRetailGroups, PluginOperations.ShowRetailGroupListView, LSOne.DataLayer.BusinessObjects.Permission.ManageRetailGroups);
            operations.AddOperation(Properties.Resources.ViewRetailDepartments, PluginOperations.ShowRetailDepartmentListView, LSOne.DataLayer.BusinessObjects.Permission.ManageRetailDepartments);
            operations.AddOperation(Properties.Resources.ViewSpecialGroups, PluginOperations.ShowSpecialGroupListView, LSOne.DataLayer.BusinessObjects.Permission.ManageSpecialGroups);
            operations.AddOperation(Properties.Resources.ActionsImportImages, PluginOperations.ImportImages, LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit);
            operations.AddOperation("", "EditRetailItem", false, true, PluginOperations.EditRetailItem, DataLayer.BusinessObjects.Permission.ItemsEdit); 
            operations.AddOperation("", "EditRetailItems", false, true, PluginOperations.EditRetailItems, DataLayer.BusinessObjects.Permission.ItemsEdit); 
            operations.AddOperation("", "EditSpecialGroups", false, false, PluginOperations.EditSpecialGroups, DataLayer.BusinessObjects.Permission.ManageSpecialGroups); 
        }

    #endregion

        public void LoadDashboardItem(IConnectionManager threadedEntry, ViewCore.Controls.DashboardItem item)
        {
            int buttonIndex = 0;

            // In case if the plugin is registering more than one then we check which one it is though we will never get item from other plugin here.
            if (item.ID == RetailItemsDashboardItemID)
            {
                IRetailItemData itemProvider = Providers.RetailItemData;

                int itemCount = itemProvider.ItemCount(threadedEntry);

                if(itemCount == 0)
                {
                    item.State = DashboardItem.ItemStateEnum.Error;

                    item.InformationText = Properties.Resources.NoItemsHaveBeenCreated;
                }
                else if(itemCount == 1)
                {
                    item.State = DashboardItem.ItemStateEnum.Passed;

                    item.InformationText = Properties.Resources.OneItem;
                }
                else
                {
                    item.State = DashboardItem.ItemStateEnum.Passed;

                    item.InformationText = Properties.Resources.ManyItems.Replace("#1",itemCount.ToString());
                }

                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit))
                {
                    item.SetButton(buttonIndex, Properties.Resources.New_RetailItem, PluginOperations.NewItem);

                    buttonIndex++;
                }

                item.SetButton(buttonIndex, Properties.Resources.Manage, PluginOperations.ShowItemsSheet);
            }
        }

        public void RegisterDashBoardItems(DashboardItemArguments args)
        {
            // Here we often would put a permission check but Manage users has no permission, and sometimes the Dashboard item will show new user and sometimes
            // manage, so we let it slide here and will handle it in LoadDashboardItem since this dashboard item will always be avalible in some form.

            DashboardItem retailItemsDashboardItem = new DashboardItem(RetailItemsDashboardItemID, Properties.Resources.RetailItems, true, 60); // 1 hour refresh interval

            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsView))
            {
                args.Add(new DashboardItemPluginResolver(retailItemsDashboardItem, this), 70); // Priority 70
            }
        }
    }
}
