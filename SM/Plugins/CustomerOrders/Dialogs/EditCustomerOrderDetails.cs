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
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;

namespace LSOne.ViewPlugins.CustomerOrders.Dialogs
{
    public partial class EditCustomerOrderDetails : DialogBase
    {
        public List<CustomerOrder> toEdit;

        private IConnectionManager dlgEntry;
        private ISettings settings;
        private CustomerOrderSettings orderSettings;
        private List<CustomerOrderAdditionalConfigurations> configList;
        private List<DataEntity> stores;
        private int noOfOrders;
        private bool initializingFields;

        private CustomerOrderType orderType;

        

        public EditCustomerOrderDetails(IConnectionManager entry)
        {
            InitializeComponent();

            dlgEntry = entry;
            settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            orderSettings = Providers.CustomerOrderSettingsData.Get(dlgEntry, CustomerOrderType.CustomerOrder);
            lblNoOfOrders.Text = "";
            toEdit = new List<CustomerOrder>();
            noOfOrders = -1;
            initializingFields = false;

            configList = Providers.CustomerOrderAdditionalConfigData.GetList(dlgEntry);
            stores = Providers.StoreData.GetList(dlgEntry);

            btnOK.Enabled = false;
        }

        public EditCustomerOrderDetails(IConnectionManager entry, CustomerOrder toEdit) : this(entry)
        {
            this.toEdit.Add(toEdit);
            orderType = toEdit.OrderType;
            noOfOrders = 1;
            InitializeDialog();
        }

        public EditCustomerOrderDetails(IConnectionManager entry, List<CustomerOrder> toEdit) : this(entry)
        {
            this.toEdit = toEdit;
            noOfOrders = toEdit.Count;
            CustomerOrder order = toEdit.FirstOrDefault();
            orderType = order != null ? order.OrderType : CustomerOrderType.CustomerOrder;
            
            InitializeDialog();
        }

