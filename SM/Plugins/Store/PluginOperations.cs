using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Auditing;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Companies;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportClasses.ImportExport;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.IO;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.Store.Dialogs;
using LSOne.ViewPlugins.Store.Properties;
using LSOne.ViewPlugins.Store.ViewPages;
using LSOne.ViewPlugins.Store.Views;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.ViewCore.Interfaces;
using LSOne.DataLayer.BusinessObjects.Images;
using LSOne.DataLayer.BusinessObjects.Tax;
using System.Reflection;

namespace LSOne.ViewPlugins.Store
{
    internal class PluginOperations
    {
        public static void ShowStoreHandler(object sender,EventArgs args)
        {
            if (args is ContextEventArguments)
            {
                RecordIdentifier id = ((ContextEventArguments)args).ContextID;

                if(!id.HasSecondaryID)
                {
                    PluginEntry.Framework.ViewController.Add(new StoreView(id));
                }
                else
                {
                    PluginEntry.Framework.ViewController.Add(new StoreView(id.PrimaryID,(string)id.SecondaryID));
                }
                
            }
        }

        public static void ShowTerminalHandler(object sender,EventArgs args)
        {
            if (args is ContextEventArguments)
            {
                IPlugin plugin = PluginEntry.Framework.FindImplementor(null, "ViewTerminal", null);

                if(plugin != null)
                {
                    plugin.Message(null,"ViewTerminal",((ContextEventArguments)args).ContextID);
                }
            }
        }

        public static Image ShowImageBankSelectDialog(ImageTypeEnum defaultImageType)
        {
            IPlugin plugin = PluginEntry.Framework.FindImplementor(null, "ShowImageBankSelectDialog", null);

            if(plugin != null)
            {
                return (Image)plugin.Message(null, "ShowImageBankSelectDialog", defaultImageType);
            }

            return null;
        }

        public static void ShowStore(RecordIdentifier storeID)
        {
            PluginEntry.Framework.ViewController.Add(new StoreView(storeID));
        }

        public static void ShowStore(RecordIdentifier storeID, string tabKey)
        {
            PluginEntry.Framework.ViewController.Add(new StoreView(storeID, tabKey));
        }

