using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Profiles.Properties;
using System.Collections.Generic;

namespace LSOne.ViewPlugins.Profiles
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        #region IPlugin Members

        public string Description
        {
            get { return Resources.ProfileManagement; }
        }

        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            if (message == "CanEditVisualProfiles")
            {
                return DataModel.HasPermission(Permission.FunctionalProfileEdit);
            }
            if (message == "CanEditFunctionalityProfiles")
            {
                return DataModel.HasPermission(Permission.FunctionalProfileView);
            }
            if (message == "CanEditTransactionServiceProfiles")
            {
                return DataModel.HasPermission(Permission.TransactionServiceProfileView);
            }
            if (message == "CanEditHardwareProfiles")
            {
                return DataModel.HasPermission(Permission.HardwareProfileView);
            }
            if (message == "CanCreateHardwareProfile")
            {
                return DataModel.HasPermission(Permission.HardwareProfileEdit);
            }
            if (message == "CanEditStyleProfiles")
            {
                return DataModel.HasPermission(Permission.StyleProfileView);
            }
            if (message == "StyleProfiles")
            {
                return DataModel.HasPermission(Permission.StyleProfileView);
            }
            if(message == "CanEditUserProfile")
            {
                return DataModel.HasPermission(Permission.ManageUserProfiles);
            }
            if(message == "CanEditUsersUserProfile")
            {
                return DataModel.HasPermission(DataLayer.BusinessObjects.Permission.SecurityEditUser);
            }
            if(message == "CanCreateUserProfile")
            {
                return DataModel.HasPermission(Permission.ManageUserProfiles);
            }

            return false;
        }

        public object Message(object sender, string message, object parameters)
        {
            if (message == "EditVisualProfile")
            {
                PluginOperations.ShowVisualProfilesSheet((RecordIdentifier)parameters);
                return null;
            }
            if (message == "EditfunctionalityProfile")
            {
                PluginOperations.ShowFunctionalityProfilesSheet((RecordIdentifier)parameters);
                return null;
            }
            if (message == "EditHardwareProfile")
            {
                PluginOperations.ShowHardwareProfilesSheet((RecordIdentifier)parameters);
                return null;
            }

            if (message == "CreateHardwareProfile")
            {
                PluginOperations.NewHardwareProfile();
                return null;
            }

            if(message == "EditUserProfile")
            {
                PluginOperations.ShowUserProfileSheet((RecordIdentifier)parameters);
                return null;
            }

            if(message == "EditUsersUserProfile")
            {
                return PluginOperations.ShowEditUserProfileDialog((List<User>)parameters);
            }

            if(message == "CreateUserProfile")
            {
                return PluginOperations.NewUserProfile();
            }

            return null;
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            DataModel = dataModel;
            Framework = frameworkCallbacks;

            // We want to be able to register items to the main application menu
            frameworkCallbacks.AddMenuConstructionConstructionHandler(PluginOperations.ConstructMenus);

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

        public void Dispose()
        {

        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation(Properties.Resources.VisualProfiles, PluginOperations.ShowVisualProfilesSheet, Permission.VisualProfileView);
            operations.AddOperation(Properties.Resources.FunctionalityProfiles, PluginOperations.ShowFunctionalityProfilesSheet, Permission.FunctionalProfileView);
            operations.AddOperation(Properties.Resources.HardwareProfiles, PluginOperations.ShowHardwareProfilesSheet, Permission.HardwareProfileView);
            operations.AddOperation(Properties.Resources.FormProfiles, PluginOperations.ShowFormProfileSheet, Permission.FormProfileView);
            operations.AddOperation(Properties.Resources.CSVImportProfiles, PluginOperations.ShowCSVImportProfileSheet, Permission.ManageImportProfiles);

            operations.AddOperation("", "AddFunctionalityProfile", false, false, PluginOperations.AddFunctionalityProfile, DataLayer.BusinessObjects.Permission.FunctionalProfileEdit);
            operations.AddOperation("", "AddHardwareProfile", false, false, PluginOperations.AddHardwareProfile, DataLayer.BusinessObjects.Permission.HardwareProfileEdit);
            operations.AddOperation("", "AddVisualProfile", false, false, PluginOperations.AddVisualProfile, DataLayer.BusinessObjects.Permission.VisualProfileEdit);
            operations.AddOperation("", "EditWindowsPrinters", false, false, PluginOperations.ShowWindowsPrinterConfigurationsView, new string[] { DataLayer.BusinessObjects.Permission.ManageStationPrinting, DataLayer.BusinessObjects.Permission.HardwareProfileEdit });
        }

        #endregion
    }
}
