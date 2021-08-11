using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Columns;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Administration.DataLayer;
using LSOne.ViewPlugins.Administration.DataLayer.DataEntities;
using LSOne.ViewPlugins.Administration.Properties;
using LSOne.ViewPlugins.Administration.QueryResults;
using ListView = LSOne.Controls.ListView;

namespace LSOne.ViewPlugins.Administration.Views
{
    public partial class AuditLogViewer : ViewBase
    {
        private WeakReference viewToAudit;
        private List<AuditLogResult> results;
        private List<AuditDescriptor>   contexts;
        private bool lockEvents;
        private Style changedCellStyle;
        private List<bool> sectionsExpanded;

        public AuditLogViewer(ViewBase sheetToAudit)
        {
            results  = new List<AuditLogResult>();
            contexts = new List<AuditDescriptor>();

            lockEvents = false;

            this.viewToAudit = new WeakReference(sheetToAudit);

            Attributes = 
                ViewAttributes.Close | 
                ViewAttributes.Help | 
                ViewAttributes.ContextBar;

            InitializeComponent();

            lvAuditLogs.ContextMenuStrip = new ContextMenuStrip();
            lvAuditLogs.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
        }

        protected override string LogicalContextName
        {
            get
            {
                return ((ViewBase)viewToAudit.Target).HeaderText;
            }
        }

        private void Expand(object sender, EventArgs args)
        {
            if(lvAuditLogs.Selection.FirstSelectedRow >= 0)
            {
                ((CollapsableCell)lvAuditLogs.Row(lvAuditLogs.Selection.FirstSelectedRow)[0]).SetCollapsed(lvAuditLogs, lvAuditLogs.Selection.FirstSelectedRow, false);
            }
        }

        private void Collapse(object sender, EventArgs args)
        {
            if (lvAuditLogs.Selection.FirstSelectedRow >= 0)
            {
                ((CollapsableCell)lvAuditLogs.Row(lvAuditLogs.Selection.FirstSelectedRow)[0]).SetCollapsed(lvAuditLogs, lvAuditLogs.Selection.FirstSelectedRow, true);
            }
        }

        private void FilterByThisUser(object sender, EventArgs args)
        {
            if (lvAuditLogs.Selection.FirstSelectedRow >= 0)
            {
                tbUser.Text = lvAuditLogs.Row(lvAuditLogs.Selection.FirstSelectedRow)[0].Text;
            }
        }

        private void FilterByThisDate(object sender, EventArgs args)
        {
            if (lvAuditLogs.Selection.FirstSelectedRow >= 0)
            {
                if (lvAuditLogs.Row(lvAuditLogs.Selection.FirstSelectedRow).Tag is AuditLogRecord)
                {
                    AuditLogRecord record = (AuditLogRecord)lvAuditLogs.Row(lvAuditLogs.Selection.FirstSelectedRow).Tag;

                    dtpFrom.Value = record.Date;
                    dtpFrom.Checked = true;

                    dtpTo.Value = record.Date;
                    dtpTo.Checked = true;
                }
            }

            
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvAuditLogs.ContextMenuStrip;

            menu.Items.Clear();

            if (lvAuditLogs.Selection.FirstSelectedRow >= 0)
            {
                if (lvAuditLogs.Row(lvAuditLogs.Selection.FirstSelectedRow) is SubRow && !(lvAuditLogs.Row(lvAuditLogs.Selection.FirstSelectedRow) is HeaderRow))
                {
                    ExtendedMenuItem item = new ExtendedMenuItem(
                       Resources.FilterByThisUser,
                       200,
                       FilterByThisUser);

                    menu.Items.Add(item);

                    item = new ExtendedMenuItem(
                       Resources.FilterByThisDate,
                       210,
                       FilterByThisDate);

                    menu.Items.Add(item);
                }
                else if (!(lvAuditLogs.Row(lvAuditLogs.Selection.FirstSelectedRow) is SubRow))
                {
                    if (((CollapsableCell)lvAuditLogs.Row(lvAuditLogs.Selection.FirstSelectedRow)[0]).Collapsed)
                    {
                        ExtendedMenuItem item = new ExtendedMenuItem(
                            Resources.Expand,
                            200,
                            Expand);

                        item.Default = true;

                        menu.Items.Add(item);
                    }
                    else
                    {
                        ExtendedMenuItem item = new ExtendedMenuItem(
                            Resources.Collapse,
                            200,
                            Collapse);

                        item.Default = true;

                        menu.Items.Add(item);
                    }
                }
            }

    

            PluginEntry.Framework.ContextMenuNotify("AuditLogList", lvAuditLogs.ContextMenuStrip, lvAuditLogs);

            e.Cancel = (menu.Items.Count == 0);
        }

