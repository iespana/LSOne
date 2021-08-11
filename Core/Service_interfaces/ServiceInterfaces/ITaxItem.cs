namespace LSOne.Services.Interfaces
{
    public interface ITaxItem  
    {
        decimal Amount { get; set; }
        decimal Percentage { get; set; }
        decimal PriceWithTax { get; set; }
        string TaxGroup { get; set; }
        string TaxCode { get; set; }

        decimal TaxRoundOff { get; set; }

        int TaxRoundOffType { get; set; }
    }
}
