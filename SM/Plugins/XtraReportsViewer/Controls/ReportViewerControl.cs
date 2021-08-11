using System;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.XtraReportsViewer.Controls
{
    public partial class ReportViewerControl : UserControl
    {
        DevExpress.XtraReports.UI.XtraReport report;


        public ReportViewerControl()
        {
            InitializeComponent();
        }

        public DevExpress.XtraReports.UI.XtraReport Report
        {
            get { return report; }
            set 
            {
                if (value != null)
                {
                    report = value;

                    // Bind the report's printing system to the print control.
                    printControl1.PrintingSystem = report.PrintingSystem;

                    report.CreateDocument();
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

    }
}
