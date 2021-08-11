using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.CustomerOrders
{
    public class PluginEntry : IPlugin
    {

        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.CustomerOrder; }
        }

        public void Dispose()
        {
            
        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation(Properties.Resources.CustomerOrders, PluginOperations.ShowCustomerOrders);
            operations.AddOperation(Properties.Resources.Quotes, PluginOperations.ShowQuotes);
            operations.AddOperation(Properties.Resources.Settings, PluginOperations.ShowSettings);
            operations.AddOperation(Properties.Resources.CustomerOrderSettings, PluginOperations.ShowSettings);
            operations.AddOperation(Properties.Resources.QuotesSettings, PluginOperations.ShowSettings);
        }

        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            return false;
        }

        public void Init(LSOne.DataLayer.GenericConnector.Interfaces.IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            DataModel = dataModel;
            Framework = frameworkCallbacks;

            // We want to be able to add to sheet contexts from other plugins
            frameworkCallbacks.ViewController.AddContextBarItemConstructionHandler(PluginOperations.TaskBarItemCallback);

            // We want to add tabs on tab panels in other plugins
            //TabControl.TabPanelConstructionHandler += PluginOperations.ConstructTabs;
            frameworkCallbacks.AddOperationCategoryConstructionHandler(PluginOperations.AddOperationCategoryHandler);
            frameworkCallbacks.AddOperationItemConstructionHandler(PluginOperations.AddOperationItemHandler);
            frameworkCallbacks.AddOperationButttonConstructionHandler(PluginOperations.AddOperationButtonHandler);

            frameworkCallbacks.AddRibbonPageConstructionHandler(PluginOperations.AddRibbonPages);
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(PluginOperations.AddRibbonPageCategories);
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(PluginOperations.AddRibbonPageCategoryItems);
        }

        public object Message(object sender, string message, object parameters)
        {
            return null;
        }

        #endregion

    }
}
