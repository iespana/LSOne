using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;


namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    public partial class ButtonProfilesView : ViewBase
    {
        private RecordIdentifier selectedKitchenDisplayMenuHeaderID = "";
        private RecordIdentifier selectedKitchenDisplayMenuLineID = "";

        public ButtonProfilesView(RecordIdentifier kitchenDisplayMenuHeaderID)
            : this()
        {
            selectedKitchenDisplayMenuHeaderID = kitchenDisplayMenuHeaderID;
            HeaderText = Properties.Resources.ButtonProfiles;
        }

        public ButtonProfilesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            HeaderText = Properties.Resources.ButtonProfiles;

            lvKitchenDisplayMenuHeaders.SmallImageList = PluginEntry.Framework.GetImageList();
            lvKitchenDisplayMenuHeaders.ContextMenuStrip = new ContextMenuStrip();
            lvKitchenDisplayMenuHeaders.ContextMenuStrip.Opening += lvKitchenDisplayMenuHeaders_Opening;

            lvKitchenDisplayMenuHeaders.SortColumn = 1;
            lvKitchenDisplayMenuHeaders.SortedBackwards = false;

            lvKitchenDisplayMenuLines.ContextMenuStrip = new ContextMenuStrip();
            lvKitchenDisplayMenuLines.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            btnsEditAddRemovePosMenu.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.EditPosMenus);
            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.EditPosMenus);
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvKitchenDisplayMenuLines.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnsEditAddRemovePosMenuLine_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemovePosMenuLine.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   btnsEditAddRemovePosMenuLine_AddButtonClicked);

            item.Enabled = btnsEditAddRemovePosMenuLine.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnsEditAddRemovePosMenuLine_RemoveButtonClicked);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemovePosMenuLine.RemoveButtonEnabled;

            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 400));

            item = new ExtendedMenuItem(
                    Properties.Resources.MoveUp,
                    500,
                    new EventHandler(btnMoveUp_Click))
            {
                Enabled = btnMoveUp.Enabled,
                Image = ContextButtons.GetMoveUpButtonImage()
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.MoveDown,
                    600,
                    new EventHandler(btnMoveDown_Click))
            {
                Enabled = btnMoveDown.Enabled,
                Image = ContextButtons.GetMoveDownButtonImage()
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("KitchenDisplayButtonGridMenuLineList", lvKitchenDisplayMenuLines.ContextMenuStrip, lvKitchenDisplayMenuLines);

            e.Cancel = (menu.Items.Count == 0);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("KITCHENDISPLAYMENUHEADERLog", RecordIdentifier.Empty, Properties.Resources.ButtonProfiles, false));
            contexts.Add(new AuditDescriptor("PosMenuLines", RecordIdentifier.Empty, Properties.Resources.Buttons, false));
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.ButtonProfiles;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            LoadItems(PosMenuHeaderSorting.MenuID, false, true);
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "PosButtonGridMenuHeader":
                    if (changeHint == DataEntityChangeType.Edit || changeHint == DataEntityChangeType.Add)
                    {
                        selectedKitchenDisplayMenuHeaderID = changeIdentifier;
                    }

                    LoadData(false);

                    lvKitchenDisplayMenuHeaders.ShowSelectedItem();
                    lvKitchenDisplayMenuLines.ShowSelectedItem();

                    break;

                case "PosButtonGridMenuLine":
                    if (changeHint == DataEntityChangeType.Edit)
                    {
                        selectedKitchenDisplayMenuLineID = changeIdentifier;
                    }

                    LoadData(false);
                    break;
            }

        }

        private void LoadItems(PosMenuHeaderSorting sortBy, bool backwards, bool doBestFit)
        {
            List<PosMenuHeader> kitchenDisplayMenuHeaders;
            ListViewItem listItem;

            lvKitchenDisplayMenuHeaders.Items.Clear();
            PosMenuHeaderFilter filter = new PosMenuHeaderFilter()
            {
                MenuType = (int)MenuTypeEnum.KitchenDisplay,
                SortBackwards = backwards,
                SortBy = sortBy
            };
            kitchenDisplayMenuHeaders = Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel, filter);

            foreach (var kitchenDisplayMenuHeader in kitchenDisplayMenuHeaders)
            {
                listItem = new ListViewItem(kitchenDisplayMenuHeader.Text);
                int buttonCount = kitchenDisplayMenuHeader.Rows * kitchenDisplayMenuHeader.Columns;
                listItem.SubItems.Add(buttonCount.ToString());
                listItem.ImageIndex = -1;

                listItem.Tag = new RecordIdentifier(kitchenDisplayMenuHeader.ID, kitchenDisplayMenuHeader.Rows * kitchenDisplayMenuHeader.Columns);

                lvKitchenDisplayMenuHeaders.Add(listItem);

                if (selectedKitchenDisplayMenuHeaderID == ((RecordIdentifier)listItem.Tag).PrimaryID)
                {
                    listItem.Selected = true;
                }
            }

            lvKitchenDisplayMenuHeaders.Columns[lvKitchenDisplayMenuHeaders.SortColumn].ImageIndex = backwards ? 1 : 0;

            lvKitchenDisplayMenuHeaders_SelectedIndexChanged(this, EventArgs.Empty);

            if (doBestFit)
            {
                lvKitchenDisplayMenuHeaders.BestFitColumns();
            }
        }

        private void LoadLines(bool doBestFit)
        {
            ListViewItem listItem;

            if (lvKitchenDisplayMenuHeaders.SelectedItems.Count == 0)
            {
                return;
            }

            lvKitchenDisplayMenuLines.Items.Clear();

            List<PosMenuLineListItem> kitchenDisplayMenuLines = Providers.PosMenuLineData.GetListItems(PluginEntry.DataModel, selectedKitchenDisplayMenuHeaderID);

            foreach (PosMenuLineListItem line in kitchenDisplayMenuLines)
            {
                listItem = new ListViewItem(line.Text);
                listItem.SubItems.Add(DataLayer.KDSBusinessObjects.KitchenDisplayButton.GetButtonText((DataLayer.KDSBusinessObjects.KitchenDisplayButton.ButtonActionEnum)(int)line.Operation));
                listItem.SubItems.Add(line.KeyMapping.ToString());
                
                listItem.Tag = line;
                listItem.ImageIndex = -1;

                lvKitchenDisplayMenuLines.Add(listItem);

                if (selectedKitchenDisplayMenuLineID == line.ID)
                {
                    listItem.Selected = true;
                }
            }

            lvKitchenDisplayMenuLines_SelectedIndexChanged(this, EventArgs.Empty);

            if (doBestFit)
            {
                lvKitchenDisplayMenuLines.BestFitColumns();
            }
        }

        void lvKitchenDisplayMenuHeaders_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvKitchenDisplayMenuHeaders.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnsEditAddRemovePosMenu_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemovePosMenu.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   btnsEditAddRemovePosMenu_AddButtonClicked);

            item.Enabled = btnsEditAddRemovePosMenu.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnsEditAddRemovePosMenu_RemoveButtonClicked);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemovePosMenu.RemoveButtonEnabled;

            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 400));

            PluginEntry.Framework.ContextMenuNotify("KitchenDisplayButtonGridMenuHeaderList", lvKitchenDisplayMenuHeaders.ContextMenuStrip, lvKitchenDisplayMenuHeaders);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvKitchenDisplayMenuHeaders_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedKitchenDisplayMenuHeaderID = lvKitchenDisplayMenuHeaders.SelectedItems.Count > 0 ? ((RecordIdentifier)lvKitchenDisplayMenuHeaders.SelectedItems[0].Tag).PrimaryID : "";

            btnsEditAddRemovePosMenu.EditButtonEnabled = lvKitchenDisplayMenuHeaders.SelectedItems.Count == 1 && !ReadOnly;
            btnsEditAddRemovePosMenu.RemoveButtonEnabled = lvKitchenDisplayMenuHeaders.SelectedItems.Count > 0 && !ReadOnly;

            if (lvKitchenDisplayMenuHeaders.SelectedItems.Count > 0)
            {

                if (!lblGroupHeader.Visible)
                {
                    lblGroupHeader.Visible = true;
                    lvKitchenDisplayMenuLines.Visible = true;
                    btnsEditAddRemovePosMenuLine.EditButtonEnabled = true;
                    btnsEditAddRemovePosMenuLine.Visible = true;
                    lblNoSelection.Visible = false;
                    btnMoveDown.Visible = true;
                    btnMoveUp.Visible = true;
                }
                LoadLines(true);
            }
            else
            {
                lblGroupHeader.Visible = false;
                lvKitchenDisplayMenuLines.Visible = false;
                btnsEditAddRemovePosMenuLine.EditButtonEnabled = false;
                btnsEditAddRemovePosMenuLine.Visible = false;
                lblNoSelection.Visible = true;
                btnMoveDown.Visible = false;
                btnMoveUp.Visible = false;
            }

        }

        private void btnsEditAddRemovePosMenu_AddButtonClicked(object sender, EventArgs e)
        {
            
            var dlg = new Dialogs.ButtonProfileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedKitchenDisplayMenuHeaderID = dlg.Id;
                LoadItems(PosMenuHeaderSorting.MenuID, false, true);
            }
        }

        private void btnsEditAddRemovePosMenu_EditButtonClicked(object sender, EventArgs e)
        {
            var selectedButtonGridMenuId =
                ((RecordIdentifier)lvKitchenDisplayMenuHeaders.SelectedItems[0].Tag).PrimaryID;
            var dlg = new Dialogs.ButtonProfileDialog(selectedButtonGridMenuId);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems(PosMenuHeaderSorting.MenuID, false, true);
            }
        }

        private void btnsEditAddRemovePosMenu_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (lvKitchenDisplayMenuHeaders.SelectedItems.Count == 1)
            {
                PluginOperationsHelper.DeleteButtonProfile(selectedKitchenDisplayMenuHeaderID);
            }
            else
            {
                PluginOperationsHelper.DeleteButtonProfiles(lvKitchenDisplayMenuHeaders.GetSelectedIDs());
            }
        }

        private void lvKitchenDisplayMenuLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedKitchenDisplayMenuLineID = (lvKitchenDisplayMenuLines.SelectedItems.Count > 0) ? ((PosMenuLineListItem)lvKitchenDisplayMenuLines.SelectedItems[0].Tag).ID : "";
            btnsEditAddRemovePosMenuLine.EditButtonEnabled = btnsEditAddRemovePosMenuLine.RemoveButtonEnabled = lvKitchenDisplayMenuLines.SelectedItems.Count > 0 && !ReadOnly;

            if (lvKitchenDisplayMenuLines.SelectedItems.Count == 0 || lvKitchenDisplayMenuLines.Items.Count <= 1)
            {
                btnMoveUp.Enabled = false;
                btnMoveDown.Enabled = false;
                return;
            }

            int currItemIndex = lvKitchenDisplayMenuLines.SelectedIndices[0];
            if (currItemIndex == 0)
            {
                btnMoveUp.Enabled = false;
                btnMoveDown.Enabled = true;
            }
            else if (currItemIndex == lvKitchenDisplayMenuLines.Items.Count - 1)
            {
                btnMoveUp.Enabled = true;
                btnMoveDown.Enabled = false;
            }
            else
            {
                btnMoveUp.Enabled = true;
                btnMoveDown.Enabled = true;
            }
        }

        private void lvKitchenDisplayMenuHeaders_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // We only want to sort based on the first two columns.
            if (e.Column > 1)
            {
                return;
            }

            if (lvKitchenDisplayMenuHeaders.SortColumn == e.Column)
            {
                lvKitchenDisplayMenuHeaders.SortedBackwards = !lvKitchenDisplayMenuHeaders.SortedBackwards;
            }
            else
            {
                lvKitchenDisplayMenuHeaders.SortedBackwards = false;
            }

            if (lvKitchenDisplayMenuHeaders.SortColumn != -1)
            {
                lvKitchenDisplayMenuHeaders.Columns[lvKitchenDisplayMenuHeaders.SortColumn].ImageIndex = 2;
                lvKitchenDisplayMenuHeaders.SortColumn = e.Column;

            }
        }

        private void btnsEditAddRemovePosMenuLine_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowButtonDialog(selectedKitchenDisplayMenuHeaderID, selectedKitchenDisplayMenuLineID);
        }

        private void lvKitchenDisplayMenuHeaders_DoubleClick(object sender, EventArgs e)
        {
            if (lvKitchenDisplayMenuHeaders.SelectedItems.Count > 0 && btnsEditAddRemovePosMenu.EditButtonEnabled)
            {
                btnsEditAddRemovePosMenu_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void lvKitchenDisplayMenuLines_DoubleClick(object sender, EventArgs e)
        {
            if (lvKitchenDisplayMenuLines.SelectedItems.Count > 0 && btnsEditAddRemovePosMenuLine.EditButtonEnabled)
            {
                btnsEditAddRemovePosMenuLine_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        protected override void OnClose()
        {
            lvKitchenDisplayMenuHeaders.SmallImageList = null;
            lvKitchenDisplayMenuLines.SmallImageList = null;

            base.OnClose();
        }

        public override void OnIsTopmost()
        {
            lvKitchenDisplayMenuHeaders.ShowSelectedItem();
            lvKitchenDisplayMenuLines.ShowSelectedItem();
        }

        private void btnsEditAddRemovePosMenuLine_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowButtonDialog(selectedKitchenDisplayMenuHeaderID);
        }

        private void btnsEditAddRemovePosMenuLine_RemoveButtonClicked(object sender, EventArgs e)
        {
            RecordIdentifier selectedMenuHeader = (RecordIdentifier)lvKitchenDisplayMenuHeaders.SelectedItems[0].Tag;
            if (lvKitchenDisplayMenuLines.SelectedItems.Count == 1)
            {
                PluginOperationsHelper.DeleteKdsButton(selectedKitchenDisplayMenuLineID, selectedMenuHeader);
            }
            else
            {
                PluginOperationsHelper.DeleteKdsButtons(lvKitchenDisplayMenuLines.GetSelectedDataEntities().ConvertAll(l => l.ID), selectedMenuHeader);
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            PosMenuLineListItem selectedLine = (PosMenuLineListItem)lvKitchenDisplayMenuLines.SelectedItems[0].Tag;
            PosMenuLineListItem upperLine = (PosMenuLineListItem)lvKitchenDisplayMenuLines.Items[lvKitchenDisplayMenuLines.SelectedIndices[0] - 1].Tag;

            int selectedKeyNumber = selectedLine.KeyNo;
            selectedLine.KeyNo = upperLine.KeyNo;
            upperLine.KeyNo = selectedKeyNumber;

            Providers.PosMenuLineData.SaveOrder(PluginEntry.DataModel, selectedLine);
            Providers.PosMenuLineData.SaveOrder(PluginEntry.DataModel, upperLine);

            LoadLines(true);
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            PosMenuLineListItem selectedLine = (PosMenuLineListItem)lvKitchenDisplayMenuLines.SelectedItems[0].Tag;
            PosMenuLineListItem lowerLine = (PosMenuLineListItem)lvKitchenDisplayMenuLines.Items[lvKitchenDisplayMenuLines.SelectedIndices[lvKitchenDisplayMenuLines.SelectedIndices.Count - 1] + 1].Tag;

            int selectedKeyNumber = selectedLine.KeyNo;
            selectedLine.KeyNo = lowerLine.KeyNo;
            lowerLine.KeyNo = selectedKeyNumber;

            Providers.PosMenuLineData.SaveOrder(PluginEntry.DataModel, selectedLine);
            Providers.PosMenuLineData.SaveOrder(PluginEntry.DataModel, lowerLine);

            LoadLines(true);
        }
    }
}
