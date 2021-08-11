using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Auditing;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportClasses.ImportExport;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.IO;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Terminals.Properties;

namespace LSOne.ViewPlugins.Terminals.Views
{
    public partial class OperationsAuditView : ViewBase
    {
        private Setting searchBarSetting;
        private DataEntity cmbStoreSelectedItem;

        private const string searchBarSettingGuid = "42D23D29-F81D-48EE-887D-E2057CBB4838";

        private DataEntitySelectionList operationsSelection;

        private List<OperationAuditing> current;

        public OperationsAuditView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Close |
                         ViewAttributes.ContextBar |
                         ViewAttributes.Help;

            HeaderText = Resources.OperationsAuditLog;
            searchBar.BuddyControl = lvOperations;
            operationsDataScroll.PageSize = PluginEntry.DataModel.PageSize;
            current = new List<OperationAuditing>();

            lvOperations.ContextMenuStrip = new ContextMenuStrip();
            lvOperations.ContextMenuStrip.Opening += lvOperations_Opening;
        }

        private void lvOperations_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvOperations.ContextMenuStrip;

            menu.Items.Clear();

            item = new ExtendedMenuItem(
                    Properties.Resources.GenerateExcelReport,
                    100,
                    btnGenerateExcelFile_Click);
            
            item.Enabled = btnGenerateExcelFile.Enabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("OperationsAuditView", lvOperations.ContextMenuStrip, lvOperations);
            e.Cancel = menu.Items.Count == 0;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.OperationsAuditLog;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            operationsDataScroll.Reset();
            LoadItems();
        }

        private void LoadItems()
        {
            RecordIdentifier store = null;
            RecordIdentifier terminal = null;
            RecordIdentifier operatorID = null;
            DateTime from = default(DateTime);
            DateTime to = default(DateTime);
            List<RecordIdentifier> selectedOperations = null;

            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Store":
                        store = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;

                    case "Terminal":
                        terminal = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;

                    case "Date":
                        if (result.Date.Checked)
                        {
                            from = result.Date.Value.Date;
                        }
                        if (result.DateTo.Checked)
                        {
                            to = result.DateTo.Value.Date.AddDays(1).AddSeconds(-1);
                        }
                        break;

                    case "Operator":
                        operatorID = ((DualDataComboBox) result.UnknownControl).SelectedData.ID;
                        break;

                    case "Operations":
                        List<DataEntity> selectedItems = ((DataEntitySelectionList)((DualDataComboBox)result.UnknownControl).SelectedData).GetSelectedItems();
                        selectedOperations = new List<RecordIdentifier>();
                        foreach (DataEntity selectedItem in selectedItems)
                        {
                            selectedOperations.Add(selectedItem.ID);   
                        }
                        break;
                }
            }

            var items = TransactionProviders.OperationAuditingData.Search(PluginEntry.DataModel, 
                    store, 
                    terminal, 
                    operatorID, 
                    from, 
                    to,
                    selectedOperations,
                    operationsDataScroll.StartRecord, 
                    operationsDataScroll.EndRecord +1);
            lvOperations.ClearRows();
            PopulateListView(items);
            HideProgress();
        }

        private void PopulateListView(List<OperationAuditing> items)
        {
            operationsDataScroll.RefreshState(items);
            current = items;
            Row row = null;
            foreach (OperationAuditing operationAuditing in items)
            {
                row = new Row();
                row.AddText(operationAuditing.CreatedDate.ToString("G"));
                row.AddText(operationAuditing.OperationName);
                row.AddText((string)operationAuditing.UserID);
                row.AddText((string)operationAuditing.ManagerID);
                row.AddText((string)operationAuditing.StoreID);
                row.AddText((string)operationAuditing.TerminalID);
                lvOperations.AddRow(row);
            }
            lvOperations.AutoSizeColumns();
        }

        private void searchBar_LoadDefault(object sender, System.EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, new Guid(searchBarSettingGuid), SettingType.UISetting, "");

            if (searchBarSetting.LongUserSetting != "")
            {
                searchBar.LoadFromSetupString(searchBarSetting.Value);
            }
        }

        private void searchBar_SaveAsDefault(object sender, System.EventArgs e)
        {
            string layoutString = searchBar.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, new Guid(searchBarSettingGuid), SettingsLevel.User, searchBarSetting);
            }
            ShowTimedProgress(searchBar.GetLocalizedSavingText());
        }

        private void searchBar_SearchClicked(object sender, System.EventArgs e)
        {
            operationsDataScroll.Reset();
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void searchBar_SetupConditions(object sender, System.EventArgs e)
        {
            searchBar.AddCondition(new ConditionType(Resources.Date, "Date", ConditionType.ConditionTypeEnum.DateRange, false, DateTime.Now.AddMonths(-1), false, DateTime.Now));
            searchBar.AddCondition(new ConditionType(Resources.StoreText, "Store", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.TerminalText, "Terminal", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.Operator, "Operator", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.Operations, "Operations", ConditionType.ConditionTypeEnum.Unknown));

            searchBar_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox) args.UnknownControl).SelectedData = PluginEntry.DataModel.IsHeadOffice ? new DataEntity("", "") : cmbStoreSelectedItem ?? new DataEntity("", "");
                    args.UnknownControl.Enabled = PluginEntry.DataModel.IsHeadOffice || cmbStoreSelectedItem == null;

                    ((DualDataComboBox) args.UnknownControl).ShowDropDownOnTyping = true;

                    ((DualDataComboBox) args.UnknownControl).RequestData += cmbStoreRequestData;
                    ((DualDataComboBox) args.UnknownControl).SelectedDataChanged += cmbStoreSelectionChanged;
                    break;
                case "Terminal":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;

                    ((DualDataComboBox) args.UnknownControl).ShowDropDownOnTyping = true;

                    ((DualDataComboBox) args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox) args.UnknownControl).RequestData += cmbTerminalRequestData;
                    break;
                case "Operator":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;

                    ((DualDataComboBox) args.UnknownControl).ShowDropDownOnTyping = true;

                    ((DualDataComboBox) args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).DropDown += cmbOperator__DropDown;
                    break;

                case "Operations":

                    var operations = Providers.PosOperationData.GetAuditableList(PluginEntry.DataModel);
                    operationsSelection = new DataEntitySelectionList(operations);

                    operationsSelection.SelectAll();

                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;

                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = operationsSelection;

                    ((DualDataComboBox)args.UnknownControl).DropDown += Operations_DropDown;
                    break;

            }
        }

        private void cmbOperator__DropDown(object sender, DropDownEventArgs e)
        {
            e.ControlToEmbed = new EmployeeSearchPanel(PluginEntry.DataModel, e.DisplayText);
        }

        private void searchBar_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                case "Terminal":
                case "Operations":
                    args.Selection = (string) ((DualDataComboBox) args.UnknownControl).SelectedData.ID;
                    break;
                case "Operator":
                    args.Selection = ((DualDataComboBox) args.UnknownControl).SelectedData.Text;
                    break;
            }
        }

        private void searchBar_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            args.HasSelection = true;
        }

        private void searchBar_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                    ((DualDataComboBox) args.UnknownControl).RequestData -= cmbStoreRequestData;
                    ((DualDataComboBox) args.UnknownControl).SelectedDataChanged -= cmbStoreSelectionChanged;
                    break;
                case "Terminal":
                    ((DualDataComboBox) args.UnknownControl).RequestData -= cmbTerminalRequestData;
                    break;
                case "Operator":
                    ((DualDataComboBox) args.UnknownControl).RequestData -= cmbOperatorsRequestData;
                    break;
                case "RetailGroup":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= Operations_DropDown;
                    break;

            }
        }

        private void searchBar_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;
            switch (args.TypeKey)
            {
                case "Store":
                    entity = Providers.StoreData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "Terminal":
                    RecordIdentifier storeID = cmbStoreSelectedItem != null ? cmbStoreSelectedItem.ID : "";
                    entity = Providers.TerminalData.Get(PluginEntry.DataModel, args.Selection, storeID);
                    break;
                case "Operator":
                    entity = Providers.UserData.Get(PluginEntry.DataModel, new Guid(args.Selection));//todo: fix for operator name
                    break;
                case "Operations":
                    entity = Providers.PosOperationData.Get(PluginEntry.DataModel, args.Selection);
                    break;
            }
            ((DualDataComboBox) args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void cmbTerminalRequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox) sender).SkipIDColumn = true;

            ((DualDataComboBox) sender).SetData(cmbStoreSelectedItem != null ? Providers.TerminalData.GetTerminals(PluginEntry.DataModel, cmbStoreSelectedItem.ID) :
                Providers.TerminalData.GetAllTerminals(PluginEntry.DataModel, true, TerminalListItem.SortEnum.STORENAME), null);
        }

        private void cmbStoreRequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox) sender).SkipIDColumn = true;

            ((DualDataComboBox) sender).SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbOperatorsRequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox) sender).SkipIDColumn = true;

            ((DualDataComboBox) sender).SetData(Providers.UserData.AllUsers(PluginEntry.DataModel), null);
        }

        private void cmbStoreSelectionChanged(object sender, EventArgs e)
        {
            cmbStoreSelectedItem = (DataEntity) ((DualDataComboBox) sender).SelectedData;
        }

        private void operationsDataScroll_PageChanged(object sender, EventArgs e)
        {
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void Operations_DropDown(object sender, DropDownEventArgs e)
        {
            if (((DualDataComboBox)sender).SelectedData != null)
            {
                e.ControlToEmbed = new CheckBoxSelectionListPanel((DataEntitySelectionList)((DualDataComboBox)sender).SelectedData);
            }
        }

        private void btnGenerateExcelFile_Click(object sender, EventArgs e)
        {
            ShowProgress((sender1, e1) => CreateExcelReportForTerminalOperationAudit(current), Properties.Resources.CreatingExcel);
        }

        private void CreateExcelReportForTerminalOperationAudit(List<OperationAuditing> operations)
        {
            btnGenerateExcelFile.Enabled = false;
            try
            {
                IExcelService excelService = Services.Interfaces.Services.ExcelService(PluginEntry.DataModel);
                Guid fileName = Guid.NewGuid();
                FolderItem excelFile = FolderItem.GetTempFile(fileName.ToString(), ".xlsx");
                WorkbookHandle workBook = excelService.CreateWorkbook(excelFile, Resources.Audit);
                WorksheetHandle sheet = excelService.GetWorksheet(workBook, Resources.Audit);
                WorksheeetOptions options = new WorksheeetOptions() {ColumnWidths = new Dictionary<int, int>()};
                for (int i = 0; i < 7; i++)
                {
                    options.ColumnWidths[i] = WorksheeetOptions.AutoFit;
                }

                //Creating header
                excelService.SetCell(sheet,
                    new WorksheetCell
                    {
                        Column = 0,
                        Row = 0,
                        Options = new CellOptions {FormatEnum = CellFormatEnum.Bold},
                        Value = Resources.Time
                    });
                excelService.SetCell(sheet,
                    new WorksheetCell
                    {
                        Column = 1,
                        Row = 0,
                        Options = new CellOptions {FormatEnum = CellFormatEnum.Bold},
                        Value = Resources.Operation
                    });
                excelService.SetCell(sheet,
                    new WorksheetCell
                    {
                        Column = 2,
                        Row = 0,
                        Options = new CellOptions {FormatEnum = CellFormatEnum.Bold},
                        Value = Resources.OperationID
                    });
                excelService.SetCell(sheet,
                    new WorksheetCell
                    {
                        Column = 3,
                        Row = 0,
                        Options = new CellOptions {FormatEnum = CellFormatEnum.Bold},
                        Value = Resources.Operator
                    });
                excelService.SetCell(sheet,
                    new WorksheetCell
                    {
                        Column = 4,
                        Row = 0,
                        Options = new CellOptions {FormatEnum = CellFormatEnum.Bold},
                        Value = Resources.OverrideOperator
                    });
                excelService.SetCell(sheet,
                    new WorksheetCell
                    {
                        Column = 5,
                        Row = 0,
                        Options = new CellOptions {FormatEnum = CellFormatEnum.Bold},
                        Value = Resources.StoreText
                    });
                excelService.SetCell(sheet,
                    new WorksheetCell
                    {
                        Column = 6,
                        Row = 0,
                        Options = new CellOptions {FormatEnum = CellFormatEnum.Bold},
                        Value = Resources.TerminalText
                    });

                //Populating the worksheet
                int workSheetIndex = 1;
                for (int collectionIndex = 0; collectionIndex < operations.Count; collectionIndex++)
                {
                    excelService.SetCellValue(sheet, workSheetIndex, 0,
                        operations[collectionIndex].CreatedDate.ToString("G"));
                    excelService.SetCellValue(sheet, workSheetIndex, 1, operations[collectionIndex].OperationName);
                    excelService.SetCellValue(sheet, workSheetIndex, 2, operations[collectionIndex].OperationID);
                    excelService.SetCellValue(sheet, workSheetIndex, 3, (string) operations[collectionIndex].UserID);
                    excelService.SetCellValue(sheet, workSheetIndex, 4, (string) operations[collectionIndex].ManagerID);
                    excelService.SetCellValue(sheet, workSheetIndex, 5, (string) operations[collectionIndex].StoreID);
                    excelService.SetCellValue(sheet, workSheetIndex, 6, (string) operations[collectionIndex].TerminalID);
                    workSheetIndex++;
                }

                excelService.SetWorksheetOptions(sheet, options);
                excelService.Save(workBook, excelFile);
                excelFile.Locked = true;
                // Set to locked to force the user to 'Save as' and select a specific place for the file
                excelFile.Launch();
            }
            finally
            {
                HideProgress();
            }

            btnGenerateExcelFile.Enabled = true;
        }

    }
}
