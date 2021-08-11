using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.Controls.Rows;
using System.Linq;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    public partial class HeaderPanesView : ViewBase
    {
        private RecordIdentifier selectedHeaderPaneID;
        private RecordIdentifier selectedHeaderPaneLineID;
        private RecordIdentifier selectedHeaderPaneColumnID;
        private List<HeaderPaneLine> headerPaneLines;

        public HeaderPanesView(RecordIdentifier headerPaneID)
            : this()
        {
            selectedHeaderPaneID = headerPaneID;
        }

        public HeaderPanesView()
        {
            InitializeComponent();

            HeaderText = Properties.Resources.HeaderProfiles;

            Attributes = ViewAttributes.Help |
                         ViewAttributes.Close |
                         ViewAttributes.ContextBar |
                         ViewAttributes.Audit;

            lvHeaderPanes.ContextMenuStrip = new ContextMenuStrip();
            lvHeaderPanes.ContextMenuStrip.Opening += lvHeaderPanes_Opening;

            lvHeaderPaneColumns.ContextMenuStrip = new ContextMenuStrip();
            lvHeaderPaneColumns.ContextMenuStrip.Opening += lvHeaderPaneColumns_Opening;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles);
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.HeaderProfiles;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return RecordIdentifier.Empty;
	        }
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "KitchenDisplayHeaderPane":
                    if (changeAction == DataEntityChangeType.Add)
                    {
                        selectedHeaderPaneID = changeIdentifier;
                    }
                    LoadData(false);
                    break;
                case "KitchenDisplayHeaderPaneLine":
                    if (changeAction == DataEntityChangeType.Delete)
                    {
                        int deletedLineNumber = headerPaneLines.Where(l => l.ID == changeIdentifier).FirstOrDefault().LineNumber;
                        foreach (var headerLine in headerPaneLines.Where(l => l.LineNumber > deletedLineNumber))
                        {
                            headerLine.LineNumber = headerLine.LineNumber - 1;
                            Providers.KitchenDisplayHeaderPaneLineData.SaveLineNumber(PluginEntry.DataModel, headerLine);
                        }
                    }

                    LoadData(false);
                    break;
                case "KitchenDisplayHeaderPaneColumn":
                    if (changeAction == DataEntityChangeType.Add)
                    {
                        selectedHeaderPaneColumnID = changeIdentifier;
                    }
                    LoadHeaderPaneColumns();
                    break;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            LoadHeaderPanes();
        }

        private void LoadHeaderPanes()
        {
            RecordIdentifier oldSelectedID = selectedHeaderPaneID;
            List<HeaderPaneProfile> headerPanes = Providers.KitchenDisplayHeaderPaneData.GetList(PluginEntry.DataModel);

            lvHeaderPanes.ClearRows();

            HeaderPaneProfileSort sortColumn = (HeaderPaneProfileSort)lvHeaderPanes.Columns.IndexOf(lvHeaderPanes.SortColumn);

            foreach (var headerPane in PluginOperationsHelper.SortList(headerPanes, sortColumn, lvHeaderPanes.SortedAscending))
            {
                Row row = new Row();

                row.AddText(headerPane.Name);
                row.AddText(headerPane.HeaderLines.Count.ToString());
                row.Tag = headerPane.ID;
                lvHeaderPanes.AddRow(row);

                if (oldSelectedID == headerPane.ID)
                {
                    lvHeaderPanes.Selection.Set(lvHeaderPanes.RowCount - 1);
                }
            }

            lvHeaderPanes.AutoSizeColumns();
            lvHeaderPanes_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void LoadHeaderPaneLines()
        {
            headerPaneLines = Providers.KitchenDisplayHeaderPaneLineData.GetList(PluginEntry.DataModel, selectedHeaderPaneID);

            cmbLine.Items.Clear();
            foreach(var headerLine in headerPaneLines)
            {
                cmbLine.Items.Add(new DataEntity { ID = headerLine.ID, Text = headerLine.LineNumber.ToString() });
            }

            if(headerPaneLines.Count == 0)
            {
                cmbLine.Enabled = false;
                btnDeleteLine.Enabled = false;
            }
            else
            {
                cmbLine.Enabled = true;
            }

            if(headerPaneLines.Count > 0)
            {
                cmbLine.SelectedIndex = 0;
            }
            cmbLine_SelectedIndexChanged(this, EventArgs.Empty);

            LoadHeaderPaneColumns();
        }

        private void LoadHeaderPaneColumns()
        {
            RecordIdentifier oldSelectedID = selectedHeaderPaneColumnID;

            if (lvHeaderPanes.Selection.Count == 0) return;

            lvHeaderPaneColumns.ClearRows();

            List<LSOneHeaderPaneLineColumn> headerPaneColumns = Providers.KitchenDisplayHeaderPaneLineColumnData.GetList(PluginEntry.DataModel, selectedHeaderPaneLineID);

            foreach (LSOneHeaderPaneLineColumn headerPaneColumn in headerPaneColumns)
            {
                Row row = new Row();
                row.AddText(headerPaneColumn.ColumnNumber.ToString());
                row.AddText(headerPaneColumn.ColumnTypeAsString());
                row.AddText(headerPaneColumn.Text);
                row.AddText(headerPaneColumn.ColumnAlignmentAsString());
                row.AddText(headerPaneColumn.Style.Text);

                row.Tag = headerPaneColumn;
                lvHeaderPaneColumns.AddRow(row);

                if (oldSelectedID == headerPaneColumn.ID)
                {
                    lvHeaderPaneColumns.Selection.Set(lvHeaderPaneColumns.RowCount - 1);
                }
            }

            lvHeaderPaneColumns.AutoSizeColumns();
            lvHeaderPaneColumns_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void lvHeaderPanes_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool headerPaneSelected = lvHeaderPanes.Selection.Count != 0;

            selectedHeaderPaneID = headerPaneSelected ? (RecordIdentifier)(lvHeaderPanes.Row(lvHeaderPanes.Selection.FirstSelectedRow).Tag) : "";
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled = headerPaneSelected;

            if (headerPaneSelected)
            {
                if (!lblGroupHeader.Visible)
                {
                    lblGroupHeader.Visible
                        = lvHeaderPaneColumns.Visible
                        = btnsContextButtonsColumns.Visible
                        = lblLine.Visible
                        = cmbLine.Visible
                        = btnAddLine.Visible
                        = btnDeleteLine.Visible
                        = btnMoveDown.Visible
                        = btnMoveUp.Visible
                        = true;
                }
                LoadHeaderPaneLines();
            }
            else
            {
                lblGroupHeader.Visible
                    = lvHeaderPaneColumns.Visible
                    = btnsContextButtonsColumns.Visible
                    = lblLine.Visible
                    = cmbLine.Visible
                    = btnAddLine.Visible
                    = btnDeleteLine.Visible
                    = btnMoveDown.Visible
                    = btnMoveUp.Visible
                    = false;
            }
        }

        private void lvHeaderPaneColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedHeaderPaneColumnID = (lvHeaderPaneColumns.Selection.Count > 0) ? ((LSOneHeaderPaneLineColumn)lvHeaderPaneColumns.Row(lvHeaderPaneColumns.Selection.FirstSelectedRow).Tag).ID : "";
            btnsContextButtonsColumns.EditButtonEnabled = btnsContextButtonsColumns.RemoveButtonEnabled = lvHeaderPaneColumns.Selection.Count > 0 && !ReadOnly;

            if (lvHeaderPaneColumns.Selection.Count == 0 || lvHeaderPaneColumns.Rows.Count <= 1)
            {
                btnMoveUp.Enabled = false;
                btnMoveDown.Enabled = false;
                return;
            }

            int currItemIndex = lvHeaderPaneColumns.Selection.GetRowIndex(0);
            if (currItemIndex == 0)
            {
                btnMoveUp.Enabled = false;
                btnMoveDown.Enabled = true;
            }
            else if (currItemIndex == lvHeaderPaneColumns.RowCount - 1)
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

        void lvHeaderPanes_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvHeaderPanes.ContextMenuStrip;
            ExtendedMenuItem item;
            menu.Items.Clear();

            item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(btnEditHeaderPane_Click))
                           {
                               Enabled = btnsContextButtons.EditButtonEnabled,
                               Image = ContextButtons.GetEditButtonImage(),
                               Default = true
                           };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnAddHeaderPane_Click))
                       {
                           Enabled = btnsContextButtons.AddButtonEnabled,
                           Image = ContextButtons.GetAddButtonImage()
                       };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Delete,
                    300,
                    new EventHandler(btnRemoveHeaderPane_Click))
                       {
                           Enabled = btnsContextButtons.RemoveButtonEnabled,
                           Image = ContextButtons.GetRemoveButtonImage()
                       };

            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        void lvHeaderPaneColumns_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvHeaderPaneColumns.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                Properties.Resources.Edit,
                100,
                new EventHandler(btnEditHeaderColumn_Click))
                    {
                        Enabled = btnsContextButtonsColumns.EditButtonEnabled,
                        Image = ContextButtons.GetEditButtonImage(),
                        Default = true
                    };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnAddHeaderColumn_Click))
                       {
                           Enabled = btnsContextButtonsColumns.AddButtonEnabled,
                           Image = ContextButtons.GetAddButtonImage()
                       };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnRemoveHeaderColumn_Click))
                       {
                           Enabled = btnsContextButtonsColumns.RemoveButtonEnabled,
                           Image = ContextButtons.GetRemoveButtonImage()
                       };

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

            PluginEntry.Framework.ContextMenuNotify("HeaderPanesView", lvHeaderPaneColumns.ContextMenuStrip, lvHeaderPaneColumns);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvHeaderPane_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEditHeaderPane_Click(this, EventArgs.Empty);
            }
        }

        private void lvHeaderPaneColumn_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtonsColumns.EditButtonEnabled)
            {
                btnEditHeaderColumn_Click(this, EventArgs.Empty);
            }
        }

        private void btnEditHeaderPane_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowHeaderPaneDialog(selectedHeaderPaneID);
        }

        private void btnAddHeaderPane_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowHeaderPaneDialog();
        }

        private void btnRemoveHeaderPane_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.DeleteHeaderPane(selectedHeaderPaneID);
        }

        private void btnAddLine_Click(object sender, EventArgs e)
        {
            HeaderPaneLine headerPaneLine = new HeaderPaneLine
            {
                HeaderProfileId = selectedHeaderPaneID,
                LineNumber = cmbLine.Items.Count + 1
            };
            Providers.KitchenDisplayHeaderPaneLineData.Save(PluginEntry.DataModel, headerPaneLine);

            LoadHeaderPanes();
            cmbLine.SelectedIndex = cmbLine.Items.Count - 1;
        }

        private void btnDeleteLine_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.DeleteHeaderPaneLine(selectedHeaderPaneLineID);
        }

        private void cmbLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLine.SelectedItem != null)
            {
                btnDeleteLine.Enabled
                    = lvHeaderPaneColumns.Visible
                    = btnsContextButtonsColumns.Visible
                    = btnMoveDown.Visible
                    = btnMoveUp.Visible
                    = true;

                selectedHeaderPaneLineID = ((DataEntity)cmbLine.SelectedItem).ID;
            }
            else
            {
                btnDeleteLine.Enabled
                    = lvHeaderPaneColumns.Visible
                    = btnsContextButtonsColumns.Visible
                    = btnMoveDown.Visible
                    = btnMoveUp.Visible
                    = false;

                selectedHeaderPaneLineID = RecordIdentifier.Empty;
            }

            LoadHeaderPaneColumns();
        }

        private void btnEditHeaderColumn_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowHeaderPaneColumnDialog(selectedHeaderPaneColumnID, selectedHeaderPaneLineID, selectedHeaderPaneID);
        }

        private void btnAddHeaderColumn_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowHeaderPaneColumnDialog(selectedHeaderPaneLineID, selectedHeaderPaneID);
        }

        private void btnRemoveHeaderColumn_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.DeleteHeaderPaneColumn(selectedHeaderPaneColumnID);
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            LSOneHeaderPaneLineColumn selectedColumn = (LSOneHeaderPaneLineColumn)lvHeaderPaneColumns.Row(lvHeaderPaneColumns.Selection.FirstSelectedRow).Tag;
            LSOneHeaderPaneLineColumn upperColumn = (LSOneHeaderPaneLineColumn)lvHeaderPaneColumns.Rows[lvHeaderPaneColumns.Selection.GetRowIndex(0) - 1].Tag;

            int selectedColumnNumber = (int)selectedColumn.ColumnNumber;
            selectedColumn.ColumnNumber = new RecordIdentifier((int)upperColumn.ColumnNumber);
            upperColumn.ColumnNumber = new RecordIdentifier(selectedColumnNumber);

            Providers.KitchenDisplayHeaderPaneLineColumnData.Save(PluginEntry.DataModel, selectedColumn);
            Providers.KitchenDisplayHeaderPaneLineColumnData.Save(PluginEntry.DataModel, upperColumn);

            selectedHeaderPaneColumnID = selectedColumn.ID;

            LoadHeaderPaneColumns();
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            LSOneHeaderPaneLineColumn selectedColumn = (LSOneHeaderPaneLineColumn)lvHeaderPaneColumns.Row(lvHeaderPaneColumns.Selection.FirstSelectedRow).Tag;
            LSOneHeaderPaneLineColumn lowerColumn = (LSOneHeaderPaneLineColumn)lvHeaderPaneColumns.Rows[lvHeaderPaneColumns.Selection.GetRowIndex(0) + 1].Tag;

            int selectedColumnNumber = (int)selectedColumn.ColumnNumber;
            selectedColumn.ColumnNumber = new RecordIdentifier((int)lowerColumn.ColumnNumber);
            lowerColumn.ColumnNumber = new RecordIdentifier(selectedColumnNumber);

            Providers.KitchenDisplayHeaderPaneLineColumnData.Save(PluginEntry.DataModel, selectedColumn);
            Providers.KitchenDisplayHeaderPaneLineColumnData.Save(PluginEntry.DataModel, lowerColumn);

            selectedHeaderPaneColumnID = selectedColumn.ID;

            LoadHeaderPaneColumns();
        }
    }
}