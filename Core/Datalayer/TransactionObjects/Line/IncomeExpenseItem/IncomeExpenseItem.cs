using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.IncomeExpenseItem
{
    public enum IncomeExpenseAccountType
    {
        INCOME = 0,
        EXPENSE = 1
    }

    [Serializable]
    public class IncomeExpenseItem : SaleLineItem
    {
        public decimal Amount { get; set; }
        [StringLength(20)]
        public string AccountNumber { get; set; }
        [StringLength(60)]
        public string AccountName { get; set; }
        public string AccountNameAlias { get; set; }
        public IncomeExpenseAccountType AccountType { get; set; }
        public string LedgerAccount { get; set; }
        public string MessageLine1 { get; set; }
        public string MessageLine2 { get; set; }
        public string SlipText1 { get; set; }
        public string SlipText2 { get; set; }
        public RecordIdentifier TaxCodeId { get; set; }

        public IncomeExpenseItem(RetailTransaction retailTransaction) : base(retailTransaction)
        {
            Amount = 0;
            AccountNumber = "";
            AccountName = "";
            AccountNameAlias = "";
            LedgerAccount = "";
            MessageLine1 = "";
            MessageLine2 = "";
            SlipText1 = "";
            SlipText2 = "";
            TaxCodeId = RecordIdentifier.Empty;
        }

        protected override SalesTransaction.ItemClassTypeEnum GetItemClassType(SaleLineItem saleItem)
        {
            return SalesTransaction.ItemClassTypeEnum.IncomeExpenseItem;
        }

        public override object Clone()
        {
            IncomeExpenseItem item = new IncomeExpenseItem((RetailTransaction)Transaction);
            Populate(item, Transaction);
            return item;
        }

        public override SaleLineItem Clone(IRetailTransaction transaction)
        {
            var item = new IncomeExpenseItem((RetailTransaction) transaction);
            Populate(item, transaction);
            return item;
        }

        public void Populate(IncomeExpenseItem item, IRetailTransaction transaction)
        {
            base.Populate(item, transaction);
            item.Amount = Amount;
            item.AccountName = AccountName;
            item.AccountNameAlias = AccountNameAlias;
            item.AccountNumber = AccountNumber;
            item.AccountType = AccountType;
            item.LedgerAccount = LedgerAccount;
            item.MessageLine1 = MessageLine1;
            item.MessageLine2 = MessageLine2;
            item.SlipText1 = SlipText1;
            item.SlipText2 = SlipText2;
            item.TaxCodeId = TaxCodeId;
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
                XElement xIEItem = new XElement("IncomeExpenseItem",
                    new XElement("amount", Amount.ToString()),
                    new XElement("accountNumber", AccountNumber),
                    new XElement("accountName", AccountName),
                    new XElement("accountNameAlias", AccountNameAlias),
                    new XElement("accountType", (int)AccountType),
                    new XElement("ledgerAccount", LedgerAccount),
                    new XElement("messageLine1", MessageLine1),
                    new XElement("messageLine2", MessageLine2),
                    new XElement("slipText1", SlipText1),
                    new XElement("slipText2", SlipText2),
                    new XElement("taxCodeId", TaxCodeId)
                );
                xIEItem.Add(base.ToXML(errorLogger));
                return xIEItem;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "IncomeExpenseItem.ToXML", ex);
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
                                        Amount = Convert.ToDecimal(xVariable.Value);
                                        break;
                                    case "accountNumber":
                                        AccountNumber = xVariable.Value;
                                        break;
                                    case "accountName":
                                        AccountName = xVariable.Value;
                                        break;
                                    case "accountNameAlias":
                                        AccountNameAlias = xVariable.Value;
                                        break;
                                    case "accountType":
                                        AccountType = (IncomeExpenseAccountType)Convert.ToInt32(xVariable.Value);
                                        break;
                                    case "ledgerAccount":
                                        LedgerAccount = xVariable.Value;
                                        break;
                                    case "messageLine1":
                                        MessageLine1 = xVariable.Value;
                                        break;
                                    case "messageLine2":
                                        MessageLine2 = xVariable.Value;
                                        break;
                                    case "slipText1":
                                        SlipText1 = xVariable.Value;
                                        break;
                                    case "slipText2": 
                                        SlipText2 = xVariable.Value;
                                        break;
                                    case "taxCodeId":
                                        TaxCodeId = xVariable.Value;
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
                                    errorLogger.LogMessage(LogMessageType.Error, "IncomeExpenseItem:" + xVariable.Name.ToString(), ex);
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
                    errorLogger.LogMessage(LogMessageType.Error, "IncomeExpenseItem.ToClass", ex);
                }

                throw ex;
            }
        }

    }
}
