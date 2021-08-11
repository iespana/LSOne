using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects
{
    [Serializable]
    public class BankDropTransaction : TenderCountTransaction
    {
        public BankDropTransaction()
        {
            BankBagNo = "";
        }

        public string BankBagNo { get; set; }

        public override TypeOfTransaction TransactionType()
        {
            return TypeOfTransaction.BankDrop;
        }

        public override object Clone()
        {
            BankDropTransaction transaction = new BankDropTransaction();
            Populate(transaction);
            return transaction;
        }

        protected void Populate(BankDropTransaction transaction)
        {
            base.Populate(transaction);
            transaction.BankBagNo = BankBagNo;
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
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
            try
            {
                XElement xBankDrop = new XElement("BankDropTransaction",
                    new XElement("bankBagNo", BankBagNo)
                );

                xBankDrop.Add(base.ToXML());
                return xBankDrop;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, ex.Message, ex);
                }
                throw ex;
            }
        }

        public override void ToClass(System.Xml.Linq.XElement xmlTrans, IErrorLog errorLogger = null)
        {
            if (xmlTrans.HasElements)
            {
                IEnumerable<XElement> transElements = xmlTrans.Elements();
                foreach (XElement transElem in transElements.Where(transElem => !transElem.IsEmpty))
                {
                    try
                    {
                        switch (transElem.Name.ToString())
                        {
                            case "bankBagNo":
                                BankBagNo = transElem.Value;
                                break;
                            case "TenderCountTransaction":
                                base.ToClass(transElem, errorLogger);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error, transElem.ToString(), ex);
                        }
                    }
                }
            }
        }
    }
}