        private void InitializeDialog()
        {
            try
            {
                this.Header = orderType == CustomerOrderType.CustomerOrder ? Properties.Resources.EditCustomerOrderDetails : Properties.Resources.EditQuotesDetails;
                this.Text = this.Header;

                initializingFields = true;

                if (noOfOrders == -1)
                {
                    noOfOrders = toEdit.Count;
                }

                if (noOfOrders == 0)
                {
                    return;
                }

                cmbSource.Enabled = orderSettings.SelectSource;
                cmbSource.Tag = ConfigurationType.Source;

                cmbDelivery.Enabled = orderSettings.SelectDelivery;
                cmbDelivery.Tag = ConfigurationType.Delivery;

                dtExpiryDate.Enabled = (orderSettings.ExpirationTimeUnit != TimeUnitEnum.None);

                if (noOfOrders == 1)
                {
                    CustomerOrder order = toEdit.FirstOrDefault();
                    tbReference.Text = (string) order.Reference;

                    if (order.Source != null && order.Source != RecordIdentifier.Empty)
                    {
                        CustomerOrderAdditionalConfigurations config = configList.FirstOrDefault(f => string.Equals(((string) f.ID), ((string) order.Source), StringComparison.InvariantCultureIgnoreCase));
                        if (config != null)
                        {
                            cmbSource.SelectedData = new DataEntity(config.ID, config.Text);
                        }

                    }

                    if (order.Delivery != null && order.Delivery != RecordIdentifier.Empty)
                    {
                        CustomerOrderAdditionalConfigurations config = configList.FirstOrDefault(f => string.Equals(((string) f.ID), ((string) order.Delivery), StringComparison.InvariantCultureIgnoreCase));
                        if (config != null)
                        {
                            cmbDelivery.SelectedData = new DataEntity(config.ID, config.Text);
                        }
                    }

                    DataEntity deliveryLocation = stores.FirstOrDefault(f => f.ID == order.DeliveryLocation);
                    if (deliveryLocation != null)
                    {
                        cmbDeliveryLocation.SelectedData = deliveryLocation;
                    }
                    
                    dtExpiryDate.Value = order.ExpiryDate != Date.Empty ? order.ExpiryDate.DateTime : DateTime.Now;

                    tbComment.Text = order.Comment;

                    tbShippingAddress.Text = "";
                    if (order.CustomerID != RecordIdentifier.Empty)
                    {
                        Customer customer = Providers.CustomerData.Get(dlgEntry, order.CustomerID, UsageIntentEnum.Normal, CacheType.CacheTypeApplicationLifeTime);

                        if (customer != null)
                        {
                            cmbCustomer.SelectedData = new DataEntity(customer.ID, customer.GetFormattedName(dlgEntry.Settings.NameFormatter));
                            tbShippingAddress.Text = dlgEntry.Settings.SystemAddressFormatter.FormatSingleLine(customer.DefaultShippingAddress, dlgEntry.Cache);
                        }
                    }
                }
                else
                {
                    lblNoOfOrders.Text = Properties.Resources.Editing.Replace("#1", Conversion.ToStr(noOfOrders));

                    tbReference.Text = "";

                    CustomerOrder toCheck = toEdit.FirstOrDefault();

                    if (toEdit.Count(c => string.Equals(((string) c.Source), ((string) toCheck.Source), StringComparison.InvariantCultureIgnoreCase)) == noOfOrders)
                    {
                        if (toCheck.Source != null && toCheck.Source != RecordIdentifier.Empty)
                        {
                            CustomerOrderAdditionalConfigurations config = configList.FirstOrDefault(f => string.Equals(((string) f.ID), ((string) toCheck.Source), StringComparison.InvariantCultureIgnoreCase));
                            if (config != null)
                            {
                                cmbSource.SelectedData = new DataEntity(config.ID, config.Text);
                            }

                        }
                    }

                    if (toEdit.Count(c => string.Equals(((string) c.Delivery), ((string) toCheck.Delivery), StringComparison.InvariantCultureIgnoreCase)) == noOfOrders)
                    {
                        if (toCheck.Delivery != null && toCheck.Delivery != RecordIdentifier.Empty)
                        {
                            CustomerOrderAdditionalConfigurations config = configList.FirstOrDefault(f => string.Equals(((string) f.ID), ((string) toCheck.Delivery), StringComparison.InvariantCultureIgnoreCase));
                            if (config != null)
                            {
                                cmbDelivery.SelectedData = new DataEntity(config.ID, config.Text);
                            }
                        }
                    }

                    if (toEdit.Count(c => string.Equals(((string) c.DeliveryLocation), ((string) toCheck.DeliveryLocation), StringComparison.InvariantCultureIgnoreCase)) == noOfOrders)
                    {
                        DataEntity deliveryLocation = stores.FirstOrDefault(f => f.ID == toCheck.DeliveryLocation);
                        if (deliveryLocation != null)
                        {
                            cmbDeliveryLocation.SelectedData = deliveryLocation;
                        }
                    }

                    if (toEdit.Count(c => c.ExpiryDate.DateTime.Date == toCheck.ExpiryDate.DateTime.Date) == noOfOrders)
                    {
                        dtExpiryDate.Value = toCheck.ExpiryDate != Date.Empty ? toCheck.ExpiryDate.DateTime : DateTime.Now;
                    }
                    else
                    {
                        dtExpiryDate.Value = orderSettings.ExpirationDate();
                    }

                    if (toEdit.Count(c => string.Equals(c.Comment, toCheck.Comment, StringComparison.InvariantCultureIgnoreCase)) == noOfOrders)
                    {
                        tbComment.Text = toCheck.Comment;
                    }
                    else
                    {
                        tbComment.Text = "";
                    }

                    if (toEdit.Count(c => c.CustomerID == toCheck.CustomerID) == noOfOrders)
                    {
                        tbShippingAddress.Text = "";
                        if (toCheck.CustomerID != RecordIdentifier.Empty)
                        {
                            Customer customer = Providers.CustomerData.Get(dlgEntry, toCheck.CustomerID, UsageIntentEnum.Normal, CacheType.CacheTypeApplicationLifeTime);

                            if (customer != null)
                            {
                                cmbCustomer.SelectedData = new DataEntity(customer.ID, customer.GetFormattedName(dlgEntry.Settings.NameFormatter));
                                tbShippingAddress.Text = dlgEntry.Settings.SystemAddressFormatter.FormatSingleLine(customer.DefaultShippingAddress, dlgEntry.Cache);
                            }
                        }
                    }
                }
            }
            finally
            {
                initializingFields = false;
            }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = false;

            if (initializingFields)
            {
                return;
            }

            if (noOfOrders == 1)
            {
                CustomerOrder toCheck = toEdit.FirstOrDefault();
                if (toCheck == null)
                {
                    btnOK.Enabled = false;
                    return;
                }

                if (tbReference.Text != toCheck.Reference)
                {
                    btnOK.Enabled = true;
                    return;
                }

                if (cmbDelivery.Enabled && cmbDelivery.SelectedDataID != toCheck.Delivery)
                {
                    btnOK.Enabled = true;
                    return;
                }

                if (cmbSource.Enabled && cmbSource.SelectedDataID != toCheck.Source)
                {
                    btnOK.Enabled = true;
                    return;
                }

                if (cmbDeliveryLocation.SelectedDataID != toCheck.DeliveryLocation)
                {
                    btnOK.Enabled = true;
                    return;
                }

                if (dtExpiryDate.Checked && dtExpiryDate.Value != toCheck.ExpiryDate.DateTime)
                {
                    btnOK.Enabled = true;
                    return;
                }

                if (tbComment.Text != toCheck.Comment)
                {
                    btnOK.Enabled = true;
                    return;
                }

                if (cmbCustomer.SelectedDataID != toCheck.CustomerID)
                {
                    btnOK.Enabled = true;
                    return;
                }

            }
            else
            {
                btnOK.Enabled = true;
            }
        }

