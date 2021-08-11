using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.IO;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.ExcelFiles.Dialogs;
using LSOne.ViewPlugins.ExcelFiles.Enums;
using LSOne.ViewPlugins.ExcelFiles.MappingLogic;
using LSOne.ViewPlugins.ExcelFiles.Properties;
using LSOne.DataLayer.GenericConnector;
using System.Linq;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.BusinessObjects.Profiles;
using System.ComponentModel;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.Services.Interfaces.SupportClasses.ImportExport;
using LSOne.DataLayer.BusinessObjects.SerialNumbers;
using LSOne.DataLayer.BusinessObjects.FileImport;
using LSOne.DataLayer.BusinessObjects.Companies;

namespace LSOne.ViewPlugins.ExcelFiles
{
    internal class PluginOperations
    {
        private static SiteServiceProfile siteServiceProfile;

        public static void TaskBarCategoryCallback(object sender, ContextBarHeaderConstructionArguments arguments)
        {
            if ((arguments.Key == "LSOne.ViewPlugins.Inventory.Views.StockCountingView"))
            {
                arguments.Add(new ContextBarHeader(Resources.Actions, arguments.Key + ".Actions"), 200);
            }
        }

        public static void ConstructMenus(object sender, MenuConstructionArguments args)
        {
            ExtendedMenuItem item;

            switch (args.Key)
            {
                case "StockCountingImportButton":
                    args.AddSeparator(10);

                    item = new ExtendedMenuItem(Resources.FromExcelDocument, 12, ImportFromExcelOrCSV)
                    {
                        Tag = new WeakReference(args.Context)
                    };

                    args.AddMenu(item);
                    break;

                case "RetailImportButton":
                    args.AddSeparator(10);
                    item = new ExtendedMenuItem(Resources.FromExcelDocument, 12, ImportFromExcelOrCSV)
                    {
                        Tag = new WeakReference(args.Context)
                    };
                    args.AddMenu(item);
                    break;


                case "RetailExportButton":

                    // NOTE: because we are limiting some of the options to basic the order of the code below does not directly represent
                    //       the order the "RetailExportButton" context menu displays items. Look at the priorities being passed to get 
                    //       the actual order of items
                    if (!PluginEntry.Framework.IsSiteManagerBasic)
                    {
                        item = new ExtendedMenuItem(Resources.ExportListToExcel, 10, ExportRetailItemsToExcelTemplate)
                        {
                            Tag = new WeakReference(args.Context)
                        };

                        args.AddMenu(item);

                        item = new ExtendedMenuItem(Resources.ExportSelectionToExcel, 20, ExportSelectionToExcelTemplate)
                        {
                            Tag = new WeakReference(args.Context),
                            Enabled = ((ViewBase)args.Context).GetListSelectionCount() > 0
                        };

                        //TODO Put back in after first release of 2016
                        //args.AddMenu(item);

                        args.AddSeparator(30);

                        item = new ExtendedMenuItem(Resources.ExportListToReplenishmentExcelTemplate, 40, ExportRetailItemsToExcelReplenishmentTemplate)
                        {
                            Tag = new WeakReference(args.Context)
                        };

                        args.AddMenu(item);

                        item = new ExtendedMenuItem(Resources.ExportSelectionToReplenishmentExcelTemplate, 50, ExportSelectionToExcelReplenishmentTemplate)
                        {
                            Tag = new WeakReference(args.Context),
                            Enabled = ((ViewBase)args.Context).GetListSelectionCount() > 0
                        };

                        //TODO Put back in after first release of 2016
                        //args.AddMenu(item);

                        item = new ExtendedMenuItem(Resources.ExportListToStockCountingExcelTemplate, 50, ExportRetailItemsToExcelStockCountingTemplate)
                        {
                            Tag = new WeakReference(args.Context),
                        };

                        args.AddMenu(item);
                    }


                    item = new ExtendedMenuItem(Resources.ExportListExcelSimple, 25, ExportRetailItemsToSimpleExcelTemplate)
                    {
                        Tag = new WeakReference(args.Context)
                    };

                    //TODO Put back in after first release of 2016
                    //args.AddMenu(item);
                    item = new ExtendedMenuItem(Resources.ExportSelectionExcelSimple, 30, ExportSelectionToSimpleExcelTemplate)
                    {
                        Tag = new WeakReference(args.Context),
                        Enabled = ((ViewBase)args.Context).GetListSelectionCount() > 0
                    };

                    //TODO Put back in after first release of 2016
                    //args.AddMenu(item);

                    break;
            }
        }

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {
            const int priority = 100;
            if (arguments.CategoryKey == "LSOne.ViewPlugins.Inventory.Views.StockCountingView.Actions")
            {
                ContextBarItem item = new ContextBarItem(Resources.ImportFromExcelDocument, ImportFromExcelOrCSV)
                {
                    Tag = new RecordIdentifier("Item", "")
                };

                arguments.Add(item, priority);
            }
            else if (arguments.CategoryKey == "LSOne.ViewPlugins.SerialNumbers.Views.SerialNumbersView.Actions")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.SerialNumberTemplate, "SerialNumberTemplate", true, DownloadSerialNumberTemplate), 3);
            }
        }

        private static void DownloadSerialNumberTemplate(object sender, ContextBarClickEventArguments args)
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = Resources.ExcelFile + " (*.xlsx)|*.xlsx",
                DefaultExt = ".xlsx"
            };
            Setting workingfolder = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, PluginEntry.ExcelFolderLocationSettingID,
             SettingType.Generic, null);

            if (workingfolder != null && !string.IsNullOrEmpty(workingfolder.Value) &&
                Directory.Exists(workingfolder.Value))
            {
                dlg.InitialDirectory = Path.Combine(workingfolder.Value, "Excel Import");
            }
            DialogResult dlgRes = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

            if (dlgRes != DialogResult.Cancel)
            {
                FolderItem source = new FolderItem(Application.ExecutablePath).Parent.Child("Templates").Child("Serial number import template.xlsx");

                if (!source.Exists)
                {
                    MessageDialog.Show(Resources.CouldNotFindBaseTemplateFile);
                    return;
                }

                try
                {
                    FolderItem destination = new FolderItem(dlg.FileName);
                    source.Copy(destination);
                    if (QuestionDialog.Show(Resources.NewExcelDocumentQuestion, Resources.NewExcelDocumentCaption) == DialogResult.Yes)
                    {
                        destination.Parent.Show();
                    }

                }
                catch (Exception)
                {
                    MessageDialog.Show(Resources.CouldNotWriteFileAtTheChoosenLocation);
                }
            }
        }

        private void DisposeCache()
        {

        }

        internal static void ImportStockCounting(object sender, PluginOperationArguments args)
        {
            ((List<ImportDescriptor>)args.Param).Add(new ImportDescriptor(PluginOperations.ProcessImport, ".xls", Resources.ExcelFile));
            ((List<ImportDescriptor>)args.Param).Add(new ImportDescriptor(PluginOperations.ProcessImport, ".xlsx", Resources.ExcelFile));
            ((List<ImportDescriptor>)args.Param).Add(new ImportDescriptor(PluginOperations.ProcessImport, ".csv", Resources.CSVFile));
        }

        internal static void ImportSerialNumbers(object sender, PluginOperationArguments args)
        {
            ((List<ImportDescriptor>)args.Param).Add(new ImportDescriptor(PluginOperations.ProcessSerialNumbersImport, ".xls", Resources.ExcelFile));
            ((List<ImportDescriptor>)args.Param).Add(new ImportDescriptor(PluginOperations.ProcessSerialNumbersImport, ".xlsx", Resources.ExcelFile));
            ((List<ImportDescriptor>)args.Param).Add(new ImportDescriptor(PluginOperations.ProcessSerialNumbersImport, ".csv", Resources.CSVFile));
        }

        internal static void ExportSerialNumbers(object sender, PluginOperationArguments args)
        {
            PluginOperations.ExportSerialNumbers((List<SerialNumber>)args.Param);
        }

        public static void CreateNewTemplate(object sender, EventArgs args)
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = Resources.ExcelFile + " (*.xlsx)|*.xlsx",
                DefaultExt = ".xlsx"
            };
            Setting workingfolder = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, PluginEntry.ExcelFolderLocationSettingID,
             SettingType.Generic, null);

            if (workingfolder != null && !string.IsNullOrEmpty(workingfolder.Value) &&
                Directory.Exists(workingfolder.Value))
            {
                dlg.InitialDirectory = Path.Combine(workingfolder.Value, "Excel Import");
            }
            DialogResult dlgRes = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

            if (dlgRes != DialogResult.Cancel)
            {
                FolderItem source = new FolderItem(Application.ExecutablePath).Parent.Child("Templates").Child("Import Template.xlsx");

                if (!source.Exists)
                {
                    MessageDialog.Show(Resources.CouldNotFindBaseTemplateFile);
                    return;
                }

                try
                {
                    FolderItem destination = new FolderItem(dlg.FileName);
                    source.Copy(destination);
                    if (QuestionDialog.Show(Resources.NewExcelDocumentQuestion, Resources.NewExcelDocumentCaption) == DialogResult.Yes)
                    {
                        destination.Parent.Show();

                    }

                }
                catch (Exception)
                {
                    MessageDialog.Show(Resources.CouldNotWriteFileAtTheChoosenLocation);
                }
            }
        }

        /// <summary>
        /// Handle export error messages
        /// </summary>
        /// <param name="errorCode">Export error code</param>
        /// <param name="worksSheetNames">Worksheet names with error</param>
        /// <returns>True if execution can continue</returns>
        private static bool PostErrorMessage(ExportErrorCodes errorCode, string worksSheetNames)
        {
            switch (errorCode)
            {
                case ExportErrorCodes.CouldNotOpenWorkbook:
                    MessageBox.Show(Resources.CouldNotOpenWorkbook, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;

                case ExportErrorCodes.CouldNotFindWorkSheet:
                    MessageBox.Show(Resources.CouldNotFindOneOfTheFollowingWorksheets.Replace("#", worksSheetNames), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;

                case ExportErrorCodes.CouldNotSaveFile:
                    MessageBox.Show(Resources.CouldNotWriteFileAtTheChoosenLocation, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;

                case ExportErrorCodes.CouldNotGenerateIdFromHQ:
                    MessageBox.Show(Resources.CouldNotGenerateIDFromHeadQuarters, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;

                default: return true;
            }
        }


        public static void ExportSerialNumbers(List<SerialNumber> serialNumbers)
        {
            string worksheetName = "Serial numbers";
            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = Resources.ExcelFile + " (*.xlsx)|*.xlsx",
                DefaultExt = ".xlsx"
            };
            Setting workingfolder = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, PluginEntry.ExcelFolderLocationSettingID,
            SettingType.Generic, null);

            if (workingfolder != null && !string.IsNullOrEmpty(workingfolder.Value) &&
                Directory.Exists(workingfolder.Value))
            {
                dlg.InitialDirectory = Path.Combine(workingfolder.Value, "Excel Import");
            }
            DialogResult dlgRes = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

            if (dlgRes != DialogResult.Cancel)
            {
                FolderItem source = new FolderItem(Application.ExecutablePath).Parent.Child("Templates").Child("Serial number export template.xlsx");

                if (!source.Exists)
                {
                    MessageDialog.Show(Resources.CouldNotFindBaseTemplateFile);
                    return;
                }
                try
                {
                    FolderItem destination = new FolderItem(dlg.FileName);

                    if (destination.Exists)
                    {
                        destination.Delete();
                    }

                    source.Copy(destination);

                    ExportErrorCodes status = ExportErrorCodes.NoError;
                    ExportSerialNumbers(ref status, destination, serialNumbers, worksheetName);

                    if (status != ExportErrorCodes.NoError && !PostErrorMessage(status, worksheetName))
                    {
                        return;
                    }

                    if (QuestionDialog.Show(Resources.ExportedFileQuestion, Resources.NewExcelDocumentCaption) == DialogResult.Yes)
                    {
                        destination.Parent.Show();
                    }
                }
                catch (Exception)
                {
                    MessageDialog.Show(Resources.CouldNotWriteFileAtTheChoosenLocation);
                }
            }
        }


        public static void ExportLogItems(List<ImportLogItem> logItems)
        {
            string worksheetName = "Log Items";
            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = Resources.ExcelFile + " (*.xlsx)|*.xlsx",
                DefaultExt = ".xlsx"
            };
            Setting workingfolder = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, PluginEntry.ExcelFolderLocationSettingID,
            SettingType.Generic, null);

            if (workingfolder != null && !string.IsNullOrEmpty(workingfolder.Value) &&
                Directory.Exists(workingfolder.Value))
            {
                dlg.InitialDirectory = Path.Combine(workingfolder.Value, "Excel Import");
            }
            DialogResult dlgRes = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

            if (dlgRes != DialogResult.Cancel)
            {
                FolderItem source = new FolderItem(Application.ExecutablePath).Parent.Child("Templates").Child("Import Error Log.xlsx");

                if (!source.Exists)
                {
                    MessageDialog.Show(Resources.CouldNotFindBaseTemplateFile);
                    return;
                }
                try
                {
                    FolderItem destination = new FolderItem(dlg.FileName);

                    if (destination.Exists)
                    {
                        destination.Delete();
                    }

                    source.Copy(destination);

                    ExportErrorCodes status = ExportErrorCodes.NoError;
                    ExportLogErrorItems(ref status, destination, logItems, worksheetName);

                    if (status != ExportErrorCodes.NoError && !PostErrorMessage(status, worksheetName))
                    {
                        return;
                    }

                    if (QuestionDialog.Show(Resources.ExportedFileQuestion, Resources.NewExcelDocumentCaption) == DialogResult.Yes)
                    {
                        destination.Parent.Show();
                    }
                }
                catch (Exception)
                {
                    MessageDialog.Show(Resources.CouldNotWriteFileAtTheChoosenLocation);
                }
            }
        }

        private static void ExportToTemplate(ViewBase view, bool all, ExportDelegate exportProc, string templateName, string worksSheetNames, bool simple = false)
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = Resources.ExcelFile + " (*.xlsx)|*.xlsx",
                DefaultExt = ".xlsx"
            };
            Setting workingfolder = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, PluginEntry.ExcelFolderLocationSettingID,
            SettingType.Generic, null);

            if (workingfolder != null && !string.IsNullOrEmpty(workingfolder.Value) &&
                Directory.Exists(workingfolder.Value))
            {
                dlg.InitialDirectory = Path.Combine(workingfolder.Value, "Excel Import");
            }
            DialogResult dlgRes = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

            if (dlgRes != DialogResult.Cancel)
            {
                FolderItem source = new FolderItem(Application.ExecutablePath).Parent.Child("Templates").Child(templateName);

                if (!source.Exists)
                {
                    MessageDialog.Show(Resources.CouldNotFindBaseTemplateFile);
                    return;
                }

                try
                {
                    FolderItem destination = new FolderItem(dlg.FileName);

                    if (destination.Exists)
                    {
                        destination.Delete();
                    }

                    source.Copy(destination);

                    ExportExcelProgressDialog exportDlg = new ExportExcelProgressDialog(exportProc, destination, view, all, simple);

                    if (exportDlg.ShowDialog() == DialogResult.OK)
                    {
                        if (exportDlg.ErrorCode != ExportErrorCodes.NoError && !PostErrorMessage(exportDlg.ErrorCode, worksSheetNames))
                        {
                            return;
                        }

                        if (QuestionDialog.Show(Resources.ExportedFileQuestion, Resources.NewExcelDocumentCaption) == DialogResult.Yes)
                        {
                            destination.Parent.Show();
                        }
                    }
                }
                catch (Exception)
                {
                    MessageDialog.Show(Resources.CouldNotWriteFileAtTheChoosenLocation);
                }
            }
        }

        private static void ExportRetailItems(ViewBase view, bool all)
        {
            ExportToTemplate(view, all, ExportRetailItems, "Import Template.xlsx", "Retail items"); // Name of document and worksheets (for error reporting) so do not localize this string
        }

        private static void ExportRetailItemsToSimpleTemplate(ViewBase view, bool all)
        {
            ExportToTemplate(view, all, ExportRetailItems, "Basic Import Template.xlsx", "Retail items", true); // Name of document and worksheets (for error reporting) so do not localize this string
        }


        private static void ExportRetailReplenishmentItems(ViewBase view, bool all)
        {
            ExportToTemplate(view, all, ExportReplenishmentRetailItems, "Replenishment template.xlsx", "Item replenishment, Item store replenishment"); // Name of document and worksheets (for error reporting) so do not localize this string
        }

        private static void ExportRetailStockCountingItems(ViewBase view, bool all)
        {
            ExportToTemplate(view, all, ExportStockCountingRetailItems, "Stock counting template.xlsx", "StockCountingHeader, StockCountingLine"); // Name of document and worksheets (for error reporting) so do not localize this string
        }

        private static void ExportSelectionToSimpleExcelTemplate(object sender, EventArgs args)
        {
            ExportRetailItemsToSimpleTemplate((ViewBase)((WeakReference)((ToolStripMenuItem)sender).Tag).Target, false);
        }

        private static void ExportSelectionToExcelTemplate(object sender, EventArgs args)
        {
            ExportRetailItems((ViewBase)((WeakReference)((ToolStripMenuItem)sender).Tag).Target, false);
        }

        private static void ExportRetailItemsToExcelTemplate(object sender, EventArgs args)
        {
            ExportRetailItems((ViewBase)((WeakReference)((ToolStripMenuItem)sender).Tag).Target, true);
        }

        private static void ExportRetailItemsToSimpleExcelTemplate(object sender, EventArgs args)
        {
            ExportRetailItemsToSimpleTemplate((ViewBase)((WeakReference)((ToolStripMenuItem)sender).Tag).Target, true);
        }

        private static void ExportSelectionToExcelReplenishmentTemplate(object sender, EventArgs args)
        {
            ExportRetailReplenishmentItems((ViewBase)((WeakReference)((ToolStripMenuItem)sender).Tag).Target, false);
        }

        private static void ExportRetailItemsToExcelReplenishmentTemplate(object sender, EventArgs args)
        {
            ExportRetailReplenishmentItems((ViewBase)((WeakReference)((ToolStripMenuItem)sender).Tag).Target, true);
        }

        private static void ExportRetailItemsToExcelStockCountingTemplate(object sender, EventArgs args)
        {
            ExportRetailStockCountingItems((ViewBase)((WeakReference)((ToolStripMenuItem)sender).Tag).Target, true);

        }

        private static void ExportStockCountingRetailItems(ref bool canceled, ref bool done, ref ExportErrorCodes errorCode,
            FolderItem destination, object context, object hint, bool simple)
        {
            // We are inside a thread proc here note that !!!
            IExcelService service = Services.Interfaces.Services.ExcelService(PluginEntry.DataModel);

            WorkbookHandle workBook = service.OpenForEditing(destination);
            if (workBook == null)
            {
                errorCode = ExportErrorCodes.CouldNotOpenWorkbook;
                done = true;
                return;
            }

            WorksheetHandle headerSheet = service.GetWorksheet(workBook, "StockCountingHeader");

            if (headerSheet == null)
            {
                errorCode = ExportErrorCodes.CouldNotFindWorkSheet;
                done = true;
                return;
            }

            int[] headerColumnMapping = service.GetColumnMapping(headerSheet,
                new[]
                {
                    "Stock counting Id", "Store Id", "Description"
                });

            ViewBase contextView = (ViewBase)context;

            RecordIdentifier numberSequence = RecordIdentifier.Empty;

            try
            {
                ISiteServiceService siteService = Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel);
                numberSequence = siteService.GenerateInventoryAdjustmentID(PluginEntry.DataModel, GetSiteServiceProfile(), true);
            }
            catch
            {
                errorCode = ExportErrorCodes.CouldNotGenerateIdFromHQ;
            }

            service.SetCellValue(headerSheet, 1, headerColumnMapping[0], (string)numberSequence);
            service.SetCellValue(headerSheet, 1, headerColumnMapping[1], (string)PluginEntry.DataModel.CurrentStoreID);
            service.SetCellValue(headerSheet, 1, headerColumnMapping[2], Date.Now.DateTime.ToString());


            List<RecordIdentifier> retailItemIDs;
            bool all = (bool)hint;
            if (all)
            {
                retailItemIDs = contextView.GetListKeys();
            }
            else
            {
                retailItemIDs = contextView.GetListSelectionKeys();
            }

            WorksheetHandle lineSheet = service.GetWorksheet(workBook, "StockCountingLine");
            if (lineSheet == null)
            {
                errorCode = ExportErrorCodes.CouldNotFindWorkSheet;
                done = true;
                return;
            }

            int[] lineColumnMapping = service.GetColumnMapping(lineSheet,
                new[]
                {
                    "Stock counting Id","Item Id", "Unit Id", "Variant description", "Counted", "Description"
                });
            RetailItem item;
            int rowNumber = 1;
            foreach (RecordIdentifier id in retailItemIDs)
            {
                if (canceled)
                {
                    done = true;
                    return;
                }

                item = Providers.RetailItemData.Get(PluginEntry.DataModel, id);
                if (item == null)
                {
                    continue;
                }

                if (item.ItemType == ItemTypeEnum.MasterItem)
                {
                    continue;
                }
                service.SetCellValue(lineSheet, rowNumber, lineColumnMapping[0], (string)numberSequence);
                service.SetCellValue(lineSheet, rowNumber, lineColumnMapping[1], (string)id);
                service.SetCellValue(lineSheet, rowNumber, lineColumnMapping[2], (string)item.InventoryUnitID);
                service.SetCellValue(lineSheet, rowNumber, lineColumnMapping[3], item.VariantName != "" ? item.VariantName : "");
                service.SetCellValue(lineSheet, rowNumber, lineColumnMapping[4], 0);
                service.SetCellValue(lineSheet, rowNumber, lineColumnMapping[5], item.Text);

                rowNumber++;
            }

            if (canceled)
            {
                done = true;
                return;
            }

            try
            {
                service.Save(workBook, destination);
            }
            catch (Exception)
            {
                errorCode = ExportErrorCodes.CouldNotSaveFile;
            }

            done = true;
        }

        private static bool ShouldExportRetailItemToExcel(RetailItem item, List<RecordIdentifier> retailItemIDs, bool showVariants)
        {
            // retail items are always exported
            bool displayRetailItem = item.HeaderItemID == RecordIdentifier.Empty;

            // variant items are exported if explecitly requested or if parent header is selected
            bool displayVariantItem = showVariants;
            if (!displayRetailItem && !showVariants)
            {
                string parentVariantHeaderId = (string)Providers.RetailItemData.GetItemIDFromMasterID(PluginEntry.DataModel, item.HeaderItemID);
                displayVariantItem = retailItemIDs.Any(a => (string)a == parentVariantHeaderId);
            }

            return displayRetailItem || displayVariantItem;
        }

        private static void ExportReplenishmentRetailItems(ref bool canceled, ref bool done, ref ExportErrorCodes errorCode, FolderItem destination, object context, object hint, bool simple = false)
        {
            // We are inside a thread proc here note that !!!
            IExcelService service = Services.Interfaces.Services.ExcelService(PluginEntry.DataModel);

            WorkbookHandle workBook = service.OpenForEditing(destination);
            if (workBook == null)
            {
                errorCode = ExportErrorCodes.CouldNotOpenWorkbook;
                done = true;
                return;
            }

            WorksheetHandle itemReplenishmentSheet = service.GetWorksheet(workBook, "Item replenishment");

            if (itemReplenishmentSheet == null)
            {
                errorCode = ExportErrorCodes.CouldNotFindWorkSheet;
                done = true;
                return;
            }

            int[] itemReplenishmentColumnMapping = service.GetColumnMapping(itemReplenishmentSheet,
                new[]
                {
                    "ITEMID", "ITEM DESCRIPTION", "VARIANT DESCRIPTION", "STOREID", "STORE DESCRIPTION",
                    "REORDER POINT", "MAXIMUM INVENTORY", "PURCHASE ORDER MULTIPLE",
                    "PURCHASE ORDER MULTIPLE ROUNDING (Nearest, Down, Up)",
                    "BLOCKED (0, 1)", "BLOCKING DATE"
                });

            ViewBase contextView = (ViewBase)context;

            List<RecordIdentifier> retailItemIDs;
            bool all = (bool)hint;
            if (all)
            {
                retailItemIDs = contextView.GetListKeys();
            }
            else
            {
                retailItemIDs = contextView.GetListSelectionKeys();
            }

            bool showVariants = (bool)contextView.Message("ShowVariants", new object());
            int rowNumber = 1;
            foreach (RecordIdentifier id in retailItemIDs)
            {
                if (canceled)
                {
                    done = true;
                    return;
                }

                RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, id);
                if (item != null)
                {
                    if (ShouldExportRetailItemToExcel(item, retailItemIDs, showVariants))
                    {
                        ItemReplenishmentSettingsContainer itemReplenishment = Providers.ItemReplenishmentSettingData.GetReplenishmentSettingsForExcel(PluginEntry.DataModel, id);

                        if (itemReplenishment.ItemReplenishmentSetting.Count == 0)
                        {
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[0], (string)id);
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[1], item.Text);
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[2], item.VariantName);
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[3], RecordIdentifier.IsEmptyOrNull(PluginEntry.DataModel.CurrentStoreID) ? "" : PluginEntry.DataModel.CurrentStoreID);
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[4], "");
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[5], 0M, new DecimalLimit(0));
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[6], 0M, new DecimalLimit(0));
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[7], 0);
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[8], "Nearest");
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[9], 0, new DecimalLimit(0));
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[10], Date.Now.DateTime.Date, ExcelStandardFormats.CurrentLocaleShortDate);

                            rowNumber++;
                        }

                        foreach (ItemReplenishmentSetting settings in itemReplenishment.ItemReplenishmentSetting)
                        {
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[0], (string)settings.ItemId);
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[1], settings.ItemName);
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[2], item.VariantName);
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[3], (string)settings.StoreId);
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[4], settings.StoreName);
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[5], settings.ReorderPoint, settings.Unit.Limit);
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[6], settings.MaximumInventory, settings.Unit.Limit);
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[7], settings.PurchaseOrderMultiple);
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[8], ItemReplenishmentMapper.MapPurchaseOrderMultipleRounding(settings.PurchaseOrderMultipleRounding));
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[9], ItemReplenishmentMapper.MapPurchaseOrderReplenishmentBlocking(settings.BlockedForReplenishment), new DecimalLimit(0));
                            service.SetCellValue(itemReplenishmentSheet, rowNumber, itemReplenishmentColumnMapping[10], settings.BlockingDate.Date, ExcelStandardFormats.CurrentLocaleShortDate);

                            rowNumber++;
                        }
                    }
                }
            }

            if (canceled)
            {
                done = true;
                return;
            }

            try
            {
                service.Save(workBook, destination);
            }
            catch (Exception)
            {
                errorCode = ExportErrorCodes.CouldNotSaveFile;
            }

            done = true;
        }

        private static void ExportRetailItems(ref bool canceled, ref bool done, ref ExportErrorCodes errorCode,
            FolderItem destination, object context, object hint, bool simple)
        {
            Dictionary<Guid, string> groups = new Dictionary<Guid, string>();
            Dictionary<Guid, string> departments = new Dictionary<Guid, string>();


            List<RetailGroup> retailGroups = Providers.RetailGroupData.GetDetailedList(PluginEntry.DataModel, RetailGroupSorting.RetailGroupName,
                false);
            foreach (RetailGroup retailGroup in retailGroups)
            {
                groups.Add((Guid)retailGroup.MasterID, (string)retailGroup.ID);
            }

            List<RetailDepartment> retaildepartments = Providers.RetailDepartmentData.GetDetailedList(PluginEntry.DataModel,
                RetailDepartment.SortEnum.RetailDepartment, false);

            foreach (RetailDepartment retailDepartment in retaildepartments)
            {
                departments.Add((Guid)retailDepartment.MasterID, (string)retailDepartment.ID);
            }

            // We are inside a thread proc here note that !!!
            IExcelService service = Services.Interfaces.Services.ExcelService(PluginEntry.DataModel);
            WorkbookHandle workBook = service.OpenForEditing(destination);
            if (workBook == null)
            {
                errorCode = ExportErrorCodes.CouldNotOpenWorkbook;
                done = true;
                return;
            }

            WorksheetHandle retailItemsSheet = service.GetWorksheet(workBook, "Retail items");
            if (retailItemsSheet == null)
            {
                errorCode = ExportErrorCodes.CouldNotFindWorkSheet;
                done = true;
                return;
            }

            int[] columnMapping = simple ?
                service.GetColumnMapping(retailItemsSheet,
              new[]
                {
                    "ITEMID", "DESCRIPTION", "SEARCH ALIAS", "NOTES",
                    "INVENTORY UNIT", "SALES UNIT", "RETAIL GROUP",
                    "RETAIL DEPARTMENT", "SALES TAX GROUP", "COST PRICE",
                    "SALES PRICE", "BARCODESETUP", "BARCODE", "BARCODEUNIT"

                })
                :

                service.GetColumnMapping(retailItemsSheet,
                new[]
                {
                    "ITEMID", "VARIANT HEADER ID", "DESCRIPTION", "VARIANT DESCRIPTION", "DIMENSIONS/ATTRIBUTES", "SEARCH ALIAS", "ITEM TYPE", "NOTES",
                    "INVENTORY UNIT", "SALES UNIT", "RETAIL GROUP",
                    "RETAIL DEPARTMENT", "SALES TAX GROUP", "COST PRICE",
                    "SALES PRICE", "BARCODESETUP", "BARCODE", "BARCODEUNIT",
                    "SCALE ITEM", "", "TARE WEIGHT", "KEY IN PRICE", "KEY IN QUANTITY",
                    "ZERO PRICE VALID", "NO DISCOUNT ALLOWED",
                    "MUST KEY IN COMMENT", "QTY BECOMES NEGATIVE", "MUST SELECT UNIT",
                    "VENDOR ITEMID", "VENDORID", "VENDOR PURCHASE PRICE", "KEY IN SERIAL NUMBER"
                });

            ViewBase contextView = (ViewBase)context;

            List<RecordIdentifier> retailItemIDs;
            bool all = (bool)hint;
            if (all)
            {
                retailItemIDs = contextView.GetListKeys();
            }
            else
            {
                retailItemIDs = contextView.GetListSelectionKeys();
            }

            bool pricesAreWithTax = false;

            RecordIdentifier defaultStoreID = Providers.StoreData.GetDefaultStoreID(PluginEntry.DataModel);

            if (defaultStoreID != null && defaultStoreID != "")
            {
                pricesAreWithTax = Providers.StoreData.GetPriceWithTaxForStore(PluginEntry.DataModel, defaultStoreID);
            }

            DecimalLimit priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            DecimalLimit quantityLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);

            bool showVariants = (bool)contextView.Message("ShowVariants", new object());

            int rowNumber = 1;
            foreach (RecordIdentifier id in retailItemIDs)
            {
                if (canceled)
                {
                    done = true;
                    return;
                }
                RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, id);
                if (item != null)
                {
                    if (ShouldExportRetailItemToExcel(item, retailItemIDs, showVariants))
                    {
                        string department = item.RetailDepartmentMasterID.IsEmpty ? "" : departments[(Guid)item.RetailDepartmentMasterID];
                        string group = item.RetailGroupMasterID.IsEmpty ? "" : groups[(Guid)item.RetailGroupMasterID];

                        service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[0], (string)item.ID);

                        string variantHeaderId = (string)Providers.RetailItemData.GetItemIDFromMasterID(PluginEntry.DataModel, item.HeaderItemID);

                        service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[1], variantHeaderId);
                        service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[2], item.Text);
                        service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[3], item.VariantName);
                        service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[4], RetailItemMapper.MapDimensionAttributesFromItem(item));
                        service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[5], item.NameAlias);
                        service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[6], RetailItemMapper.MapItemType(item.ItemType));
                        service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[7], item.ExtendedDescription);
                        service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[8], (string)item.InventoryUnitID);
                        service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[9], (string)item.SalesUnitID);
                        service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[10], @group);
                        service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[11], department);
                        service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[12], (string)item.SalesTaxItemGroupID);
                        service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[13], item.PurchasePrice, priceLimiter);
                        service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[14], pricesAreWithTax ? item.SalesPriceIncludingTax : item.SalesPrice, priceLimiter);

                        BarCode barCode = Providers.BarCodeData.GetBarCodeForItem(PluginEntry.DataModel, id);

                        if (barCode != null)
                        {
                            service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[15], (string)barCode.BarCodeSetupID, ExcelStandardFormats.Textual);
                            service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[16], (string)barCode.ItemBarCode, ExcelStandardFormats.Textual);
                            service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[17], (string)barCode.UnitID, ExcelStandardFormats.Textual);
                        }

                        if (!simple)
                        {
                            service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[18], item.ScaleItem ? "1" : "0");
                            service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[19], "");
                            service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[20], item.TareWeight);
                            service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[21], RetailItemMapper.MapKeyInPrice(item.KeyInPrice));
                            service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[22], RetailItemMapper.MapKeyInQuantity(item.KeyInQuantity));
                            service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[23], item.ZeroPriceValid ? "1" : "0");
                            service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[24], item.NoDiscountAllowed ? "1" : "0");
                            service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[25], item.MustKeyInComment ? "1" : "0");
                            service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[26], item.QuantityBecomesNegative ? "1" : "0");
                            service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[27], item.MustSelectUOM ? "1" : "0");

                            if (item.DefaultVendorID != "")
                            {
                                VendorItem vendorItem = Providers.VendorItemData.GetVendorForItem(PluginEntry.DataModel, item.ID, item.DefaultVendorID);

                                if (vendorItem != null)
                                {
                                    service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[28], (string)vendorItem.VendorItemID);
                                    service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[29], (string)item.DefaultVendorID);
                                    service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[30], vendorItem.DefaultPurchasePrice, priceLimiter);
                                }
                            }

                            service.SetCellValue(retailItemsSheet, rowNumber, columnMapping[31], RetailItemMapper.MapKeyInSerialNumber(item.KeyInSerialNumber));
                        }
                        rowNumber++;
                    }
                }
            }
            if (canceled)
            {
                done = true;
                return;
            }

            WorksheetHandle retailDepartmentsheet = service.GetWorksheet(workBook, "Retail department");
            if (retailDepartmentsheet == null)
            {
                errorCode = ExportErrorCodes.CouldNotFindWorkSheet;
                done = true;
                return;
            }

            columnMapping =
                service.GetColumnMapping(retailDepartmentsheet,
                    new[]
                    {
                        "ID", "DESCRIPTION"

                    });

            List<DataEntity> retailDepartments = Providers.RetailDepartmentData.GetList(PluginEntry.DataModel);

            rowNumber = 1;
            foreach (DataEntity retailDepartment in retailDepartments)
            {
                if (canceled)
                {
                    done = true;
                    return;
                }

                service.SetCellValue(retailDepartmentsheet, rowNumber, columnMapping[0], (string)retailDepartment.ID);
                service.SetCellValue(retailDepartmentsheet, rowNumber, columnMapping[1], retailDepartment.Text);

                rowNumber++;
            }

            WorksheetHandle retailGroupssheet = service.GetWorksheet(workBook, "Retail groups");
            if (retailGroupssheet == null)
            {
                errorCode = ExportErrorCodes.CouldNotFindWorkSheet;
                done = true;
                return;
            }

            columnMapping =
                service.GetColumnMapping(retailGroupssheet,
                    new[]
                    {
                        "ID", "DESCRIPTION","RETAIL DEPARTMENT ID", "SALES TAX GROUP", "TARE WEIGHT"

                    });

            rowNumber = 1;

            foreach (RetailGroup retailGroup in retailGroups)
            {
                if (canceled)
                {
                    done = true;
                    return;
                }

                service.SetCellValue(retailGroupssheet, rowNumber, columnMapping[0], (string)retailGroup.ID);
                service.SetCellValue(retailGroupssheet, rowNumber, columnMapping[1], retailGroup.Text);
                service.SetCellValue(retailGroupssheet, rowNumber, columnMapping[2], (string)retailGroup.RetailDepartmentID);
                service.SetCellValue(retailGroupssheet, rowNumber, columnMapping[3], (string)retailGroup.ItemSalesTaxGroupId);
                service.SetCellValue(retailGroupssheet, rowNumber, columnMapping[4], retailGroup.TareWeight);

                rowNumber++;
            }

            if (canceled)
            {
                done = true;
                return;
            }

            try
            {
                service.Save(workBook, destination);
            }
            catch (Exception)
            {
                errorCode = ExportErrorCodes.CouldNotSaveFile;
            }

            done = true;
        }

        private static void ExportRetailDepartments(ref bool canceled, ref bool done, ref ExportErrorCodes errorCode, FolderItem destination, object context, object hint, bool simple)
        {
            // We are inside a thread proc here note that !!!
            IExcelService service = Services.Interfaces.Services.ExcelService(PluginEntry.DataModel);
            WorkbookHandle workBook = service.OpenForEditing(destination);
            if (workBook == null)
            {
                errorCode = ExportErrorCodes.CouldNotOpenWorkbook;
                done = true;
                return;
            }

            WorksheetHandle retailDepartmentsheet = service.GetWorksheet(workBook, "Retail department");
            if (retailDepartmentsheet == null)
            {
                errorCode = ExportErrorCodes.CouldNotFindWorkSheet;
                done = true;
                return;
            }

            int[] columnMapping =
                service.GetColumnMapping(retailDepartmentsheet,
                    new[]
                    {
                        "ID", "DESCRIPTION"
                    });

            List<DataEntity> retailDepartments = Providers.RetailDepartmentData.GetList(PluginEntry.DataModel);

            int rowNumber = 1;
            foreach (DataEntity retailDepartment in retailDepartments)
            {
                if (canceled)
                {
                    done = true;
                    return;
                }

                service.SetCellValue(retailDepartmentsheet, rowNumber, columnMapping[0], (string)retailDepartment.ID);
                service.SetCellValue(retailDepartmentsheet, rowNumber, columnMapping[1], retailDepartment.Text);

                rowNumber++;
            }
            if (canceled)
            {
                done = true;
                return;
            }


            try
            {
                service.Save(workBook, destination);
            }
            catch (Exception)
            {
                errorCode = ExportErrorCodes.CouldNotSaveFile;
            }

            done = true;
        }


        private static void ExportRetailGroups(ref bool canceled, ref bool done, ref ExportErrorCodes errorCode,
        FolderItem destination, object context, object hint, bool simple)
        {
            // We are inside a thread proc here note that !!!
            IExcelService service = Services.Interfaces.Services.ExcelService(PluginEntry.DataModel);
            WorkbookHandle workBook = service.OpenForEditing(destination);
            if (workBook == null)
            {
                errorCode = ExportErrorCodes.CouldNotOpenWorkbook;
                done = true;
                return;
            }

            WorksheetHandle retailGroupssheet = service.GetWorksheet(workBook, "Retail groups");
            if (retailGroupssheet == null)
            {
                errorCode = ExportErrorCodes.CouldNotFindWorkSheet;
                done = true;
                return;
            }

            int[] columnMapping =
                service.GetColumnMapping(retailGroupssheet,
                    new[]
                    {
                        "ID", "DESCRIPTION","RETAIL DEPARTMENT ID", "SALES TAX GROUP", "TARE WEIGHT"

                    });

            List<DataEntity> retailGroups = Providers.RetailGroupData.GetList(PluginEntry.DataModel);

            int rowNumber = 1;

            DecimalLimit quantityLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);

            foreach (DataEntity retailGroup in retailGroups)
            {
                if (canceled)
                {
                    done = true;
                    return;
                }

                RetailGroup group = Providers.RetailGroupData.Get(PluginEntry.DataModel, retailGroup.ID);
                service.SetCellValue(retailGroupssheet, rowNumber, columnMapping[0], (string)group.ID);
                service.SetCellValue(retailGroupssheet, rowNumber, columnMapping[1], group.Text);
                service.SetCellValue(retailGroupssheet, rowNumber, columnMapping[2], (string)group.RetailDepartmentID);
                service.SetCellValue(retailGroupssheet, rowNumber, columnMapping[3], (string)group.ItemSalesTaxGroupId);
                service.SetCellValue(retailGroupssheet, rowNumber, columnMapping[4], group.TareWeight);

                rowNumber++;

            }
            if (canceled)
            {
                done = true;
                return;
            }
            try
            {
                service.Save(workBook, destination);
            }
            catch (Exception)
            {
                errorCode = ExportErrorCodes.CouldNotSaveFile;
            }

            done = true;
        }

        private static void ExportLogErrorItems(ref ExportErrorCodes errorCode, FolderItem destination, List<ImportLogItem> logItems, string worksheetName)
        {
            IExcelService service = Services.Interfaces.Services.ExcelService(PluginEntry.DataModel);
            WorkbookHandle workBook = service.OpenForEditing(destination);
            if (workBook == null)
            {
                errorCode = ExportErrorCodes.CouldNotOpenWorkbook;
                return;
            }

            WorksheetHandle logItemsWorksheet = service.GetWorksheet(workBook, worksheetName);
            if (logItemsWorksheet == null)
            {
                errorCode = ExportErrorCodes.CouldNotFindWorkSheet;
                return;
            }

            int[] columnMapping =
                service.GetColumnMapping(logItemsWorksheet,
                    new[]
                    {
                        "LINE NUMBER", "ITEM ID","ITEM DESCRIPTION","ERROR MESSAGE"
                    });

            int rowNumber = 1;
            foreach (ImportLogItem logItem in logItems)
            {
                service.SetCellValue(logItemsWorksheet, rowNumber, columnMapping[0], logItem.LineNumber.HasValue ? logItem.LineNumber.Value.ToString() : string.Empty);
                service.SetCellValue(logItemsWorksheet, rowNumber, columnMapping[1], logItem.ID.ToString());
                service.SetCellValue(logItemsWorksheet, rowNumber, columnMapping[2], logItem.ItemDescription);
                service.SetCellValue(logItemsWorksheet, rowNumber, columnMapping[3], logItem.Reason);

                rowNumber++;
            }

            try
            {
                service.Save(workBook, destination);
            }
            catch (Exception)
            {
                errorCode = ExportErrorCodes.CouldNotSaveFile;
            }
        }

        private static void ExportSerialNumbers(ref ExportErrorCodes errorCode, FolderItem destination, List<SerialNumber> serialNumbers, string worksheetName)
        {
            IExcelService service = Services.Interfaces.Services.ExcelService(PluginEntry.DataModel);
            WorkbookHandle workBook = service.OpenForEditing(destination);
            if (workBook == null)
            {
                errorCode = ExportErrorCodes.CouldNotOpenWorkbook;
                return;
            }

            WorksheetHandle logItemsWorksheet = service.GetWorksheet(workBook, worksheetName);
            if (logItemsWorksheet == null)
            {
                errorCode = ExportErrorCodes.CouldNotFindWorkSheet;
                return;
            }

            int[] columnMapping =
                service.GetColumnMapping(logItemsWorksheet,
                    new[]
                    {
                        "ITEMID",  "DESCRIPTION", "VARIANT", "SERIALNUMBER"  ,  "TYPE"   , "SOLD"    ,"REFERENCE" ,  "MANUALENTRY"

                    });

            int rowNumber = 1;
            foreach (SerialNumber sn in serialNumbers)
            {
                service.SetCellValue(logItemsWorksheet, rowNumber, columnMapping[0], sn.ItemID);
                service.SetCellValue(logItemsWorksheet, rowNumber, columnMapping[1], sn.ItemDescription);
                service.SetCellValue(logItemsWorksheet, rowNumber, columnMapping[2], sn.ItemVariant);
                service.SetCellValue(logItemsWorksheet, rowNumber, columnMapping[3], sn.SerialNo);
                service.SetCellValue(logItemsWorksheet, rowNumber, columnMapping[4], SerialNumber.GetTypeOfSerialString(sn.SerialType));
                service.SetCellValue(logItemsWorksheet, rowNumber, columnMapping[5], sn.UsedDate.HasValue ? sn.UsedDate.ToString() : string.Empty);
                service.SetCellValue(logItemsWorksheet, rowNumber, columnMapping[6], sn.Reference);
                service.SetCellValue(logItemsWorksheet, rowNumber, columnMapping[7], sn.ManualEntry ? Resources.True : Resources.False);
                rowNumber++;
            }

            try
            {
                service.Save(workBook, destination);
            }
            catch (Exception)
            {
                errorCode = ExportErrorCodes.CouldNotSaveFile;
            }
        }

        public static void ConfigureWorkingExcelFolder(object sender, EventArgs args)
        {
            ConfigureExcelFolder dlg = new ConfigureExcelFolder();
            dlg.ShowDialog();
        }

        public static void ImportFromDashboard(object sender, EventArgs args)
        {
            Setting workingfolder = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, PluginEntry.ExcelFolderLocationSettingID, SettingType.Generic, null);

            if (workingfolder == null || string.IsNullOrEmpty(workingfolder.Value) || !Directory.Exists(workingfolder.Value))
            {
                ConfigureWorkingExcelFolder(null, new EventArgs());
                workingfolder = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, PluginEntry.ExcelFolderLocationSettingID, SettingType.Generic, null);
            }

            if (workingfolder != null && !string.IsNullOrEmpty(workingfolder.Value) && Directory.Exists(workingfolder.Value))
            {
                ImportFromExcelOrCSV(sender, args);
            }
            else
            {
                MessageBox.Show(Resources.WorkingFolderNotDefined + " " + Resources.DefineWorkingFolderBeforeContinuing, Resources.WorkingFolder, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void ImportFromExcelOrCSV(object sender, EventArgs args)
        {
            Setting workingfolder = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, PluginEntry.ExcelFolderLocationSettingID,
              SettingType.Generic, null);

            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = Resources.ExcelOrCSVFile + " (*.xls;*.xlsx)|*.xls;*.xlsx|" +
                             Resources.ExcelFile + " (*.xls;*.xlsx)|*.xls;*.xlsx"
            };
            if (workingfolder != null && !string.IsNullOrEmpty(workingfolder.Value) && Directory.Exists(workingfolder.Value))
            {
                dlg.InitialDirectory = Path.Combine(workingfolder.Value, "Excel Import");
            }
            DialogResult dlgRes = dlg.ShowDialog(PluginEntry.Framework.MainWindow);
            if (dlgRes != DialogResult.Cancel)
            {

                try
                {
                    FolderItem file = new FolderItem(dlg.FileName);

                    if (file.FileType == "xls" || file.FileType == "xlsx")
                    {
                        ProcessExcelImport(PluginEntry.DataModel, file, true);
                    }
                    else
                    {
                        // Handle CSV here
                    }
                }
                catch (FileNotFoundException)
                {
                    MessageDialog.Show(Resources.TheRequestedFileWasNotFound);
                }
                catch (Exception e)
                {
                    MessageDialog.Show(Resources.UnknownError + " " + e.Message);
                }
            }
        }

        public static DataTable ImportFromCSV(IConnectionManager entry, FolderItem file, List<ImportProfileLine> columns, bool hasHeaders, string tableName = "")
        {
            if (file == null || !file.Exists)
            {
                throw new FileNotFoundException();
            }

            if (columns == null || columns.Count == 0)
            {
                return new DataTable();
            }

            DataTable result = new DataTable(tableName);

            foreach (ImportProfileLine column in columns)
            {
                DataColumn dc = new DataColumn(column.Field.GetAttributeOfType<DescriptionAttribute>().Description.ToUpper());
                switch (column.FieldType)
                {
                    case FieldType.String:
                        dc.DataType = typeof(string);
                        break;
                    case FieldType.Decimal:
                        dc.DataType = typeof(double);
                        break;
                    case FieldType.Integer:
                        dc.DataType = typeof(int);
                        break;
                    default:
                        dc.DataType = typeof(string);
                        break;
                }
                result.Columns.Add(dc);
            }

            int nrOfcolumns = result.Columns.Count;

            using (CSVReader reader = new CSVReader(file.AbsolutePath, nrOfcolumns, hasHeaders))
            {
                reader.Read(result);
            }

            return result;
        }

        public static void ShowImportResultsView(IConnectionManager entry, List<FileImportLogItem> fileImportLogItems, ImportTypeEnum importType)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ExcelImportResultsView(fileImportLogItems, importType));
        }

        public static void ShowImportResultsView(IConnectionManager entry, List<ImportLogItem> importLogItems, ImportTypeEnum importType)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ExcelImportResultsView(importLogItems, importType));
        }

        public static void ProcessImport(IConnectionManager entry, List<ImportFileItem> files)
        {
            List<FileImportLogItem> fileLogImportItems = new List<FileImportLogItem>();

            files.ForEach(file =>
            {
                FileImportLogItem logItem;
                if (Path.GetExtension(file.FolderItem.AbsolutePath).Equals(".csv"))
                {
                    logItem = ProcessCsvImport(entry, file.FolderItem, file.RecordIdentifier, file.InventoryAdjustment);
                }
                else
                {
                    logItem = ProcessExcelImport(entry, file.FolderItem);
                }
                if (logItem != null)
                {
                    fileLogImportItems.Add(logItem);
                }
            });
            ShowImportResultsView(entry, fileLogImportItems, ImportTypeEnum.StockCounting);
        }

        public static void ProcessSerialNumbersImport(IConnectionManager entry, List<ImportFileItem> files)
        {
            List<FileImportLogItem> fileLogImportItems = new List<FileImportLogItem>();

            files.ForEach(file =>
            {
                FileImportLogItem logItem;
                if (Path.GetExtension(file.FolderItem.AbsolutePath).Equals(".csv"))
                {
                    logItem = ProcessSerialNumbersCsvImport(entry, file.FolderItem, file.RecordIdentifier);
                }
                else
                {
                    logItem = ProcessSerialNumbersExcelImport(entry, file.FolderItem);
                }
                if (logItem != null)
                {
                    fileLogImportItems.Add(logItem);
                }
            });

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.None,
                   "SerialNumbers", RecordIdentifier.Empty, null);

            ShowImportResultsView(entry, fileLogImportItems, ImportTypeEnum.SerialNumbers);
        }

        public static FileImportLogItem ProcessSerialNumbersExcelImport(IConnectionManager entry, FolderItem fi, bool showResultsView = false)
        {
            ImportTypeEnum importType = ImportTypeEnum.SerialNumbers;

            DataSet ds = null;

            IExcelService service = (IExcelService)PluginEntry.DataModel.Service(ServiceType.ExcelService);

            ds = service.ImportFromExcel(PluginEntry.DataModel, fi, true);

            if (ds != null)
            {
                ImportExcelProgressDialog task = new ImportExcelProgressDialog(ds, importType);

                task.ShowDialog(PluginEntry.Framework.MainWindow);

                if (showResultsView)
                {
                    ShowImportResultsView(entry, task.ImportLogItems, importType);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.None,
                    "SerialNumbers", RecordIdentifier.Empty, null);
                }

                return new FileImportLogItem() { ID = Guid.NewGuid().ToString(), ImportLogItems = task.ImportLogItems, File = fi, ImportProfile = Resources.ExcelFile, ImportType = Resources.ExcelFile };
            }

            return null;
        }

        public static FileImportLogItem ProcessSerialNumbersCsvImport(IConnectionManager entry, FolderItem fi, RecordIdentifier profileID, bool showResultsView = false)
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();
            cols.Add(Field.ItemId.GetAttributeOfType<DescriptionAttribute>().Description, "ITEMID");
            cols.Add(Field.SerialNumber.GetAttributeOfType<DescriptionAttribute>().Description, "SERIALNUMBER");
            cols.Add(Field.TypeOfSerial.GetAttributeOfType<DescriptionAttribute>().Description, "TYPE");

            ImportProfile profile = Providers.ImportProfileData.Get(PluginEntry.DataModel, profileID);
            List<ImportProfileLine> profileLines = Providers.ImportProfileLineData.GetSelectList(PluginEntry.DataModel, profileID);
            DataTable serialNumbersTable = ImportFromCSV(PluginEntry.DataModel, fi, profileLines, profile.HasHeaders, "SerialNumbers");
            foreach (DataColumn dc in serialNumbersTable.Columns)
            {
                if (cols.ContainsKey(dc.ColumnName))
                {
                    dc.ColumnName = cols[dc.ColumnName];
                }
            }
            serialNumbersTable.Columns.Add(new DataColumn("Line Number",typeof(int)));
            foreach (DataRow dr in serialNumbersTable.Rows)
            {
                dr["Line Number"] = serialNumbersTable.Rows.IndexOf(dr) + 1 + (profile.HasHeaders ? 1 : 0);
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(serialNumbersTable);

            ImportTypeEnum importType = ImportTypeEnum.SerialNumbers;
            ImportExcelProgressDialog task = new ImportExcelProgressDialog(ds, importType);

            task.ShowDialog(PluginEntry.Framework.MainWindow);

            if (showResultsView)
            {
                ShowImportResultsView(entry, task.ImportLogItems, importType);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.None,
               "SerialNumbers", RecordIdentifier.Empty, null);
            }

            return new FileImportLogItem() { ID = Guid.NewGuid().ToString(), ImportLogItems = task.ImportLogItems, File = fi, ImportProfile = profile.Description, ImportType = Resources.CSVFile };
        }

        public static FileImportLogItem ProcessCsvImport(IConnectionManager entry, FolderItem fi, RecordIdentifier profileID, InventoryAdjustment stockCountingJournal, bool showResultsView = false)
        {
            ImportProfile profile = Providers.ImportProfileData.Get(PluginEntry.DataModel, profileID);
            List<ImportProfileLine> profileLines = Providers.ImportProfileLineData.GetSelectList(PluginEntry.DataModel, profileID);

            DataTable stockCountingLineTable = ImportFromCSV(PluginEntry.DataModel, fi, profileLines, profile.HasHeaders, "StockCountingLine");
            stockCountingLineTable.Columns.Add(new DataColumn("STOCK COUNTING ID", typeof(string)));
            stockCountingLineTable.Columns.Add(new DataColumn("VARIANT DESCRIPTION", typeof(string)));
            stockCountingLineTable.Columns.Add(new DataColumn("LINE NUMBER", typeof(int)));
            if (!stockCountingLineTable.Columns.Contains("ITEM ID"))
            {
                stockCountingLineTable.Columns.Add(new DataColumn("ITEM ID", typeof(double)));
            }
            if (!stockCountingLineTable.Columns.Contains("UNIT ID"))
            {
                stockCountingLineTable.Columns.Add(new DataColumn("UNIT ID", typeof(string)));
            }
            if (!stockCountingLineTable.Columns.Contains("BARCODE"))
            {
                stockCountingLineTable.Columns.Add(new DataColumn("BARCODE", typeof(string)));
            }

            int rowNumber = 1;
            foreach (DataRow dr in stockCountingLineTable.Rows)
            {
                dr["STOCK COUNTING ID"] = stockCountingJournal.ID;
                dr["VARIANT DESCRIPTION"] = string.Empty;
                dr["LINE NUMBER"] = rowNumber++;
            }

            DataTable stockCountingHeaderTable = new DataTable("StockCountingHeader");
            stockCountingHeaderTable.Columns.Add(new DataColumn("STOCK COUNTING ID", typeof(string)));
            stockCountingHeaderTable.Columns.Add(new DataColumn("STORE ID", typeof(string)));
            stockCountingHeaderTable.Columns.Add(new DataColumn("DESCRIPTION", typeof(string)));
            DataRow stockCountingHeader = stockCountingHeaderTable.NewRow();
            stockCountingHeader["STOCK COUNTING ID"] = stockCountingJournal.ID;
            stockCountingHeader["STORE ID"] = stockCountingJournal.StoreId;
            stockCountingHeader["DESCRIPTION"] = stockCountingJournal.Text;
            stockCountingHeaderTable.Rows.Add(stockCountingHeader);

            DataSet ds = new DataSet();
            ds.Tables.Add(stockCountingHeaderTable);
            ds.Tables.Add(stockCountingLineTable);

            ImportTypeEnum importType = ImportTypeEnum.StockCounting;
            ImportExcelProgressDialog task = new ImportExcelProgressDialog(ds, importType);

            task.ShowDialog(PluginEntry.Framework.MainWindow);

            if (importType == ImportTypeEnum.StockCounting)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.None,
                    "JournalTrans", RecordIdentifier.Empty, null);
            }

            if (showResultsView)
            {
                ShowImportResultsView(entry, task.ImportLogItems, importType);
            }

            return new FileImportLogItem() { ID = Guid.NewGuid().ToString(), ImportLogItems = task.ImportLogItems, File = fi, ImportProfile = profile.Description, ImportType = Resources.CSVFile };
        }

        public static FileImportLogItem ProcessExcelImport(IConnectionManager entry, FolderItem fi, bool showResultsView = false)
        {
            ImportTypeEnum importType;

            bool replenishmentFound = false;
            bool stockCountingFound = false;
            DataSet ds = null;

            ImportExcelProgressDialog task;
            IExcelService service = (IExcelService)PluginEntry.DataModel.Service(ServiceType.ExcelService);

            SpinnerDialog spinnerDialog = new SpinnerDialog(Resources.RetrievingDataFromFile, () => ds = service.ImportFromExcel(PluginEntry.DataModel, fi, true));
            spinnerDialog.ShowDialog();
            
            if (ds != null)
            {
                // Check if we are importing replenishment template, if not then we will assume we are importing normal template.
                foreach (DataTable dt in ds.Tables)
                {
                    if (dt.TableName == "Item replenishment")
                    {
                        replenishmentFound = true;
                        break;
                    }
                    if (dt.TableName == "StockCountingHeader")
                    {
                        stockCountingFound = true;
                        break;
                    }
                }

                if (stockCountingFound)
                {
                    importType = ImportTypeEnum.StockCounting;

                    task = new ImportExcelProgressDialog(ds, importType);
                }
                else if (replenishmentFound)
                {
                    importType = ImportTypeEnum.Replenishment;

                    task = new ImportExcelProgressDialog(ds, importType);
                }
                else
                {
                    importType = ImportTypeEnum.Normal;

                    ImportExcelDocumentDialog optionDlg = new ImportExcelDocumentDialog();
                    if (optionDlg.ShowDialog(PluginEntry.Framework.MainWindow) != DialogResult.OK)
                    {
                        return null;
                    }

                    task = new ImportExcelProgressDialog(optionDlg.Settings, ds, importType);
                }

                task.ShowDialog(PluginEntry.Framework.MainWindow);

                if (importType == ImportTypeEnum.StockCounting)
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.None,
                        "JournalTrans", RecordIdentifier.Empty, null);
                }

                if (showResultsView)
                {
                    ShowImportResultsView(entry, task.ImportLogItems, importType);
                }

                return new FileImportLogItem() { ID = Guid.NewGuid().ToString(), ImportLogItems = task.ImportLogItems, File = fi, ImportProfile = Resources.ExcelFile, ImportType = Resources.ExcelFile };
            }

            return null;
        }

        public static SiteServiceProfile GetSiteServiceProfile()
        {
            if (siteServiceProfile == null)
            {
                Parameters parameters = Providers.ParameterData.Get(PluginEntry.DataModel);
                if (parameters != null)
                {
                    siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, parameters.SiteServiceProfile);
                }
            }

            return siteServiceProfile;
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            //TODO Put back in after first release of 2016
            //args.Add(new Category(Resources.GeneralSetup, "General setup", null), 75);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "General setup")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageBarcodeSetup) &&
                    PluginEntry.DataModel.HasPermission(Permission.ManageRetailGroups) &&
                    PluginEntry.DataModel.HasPermission(Permission.ItemsEdit) &&
                    PluginEntry.DataModel.HasPermission(Permission.CustomerEdit) &&
                    PluginEntry.DataModel.HasPermission(Permission.CurrencyEdit) &&
                    PluginEntry.DataModel.HasPermission(Permission.VendorEdit) &&
                    PluginEntry.DataModel.HasPermission(Permission.ManageRetailDepartments))
                {
                    //TODO Put back in after first release of 2016
                    //args.Add(new Item(Resources.System, "System", null), 2000);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "General setup" && args.ItemKey == "System")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageItemBarcodes) &&
                    PluginEntry.DataModel.HasPermission(Permission.ManageRetailGroups) &&
                    PluginEntry.DataModel.HasPermission(Permission.ItemsEdit) &&
                    PluginEntry.DataModel.HasPermission(Permission.CustomerEdit) &&
                    PluginEntry.DataModel.HasPermission(Permission.CurrencyEdit) &&
                    PluginEntry.DataModel.HasPermission(Permission.VendorEdit) &&
                    PluginEntry.DataModel.HasPermission(Permission.ManageRetailDepartments))
                {
                    //TODO Put back in after first release of 2016
                    /*
                    args.Add(new ItemButton(Resources.ImportFromExcelOrCSV, Resources.ImportFromExcelOrCSVDescription, 
                        ImportFromExcelOrCSV), 1200);

                    args.Add(new ItemButton(Resources.CreateNewExcelTemplate, Resources.CreateNewExcelTemplateDescription, 
                        CreateNewTemplate), 1202);
                        */
                }
            }
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Resources.Tools, "Tools"), 1000);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Tools")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageItemBarcodes) &&
                    PluginEntry.DataModel.HasPermission(Permission.ManageRetailGroups) &&
                    PluginEntry.DataModel.HasPermission(Permission.ItemsEdit) &&
                    PluginEntry.DataModel.HasPermission(Permission.CustomerEdit) &&
                    PluginEntry.DataModel.HasPermission(Permission.CurrencyEdit) &&
                    PluginEntry.DataModel.HasPermission(Permission.VendorEdit) &&
                    PluginEntry.DataModel.HasPermission(Permission.ManageRetailDepartments))
                {
                    args.Add(new PageCategory(Resources.Import, "Import"), 200);
                }
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {

            if (args.PageKey == "Tools" && args.CategoryKey == "Import")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageItemBarcodes) &&
                    PluginEntry.DataModel.HasPermission(Permission.ManageRetailGroups) &&
                    PluginEntry.DataModel.HasPermission(Permission.ItemsEdit) &&
                    PluginEntry.DataModel.HasPermission(Permission.CustomerEdit) &&
                    PluginEntry.DataModel.HasPermission(Permission.CurrencyEdit) &&
                    PluginEntry.DataModel.HasPermission(Permission.VendorEdit) &&
                    PluginEntry.DataModel.HasPermission(Permission.ManageRetailDepartments))
                {
                    args.Add(new CategoryItem(
                            Resources.ImportFromExcel,
                            Resources.ImportFromExcelOrCSV,
                            Resources.ImportFromExcelOrCSVTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            Resources.import_16,
                            null,
                            ImportFromExcelOrCSV,
                            "ImportFromExcel"), 10);


                    args.Add(new CategoryItem(
                            Resources.CreateImportTemplate,
                            Resources.CreateNewExcelTemplate,
                            Resources.CreateNewExcelTemplateTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            Resources.excel_template_16,
                            null,
                            CreateNewTemplate,
                            "NewExcelTemplate"), 20);
                }
            }
        }
    }
}
