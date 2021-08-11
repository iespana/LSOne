using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.SerialNumbers
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        internal static Guid ExcelFolderLocationSettingID = new Guid("EB2EE2BD-5A52-4A9A-B84B-176458668AF9");

        private void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            if (args.ContextName == "LSOne.ViewPlugins.SiteService.Views.SiteServiceProfileView")
            {
                args.Add(new TabControl.Tab(Properties.Resources.SerialNumbersTabName, new PanelFactoryHandler(ViewPages.SiteServiceViewSerialNumbersPage.CreateInstance)), 800);
            }
        }


        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.SerialNumbers; }
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

            TabControl.TabPanelConstructionHandler += ConstructTabs;

            frameworkCallbacks.AddOperationButttonConstructionHandler(new OperationbuttonEventHandler(PluginOperations.AddOperationButtonHandler));
            frameworkCallbacks.AddRibbonPageConstructionHandler(PluginOperations.AddRibbonPages);
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(PluginOperations.AddRibbonPageCategories);
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(PluginOperations.AddRibbonPageCategoryItems);

            // Register data providers
            PluginProviders.RegisterDataProviders();
        }

        public void Dispose()
        {

        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation("", "KeyInSerialNumber", false, false, null, "");
        }

        #endregion
    }
}
