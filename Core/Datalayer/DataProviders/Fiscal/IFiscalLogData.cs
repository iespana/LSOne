using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Fiscal;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Fiscal
{
    public interface IFiscalLogData : IDataProvider<FiscalLogEntity>
    {
        List<FiscalLogEntity> GetList(IConnectionManager entry, DateTime fromDate, DateTime toDate);
        string[] GetTransactionData(DateTime fromDate, DateTime toDate);
    }
}
