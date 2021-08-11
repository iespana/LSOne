//using LSRetail.Utilities.String;
//using LSRetail.StoreController.Common.Settings;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.Utilities.Tools;
using LSOne.ViewCore;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Customer
{
    public partial class CustomerSearchPanel : SearchPanelBase
    {
        string lastSearch = "";
        int maxNumberOfRecords = 50;
        ISearchPanelFactory provider;

        public CustomerSearchPanel(ISearchPanelFactory provider)
            : this()
        {
            this.provider = provider;
        }


        public CustomerSearchPanel()
        {
            InitializeComponent();

            lvCustomerSearchResults.SmallImageList = PluginEntry.Framework.GetImageList();

            lvCustomerSearchResults.ContextMenuStrip = new ContextMenuStrip();
            lvCustomerSearchResults.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
            lvCustomerSearchResults.ContextMenuStrip.Closed += new ToolStripDropDownClosedEventHandler(ContextMenuStrip_Closed);

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        void ContextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            PluginEntry.Framework.ResumeSearchBarClosing();
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu;

            menu = lvCustomerSearchResults.ContextMenuStrip;

            menu.Items.Clear();
            
            /* We can optionally add our own items right here but we do not do it
             * because there are other places where we are displaying almost the same menu*/

            PluginEntry.Framework.SuspendSearchBarClosing();
            PluginEntry.Framework.ContextMenuNotify("CustomerSearchList", lvCustomerSearchResults.ContextMenuStrip, lvCustomerSearchResults);

            e.Cancel = (menu.Items.Count == 0);

            if (e.Cancel)
            {
                PluginEntry.Framework.ResumeSearchBarClosing();
            }
        }

       

     

        private void lvUserSearchResults_Resize(object sender, EventArgs e)
        {
            lvCustomerSearchResults.SuspendLayout();
            lvCustomerSearchResults.BeginUpdate();

            // This seems very stupid but it solves a .NET Listview layout bug when going back from maximized
            lvCustomerSearchResults.Columns[lvCustomerSearchResults.Columns.Count - 1].Width = 10;
            lvCustomerSearchResults.Columns[lvCustomerSearchResults.Columns.Count - 1].Width = -2;
            //lvCustomerSearchResults.Columns[lvCustomerSearchResults.Columns.Count - 1].Width = lvCustomerSearchResults.Columns[lvCustomerSearchResults.Columns.Count - 1].Width - 20;

            lvCustomerSearchResults.EndUpdate();
            lvCustomerSearchResults.ResumeLayout();
        }

        public override void Search(string searchText)
        {
            List<CustomerListItem> customers;
            SearchListViewItem item;

            if (searchText == null)
            {
                searchText = "";
            }
            lastSearch = searchText;

            customers = CustomerSearchPanelFactory.GetCustomers(searchText, 0, maxNumberOfRecords, CustomerSorting.Name, true, false);

            
            lvCustomerSearchResults.Items.Clear();

            foreach (CustomerListItem customer in customers)
            {
                item = new SearchListViewItem(provider, (string)customer.ID, "Customer");

                item.SubItems.Add(customer.Text);
                item.SubItems.Add(customer.AccountNumber);
                item.ID = customer.ID;

                item.ImageIndex = PluginEntry.CustomerImageIndex;

                lvCustomerSearchResults.Add(item);
            }

            OnSelectionChanged(false, false, false);
            
        }

        public override void Search(ISearchPanelFactory searchFactory, SearchParameter[] parameters, int maxDisplay)
        {
            List<CustomerListItem> customers;
            SearchListViewItem item;

            lastSearch = "";

            customers = Providers.CustomerData.Search(PluginEntry.DataModel, parameters, 0);

            lvCustomerSearchResults.Items.Clear();

            foreach (CustomerListItem customer in customers)
            {
                item = new SearchListViewItem(provider, (string)customer.ID, "Customer");

                item.SubItems.Add(customer.Text);
                item.SubItems.Add(customer.AccountNumber);
                item.ID = customer.ID;

                item.ImageIndex = PluginEntry.CustomerImageIndex;

                lvCustomerSearchResults.Add(item);
            }

            OnSelectionChanged(false, false,false);
        }

        public void SearchAgain()
        {
            Search(lastSearch);
        }


        

        public ListView ItemList
        {
            get
            {
                return lvCustomerSearchResults;
            }
        }

        public override bool CanAdd
        {
            get
            {
                return PluginEntry.DataModel.HasPermission(Permission.CustomerEdit);
            }
        }

        private void lvCustomerSearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool hasSelection = lvCustomerSearchResults.SelectedItems.Count > 0;

            OnSelectionChanged(hasSelection, hasSelection && PluginEntry.DataModel.HasPermission(Permission.CustomerEdit),false);
        }
    }
}
