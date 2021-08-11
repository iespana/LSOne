using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.VariantFramework.Dialogs;
using LSOne.ViewPlugins.VariantFramework.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.VariantFramework
{
    internal class PluginOperations
    {
        internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            if (args.ContextName == "LSOne.ViewPlugins.RetailItems.Dialogs.NewRetailItemDialog")
            {
                args.Add(new TabControl.Tab(Resources.VariantItems, new PanelFactoryHandler(DialogPages.NewRetailItemVariantsPage.CreateInstance), true, true), 100);
                args.Add(new TabControl.Tab(Resources.Dimensions, new PanelFactoryHandler(DialogPages.NewRetailItemDimensionsPage.CreateInstance), true, true), 90);
            }

            if ((args.ContextName == "LSOne.ViewPlugins.RetailItems.Views.ItemView") && (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ManageItemDimensions)) && ((SimpleRetailItem)args.InternalContext).ItemType == ItemTypeEnum.MasterItem)
            {
                args.Add(new TabControl.Tab(Resources.VariantItems, ItemTabKey.VariantItems.ToString(), new PanelFactoryHandler(ViewPages.ItemViewVariantItemsPage.CreateInstance)), 240);
            }
        }

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {
            if ((arguments.CategoryKey == "LSOne.ViewPlugins.RetailItems.Views.ItemView.Actions") &&
                (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ManageItemDimensions)))
            {
                RetailItem retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, arguments.ContextIdentifier, false);

                if (retailItem != null)
                {
                    if (retailItem.ItemType == ItemTypeEnum.MasterItem)
                    {
                        var item = new ContextBarItem(Resources.SaveDimensionTemplate, CreateDimensionTemplateClickedFromContextBar);
                        arguments.Add(item, 110);
                    }
                }
            }
        }


        public static bool DeleteItem(RecordIdentifier itemID, RecordIdentifier itemMasterID)
        {
            bool retValue = false;

            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit))
            {
                if (QuestionDialog.Show(Resources.DeleteVariantItemQuestion, Resources.DeleteVariantItem) == DialogResult.Yes)
                {
                    Providers.RetailItemData.Delete(PluginEntry.DataModel, itemMasterID);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "VariantItem", itemID, null);

                    retValue = true;
                }
            }

            return retValue;
        }

        private static void CreateDimensionTemplateClickedFromContextBar(object sender, ContextBarClickEventArguments args)
        {
            DimensionTemplatesDialog dlg = new DimensionTemplatesDialog(args.ContextID);
            dlg.ShowDialog(PluginEntry.Framework.MainWindow);
        }

        public static void DeleteItems(List<RecordIdentifier> items)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit))
            {

                if (QuestionDialog.Show(Resources.DeleteVariantItemsQuestion, Resources.DeleteVariantItems) == DialogResult.Yes)
                {
                    List<RecordIdentifier> itemIDs = new List<RecordIdentifier>();

                    foreach (RecordIdentifier item in items)
                    {
                        Providers.RetailItemData.Delete(PluginEntry.DataModel, item);

                        itemIDs.Add(item);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "VariantItem", null, itemIDs);
                }
            }
        }

        public static void DeleteAttributesAndVariantItems(RecordIdentifier attribute)
        {
            Providers.DimensionAttributeData.Delete(PluginEntry.DataModel, attribute);

            List<SimpleRetailItem> variantItems = Providers.RetailItemData.GetRetailItemsFromAttribute(PluginEntry.DataModel, attribute);

            List<RecordIdentifier> itemIDs = new List<RecordIdentifier>();

            foreach (SimpleRetailItem variantItem in variantItems)
            {
                Providers.RetailItemData.Delete(PluginEntry.DataModel, variantItem.MasterID);

                itemIDs.Add(variantItem.MasterID);
            }
            PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.RetailItemsDashboardItemID);

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "VariantItem", null, itemIDs);
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
        }

    }
}
