using System;
using System.Drawing;
using DevExpress.XtraReports.UI;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.XtraReportsViewer.Views
{
    public partial class XtraReportViewerView : ViewBase
    {
        RecordIdentifier uniqueID;
        XtraReport report;

        public XtraReportViewerView(RecordIdentifier reportUniqueID, XtraReport report, string headerText, Image image)
            : this()
        {
            uniqueID = reportUniqueID;
            this.report = report;
            this.HeaderText = headerText;
            this.HeaderIcon = image;
        }

        public XtraReportViewerView()
        {
            report = null;
            uniqueID = RecordIdentifier.Empty;

            InitializeComponent();

            Attributes = ViewAttributes.Help | ViewAttributes.Close | ViewAttributes.Print | ViewAttributes.PageSetup;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.ShowProgress();

            timer1.Start();
        }

        protected override string LogicalContextName
        {
            get
            {
                return HeaderText;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        {
                return uniqueID;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            
        }

      

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        protected override void OnPrint()
        {
            if (reportViewerControl1.Report != null)
            {
                reportViewerControl1.Report.PrintDialog();
            }
        }

        protected override void OnPageSetup()
        {
            if (reportViewerControl1.Report != null)
            {
                reportViewerControl1.Report.ShowPageSetupDialog();

                PluginEntry.PaperKind = reportViewerControl1.Report.PaperKind;
            }
        }

        private void reportViewerControl1_CloseSheet(object sender, EventArgs e)
        {
            this.ManualClose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            if (report is IReport)
            {
                ((IReport)report).Run();
            }

            report.PaperKind = PluginEntry.PaperKind;

            if (PluginEntry.StockReport != null)
            {
                report.Margins.Left = PluginEntry.StockReport.Margins.Left;
                report.Margins.Top = PluginEntry.StockReport.Margins.Top;
                report.Margins.Right = PluginEntry.StockReport.Margins.Right;
                report.Margins.Bottom = PluginEntry.StockReport.Margins.Bottom;

                report.Landscape = PluginEntry.StockReport.Landscape;
            }

            reportViewerControl1.Report = report;

            this.HideProgress();

            reportViewerControl1.Visible = true;

            
        }
    }
}