        protected override void LoadData(bool isRevert)
        {
            AuditLogResult result;
            SortedList<string,string> names = new SortedList<string,string>();

            sectionsExpanded = new List<bool>();

            changedCellStyle = new Style(lvAuditLogs.DefaultStyle);
            changedCellStyle.BackColor = ColorPalette.RedLight;
            changedCellStyle.Font = new Font(lvAuditLogs.DefaultStyle.Font, FontStyle.Bold);

            if (viewToAudit.IsAlive)
            {
                CopyParentDescriptorsFromView((ViewBase)viewToAudit.Target, true);

                ((ViewBase)viewToAudit.Target).GetAuditDescriptors(contexts);

                
                HeaderText = LogicalContextName;
                
            }

            DateTime today = DateTime.Now;

            var auditProvider = DataProviderFactory.Instance.Get<IAuditingData, AuditLogResult>();
            foreach (AuditDescriptor context in contexts)
            {
                result =  auditProvider.GetAuditLog(PluginEntry.DataModel, context.Name, context.Identifier, tbUser.Text, today.AddMonths(-1).Date, today.AddDays(1).Date);

                results.Add(result);

                var row = new Row {Tag = sectionsExpanded.Count};
                sectionsExpanded.Add(contexts.Count == 1);

                FillView(row,context.Description,context, lvAuditLogs, result, names, "", null, null);

                row.BackColor = ColorPalette.GrayLight;

                row.LockLeft = true;
                lvAuditLogs.AddRow(row);

                if (sectionsExpanded[sectionsExpanded.Count - 1])
                {
                    ((CollapsableCell)row[0]).SetCollapsed(lvAuditLogs, lvAuditLogs.RowCount - 1, false);
                }
            }

            //HeaderIcon = Properties.Resources.ViewAuditLogImage16;

            lvAuditLogs.AutoSizeColumns();
        }

        private void FillView(
            Row row,
            string description,
            AuditDescriptor context,
            ListView view,
            AuditLogResult result,
            SortedList<string, string> names,
            string userFilter,
            Date dateFromFilter,
            Date dateToFilter)
        {
            int extraColumnCount;
            object value;

            SubRow subRow;
            SubRow lastRow;
            List<SubRow> rows = new List<SubRow>();
            
            ColumnCollection subTableColumns = new ColumnCollection();

            subTableColumns.Add(new Column(Properties.Resources.User, true));
            subTableColumns.Add(new Column(Properties.Resources.Date, true));

            foreach (string columnName in result.ColumnNames)
            {
                subTableColumns.Add(new Column(columnName, true));
            }

            extraColumnCount = result.ColumnNames.Count;

            foreach (AuditLogRecord record in result.Records)
            {
                if (names != null)
                {
                    if (!names.Keys.Contains(record.UserLogin))
                    {
                        names.Add(record.UserLogin, record.UserLogin);
                    }
                }

                if (dateFromFilter != null)
                {
                    if (!dateFromFilter.IsEmpty)
                    {
                        if (record.Date.Date < dateFromFilter.DateTime.Date)
                        {
                            continue;
                        }
                    }

                    if (!dateToFilter.IsEmpty)
                    {
                        if (record.Date.Date > dateToFilter.DateTime.Date)
                        {
                            continue;
                        }
                    }
                }

                if (userFilter != "")
                {
                    if (record.UserLogin != userFilter)
                    {
                        continue;
                    }
                }

                subRow = new SubRow(subTableColumns);
                
                subRow.AddText(record.UserLogin);
                subRow.AddText(record.Date.ToShortDateString() + " - " + record.Date.ToShortTimeString());

                subRow.Tag = record;

                for (int i = 0; i < extraColumnCount; i++)
                {
                    string text;
                    value = record.FieldValues[i];

                    if (value is DateTime)
                    {
                        subRow.AddText(((DateTime)(value)).ToShortDateString() + " - " + ((DateTime)(value)).ToShortTimeString());
                    }
                    else
                    {
                        text = value.ToString();

                        if (text == "<Not touched>")
                        {
                            text = Properties.Resources.NotTouched;
                        }

                        subRow.AddText(text);
                    }
                }

                if (context.DisplaysOneRecordOnly)
                {
                    
                    if (rows.Count > 0)
                    {
                        lastRow = rows[rows.Count - 1];

                        for (int i = 2; i < lastRow.CellCount; i++)
                        {
                            if (subRow[(uint)i].Text != Properties.Resources.NotTouched)
                            {
                                CompareSubItems(subRow, lastRow, i, rows, rows.Count - 1);
                            }
                        }
                    }
                }

                rows.Add(subRow);
            }

            CollapsableCell cell = new CollapsableCell(subTableColumns, rows, description, true);
            cell.HeaderRowHeight = 26;

            row.AddCell(cell);

            int rowWidth = ((CollapsableCell)row[0]).AutoSizeColumns(lvAuditLogs);

            while (lvAuditLogs.Columns.TotalWidth < rowWidth)
            {
                var column = new Column {Width = 50, Clickable = false, AutoSize = false};

                lvAuditLogs.Columns.Add(column);
            }
        }

