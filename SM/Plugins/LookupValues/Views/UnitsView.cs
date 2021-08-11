using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Units;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.LookupValues.Views
{
    public partial class UnitsView : ViewBase
    {
        private RecordIdentifier selectedID = "";

        public UnitsView(RecordIdentifier selectedID)
            : this()
        {
            this.selectedID = selectedID.ToString();
        }

        public UnitsView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help;

            lvGroups.ContextMenuStrip = new ContextMenuStrip();
            lvGroups.ContextMenuStrip.Opening += new CancelEventHandler(lvGroups_Opening);

            lvGroups.SmallImageList = PluginEntry.Framework.GetImageList();

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageUnits);

            lvGroups.Columns[0].Tag = UnitSorting.ID;
            lvGroups.Columns[1].Tag = UnitSorting.Description;
            lvGroups.Columns[2].Tag = UnitSorting.MinimumDecimals;
            lvGroups.Columns[3].Tag = UnitSorting.MaximumDecimals;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("Units", RecordIdentifier.Empty, Properties.Resources.Units, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.Units;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return RecordIdentifier.Empty;
	        }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType().ToString() + ".Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.UnitConversions, new ContextbarClickEventHandler(PluginOperations.ShowUnitConversionsView)), 10);

                PluginEntry.Framework.FindImplementor(this, "CanInsertDefaultData", arguments);
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (isRevert)
            {
                
            }

            HeaderText = Properties.Resources.Units;
            //HeaderIcon = Properties.Resources.Units16;

            LoadRecords();

            
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Dialogs.UnitDialog dlg = new Dialogs.UnitDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedID = dlg.UnitID;
                LoadRecords();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Dialogs.UnitDialog dlg = new Dialogs.UnitDialog((RecordIdentifier)lvGroups.SelectedItems[0].Tag);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadRecords();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteUnitQuestion,
                Properties.Resources.DeleteUnit) == DialogResult.Yes)
            {
                var deletedSuccessful = Providers.UnitData.Delete(PluginEntry.DataModel, (RecordIdentifier)lvGroups.SelectedItems[0].Tag);

                if (!deletedSuccessful)
                {
                    MessageDialog.Show(Properties.Resources.CannotDeleteUnit, Properties.Resources.ErrorDeletingUnit,
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    LoadRecords(); 
                }
            }

        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "DefaultData":
                case "Units":
                    LoadRecords();
                    break;
            }
        }

        private void lvItems_SizeChanged(object sender, EventArgs e)
        {
            lvGroups.Columns[1].Width = -2;
        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedID = (lvGroups.SelectedItems.Count > 0) ? (RecordIdentifier)lvGroups.SelectedItems[0].Tag : "";
            btnsContextButtons.EditButtonEnabled = (lvGroups.SelectedItems.Count != 0) && PluginEntry.DataModel.HasPermission(Permission.ManageUnits); ;
            btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled;
        }

        private void lvItems_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        void lvGroups_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvGroups.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
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
            item.Enabled = (lvGroups.SelectedItems.Count == 1);
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("UnitsList", lvGroups.ContextMenuStrip, lvGroups);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void CopyID(object sender, EventArgs args)
        {
            Clipboard.SetText(lvGroups.SelectedItems[0].Text);
        }

        private void LoadRecords()
        {
            List<Unit> units;
            ListViewItem item;

            lvGroups.Items.Clear();

            units = Providers.UnitData.GetUnits(PluginEntry.DataModel, (UnitSorting)lvGroups.Columns[lvGroups.SortColumn].Tag, lvGroups.SortedBackwards);

            foreach (Unit unit in units)
            {
                item = new ListViewItem((string)unit.ID);
                item.SubItems.Add(unit.Text);
                item.SubItems.Add(unit.MinimumDecimals.ToString());
                item.SubItems.Add(unit.MaximumDecimals.ToString());
                item.Tag = unit.ID;
                item.ImageIndex = -1;

                lvGroups.Add(item);

                if (selectedID == (RecordIdentifier)item.Tag)
                {
                    item.Selected = true;
                }
            }

            lvGroups.Columns[lvGroups.SortColumn].ImageIndex = (lvGroups.SortedBackwards ? 1 : 0);

            lvGroups.BestFitColumns();

            lvItems_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void lvGroups_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvGroups.SortColumn == e.Column)
            {
                lvGroups.SortedBackwards = !lvGroups.SortedBackwards;
            }
            else
            {
                if (lvGroups.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvGroups.Columns[lvGroups.SortColumn].ImageIndex = 2;

                    lvGroups.SortColumn = e.Column;
                }
                lvGroups.SortedBackwards = false;
            }

            LoadRecords();
        }

        protected override void OnClose()
        {
            lvGroups.SmallImageList = null;

            base.OnClose();
        }



 
    }
}
