using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Ribbon;

namespace LSOne.ViewPlugins.CreditVouchers
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
                (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
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

        public static void ShowCreditVouchersView(object sender, EventArgs args)
        {
            if (TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.CreditVouchersView());
            }
        }

        public static void ShowCreditVoucherView(RecordIdentifier id, IEnumerable<IDataEntity> recordBrowsingContext)
        {
            if (TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.CreditVoucherView(id, recordBrowsingContext));
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
                if (PluginEntry.DataModel.HasPermission(Permission.ManageCreditVouchers))
                {
                    args.Add(new Item(Properties.Resources.GiftcardsCreditMemo, "Vouchers", null), 450);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Retail" && args.ItemKey == "Vouchers")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageCreditVouchers))
                {
                    args.Add(new ItemButton(Properties.Resources.CreditMemos, Properties.Resources.CreditMemoDescription, ShowCreditVouchersView), 100);
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
                args.Add(new PageCategory(Properties.Resources.CreditMemo, "CreditMemo"), 400);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Sales" && args.CategoryKey == "CreditMemo")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageCreditVouchers))
                {
                    args.Add(new CategoryItem(
                           Properties.Resources.CreditMemos,
                           Properties.Resources.CreditMemos,
                           Properties.Resources.CreditMemoTooltipDescription,
                           CategoryItem.CategoryItemFlags.Button,
                           null,
                           Properties.Resources.credit_memos_32,
                           ShowCreditVouchersView,
                           "CreditMemos"), 10);
                }
            }
        }

        /*internal static void ConstructMenus(object sender, MenuConstructionArguments args)
        {
            ExtendedMenuItem item;

            switch (args.Key)
            {
                // We dont need to check permissions here since thats been done on the parent menu allready
                case "RibbonCreditVouchers":

                    item = new ExtendedMenuItem(
                        Properties.Resources.ViewCreditMemos,
                        null,
                        50,
                        ShowCreditVouchersView);

                    args.AddMenu(item);

                    break;
            }
        }*/

        public static bool DeleteCreditVoucher(RecordIdentifier id, SiteServiceProfile siteServiceProfile)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageCreditVouchers))
            {
                if (
                    QuestionDialog.Show(Properties.Resources.DeleteCreditMemoQuestion,
                                        Properties.Resources.DeleteCreditMemo) == DialogResult.Yes)
                {
                    // We are not on head office so we need to use the Store server
                    ISiteServiceService service =
                        (ISiteServiceService) PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

                    try
                    {
                        service.DeleteCreditVoucher(PluginEntry.DataModel, siteServiceProfile, id, true);
                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
                        return false;
                    }
                }

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "CreditVoucher", null, null);
                return true;
            }
            return false;
        }

        public static void DeleteCreditVouchers(List<IDataEntity> creditVouchers, SiteServiceProfile siteServiceProfile)
        {
            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.ManageCreditVouchers)) 
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteCreditMemosQuestion, Properties.Resources.DeleteCreditMemos) == DialogResult.Yes) 
                {

                        // We are not on head office so we need to use the Store server
                    ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

                        try
                        {
                            foreach (var voucher in creditVouchers)
                            {
                                service.DeleteCreditVoucher(PluginEntry.DataModel, siteServiceProfile, voucher.ID, false);
                            }

                            service.Disconnect(PluginEntry.DataModel);
                        }
                        catch (Exception ex)
                        {
                            MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
                            return;
                        }
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "CreditVoucher", null, creditVouchers);
                //}
            }
        }

        public static void AddToCreditVoucher(RecordIdentifier creditVoucherID, SiteServiceProfile siteServiceProfile)
        {
            SimpleValueEditor dlg = new SimpleValueEditor(Properties.Resources.EnterAmountToAddToCreditMemo, 0.0, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices), 0, 32767);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                decimal amount = 0.0m;
                RecordIdentifier storeID = (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty) ? "" : PluginEntry.DataModel.CurrentStoreID;

                /*if (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty)
                {
                    amount = Providers.CreditVoucherData.AddToCreditVoucher(PluginEntry.DataModel, creditVoucherID, (decimal)dlg.DoubleValue, storeID, "", PluginEntry.DataModel.CurrentUser.ID, "");
                }
                else
                {*/
                    // We are not on head office so we need to use the Store server
                ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

                    try
                    {
                        amount = service.AddToCreditVoucher(PluginEntry.DataModel, siteServiceProfile, creditVoucherID, (decimal)dlg.DoubleValue, true);
                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
                        return;
                    }     
                //}

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.VariableChanged, "CreditVoucher", creditVoucherID, new object[] { "Amount", amount });
            }
        }

        
    }
}
