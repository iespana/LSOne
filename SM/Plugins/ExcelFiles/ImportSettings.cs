using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewPlugins.ExcelFiles.Enums;

namespace LSOne.ViewPlugins.ExcelFiles
{
    public class ImportSettings
    {
        public MergeModeEnum RetailItemImportStrategy {get; set;}
        public MergeModeEnum RetailGroupImportStrategy {get; set;}
        public MergeModeEnum CustomerImportStrategy {get; set;}
        public MergeModeEnum VendorImportStrategy { get; set; }
        public MergeModeEnum RetailDepartmentImportStrategy { get; set; }

        public bool CalculateProfitMargins { get; set; }

        public char DimensionsAttributesSeparator { get; set; }
    }
}
