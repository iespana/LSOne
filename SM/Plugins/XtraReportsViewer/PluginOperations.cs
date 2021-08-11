using System;
using System.Drawing;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Settings;
using DevExpress.XtraReports.UI;

namespace LSOne.ViewPlugins.XtraReportsViewer
{
    internal class PluginOperations
    {
        public static void ShowHelloWorldSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.XtraReportViewerView());
        }

        public static void ShowReportSheet(RecordIdentifier reportUniqueID, XtraReport report,string headerText,Image image)
        {
            PluginEntry.Framework.ViewController.Add(new Views.XtraReportViewerView(reportUniqueID,report,headerText,image));
        }

        public static void PrintReport(XtraReport report)
        {
            if (PluginEntry.StockReport != null)
            {
                report.Margins.Left = PluginEntry.StockReport.Margins.Left;
                report.Margins.Top = PluginEntry.StockReport.Margins.Top;
                report.Margins.Right = PluginEntry.StockReport.Margins.Right;
                report.Margins.Bottom = PluginEntry.StockReport.Margins.Bottom;

                report.Landscape = PluginEntry.StockReport.Landscape;
            }

            report.PaperKind = PluginEntry.PaperKind;
            report.PrintDialog();
        }

        public static void PageSetup()
        {
            if (PluginEntry.StockReport == null)
            {
                PluginEntry.StockReport = new XtraReport();
            }

            PluginEntry.StockReport.PaperKind = PluginEntry.PaperKind;
            PluginEntry.StockReport.ShowPageSetupDialog();
            PluginEntry.PaperKind = PluginEntry.StockReport.PaperKind;
        }

        public static void SettingsHandler(SettingsBase stream, bool write)
        {
            if (write)
            {
                stream.Write("Client.Plugins.XtraReportsViewer.PaperKind", ((int)PluginEntry.PaperKind).ToString());
            }
            else
            {
                PluginEntry.PaperKind = (System.Drawing.Printing.PaperKind)Convert.ToInt32(stream.Read("Client.Plugins.XtraReportsViewer.PaperKind", ((int)System.Drawing.Printing.PaperKind.A4).ToString()));
            }
        }
    }
}
