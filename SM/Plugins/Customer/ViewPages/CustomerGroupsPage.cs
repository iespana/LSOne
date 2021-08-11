using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Customer.ViewPages
{
    public partial class CustomerGroupsPage : UserControl, ITabView
    {
        private DataLayer.BusinessObjects.Customers.Customer selectedCustomer;
        private int selectedRow;
        private CustomerGroup selectedGroup = new CustomerGroup();

        WeakReference owner;

        public CustomerGroupsPage(TabControl owner) 
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public CustomerGroupsPage()
            : base()
        {
            InitializeComponent();

            selectedCustomer = new DataLayer.BusinessObjects.Customers.Customer();

            lvCustomerGroup.ContextMenuStrip = new ContextMenuStrip();
            lvCustomerGroup.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvCustomerGroup.ContextMenuStrip;

            menu.Items.Clear();

            item = new ExtendedMenuItem(
                    Properties.Resources.Add + "...",
                    100,
                    btnAdd_Click)
            {
                Enabled = btnsContextButtonsCustomers.AddButtonEnabled,
                Image = ContextButtons.GetAddButtonImage(),
                Default = true
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    200,
                    btnDelete_Click)
            {
                Enabled = btnsContextButtonsCustomers.RemoveButtonEnabled,
                Image = ContextButtons.GetRemoveButtonImage()
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem("-", 300);
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.ViewCustomerGroup + "...",
                    400,
                    btnViewCustomerGroup_Click)
            {
                Enabled = btnsContextButtonsCustomers.EditButtonEnabled,
                Image = ContextButtons.GetEditButtonImage()
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem("-", 500);
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.SetGroupAsDefault,
                    600,
                    btnSetAsDefault_Click)
            {
                Enabled = btnsContextButtonsCustomers.EditButtonEnabled,
                Image = ContextButtons.GetEditButtonImage()
            };
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify(NotifyContexts.CustomerGroupsPageListContextMenu, lvCustomerGroup.ContextMenuStrip, lvCustomerGroup);

            e.Cancel = (menu.Items.Count == 0);    
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.CustomerGroupsPage((TabControl)sender);
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            selectedCustomer = (DataLayer.BusinessObjects.Customers.Customer)internalContext;
            

            LoadLines();
        }

        private void LoadLines()
        {
            lvCustomerGroup.Rows.Clear();

            List<CustomerGroup> groups = Providers.CustomerGroupData.GetGroupsForCustomer(PluginEntry.DataModel, selectedCustomer.ID);

            if (groups.Count > 0 && groups.Count(c => c.DefaultGroup) == 0)
            {
                SetGroupAsDefault(groups.First());
                groups.First().DefaultGroup = true;
            }

            DecimalLimit limiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

            foreach (CustomerGroup group in groups)
            {
                Row row = new Row();

                if (group.DefaultGroup)
                {
                    var button = new IconButton(Properties.Resources.tick, Properties.Resources.Default, true);
                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", false));
                }
                else
                {
                    row.AddText("");
                }
                
                row.AddText(group.Text);
                row.AddText(group.Category.Text);
                row.AddCell(new CheckBoxCell(group.Exclusive, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                row.AddCell(new CheckBoxCell(group.UsesDiscountedPurchaseLimit, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                row.AddCell(new NumericCell(group.MaxDiscountedPurchases > decimal.Zero ? group.MaxDiscountedPurchases.FormatWithLimits(limiter) : "", group.MaxDiscountedPurchases));
                row.AddText(group.PeriodString);
                row.Tag = group;
                lvCustomerGroup.AddRow(row);

                if (selectedGroup.ID != RecordIdentifier.Empty && group.ID == selectedGroup.ID)
                {
                    lvCustomerGroup.Selection.Set(lvCustomerGroup.RowCount - 1);
                }
            }

            lvCustomerGroup.Sort();

            lvCustomerGroup_SelectionChanged(this, EventArgs.Empty);

            lvCustomerGroup.AutoSizeColumns();
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            //contexts.Add(new AuditDescriptor("RetailGroups", 0, Properties.Resources.RetailGroups, false));
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case NotifyContexts.CustomerCardGroupsPageList:
                case NotifyContexts.CustomersInGroupPage:
                case NotifyContexts.CustomerGroupsView:
                case NotifyContexts.CustomerGroupsPage:
                case NotifyContexts.CustomerDefaultGroupChanged:
                    LoadLines();
                    break;
            }
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PluginOperations.EditGroupsOnCustomer(selectedCustomer.ID);
        }

        private void btnViewCustomerGroup_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowCustomerGroupView(selectedGroup.ID);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<CustomerInGroup> toRemove = new List<CustomerInGroup>();

            for (int i = 0; i < lvCustomerGroup.Selection.Count; i++)
            {
                CustomerGroup custGroup = (CustomerGroup)lvCustomerGroup.Row(lvCustomerGroup.Selection.GetRowIndex(i)).Tag;
                CustomerInGroup inGroup = new CustomerInGroup();

                inGroup.ID = new RecordIdentifier(selectedCustomer.ID, custGroup.ID);
                toRemove.Add(inGroup);
            }

            PluginOperations.DeleteCustomerInGroup(toRemove, selectedGroup.ID);
        }

        private void btnSetAsDefault_Click(object sender, EventArgs e)
        {
            SetGroupAsDefault();
        }

        private void SetGroupAsDefault(CustomerGroup group)
        {
            CustomerInGroup inGroup = new CustomerInGroup();
            inGroup.ID = new RecordIdentifier(selectedCustomer.ID, group.ID);
            inGroup.Default = true;

            List<CustomerInGroup> groupList = new List<CustomerInGroup>();
            groupList.Add(inGroup);

            SetGroupAsDefault(groupList);

        }

        private void SetGroupAsDefault(List<CustomerInGroup> groups = null)
        {
            if (selectedGroup.ID == RecordIdentifier.Empty) return;
            if (groups == null)
            {
                groups = Providers.CustomersInGroupData.GetGroupsForCustomerList(PluginEntry.DataModel, selectedCustomer.ID);
            }
            
            foreach (CustomerInGroup group in groups)
            {
                if (group.GroupID != selectedGroup.ID && group.Default)
                {
                    Providers.CustomersInGroupData.ClearDefaultValueForCustomer(PluginEntry.DataModel, group);
                }
                else if (group.GroupID == selectedGroup.ID)
                {
                    Providers.CustomersInGroupData.SetGroupAsDefault(PluginEntry.DataModel, group);
                }
            }
            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, NotifyContexts.CustomerDefaultGroupChanged, selectedCustomer.ID, null);
        }

        private void lvCustomerGroup_SelectionChanged(object sender, EventArgs e)
        {
            if (lvCustomerGroup.Selection.Count > 0)
            {
                selectedGroup = (CustomerGroup)lvCustomerGroup.Row(lvCustomerGroup.Selection.FirstSelectedRow).Tag;
                selectedRow = lvCustomerGroup.Selection.FirstSelectedRow;
            }
            else
            {
                selectedGroup = new CustomerGroup(); 
            }

            btnsContextButtonsCustomers.RemoveButtonEnabled =
            btnsContextButtonsCustomers.EditButtonEnabled =
            btnViewCustomerGroup.Enabled =
            btnSetGroupAsDefault.Enabled = (lvCustomerGroup.Selection.Count > 0);
        }
    }
}
