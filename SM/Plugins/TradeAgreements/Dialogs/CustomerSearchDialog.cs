using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.TradeAgreements.Dialogs
{
    public partial class CustomerSearchDialog : DialogBase
    {
        private int groupType;
        private string groupId;
        private RecordIdentifier customerAccountNumber;

        private IConnectionManager connection;
        private IApplicationCallbacks callbacks = null;

        public CustomerSearchDialog(IConnectionManager connection, IApplicationCallbacks callbacks, int groupType, string groupId)
            :this()
        {
            this.connection = connection;
            this.callbacks = callbacks;
            this.groupType = groupType;
            this.groupId = groupId;
        }

        public CustomerSearchDialog(IConnectionManager connection, IApplicationCallbacks callbacks)
            : this()
        {
            this.connection = connection;
            this.callbacks = callbacks;
        }

        public CustomerSearchDialog()
        {
            InitializeComponent();

            customerDataScroll2.PageSize = 50;
            customerDataScroll2.Reset();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Avoid the Microsoft memory leak error on ListViews
            lvItems.SmallImageList = null;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            lvItems.Columns[1].ImageIndex = 0;
            lvItems.SortColumn = 1;
            lvItems.BestFitColumns();

            Search();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return callbacks;
        }

        public RecordIdentifier GetCustomerAccountNumber()
        {
            return customerAccountNumber;
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            btnSearch.Enabled = tbSearch.Text.Length > 0;
            Search();
        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSelect.Enabled = (lvItems.SelectedItems.Count > 0);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            customerAccountNumber = (RecordIdentifier)lvItems.SelectedItems[0].Tag;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            Search();
        }

        private void Search()
        {
            lvItems.Items.Clear();

            string searchText = tbSearch.Text;
            var customers = Providers.PriceDiscountGroupData
                .SearchCustomersNotInGroup(connection, searchText, 
                    customerDataScroll2.StartRecord, customerDataScroll2.EndRecord + 1, groupType, groupId);

            customerDataScroll2.RefreshState(customers);

            foreach (var customer in customers)
            {
                var listItem = new ListViewItem((string)customer.ID);
                string formattedCustomerName = PluginEntry.DataModel.Settings.NameFormatter.Format(customer.Name);
                if(formattedCustomerName == string.Empty)
                {
                    formattedCustomerName = customer.Text;
                }
                listItem.SubItems.Add(formattedCustomerName);
                listItem.SubItems.Add(customer.GroupName);

                listItem.Tag = customer.ID;
                
                lvItems.Add(listItem);
            }

            lvItems.BestFitColumns();
        }

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (btnSearch.Enabled)
                {
                    btnSearch_Click(this, EventArgs.Empty);
                }
                e.Handled = true;
            }

            lvItems_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void lvItems_DoubleClick(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count > 0)
            {
                btnSelect_Click(this, EventArgs.Empty);
            }
        }

        private void OnPageScrollPageChanged(object sender, EventArgs e)
        {
            Search();
        }
    }
}
