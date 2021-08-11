using System;
using System.Collections.Generic;
using System.Linq;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.Customer.Views
{
    public partial class CustomerView : ViewBase
    {
        RecordIdentifier customerID;
        LSOne.DataLayer.BusinessObjects.Customers.Customer customer;

        TabControl.Tab lastOpenedTab;

        TabControl.Tab detailTab;
        TabControl.Tab contactTab;
        TabControl.Tab addressTab;
        TabControl.Tab discountsTab;
        TabControl.Tab groupTab;
        int selectedTabIndex = -1;
        bool contextBarInitialized;
        bool initialized;

        IEnumerable<IDataEntity> recordBrowsingContext;

        public CustomerView(RecordIdentifier customerID, IEnumerable<IDataEntity> recordBrowsingContext, int selectedTabIndex)
            : this(customerID, recordBrowsingContext)
        {
            this.selectedTabIndex = selectedTabIndex;
        }

        public CustomerView(RecordIdentifier customerID, IEnumerable<IDataEntity> recordBrowsingContext)
            : this()
	    {
            this.recordBrowsingContext = recordBrowsingContext;
            this.customerID = customerID;
	    }

        public CustomerView(RecordIdentifier customerID)
            : this()
        {
            this.customerID = customerID;
        }

        public CustomerView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help |
                ViewAttributes.RecordCursor;

            this.ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.CustomerEdit);

            fullNameControl.PopulateNamePrefixes(PluginEntry.DataModel.Cache.GetNamePrefixes());
        }

        protected override ViewBase GetRecordCursorView(int cursorIndex)
        {
            if (recordBrowsingContext != null)
            {
                RecordIdentifier id = recordBrowsingContext.ElementAt<IDataEntity>(cursorIndex).ID;

                if (Providers.CustomerData.Exists(PluginEntry.DataModel, id))
                {
                    return new CustomerView(recordBrowsingContext.ElementAt<IDataEntity>(cursorIndex).ID, recordBrowsingContext, tabSheetTabs.SelectedTabIndex);
                }
            }

            return null;
        }

        protected override void GetRecordCursorInfo(ContextBarCursorEventArguments args)
        {
            if (recordBrowsingContext != null)
            {
                args.Position = 0;
                args.Count = recordBrowsingContext.Count<IDataEntity>();

                foreach (IDataEntity entity in recordBrowsingContext)
                {
                    if (entity.ID == customerID)
                    {
                        return;
                    }

                    args.Position++;
                }
            }
            else
            {
                args.Count = 1;
                args.Position = 0;
            }
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("Customer", customerID, Properties.Resources.CustomerText, true));
            contexts.Add(new AuditDescriptor("RBOCustomer", customerID, Properties.Resources.CustomerText, true));
        }

        public override ParentViewDescriptor CurrentViewDescriptor()
        {
            return new ParentViewDescriptor(
                    customerID,
                    Description,
                    Properties.Resources.Customer,
                    new ShowParentViewHandler(PluginOperations.ShowCustomer));
        }

        public override string ShortHeaderText
        {
            get
            {
                string formattedCustomerName = customer.GetFormattedName(PluginEntry.DataModel.Settings.NameFormatter);
                return customerID + " - " + formattedCustomerName;
            }
        }

        public string Description
        {
            get
            {
                return Properties.Resources.CustomerText + ": " + ShortHeaderText;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.CustomerText;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        {
                return customerID;
	        }
        }

        protected override void InitializeView(bool isMultiEdit)
        {
            if (!initialized)
            {
                addressTab = new TabControl.Tab(Properties.Resources.Address, ViewPages.CustomerAddressPage.CreateInstance);
                detailTab = new TabControl.Tab(Properties.Resources.Details, ViewPages.CustomerDetailPage.CreateInstance);
                contactTab = new TabControl.Tab(Properties.Resources.Contact, ViewPages.CustomerContactPage.CreateInstance);
                discountsTab = new TabControl.Tab(Properties.Resources.Discounts, "", ViewPages.CustomerDiscountsPage.CreateInstance, ViewPages.CustomerDiscountsPage.TabMessage);
                groupTab = new TabControl.Tab(Properties.Resources.CustomerGroups, "", ViewPages.CustomerGroupsPage.CreateInstance);

                tabSheetTabs.AddTab(addressTab);
                tabSheetTabs.AddTab(detailTab);
                tabSheetTabs.AddTab(contactTab);
                tabSheetTabs.AddTab(discountsTab);
                tabSheetTabs.AddTab(groupTab);

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, customerID, customer);

                initialized = true;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            customer = Providers.CustomerData.Get(PluginEntry.DataModel, customerID, UsageIntentEnum.Reporting);

            HeaderText = Description;
            tbID.Text = (string)customer.ID;
            fullNameControl.NameRecord = customer.CopyName();
            tbDisplayName.Text = customer.Text;

            tabSheetTabs.SetData(isRevert, customerID, customer);

            if (selectedTabIndex != -1)
            {
                tabSheetTabs.SelectedTab = tabSheetTabs[selectedTabIndex];
            }

            tbDisplayName.Enabled = fullNameControl.IsEmpty;

            tabSheetTabs.SendTabMessage(0, null);
        }

        protected override bool DataIsModified()
        {
            customer.Dirty = 
                (!customer.CompareNames(fullNameControl.NameRecord)) ||
                (tbDisplayName.Text != customer.Text);

            bool tabsModified = tabSheetTabs.IsModified();

            return tabsModified || customer.Dirty;
        }
         
        protected override bool SaveData()
        {
            if (!customer.CompareNames(fullNameControl.NameRecord))
            {
                customer.FirstName = fullNameControl.NameRecord.First;
                customer.MiddleName = fullNameControl.NameRecord.Middle;
                customer.LastName = fullNameControl.NameRecord.Last;
                customer.Prefix = fullNameControl.NameRecord.Prefix;
                customer.Suffix = fullNameControl.NameRecord.Suffix;
            }
            customer.Text = tbDisplayName.Text;

            tabSheetTabs.GetData();

            if (customer.Dirty)
            {
                Providers.CustomerData.Save(PluginEntry.DataModel, customer);

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Customer", customer.ID, null);
            }

            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "Customer":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == customerID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    if (changeHint == DataEntityChangeType.MultiDelete && ((List<RecordIdentifier>)param).Contains(customerID))
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    if (changeHint == DataEntityChangeType.Edit && (changeIdentifier == customerID || changeIdentifier == RecordIdentifier.Empty))
                    {
                        LoadData(true);
                    }
                    break;
                case "SelectedCustomerAddressChanged":
                    RebuildContextBarOnAddressChanged();
                    break;
            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteCustomer(customerID);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.ViewAllCustomers, null, PluginOperations.ShowCustomersListView), 200);                
            }

            if(arguments.CategoryKey == base.GetType().ToString() + ".Actions")
            {
                if(tabSheetTabs.SelectedTab == addressTab)
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.SetGroupAsDefault, "SetAddressDefault", 
                        ((ViewPages.CustomerAddressPage)addressTab.View).CanSetAsDefault, 
                        ((ViewPages.CustomerAddressPage)addressTab.View).SetAsDefault), 100);
                }
                if (tabSheetTabs.SelectedTab == detailTab && PluginEntry.DataModel.HasPermission(Permission.AdministrationMaintainSettings))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.StoreManagement, "SetDefaultTaxExemptionGroup",
                        PluginEntry.DataModel.HasPermission(Permission.AdministrationMaintainSettings),
                        ((ViewPages.CustomerDetailPage)detailTab.View).SetDefaultTaxExemptionGroup), 200);
                }
            }

            contextBarInitialized = true;
        }

        public override string ContextDescription
        {
            get
            {
                return fullNameControl.IsEmpty ? tbDisplayName.Text : PluginEntry.DataModel.Settings.NameFormatter.Format(fullNameControl.NameRecord);
            }
        }

        protected override void OnClose()
        {
            fullNameControl.Dispose();

            if(fullNameControl != null)
            {
                fullNameControl = null;
            }

            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }

        private void fullNameControl_ValueChanged(object sender, EventArgs e)
        {
            tbDisplayName.Enabled = fullNameControl.IsEmpty;
            tbDisplayName.Text = PluginEntry.DataModel.Settings.NameFormatter.Format(fullNameControl.NameRecord);
        }

        private void tabSheetTabs_SelectedTabChanged(object sender, EventArgs e)
        {
            RebuildContextBarOnAddressChanged();
            lastOpenedTab = tabSheetTabs.SelectedTab;
        }

        private void RebuildContextBarOnAddressChanged()
        {
            if (lastOpenedTab == addressTab || tabSheetTabs.SelectedTab == addressTab)
            {
                if (contextBarInitialized)
                {
                    PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
                }
            }
        }
    }
}
