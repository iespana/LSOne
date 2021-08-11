using System;
using System.Collections.Generic;
using System.IO;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Reports;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore;

namespace LSOne.ViewPlugins.ReportViewer
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;
        internal static System.Drawing.Printing.PaperKind PaperKind = System.Drawing.Printing.PaperKind.A4;

        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.ReportViewer; }
        }


        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            if (message == "CanDisplayReport")
            {
                if (parameters == null)
                {
                    return true;
                }
                if (parameters is RecordIdentifier)
                {
                    return Providers.ReportData.Exists(DataModel, (RecordIdentifier)parameters);
                }
                if (parameters is Guid)
                {
                    return Providers.ReportData.Exists(DataModel, (Guid)parameters);
                }

                return true;
            }

            return false;
        }

        public object Message(object sender, string message, object parameters)
        {
            // ShowReport message can be used either by sending a object array containing only report id contained as Guid in a RecordIdentifier,
            // or a ReportID and a dictionary of pre set parameters.

            if (message == "ShowReport")
            {
                if (parameters is object[])
                {
                    if (((object[])parameters).Length == 1)
                    {
                        PluginOperations.ShowReport((RecordIdentifier)((object[])parameters)[0]);
                    }
                    else if (((object[])parameters).Length > 1)
                    {
                        Dictionary<string, object> reportParams = (Dictionary<string, object>)((object[])parameters)[1];
                        Dictionary<string, ProcedureParameter> preSetValues = new Dictionary<string, ProcedureParameter>();
                        ProcedureParameter prm;

                        foreach (var item in reportParams)
                        {
                            prm = new ProcedureParameter(item.Key.StartsWith("@") ? item.Key : "@" + item.Key);
                            prm.Value = item.Value;
                        }

                        PluginOperations.ShowReport((RecordIdentifier)((object[])parameters)[0], preSetValues);
                    }
                }
            }
            else if (message == "ShowReportPrintPreview")
            {
                PluginOperations.ShowReportPreview(sender, (PluginOperationArguments)parameters);
            }
            else if (message == "ShowReportPrint")
            {
                PluginOperations.ShowReportPrint(sender, (PluginOperationArguments)parameters);
            }
            else if (message == "ShowFinancialReportByStatement")
            {
                PluginOperations.ShowFinancialReportByStatement(sender, EventArgs.Empty);
            }

            return null;
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            DataModel = dataModel;
            Framework = frameworkCallbacks;

            // We want to be able to add to sheet contexts from other plugins
            frameworkCallbacks.ViewController.AddContextBarCategoryConstructionHandler(PluginOperations.TaskBarCategoryCallback);
            frameworkCallbacks.ViewController.AddContextBarItemConstructionHandler(PluginOperations.TaskBarItemCallback);

            frameworkCallbacks.AddOperationCategoryConstructionHandler(PluginOperations.AddOperationCategoryHandler);
            frameworkCallbacks.AddOperationItemConstructionHandler(PluginOperations.AddOperationItemHandler);
            frameworkCallbacks.AddOperationButttonConstructionHandler(PluginOperations.AddOperationButtonHandler);

            frameworkCallbacks.AddRibbonPageConstructionHandler(PluginOperations.AddRibbonPages);
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(PluginOperations.AddRibbonPageCategories);
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(PluginOperations.AddRibbonPageCategoryItems);

            frameworkCallbacks.AddMenuConstructionConstructionHandler(PluginOperations.ConstructMenus);
        }

        public void Dispose()
        {

        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation(Properties.Resources.FinancialReport, PluginOperations.ShowFinancialReport, Permission.ViewFinancialReports);
            operations.AddOperation(Properties.Resources.FinancialReportByEmployee, PluginOperations.ShowFinancialReportByEmployee, Permission.ViewFinancialReports);
            operations.AddOperation(Properties.Resources.FinancialReportByStatement, PluginOperations.ShowFinancialReportByStatement, Permission.ViewFinancialReports);

            operations.AddOperation(Properties.Resources.ItemSales, PluginOperations.ShowItemSalesReport, Permission.ViewSalesReports);
            operations.AddOperation(Properties.Resources.GiftCardSales, PluginOperations.ShowGiftCardSalesReport, Permission.ViewSalesReports);
            operations.AddOperation(Properties.Resources.IssuedCreditMemos, PluginOperations.ShowIssuedCreditMemosReport, Permission.ViewSalesReports);
            operations.AddOperation(Properties.Resources.SalesPerEmployee, PluginOperations.ShowSalesPerEmployeeReport, Permission.ViewSalesReports);
            operations.AddOperation(Properties.Resources.SalesPerTerminal, PluginOperations.ShowSalesPerTerminalReport, Permission.ViewSalesReports);

            operations.AddOperation(Properties.Resources.StockLevelReport, PluginOperations.ShowStockLevelReport, Permission.ViewInventoryReports);
            operations.AddOperation(Properties.Resources.InventoryBelowReorderpoint, PluginOperations.ShowInventoryBelowReorderpointReport, Permission.ViewInventoryReports);
        }
    }

    #endregion
}