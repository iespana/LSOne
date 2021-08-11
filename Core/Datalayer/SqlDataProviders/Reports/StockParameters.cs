using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.SqlDataProviders.Reports
{
    public class StockParameters
    {
        private Dictionary<string, object> values;

        public StockParameters(IConnectionManager dataModel)
        {
            values = new Dictionary<string, object>();

            values.Add("@DATAAREAID", dataModel.Connection.DataAreaId);
        }

        public bool IsStockParameter(ProcedureParameter parameter)
        {
            return values.ContainsKey(parameter.Name.ToUpper());
        }

        public void SetValue(ProcedureParameter parameter)
        {
            if (values.ContainsKey(parameter.Name.ToUpper()))
            {
                parameter.Value = values[parameter.Name.ToUpper()];
                parameter.StockParameter = true;
            }
        }
    }
}
