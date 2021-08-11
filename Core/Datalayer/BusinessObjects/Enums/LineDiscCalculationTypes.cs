namespace LSOne.DataLayer.BusinessObjects.Enums
{
   /// <summary>
    /// How is the discount calculated when multiple discounts are found.
    /// In Axapta this is found in the account receivable parameters under the price section
    /// </summary>
    public enum LineDiscCalculationTypes
    {
        /// <summary>
        /// 0 = Discount always equal to the line discount 
        /// </summary>
        Line = 0,
        /// <summary>
        /// 1= Discount always equal to the multiline discount
        /// </summary>
        MultiLine = 1,
        /// <summary>
        /// 2 = Discount is always the max discount of line and multiline discount
        /// </summary>
        MaxLineMultiLine = 2,
        /// <summary>
        /// 3 = Discount is always the min discount of line and multiline discount
        /// </summary>
        MinLineMultiLine = 3,
        /// <summary>
        /// 4 = Discount is the sum of the line and multiline discount
        /// </summary>
        LinePlusMultiLine = 4,
        /// <summary>
        /// 5 = The same as 4
        /// </summary>
        LineMultiplyMultiLine = 5
    }
}
