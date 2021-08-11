using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Scheduler.Controls;
using LSOne.ViewPlugins.Scheduler.Dialogs;
using LSRetail.DD.Common;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    public partial class SubJobActionPage : UserControl, ITabView
    {
        private Views.SubJobView.ViewPagesContext viewPagesContext;
        private Guid parentDatabaseDesignId;

        public SubJobActionPage()
        {
            InitializeComponent();

        }

        public void OnClose()
        {
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            if (!isRevert)
            {
                viewPagesContext = (Views.SubJobView.ViewPagesContext)internalContext;
                if (viewPagesContext.SubJob.JscTableFrom != null)
                {
                    parentDatabaseDesignId = (Guid) viewPagesContext.SubJob.JscTableFrom.DatabaseDesign;
                }
                else
                {
                    parentDatabaseDesignId = Guid.Empty;
                }
            }

            SubJobToForm();
        }

        private void SubJobToForm()
        {
            JscSubJob subJob = viewPagesContext.SubJob;

            if (subJob.JscActionTable == null && subJob.ActionTable != null)
            {
                subJob.JscActionTable = DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetTableDesign(PluginEntry.DataModel, subJob.ActionTable);
            }
            cmbActionTable.SelectedData = subJob.JscActionTable;
            if (subJob.ActionCounterInterval.HasValue)
            {
                ntbActionCounterInterval.Value = subJob.ActionCounterInterval.Value;
            }
            else
            {
                ntbActionCounterInterval.Text = string.Empty;
            }
            btnLoadActions.Enabled = !string.IsNullOrEmpty(subJob.ObjectName) && subJob.JscActionTable != null;

            SetFieldAccessByReplicationMethod(subJob.ReplicationMethod);

            chkAlwaysExecute.Checked = subJob.AlwaysExecute;
        }


        private void SubJobFromForm()
        {
            JscSubJob subJob = viewPagesContext.SubJob;

            subJob.JscActionTable = cmbActionTable.SelectedData as JscTableDesign;
            subJob.ActionCounterInterval = GetFormActionCounterInterval();
            subJob.AlwaysExecute = chkAlwaysExecute.Checked;
        }

        private int? GetFormActionCounterInterval()
        {
            if (ntbActionCounterInterval.Text.Length > 0)
                return Convert.ToInt32(ntbActionCounterInterval.Value);
            else
                return null;
        }


        public bool DataIsModified()
        {
            if (cmbActionTable.SelectedData != viewPagesContext.SubJob.JscActionTable)
                return true;

            if (viewPagesContext.SubJob.ActionCounterInterval != GetFormActionCounterInterval())
                return true;

            if (chkAlwaysExecute.Checked != viewPagesContext.SubJob.AlwaysExecute)
                return true;
    
            return false;
        }


        private void SetFieldAccessByReplicationMethod(ReplicationTypes method)
        {
            bool enableActions = method == ReplicationTypes.Action || method == ReplicationTypes.Procedure;
            cmbActionTable.Enabled = enableActions;
            ntbActionCounterInterval.Enabled = enableActions;

            chkAlwaysExecute.Enabled = lblAlwaysExecute.Enabled = method == ReplicationTypes.Procedure;
            if(method != ReplicationTypes.Procedure)
            {
                chkAlwaysExecute.Checked = false;
            }
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
            return new SubJobActionPage();
        }


        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (viewPagesContext != null && viewPagesContext.SubJob != null && viewPagesContext.SubJob.ID == changeIdentifier && changeHint == DataEntityChangeType.VariableChanged)
            {
                if (objectName == "SubJob.TableFrom")
                {
                    // Top view has changed the table of the subjob
                    JscTableDesign tableDesign = (JscTableDesign)param;
                    if (tableDesign != null)
                    {
                        if (tableDesign.DatabaseDesign != parentDatabaseDesignId)
                        {
                            parentDatabaseDesignId = (Guid)tableDesign.DatabaseDesign;
                            // This is a different database design, clear the current table selections
                            cmbActionTable.SelectedData = null;
                        }
                    }
                }
                else if (objectName == "SubJob.ReplicationMethod")
                {
                    var method = (ReplicationTypes)param;
                    SetFieldAccessByReplicationMethod(method);
                    if (method == ReplicationTypes.Normal)
                    {
                        viewPagesContext.SubJob.JscActionTable = null;
                        cmbActionTable.SelectedData = null;
                    }
                }
            }
        }

        private void cmbActionTable_DropDown(object sender, DropDownEventArgs e)
        {
            var tableSelectPanel = new TableSelectPanel(cmbActionTable.SelectedData as JscTableDesign);
            e.ControlToEmbed = tableSelectPanel;
        }

        private void cmbActionTable_FormatData(object sender, DropDownFormatDataArgs e)
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

        private void cmbActionTable_SelectedDataChanged(object sender, EventArgs e)
        {
            string errorMsg = string.Empty;

            if (parentDatabaseDesignId != Guid.Empty)
            {
                if (cmbActionTable.SelectedData != null)
                {
                    var actionTableDatabaseId = ((JscTableDesign)cmbActionTable.SelectedData).DatabaseDesign;
                    if (parentDatabaseDesignId != actionTableDatabaseId)
                    {
                        errorMsg = Properties.Resources.SubjobActionDatabaseDesignMismatch;
                    }
                }
            }

            errorProvider.SetError(cmbActionTable, errorMsg);
        }

        private void btnRemoveAction_Click(object sender, EventArgs e)
        {
            var x =  lvActions.Rows[lvActions.Selection.FirstSelectedRow];
            var action = x.Tag as ReplicationAction;
            JscTableDesign actionTable = cmbActionTable.SelectedData as JscTableDesign;
            RemoveActionDialog dialog = new RemoveActionDialog(action, actionTable);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadData(false, RecordIdentifier.Empty, viewPagesContext);
            }
        }

        private void btnLoadActions_Click(object sender, EventArgs e)
        {
            JscSubJob subJob = viewPagesContext.SubJob;
                var replicationActions = DataProviderFactory.Instance.Get<IReplicationActionData, ReplicationAction>()
                    .Get(PluginEntry.DataModel, subJob.ObjectName, subJob.JscActionTable.TableName, subJob.ID);
                foreach (var replicationAction in replicationActions)
                {
                    Row row = new Row();
                    row.AddText(replicationAction.ActionId.ToString());
                    row.AddText(replicationAction.ActionType.ToString());
                    row.AddText(replicationAction.ActionTarget);
                    string paramText = string.Empty;
                    for (int index = 0; index < replicationAction.Parameters.Count; index += 2)
                    {
                        var actionParameter = replicationAction.Parameters[index];
                        if (paramText != string.Empty)
                        {
                            paramText += Environment.NewLine;
                        }
                        paramText += actionParameter;
                        paramText += " = ";
                        if (index + 1 < replicationAction.Parameters.Count)
                        {
                            var actionParameter1 = replicationAction.Parameters[index + 1];
                            paramText += actionParameter1;
                        }
                        else
                        {
                            paramText += "Missing or NULL";
                        }
                    }

                    row.AddText(paramText);
                    var x =
                        TextRenderer.MeasureText(paramText,
                            lvActions.Font, new Size(100, row.Height));
                    row.Height = (short)x.Height;
                    row.AddText(replicationAction.DateCreated.ToString());
                    row.Tag = replicationAction;
                    lvActions.AddRow(row);
                }
           
            lvActions.AutoSizeColumns();
        }

        private void chkAlwaysExecute_CheckedChanged(object sender, EventArgs e)
        {
            cmbActionTable.Enabled = btnLoadActions.Enabled = ntbActionCounterInterval.Enabled = lvActions.Enabled = !chkAlwaysExecute.Checked;
        }
    }
}
