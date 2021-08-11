using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Integrations;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.DataProviders.Integrations
{
    public interface IIntegrationLogData : IDataProviderBase<IntegrationLog>
    {
        void Clear(IConnectionManager entry, DateTime stampDate);

        List<IntegrationLog> Search(IConnectionManager entry, 
            int rowFrom, int rowTo, 
            DateTime dateFrom, DateTime dateTo);

        int Count(IConnectionManager entry);

        void Save(IConnectionManager entry, IntegrationLog log);
    }
}