using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities;

namespace LSOne.ViewPlugins.EndOfDay.DataLayer
{
    public interface IReportData : IDataProviderBase<DataEntity>
    {
        Report GetReport(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType);
        DataTable GetTenderTable(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType);
        DataTable GetTenderDetailTable(IConnectionManager entry, RecordIdentifier paymentTypeId, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType);
        DataTable GetTaxData(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType);
        decimal GetNumberOfManuallyEnteredItems(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType);
        decimal GetNumberOfScannedItems(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType);
        int GetNumOfNegative(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType);
        int GetNumOfTransactions(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType);
        int GetNumberOfOpenedSales(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType);
        decimal GetNumberOfItemsSold(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType);
        decimal GetSumOfNegative(IConnectionManager entry, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType);
        List<HourlyDataLine> GetHourlyDataLines(IConnectionManager entry, bool includeReportFormatting, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType);
        List<CurrencyDataLine> GetCurrencyDataLines(IConnectionManager entry, bool includeReportFormatting, RecordIdentifier storeID, DateTime from, DateTime to, RecordIdentifier statementID, ReportIntervalType intervalType);
    }
}