using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.EFT
{
    /// <summary>
    /// Payment information for EFT transactions
    /// </summary>
    public class PaymentInfo
    {
        /// <summary>
        /// The total amount of the transaction
        /// </summary>
        public decimal TotalTransactionAmount { get; set; }

        /// <summary>
        /// The current balance amount of the transaction
        /// </summary>
        public decimal BalanceAmount { get; set; }

        /// <summary>
        /// Gratuity amount, if any
        /// </summary>
        public decimal GratuityAmount { get; set; }

        /// <summary>
        /// The restriction amount
        /// </summary>
        public decimal RestrictedAmount { get; set; }

        /// <summary>
        /// An unspecific parameter that can be used in customization
        /// </summary>
        public object Parameter { get; set; }
    }
}
