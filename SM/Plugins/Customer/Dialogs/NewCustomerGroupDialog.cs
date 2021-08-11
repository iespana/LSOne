using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Customer.Dialogs
{
    public partial class NewCustomerGroupDialog : DialogBase
    {
        public CustomerGroup Group { get; set; }

        private List<CustomerGroup> groupList;

        public NewCustomerGroupDialog()
        {
            InitializeComponent();
            
            Group = new CustomerGroup();
            Header = Properties.Resources.NewCustomerGroupHeader;

            this.Text = Properties.Resources.NewCustomerGroupDlgCaption;

            groupList = Providers.CustomerGroupData.GetList(PluginEntry.DataModel);

            cmbCategories.SelectedData = new DataEntity("", "");
            cmbPurchasePeriod.SelectedIndex = 0;

            btnAddCategory.Visible = PluginEntry.DataModel.HasPermission(Permission.CategoriesEdit);
            chkLimitDiscount_CheckedChanged(null, EventArgs.Empty);
            CheckEnabled();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void cmbCategories_RequestData(object sender, EventArgs e)
        {
            List<GroupCategory> categories = Providers.GroupCategoryData.GetList(PluginEntry.DataModel);
            cmbCategories.SetData(categories, null);
        }

        private void cmbCategories_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
            CheckEnabled();
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            GroupCategory newCategory = PluginOperations.EditCategory(new GroupCategory());
            cmbCategories.SelectedData = newCategory;
            CheckEnabled();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
             CheckEnabled();
        }

        private void CheckEnabled()
        {
            btnOK.Enabled = (tbDescription.Text != "");   
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            CustomerGroup exists = groupList.FirstOrDefault(f => f.Text.ToLower() == tbDescription.Text.ToLower());
            if (exists != null)
            {
                errorProvider.SetError(tbDescription, Properties.Resources.CustomerGroupAlreadyExists);
                btnOK.Enabled = false;
                return;
            }

            Group.Text = tbDescription.Text;
            Group.Category.ID = cmbCategories.SelectedData.ID;
            Group.Exclusive = chkExclusive.Checked;
            Group.UsesDiscountedPurchaseLimit = chkLimitDiscount.Checked;
            Group.MaxDiscountedPurchases = (decimal)ntbMaxAmount.Value;
            Group.Period = (CustomerGroup.PeriodEnum) cmbPurchasePeriod.SelectedIndex;

            DataProviderFactory.Instance.Get<ICustomerGroupData, CustomerGroup>().Save(PluginEntry.DataModel, Group);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, NotifyContexts.CustomerGroupsView, Group.ID, Group);

            DialogResult = DialogResult.OK;
            Close();
            
        }

        private void chkLimitDiscount_CheckedChanged(object sender, EventArgs e)
        {
            lblMaxAmount.Enabled = lblPurchasePeriod.Enabled = ntbMaxAmount.Enabled = cmbPurchasePeriod.Enabled = chkLimitDiscount.Checked;
        }
        
    }
}
