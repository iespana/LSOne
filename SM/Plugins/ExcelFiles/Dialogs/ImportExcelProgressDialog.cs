using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.ExcelFiles.Enums;
using LSOne.ViewPlugins.ExcelFiles.Exceptions;
using LSOne.ViewPlugins.ExcelFiles.MappingLogic;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.FileImport;

namespace LSOne.ViewPlugins.ExcelFiles.Dialogs
{
    public partial class ImportExcelProgressDialog : DialogBase
    {
        ImportSettings settings;
        System.Threading.Thread thread;
        Timer timer;
        DataSet ds;
        List<ImportLogItem> importLogItems;
        ImportTypeEnum importType;

        public ImportExcelProgressDialog(DataSet ds, ImportTypeEnum importType)
            : this()
        {
            this.settings = null;
            this.ds = ds;
            this.importType = importType;
        }

        public ImportExcelProgressDialog(ImportSettings settings, DataSet ds, ImportTypeEnum importType)
            : this()
        {
            this.settings = settings;
            this.ds = ds;
            this.importType = importType;
        }

        public ImportExcelProgressDialog()
        {
            settings = null;

            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            progressBar1.Value = 50;

            timer = new Timer();
            timer.Interval = 10;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }


        void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            timer = null;

            thread = new System.Threading.Thread(Import);
            thread.Start();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        void Import()
        {
            try
            {
                if (importType == ImportTypeEnum.Normal)
                {
                    if (settings != null)
                    {
                        importLogItems = ProcessImportFromDataSet(ds, settings);
                    }
                }
                else if (importType == ImportTypeEnum.Replenishment)
                {
                    importLogItems = ProcessImportFromReplenishmentDataSet(ds);
                }
                else if (importType == ImportTypeEnum.StockCounting)
                {
                    importLogItems = ProcessImportFromStockCountingDataSet(ds);
                }
                else if (importType == ImportTypeEnum.SerialNumbers)
                {
                    importLogItems = ProcessImportFromSerialNumbersDataSet(ds);
                }
            }
            catch (Exception)
            {
                // We do nothing here this task must end gracefully not matter what so that the thread ends correctly.
            }

            EventHandler del = new EventHandler(ClosingFromThread);
            object[] parameters = { null, EventArgs.Empty };

            this.Invoke(del, parameters);


        }

        internal List<ImportLogItem> ImportLogItems
        {
            get
            {
                return importLogItems;
            }
        }

        void ClosingFromThread(object sender, EventArgs args)
        {
            Close();
        }

