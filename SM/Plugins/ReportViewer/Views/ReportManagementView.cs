using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Reports;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.ReportViewer.Dialogs;
using LSRetail.SiteManager.Plugins.ReportViewer.Dialogs;
using LSOne.Controls.Rows;
using LSOne.Controls;

namespace LSOne.ViewPlugins.ReportViewer.Views
{
    public partial class ReportManagementView : ViewBase
    {
        private RecordIdentifier selectedID = RecordIdentifier.Empty;

        public ReportManagementView(RecordIdentifier selectedID)
            : this()
        {
            this.selectedID = selectedID.ToString();
        }

        public ReportManagementView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.ContextBar |
                ViewAttributes.Audit |
                ViewAttributes.Help |
                ViewAttributes.Close;


            lvReports.ContextMenuStrip = new ContextMenuStrip();
            lvReports.ContextMenuStrip.Opening += lvReports_Opening;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageReports);
            btnsContextButtons.AddButtonEnabled = !ReadOnly;

            lvReports.SetSortColumn(0, true);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.ManageReports;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return RecordIdentifier.Empty;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            

            if (isRevert)
            {
                
            }

            HeaderText = Properties.Resources.ManageReports;
            LoadReportList();
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "Report":
                    LoadReportList();
                    break;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string line;
            string trimmedLine;
            bool startedSqlPart = false;

            AddReportDialog dlg = new AddReportDialog();

            if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                Report report = new Report();
                report.ReportCategory = dlg.ReportCategory;
                List<ReportContext> contexts = new List<ReportContext>();
                List<ReportEnumValue> reportEnums = new List<ReportEnumValue>();

                try
                {
                    StreamReader sr = File.OpenText(dlg.ReportDescriptionFile.AbsolutePath);

                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();

                        if (line.TrimStart().Left(2) != "//") // If line starts with a comment then we skip the damn thing
                        {
                            if (startedSqlPart)
                            {
                                if (line == "")
                                {
                                    report.SqlData += line;
                                }
                                else
                                {
                                    report.SqlData = report.SqlData + System.Environment.NewLine + line;
                                }
                            }
                            else
                            {
                                trimmedLine = line.Trim();

                                if (line != "")
                                {
                                    if (line.Left(9) == "ReportID:")
                                    {
                                        report.ID = new Guid(line.Substring(9).TrimStart());
                                    }
                                    else if (line.Left(5) == "Name:")
                                    {
                                        report.Text = line.Substring(5).TrimStart();
                                    }
                                    else if (line.Left(12) == "Description:")
                                    {
                                        report.Description = line.Substring(12).TrimStart();
                                    }
                                    else if (line.Left(11) == "LanguageID:")
                                    {
                                        report.LanguageID = line.Substring(11).TrimStart();
                                    }
                                    else if (line.Left(9) == "Contexts:")
                                    {
                                        ReportContext ctx;
                                        string[] contextParts = line.Substring(9).Split(',');

                                        for (int i = 0; i < contextParts.Length; i++)
                                        {
                                            ctx = new ReportContext();

                                            ctx.Text = contextParts[i].Trim();
                                            ctx.Active = true;

                                            contexts.Add(ctx);
                                        }
                                    }
                                    else if (line.Left(5) == "Enum:")
                                    {
                                        string[] segments = line.Substring(5).Split(';');
                                        string[] values;
                                        string enumName = "";
                                        string enumLabel = "";

                                        if (segments.Length > 2)
                                        {
                                            enumName = segments[0].Trim();
                                            enumLabel = segments[1].Trim();
                                            values = segments[2].Split(',');

                                            for (int i = 0; i < values.Length; i++)
                                            {
                                                string[] valueParts;

                                                valueParts = values[i].Split('=');
                                                
                                                if (valueParts.Length > 1)
                                                {
                                                    ReportEnumValue value = new ReportEnumValue();

                                                    value.EnumName = enumName;
                                                    value.Label = enumLabel;
                                                    value.Text = valueParts[0].Trim();
                                                    value.EnumValue = Convert.ToInt32(valueParts[1].Trim());

                                                    reportEnums.Add(value);
                                                }
                                            }
                                        }
                                    }
                                    else if (line.Left(20) == "IsSiteServiceReport:")
                                    {
                                        report.SiteServiceReport = line.Substring(20).TrimStart() == "true";
                                    }
                                    else if (line.Left(18) == "Stored procedures:")
                                    {
                                        startedSqlPart = true;
                                    }
                                }
                            }
                        }
                    }

