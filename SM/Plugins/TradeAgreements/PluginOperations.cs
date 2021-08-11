using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders.Units;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.TradeAgreements.Dialogs;
using LSOne.ViewPlugins.TradeAgreements.Properties;
using LSOne.ViewPlugins.TradeAgreements.ViewPages;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.TradeAgreements
{
    internal class PluginOperations
    {
        internal static void ShowCustomerDiscountGroups(PriceDiscGroupEnum type)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CustomerDiscountGroupView(type));
        }

        internal static void ShowCustDiscountGroups(RecordIdentifier selectedID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CustomerDiscountGroupView(selectedID));
        }

        internal static void ShowCustomerDiscountGroups(RecordIdentifier context)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CustomerDiscountGroupView((PriceDiscGroupEnum)(int)context));
        }

        internal static void ShowCustomerDiscountGroups(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CustomerDiscountGroupView(PriceDiscGroupEnum.LineDiscountGroup));
        }

        internal static void ShowCustomerPriceGroups()
        {
            PluginEntry.Framework.ViewController.Add(new Views.PriceGroupView());
        }

        internal static void ShowCustomerPriceGroups(RecordIdentifier context)
        {
            PluginEntry.Framework.ViewController.Add(new Views.PriceGroupView());
        }

        internal static void ShowPriceGroups(RecordIdentifier selectedID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.PriceGroupView(selectedID));
        }

        internal static void ShowCustomerPriceGroups(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.PriceGroupView());
        }


        internal static void ShowItemDiscountGroups(PriceDiscGroupEnum type)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ItemDiscountGroupView(type));
        }

        internal static void ShowItemDiscountGroups(RecordIdentifier selectedID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ItemDiscountGroupView(selectedID));
        }

        internal static void ShowItemDiscountGroups(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ItemDiscountGroupView(PriceDiscGroupEnum.LineDiscountGroup));
        }

        internal static void AddToPriceGroup(object sender, EventArgs args)
        {
            if (!CustomerDeleted(PluginEntry.Framework.ViewController.CurrentView.GetListSelection()))
            {
                var dialog = new CustomerInGroupDialog(PluginEntry.Framework.ViewController.CurrentView.GetListSelection(), PriceDiscGroupEnum.PriceGroup);
                dialog.ShowDialog();
            }
            else
            {
                MessageDialog.Show(Resources.CantAddDeletedCustomerToGroup);
            }
        }

        internal static void AddToPriceGroup(object sender, ContextBarClickEventArguments args)
        {
            if (!CustomerDeleted(PluginEntry.Framework.ViewController.CurrentView.GetListSelection()))
            {
                var dialog = new CustomerInGroupDialog(((ViewBase) args.ContextView).GetListSelection(),PriceDiscGroupEnum.PriceGroup);
                dialog.ShowDialog();
            }
            else
            {
                MessageDialog.Show(Resources.CantAddDeletedCustomerToGroup);
            }
        }

        internal static void AddToLineDiscountGroup(object sender, EventArgs args)
        {
            if (!CustomerDeleted(PluginEntry.Framework.ViewController.CurrentView.GetListSelection()))
            {
                var dialog =new CustomerInGroupDialog(PluginEntry.Framework.ViewController.CurrentView.GetListSelection(),PriceDiscGroupEnum.LineDiscountGroup);
                dialog.ShowDialog();
            }
            else
            {
                MessageDialog.Show(Resources.CantAddDeletedCustomerToGroup);
            }
        }
        
        internal static void AddToLineDiscountGroup(object sender, ContextBarClickEventArguments args)
        {
            if (!CustomerDeleted(PluginEntry.Framework.ViewController.CurrentView.GetListSelection()))
            {
                var dialog = new CustomerInGroupDialog(((ViewBase) args.ContextView).GetListSelection(),
                    PriceDiscGroupEnum.LineDiscountGroup);
                dialog.ShowDialog();
            }
            else
            {
                MessageDialog.Show(Resources.CantAddDeletedCustomerToGroup);
            }
        }

        internal static void AddToTotalDiscountGroup(object sender, EventArgs args)
        {
            if (!CustomerDeleted(PluginEntry.Framework.ViewController.CurrentView.GetListSelection()))
            {
                var dialog = new CustomerInGroupDialog(PluginEntry.Framework.ViewController.CurrentView.GetListSelection(), PriceDiscGroupEnum.TotalDiscountGroup);
                dialog.ShowDialog();
            }
            else
            {
                MessageDialog.Show(Resources.CantAddDeletedCustomerToGroup);
            }
        }
        
        internal static void AddToTotalDiscountGroup(object sender, ContextBarClickEventArguments args)
        {
            if (!CustomerDeleted(PluginEntry.Framework.ViewController.CurrentView.GetListSelection()))
            {
                var dialog = new CustomerInGroupDialog(((ViewBase)args.ContextView).GetListSelection(), PriceDiscGroupEnum.TotalDiscountGroup);
            dialog.ShowDialog();
            }
            else
            {
                MessageDialog.Show(Resources.CantAddDeletedCustomerToGroup);
            }
        }

        internal static bool CustomerDeleted(List<IDataEntity> customers)
        {
            foreach (var customer in customers)
            {
                Customer cust = Providers.CustomerData.Get(PluginEntry.DataModel, customer.ID, UsageIntentEnum.Normal);
                if (cust.Deleted)
                {
                    return true;
                }
            }
            return false;
        }

        public static void TaskBarCategoryCallback(object sender, ContextBarHeaderConstructionArguments arguments)
        {
            if (arguments.Key == "LSOne.ViewPlugins.RetailItems.Views.ItemView")
            {
                arguments.Add(new ContextBarHeader(Properties.Resources.TradeAgreements, "LSOne.ViewPlugins.RetailItems.Views.ItemView.TradeAgreements"), 200);
            }
            if (arguments.Key == "LSOne.ViewPlugins.Customer.Views.CustomersView")
            {
                arguments.Add(new ContextBarHeader("Actions", arguments.Key + ".Actions"), 200);
            }
        }

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == "LSOne.ViewPlugins.TradeAgreements.Views.CustomerDiscountGroupView.Related")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ViewItemDiscGroups))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.ItemDiscountGroups, PluginOperations.ShowItemDiscountGroups), 200);
                }
            }
            if (arguments.CategoryKey == "LSOne.ViewPlugins.TradeAgreements.Views.ItemDiscountGroupView.Related")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ViewCustomerDiscGroups))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.CustomerDiscountGroups, PluginOperations.ShowCustomerDiscountGroups), 200);
                }
            }
            if (arguments.CategoryKey == "LSOne.ViewPlugins.Customer.Views.CustomersView.Actions")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ViewCustomerDiscGroups))
                {
                    bool enabled = arguments.View.GetListSelectionCount() > 0;
                    arguments.Add(new ContextBarItem(Resources.AddToPriceGroup, AddToPriceGroup) { Enabled = enabled }, 200);
                    arguments.Add(new ContextBarItem(Resources.AddToLineDiscountGroup, AddToLineDiscountGroup) { Enabled = enabled }, 300);
                    arguments.Add(new ContextBarItem(Resources.AddToTotalDiscountGroup, AddToTotalDiscountGroup) { Enabled = enabled }, 400);
                }
            }
        }

        internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            switch (args.ContextName)
            {
                case "LSOne.ViewPlugins.RetailItems.ViewPages.ItemDiscountsPage":
                    if (PluginEntry.DataModel.HasPermission(Permission.ManageTradeAgreementPrices))
                    {

                        args.Add(
                            new TabControl.Tab(
                                Properties.Resources.TradeAgreements,
                                ItemTradeAgreementDiscountsPage.CreateInstance
                                ),
                            120);
                    }
                    break;
                case "LSOne.ViewPlugins.RetailItems.ViewPages.ItemPricesPage":
                    if (PluginEntry.DataModel.HasPermission(Permission.ManageTradeAgreementPrices))
                    {
                        args.Add(
                            new TabControl.Tab(
                                Properties.Resources.SalesPrices,
                                ItemSalesPricePage.CreateInstance
                                ),
                            120);
                    }
                    break;
                case "LSOne.ViewPlugins.Customer.Views.CustomerView":
                    args.Add(
                        new TabControl.Tab(
                            Properties.Resources.SalesPrices,
                            SalesPricesPage.CreateInstance
                            ),
                        120);
                    break;
                case "LSOne.ViewPlugins.Customer.ViewPages.CustomerDiscountsPage":
                    if (PluginEntry.DataModel.HasPermission(Permission.ManageTradeAgreementPrices))
                    {
                        args.Add(
                            new TabControl.Tab(
                                Properties.Resources.TradeAgreements,
                                CustomerTradeAgreementsPage.CreateInstance
                                ),
                            100);
                    }

                    if (PluginEntry.DataModel.HasPermission(Permission.ManageCustomerGroups))
                    {
                        args.Add(
                        new TabControl.Tab(
                            Properties.Resources.GroupLimit,
                            CustomerGroupLimitPage.CreateInstance
                            ),
                        200);
                    }
                    break;
            }
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Properties.Resources.Retail, "Retail", null), 50);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "Retail")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ViewCustomerDiscGroups) ||
                    PluginEntry.DataModel.HasPermission(Permission.ViewItemDiscGroups))
                {
                    args.Add(new Item(Properties.Resources.PriceDiscount, "PriceDiscount", null), 400);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Retail" && args.ItemKey == "PriceDiscount")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ViewPriceGroups))
                {
                    
                    args.Add(new ItemButton(Properties.Resources.PriceGroups, Properties.Resources.PriceGroupsDescription, new EventHandler(ShowCustomerPriceGroups)), 10);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.ViewCustomerDiscGroups))
                {
                    args.Add(new ItemButton(Properties.Resources.CustomerDiscountGroups, Properties.Resources.CustomerDiscountGroupsDescription, new EventHandler(ShowCustomerDiscountGroups)), 15);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.ViewItemDiscGroups))
                {
                    args.Add(new ItemButton(Properties.Resources.ItemDiscountGroups, Properties.Resources.ItemDiscountGroupsDescription, new EventHandler(ShowItemDiscountGroups)), 20);
                }
            }
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Properties.Resources.Retail, "Item"), 200);
            args.Add(new Page(Properties.Resources.Customers, "Customers"), 500);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {

            if (args.PageKey == "Customers")
            {
                args.Add(new PageCategory(Properties.Resources.Groups, "CustomerGroups"), 200);
            }
            if (args.PageKey == "Item")
            {
                args.Add(new PageCategory(Properties.Resources.Groups, "PriceDiscounts"), 300);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {

            if (args.PageKey == "Item" && args.CategoryKey == "PriceDiscounts")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ViewItemDiscGroups))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.DiscountGroups,
                            Properties.Resources.ItemDiscountGroups,
                            Properties.Resources.ItemDiscountGroupsTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            Properties.Resources.discount_groups_16,
                            null,
                            new EventHandler(ShowItemDiscountGroups),
                            "LineDiscountGroup"), 70);
                }
            }

             if (args.PageKey == "Customers" && args.CategoryKey == "CustomerGroups")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ViewCustomerDiscGroups))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.DiscountGroups,
                            Properties.Resources.CustomerDiscountGroups,
                            Properties.Resources.CustomerDiscountGroupsTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            Properties.Resources.discount_groups_16,
                            null,
                            ShowCustomerDiscountGroups,
                            "DiscountGroups"), 30);
                }
            }

            if (args.PageKey == "Sites" && args.CategoryKey == "SiteTerminal")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ViewCustomerDiscGroups))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.PriceGroups,
                            Properties.Resources.PriceGroups,
                            Properties.Resources.PriceGroupsTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            Properties.Resources.price_groups_16,
                            null,
                            ShowCustomerPriceGroups,
                            "PriceGroups"), 40);
                }
            }
        }

        internal static void ConstructMenus(object sender, MenuConstructionArguments args)
        {
            switch (args.Key)
            {
                case "RibbonPriceDiscountGroups":
                    if (PluginEntry.DataModel.HasPermission(Permission.ViewItemDiscGroups))
                    {
                        args.AddMenu(new ExtendedMenuItem(
                              Properties.Resources.ItemDiscountGroups,
                              null,
                              205,
                              new EventHandler(ShowItemDiscountGroups)));
                    }
                    if (PluginEntry.DataModel.HasPermission(Permission.ViewCustomerDiscGroups))
                    {
                        args.AddMenu(new ExtendedMenuItem(
                              Properties.Resources.CustomerDiscountGroups,
                              null,
                              204,
                              new EventHandler(ShowCustomerDiscountGroups)));

                        args.AddMenu(new ExtendedMenuItem(
                              Properties.Resources.PriceGroups,
                              null,
                              203,
                              new EventHandler(ShowCustomerPriceGroups)));
                    }
                    break;

                case "CustomersList":
                    if (PluginEntry.DataModel.HasPermission(Permission.ViewCustomerDiscGroups))
                    {
                        args.AddMenu(new ExtendedMenuItem("-", 1000));

                        args.AddMenu(new ExtendedMenuItem(
                              Properties.Resources.AddToPriceGroup,
                              null,
                              1010,
                              new EventHandler(AddToPriceGroup)));
                        args.AddMenu(new ExtendedMenuItem(
                              Properties.Resources.AddToLineDiscountGroup,
                              null,
                              1020,
                              new EventHandler(AddToLineDiscountGroup)));
                        args.AddMenu(new ExtendedMenuItem(
                              Properties.Resources.AddToTotalDiscountGroup,
                              null,
                              1030,
                              new EventHandler(AddToTotalDiscountGroup)));
                    }
                    break;
            }
        }

        internal static bool ListViewItemIsCustomer(ListViewItem lvItem)
        {
            return (((TradeAgreementEntry)lvItem.Tag).AccountCode == TradeAgreementEntryAccountCode.Table &&
                    (((TradeAgreementEntry)lvItem.Tag).AccountRelation != ""));
        }
        internal static bool ListViewItemIsCustomer(TradeAgreementEntry lvItem)
        {
            return lvItem.AccountCode == TradeAgreementEntryAccountCode.Table && lvItem.AccountRelation != "";
        }

        internal static bool ListViewItemIsRetailItem(ListViewItem lvItem)
        {
            return (((TradeAgreementEntry)lvItem.Tag).ItemID != "");
        }

        internal static bool ListViewItemIsRetailItem(TradeAgreementEntry tradeAgreementItem)
        {
            return (tradeAgreementItem.ItemID != "");
        }

        internal static bool RowIsRetailItem(Row lvItem)
        {
            return (((TradeAgreementEntry)lvItem.Tag).ItemID != "");
        }

        internal static bool ConversionRuleToInventoryUnitExists(RetailItem item, RecordIdentifier unitToConvertToID)
        {
            RecordIdentifier inventoryUnitID = item.InventoryUnitID.PrimaryID;
            bool conversionRuleExists =
                Providers.UnitConversionData.UnitConversionRuleExists(PluginEntry.DataModel, item.ID, unitToConvertToID, inventoryUnitID);

            if (!conversionRuleExists)
            {
                if (QuestionDialog.Show(
                        Properties.Resources.InventoryUnitConversionRuleMissing,
                        Properties.Resources.UnitConversionRuleMissing) == DialogResult.Yes)
                {
                    IPlugin unitConversionAdder =
                        PluginEntry.Framework.FindImplementor(null, "CanAddUnitConversions", null);

                    if (unitConversionAdder != null)
                    {
                        bool unitConversionAdded = (bool)unitConversionAdder.Message(null,
                                                                                    "AddUnitConversion",
                                                                                    new object[] { 
                                                                                                item, 
                                                                                                unitToConvertToID, 
                                                                                                inventoryUnitID });

                        conversionRuleExists = unitConversionAdded;
                    }
                }
            }

            return conversionRuleExists;

        }
    
        internal static bool DeletePriceDiscountGroup(PriceDiscountGroup group, PriceDiscGroupEnum groupType)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteGroupQuestion,
                Properties.Resources.DeleteGroup) == DialogResult.Yes)
            {
                switch (groupType)
                {
                    case PriceDiscGroupEnum.PriceGroup:
                        Providers.PriceDiscountGroupData.RemoveStoresFromPriceGroup(PluginEntry.DataModel, group.GroupID);
                        break;
                    case PriceDiscGroupEnum.LineDiscountGroup:
                    case PriceDiscGroupEnum.MultilineDiscountGroup:
                        Providers.ItemInPriceDiscountGroupData.
                            RemoveAllItemsFromGroup(PluginEntry.DataModel, group.GroupID, groupType);
                        break;
                }

                Providers.PriceDiscountGroupData.RemoveCustomersFromGroup(PluginEntry.DataModel, group.GroupID, groupType);

                Providers.PriceDiscountGroupData.Delete(PluginEntry.DataModel, group);
                return true;
            }
            return false;
        }
    }
}
