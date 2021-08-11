using System;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportClasses.ImportExport;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.SaleByPaymentMediaReport.Views
{
    public partial class SaleByPaymentMediaReportView
    {
        private void AddCustomHeader(IExcelService excelService, WorksheetHandle sheet,
            ref int row, int dataColumns, DateFilterTypeEnum dateFilterType,
            IDataEntity store, DateTime periodFrom, DateTime periodTo)
        {
            // Time period
            SetGrayFormatted(excelService, sheet, row, 0, Properties.Resources.Store);
            excelService.SetCellValue(sheet, row, 1, (string)store.ID);

            row++;
            SetGrayFormatted(excelService, sheet, row, 0, Properties.Resources.DateFrom);
            excelService.SetCellValue(sheet, row, 1, periodFrom.ToShortDateString());

            row++;
            SetGrayFormatted(excelService, sheet, row, 0, Properties.Resources.DateTo);
            excelService.SetCellValue(sheet, row, 1, periodTo.ToShortDateString());
        }
    }
}
