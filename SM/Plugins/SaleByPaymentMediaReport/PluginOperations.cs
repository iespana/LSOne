using System;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;

namespace LSOne.ViewPlugins.SaleByPaymentMediaReport
{
    internal partial class PluginOperations
    {
        public static string DateFilterTypeString(DateFilterTypeEnum dateFilterTypeEnum)
        {
            switch (dateFilterTypeEnum)
            {
                case DateFilterTypeEnum.BusinessDay:
                    return Properties.Resources.BusinessDate;
                case DateFilterTypeEnum.TransDate:
                    return Properties.Resources.TransactionDate;
                default:
                    return Properties.Resources.BusinessDate;
            }
        }

        public static void ShowSalesByPaymentView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.SaleByPaymentMediaReportView());
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Properties.Resources.Retail, "Retail", null), 50);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "Retail")
            {
                args.Add(new Item(Properties.Resources.EndOfDay, "EOD", null), 500);
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Retail" && args.ItemKey == "EOD")
            {
                //args.Add(new ItemButton(Properties.Resources.SaleByPaymentMediaReport, Properties.Resources.SaleByPaymentMediaReportDescription, ShowSalesByPaymentView), 60);
            }
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
           // args.Add(new Page(Properties.Resources.Retail, "Retail"), 100);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            /*if (args.PageKey == "Retail")
            {
                args.Add(new PageCategory(Properties.Resources.EndOfDay, "EOD"), 500);
            }*/
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            /* if (args.PageKey == "Retail" && args.CategoryKey == "EOD")
             {
                 args.Add(new CategoryItem(
                     Properties.Resources.SaleByPaymentMediaReport,
                     Properties.Resources.SaleByPaymentMediaReport,
                     Properties.Resources.SaleByPaymentMediaReportDescription,
                     CategoryItem.CategoryItemFlags.Button,
                     null,
                     ShowSalesByPaymentView,
                     "salesByPaymentView"), 60);
             }*/

        }

    }
}
