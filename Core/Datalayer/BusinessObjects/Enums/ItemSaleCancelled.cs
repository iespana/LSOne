namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// Details the reasons for the item sale not finishing
    /// </summary>
    public enum ItemSaleCancelledEnum    
    {
        /// <summary>
        /// Item sale finished normally. Is the default value and the trigger is not called if the operation finishes with this value.
        /// </summary>
        None,
        /// <summary>
        /// Item sale was cancelled through the PreSale Trigger
        /// </summary>
        PreTrigger,
        /// <summary>
        /// Item is configured to have mandatory keying in price but the user selected cancel on the Key in Price dialog.
        /// </summary>
        MustKeyInPrice,
        /// <summary>
        /// Price is zero and the item is configured not to allow that
        /// </summary>
        ZeroPriceNotValid,
        /// <summary>
        /// Item has been blocked for sale
        /// </summary>
        ItemIsBlocked,
        /// <summary>
        /// Item has not been activated for sale
        /// </summary>
        DateToActivate,
        /// <summary>
        /// The quantity of the item is 0, f.ex. scale item where the qty is not retrieved or Key in Qty dialog is cancelled.
        /// </summary>
        QtyIsZero,
        /// <summary>
        /// The user cancelled the dimension dialog
        /// </summary>
        DimensionCancelled,
        /// <summary>
        /// The user cancelled the entering of an item comment when the item is configured to require a comment
        /// </summary>
        ItemCommentCancelled,
        /// <summary>
        /// The user entered an empty string (spaces) for a comment when the item is configured to require a comment
        /// </summary>
        CommentIsEmpty,
        /// <summary>
        /// A pre item infocode (f.ex. Age limit) determines that the item cannot be sold
        /// </summary>
        PreItemInfocode,
        /// <summary>
        /// The sale is a return item sale and the value is more than the user is allowed to return
        /// </summary>
        MaxLineReturnAmt,
        /// <summary>
        /// PostSale trigger deleted all items on the sale
        /// </summary>
        PostSale,
        /// <summary>
        /// The sale is a return item sale and the total value returned including current sale is more than the user is allowed to reutn
        /// </summary>
        MaxTotalReturnAmt,
        /// <summary>
        /// Mixing of a regulare sale and return is not allowed
        /// </summary>
        MixingRegularSaleAndReturn,
        /// <summary>
        /// Store settings don't allow mixing regular sales and items that are automatically sold as returns in the same transaction.
        /// </summary>
        MixingRegularSaleAndReturnBecomesNegative,
        /// <summary>
        /// Item's validation period is invalid
        /// </summary>
        PeriodIsInvalid,
        /// <summary>
        /// The item was not found in the database
        /// </summary>
        ItemNotFound,
        /// <summary>
        /// The item is not returnable
        /// </summary>
        ItemNotReturnable,
        /// <summary>
        /// Operation aborted
        /// </summary>
        Aborted,
        /// <summary>
        /// Serial number for the item was already used.
        /// </summary>
        SerialNumberUsed,
        /// <summary>
        /// Item with this serial number already exists.
        /// </summary>
        DuplicateSerialNumber,
        /// <summary>
        /// Returned item does not have a reason code
        /// </summary>
        NoReasonCode,
        /// <summary>
        /// Item is configured to have mandatory keying in serial number but the user selected cancel the Key in Serial number dialog.
        /// </summary>
        MustKeyInSerialNumber,
        /// <summary>
        /// Item is configured so it can not be sold on the POS
        /// </summary>
        ItemCannotBeSold,
        /// <summary>
        /// The user cancelled the sale of an assembly item
        /// </summary>
        AssemblyCancelled,

    }
}
