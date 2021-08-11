using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.Units
{
    [DataContract]
    public class Unit : OptimizedUpdateDataEntity
    {
        int maximumDecimals;
        int minimumDecimals;

        public Unit()
            : base()
        {
        }

        public Unit(RecordIdentifier id, string text, int minimumDecimals,int maximumDecimals)
            : base(id, text)
        {
            this.maximumDecimals = maximumDecimals;
            this.minimumDecimals = minimumDecimals;
        }

        [DataMember]
        [StringLength(20)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (base.Text != value)
                {
                    PropertyChanged("TXT", value);
                    base.Text = value;
                }
            }
        }

        [DataMember]
        public int MaximumDecimals
        {
            get
            {
                return maximumDecimals;
            }
            set
            {
                if (maximumDecimals != value)
                {
                    maximumDecimals = value;
                    PropertyChanged("UNITDECIMALS", value);
                }
            }
        }

        [DataMember]
        public int MinimumDecimals
        {
            get
            {
                return minimumDecimals;
            }
            set
            {
                if (minimumDecimals != value)
                {
                    minimumDecimals = value;
                    PropertyChanged("MINUNITDECIMALS", value);
                }
            }
        }

        public DecimalLimit Limit
        {
            get
            {
                return new DecimalLimit(minimumDecimals, maximumDecimals);
            }
        }

        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            var elements = element.Elements();
            foreach (XElement current in elements)
            {
                if (!current.IsEmpty)
                {
                    try
                    {
                        switch (current.Name.ToString())
                        {
                            case "unitID":
                                ID = current.Value;
                                break;
                            case "unitName":
                                Text = current.Value;
                                break;
                            case "minimumDecimals":
                                MinimumDecimals = Convert.ToInt32(current.Value);
                                break;
                            case "maximumDecimals":
                                MaximumDecimals = Convert.ToInt32(current.Value);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error, current.Name.ToString(), ex);
                        }
                    }

                }
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            XElement xml = new XElement("unit",
                    new XElement("unitID", ID),
                    new XElement("unitName", Text),
                    new XElement("minimumDecimals", MinimumDecimals),
                    new XElement("maximumDecimals", MaximumDecimals));
            return xml;
        }
    }
}
