using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Ribbon;

namespace LSOne.ViewPlugins.GiftCards
{
    internal class PluginOperations
    {
        private static bool TestSiteService()
        {
            bool serviceConfigured = false;
            bool serviceActivce = false;

            if (PluginEntry.DataModel.SiteServiceAddress == "")
            {
                MessageDialog.Show(Properties.Resources.NoStoreServerIsSetUp);
            }
            else
            {
                serviceConfigured = true;
            }

            ISiteServiceService service =
                (ISiteServiceService) PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
            ConnectionEnum result = service.TestConnection(PluginEntry.DataModel,
                                                           PluginEntry.DataModel.SiteServiceAddress,
                                                           PluginEntry.DataModel.SiteServicePortNumber);

            PluginEntry.Framework.ViewController.CurrentView.HideProgress();


            if (result != ConnectionEnum.Success)
            {
                MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
            }
            else
            {
                serviceActivce = true;
            }

            bool serviceValid = serviceActivce && serviceConfigured;
            if (!serviceValid)
            {
                IPlugin plugin;

                plugin = PluginEntry.Framework.FindImplementor(null, "CanViewAdministrationTab", null);

                if (plugin != null)
                {
                    plugin.Message(null, "ViewSiteServiceTab", null);
                }
            }

            return serviceValid;
        }


        public static void ShowGiftCardsView(object sender, EventArgs args)
        {
            if (TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.GiftCardsView());
            }
        }

        public static void ShowGiftCard(RecordIdentifier id, IEnumerable<IDataEntity> recordBrowsingContext)
        {
            if (TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.GiftCardView(id, recordBrowsingContext));
            }
        }

        public static void NewGiftCards(object sender, EventArgs args)
        {
            if (TestSiteService())
            {
                var paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
                SiteServiceProfile siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);                

                Dialogs.NewGiftCardDialog dlg = new Dialogs.NewGiftCardDialog(siteServiceProfile);

                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    ShowGiftCard(dlg.NewGiftCards[0].ID, dlg.NewGiftCards);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add,
                                                                           "GiftCard", RecordIdentifier.Empty,
                                                                           dlg.NewGiftCards);
                }

            }
        }

        public static void NewGiftCardWithID(object sender, EventArgs args)
        {
            if (TestSiteService())
            {
                var paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
                SiteServiceProfile siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);

                Dialogs.NewGiftCardManualIDDialog dlg = new Dialogs.NewGiftCardManualIDDialog(siteServiceProfile);

                dlg.ShowDialog(PluginEntry.Framework.MainWindow);

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add,
                                                                           "GiftCard", RecordIdentifier.Empty,
                                                                           dlg.GiftCardsCreated);
            }
        }

        internal static
            void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Properties.Resources.Retail, "Retail", null), 50);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "Retail")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageGiftCards))
                {
                    args.Add(new Item(Properties.Resources.GiftcardCreditMemo, "Vouchers", null), 450);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Retail" && args.ItemKey == "Vouchers")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageGiftCards))
                {
                    args.Add(new ItemButton(Properties.Resources.NewGiftCards, Properties.Resources.NewGiftCardsDescription, NewGiftCards), 10);

                    args.Add(new ItemButton(Properties.Resources.GiftCards, Properties.Resources.GiftCardsDescription, ShowGiftCardsView), 50);
                }
            }
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Properties.Resources.Retail, "Sales"), 300);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Sales")
            {
                args.Add(new PageCategory(Properties.Resources.GiftCards, "GiftCard"), 500);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Sales" && args.CategoryKey == "GiftCard")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageGiftCards))
                {
                    args.Add(new CategoryItem(
                           Properties.Resources.GiftCards,
                           Properties.Resources.GiftCards,
                           Properties.Resources.GiftCardsTooltipDescription,
                           CategoryItem.CategoryItemFlags.DropDown,
                           null,
                           Properties.Resources.gift_cards_32,
                           null,
                           "GiftCards"), 10);
                }
            }
        }

        internal static void ConstructMenus(object sender, MenuConstructionArguments args)
        {
            ExtendedMenuItem item;

            switch (args.Key)
            {
                // We dont need to check permissions here since thats been done on the parent menu allready
                case "RibbonGiftCards":
                    item = new ExtendedMenuItem(
                        Properties.Resources.NewGiftCards + "...",
                        null,
                        10,
                        NewGiftCards);

                    args.AddMenu(item);

                    item = new ExtendedMenuItem(
                        Properties.Resources.ViewGiftCards,
                        null,
                        20,
                        ShowGiftCardsView);

                    args.AddMenu(item);

                    break;
            }
        }

        public static bool DeleteGiftCard(RecordIdentifier id, SiteServiceProfile siteServiceProfile)
        {
            bool retValue = false;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageGiftCards))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteGiftCardQuestion, Properties.Resources.DeleteGiftCard) == DialogResult.Yes)
                {
                    // We are not on head office so we need to use the Store server
                    ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

                    try
                    {
                        service.DeleteGiftCard(PluginEntry.DataModel, siteServiceProfile, id, true);
                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
                        return false;
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "GiftCard", null, null);
                }
            }
            return retValue;
        }

        public static void DeleteGiftCards(List<IDataEntity> giftCards, SiteServiceProfile siteServiceProfile)
        {
            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.ManageGiftCards))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteGiftCardsQuestion, Properties.Resources.DeleteGiftCards) == DialogResult.Yes)
                {
                    // We are not on head office so we need to use the Store server
                    ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

                    try
                    {
                        foreach (var giftCard in giftCards)
                        {
                            service.DeleteGiftCard(PluginEntry.DataModel, siteServiceProfile, giftCard.ID, false);
                        }

                        service.Disconnect(PluginEntry.DataModel);
                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
                        return;
                    }
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "GiftCard", null, giftCards);
                }
            }
        }

        public static void AddToGiftCard(RecordIdentifier giftCardID, SiteServiceProfile siteServiceProfile)
        {
            SimpleValueEditor dlg = new SimpleValueEditor(Properties.Resources.EnterAmountToAddToGiftCard, 0.0, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices), 0, 32767);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                decimal amount = 0.0m;
                // We are not on head office so we need to use the Store server
                ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

                try
                {
                    amount = service.AddToGiftCard(PluginEntry.DataModel, siteServiceProfile, giftCardID, (decimal) dlg.DoubleValue, true);
                }
                catch (Exception ex)
                {
                    MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
                    return;
                }

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.VariableChanged, "GiftCard", giftCardID, new object[] {"Amount", amount});
            }
        }
    }
}
