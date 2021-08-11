using System.Runtime.Serialization;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// The status of a stock counting
    /// THIS DOES NOT SEEM TO BE USED ANYWHERE
    /// </summary>
    [DataContract(Name = "StockCountingStatusEnum")]
    public enum StockCountingStatusEnum
    {
        /// <summary>
        /// The stock counting is not posted
        /// </summary>
        [EnumMember]
        Unposted,
        /// <summary>
        /// The stock counting is posted
        /// </summary>
        [EnumMember]
        Posted,
        /// <summary>
        /// All
        /// </summary>
        [EnumMember]
        All
    }

    /// <summary>
    /// The status received when creating a new stock counting journal
    /// </summary>
    [DataContract(Name = "CreateStockCountingResult")]
    public enum CreateStockCountingResult
    {
        /// <summary>
        /// The journal was successfully created
        /// </summary>
        [EnumMember]
        Success,
        /// <summary>
        /// The journal to copy from was not found
        /// </summary>
        [EnumMember]
        JournalToCopyNotFound,
        /// <summary>
        /// The template from which to create the journal was not found
        /// </summary>
        [EnumMember]
        TemplateNotFound,
        /// <summary>
        /// The journal header was created but no lines were created/copied
        /// </summary>
        [EnumMember]
        NoLinesCreated,
        /// <summary>
        /// The journal header was created but not all the lines were created/copied
        /// </summary>
        [EnumMember]
        NotAllLinesCreated,
        /// <summary>
        /// There was some error creating the journal
        /// </summary>
        [EnumMember]
        ErrorCreatingStockCounting
    }

    /// <summary>
    /// The results values that the site service can send after doing a posting of a stock counting journal
    /// </summary>
    [DataContract(Name = "PostStockCountingResult")]
    public enum PostStockCountingResult
    {
        /// <summary>
        /// The posting of all stock counting lines was successful
        /// </summary>
        [EnumMember]
        Success,
        /// <summary>
        /// The stock counting journal was not found
        /// </summary>
        [EnumMember]
        JournalNotFound,
        /// <summary>
        /// Journal isn't a stock counting journal
        /// </summary>
        [EnumMember]
        InvalidAdjustmentType,
        /// <summary>
        /// The stock counting journal has already been posted
        /// </summary>
        [EnumMember]
        JournalAlreadyPosted,
        /// <summary>
        /// An error occured when aggregating the item lines in the stock counting journal
        /// </summary>
        [EnumMember]
        ErrorCompressingJournalLines,
        /// <summary>
        /// There was some error with posting the stock counting journal
        /// </summary>
        [EnumMember]
        ErrorPostingJournal,
        /// <summary>
        /// Unable to post stock counting lines because the method does not support posting lines from multiple stock counting journals at once
        /// </summary>
        [EnumMember]
        ErrorPostingLinesDueToMixingJournals,
        /// <summary>
        /// Unable to post stock counting lines because there was nothing to post
        /// </summary>
        [EnumMember]
        ErrorPostingLinesDueToEmptyList,
        /// <summary>
        /// Unable to post stock counting journal because there are still unposted lines
        /// </summary>
        [EnumMember]
        ErrorPostingJournalDueToUnpostedLines,
        /// <summary>
        /// Journal is in some form of processing
        /// </summary>
        [EnumMember]
        JournalCurrentlyProcessing
    }

    [DataContract(Name = "JournalTransStatusEnum")]
    public enum JournalTransStatusEnum
    {
        /// <summary>
        /// The journal  line was not calculated i.e. onhand inventory or adjustment value when it was saved
        /// </summary>
        [EnumMember]
        NotCalculated,
        /// <summary>
        /// Onhand inventory for the journal line has been calculated
        /// </summary>
        [EnumMember]
        OnHandCalculated,
        /// <summary>
        /// Adjustment value has been calculated
        /// </summary>
        [EnumMember]
        AdjustmentCalculated
    }

    [DataContract(Name = "DeleteJournalTransactionsResult")]
    public enum DeleteJournalTransactionsResult
    {
        /// <summary>
        /// The deletion of journal transactions was successful
        /// </summary>
        [EnumMember]
        Success,
        /// <summary>
        /// Unable to delete transactions because the method does not support deletion of lines belonging to multiple journals at once
        /// </summary>
        [EnumMember]
        ErrorPostingLinesDueToMixingJournals,
    }

    [DataContract(Name = "DeleteJournalResult")]
    public enum DeleteJournalResult
    {
        /// <summary>
        /// The deletion of journal was successful
        /// </summary>
        [EnumMember]
        Success,
        /// <summary>
        /// Unable to delete the journal because it is partially posted
        /// </summary>
        [EnumMember]
        PartiallyPosted,
    }
}