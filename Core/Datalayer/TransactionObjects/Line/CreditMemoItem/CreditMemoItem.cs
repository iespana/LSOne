using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.CreditMemoItem
{
    [Serializable]
    public class CreditMemoItem : SaleLineItem, ICreditMemoItem
    {
        #region Member variables

        /// <summary>
        /// The credit memo serial number.
        /// </summary>
        private string creditMemoNumber;  
        
        /// <summary>
        /// The credit memo amount.
        /// </summary>
        private decimal amount;

        #endregion

        #region Properties

        /// <summary>
        /// The serial number on the credit memo
        /// </summary>
        public string CreditMemoNumber
        {
            get
            {
                return creditMemoNumber;
            }
            set
            {
                if (creditMemoNumber == value)
                    return;
                creditMemoNumber = value;
            }
        }

        /// <summary>
        /// The credit memo amount
        /// </summary>
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

        #endregion

        public CreditMemoItem(RetailTransaction transaction)
            : base(transaction)
        {
            ItemClassType = SalesTransaction.ItemClassTypeEnum.CreditMemo;
            creditMemoNumber = string.Empty;
            amount = 0M;
        }

        public CreditMemoItem(CustomerPaymentTransaction transaction)
            : base(transaction, transaction.StoreId, transaction.StoreCurrencyCode, false)
        {
            ItemClassType = SalesTransaction.ItemClassTypeEnum.CreditMemo;
            creditMemoNumber = string.Empty;
            amount = 0M;
        }

        protected override SalesTransaction.ItemClassTypeEnum GetItemClassType(SaleLineItem saleItem)
        {
            return SalesTransaction.ItemClassTypeEnum.CreditMemo;
        }

        public override object Clone()
        {
            CreditMemoItem item = new CreditMemoItem((RetailTransaction)Transaction);
            Populate(item, (RetailTransaction)Transaction);
            return item;
        }

        public override SaleLineItem Clone(IRetailTransaction transaction)
        {
            var item = new CreditMemoItem((RetailTransaction)transaction);
            Populate(item, (RetailTransaction)transaction);
            return item;
        }

        internal void Populate(CreditMemoItem item, RetailTransaction transaction)
        {
            base.Populate(item, transaction);
            item.creditMemoNumber = creditMemoNumber;
            item.amount = amount;
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
                XElement xCreditMemo = new XElement("CreditMemoItem",
                    new XElement("creditMemoNumber", creditMemoNumber),
                    new XElement("amount", Conversion.ToXmlString(amount))
                );

                xCreditMemo.Add(base.ToXML(errorLogger));
                return xCreditMemo;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "CreditMemoItem.ToXML", ex);

                throw;
            }
        }

        public override void ToClass(XElement xItem, IErrorLog errorLogger = null)
        {
            try
            {
                if (xItem.HasElements)
                {
                    IEnumerable<XElement> classElements = xItem.Elements("CreditMemoItem");
                    foreach (XElement xClass in classElements)
                    {
                        if (xClass.HasElements)
                        {
                            IEnumerable<XElement> classVariables = xClass.Elements();
                            foreach (XElement xVariable in classVariables)
                            {
                                if (!xVariable.IsEmpty)
                                {
                                    try
                                    {
                                        switch (xVariable.Name.ToString())
                                        {
                                            case "creditMemoNumber":
                                                creditMemoNumber = xVariable.Value;
                                                break;
                                            case "amount":
                                                amount = Conversion.XmlStringToDecimal(xVariable.Value);
                                                break;
                                            default:
                                                base.ToClass(xVariable, errorLogger);
                                                break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        errorLogger?.LogMessage(LogMessageType.Error, "CreditMemoItem:" + xVariable.Name, ex);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "CreditMemoItem.ToClass", ex);

                throw;
            }
        }
    }
}
