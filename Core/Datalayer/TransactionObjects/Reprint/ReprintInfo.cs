using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Reprint
{
    public class ReprintInfo : DataEntity, IReprintInfo
    {
        public RecordIdentifier LineID { get; set; }
        public Date ReprintDate { get; set; }
        public RecordIdentifier Staff { get; set; }
        public RecordIdentifier Store { get; set; }
        public RecordIdentifier Terminal { get; set; }
        public ReprintTypeEnum ReprintType { get; set; }

        public ReprintInfo()
        {
            LineID = RecordIdentifier.Empty;
            ReprintDate = Date.Empty;
            Staff = RecordIdentifier.Empty;
            Store = RecordIdentifier.Empty;
            Terminal = RecordIdentifier.Empty;
            ReprintType = ReprintTypeEnum.ReceiptCopy; 
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            try
            {
                /*
                * Strings      added as is
                * Int          added as is
                * Bool         added as is
                * 
                * Decimal      added with ToString()
                * DateTime     added with DevUtilities.Utility.DateTimeToXmlString()
                * 
                * Enums        added with an (int) cast   
                * 
               */
                XElement xReprintInfo = new XElement("ReprintInfo",
                    new XElement("LineID", LineID),
                    new XElement("ReprintDate", ReprintDate.ToXmlString()),
                    new XElement("Staff", (string)Staff),
                    new XElement("Store", (string)Store),
                    new XElement("Terminal", (string)Terminal),
                    new XElement("ReprintType", (int)ReprintType)
                );

                xReprintInfo.Add(base.ToXML(errorLogger));
                return xReprintInfo;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "ReprintInfo.ToXml", ex);
                }

                throw ex;
            }
        }

        public override void ToClass(XElement xElement, IErrorLog errorLogger = null)
        {
            try
            {
                if (xElement.HasElements)
                {
                    IEnumerable<XElement> orderVariables = xElement.Elements();
                    foreach (XElement elem in orderVariables)
                    {
                        if (!elem.IsEmpty)
                        {
                            try
                            {
                                switch (elem.Name.ToString())
                                {
                                    case "LineID":
                                        LineID = elem.Value;
                                        break;
                                    case "ReprintDate":
                                        ReprintDate = new Date(Conversion.XmlStringToDateTime(elem.Value), true);
                                        break;
                                    case "Staff":
                                        Staff = elem.Value;
                                        break;
                                    case "Store":
                                        Store = elem.Value;
                                        break;
                                    case "Terminal":
                                        Terminal = elem.Value;
                                        break;
                                    case "ReprintType":
                                        ReprintType = (ReprintTypeEnum) Conversion.ToInt(elem.Value);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, elem.Name.ToString(), ex);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, xElement.Name.ToString(), ex);
                }
            }
        }
    }
}
