using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LSOne.DataLayer.BusinessObjects.Reports
{
    [DataContract]
    public class ReportResult
    {
        public ReportResult()
        {
            DataSources = new List<DataSourceKeyValuePair>();
            ResultText = string.Empty;
        }

        [DataMember]
        public List<DataSourceKeyValuePair> DataSources { get; set; }

        [DataMember]
        public ReportResultStatusEnum ReportResultStatus { get; set; }

        [DataMember]
        public string ResultText { get; set; }
        
    }

   
}
