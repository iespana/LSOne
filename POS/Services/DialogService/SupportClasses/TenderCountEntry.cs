using LSOne.Utilities.DataTypes;

namespace LSOne.Services.SupportClasses
{
    /// <summary>
    /// Class that holds information about an entered tender count on the TenderCountDialog
    /// </summary>
    internal class TenderCountEntry
    {
        /// <summary>
        /// ID of the tender count entry (Can be a denomination, currency code or payment type)
        /// </summary>
        internal RecordIdentifier ID;

        /// <summary>
        /// Quantity of counted denominations (only used for cash)
        /// </summary>
        internal int Quantity;

        /// <summary>
        /// Value of counted tender
        /// </summary>
        internal decimal Value;

        /// <summary>
        /// Value of counted tender converted to the store currency
        /// </summary>
        internal decimal ValueInStoreCurrency;

        /// <summary>
        /// ID of the currency used for the value
        /// </summary>
        internal RecordIdentifier CurrencyCode;

        /// <summary>
        /// Description of the tender count
        /// </summary>
        internal string Description;

        /// <summary>
        /// Denomination amount for cash entries
        /// </summary>
        internal decimal DenominationAmount;

        /// <summary>
        /// Maximum decimal places allowed for this tender
        /// </summary>
        internal int MaxDecimals;

        /// <summary>
        /// ID of the POS operation
        /// </summary>
        internal int PosOperationID;

        /// <summary>
        /// ID of the tender
        /// </summary>
        internal string TenderID;

        /// <summary>
        /// Name of the tender
        /// </summary>
        internal string TenderName;

        /// <summary>
        /// True if this tender entry was successfuly counted
        /// </summary>
        internal bool Counted;

        internal TenderCountEntry(RecordIdentifier ID)
        {
            this.ID = ID;
            Quantity = 0;
            Value = 0;
            ValueInStoreCurrency = 0;
            CurrencyCode = RecordIdentifier.Empty;
            Description = "";
            MaxDecimals = 0;
            DenominationAmount = 0;
            TenderID = "";
            TenderName = "";
        }
    }
}
