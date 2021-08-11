using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.SaleItem
{

    [Serializable]
    public class DiscountVoucherItem : SaleLineItem, IDiscountVoucherItem
    {
        #region Member variables
        private int sourceLineNum;
        #endregion

        #region Properties

        public int SourceLineNum
        {
            get { return sourceLineNum; }
            set { sourceLineNum = value; }
        }

        #endregion

        public DiscountVoucherItem(RetailTransaction transaction, string voucherID, string voucherDescription, decimal voucherValue)
            : base(transaction)
        {
            this.ItemClassType = SalesTransaction.ItemClassTypeEnum.DiscountVoucherItem;
            this.Found = true;
            this.Quantity = -1;
            this.ItemId = voucherID;
            this.Description = voucherDescription;
            this.PriceWithTax = voucherValue;
            this.OriginalDiscountVoucherPriceWithTax = voucherValue;
            this.TaxIncludedInItemPrice = true;
            this.Price = voucherValue;
            this.Transaction = transaction;
            this.NoDiscountAllowed = true;
        }

        protected override SalesTransaction.ItemClassTypeEnum GetItemClassType(SaleLineItem saleItem)
        {
            return SalesTransaction.ItemClassTypeEnum.DiscountVoucherItem;
        }

        public override object Clone()
        {
            DiscountVoucherItem item = new DiscountVoucherItem((RetailTransaction)Transaction, "", "", 0);
            Populate(item, Transaction);
            return item;
        }

        public override SaleLineItem Clone(IRetailTransaction transaction)
        {
            var item = new DiscountVoucherItem((RetailTransaction)transaction, "", "", 0);
            Populate(item, transaction);
            return item;
        }

        protected void Populate(DiscountVoucherItem item, IRetailTransaction transaction)
        {
            base.Populate(item, transaction);
            item.sourceLineNum = sourceLineNum;
            item.Price = Price;
            item.Description = Description;
            item.ItemId = ItemId;
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
                XElement xDiscountVoucher = new XElement("DiscountVoucherItem",
                    new XElement("sourceLineNum", sourceLineNum)
                );

                xDiscountVoucher.Add(base.ToXML(errorLogger));
                return xDiscountVoucher;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "DiscountVoucherItem.ToXML", ex);
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
                                    case "sourceLineNum":
                                        sourceLineNum = Convert.ToInt32(xVariable.Value.ToString());
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
                                    errorLogger.LogMessage(LogMessageType.Error, "DiscountVoucherItem:" + xVariable.Name.ToString(), ex);
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
                    errorLogger.LogMessage(LogMessageType.Error, "DiscountVoucherItem.ToClass", ex);
                }

                throw ex;
            }
        }
    }
}
