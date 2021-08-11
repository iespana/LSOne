using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Vouchers
{
    public interface ICreditVoucherData : IDataProvider<CreditVoucher>, ISequenceable
    {
        /// <summary>
        /// Searches for credit vouchers
        /// </summary>
        /// <param name="entry">Database entry</param>
        /// <param name="itemCount">Out parameter. Returns the total number of returned items</param>
        /// <param name="filter">Search filter</param>
        /// <returns>A list of credit vouchers</returns>
        List<CreditVoucher> Search(IConnectionManager entry, CreditVoucherFilter filter, out int itemCount);

        /// <summary>
        /// Adds a given amount to a credit voucher with a given id
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="creditVoucherID">ID of the credit voucher to add to</param>
        /// <param name="amount">The amount to add to the credit voucher</param>
        /// <param name="storeID">ID of the store where the operation is done or empty string if head office</param>
        /// <param name="terminalID">ID of the terminal where the operation is done or empty string if on store or headoffice level</param>
        /// <param name="userID">ID of the Site Manager user, or Guid.Empty if not avalible</param>
        /// <param name="staffID">ID of the POS staff or empty string if not avalible</param>
        /// <returns>Balance on the credit voucher after the transaction</returns>
        decimal AddToCreditVoucher(IConnectionManager entry, RecordIdentifier creditVoucherID, decimal amount, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier userID, RecordIdentifier staffID);

        CreditVoucher Get(IConnectionManager entry, RecordIdentifier voucherID);
    }
}