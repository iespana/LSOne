using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.SalesInvoiceItem
{
    
    [Serializable]
    public class SalesInvoiceLineItem : SaleLineItem
    {
        
        private decimal amount;                             // The amount of the invoice 
        private string salesInvoiceId;                      // The Id of the sales order
        private DateTime creationDate = DateTime.ParseExact("01.01.1900", "dd.MM.yyyy", null);                      // When was it created


        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        
        public string SalesInvoiceId
        {
            get { return salesInvoiceId; }
            set { salesInvoiceId = value; }
        }

        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        public SalesInvoiceLineItem(RetailTransaction transaction)
            : base(transaction)
        {
        }

        protected override SalesTransaction.ItemClassTypeEnum GetItemClassType(SaleLineItem saleItem)
        {
            return SalesTransaction.ItemClassTypeEnum.SalesInvoice;
        }

        public override object Clone()
        {
            SalesInvoiceLineItem item = new SalesInvoiceLineItem((RetailTransaction)Transaction);
            Populate(item, Transaction);
            return item;
        }

        public override SaleLineItem Clone(IRetailTransaction transaction)
        {
            var item = new SalesInvoiceLineItem((RetailTransaction)transaction);
            Populate(item, transaction);
            return item;
        }

        protected void Populate(SalesInvoiceLineItem item, IRetailTransaction transaction)
        {
            base.Populate(item, transaction);
            item.amount = amount;
            item.salesInvoiceId = salesInvoiceId;
            item.creationDate = creationDate;
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
                * DateTime     added with ToString()
                * 
                * Enums        added with an (int) cast   
                * 
               */
                XElement xSalesInvoice = new XElement("SalesInvoiceLineItem",
                    new XElement("amount", amount.ToString()),
                    new XElement("salesInvoiceId", salesInvoiceId),
                    new XElement("creationDate", Conversion.ToXmlString(creationDate))
                );

                xSalesInvoice.Add(base.ToXML(errorLogger));
                return xSalesInvoice;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "SalesInvoiceLineItem.ToXML", ex);
                }

                throw ex;
            }
        }

        public override void ToClass(XElement xItem, IErrorLog errorLogger = null)
        {
            try
            {
                if (xItem.HasElements)
                {
                    IEnumerable<XElement> classVariables = xItem.Elements();
                    foreach (XElement xVariable in classVariables)
                    {
                        if (!xVariable.IsEmpty)
                        {
                            try
                            {
                                switch (xVariable.Name.ToString())
                                {
                                    case "amount":
                                        amount = Convert.ToDecimal(xVariable.Value.ToString());
                                        break;
                                    case "salesInvoiceId":
                                        salesInvoiceId = xVariable.Value.ToString();
                                        break;
                                    case "creationDate":
                                        creationDate = Conversion.XmlStringToDateTime(xVariable.Value);
                                        break;
                                    default:
                                        base.ToClass(xVariable,errorLogger);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, "SalesInvoiceLineItem:" + xVariable.Name.ToString(), ex);
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
                    errorLogger.LogMessage(LogMessageType.Error, "SalesInvoiceLineItem.ToClass", ex);
                }

                throw ex;
            }
        }

    }
}
