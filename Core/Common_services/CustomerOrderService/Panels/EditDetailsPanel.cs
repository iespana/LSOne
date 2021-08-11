using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Controls.SupportClasses;
using LSOne.Services.Dialogs;
using LSOne.Peripherals;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.GenericConnector.Enums;

namespace LSOne.Services.Panels
{
    public partial class EditDetailsPanel : UserControl
    {
        private IConnectionManager dlgEntry;
        private ISettings settings;
        private CustomerOrderSettings orderSettings;
        private IRetailTransaction retailTransaction;
        private CustomerOrderItem customerOrder;
        private bool newCustomerOrder;

        private bool customerValid;
        private bool doEvents;

        private enum Buttons
        {
            Next,
            Save,
            Cancel
        }

        /// <summary>
        /// The customer order / quote information as it is now in the dialog
        /// </summary>
        public CustomerOrderItem CustomerOrder
        {
            get { return UpdateCustomerOrder(); }

            set { customerOrder = value; }
        }

        /// <summary>
        /// Constructor that initializes the edit details dialog with informatoin about the customer order
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="newCustomerOrder">If true the customer order is being created</param>
        public EditDetailsPanel(IConnectionManager entry, IRetailTransaction retailTransaction, bool newCustomerOrder)
        {
            InitializeComponent();

            this.newCustomerOrder = newCustomerOrder;
            dlgEntry = entry;
            settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            orderSettings = Providers.CustomerOrderSettingsData.Get(dlgEntry, retailTransaction.CustomerOrder.OrderType);
            this.retailTransaction = retailTransaction;

            customerOrder = retailTransaction.CustomerOrder;

            InitializeButtons();
            SetCustomerOrderDetails();

            if (!DesignMode)
            {
                tbCustomerName.StartCharacter = settings.HardwareProfile.StartTrack1;
                tbCustomerName.EndCharacter = settings.HardwareProfile.EndTrack1;
                tbCustomerName.Seperator = settings.HardwareProfile.Separator1;
                tbCustomerName.TrackSeperation = TrackSeperation.Before;
            }

            doEvents = true;
        }

        /// <summary>
        /// Displays the customer order details in the input controls.
        /// </summary>
        public void SetCustomerOrderDetails()
        {

            #region Customer name

            SetCustomer(retailTransaction.Customer);
            
            #endregion

            #region Delivery and source
            
            lblType.Enabled = orderSettings.SelectSource;
            cmbType.Enabled = orderSettings.SelectSource;
            cmbType.Tag = ConfigurationType.Source;

            lblDelivery.Enabled = orderSettings.SelectDelivery;
            cmbDelivery.Enabled = orderSettings.SelectDelivery;
            cmbDelivery.Tag = ConfigurationType.Delivery;

            List<CustomerOrderAdditionalConfigurations> configList = Providers.CustomerOrderAdditionalConfigData.GetList(dlgEntry);

            if (customerOrder.Source != null && customerOrder.Source.ID != RecordIdentifier.Empty)
            {
                CustomerOrderAdditionalConfigurations config = configList.FirstOrDefault(f => string.Equals(((string)f.ID), ((string)customerOrder.Source.ID), StringComparison.InvariantCultureIgnoreCase));
                if (config != null)
                {
                    cmbType.SelectedData = new DataEntity(config.ID, config.Text);
                }

            }

            if (customerOrder.Delivery != null && customerOrder.Delivery.ID != RecordIdentifier.Empty)
            {
                CustomerOrderAdditionalConfigurations config = configList.FirstOrDefault(f => string.Equals(((string)f.ID), ((string)customerOrder.Delivery.ID), StringComparison.InvariantCultureIgnoreCase));
                if (config != null)
                {
                    cmbDelivery.SelectedData = new DataEntity(config.ID, config.Text);
                }
            }

            #endregion

            #region Deposit

            //Not displayed in edit details panel - is set in UpdateCustomerOrder function

            #endregion

            #region Expiration

            if (newCustomerOrder)
            {
                dtExpires.Enabled = (orderSettings.ExpirationTimeUnit != TimeUnitEnum.None);
                dtExpires.Value = dtExpires.Enabled ? orderSettings.ExpirationDate() : DateTime.Now;
            }
            else
            {
                dtExpires.Value = customerOrder.ExpirationDate.DateTime;
            }

            #endregion

            #region Reference

            if (RecordIdentifier.IsEmptyOrNull(customerOrder.Reference))
            {
                tbReference.Text = (string)Interfaces.Services.SiteServiceService(dlgEntry).CreateReferenceNo(dlgEntry, settings.SiteServiceProfile, orderSettings.SettingsType);
            }
            else
            {
                tbReference.Text = (string)customerOrder.Reference;
            }

            #endregion

            #region Pickup

            SetLocation(customerOrder.DeliveryLocation);

            #endregion

            #region Comment

            tbComment.Text = customerOrder.Comment;

            #endregion

            #region Update stock

            chkUpdateStock.Visible = customerOrder.OrderType == CustomerOrderType.CustomerOrder;

            if (customerOrder.OrderType == CustomerOrderType.CustomerOrder)
            {
                chkUpdateStock.Checked = customerOrder.UpdateStock;
                chkUpdateStock.Enabled = newCustomerOrder || customerOrder.ConvertedFrom == CustomerOrderType.Quote;
            }

            #endregion

        }

