using System;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects
{
    /// <summary>
    /// Used only for internal operations on the POS - NOT TO BE SAVED TO DATABASE
    /// </summary>
    [Serializable]
    public class InternalTransaction : PosTransaction, ICloneable
    {
        public override void Save()
        {             
        }

        public override TypeOfTransaction TransactionType()
        {
            return TypeOfTransaction.Internal;
        }


        public override object Clone()
        {
            InternalTransaction transaction = new InternalTransaction();
            Populate(transaction);
            return transaction;
        }

        public override System.Xml.Linq.XElement ToXML(IErrorLog errorLogger = null)
        {
            return base.ToXML(errorLogger);
        }

        public override void ToClass(System.Xml.Linq.XElement xmlTrans, IErrorLog errorLogger = null)
        {
            if (xmlTrans.HasElements)
            {
                base.ToClass(xmlTrans, errorLogger);
            }  
        }
    }
}
