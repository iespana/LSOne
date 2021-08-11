using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.RMSMigration.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Model
{
    public class LookupManager : ILookupManager
    {
        public Dictionary<int, RecordIdentifier> CurrencyLookup
        {
            get; set;
        } = new Dictionary<int, RecordIdentifier>();

        public Dictionary<int, RecordIdentifier> StoreLookup
        {
            get; set;
        } = new Dictionary<int, RecordIdentifier>();

        public Dictionary<int, RecordIdentifier> TerminalLookup
        {
            get; set;
        } = new Dictionary<int, RecordIdentifier>();

        public Dictionary<int, RecordIdentifier> CustomerLookup
        {
            get; set;
        } = new Dictionary<int, RecordIdentifier>();
        public Dictionary<int, RecordIdentifier> VendorLookup
        {
            get; set;
        } = new Dictionary<int, RecordIdentifier>();

        public Dictionary<int, Tuple<RecordIdentifier, RecordIdentifier>> RetailDepartmentLookup { get; set; } = new Dictionary<int, Tuple<RecordIdentifier, RecordIdentifier>>();
        public Dictionary<int, Tuple<RecordIdentifier, RecordIdentifier>> RetailGroupLookup { get; set; } = new Dictionary<int, Tuple<RecordIdentifier, RecordIdentifier>>();

        public Dictionary<int, RecordIdentifier> SaleTaxLookup { get; set; } = new Dictionary<int, RecordIdentifier>();

        public Dictionary<int, RecordIdentifier> SaleGroupTaxLookup { get; set; } = new Dictionary<int, RecordIdentifier>();

        public Dictionary<int, RecordIdentifier> MatrixItemLookup { get; set; } = new Dictionary<int, RecordIdentifier>();

        public RecordIdentifier SetupBarcodeID { get; set; }

        public RecordIdentifier DefaultUnitOfMeasureID { get; set; }

        public Dictionary<string, RecordIdentifier> UnitOfMeasure { get; set; }

        public Dictionary<int, Tuple<RecordIdentifier, RecordIdentifier>> StandardItemsLookup { get; set; } = new Dictionary<int, Tuple<RecordIdentifier, RecordIdentifier>>();
        public Dictionary<int, Tuple<RecordIdentifier, RecordIdentifier>> VariantItemsLookup { get; set; } = new Dictionary<int, Tuple<RecordIdentifier, RecordIdentifier>>();

        public Dictionary<int, RecordIdentifier> PurchaseOrderHeaderLookup { get; set; } = new Dictionary<int, RecordIdentifier>();
        public Dictionary<string, RecordIdentifier> TransactionHeaderLookup { get; set; } = new Dictionary<string, RecordIdentifier>();

        public Dictionary<string, RecordIdentifier> PaymentMethodLookup { get; set; } = new Dictionary<string, RecordIdentifier>();

        public Dictionary<string, string> VendorItemLookup { get; set; } = new Dictionary<string, string>();

        public int SystemTax { get; set; } = 1;

        public string BaseImagePath { get; set; }
    }
}
