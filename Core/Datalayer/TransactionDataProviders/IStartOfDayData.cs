using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface IStartOfDayData : IDataProvider<RecordIdentifier>
    {
        bool FloatRequired(IConnectionManager entry);
        DateTime? GetBusinessDay();
        void SaveBusinessDay(DateTime? day);
        decimal GetFloatsFromLastTenderDeclaration(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier tenderTypeID);
        decimal GetStartOfDayAmount(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier tenderTypeID);
        void SaveBusinessSystemDay(DateTime? day);
        DateTime? GetBusinessSystemDay();
    }
}
