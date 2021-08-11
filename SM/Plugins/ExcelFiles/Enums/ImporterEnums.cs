using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.ExcelFiles.Enums
{
    public enum MergeModeEnum
    {
        InsertIfNotExists = 0,
        Merge = 1,
        Override = 2,
        Ignore = 3
    }

    public enum ImportTypeEnum
    {
        Normal,
        Replenishment,
        StockCounting,
        SerialNumbers
    }
}
