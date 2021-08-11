﻿using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.CreditVouchers
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;
      
        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.CreditMemos; }
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

            frameworkCallbacks.AddOperationCategoryConstructionHandler(PluginOperations.AddOperationCategoryHandler);
            frameworkCallbacks.AddOperationItemConstructionHandler(PluginOperations.AddOperationItemHandler);
            frameworkCallbacks.AddOperationButttonConstructionHandler(PluginOperations.AddOperationButtonHandler);

            frameworkCallbacks.AddRibbonPageConstructionHandler(new RibbonPageEventHandler(PluginOperations.AddRibbonPages));
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(new RibbonPageCategoryEventHandler(PluginOperations.AddRibbonPageCategories));
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(new RibbonPageCategoryItemEventHandler(PluginOperations.AddRibbonPageCategoryItems));

            // We want to be able to register items to the main application menu
            //frameworkCallbacks.AddMenuConstructionConstructionHandler(PluginOperations.ConstructMenus);
        }

        public void Dispose()
        {
            
        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation(Properties.Resources.ViewCreditMemos, PluginOperations.ShowCreditVouchersView, Permission.ManageCreditVouchers);
        }

        #endregion
    }
}
