using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces
{
    public interface ICashManagementService : IService
    {
        /// <summary>
        /// Perform a tender declaration of any type
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="posTransaction">Current transaction</param>
        /// <param name="tenderDeclarationType">Type of tender declaration</param>
        /// <param name="previousAmount">Amount that was previously declared in a tender declaration</param>
        /// <param name="amount">Amount that is declared in the currenct tender declaration</param>
        /// <param name="description">Description of the tender declaration</param>
        void TenderDeclaration(IConnectionManager entry, IPosTransaction posTransaction, TenderDeclarationType tenderDeclarationType, decimal previousAmount, decimal amount, string description);

        /// <summary>
        /// Get the amount that was declared at the start of day
        /// </summary>
        /// <param name="entry">Database connection</param>
        decimal GetDeclaredStartOfDayAmount(IConnectionManager entry);

        /// <summary>
        /// Get the last tender declaration amount
        /// </summary>
        /// <param name="entry">Database connection</param>
        decimal GetLastTenderDeclarationAmount(IConnectionManager entry);
    }
}
