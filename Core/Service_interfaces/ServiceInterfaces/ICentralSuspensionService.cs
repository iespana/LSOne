using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Delegates;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// Interface for central suspension operations. Implement this for suspending centrally, recalling transaction, updating suspended transactions and etc.
    /// </summary>
    public interface ICentralSuspensionService : IService
    {
        /// <summary>
        /// Test function that can be used when developing the interface to make sure that
        /// the connection has been made
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        void Test(IConnectionManager entry);


        /// <summary>
        /// Suspends the transaction to a central database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="session">The current session</param>
        /// <param name="settings">Application settings for the running application</param>
        /// <param name="retailTransaction">The current retail transaction</param>
        /// <param name="transactionTypeID">The type of suspension the transaction should be suspended as</param>
        /// <param name="msgHandler">A message function that the service uses to display any messages to the user</param>
        /// <param name="keyboardHandler">A message function that the service uses to display a keyboard for the user to enter any data</param>
        /// <param name="forecourtAllowsSuspension">When using forecourt the configuration for whether suspension is allowed is on the hardware profile.</param>
        /// <param name="cultureInfo">The current culture info settings</param>
        /// <returns>The unique ID of the supension</returns>
        RecordIdentifier SuspendTransaction(IConnectionManager entry, ISession session, ISettings settings, IRetailTransaction retailTransaction, RecordIdentifier transactionTypeID,
            ShowMessageHandler msgHandler, ShowKeyboardInputHandler keyboardHandler, bool forecourtAllowsSuspension, CultureInfo cultureInfo);

        /// <summary>
        /// Recalls the transaction from a central database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">Application settings for the running application</param>
        /// <param name="POSTransaction">The current retail transaction</param>
        /// <param name="transactionTypeID">The type of suspension the transaction should be suspended as</param>
        /// <param name="mainFormInfo">Information about the main form size, hight and etc.</param>
        void RecallTransaction(IConnectionManager entry, ISettings settings, IPosTransaction POSTransaction, RecordIdentifier transactionTypeID, MainFormInfo mainFormInfo);

        /// <summary>
        /// Retrieves a list of user input for a specific suspended transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">Application settings for the running application</param>
        /// <param name="transactionID">The unique ID of the suspended transaction</param>
        /// <param name="suspendedTransaction">The suspended transaction object that the answers belong to. The function can use information from this class when retrieving answers</param>
        /// <returns>
        /// An list of <see cref="SuspendedTransactionAnswer"/> with information about the user input done when transaction was
        /// suspende
        /// </returns>
        List<SuspendedTransactionAnswer> GetSuspendedTransactionAnswers(IConnectionManager entry,ISettings settings,RecordIdentifier transactionID, SuspendedTransaction suspendedTransaction);

        /// <summary>
        /// After the transaction has been selected the store and terminal information are updated as well as begin/end dates on the transaction, items and tenders.
        /// </summary>
        /// <param name="entry">The entry itno the database</param>
        /// <param name="settings">Application settings for the running application</param>
        /// <param name="retailTransaction">Selected transaction</param>
        /// <param name="SelectedTransaction">Information about the suspended
        /// transaction</param>
        /// <returns>
        /// Updated transaction
        /// </returns>
        IRetailTransaction UpdateRecalledTransaction(IConnectionManager entry, ISettings settings, IRetailTransaction retailTransaction, SuspendedTransaction SelectedTransaction);

        /// <summary>
        /// Returns the number of suspended transactions
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">Application settings for the running application</param>
        /// <param name="limitToTerminal">If true then the count is limited to the current terminal otherwise to the current store</param>
        /// <returns>
        /// The number of suspended transactions
        /// </returns>
        int GetSuspendedTransactionCount(IConnectionManager entry, ISettings settings, bool limitToTerminal);
    }

}
