using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces.EFT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    /// <summary>
    /// A support class that handles cloning a <see cref="IPosTransaction"/>
    /// </summary>
    public interface ICloneTransactions
    {
        /// <summary>
        /// Clones the given transaction and returns the populated deep clone
        /// </summary>
        /// <param name="dataModel">The connection to the database</param>
        /// <param name="transactionToClone">The transaction to clone</param>
        /// <returns></returns>
        IPosTransaction CloneTransaction(IConnectionManager dataModel, IPosTransaction transactionToClone);

        /// <summary>
        /// Clones the PartnerObject if it is present on the transaction
        /// </summary>
        /// <param name="dataModel">The connection to the database</param>
        /// <param name="posTransaction">The transaction to populate</param>
        void ClonePartnerObject(IConnectionManager dataModel, IPosTransaction posTransaction);

        /// <summary>
        /// Clones any <see cref="IEFTExtraInfo"/> that might exist on <see cref="IEFTInfo"/>
        /// </summary>
        /// <param name="dataModel">The connection to the database</param>
        /// <param name="posTransaction">The transaction to populate</param>
        void CloneEFTExtraInfo(IConnectionManager dataModel, IPosTransaction posTransaction);

        /// <summary>
        /// Clones <see cref="IRetailTransaction.EFTTransactionExtraInfo"/> if it is present on the tranasction.
        /// </summary>
        /// <param name="dataModel">The connection to the database</param>
        /// <param name="posTransaction">The transaction that contains the instance of <see cref="IEFTTransactionExtraInfo"/></param>
        /// <remarks>This method will not do anything if <paramref name="posTransaction"/> is anything other than <see cref="IRetailTransaction"/>. The parameter is defined as <see cref="IPosTransaction"/> because when this is called the POS
        /// only has an <see cref="IPosTransaction"/> and it is up to the method to determine the exact type</remarks>
        void CloneEFTTransactionExtraInfo(IConnectionManager dataModel, IPosTransaction posTransaction);
    }
}
