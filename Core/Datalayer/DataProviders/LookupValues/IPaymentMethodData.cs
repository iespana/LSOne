using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.LookupValues
{
    public interface IPaymentMethodData : IDataProvider<PaymentMethod>, ISequenceable
    {
        /// <summary>
        /// Returns the corresponding default function intended for the given posoperation
        /// </summary>
        /// <param name="id">The ID of the POSOperation</param>
        /// <returns></returns>
        PaymentMethodDefaultFunctionEnum GetDefaultFunctionFromPOSOperation(RecordIdentifier id);

        List<PaymentMethod> GetList(IConnectionManager entry);
        RecordIdentifier GetAddRemoveFloatTenderID(IConnectionManager entry, RecordIdentifier storeID);
        List<PaymentMethod> GetListForFunction(IConnectionManager entry, RecordIdentifier storeID, PaymentMethodDefaultFunctionEnum function);
        bool InUse(IConnectionManager entry, RecordIdentifier tenderTypeID);

        PaymentMethod Get(IConnectionManager entry, RecordIdentifier paymentMethodID);

        /// <summary>
        /// Gets count of payment methods
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>Count of payment methods</returns>
        int GetPaymentMethodCount(IConnectionManager entry);

        /// <summary>
        /// Set a payment type as local currency. This action will delete all limitations set on the payment type
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="paymentTypeID">ID of the payment type</param>
        void SetAsLocalCurrency(IConnectionManager entry, RecordIdentifier paymentTypeID);
    }
}