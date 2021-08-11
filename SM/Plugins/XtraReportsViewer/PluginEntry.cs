using System.Drawing;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.XtraReportsViewer
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;
        internal static System.Drawing.Printing.PaperKind PaperKind = System.Drawing.Printing.PaperKind.A4;
        internal static DevExpress.XtraReports.UI.XtraReport StockReport = null;

        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.XtraReportsViewer; }
        }


        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            if (message == "CanDisplayXtraReport")
            {
                return true;
            }

            return false;
        }

        public object Message(object sender, string message, object parameters)
        {
            if (message == "ShowReport")
            {
                var reportUniqueID = RecordIdentifier.FromObject(((object[])parameters)[0]);
                var report = (DevExpress.XtraReports.UI.XtraReport)((object[])parameters)[1];
                var headerText = (string)((object[])parameters)[2];
                var image = (Image)((object[])parameters)[3];

                PluginOperations.ShowReportSheet(
                    reportUniqueID,
                    report,
                    headerText,
                    image);
            }
            else if (message == "PrintReport")
            {
                PluginOperations.PrintReport((DevExpress.XtraReports.UI.XtraReport)parameters);
            }
            else if (message == "PageSetup")
            {
                PluginOperations.PageSetup();
            }

            return null;
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            DataModel = dataModel;
            Framework = frameworkCallbacks;

            Framework.AddSettingsHandler(PluginOperations.SettingsHandler);
        }

        public void Dispose()
        {
            
        }

        public void GetOperations(IOperationList operations)
        {
            
        }

        #endregion
    }
}
