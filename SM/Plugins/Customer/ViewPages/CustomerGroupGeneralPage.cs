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
    public partial class CustomerGroupGeneralPage : UserControl, ITabView
    {
        private CustomerGroup selectedGroup;
        
        WeakReference owner;

        public CustomerGroupGeneralPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public CustomerGroupGeneralPage()
            : base()
        {
            InitializeComponent();

            selectedGroup = new CustomerGroup();

        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new CustomerGroupGeneralPage((TabControl)sender);
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            selectedGroup = (CustomerGroup)internalContext;

            cmbCategories.SelectedData = selectedGroup.Category;
            chkExclusive.Checked = selectedGroup.Exclusive;
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case NotifyContexts.CustomerGroupView:
                    break;
            }
        }

        public bool DataIsModified()
        {
            if (chkExclusive.Checked != selectedGroup.Exclusive) return true;
            if (cmbCategories.SelectedData.ID != selectedGroup.Category.ID) return true;
            
            return false;
        }

        public bool SaveData()
        {
            selectedGroup.Exclusive = chkExclusive.Checked;
            selectedGroup.Category.ID = cmbCategories.SelectedData.ID;
            selectedGroup.Category.Text = cmbCategories.SelectedData.Text;

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, NotifyContexts.CustomerGroupView, selectedGroup.ID, selectedGroup);

            return true;
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {

        }

        private void cmbCategories_RequestData(object sender, EventArgs e)
        {
            List<GroupCategory> categories = Providers.GroupCategoryData.GetList(PluginEntry.DataModel);
            cmbCategories.SetData(categories, null);
        }

        private void cmbCategories_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            GroupCategory newCategory = PluginOperations.EditCategory(new GroupCategory());
            cmbCategories.SelectedData = newCategory;
        }
    }
}
