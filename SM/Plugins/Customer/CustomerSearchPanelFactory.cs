using System;
using System.Collections.Generic;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Localization;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Customer
{
    class CustomerSearchPanelFactory : ISearchPanelFactory
    {
        private CustomerSearchPanel searchPanel = null;
        static List<CustomerListItem> customers;
        //static List<CustomerListItemAdvanced> customersAdvanced;
        static int maxNumberOfRecords = 50;

        #region ISearchPanelProvider Members

        public void GetItemAllowedOperations(SearchListViewItem item, ref bool canEdit, ref bool canDelete, ref bool canRun)
        {
            canEdit = true;
            canDelete = PluginEntry.DataModel.HasPermission(Permission.CustomerEdit);
        }

        public void Add()
        {
            PluginOperations.NewCustomer();
        }

        public void Delete(SearchListViewItem item)
        {
            if (item == null)
                PluginOperations.DeleteCustomer(((SearchListViewItem)searchPanel.ItemList.SelectedItems[0]).ID);
            else
                PluginOperations.DeleteCustomer(item.ID);
        }

        public void Edit(SearchListViewItem item)
        {
            if (item == null)
                DoubleClickedItem((SearchListViewItem)searchPanel.ItemList.SelectedItems[0]);
            else
                DoubleClickedItem(item);
        }


        public SearchPanelBase GetPanel()
        {
            if (searchPanel == null)
            {
                searchPanel = new CustomerSearchPanel(this);
                searchPanel.ItemList.DoubleClick += new EventHandler(searchPanel_ItemDoubleClick);
            }
            return searchPanel;
        }

        public void Close()
        {
            searchPanel = null;
        }

        public string SearchContextName
        {
            get { return Properties.Resources.Customers; }
        }

        public void ReceiveChangeInformation(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (searchPanel != null)
            {
                if (objectName == "Customer")
                {
                    if (changeHint == DataEntityChangeType.Delete)
                    {
                        // We can handle this one locally.
                        foreach (SearchListViewItem item in searchPanel.ItemList.Items)
                        {
                            if (item.Key == "Customer" && item.ID == changeIdentifier)
                            {
                                searchPanel.ItemList.Items.Remove(item);
                                break;
                            }
                        }
                    }
                    else
                    {
                        searchPanel.SearchAgain();
                    }
                }
            }
        }

        public void SearchAllHandler(string text, List<SearchListViewItem> items, int maxDisplay)
        {
            
            SearchListViewItem item;

            if (text == null)
            {
                text = "";
            }

            customers = GetCustomers(text,0,maxNumberOfRecords, CustomerSorting.Name, true, false);

            foreach (CustomerListItem customer in customers)
            {
                item = new SearchListViewItem(this, (string)customer.ID, "Customer");

                item.SubItems.Add(customer.Text);
                item.ID = (string)customer.ID;


                item.ImageIndex = PluginEntry.CustomerImageIndex;


                items.Add(item);
            }
        }

        public void DoubleClickedItem(SearchListViewItem item)
        {
            PluginOperations.ShowCustomer((string)item.ID, customers.Cast<IDataEntity>());
        }

        public int ItemCount
        {
            get
            {
                return searchPanel.ItemList.Items.Count;
            }
        }

        #endregion

        void searchPanel_ItemDoubleClick(object sender, EventArgs e)
        {
            if (searchPanel.ItemList.SelectedItems.Count > 0)
            {
                DoubleClickedItem((SearchListViewItem)searchPanel.ItemList.SelectedItems[0]);
            }
        }

        internal static List<CustomerListItem> GetCustomers(string search,int recordFrom, int recordTo, CustomerSorting sortEnum , bool beginsWith, bool sortBackwards)
        {
            customers = new List<CustomerListItem>();

            // If this returns one field, then we want to search in NAME, FIRSTNAME and LASTNAME with this single string
            string[] fields = search.Split(new[] { " ", "," }, StringSplitOptions.None);
            int numberOfFields = fields.Length;

            if (numberOfFields == 1)
            {
                string searchString = fields[0];
                customers = Providers.CustomerData.Search(PluginEntry.DataModel, searchString, recordFrom, recordTo, beginsWith, sortEnum, sortBackwards);
            }
            else
            {
                Name name = NameParser.ParseName(search, PluginEntry.DataModel.Settings.NameFormat == NameFormat.LastNameFirst);

                string displayName = PluginEntry.DataModel.Settings.NameFormatter.Format(name);

                customers = Providers.CustomerData.Search(PluginEntry.DataModel, displayName, name.First, name.Last, recordFrom, recordTo, beginsWith, sortEnum, sortBackwards);
            }

            return customers;
        }

        internal static List<CustomerListItem> GetAllExceptCurrentCust(string search, CustomerSorting sortEnum, bool beginsWith, bool sortBackwards, RecordIdentifier id)
        {
            customers = new List<CustomerListItem>();
         
            Name name = NameParser.ParseName(search, PluginEntry.DataModel.Settings.NameFormat == NameFormat.LastNameFirst);

            string displayName = PluginEntry.DataModel.Settings.NameFormatter.Format(name);

            customers = Providers.CustomerData.GetList(PluginEntry.DataModel, displayName, CustomerSorting.ID, false, false, id );
            

            return customers;
        }
    }
}
