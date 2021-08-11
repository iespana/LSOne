using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.LSCommerce.Properties;

namespace LSOne.ViewPlugins.LSCommerce
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        #region IPlugin Members

        public string Description
        {
            get { return Resources.LSCommerce; }
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
            frameworkCallbacks.AddOperationButttonConstructionHandler(PluginOperations.AddOperationButtonHandler);
            frameworkCallbacks.AddOperationItemConstructionHandler(PluginOperations.AddOperationItemHandler);
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(PluginOperations.AddRibbonPageCategories);
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(PluginOperations.AddRibbonPageCategoryItems);
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
