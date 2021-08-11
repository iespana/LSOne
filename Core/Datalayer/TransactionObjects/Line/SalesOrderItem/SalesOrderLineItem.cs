using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.SalesOrderItem
{
    public enum SalesOrderItemType
    {
        PrePayment = 0,
        FullPayment = 1
    }

    [Serializable]
    public class SalesOrderLineItem : SaleLineItem
    {

        private SalesOrderItemType salesOrderItemType;      // It is a prepayment or full payment of the sales order.
        private decimal amount;                             // The amount of the sales order
        private decimal prepayAmount;                       // A calculated amount from AX telling how much should be prepaid while ordering.
        private string salesOrderId;                        // The Id of the sales order
        private DateTime creationDate = DateTime.ParseExact("01.01.1900", "dd.MM.yyyy", null);                      // When was it created
        private decimal balance;                            // The balance of the sales order

        #region Properties
        public SalesOrderItemType SalesOrderItemType
        {
            get { return salesOrderItemType; }
            set { salesOrderItemType = value; }
        }

        public decimal PrepayAmount
        {
            get { return prepayAmount; }
            set { prepayAmount = value; }
        }

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }


        public string SalesOrderId
        {
            get { return salesOrderId; }
            set { salesOrderId = value; }
        }

        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        public decimal Balance
        {
            get { return balance; }
            set { balance = value; }
        }

        #endregion

        public SalesOrderLineItem(RetailTransaction transaction) : base(transaction)
        {
        }

        protected override SalesTransaction.ItemClassTypeEnum GetItemClassType(SaleLineItem saleItem)
        {
            return SalesTransaction.ItemClassTypeEnum.SalesOrder;
        }

        public override object Clone()
        {
            SalesOrderLineItem item = new SalesOrderLineItem((RetailTransaction)Transaction);
            Populate(item, Transaction);
            return item;
        }

        public override SaleLineItem Clone(IRetailTransaction transaction)
        {
            var item = new SalesOrderLineItem((RetailTransaction)transaction);
            Populate(item, transaction);
            return item;
        }

        protected void Populate(SalesOrderLineItem item, IRetailTransaction transaction)
        {
            base.Populate(item, transaction);
            item.amount = amount;
            item.salesOrderId = salesOrderId;
            item.creationDate = creationDate;
            item.prepayAmount = prepayAmount;
            item.salesOrderItemType = salesOrderItemType;
            item.balance = balance;
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
                XElement xSalesOrder = new XElement("SalesOrderLineItem",
                    new XElement("amount", amount.ToString()),
                    new XElement("salesOrderId", salesOrderId),
                    new XElement("creationDate", Conversion.ToXmlString(creationDate)),
                    new XElement("prepayAmount", prepayAmount.ToString()),
                    new XElement("salesOrderItemType", (int)salesOrderItemType),
                    new XElement("balance", balance.ToString())
                );

                xSalesOrder.Add(base.ToXML());
                return xSalesOrder;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "SalesOrderLineItem.ToXml", ex);
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
                                        amount = Convert.ToDecimal(xVariable.Value);
                                        break;
                                    case "salesOrderId":
                                        salesOrderId = xVariable.Value.ToString();
                                        break;
                                    case "creationDate":
                                        creationDate = Conversion.XmlStringToDateTime(xVariable.Value);
                                        break;                    
                                    case "prepayAmount":
                                        prepayAmount = Convert.ToDecimal(xVariable.Value);
                                        break;
                                    case "salesOrderItemType":
                                        salesOrderItemType = (SalesOrderItemType)Convert.ToInt32(xVariable.Value);
                                        break;
                                    case "balance":
                                        balance = Convert.ToDecimal(xVariable.Value);
                                        break;
                                    default:
                                        base.ToClass(xVariable, errorLogger);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, "SalesOrderLineItem:" + xVariable.Name.ToString(), ex);
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
                    errorLogger.LogMessage(LogMessageType.Error, "SalesOrderLineItem.ToClass", ex);
                }
            }
        }

    }
}
