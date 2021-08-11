using System;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.UserInterfaceStyles
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        public string Description
        {
            get { return Properties.Resources.UserInterfaceStyles; }
        }

        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            return false;
        }

        public object Message(object sender, string message, object parameters)
        {
            return null;
        }

        public void Dispose()
        {
          
        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation(Properties.Resources.UserInterfaceStyles, "UIStyles", true, true, PluginOperations.ShowStylesView, LSOne.DataLayer.BusinessObjects.Permission.ManageUIStyleSetup);
            operations.AddOperation("", "EditUIStyle", false, false, PluginOperations.EditStyle, LSOne.DataLayer.BusinessObjects.Permission.ManageUIStyleSetup);
            operations.AddOperation("", "NewUIStyle", false, false, PluginOperations.NewStyle, LSOne.DataLayer.BusinessObjects.Permission.ManageUIStyleSetup);
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            DataModel = dataModel;
            Framework = frameworkCallbacks;

            frameworkCallbacks.AddOperationCategoryConstructionHandler(PluginOperations.AddOperationCategoryHandler);
            frameworkCallbacks.AddOperationItemConstructionHandler(PluginOperations.AddOperationItemHandler);
            frameworkCallbacks.AddOperationButttonConstructionHandler(PluginOperations.AddOperationButtonHandler);

            frameworkCallbacks.AddRibbonPageConstructionHandler(PluginOperations.AddRibbonPages);
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(PluginOperations.AddRibbonPageCategories);
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(PluginOperations.AddRibbonPageCategoryItems);

            frameworkCallbacks.ViewController.AddContextBarItemConstructionHandler(PluginOperations.TaskBarItemCallback);
        }
    }
}
