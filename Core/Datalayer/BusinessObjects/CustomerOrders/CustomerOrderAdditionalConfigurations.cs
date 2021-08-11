using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.CustomerOrders
{

    /// <summary>
    /// A business object that holds all settings for a type of customer orders
    /// </summary>
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public partial class CustomerOrderAdditionalConfigurations : DataEntity
    {
        [DataMember]
        public ConfigurationType AdditionalType { get; set; }

        public CustomerOrderAdditionalConfigurations()
        {
            Clear();
        }

        public CustomerOrderAdditionalConfigurations(RecordIdentifier id, string text, ConfigurationType configType) : base()
        {
            ID = id;
            Text = text;
            AdditionalType = configType;
        }

        public void Clear()
        {
            ID = new RecordIdentifier(Guid.Empty);
            Text = "";
            AdditionalType = ConfigurationType.None;
        }

        public string AsString(ConfigurationType Value)
        {
            switch (Value)
            {
                case ConfigurationType.None:
                    return Properties.Resources.COConfig_None;
                case ConfigurationType.Source:
                    return Properties.Resources.COConfig_Source;
                case ConfigurationType.Delivery:
                    return Properties.Resources.COConfig_Delivery;
                default:
                    return Properties.Resources.COConfig_None;
            }
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public override object Clone()
        {
            CustomerOrderAdditionalConfigurations newConfig = new CustomerOrderAdditionalConfigurations(this.ID, this.Text, this.AdditionalType);
            return newConfig;
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            try
            {

                XElement xConfig = new XElement("CustomerOrderAdditionalConfigurations",
                    new XElement("AdditionalType", Conversion.ToXmlString((int)AdditionalType))
                    );

                xConfig.Add(base.ToXML());
                return xConfig;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, ex.Message, ex);
                throw;
            }
        }

        public override void ToClass(XElement xmlAnswer, IErrorLog errorLogger = null)
        {
            if (xmlAnswer.HasElements)
            {
                var orderElements = xmlAnswer.Elements("CustomerOrderAdditionalConfigurations");
                foreach (XElement order in orderElements)
                {
                    if (order.HasElements)
                    {
                        IEnumerable<XElement> orderVariables = order.Elements();
                        foreach (XElement elem in orderVariables)
                        {
                            if (!elem.IsEmpty)
                            {
                                try
                                {
                                    switch (elem.Name.ToString())
                                    {
                                        case "AdditionalType":
                                            AdditionalType = (ConfigurationType)Conversion.XmlStringToInt(elem.Value);
                                            break;
                                        default:
                                            base.ToClass(elem);
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    errorLogger?.LogMessage(LogMessageType.Error, elem.ToString(), ex);
                                    throw;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
