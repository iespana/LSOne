
namespace LSOne.DataLayer.BusinessObjects.Reports
{
    /// <summary>
    /// Specifies the type of a report
    /// </summary>
    public enum ReportCategory
    {
        Customer = 0,
        Financial = 1,
        Generic = 2,
        Hospitality = 3,
        Inventory = 4,
        Item = 5,
        Sales = 6,
        Setup = 7,
        User = 8
    }

    public static class ReportCategoryExtensions
    {
        /// <summary>
        /// Get a string representation of the enum value based on the current culture
        /// </summary>
        /// <param name="reportCategory">Current enum</param>
        /// <returns></returns>
        public static string ToCultureString(this ReportCategory reportCategory)
        {
            switch (reportCategory)
            {
                case ReportCategory.Financial: return Properties.Resources.Financial;
                case ReportCategory.Generic: return Properties.Resources.Generic;
                case ReportCategory.Inventory: return Properties.Resources.Inventory;
                case ReportCategory.Sales: return Properties.Resources.Sales;
                case ReportCategory.Customer: return Properties.Resources.Customer;
                case ReportCategory.Hospitality: return Properties.Resources.Hospitality;
                case ReportCategory.Item: return Properties.Resources.Item;
                case ReportCategory.Setup: return Properties.Resources.Setup;
                case ReportCategory.User: return Properties.Resources.User;
                default: return "";
            }
        }
    }
}