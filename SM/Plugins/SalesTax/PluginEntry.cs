using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.SalesTax
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.SalesTax; }
        }


        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            if (message == "ViewItemSalesTaxGroups")
            {
                return DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ViewSalesTaxSetup);
            }
            if (message == "ViewSalesTaxGroups")
            {
                return DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ViewSalesTaxSetup);
            }
            if (message == "NewStoreSalesTaxGroup")
            {
                return DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.EditSalesTaxSetup);
            }
            if (message == "NewSalesTaxItemGroup")
            {
                return DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.EditSalesTaxSetup);
            }

            return false;
        }

        public object Message(object sender, string message, object parameters)
        {
            if (message == "ViewItemSalesTaxGroups")
            {
                PluginOperations.ShowItemSalesTaxGroupView((RecordIdentifier)parameters);
            }
            else if (message == "ViewSalesTaxGroups")
            {
                PluginOperations.ShowSalesTaxGroupView((RecordIdentifier)parameters);
            }
            if (message == "NewSalesTaxItemGroup")
            {
                ItemSalesTaxGroup istg = PluginOperations.NewSalesTaxItemGroup((RecordIdentifier)parameters);
                return istg;
            }
            if (message == "NewStoreSalesTaxGroup")
            {
                SalesTaxGroup stg = PluginOperations.NewStoreSalesTaxGroup();
                return stg;
            }

            return null;
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            DataModel = dataModel;
            Framework = frameworkCallbacks;

            frameworkCallbacks.AddOperationCategoryConstructionHandler(new OperationCategoryEventHandler(PluginOperations.AddOperationCategoryHandler));
            frameworkCallbacks.AddOperationItemConstructionHandler(new OperationItemEventHandler(PluginOperations.AddOperationItemHandler));
            frameworkCallbacks.AddOperationButttonConstructionHandler(new OperationbuttonEventHandler(PluginOperations.AddOperationButtonHandler));

            frameworkCallbacks.AddRibbonPageConstructionHandler(new RibbonPageEventHandler(PluginOperations.AddRibbonPages));
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(new RibbonPageCategoryEventHandler(PluginOperations.AddRibbonPageCategories));
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(new RibbonPageCategoryItemEventHandler(PluginOperations.AddRibbonPageCategoryItems));
        }

        public void Dispose()
        {
            
        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation(Properties.Resources.SalesTaxCodes, PluginOperations.ShowSalesTaxCodesView, LSOne.DataLayer.BusinessObjects.Permission.ViewSalesTaxSetup);
            operations.AddOperation(Properties.Resources.ItemSalesTaxGroup, PluginOperations.ShowItemSalesTaxGroupView, LSOne.DataLayer.BusinessObjects.Permission.ViewSalesTaxSetup);
            operations.AddOperation(Properties.Resources.SalesTaxGroup, PluginOperations.ShowSalesTaxGroupView, LSOne.DataLayer.BusinessObjects.Permission.ViewSalesTaxSetup);
        }

        #endregion
    }
}
