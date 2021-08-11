using System.Runtime.Serialization;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    [DataContract(Name = "PurchaseStatusEnum")]
    public enum PurchaseStatusEnum
    {
        [EnumMember]
        Open = 0,
        [EnumMember]
        Closed = 1,
        [EnumMember]
        PartiallyRecieved = 2,
        [EnumMember]
        Placed = 3
    }

    [DataContract(Name = "PurchaseOrderLinesDeleteResult")]
    public enum PurchaseOrderLinesDeleteResult
    {
        /// <summary>
        /// The purchase order line can be deleted
        /// </summary>
        [EnumMember]
        CanBeDeleted = 0,
        /// <summary>
        /// There are goods receiving lines attached to this purchase order line
        /// </summary>
        [EnumMember]
        GoodsReceivingLinesExist = 1
    }

    [DataContract(Name = "CreatePurchaseOrderResult")]
    public enum CreatePurchaseOrderResult
    {
        /// <summary>
        /// The creating of the purchase order was successful
        /// </summary>
        [EnumMember]
        Success,
        /// <summary>
        /// The purchase order cannot be found
        /// </summary>
        [EnumMember]
        PurchaseOrderNotFound,
        /// <summary>
        /// The inventory template cannot be found
        /// </summary>
        [EnumMember]
        TemplateNotFound,
        /// <summary>
        /// There was some error with creating the purchase orders
        /// </summary>
        [EnumMember]
        ErrorCreatingPurchaseOrder
    }

    [DataContract(Name = "PostPurchaseWorksheetResult")]
    public enum PostPurchaseWorksheetResult
    {
        /// <summary>
        /// The purchase worksheet was created succesfully
        /// </summary>
        [EnumMember]
        Success,
        /// <summary>
        /// The purchase worksheet contains no items to post
        /// </summary>
        [EnumMember]
        NoItems,
        /// <summary>
        /// Purchase worksheet could not be posted because it contains items without a vendor
        /// </summary>
        [EnumMember]
        NonVendorItems,
        /// <summary>
        /// The template associated to the purchase worksheet was not found
        /// </summary>
        [EnumMember]
        TemplateNotFound,
        /// <summary>
        /// There was an error posting the purchase worksheet
        /// </summary>
        [EnumMember]
        Error
    }
}
