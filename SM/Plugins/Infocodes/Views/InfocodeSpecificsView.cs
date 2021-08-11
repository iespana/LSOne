using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LSRetail.StoreController.SharedCore;
using LSRetail.Utilities.DataTypes;
using LSRetail.StoreController.SharedCore.Enums;
using LSRetail.StoreController.SharedDatabase.DataEntities;
using LSRetail.StoreController.BusinessObjects.Dimensions;
using LSRetail.StoreController.BusinessObjects;
using LSRetail.StoreController.BusinessObjects.DataEntities;
using LSRetail.StoreController.BusinessObjects.DataEntities.Infocodes;
using LSRetail.StoreController.BusinessObjects.Infocodes;

namespace LSRetail.StoreController.Infocodes.Views
{
    public partial class InfocodeSpecificsView : ViewBase
    {
        private RecordIdentifier selectedID = "";

        public InfocodeSpecificsView(RecordIdentifier infocodeID)
            : this()
        {
            selectedID = infocodeID;
        }

        public InfocodeSpecificsView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                         ViewAttributes.ContextBar |
                         ViewAttributes.Close |
                         ViewAttributes.Help;

            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += new CancelEventHandler(lvItems_Opening);

            lvItems.SmallImageList = PluginEntry.Framework.GetImageList();

            lvItems.SortColumn = 0;
            lvItems.SortedBackwards = false;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.InfocodeEdit);
            btnAdd.Enabled = PluginEntry.DataModel.HasPermission(Permission.InfocodeEdit);
            btnEdit.Enabled  = PluginEntry.DataModel.HasPermission(Permission.InfocodeEdit);
            btnRemove.Enabled = PluginEntry.DataModel.HasPermission(Permission.InfocodeEdit);
        }

        void lvItems_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuitem item;
            ContextMenuStrip menu;

            menu = lvItems.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right click here
            item = new ExtendedMenuitem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(btnEdit_Click));

            item.Image = Properties.Resources.EditImage;
            item.Enabled = btnEdit.Enabled;
            item.Default = true;

            menu.Items.Add(item);

            item = new ExtendedMenuitem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnAdd_Click));

            item.Enabled = btnAdd.Enabled;

            item.Image = Properties.Resources.PlusImage;

            menu.Items.Add(item);

            item = new ExtendedMenuitem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnRemove_Click));

            item.Image = Properties.Resources.MinusImage;
            item.Enabled = btnRemove.Enabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("InfocodeList", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        public override Utilities.DataTypes.RecordIdentifier ID
        {
            get
            {
                return Utilities.DataTypes.RecordIdentifier.Empty;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadItems(0, false, true, selectedID);
        }

        private void LoadItems(int sortBy, bool backwards, bool doBestFit, RecordIdentifier idToSelect)
        {
            List<Infocode> items;
            ListViewItem listItem;

            lvItems.Items.Clear();

            items = Providers.InfocodeData.GetInfocodes(PluginEntry.DataModel, new UsageCategories[]{ UsageCategories.None});

            foreach (DataEntity item in items)
            {
                listItem = new ListViewItem((string)item.ID);
                listItem.SubItems.Add(item.Text);

                listItem.Tag = item.ID;
                listItem.ImageIndex = -1;

                lvItems.Add(listItem);

                if (idToSelect == (RecordIdentifier)listItem.Tag)
                {
                    listItem.Selected = true;
                }
            }

            lvItems.Columns[sortBy].ImageIndex = (backwards ? 1 : 0);
            lvItems.SortColumn = sortBy;

            //lvGroups_SelectedIndexChanged(this, EventArgs.Empty);

            if (doBestFit)
            {
                lvItems.BestFitColumns();
            }
        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedID = (lvItems.SelectedItems.Count > 0) ? (RecordIdentifier)lvItems.SelectedItems[0].Tag : "";
            //btnEdit.Enabled = (lvGroups.SelectedItems.Count != 0) && PluginEntry.DataModel.HasPermission(Permission.StylesEdit);
            btnRemove.Enabled = btnEdit.Enabled;
            
        }

        private RecordIdentifier SelectedGroupID
        {
            get { return (RecordIdentifier)lvItems.SelectedItems[0].Tag; }
        }

        private RecordIdentifier SelectedItemID
        {
            get { return (RecordIdentifier)lvItems.SelectedItems[0].Tag; }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PluginOperations.NewInfocode(UsageCategories.None);
            LoadItems(lvItems.SortColumn, lvItems.SortedBackwards, true, selectedID);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowInfocode((RecordIdentifier)SelectedGroupID);
            LoadItems(lvItems.SortColumn, lvItems.SortedBackwards, true, selectedID);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (LSRetail.StoreController.SharedCore.Dialogs.QuestionDialog.Show(
                Properties.Resources.DeleteInfocodeQuestion,
                Properties.Resources.DeleteInfocodeCaption) == DialogResult.Yes)
            {
                Providers.InfocodeData.Delete(PluginEntry.DataModel, SelectedGroupID);
                LoadItems(lvItems.SortColumn, lvItems.SortedBackwards, true, selectedID);
            }
        }

        private void lvItems_DoubleClick(object sender, EventArgs e)
        {
            PluginOperations.ShowInfocode((RecordIdentifier)SelectedGroupID);
        }

        private void lvItems_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // We only want to sort based on the first column.
            if (e.Column > 0)
            {
                return;
            }

            if (lvItems.SortColumn == e.Column)
            {
                lvItems.SortedBackwards = !lvItems.SortedBackwards;
            }
            else
            {
                lvItems.SortedBackwards = false;
            }
            
            if (lvItems.SortColumn != -1)
            {
                lvItems.Columns[lvItems.SortColumn].ImageIndex = 2;
                lvItems.SortColumn = e.Column;

            }

            LoadItems(lvItems.SortColumn, lvItems.SortedBackwards, true, selectedID);
        }

        private void lvItems_VisibleChanged(object sender, EventArgs e)
        {
            if (lvItems.Visible)
            {
                LoadItems(lvItems.SortColumn, lvItems.SortedBackwards, true, selectedID);
            }
        }

    }
}
