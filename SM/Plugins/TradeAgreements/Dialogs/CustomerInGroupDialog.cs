using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.TradeAgreements.Properties;

namespace LSOne.ViewPlugins.TradeAgreements.Dialogs
{
    public partial class CustomerInGroupDialog : DialogBase
    {
        RecordIdentifier priceGroupID;
        private List<IDataEntity> customerList;
        private PriceDiscGroupEnum group;
        private bool newGroup;

        public CustomerInGroupDialog(List<IDataEntity> customers, PriceDiscGroupEnum priceDiscountGroup)
            : this()
        {
            customerList = customers;
            group = priceDiscountGroup;
            newGroup = false;
            switch (priceDiscountGroup)
            {
                case PriceDiscGroupEnum.PriceGroup:
                    lblPriceGroup.Text = Resources.PriceGroup;
                    Header = Resources.AddCustomerToPriceGroup;

                    RecordIdentifier priceGroupID = ((CustomerListItemAdvanced) customers[0]).PriceGroupID;

                    for(int i = 1; i < customers.Count; i++)
                    {
                        if (((CustomerListItemAdvanced)customers[i]).PriceGroupID != priceGroupID)
                        {
                            priceGroupID = RecordIdentifier.Empty;
                            break;
                        }
                    }

                    if (priceGroupID != RecordIdentifier.Empty)
                    {
                        cmbGroup.SelectedData = new DataEntity(priceGroupID, ((CustomerListItemAdvanced)customers[0]).PriceGroupName);
                    }

                    break;

                case PriceDiscGroupEnum.LineDiscountGroup:
                    lblPriceGroup.Text = Resources.LineDiscountGroup;
                    Header = Resources.AddCustomerToLineDiscountGroup;

                    RecordIdentifier lineDiscountGroupID = ((CustomerListItemAdvanced)customers[0]).LineDiscountGroupID;

                    for(int i = 1; i < customers.Count; i++)
                    {
                        if (((CustomerListItemAdvanced)customers[i]).LineDiscountGroupID != lineDiscountGroupID)
                        {
                            lineDiscountGroupID = RecordIdentifier.Empty;
                            break;
                        }
                    }

                    if (lineDiscountGroupID != RecordIdentifier.Empty)
                    {
                        cmbGroup.SelectedData = new DataEntity(lineDiscountGroupID, ((CustomerListItemAdvanced)customers[0]).LineDiscountGroupName);
                    }

                    break;

                case PriceDiscGroupEnum.TotalDiscountGroup:
                    lblPriceGroup.Text = Resources.TotalDiscountGroup;
                    Header = Resources.AddCustomerToTotalDiscountGroup;

                    RecordIdentifier totalDiscountGroupID = ((CustomerListItemAdvanced)customers[0]).TotalDiscountGroupID;

                    for(int i = 1; i < customers.Count; i++)
                    {
                        if (((CustomerListItemAdvanced)customers[i]).TotalDiscountGroupID != totalDiscountGroupID)
                        {
                            totalDiscountGroupID = RecordIdentifier.Empty;
                            break;
                        }
                    }

                    if (totalDiscountGroupID != RecordIdentifier.Empty)
                    {
                        cmbGroup.SelectedData = new DataEntity(totalDiscountGroupID, ((CustomerListItemAdvanced)customers[0]).TotalDiscountGroupName);
                    }

                    break;
            }
        }

        public CustomerInGroupDialog()
        {
            InitializeComponent();

            cmbGroup.SelectedData = new DataEntity("", "");
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            priceGroupID = newGroup ? cmbGroup.SelectedData.ID[2] : cmbGroup.SelectedData.ID;
            foreach (IDataEntity customer in customerList)
            {
                if (!Providers.PriceDiscountGroupData.CustomerExistsInGroup(PluginEntry.DataModel, group, priceGroupID, customer.ID))
                {
                    Providers.PriceDiscountGroupData.AddCustomerToGroup(PluginEntry.DataModel, customer.ID, group, (string)priceGroupID);
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

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = cmbGroup.SelectedData.ID != "";
        }

        private void RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void cmbGroup_RequestData(object sender, EventArgs e)
        {
            cmbGroup.SetData(Providers.PriceDiscountGroupData.GetGroupList(
                PluginEntry.DataModel,
                PriceDiscountModuleEnum.Customer,
                group, true), null);
        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            CustomerPriceDiscDialog dlg = new CustomerPriceDiscDialog(group);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DataEntity pg = Providers.PriceDiscountGroupData.Get(PluginEntry.DataModel, dlg.ID);
                cmbGroup.SelectedData = pg;
                newGroup = true;
                CheckEnabled(this, EventArgs.Empty);
            }
        }
    }
}
