using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Customer.Views
{
    public partial class CategoriesView : ViewBase
    {
        private int selectedRow;
        
        private GroupCategory selectedCategory = new GroupCategory();

        public CategoriesView(RecordIdentifier categoryID)
            : this()
        {
            selectedCategory.ID = categoryID;
        }

        public CategoriesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.ContextBar
                | ViewAttributes.Close
                | ViewAttributes.Help;

            HeaderText = Properties.Resources.GroupCategories;

            colDescription.HeaderText = Properties.Resources.CategoryDescription;

            selectedRow = -1;

            lvCategories.ContextMenuStrip = new ContextMenuStrip();
            lvCategories.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            colDescription.HeaderText = Properties.Resources.CategoryDescription;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.CategoriesEdit);
            btnsEditAddRemove.AddButtonEnabled = !ReadOnly;
        }

        void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ContextMenuStrip menu = lvCategories.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.EditString,
                    100,
                    btnsEditAddRemove_EditButtonClicked)
            {
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

            PluginEntry.Framework.ContextMenuNotify(NotifyContexts.CategoriesViewListContextMenu, lvCategories.ContextMenuStrip, lvCategories);

            e.Cancel = (menu.Items.Count == 0);
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case NotifyContexts.CategoriesView:
                    if (changeAction == DataEntityChangeType.Edit || changeAction == DataEntityChangeType.Add)
                    {
                        selectedCategory = (GroupCategory)param;
                    }
                    LoadItems();
                    break;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.GroupCategories;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return selectedCategory.ID;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            LoadItems();
        }

        private void LoadItems()
        {

            lvCategories.Rows.Clear();

            List<GroupCategory> categories = Providers.GroupCategoryData.GetList(PluginEntry.DataModel);

            int rowSelect = selectedRow;

            foreach (GroupCategory cat in categories)
            {
                Row row = new Row();
                row.AddText(cat.Text);
                row.Tag = cat;
                lvCategories.AddRow(row);

                if (selectedCategory.ID != RecordIdentifier.Empty && cat.ID == selectedCategory.ID)
                {
                    rowSelect = lvCategories.RowCount-1;
                }
            }

            if (rowSelect > -1)
            {
                rowSelect = rowSelect > lvCategories.RowCount - 1 ? lvCategories.RowCount - 1 : rowSelect;
                lvCategories.Selection.Set(rowSelect);
            }

            lvCategories.Sort();

            lvCategories_SelectionChanged(this, EventArgs.Empty);

            lvCategories.AutoSizeColumns();
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.EditCategory(new GroupCategory());
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.EditCategory(selectedCategory);
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.DeleteCategory(selectedCategory.ID);
        }

        private void lvCategories_SelectionChanged(object sender, EventArgs e)
        {
            if (lvCategories.Selection.Count > 0)
            {
                selectedCategory = (GroupCategory)lvCategories.Row(lvCategories.Selection.FirstSelectedRow).Tag;
                selectedRow = lvCategories.Selection.FirstSelectedRow;
                btnsEditAddRemove.AddButtonEnabled = btnsEditAddRemove.RemoveButtonEnabled = btnsEditAddRemove.EditButtonEnabled = !ReadOnly;
            }
            else
            {
                selectedRow = -1;
                btnsEditAddRemove.AddButtonEnabled = !ReadOnly;
                btnsEditAddRemove.RemoveButtonEnabled = btnsEditAddRemove.EditButtonEnabled = false;
            }
        }
    }
}
