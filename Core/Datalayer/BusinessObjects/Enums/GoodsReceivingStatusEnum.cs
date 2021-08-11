using System.Runtime.Serialization;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    [DataContract(Name = "GoodsReceivingStatusEnum")]
    public enum GoodsReceivingStatusEnum
    {
        [EnumMember]
        Active = 0,
        [EnumMember]
        Posted = 1
    }

    [DataContract(Name = "GoodsReceivingDocumentDeleteResult")]
    public enum GoodsReceivingDocumentDeleteResult
    {
        /// <summary>
        /// The goods receiving document was deleted
        /// </summary>
        [EnumMember]
        DocumentDeleted = 0,
        /// <summary>
        /// There are posted lines on the goods receiving document
        /// </summary>
        [EnumMember]
        HasPostedLines = 1
    }

    /// <summary>
    /// Operation result when posting a goods receiving document
    /// </summary>
    [DataContract(Name = "GoodsReceivingPostResult")]
    public enum GoodsReceivingPostResult
    {
        /// <summary>
        /// The goods receiving document was posted
        /// </summary>
        [EnumMember]
        Success = 0,
        /// <summary>
        /// Some items do not have a valid unit conversion rule
        /// </summary>
        [EnumMember]
        MissingUnitConversion = 1,
        /// <summary>
        /// Some items have an invalid receiving quantity (0 or higher than ordered quantity)
        /// </summary>
        [EnumMember]
        InvalidReceivingQuantity = 2,
        /// <summary>
        /// The goods receiving document was not found
        /// </summary>
        [EnumMember]
        NotFound = 3,
        /// <summary>
        /// There are no valid lines to post
        /// </summary>
        [EnumMember]
        NoLinesToPost = 4,
        /// <summary>
        /// The goods receiving document is already processing
        /// </summary>
        [EnumMember]
        AlreadyProcessing = 5,
        /// <summary>
        /// The goods receiving document was already posted
        /// </summary>
        [EnumMember]
        AlreadyPosted = 6,
        /// <summary>
        /// There was an error posting the goods receiving document
        /// </summary>
        [EnumMember]
        Error = 7
    }
}
