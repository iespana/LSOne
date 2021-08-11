namespace LSOne.DataLayer.BusinessObjects
{
    /// <summary>
    /// Represents a single tax group line in a financial report
    /// </summary>
    public class FinancialReportTaxGroupLine : DataEntity
    {
        public string SalesTaxGroupName { get; set; }
        public string SalesTaxGroupReceiptDisplay { get; set; }

        public decimal NetAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal GrossAmount { get; set; }

        public string FormattedNetAmount { get; set; }
        public string FormattedTaxAmount { get; set; }
        public string FormattedGrossAmount { get; set; }
    }
}
