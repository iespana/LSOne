using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.DataProviders.ProviderConfig;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Customer.ViewPages
{
    public partial class CustomerGroupDiscountedPurchasesPage : UserControl, ITabView
    {
        private CustomerGroup selectedGroup;
        
        WeakReference owner;

        public CustomerGroupDiscountedPurchasesPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public CustomerGroupDiscountedPurchasesPage()
            : base()
        {
            InitializeComponent();

            selectedGroup = new CustomerGroup();

        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new CustomerGroupDiscountedPurchasesPage((TabControl)sender);
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            selectedGroup = (CustomerGroup)internalContext;

            chkLimitDiscount.Checked = selectedGroup.UsesDiscountedPurchaseLimit;
            ntbMaxAmount.Value = (double)selectedGroup.MaxDiscountedPurchases;
            cmbPurchasePeriod.SelectedIndex = (int) selectedGroup.Period;
            chkLimitDiscount_CheckedChanged(null, EventArgs.Empty);
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
        }

        public bool DataIsModified()
        {
            if (chkLimitDiscount.Checked != selectedGroup.UsesDiscountedPurchaseLimit) return true;
            if ((decimal)ntbMaxAmount.Value != selectedGroup.MaxDiscountedPurchases) return true;
            if (cmbPurchasePeriod.SelectedIndex != (int) selectedGroup.Period) return true;

            return false;
        }

        public bool SaveData()
        {
            selectedGroup.UsesDiscountedPurchaseLimit = chkLimitDiscount.Checked;
            selectedGroup.MaxDiscountedPurchases = (decimal) ntbMaxAmount.Value;
            selectedGroup.Period = (CustomerGroup.PeriodEnum) cmbPurchasePeriod.SelectedIndex;

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, NotifyContexts.CustomerGroupView, selectedGroup.ID, selectedGroup);

            return true;
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {

        }

        private void chkLimitDiscount_CheckedChanged(object sender, EventArgs e)
        {
            cmbPurchasePeriod.Enabled = lblPurchasePeriod.Enabled
                = lblMaxAmount.Enabled = ntbMaxAmount.Enabled = chkLimitDiscount.Checked;
        }
    }
}
