using System;
using System.Xml.Serialization;

namespace LSOne.DataLayer.DataProviders.ProviderConfig
{
    public class ConfigurationSerializer : XmlSerializer
    {
        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader)
        {
            return xmlReader.IsStartElement(@"LSOneDataLayer", @"");
        }

        protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter()
        {
            throw new NotImplementedException();
        }
        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer)
        {
            throw new NotImplementedException();
        }

        protected override System.Xml.Serialization.XmlSerializationReader CreateReader()
        {
            return new ConfigurationReader();
        }
        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader)
        {
            return ((ConfigurationReader)reader).Read();
        }
    }
}
