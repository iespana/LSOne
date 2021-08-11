using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.ExcelFiles.Enums
{
    public enum ExportErrorCodes
    {
        NoError,
        CouldNotGenerateIdFromHQ,
        CouldNotOpenWorkbook,
        CouldNotFindWorkSheet,
        CouldNotSaveFile
    }
}
