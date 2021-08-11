using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Reports;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.ReportViewer;
using LSOne.Controls.Rows;

namespace LSRetail.SiteManager.Plugins.ReportViewer.Dialogs
{
    public partial class ReportManagementDialog : DialogBase
    {
        RecordIdentifier reportID;

        private Report report;
        private bool contextStateChanged = false;


        public ReportManagementDialog(RecordIdentifier reportID)
            : base()
        {
            InitializeComponent();

            this.reportID = reportID;
            report = Providers.ReportData.Get(PluginEntry.DataModel, reportID);
            tbReportDescription.Text = report.Text;
            cmbCategory.SelectedIndex = (int)report.ReportCategory;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadContexts();
        }
        
        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = tbReportDescription.Text.Trim() != string.Empty &&
                               (contextStateChanged
                            || cmbCategory.SelectedIndex != (int)report.ReportCategory
                            || tbReportDescription.Text != report.Text);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach(Row row in lvContexts.Rows)
            {
                ReportContext currentContext = (ReportContext)row.Tag;

                if(((LSOne.Controls.Cells.CheckBoxCell)row[0]).Checked != currentContext.Active)
                {
                    currentContext.Active = ((LSOne.Controls.Cells.CheckBoxCell)row[0]).Checked;

                    Providers.ReportContextData.Save(PluginEntry.DataModel, currentContext);
                }
            }

            report.Text = tbReportDescription.Text.Trim();
            report.ReportCategory = (ReportCategory)cmbCategory.SelectedIndex;
            Providers.ReportData.Save(PluginEntry.DataModel, report);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void LoadContexts()
        {
            List<ReportContext> reports;
            Row row;
            LSOne.Controls.Cells.CheckBoxCell cell;

            reports = Providers.ReportContextData.GetList(PluginEntry.DataModel, reportID);

            foreach(ReportContext reportContext in reports)
            {
                row = new Row();

                cell = new LSOne.Controls.Cells.CheckBoxCell(reportContext.Description, reportContext.Active);
                cell.CheckboxLeftIndent = 4;

                row.AddCell(cell);

                row.Tag = reportContext;

                lvContexts.AddRow(row);
            }
        }

        private void lvContexts_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            contextStateChanged = StateHasChanged();
            CheckEnabled(sender, e);
        }

        private bool StateHasChanged()
        {
            foreach(Row row in lvContexts.Rows)
            {
                ReportContext currentContext = (ReportContext)row.Tag;

                if(((LSOne.Controls.Cells.CheckBoxCell)row[0]).Checked != currentContext.Active)
                {
                    return true;
                }
            }

            return false;
        }

        private void lvContexts_CellAction(object sender, LSOne.Controls.EventArguments.CellEventArgs args)
        {
            contextStateChanged = StateHasChanged();
            CheckEnabled(sender, args);
        }
    }
}
