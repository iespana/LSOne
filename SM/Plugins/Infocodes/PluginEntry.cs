using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Infocodes
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;


        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.Infocodes; }
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

            TabControl.TabPanelConstructionHandler += PluginOperations.ConstructTabs;

            frameworkCallbacks.ViewController.AddContextBarItemConstructionHandler(PluginOperations.TaskBarItemCallback);

            frameworkCallbacks.AddOperationCategoryConstructionHandler(PluginOperations.AddOperationCategoryHandler);
            frameworkCallbacks.AddOperationItemConstructionHandler(PluginOperations.AddOperationItemHandler);
            frameworkCallbacks.AddOperationButttonConstructionHandler(PluginOperations.AddOperationButtonHandler);

            frameworkCallbacks.AddRibbonPageConstructionHandler(PluginOperations.AddRibbonPages);
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(PluginOperations.AddRibbonPageCategories);
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(PluginOperations.AddRibbonPageCategoryItems);

            frameworkCallbacks.AddMenuConstructionConstructionHandler(PluginOperations.ConstructMenus);
        }

        public void Dispose()
        {

        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation(Properties.Resources.Infocodes, PluginOperations.ShowInfocodeSetupSheet, DataLayer.BusinessObjects.Permission.InfocodeEdit);
            operations.AddOperation(Properties.Resources.CrossSellInfocodeGroups, PluginOperations.ShowCrossSellingGroupInfocodeSetupSheet, DataLayer.BusinessObjects.Permission.InfocodeEdit);
            operations.AddOperation(Properties.Resources.ItemModifierGroups, PluginOperations.ShowItemModifierGroupInfocodeSetupSheet, DataLayer.BusinessObjects.Permission.InfocodeEdit);
        }

        #endregion
    }
}
