using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.CustomerOrders
{
    [Serializable]
    public class PaymentItem : DataEntity
    {
        private readonly string XmlElementName;

        public decimal Amount { get; set; }
        public bool AmountPaid { get; set; }

        public PaymentStatus Status { get; set; }

        public PaymentItem()
        {
            XmlElementName = "PaymentItem";
            Clear();
        }

        public PaymentItem(decimal amount, bool amountPaid, PaymentStatus status) : this()
        {
            Amount = amount;
            AmountPaid = amountPaid;
            Status = status;
        }

        public PaymentItem(decimal amount) : this(amount, false, PaymentStatus.Normal)
        {
            
        }

        public void Clear()
        {
            Amount = decimal.Zero;
            AmountPaid = false;
            Status = PaymentStatus.Normal;
        }

        public bool Empty()
        {
            return Amount == decimal.Zero;
        }
        

        public override object Clone()
        {
            PaymentItem item = new PaymentItem();
            Populate(item);
            return item;
        }

        private void Populate(PaymentItem item)
        {
            item.Amount = Amount;
            item.AmountPaid = AmountPaid;
            item.Status = Status;
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
                                    case "Amount":
                                        Amount = Conversion.XmlStringToDecimal(elem.Value);
                                        break;
                                    case "AmountPaid":
                                        AmountPaid = Conversion.XmlStringToBool(elem.Value);
                                        break;
                                    case "Status":
                                        Status = (PaymentStatus)Conversion.XmlStringToInt(elem.Value);
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
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, xElement.Name.ToString(), ex);
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            try
            {
                var xItem = new XElement(XmlElementName,
                    new XElement("Amount", Conversion.ToXmlString(Amount)),
                    new XElement("AmountPaid", Conversion.ToXmlString(AmountPaid)),
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
