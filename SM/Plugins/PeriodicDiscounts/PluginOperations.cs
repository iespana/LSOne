using System;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.PeriodicDiscounts.Views;

namespace LSOne.ViewPlugins.PeriodicDiscounts
{
    internal class PluginOperations
    {
        public static void ShowPromotionsView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.PromotionsView());
        }

        public static void ShowSpecificPromotionsView(RecordIdentifier selectedID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.PromotionsView(selectedID));
        }

        public static void ShowDiscountOffersView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.DiscountOffersView());
        }

        public static void ShowSpecificDiscountOffersView(RecordIdentifier selectedID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.DiscountOffersView(selectedID));
        }

        public static void ShowMultibuyView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.MultiBuyView());
        }

        public static void ShowSpecificMultibuyView(RecordIdentifier selectedID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.MultiBuyView(selectedID));
        }

        public static void ShowMixAndMatchView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.MixAndMatchView());
        }

        public static void ShowSpecificMixAndMatchView(RecordIdentifier selectedID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.MixAndMatchView(selectedID));
        }

        public static void ShowDiscountPeriodsView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.DiscountPeriodView());
        }

        public static void ShowSpecificValidationPeriod(RecordIdentifier ID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.DiscountPeriodView(ID));
        }

        public static void ShowPeriodicDiscountPrioritiesView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new PeriodicDiscountPrioritiesView());
        }

        public static void ShowPeriodicDiscountPrioritiesView(RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new PeriodicDiscountPrioritiesView(id));
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Properties.Resources.Retail, "Retail", null), 50);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "Retail")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageDiscounts))
                {
                    args.Add(new Item(Properties.Resources.PriceDiscount, "PriceDiscount", null), 400);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Retail" && args.ItemKey == "PriceDiscount")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageDiscounts))
                {
                    args.Add(new ItemButton(Properties.Resources.Promotions, Properties.Resources.PromotionsDescription, new EventHandler(ShowPromotionsView)), 40);
                    args.Add(new ItemButton(Properties.Resources.DiscountOffers, Properties.Resources.DiscountOffersDescription, new EventHandler(ShowDiscountOffersView)), 50);
                    args.Add(new ItemButton(Properties.Resources.Multibuy, Properties.Resources.MultibuyDescription, new EventHandler(ShowMultibuyView)), 60);
                    args.Add(new ItemButton(Properties.Resources.MixAndMatch, Properties.Resources.MixAndMatchDescription, new EventHandler(ShowMixAndMatchView)), 70);
                    args.Add(new ItemButton(Properties.Resources.PeriodicDiscountPriorities, Properties.Resources.PeriodicDiscountPrioritiesDescription, new EventHandler(ShowPeriodicDiscountPrioritiesView)), 80);
                    args.Add(new ItemButton(Properties.Resources.ValidationPeriods, Properties.Resources.ValidationPeriodDescription, new EventHandler(ShowDiscountPeriodsView)), 100);
                }
            }
        }

        internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            switch (args.ContextName)
            {
                case "LSOne.ViewPlugins.RetailItems.Views.RetailGroupsView":
                    args.Add(
                    new TabControl.Tab(
                        Properties.Resources.PricesAndDiscounts,
                        ViewPages.RetailGroupDiscountsPage.CreateInstance
                        ),
                    120);
                    break;
                case "LSOne.ViewPlugins.RetailItems.Views.RetailDepartmentsView":
                    args.Add(
                    new TabControl.Tab(
                        Properties.Resources.PricesAndDiscounts,
                        ViewPages.RetailDepartmentDiscountsPage.CreateInstance
                        ),
                    120);
                    break;
                case "LSOne.ViewPlugins.RetailItems.Views.SpecialGroupsView":
                    args.Add(
                    new TabControl.Tab(
                        Properties.Resources.PricesAndDiscounts,
                        ViewPages.SpecialGroupDiscountsPage.CreateInstance
                        ),
                    120);
                    break;
                case "LSOne.ViewPlugins.RetailItems.ViewPages.ItemDiscountsPage":
                   

                    args.Add(
                    new TabControl.Tab(
                        Properties.Resources.PeriodicDiscounts,
                        ViewPages.ItemPeriodicDiscountsPage.CreateInstance
                        ) { Enabled = args.Context != RecordIdentifier.Empty },
                    120);
                    break;
                case "LSOne.ViewPlugins.RetailItems.ViewPages.ItemPricesPage":
                    args.Add(
                        new TabControl.Tab(Properties.Resources.PromotionOffers,ViewPages.ItemPromotionOffersPage.CreateInstance) { Enabled = !args.IsMultiEdit },
                    120);
                    break;
                case "LSOne.ViewPlugins.TradeAgreements.Views.PriceDiscGroupView":
                    TabControl.Tab tab = new TabControl.Tab(Properties.Resources.PeriodicDiscounts,
                                                            "",
                                                            ViewPages.PriceDiscountGroupPeriodicDiscountsPage.CreateInstance,
                                                            ViewPages.PriceDiscountGroupPeriodicDiscountsPage.TabMessage);
                    tab.Visible = false;
                    args.Add(tab, 120);                 
                
                    tab = new TabControl.Tab(Properties.Resources.Promotions,
                                             "",
                                             ViewPages.PriceDiscountGroupPromotionsPage.CreateInstance,
                                             ViewPages.PriceDiscountGroupPromotionsPage.TabMessage);
                    tab.Visible = false;
                    args.Add(tab, 130);    
                    break;

                case "LSOne.ViewPlugins.Customer.ViewPages.CustomerDiscountsPage":
                    args.Add(
                    new TabControl.Tab(
                        Properties.Resources.PeriodicDiscounts,
                        ViewPages.CustomerPeriodicDiscountsPage.CreateInstance
                        ),
                    120);
                    break;

                case "LSOne.ViewPlugins.TradeAgreements.ViewPages.SalesPricesPage":
                    args.Add(
                    new TabControl.Tab(
                        Properties.Resources.Promotions,
                        ViewPages.CustomerPromotionsPage.CreateInstance
                        ),
                    120);
                    break;
                default:
                    break;
            }
        }


        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Properties.Resources.Item, "Item"), 200);
        }



        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Item")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageDiscounts))
                {
                    args.Add(new PageCategory(Properties.Resources.PriceDiscount, "PriceDiscounts"), 300);
                    args.Add(new PageCategory(Properties.Resources.Price, "Price"), 400);
                }
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Item" && args.CategoryKey == "PriceDiscounts")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageDiscounts))
                {

                    args.Add(new CategoryItem(
                             Properties.Resources.Promotions,
                             Properties.Resources.Promotions,
                             Properties.Resources.PromotionsTooltipDescription,
                             CategoryItem.CategoryItemFlags.Button,
                             null,
                             Properties.Resources.promotions_32,
                             new EventHandler(ShowPromotionsView),
                             "Promotions"), 10);


                    args.Add(new CategoryItem(
                            Properties.Resources.DiscountOffers,
                            Properties.Resources.DiscountOffers,
                            Properties.Resources.DiscountOffersTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button | CategoryItem.CategoryItemFlags.BeginOfGroup,
                            null,
                            null,
                            new EventHandler(ShowDiscountOffersView),
                            "DiscountOffers"), 20);


                    args.Add(new CategoryItem(
                            Properties.Resources.Multibuy,
                            Properties.Resources.Multibuy,
                            Properties.Resources.MultibuyTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            null,
                            null,
                            new EventHandler(ShowMultibuyView),
                            "Multibuy"), 30);

                    args.Add(new CategoryItem(
                            Properties.Resources.MixAndMatch,
                            Properties.Resources.MixAndMatch,
                            Properties.Resources.MixAndMatchTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            null,
                            null,
                            new EventHandler(ShowMixAndMatchView),
                            "MixAndMatch"), 40);

                    args.Add(new CategoryItem(
                            Properties.Resources.ValidationPeriods,
                            Properties.Resources.ValidationPeriods,
                            Properties.Resources.ValidationPeriodTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button | CategoryItem.CategoryItemFlags.BeginOfGroup,
                            Properties.Resources.validation_periods_16,
                            null,
                            new EventHandler(ShowDiscountPeriodsView),
                            "ValidationPeriods"), 50);

                    args.Add(new CategoryItem(
                            Properties.Resources.Priority,
                            Properties.Resources.PeriodicDiscountPriorities,
                            Properties.Resources.PeriodicDiscountPrioritiesTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            Properties.Resources.priority_16,
                            null,
                            new EventHandler(ShowPeriodicDiscountPrioritiesView),
                            "DiscountPriority"), 60);
                }
            }

        }

        /// <summary>
        /// Returns the readable item ID (not GUID) for the combobox that is displaying either an item list or a variant item list
        /// </summary>
        /// <param name="comboBox">The combobox that is to be checked</param>
        /// <returns>The item ID selected</returns>
        public static RecordIdentifier GetReadableItemID(DualDataComboBox comboBox)
        {
            if (comboBox.SelectedData is MasterIDEntity)
            {
                return (comboBox.SelectedData as MasterIDEntity).ReadadbleID;
            }

            return (comboBox.SelectedData as DataEntity).ID;
        }

        /// <summary>
        /// Returns the selected item ID. If the variant combo box is enabled and has a selection the ID for that item is returned
        /// otherwise the item from the item combo box is returned
        /// </summary>
        /// <param name="itemComboBox"></param>
        /// <param name="variantComboBox"></param>
        /// <returns></returns>
        public static RecordIdentifier GetSelectedItemID(DualDataComboBox itemComboBox, DualDataComboBox variantComboBox)
        {

            if (variantComboBox.Enabled && variantComboBox.SelectedData != null
                && variantComboBox.SelectedData.ID != RecordIdentifier.Empty && (string)variantComboBox.SelectedData.ID != string.Empty)
            {
                return GetReadableItemID(variantComboBox);

            }

            return GetReadableItemID(itemComboBox);
        }

        /*    internal static void ConstructMenus(object sender, MenuConstructionArguments args)
            {
                switch (args.Key)
                {
                    case "RibbonPriceDiscountPrice":

                        args.AddMenu(new ExtendedMenuItem(
                                   Properties.Resources.Promotions,
                                   null,
                                   100,
                                   new EventHandler(ShowPromotionsView)));

                        args.AddSeparator(200);

                        args.AddMenu(new ExtendedMenuItem(
                                   Properties.Resources.ValidationPeriods + "...",
                                   null,
                                   205,
                                   new EventHandler(ShowDiscountPeriodsView)));


                        break;

                    case "RibbonPriceDiscountDiscounts":
                        args.AddMenu(new ExtendedMenuItem(
                                   Properties.Resources.DiscountOffers,
                                   null,
                                   100,
                                   new EventHandler(ShowDiscountOffersView)));

                        args.AddMenu(new ExtendedMenuItem(
                                   Properties.Resources.Multibuy,
                                   null,
                                   110,
                                   new EventHandler(ShowMultibuyView)));

                        args.AddMenu(new ExtendedMenuItem(
                                   Properties.Resources.MixAndMatch,
                                   null,
                                   120,
                                   new EventHandler(ShowMixAndMatchView)));

                        args.AddSeparator(200);

                        args.AddMenu(new ExtendedMenuItem(
                                   Properties.Resources.ValidationPeriods + "...",
                                   null,
                                   205,
                                   new EventHandler(ShowDiscountPeriodsView)));

                        break;

                }
            }*/

    }
}
