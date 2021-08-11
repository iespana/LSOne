using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.Views
{
    public partial class SpecialGroupsView : ViewBase
    {
        RecordIdentifier selectedID = "";

        public SpecialGroupsView(RecordIdentifier specialGroupID):
            this()
        {
            selectedID = specialGroupID;
        }

        public SpecialGroupsView()
            : base()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            HeaderText = Properties.Resources.SpecialGroups;

            lvItems.SmallImageList = PluginEntry.Framework.GetImageList();

            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += new CancelEventHandler(lvItems_Opening);

            ReadOnly = !PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageSpecialGroups);
            btnsContextButtons.AddButtonEnabled = !ReadOnly;

            lvItems.Columns[0].Tag = SpecialGroupSorting.GroupID;
            lvItems.Columns[1].Tag = SpecialGroupSorting.GroupName;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("SpecialGroups", 0, Properties.Resources.SpecialGroups, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.SpecialGroups;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.Items, ViewPages.SpecialGroupItemsPage.CreateInstance));
                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, selectedID);
            }
            LoadItems();
            loadTab(isRevert);
        }

        private void LoadItems()
        {
            List<DataEntity> items;
            ListViewItem listItem;

            lvItems.Items.Clear();

            items = Providers.SpecialGroupData.GetList(PluginEntry.DataModel, 
                                                            (SpecialGroupSorting)lvItems.Columns[lvItems.SortColumn].Tag, 
                                                            lvItems.SortedBackwards);

            foreach (var item in items)
            {
                listItem = new ListViewItem((string)item.ID);
                listItem.SubItems.Add(item.Text);
                listItem.ImageIndex = -1;

                listItem.Tag = item;

                lvItems.Add(listItem);

                if (selectedID == (RecordIdentifier)((DataEntity)listItem.Tag).ID)
                {
                    listItem.Selected = true;
                }
            }

            lvItems.Columns[lvItems.SortColumn].ImageIndex = (lvItems.SortedBackwards ? 1 : 0);

            lvItems_SelectedIndexChanged(this, EventArgs.Empty);

            lvItems.BestFitColumns();
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled = 
                lvItems.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageSpecialGroups);
            if (lvItems.SelectedItems.Count > 0)
            {
                tabSheetTabs.Visible = true;
                groupPanelNoSelection.Visible = false;
                lblNoSelection.Visible = false;
                loadTab(true);
            }
            else
            {
                tabSheetTabs.Visible = false;
                groupPanelNoSelection.Visible = true;
                lblNoSelection.Visible = true;
            }
        }

        private void lvItems_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvItems.SortColumn == e.Column)
            {
                lvItems.SortedBackwards = !lvItems.SortedBackwards;
            }
            else
            {
                if (lvItems.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvItems.Columns[lvItems.SortColumn].ImageIndex = 2;
                    lvItems.SortColumn = e.Column;
                }
                lvItems.SortedBackwards = false;
            }

            LoadItems();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Dialogs.SpecialGroupDialog dlg = new Dialogs.SpecialGroupDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedID = dlg.GetSelectedId();
                LoadItems();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string specialGroupId = (string)((RecordIdentifier)((DataEntity)lvItems.SelectedItems[0].Tag).ID).PrimaryID;
            Dialogs.SpecialGroupDialog dlg = new Dialogs.SpecialGroupDialog(specialGroupId);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedID = dlg.GetSelectedId();
                LoadItems();
            }
        }

        private void lvItems_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteSpecialGroupQuestion,
                Properties.Resources.DeleteSpecialGroup) == DialogResult.Yes)
            {
                Providers.SpecialGroupData.Delete(
                    PluginEntry.DataModel,
                    (RecordIdentifier)((DataEntity)lvItems.SelectedItems[0].Tag).ID);

                LoadItems();
            }
        }

        void lvItems_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvItems.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnEdit_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnAdd_Click);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemove_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 2000));

            item = new ExtendedMenuItem(Properties.Resources.CopyID, 2010, CopyID);
            item.Enabled = (lvItems.SelectedItems.Count == 1);
            menu.Items.Add(item);
            
            
            PluginEntry.Framework.ContextMenuNotify("SpecialGroupList", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void CopyID(object sender, EventArgs args)
        {
            Clipboard.SetText(lvItems.SelectedItems[0].Text);
        }

        protected override void OnClose()
        {
            lvItems.SmallImageList = null;

            base.OnClose();
        }

        private void loadTab(bool isRevert)
        {
            DataEntity specialGroup;
            if (lvItems.SelectedItems.Count > 0)
            {
                specialGroup = (DataEntity)lvItems.SelectedItems[0].Tag;
            }
            else
            {
                specialGroup = new DataEntity();
            }
            selectedID = (RecordIdentifier)specialGroup.ID;
            tabSheetTabs.SetData(isRevert, selectedID, specialGroup); ;
        }
    }
}
