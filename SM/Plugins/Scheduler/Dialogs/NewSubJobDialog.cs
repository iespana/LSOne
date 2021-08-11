using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Scheduler.Controls;
using LSOne.ViewPlugins.Scheduler.Utils;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class NewSubJobDialog : DialogBase
    {
        private DataSelector emptyItem;

        public NewSubJobDialog()
        {
            InitializeComponent();
            
            ComboUtils.PopulateComboBoxItems<ReplicationTypes>(cmbRepliactionMethod, Properties.Resources.ResourceManager);
            emptyItem = new DataSelector { GuidId = Guid.Empty, Text = Properties.Resources.SubjobDoNotCopyExisting };
        }


        public JscSubJob SubJob { get; private set; }



        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        
            SubJob = null;

            PopulateCopyFrom();

            ComboUtils.SetComboSelection(cmbRepliactionMethod, (int)ReplicationTypes.Normal);
        }


        private void PopulateCopyFrom()
        {
            cmbCopyFrom.Items.Clear();
            cmbCopyFrom.Items.Add(emptyItem);
            cmbCopyFrom.Items.AddRange(DataProviderFactory.Instance.Get<IJobData, JscJob>().GetSubJobSelectorList(PluginEntry.DataModel));
        }


        private void NewSubJobDialog_Shown(object sender, EventArgs e)
        {
            tbDescription.Clear();
            tbDescription.Focus();
            cmbCopyFrom.SelectedItem = emptyItem;

            UpdateActions();
        }

        private void NewSubJobDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void UpdateActions()
        {
            btnOK.Enabled = tbDescription.TextLength > 0 &&
                            cmbRepliactionMethod.SelectedIndex >= 0 &&
                            (cmbActionTable.SelectedData != null ||
                             (ReplicationTypes) ComboUtils.GetComboSelectionInt(cmbRepliactionMethod) !=
                             ReplicationTypes.Action
                                );

        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            // Create the new subjob
            JscSubJob subJob = null;

            DataSelector copyFrom = cmbCopyFrom.SelectedItem as DataSelector;
            if (copyFrom == null || copyFrom.GuidId == Guid.Empty)
            {
                subJob = new JscSubJob();
                FormToSubJob(subJob);

                // Set default values
                subJob.WhatToDo = ModeDef.UpdateAdd;
                subJob.Enabled = true;
                subJob.MoveActions = false;
                subJob.NoDistributionFilter = false;
                subJob.UpdateRepCounter = false;
                subJob.UpdateRepCounterOnEmptyInt = false;
            }
            else
            {
               
                JscSubJob fromSubJob = DataProviderFactory.Instance.Get<IJobData, JscJob>().GetSubJob(PluginEntry.DataModel, copyFrom.GuidId);
                subJob = ShallowCopyObject(fromSubJob);
                FormToSubJob(subJob);

                foreach (var fromTableFilters in fromSubJob.JscSubJobFromTableFilters)
                {
                    var copiedFilter = ShallowCopyObject(fromTableFilters);
                    subJob.JscSubJobFromTableFilters.Add(copiedFilter);
                }
            }

            // Save the new subjob
            DataProviderFactory.Instance.Get<IJobData, JscJob>().Save(PluginEntry.DataModel, subJob);
            SubJob = subJob;
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "SubJob", subJob.ID, null);

            DialogResult = DialogResult.OK;
        }
        public static T ShallowCopyObject<T>(T source) where T : new()
        {
            PropertyInfo[] sourcePropInfos = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] destinationPropInfos = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // create an object to copy values into
            Type entityType = source.GetType();

            T destination = (T)Activator.CreateInstance(entityType);
            foreach (PropertyInfo sourcePropInfo in sourcePropInfos)
            {
                if (sourcePropInfo.Name.Equals("ID", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                PropertyInfo destPropInfo = destinationPropInfos.Where(pi => pi.Name == sourcePropInfo.Name).First();
                destPropInfo.SetValue(destination, sourcePropInfo.GetValue(source, null), null);
            }
            return destination;
        }

        private void FormToSubJob(JscSubJob subJob)
        {
            subJob.Description = tbDescription.Text;
            subJob.ReplicationMethod = (ReplicationTypes)ComboUtils.GetComboSelectionInt(cmbRepliactionMethod);
            if (subJob.ReplicationMethod == ReplicationTypes.Action)
            {
                subJob.JscActionTable = cmbActionTable.SelectedData as JscTableDesign;
            }
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
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

        private void cmbRepliactionMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbActionTable.Enabled = cmbActionTable.Visible = lblActionTable.Visible
                = (ReplicationTypes) ComboUtils.GetComboSelectionInt(cmbRepliactionMethod) ==
                  ReplicationTypes.Action;
            
                UpdateActions();
            

        }

        private void cmbActionTable_SelectedDataChanged(object sender, EventArgs e)
        {
            UpdateActions();
        }



    }
}
