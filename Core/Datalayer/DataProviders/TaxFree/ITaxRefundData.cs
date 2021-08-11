using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;

namespace LSOne.DataLayer.DataProviders.TaxFree
{
    public interface ITaxRefundData : IDataProvider<TaxRefund>
    {
        TaxRefund Get(IConnectionManager entry, RecordIdentifier id);
        List<TaxRefund> GetForTourist(IConnectionManager entry, RecordIdentifier id);
        bool RunningNumberExists(IConnectionManager entry, string bookingNumber, string runningNumber);
        TaxRefund GetFromRunningNumber(IConnectionManager entry, string bookingNumber, string runningNumber);
    }
}
