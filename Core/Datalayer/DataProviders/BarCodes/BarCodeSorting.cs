using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.BarCodes
{
    /// <summary>
    /// A enum that defines sorting for the currencies
    /// </summary>
    public enum BarCodeSorting
    {
        /// <summary>
        /// Sort by Item bar code
        /// </summary>
        ItemBarCode,
        /// <summary>
        /// Sort by variant id
        /// </summary>
        VariantID,
        /// <summary>
        /// Sort by size id
        /// </summary>
        SizeID,
        /// <summary>
        /// Sort by color id
        /// </summary>
        ColorID,
        /// <summary>
        /// Sort by style id
        /// </summary>
        StyleID,
        /// <summary>
        /// Sort by bar code setup
        /// </summary>
        BarcodeSetupID,
        /// <summary>
        /// Sort by the show for item field
        /// </summary>
        ShowForItem,
        /// <summary>
        /// Sort by the use for input field
        /// </summary>
        UseForInput,
        /// <summary>
        /// Sort by the use for printing field
        /// </summary>
        UseForPrinting,
        /// <summary>
        /// Sorts by Unit description
        /// </summary>
        Unit,
        /// <summary>
        /// Sort by Item ID
        /// </summary>
        ItemID
    };
}