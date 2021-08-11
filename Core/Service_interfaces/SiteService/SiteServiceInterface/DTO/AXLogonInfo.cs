using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace LSRetail.SiteService.SiteServiceInterface.DTO
{
    
    public enum AXVersions
    {
        AXAPTA3 = 0,
        AXAPTA4 = 1,
        AXAPTA2009 = 2
    }

    public class AXLogonInfo
    {
        // Logon properties
        [DataMember]
        public string userName { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember]
        public string company { get; set; }
        [DataMember]
        public string domain { get; set; }
        [DataMember]
        public string objectServer { get; set; }
        [DataMember]
        public string configuration { get; set; }
        [DataMember]
        public string language { get; set; }
    }
}
