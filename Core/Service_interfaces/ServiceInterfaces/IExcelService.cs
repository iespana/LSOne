using System.Data;
using System.Drawing;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportClasses.ImportExport;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.IO;

namespace LSOne.Services.Interfaces
{
    public interface IExcelService : IService
    {
        DataSet ImportFromExcel(IConnectionManager entry, FolderItem file, bool includeLineNumber = false);

        WorkbookHandle CreateWorkbook(FolderItem file, string sheetName);
        WorksheetHandle AddWorksheet(WorkbookHandle handle, string name);

        WorkbookHandle OpenStream(System.IO.Stream stream, bool isXLSX, string password = null);

        WorkbookHandle OpenForEditing(FolderItem file, string password = null);
        void Save(WorkbookHandle handle, FolderItem destination, string password = null);

        void SetWorksheetOptions(WorksheetHandle handle, WorksheeetOptions options);
        void SetCell(WorksheetHandle handle, WorksheetCell cell);

        WorksheetHandle GetWorksheet(WorkbookHandle handle, string worksSheetName);
        WorksheetHandle GetWorksheet(WorkbookHandle handle, int worksheetIndex, string renameTo = null);

        void SetCellValue(WorksheetHandle handle, int rowIndex, int columnIndex, object value);
        void SetCellValue(WorksheetHandle handle, int rowIndex, int columnIndex, decimal value, DecimalLimit limiter);
        void SetCellValue(WorksheetHandle handle, int rowIndex, int columnIndex, object value, ExcelStandardFormats format);

        void AddImage(WorksheetHandle handle, int rowIndex, int columnIndex, FolderItem imageFile);

        int[] GetColumnMapping(WorksheetHandle handle, string[] columns);
    }
}
