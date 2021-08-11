using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Delegates;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces
{
    public interface ICashChangerService : IService
    {
        event InsertedAmountDelegate InsertedAmount;
        event LevelStatusDelegate LevelStatusEvent;
        event ErrorEventDelegate ErrorEvent;

        /// <summary>
        /// Used to create an instance of the ActiveX API and carry out the
        /// initialize funtion.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>Returns true if init function returns OK</returns>
        bool Initialize(IConnectionManager entry);

        /// <summary>
        /// The login function is used to register the user with the Cash guard machine
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="terminalID"></param>
        /// <param name="operatorID"></param>
        /// <returns>Returns whether the function was successfully completed</returns>
        bool Login(IConnectionManager entry, string terminalID, string operatorID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        bool Logout(IConnectionManager entry);

        /// <summary>
        /// This functions registers in to the transaction the amount that has been entered
        /// into the cash machine.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="amountDue">The amount that should be paid by the customer</param>
        /// <param name="receiptID">The receipt ID of the transaction</param>
        /// <param name="amountRest">If not enough amount has been entered, this parameter will specify what is left to pay</param>
        /// <returns>An instance of the CashChangerReturn enum specifying the </returns>
        CashGuardReturn RegisterAmount(IConnectionManager entry, decimal amountDue, string receiptID, ref decimal amountRest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        bool Change(IConnectionManager entry);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        bool Reset(IConnectionManager entry);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="regretType"></param>
        /// <returns></returns>
        bool Regret(IConnectionManager entry, CashGuardRegretType regretType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        bool Exit(IConnectionManager entry);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        bool GetAutoMode(IConnectionManager entry);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="mode"></param>
        void SetAutoMode(IConnectionManager entry, bool mode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        void ConcludeTransaction(IConnectionManager entry, IPosTransaction posTransaction);
    }
}