        private void cmbDelivery_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity();
        }

        private void Configurations_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SetData(Providers.CustomerOrderAdditionalConfigData.GetList(dlgEntry, (ConfigurationType)(int)((DualDataComboBox)sender).Tag), null);
        }

        private void cmbDeliveryLocation_RequestData(object sender, EventArgs e)
        {
            cmbDeliveryLocation.SetData(Providers.StoreData.GetList(dlgEntry), null);
        }

        private void cmbCustomer_DropDown(object sender, DropDownEventArgs e)
        {
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, e.DisplayText, SearchTypeEnum.Customers);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (noOfOrders == 1)
            {
                CustomerOrder order = toEdit.FirstOrDefault();
                order.Reference = tbReference.Text;
                order.Delivery = cmbDelivery.SelectedDataID;
                order.Source = cmbSource.SelectedDataID;
                order.DeliveryLocation = cmbDeliveryLocation.SelectedDataID;
                order.ExpiryDate = dtExpiryDate.Checked ? new Date(dtExpiryDate.Value) : order.ExpiryDate;
                order.Comment = tbComment.Text;
                order.CustomerID = cmbCustomer.SelectedDataID;
            }
            else if (noOfOrders > 1)
            {
                foreach (CustomerOrder order in toEdit)
                {
                    if (!string.IsNullOrWhiteSpace(tbReference.Text))
                    {
                        order.Reference = tbReference.Text;
                    }

                    if (cmbDelivery.SelectedDataID != null || cmbDelivery.SelectedDataID != RecordIdentifier.Empty)
                    {
                        order.Delivery = cmbDelivery.SelectedDataID;
                    }

                    if (cmbSource.SelectedDataID != null || cmbSource.SelectedDataID != RecordIdentifier.Empty)
                    {
                        order.Source = cmbSource.SelectedDataID;
                    }

                    if (cmbDeliveryLocation.SelectedDataID != null || cmbDeliveryLocation.SelectedDataID != RecordIdentifier.Empty)
                    {
                        order.DeliveryLocation = cmbDeliveryLocation.SelectedDataID;
                    }

                    if (dtExpiryDate.Checked)
                    {
                        order.ExpiryDate = new Date(dtExpiryDate.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(tbComment.Text))
                    {
                        order.Comment = tbComment.Text;
                    }

                    if (cmbCustomer.SelectedDataID != null || cmbCustomer.SelectedDataID != RecordIdentifier.Empty)
                    {
                        order.CustomerID = cmbCustomer.SelectedDataID;
                    }
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void EditCustomerOrderDetails_Shown(object sender, EventArgs e)
        {
            dtExpiryDate.Checked = false;
        }
    }
}
