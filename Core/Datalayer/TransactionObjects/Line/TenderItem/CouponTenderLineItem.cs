using System;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.TenderItem
{
    //Used to register coupon payments
    [Serializable]
    public class CouponTenderLineItem : TenderLineItem
    {
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

        public override void ToClass(System.Xml.Linq.XElement xItem, IErrorLog errorLogger = null)
        {
            base.ToClass(xItem,errorLogger);
        }
    }
}
