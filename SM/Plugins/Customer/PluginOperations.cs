using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.Services.Interfaces.SupportClasses.IDialog;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.Customer.Dialogs;
using Item = LSOne.Controls.Item;
using ListView = System.Windows.Forms.ListView;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Tax;

namespace LSOne.ViewPlugins.Customer
{
    internal class PluginOperations
    {
        public static void ShowCustomer(RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CustomerView(id));
        }

        public static void ShowCustomer(RecordIdentifier id, IEnumerable<IDataEntity> recordBrowsingContext)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CustomerView(id, recordBrowsingContext));
        }

        public static void ShowCustomer(object sender, ListItemEventArguments args)
        {
            ShowCustomer(args.Item.ID);
        }

        public static void ShowCustomersListView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CustomersView());
        }

        public static void ShowCustomerGroupListView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CustomerGroupsView());
        }

        public static void ShowCustomerGroupView(RecordIdentifier customerGroupID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CustomerGroupView(customerGroupID));
        }

        public static void ShowGroupCategoriesListView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CategoriesView());
        }

        public static void DeleteCustomers(List<RecordIdentifier> IDs)
        {
            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.CustomerEdit))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteCustomersQuestion, Properties.Resources.DeleteCustomers) == DialogResult.Yes)
                {
                    foreach (var ID in IDs)
                    {
                        if (!Providers.CustomerData.CustomerCanBeDeleted(PluginEntry.DataModel, ID))
                        {
                            if (QuestionDialog.Show(Properties.Resources.CustomerHasDependantRecordsQuestion,
                            Properties.Resources.CustomerHasDependantRecords) != DialogResult.Yes)
                            {
                                continue;
                            }
                        }
                        Providers.CustomerData.Delete(PluginEntry.DataModel, ID);
                        var groupsForCustomer = Providers.CustomerGroupData.GetGroupsForCustomer(PluginEntry.DataModel, ID);
                        foreach (var customerGroup in groupsForCustomer)
                        {
                            Providers.CustomerGroupData.DeleteCustomerFromGroup(PluginEntry.DataModel, ID, customerGroup.ID);
                        }
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "Customer", null, IDs);
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();
        }

        public static bool DeleteCustomer(RecordIdentifier ID)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.CustomerEdit))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteCustomerQuestion, Properties.Resources.DeleteCustomer) == DialogResult.Yes)
                {
                    if (!Providers.CustomerData.CustomerCanBeDeleted(PluginEntry.DataModel, ID))
                    {
                        if (QuestionDialog.Show(Properties.Resources.CustomerHasDependantRecordsQuestion, Properties.Resources.CustomerHasDependantRecords) == DialogResult.Yes)
                        {
                            Providers.CustomerData.Delete(PluginEntry.DataModel, ID);
                            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "Customer", ID, null);
                            return true;
                        }
                        return false;
                    }
                    Providers.CustomerData.DeleteCustomer(PluginEntry.DataModel, ID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "Customer", ID, null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }

        public static RecordIdentifier NewCustomer()
        {
            DialogResult result;
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.CustomerEdit))
            {
                Dialogs.NewCustomerDialog dlg = new Dialogs.NewCustomerDialog();

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog();

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.CustomerID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Customer", selectedID, null);

                    if (!dlg.MultiAdd)
                    {
                        PluginOperations.ShowCustomer(selectedID);
                    }
                }
            }

            return selectedID;
        }

        public static void NewCustomer(object sender, EventArgs args)
        {
            NewCustomer();
        }

        public static void DeleteCustomerContextHandler(object sender, EventArgs args)
        {
            ListView lv = (ListView)PluginEntry.Framework.GetContextMenuContext();

            DeleteCustomer((RecordIdentifier)lv.SelectedItems[0].Tag);
        }

        public static void EditCustomerContextHandler(object sender, EventArgs args)
        {
            ListView lv = (ListView)PluginEntry.Framework.GetContextMenuContext();

            ShowCustomer((RecordIdentifier)lv.SelectedItems[0].Tag);
        }

        public static void AddSearchHandler(object sender, SearchBarConstructionArguments args)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.CustomerView))
            {
                PluginEntry.CustomerSearchProvider = new CustomerSearchPanelFactory();

                args.AddItem(Properties.Resources.Customers, PluginEntry.CustomerImageIndex, PluginEntry.CustomerSearchProvider, 140);
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
                if (PluginEntry.DataModel.HasPermission(Permission.CustomerView))
                {
                    args.Add(new Item(Properties.Resources.Customers, "Customers", null), 300);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Retail" && args.ItemKey == "Customers")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.CustomerView))
                {
                    if (PluginEntry.DataModel.HasPermission(Permission.CustomerEdit))
                    {
                        args.Add(new ItemButton(Properties.Resources.NewCustomer, Properties.Resources.NewCustomerDescription, new EventHandler(NewCustomer)), 100);
                    }
                    
                    args.Add(new ItemButton(Properties.Resources.Customers, Properties.Resources.ViewAllCustomersDescription, new EventHandler(ShowCustomersListView)), 200);
                    
                    args.Add(new SearchItemButton(Properties.Resources.SearchForCustomer, new SearchEventHandler(SearchCustomers)), 300);
                    if (PluginEntry.DataModel.HasPermission(Permission.ManageCustomerGroups))
                    {
                        args.Add(
                            new ItemButton(Properties.Resources.CustomerGroups,
                                Properties.Resources.ViewAllCustomerGroupDescription,
                                new EventHandler(ShowCustomerGroupListView)), 400);

                        args.Add(
                            new ItemButton(Properties.Resources.GroupCategories,
                                Properties.Resources.ViewAllCategoriesDescription,
                                new EventHandler(ShowGroupCategoriesListView)), 500);
                    }
                }
            }
        }

        private static void SearchCustomers(object sender, SearchEventArgs args)
        {
            PluginEntry.Framework.Search(PluginEntry.CustomerSearchProvider, args.Text);
        }


        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Properties.Resources.Customers, "Customers"), 500);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Customers")
            {
                args.Add(new PageCategory(Properties.Resources.Customers, "Customers"), 100);
                args.Add(new PageCategory(Properties.Resources.Groups, "CustomerGroups"), 200);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Customers" && args.CategoryKey == "Customers")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.CustomerEdit))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.CustomerNew,
                            Properties.Resources.NewCustomer,
                            Properties.Resources.NewCustomerTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            null,
                            Properties.Resources.new_customer_32,
                            new EventHandler(NewCustomer),
                            "NewCustomer"), 10);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.CustomerView))
                {
                    args.Add(new CategoryItem(
                        Properties.Resources.Customers,
                        Properties.Resources.Customers,
                        Properties.Resources.CustomersTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button | CategoryItem.CategoryItemFlags.BeginOfGroup,
                        Properties.Resources.customer_16,
                        new EventHandler(ShowCustomersListView),
                        "ViewAll"),20);
                }
                //Customer card should add itself here from lookup values
                /*args.AddEditField(new CategoryItem(
                           Properties.Resources.Search,
                           Properties.Resources.Search,
                           Properties.Resources.SearchForCustomerTooltip,
                           CategoryItem.CategoryItemFlags.None,
                           "SearchCustomer"), 40, new SearchEventHandler(SearchCustomers));*/
               
            }
            if (args.PageKey == "Customers" && args.CategoryKey == "CustomerGroups")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageCustomerGroups))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.Groups,
                            Properties.Resources.CustomerGroups,
                            Properties.Resources.CustomerGroupsTooltip,
                            CategoryItem.CategoryItemFlags.Button,
                            Properties.Resources.customer_group_16,
                            null,
                            new EventHandler(ShowCustomerGroupListView),
                            "CustomerGroups"), 10);
                
                    args.Add(new CategoryItem(
                            Properties.Resources.GroupCategories,
                            Properties.Resources.GroupCategories,
                            Properties.Resources.GroupCategoriesTooltip,
                            CategoryItem.CategoryItemFlags.Button,
                            Properties.Resources.group_categories_16,
                            null,
                            new EventHandler(ShowGroupCategoriesListView),
                            "GroupCategories"), 20);
                }
                //Customer/Store line discount group should add itself here
            }
        }

        #region TaxExempt

        public static SalesTaxGroup GetTaxExemptInformation()
        {
            Parameters parameters = Providers.ParameterData.Get(PluginEntry.DataModel);
            if (parameters != null && !RecordIdentifier.IsEmptyOrNull(parameters.TaxExemptTaxGroup))
            {
                return Providers.SalesTaxGroupData.Get(PluginEntry.DataModel, parameters.TaxExemptTaxGroup);                
            }

            return null;
        }

        #endregion

        #region Categories

        /// <summary>
        /// Creates or edits a group category. If the category was edited or a new one created the edited/new category is returned
        /// </summary>
        /// <param name="category">If empty then a new category is created</param>
        /// <returns>The group category that was created/edited</returns>
        public static GroupCategory EditCategory(GroupCategory category)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.CategoriesEdit))
            {
                NewCategoryDialog dlg = new NewCategoryDialog(category);
                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                {
                    return dlg.Category;
                }
            }
            return category;
        }

        /// <summary>
        /// Deletes a category with a specific ID. If the category is in use on a customer group the delete operation is not allowed
        /// </summary>
        /// <param name="categoryID">The ID to be deleted</param>
        public static void DeleteCategory(RecordIdentifier categoryID)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.CategoriesEdit))
            {
                var groupCategoryData = Providers.GroupCategoryData;
                if (groupCategoryData.CategoryUsedInGroups(PluginEntry.DataModel, categoryID))
                {
                    MessageDialog.Show(Properties.Resources.CategoryAlreadyInUse, MessageBoxButtons.OK);
                    return;
                }

                if (QuestionDialog.Show(Properties.Resources.AreYouSureDeleteCategories) == DialogResult.No)
                {
                    return;
                }

                groupCategoryData.Delete(PluginEntry.DataModel, categoryID);

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, NotifyContexts.CategoriesView, categoryID, new GroupCategory());
            }
        }

        #endregion

        #region Customer Groups

        public static CustomerGroup NewCustomerGroup()
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageCustomerGroups))
            {
                NewCustomerGroupDialog dlg = new NewCustomerGroupDialog();
                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                {
                    return dlg.Group;
                }
            }
            return null;
        }

        public static void EditGroupsOnCustomer(RecordIdentifier customerID)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageCustomerGroups))
            {
                GroupsOnCustomerMultiEditDialog dlg = new GroupsOnCustomerMultiEditDialog(customerID);
                dlg.ShowDialog();
            }
        }

        public static bool DeleteCustomerGroup(RecordIdentifier groupID)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageCustomerGroups))
            {
                if (Providers.CustomerGroupData.GroupHasCustomers(PluginEntry.DataModel, groupID))
                {
                    MessageDialog.Show(Properties.Resources.CustomerGroupInUse, MessageBoxButtons.OK);
                    return false;
                }

                if (QuestionDialog.Show(Properties.Resources.AreYouSureDeleteCustomerGroup) == DialogResult.No)
                {
                    return false;
                }

                Providers.CustomerGroupData.Delete(PluginEntry.DataModel, groupID);

                return true;
            }

            return false;
        }

        public static void DeleteCustomerInGroup(List<CustomerInGroup> toRemove, RecordIdentifier groupID)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageCustomerGroups))
            {
                if (QuestionDialog.Show(Properties.Resources.AreYouSureDeleteCustomersFromGroup) == DialogResult.No)
                {
                    return;
                }

                foreach (CustomerInGroup cust in toRemove)
                {
                    RemoveCustomerFromGroup(cust.ID, groupID);
                }

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, NotifyContexts.CustomersInGroupPage, groupID, null);
            }
        }


        #endregion

        #region Customers in Group

        public static bool EditCustomersInGroup(string permission, CustomerGroupTypeEnum groupType, PriceDiscGroupEnum? priceDiscGroup, RecordIdentifier groupID)
        {
            if (PluginEntry.DataModel.HasPermission(permission))
            {
                CustomersInGroupMultiEditDialog dlg = new CustomersInGroupMultiEditDialog(groupType, priceDiscGroup, groupID);
                dlg.ShowDialog();
                return dlg.DialogResult == DialogResult.OK;
            }

            return false;
        }

        public static void AddCustomerToGroup(RecordIdentifier customerID, RecordIdentifier groupID)
        {
            CustomerInGroup group = new CustomerInGroup();
            group.ID = new RecordIdentifier(customerID, groupID);
            var customerInGroups = Providers.CustomersInGroupData.GetGroupsForCustomerList(PluginEntry.DataModel, customerID);
            group.Default = customerInGroups.Count == 0;
            Providers.CustomersInGroupData.Save(PluginEntry.DataModel, group);

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, NotifyContexts.CustomerDefaultGroupChanged, customerID, null);
        }

        public static void RemoveCustomerFromGroup(RecordIdentifier customerID, RecordIdentifier groupID)
        {
            Providers.CustomerGroupData.DeleteCustomerFromGroup(PluginEntry.DataModel, customerID, groupID);

            var customerInGroups = Providers.CustomersInGroupData.GetGroupsForCustomerList(PluginEntry.DataModel, customerID);
            if (customerInGroups.Count > 0)
            {
                bool needsNewDefault = customerInGroups.FirstOrDefault(g => g.Default) == null;
                if (needsNewDefault)
                {
                    Providers.CustomersInGroupData.SetGroupAsDefault(PluginEntry.DataModel, customerInGroups[0]);
                }
            }

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, NotifyContexts.CustomerDefaultGroupChanged, customerID, null);
        }
        #endregion
    }
}
