using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.RetailItemAssemblies
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        public string Description => Properties.Resources.RetailItemAssemblies;

        public void Dispose()
        {
            
        }

        public void GetOperations(IOperationList operations)
        {
            
        }

        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            return false;
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            DataModel = dataModel;
            Framework = frameworkCallbacks;

            // We want to be able to register items to the main application menu
            frameworkCallbacks.AddMenuConstructionConstructionHandler(PluginOperations.ConstructMenus);

            // We want to add tabs on tab panels in other plugins
            TabControl.TabPanelConstructionHandler += PluginOperations.ConstructTabs;

            // Register Icons that this plugin uses to the framework
            // -------------------------------------------------
            frameworkCallbacks.AddOperationCategoryConstructionHandler(new OperationCategoryEventHandler(PluginOperations.AddOperationCategoryHandler));
            frameworkCallbacks.AddOperationItemConstructionHandler(new OperationItemEventHandler(PluginOperations.AddOperationItemHandler));
            frameworkCallbacks.AddOperationButttonConstructionHandler(new OperationbuttonEventHandler(PluginOperations.AddOperationButtonHandler));

            frameworkCallbacks.ViewController.AddContextBarItemConstructionHandler(PluginOperations.TaskBarItemCallback);

            frameworkCallbacks.AddRibbonPageConstructionHandler(new RibbonPageEventHandler(PluginOperations.AddRibbonPages));
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(new RibbonPageCategoryEventHandler(PluginOperations.AddRibbonPageCategories));
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(new RibbonPageCategoryItemEventHandler(PluginOperations.AddRibbonPageCategoryItems));
        }

        public object Message(object sender, string message, object parameters)
        {
            return null;
        }
    }
}
