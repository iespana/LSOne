using System;
using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.TaxRefund.Views;

namespace LSOne.ViewPlugins.TaxRefund
{
    internal class PluginOperations
    {
        public static void ShowTaxReturnRangesView(object sender, EventArgs args)
        {
            //PluginEntry.Framework.ViewController.Add(new TaxRefundRangesView());
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ViewTaxFree))
            {
                args.Add(Properties.Resources.GeneralSetup, "General setup", null, 100);
            }
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "General setup")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ViewTaxFree))
                {
                    args.Add(Properties.Resources.SalesTax, "SalesTax", null /*Properties.Resources.Tax16*/, null, 420);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            //if (args.CategoryKey == "General setup" && args.ItemKey == "SalesTax")
            //{
            //    if (PluginEntry.DataModel.HasPermission(Permission.ViewTaxFree))
            //    {
            //        args.Add(Properties.Resources.TaxRefundRanges, Properties.Resources.TaxRefundViewDescription, ShowTaxReturnRangesView, 30);
            //    }
            //}
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ViewTaxFree))
            {
                args.Add(new Page(Properties.Resources.GeneralSetup, "General setup"), 100);
            }
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "General setup")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ViewTaxFree))
                {
                    args.Add(new PageCategory(Properties.Resources.SalesTax, "SalesTax"), 450);
                }
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            //if (args.PageKey == "General setup" && args.CategoryKey == "SalesTax")
            //{
            //    if (PluginEntry.DataModel.HasPermission(Permission.ViewTaxFree))
            //    {
            //        args.Add(new CategoryItem(Properties.Resources.TaxRefund,
            //            Properties.Resources.TaxRefund,
            //            Properties.Resources.TaxRefund,
            //            CategoryItem.CategoryItemFlags.Button,
            //            null,
            //            new EventHandler(ShowTaxReturnRangesView),
            //            "TaxRefund"), 30);
            //    }
            //}
        }
    }
}
