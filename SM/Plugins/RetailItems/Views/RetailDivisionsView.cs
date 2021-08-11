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
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.Views
{
    public partial class RetailDivisionsView : ViewBase
    {
        private RecordIdentifier selectedID = "";

        public RetailDivisionsView(RecordIdentifier divisionID) 
            : this()
        {
            selectedID = divisionID;
        }

        public RetailDivisionsView()
            : base()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            HeaderText = Properties.Resources.RetailDivisions;

            lvItems.SmallImageList = PluginEntry.Framework.GetImageList();

            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += lvItems_Opening;

            ReadOnly = !PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailDivisions);

            btnsContextButtons.AddButtonEnabled = !ReadOnly;

            lvItems.Columns[0].Tag = RetailDivisionSorting.RetailDivisionId;
            lvItems.Columns[1].Tag = RetailDivisionSorting.RetailDivisionName;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("RetailDivisions", 0, Properties.Resources.RetailDivisions, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.RetailDivisions;
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
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.RetailDepartments, ViewPages.RetailDivisionDepartmentPage.CreateInstance));
                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, selectedID);
            }
            LoadTab(isRevert);
            LoadItems();
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "RetailDivision":
                    if (changeHint == DataEntityChangeType.Edit)
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
            lvItems.Items.Clear();

            var divs = Providers.RetailDivisionData.GetDetailedList(PluginEntry.DataModel, (RetailDivisionSorting)lvItems.Columns[lvItems.SortColumn].Tag, lvItems.SortedBackwards);

            foreach (var division in divs)
            {
                var listItem = new ListViewItem((string)division.ID);
                listItem.SubItems.Add(division.Text);

                listItem.ImageIndex = -1;

                listItem.Tag = division;

                lvItems.Add(listItem);

                if (selectedID == (RecordIdentifier)((RetailDivision)listItem.Tag).ID)
                {
                    listItem.Selected = true;
                }
            }

            lvItems.Columns[lvItems.SortColumn].ImageIndex = (lvItems.SortedBackwards ? 1 : 0);

            lvItems_SelectedIndexChanged(this, EventArgs.Empty);

            lvItems.BestFitColumns();
        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled =
                lvItems.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailDivisions);
            if (lvItems.SelectedItems.Count > 0)
            {
                tabSheetTabs.Visible = true;
                groupPanelNoSelection.Visible = false;
                lblNoSelection.Visible = false;
                LoadTab(true);
            }
            else
            {
                tabSheetTabs.Visible = false;
                groupPanelNoSelection.Visible = true;
                lblNoSelection.Visible = true;
            }
        }

        private void LoadTab(bool isRevert)
        {
            RetailDivision retailDivision;
            if (lvItems.SelectedItems.Count > 0)
            {
                retailDivision = (RetailDivision)lvItems.SelectedItems[0].Tag;
            }
            else
            {
                retailDivision = new RetailDivision();
            }
            selectedID = (RecordIdentifier)retailDivision.ID;
            tabSheetTabs.SetData(isRevert, selectedID, retailDivision);
        }

        private void lvItems_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column > 2)
            {
                return;
            }

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
            PluginOperations.NewRetailDivision(this, EventArgs.Empty);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.EditRetailDivision((RecordIdentifier)((RetailDivision)lvItems.SelectedItems[0].Tag).ID);
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
                Properties.Resources.DeleteRetailDivisionQuestion,
                Properties.Resources.DeleteRetailDivision) == DialogResult.Yes)
            {
                PluginOperations.DeleteRetailDivision((RecordIdentifier)((RetailDivision)lvItems.SelectedItems[0].Tag).MasterID);

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "RetailDivision", null, null);
            }
        }

        void lvItems_Opening(object sender, CancelEventArgs e)
        {
            var menu = lvItems.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
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

            PluginEntry.Framework.ContextMenuNotify("RetailDivisionList", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void CopyID(object sender, EventArgs args)
        {
            Clipboard.SetText(lvItems.SelectedItems[0].Text);
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

    }
}
