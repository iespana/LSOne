using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.RetailItems.Views
{
    public partial class RetailDepartmentsView : ViewBase
    {

        RecordIdentifier selectedID = "";

        public RetailDepartmentsView(RecordIdentifier groupId):
            this()
        {
            selectedID = groupId;
        }

     
        public RetailDepartmentsView()            
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            HeaderText = Properties.Resources.RetailDepartments;

            btnsDepartments.AddButtonEnabled = PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailDepartments);

            lvDepartments.SmallImageList = PluginEntry.Framework.GetImageList();

            lvDepartments.Columns[0].Tag = RetailDepartment.SortEnum.RetailDepartment;
            lvDepartments.Columns[1].Tag = RetailDepartment.SortEnum.Description;

            lvDepartments.ContextMenuStrip = new ContextMenuStrip();
            lvDepartments.ContextMenuStrip.Opening += lvItems_Opening;

            ReadOnly = !PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailDepartments);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("RetailDepartments", 0, Properties.Resources.RetailDepartments, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.RetailDepartments;
            }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == this.GetType().ToString() + ".Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.RetailGroups, PluginOperations.ShowRetailGroupListView), 100);
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
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.RetailGroups, ViewPages.RetailDepartmentRetailGroupsPage.CreateInstance));
                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, selectedID);
            }
            loadTab(isRevert);
            LoadItems();
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "Defaultdata":

                case "RetailDepartment":
                    if (changeHint == DataEntityChangeType.Edit || changeHint == DataEntityChangeType.Add)
                    {
                        selectedID = changeIdentifier;
                    }


                    LoadItems();
                    break;
            }
            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        private void LoadItems()
        {
            List<RetailDepartment> retailDepartments;
            ListViewItem listItem;

            lvDepartments.Items.Clear();

            retailDepartments = Providers.RetailDepartmentData.GetDetailedList(PluginEntry.DataModel, (RetailDepartment.SortEnum)lvDepartments.Columns[lvDepartments.SortColumn].Tag, lvDepartments.SortedBackwards);

            foreach (var item in retailDepartments)
            {
                listItem = new ListViewItem((string)item.ID);
                listItem.SubItems.Add(item.Text);

                listItem.ImageIndex = -1;

                listItem.Tag = item.ID;

                lvDepartments.Add(listItem);

                if (selectedID == (RecordIdentifier)listItem.Tag)
                {
                    listItem.Selected = true;
                }
            }

            lvDepartments.Columns[lvDepartments.SortColumn].ImageIndex = (lvDepartments.SortedBackwards ? 1 : 0);

            lvItems_SelectedIndexChanged(this, EventArgs.Empty);

            lvDepartments.BestFitColumns();
            lvDepartments.ShowSelectedItem();
        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCopyID.Enabled = lvDepartments.SelectedItems.Count > 0;
            btnsDepartments.EditButtonEnabled = btnsDepartments.RemoveButtonEnabled = lvDepartments.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailDepartments);

            if (lvDepartments.SelectedItems.Count > 0)
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


        private void loadTab(bool isRevert)
        {
            if (lvDepartments.SelectedItems.Count > 0)
            {
                selectedID = (RecordIdentifier)lvDepartments.SelectedItems[0].Tag;
            }
            else
            {
                selectedID = RecordIdentifier.Empty;
            }
            tabSheetTabs.SetData(isRevert, selectedID, null); 
        }

        private void lvItems_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvDepartments.SortColumn == e.Column)
            {
                lvDepartments.SortedBackwards = !lvDepartments.SortedBackwards;
            }
            else
            {
                if (lvDepartments.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvDepartments.Columns[lvDepartments.SortColumn].ImageIndex = 2;
                    lvDepartments.SortColumn = e.Column;
                }
                lvDepartments.SortedBackwards = false;
            }

            LoadItems();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PluginOperations.NewRetailDepartment(this, EventArgs.Empty);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.EditRetailDepartment(((RecordIdentifier)lvDepartments.SelectedItems[0].Tag).PrimaryID);
        }

        private void lvItems_DoubleClick(object sender, EventArgs e)
        {
            if (btnsDepartments.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteRetailDepartmentQuestion,
                Properties.Resources.DeleteRetailDepartment) == DialogResult.Yes)
            {
                int selectedIndex = lvDepartments.SelectedIndices[0];
                int nextSelection = selectedIndex == (lvDepartments.Items.Count - 1) ? selectedIndex - 1 : selectedIndex + 1;

                PluginOperations.DeleteRetailDepartment((RecordIdentifier)lvDepartments.SelectedItems[0].Tag);

                selectedID = nextSelection >= 0 ? (RecordIdentifier) lvDepartments.Items[nextSelection].Tag : RecordIdentifier.Empty;

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "RetailDepartment", null, null);
            }
        }

        void lvItems_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvDepartments.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnEdit_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsDepartments.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnAdd_Click);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsDepartments.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemove_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsDepartments.RemoveButtonEnabled;
            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 2000));

            item = new ExtendedMenuItem(Properties.Resources.CopyID, 2010, CopyID);
            item.Enabled = (lvDepartments.SelectedItems.Count == 1);
            menu.Items.Add(item);
            
            PluginEntry.Framework.ContextMenuNotify("RetailDepartmentList", lvDepartments.ContextMenuStrip, lvDepartments);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void CopyID(object sender, EventArgs args)
        {
            Clipboard.SetText(lvDepartments.SelectedItems[0].Text);
        }

        public override ParentViewDescriptor CurrentViewDescriptor()
        {
            //return new ParentViewDescriptor(
            //        (int)displayType,
            //        LogicalContextName,
            //        null,
            //        new ShowParentViewHandler(PluginOperations.ShowCustomerPriceDiscountGroups));

            return null;
        }

        protected override void OnClose()
        {
            lvDepartments.SmallImageList = null;

            base.OnClose();
        }
    }
}
