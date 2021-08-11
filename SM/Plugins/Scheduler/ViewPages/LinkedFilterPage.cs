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

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    public partial class LinkedFilterPage : UserControl, IDetailView
    {
        private JscLinkedTable linkedTable;
        private LinkedFilterDialog linkedFilterDialog = new LinkedFilterDialog();

        private bool dataIsModified;

        public class InternalContext
        {
            public JscLinkedTable LinkedTable { get; set; }
        }

        public LinkedFilterPage()
        {
            InitializeComponent();

            lvFilters.ContextMenuStrip = new ContextMenuStrip();
            lvFilters.ContextMenuStrip.Opening += lvFilters_Opening;
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            if (!isRevert)
            {
                linkedTable = ((InternalContext)internalContext).LinkedTable;
            }
            ObjectToForm(linkedTable);

            UpdateActions();
        }

        private void ObjectToForm(JscLinkedTable linkedTable)
        {
            tbToDesign.Text = linkedTable.ToJscTableDesign.TableName;
            PopulateListBox(linkedTable.JscLinkedFilters);
            dataIsModified = false;
        }

        private void PopulateListBox(IEnumerable<JscLinkedFilter> linkedFilters)
        {
            Cursor.Current = Cursors.WaitCursor;
            lvFilters.ClearRows();
            if (linkedFilters != null)
                foreach (var linkedFilter in linkedFilters)
                {
                    var row = new Row();
                    SetRow(row, linkedFilter);
                    lvFilters.AddRow(row);
                }
            lvFilters.AutoSizeColumns();
            lvFilters.Sort();
        }

        private void SetRow(Row row, JscLinkedFilter linkedFilter)
        {
            row.Clear();
            if (linkedFilter.LinkedJscFieldDesign == null && linkedFilter.LinkedField != null && !linkedFilter.LinkedField.IsEmpty)
            {
                linkedFilter.LinkedJscFieldDesign = DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetFieldDesign(PluginEntry.DataModel, linkedFilter.LinkedField);
            }
            row.AddText(linkedFilter.LinkedJscFieldDesign == null ? string.Empty : linkedFilter.LinkedJscFieldDesign.FieldName);

            //row.AddText(Utils.Utils.EnumResourceString(Properties.Resources.ResourceManager, linkedFilter.LinkType));
            if (linkedFilter.ToJscFieldDesign == null && linkedFilter.ToField != null && !linkedFilter.ToField.IsEmpty)
            {
                linkedFilter.ToJscFieldDesign = DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetFieldDesign(PluginEntry.DataModel, linkedFilter.ToField);
            }
            row.AddText(linkedFilter.ToJscFieldDesign != null ? linkedFilter.ToJscFieldDesign.FieldName : string.Empty);
            //row.AddText(linkedFilter.Filter);
            row.Tag = linkedFilter;
        }

        private void UpdateActions()
        {
            int selectedCount = lvFilters.Selection.Count;

            btnsEditAddRemove.EditButtonEnabled = selectedCount == 1;
            btnsEditAddRemove.RemoveButtonEnabled = selectedCount >= 1;
        }

        public bool DataIsModified()
        {
            return dataIsModified;
        }

        public bool SaveData()
        {
            DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().Save(PluginEntry.DataModel, linkedTable);

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            // TODO: Implement
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            // TODO: Implement
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var selectedRow = lvFilters.Rows[lvFilters.Selection.FirstSelectedRow];
            JscLinkedFilter linkedFilter = (JscLinkedFilter)selectedRow.Tag;

            if (linkedFilterDialog.ShowDialog(PluginEntry.Framework.MainWindow, linkedFilter, linkedTable.FromJscTableDesign, linkedTable.ToJscTableDesign) == DialogResult.OK)
            {
                SetRow(selectedRow, linkedFilter);
                lvFilters.AutoSizeColumns();
                dataIsModified = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (linkedFilterDialog.ShowDialog(PluginEntry.Framework.MainWindow, null, linkedTable.FromJscTableDesign, linkedTable.ToJscTableDesign) == DialogResult.OK)
            {
                JscLinkedFilter linkedFilter = linkedFilterDialog.LinkedFilter;
                if (linkedTable.JscLinkedFilters == null)
                {
                    linkedTable.JscLinkedFilters = new List<JscLinkedFilter>();
                }
                linkedFilter.JscLinkedTable = linkedTable;
                linkedTable.JscLinkedFilters.Add(linkedFilter);
                DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().Save(PluginEntry.DataModel, linkedFilter);
                
                Row row = new Row();
                SetRow(row, linkedFilter);
                lvFilters.AddRow(row);
                lvFilters.AutoSizeColumns();
                lvFilters.Sort();
                dataIsModified = true;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            string msg;
            if (lvFilters.Selection.Count == 1)
            {
                msg = Properties.Resources.LinkedFilterRemoveMsg;
            }
            else if (lvFilters.Selection.Count > 1)
            {
                msg = string.Format(Properties.Resources.LinkedFilterRemoveManyMsg, lvFilters.Selection.Count);
            }
            else
            {
                return;
            }

            if (QuestionDialog.Show(msg, Properties.Resources.LinkedFilterRemoveHeader) == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                var rowsToRemove = new List<Row>();
                for (int i = 0; i < lvFilters.Selection.Count; i++)
                {
                    rowsToRemove.Add(lvFilters.Rows[lvFilters.Selection.GetRowIndex(i)]);
                }
                foreach (var row in rowsToRemove)
                {
                    var linkedFilter = (JscLinkedFilter) row.Tag;
                    DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().DeleteLinkedFilter(PluginEntry.DataModel, linkedFilter);
                    lvFilters.RemoveRow(lvFilters.Rows.IndexOf(row));
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "LinkedFilter", linkedFilter.ID, null);
                }
                lvFilters.AutoSizeColumns();
                dataIsModified = true;
            }
        }

        private void lvFilters_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvFilters.ContextMenuStrip;

            menu.Items.Clear();

            item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(btnEdit_Click));

            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsEditAddRemove.EditButtonEnabled;
            item.Default = true;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnAdd_Click));

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;

            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnRemove_Click));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("LinkedFilterPageListViewMenu", lvFilters.ContextMenuStrip, lvFilters);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvFilters_SelectionChanged(object sender, EventArgs e)
        {
            UpdateActions();
        }

        private void lvFilters_RowDoubleClick(object sender, LSOne.Controls.EventArguments.RowEventArgs args)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }
    }
}
