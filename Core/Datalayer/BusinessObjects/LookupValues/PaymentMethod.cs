using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.LookupValues
{
    public class PaymentMethod : DataEntity
    {

        public PaymentMethodDefaultFunctionEnum DefaultFunction { get; set; }

        [RecordIdentifierValidation(20)]
        public override RecordIdentifier ID
        {
            get { return base.ID; }
            set { base.ID = value; }
        }

        [StringLength(30)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        /// Specifies if a payment method is the local currency of the store (default payment type).
        /// Usually this is cash in the currency of the country.
        /// Only one payment type can be marked as local currency. Payment types marked as local currency are not allowed to have any limitations.
        /// </summary>
        public bool IsLocalCurrency { get; set; }

        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            var currencyElements = element.Elements();
            foreach (XElement storeElem in currencyElements)
            {
                if (!storeElem.IsEmpty)
                {
                    try
                    {
                        switch (storeElem.Name.ToString())
                        {
                            case "name":
                                Text = storeElem.Value;
                                break;
                            case "paymentMethodID":
                                ID = storeElem.Value;
                                break;
                            case "defaultFunction":
                                DefaultFunction = (PaymentMethodDefaultFunctionEnum) Convert.ToInt32(storeElem.Value);
                                break;
                            case "isLocalCurrency":
                                IsLocalCurrency = Convert.ToBoolean(storeElem.Value);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error,
                                                   storeElem.Name.ToString(), ex);
                        }
                    }
                }
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            XElement xml = new XElement("paymentMethods",
                    new XElement("paymentMethodID", (string)ID),
                    new XElement("name", Text),
                    new XElement("defaultFunction", (int)DefaultFunction),
                    new XElement("isLocalCurrency", IsLocalCurrency));
            return xml;
        }

        public override object Clone()
        {
            var o = new PaymentMethod();
            Populate(o);
            return o;
        }

        protected void Populate(PaymentMethod o)
        {
            o.ID = (RecordIdentifier)ID.Clone();
            o.Text = Text;
            o.DefaultFunction = DefaultFunction;
            o.IsLocalCurrency = IsLocalCurrency;
        }
    }
}
