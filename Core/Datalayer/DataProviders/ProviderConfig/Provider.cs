using System.Xml.Serialization;

namespace LSOne.DataLayer.DataProviders.ProviderConfig
{
    public class Provider
    {
        [XmlAttribute("assembly")]
        public string Assembly { get; set; }

        [XmlAttribute("interface")]
        public string Interface { get; set; }

        [XmlAttribute("implementation")]
        public string Implementation { get; set; }
    }
}
