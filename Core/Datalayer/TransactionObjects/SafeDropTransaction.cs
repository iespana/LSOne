using System;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects
{
    [Serializable]
    public class SafeDropTransaction : TenderCountTransaction
    {
        public override object Clone()
        {
            SafeDropTransaction transaction = new SafeDropTransaction();
            Populate(transaction);
            return transaction;
        }

        public override TypeOfTransaction TransactionType()
        {
            return TypeOfTransaction.SafeDrop;
        }

        public override void ToClass(System.Xml.Linq.XElement xmlTrans, IErrorLog errorLogger = null)
        {
            if (xmlTrans.HasElements)
            {
                try
                {
                    base.ToClass(xmlTrans, errorLogger);
                }
                catch (Exception ex)
                {
                    if (errorLogger != null)
                    {
                        errorLogger.LogMessage(LogMessageType.Error, ex.Message, ex);
                    }
                }
            }
        }

        public override System.Xml.Linq.XElement ToXML(IErrorLog errorLogger = null)
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
            return base.ToXML(errorLogger);
        }
    }
}
