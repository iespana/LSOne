using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Customer.Views
{
    public partial class CustomerGroupsView : ViewBase
    {
        private CustomerGroup selectedGroup = new CustomerGroup();

        public CustomerGroupsView(RecordIdentifier groupID)
            : this()
        {
            selectedGroup.ID = groupID;
        }

        public CustomerGroupsView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.ContextBar
                | ViewAttributes.Close
                | ViewAttributes.Help;

            HeaderText = Properties.Resources.CustomerGroups;
       
            colID.HeaderText = Properties.Resources.CustomerGroup;
            colDescription.HeaderText = Properties.Resources.CustGroupDescription;
            colCategory.HeaderText = Properties.Resources.CustGroupCategory;

            lvCustomerGroups.SecondarySortColumn = 1;

            lvCustomerGroups.ContextMenuStrip = new ContextMenuStrip();
            lvCustomerGroups.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageCustomerGroups);
            btnsEditAddRemove.EditButtonEnabled = !ReadOnly;
        }

        void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ContextMenuStrip menu = lvCustomerGroups.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.EditString,
                    100,
                    btnsEditAddRemove_EditButtonClicked)
            {
                //Image = Properties.Resources.EditImage,
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   btnsEditAddRemove_AddButtonClicked)
            {
                Enabled = btnsEditAddRemove.AddButtonEnabled,
                Image = ContextButtons.GetAddButtonImage()
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.DeleteString,
                    300,
                    btnsEditAddRemove_RemoveButtonClicked)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsEditAddRemove.RemoveButtonEnabled
            };

            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 350));

            item = new ExtendedMenuItem(btnSetGroupAsDefault.Text, 400, btnSetGroupAsDefault_Click)
            {
                Enabled = btnSetGroupAsDefault.Enabled
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify(NotifyContexts.CategoriesViewListContextMenu, lvCustomerGroups.ContextMenuStrip, lvCustomerGroups);

            e.Cancel = (menu.Items.Count == 0);
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case NotifyContexts.CustomerGroupsView:
                    selectedGroup = (CustomerGroup)param;
                    LoadItems();
                    break;
                case NotifyContexts.CustomerGroupView:
                    if (changeAction == DataEntityChangeType.Edit)
                    {
                        selectedGroup = (CustomerGroup) param;
                    }
                    LoadItems();
                    break;
            }
            tabSheetTabs.BroadcastChangeInformation(changeAction, objectName, changeIdentifier, param);
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.CustomerGroups;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return selectedGroup.ID;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.Customers, ViewPages.CustomerGroupCustomersPage.CreateInstance));
                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, selectedGroup.ID);
            }
            LoadTab(isRevert);
            LoadItems();
        }

        private void LoadTab(bool isRevert)
        {
            tabSheetTabs.SetData(isRevert, selectedGroup.ID, selectedGroup); ;
        }

        private void LoadItems()
        {

            lvCustomerGroups.Rows.Clear();

            List<CustomerGroup> groups = Providers.CustomerGroupData.GetList(PluginEntry.DataModel);

            foreach (CustomerGroup group in groups)
            {
                Row row = new Row();
                row.AddText((string)group.ID);
                row.AddText(group.Text);
                row.AddText(group.Category.Text);
                row.Tag = group;
                lvCustomerGroups.AddRow(row);

                if (selectedGroup.ID != RecordIdentifier.Empty && group.ID == selectedGroup.ID)
                {
                    lvCustomerGroups.Selection.Set(lvCustomerGroups.RowCount - 1);
                }
            }

            lvCustomerGroups.Sort();
            lvCustomerGroups_SelectionChanged(this, EventArgs.Empty);

            lvCustomerGroups.AutoSizeColumns();
            
        }

        private void lvCustomerGroups_SelectionChanged(object sender, EventArgs e)
        {
            btnsEditAddRemove.EditButtonEnabled = btnsEditAddRemove.RemoveButtonEnabled = lvCustomerGroups.Selection.Count > 0;
            if (lvCustomerGroups.Selection.Count > 0 && lvCustomerGroups.RowCount > 0)
            {
                selectedGroup = (CustomerGroup)lvCustomerGroups.Row(lvCustomerGroups.Selection.FirstSelectedRow).Tag;
                
                tabSheetTabs.Visible = true;
                groupPanelNoSelection.Visible = false;
                lblNoSelection.Visible = false;

                lvCustomerGroups.Select();

                btnsEditAddRemove.AddButtonEnabled = btnsEditAddRemove.RemoveButtonEnabled = btnsEditAddRemove.EditButtonEnabled = !ReadOnly;
                btnSetGroupAsDefault.Enabled = !ReadOnly;
                
                LoadTab(true);
            }
            else
            {
                selectedGroup = new CustomerGroup();

                btnsEditAddRemove.AddButtonEnabled = !ReadOnly;
                btnsEditAddRemove.RemoveButtonEnabled = btnsEditAddRemove.EditButtonEnabled = 
                    btnSetGroupAsDefault.Enabled =  false;

                tabSheetTabs.Visible = false;
                groupPanelNoSelection.Visible = true;
                lblNoSelection.Visible = true;

                lvCustomerGroups.Select();
            }
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            var tmpGroup = PluginOperations.NewCustomerGroup();
            if (tmpGroup != null)
            {
                selectedGroup = tmpGroup;
            }
            lvCustomerGroups.Select();
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowCustomerGroupView(selectedGroup.ID);
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            var idToRemove = selectedGroup.ID;

            if(!PluginOperations.DeleteCustomerGroup(idToRemove)) return;
            if (lvCustomerGroups.RowCount == 1)
            {
                lvCustomerGroups.Selection.Clear();
            }
            else if (lvCustomerGroups.Selection.FirstSelectedRow == lvCustomerGroups.RowCount - 1)
            {
                //Last row was deleted
                lvCustomerGroups.Selection.Set(lvCustomerGroups.Selection.FirstSelectedRow - 1);
            }
            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, NotifyContexts.CustomerGroupsView, idToRemove, new CustomerGroup());
        }

        private void btnSetGroupAsDefault_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(Properties.Resources.AreYouSureYouWantToSetAsDefault) == DialogResult.No)
            {
                return;
            }

            List<CustomerInGroup> inGroup = Providers.CustomersInGroupData.GetCustomersInGroupList(PluginEntry.DataModel, selectedGroup.ID, null, null);

            ClearDefaultOfAllCustomers(inGroup);

            //Update all the customers with the default value
            foreach (CustomerInGroup gr in inGroup)
            {
                Providers.CustomersInGroupData.SetGroupAsDefault(PluginEntry.DataModel, gr);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, NotifyContexts.CustomerDefaultGroupChanged, gr.ID.PrimaryID, null);
            }
        }
        
        private void ClearDefaultOfAllCustomers(List<CustomerInGroup> inGroup)
        {
            if (inGroup == null)
            {
                inGroup = Providers.CustomersInGroupData.GetCustomersInGroupList(PluginEntry.DataModel, selectedGroup.ID, null, null);
            }

            //Clear all default values currently on the groups that belong to each customer
            foreach (CustomerInGroup curr in inGroup)
            {
                List<CustomerInGroup> groups = Providers.CustomersInGroupData.GetGroupsForCustomerList(PluginEntry.DataModel, curr.ID.PrimaryID);
                foreach (CustomerInGroup group in groups)
                {
                    Providers.CustomersInGroupData.ClearDefaultValueForCustomer(PluginEntry.DataModel, group);   
                }
            }
        }

        private void lvCustomerGroups_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
                btnsEditAddRemove_EditButtonClicked(sender, EventArgs.Empty);
        }
    }
}
