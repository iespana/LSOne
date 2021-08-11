using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects.EOD;

namespace LSOne.DataLayer.TransactionDataProviders.EOD
{
    public interface IItemReportInfoData : IDataProviderBase<ItemSaleReportLine>
    {
        List<ItemSaleReportLine> GetItemSaleReportLines(IConnectionManager entry, DateTime dtFrom, DateTime dtTo, string storeId, string terminalId, SalesOrReturnsEnum salesOrReturns, ItemSaleReportGroupEnum printGroup);
    }
}
