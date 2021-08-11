using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces
{
    public interface ICreditMemoService : IService
    {
        /// <summary>
        /// Authorize payment with a credit memo
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="creditMemoId">Credit memo ID</param>
        /// <param name="amount">Amount to be paid</param>
        /// <param name="valid">True if the credit memo is valid</param>
        /// <param name="comment">Additional comment</param>
        /// <param name="posTransaction">Current transaction</param>
        /// <param name="tenderInfo">Payment method information</param>
        /// <param name="amountDue">Remaining amount to be paid</param>
        void AuthorizeCreditMemoPayment(IConnectionManager entry, ref bool valid, ref string comment, ref string creditMemoId, ref decimal amount, IPosTransaction posTransaction, StorePaymentMethod tenderInfo, decimal amountDue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="creditMemoItem"></param>
        /// <param name="retailTransaction"></param>
        void IssueCreditMemo(IConnectionManager entry, ICreditMemoTenderLineItem creditMemoItem, IPosTransaction retailTransaction);

        /// <summary>
        /// Handles results other than ValidationSuccess and displays the appropriate message if that applies
        /// </summary>
        /// <param name="cvEnum"></param>
        /// <param name="entry">The entry into the database</param>
        void HandleCreditVoucherValidationEnum(IConnectionManager entry, CreditVoucherValidationEnum cvEnum);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="validated"></param>
        /// <param name="comment"></param>
        /// <param name="amount"></param>
        /// <param name="creditMemoNumber"></param>
        /// <param name="retailTransaction"></param>
        void ValidateCreditMemo(IConnectionManager entry, ref bool validated, ref string comment, ref decimal amount, string creditMemoNumber, IRetailTransaction retailTransaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="creditMemoNumber"></param>
        /// <param name="amount"></param>
        /// <param name="posTransaction"></param>        
        void UpdateCreditMemo(IConnectionManager entry, string creditMemoNumber, decimal amount, IPosTransaction posTransaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="voided"></param>
        /// <param name="comment"></param>
        /// <param name="creditMemoNumber"></param>
        /// <param name="retailTransaction"></param>
        void VoidCreditMemoPayment(IConnectionManager entry, ref bool voided, ref string comment, string creditMemoNumber, IRetailTransaction retailTransaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="creditMemoNumber"></param>
        /// <param name="balance"></param>
        void GetCreditmemoBalance(IConnectionManager entry, string creditMemoNumber, ref decimal balance);

        /// <summary>
        /// Returns true if the Site service is needed to conclude a transaction that includes a credit memo
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction</param>
        /// <returns></returns>
        bool SiteServiceIsNeeded(IConnectionManager entry, IPosTransaction transaction);
    }
}
