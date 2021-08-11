using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Model
{
    public static class Enums
    {
        public enum RMSMigrationItemType
        {
            [Display(Name = "Currencies")]
            Currency = 1,
            [Display(Name = "Sales Tax")]
            SaleTax = 2,
            [Display(Name = "Customers")]
            Customer = 3,
            [Display(Name = "Items")]
            Item = 4,
            [Display(Name = "Vendors")]
            Vendor = 5,
            [Display(Name = "Users")]
            User = 6,
            [Display(Name = "Open purchase orders")]
            OpenPurchaseOrder = 7,
            [Display(Name = "Transactions")]
            Transaction = 8
        }

        public enum ProgressStatus
        {
            [Display(Name = "Ignored")]
            Ignored = 1,
            [Display(Name = "Ready")]
            Ready = 2,
            [Display(Name = "Importing...")]
            Importing = 3,
            [Display(Name = "Done")]
            Done = 4,
            [Display(Name = "Error")]
            Error = 5
        }
    }
}
