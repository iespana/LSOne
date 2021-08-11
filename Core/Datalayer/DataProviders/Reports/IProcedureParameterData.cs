using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.DataProviders.Reports
{
    public interface IProcedureParameterData : IDataProviderBase<ProcedureParameter>
    {
        List<ProcedureParameter> GetParameters(IConnectionManager entry, string procedureName);
        DataTable ExecuteReportQuery(IConnectionManager entry,string procedureName,List<ProcedureParameter> parameters);
    }
}