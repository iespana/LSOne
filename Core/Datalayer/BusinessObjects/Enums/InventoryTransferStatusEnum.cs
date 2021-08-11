using System.Runtime.Serialization;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// The statuses the transfer order or request can have
    /// </summary>
    [DataContract(Name = "TransferOrderStatusEnum")]
    public enum TransferOrderStatusEnum
    {
        /// <summary>
        /// The transfer order/request is new
        /// </summary>
        [EnumMember]
        New,
        /// <summary>
        /// The transfer order/request has been sent
        /// </summary>
        [EnumMember]
        Sent,
        /// <summary>
        /// The transfer order/request has been recieved 
        /// </summary>
        [EnumMember]
        Received,
        /// <summary>
        /// The transfer order/request has been rejected
        /// </summary>
        [EnumMember]
        Rejected,
        /// <summary>
        /// The transfer order/request has been closed
        /// </summary>
        [EnumMember]
        Closed
    }

    /// <summary>
    /// The return value when the user tries to send a transfer order
    /// </summary>
    [DataContract(Name = "SendTransferOrderResult")]
    public enum SendTransferOrderResult
    {
        /// <summary>
        /// The sending of the transfer was successful
        /// </summary>
        [EnumMember]
        Success,
        /// <summary>
        /// The transfer order cannot be found
        /// </summary>
        [EnumMember]
        NotFound,
        /// <summary>
        /// The transfer order has already been sent and cannot be sent again
        /// </summary>
        [EnumMember]
        TransferAlreadySent,
        /// <summary>
        /// The transfer order has already been rejected
        /// </summary>
        [EnumMember]
        TransferOrderIsRejected,
        /// <summary>
        /// There are no items on the order so it cannot be sent
        /// </summary>
        [EnumMember]
        NoItemsOnTransfer,
        /// <summary>
        /// There are lines on the order that have 0 sent quantity
        /// </summary>
        [EnumMember]
        LinesHaveZeroSentQuantity,
        /// <summary>
        /// The order has already been sent and been viewed by the receiving store
        /// </summary>
        [EnumMember]
        FetchedByReceivingStore,
        /// <summary>
        /// One or more of the transfer order lines has units that do not have conversion rules between them
        /// </summary>
        [EnumMember]
        UnitConversionError,
        /// <summary>
        /// There was some error with sending the transfer order
        /// </summary>
        [EnumMember]
        ErrorSendingTransferOrder
    }

    /// <summary>
    /// The return value when the user tries save a transfer order line
    /// </summary>
    [DataContract(Name = "SaveTransferOrderLineResult")]
    public enum SaveTransferOrderLineResult
    {
        /// <summary>
        /// The transfer order line was successfuly saved
        /// </summary>
        [EnumMember]
        Success,
        /// <summary>
        /// The transfer order line cannot be found
        /// </summary>
        [EnumMember]
        NotFound,
        /// <summary>
        /// The transfer order has already been sent and cannot be edited
        /// </summary>
        [EnumMember]
        TransferOrderAlreadySent,
        /// <summary>
        /// The transfer order has already been rejected and cannot be edited
        /// </summary>
        [EnumMember]
        TransferOrderAlreadyRejected,
        /// <summary>
        /// There was some error with saving the transfer order line
        /// </summary>
        [EnumMember]
        ErrorSavingTransferOrderLine,
        /// <summary>
        /// The transfer order has already been received
        /// </summary>
        [EnumMember]
        TransferOrderAlreadyReceived
    }

    /// <summary>
    /// The return value when the user tries to receive a transfer order
    /// </summary>
    [DataContract(Name = "ReceiveTransferOrderResult")]
    public enum ReceiveTransferOrderResult
    {
        /// <summary>
        /// The sending of the transfer was successful
        /// </summary>
        [EnumMember]
        Success,
        /// <summary>
        /// The transfer order cannot be found
        /// </summary>
        [EnumMember]
        NotFound,
        /// <summary>
        /// Transfer order already received
        /// </summary>
        [EnumMember]
        Received,
        /// <summary>
        /// There are no items on the order so it cannot be received
        /// </summary>
        [EnumMember]
        NoItemsOnTransfer,
        /// <summary>
        /// One or more of the transfer order lines has units that do not have conversion rules between them
        /// </summary>
        [EnumMember]
        UnitConversionError,
        /// <summary>
        /// One or more of the transfer order lines doesn't have qty sent = qty received
        /// </summary>
        [EnumMember]
        QuantitiesReceivedNotAccurate,
        /// <summary>
        /// One or more of the transfer order lines doesn't have qty sent = qty received when receiving in SAP
        /// </summary>
        [EnumMember]
        SAPQuantitiesReceivedNotAccurate,
        /// <summary>
        /// There was some error with sending the transfer order
        /// </summary>
        [EnumMember]
        ErrorReceivingTransferOrder
    }

    /// <summary>
    /// Result value when the user tries to auto set the quantity on a receiving transfer order
    /// </summary>
    [DataContract(Name = "AutoSetQuantityResult")]
    public enum AutoSetQuantityResult
    {
        [EnumMember]
        Success,
        [EnumMember]
        NotFound,
        [EnumMember]
        AlreadyReceived,
        [EnumMember]
        ErrorAutoSettingQuantity
    }

    [DataContract(Name = "CreateTransferOrderResult")]
    public enum CreateTransferOrderResult
    {
        /// <summary>
        /// The creating of the transfer order was successful
        /// </summary>
        [EnumMember]
        Success,
        /// <summary>
        /// The transfer order cannot be found
        /// </summary>
        [EnumMember]
        OrderNotFound,
        /// <summary>
        /// The inventory template cannot be found
        /// </summary>
        [EnumMember]
        TemplateNotFound,
        /// <summary>
        /// The transfer request cannot be found
        /// </summary>
        [EnumMember]
        RequestNotFound,
        /// <summary>
        /// The information for the order header isn't sufficient to create a transfer order
        /// </summary>
        [EnumMember]
        HeaderInformationInsufficient,
        /// <summary>
        /// The order header was created but no lines were created/copied
        /// </summary>
        [EnumMember]
        NoLinesCreated,
        /// <summary>
        /// The order header was created but not all the lines were created/copied
        /// </summary>
        [EnumMember]
        NotAllLinesCreated,
        /// <summary>
        /// There was some error with creating the transfer orders
        /// </summary>
        [EnumMember]
        ErrorCreatingTransferOrder
    }

    [DataContract(Name = "DeleteTransferResult")]
    public enum DeleteTransferResult
    {
        /// <summary>
        /// Store transfer deleted succesfully
        /// </summary>
        [EnumMember]
        Success,
        /// <summary>
        /// Store transfer not found
        /// </summary>
        [EnumMember]
        NotFound,
        /// <summary>
        /// Store transfer already sent
        /// </summary>
        [EnumMember]
        Sent,
        /// <summary>
        /// Store transfer already fetched
        /// </summary>
        [EnumMember]
        FetchedByReceivingStore,
        /// <summary>
        /// Store transfer already received
        /// </summary>
        [EnumMember]
        Received,
        /// <summary>
        /// Error deleting store transfer
        /// </summary>
        [EnumMember]
        ErrorDeletingTransfer
    }
}
