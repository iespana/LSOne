using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Customer;
using LSOne.ViewPlugins.Customer.Properties;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.ViewPlugins.Customer.Dialogs
{
    public partial class CustomerBlockingDialog : DialogBase
    {
        private BlockedEnum setBlocking;
        private List<CustomerListItemAdvanced> customerList;
        private DataLayer.BusinessObjects.Customers.Customer singleCustomer;
        private int lastSelectedBlocking;

        public CustomerBlockingDialog(List<CustomerListItemAdvanced> customers)
            :this()
        {
            customerList = customers;
            if (customerList.Count == 1)
            {
                singleCustomer = Providers.CustomerData.Get(PluginEntry.DataModel, customerList[0].ID, UsageIntentEnum.Normal);
                cmbBlocking.SelectedIndex = (int)singleCustomer.Blocked;
                Header = Resources.SetBlockingStatusForCustomer;
            }

            lastSelectedBlocking = cmbBlocking.SelectedIndex;
        }

        public CustomerBlockingDialog()
        {
            InitializeComponent();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            setBlocking = (BlockedEnum) cmbBlocking.SelectedIndex;
            foreach (CustomerListItemAdvanced customer in customerList)
            {
                if (customer.Blocked != setBlocking)
                {
                    Providers.CustomerData.SaveBlockStatus(PluginEntry.DataModel, customer.ID, setBlocking);
                }
            }
            
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Customer", RecordIdentifier.Empty, null);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbBlocking_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = lastSelectedBlocking != cmbBlocking.SelectedIndex;
        }
    }
}
