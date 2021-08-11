using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Financials
{
    public interface IFinancialReportData : IDataProviderBase<FinancialReportTaxGroupLine>
    {
        List<FinancialReportTaxGroupLine> GetTaxGroupLines(IConnectionManager entry, RecordIdentifier storeID, DateTime fromDate, DateTime toDate);
    }
}