        public static void ShowStoresListView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new StoresView());
        }

        public static void ShowRegionsView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new RegionsView());
        }

        public static void ShowStoreTender(RecordIdentifier storeAndTenderID,string storeDescription)
        {
            PluginEntry.Framework.ViewController.Add(new StoreTenderView(storeAndTenderID, storeDescription));
        }

        public static void ShowCompanyInfo(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new CompanyInfoView());
        }

        public static void UpdateTaxExemptInformation(object sender, EventArgs args)
        {
            //Get the selected tax group from the store management tab page
            Type administrationView = PluginEntry.Framework.ViewController.CurrentView.GetType();

            if(administrationView.FullName == "LSOne.ViewPlugins.Administration.Views.AdministrationView")
            {
                PropertyInfo info = administrationView.GetProperty("SelectedTaxExemptGroup");
                DataEntity taxExemptGroup = (DataEntity)info.GetValue(PluginEntry.Framework.ViewController.CurrentView);

                string groupName = (taxExemptGroup == null || RecordIdentifier.IsEmptyOrNull(taxExemptGroup.ID)) ? Resources.NoTaxGroup : taxExemptGroup.Text;
                if (QuestionDialog.Show(Resources.AllCustomersMarkedAsTaxExemptWillBeUpdatedToUseDefaultTaxExemptTaxGroup.Replace("#1", groupName) + "\r\n" + Resources.DoYouWantToContinue) == DialogResult.Yes)
                {
                    Providers.CustomerData.UpdateTaxExemptInformation(PluginEntry.DataModel, taxExemptGroup.ID);
                    MessageDialog.Show(Resources.TaxExemptCustomersHaveBeenUpdated);
                }
            }
        }

        public static bool DeleteTerminal(RecordIdentifier id)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.TerminalEdit))
            {
                if (QuestionDialog.Show(Resources.DeleteTerminalQuestion, Resources.DeleteTerminal) == DialogResult.Yes)
                {
                    Providers.TerminalData.Delete(PluginEntry.DataModel, id);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "Terminal", id, null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }

        public static bool DeletePriceGroupLine(StoreInPriceGroup line)
        {
            bool retValue = false;

            if (PluginEntry.DataModel.HasPermission(Permission.StoreEdit))
            {
                if (QuestionDialog.Show(Resources.DeletePriceGroupLineQuestion, Resources.DeletePriceGroup) == DialogResult.Yes)
                {
                    Providers.PriceDiscountGroupData.RemoveStoreFromPriceGroup(PluginEntry.DataModel, line.StoreID, line.PriceGroupID);

                    retValue = true;
                }
            }

            return retValue;
        }

        public static bool DeleteStorePaymentMethod(RecordIdentifier storeAndPaymentMethodID)
        {

            if (PluginEntry.DataModel.HasPermission(Permission.StoreEdit))
            {
                if (QuestionDialog.Show(
                    Resources.DeletePaymentMethodQuestion,
                    Resources.DeletePaymentMethod) == DialogResult.Yes)
                {
                    Providers.StorePaymentMethodData.Delete(PluginEntry.DataModel, storeAndPaymentMethodID);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "StorePaymentMethod", storeAndPaymentMethodID, null);

                    return true;
                }
            }

            return false;
        }

        public static bool DeleteStore(RecordIdentifier id)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.StoreEdit))
            {
                List<TerminalListItem> terminals = Providers.TerminalData.GetTerminals(PluginEntry.DataModel, id);
                if (terminals != null && terminals.Count > 0)
                {
                    MessageDialog.Show(Resources.StoreHasTerminals, MessageBoxButtons.OK);
                    retValue = false;
                }
                else if (QuestionDialog.Show(Resources.DeleteStoreQuestion, Resources.DeleteStore) == DialogResult.Yes)
                {
                    Providers.StoreData.Delete(PluginEntry.DataModel, id);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "Store", id, null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }

        /// <summary>
        /// Shows the new store dialog without showing the store view
        /// </summary>
        /// <returns>The ID of the new store</returns>
        public static RecordIdentifier NewStoreDialog()
        {
            DialogResult result;
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.StoreEdit))
            {
                var dlg = new NewStoreDialog();

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog();

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.StoreID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Store", dlg.StoreID, null);
                }
            }

            return selectedID;
        }

        public static RecordIdentifier NewStore()
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.StoreEdit))
            {
                var dlg = new NewStoreDialog();

                PluginEntry.Framework.SuspendSearchBarClosing();

                var result = dlg.ShowDialog();

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.StoreID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Store", dlg.StoreID, null);

                    PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.InitialConfigurationDashboardID);

                    ShowStore(dlg.StoreID);
                }
            }

            return selectedID;
        }

        public static void NewStore(object sender, EventArgs args)
        {
            if (PluginEntry.DataModel.CurrentStoreID != RecordIdentifier.Empty)
            {
                MessageDialog.Show(Resources.OnlyHeadOffice);
            }
            else
            {
                NewStore();
            }
        }

        public static Region EditRegion(RecordIdentifier regionID)
        {
            RegionDialog dialog;

            if(regionID == RecordIdentifier.Empty)
            {
                dialog = new RegionDialog();
            }
            else
            {
                dialog = new RegionDialog(regionID);
            }

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "Region", regionID, null);
                return dialog.StoreRegion;
            }

            return null;
        }

        public static void ChangeCurrentStore(object sender, EventArgs args)
        {
            PluginEntry.Framework.ShowOptionsDialog();
        }
       
        public static void AddSearchHandler(object sender, SearchBarConstructionArguments args)
        {
            if(PluginEntry.DataModel.HasPermission(Permission.StoreView))
            {
                PluginEntry.storeSearchProvider = new StoreSearchPanelFactory();

                args.AddItem(Resources.Stores, PluginEntry.StoreImageIndex,PluginEntry.storeSearchProvider , 100);
            }
        }

        public static void DeleteStoreContextHandler(object sender, EventArgs args)
        {
            var lv = (System.Windows.Forms.ListView)PluginEntry.Framework.GetContextMenuContext();

            DeleteStore((RecordIdentifier)lv.SelectedItems[0].Tag);
        }

        public static void DeleteTerminalContextHandler(object sender, EventArgs args)
        {
            var lv = (System.Windows.Forms.ListView)PluginEntry.Framework.GetContextMenuContext();

            DeleteTerminal((RecordIdentifier)lv.SelectedItems[0].Tag);
        }

        public static void EditStoreStoreContextHandler(object sender, EventArgs args)
        {
            var lv = (System.Windows.Forms.ListView)PluginEntry.Framework.GetContextMenuContext();

            ShowStore((RecordIdentifier)lv.SelectedItems[0].Tag);
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Resources.StoreSetup, "Store setup", Properties.Resources.store_setup_green_16),75);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "Store setup")
            {
                if(PluginEntry.DataModel.HasPermission(Permission.StoreView))
                {
                    args.Add(new Item(Resources.Stores, "Stores", null), 100);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Store setup" && args.ItemKey == "Stores")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.StoreEdit))
                {
                    args.Add(new ItemButton(Resources.NewStore, Resources.NewStoreDescription, NewStore), 100);
                }
                args.Add(new ItemButton(Resources.Stores, Resources.ViewAllStoresDescription, ShowStoresListView), 200);
                args.Add(new SearchItemButton(Resources.SearchForStore, SearchStores), 300);

                if (PluginEntry.DataModel.HasPermission(Permission.StoreView))
                {
                    args.Add(new ItemButton(Resources.CompanyInformation, Resources.CompanyInfoDescription, ShowCompanyInfo), 400);
                }
            }
        }

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == "LSOne.ViewPlugins.LookupValues.Views.CurrencyView.Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.EditCompanyCurrency, ShowCompanyInfo), 300);
            }

            if (arguments.CategoryKey == "LSOne.ViewPlugins.Administration.Views.AdministrationView.Related")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.AdministrationMaintainSettings))
                {
                    Type administrationType = arguments.View.GetType();
                    PropertyInfo info = administrationType.GetProperty("StoreManagementTabSelected");
                    bool storeManagementTabSelected = (bool)info.GetValue(arguments.View);
                    
                    if(storeManagementTabSelected)
                    {
                        arguments.Add(new ContextBarItem(Resources.UpdateTaxExemptTaxGroup, UpdateTaxExemptInformation), 400);
                    }
                }
            }
        }       

        private static void SearchStores(object sender, SearchEventArgs args)
        {
            PluginEntry.Framework.Search(PluginEntry.storeSearchProvider, args.Text);
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Resources.Sites, "Sites"), 700);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Sites")
            {
                args.Add(new PageCategory(Resources.SitesTerminals, "SiteTerminal"), 100);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Sites" && args.CategoryKey == "SiteTerminal")
            {
                /*if (PluginEntry.DataModel.HasPermission(Permission.StoreEdit) ||
                    PluginEntry.DataModel.HasPermission(Permission.TerminalEdit))
                {
                    args.Add(new CategoryItem(
                            Resources.NewStoreTerminal,
                            Resources.NewStoreTerminal,
                            Resources.AddStoresAndTerminals,
                            CategoryItem.CategoryItemFlags.Button | CategoryItem.CategoryItemFlags.DropDownSplit | CategoryItem.CategoryItemFlags.EndOfGroup,
                            null,
                            Resources.new_store_32,
                            NewStore,
                            "NewStore"), 10);
                }*/

                if (PluginEntry.DataModel.HasPermission(Permission.StoreView) || PluginEntry.DataModel.HasPermission(Permission.StoreEdit))
                {
                    args.Add(new CategoryItem(
                                Resources.Stores,
                                Resources.Stores,
                                Resources.StoresTooltipDescription,
                                CategoryItem.CategoryItemFlags.Button,
                                null,
                                Resources.store_32,
                                ShowStoresListView,
                                "ViewStores"), 10);

                    args.Add(
                        new CategoryItem(
                            Resources.Regions,
                            Resources.Regions,
                            Resources.RegionsTooltip,
                            CategoryItem.CategoryItemFlags.Button,
                            Resources.company_information_16,
                            ShowRegionsView,
                            "RegionsView"), 35);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.StoreView))
                {
                    args.Add(
                        new CategoryItem(
                            Resources.CompanyInformation,
                            Resources.CompanyInformation,
                            Resources.CompanyInfoTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            Resources.company_information_16,
                            ShowCompanyInfo,
                            "CompanyInfo"), 30);
                }
            }
        }

        internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            if (args.ContextName == "LSOne.ViewPlugins.Administration.Views.AdministrationView" && PluginEntry.DataModel.HasPermission(Permission.AdministrationMaintainSettings))
            {                
                args.Add(new TabControl.Tab(Resources.StoreManagement, "StoreManagement", StoreManagementAdminPage.CreateInstance), 80);
            }

            if (args.ContextName == "LSOne.ViewPlugins.Store.Views.StoreView")
            {
                args.Add(new TabControl.Tab(Resources.Settings, "SettingsPage", StoreSettingsPage.CreateInstance), 100);
                args.Add(new TabControl.Tab(Resources.FormSettings, StoreFormSettingsPage.CreateInstance), 125);
                args.Add(new TabControl.Tab(Resources.AllowedPaymentTypes, "AllowedPaymentTypesPage", StorePaymentMethodPage.CreateInstance), 150);
                if (PluginEntry.DataModel.HasPermission(Permission.ManageTradeAgreementPrices))
                {
                    args.Add(
                        new TabControl.Tab(Resources.LocationPriceGroups, StoreLocationPriceGroupPage.CreateInstance),
                        200);
                }
                if (PluginEntry.DataModel.HasPermission(Permission.IncomeExpenseView))
                {
                    args.Add(new TabControl.Tab(Resources.IncomeExpenseAccounts, IncomeExpenseAccountPage.CreateInstance), 250);
                }
            }
        }

        internal static object[] SelectLocation()
        {
            var dlg = new SelectLocationDialog(false);

            if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                return new object[]{DialogResult.OK,dlg.StoreID};
            }
            return new object[]{DialogResult.Cancel,dlg.StoreID};
        }

        internal static void OnLogin(object sender, LoginEventArgs args)
        {
            if (args.LoginEventType == LoginEventArgs.LoginEventTypeEnum.Login)
            {
                // No point in checking if we need to run the easy store wizard if we do not have any permission
                if (PluginEntry.DataModel.HasPermission(Permission.StoreEdit))
                {
                    Parameters data = Providers.ParameterData.Get(PluginEntry.DataModel);
                    if (data.LocalStore == RecordIdentifier.Empty || data.LocalStore == "")
                    {
                        // Local store is not properly set so we check if we have some store.
                        List<StoreListItem> stores = Providers.StoreData.Search(PluginEntry.DataModel, new StoreListSearchFilter());

                        if (stores.Count > 0)
                        {
                            // We have a store so only the default store needs to be correctly set
                            data.LocalStore = stores[0].ID;

                            Providers.ParameterData.Save(PluginEntry.DataModel, data);
                        }
                        else
                        {
                            // We dont have a store and we dont have a local store set
                            //Dialogs.CreateStoreWizard wiz = new Dialogs.CreateStoreWizard(PluginEntry.DataModel);

                            //wiz.ShowDialog(PluginEntry.Framework.MainWindow);
                        }
                    }
                }

                // finally we check if no location is selected then we pop up a dialog that lets the user select a location
                if (PluginEntry.DataModel.CurrentStoreID == 0)
                {
                    var dlg = new SelectLocationDialog(true);

                    dlg.ShowDialog(PluginEntry.Framework.MainWindow);
                }
            }
        }
    }
}
