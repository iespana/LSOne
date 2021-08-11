using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ColorPalette;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Dialogs
{
    /// <summary>
    /// A dialog that allows the user to narrow the list of quotes/customer orders down by creating search criteria
    /// </summary>
    public partial class SearchCustomerOrders : TouchBaseForm
    {

        /// <summary>
        /// The search critiera creates by the user input into the dialog
        /// </summary>
        public CustomerOrderSearch SearchCriteria;

        private IConnectionManager dlgEntry;
        private IPosTransaction transaction;

        /// <summary>
        /// the constructor that initializes the dialog
        /// </summary>
        /// <param name="entry"></param>
        public SearchCustomerOrders(IConnectionManager entry)
        {
            this.dlgEntry = entry;
            InitializeComponent();

            cmbSource.Tag = ConfigurationType.Source;
            cmbDelivery.Tag = ConfigurationType.Delivery;

            SearchCriteria = new CustomerOrderSearch();
            ClearSearchCriteria();
        }

        /// <summary>
        /// A constructor that takes in a search critiera object that has already been set and displays it
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="search"></param>
        public SearchCustomerOrders(IConnectionManager entry, CustomerOrderSearch search, IPosTransaction transaction, CustomerOrderType orderType) : this (entry)
        {
            if(orderType == CustomerOrderType.Quote)
            {
                touchDialogBanner1.BannerText = Properties.Resources.SearchQuotes;
            }

            SearchCriteria = search;
            this.transaction = transaction;
            DisplaySearchCriteria();
        }

        private void SetLocation(RecordIdentifier storeID)
        {
            if (string.IsNullOrEmpty(storeID.StringValue))
            {
                cmbDeliveryLocation.SelectedData = new DataEntity("", "");
                return;
            }
            List<DataEntity> stores = Providers.StoreData.GetList(dlgEntry);
            if (stores != null)
            {
                DataEntity deliveryLocation = stores.FirstOrDefault(f => f.ID == storeID);
                if (deliveryLocation != null)
                {
                    cmbDeliveryLocation.SelectedData = deliveryLocation;
                }
            }
        }

        private void ClearSearchCriteria()
        {
            SearchCriteria.Reference = RecordIdentifier.Empty;
            tbReference.Text = "";

            SearchCriteria.CustomerID = RecordIdentifier.Empty;
            SearchCriteria.CustomerName = "";
            tbCustomerName.Text = "";

            SearchCriteria.Delivery = RecordIdentifier.Empty;
            cmbDelivery.SelectedData = new DataEntity("", "");

            SearchCriteria.Source = RecordIdentifier.Empty;
            cmbSource.SelectedData = new DataEntity("", "");

            SearchCriteria.DeliveryLocation = RecordIdentifier.Empty;
            cmbDeliveryLocation.SelectedData = new DataEntity("", "");

            SearchCriteria.Comment = "";
            tbComment.Text = "";

            SearchCriteria.Expired = false;
            chkExpired.Checked = false;
        }

        private void UpdateSearchCriteria()
        {
            //Customer info is set when the search is performed

            SearchCriteria.Reference = tbReference.Text;
            SearchCriteria.Delivery = cmbDelivery.SelectedDataID ?? RecordIdentifier.Empty;
            SearchCriteria.Source = cmbSource.SelectedDataID ?? RecordIdentifier.Empty;
            SearchCriteria.DeliveryLocation = cmbDeliveryLocation.SelectedDataID ?? RecordIdentifier.Empty;
            SearchCriteria.Comment = tbComment.Text;
            SearchCriteria.Expired = chkExpired.Checked;
        }

        private void CheckSearchCriteriaEntered(object sender, EventArgs e)
        {
            bool searchCriteriaEntered = tbReference.Text != ""
                || tbCustomerName.Text != ""
                || !RecordIdentifier.IsEmptyOrNull(cmbDelivery.SelectedDataID)
                || !RecordIdentifier.IsEmptyOrNull(cmbSource.SelectedDataID)
                || !RecordIdentifier.IsEmptyOrNull(cmbDeliveryLocation.SelectedDataID)
                || SearchCriteria.Comment != ""
                || tbComment.Text != ""
                || chkExpired.Checked;

            btnOk.Enabled = searchCriteriaEntered;
        }

        private void DisplaySearchCriteria()
        {
            tbCustomerName.Text = SearchCriteria.CustomerName;
            tbReference.Text = (string)SearchCriteria.Reference;

            List<CustomerOrderAdditionalConfigurations> configList = Providers.CustomerOrderAdditionalConfigData.GetList(dlgEntry);

            if (SearchCriteria.Delivery != null && SearchCriteria.Delivery != RecordIdentifier.Empty)
            {
                CustomerOrderAdditionalConfigurations config = configList.FirstOrDefault(f => string.Equals(((string)f.ID), ((string)SearchCriteria.Delivery), StringComparison.InvariantCultureIgnoreCase));
                if (config != null)
                {
                    cmbDelivery.SelectedData = new DataEntity(config.ID, config.Text);
                }
            }

            if (SearchCriteria.Source != null && SearchCriteria.Source != RecordIdentifier.Empty)
            {
                CustomerOrderAdditionalConfigurations config = configList.FirstOrDefault(f => string.Equals(((string)f.ID), ((string)SearchCriteria.Source), StringComparison.InvariantCultureIgnoreCase));
                if (config != null)
                {
                    cmbSource.SelectedData = new DataEntity(config.ID, config.Text);
                }
            }

            SetLocation(SearchCriteria.DeliveryLocation);
            tbComment.Text = SearchCriteria.Comment;
            chkExpired.Checked = SearchCriteria.Expired ?? false;
        }

        private void cmbDelivery_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SetData(Providers.CustomerOrderAdditionalConfigData.GetList(dlgEntry, (ConfigurationType)(int)((DualDataComboBox)sender).Tag), null);
        }

        private void cmbDeliveryLocation_RequestData(object sender, EventArgs e)
        {
            cmbDeliveryLocation.SetData(Providers.StoreData.GetList(dlgEntry), null);
        }

        private void cmbDeliveryLocation_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void BuddyControlEnter(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = (Control)sender;
            touchKeyboard.DelayedEnabled = true;
            touchKeyboard.KeystrokeMode = true;
        }

        private void BuddyControlLeave(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = null;
            touchKeyboard.DelayedEnabled = false;
        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            Customer customer = Interfaces.Services.CustomerService(dlgEntry).Search(dlgEntry, false, transaction);
            SearchCriteria.CustomerID = customer?.ID ?? RecordIdentifier.Empty;
            SearchCriteria.CustomerName = tbCustomerName.Text = customer?.GetFormattedName(dlgEntry.Settings.NameFormatter) ?? "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            UpdateSearchCriteria();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnMyLocation_Click(object sender, EventArgs e)
        {
            SetLocation(dlgEntry.CurrentStoreID);
        }

        private void tbReference_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnOk_Click(this, new EventArgs());
        }

        private void cmbSource_RequestClear(object sender, EventArgs e)
        {
            cmbSource.SelectedData = new DataEntity("", "");
        }

        private void cmbDelivery_RequestClear(object sender, EventArgs e)
        {
            cmbDelivery.SelectedData = new DataEntity("", "");
        }
    }
}
