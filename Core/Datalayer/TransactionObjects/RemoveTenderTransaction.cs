using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects
{
    [Serializable]
    public class RemoveTenderTransaction : PosTransaction
    {
        private decimal amount;
        private string description;
        private decimal amountmst;
        private decimal exchratemst;      

        #region Properties

        public decimal AmountMST
        {
            get { return amountmst; }
            set { amountmst = value; }
        }

        public decimal ExchrateMST
        {
            get { return exchratemst; }
            set { exchratemst = value; }
        }

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        #endregion

        protected void Populate(RemoveTenderTransaction transaction)
        {
            base.Populate(transaction);
            transaction.amount = amount;
            transaction.description = description;
            transaction.amountmst = amountmst;
            transaction.exchratemst = exchratemst;
        }

        public override TypeOfTransaction TransactionType()
        {
            return TypeOfTransaction.RemoveTender;
        }

        public override object Clone()
        {
            RemoveTenderTransaction transaction = new RemoveTenderTransaction();
            Populate(transaction);
            return transaction;
        }

        public override void Save()
        {
        }

        public override void ToClass(System.Xml.Linq.XElement xmlTrans, IErrorLog errorLogger = null)
        {

            if (xmlTrans.HasElements)
            {
                IEnumerable<XElement> transElements = xmlTrans.Elements();
                foreach (XElement transElem in transElements)
                {
                    if (!transElem.IsEmpty)
                    {
                        try
                        {
                            switch (transElem.Name.ToString())
                            {
                                case "amount":
                                    amount = Convert.ToDecimal(transElem.Value.ToString());
                                    break;

                                case "description":
                                    description = transElem.Value.ToString();
                                    break;

                                case "amountmst":
                                    amountmst = Convert.ToDecimal(transElem.Value.ToString());
                                    break;

                                case "exchratemst":
                                    exchratemst = Convert.ToDecimal(transElem.Value.ToString());
                                    break;

                                case "PosTransaction":
                                    base.ToClass(transElem);
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

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            try
            {
                XElement xRemoveTender = new XElement("RemoveTenderTransaction",
                    new XElement("amount", amount.ToString()),
                    new XElement("description", description),
                    new XElement("amountmst", amountmst.ToString()),
                    new XElement("exchratemst", exchratemst.ToString())
                );

                xRemoveTender.Add(base.ToXML());
                return xRemoveTender;
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
    }
}
