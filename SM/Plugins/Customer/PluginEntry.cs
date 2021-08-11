using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Tools;
using LSOne.ViewCore;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Customer
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;
        internal static int CustomerImageIndex = -1;
        internal static CustomerSearchPanelFactory CustomerSearchProvider = null;

        private void ConstructMenus(object sender, MenuConstructionArguments args)
        {
            ExtendedMenuItem item;
            SearchListViewItem selectedItem;

            switch (args.Key)
            {
                case "Insert":
                    if (DataModel.HasPermission(Permission.CustomerEdit))
                    {

                        item = new ExtendedMenuItem(
                            Properties.Resources.NewCustomer + "...",
                            Properties.Resources.new_customer_16,
                            80,
                            new EventHandler(PluginOperations.NewCustomer));

                        args.AddMenu(item);
                    }
                    break;

                case "CustomerSearchList":
                case "AllSearchList":
                    if (args.Key != "AllSearchList")
                    {
                        if (DataModel.HasPermission(Permission.CustomerEdit))
                        {

                            item = new ExtendedMenuItem(
                                Properties.Resources.NewCustomer + "...",
                                Properties.Resources.NewCusomerImage,
                                80,
                                new EventHandler(PluginOperations.NewCustomer));

                            args.AddMenu(item);
                        }
                    }

                    if (((System.Windows.Forms.ListView)args.Context).SelectedItems.Count > 0)
                    {
                        selectedItem = ((SearchListViewItem)((System.Windows.Forms.ListView)args.Context).SelectedItems[0]);

                        if (selectedItem.Key == "Customer")
                        {
                            item = new ExtendedMenuItem(
                                Properties.Resources.EditString + "...",
                                Properties.Resources.Edit,
                                50,
                                new EventHandler(PluginOperations.EditCustomerContextHandler));

                            item.Default = true;

                            args.AddMenu(item);


                            if (DataModel.HasPermission(Permission.CustomerEdit))
                            {

                                item = new ExtendedMenuItem(
                                    Properties.Resources.Delete + "...",
                                    Properties.Resources.MinusImage,
                                    150,
                                    new EventHandler(PluginOperations.DeleteCustomerContextHandler));

                                args.AddMenu(item);
                            }
                        }
                    }

                    break;
            }
        }

        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.CustomerManagement; }
        }


        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            if (message == "CanEditCustomer" || message == "CanSearchCustomer")
            {
                return DataModel.HasPermission(Permission.CustomerView);
            }
            else if (message == "CanCreateCustomer")
            {
                return DataModel.HasPermission(Permission.CustomerEdit);
            }

            return false;
        }

        public object Message(object sender, string message, object parameters)
        {
            if (message == "EditCustomer")
            {
                if (parameters is RecordIdentifier)
                {
                    PluginOperations.ShowCustomer((RecordIdentifier)parameters);
                }
                else
                {
                    PluginOperations.ShowCustomer((RecordIdentifier)((object[])parameters)[0], (IEnumerable<IDataEntity>)((object[])parameters)[1]);
                }
                
            }
            else if (message == "SearchCustomer")
            {
                PluginEntry.Framework.Search(PluginEntry.CustomerSearchProvider, (SearchParameter[])parameters);
            }
            else if (message == "NewCustomer")
            {
                PluginOperations.NewCustomer();
            }


            return null;
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            ImageList iml;
            DataModel = dataModel;
            Framework = frameworkCallbacks;

            // Register Icons that this plugin uses to the framework
            // -------------------------------------------------
            iml = frameworkCallbacks.GetImageList();

            iml.Images.Add(Properties.Resources.customer_16);
            CustomerImageIndex = iml.Images.Count - 1;

            // We want to be able to register items various of context menus
            frameworkCallbacks.AddMenuConstructionConstructionHandler(new MenuConstructionEventHandler(ConstructMenus));

            frameworkCallbacks.AddSearchBarConstructionHandler(new SearchBarConstructionEventHandler(PluginOperations.AddSearchHandler));

            frameworkCallbacks.AddOperationCategoryConstructionHandler(new OperationCategoryEventHandler(PluginOperations.AddOperationCategoryHandler));
            frameworkCallbacks.AddOperationItemConstructionHandler(new OperationItemEventHandler(PluginOperations.AddOperationItemHandler));
            frameworkCallbacks.AddOperationButttonConstructionHandler(new OperationbuttonEventHandler(PluginOperations.AddOperationButtonHandler));

            frameworkCallbacks.AddRibbonPageConstructionHandler(new RibbonPageEventHandler(PluginOperations.AddRibbonPages));
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(new RibbonPageCategoryEventHandler(PluginOperations.AddRibbonPageCategories));
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(new RibbonPageCategoryItemEventHandler(PluginOperations.AddRibbonPageCategoryItems));
        }

        public void Dispose()
        {
            
        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation(Properties.Resources.ViewAllCustomers, PluginOperations.ShowCustomersListView, Permission.CustomerView);
            operations.AddOperation(Properties.Resources.NewCustomer, PluginOperations.NewCustomer, Permission.CustomerEdit);
        }

        #endregion
    }
}
