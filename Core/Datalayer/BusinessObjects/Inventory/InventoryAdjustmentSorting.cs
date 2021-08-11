using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    /// <summary>
    /// A enum that defines sorting for the InventoryJournalData rows
    /// </summary>
    public enum InventoryAdjustmentSorting
    {
        ID,
        Description,
        StoreName,
        Posted,
        PostingDate,
        DeletePostedLines,
        CreatedDateTime,
        ProcessingStatus,
        None
    };
}