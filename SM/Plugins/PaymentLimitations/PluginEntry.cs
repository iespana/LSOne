using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.PaymentLimitations
{
    public class PluginEntry : IPlugin, IPluginDashboardProvider
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;     

        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.Criminal; }
        }

        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            return false;
        }

        public object Message(object sender, string message, object parameters)
        {
            return null;
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            DataModel = dataModel;
            Framework = frameworkCallbacks;
            frameworkCallbacks.AddOperationCategoryConstructionHandler(PluginOperations.AddOperationCategoryHandler);
            frameworkCallbacks.AddOperationItemConstructionHandler(PluginOperations.AddOperationItemHandler);
            frameworkCallbacks.AddOperationButttonConstructionHandler(PluginOperations.AddOperationButtonHandler);
            TabControl.TabPanelConstructionHandler += PluginOperations.ConstructTabs;
        }

        public void Dispose()
        {
            
        }

        public void GetOperations(IOperationList operations)
        {
            
        }

        public void RegisterDashBoardItems(DashboardItemArguments args)
        {
        }

        public void LoadDashboardItem(IConnectionManager threadedEntry, DashboardItem item)
        {
        }

        #endregion
    }
}
