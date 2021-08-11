using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Rows;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Scheduler.Controls;
using LSOne.ViewPlugins.Scheduler.Dialogs;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    public partial class SubJobFromTableFiltersPage : UserControl, ITabView
    {
        private Views.SubJobView.ViewPagesContext viewPagesContext;
        private SubJobFilterDialog filterDialog = new SubJobFilterDialog();
        private bool dataIsModified;

        public SubJobFromTableFiltersPage()
        {
            InitializeComponent();
        }


        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new SubJobFromTableFiltersPage();
        }


        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            viewPagesContext = (Views.SubJobView.ViewPagesContext)internalContext;
            
            SubJobToForm();
            dataIsModified = false;
        }



        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        private void SubJobToForm()
        {
            JscSubJob subJob = viewPagesContext.SubJob;
            lvFilters.ClearRows();
            var filters =
                from filter in subJob.JscSubJobFromTableFilters
                orderby filter.JscField.FieldName
                select filter;

            foreach (var filter in filters)
            {
                Row row = new Row();
                SetRow(row, filter);
                lvFilters.AddRow(row);
            }
            lvFilters.AutoSizeColumns();
            lvFilters.Sort();
            UpdateActions();
        }

        private void SetRow(Row row, JscSubJobFromTableFilter filter)
        {
            row.Clear();
            row.AddText(filter.JscField.FieldName);
            var filterTypeString = Utils.Utils.EnumResourceString(Properties.Resources.ResourceManager, filter.FilterType);
            row.AddText(filterTypeString);
            row.AddText(filter.Value1);
            row.AddText(filter.Value2);

            var applyFilterString = Utils.Utils.EnumResourceString(Properties.Resources.ResourceManager, filter.ApplyFilter);
            row.AddText(applyFilterString);
            row.Tag = filter;
        }

        private void UpdateActions()
        {
            contextButtons.EditButtonEnabled = lvFilters.Selection.Count == 1;
            contextButtons.RemoveButtonEnabled = lvFilters.Selection.Count >= 1;
        }



        public bool DataIsModified()
        {
            return dataIsModified;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
        }

        public bool SaveData()
        {
            return true;
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            lvFilters.ContextMenuStrip.Items.Clear();
            contextButtons.AddToContextMenu(lvFilters.ContextMenuStrip, 100, contextButtons_EditButtonClicked, contextButtons_AddButtonClicked, contextButtons_RemoveButtonClicked);
        }

        private void contextButtons_AddButtonClicked(object sender, EventArgs e)
        {
            filterDialog.FromTableFilter = new JscSubJobFromTableFilter { JscSubJob = viewPagesContext.SubJob };
            if (filterDialog.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().Save(PluginEntry.DataModel, filterDialog.FromTableFilter);
                var row = new Row();
                SetRow(row, filterDialog.FromTableFilter);
                lvFilters.AddRow(row);
                lvFilters.AutoSizeColumns();
                lvFilters.Sort();
                dataIsModified = true;
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "FromTableFilter", null, null);
            }
        }

        private void contextButtons_EditButtonClicked(object sender, EventArgs e)
        {
            if (lvFilters.Selection.Count != 1)
            {
                return;
            }

            var row = lvFilters.Rows[lvFilters.Selection.FirstSelectedRow];
            filterDialog.FromTableFilter = (JscSubJobFromTableFilter)row.Tag;
            if (filterDialog.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().Save(PluginEntry.DataModel, filterDialog.FromTableFilter);
                SetRow(row, filterDialog.FromTableFilter);
                lvFilters.AutoSizeColumns();
                dataIsModified = true;
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "FromTableFilter", null, null);
            }
        }

        private void contextButtons_RemoveButtonClicked(object sender, EventArgs e)
        {
            string msg = null;
            if (lvFilters.Selection.Count == 1)
            {
                var filter = (JscSubJobFromTableFilter)lvFilters.Rows[lvFilters.Selection.FirstSelectedRow].Tag;
                msg = string.Format(Properties.Resources.SubJobFromTableFilterRemoveOneField, filter.JscField.FieldName);
            }
            else if (lvFilters.Selection.Count > 1)
            {
                msg = string.Format(Properties.Resources.SubJobFromTableFilterRemoveManyFields, lvFilters.Selection.Count);
            }


            if (msg != null)
            {
                if (QuestionDialog.Show(msg, Properties.Resources.SubJobFromTableFilterRemoveHeader) == DialogResult.Yes)
                {
                    var rowsToRemove = new List<Row>();
                    for (int i = 0; i < lvFilters.Selection.Count; i++)
                    {
                        rowsToRemove.Add(lvFilters.Rows[lvFilters.Selection.GetRowIndex(i)]);
                    }
                    foreach (var row in rowsToRemove)
                    {
                        DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().Delete(PluginEntry.DataModel, (JscSubJobFromTableFilter)row.Tag);
                        lvFilters.RemoveRow(lvFilters.Rows.IndexOf(row));
                    }
                    lvFilters.AutoSizeColumns();
                    dataIsModified = true;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "FromTableFilter", null, null);
                }
            }
        }

        private void lvFilters_SelectionChanged(object sender, EventArgs e)
        {
            UpdateActions();
        }

        private void lvFilters_RowDoubleClick(object sender, LSOne.Controls.EventArguments.RowEventArgs args)
        {
            if (contextButtons.EditButtonEnabled)
            {
                contextButtons_EditButtonClicked(contextButtons, EventArgs.Empty);
            }
        }
    }
}
