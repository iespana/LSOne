using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Auditing;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders.Auditing
{
    public interface IOperationAuditingData : IDataProviderBase<OperationAuditing>
    {
        List<OperationAuditing> GetList(IConnectionManager entry, RecordIdentifier storeID);
        List<OperationAuditing> Search(IConnectionManager entry, 
                                       RecordIdentifier store, 
                                       RecordIdentifier terminal, 
                                       RecordIdentifier operatorID, 
                                       DateTime fromDate, 
                                       DateTime toDate, 
                                       List<RecordIdentifier> operations, 
                                       int recordFrom, 
                                       int recordTo);
        bool Exists(IConnectionManager entry, OperationAuditing item);
        void Save(IConnectionManager entry, OperationAuditing item);
    }
}