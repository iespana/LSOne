using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.RetailItems.Dialogs;
using LSOne.ViewPlugins.RetailItems.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using System.Linq;
using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    public partial class RetailDepartmentRetailGroupsPage : UserControl, ITabView
    {
        RecordIdentifier selectedID = "";
        RecordIdentifier retailDepartmentID;
        
        WeakReference owner;

        public RetailDepartmentRetailGroupsPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public RetailDepartmentRetailGroupsPage()
        {
            InitializeComponent();

            btnsGroups.AddButtonEnabled = PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailGroups);

            lvGroups.Columns[0].Tag = RetailGroupSorting.RetailGroupId;
            lvGroups.Columns[1].Tag = RetailGroupSorting.RetailGroupName;

            lvGroups.ContextMenuStrip = new ContextMenuStrip();
            lvGroups.ContextMenuStrip.Opening += lvGroups_Opening;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.RetailDepartmentRetailGroupsPage((TabControl)sender);
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            retailDepartmentID = context;
            LoadGroups();
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "Defaultdata":
                    LoadGroups();
                    break;
            }
        }

        private void LoadGroups()
        {
            if (retailDepartmentID == RecordIdentifier.Empty)
            {
                return;
            }
            List<RetailGroup> items;
            ListViewItem listItem;

            lvGroups.Items.Clear();

            items = Providers.RetailGroupData.GetRetailGroupsInRetailDepartment(
                PluginEntry.DataModel,
                retailDepartmentID,
                (RetailGroupSorting)lvGroups.Columns[lvGroups.SortColumn].Tag,
                lvGroups.SortedBackwards);

            foreach (var item in items)
            {
                listItem = new ListViewItem((string)item.ID);
                listItem.SubItems.Add(item.Text);
                listItem.ImageIndex = -1;

                listItem.Tag = item.ID;

                lvGroups.Add(listItem);

                if (selectedID == (RecordIdentifier)listItem.Tag)
                {
                    listItem.Selected = true;
                }
            }

            lvGroups.Columns[lvGroups.SortColumn].ImageIndex = (lvGroups.SortedBackwards ? 1 : 0);

            lvGroups_SelectedIndexChanged(this, EventArgs.Empty);

            lvGroups.BestFitColumns();
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }

        void lvGroups_Opening(object sender, CancelEventArgs e)
        {
            var menu = lvGroups.ContextMenuStrip;
            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnsGroups_AddButtonClicked)
                           {
                               Enabled = btnsGroups.AddButtonEnabled,
                               Image = ContextButtons.GetAddButtonImage()
                           };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnsGroups_RemoveButtonClicked)
                       {
                           Enabled = btnsGroups.RemoveButtonEnabled,
                           Image = ContextButtons.GetRemoveButtonImage()
                       };

            menu.Items.Add(item);

            item = new ExtendedMenuItem("-", 400);
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.ViewRetailGroup,
                    500,
                    ShowRetailGroupView) {Enabled = btnsGroups.RemoveButtonEnabled};

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("RetailGroupList", lvGroups.ContextMenuStrip, lvGroups);

            e.Cancel = (menu.Items.Count == 0);
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        private void lvGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnViewRetailGroup.Enabled = lvGroups.SelectedItems.Count > 0;
            btnsGroups.RemoveButtonEnabled = lvGroups.SelectedItems.Count > 0 && btnsGroups.AddButtonEnabled;
        }

        private void btnsGroups_AddButtonClicked(object sender, EventArgs e)
        {
            var dlg = new RetailGroupNotInRetailDepartmentSearchDialog(PluginEntry.DataModel, PluginEntry.Framework, retailDepartmentID);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                List<RetailGroup> retailGroups = dlg.RetailGroups;
                if (retailGroups.Any(x => x.RetailDepartmentID.StringValue != ""))
                {
                    if (QuestionDialog.Show(Resources.RetailGroupAlreadyAssignedToADepartment + "\n" +
                                            Resources.DoYouWantToContinue) == DialogResult.Yes)
                    {
                        Providers.RetailGroupData.AddRetailGroupsToRetailDepartment(PluginEntry.DataModel, retailGroups.Select(x => x.ID).ToList(), retailDepartmentID);
                    }
                }
                else
                {
                    Providers.RetailGroupData.AddRetailGroupsToRetailDepartment(PluginEntry.DataModel, retailGroups.Select(x => x.ID).ToList(), retailDepartmentID);
                }
                LoadGroups();
            }
        }

        private void btnsGroups_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.RemoveRetailGroupFromRetailDepartmentQuestion,
                Properties.Resources.RemoveRetailGroupFromRetailDepartment) == DialogResult.Yes)
            {
                var retailGroupID = (RecordIdentifier)lvGroups.SelectedItems[0].Tag;

                Providers.RetailGroupData.RemoveRetailGroupFromRetailDepartment(PluginEntry.DataModel, retailGroupID);

                LoadGroups();

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "RetailGroup", retailGroupID, null);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "RetailGroup", retailDepartmentID, null);
            }
        }

        private void ShowRetailGroupView(object sender, EventArgs e)
        {
            var retailGroupID = (RecordIdentifier)lvGroups.SelectedItems[0].Tag;
            PluginOperations.ShowRetailGroupView(retailGroupID);
        }
    }
}