        private void Configurations_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SetData(Providers.CustomerOrderAdditionalConfigData.GetList(dlgEntry, (ConfigurationType)(int)((DualDataComboBox)sender).Tag), null);
        }

        private void cmbPickUp_RequestData(object sender, EventArgs e)
        {
            cmbDeliveryLocation.SetData(Providers.StoreData.GetList(dlgEntry), null);
        }

        private void cmbPickUp_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        /// <summary>
        /// Displays information about the customer
        /// </summary>
        /// <param name="customer">The customer to be displayed</param>
        public void SetCustomer(Customer customer)
        {
            doEvents = false;
            customerValid = false;

            tbCustomerName.Text = "";
            tbAddress.Text = "";

            if (customer != null && customer.ID != RecordIdentifier.Empty)
            {
                customerValid = true;
                tbCustomerName.Text = customer.GetFormattedName(dlgEntry.Settings.NameFormatter);
                tbAddress.Text = dlgEntry.Settings.SystemAddressFormatter.FormatMultipleLines(customer.DefaultShippingAddress, dlgEntry.Cache, "\r\n");

                if(retailTransaction.Customer.ID != customer.ID)
                {
                    Services.Interfaces.Services.CustomerService(dlgEntry).AddCustomerToTransaction(dlgEntry, customer.ID, retailTransaction, false);
                }
            }

            doEvents = true;
        }

        /// <summary>
        /// Displays the name of the store that has been selected for the customer order / quote
        /// </summary>
        /// <param name="storeID">The ID of the store</param>
        public void SetLocation(RecordIdentifier storeID)
        {
            if (storeID == RecordIdentifier.Empty)
            {
                return;
            }

            List<DataEntity> stores = Providers.StoreData.GetList(dlgEntry);
            if (stores != null)
            {
                DataEntity deliveryLocation = stores.FirstOrDefault(f => f.ID.StringValue == storeID.StringValue);
                if (deliveryLocation != null)
                {
                    cmbDeliveryLocation.SelectedData = deliveryLocation;
                    cmbDeliveryLocation.Enabled = retailTransaction.SaleItems.FirstOrDefault(f => !f.Voided && f.Order.ReservationDone) == null;
                }
            }
        }

        private CustomerOrderItem UpdateCustomerOrder()
        {
            CustomerOrderItem order = (CustomerOrderItem) customerOrder.Clone();
            
            order.Reference = tbReference.Text;

            if (orderSettings.AcceptsDeposits)
            {
                order.MinimumDeposit = retailTransaction.TransSalePmtDiff * (orderSettings.MinimumDeposits / 100);
            }

            #region Delivery and source
            List<CustomerOrderAdditionalConfigurations> configList = Providers.CustomerOrderAdditionalConfigData.GetList(dlgEntry);
            CustomerOrderAdditionalConfigurations config = configList.FirstOrDefault(f => f.ID == cmbType.SelectedDataID);
            if (config != null)
            {
                order.Source = config;
            }

            config = configList.FirstOrDefault(f => f.ID == cmbDelivery.SelectedDataID);
            if (config != null)
            {
                order.Delivery = config;
            }

            #endregion

            order.DeliveryLocation = cmbDeliveryLocation.SelectedDataID ?? RecordIdentifier.Empty;
            order.ExpirationDate = new Date(dtExpires.Value);
            order.Comment = tbComment.Text;
            order.UpdateStock = chkUpdateStock.Checked;
            return order;
        }

        private void BuddyControlEnter(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = (Control) sender;
            touchKeyboard.DelayedEnabled = true;
            touchKeyboard.KeystrokeMode = true;
        }

        private void BuddyControlLeave(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = null;
            touchKeyboard.DelayedEnabled = false;
        }

        private void EditDetailsPanel_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                MSR.MSRMessageEvent += MSR_MSRMessageEvent;
                MSR.EnableForSwipe();

