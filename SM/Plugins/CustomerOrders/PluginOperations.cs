using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.CustomerOrders.Dialogs;
using LSOne.ViewPlugins.CustomerOrders.Properties;

namespace LSOne.ViewPlugins.CustomerOrders
{
    internal class PluginOperations
    {

        private static bool TestSiteService()
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

                ISiteServiceService service = (ISiteServiceService) PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
                ConnectionEnum result = service.TestConnection(PluginEntry.DataModel,
                    siteServiceProfile.SiteServiceAddress,
                    (ushort) siteServiceProfile.SiteServicePortNumber);

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

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {

            if (arguments.CategoryKey == "LSOne.ViewPlugins.CustomerOrders.Views.CustomerOrderSettingsView.Related")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.AdministrationEditNumberSequences) && PluginEntry.Framework.CanRunOperation("ShowNumberSequences"))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.NumberSequences, ShowNumberSequences), PluginPriority.NumberSequenceRelatedKey);
                }
            }
        }

        private static void ShowNumberSequences(object sender, ContextBarClickEventArguments args)
        {
            PluginOperationArguments operationArgs = new PluginOperationArguments(RecordIdentifier.Empty, null, true);

            PluginEntry.Framework.RunOperation("ShowNumberSequences", null, operationArgs);

            PluginEntry.Framework.ViewController.Add(operationArgs.ResultView);
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Properties.Resources.RetailSetup, PluginKeys.CustomersKey), 500);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == PluginKeys.CustomersKey)
            {
                args.Add(new PageCategory(Resources.OrdersAndQuotes, PluginKeys.CustomerOrdersKey), 400);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == PluginKeys.CustomersKey && args.CategoryKey == PluginKeys.CustomerOrdersKey)
            {
                {
                    if (PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrderSettings))
                    {
                        args.Add(new CategoryItem(
                                     Resources.Settings,
                                     Resources.OrdersAndQuotes,
                                     Properties.Resources.CustomerOrderSettingsTooltipDescription,
                                     CategoryItem.CategoryItemFlags.Button,
                                     Properties.Resources.customer_order_settings_16,
                                     null,
                                     ShowSettings,
                                     PluginKeys.SettingsKey),
                                 PluginPriority.SettingsPriority);
                    }

                    if (PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrders))
                    {
                        args.Add(new CategoryItem(
                                     Properties.Resources.Orders,
                                     Resources.OrdersAndQuotes,
                                     Properties.Resources.CustomerOrderTooltipDescription,
                                     CategoryItem.CategoryItemFlags.Button,
                                     Properties.Resources.customer_orders_16,
                                     null,
                                     ShowCustomerOrders,
                                     PluginKeys.CustomerOrdersKey),
                                 PluginPriority.CustomerOrdersPriority);
                    }

                    if (PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrders))
                    {
                        args.Add(new CategoryItem(
                                     Properties.Resources.Quotes,
                                     Resources.OrdersAndQuotes,
                                     Properties.Resources.QuotesDescription,
                                     CategoryItem.CategoryItemFlags.Button,
                                     Properties.Resources.customer_orders_16,
                                     null,
                                     ShowQuotes,
                                     PluginKeys.QuotesKey),
                                 PluginPriority.QuotesPriority);
                    }
                }
            }
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Properties.Resources.RetailSetup, PluginKeys.RetailKey, null), 50);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == PluginKeys.RetailKey)
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrders) || PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrderSettings))
                {
                    args.Add(new Item(Properties.Resources.CustomerOrders, PluginKeys.CustomerOrdersKey, null), 475);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == PluginKeys.RetailKey && args.ItemKey == PluginKeys.CustomerOrdersKey)
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrders))
                {
                    args.Add(new ItemButton(Properties.Resources.CustomerOrders, Properties.Resources.CustomerOrderDescription, ShowCustomerOrders), PluginPriority.CustomerOrdersPriority);
                    args.Add(new ItemButton(Properties.Resources.Quotes, Properties.Resources.QuotesDescription, ShowQuotes), PluginPriority.QuotesPriority);
                }
                
                if (PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrderSettings))
                {
                    args.Add(new ItemButton(Properties.Resources.CustomerOrderSettings, Properties.Resources.CustomerOrderSettingsDescription, ShowSettings), PluginPriority.SettingsPriority);
                }

            }
        }

        public static void ShowCustomerOrders(object sender, EventArgs args)
        {
            if (TestSiteService())
            {
                if (PluginEntry.Framework.ViewController.CurrentView.HeaderText == Resources.Quotes)
                {
                    PluginEntry.Framework.ViewController.CloseCurrentView();
                }
                PluginEntry.Framework.ViewController.Add(new Views.CustomerOrdersView(CustomerOrderType.CustomerOrder));
            }
        }

        public static void ShowQuotes(object sender, EventArgs args)
        {
            if (TestSiteService())
            {
                if (PluginEntry.Framework.ViewController.CurrentView.HeaderText == Resources.CustomerOrders)
                {
                    PluginEntry.Framework.ViewController.CloseCurrentView();
                }
                PluginEntry.Framework.ViewController.Add(new Views.CustomerOrdersView(CustomerOrderType.Quote));
            }
        }

        public static void ShowSettings(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CustomerOrderSettingsView());
        }

        /// <summary>
        /// Displays the edit/new dialog for creating/editing configurations such as delivery and source
        /// </summary>
        /// <param name="configuration">The configuration to be edited. If empty or null the dialog will create a new configuration</param>
        /// <returns>If the user clicks OK this is the configuration that was created/edited in the dialog. Returns null if no permission or cancel was pressed</returns>
        public static void EditConfigurationDialog(CustomerOrderAdditionalConfigurations configuration)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrderSettings))
            {
                bool configInUse = configuration.ID != Guid.Empty && Providers.CustomerOrderAdditionalConfigData.ConfigIsInUse(PluginEntry.DataModel, configuration);
                Dialogs.NewConfigurationDialog dlg = new Dialogs.NewConfigurationDialog(configuration, configInUse);

                PluginEntry.Framework.SuspendSearchBarClosing();

                dlg.ShowDialog();

                PluginEntry.Framework.ResumeSearchBarClosing();
            }
        }

        public static bool DeleteConfiguration(CustomerOrderAdditionalConfigurations toDelete)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrderSettings))
            {
                if (Providers.CustomerOrderAdditionalConfigData.ConfigIsInUse(PluginEntry.DataModel, toDelete))
                {
                    MessageDialog.Show(Properties.Resources.ConfigurationInUse, Properties.Resources.DeleteConfiguration, MessageBoxButtons.OK);
                    return false;
                }

                if (QuestionDialog.Show(Properties.Resources.DeleteConfigurationQuestion, Properties.Resources.DeleteConfiguration) == DialogResult.Yes)
                {
                    Providers.CustomerOrderAdditionalConfigData.Delete(PluginEntry.DataModel, toDelete.ID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "Configuration", toDelete.ID, toDelete);
                    return true;
                }
            }

            return false;
        }

        public static bool DeleteCustomerOrders(List<CustomerOrder> toDelete)
        {
            if (toDelete == null || !toDelete.Any())
            {
                return false;
            }

            if (!TestSiteService())
            {
                return false;
            }

            if (!PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrders)) return false;


            ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
            Parameters paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);

            try
            {
                SiteServiceProfile siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);

                CustomerOrderType orderType = toDelete[0].OrderType;

                if (QuestionDialog.Show(orderType == CustomerOrderType.CustomerOrder ? Resources.DeleteCustomerOrderQuestion : Resources.DeleteQuoteQuestion, 
                                        orderType == CustomerOrderType.CustomerOrder ? Resources.DeleteCustomerOrder : Resources.DeleteQuote) == DialogResult.Yes)
                {
                    foreach (CustomerOrder order in toDelete)
                    {
                        order.Status = CustomerOrderStatus.Deleted;
                        service.SaveCustomerOrder(PluginEntry.DataModel, siteServiceProfile, order);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "CustomerOrders", null, null);
                    return true;
                }

                return false;
            }
            finally
            {
                service.Disconnect(PluginEntry.DataModel);
            }
        }

        public static bool EditCustomerOrdersDetails(List<CustomerOrder> toEdit)
        {
            if (toEdit == null || !toEdit.Any())
            {
                return false;
            }

            if (!TestSiteService())
            {
                return false;
            }

            if (!PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrders)) return false;

            EditCustomerOrderDetails dlg = new EditCustomerOrderDetails(PluginEntry.DataModel, toEdit);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
                Parameters paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);

                try
                {
                    SiteServiceProfile siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);

                    foreach (CustomerOrder order in dlg.toEdit)
                    {
                        service.SaveCustomerOrderDetails(PluginEntry.DataModel, siteServiceProfile, order);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiAdd, "CustomerOrders", null, null);
                }
                finally
                {
                    service.Disconnect(PluginEntry.DataModel);
                }
                return true;
            }

            return false;
        }
    }
}
