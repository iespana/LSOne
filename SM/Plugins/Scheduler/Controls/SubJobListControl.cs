using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.DataLayer.SqlDDDataProviders;
using LSOne.ViewCore;
using LSOne.ViewPlugins.Scheduler.Properties;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public partial class SubJobListControl : UserControl
    {
        private List<SubJobListItem> latestDataList;

        public event EventHandler<SubJobEventArgs> ShowSubJob;
        public event EventHandler<EventArgs> AddMultipleAction;
        public event EventHandler<SubJobEventArgs> AddAction;
        public event EventHandler<SubJobEventArgs> EditAction;
        public event EventHandler<SubJobsEventArgs> RemoveAction;

        private int lastSelectedRow;

        public SubJobListControl()
        {
            InitializeComponent();

            lvSubJobs.ContextMenuStrip = new ContextMenuStrip();
            lvSubJobs.ContextMenuStrip.Opening += ContextMenuStripOpening;
            lvSubJobs.SetSortColumn(colDescription, true);
            lastSelectedRow = -1;
        }

        public void LoadData(IEnumerable<SubJobListItem> subJobItems)
        {
            this.latestDataList = new List<SubJobListItem>(subJobItems);
            RefreshData();
            UpdateActions();
        }

        private void RefreshData()
        {
            if (this.latestDataList == null)
                return;

            Cursor.Current = Cursors.WaitCursor;

            lvSubJobs.ClearRows();

            foreach (var subJobItem in this.latestDataList)
            {
                var row = GetRowFromSubJob(subJobItem);
                lvSubJobs.AddRow(row);
            }

            lvSubJobs.Sort();
            lvSubJobs.AutoSizeColumns(false);

            if (lastSelectedRow >= 0)
            {
                lvSubJobs.Selection.Set(lastSelectedRow);
                lvSubJobs.ScrollRowIntoView(lastSelectedRow);
            }
        }

        private Row GetRowFromSubJob(SubJobListItem subJobItem)
        {
            var row = new Row();
            row.AddCell(new ExtendedCell("", subJobItem.JscSubJob.Enabled ? null : Properties.Resources.Disabled12));
            row.AddText(subJobItem.JscSubJob.Description);
            row.AddText(Utils.Utils.EnumResourceString<ReplicationTypes>(Properties.Resources.ResourceManager, subJobItem.JscSubJob.ReplicationMethod));
            string objectName = string.Empty;
            if (subJobItem.JscSubJob.ReplicationMethod != ReplicationTypes.Procedure)
            {
                if (subJobItem.TableFromName != null)
                {
                    objectName = subJobItem.TableFromName;
                }
            }
            else
            {
                objectName = subJobItem.JscSubJob.StoredProcName;
            }
            row.AddText(objectName);
            row.Tag = subJobItem;

            return row;
        }

        private void UpdateActions()
        {
            bool hasPermission = PluginEntry.DataModel.HasPermission(SchedulerPermissions.JobSubjobEdit);
            int selectedCount = lvSubJobs.Selection.Count;

            btnAddMultipleSubjobs.Enabled = hasPermission && PluginEntry.DataModel.HasPermission(SchedulerPermissions.SubjobCreateMultiple);
            contextButtons.AddButtonEnabled = hasPermission;
            contextButtons.EditButtonEnabled = selectedCount == 1;
            contextButtons.RemoveButtonEnabled = hasPermission && selectedCount >= 1;
        }

        private void lvSubJobs_RowDoubleClick(object sender, LSOne.Controls.EventArguments.RowEventArgs args)
        {
            OnEditAction();
        }

        private void lvSubJobs_SelectionChanged(object sender, EventArgs e)
        {
            if (lvSubJobs.Selection.Count > 0)
                lastSelectedRow = lvSubJobs.Selection.FirstSelectedRow;
            UpdateActions();
        }

        private void contextButtons_EditButtonClicked(object sender, EventArgs e)
        {
            OnEditAction();
        }

        private void contextButtons_AddButtonClicked(object sender, EventArgs e)
        {
            var subJob = OnAddAction();
            if (subJob != null)
            {
                // If the item has not already (through LoadData) been added to the list view,
                // we add it now.
                bool found = false;
                foreach (var row in lvSubJobs.Rows)
                {
                    if (((SubJobListItem)row.Tag).JscSubJob.ID == subJob.ID)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    var subJobItem = new SubJobListItem { JscSubJob = subJob };
                    if (!subJob.TableFrom.IsEmpty)
                    {
                        subJobItem.TableFromName = subJob.JscTableFrom.TableName;
                    }
                    var row = GetRowFromSubJob(subJobItem);
                    lvSubJobs.AddRow(row);
                }
            }
            UpdateActions();
        }

        private void contextButtons_RemoveButtonClicked(object sender, EventArgs e)
        {
            OnRemoveAction();
        }

        private void btnAddMultiple_Click(object sender, EventArgs e)
        {
            OnAddMultipleAction();
        }

        private void OnEditAction()
        {
            if (lvSubJobs.Selection.Count != 1)
                return;

            if (EditAction != null)
            {
                var subJobItem = lvSubJobs.Selection[0].Tag as SubJobListItem;
                EditAction(this, new SubJobEventArgs { SubJob = subJobItem.JscSubJob });
            }
        }

        private void OnShowSubJob(JscSubJob subJob)
        {
            if (ShowSubJob != null)
            {
                ShowSubJob(this, new SubJobEventArgs { SubJob = subJob });
            }
        }

        private JscSubJob OnAddAction()
        {
            JscSubJob subJob = null;

            if (AddAction != null)
            {
                var e = new SubJobEventArgs();
                AddAction(this, e);
                subJob = e.SubJob;
            }

            return subJob;
        }

        private void OnRemoveAction()
        {
            if (RemoveAction != null)
            {
                var subJobs = new List<JscSubJob>();
                for (int i = 0; i < lvSubJobs.Selection.Count; i++)
                {
                    subJobs.Add(((SubJobListItem)lvSubJobs.Selection[i].Tag).JscSubJob);
                }
                RemoveAction(this, new SubJobsEventArgs { SubJobs = subJobs });
            }
        }

        private void OnAddMultipleAction()
        {
            if (AddMultipleAction != null)
            {
                AddMultipleAction(this, EventArgs.Empty);
                UpdateActions();
            }
        }

        private void ContextMenuStripOpening(object sender, CancelEventArgs e)
        {
            if (!EnableActions)
            {
                e.Cancel = true;
                return;
            }

            ExtendedMenuItem item;
            ContextMenuStrip menu = (ContextMenuStrip)sender;

            menu.Items.Clear();
            
            item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(contextButtons_EditButtonClicked));

            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = contextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(contextButtons_AddButtonClicked));

            item.Enabled = contextButtons.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(contextButtons_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = contextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);
            

            item = new ExtendedMenuItem(Resources.AddMultipleSubjobs, 500, btnAddMultiple_Click);
            item.Enabled = btnAddMultipleSubjobs.Enabled;
            menu.Items.Add(item);
            e.Cancel = (menu.Items.Count == 0);
        }

        public bool EnableActions
        {
            get { return pnlBottom.Visible; }
            set { pnlBottom.Visible = value; }
        }

    }

    public class SubJobEventArgs : EventArgs
    {
        public JscSubJob SubJob { get; set; }
    }

    public class SubJobsEventArgs : EventArgs
    {
        public IEnumerable<JscSubJob> SubJobs { get; set; }
    }
}
