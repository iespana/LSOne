using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Scheduler.Dialogs;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    public partial class DatabaseMapListView : ViewBase
    {
        private NewTableMapDialog newTableMapDialog = new NewTableMapDialog();

        public DatabaseMapListView()
        {
            InitializeComponent();
            Attributes =
                ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.ContextBar;

            lvDatabaseMap.ContextMenuStrip = new ContextMenuStrip();
            lvDatabaseMap.ContextMenuStrip.Opening += lvDatabaseMap_Opening;
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.DatabaseMapListDescription;
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
            HeaderText = Properties.Resources.DatabaseMapList;

            // Trigger refresh of combo box filters
            cmbFromDesign.InvalidateData();
            cmbToDesign.InvalidateData();

            cmbFromDesign.DatabaseDesignId = null;
            cmbToDesign.DatabaseDesignId = null;

            RefreshDatabaseMaps();
            UpdateEditActions();
        }


        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "TableMap")
            {
                LoadData(false);
            }
        }

        private void RefreshDatabaseMaps()
        {
            LoadDatabaseMaps(DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetDatabaseMaps(PluginEntry.DataModel, cmbFromDesign.DatabaseDesignId, cmbToDesign.DatabaseDesignId));
        }

        private void LoadDatabaseMaps(IEnumerable<JscTableMap> databaseMaps)
        {
            lblNoResults.Visible = false;
            lvDatabaseMap.ClearRows();
            foreach (var databaseMap in databaseMaps)
            {
                var row = new Row();
                SetRow(row, databaseMap);
                lvDatabaseMap.AddRow(row);
            }

            lvDatabaseMap.Sort();
            lvDatabaseMap.AutoSizeColumns();

            lblNoResults.Visible = lvDatabaseMap.RowCount == 0;
        }

        private void SetRow(Row row, JscTableMap databaseMap)
        {
            row.AddText(databaseMap.FromJscTableDesign.JscDatabaseDesign.Description);
            row.AddText(databaseMap.ToJscTableDesign.JscDatabaseDesign.Description);
            row.AddText(databaseMap.FromJscTableDesign.TableName);
            row.AddText(databaseMap.ToJscTableDesign.TableName);
            row.Tag = databaseMap;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDatabaseMaps();
        }

        private void UpdateEditActions()
        {
            bool hasPermission = PluginEntry.DataModel.HasPermission(SchedulerPermissions.DatabaseMapEdit);

            int selectedItemsCount = lvDatabaseMap.Selection.Count;

            contextButtonsMaps.AddButtonEnabled = hasPermission;
            contextButtonsMaps.EditButtonEnabled = selectedItemsCount == 1;
            contextButtonsMaps.RemoveButtonEnabled = hasPermission && selectedItemsCount > 0;

            if (lvDatabaseMap.RowCount > 0)
                lblNoResults.Visible = false;
        }


        private JscTableMap SelectedTableMap
        {
            get
            {
                JscTableMap tableMap = null;
                if (lvDatabaseMap.Selection.Count == 1)
                {
                    tableMap = lvDatabaseMap.Row(lvDatabaseMap.Selection.FirstSelectedRow).Tag as JscTableMap;
                }

                return tableMap;
            }
        }

        private void AddMapClicked(object sender, EventArgs e)
        {
            AddMap();
        }

        private void AddMap()
        {
            if (newTableMapDialog.ShowDialog(PluginEntry.Framework.MainWindow) != DialogResult.OK)
            {
                return;
            }

            // Show the new item in the list view
            var row = new Row();
            SetRow(row, newTableMapDialog.TableMap);
            lvDatabaseMap.AddRow(row);
            UpdateEditActions();

            // TODO: From KA 2010.08.26 - Check if ina JscSubJobs have jobs that have source location having this database
            // design and subjebs has TableMap == null and there already exists an older map for this
            // table that would now cause the table name lookup to yield two results (the old one and this new one)

            // Open up the detail view page
            ShowTableMap(newTableMapDialog.TableMap);
        
        }



        private void EditMapClicked(object sender, EventArgs e)
        {
            EditSelectedMap();
        }

        private void EditSelectedMap()
        {
            JscTableMap tableMap = SelectedTableMap;
            if (tableMap != null)
            {
                ShowTableMap(tableMap);
            }
        }

        private void ShowTableMap(JscTableMap tableMap)
        {
            PluginEntry.Framework.ViewController.Add(new Views.DatabaseMapView((Guid)tableMap.ID));
        }

        private void RemoveMapClicked(object sender, EventArgs e)
        {
            string msg;
            if (lvDatabaseMap.Selection.Count == 1)
            {
                JscTableMap tableMap = (JscTableMap)lvDatabaseMap.Row(lvDatabaseMap.Selection.FirstSelectedRow).Tag;
                msg = string.Format(Properties.Resources.DatabaseMapRemoveMsg, tableMap.Description);
            }
            else if (lvDatabaseMap.Selection.Count > 1)
            {
                msg = string.Format(Properties.Resources.DatabaseMapsRemoveManyMsg, lvDatabaseMap.Selection.Count);
            }
            else
            {
                return;
            }

            if (QuestionDialog.Show(msg, Properties.Resources.DatabaseMapRemoveHeader) == DialogResult.Yes)
            {
                List<JscTableMap> tableMapsToDelete = new List<JscTableMap>();
                while (lvDatabaseMap.Selection.Count > 0)
                {
                    JscTableMap tableMapToDelete = (JscTableMap)lvDatabaseMap.Row(lvDatabaseMap.Selection.FirstSelectedRow).Tag;
                    tableMapsToDelete.Add(tableMapToDelete);
                    lvDatabaseMap.RemoveRow(lvDatabaseMap.Selection.FirstSelectedRow);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "TableMap", tableMapToDelete.ID, null);
                }
                DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().DeleteTableMaps(PluginEntry.DataModel, tableMapsToDelete);
            }
        }
        
        private void lvDatabaseMap_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvDatabaseMap.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(EditMapClicked));

            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = contextButtonsMaps.EditButtonEnabled;
            item.Default = true;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(AddMapClicked));

            item.Enabled = contextButtonsMaps.AddButtonEnabled;

            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(RemoveMapClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = contextButtonsMaps.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("DatabaseMapListViewMenu", lvDatabaseMap.ContextMenuStrip, lvDatabaseMap);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvDatabaseMap_RowDoubleClick(object sender, LSOne.Controls.EventArguments.RowEventArgs args)
        {
            if (contextButtonsMaps.EditButtonEnabled)
            {
                EditSelectedMap();
            }
        }

        private void lvDatabaseMap_SelectionChanged(object sender, EventArgs e)
        {
            UpdateEditActions();
        }
    }
}
