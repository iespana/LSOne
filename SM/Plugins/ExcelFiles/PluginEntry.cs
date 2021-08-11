using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.ExcelFiles.Properties;

namespace LSOne.ViewPlugins.ExcelFiles
{
    public class PluginEntry : IPlugin, IPluginDashboardProvider
    {
        internal static Guid ExcelFolderLocationSettingID = new Guid("EB2EE2BD-5A52-4A9A-B84B-176458668AF9");
        internal static Guid ExcelDashboardID = new Guid("F10221D5-C1F6-475E-9BF3-67E39033D3F8");

        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        #region IPlugin Members

        public string Description
        {
            get { return Resources.ExcelFilesPlugin; }
        }

        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            if (message == "ImportStockCounting")
            {
                if (parameters is List<ImportDescriptor>)
                {
                    ((List<ImportDescriptor>)parameters).Add(new ImportDescriptor(PluginOperations.ProcessImport, ".xls", Resources.ExcelFile));
                    ((List<ImportDescriptor>)parameters).Add(new ImportDescriptor(PluginOperations.ProcessImport, ".xlsx", Resources.ExcelFile));
                    ((List<ImportDescriptor>)parameters).Add(new ImportDescriptor(PluginOperations.ProcessImport, ".csv", Resources.CSVFile));
                }
                return false;
            }
            return false;
        }

        public object Message(object sender, string message, object parameters)
        {
            throw new NotImplementedException();
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            DataModel = dataModel;
            Framework = frameworkCallbacks;
            frameworkCallbacks.ViewController.AddContextBarItemConstructionHandler(PluginOperations.TaskBarItemCallback);
            frameworkCallbacks.ViewController.AddContextBarCategoryConstructionHandler(PluginOperations.TaskBarCategoryCallback);

            frameworkCallbacks.AddOperationCategoryConstructionHandler(new OperationCategoryEventHandler(PluginOperations.AddOperationCategoryHandler));
            frameworkCallbacks.AddOperationItemConstructionHandler(new OperationItemEventHandler(PluginOperations.AddOperationItemHandler));
            frameworkCallbacks.AddOperationButttonConstructionHandler(new OperationbuttonEventHandler(PluginOperations.AddOperationButtonHandler));

            frameworkCallbacks.AddRibbonPageConstructionHandler(new RibbonPageEventHandler(PluginOperations.AddRibbonPages));
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(new RibbonPageCategoryEventHandler(PluginOperations.AddRibbonPageCategories));
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(new RibbonPageCategoryItemEventHandler(PluginOperations.AddRibbonPageCategoryItems));

            frameworkCallbacks.AddMenuConstructionConstructionHandler(new MenuConstructionEventHandler(PluginOperations.ConstructMenus));
        }

        public void Dispose()
        {

        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation(Properties.Resources.ImportFromExcelOrCSV, PluginOperations.ImportFromExcelOrCSV, new string[] { Permission.ManageItemBarcodes, Permission.ManageRetailGroups, Permission.ItemsEdit, Permission.CustomerEdit, Permission.CurrencyEdit, Permission.VendorEdit });
            operations.AddOperation(Properties.Resources.CreateNewExcelTemplate, PluginOperations.CreateNewTemplate, new string[] { Permission.ManageItemBarcodes, Permission.ManageRetailGroups, Permission.ItemsEdit, Permission.CustomerEdit, Permission.CurrencyEdit, Permission.VendorEdit });
            operations.AddOperation("", "ImportStockCounting", false, false, PluginOperations.ImportStockCounting, new string[] { Permission.ManageItemBarcodes, Permission.ManageRetailGroups, Permission.ItemsEdit, Permission.CustomerEdit, Permission.CurrencyEdit, Permission.VendorEdit });
            operations.AddOperation("", "ExportSerialNumbers", false, false, PluginOperations.ExportSerialNumbers, new string[] { Permission.ManageSerialNumbers });
            operations.AddOperation("", "ImportSerialNumbers", false, false, PluginOperations.ImportSerialNumbers, new string[] { Permission.ManageSerialNumbers });
        }



        #endregion

        public void LoadDashboardItem(IConnectionManager threadedEntry, DashboardItem item)
        {
            if (item.ID == ExcelDashboardID)
            {
                var workingfolder = threadedEntry.Settings.GetSetting(threadedEntry,
                    PluginEntry.ExcelFolderLocationSettingID,
                    SettingType.Generic, null);
                if (workingfolder != null && !string.IsNullOrEmpty(workingfolder.Value))
                {
                    if (Directory.Exists(workingfolder.Value))
                    {
                        item.InformationText = Resources.DashboardItemInfo;

                        item.State = DashboardItem.ItemStateEnum.Info;
                    }
                    else
                    {
                        item.InformationText = Resources.DashboardItemConfigureWorkingFolderError;

                        item.State = DashboardItem.ItemStateEnum.Error;
                    }

                }
                else
                {
                    item.InformationText = Resources.DashboardItemConfigureWorkingFolder;

                    item.State = DashboardItem.ItemStateEnum.Error;
                }
                item.SetButton(0, Resources.DashboardImportButton, PluginOperations.ImportFromDashboard);
                item.SetButton(1, Resources.DashboardConfigureButton, PluginOperations.ConfigureWorkingExcelFolder);
            }
        }

        public void RegisterDashBoardItems(ViewCore.EventArguments.DashboardItemArguments args)
        {
            DashboardItem excelDashboardItem = new DashboardItem(ExcelDashboardID, Resources.ExcelImport,
                Framework.IsSiteManagerBasic || PluginEntry.DataModel.IsCloud, 60);

            args.Add(new DashboardItemPluginResolver(excelDashboardItem, this), 600);
        }
    }
}
