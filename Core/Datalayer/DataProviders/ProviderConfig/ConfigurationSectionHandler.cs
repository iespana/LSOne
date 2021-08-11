using System.Xml.Serialization;

namespace LSOne.DataLayer.DataProviders.ProviderConfig
{
    public class ConfigurationSectionHandler : BaseSerializedSectionHandler
    {
        protected override XmlSerializer Serializer
        {
            get 
            {
                return new ConfigurationSerializer(); 
            }
        }
    }
}