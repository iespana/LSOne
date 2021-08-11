
using LSOne.DataLayer.BusinessObjects.Transactions.Line;

namespace LSOne.Services.Interfaces
{
    public interface IInfocodeLineItem : ILineItem
    {
        bool CreateTransactionEntries { get; set; }
        string InfocodeId { get; set; }
        string Prompt { get; set; }
        bool OncePerTransaction { get; set; }
        bool ValueIsAmountOrQuantity { get; set; }
        bool PrintPromptOnReceipt { get; set; }
        bool PrintInputOnReceipt { get; set; }
        bool PrintInputNameOnReceipt { get; set; }
        decimal MinimumValue { get; set; }
        decimal MaximumValue { get; set; }
        int MinimumLength { get; set; }
        int MaximumLength { get; set; }
        bool InputRequired { get; set; }
        string LinkedInfoCodeId { get; set; }
        decimal RandomFactor { get; set; }
        string Information { get; set; }
        string Information2 { get; set; }
        string Subcode { get; set; }
        string Variantcode { get; set; }
        string RefRelation { get; set; }
        string RefRelation2 { get; set; }
        string RefRelation3 { get; set; }
        decimal Amount { get; set; }
        int AdditionalCheck { get; set; }
        bool LinkItemLinesToTriggerLine { get; set; }
        string UnitOfMeasure { get; set; }
        string SalesTypeFilter { get; set; }


    }
}
