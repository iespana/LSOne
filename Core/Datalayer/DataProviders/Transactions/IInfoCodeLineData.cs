using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.DataProviders.Transactions
{
    public interface IInfoCodeLineData : IDataProviderBase<InfoCodeLineItem>
    {
        List<InfoCodeLineItem> GetInfoCodes(IConnectionManager entry, string refRelation, string refRelation2, 
            string refRelation3, InfoCodeLineItem.TableRefId refTableId);
        List<InfoCodeLineItem> GetInfoCodes(IConnectionManager entry, string infoCodeId);
    }
}