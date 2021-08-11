
namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// Type of tender declaration
    /// </summary>
    public enum TenderDeclarationType
    {
        /// <summary>
        /// Declare the start amount of cash in a POS
        /// </summary>
        DeclareStartAmount,
        /// <summary>
        /// Add cash into the POS
        /// </summary>
        Float,
        /// <summary>
        /// Remove cash from the POS
        /// </summary>
        TenderRemoval
    }
}
