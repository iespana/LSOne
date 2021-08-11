using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.DataLayer.SqlDDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Scheduler.Controls;
using LSOne.ViewPlugins.Scheduler.Dialogs;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    public partial class ReplicationCountersView : ViewBase
    {
        private List<ReplicationCounterListItem> latestDataList;
        private ReplicationCounterDialog replicationCounterDialog = new ReplicationCounterDialog();

        public ReplicationCountersView()
        {
            InitializeComponent();

            Attributes =
                ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.ContextBar;

            lvRepCounters.ContextMenuStrip = new ContextMenuStrip();
            lvRepCounters.ContextMenuStrip.Opening += contextMenuStrip_Opening;
        }

        protected override void LoadData(bool isRevert)
        {
            HeaderText = Properties.Resources.ReplicationCounters;

            InitializeComboBoxes();
            RefreshData();
            UpdateActions();
        }

        private void InitializeComboBoxes()
        {
            PopulateJobs();
            cmbJobs.SelectedIndex = 0;

            PopulateSubJobs();
            cmbSubJobs.SelectedIndex = 0;

            PopulateLocations();
            cmbLocations.SelectedIndex = 0;
        }

        private void PopulateJobs()
        {
            cmbJobs.Items.Clear();
            cmbJobs.Items.Add(new DataSelector { Text = Properties.Resources.JobsAllFilter });
            foreach (var jobSelector in DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJobSelectorList(PluginEntry.DataModel))
            {
                cmbJobs.Items.Add(jobSelector);
            }
        }

        private void PopulateSubJobs()
        {
            cmbSubJobs.Items.Clear();
            cmbSubJobs.Items.Add(new DataSelector { Text = Properties.Resources.SubJobsAllFilter });
            foreach (var subJobSelector in DataProviderFactory.Instance.Get<IJobData, JscJob>().GetSubJobSelectorList(PluginEntry.DataModel))
            {
                cmbSubJobs.Items.Add(subJobSelector);
            }
        }

        private void PopulateLocations()
        {
            cmbLocations.Items.Clear();
            cmbLocations.Items.Add(new DataSelector { Text = Properties.Resources.LocationsAllFilter });
            var locations =
                from location in DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocations(PluginEntry.DataModel, true)
                where location.DBConnectionString != null
                orderby location.Text
                select new
                {
                    Id = location.ID,
                    Text = location.Text,
                };

            foreach (var location in locations)
            {
                cmbLocations.Items.Add(new DataSelector { GuidId = (Guid)location.Id, Text = location.Text });
            }
        }

        private void UpdateActions()
        {
            contextButtonEdit.Enabled = lvRepCounters.Selection.Count == 1;
            contextButtonRemove.Enabled = lvRepCounters.Selection.Count >= 1;
        }

        private void RefreshData()
        {
            Cursor.Current = Cursors.WaitCursor;
            var repCounterFilter = CreateRepCountersFilter();
            latestDataList = new List<ReplicationCounterListItem>(DataProviderFactory.Instance.Get<IJobData, JscJob>().GetReplicationCounterListItems(PluginEntry.DataModel, repCounterFilter));
            RefreshListView();
        }

        private void RefreshListView()
        {
            if (latestDataList == null)
                return;

            Cursor.Current = Cursors.WaitCursor;
            lvRepCounters.ClearRows();
            foreach (var repCounterItem in latestDataList)
            {
                Row row = new Row();
                SetRow(row, repCounterItem);
                lvRepCounters.AddRow(row);
            }

            lvRepCounters.AutoSizeColumns();
            lvRepCounters.Sort();

        }

        private void SetRow(Row row, ReplicationCounterListItem repCounterItem)
        {
            row.Clear();
            row.AddText(repCounterItem.JobText);
            row.AddText(repCounterItem.SubJobText);
            row.AddText(repCounterItem.LocationText);
            row.AddCell(new NumericCell(repCounterItem.JscRepCounter.Counter.ToString(), repCounterItem.JscRepCounter.Counter));
            row.Tag = repCounterItem;
        }

        private ReplicationCountersFilter CreateRepCountersFilter()
        {
            return new ReplicationCountersFilter
            {
                JobId = GuidFromSelector((DataSelector)cmbJobs.SelectedItem),
                SubJobId = GuidFromSelector((DataSelector)cmbSubJobs.SelectedItem),
                LocationId = GuidFromSelector((DataSelector)cmbLocations.SelectedItem)
            };
        }

        private Guid? GuidFromSelector(DataSelector selector)
        {
            if (selector.GuidId != null &&  selector.GuidId != default(Guid))
            {
                return (Guid)selector.GuidId;
            }
            return null;
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void contextButtonEdit_Click(object sender, EventArgs e)
        {
            if (lvRepCounters.Selection.Count != 1)
                return;

            var row = lvRepCounters.Row(lvRepCounters.Selection.FirstSelectedRow);
            var repItem = (ReplicationCounterListItem)row.Tag;
            if (this.replicationCounterDialog.ShowDialog(PluginEntry.Framework.MainWindow, repItem.JscRepCounter) == DialogResult.OK)
            {
                SetRow(row, repItem);
                DataProviderFactory.Instance.Get<IJobData, JscJob>().Save(PluginEntry.DataModel,repItem.JscRepCounter);
                lvRepCounters.Sort();
                lvRepCounters.AutoSizeColumns();
            }
        }

        private void contextButtonRemove_Click(object sender, EventArgs e)
        {
            string msg = null;

            if (lvRepCounters.Selection.Count == 1)
            {
                msg = Properties.Resources.ReplicationCounterRemoveMsg;
            }
            else if (lvRepCounters.Selection.Count > 1)
            {
                msg = string.Format(Properties.Resources.ReplicationCounterRemoveManyMsg, lvRepCounters.Selection.Count);
            }

            if (msg == null)
                return;

            if (QuestionDialog.Show(msg, Properties.Resources.ReplicationCounterRemoveHeader) != DialogResult.Yes)
                return;

            List<Row> rowsToRemove = new List<Row>();
            for (int i = 0; i < lvRepCounters.Selection.Count; i++)
            {
                rowsToRemove.Add(lvRepCounters.Rows[lvRepCounters.Selection.GetRowIndex(i)]);   
            }
            DataProviderFactory.Instance.Get<IJobData, JscJob>().Delete(PluginEntry.DataModel,rowsToRemove.Select(r => ((ReplicationCounterListItem)r.Tag).JscRepCounter));
            RefreshData();
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            var menu = lvRepCounters.ContextMenuStrip;

            menu.Items.Clear();
            contextButtonEdit.AddToContextMenu(menu, ContextButtons.GetEditButtonImage(), Properties.Resources.Edit, 100, contextButtonEdit_Click);
            contextButtonRemove.AddToContextMenu(menu, ContextButtons.GetAddButtonImage(), Properties.Resources.Delete, 200, contextButtonRemove_Click);

            PluginEntry.Framework.ContextMenuNotify("ReplicationCounter", lvRepCounters.ContextMenuStrip, lvRepCounters);
            e.Cancel = menu.Items.Count == 0;
        }

        protected override string LogicalContextName
        {
            get { return Properties.Resources.ReplicationCounters; }
        }

        public override RecordIdentifier ID
        {
            get { return RecordIdentifier.Empty; }
        }

        private void lvRepCounters_SelectionChanged(object sender, EventArgs e)
        {
            UpdateActions();
        }

        private void lvRepCounters_RowDoubleClick(object sender, LSOne.Controls.EventArguments.RowEventArgs args)
        {
            if (contextButtonEdit.Enabled)
            {
                contextButtonEdit_Click(contextButtonEdit, EventArgs.Empty);
            }
        }
    }
}
