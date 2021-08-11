using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.Utilities.IO;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.ViewPlugins.Inventory.Properties;
using LSOne.Controls;
using LSOne.ViewCore;
using LSOne.Services.Interfaces;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    public partial class ImportSCJFromFile : UserControl, IWizardPage
    {
        private readonly ImportProfile defaultImportProfile;

        private List<string> fileTypeFilters;

        WizardBase parent;

        private InventoryTypeAction inventoryTypeAction;

        public List<ImportFileListItem> ImportFilesListContent
        {
            get;
            private set;
        }

        public InventoryAdjustment SelectedStockCountingJournal
        {
            get
            {
                return (InventoryAdjustment)cmbStockCountingJournal.SelectedData;
            }
        }

        public List<ImportDescriptor> ImportDescriptors { get; private set; }

        public Setting WorkingFolder { get; private set; }

        public ImportSCJFromFile(WizardBase parent, InventoryTypeAction inventoryTypeAction)
        {
            InitializeComponent();

            WorkingFolder = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, PluginEntry.ExcelFolderLocationSettingID, SettingType.Generic, null);
            if (WorkingFolder != null && !string.IsNullOrEmpty(WorkingFolder.Value))
            {
                WorkingFolder.Value = Path.Combine(WorkingFolder.Value, "Excel Import");
            }
            this.parent = parent;
            this.inventoryTypeAction = inventoryTypeAction;

            defaultImportProfile = Providers.ImportProfileData.GetDefaultImportProfile(PluginEntry.DataModel, ImportType.StockCounting);

            lvImportFiles.ContextMenuStrip = new ContextMenuStrip();
            lvImportFiles.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            ImportFilesListContent = new List<ImportFileListItem>();
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ContextMenuStrip menu = lvImportFiles.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Resources.Edit,
                    100,
                    new EventHandler(btnsContextButtons_EditButtonClicked))
            {
                //Image = Properties.Resources.EditImage,
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsContextButtons.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Resources.Add,
                   200,
                   new EventHandler(btnsContextButtons_AddButtonClicked));

            item.Enabled = btnsContextButtons.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Delete,
                    300,
                    new EventHandler(btnsContextButtons_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;

            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void LoadDefaultFiles()
        {
            ImportDescriptors = new List<ImportDescriptor>();

            if (PluginEntry.Framework.CanRunOperation("ImportStockCounting"))
            {
                PluginOperationArguments args = new PluginOperationArguments("", ImportDescriptors);
                PluginEntry.Framework.RunOperation("ImportStockCounting", this, args);
            }

            fileTypeFilters = new List<string>();

            foreach (ImportDescriptor importDescriptor in ImportDescriptors)
            {
                fileTypeFilters.Add(importDescriptor.Mask);
            }

            List<string> fileNames = PluginOperations.GetFilesFromDefaultFolder(WorkingFolder, fileTypeFilters);
            AddFilesToView(fileNames);
        }

        private bool SkipCSVFile(ImportDescriptor importableFile)
        {
            return importableFile.Label == Resources.CSVFile && defaultImportProfile.MasterID == RecordIdentifier.Empty;
        }

        private bool DuplicateFiles(List<string> files)
        {
            List<string> fileNames = files.Select(f => Path.GetFileName(f)).ToList();
            foreach (ImportFileListItem fileItem in ImportFilesListContent)
            {
                string file = Path.GetFileName(fileItem.FullFileName);
                if (fileNames.Contains(file))
                {
                    return true;
                }
            }
            return false;
        }

        private void AddFilesToView(List<string> fileNames)
        {
            if (DuplicateFiles(fileNames))
            {
                if (QuestionDialog.Show(Resources.FileAlreadyAddedToImport + " " + Resources.OverwritePreviouslyAddedFile, Resources.ImportDuplicateFileHeader) == DialogResult.Yes)
                {
                    List<string> shortFileNames = fileNames.Select(f => Path.GetFileName(f)).ToList();
                    ImportFilesListContent.RemoveAll(a => shortFileNames.Contains(Path.GetFileName(a.FullFileName)));
                }
                else
                {
                    return;
                }
            }

            bool showMissingCsvProfileMessage = false;
            bool showInvalidCsvProfileMessage = false;
            FolderItem file;
            foreach (string fileName in fileNames)
            {
                ImportDescriptor importableFile = isImportable(fileName);
                file = new FolderItem(fileName);
                bool skipCSVFile = SkipCSVFile(importableFile);
                if (skipCSVFile)
                {
                    showMissingCsvProfileMessage = true;
                }
                else if (importableFile.Label == Resources.CSVFile && !PluginOperations.IsDefaultProfileValid(defaultImportProfile.MasterID))
                {
                    skipCSVFile = true;
                    showInvalidCsvProfileMessage = true;
                }

                if (importableFile != null && !skipCSVFile)
                {
                    ImportFileListItem importFile = new ImportFileListItem();
                    importFile.DisplayedFileName = file.Name;
                    importFile.FullFileName = fileName;
                    importFile.ImportTypeText = importableFile.Label;
                    if (importableFile.Mask == ".xlsx" || importableFile.Mask == ".xls")
                    {
                        importFile.ProfileId = RecordIdentifier.Empty;
                        importFile.ProfileText = Resources.ExcelFile;
                    }
                    else
                    {
                        importFile.ProfileId = defaultImportProfile.MasterID;
                        importFile.ProfileText = defaultImportProfile.Description;
                    }
                    ImportFilesListContent.Add(importFile);
                }

            }
            if (showMissingCsvProfileMessage)
            {
                MessageDialog.Show(Resources.MissingCSVImportProfile);
            }
            else if (showInvalidCsvProfileMessage)
            {
                MessageDialog.Show(Resources.CSVHasNoMapping + " " + Resources.AddMappingToProfile);
            }

            RefreshListContent();
        }

        #region IWizardPage Members

        public void Display()
        {
            LoadDefaultFiles();
        }

        public IWizardPage RequestNextPage()
        {
            throw new NotImplementedException();
        }

        public bool NextButtonClick(ref bool canUseFromForwardStack)
        {
            canUseFromForwardStack = false;
            return true;
        }

        public bool HasFinish
        {
            get { return true; }
        }

        public bool HasForward
        {
            get { return false; }
        }

        public Control PanelControl
        {
            get { return this; }
        }

        public void ResetControls()
        {

        }

        #endregion

        private ImportDescriptor isImportable(string fileName)
        {
            return ImportDescriptors?.Find(x => fileName.EndsWith(x.Mask));
        }

        private void btnsContextButtons_AddButtonClicked(object sender, EventArgs e)
        {
            List<string> fileNames = SelectFilesExcelOrCSV();

            AddFilesToView(fileNames);
        }

        public List<string> SelectFilesExcelOrCSV()
        {
            List<string> fileNames = new List<string>();

            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = Resources.ExcelFile + " (*.xls;*.xlsx)|*.xls;*.xlsx|" + Resources.CSVFile + " (*.csv)|*.csv";

            dlg.Multiselect = true;

            if (WorkingFolder != null && !string.IsNullOrEmpty(WorkingFolder.Value) && Directory.Exists(WorkingFolder.Value))
            {
                dlg.InitialDirectory = WorkingFolder.Value;
            }

            if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                fileNames = dlg.FileNames.ToList();
            }

            return fileNames;
        }

        private void lvImportFiles_SelectionChanged(object sender, EventArgs e)
        {
            btnsContextButtons.RemoveButtonEnabled = lvImportFiles.Selection.Count > 0;

            btnsContextButtons.EditButtonEnabled = EditButtonEnabled();

            FinishButtonEnabledState();
        }

        private bool EditButtonEnabled()
        {
            bool editButtonEnabled = (lvImportFiles.Selection.Count > 0);
            for (int selectionIndex = 0; selectionIndex < lvImportFiles.Selection.Count; selectionIndex++)
            {
                if (((ImportFileListItem)lvImportFiles.Selection[selectionIndex].Tag).IsCSVFile == false)
                {
                    editButtonEnabled = false;
                    break;
                }
            }
            return editButtonEnabled;
        }

        private void btnsContextButtons_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(Resources.DeleteFileImportQuestion, Resources.DeleteFileImport) == DialogResult.Yes)
            {
                for (int selectionIndex = 0; selectionIndex < lvImportFiles.Selection.Count; selectionIndex++)
                {
                    string fileToRemove = ((ImportFileListItem)lvImportFiles.Selection[selectionIndex].Tag).FullFileName;
                    ImportFilesListContent.RemoveAll(a => a.FullFileName == fileToRemove);
                }

                RefreshListContent();
            }
        }

        private void btnsContextButtons_EditButtonClicked(object sender, EventArgs e)
        {
            string profileSelection = ((ImportFileListItem)lvImportFiles.Selection[0].Tag).ProfileText;
            EditFileImportProfileDialog editFileImportProfileDialog = new EditFileImportProfileDialog(profileSelection);
            if (editFileImportProfileDialog.ShowDialog() == DialogResult.OK)
            {
                for (int selectionIndex = 0; selectionIndex < lvImportFiles.Selection.Count; selectionIndex++)
                {
                    ImportFileListItem oldItem = (ImportFileListItem)lvImportFiles.Selection[selectionIndex].Tag;
                    var newItem = ImportFilesListContent.Where(a => a.FullFileName == oldItem.FullFileName).FirstOrDefault();
                    if (newItem != null)
                    {
                        newItem.ProfileId = editFileImportProfileDialog.SelectedProfileId;
                        newItem.ProfileText = editFileImportProfileDialog.SelectedProfileText;
                    }
                }

                RefreshListContent();
            }
        }

        private void RefreshListContent()
        {
            lvImportFiles.ClearRows();

            foreach (ImportFileListItem importFileItem in ImportFilesListContent)
            {
                Row row = new Row();
                row.Tag = importFileItem;
                row.AddText(importFileItem.DisplayedFileName);
                row.AddText(importFileItem.ImportTypeText);
                row.AddText(importFileItem.ProfileText);
                lvImportFiles.AddRow(row);
            }

            lvImportFiles.Refresh();
            FinishButtonEnabledState();
        }

        private bool ExistsCsvFile()
        {
            bool exists = false;
            foreach (Row row in lvImportFiles.Rows)
            {
                if (Path.GetExtension(((ImportFileListItem)row.Tag).FullFileName).Equals(".csv"))
                {
                    exists = true;
                }
            }

            return exists;
        }

        private void FinishButtonEnabledState()
        {
            parent.NextEnabled = (lvImportFiles.RowCount > 0) &&
                (cmbStockCountingJournal.SelectedData is InventoryAdjustment || !ExistsCsvFile());

        }

        private void btnAddStockCountingJournal_Click(object sender, EventArgs e)
        {
            var dlg = new Dialogs.StockCountingDialog();

            if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                InventoryAdjustment addedStockCounting = service.GetInventoryAdjustment(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), dlg.JournalID, true);
                cmbStockCountingJournal.SelectedData = addedStockCounting;
                FinishButtonEnabledState();
            }
        }

        private void cmbStockCountingJournal_DropDown(object sender, Controls.DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;
            if (!(cmbStockCountingJournal.SelectedData is InventoryAdjustment))
            {
                initialSearchText = string.Empty;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)cmbStockCountingJournal.SelectedData).Text;
                textInitallyHighlighted = true;
            }

            e.ControlToEmbed = new SingleSearchPanel(
            PluginEntry.DataModel,
            false,
            initialSearchText,
            SearchTypeEnum.StockCountingJournal,
            textInitallyHighlighted);
        }

        private void cmbStockCountingJournal_SelectedDataChanged(object sender, EventArgs e)
        {
            FinishButtonEnabledState();
        }

        private void cmbStockCountingJournal_RequestClear(object sender, EventArgs e)
        {
            cmbStockCountingJournal.SelectedData = null;
            FinishButtonEnabledState();
        }

        private void lvImportFiles_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (EditButtonEnabled())
            {
                btnsContextButtons_EditButtonClicked(this, EventArgs.Empty);
            }
        }
    }

    public class ImportFileListItem
    {
        public RecordIdentifier ProfileId { get; set; }

        public string ImportTypeText { get; set; }

        public string ProfileText { get; set; }

        public string DisplayedFileName { get; set; }

        public string FullFileName { get; set; }

        public bool IsCSVFile
        {
            get
            {
                return FullFileName.ToLower().EndsWith(".csv");
            }
        }

        public ImportFileListItem() { }

        public ImportFileListItem(RecordIdentifier profileId, string fileName)
        {
            ProfileId = profileId;
            FullFileName = fileName;
        }
    }

}