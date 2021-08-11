using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Scheduler.Controls;
using LSOne.ViewPlugins.Scheduler.Dialogs;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    public partial class SubJobListView : ViewBase
    {
        private NewSubJobDialog newSubJobDialog = new NewSubJobDialog();
        private NewSubJobsFromTablesDialog newSubJobsFromTablesDialog = new NewSubJobsFromTablesDialog();
        private List<SubJobListItem> latestItems;
    
        public SubJobListView()
        {
            InitializeComponent();
            Attributes =
                ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.ContextBar;
        }

        protected override void LoadData(bool isRevert)
        {
            HeaderText = Properties.Resources.Subjobs;
            LoadItems();
        }

        private void LoadItems()
        {
            latestItems = DataProviderFactory.Instance.Get<IJobData, JscJob>().GetSubJobListItems(PluginEntry.DataModel, chkShowDisabled.Checked).ToList();
            subJobListControl.LoadData(latestItems);
        }

        private void ShowSubJob(JscSubJob subJob)
        {
            PluginEntry.Framework.ViewController.Add(new SubJobView((Guid) subJob.ID, null, latestItems.ConvertAll(x => new DataEntity(x.JscSubJob.ID, x.JscSubJob.Description))));
        }

        private void chkShowDisabled_CheckedChanged(object sender, EventArgs e)
        {
            LoadItems();
        }

        private void subJobListControl_ShowSubJob(object sender, SubJobEventArgs e)
        {
            ShowSubJob(e.SubJob);
        }

        private void subJobListControl_EditAction(object sender, SubJobEventArgs e)
        {
            ShowSubJob(e.SubJob);
        }

        private void subJobListControl_AddAction(object sender, SubJobEventArgs e)
        {
            if (newSubJobDialog.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                e.SubJob = newSubJobDialog.SubJob;
                // Open up the detail view page
                ShowSubJob(newSubJobDialog.SubJob);
            }
        }

        private void subJobListControl_RemoveAction(object sender, SubJobsEventArgs e)
        {
            int subjobCount = e.SubJobs.Count();
            bool doRemove = false;

            if (subjobCount == 1)
            {
                doRemove =
                    QuestionDialog.Show
                    (
                        string.Format(Properties.Resources.SubjobRemoveMsg, e.SubJobs.First().Description),
                        Properties.Resources.SubjobRemoveHeader
                    ) == DialogResult.Yes;
            }
            else if (subjobCount > 1)
            {
                doRemove =
                    QuestionDialog.Show
                    (
                        string.Format(Properties.Resources.SubjobRemoveManyMsg, subjobCount),
                        Properties.Resources.SubjobRemoveHeader
                    ) == DialogResult.Yes;
            }

            if (doRemove)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var subJob in e.SubJobs)
                {
                    if (DataProviderFactory.Instance.Get<IJobData, JscJob>().IsSubjobUsedInJobs(PluginEntry.DataModel, subJob.ID))
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator);
                        }
                        sb.Append(subJob.Description);
                    }
                }

                if (sb.Length > 0)
                {
                    string msg;
                    if (subjobCount == 1)
                    {
                        msg = Properties.Resources.SubjobInUseMsg;
                    }
                    else
                    {
                        msg = Properties.Resources.SubjobManyInUseMsg;
                    }

                    MessageBox.Show(PluginEntry.Framework.MainWindow, string.Format(msg, sb), Properties.Resources.SubjobRemoveHeader, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    doRemove = false;
                }


                if (doRemove)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    DataProviderFactory.Instance.Get<IJobData, JscJob>().Delete(PluginEntry.DataModel, e.SubJobs);
                    LoadItems();
                }
            }
        }

        private void subJobListControl_AddMultipleAction(object sender, EventArgs e)
        {
            if (newSubJobsFromTablesDialog.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                LoadItems();
            }
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            base.OnDataChanged(changeAction, objectName, changeIdentifier, param);
            if (objectName == "SubJob")
            {
                btnSearch_Click(this, EventArgs.Empty);
            }
        }

        protected override string LogicalContextName
        {
            get { return Properties.Resources.Subjobs; }
        }

        public override RecordIdentifier ID
        {
            get { return RecordIdentifier.Empty; }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbSearch.Text = string.Empty;
            LoadItems();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            latestItems = DataProviderFactory.Instance.Get<IJobData, JscJob>()
                .GetSubJobListItems(PluginEntry.DataModel, chkShowDisabled.Checked).ToList().FindAll(subjob => 
                    CultureInfo.InvariantCulture.CompareInfo.IndexOf(subjob.JscSubJob.Description,tbSearch.Text, CompareOptions.IgnoreCase) > -1);
            subJobListControl.LoadData(latestItems);
        }

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (btnSearch.Enabled)
                {
                    btnSearch_Click(this, EventArgs.Empty);
                }
                e.Handled = true;
            }
        }
    }
}
