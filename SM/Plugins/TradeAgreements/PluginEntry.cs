using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.TradeAgreements
{
    public class PluginEntry : IPlugin
    {
        public enum Mode
        {
            Item = 0,
            Customer = 1,
            Group = 2,
            ItemDiscountGroup = 3
        }

        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;


        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.TradeAgreements; }
        }


        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            switch (message)
            {
                case "CanEditTradeAgreementItem":
                case "CanAddTradeAgreementItem":
                    return DataModel.HasPermission(Permission.ManageTradeAgreementPrices);
                case "CustomerPriceDiscGroups":
                    return DataModel.HasPermission(Permission.ViewCustomerDiscGroups);
                case "ItemDiscountGroups":
                    return DataModel.HasPermission(Permission.ViewItemDiscGroups);
                case "CanAddPriceGroup":
                    return DataModel.HasPermission(Permission.ViewPriceGroups);
            }

            return false;
        }

        public object Message(object sender, string message, object parameters)
        {
            switch (message)
            {
                case "ViewCustomerPriceDiscGroups":
                    if ((PriceDiscGroupEnum)(int)parameters == PriceDiscGroupEnum.PriceGroup)
                    {
                        PluginOperations.ShowCustomerPriceGroups();
                    }
                    else
                    {
                        PluginOperations.ShowCustomerDiscountGroups((PriceDiscGroupEnum)(int)parameters);
                    }
                    break;
                case "ViewItemDiscountGroups":
                    PluginOperations.ShowItemDiscountGroups((PriceDiscGroupEnum)(int)parameters);
                    break;
                case "AddPriceGroup":
                    Dialogs.CustomerPriceDiscDialog dialog = new Dialogs.CustomerPriceDiscDialog();
                    dialog.ShowDialog();
                    break;
            }

            return null;
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            DataModel = dataModel;
            Framework = frameworkCallbacks;

            frameworkCallbacks.AddMenuConstructionConstructionHandler(PluginOperations.ConstructMenus);
            // We want to be able to add to sheet contexts from other plugins
            frameworkCallbacks.ViewController.AddContextBarCategoryConstructionHandler(PluginOperations.TaskBarCategoryCallback);
            frameworkCallbacks.ViewController.AddContextBarItemConstructionHandler(PluginOperations.TaskBarItemCallback);

            frameworkCallbacks.AddOperationCategoryConstructionHandler(PluginOperations.AddOperationCategoryHandler);
            frameworkCallbacks.AddOperationItemConstructionHandler(PluginOperations.AddOperationItemHandler);
            frameworkCallbacks.AddOperationButttonConstructionHandler(PluginOperations.AddOperationButtonHandler);

            frameworkCallbacks.AddRibbonPageConstructionHandler(PluginOperations.AddRibbonPages);
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(PluginOperations.AddRibbonPageCategories);
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(PluginOperations.AddRibbonPageCategoryItems);

            TabControl.TabPanelConstructionHandler += PluginOperations.ConstructTabs;
        }

        public void Dispose()
        {
            
        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation(Properties.Resources.CustomerDiscountGroups, PluginOperations.ShowCustomerDiscountGroups, Permission.ViewCustomerDiscGroups);
            operations.AddOperation(Properties.Resources.PriceGroups, PluginOperations.ShowCustomerPriceGroups, Permission.ViewCustomerDiscGroups);
            operations.AddOperation(Properties.Resources.ItemDiscountGroups, PluginOperations.ShowItemDiscountGroups, Permission.ViewItemDiscGroups);
        }

        #endregion
    }
}
