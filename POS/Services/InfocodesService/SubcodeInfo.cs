namespace LSOne.Services
{
    #region Enums
    public enum TriggerFunctionEnum
    {
        None = 0,
        /// <summary>
        /// Adds an item to the transaction
        /// </summary>
        Item = 1,
        /// <summary>
        /// Calls another infocode
        /// </summary>
        Infocode = 5
    }
    public enum PriceTypeEnum
    {
        None = 0,
        /// <summary>
        /// Takes the price from the item card
        /// </summary>
        FromItem = 1,
        /// <summary>
        /// Sets the price from variable AmountPercent
        /// </summary>
        Price = 2,
        /// <summary>
        /// Adds a discount from variable AmountPercent
        /// </summary>
        Percent = 3
    }

    #endregion   

    public class PopupPrimaryKey
    {
        public string InfocodeId { get; set; }
        public string SubCodeId { get; set; }

        public PopupPrimaryKey()
        {
            InfocodeId = "";
            SubCodeId = "";
        }
    }
 }

