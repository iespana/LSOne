using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.USConfigurations
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.USConfigurations; }
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

            // We want to add tabs on tabs in other plugins
            TabControl.TabPanelConstructionHandler += PluginOperations.ConstructTabs;

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
