using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Reports
{
    [DataContract]
    public class DataSourceKeyValuePair
    {
        public DataSourceKeyValuePair()
        {
            Key = string.Empty;
            Value = string.Empty;
        }
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string Value { get; set; }
    }
}
