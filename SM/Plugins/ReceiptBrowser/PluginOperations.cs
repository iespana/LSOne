using System;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewCore;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;

namespace LSOne.ViewPlugins.ReceiptBrowser
{
    internal class PluginOperations
    {
        public static void ShowFindReceiptSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ReceiptBrowserView());
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Properties.Resources.Retail, "Retail", null), 50);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "Retail")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ReceiptsView))
                {
                    args.Add(new Item(Properties.Resources.Receipts, "Receipts", null), 200);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Retail" && args.ItemKey == "Receipts")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ReceiptsView))
                {
                    args.Add(new ItemButton(Properties.Resources.SearchForReceipt, Properties.Resources.SearchForReceiptDescription, new EventHandler(ShowFindReceiptSheet)), 100);
                }
            }
        }

        public static void ShowFindReceiptSheetByStaff(object sender, ContextBarClickEventArguments args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ReceiptBrowserView(args.ContextID.SecondaryID, ((ViewBase)args.ContextView).ContextDescription,"","","",""));
        }

        public static void ShowFindReceiptSheetByStore(object sender, ContextBarClickEventArguments args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ReceiptBrowserView("","",args.ContextID, ((ViewBase)args.ContextView).ContextDescription,"",""));
        }

        public static void ShowFindReceiptSheetByTerminal(object sender, ContextBarClickEventArguments args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ReceiptBrowserView("","","", "", args.ContextID, ((ViewBase)args.ContextView).ContextDescription));
        }

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ReceiptsView))
            {
                if (arguments.CategoryKey == "LSOne.ViewPlugins.User.Views.User.Related")
                {
                    if (arguments.ContextIdentifier.SecondaryID != null)
                    {
                        arguments.Add(new ContextBarItem(Properties.Resources.Receipts, ShowFindReceiptSheetByStaff), 300);
                    }
                }
                else if (arguments.CategoryKey == "LSOne.ViewPlugins.Store.Views.StoreView.Related")
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.Receipts, ShowFindReceiptSheetByStore), 300);
                }
                else if (arguments.CategoryKey == "LSOne.ViewPlugins.Store.Views.TerminalView.Related")
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.Receipts, ShowFindReceiptSheetByTerminal), 300);
                }

            }
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Properties.Resources.Sales, "Sales"), 300);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Sales")
            {
                args.Add(new PageCategory(Properties.Resources.Transactions, "Transactions"), 120);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Sales" && args.CategoryKey == "Transactions")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ReceiptsView))
                {
                    args.Add(new CategoryItem(
                        Properties.Resources.Receipts,
                        Properties.Resources.ReceiptBrowser,
                        Properties.Resources.ReceiptsTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        null,
                        Properties.Resources.receipt_32,
                        new EventHandler(ShowFindReceiptSheet),
                        "FindReceipt"), 10);
                }
            }
        }
    }
}
