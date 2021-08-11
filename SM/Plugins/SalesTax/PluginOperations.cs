using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;

namespace LSOne.ViewPlugins.SalesTax
{
    internal class PluginOperations
    {
        public static void ShowSalesTaxCodesView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.SalesTaxCodesView());
        }

        public static void ShowSalesTaxCodesView(RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new Views.SalesTaxCodesView(id));
        }

        public static void ShowItemSalesTaxGroupView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ItemSalesTaxGroupView());
        }

        public static void ShowItemSalesTaxGroupView(RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ItemSalesTaxGroupView(id));
        }

        public static void ShowSalesTaxGroupView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.SalesTaxGroupView());
        }

        public static void ShowSalesTaxGroupView(RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new Views.SalesTaxGroupView(id));
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ViewSalesTaxSetup))
            {
                args.Add(new Category(Properties.Resources.Yes, "General setup", Properties.Resources.general_setup_green_16), 100);
            }
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "General setup")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ViewSalesTaxSetup))
                {
                    args.Add(new Item(Properties.Resources.SalesTax, "SalesTax", null), 420);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "General setup" && args.ItemKey == "SalesTax")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ViewSalesTaxSetup))
                {
                    args.Add(new ItemButton(Properties.Resources.SalesTaxCodes, Properties.Resources.SalesTaxCodesDescription, new EventHandler(ShowSalesTaxCodesView)), 30);
                    args.Add(new ItemButton(Properties.Resources.ItemSalesTaxGroup, Properties.Resources.ItemSalesTaxGroupDescription, new EventHandler(ShowItemSalesTaxGroupView)), 20);
                    args.Add(new ItemButton(Properties.Resources.SalesTaxGroup, Properties.Resources.SalesTaxGroupDescription, new EventHandler(ShowSalesTaxGroupView)), 10);
                }
            }
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ViewSalesTaxSetup))
            {
                args.Add(new Page(Properties.Resources.Setup, "Setup"), 900);
            }
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Setup")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ViewSalesTaxSetup))
                {
                    args.Add(new PageCategory(Properties.Resources.SalesTax, "SalesTax"), 200);
                }
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Setup" && args.CategoryKey == "SalesTax")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ViewSalesTaxSetup))
                {
                    args.Add(new CategoryItem(
                        Properties.Resources.SalesTaxCodes,
                        Properties.Resources.SalesTaxCodes,
                        Properties.Resources.SalesTaxCodesTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        null,
                        new EventHandler(ShowSalesTaxCodesView),
                        "SalesTaxCodes"), 30);

                    args.Add(new CategoryItem(
                        Properties.Resources.ItemGroup,
                        Properties.Resources.ItemSalesTaxGroup,
                        Properties.Resources.ItemSalesTaxGroupTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        null,
                        new EventHandler(ShowItemSalesTaxGroupView),
                        "ItemSalesTaxGroup"), 20);

                    args.Add(new CategoryItem(
                        Properties.Resources.CustomerStoreGroup,
                        Properties.Resources.SalesTaxGroup,
                        Properties.Resources.SalesTaxGroupTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        null,
                        new EventHandler(ShowSalesTaxGroupView),
                        "SalesTaxGroup"), 10);
                }
            }
        }

        internal static ItemSalesTaxGroup NewSalesTaxItemGroup(RecordIdentifier recordIdentifier)
        {
            Dialogs.ItemSalesTaxGroupDialog dlg = new Dialogs.ItemSalesTaxGroupDialog();
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                return dlg.GetItemSalesTaxGroup;
            }

            return null;
        }

        internal static SalesTaxGroup NewStoreSalesTaxGroup()
        {
            Dialogs.SalesTaxGroupDialog dlg = new Dialogs.SalesTaxGroupDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return dlg.GetSalesTaxGroup;
            }

            return null;
        }
    }
}