        private static List<ImportLogItem> ProcessImportFromStockCountingDataSet(DataSet ds)
        {
            DataTable stockCountingHeaderTable = null;
            DataTable stockCountingLineTable = null;
            List<ImportLogItem> importLogItems = new List<ImportLogItem>();

            foreach (DataTable dt in ds.Tables)
            {
                if (dt.TableName == "StockCountingHeader")
                {
                    stockCountingHeaderTable = dt;
                }
                if (dt.TableName == "StockCountingLine")
                {
                    stockCountingLineTable = dt;
                }
            }

            if (stockCountingHeaderTable == null || stockCountingLineTable == null)
            {
                return importLogItems;
            }

            try
            {
                StockCountingMapper.ImportHeader(stockCountingHeaderTable, importLogItems);
                try
                {
                    StockCountingMapper.ImportLine(stockCountingLineTable, importLogItems, stockCountingHeaderTable.Rows[0].GetStringValue("Store Id"));
                }
                catch (ColumnMissingException ex)
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, stockCountingLineTable.TableName, "", Properties.Resources.CouldNotImportStockCountingLinesSinceMandatory.Replace("#1", ex.ColumnName)));
                }
            }
            catch (ColumnMissingException ex)
            {
                importLogItems.Add(new ImportLogItem(ImportAction.Skipped, stockCountingHeaderTable.TableName, "", Properties.Resources.CouldNotImportStockCountingHeaderSinceMandatory.Replace("#1", ex.ColumnName)));
            }

            return importLogItems;
        }

        private static List<ImportLogItem> ProcessImportFromReplenishmentDataSet(DataSet ds)
        {
            // Used from thread DO NOT talk to any UI from here !!!!

            DataTable itemReplenishmentDataTable = null;
            List<ImportLogItem> importLogItems = new List<ImportLogItem>();

            foreach (DataTable dt in ds.Tables)
            {
                if (dt.TableName == "Item replenishment")
                {
                    itemReplenishmentDataTable = dt;
                    break;
                }
            }

            if (itemReplenishmentDataTable == null)
            {
                return importLogItems;
            }

            try
            {
                ItemReplenishmentMapper.Import(itemReplenishmentDataTable, importLogItems);
            }
            catch (ColumnMissingException ex)
            {
                importLogItems.Add(new ImportLogItem(ImportAction.Skipped, itemReplenishmentDataTable.TableName, "", Properties.Resources.CouldNotImportReplenishmentDataSinceMandatory.Replace("#1", ex.ColumnName)));
            }

            return importLogItems;
        }

        private static List<ImportLogItem> ProcessImportFromDataSet(DataSet ds, ImportSettings settings)
        {
            // Used from thread DO NOT talk to any UI from here !!!!

            List<ImportLogItem> importLogItems = new List<ImportLogItem>();

            Dictionary<string, DataTable> foundSheets = new Dictionary<string, DataTable>();

            foreach (DataTable dt in ds.Tables)
            {
                foundSheets.Add(dt.TableName, dt);
            }

            // Process the data in most logical order


            if (foundSheets.ContainsKey("Retail department"))
            {
                try
                {
                    if (settings.RetailDepartmentImportStrategy != MergeModeEnum.Ignore)
                    {
                        RetailDepartmentMapper.Import(foundSheets["Retail department"], importLogItems, settings.RetailDepartmentImportStrategy);
                    }
                }
                catch (ColumnMissingException ex)
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, foundSheets["Retail department"].TableName, "", Properties.Resources.CouldNotImportDepartmentsSinceMandatory.Replace("#1", ex.ColumnName)));
                }

            }

            if (foundSheets.ContainsKey("Retail groups"))
            {
                try
                {
                    if (settings.RetailGroupImportStrategy != MergeModeEnum.Ignore)
                    {
                        RetailGroupMapper.Import(foundSheets["Retail groups"], importLogItems, settings.RetailGroupImportStrategy);
                    }
                }
                catch (ColumnMissingException ex)
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, foundSheets["Retail groups"].TableName, "", Properties.Resources.CouldNotImportRetailGroupsSinceMandatory.Replace("#1", ex.ColumnName)));
                }
            }

            if (foundSheets.ContainsKey("Customers"))
            {
                try
                {
                    if (settings.CustomerImportStrategy != MergeModeEnum.Ignore)
                    {
                        CustomerMapper.Import(foundSheets["Customers"], importLogItems, settings.CustomerImportStrategy);
                    }
                }
                catch (ColumnMissingException ex)
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, foundSheets["Customers"].TableName, "", Properties.Resources.CouldNotImportCustomersSinceMandatory.Replace("#1", ex.ColumnName)));
                }
            }

            if (foundSheets.ContainsKey("Vendors"))
            {
                try
                {
                    if (settings.VendorImportStrategy != MergeModeEnum.Ignore)
                    {
                        VendorMapper.Import(foundSheets["Vendors"], importLogItems, settings.VendorImportStrategy);
                    }
                }
                catch (ColumnMissingException ex)
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, foundSheets["Vendors"].TableName, "", Properties.Resources.CouldNotImportVendorsSinceMandatory.Replace("#1", ex.ColumnName)));
                }
            }

            if (foundSheets.ContainsKey("Retail items"))
            {
                try
                {
                    if (settings.RetailItemImportStrategy != MergeModeEnum.Ignore)
                    {
                        RetailItemMapper.Import(foundSheets["Retail items"], importLogItems, settings.RetailItemImportStrategy, settings.CalculateProfitMargins, settings.DimensionsAttributesSeparator);
                    }
                }
                catch (ColumnMissingException ex)
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, foundSheets["Retail items"].TableName, "", Properties.Resources.CouldNotImportRetailItemsSinceMandatory.Replace("#1", ex.ColumnName)));
                }
            }



            return importLogItems;
        }

        private static List<ImportLogItem> ProcessImportFromSerialNumbersDataSet(DataSet ds)
        {
            // Used from thread DO NOT talk to any UI from here !!!!

            List<ImportLogItem> importLogItems = new List<ImportLogItem>();

            if (ds.Tables.Count == 0)
            {
                return new List<ImportLogItem>();
            }

            try
            {
                ds.Tables[0].TableName = "SerialNumbers";
                SerialNumberMapper.Import(ds.Tables[0], importLogItems);
            }
            catch (ColumnMissingException ex)
            {
                importLogItems.Add(new ImportLogItem(ImportAction.Skipped, ds.Tables[0].TableName, "", Properties.Resources.CouldNotImportSerialNumberDataSinceMandatory.Replace("#1", ex.ColumnName)));
            }

            return importLogItems;
        }
    }
}
