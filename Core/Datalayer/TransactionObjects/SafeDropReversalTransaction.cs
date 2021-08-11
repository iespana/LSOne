using System;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects
{
    [Serializable]
    public class SafeDropReversalTransaction : TenderCountTransaction
    {

        public override TypeOfTransaction TransactionType()
        {
            return TypeOfTransaction.SafeDropReversal;
        }

        public override object Clone()
        {
            SafeDropReversalTransaction transaction = new SafeDropReversalTransaction();
            Populate(transaction);
            return transaction;
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
