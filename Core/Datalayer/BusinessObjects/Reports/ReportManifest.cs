using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Reports
{
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class ReportManifest
    {
        public ReportManifest()
        {
            DataSourceNames = new List<string>();
            UserInputParameters = new List<ProcedureParameter>();
            Parameters = new List<string>();
        }
        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier ReportGuid { get; set; }
        [DataMember]
        public List<ProcedureParameter> UserInputParameters { get; set; }
        [DataMember]
        public List<string> Parameters { get; set; }
        [DataMember]
        public List<string> DataSourceNames { get; set; }  

    }
    
}
