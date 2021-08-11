using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Contracts
{
    public interface ILookupManager
    {
        Dictionary<int, RecordIdentifier> StoreLookup { get; set; }
        Dictionary<int, RecordIdentifier> TerminalLookup { get; set; }
        Dictionary<int, RecordIdentifier> CurrencyLookup { get; set; }
        Dictionary<int, RecordIdentifier> CustomerLookup { get; set; }
        Dictionary<int, RecordIdentifier> VendorLookup { get; set; }
        Dictionary<int, Tuple<RecordIdentifier, RecordIdentifier>> RetailDepartmentLookup { get; set; }
        Dictionary<int, Tuple<RecordIdentifier, RecordIdentifier>> RetailGroupLookup { get; set; }

        Dictionary<int, RecordIdentifier> SaleTaxLookup { get; set; }
        Dictionary<int, RecordIdentifier> SaleGroupTaxLookup { get; set; }
        Dictionary<int, RecordIdentifier> MatrixItemLookup { get; set; }

        RecordIdentifier SetupBarcodeID { get; set; }

        RecordIdentifier DefaultUnitOfMeasureID { get; set; }

        Dictionary<string, RecordIdentifier> UnitOfMeasure { get; set; }

        Dictionary<int, Tuple<RecordIdentifier, RecordIdentifier>> StandardItemsLookup { get; set; }
        Dictionary<int, Tuple<RecordIdentifier, RecordIdentifier>> VariantItemsLookup { get; set; }
        Dictionary<int, RecordIdentifier> PurchaseOrderHeaderLookup { get; set; }
        Dictionary<string, RecordIdentifier> TransactionHeaderLookup { get; set; }
        Dictionary<string, RecordIdentifier> PaymentMethodLookup { get; set; }

        Dictionary<string, string> VendorItemLookup { get; set; }
        int SystemTax { get; set; }

        string BaseImagePath { get; set; }
    }
}
