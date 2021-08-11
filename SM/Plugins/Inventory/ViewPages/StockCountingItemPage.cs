using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Expressions;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Dialogs;
using LSOne.ViewPlugins.Inventory.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    public partial class StockCountingItemPage : UserControl, ITabView
    {
        IInventoryService service = null;

        private static Guid BarSettingID = new Guid("36F4ED4C-A613-49BF-A006-A4D3AB352B24");
        private Setting searchBarSetting;
        InventoryAdjustment stockCounting;
        InventoryTemplate inventoryTemplate;
        AdjustmentStatus journalStatus;
        private bool canEditImages = false;
        private bool suppressInventoryRefresh = true;
        private bool canDeleteImages = false;
        private bool postAllLinesAsyncState = false;

        public StockCountingItemPage()
        {
            InitializeComponent();

            lvStockCount.ContextMenuStrip = new ContextMenuStrip();
            lvStockCount.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            searchBar1.BuddyControl = lvStockCount;
            btnsEditAddRemove.EditButtonEnabled = false;
            btnsEditAddRemove.RemoveButtonEnabled = false;

            stocCountDataScroll.PageSize = PluginEntry.DataModel.PageSize;

            stocCountDataScroll.Reset();

            canEditImages = PluginEntry.Framework.CanRunOperation("AddEditImage");
            canDeleteImages = PluginEntry.Framework.CanRunOperation("DeleteImage");
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new StockCountingItemPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            stockCounting = (InventoryAdjustment)internalContext;

            if (service == null)
            {
                service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
            }

            if (!RecordIdentifier.IsEmptyOrNull(stockCounting.TemplateID))
            {
                inventoryTemplate = service.GetInventoryTemplate(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), stockCounting.TemplateID, false); //Keep connection open, will be close in CheckProcessingStatus
            }

            LoadItems();
            CheckProcessingStatus(false);
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        #endregion

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvStockCount.ContextMenuStrip;

            menu.Items.Clear();
            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(Resources.Edit, 100, new EventHandler(btnsEditAddRemove_EditButtonClicked))
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(Resources.Add, 200, btnsEditAddRemove_AddButtonClicked);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsEditAddRemove.AddButtonEnabled;
            menu.Items.Add(item);

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(Resources.Delete, 300, btnRemoveClicked);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;
            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 350));

            item = new ExtendedMenuItem(Resources.PostSelectedItem, 400, btnPostLine_Click);
            item.Enabled = btnPostLine.Enabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(Resources.PostAllLines, 500, btnPostAllLines_Click);
            item.Enabled = btnPostAllLines.Enabled;
            menu.Items.Add(item);

            if (canDeleteImages)
            {
                menu.Items.Add(new ExtendedMenuItem("-", 600));

                item = new ExtendedMenuItem(Resources.DeleteImages, 650, DeleteImageHandler);
                item.Enabled = btnsEditAddRemove.EditButtonEnabled && LinesWithImagesSelected();
                menu.Items.Add(item);
            }

            PluginEntry.Framework.ContextMenuNotify("StockCountingView", lvStockCount.ContextMenuStrip, lvStockCount);

            e.Cancel = (menu.Items.Count == 0);
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "AddEditStockCountLine":
                    suppressInventoryRefresh = true;
                    LoadItems();
                    break;
                case "StockCountLine":
                    LoadItems();
                    break;
                case "StockCounting":
                    stockCounting = (InventoryAdjustment)param;
                    suppressInventoryRefresh = true;
                    LoadItems();
                    journalStatus.InventoryStatus = stockCounting.Posted;
                    journalStatus.ProcessingStatus = stockCounting.ProcessingStatus;
                    RefreshViewProcessingStatus();
                    break;
            }
        }

        public void OnClose()
        {
            postAllLinesAsyncState = false;
        }

        public void SaveUserInterface()
        {
        }

        private void RefreshViewProcessingStatus()
        {
            btnsEditAddRemove.AddButtonEnabled = journalStatus.ProcessingStatus == InventoryProcessingStatus.None && stockCounting.Posted == InventoryJournalStatus.Active;

            lvStockCount_SelectionChanged(this, EventArgs.Empty);

            string processingText = Resources.None;

            switch (journalStatus.ProcessingStatus)
            {
                case InventoryProcessingStatus.Compressing:
                    processingText = Resources.Compressing;
                    break;
                case InventoryProcessingStatus.Posting:
                    processingText = Resources.Posting;
                    break;
                case InventoryProcessingStatus.Importing:
                    processingText = Resources.Importing;
                    break;
                case InventoryProcessingStatus.Other:
                    processingText = Resources.Other;
                    break;
            }

            lblProcessingStatus.Text = processingText;
            lblProcessingStatus.Font = new Font(lblProcessingStatus.Font, journalStatus.ProcessingStatus == InventoryProcessingStatus.None ? FontStyle.Regular : FontStyle.Bold);
            lblProcessingStatus.ForeColor = journalStatus.ProcessingStatus == InventoryProcessingStatus.None ? Color.Black : Color.Red;
        }

        private bool CheckProcessingStatus(bool displayMessage)
        {
            if (service == null)
            {
                service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
            }

            AdjustmentStatus currentStatus = service.GetAdjustmentStatus(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), stockCounting.ID, true);

            bool processingStatusChanged = (journalStatus.ProcessingStatus == InventoryProcessingStatus.None && currentStatus.ProcessingStatus != InventoryProcessingStatus.None)
                                        || (journalStatus.ProcessingStatus != InventoryProcessingStatus.None && currentStatus.ProcessingStatus == InventoryProcessingStatus.None);

            if (displayMessage && ((journalStatus.ProcessingStatus == InventoryProcessingStatus.None && currentStatus.ProcessingStatus != InventoryProcessingStatus.None) || currentStatus.InventoryStatus != InventoryJournalStatus.Active))
            {
                MessageDialog.Show(Resources.JournalCannotBeUpdated, MessageBoxIcon.Warning);
            }

            journalStatus = currentStatus;
            stockCounting.Posted = currentStatus.InventoryStatus;
            stockCounting.ProcessingStatus = currentStatus.ProcessingStatus;

            RefreshViewProcessingStatus();

            if(processingStatusChanged)
            {
                //Refresh the context bar on the main view
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.None, "StockCounting", stockCounting.ID, stockCounting);
            }

            return journalStatus.ProcessingStatus == InventoryProcessingStatus.None && currentStatus.InventoryStatus == InventoryJournalStatus.Active;
        }

        private void LoadItems()
        {
            if (!suppressInventoryRefresh)
            {
                PluginOperations.RefreshJournalInventoryOnHand(stockCounting.ID);
            }

            suppressInventoryRefresh = false;

            List<InventoryJournalTransaction> journals = new List<InventoryJournalTransaction>();

            lvStockCount.ClearRows();

            InventoryJournalTransactionFilter filter = new InventoryJournalTransactionFilter();

            InventoryJournalTransactionSorting?[] columns = new InventoryJournalTransactionSorting?[]
            {
                InventoryJournalTransactionSorting.ItemId,
                InventoryJournalTransactionSorting.ItemName,
                InventoryJournalTransactionSorting.Variant,
                InventoryJournalTransactionSorting.Counted,
                InventoryJournalTransactionSorting.InventOnHand,
                InventoryJournalTransactionSorting.CountedDifference,
                InventoryJournalTransactionSorting.CountedDifferencePercantage,
                InventoryJournalTransactionSorting.TransactionDate,
                InventoryJournalTransactionSorting.Posted,
                InventoryJournalTransactionSorting.PostedDate,
                InventoryJournalTransactionSorting.RetailGroup,
                InventoryJournalTransactionSorting.RetailDepartment,
                InventoryJournalTransactionSorting.Staff,
                InventoryJournalTransactionSorting.Area,
            };

            if (lvStockCount.SortColumn == null)
            {
                lvStockCount.SetSortColumn(lvStockCount.Columns[1], true);
            }

            int sortColumnIndex = lvStockCount.Columns.IndexOf(lvStockCount.SortColumn);

            filter.Sort = (InventoryJournalTransactionSorting)((int)columns[sortColumnIndex]);

            List<SearchParameterResult> results = searchBar1.SearchParameterResults;
            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Description":
                        filter.IdOrDescriptions = result.StringValue.Tokenize();
                        filter.IdOrDescriptionStartsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Variant":
                        filter.Variants = result.StringValue.Tokenize();
                        filter.VariantStartsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Status":
                        filter.Posted = result.CheckedValues[0];
                        filter.PostedSet = true;
                        break;
                    case "PostedDate":
                        if (result.Date.Checked && !result.Time.Checked)
                        {
                            filter.DateFrom.DateTime = result.Date.Value.Date;
                        }
                        if (result.DateTo.Checked && !result.TimeTo.Checked)
                        {
                            filter.DateTo.DateTime = result.DateTo.Value.Date.AddDays(1).AddSeconds(-1);
                        }
                        if (result.Date.Checked && result.Time.Checked)
                        {
                            filter.DateFrom.DateTime = new DateTime(result.Date.Value.Year, result.Date.Value.Month,
                                result.Date.Value.Day, result.Time.Value.Hour, result.Time.Value.Minute,
                                result.Time.Value.Second);
                        }
                        if (result.DateTo.Checked && result.TimeTo.Checked)
                        {
                            filter.DateTo.DateTime = new DateTime(result.DateTo.Value.Year, result.DateTo.Value.Month,
                                result.DateTo.Value.Day, result.TimeTo.Value.Hour, result.TimeTo.Value.Minute,
                                result.TimeTo.Value.Second);
                        }
                        break;
                    case "Counted":
                        filter.CountedSet = true;
                        filter.Counted = result.DoubleValue;
                        if (result.SearchModification == SearchParameterResult.SearchModificationEnum.Equals)
                        {
                            filter.CountedComparison = DoubleValueOperator.Equals;
                        }
                        else if (result.SearchModification == SearchParameterResult.SearchModificationEnum.GreaterThan)
                        {
                            filter.CountedComparison = DoubleValueOperator.GreaterThan;
                        }
                        else if (result.SearchModification == SearchParameterResult.SearchModificationEnum.LessThan)
                        {
                            filter.CountedComparison = DoubleValueOperator.LessThan;
                        }
                        else
                        {
                            filter.CountedSet = false;
                        }
                        break;
                    case "InventoryOnHand":
                        filter.InventoryOnHandSet = true;
                        filter.InventoryOnHand = result.DoubleValue;
                        if (result.SearchModification == SearchParameterResult.SearchModificationEnum.Equals)
                        {
                            filter.InventoryOnHandComparison = DoubleValueOperator.Equals;
                        }
                        else if (result.SearchModification == SearchParameterResult.SearchModificationEnum.GreaterThan)
                        {
                            filter.InventoryOnHandComparison = DoubleValueOperator.GreaterThan;
                        }
                        else if (result.SearchModification == SearchParameterResult.SearchModificationEnum.LessThan)
                        {
                            filter.InventoryOnHandComparison = DoubleValueOperator.LessThan;
                        }
                        else
                        {
                            filter.InventoryOnHandSet = false;
                        }
                        break;
                    case "Difference":
                        filter.DifferenceSet = true;
                        filter.Difference = result.DoubleValue;
                        if (result.SearchModification == SearchParameterResult.SearchModificationEnum.Equals)
                        {
                            filter.DifferenceComparison = DoubleValueOperator.Equals;
                        }
                        else if (result.SearchModification == SearchParameterResult.SearchModificationEnum.GreaterThan)
                        {
                            filter.DifferenceComparison = DoubleValueOperator.GreaterThan;
                        }
                        else if (result.SearchModification == SearchParameterResult.SearchModificationEnum.LessThan)
                        {
                            filter.DifferenceComparison = DoubleValueOperator.LessThan;
                        }
                        else
                        {
                            filter.DifferenceSet = false;
                        }
                        break;
                    case "Difference%":
                        filter.DifferencePercentageSet = true;
                        filter.DifferencePercentage = result.DoubleValue;
                        if (result.SearchModification == SearchParameterResult.SearchModificationEnum.Equals)
                        {
                            filter.DifferencePercentageComparison = DoubleValueOperator.Equals;
                        }
                        else if (result.SearchModification == SearchParameterResult.SearchModificationEnum.GreaterThan)
                        {
                            filter.DifferencePercentageComparison = DoubleValueOperator.GreaterThan;
                        }
                        else if (result.SearchModification == SearchParameterResult.SearchModificationEnum.LessThan)
                        {
                            filter.DifferencePercentageComparison = DoubleValueOperator.LessThan;
                        }
                        else
                        {
                            filter.DifferencePercentageSet = false;
                        }
                        break;
                    case "RetailGroup":
                        filter.RetailGroupID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "RetailDepartment":
                        filter.RetailDepartmentID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "Staff":
                        filter.StaffID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "Area":
                        filter.AreaID = (Guid)((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                }
            }

            filter.SortBackwards = !lvStockCount.SortedAscending;
            filter.RowFrom = stocCountDataScroll.StartRecord;
            filter.RowTo = stocCountDataScroll.EndRecord + 1;
            filter.JournalTransactionID = stockCounting.ID;

            int totalRecords = 0;
            try
            {
                if(service == null)
                {
                    service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                }

                SpinnerDialog dlg = new SpinnerDialog(Resources.FewMinutesMessage, () => journals = service.SearchJournalTransactions(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), filter, out totalRecords, true));

                dlg.ShowDialog();
            }
            catch
            {
                MessageBox.Show(Resources.CouldNotConnectToStoreServer);
                return;
            }

            stocCountDataScroll.RefreshState(journals, totalRecords);

            Style strikeThroughStyle = new Style(lvStockCount.DefaultStyle);
            strikeThroughStyle.Font = new Font(strikeThroughStyle.Font, FontStyle.Strikeout);

            Row row;
            Style rowStyle;
            Style receivedCellStyle;
            foreach (InventoryJournalTransaction transJournal in journals)
            {
                row = new Row();

                rowStyle = transJournal.ItemInventoryExcluded ? strikeThroughStyle : lvStockCount.DefaultStyle;
                receivedCellStyle = new Style(rowStyle);

                row.AddCell(new Cell((string)transJournal.ItemId, rowStyle));
                row.AddCell(new Cell(transJournal.ItemName, rowStyle));
                row.AddCell(new Cell(transJournal.VariantName, rowStyle));

                DecimalLimit qtyLimiteer = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
                decimal countedInInventoryUnits = 0;
                decimal countedDifference = 0;

                // Add the amount counted in inventory units if the counted unit is not the inventory unit. This is displayed for clarity
                if (transJournal.UnitID != transJournal.InventoryUnitID)
                {
                    countedInInventoryUnits = Providers.UnitConversionData.ConvertQtyBetweenUnits(PluginEntry.DataModel, transJournal.ItemId, transJournal.UnitID, transJournal.InventoryUnitID, transJournal.Counted);

                    countedDifference = countedInInventoryUnits - transJournal.InventOnHandInInventoryUnits;
                    if (countedDifference != 0)
                    {
                        receivedCellStyle.TextColor = countedDifference > 0 ? ColorPalette.GreenLight : ColorPalette.RedLight;
                    }

                    string countedInInventoryUnitsString = "";
                    if (countedInInventoryUnits != transJournal.Counted)
                    {
                        countedInInventoryUnitsString = countedInInventoryUnits.FormatWithLimits(transJournal.UnitQuantityLimiter) + " " + transJournal.InventoryUnitDescription;
                    }

                    string cellText = transJournal.Counted.FormatWithLimits(transJournal.UnitQuantityLimiter) + " " + transJournal.UnitDescription + " (" + countedInInventoryUnitsString + ")";
                    row.AddCell(new NumericCell(cellText, receivedCellStyle, transJournal.Counted));
                }
                else
                {
                    countedInInventoryUnits = transJournal.Counted;

                    countedDifference = countedInInventoryUnits - transJournal.InventOnHandInInventoryUnits;
                    if (countedDifference != 0)
                    {
                        receivedCellStyle.TextColor = countedDifference > 0 ? ColorPalette.GreenLight : ColorPalette.RedLight;
                    }

                    string cellText = transJournal.Counted.FormatWithLimits(transJournal.UnitQuantityLimiter) + " " + transJournal.UnitDescription;
                    row.AddCell(new NumericCell(cellText, receivedCellStyle, transJournal.Counted));
                }
                Cell cell = new NumericCell(transJournal.InventOnHandInInventoryUnits.FormatWithLimits(qtyLimiteer) + " " + transJournal.InventoryUnitDescription, rowStyle, transJournal.InventOnHandInInventoryUnits);
                row.AddCell(cell);

                row.AddCell(
                    new NumericCell(
                        countedDifference.FormatWithLimits(qtyLimiteer) + " " +
                        transJournal.InventoryUnitDescription, rowStyle, countedDifference));

                decimal differncePercentage = transJournal.InventOnHandInInventoryUnits == 0
                    ? 100
                    : 
                    ((transJournal.InventOnHandInInventoryUnits < countedInInventoryUnits && transJournal.InventOnHandInInventoryUnits < 0)
                    ? (countedInInventoryUnits / transJournal.InventOnHandInInventoryUnits * -100) + 100 
                    : (countedInInventoryUnits / transJournal.InventOnHandInInventoryUnits * 100) - 100);

                row.AddCell(
                  new NumericCell(
                      differncePercentage.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent)) + " " +
                      "%", rowStyle, differncePercentage));

                row.AddCell(new DateTimeCell(transJournal.TransDate.ToShortDateString(), transJournal.TransDate, rowStyle));
                row.AddCell(new Cell((transJournal.Posted) ? Properties.Resources.Yes : Properties.Resources.No, rowStyle));
                row.AddCell(new DateTimeCell(transJournal.Posted ? transJournal.PostedDateTime.ToShortDateString() : "", transJournal.PostedDateTime, rowStyle));

                row.AddCell(new Cell(transJournal.RetailGroup, rowStyle));
                row.AddCell(new Cell(transJournal.RetailDepartment, rowStyle));
                row.AddCell(new Cell((string)transJournal.StaffLogin + " - " +  transJournal.StaffName, rowStyle));
                row.AddCell(new Cell(transJournal.AreaName, rowStyle));

                if (!RecordIdentifier.IsEmptyOrNull(transJournal.PictureID) && canEditImages)
                {
                    IconButton button = new IconButton(Resources.Camera24, Properties.Resources.EditImage);
                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.HorizontalCenter | IconButtonCell.IconButtonIconAlignmentEnum.VerticalCenter, ""));
                }

                row.Tag = transJournal;

                lvStockCount.AddRow(row);
            }

            lvStockCount_SelectionChanged(this, EventArgs.Empty);

            lvStockCount.AutoSizeColumns();
        }

        private void lvStockCount_SelectionChanged(object sender, EventArgs e)
        {
            bool actionsEnabled = false;
            bool isPosted = false;
            if (lvStockCount.Selection.Count > 0)
            {
                for (int i = 0; i < lvStockCount.Selection.Count; i++)
                {
                    InventoryJournalTransaction currentTransaction = ((InventoryJournalTransaction)lvStockCount.Row(lvStockCount.Selection.GetRowIndex(i)).Tag);
                    if (!currentTransaction.Posted && !currentTransaction.ItemInventoryExcluded)
                    {
                        actionsEnabled = true;
                    }
                    if (currentTransaction.Posted)
                    {
                        isPosted = true;
                    }
                }
            }

            bool journalCanBeEdited = stockCounting.Posted == InventoryJournalStatus.Active && journalStatus.ProcessingStatus == InventoryProcessingStatus.None;

            btnsEditAddRemove.RemoveButtonEnabled = journalCanBeEdited && !isPosted && lvStockCount.Selection.Count > 0 ;
            btnsEditAddRemove.EditButtonEnabled = journalCanBeEdited && actionsEnabled;
            btnPostLine.Enabled = journalCanBeEdited && actionsEnabled;
            btnPostAllLines.Enabled = journalCanBeEdited && lvStockCount.Rows.Any();
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            if(CheckProcessingStatus(true))
            {
                StockCountingLineDialog dlg = new StockCountingLineDialog(stockCounting.ID, stockCounting.StoreId);
                if(dlg.ShowDialog() == DialogResult.Abort)
                {
                    journalStatus = dlg.JournalStatus;
                    RefreshViewProcessingStatus();
                }
            }
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            if(CheckProcessingStatus(true))
            {
                if (lvStockCount.Selection.Count == 1)
                {
                    var item = (InventoryJournalTransaction)lvStockCount.Rows[lvStockCount.Selection.FirstSelectedRow].Tag;
                    StockCountingLineDialog dlg = new StockCountingLineDialog(stockCounting.ID, stockCounting.StoreId, item.ID);
                    if (dlg.ShowDialog() == DialogResult.Abort)
                    {
                        journalStatus = dlg.JournalStatus;
                        RefreshViewProcessingStatus();
                    }
                }
                else
                {
                    List<InventoryJournalTransaction> Journals = new List<InventoryJournalTransaction>();

                    for (int i = 0; i < lvStockCount.Selection.Count; i++)
                    {
                        InventoryJournalTransaction currentTransaction = ((InventoryJournalTransaction)lvStockCount.Row(lvStockCount.Selection.GetRowIndex(i)).Tag);
                        if (!currentTransaction.Posted)
                        {
                            Journals.Add(currentTransaction);
                        }
                    }

                    if (Journals.Count == 1)
                    {
                        var item = Journals[0];
                        StockCountingLineDialog dlg = new StockCountingLineDialog(stockCounting.ID, stockCounting.StoreId, item.ID);
                        if (dlg.ShowDialog() == DialogResult.Abort)
                        {
                            journalStatus = dlg.JournalStatus;
                            RefreshViewProcessingStatus();
                        }
                    }
                    else
                    {
                        StockCountingLineDialogMulti dlg = new StockCountingLineDialogMulti(stockCounting.ID, Journals);
                        if (dlg.ShowDialog() == DialogResult.Abort)
                        {
                            journalStatus = dlg.JournalStatus;
                            RefreshViewProcessingStatus();
                        }
                    }
                }
            }
        }

        private void btnRemoveClicked(object sender, EventArgs e)
        {
            bool continueRemoving = false;

            if (lvStockCount.Selection.Count > 1)
            {
                continueRemoving = QuestionDialog.Show(Resources.SureToRemoveMultipleItems, Resources.RemoveSelectedLine) == DialogResult.Yes;
            }
            else if (lvStockCount.Selection.Count == 1)
            {
                continueRemoving = QuestionDialog.Show(Resources.SureToRemoveItem, Resources.RemoveSelectedLine) == DialogResult.Yes;
            }

            if (continueRemoving && CheckProcessingStatus(true))
            {
                List<RecordIdentifier> transactionsIDsToRemove = new List<RecordIdentifier>();
                
                while (lvStockCount.Selection.Count > 0)
                {
                    var transaction = ((InventoryJournalTransaction)lvStockCount.Row(lvStockCount.Selection.FirstSelectedRow).Tag);
                    if (transaction.Posted == false)
                    {
                        transactionsIDsToRemove.Add(transaction.ID);
                    }
                    lvStockCount.RemoveRow(lvStockCount.Selection.FirstSelectedRow);
                };

                PluginOperations.RemoveJournalTransactions(transactionsIDsToRemove);

                if (lvStockCount.RowCount == 0)
                {
                    stocCountDataScroll.ScrollOneLeft();
                }
            }
        }

        private void btnPostLine_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(Resources.SureToPostItem, Resources.PostSelectedItem) == DialogResult.Yes)
            {
                List<InventoryJournalTransaction> transactionsToPost = new List<InventoryJournalTransaction>();

                for (int i = 0; i < lvStockCount.Selection.Count; i++)
                {
                    InventoryJournalTransaction currentTransaction = ((InventoryJournalTransaction)lvStockCount.Row(lvStockCount.Selection.GetRowIndex(i)).Tag);

                    if (!currentTransaction.ItemInventoryExcluded && !currentTransaction.Posted)
                    {
                        transactionsToPost.Add(currentTransaction);
                    }
                }

                if(CheckProcessingStatus(true))
                {
                    journalStatus.ProcessingStatus = InventoryProcessingStatus.Posting;
                    RefreshViewProcessingStatus();

                    PostStockCountingLinesContainer result = PluginOperations.PostMultipleStockCountingLines(transactionsToPost, stockCounting.StoreId);

                    RefreshViews(result.HasUnpostedLines);
                    suppressInventoryRefresh = true;
                    LoadItems();
                    CheckProcessingStatus(false);
                }
            }
        }

        private async void btnPostAllLines_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(Resources.SureToPostAllLines, Resources.PostAllLines) == DialogResult.Yes)
            {
                if(CheckProcessingStatus(true))
                {
                    try
                    {
                        var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                        postAllLinesAsyncState = true;
                        Task<PostStockCountingLinesContainer> task = Task.Run(() => service.AsyncPostAllStockCountingLines(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), stockCounting.ID));

                        journalStatus.ProcessingStatus = InventoryProcessingStatus.Posting;
                        RefreshViewProcessingStatus();

                        //Refresh journals view to see the processing status
                        PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.None, "JournalTrans", RecordIdentifier.Empty, null);
                        await task;

                        if(postAllLinesAsyncState)
                        {
                            PluginOperations.CheckPostStockCountingStatus(task.Result.Result);
                            RefreshViews(task.Result.HasUnpostedLines);
                        }
                    }
                    catch(Exception ex)
                    {
                        postAllLinesAsyncState = false;
                        PluginEntry.Framework.LogMessage(Utilities.ErrorHandling.LogMessageType.Error, stockCounting.ID + ": " + Resources.ErrorPostingJournalConnectionLost, ex);
                    }

                    if (postAllLinesAsyncState)
                    {
                        suppressInventoryRefresh = true;
                        LoadItems();
                        CheckProcessingStatus(false);
                    }

                    postAllLinesAsyncState = false;
                }
            }
        }

        private void RefreshViews(bool hasUnpostedLines)
        {
            if (!hasUnpostedLines)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "StockCountPosted", stockCounting.ID, null);

                stockCounting.Posted = InventoryJournalStatus.Posted;
                btnPostAllLines.Enabled = false;
                btnPostLine.Enabled = false;
                btnsEditAddRemove.AddButtonEnabled = false;

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.None, "JournalTrans", RecordIdentifier.Empty, null);
            }
        }

        private void searchBar1_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar1.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private void searchBar1_SaveAsDefault(object sender, EventArgs e)
        {
            string layoutString = searchBar1.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, BarSettingID, SettingsLevel.User, searchBarSetting);
            }
        }

        private void searchBar1_SetupConditions(object sender, EventArgs e)
        {
            searchBar1.AddCondition(new ConditionType(Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar1.AddCondition(new ConditionType(Resources.VariantDescription, "Variant", ConditionType.ConditionTypeEnum.Text));
            searchBar1.AddCondition(new ConditionType(Resources.RetailGroup, "RetailGroup", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.RetailDepartment, "RetailDepartment", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.Status, "Status", ConditionType.ConditionTypeEnum.Checkboxes, Resources.Posted, true));
            searchBar1.AddCondition(new ConditionType(Resources.PostedDate, "PostedDate", ConditionType.ConditionTypeEnum.DateAndTimeRange));
            searchBar1.AddCondition(new ConditionType(Resources.Counted, "Counted", ConditionType.ConditionTypeEnum.Numeric));
            searchBar1.AddCondition(new ConditionType(Resources.InventoryOnHand, "InventoryOnHand", ConditionType.ConditionTypeEnum.Numeric));
            searchBar1.AddCondition(new ConditionType(Resources.Difference, "Difference", ConditionType.ConditionTypeEnum.Numeric));
            searchBar1.AddCondition(new ConditionType(Resources.DifferencePercentage, "Difference%", ConditionType.ConditionTypeEnum.Numeric));
            searchBar1.AddCondition(new ConditionType(Resources.SearchBar_Staff, "Staff", ConditionType.ConditionTypeEnum.Unknown));

            if(inventoryTemplate != null && !RecordIdentifier.IsEmptyOrNull(inventoryTemplate.AreaID))
            {
                searchBar1.AddCondition(new ConditionType(Resources.SearchBar_Area, "Area", ConditionType.ConditionTypeEnum.Unknown));
            }

            searchBar1_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar1_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            switch (args.TypeKey)
            {
                case "RetailGroup":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");
                    ((DualDataComboBox)args.UnknownControl).DropDown += new DropDownEventHandler(RetailGroup_DropDown);
                    break;
                case "RetailDepartment":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");
                    ((DualDataComboBox)args.UnknownControl).DropDown += new DropDownEventHandler(RetailDepartments_DropDown);
                    break;
                case "Staff":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");
                    ((DualDataComboBox)args.UnknownControl).DropDown += cmbEmployee__DropDown;
                    break;
                case "Area":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");
                    ((DualDataComboBox)args.UnknownControl).RequestData += Area_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear += ComboBox_RequestClear;
                    break;
            }
        }

        private void cmbEmployee__DropDown(object sender, DropDownEventArgs e)
        {
            e.ControlToEmbed = new EmployeeSearchPanel(PluginEntry.DataModel, e.DisplayText);
        }

        private void searchBar1_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "RetailGroup":
                case "RetailDepartment":
                case "Staff":
                case "Area":
                    args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
                    break;
            }
        }

        private void searchBar1_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "RetailGroup":
                case "RetailDepartment":
                case "Staff":
                case "Area":
                    args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
                    break;
            }
        }

        private void searchBar1_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;
            try
            {
                switch (args.TypeKey)
                {
                    case "RetailGroup":
                        entity = Providers.RetailGroupData.Get(PluginEntry.DataModel, args.Selection);
                        break;
                    case "RetailDepartment":
                        entity = Providers.RetailDepartmentData.Get(PluginEntry.DataModel, args.Selection);
                        break;
                    case "SpecialGroup":
                        entity = Providers.SpecialGroupData.Get(PluginEntry.DataModel, args.Selection);
                        break;
                    case "Vendor":
                        entity = Services.Interfaces.Services
                            .InventoryService(PluginEntry.DataModel)
                            .GetVendor(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), args.Selection, true);
                        break;
                    case "TaxGroup":
                        entity = Providers.ItemSalesTaxGroupData.Get(PluginEntry.DataModel, args.Selection);
                        break;
                    case "Staff":
                        entity = Providers.POSUserData.Get(PluginEntry.DataModel, args.Selection, UsageIntentEnum.Minimal);
                        break;
                    case "Area":
                        entity = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryAreaLine(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), args.Selection, true);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }

            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void searchBar1_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "RetailGroup":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= RetailGroup_DropDown;
                    break;
                case "RetailDepartment":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= RetailDepartments_DropDown;
                    break;
                case "Staff":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= Staff_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear -= ComboBox_RequestClear;
                    break;
                case "Area":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= Area_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear -= ComboBox_RequestClear;
                    break;
            }
        }

        private void searchBar1_SearchClicked(object sender, EventArgs e)
        {
            suppressInventoryRefresh = true;
            LoadItems();
        }

        void RetailDepartments_DropDown(object sender, DropDownEventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, e.DisplayText, SearchTypeEnum.RetailDepartmentsMasterID, "");
        }

        void RetailGroup_DropDown(object sender, DropDownEventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, e.DisplayText, SearchTypeEnum.RetailGroupsMasterID, "");
        }

        private void Area_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            ((DualDataComboBox)sender).SetData(
                (inventoryTemplate == null || RecordIdentifier.IsEmptyOrNull(inventoryTemplate.AreaID)) 
                ? new List<InventoryAreaLine>() 
                : Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryAreaLinesByArea(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), inventoryTemplate.AreaID, true), 
            null);
        }

        private void Staff_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            ((DualDataComboBox)sender).SetData(Providers.POSUserData.GetList(PluginEntry.DataModel), null);
        }

        private void ComboBox_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void lvStockCount_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void lvStockCount_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            if (lvStockCount.SortColumn == args.Column)
            {
                lvStockCount.SetSortColumn(args.Column, !lvStockCount.SortedAscending);
            }
            else
            {
                lvStockCount.SetSortColumn(args.Column, true);
            }

            stocCountDataScroll.Reset();

            suppressInventoryRefresh = true;
            LoadItems();
        }

        private void itemDataScroll_PageChanged(object sender, EventArgs e)
        {
            suppressInventoryRefresh = true;
            LoadItems();
        }

        private void LvStockCount_CellAction(object sender, Controls.EventArguments.CellEventArgs args)
        {
            if(args.Cell is IconButtonCell)
            {                
                RecordIdentifier pictureID = ((InventoryJournalTransaction)lvStockCount.Row(args.RowNumber).Tag).PictureID;

                if (!Providers.ImageData.Exists(PluginEntry.DataModel, pictureID))
                {
                    MessageDialog.Show(Properties.Resources.CannotEditImageLocally);
                    return;
                }

                PluginEntry.Framework.RunOperation("AddEditImage", this, new ViewCore.EventArguments.PluginOperationArguments(pictureID, null));
            }
        }

        private void DeleteImageHandler(object sender, EventArgs args)
        {
            List<InventoryJournalTransaction> modifiedTransactions = new List<InventoryJournalTransaction>();

            if(lvStockCount.Selection.Count == 1)
            {
                // We don't have to check for an image here since the "Delete images" button is only enabled for a single line if it has a picture ID
                if(QuestionDialog.Show(Properties.Resources.DeleteImageQuestion) == DialogResult.Yes)
                {
                    InventoryJournalTransaction transaction = ((InventoryJournalTransaction)lvStockCount.Row(lvStockCount.Selection.FirstSelectedRow).Tag);
                    modifiedTransactions.Add(transaction);

                    Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).DeleteImage(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), transaction.PictureID, false);                    
                }
            }
            else
            {                
                List<InventoryJournalTransaction> transactionsWithPictureIDs = new List<InventoryJournalTransaction>();

                for(int i = 0; i < lvStockCount.Selection.Count; i++)
                {
                    InventoryJournalTransaction transaction = ((InventoryJournalTransaction)lvStockCount.Selection[i].Tag);

                    if (!RecordIdentifier.IsEmptyOrNull(transaction.PictureID))
                    {
                        transactionsWithPictureIDs.Add(transaction);
                    }
                }

                if(QuestionDialog.Show(Properties.Resources.DeleteImagesQuestion) == DialogResult.Yes)
                {
                    var pictureIDs = from transaction in transactionsWithPictureIDs select transaction.PictureID;
                    Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).DeleteImageList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), pictureIDs.ToList(), false);

                    modifiedTransactions = transactionsWithPictureIDs;
                }
            }
            
            int totalCount = modifiedTransactions.Count;
            using (ProgressDialog dlg = new ProgressDialog(Resources.SavingStockcountingLines, Resources.SavingCounter, totalCount))
            {
                Action saveAction = () =>
                {
                    int count = 1;

                    foreach (InventoryJournalTransaction modifiedTransaction in modifiedTransactions)
                    {
                        modifiedTransaction.PictureID = RecordIdentifier.Empty;
                        PluginOperations.SaveJournalTransaction(
                            modifiedTransaction.JournalId,
                            modifiedTransaction.StoreId,
                            modifiedTransaction.ItemId,
                            modifiedTransaction.UnitID,
                            modifiedTransaction.InventoryUnitID,
                            modifiedTransaction.Counted,
                            modifiedTransaction.LineNum,
                            modifiedTransaction.AreaID,
                            modifiedTransaction.PictureID,
                            false);

                        dlg.Report(count, totalCount);
                        count++;
                    }
                    
                    Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
                };

                dlg.ProgressTask = Task.Run(saveAction);
                dlg.ShowDialog(PluginEntry.Framework.MainWindow);
            }
            
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "StockCountLine", stockCounting.ID, stockCounting);
        }

        /// <summary>
        /// Checks if any of the selected lines has a picture ID
        /// </summary>
        /// <returns></returns>
        private bool LinesWithImagesSelected()
        {
            if(lvStockCount.Selection.Count == 0)
            {
                return false;
            }

            for(int i = 0; i < lvStockCount.Selection.Count; i++)
            {
                if(!RecordIdentifier.IsEmptyOrNull(((InventoryJournalTransaction)lvStockCount.Selection[i].Tag).PictureID))
                {
                    return true;
                }
            }

            return false;
        }
    }
}