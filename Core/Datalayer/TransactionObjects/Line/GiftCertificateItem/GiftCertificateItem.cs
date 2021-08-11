using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.GiftCertificateItem
{

    [Serializable]
    public class GiftCertificateItem : SaleLineItem, IGiftCertificateItem
    {

        #region Member variables

        private string serialNumber;                            // The gift certificate serial number
        private decimal amount;                                 // The  amount
        private string storeId;                                 // The issueing store
        private string terminalId;                              // The issueing terminal
        private string staffId;                                 // The issueing staff member
        private string transactionId;                           // The id of the transaction in which the gift certificate was created.
        private string receiptId;                               // The receipt id of the transaction in which the gift certificate was created.
        private Date date = new Date();                         // The date the gift certificate was issued.

        #endregion

        #region Properties

        public string SerialNumber
        {
            get
            {
                return serialNumber;
            }
            set
            {
                if (serialNumber == value)
                    return;
                serialNumber = value;
            }
        }
        public decimal Amount
        {
            get
            {
                return amount;
            }
            set
            {
                if (amount == value)
                    return;
                amount = value;
            }
        }

        public string StoreId
        {
            get
            {
                return storeId;
            }
            set
            {
                if (storeId == value)
                    return;
                storeId = value;
            }
        }
        public string TerminalId
        {
            get
            {
                return terminalId;
            }
            set
            {
                if (terminalId == value)
                    return;
                terminalId = value;
            }
        }
        public string StaffId
        {
            get
            {
                return staffId;
            }
            set
            {
                if (staffId == value)
                    return;
                staffId = value;
            }
        }
        public string TransactionId
        {
            get
            {
                return transactionId;
            }
            set
            {
                if (transactionId == value)
                    return;
                transactionId = value;
            }
        }
        public string ReceiptId
        {
            get
            {
                return receiptId;
            }
            set
            {
                if (receiptId == value)
                    return;
                receiptId = value;
            }
        }
        public DateTime Date
        {
            get
            {
                return date.DateTime;
            }
            set
            {
                date = new Date(value, true);
            }
        }

        #endregion

        public GiftCertificateItem(IRetailTransaction transaction) 
            : base(transaction)
        {
			this.ItemClassType = SalesTransaction.ItemClassTypeEnum.GiftCertificateItem;
        }

        protected override SalesTransaction.ItemClassTypeEnum GetItemClassType(SaleLineItem saleItem)
        {
            return SalesTransaction.ItemClassTypeEnum.GiftCertificateItem;
        }

        public override object Clone()
        {
            GiftCertificateItem item = new GiftCertificateItem((RetailTransaction)Transaction);
            Populate(item, Transaction);
            return item;
        }

        public override SaleLineItem Clone(IRetailTransaction transaction)
        {
            var item = new GiftCertificateItem((RetailTransaction)transaction);
            Populate(item, transaction);
            return item;
        }

        protected void Populate(GiftCertificateItem item, IRetailTransaction transaction)
        {
            base.Populate(item, transaction);
            item.serialNumber = serialNumber;
            item.amount = amount;
            item.storeId = storeId;
            item.terminalId = terminalId;
            item.staffId = staffId;
            item.transactionId = transactionId;
            item.receiptId = receiptId;
            item.Date = Date;
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
                XElement xGiftCertificate = new XElement("GiftCertificateItem",
                    new XElement("serialNumber", serialNumber),
                    new XElement("amount", amount.ToString()),
                    new XElement("storeId", storeId),
                    new XElement("terminalId", terminalId),
                    new XElement("staffId", staffId),
                    new XElement("transactionId", transactionId),
                    new XElement("receiptId", receiptId),
                    new XElement("date", date.ToXmlString())
                );

                xGiftCertificate.Add(base.ToXML(errorLogger));
                return xGiftCertificate;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "GiftCertificateItem.ToXml", ex);
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
                                    case "serialNumber":
                                        serialNumber = xVariable.Value.ToString();
                                        break;
                                    case "amount":
                                        amount = Convert.ToDecimal(xVariable.Value.ToString());
                                        break;
                                    case "storeId":
                                        storeId = xVariable.Value.ToString();
                                        break;
                                    case "terminalId":
                                        terminalId = xVariable.Value.ToString();
                                        break;
                                    case "staffId": 
                                        staffId = xVariable.Value.ToString();
                                        break;
                                    case "transactionId":
                                        transactionId = xVariable.Value.ToString();
                                        break;
                                    case "receiptId":
                                        receiptId = xVariable.Value.ToString();
                                        break;
                                    case "date":
                                        date = new Date(Conversion.XmlStringToDateTime(xVariable.Value), true);
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
                                    errorLogger.LogMessage(LogMessageType.Error, "GiftCertificateItem:" + xVariable.Name.ToString(), ex);
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
                    errorLogger.LogMessage(LogMessageType.Error, "GiftCertificateItem.ToClass", ex);
                }

                throw ex;
            }
        }
    }
}
