using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LSOne.Controls.Cells;
using LSOne.Controls.Columns;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportClasses.ImportExport;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.IO;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.SaleByPaymentMediaReport.Datalayer.DataEntities;
using LSOne.ViewPlugins.SaleByPaymentMediaReport.ReportData;

namespace LSOne.ViewPlugins.SaleByPaymentMediaReport.Views
{
    public partial class SaleByPaymentMediaReportView : ViewBase
    {
        public SaleByPaymentMediaReportView()
        {
            InitializeComponent();
            Attributes = ViewAttributes.Help | ViewAttributes.Close | ViewAttributes.ContextBar;
            dtpFromDate.MaxDate = DateTime.Now;
            dtpToDate.MaxDate = DateTime.Now.AddDays(1);

            cmbUse.Items.Add(new DataEntity((int)DateFilterTypeEnum.BusinessDay, PluginOperations.DateFilterTypeString(DateFilterTypeEnum.BusinessDay)));
            cmbUse.Items.Add(new DataEntity((int)DateFilterTypeEnum.TransDate, PluginOperations.DateFilterTypeString(DateFilterTypeEnum.TransDate)));
            cmbUse.SelectedIndex = 0;
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.SaleByPaymentMediaReport;
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
            HeaderText = Properties.Resources.SaleByPaymentMediaReport;            
            HeaderIcon = null;

            dtpFromDate.Value = DateTime.Today;
            dtpToDate.Value = DateTime.Today;

            if (PluginEntry.DataModel.IsHeadOffice)
            {
                var allStores = Providers.StoreData.GetList(PluginEntry.DataModel);
                if (allStores.Any())
                {
                    cmbStore.SelectedData = allStores[0];
                }
            }
            else
            {
                cmbStore.Enabled = false;
                var store = Providers.StoreData.Get(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
                if (store != null)
                {
                    cmbStore.SelectedData = store;                 
                }
            }
            SetGenerateButtonEnabled();
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        private void SetGenerateButtonEnabled()
        {           
            btnSearch.Enabled = 
            btnGetReport.Enabled = (dtpFromDate.Value.Date <= dtpToDate.Value.Date);                    
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var storeId = (string)cmbStore.SelectedDataID;
            var periodFrom = dtpFromDate.Value;
            var periodTo = dtpToDate.Value.AddDays(1);
            var dateFilterType = (DateFilterTypeEnum)(int)((DataEntity) cmbUse.SelectedItem).ID;

            ShowProgress((sender1, e1) => FillListView(storeId, periodFrom, periodTo, dateFilterType), Properties.Resources.Searching);
        }
     
        private void btnGetReport_Click(object sender, EventArgs e)
        {
            var periodFrom = dtpFromDate.Value;
            var periodTo = dtpToDate.Value;
            var dateFilterType = (DateFilterTypeEnum)(int)((DataEntity)cmbUse.SelectedItem).ID;

            ShowProgress((sender1, e1) => CreateSaleByPaymentMediaReport(cmbStore.SelectedData, periodFrom, periodTo, dateFilterType), 
                Properties.Resources.CreatingExcel);
        }
        
        private void dtpFromDate_ValueChanged_1(object sender, EventArgs e)
        {
            SetGenerateButtonEnabled();
        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            SetGenerateButtonEnabled();
        }

        private void cmbStoreSelect_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> stores = Providers.StoreData.GetList(PluginEntry.DataModel);            
            cmbStore.SetData(stores, null);     
        }

        private void cmbStoreSelect_SelectedDataChanged(object sender, EventArgs e)
        {
            SetGenerateButtonEnabled();
        }

        #region Report generation

        private void FillListView(string storeId, DateTime periodFrom, DateTime periodTo, DateFilterTypeEnum dateFilterType)
        {
            lvPayments.SuspendLayout();
            try
            {
                lvPayments.Columns.Clear();
                lvPayments.ClearRows();

                var listOfTerminalPayments = new List<TerminalPaymentMethods>();
                var listOfSumsForPaymentTypes = new List<PaymentMethodAmount>();

                GetReportPaymentBreakdown(PluginEntry.DataModel, storeId, periodFrom, periodTo, dateFilterType, 
                    ref listOfTerminalPayments, ref listOfSumsForPaymentTypes);
                var data = ConstructForExcel(listOfTerminalPayments, listOfSumsForPaymentTypes);

                int minRows = listOfTerminalPayments.Count;

                AddColumn(Properties.Resources.WorkstationMedia, false);
                foreach (var tenderData in data)
                {
                    // Create a column for the tender
                    AddColumn(tenderData.TenderName, true);

                    var sum = tenderData.Values.Sum(x => x);
                    tenderData.Values.Add(sum);

                    // Should all have the same number of rows - but better safe than sorry ...
                    minRows = Math.Min(minRows, tenderData.Values.Count);
                }
                AddColumn(Properties.Resources.GrandTotal, true);
                minRows++;

                listOfTerminalPayments.Add(new TerminalPaymentMethods { TerminalName = Properties.Resources.GrandTotal});

                var amountLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

                // Fill list
                for (int row = 0; row < minRows; row++)
                {
                    var dataRow = new Row();

                    dataRow.AddText(listOfTerminalPayments[row].TerminalName);

                    var grandTotal = 0m;
                    for (int col = 0; col < data.Count; col++)
                    {
                        var value = data[col].Values[row];
                        grandTotal += value;
                        dataRow.AddCell(new NumericCell(value.FormatWithLimits(amountLimiter), value));
                    }
                    dataRow.AddCell(new NumericCell(grandTotal.FormatWithLimits(amountLimiter), grandTotal));
                    lvPayments.AddRow(dataRow);
                }
                lvPayments.AutoSizeColumns();
            }
            finally
            {
                HideProgress();
                lvPayments.ResumeLayout();
            }
        }

        private void AddColumn(string header, bool rightAlign)
        {
            var col = new Controls.Columns.Column
            {
                AutoSize = true,
                Clickable = false,
                DefaultStyle = null,
                HeaderText = header,
                MaximumWidth = 0,
                MinimumWidth = 10,
                Width = 100
            };
            if (rightAlign)
                col.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            lvPayments.Columns.Add(col);
        }

        private void CreateSaleByPaymentMediaReport(IDataEntity store, DateTime periodFrom, DateTime periodTo, DateFilterTypeEnum dateFilterType)
        {
            btnSearch.Enabled = 
            btnGetReport.Enabled = false;
            try
            {
                var listOfTerminalPayments = new List<TerminalPaymentMethods>();
                var listOfSumsForPaymentTypes = new List<PaymentMethodAmount>();

                GetReportPaymentBreakdown(PluginEntry.DataModel, (string)store.ID, periodFrom, periodTo.AddDays(1), dateFilterType,
                    ref listOfTerminalPayments, ref listOfSumsForPaymentTypes);
                var data = ConstructForExcel(listOfTerminalPayments, listOfSumsForPaymentTypes);

                int cols = data.Count + 2; // +2 for terminal column and grand total column

                var excelFile = FolderItem.GetTempFile(DateTime.Today.ToString("yyyy.MM.dd"), ".xlsx");
                var excelService = Services.Interfaces.Services.ExcelService(PluginEntry.DataModel);

                var workBook = excelService.CreateWorkbook(excelFile, Properties.Resources.PaymentsForTerminals);
                var sheet = excelService.GetWorksheet(workBook, Properties.Resources.PaymentsForTerminals);
                var woptions = new WorksheeetOptions {ColumnWidths = new Dictionary<int, int>()};
                int rowOffset = 0;
                AddCustomHeader(excelService, sheet, ref rowOffset, cols, dateFilterType, store, periodFrom, periodTo);

                rowOffset += 2;

                // Autofit all columns
                for (int i = 0; i < cols; i++)
                {
                    // Deal with column widths
                    woptions.ColumnWidths[i] = WorksheeetOptions.AutoFit;
                }

                // Populate the excel document

                // First write all terminals
                SetGrayFormatted(excelService, sheet, rowOffset, 0, Properties.Resources.WorkstationMedia);
                for (int row = 0; row < listOfTerminalPayments.Count; row++)
                {
                    excelService.SetCellValue(sheet, row + rowOffset + 1, 0, listOfTerminalPayments[row].TerminalName);
                }
                SetGrayFormatted(excelService, sheet, rowOffset + 1 + listOfTerminalPayments.Count, 0, Properties.Resources.GrandTotal);

                // Then iterate through all tenders and write their values
                var amountLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
                int col = 1;
                char colChar = 'B';
                foreach (var tenderData in data)
                {
                    int row = rowOffset;
                    
                    // Write the tender name
                    SetGrayFormatted(excelService, sheet, row++, col, tenderData.TenderName,
                        -1, CellHorizontalAlignment.Right);

                    foreach (var value in tenderData.Values)
                    {
                        excelService.SetCellValue(sheet, row++, col, value, amountLimiter);
                    }

                    // Create a formulate for sum of values
                    string formula = string.Format("=SUM({0}{1}:{0}{2})", colChar, rowOffset + 2, row);
                    SetGrayFormatted(excelService, sheet, row++, col, formula, amountLimiter.Max);

                    // Next column
                    colChar++;
                    col++;
                }

                // Finally create a grand total column at the end
                colChar--;
                SetGrayFormatted(excelService, sheet, rowOffset, col, Properties.Resources.GrandTotal,
                    -1, CellHorizontalAlignment.Right);
                for (int row = 0; row < listOfTerminalPayments.Count; row++)
                {
                    string formula = string.Format("=SUM(B{0}:{1}{0})", row + rowOffset + 2, colChar);
                    SetGrayFormatted(excelService, sheet, row + rowOffset + 1, col, formula, amountLimiter.Max);
                }

                string grandTotalFormula = string.Format("=SUM(B{0}:{1}{0})", listOfTerminalPayments.Count + rowOffset + 2, colChar);
                SetGrayFormatted(excelService, sheet, rowOffset + listOfTerminalPayments.Count + 1, col, grandTotalFormula, amountLimiter.Max);

                excelService.SetWorksheetOptions(sheet, woptions);
                excelService.Save(workBook, excelFile);
                excelFile.Locked = true;
                // Set to locked to force the user to 'Save as' and select a specific place for the file
                excelFile.Launch();
            }
            finally
            {
                HideProgress();
            }
            btnSearch.Enabled = 
            btnGetReport.Enabled = true;
        }

        private static void GetReportPaymentBreakdown(
            IConnectionManager entry,
            string storeId,
            DateTime periodFrom,
            DateTime periodTo,
            DateFilterTypeEnum dateFilterType,
            ref List<TerminalPaymentMethods> terminalPaymentBreakdown,
            ref List<PaymentMethodAmount> paymentMethodBreakdown)
        {
            var allTerminals = Providers.TerminalData.GetTerminals(entry, storeId);
            var allPaymentTypesForStore =
                    Providers.StorePaymentMethodData.GetRecords(
                        entry,
                        storeId, true);
            var allPaymentTransactionsForPeriod =
                Providers.PaymentTransactionData.GetForStoreAndPeriod(entry, storeId, periodFrom, periodTo, dateFilterType);

            foreach (var terminal in allTerminals)
            {
                var terminalsPayments = new TerminalPaymentMethods();
                terminalsPayments.TerminalId = (string)terminal.ID;
                terminalsPayments.TerminalName = terminal.Text;
                terminalsPayments.PaymentTypes = new List<PaymentMethodAmount>();

                var paymentTransactionsForPos = allPaymentTransactionsForPeriod.Where(x => x.TerminalID == terminal.ID);
                foreach (var paymentType in allPaymentTypesForStore)
                {
                    var paymentsForPaymentType = paymentTransactionsForPos.Where(x => x.TenderType == paymentType.PaymentTypeID);
                    decimal amount = 0;


                    if (paymentType.DefaultFunction == PaymentMethodDefaultFunctionEnum.Normal)
                    {
                        // Only include payments with receipt ids because we don't want safe drop, bank drop, float entry and tender removal payments here 
                        paymentsForPaymentType = paymentsForPaymentType.Where(x => x.ReceiptID != "");

                        foreach (var payment in paymentsForPaymentType)
                        {
                            amount += payment.AmountTenderd;
                        }
                        terminalsPayments.PaymentTypes.Add(new PaymentMethodAmount
                        {
                            PaymentType = paymentType.Text,
                            Amount = amount
                        });
                        AddPaymentToPaymentTypeSum(paymentType.Text, amount, paymentMethodBreakdown);
                    }
                    else if (paymentType.DefaultFunction == PaymentMethodDefaultFunctionEnum.Card)
                    {
                        var allCardIdsForStore = Providers.StoreCardTypesData.GetList(
                            PluginEntry.DataModel,
                            storeId,
                            paymentType.PaymentTypeID);

                        List<PaymentTransaction> transactions = paymentsForPaymentType.ToList();

                        foreach (var card in allCardIdsForStore)
                        {
                            var cardTransactions = PluginOperations.ResolveCardTenderID(card, paymentType.PaymentTypeID, transactions) ?? paymentsForPaymentType.Where(x => x.CardTypeID == card.CardTypeID);
                            var cardName = card.Text;

                            foreach (var cardPayment in cardTransactions)
                            {
                                amount += cardPayment.AmountTenderd;
                            }

                            terminalsPayments.PaymentTypes.Add(new PaymentMethodAmount
                            {
                                PaymentType = cardName,
                                Amount = amount
                            });
                            AddPaymentToPaymentTypeSum(cardName, amount, paymentMethodBreakdown);
                            amount = 0;
                        }
                    }
                    else if (paymentType.DefaultFunction == PaymentMethodDefaultFunctionEnum.FloatTender)
                    {
                        foreach (var payment in paymentsForPaymentType)
                        {
                            if (payment.TransactionType == TypeOfTransaction.RemoveTender)
                            {
                                amount += payment.AmountTenderd;
                                
                            }
                            else if (payment.TransactionType == TypeOfTransaction.FloatEntry)
                            {
                                amount += -1 * payment.AmountTenderd;                                
                            }
                        }
                        
                        terminalsPayments.PaymentTypes.Add(new PaymentMethodAmount
                        {
                            PaymentType = paymentType.Text,
                            Amount = amount
                        });
                        AddPaymentToPaymentTypeSum(paymentType.Text, amount, paymentMethodBreakdown);
                    }
                    else
                    {
                        foreach (var payment in paymentsForPaymentType)
                        {
                            amount += payment.AmountTenderd;
                        }
                        terminalsPayments.PaymentTypes.Add(new PaymentMethodAmount
                        {
                            PaymentType = paymentType.Text,
                            Amount = amount
                        });
                        AddPaymentToPaymentTypeSum(paymentType.Text, amount, paymentMethodBreakdown);
                    }
                }

                terminalPaymentBreakdown.Add(terminalsPayments);
            }

            PluginOperations.AddDataToReport(storeId, periodFrom, periodTo, terminalPaymentBreakdown, paymentMethodBreakdown);
        }

        private static void AddPaymentToPaymentTypeSum(
            RecordIdentifier paymentTypeId,
            decimal amount,
            List<PaymentMethodAmount> paymentSumList)
        {
            bool paymentAlreadyInSum = paymentSumList.Select(x => x.PaymentType).Contains((string)paymentTypeId);
            if (paymentAlreadyInSum)
            {
                paymentSumList.First(x => x.PaymentType == (string)paymentTypeId).Amount += amount;
            }
            else
            {
                paymentSumList.Add(new PaymentMethodAmount { PaymentType = (string)paymentTypeId, Amount = amount });
            }
        }


        private static List<TenderReportData> ConstructForExcel(
            List<TerminalPaymentMethods> terminalPaymentBreakdown,
            List<PaymentMethodAmount> paymentMethodBreakdown)
        {
            var data = new List<TenderReportData>();

            for (int tenderIndex = 0; tenderIndex < paymentMethodBreakdown.Count; tenderIndex++)
            {
                var tenderInfo = new TenderReportData();
                tenderInfo.TenderName = paymentMethodBreakdown[tenderIndex].PaymentType;
                tenderInfo.Values = new List<decimal>();

                for (int terminalIndex = 0; terminalIndex < terminalPaymentBreakdown.Count; terminalIndex++)
                {
                    decimal value = 0m;
                    var expectedPaymentType = paymentMethodBreakdown[tenderIndex].PaymentType;
                    var terminalBreakDown = terminalPaymentBreakdown[terminalIndex];
                    bool terminalHasExpectedPaymentType =
                        terminalBreakDown.PaymentTypes.Select(pt => pt.PaymentType).Contains(expectedPaymentType);
                    if (terminalHasExpectedPaymentType)
                    {
                        value = terminalBreakDown.PaymentTypes.First(pt => pt.PaymentType == expectedPaymentType).Amount;
                    }

                    tenderInfo.Values.Add(value);
                }
                data.Add(tenderInfo);
            }

            return data;
        }
        #endregion

        #region Excel formatting helpers

        private void SetGrayFormatted(IExcelService excelService, WorksheetHandle sheet, int row, int col, object value,
            int decimals = -1, CellHorizontalAlignment alignment = CellHorizontalAlignment.Left)
        {
            SetFormatted(excelService, sheet, row, col,
                Color.White, Color.DarkGray, CellFormatEnum.Bold, alignment,
                null, -1, value, decimals);
        }

        private void SetFormatted(IExcelService excelService, WorksheetHandle sheet, int row, int col,
            Color foreColor, Color backColor, CellFormatEnum format, CellHorizontalAlignment alignment,
            string fontName, short fontSize,
            object value, int decimals = -1)
        {
            var cell = new WorksheetCell
            {
                Value = value,
                Row = row,
                Column = col
            };

            SetCell(excelService, sheet, cell, foreColor, backColor, format, alignment, fontName, fontSize, decimals);
        }

        private void SetFormatted(IExcelService excelService, WorksheetHandle sheet, string cellReference,
            Color foreColor, Color backColor, CellFormatEnum format, CellHorizontalAlignment alignment,
            string fontName, short fontSize,
            object value, int decimals = -1)
        {
            var cell = new WorksheetCell
            {
                Value = value,
                CellReference = cellReference
            };

            SetCell(excelService, sheet, cell, foreColor, backColor, format, alignment, fontName, fontSize, decimals);
        }

        private void SetCell(IExcelService excelService, WorksheetHandle sheet, WorksheetCell cell,
            Color foreColor, Color backColor, CellFormatEnum format, CellHorizontalAlignment alignment,
            string fontName, short fontSize,
            int decimals = -1)
        {
            var options = new CellOptions();
            if (foreColor != Color.Empty)
                options.ForeColor = foreColor;
            if (backColor != Color.Empty)
                options.BackColor = backColor;
            if (format != CellFormatEnum.UnderlineSingleAccounting) // No .None, so used this one to indicate not set
                options.FormatEnum = format;
            if (!string.IsNullOrEmpty(fontName))
                options.FontName = fontName;
            if (fontSize > 0)
                options.FontSize = fontSize;
            if (alignment != CellHorizontalAlignment.Left)
                options.HorizontalAlignment = alignment;
            if (!string.IsNullOrEmpty(cell.CellReference))
                options.Merge = true;

            cell.Options = options;

            if (decimals >= 0)
            {
                string decimalFormat = "#,##0";
                if (decimals > 0)
                {
                    decimalFormat += ".";
                    decimalFormat += new String('0', decimals);
                }
                cell.Options.Format = decimalFormat;
            }

            excelService.SetCell(sheet, cell);
        }
        #endregion
    }
}
