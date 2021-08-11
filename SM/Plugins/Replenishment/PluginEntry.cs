using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Replenishment.Properties;

namespace LSOne.ViewPlugins.Replenishment
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        #region IPlugin Members

        public string Description
        {
            get { return Resources.Replenishment; }
        }

        public void Dispose()
        {

        }

        public void GetOperations(IOperationList operations)
        {            
            operations.AddOperation(Resources.InventoryTemplates, PluginOperations.ShowInventoryTemplates, Permission.ManageInventoryTemplates);
            operations.AddOperation(Resources.PurchaseWorksheets, PluginOperations.ShowPurchaseWorksheets,Permission.ManageReplenishment);
            operations.AddOperation("", "ViewPOWorksheet", false, false, PluginOperations.ShowPurchaseWorksheet, Permission.ManageReplenishment);
        }

        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            switch (message)
            {
                case "CanEditReplenishmentProfiles":
                    return DataModel.HasPermission(Permission.ManageReplenishment);
                case "CanBlockItemReplenishment":
                    return DataModel.HasPermission(Permission.ManageReplenishment) || DataModel.HasPermission(Permission.ManageItemTypes);
            }
            
            return false;
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

            TabControl.TabPanelConstructionHandler += PluginOperations.ConstructTabs;
        }

        public object Message(object sender, string message, object parameters)
        {
            switch (message)
            {   
                case "BlockItemReplenishmentNewServiceItem":
                    if (parameters is object)
                    {
                        PluginOperations.BlockItemReplenishmentNewServiceItem((RecordIdentifier)parameters);
                    }
                    break;
                case "BlockItemReplenishmentThroughService":
                    if (parameters is object)
                    {
                        PluginOperations.BlockItemReplenishmentChangeItemType((RecordIdentifier)parameters);
                    }
                    break;
            }

            return null;
        }
        #endregion
    }
}
