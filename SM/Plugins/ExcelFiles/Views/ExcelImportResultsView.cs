using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.ExcelFiles.Enums;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.FileImport;

namespace LSOne.ViewPlugins.ExcelFiles.Views
{
    public partial class ExcelImportResultsView : ViewBase
    {
        List<ImportLogItem> items;
        List<FileImportLogItem> fileItems;
        ImportTypeEnum importType;

        internal ExcelImportResultsView(List<ImportLogItem> items, ImportTypeEnum importType)
            : this()
        {
            this.items = items;
            this.importType = importType;
            lvFiles.Visible = false;
            lvResults.Location = new System.Drawing.Point(9, 10);
            lvResults.Size = new System.Drawing.Size(lvResults.Size.Width, lvResults.Size.Height + 120);
            DisplayResults();
        }

        internal ExcelImportResultsView(List<FileImportLogItem> fileItems, ImportTypeEnum importType)
           : this()
        {
            this.fileItems = fileItems;
            this.importType = importType;
            this.items = fileItems[0].ImportLogItems;
            DisplayFiles();
        }


        public ExcelImportResultsView()
        {
            importType = ImportTypeEnum.Normal;

            InitializeComponent();

            Attributes =
                ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.ContextBar;

            imageList1.Images.Add(PluginEntry.Framework.GetImage(ViewCore.Interfaces.ImageEnum.EmbeddedInformation));
            imageList1.Images.Add(PluginEntry.Framework.GetImage(ViewCore.Interfaces.ImageEnum.EmbeddedSuccess));
            imageList1.Images.Add(PluginEntry.Framework.GetImage(ViewCore.Interfaces.ImageEnum.EmbeddedError));

            //HeaderIcon = Properties.Resources.ExcelImport16;
            HeaderText = Properties.Resources.ImportResults;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.ImportResults;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return Guid.NewGuid();
            }
        }

        protected override void LoadData(bool isRevert)
        {

        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        private void lvResults_Resize(object sender, EventArgs e)
        {
            lvResults.Columns[0].Width = lvResults.Width - 20;
        }

        private void singleListView1_Load(object sender, EventArgs e)
        {

        }

        private void DisplayFiles()
        {
            foreach (FileImportLogItem file in fileItems)
            {
                Row row = new Row();
                row.Tag = file.ID;
                row.AddText(file.File.Name);
                row.AddText(file.ImportType);
                row.AddText(file.ImportProfile);
                lvFiles.AddRow(row);
            }
            if (fileItems.Count > 0)
            {
                lvFiles.Selection.Set(0);
            }
        }

        private void DisplayResults()
        {
            lvResults.Groups.Clear();
            lvErrorLines.ClearRows();

            ListViewItem lvItem;

            //Dictionary keys should be the same as sheet names on Excel file being imported
            Dictionary<string, ListViewGroup> lvGroups = new Dictionary<string, ListViewGroup>();

            if (importType == ImportTypeEnum.Normal)
            {
                lvGroups.Add("Retail items", new ListViewGroup(Properties.Resources.RetailItems));
                lvGroups.Add("Retail groups", new ListViewGroup(Properties.Resources.RetailGroups));
                lvGroups.Add("Customers", new ListViewGroup(Properties.Resources.Customers));
                lvGroups.Add("Vendors", new ListViewGroup(Properties.Resources.Vendors));
                lvGroups.Add("Retail department", new ListViewGroup(Properties.Resources.RetailDepartment));
            }
            else if (importType == ImportTypeEnum.Replenishment)
            {
                lvGroups.Add("Item replenishment", new ListViewGroup(Properties.Resources.ItemReplenishment));
            }
            else if (importType == ImportTypeEnum.StockCounting)
            {
                lvGroups.Add("StockCountingHeader", new ListViewGroup(Properties.Resources.StockCountingHeader));
                lvGroups.Add("StockCountingLine", new ListViewGroup(Properties.Resources.StockCountingLine));
            }
            else if (importType == ImportTypeEnum.SerialNumbers)
            {
                lvGroups.Add("SerialNumbers", new ListViewGroup(Properties.Resources.SerialNumbersListViewHeader));
            }

            foreach (var group in lvGroups.Values)
            {
                lvResults.Groups.Add(group);
            }

            foreach (var keyValue in lvGroups)
            {
                var insertCount = items.Where(i => i.SheetName == keyValue.Key && i.Action == ImportAction.Inserted).Sum(x => x.Count);
                string inserted = insertCount > 0 ? Properties.Resources.SuccessfullyInserted.Replace("#1", insertCount.ToString()) : Properties.Resources.NoRecordsInserted;
                lvItem = new ListViewItem(inserted);
                lvItem.ImageIndex = insertCount > 0 ? 1 : 0;
                lvItem.Group = keyValue.Value;
                lvResults.Add(lvItem);

                var updateCount = items.Where(i => i.SheetName == keyValue.Key && i.Action == ImportAction.Updated).Sum(x => x.Count);
                string updated = updateCount > 0 ? Properties.Resources.SuccessfullyUpdated.Replace("#1", updateCount.ToString()) : Properties.Resources.NoRecordsUpdated;
                lvItem = new ListViewItem(updated);
                lvItem.ImageIndex = updateCount > 0 ? 1 : 0;
                lvItem.Group = keyValue.Value;
                lvResults.Add(lvItem);

                var skippedCount = items.Where(i => i.SheetName == keyValue.Key && i.Action == ImportAction.Skipped).Sum(x => x.Count);
                string skipped = skippedCount > 0 ? Properties.Resources.RecordsSkippedNotificationMessage.Replace("#1", skippedCount.ToString()) : Properties.Resources.NoRecordsSkipped;
                lvItem = new ListViewItem(skipped);
                lvItem.ImageIndex = skippedCount > 0 ? 2 : 0;
                lvItem.Group = keyValue.Value;
                lvResults.Add(lvItem);
            }


            List<ImportLogItem> skippedItems = items.OrderBy(i => i.LineNumber).Take(100).Where(i => i.Action == ImportAction.Skipped).ToList();
            foreach (ImportLogItem item in skippedItems)
            {
                Row row = new Row();
                row.Tag = item.ID;
                row.AddText(item.LineNumber.HasValue ? item.LineNumber.ToString() : string.Empty);
                row.AddText(item.ID.ToString());
                row.AddText(item.ItemDescription);
                row.AddText(item.Reason);
                lvErrorLines.AddRow(row);
            }
            if (skippedItems.Count == 0)
            {
                btnExportToExcel.Enabled = false;
            }
            else
            {
                btnExportToExcel.Enabled = true;
            }
        }

        private void lvFiles_SelectionChanged(object sender, EventArgs e)
        {
            int firstSelectedRowIndex = lvFiles.Selection.FirstSelectedRow;
            Row selectedRow = lvFiles.Rows[firstSelectedRowIndex];
            FileImportLogItem fileItem = fileItems.Where(f => f.ID == (string)selectedRow.Tag).FirstOrDefault();
            items = fileItem.ImportLogItems;
            DisplayResults();
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            PluginOperations.ExportLogItems(items.Where(el => el.Action == ImportAction.Skipped).ToList());
        }
    }
}
