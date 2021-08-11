using System;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.BarCodes.Datalayer;
using LSOne.ViewPlugins.BarCodes.Datalayer.DataEntities;
using LSOne.Controls;

namespace LSOne.ViewPlugins.BarCodes
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

       
        private void ConstructMenus(object sender, MenuConstructionArguments args)
        {
            ExtendedMenuItem item;

            switch (args.Key)
            {
                case "Edit":
                    
                    item = new ExtendedMenuItem(Properties.Resources.BarCodes, 1020, "BarCodes");
                    item.Image = Properties.Resources.BarcodeImage;

                    args.Add(item, 1020);
                    break;

                case "RibbonBarCodes":
                    if(PluginEntry.DataModel.HasPermission(Permission.ManageBarcodeSetup))
                    {
                        args.AddMenu(new ExtendedMenuItem(Properties.Resources.BarCodeSetup, 10,new EventHandler(PluginOperations.ShowBarCodeSetup)));
                    }

                    if(PluginEntry.DataModel.HasPermission(Permission.ManageBarcodesMasks))
                    {
                        args.AddMenu(new ExtendedMenuItem(Properties.Resources.BarCodeMaskSetup, 20, new EventHandler(PluginOperations.ShowBarCodeMaskSetup)));
                    }
                    break;
            }
        }

        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.BarCodes; }
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

            // We want to be able to register items to the main application menu
            frameworkCallbacks.AddMenuConstructionConstructionHandler(ConstructMenus);

            // We want to add tabs on tab panels in other plugins
            TabControl.TabPanelConstructionHandler += PluginOperations.ConstructTabs;

            // We want to be able to add to sheet contexts from other plugins
            frameworkCallbacks.ViewController.AddContextBarItemConstructionHandler(PluginOperations.TaskBarItemCallback);


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
            operations.AddOperation(Properties.Resources.BarCodeSetup, PluginOperations.ShowBarCodeSetup, Permission.ManageBarcodeSetup);
            operations.AddOperation(Properties.Resources.BarCodeMaskSetup, PluginOperations.ShowBarCodeMaskSetup, Permission.ManageBarcodesMasks);
        }

        #endregion
    }
}
