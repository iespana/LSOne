using System.Collections.Generic;
using System.Windows.Forms.PropertyGridInternal;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Customer;

namespace LSOne.ViewPlugins.Customer.Views
{
    public partial class CustomerGroupView : ViewBase
    {
        RecordIdentifier customerGroupID;
        CustomerGroup customerGroup;
        
        public CustomerGroupView(RecordIdentifier groupID)
            : this()
        {
            customerGroupID = groupID;

            customerGroup = Providers.CustomerGroupData.Get(PluginEntry.DataModel, groupID, CacheType.CacheTypeApplicationLifeTime);
        }

        public CustomerGroupView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help;

            customerGroup = new CustomerGroup();

            this.ReadOnly = !PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageCustomerGroups);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        protected override string LogicalContextName
        {
            get { return Properties.Resources.CustomerGroup; }
        }

        public override RecordIdentifier ID
        {
	        get 
	        {
                return customerGroupID;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.General, ViewPages.CustomerGroupGeneralPage.CreateInstance));
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.DiscountedPurchases, ViewPages.CustomerGroupDiscountedPurchasesPage.CreateInstance));
                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, customerGroupID, customerGroup);
            }

            tbRetailGroup.Text = (string)customerGroup.ID;
            tbDescription.Text = customerGroup.Text;
  
            HeaderText = Properties.Resources.CustomerGroup;
            //HeaderIcon = Properties.Resources.Customer24;

            tabSheetTabs.SetData(isRevert, customerGroupID, customerGroup);
        }

        protected override bool DataIsModified()
        {
            if (tabSheetTabs.IsModified()) return true;
            if (tbRetailGroup.Text != customerGroup.ID) return true;
            if (tbDescription.Text != customerGroup.Text) return true;            

            return false;
        }

        protected override bool SaveData()
        {

            customerGroup.ID = tbRetailGroup.Text;
            customerGroup.Text = tbDescription.Text;

            tabSheetTabs.GetData();

            Providers.CustomerGroupData.Save(PluginEntry.DataModel, customerGroup);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, NotifyContexts.CustomerGroupView, customerGroup.ID, customerGroup);

            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case NotifyContexts.CustomerGroupView:
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == customerGroupID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;
            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        protected override void OnDelete()
        {

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, NotifyContexts.CustomerGroupView, customerGroupID, null);
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }
    }
}
