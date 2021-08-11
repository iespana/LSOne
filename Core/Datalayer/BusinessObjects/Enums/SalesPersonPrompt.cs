
namespace LSOne.DataLayer.BusinessObjects.Enums
{
    public enum SalesPersonPrompt
    {
        /// <summary>
        /// No prompt for sales person
        /// </summary>
        None = 0,
        /// <summary>
        /// The user is prompted to select a sales person at the start of a sale
        /// </summary>
        StartOfSale = 1,
        /// <summary>
        /// The sales person is set to the current user, without prompt
        /// </summary>
        CurrentUser = 2
    }
}
