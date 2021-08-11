using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewPlugins.Scheduler.Controls;
using LSOne.ViewPlugins.Scheduler.Utils;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class NewSubJobsFromTablesDialog : DialogBase
    {
        private bool hasCheckedItems;

        public NewSubJobsFromTablesDialog()
        {
            InitializeComponent();
            
            ComboUtils.PopulateComboBoxItems<ReplicationTypes>(cmbRepliactionMethod, Properties.Resources.ResourceManager);
            tableDesignTree.CheckBoxes = true;
            UpdateActions();
        }

        private void UpdateActions()
        {
            btnOK.Enabled = hasCheckedItems
                &&(
                    (ReplicationTypes)ComboUtils.GetComboSelectionInt(cmbRepliactionMethod) == ReplicationTypes.Normal 
                    ||cmbActionTable.SelectedData != null
                );
        
        }

        private void NewSubJobsFromTablesDialog_Shown(object sender, EventArgs e)
        {
            tableDesignTree.LoadData();
            hasCheckedItems = false;
            ComboUtils.SetComboSelection(cmbRepliactionMethod, (int)ReplicationTypes.Normal);
            tbNamePrefix.Clear();
        }
        private void NewSubJobsFromTablesDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void tableDesignTree_ItemCheckedChanged(object sender, ItemCheckChangedEventArgs e)
        {
            hasCheckedItems = e.HasCheckedItems;
            UpdateActions();
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                grpOptions.Enabled = false;
                btnOK.Enabled = false;
                btnCancel.Enabled = false;

                var tables = tableDesignTree.GetCheckedTableDesigns();
                progressBar.Maximum = tables.Count;
                progressBar.Value = 0;
                progressBar.Visible = true;

                Application.DoEvents();
                Cursor.Current = Cursors.WaitCursor;

                foreach (var tableDesign in tableDesignTree.GetCheckedTableDesigns())
                {
                    JscSubJob subJob = new JscSubJob();

                    subJob.TableFrom = tableDesign.ID;
                    subJob.Description = tbNamePrefix.Text + tableDesign.TableName;
                    subJob.TableNameTo = tableDesign.TableName;

                    // Set default values
                    subJob.ReplicationMethod = (ReplicationTypes) ComboUtils.GetComboSelectionInt(cmbRepliactionMethod);
                    subJob.WhatToDo = ModeDef.UpdateAdd;
                    subJob.Enabled = true;
                    subJob.MoveActions = false;
                    subJob.NoDistributionFilter = false;
                    subJob.UpdateRepCounter = false;
                    subJob.UpdateRepCounterOnEmptyInt = false;
                    if (subJob.ReplicationMethod == ReplicationTypes.Action)
                    {
                        subJob.JscActionTable = cmbActionTable.SelectedData as JscTableDesign;
                    }

                    DataProviderFactory.Instance.Get<IJobData, JscJob>().Save(PluginEntry.DataModel, subJob);

                    progressBar.Increment(1);
                }
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            finally
            {
                btnCancel.Enabled = true;
                progressBar.Visible = false;
                UpdateActions();
                grpOptions.Enabled = true;
            }


        }

        private void cmbRepliactionMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbActionTable.Enabled = cmbActionTable.Visible = lblActionTable.Visible
                = (ReplicationTypes)ComboUtils.GetComboSelectionInt(cmbRepliactionMethod) ==
                  ReplicationTypes.Action;
            UpdateActions();
           
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
            UpdateActions();
        }

    }
}
