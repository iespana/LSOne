using System;
using System.Xml.Serialization;

namespace LSOne.Services.Interfaces.Configurations
{
    public class SiteServiceSettings
    {
        //[XmlElement("SiteServiceHost")]
        //public string SiteServiceHost { get; set; }

        //[XmlElement("SiteServicePort")]
        //public UInt16 SiteServicePort { get; set; }

        [XmlElement("ClientID")]
        public Guid ClientID { get; set; }

        [XmlElement("PassCode")]
        public string PassCode { get; set; }

        [XmlElement("UserGuid")]
        public Guid UserGuid { get; set; }

        [XmlElement("TerminalClaimed")]
        public bool TerminalClaimed { get; set; }

        [XmlElement("Login")]
        public string Login { get; set; }
       
    }
}
