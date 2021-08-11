using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
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
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Scheduler.Dialogs;
using LSOne.ViewPlugins.Scheduler.Utils;
using LSRetail.DD.Common;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    public partial class SubJobReplicationPage : UserControl, ITabView
    {
        private Views.SubJobView.ViewPagesContext viewPagesContext;
        private Guid activeTableDesignId;
        private Guid pendingTableDesignId;
        private List<JscRepCounter> replicationCounters;
        private ReplicationCounterDialog replicationCounterDialog = new ReplicationCounterDialog();

        public SubJobReplicationPage()
        {
            InitializeComponent();
            lvCounters.ContextMenuStrip = new ContextMenuStrip();
            lvCounters.ContextMenuStrip.Opening += ContextMenuStripOpening;
        }


        public void OnClose()
        {
        }


        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            if (!isRevert)
            {
                this.viewPagesContext =
                    (Views.SubJobView.ViewPagesContext) internalContext;

                activeTableDesignId = Guid.Empty;
                pendingTableDesignId = (Guid) (viewPagesContext.SubJob.TableFrom ?? Guid.Empty);

                LoadReplicationCounters();
                AssertComboFields();
            }
            SubJobToForm();
            UpdateActions();


            if (viewPagesContext.HintJobId == null)
            {
                btnsEditAddRemove.Context = ButtonTypes.EditRemove;
                btnsEditAddRemove.Location = new Point(lvCounters.Right - btnsEditAddRemove.Size.Width, btnsEditAddRemove.Location.Y);
            }
            else
            {
                if (viewPagesContext.SubJob.ReplicationMethod == ReplicationTypes.Action || viewPagesContext.SubJob.ReplicationMethod == ReplicationTypes.Procedure)
                {
                    btnsEditAddRemove.AddButtonEnabled = viewPagesContext.SubJob.ActionTable != RecordIdentifier.Empty;
                }
            }
        }


        private void LoadReplicationCounters()
        {
            replicationCounters =
                new List<JscRepCounter>(DataProviderFactory.Instance.Get<IJobData, JscJob>().GetReplicationCounters(PluginEntry.DataModel, viewPagesContext.SubJob.ID));
            PopulateCounterJobComboBox();
        }

        private void PopulateCounterJobComboBox()
        {
            var jobs = new List<JscJob>(
                (from repCounter in replicationCounters select repCounter.JscJob).Distinct()
                );

            if (viewPagesContext.HintJobId != null)
            {
                // Make sure that the hint job is included in the list, even if there are no replication counters for it
                if (!jobs.Any(job => job.ID == viewPagesContext.HintJobId.Value))
                {
                    jobs.Add(DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJob(PluginEntry.DataModel, viewPagesContext.HintJobId.Value));
                }
            }

            jobs.Sort((x, y) => StringComparer.CurrentCultureIgnoreCase.Compare(x.Text, y.Text));

            cmbCounterJob.BeginUpdate();
            cmbCounterJob.Items.Clear();
            foreach (var job in jobs)
            {
                int index =
                    cmbCounterJob.Items.Add(new DataSelector
                        {
                            GuidId = (Guid) job.ID,
                            Text = job.Text,
                            Object = job
                        });
                if (viewPagesContext.HintJobId != null && job.ID == viewPagesContext.HintJobId.Value)
                {
                    cmbCounterJob.SelectedIndex = index;
                }
            }
            cmbCounterJob.EndUpdate();
            UpdateReplicationCounters();
        }


        private void PopulateFieldComboBox(ComboBox cmb, IEnumerable<JscFieldDesign> fieldDesigns)
        {
            cmb.Items.Clear();
            cmb.Items.Add(new DataSelector
                {
                    GuidId = Guid.Empty,
                    Text = Properties.Resources.ReplicationCounterFieldNone,
                    Object = null
                });
            foreach (var field in fieldDesigns)
            {
                cmb.Items.Add(new DataSelector {GuidId = (Guid) field.ID, Text = field.FieldName, Object = field});
            }
        }


        private void UpdateActions()
        {
            int selectedItemsCount = lvCounters.Selection.Count;
            btnsEditAddRemove.EditButtonEnabled = selectedItemsCount == 1;
            btnsEditAddRemove.RemoveButtonEnabled = selectedItemsCount >= 1;
        }


        private void SubJobToForm()
        {
            JscSubJob subJob = viewPagesContext.SubJob;

            ComboUtils.SetComboSelectionNullableGuid(cmbReplCounterField,subJob.RepCounterField == null ? null : (Guid?)subJob.RepCounterField);
            if (subJob.RepCounterInterval.HasValue)
            {
                ntbRepCounterInterval.Value = subJob.RepCounterInterval.Value;
            }
            else
            {
                ntbRepCounterInterval.Text = string.Empty;
            }

            chkUpdateRepCounter.Checked = subJob.UpdateRepCounter;
            chkUpdateRepCounterOnEmptyInt.Checked = subJob.UpdateRepCounterOnEmptyInt;
            ComboUtils.SetComboSelectionNullableGuid(cmbRecordsSentField, subJob.MarkSentRecordsField == null ? null : (Guid?)subJob.MarkSentRecordsField);
            SetFieldAccessByReplicationMethod(subJob.ReplicationMethod);
        }



        private void SubJobFromForm()
        {
            JscSubJob subJob = viewPagesContext.SubJob;

            subJob.RepCounterField = ComboUtils.GetComboSelectionNullableGuid(cmbReplCounterField);
            subJob.RepCounterInterval = GetFormRepCounterInterval();
            subJob.UpdateRepCounter = chkUpdateRepCounter.Checked;
            subJob.UpdateRepCounterOnEmptyInt = chkUpdateRepCounterOnEmptyInt.Checked;
            subJob.MarkSentRecordsField = ComboUtils.GetComboSelectionNullableGuid(cmbRecordsSentField);
        }


        public bool DataIsModified()
        {
            JscSubJob subJob = viewPagesContext.SubJob;

            if (subJob.RepCounterField != ComboUtils.GetComboSelectionNullableGuid(cmbReplCounterField))
                return true;

            if (subJob.RepCounterInterval != GetFormRepCounterInterval())
                return true;

            if (subJob.UpdateRepCounter != chkUpdateRepCounter.Checked)
                return true;

            if (subJob.UpdateRepCounterOnEmptyInt != chkUpdateRepCounterOnEmptyInt.Checked)
                return true;

            if (subJob.MarkSentRecordsField != ComboUtils.GetComboSelectionNullableGuid(cmbRecordsSentField))
                return true;

            return false;
        }


        private void SetFieldAccessByReplicationMethod(ReplicationTypes method)
        {
            // TODO: Consider read only access

            cmbReplCounterField.Enabled = method == ReplicationTypes.Normal;
            ntbRepCounterInterval.Enabled = method == ReplicationTypes.Normal;
            chkUpdateRepCounter.Enabled = method == ReplicationTypes.Normal;
            chkUpdateRepCounterOnEmptyInt.Enabled = method == ReplicationTypes.Normal;
            cmbRecordsSentField.Enabled = method == ReplicationTypes.Normal;
        }


        private int? GetFormRepCounterInterval()
        {
            if (ntbRepCounterInterval.Text.Length > 0)
                return Convert.ToInt32(ntbRepCounterInterval.Value);
            return null;
        }


        public bool SaveData()
        {
            SubJobFromForm();
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            //throw new NotImplementedException();
        }

        public void SaveUserInterface()
        {
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new SubJobReplicationPage();
        }


        public void OnDataChanged(DataEntityChangeType changeHint,
                                  string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (viewPagesContext != null && viewPagesContext.SubJob != null &&
                viewPagesContext.SubJob.ID == changeIdentifier &&
                changeHint == DataEntityChangeType.VariableChanged)
            {
                if (objectName == "SubJob.TableFrom")
                {
                    // Top view has changed the table of the subjob
                    JscTableDesign tableDesign = param as JscTableDesign;
                    if (tableDesign != null)
                    {
                        pendingTableDesignId = (Guid) tableDesign.ID;
                    }
                    else
                    {
                        pendingTableDesignId = Guid.Empty;
                    }
                    if (activeTableDesignId != pendingTableDesignId)
                    {
                        // This is a different table, clear the current field selections
                        ComboUtils.SetComboSelectionNullableGuid(cmbReplCounterField, null);
                        ComboUtils.SetComboSelectionNullableGuid(cmbRecordsSentField, null);
                    }
                }
                else if (objectName == "SubJob.ReplicationMethod")
                {
                    SetFieldAccessByReplicationMethod((ReplicationTypes) param);
                }
            }
        }

        private void AssertComboFields()
        {
            // If we have a particular table design available, we populate the field combo boxes
            if (activeTableDesignId != pendingTableDesignId)
            {
                IEnumerable<JscFieldDesign> fieldDesigns;
                if (pendingTableDesignId != Guid.Empty)
                {
                    fieldDesigns = DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetFieldDesignsOrderedByFieldName(
                        PluginEntry.DataModel, pendingTableDesignId);
                }
                else
                {
                    fieldDesigns = Enumerable.Empty<JscFieldDesign>();
                }
                PopulateFieldComboBox(cmbReplCounterField, fieldDesigns);

                // Note: cmbRecordsSentField currently has Visible = false
                PopulateFieldComboBox(cmbRecordsSentField, fieldDesigns);

                activeTableDesignId = pendingTableDesignId;
            }
        }

        private void cmbReplCounterField_DropDown(object sender, EventArgs e)
        {
            AssertComboFields();
        }

        private void cmbRecordsSentField_DropDown(object sender, EventArgs e)
        {
            AssertComboFields();
        }

        private void cmbCounterJob_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateReplicationCounters();
        }

        private void UpdateReplicationCounters()
        {
            lvCounters.ClearRows();
            var selectedJob = cmbCounterJob.SelectedItem as DataSelector;
            if (selectedJob == null)
            {
                return;
            }

            foreach (var repCounter in replicationCounters)
            {
                if (repCounter.Job == selectedJob.GuidId)
                {
                    Row row = new Row();
                    SetRow(row, repCounter);
                    lvCounters.AddRow(row);
                }
            }

            lvCounters.AutoSizeColumns();
            lvCounters.Sort();
        }

        private void SetRow(Row row, JscRepCounter repCounter)
        {
            row.Clear();
            row.AddText(repCounter.JscLocation.Text);
            row.AddText(repCounter.Counter.ToString());
            row.Tag = repCounter;
        }

        private void EditSelectedItem()
        {
            if (lvCounters.Selection.Count != 1)
                return;

            var selectedRow = lvCounters.Rows[lvCounters.Selection.FirstSelectedRow];
            var selectedCounter = (JscRepCounter)selectedRow.Tag;
            if (replicationCounterDialog.ShowDialog(PluginEntry.Framework.MainWindow, selectedCounter) ==
                DialogResult.OK)
            {
                SetRow(selectedRow, selectedCounter);
                lvCounters.Invalidate();
            }
        }


        private void RemoveSelectedItems()
        {
            bool doRemove = false;

            var countersToRemove = new List<JscRepCounter>();
            for (int i = 0; i < lvCounters.Selection.Count; i++)
            {
                var selectedRow = lvCounters.Rows[lvCounters.Selection.GetRowIndex(i)];
                countersToRemove.Add((JscRepCounter)selectedRow.Tag);
            }

            if (countersToRemove.Count == 1)
            {
                doRemove =
                    QuestionDialog.Show
                        (
                            Properties.Resources.ReplicationCounterRemoveMsg,
                            Properties.Resources.ReplicationCounterRemoveHeader
                        ) == DialogResult.Yes;
            }
            else if (countersToRemove.Count > 1)
            {
                doRemove =
                    QuestionDialog.Show
                        (
                            string.Format(Properties.Resources.ReplicationCounterRemoveManyMsg, countersToRemove.Count),
                            Properties.Resources.ReplicationCounterRemoveHeader
                        ) == DialogResult.Yes;
            }


            if (doRemove)
            {
                Cursor.Current = Cursors.WaitCursor;
                DataProviderFactory.Instance.Get<IJobData, JscJob>().Delete(PluginEntry.DataModel, countersToRemove);
                LoadReplicationCounters();
                UpdateReplicationCounters();
                UpdateActions();
            }
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            RemoveSelectedItems();
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            EditSelectedItem();
        }


        private void ContextMenuStripOpening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu = (ContextMenuStrip) sender;

            menu.Items.Clear();

            item = new ExtendedMenuItem(
                Properties.Resources.Edit,
                100,
                btEdit_Click);

            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsEditAddRemove.EditButtonEnabled;
            item.Default = true;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Delete,
                300,
                btRemove_Click);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvCounters_SelectionChanged(object sender, EventArgs e)
        {
            UpdateActions();
        }

        private void lvCounters_RowDoubleClick(object sender, LSOne.Controls.EventArguments.RowEventArgs args)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                EditSelectedItem();
            }
        }

        private void btnsEditAddDelete_AddButtonClicked(object sender, EventArgs e)
        {
            // Check if we have a location context and send it to the dialog if we do. This is so that the
            // dialog can filter to the correct locations when allowing the user to create counters.      
            JscJob hintJob = null;

            if(viewPagesContext.HintJobId != null)
            {
                hintJob = DDProviders.JobData.GetJob(PluginEntry.DataModel, viewPagesContext.HintJobId);

            }

            using (CreateReplicationCounterDialog dlg = new CreateReplicationCounterDialog(viewPagesContext.SubJob, hintJob))
            {
                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    LoadReplicationCounters();
                }
            }
        }

        private void cmbReplCounterField_SelectedValueChanged(object sender, EventArgs e)
        {
            btnsEditAddRemove.AddButtonEnabled = (ComboUtils.GetComboSelectionNullableGuid(cmbReplCounterField) != null);
        }
    }
}
