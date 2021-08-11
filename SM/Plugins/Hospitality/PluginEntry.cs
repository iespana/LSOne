using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Hospitality
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.Hospitality; }
        }


        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            switch (message)
            {                
                case "CanEditKitchenManagerProfile":
                    return DataModel.HasPermission(Permission.ManageKitchenServiceProfiles);
            }

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

            // We want to be able to register items to the main application menu
            frameworkCallbacks.AddMenuConstructionConstructionHandler(PluginOperations.ConstructMenus);

            // We want to be able to add to sheet contexts from other plugins
            frameworkCallbacks.ViewController.AddContextBarItemConstructionHandler(PluginOperations.TaskBarItemCallback);
        
            // We want to add tabs on tab panels in other plugins
            TabControl.TabPanelConstructionHandler += PluginOperations.ConstructTabs;

            frameworkCallbacks.AddOperationCategoryConstructionHandler(PluginOperations.AddOperationCategoryHandler);
            frameworkCallbacks.AddOperationItemConstructionHandler(PluginOperations.AddOperationItemHandler);
            frameworkCallbacks.AddOperationButttonConstructionHandler(PluginOperations.AddOperationButtonHandler);

            frameworkCallbacks.AddRibbonPageConstructionHandler(PluginOperations.AddRibbonPages);
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(PluginOperations.AddRibbonPageCategories);
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(PluginOperations.AddRibbonPageCategoryItems);
        }

        public void Dispose()
        {
            
        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation(Properties.Resources.HospitalitySetup, PluginOperations.ShowHospitalitySetupSheet, Permission.ManageHospitalitySetup);
            operations.AddOperation(Properties.Resources.SalesTypes, PluginOperations.ShowSalesTypesListView, Permission.ManageSalesTypes);
            operations.AddOperation(Properties.Resources.HospitalityTypes,"HospitalityTypes",true,false, PluginOperations.ShowHospitalityTypesLisetView, Permission.ManageHospitalityTypes);
            operations.AddOperation(Properties.Resources.PrintingStations, PluginOperations.ShowPrintingStationsListView, Permission.ManageStationPrinting);
            
            operations.AddOperation(Properties.Resources.StationRouting, PluginOperations.ShowStationPrintingView, Permission.ManageStationPrinting);
            operations.AddOperation(Properties.Resources.HospitalityPosMenus, PluginOperations.ShowPosMenusListView, Permission.ViewPosMenus);
            operations.AddOperation(Properties.Resources.HospitalityMenuGroups, PluginOperations.ShowPosLookupListView, Permission.ManageMenuGroups);

            operations.AddOperation(Properties.Resources.RestaurantStations, "EditRestaurantStations", true, false, PluginOperations.ShowPrintingStationsListView, Permission.ManageStationPrinting);
        }

        #endregion
    }
}
