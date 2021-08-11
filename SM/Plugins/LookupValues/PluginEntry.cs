using System;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.LookupValues.Dialogs;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.BusinessObjects.Images;

namespace LSOne.ViewPlugins.LookupValues
{
    public class PluginEntry : IPlugin, IPluginDashboardProvider
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        internal static Guid PaymentTypeDashboardItemID = new Guid("189b389f-a70f-480d-b3fa-0483f47bde25");

        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.LookupValueManagement; }
        }


        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            switch (message)
            {
                case "CanEditCardTypes":
                    return DataModel.HasPermission(Permission.CardTypesView);

                case "CanEditPaymentTypes":
                case "CanAddPayments":
                    return DataModel.HasPermission(Permission.PaymentMethodsView);                    

                case "ViewCurrency" :
                    return DataModel.HasPermission(Permission.CurrencyView);

                case "CanAddCurrency":
                    return DataModel.HasPermission(Permission.CurrencyEdit);              

                case "CanEditUnits":
                case "CanEditUnitConversions":
                case "CanAddUnits":
                case "CanAddUnitConversions":
                case "CanAddUnitConversionsSiteService":
                case "CanEnforceInventoryUnitConversion":
                    return DataModel.HasPermission(Permission.ManageUnits);

                case "CanEditRemoteHosts":
                    return DataModel.HasPermission(Permission.ManageRemoteHosts);
                case "ShowImageBankSelectDialog":
                    return true;
            }

            return false;
        }

        public object Message(object sender, string message, object parameters)
        {
            switch (message)
            {
                case "EditCardTypes":
                    PluginOperations.ShowCardTypesSheet(this,EventArgs.Empty);
                    break;
                case "EditPaymentTypes":
                    PluginOperations.ShowPaymentMethodsSheet(this, EventArgs.Empty);
                    break; 
                case "AddPayments":
                    NewLookupPaymentMethodDialog paymDlg = new NewLookupPaymentMethodDialog();
                    paymDlg.ShowDialog();
                    break;
                case "ViewCurrency":
                    PluginOperations.ShowCurrencySheet((RecordIdentifier)parameters);
                    break;
                case "AddCurrency":
                    CurrencyDialog dlg = new CurrencyDialog();
                    dlg.ShowDialog();
                    break;
                case "EditUnits":
                    PluginOperations.ShowUnitsView((RecordIdentifier)parameters);
                    break;
                case "AddUnits":
                    UnitDialog dlg2 = new UnitDialog();
                    dlg2.ShowDialog();
                    return dlg2.Unit;

                case "ViewUnitConversions":
                    PluginOperations.ShowUnitConversionsViewFromContext(this, new ContextBarClickEventArguments((RecordIdentifier)parameters, null, null, null));
                    break;

                case "AddUnitConversion":
                    return (bool)PluginOperations.NewUnitConversion(
                        (DataEntity)(((object[])parameters)[0]),
                        (RecordIdentifier)(((object[])parameters)[1]),
                        (RecordIdentifier)(((object[])parameters)[2]));
                case "AddUnitConversionIncludeSiteService":
                    return (bool)PluginOperations.NewUnitConversionSiteService(
                        (DataEntity)(((object[])parameters)[0]),
                        (RecordIdentifier)(((object[])parameters)[1]),
                        (RecordIdentifier)(((object[])parameters)[2]));
                case "AddUnitConversionNoSave":
                    return (UnitConversion)PluginOperations.NewUnitConversionNoSave(
                        (DataEntity)(((object[])parameters)[0]),
                        (RecordIdentifier)(((object[])parameters)[1]),
                        (RecordIdentifier)(((object[])parameters)[2]));

                case "EditRemoteHost":
                    PluginOperations.EditRemoteHosts((RecordIdentifier)parameters);
                    break;

                case "EnforceInventoryUnitConversion":
                    DataEntity itemDataEntity = (DataEntity) (((object[]) parameters)[0]);
                    RecordIdentifier targetUnitId = (RecordIdentifier) (((object[]) parameters)[1]);
                    return PluginOperations.EnforceInventoryUnitConversion(itemDataEntity, targetUnitId);
                case "ShowImageBankSelectDialog":
                    ImageBankSelectDialog dlg3 = new ImageBankSelectDialog((ImageTypeEnum)parameters);
                    dlg3.ShowDialog(Framework.MainWindow);
                    return dlg3.Image;
            }

            return null;
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            DataModel = dataModel;
            Framework = frameworkCallbacks;

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
            operations.AddOperation(Properties.Resources.CardTypes, PluginOperations.ShowCardTypesSheet, Permission.CardTypesView);
            operations.AddOperation(Properties.Resources.PaymentTypes, PluginOperations.ShowPaymentMethodsSheet, Permission.PaymentMethodsView);
            operations.AddOperation(Properties.Resources.EditCurrencies, PluginOperations.ShowCurrencySheet, Permission.CurrencyView);
            operations.AddOperation(Properties.Resources.Units, PluginOperations.ShowUnitsView, Permission.ManageUnits);
            operations.AddOperation(Properties.Resources.UnitConversions, PluginOperations.ShowUnitConversionsView, Permission.ManageUnits);
            operations.AddOperation(Properties.Resources.RemoteHosts, PluginOperations.EditRemoteHosts, Permission.ManageRemoteHosts);
            operations.AddOperation(Properties.Resources.CustomerCards, PluginOperations.ShowCustomerCards);
            operations.AddOperation(Properties.Resources.POSUserCards, PluginOperations.ShowPOSUserCards);

            operations.AddOperation("", "ShowImageBank", false, true, PluginOperations.ShowImageBank, Permission.EditPOSLayout);
            operations.AddOperation("", "AddEditImage", false, true, PluginOperations.AddEditImageHandler, Permission.ManageImageBank);
            operations.AddOperation("", "DeleteImage", false, true, PluginOperations.DeleteImage, Permission.ManageImageBank);
        }

        #endregion

        public void LoadDashboardItem(IConnectionManager threadedEntry, ViewCore.Controls.DashboardItem item)
        {
            if (item.ID == PaymentTypeDashboardItemID)
            {
                int buttonIndex = 0;

                int paymentMethodCount = Providers.PaymentMethodData.GetPaymentMethodCount(threadedEntry);

                if (paymentMethodCount > 0)
                {
                    if(paymentMethodCount == 1)
                    {
                        item.InformationText = Properties.Resources.OnePaymentType;
                    }
                    else
                    {
                        item.InformationText = Properties.Resources.ManyPaymentTypes.Replace("#1", paymentMethodCount.ToString());
                    }

                    item.State = DashboardItem.ItemStateEnum.Passed;
                }
                else
                {
                    item.State = DashboardItem.ItemStateEnum.Error;

                    item.InformationText = Properties.Resources.NoPaymentTypes;
                }

                if (DataModel.HasPermission(Permission.PaymentMethodsEdit))
                {
                    item.SetButton(buttonIndex, Properties.Resources.NewPaymentMethod, PluginOperations.NewPaymentMethod);

                    buttonIndex++;
                }

                item.SetButton(buttonIndex, Properties.Resources.ManagePaymentMethod, PluginOperations.ShowPaymentMethodsSheet);
            }
        }

        public void RegisterDashBoardItems(ViewCore.EventArguments.DashboardItemArguments args)
        {
            DashboardItem paymentTypeDashBoardItem = new DashboardItem(PaymentTypeDashboardItemID, Properties.Resources.PaymentTypes, true, 60); // 1 hour refresh interval

            if (DataModel.HasPermission(Permission.PaymentMethodsView))
            {
                args.Add(new DashboardItemPluginResolver(paymentTypeDashBoardItem, this), 40); // Priority 40
            }
        }
    }
}
