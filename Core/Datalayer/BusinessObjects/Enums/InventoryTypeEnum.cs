using System.Runtime.Serialization;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    [DataContract(Name = "InventoryTypeEnum")]
    public enum InventoryTypeEnum
    {
        [EnumMember]
        Sale = 0,
        [EnumMember]
        Purchase = 1,
        [EnumMember]
        Adjustment = 2,
        [EnumMember]
        Transfer = 3,
        [EnumMember]
        Reservation = 4,
        [EnumMember]
        VoidedSale = 5,
        [EnumMember]
        TransferIn = 6,
        [EnumMember]
        TransferOut = 7,
        [EnumMember]
        StockCount = 8,
        [EnumMember]
        Parked = 9
    }

    /// <summary>
    /// Inventory journal and inventory journal line posted status.
    /// </summary>
    [DataContract(Name = "InventoryJournalStatus")]
    public enum InventoryJournalStatus
    {
        /// <summary>
        /// Inventory journal has no posted line(s). Inventory journal line is not posted.
        /// </summary>
        [EnumMember]
        Active = 0,
        /// <summary>
        /// Inventory journal has all line(s) posted. Inventory journal line is posted.
        /// </summary>
        [EnumMember]
        Posted = 1,
        /// <summary>
        /// Inventory journal has some line(s) posted. Parked(offline) inventory journal line is not fully posted.
        /// </summary>
        [EnumMember]
        PartialPosted = 2,
        /// <summary>
        /// Parked(offline) inventory journal lines were fully moved to main inventory.
        /// </summary>
        [EnumMember]
        Closed = 3
    }

    [DataContract(Name = "InventoryEnum")]
    public enum InventoryEnum
    {
        [EnumMember]
        PurchaseOrder,
        [EnumMember]
        StockCounting,
        [EnumMember]
        GoodsReceiving,
        [EnumMember]
        InventoryJournal,
        [EnumMember]
        StoreTransfer
    }

    [DataContract(Name = "InventoryActionEnum")]
    public enum InventoryActionEnum
    {
        [EnumMember]
        Manage,
        [EnumMember]
        New,
        [EnumMember]
        GenerateFromExisting,
        [EnumMember]
        GenerateFromTemplate,
        [EnumMember]
        GenerateFromFilter,
        [EnumMember]
        ExcelImport,
        [EnumMember]
        Other
    }

    [DataContract(Name = "CompressAdjustmentLinesResult")]
    public enum CompressAdjustmentLinesResult
    {
        [EnumMember]
        Success,
        [EnumMember]
        NotFound,
        [EnumMember]
        InvalidAdjustmentType,
        [EnumMember]
        Posted,
        [EnumMember]
        Processing,
        [EnumMember]
        ErrorCompressingLines
    }

    /// <summary>
    /// Current processing status of an inventory document
    /// </summary>
    [DataContract(Name = "InventoryProcessingStatus")]
    public enum InventoryProcessingStatus
    {
        /// <summary>
        /// Document is not processing
        /// </summary>
        [EnumMember]
        None,
        /// <summary>
        /// Document is compressing lines
        /// </summary>
        [EnumMember]
        Compressing,
        /// <summary>
        /// Document is posting
        /// </summary>
        [EnumMember]
        Posting,
        /// <summary>
        /// Document is importing new lines
        /// </summary>
        [EnumMember]
        Importing,
        /// <summary>
        /// Document is performing an unknown type of processing
        /// </summary>
        [EnumMember]
        Other
    }
}
