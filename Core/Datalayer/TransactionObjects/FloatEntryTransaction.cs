using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects
{
    public enum FloatEntryTransactionSubType
    {
        DeclareStartAmount = 0,
        FloatEntry = 1
    }

    [Serializable]
    public class FloatEntryTransaction : PosTransaction, ICloneable
    {

        public decimal AmountMST { get; set; }

        public decimal ExchrateMST { get; set; }

        public decimal Amount { get; set; }

        public decimal AddedAmount { get; set; }

        public decimal PreviouslyTendered { get; set; }

        public string Description { get; set; }

        public FloatEntryTransactionSubType FloatEntryTransactionSubType { get; set; }

        public override void Save() {  }

        public override TypeOfTransaction TransactionType()
        {
            return TypeOfTransaction.FloatEntry;
        }

        public override int TransactionSubType
        {
            get
            {
                return (int)FloatEntryTransactionSubType;
            }

            set
            {
                if(Enum.IsDefined(typeof(FloatEntryTransactionSubType), value))
                {
                    FloatEntryTransactionSubType = (FloatEntryTransactionSubType)value;
                }
                else
                {
                    FloatEntryTransactionSubType = FloatEntryTransactionSubType.FloatEntry;
                }
            }
        }

        public FloatEntryTransaction()
        {
            //Default
        }

        public FloatEntryTransaction(FloatEntryTransactionSubType subType)
        {
            TransactionSubType = (int)subType;
        }

        protected void Populate(FloatEntryTransaction transaction)
        {
            base.Populate(transaction);
            transaction.Amount = Amount;
            transaction.Description = Description;
            transaction.AmountMST = AmountMST;
            transaction.ExchrateMST = ExchrateMST;
            transaction.AddedAmount = AddedAmount;
            transaction.PreviouslyTendered = PreviouslyTendered;
            transaction.FloatEntryTransactionSubType = FloatEntryTransactionSubType;
        }

        public override object Clone()
        {
            FloatEntryTransaction transaction = new FloatEntryTransaction();
            Populate(transaction);
            return transaction;
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
                XElement xFloatEntry = new XElement("FloatEntryTransaction",
                    new XElement("amount", Amount.ToString()),
                    new XElement("description", Description),
                    new XElement("amountmst", AmountMST.ToString()),
                    new XElement("exchratemst", ExchrateMST.ToString())
                );

                xFloatEntry.Add(base.ToXML());
                return xFloatEntry;
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
                            case "amount":
                                Amount = Convert.ToDecimal(transElem.Value);
                                break;
                            case "description":
                                Description = transElem.Value;
                                break;
                            case "amountmst":
                                AmountMST = Convert.ToDecimal(transElem.Value);
                                break;
                            case "exchratemst":
                                ExchrateMST = Convert.ToDecimal(transElem.Value);
                                break;
                            case "PosTransaction":
                                base.ToClass(transElem, errorLogger);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error, transElem.Name.ToString(), ex);
                        }
                    }
                }
            }
        }
    }
}
