using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Hospitality
{
    public interface ITableInfoData : IDataProviderBase<TableInfo>
    {
        List<TableInfo> GetListForTerminal(IConnectionManager entry, RecordIdentifier terminalID, RecordIdentifier storeID);
        List<TableInfo> GetList(IConnectionManager entry, DiningTableLayout tableLayout);
        TableInfo RefreshTableInfo(IConnectionManager entry, TableInfo table);
        bool Exists(IConnectionManager entry, RecordIdentifier resturantID, RecordIdentifier salesType, RecordIdentifier sequence, int dineInTableNumber, RecordIdentifier dineInTableLayoutID);

        void Save(IConnectionManager entry, TableInfo diningTable);

        void SaveUnlockedTransaction(IConnectionManager entry, Guid transactionID);
        bool ExistsUnlockedTransaction(IConnectionManager entry, Guid transactionID);
    }
}