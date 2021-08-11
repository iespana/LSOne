using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Customer.ViewPages
{
    public partial class CustomerGroupCustomersPage : UserControl, ITabView
    {
        private CustomerGroup selectedGroup;
        private RecordIdentifier selectedCustomer;
        private int selectedRow;
        
        WeakReference owner;

        public CustomerGroupCustomersPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public CustomerGroupCustomersPage()
            : base()
        {
            InitializeComponent();

            selectedGroup = new CustomerGroup();
            selectedCustomer = RecordIdentifier.Empty;

            selectedRow = -1;

            lvCustomerList.ContextMenuStrip = new ContextMenuStrip();
            lvCustomerList.ContextMenuStrip.Opening += lvCustomersList_Opening;

            colCustomer.HeaderText = Properties.Resources.CustomerText;

            customerDataScroll.PageSize = PluginEntry.DataModel.PageSize;
            customerDataScroll.Reset();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.CustomerGroupCustomersPage((TabControl)sender);
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            //contexts.Add(new AuditDescriptor("RetailGroups", 0, Properties.Resources.RetailGroups, false));
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            selectedGroup = (CustomerGroup)internalContext;
            
            LoadLines();
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case NotifyContexts.CustomersInGroupPage:
                    if (changeHint == DataEntityChangeType.Edit)
                    {
                        selectedGroup = param == null ? new CustomerGroup() : (CustomerGroup)param;
                        selectedGroup.ID = changeIdentifier;
                    }                    
                    LoadLines();
                    break;
            }
        }

        private void LoadLines()
        {
            lvCustomerList.ClearRows();

            var customers = Providers.CustomersInGroupData.GetCustomersInGroupList(PluginEntry.DataModel,
                                                                    selectedGroup.ID,
                                                                    customerDataScroll.StartRecord,
                                                                    customerDataScroll.EndRecord + 1);

            customerDataScroll.RefreshState(customers);
            int rowSelect = selectedRow;

            foreach (CustomerInGroup cust in customers)
            {
                Row row = new Row();
                row.AddText(PluginEntry.DataModel.Settings.NameFormatter.Format(cust.Name));
                row.Tag = cust;
                lvCustomerList.AddRow(row);

                if (selectedCustomer != RecordIdentifier.Empty && cust.ID == selectedCustomer)
                {
                    rowSelect = lvCustomerList.RowCount;
                }
            }

            if (rowSelect != -1)
            {
                rowSelect = rowSelect > lvCustomerList.RowCount - 1 ? lvCustomerList.RowCount - 1 : rowSelect;
                lvCustomerList.Selection.Set(rowSelect);
            }
            
            lvCustomers_SelectedIndexChanged(this, new EventArgs());

            lvCustomerList.AutoSizeColumns();
            
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }

        void lvCustomersList_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvCustomerList.ContextMenuStrip;

            menu.Items.Clear();

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    100,
                    btnAdd_Click)
                {
                    Enabled = btnsContextButtonsCustomers.AddButtonEnabled,
                    Image = ContextButtons.GetAddButtonImage(),
                    Default = true
                };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    200,
                    btnRemove_Click)
                {
                    Enabled = btnsContextButtonsCustomers.RemoveButtonEnabled,
                    Image = ContextButtons.GetRemoveButtonImage()
                };
            menu.Items.Add(item);

            item = new ExtendedMenuItem("-", 300);
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.ViewCustomer,
                    400,
                    ViewCustomer_Click)
            {
                Enabled = btnViewCustomer.Enabled
            };
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify(NotifyContexts.CustomersInGroupPageListContextMenu, lvCustomerList.ContextMenuStrip, lvCustomerList);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsContextButtonsCustomers.RemoveButtonEnabled =
            btnsContextButtonsCustomers.EditButtonEnabled =
            btnViewCustomer.Enabled = (lvCustomerList.Selection.Count > 0);
            
            selectedRow = lvCustomerList.Selection.Count > 0 ? lvCustomerList.Selection.FirstSelectedRow : -1;
        }

        private void OnPageScrollPageChanged(object sender, EventArgs e)
        {
            LoadLines();
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PluginOperations.EditCustomersInGroup(Permission.ManageCustomerGroups, CustomerGroupTypeEnum.CustomerGroup, null, selectedGroup.ID);
        }

        private void ViewCustomer_Click(object sender, EventArgs e)
        {
            if (lvCustomerList.Selection.Count > 0)
            {
                CustomerInGroup cust = (CustomerInGroup)lvCustomerList.Row(lvCustomerList.Selection.FirstSelectedRow).Tag;
                if (cust != null && cust.ID.PrimaryID != RecordIdentifier.Empty)
                {
                    PluginOperations.ShowCustomer(cust.ID.PrimaryID);
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            List<CustomerInGroup> toRemove = new List<CustomerInGroup>();

            for (int i = 0; i < lvCustomerList.Selection.Count; i++)
            {
                toRemove.Add((CustomerInGroup)lvCustomerList.Row(lvCustomerList.Selection.GetRowIndex(i)).Tag);
            }

            PluginOperations.DeleteCustomerInGroup(toRemove, selectedGroup.ID);
        }

        private void lvCustomerList_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsContextButtonsCustomers.AddButtonEnabled)
            {
                btnAdd_Click(this, null);
            }
        }
    }
}
