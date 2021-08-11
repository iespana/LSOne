using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Terminals
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        internal static int StoreImageIndex;
        internal static int TerminalImageIndex;

        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.Terminals; }
        }

        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            switch (message)
            {
                case "ViewTerminal": return DataModel.HasPermission(Permission.TerminalView);
                default : return false;
            }
        }

        public object Message(object sender, string message, object parameters)
        {
            switch (message)
            {
                case "ViewTerminal":
                    var terminalID = (RecordIdentifier)parameters;
                    PluginOperations.ShowTerminal(terminalID[0], terminalID[1]);
                    break;
            }
            return null;
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            ImageList iml;
            DataModel = dataModel;
            Framework = frameworkCallbacks;

            // Register Icons that this plugin uses to the framework
            // -------------------------------------------------
            iml = frameworkCallbacks.GetImageList();

            iml.Images.Add(Properties.Resources.store_16);
            StoreImageIndex = iml.Images.Count - 1;

            iml.Images.Add(Properties.Resources.terminal_16);
            TerminalImageIndex = iml.Images.Count - 1;

            frameworkCallbacks.AddMenuConstructionConstructionHandler(PluginOperations.ConstructMenus);
            frameworkCallbacks.AddSearchBarConstructionHandler(PluginOperations.AddSearchHandler);
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
            if (DataModel.HasPermission(Permission.TerminalEdit))
            {
                operations.AddOperation(Properties.Resources.NewTerminal, PluginOperations.NewTerminal);
            }

            if (DataModel.HasPermission(Permission.TerminalView))
            {
                operations.AddOperation(Properties.Resources.Terminals, PluginOperations.ShowTerminals);
                //operations.AddOperation(Properties.Resources.TerminalGroups, PluginOperations.ShowTerminalGroups);

                operations.AddOperation(Properties.Resources.TerminalOperationAudit, PluginOperations.ShowTerminalOperationsAudit);
            }
        }

        #endregion
    }
}
