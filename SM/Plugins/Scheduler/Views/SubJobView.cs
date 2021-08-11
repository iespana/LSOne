using System;
using System.ComponentModel;
using LSOne.Controls;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Scheduler.Controls;
using LSOne.ViewPlugins.Scheduler.Utils;
using LSRetail.DD.Common;
using System.Collections.Generic;
using System.Linq;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    public partial class SubJobView : ViewBase
    {
        private Guid subJobId;

        private JscSubJob subJob;
        private Guid? hintJobId;

        private TabControl.Tab actionPage;
        private TabControl.Tab replicationPage;
        private TabControl.Tab filterPage;

        IEnumerable<IDataEntity> recordBrowsingContext;

        internal class ViewPagesContext
        {
            public JscSubJob SubJob { get; set; }
            public Guid? HintJobId { get; set; }
        }

        public SubJobView(Guid subJobId, Guid? hintJobId)
            : this()
        {
            this.subJobId = subJobId;
            this.hintJobId = hintJobId;
        }

        public SubJobView(Guid subJobId, Guid? hintJobId, IEnumerable<IDataEntity> recordBrowsingContext)
            : this()
        {
            this.subJobId = subJobId;
            this.hintJobId = hintJobId;
            this.recordBrowsingContext = recordBrowsingContext;
        }

        private SubJobView()
        {
            InitializeComponent();
            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.RecordCursor;

            ComboUtils.PopulateComboBoxItems<ReplicationTypes>(cmbRepliactionMethod, Properties.Resources.ResourceManager);

            ComboUtils.PopulateComboBoxItems<ModeDef>(cmbWhatToDo, Properties.Resources.ResourceManager, modeDef => modeDef != ModeDef.None);
        }


        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.Subjob;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "FromTableFilter")
            {
                if (changeHint == DataEntityChangeType.Delete || changeHint == DataEntityChangeType.Add || changeHint == DataEntityChangeType.Edit)
                {
                    subJob.JscSubJobFromTableFilters = DDProviders.JobData.GetSubJobFromTableFiltersList(PluginEntry.DataModel, subJob);
                }
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                HeaderText = Properties.Resources.Subjob;

                actionPage = new TabControl.Tab(Properties.Resources.SubJobActions, ViewPages.SubJobActionPage.CreateInstance);
                tabControl.AddTab(actionPage);

                replicationPage = new TabControl.Tab(Properties.Resources.SubJobReplicationCounters, ViewPages.SubJobReplicationPage.CreateInstance);
                tabControl.AddTab(replicationPage);

                filterPage = new TabControl.Tab(Properties.Resources.SubJobFromTableFilters, ViewPages.SubJobFromTableFiltersPage.CreateInstance);
                tabControl.AddTab(filterPage);
            }

            ReadOnly = !PluginEntry.DataModel.HasPermission(SchedulerPermissions.JobSubjobEdit);
           
            subJob = DataProviderFactory.Instance.Get<IJobData, JscJob>().GetSubJob(PluginEntry.DataModel, subJobId);

            SubJobToForm();
            tabControl.SetData(isRevert, null, new ViewPagesContext { SubJob = subJob, HintJobId = hintJobId });
        }

        private void SubJobToForm()
        {
            tbDescription.Text = subJob.Description;
            ComboUtils.SetComboSelection(cmbRepliactionMethod, (int)subJob.ReplicationMethod);
            cmbTableName.SelectedData = subJob.JscTableFrom;
            cmbTableNameTo.Text = subJob.TableNameTo;
            tbStoredProcName.Text = subJob.StoredProcName;
            tbDescription.Text = subJob.Description;
            ComboUtils.SetComboSelection(cmbWhatToDo, (int)subJob.WhatToDo);
            chkEnabled.Checked = subJob.Enabled;
            chkIncludeFlowFields.Checked = subJob.IncludeFlowFields;
            chkMoveActions.Checked = subJob.MoveActions;
            tbFilterCodeFilter.Text = subJob.FilterCodeFilter;
            SetFieldAccessByReplicationMethod(subJob.ReplicationMethod);
        }

        private void SubJobFromForm()
        {
            subJob.Description = tbDescription.Text;
            subJob.ReplicationMethod = (ReplicationTypes)ComboUtils.GetComboSelectionInt(cmbRepliactionMethod);
            subJob.JscTableFrom = (JscTableDesign)cmbTableName.SelectedData;
            subJob.TableNameTo = string.IsNullOrEmpty(cmbTableNameTo.Text) ? null : cmbTableNameTo.Text;
            subJob.StoredProcName = string.IsNullOrEmpty(tbStoredProcName.Text) ? null : tbStoredProcName.Text;
            subJob.Description = string.IsNullOrEmpty(tbDescription.Text) ? null : tbDescription.Text; 
            subJob.WhatToDo = (ModeDef)ComboUtils.GetComboSelectionInt(cmbWhatToDo);
            subJob.Enabled = chkEnabled.Checked;
            subJob.IncludeFlowFields = chkIncludeFlowFields.Checked;
            subJob.MoveActions = chkMoveActions.Checked;
            subJob.FilterCodeFilter = tbFilterCodeFilter.Text;
        }


        protected override bool DataIsModified()
        {
            if (tabControl.IsModified())
                return true;

            if ((string.IsNullOrEmpty(tbDescription.Text) ? null : tbDescription.Text) != subJob.Description)
                return true;

            if (ComboUtils.GetComboSelectionInt(cmbRepliactionMethod) != (int)subJob.ReplicationMethod)
                return true;

            Guid? formTableId = null;
            JscTableDesign tableDesign = cmbTableName.SelectedData as JscTableDesign;
            if (tableDesign != null)
                formTableId = (Guid)tableDesign.ID;

            if (formTableId != subJob.TableFrom)
                return true;

            if ((string.IsNullOrEmpty(cmbTableNameTo.Text) ? "" : cmbTableNameTo.Text) != subJob.TableNameTo)
               return true;

            if ((string.IsNullOrEmpty(tbStoredProcName.Text) ? "" : tbStoredProcName.Text) != subJob.StoredProcName)
               return true;

            if ((string.IsNullOrEmpty(tbDescription.Text) ? "" : tbDescription.Text) != subJob.Description)
                return true;

            if (ComboUtils.GetComboSelectionInt(cmbWhatToDo) != (int)subJob.WhatToDo)
                return true;

            if (chkEnabled.Checked != subJob.Enabled)
                return true;

            if (chkIncludeFlowFields.Checked != subJob.IncludeFlowFields)
                return true;

            if (chkMoveActions.Checked != subJob.MoveActions)
                return true;

            if (tbFilterCodeFilter.Text != subJob.FilterCodeFilter)
                return true;

            return false;
        }


        protected override bool SaveData()
        {
            SubJobFromForm();
            tabControl.GetData();

            DataProviderFactory.Instance.Get<IJobData, JscJob>().Save(PluginEntry.DataModel, subJob);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "SubJob", subJob.ID, null);

            return true;
        }


        private void SetFieldAccessByReplicationMethod(ReplicationTypes method)
        {
            cmbTableName.Enabled = method != ReplicationTypes.Procedure;
            cmbTableNameTo.Enabled = method != ReplicationTypes.Procedure;
            tbStoredProcName.Enabled = method == ReplicationTypes.Procedure;
            cmbWhatToDo.Enabled = method == ReplicationTypes.Normal;
            if (method == ReplicationTypes.Normal && (ModeDef)ComboUtils.GetComboSelectionInt(cmbWhatToDo) == ModeDef.None)
            {
                ComboUtils.SetComboSelection(cmbWhatToDo, (int)ModeDef.UpdateAdd);
            }
            chkIncludeFlowFields.Enabled = method != ReplicationTypes.Procedure;
            chkMoveActions.Enabled = method != ReplicationTypes.Normal;
        }




        private void cmbTableName_FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (e.Data == null)
            {
                e.TextToDisplay = string.Empty;
            }
            else
            {
                var tableDesign = (JscTableDesign)e.Data;
                e.TextToDisplay = string.Format("{0}:{1}", tableDesign.JscDatabaseDesign.Description, tableDesign.TableName);
            }
        }

        private void cmbTableName_DropDown(object sender, DropDownEventArgs e)
        {
            var tableSelectPanel = new TableSelectPanel( cmbTableName.SelectedData as JscTableDesign);
            e.ControlToEmbed = tableSelectPanel;
        }

        private void cmbTableName_SelectedDataChanged(object sender, EventArgs e)
        {
            JscTableDesign tableDesign = cmbTableName.SelectedData as JscTableDesign;
            if (tableDesign != null)
            {
                // Broadcast data change view pages can detect that a new table has been selected
                tabControl.BroadcastChangeInformation(DataEntityChangeType.VariableChanged, "SubJob.TableFrom", subJob.ID, tableDesign);
            }
        }


        private void cmbTableNameTo_DropDown(object sender, DropDownEventArgs e)
        {
            string selectText = null;
            var fromTable = cmbTableName.SelectedData as JscTableDesign;
            if (fromTable != null)
            {
                selectText = fromTable.TableName;
            }

            var tableSelectPanel = new TableSelectPanel( selectText);
            e.ControlToEmbed = tableSelectPanel;
        }

        private void cmbTableNameTo_FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (e.Data == null)
            {
                e.TextToDisplay = string.Empty;
            }
            else
            {
                e.TextToDisplay = ((JscTableDesign)e.Data).TableName;
            }
        }

        private void cmbRepliactionMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReplicationTypes method = (ReplicationTypes)ComboUtils.GetComboSelectionInt(cmbRepliactionMethod);
            SetFieldAccessByReplicationMethod(method);
            tabControl.BroadcastChangeInformation(DataEntityChangeType.VariableChanged, "SubJob.ReplicationMethod", subJob.ID, method);
        }

        private void tbDescription_Validating(object sender, CancelEventArgs e)
        {
            string errorMsg = string.Empty;
            if (tbDescription.TextLength == 0)
            {
                errorMsg = Properties.Resources.FieldCannotBeEmptyMsg;
                e.Cancel = true;
            }
            errorProvider.SetError(tbDescription, errorMsg);
        }

        protected override ViewBase GetRecordCursorView(int cursorIndex)
        {
            if (recordBrowsingContext != null)
            {
                var id = recordBrowsingContext.ElementAt(cursorIndex).ID;

                if (DataProviderFactory.Instance.Get<IJobData, JscJob>().SubJobExists(PluginEntry.DataModel, id))
                {
                    return new SubJobView((Guid)recordBrowsingContext.ElementAt(cursorIndex).ID, hintJobId, recordBrowsingContext);
                }
            }

            return null;
        }

        protected override void GetRecordCursorInfo(ContextBarCursorEventArguments args)
        {
            if (recordBrowsingContext != null)
            {
                args.Position = 0;
                args.Count = recordBrowsingContext.Count();

                foreach (IDataEntity entity in recordBrowsingContext)
                {
                    if (entity.ID == subJobId)
                    {
                        return;
                    }

                    args.Position++;
                }
            }
            else
            {
                args.Count = 1;
                args.Position = 0;
            }
        }
    }
}