                Scanner.ScannerMessageEvent += ProcessScannedItem;
                Scanner.ReEnableForScan();
            }

            tbReference.Focus();
            BuddyControlEnter(tbReference, new EventArgs());
        }

        private void ProcessScannedItem(ScanInfo scanInfo)
        {
            try
            {
                BarCode barCode = Services.Interfaces.Services.BarcodeService(dlgEntry).ProcessBarcode(dlgEntry, scanInfo);
                SetCustomer(FindCustomer(barCode.CustomerId, scanInfo));
            }
            finally
            {
                Scanner.ReEnableForScan();
            }
        }

        private void MSR_MSRMessageEvent(string track2)
        {
            tbCustomerName.Text = StringExtensions.TrackBeforeSeparator(track2, settings.HardwareProfile.StartTrack1, settings.HardwareProfile.Separator1, settings.HardwareProfile.EndTrack1);
            btnSearchCustomer_Click(null, EventArgs.Empty);
        }

        private void InitializeButtons()
        {
            panelButtons.Clear();
            panelButtons.AddButton(Properties.Resources.Save, Buttons.Save, "", TouchButtonType.Normal, DockEnum.DockEnd);
            panelButtons.AddButton(Properties.Resources.Next, Buttons.Next, "", TouchButtonType.OK, DockEnum.DockEnd);
            panelButtons.AddButton(Properties.Resources.Cancel, Buttons.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);
        }

        private void panelButtons_Click(object sender, ScrollButtonEventArguments args)
        {
            switch ((Buttons)(int)args.Tag)
            {
                case Buttons.Cancel:
                    GetParent().ClickCancel();
                    break;
                case Buttons.Save:
                    GetParent().ClickSave();
                    break;
                case Buttons.Next:
                    GetParent().ClickNext();
                    break;
            }
        }

        private CustomerOrderDetails GetParent()
        {
            return this.Parent as CustomerOrderDetails;
        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            if (tbCustomerName.Text.Trim() == "")
            {
                Interfaces.Services.CustomerService(dlgEntry).Search(dlgEntry, retailTransaction, false);
                SetCustomer(retailTransaction.Customer);
                return;
            }

            SetCustomer(FindCustomer(tbCustomerName.Text, new ScanInfo(tbCustomerName.Text)));
        }

        private void tbCustomerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbCustomerName.Text = StringExtensions.TrackBeforeSeparator(tbCustomerName.Text, settings.HardwareProfile.StartTrack1, settings.HardwareProfile.Separator1, settings.HardwareProfile.EndTrack1);
                btnSearchCustomer_Click(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Searching for a customer both by Customer ID and by a full barcode if a scanner was used
        /// </summary>
        /// <param name="customerID">The customer ID entered into the text box</param>
        /// <param name="scanInfo">The scan info from the barcode</param>
        /// <returns></returns>
        private Customer FindCustomer(RecordIdentifier customerID, ScanInfo scanInfo)
        {
            Customer customer = Providers.CustomerData.Get(dlgEntry, customerID, UsageIntentEnum.Normal, CacheType.CacheTypeTransactionLifeTime);

            if (customer == null && scanInfo != null)
            {
                customer = Providers.CustomerData.GetByCardNumber(dlgEntry, scanInfo.ScanDataLabel, UsageIntentEnum.Normal, CacheType.CacheTypeTransactionLifeTime);
            }

            //Search by name
            if (customer == null)
            {
                List<CustomerListItem> customers = Providers.CustomerData.Search(dlgEntry, tbCustomerName.Text, 0, 2, false, DataLayer.DataProviders.Customers.CustomerSorting.ID, false);

                if (customers.Count > 0)
                {
                    if (customers.Count == 1)
                    {
                        customer = Providers.CustomerData.Get(dlgEntry, customers[0].ID, UsageIntentEnum.Normal, CacheType.CacheTypeTransactionLifeTime);
                    }
                    else
                    {
                        Interfaces.Services.CustomerService(dlgEntry).Search(dlgEntry, retailTransaction, false, customerID.StringValue);
                        customer = retailTransaction.Customer;
                    }
                }
            }

            return customer;
        }

        private void tbCustomerName_TextChanged(object sender, EventArgs e)
        {
            if (customerValid && doEvents)
            {
                customerValid = false;
                IPosTransaction tempTransaction = retailTransaction;
                Interfaces.Services.TransactionService(dlgEntry).ClearCustomerFromTransaction(dlgEntry, ref tempTransaction);
                retailTransaction = (IRetailTransaction)tempTransaction;
                tbAddress.Text = "";
            }
        }

        public void DisableScanner()
        {
            if(!DesignMode)
            {
                Scanner.ScannerMessageEvent -= ProcessScannedItem;
                Scanner.DisableForScan();
                MSR.MSRMessageEvent -= MSR_MSRMessageEvent;
                MSR.DisableForSwipe();
            }
        }
    }
}
