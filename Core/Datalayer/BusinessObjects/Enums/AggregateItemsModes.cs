namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// The aggregation modes available
    /// </summary>
    public enum AggregateItemsModes
    {
        /// <summary>
        /// 0 = Items are not aggregated
        /// </summary>
        None = 0,
        /// <summary>
        /// 1 = Items are are aggregated if they are entered one after another
        /// </summary>
        Normal = 1
        ///// <summary>
        ///// 2 = Items are aggregated throughout the whole transaction.
        ///// </summary>
        //Full = 2,
        ///// <summary>
        ///// 3 = Items are aggregated by barcodes
        ///// </summary>
        //Barcode = 3
    }
}
