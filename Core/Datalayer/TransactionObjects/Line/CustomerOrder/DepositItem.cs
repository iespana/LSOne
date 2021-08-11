using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.CustomerOrder
{
    [Serializable]
    public class DepositItem : IDepositItem
    {
        private string XmlElementName;
        
        public decimal Deposit { get; set; }
        public bool DepositPaid { get; set; }

        public DepositsStatus Status { get; set; }

        public DepositItem()
        {
            XmlElementName = "DepositItem";
            Clear();
        }

        public DepositItem(decimal deposit, bool depositPaid, DepositsStatus status) : this()
        {
            Deposit = deposit;
            DepositPaid = depositPaid;
            Status = status;
        }

        public DepositItem(decimal deposit) : this(deposit, false, DepositsStatus.Normal)
        {
            
        }

        public void Clear()
        {
            Deposit = decimal.Zero;
            DepositPaid = false;
            Status = DepositsStatus.Normal;
        }

        public bool Empty()
        {
            return Deposit == decimal.Zero;
        }
        

        public object Clone()
        {
            DepositItem item = new DepositItem();
            Populate(item);
            return item;
        }

        private void Populate(DepositItem item)
        {
            item.Deposit = Deposit;
            item.DepositPaid = DepositPaid;
            item.Status = Status;
        }

        public void ToClass(XElement xElement, IErrorLog errorLogger = null)
        {
            try
            {
                if (xElement.HasElements)
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
                                        case "Deposit":
                                            Deposit = Conversion.XmlStringToDecimal(elem.Value);
                                            break;
                                        case "DepositPaid":
                                            DepositPaid = Conversion.XmlStringToBool(elem.Value);
                                            break;
                                        case "DepositReturned":
                                            Status = DepositsStatus.Returned;
                                            break;
                                        case "Status":
                                            Status = (DepositsStatus)Conversion.XmlStringToInt(elem.Value);
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    errorLogger?.LogMessage(LogMessageType.Error, elem.Name.ToString(), ex);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, xElement.Name.ToString(), ex);
            }
        }

        public XElement ToXML(IErrorLog errorLogger = null)
        {
            try
            {
                var xItem = new XElement(XmlElementName,
                    new XElement("Deposit", Conversion.ToXmlString(Deposit)),
                    new XElement("DepositPaid", Conversion.ToXmlString(DepositPaid)),
                    new XElement("Status", Conversion.ToXmlString((int)Status))
                );

                return xItem;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, ex.Message, ex);
                throw;
            }
        }
    }
}
