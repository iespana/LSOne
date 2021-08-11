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
    public partial class _ModifierInfocodesView : ViewBase
    {
        private RecordIdentifier selectedID = "";

        public _ModifierInfocodesView(RecordIdentifier infocodeId)
            : this()
        {
            selectedID = infocodeId;
        }

        public _ModifierInfocodesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                         ViewAttributes.ContextBar |
                         ViewAttributes.Close |
                         ViewAttributes.Help;

            //Infocode right click
            lvGroups.ContextMenuStrip = new ContextMenuStrip();
            lvGroups.ContextMenuStrip.Opening += new CancelEventHandler(lvGroups_Opening);

            //InfoSubcode right click
            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += new CancelEventHandler(lvItems_Opening);

            lvGroups.SmallImageList = PluginEntry.Framework.GetImageList();

            lvItems.SortColumn = 0;
            lvItems.SortedBackwards = false;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.InfocodeEdit);
            btnAdd.Enabled = btnAddItem.Enabled = PluginEntry.DataModel.HasPermission(Permission.InfocodeEdit);
            btnEdit.Enabled = btnEditItem.Enabled = PluginEntry.DataModel.HasPermission(Permission.InfocodeEdit);
            btnRemove.Enabled = btnRemoveItem.Enabled = PluginEntry.DataModel.HasPermission(Permission.InfocodeEdit);
        }

        void lvGroups_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuitem item;
            ContextMenuStrip menu;

            menu = lvGroups.ContextMenuStrip;

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

            PluginEntry.Framework.ContextMenuNotify("InfocodeList", lvGroups.ContextMenuStrip, lvGroups);

            e.Cancel = (menu.Items.Count == 0);
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
                    new EventHandler(btnEditItem_Click));

            item.Image = Properties.Resources.EditImage;
            item.Enabled = btnEditItem.Enabled;
            item.Default = true;

            menu.Items.Add(item);

            item = new ExtendedMenuitem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnAddItem_Click));

            item.Enabled = btnAddItem.Enabled;

            item.Image = Properties.Resources.PlusImage;

            menu.Items.Add(item);

            item = new ExtendedMenuitem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnRemoveItem_Click));

            item.Image = Properties.Resources.MinusImage;
            item.Enabled = btnRemoveItem.Enabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("InfocodeSubcodeList", lvItems.ContextMenuStrip, lvItems);

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

            lvGroups.Items.Clear();

            items = null; // Providers.InfocodeData.GetList(PluginEntry.DataModel, UsageCategories.ItemModifier);

            foreach (DataEntity item in items)
            {
                listItem = new ListViewItem((string)item.ID);
                listItem.SubItems.Add(item.Text);

                listItem.Tag = item.ID;
                listItem.ImageIndex = -1;

                lvGroups.Add(listItem);

                if (idToSelect == (RecordIdentifier)listItem.Tag)
                {
                    listItem.Selected = true;
                }
            }

            lvGroups.Columns[sortBy].ImageIndex = (backwards ? 1 : 0);
            lvGroups.SortColumn = sortBy;

            //lvGroups_SelectedIndexChanged(this, EventArgs.Empty);

            if (doBestFit)
            {
                lvGroups.BestFitColumns();
            }
        }

        private void lvGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedID = (lvGroups.SelectedItems.Count > 0) ? (RecordIdentifier)lvGroups.SelectedItems[0].Tag : "";
            //btnEdit.Enabled = (lvGroups.SelectedItems.Count != 0) && PluginEntry.DataModel.HasPermission(Permission.StylesEdit);
            btnRemove.Enabled = btnEdit.Enabled;
            ///
            if (lvGroups.SelectedItems.Count > 0)
            {
                if (lblGroupHeader.Visible == false)
                {
                    lblGroupHeader.Visible = true;
                    lvItems.Visible = true;
                    btnAddItem.Visible = true;
                    btnEditItem.Visible = true;
                    btnRemoveItem.Visible = true;
                    lblNoSelection.Visible = false;
                }

                LoadLines();
            }
            else if (lblGroupHeader.Visible == true)
            {
                lblGroupHeader.Visible = false;
                lvItems.Visible = false;
                btnAddItem.Visible = false;
                btnEditItem.Visible = false;
                btnRemoveItem.Visible = false;
                lblNoSelection.Visible = true;
            }
        }

        private void LoadLines()
        {
            List<InfocodeSubcode> items;
            ListViewItem listItem;

            if (lvGroups.SelectedItems.Count == 0)
            {
                return;
            }

            lvItems.Items.Clear();

            //items = Providers.InfocodeData.GetSubCodeLines(PluginEntry.DataModel, SelectedGroupID, "");
            items = Providers.InfocodeSubcodeData.GetListForInfocode(PluginEntry.DataModel, SelectedGroupID);

            foreach (InfocodeSubcode item in items)
            {
                listItem = new ListViewItem((string)item.SubcodeId);
                listItem.SubItems.Add(item.Text);
                listItem.SubItems.Add(item.TriggerFunction.ToString());
                listItem.SubItems.Add(item.TriggerCode.ToString());
                listItem.SubItems.Add(item.VariantCode.ToString());
                listItem.SubItems.Add(item.PriceHandling.ToString());
                listItem.SubItems.Add(item.PriceType.ToString());
                listItem.SubItems.Add(item.AmountPercent.ToString("N2"));
                listItem.SubItems.Add(item.QtyPerUnitOfMeasure.ToString("N2"));
                listItem.SubItems.Add(item.QtyLinkedToTriggerLine.ToString());
                listItem.SubItems.Add(item.MaxSelection.ToString());
                listItem.SubItems.Add(item.VariantNeeded.ToString());
                listItem.SubItems.Add(item.SerialLotNeeded.ToString());
                listItem.Tag = item.ID;
                listItem.ImageIndex = -1;

                lvItems.Add(listItem);
            }

            lvItems.BestFitColumns();
        }

        private RecordIdentifier SelectedGroupID
        {
            get { return (RecordIdentifier)lvGroups.SelectedItems[0].Tag; }
        }

        private RecordIdentifier SelectedItemID
        {
            get { return (RecordIdentifier)lvItems.SelectedItems[0].Tag; }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PluginOperations.NewInfocode(UsageCategories.ItemModifier);
            LoadItems(lvGroups.SortColumn, lvGroups.SortedBackwards, true, selectedID);
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (lvGroups.SelectedItems.Count > 0)
            {
                PluginOperations.NewSubcode(SelectedGroupID);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowCrossAndModifierInfocode((RecordIdentifier)SelectedGroupID);
            LoadItems(lvGroups.SortColumn, lvGroups.SortedBackwards, true, selectedID);
        }

        private void btnEditItem_Click(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count > 0)
            {
                PluginOperations.ShowSubcode((RecordIdentifier)SelectedItemID);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (LSRetail.StoreController.SharedCore.Dialogs.QuestionDialog.Show(
                Properties.Resources.DeleteInfocodeQuestion,
                Properties.Resources.DeleteInfocodeCaption) == DialogResult.Yes)
            {
                Providers.InfocodeData.Delete(PluginEntry.DataModel, SelectedGroupID);
                LoadItems(lvGroups.SortColumn, lvGroups.SortedBackwards, true, selectedID);
            }
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count > 0)
            {
                if (LSRetail.StoreController.SharedCore.Dialogs.QuestionDialog.Show(
                    Properties.Resources.DeleteInfocodeSubcodeQuestion,
                    Properties.Resources.DeleteInfocodeSubcodeCaption) == DialogResult.Yes)
                {
                    Providers.InfocodeSubcodeData.Delete(PluginEntry.DataModel, SelectedItemID);
                    lvItems.Items.Remove(lvItems.SelectedItems[0]);
                    lvItems_SelectedIndexChanged(this, EventArgs.Empty);
                }
            }
        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEdit.Enabled = btnRemove.Enabled = (lvItems.SelectedItems.Count > 0);
        }

        private void lvGroups_DoubleClick(object sender, EventArgs e)
        {
            PluginOperations.ShowCrossAndModifierInfocode((RecordIdentifier)SelectedGroupID);
        }

        private void lvItems_DoubleClick(object sender, EventArgs e)
        {
            PluginOperations.ShowSubcode((RecordIdentifier)SelectedItemID);
        }

        private void lvGroups_ColumnClick(object sender, ColumnClickEventArgs e)
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
                LoadLines();
            }
        }

        private void lvGroups_VisibleChanged(object sender, EventArgs e)
        {
            if (lvGroups.Visible)
            {
                LoadItems(lvGroups.SortColumn, lvGroups.SortedBackwards, true, selectedID);
            }
        }
    }
}
