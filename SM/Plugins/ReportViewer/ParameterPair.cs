using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Reports;

namespace LSRetail.SiteManager.Plugins.ReportViewer
{
    internal class ParameterPair
    {
        Dictionary<string, ProcedureParameter> parameters;

        public ParameterPair()
        {
            parameters = new Dictionary<string, ProcedureParameter>();       
        }

        public ProcedureParameter Primary { get; set; }
        public ProcedureParameter Secondary { get; set; }

        public string Name { get; set; }
        public string LabelText { get; set; }
        public int Priority { get; set; }

        public object ExtraData { get; set; }

        public ProcedureParameter this[string key]  
        {
            get
            {
                if (parameters.ContainsKey(key))
                {
                    return parameters[key];
                }

                return null;
            }
            set
            {
                parameters[key] = value;
            }
        }
    }
}