        private void CompareSubItems(SubRow item, SubRow lastItem, int index, List<SubRow> rows, int listPosition)
        {
            if (lastItem[(uint)index].Text == Properties.Resources.NotTouched)
            {
                if (listPosition - 1 >= 0)
                {
                    CompareSubItems(item, rows[listPosition - 1], index, rows, listPosition - 1);
                }
            }
            else
            {
                if (lastItem[(uint)index].Text != item[(uint)index].Text)
                {
                    item[(uint)index].SetStyle(changedCellStyle);
                }
            }
        }

        

        /*private void ShowParentSheet_Handler(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(sheetToAudit);   
        }*/

        public override RecordIdentifier ID
        {
            get
            {
                if (viewToAudit.IsAlive)
                {
                    return ((ViewBase)viewToAudit.Target).ID;
                }
                return RecordIdentifier.Empty;
            }
        }

       
        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            if (lockEvents) return;

            tbUser_TextChanged(this, EventArgs.Empty);
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            if (lockEvents) return;

            tbUser_TextChanged(this, EventArgs.Empty);
        }

        private void AuditLogViewer_Load(object sender, EventArgs e)
        {
            lockEvents = true;

            DateTime today = DateTime.Now;

            dtpFrom.Value = today.AddMonths(-1).Date;
            dtpTo.Value = today.AddDays(1).Date;

            lockEvents = false;
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.SecurityManageAuditLogs))
                {
                    arguments.Add(new ContextBarItem(
                            Properties.Resources.ManageAuditLogs, 
                            ManageAuditLogs_Handler), 
                        300);
                }
            }
        }

        private void ManageAuditLogs_Handler(object sender, ContextBarClickEventArguments args)
        {
            PluginOperations.ManageAuditLog(this, EventArgs.Empty);
        }

        private void tbUser_TextChanged(object sender, EventArgs e)
        {
            //Filter();
            var dateFrom = dtpFrom.Checked ? new Date(dtpFrom.Value) : Date.Empty;
            var dateTo = dtpTo.Checked ? new Date(dtpTo.Value) : Date.Empty;

            lvAuditLogs.ClearRows();
            lvAuditLogs.Columns.Clear();

            var column = new Column {AutoSize = true};

            lvAuditLogs.Columns.Add(column);

            var auditProvider = DataProviderFactory.Instance.Get<IAuditingData, AuditLogResult>();
            for (int i = 0; i < results.Count; i++)
            {
                var row = new Row {Tag = i};

                results[i] = auditProvider.GetAuditLog(PluginEntry.DataModel, contexts[i].Name, contexts[i].Identifier, tbUser.Text, dtpFrom.Value.Date, dtpTo.Value.Date.Add(new TimeSpan(23, 59, 59)));

                FillView(row, contexts[i].Description, contexts[i], lvAuditLogs, results[i], null, tbUser.Text, dateFrom, dateTo);

                row.BackColor = ColorPalette.GrayLight;

                lvAuditLogs.AddRow(row);

                if (sectionsExpanded[i])
                {
                    ((CollapsableCell)row[0]).SetCollapsed(lvAuditLogs, lvAuditLogs.RowCount - 1, false);
                }
            }

            lvAuditLogs.AutoSizeColumns();
        }

        private void lvAuditLogs_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (args.Row[0] is CollapsableCell)
            {
                ((CollapsableCell)args.Row[0]).SetCollapsed(lvAuditLogs,args.RowNumber,!((CollapsableCell)args.Row[0]).Collapsed);
            }
        }

        private void lvAuditLogs_RowExpanded(object sender, RowEventArgs args)
        {
            sectionsExpanded[(int)args.Row.Tag] = true;
        }

        private void lvAuditLogs_RowCollapsed(object sender, RowEventArgs args)
        {
            sectionsExpanded[(int)args.Row.Tag] = false;
        }
    }
}
