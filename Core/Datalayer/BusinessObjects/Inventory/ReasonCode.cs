using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    [Serializable]
    public class ReasonCode : DataEntity
    {
        public const string XmlElementName = "ReasonCode";

        public ReasonCode()
        {
            Action = ReasonActionEnum.MainInventory;
            BeginDate = new DateTime(DateTime.Now.Year, 1, 1);
            IsSystemReasonCode = false;
            ShowOnPos = false;
        }

        public ReasonCode(string reasonId, string reasonText)
            : base(reasonId, reasonText)
        {
            Action = ReasonActionEnum.MainInventory;
            BeginDate = new DateTime(DateTime.Now.Year, 1, 1);
            IsSystemReasonCode = false;
            ShowOnPos = false;
        }

        public ReasonActionEnum Action { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsSystemReasonCode { get; set; }

        public bool ShowOnPos { get; set; }

        /// <summary>
        /// Returns a default system reason code for return transactions/items
        /// </summary>
        /// <returns></returns>
        public static ReasonCode DefaultReturns()
        {
            return new ReasonCode
            {
                ID = "RT001",
                Text = Properties.Resources.ReasonCodeDefaultReturns,
                Action = ReasonActionEnum.MainInventory,
                BeginDate = DateTime.Today.AddDays(-1),
                IsSystemReasonCode = true,
                ShowOnPos = true
            };
        }

        public override object Clone()
        {
            ReasonCode r = new ReasonCode();
            Populate(r);
            return r;
        }

        protected void Populate(ReasonCode code)
        {
            code.ID = ID;
            code.Text = Text;
            code.Action = Action;
            code.BeginDate = BeginDate;
            code.EndDate = EndDate;
            code.IsSystemReasonCode = IsSystemReasonCode;
            code.ShowOnPos = ShowOnPos;
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
                XElement xSaleItem = new XElement(XmlElementName,
                    new XElement("action", Conversion.ToXmlString((int)Action)),
                    new XElement("beginDate", Conversion.ToXmlString(BeginDate)),
                    new XElement("endDate", EndDate.HasValue ? Conversion.ToXmlString(EndDate.Value) : string.Empty),
                    new XElement("isSystemReasonCode", Conversion.ToXmlString(IsSystemReasonCode)),
                    new XElement("showOnPos", Conversion.ToXmlString(ShowOnPos))
                );

                xSaleItem.Add(base.ToXML(errorLogger));
                return xSaleItem;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "ReasonCode.ToXml", ex);

                throw;
            }
        }

        public override void ToClass(XElement xItem, IErrorLog errorLogger = null)
        {
            try
            {
                if (xItem.HasElements)
                {
                    IEnumerable<XElement> reasonCodes = xItem.Elements(XmlElementName);
                    foreach (XElement xElement in reasonCodes)
                    {
                        if (xElement.HasElements)
                        {
                            IEnumerable<XElement> classVariables = xElement.Elements();
                            foreach (XElement xVariable in classVariables)
                            {
                                if (!xVariable.IsEmpty)
                                {
                                    try
                                    {
                                        switch (xVariable.Name.ToString())
                                        {
                                            case "action":
                                                Action = (ReasonActionEnum)Conversion.XmlStringToInt(xVariable.Value);
                                                break;
                                            case "beginDate":
                                                BeginDate = Conversion.XmlStringToDateTime(xVariable.Value);
                                                break;
                                            case "endDate":
                                                EndDate = xVariable.Value != string.Empty ? Conversion.XmlStringToDateTime(xVariable.Value) : (DateTime?)null;
                                                break;
                                            case "isSystemReasonCode":
                                                IsSystemReasonCode = Conversion.XmlStringToBool(xVariable.Value);
                                                break;
                                            case "showOnPos":
                                                ShowOnPos = Conversion.XmlStringToBool(xVariable.Value);
                                                break;
                                            default:
                                                base.ToClass(xVariable);
                                                break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        errorLogger?.LogMessage(LogMessageType.Error, "ReasonCode:" + xVariable.Name, ex);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "ReasonCode.ToClass", ex);

                throw;
            }
        }
    }
}
