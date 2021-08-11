using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Loyalty;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.CustomerLoyalty.Dialogs;
using LSOne.ViewPlugins.CustomerLoyalty.Properties;
using LSOne.ViewPlugins.CustomerLoyalty.ViewPages;
using LSOne.ViewPlugins.CustomerLoyalty.Views;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.CustomerLoyalty
{
    internal class PluginOperations
    {
        private static bool TestSiteService(LoyaltyCustomerParams loyaltyParams)
        {
            bool serviceConfigured = false;
            bool serviceActivce = false;
            bool serviceValid;


            if (PluginEntry.DataModel.SiteServiceAddress == "")
            {
                MessageDialog.Show(Resources.NoStoreServerIsSetUp);
            }
            else
            {
                serviceConfigured = true;
            }

            Parameters paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
            SiteServiceProfile siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);

            if (siteServiceProfile != null)
            {

                ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
                ConnectionEnum result = service.TestConnection(PluginEntry.DataModel,
                    siteServiceProfile.SiteServiceAddress,
                    (ushort)siteServiceProfile.SiteServicePortNumber);

                PluginEntry.Framework.ViewController.CurrentView.HideProgress();


                if (result != ConnectionEnum.Success)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                }
                else
                {
                    serviceActivce = true;
                }
            }
            else
            {
                MessageDialog.Show(Properties.Resources.NoStoreServerIsSetUp);
            }

            serviceValid = serviceActivce && serviceConfigured;
            if (!serviceValid)
            {
                IPlugin plugin = PluginEntry.Framework.FindImplementor(null, "CanViewAdministrationTab", null);

                if (plugin != null)
                {
                    plugin.Message(null, "ViewSiteServiceTab", null);
                }
            }

            return serviceValid;


        }

        public static void ShowLoyaltyTransView(object sender, EventArgs args)
        {
            LoyaltyCustomerParams loyaltyParams = Providers.LoyaltyCustomerParamsData.Get(PluginEntry.DataModel);

            if (TestSiteService(loyaltyParams))
            {
                PluginEntry.Framework.ViewController.Add(new LoyaltyTransView(loyaltyParams));
            }
        }

        public static void ShowLoyaltySchemesView(object sender, EventArgs args)
        {
            LoyaltyCustomerParams loyaltyParams = Providers.LoyaltyCustomerParamsData.Get(PluginEntry.DataModel);

            if (TestSiteService(loyaltyParams))
            {
                PluginEntry.Framework.ViewController.Add(new LoyaltySchemesView(loyaltyParams));
            }
        }

        public static void ShowLoyaltyCardsView(object sender, EventArgs args)
        {
            LoyaltyCustomerParams loyaltyParams = Providers.LoyaltyCustomerParamsData.Get(PluginEntry.DataModel);

            if (TestSiteService(loyaltyParams))
            {
                PluginEntry.Framework.ViewController.Add(new LoyaltyCardsView(loyaltyParams));
            }

        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Properties.Resources.RetailSetup, "Customers"), 500);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Customers")
            {
                args.Add(new PageCategory(Properties.Resources.Loyalty, "Loyalty"), 300);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Customers" && args.CategoryKey == "Loyalty")
            {
                {
                    if (PluginEntry.DataModel.HasPermission(Permission.LoyaltyTransactions))
                    {
                        args.Add(
                            new CategoryItem(
                                Properties.Resources.LoyaltyTransView,
                                Properties.Resources.LoyaltyTransView,
                                Properties.Resources.LoyaltyTransTooltipDescription,
                                CategoryItem.CategoryItemFlags.Button,
                                null,
                                Properties.Resources.loyaltyV2_32,
                                ShowLoyaltyTransView,
                                "LoyaltyTrans"),
                            10);
                    }
                    if (PluginEntry.DataModel.HasPermission(Permission.CardsEdit))
                    {
                        args.Add(new CategoryItem(
                                     Properties.Resources.NewCards,
                                     Properties.Resources.LoyaltyCardNew,
                                     Properties.Resources.NewCardTooltipDescription,
                                     CategoryItem.CategoryItemFlags.Button,
                                     NewCard,
                                     "NewCard"),
                                 20);
                    }

                    if (PluginEntry.DataModel.HasPermission(Permission.CardsView))
                    {
                        args.Add(
                            new CategoryItem(
                                Properties.Resources.LoyaltyCardsView,
                                Properties.Resources.LoyaltyCardsView,
                                Properties.Resources.LoyaltyCardsTooltipDescription,
                                CategoryItem.CategoryItemFlags.Button,
                                ShowLoyaltyCardsView,
                                "LoyaltyCards"),
                            30);
                    }

                    if (PluginEntry.DataModel.HasPermission(Permission.SchemesView))
                    {
                        args.Add(
                            new CategoryItem(
                                Properties.Resources.LoyaltySchemesView,
                                Properties.Resources.LoyaltySchemesView,
                                Properties.Resources.LoyaltySchemesTooltipDescription,
                                CategoryItem.CategoryItemFlags.Button,
                                ShowLoyaltySchemesView,
                                "LoyaltySchemes"),
                            40);
                    }

                    
                }
            }
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Properties.Resources.RetailSetup, "Retail", null), 50);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "Retail")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.CardsView) ||
                    PluginEntry.DataModel.HasPermission(Permission.SchemesView) ||
                    PluginEntry.DataModel.HasPermission(Permission.LoyaltyTransactions))
                {
                    args.Add(new Item(Properties.Resources.Loyalty, "Loyalty", null), 475);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Retail" && args.ItemKey == "Loyalty")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.CardsEdit))
                {
                    args.Add(
                        new ItemButton(Properties.Resources.LoyaltyCardNew, Properties.Resources.NewCardDescription,
                                       NewCard), 10);
                }
                if (PluginEntry.DataModel.HasPermission(Permission.CardsView))
                {
                    args.Add(
                        new ItemButton(Properties.Resources.LoyaltyCardsView,
                                       Properties.Resources.LoyaltyCardsViewDescription, ShowLoyaltyCardsView), 20);
                }
                if (PluginEntry.DataModel.HasPermission(Permission.SchemesView))
                {
                    args.Add(
                        new ItemButton(Properties.Resources.LoyaltySchemesView,
                                       Properties.Resources.LoyaltySchemesViewDescription, ShowLoyaltySchemesView), 30);
                }
                if (PluginEntry.DataModel.HasPermission(Permission.LoyaltyTransactions))
                {
                    args.Add(
                        new ItemButton(Properties.Resources.LoyaltyTransView,
                            Properties.Resources.LoyaltyTransViewDescription, ShowLoyaltyTransView), 40);
                }
            }
        }

        internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {

            if (args.ContextName == "LSOne.ViewPlugins.Customer.Views.CustomerView")
            {
                args.Add(
                    new TabControl.Tab(Properties.Resources.Loyalty,
                                                                CustomerLoyaltyPage.CreateInstance), 130);
                args.Add(
                    new TabControl.Tab(Properties.Resources.CustomerTabLoyaltyTrans,
                                                                CustomerLoyaltyTransPage.CreateInstance), 131);
            }
        }

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {

            if (arguments.CategoryKey == "LSOne.ViewPlugins.Customer.Views.CustomerView.Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.LoyaltyTransView, ShowLoyaltyTransView), 252);
                if (PluginEntry.DataModel.HasPermission(Permission.SchemesView))
                    arguments.Add(new ContextBarItem(Properties.Resources.LoyaltySchemesView, ShowLoyaltySchemesView),
                                  253);
            }
        }

        public static void NewCard(object sender, EventArgs args)
        {
            NewCard();
        }

        public static RecordIdentifier NewCard()
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.CardsEdit))
            {
                LoyaltyCustomerParams loyaltyParams = Providers.LoyaltyCustomerParamsData.Get(PluginEntry.DataModel);
                Parameters paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);

                SiteServiceProfile siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);

                if (TestSiteService(loyaltyParams))
                {
                    var dlg = new LoyaltyCardDialog(loyaltyParams, siteServiceProfile);

                    if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                    {
                        // We put null in sender so that we also get our own change hint sent.
                        selectedID = dlg.CardID;
                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Loyalty",
                                                                               selectedID, null);

                        ShowLoyaltyCardsView(selectedID, null);
                    }
                }
            }


            return selectedID;
        }
    }
}
