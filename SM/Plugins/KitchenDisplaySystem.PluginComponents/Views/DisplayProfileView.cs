using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using static LSOne.DataLayer.KDSBusinessObjects.KitchenDisplayStation;


namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    public partial class DisplayProfileView : ViewBase
    {
        private KitchenDisplayProfile kitchenDisplayProfile;
        private RecordIdentifier selectedChitDisplayLineID = "";
        private RecordIdentifier selectedChitDisplayColumnID = "";

        private RecordIdentifier selectedLineDisplayLineID = "";
        private RecordIdentifier selectedLineDisplayColumnID = "";

        public DisplayProfileView(RecordIdentifier profileId)
            : this()
        {
            kitchenDisplayProfile = Providers.KitchenDisplayProfileData.Get(PluginEntry.DataModel, profileId);

            lvChitDisplayLines.ContextMenuStrip = new ContextMenuStrip();
            lvChitDisplayLines.ContextMenuStrip.Opening += ContextChitDisplayLines_Opening;

            lvChitDisplayColumns.ContextMenuStrip = new ContextMenuStrip();
            lvChitDisplayColumns.ContextMenuStrip.Opening += ContextChitDisplayColumns_Opening;

            lvChitDisplayLines.SortColumn = 1;
            lvChitDisplayLines.SortedBackwards = false;

            lvLineDisplayColumns.ContextMenuStrip = new ContextMenuStrip();
            lvLineDisplayColumns.ContextMenuStrip.Opening += ContextLineDisplayColumns_Opening;

            btnsEditAddRemoveChitDisplayLines.AddButtonEnabled 
                = btnsEditAddRemoveLineDisplayColumns.AddButtonEnabled
                = PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles);
        }

        private DisplayProfileView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles);

            foreach (var mode in Enum.GetValues(typeof(DisplayModeEnum)))
            {
                cmbDisplayMode.Items.Add(GetDisplayModeText((DisplayModeEnum)mode));
            }
        }

        public string Description
        {
            get
            {
                return Properties.Resources.DisplayProfile + ": " + kitchenDisplayProfile.Text;
            }
        }

        public override string ContextDescription
        {
            get
            {
                return kitchenDisplayProfile.Text;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.DisplayProfile + ": " + kitchenDisplayProfile.Text;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return kitchenDisplayProfile.ID;
            }
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "KitchenDisplayLine":
                    if (changeAction == DataEntityChangeType.Delete)
                    {
                        LoadChitDisplayLines(true);
                    }
                    break;

                case "KitchenDisplayLineColumn":
                    if (changeAction == DataEntityChangeType.Delete)
                    {
                        LoadChitDisplayColumns(true, selectedChitDisplayLineID);
                        LoadLineDisplayColumns(true);
                    }
                    break;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            tbProfileName.Text = kitchenDisplayProfile.Text;
            cmbDisplayMode.SelectedIndex = (int)kitchenDisplayProfile.DisplayMode;
            selectedLineDisplayLineID = Providers.KitchenDisplayLineData.GetList(PluginEntry.DataModel, kitchenDisplayProfile.ID).Find(i => i.Type == DisplayRowTypeEnum.LineDisplayRow)?.ID;
            HeaderText = Description;

            LoadChitDisplayLines(true);
            LoadLineDisplayColumns(true);
        }

        private void LoadChitDisplayLines(bool doBestFit)
        {
            List<KitchenDisplayLine> kitchenDisplayLines;
            ListViewItem listItem;

            lvChitDisplayLines.Items.Clear();

            kitchenDisplayLines = Providers.KitchenDisplayLineData.GetList(PluginEntry.DataModel, kitchenDisplayProfile.ID);
            foreach (KitchenDisplayLine displayLine in kitchenDisplayLines)
            {
                if (displayLine.Type == DisplayRowTypeEnum.LineDisplayRow) continue;

                listItem = new ListViewItem(displayLine.No.ToString());
                listItem.ImageIndex = -1;
                listItem.Tag = displayLine;
                listItem.SubItems.Add(displayLine.GetTypeText());
                lvChitDisplayLines.Add(listItem);

                if (selectedChitDisplayLineID == displayLine.ID)
                {
                    listItem.Selected = true;
                }
            }

            if (doBestFit)
            {
                lvChitDisplayLines.BestFitColumns();
            }

            lvChitDisplayLines_SelectedIndexChanged(null, null);
            lvChitDisplayLines.ShowSelectedItem();
        }

        private void LoadChitDisplayColumns(bool doBestFit, RecordIdentifier lineID)
        {
            ListViewItem listItem;

            if (lvChitDisplayLines.SelectedItems.Count == 0)
            {
                return;
            }

            lvChitDisplayColumns.Items.Clear();

            List<KitchenDisplayLineColumn> kitchenDisplayLineColumns = Providers.KitchenDisplayLineColumnData.GetList(PluginEntry.DataModel, lineID);

            foreach (KitchenDisplayLineColumn column in kitchenDisplayLineColumns)
            {
                listItem = new ListViewItem(column.No.ToString());
                KitchenDisplayColumnField fieldData = new KitchenDisplayColumnField(column);
                listItem.SubItems.Add(fieldData.GetDataTypeName());
                listItem.SubItems.Add(fieldData.Text);
                listItem.SubItems.Add(column.GetAlignmentText());
                listItem.SubItems.Add(column.RelativeSize.ToString());
                listItem.SubItems.Add(column.StyleDescription);

                listItem.Tag = column;
                listItem.ImageIndex = -1;

                lvChitDisplayColumns.Add(listItem);

                if (selectedChitDisplayColumnID == column.ID)
                {
                    listItem.Selected = true;
                }
            }

            lvChitDisplayColumns_SelectedIndexChanged(this, EventArgs.Empty);

            if (doBestFit)
            {
                lvChitDisplayColumns.BestFitColumns();
            }
        }

        private void LoadLineDisplayColumns(bool doBestFit)
        {
            ListViewItem listItem;

            lvLineDisplayColumns.Items.Clear();

            List<KitchenDisplayLineColumn> kitchenDisplayLineColumns = Providers.KitchenDisplayLineColumnData.GetList(PluginEntry.DataModel, selectedLineDisplayLineID);

            foreach (KitchenDisplayLineColumn column in kitchenDisplayLineColumns)
            {
                listItem = new ListViewItem(column.No.ToString());
                KitchenDisplayColumnField fieldData = new KitchenDisplayColumnField(column);
                listItem.SubItems.Add(fieldData.GetDataTypeName());
                listItem.SubItems.Add(fieldData.Text);
                listItem.SubItems.Add(column.Caption);
                listItem.SubItems.Add(column.GetAlignmentText());
                listItem.SubItems.Add(column.RelativeSize.ToString());
                listItem.SubItems.Add(column.StyleDescription);

                listItem.Tag = column;
                listItem.ImageIndex = -1;

                lvLineDisplayColumns.Add(listItem);

                if (selectedLineDisplayColumnID == column.ID)
                {
                    listItem.Selected = true;
                }
            }

            lvLineDisplayColumns_SelectedIndexChanged(this, EventArgs.Empty);

            if (doBestFit)
            {
                lvLineDisplayColumns.BestFitColumns();
            }
        }

        private void lvChitDisplayLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedChitDisplayLineID = lvChitDisplayLines.SelectedItems.Count > 0 ? ((KitchenDisplayLine)lvChitDisplayLines.SelectedItems[0].Tag).ID : "";

            if (lvChitDisplayLines.SelectedItems.Count > 0)
            {
                btnsEditAddRemoveChitDisplayLines.EditButtonEnabled = true;
                btnsEditAddRemoveChitDisplayLines.RemoveButtonEnabled = true;

                btnChitMoveDown.Visible = true;
                btnChitMoveUp.Visible = true;

                LoadChitDisplayColumns(true, selectedChitDisplayLineID);
            }
            else
            {
                btnsEditAddRemoveChitDisplayLines.EditButtonEnabled = false;
                btnsEditAddRemoveChitDisplayLines.RemoveButtonEnabled = false;

                btnsEditAddRemoveChitDisplayColumns.EditButtonEnabled = false;
                btnsEditAddRemoveChitDisplayColumns.AddButtonEnabled = false;
                btnsEditAddRemoveChitDisplayColumns.RemoveButtonEnabled = false;

                btnChitMoveDown.Visible = false;
                btnChitMoveUp.Visible = false;

                lvChitDisplayColumns.Items.Clear();
            }
        }

        private void lvChitDisplayColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedChitDisplayColumnID = (lvChitDisplayColumns.SelectedItems.Count > 0) ? ((KitchenDisplayLineColumn)(lvChitDisplayColumns.SelectedItems[0].Tag)).ID : "";
            btnsEditAddRemoveChitDisplayColumns.EditButtonEnabled = lvChitDisplayColumns.SelectedItems.Count > 0;
            btnsEditAddRemoveChitDisplayColumns.AddButtonEnabled = lvChitDisplayLines.SelectedItems.Count > 0;
            btnsEditAddRemoveChitDisplayColumns.RemoveButtonEnabled = lvChitDisplayColumns.SelectedItems.Count > 0;

            if (lvChitDisplayColumns.SelectedItems.Count == 0 || lvChitDisplayColumns.Items.Count <= 1)
            {
                btnChitMoveUp.Enabled = false;
                btnChitMoveDown.Enabled = false;
                return;
            }

            int currItemIndex = lvChitDisplayColumns.SelectedIndices[0];
            if (currItemIndex == 0)
            {
                btnChitMoveUp.Enabled = false;
                btnChitMoveDown.Enabled = true;
            }
            else if (currItemIndex == lvChitDisplayColumns.Items.Count - 1)
            {
                btnChitMoveUp.Enabled = true;
                btnChitMoveDown.Enabled = false;
            }
            else
            {
                btnChitMoveUp.Enabled = true;
                btnChitMoveDown.Enabled = true;
            }
        }

        private void lvLineDisplayColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedLineDisplayColumnID = (lvLineDisplayColumns.SelectedItems.Count > 0) ? ((KitchenDisplayLineColumn)(lvLineDisplayColumns.SelectedItems[0].Tag)).ID : "";
            btnsEditAddRemoveLineDisplayColumns.EditButtonEnabled = lvLineDisplayColumns.SelectedItems.Count > 0;
            btnsEditAddRemoveLineDisplayColumns.RemoveButtonEnabled = lvLineDisplayColumns.SelectedItems.Count > 0;

            if (lvLineDisplayColumns.SelectedItems.Count == 0 || lvLineDisplayColumns.Items.Count <= 1)
            {
                btnLineMoveUp.Enabled = false;
                btnLineMoveDown.Enabled = false;
                return;
            }

            int currItemIndex = lvLineDisplayColumns.SelectedIndices[0];
            if (currItemIndex == 0)
            {
                btnLineMoveUp.Enabled = false;
                btnLineMoveDown.Enabled = true;
            }
            else if (currItemIndex == lvLineDisplayColumns.Items.Count - 1)
            {
                btnLineMoveUp.Enabled = true;
                btnLineMoveDown.Enabled = false;
            }
            else
            {
                btnLineMoveUp.Enabled = true;
                btnLineMoveDown.Enabled = true;
            }
        }

        private void lvLineDisplayColumns_DoubleClick(object sender, EventArgs e)
        {
            if (lvLineDisplayColumns.SelectedItems.Count > 0 && btnsEditAddRemoveLineDisplayColumns.EditButtonEnabled)
            {
                btnsEditAddRemoveLineDisplayColumns_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void lvChitDisplayColumns_DoubleClick(object sender, EventArgs e)
        {
            if (lvChitDisplayColumns.SelectedItems.Count > 0 && btnsEditAddRemoveChitDisplayColumns.EditButtonEnabled)
            {
                btnsEditAddRemoveChitDisplayColumns_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void btnsEditAddRemoveChitDisplayLines_EditButtonClicked(object sender, EventArgs e)
        {
            var dlg = new Dialogs.DisplayLineDialog(selectedChitDisplayLineID, kitchenDisplayProfile);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadChitDisplayLines(true);
            }
        }

        private void btnsEditAddRemoveChitDisplayLines_AddButtonClicked(object sender, EventArgs e)
        {
            var dlg = new Dialogs.DisplayLineDialog(null, kitchenDisplayProfile);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedChitDisplayLineID = dlg.ID;
                LoadChitDisplayLines(true);
            }
        }

        private void btnsEditAddRemoveChitDisplayColumns_EditButtonClicked(object sender, EventArgs e)
        {
            var dlg = new Dialogs.DisplayLineColumnsDialog(selectedChitDisplayColumnID, selectedChitDisplayLineID, DisplayModeEnum.ChitDisplay);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadChitDisplayLines(true);
            }
        }

        private void btnsEditAddRemoveChitDisplayColumns_AddButtonClicked(object sender, EventArgs e)
        {
            var dlg = new Dialogs.DisplayLineColumnsDialog(null, selectedChitDisplayLineID, DisplayModeEnum.ChitDisplay);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadChitDisplayColumns(true, selectedChitDisplayLineID);
            }
        }

        private void btnsEditAddRemoveChitDisplayLines_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperationsHelper.DeleteDisplayLine(selectedChitDisplayLineID);
        }

        private void btnsEditAddRemoveChitDisplayColumns_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperationsHelper.DeleteDisplayLineColumn(selectedChitDisplayColumnID);
        }

        private void btnsEditAddRemoveLineDisplayColumns_EditButtonClicked(object sender, EventArgs e)
        {
            var dlg = new Dialogs.DisplayLineColumnsDialog(selectedLineDisplayColumnID, selectedLineDisplayLineID, DisplayModeEnum.LineDisplay);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadLineDisplayColumns(true);
            }
        }

        private void btnsEditAddRemoveLineDisplayColumns_AddButtonClicked(object sender, EventArgs e)
        {
            var dlg = new Dialogs.DisplayLineColumnsDialog(null, selectedLineDisplayLineID, DisplayModeEnum.LineDisplay);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadLineDisplayColumns(true);
            }
        }

        private void btnsEditAddRemoveLineDisplayColumns_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperationsHelper.DeleteDisplayLineColumn(selectedLineDisplayColumnID);
        }

        void ContextChitDisplayLines_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvChitDisplayLines.ContextMenuStrip;
            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                Properties.Resources.Add,
                200,
                new EventHandler(btnsEditAddRemoveChitDisplayLines_AddButtonClicked));
            item.Enabled = btnsEditAddRemoveChitDisplayLines.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Delete,
                300,
                new EventHandler(btnsEditAddRemoveChitDisplayLines_RemoveButtonClicked));
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemoveChitDisplayLines.RemoveButtonEnabled;
            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void ContextChitDisplayColumns_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvChitDisplayColumns.ContextMenuStrip;
            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                Properties.Resources.Edit,
                100,
                btnsEditAddRemoveChitDisplayColumns_EditButtonClicked);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsEditAddRemoveChitDisplayColumns.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Add,
                200,
                new EventHandler(btnsEditAddRemoveChitDisplayColumns_AddButtonClicked));
            item.Enabled = btnsEditAddRemoveChitDisplayColumns.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Delete,
                300,
                new EventHandler(btnsEditAddRemoveChitDisplayColumns_RemoveButtonClicked));
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemoveChitDisplayColumns.RemoveButtonEnabled;
            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 400));

            item = new ExtendedMenuItem(
                    Properties.Resources.MoveUp,
                    500,
                    new EventHandler(btnChitMoveUp_Click))
            {
                Enabled = btnChitMoveUp.Enabled,
                Image = ContextButtons.GetMoveUpButtonImage()
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.MoveDown,
                    600,
                    new EventHandler(btnChitMoveDown_Click))
            {
                Enabled = btnChitMoveDown.Enabled,
                Image = ContextButtons.GetMoveDownButtonImage()
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("ChitDisplayColumns", lvChitDisplayColumns.ContextMenuStrip, lvChitDisplayColumns);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void ContextLineDisplayColumns_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvLineDisplayColumns.ContextMenuStrip;
            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                Properties.Resources.Edit,
                100,
                btnsEditAddRemoveLineDisplayColumns_EditButtonClicked);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsEditAddRemoveLineDisplayColumns.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Add,
                200,
                new EventHandler(btnsEditAddRemoveLineDisplayColumns_AddButtonClicked));
            item.Enabled = btnsEditAddRemoveLineDisplayColumns.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Delete,
                300,
                new EventHandler(btnsEditAddRemoveLineDisplayColumns_RemoveButtonClicked));
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemoveLineDisplayColumns.RemoveButtonEnabled;
            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 400));

            item = new ExtendedMenuItem(
                    Properties.Resources.MoveUp,
                    500,
                    new EventHandler(btnLineMoveUp_Click))
            {
                Enabled = btnLineMoveUp.Enabled,
                Image = ContextButtons.GetMoveUpButtonImage()
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.MoveDown,
                    600,
                    new EventHandler(btnLineMoveDown_Click))
            {
                Enabled = btnLineMoveDown.Enabled,
                Image = ContextButtons.GetMoveDownButtonImage()
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("LineDisplayColumns", lvLineDisplayColumns.ContextMenuStrip, lvLineDisplayColumns);

            e.Cancel = (menu.Items.Count == 0);
        }

        protected override bool DataIsModified()
        {
            if (tbProfileName.Text != kitchenDisplayProfile.Text) return true;
            if (cmbDisplayMode.SelectedIndex != (int)kitchenDisplayProfile.DisplayMode) return true;

            return false;
        }

        protected override bool SaveData()
        {
            kitchenDisplayProfile.Text = tbProfileName.Text;
            kitchenDisplayProfile.DisplayMode = (DisplayModeEnum)cmbDisplayMode.SelectedIndex;

            Providers.KitchenDisplayProfileData.Save(PluginEntry.DataModel, kitchenDisplayProfile);
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "KitchenDisplayProfile", kitchenDisplayProfile.ID, null);

            return true;
        }

        protected override void OnDelete()
        {
            PluginOperationsHelper.DeleteDisplayProfile(kitchenDisplayProfile.ID);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (PluginEntry.Framework.CanRunOperation("UIStyles"))
            {
                if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
                {
                    if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
                    {
                        arguments.Add(new ContextBarItem(
                            Properties.Resources.Styles,
                            null,
                            PluginOperationsHelper.ShowStylesView), 100);
                    }
                }
            }
        }

        private void cmbDisplayMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch((DisplayModeEnum)cmbDisplayMode.SelectedIndex)
            {
                case DisplayModeEnum.ChitDisplay:
                    pnlChit.Visible = true;
                    pnlLine.Visible = false;
                    break;
                case DisplayModeEnum.LineDisplay:
                    pnlChit.Visible = false;
                    pnlLine.Visible = true;
                    break;
            }
        }

        private void btnChitMoveUp_Click(object sender, EventArgs e)
        {
            KitchenDisplayLineColumn selectedColumn = (KitchenDisplayLineColumn)lvChitDisplayColumns.SelectedItems[0].Tag;
            KitchenDisplayLineColumn upperColum = (KitchenDisplayLineColumn)lvChitDisplayColumns.Items[lvChitDisplayColumns.SelectedIndices[0] - 1].Tag;

            int selectedNumber = selectedColumn.No;
            selectedColumn.No = upperColum.No;
            upperColum.No = selectedNumber;

            Providers.KitchenDisplayLineColumnData.SaveOrder(PluginEntry.DataModel, selectedColumn);
            Providers.KitchenDisplayLineColumnData.SaveOrder(PluginEntry.DataModel, upperColum);

            selectedChitDisplayColumnID = selectedColumn.ID;

            LoadChitDisplayColumns(true, selectedChitDisplayLineID);
        }

        private void btnChitMoveDown_Click(object sender, EventArgs e)
        {
            KitchenDisplayLineColumn selectedColumn = (KitchenDisplayLineColumn)lvChitDisplayColumns.SelectedItems[0].Tag;
            KitchenDisplayLineColumn lowerColumn = (KitchenDisplayLineColumn)lvChitDisplayColumns.Items[lvChitDisplayColumns.SelectedIndices[lvChitDisplayColumns.SelectedIndices.Count - 1] + 1].Tag;

            int selectedNumber = selectedColumn.No;
            selectedColumn.No = lowerColumn.No;
            lowerColumn.No = selectedNumber;

            Providers.KitchenDisplayLineColumnData.SaveOrder(PluginEntry.DataModel, selectedColumn);
            Providers.KitchenDisplayLineColumnData.SaveOrder(PluginEntry.DataModel, lowerColumn);

            selectedChitDisplayColumnID = selectedColumn.ID;

            LoadChitDisplayColumns(true, selectedChitDisplayLineID);
        }

        private void btnLineMoveUp_Click(object sender, EventArgs e)
        {
            KitchenDisplayLineColumn selectedColumn = (KitchenDisplayLineColumn)lvLineDisplayColumns.SelectedItems[0].Tag;
            KitchenDisplayLineColumn upperColum = (KitchenDisplayLineColumn)lvLineDisplayColumns.Items[lvLineDisplayColumns.SelectedIndices[0] - 1].Tag;

            int selectedNumber = selectedColumn.No;
            selectedColumn.No = upperColum.No;
            upperColum.No = selectedNumber;

            Providers.KitchenDisplayLineColumnData.SaveOrder(PluginEntry.DataModel, selectedColumn);
            Providers.KitchenDisplayLineColumnData.SaveOrder(PluginEntry.DataModel, upperColum);

            selectedLineDisplayColumnID = selectedColumn.ID;

            LoadLineDisplayColumns(true);
        }

        private void btnLineMoveDown_Click(object sender, EventArgs e)
        {
            KitchenDisplayLineColumn selectedColumn = (KitchenDisplayLineColumn)lvLineDisplayColumns.SelectedItems[0].Tag;
            KitchenDisplayLineColumn lowerColumn = (KitchenDisplayLineColumn)lvLineDisplayColumns.Items[lvLineDisplayColumns.SelectedIndices[lvLineDisplayColumns.SelectedIndices.Count - 1] + 1].Tag;

            int selectedNumber = selectedColumn.No;
            selectedColumn.No = lowerColumn.No;
            lowerColumn.No = selectedNumber;

            Providers.KitchenDisplayLineColumnData.SaveOrder(PluginEntry.DataModel, selectedColumn);
            Providers.KitchenDisplayLineColumnData.SaveOrder(PluginEntry.DataModel, lowerColumn);

            selectedLineDisplayColumnID = selectedColumn.ID;

            LoadLineDisplayColumns(true);
        }
    }
}
