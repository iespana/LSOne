using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Vouchers
{
    public interface ICreditVoucherLineData : IDataProviderBase<CreditVoucherLine>
    {
        /// <summary>
        /// Gets a list of credit voucher lines for a given crdit voucher.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="creditVoucherID">ID of the credit voucher to get credit voucher lines for</param>
        /// <returns>List of credit voucher lines for the given ID</returns>
        List<CreditVoucherLine> GetList(IConnectionManager entry, RecordIdentifier creditVoucherID);

        void Save(IConnectionManager entry, CreditVoucherLine item);
    }
}