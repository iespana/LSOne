using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Ledger;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.DataProviders.Ledger;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.CustomerLedger.Dialogs
{
    public partial class RecreateCustomerLedgerDialog : DialogBase
    {
        private List<DataEntity> customerEntity;
        private DataEntitySelectionList customerEntityList;
        private List<DataEntity> selectedItems;
        private bool useLocalDatabase;

        public RecreateCustomerLedgerDialog(bool useLocalDatabase)
        {
            InitializeComponent();
            this.useLocalDatabase = useLocalDatabase;
            var customerList = Providers.CustomerData.GetList(PluginEntry.DataModel, "", CustomerSorting.Name, false, false);
            customerEntity = new List<DataEntity>();

            foreach (CustomerListItem customer in customerList)
            {
                customerEntity.Add(new DataEntity(customer.ID, customer.Text));
            }

            customerEntityList = new DataEntitySelectionList(customerEntity);
            cmbCustomer.SelectedData = customerEntityList;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool getError = false;
            if (QuestionDialog.Show(Properties.Resources.Mess001, Properties.Resources.Warning) == DialogResult.Yes)
            {
                int returnValue;
                string errorText;
                if (customerEntityList.HasSelection)
                {
                    errorText = Properties.Resources.Error001;
                    
                    foreach (DataEntity customerEntity in selectedItems)
                    {
                        returnValue = Providers.CustomerLedgerEntriesData.RecreateCustomerLedger(PluginEntry.DataModel, customerEntity.ID, "");
                        if (returnValue != 0)
                        {
                            getError = true;
                            errorText += customerEntity.ID + " " + customerEntity.Text + " ";
                        }
                        
                        Providers.CustomerLedgerEntriesData.UpdateRemainingAmount(PluginEntry.DataModel, customerEntity.ID);
                    }
                }
                else
                {
                    errorText = Properties.Resources.Error002;
                    returnValue = Providers.CustomerLedgerEntriesData.RecreateCustomerLedger(PluginEntry.DataModel, "", "");
                    if (returnValue != 0)
                        getError = true;
                }
                if (getError)
                    MessageDialog.Show(errorText, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
           DialogResult = DialogResult.OK;
           Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbCustomer_DropDown(object sender, DropDownEventArgs e)
        {
            e.ControlToEmbed = new CheckBoxSelectionListPanel(customerEntityList);
            DataEntitySelectionList list = ((DualDataComboBox)sender).SelectedData as DataEntitySelectionList;
            if (list != null)
            {
                e.ControlToEmbed = new CheckBoxSelectionListPanel(list);
            }
        }

        private void cmbCustomer_SelectedDataChanged(object sender, EventArgs e)
        {
            if (((DualDataComboBox) sender).SelectedData != null)
            {
                customerEntityList = (DataEntitySelectionList) ((DualDataComboBox) sender).SelectedData;

                selectedItems = customerEntityList.GetSelectedItems();
            }
        }
    }
}