                    sr.Close();

                    FileStream fs = File.OpenRead(dlg.ReportFile.AbsolutePath);

                    report.ReportData = new byte[fs.Length];

                    fs.Read(report.ReportData, 0, (int)fs.Length);

                    fs.Close();

                    IReportData reportData = Providers.ReportData;
                    if(reportData.Exists(PluginEntry.DataModel,report.ID))
                    {
                        if (QuestionDialog.Show(
                            Properties.Resources.ImportReportQuestion,
                            Properties.Resources.ImportReport) == DialogResult.No)
                        {
                            return;
                        }
                    }

                    reportData.Save(PluginEntry.DataModel, report);

                    IReportContextData reportContextData = Providers.ReportContextData;
                    reportContextData.DeleteAllForReport(PluginEntry.DataModel, report.ID, report.LanguageID);

                    foreach (ReportContext context in contexts)
                    {
                        context.ReportID = report.ID;
                        reportContextData.Save(PluginEntry.DataModel, context);
                    }

                    IReportEnumValueData reportEnumValueData = Providers.ReportEnumValueData;
                    foreach (ReportEnumValue enumValue in reportEnums)
                    {
                        enumValue.ReportID = report.ID;
                        enumValue.LanguageID = report.LanguageID;

                        reportEnumValueData.Save(PluginEntry.DataModel, enumValue);
                    }

                    // Tell the home screen of the change
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.VariableChanged, "Home", "RetailReports", null);

                }
                catch (Exception ex)
                {
                    MessageDialog.Show(Properties.Resources.UnknownErrorOccured + ex.Message);
                }

                LoadReportList();
            }
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            RecordIdentifier selectedReportID = ((DataEntity)lvReports.Rows[lvReports.Selection.FirstSelectedRow].Tag).ID;

            ReportManagementDialog dlg = new ReportManagementDialog(selectedReportID);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // Tell the home screen of the change
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.VariableChanged, "Home", "RetailReports", null);
            }
        }

        private void lvReports_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        void lvReports_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvReports.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here

            item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnEdit_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnAdd_Click);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemove_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("HardwareProfileList", lvReports.ContextMenuStrip, lvReports);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteReportQuestion,
                Properties.Resources.DeleteReport) == DialogResult.No)
            {
                return;
            }

            RecordIdentifier selectedReportID = ((DataEntity)lvReports.Rows[lvReports.Selection.FirstSelectedRow].Tag).ID;

            Providers.ReportData.Delete(PluginEntry.DataModel, selectedReportID);

            LoadReportList();

            // Tell the home screen of the change
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.VariableChanged, "Home", "RetailReports", null);
        }

        private void LoadReportList()
        {
            List<ReportListItem> reports;
            Row row;

            lvReports.ClearRows();

            IReportData reportData = Providers.ReportData;

            reports = reportData.GetList(PluginEntry.DataModel);

            foreach (ReportListItem report in reports)
            {
                row = new Row();

                row.AddText(report.Text);
                row.AddText(report.Description);
                row.AddCell(new LSOne.Controls.Cells.CheckBoxCell(!report.SystemReport, false, LSOne.Controls.Cells.CheckBoxCell.CheckBoxAlignmentEnum.Center));
                row.AddText(report.ReportCategory.ToCultureString());

                row.Tag = report;

                lvReports.AddRow(row);

                if (selectedID == (report.ID))
                {
                    lvReports.Selection.Set(lvReports.RowCount - 1);
                }
            }

            lvReports.Sort();
            lvReports.AutoSizeColumns();
            lvReports_SelectionChanged(this, EventArgs.Empty);
        }
        
        private void lvReports_SelectionChanged(object sender, EventArgs e)
        {
            btnsContextButtons.EditButtonEnabled = (lvReports.Selection.Count > 0) && ((LSOne.Controls.Cells.CheckBoxCell)lvReports.Rows[lvReports.Selection.FirstSelectedRow][2]).Checked;
            btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled;
        }
    }
}
