namespace LSOne.DataLayer.TransactionObjects.EOD
{
    /// <summary>
    /// Declares the class NoSaleInfo. A linked list of that type is created in the constructor.
    /// </summary>
    public class NoSaleInfo
    {
        /// <summary>
        /// For text that shall appear in the Report but is not relevant to a certain sales operation.
        /// </summary>
        public string NosaleText = "";

        /// <summary>
        /// For amounts that shall appear in the Report but is not relevant to a certain sales operation.
        /// </summary>
        public decimal NoSaleAmount = decimal.Zero;
    }
}
