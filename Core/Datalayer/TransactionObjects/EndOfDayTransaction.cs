using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects
{    
    /// <summary>
    /// Used when ending a day.
    /// </summary>
    [Serializable]
    public class EndOfDayTransaction : PosTransaction, ICloneable, IEndOfDayTransaction
    {
        public bool Cancelled { get; set; }

        public override void Save() { }

        public override TypeOfTransaction TransactionType()
        {
            return TypeOfTransaction.EndOfDay;
        }

        public override object Clone()
        {
            EndOfDayTransaction transaction = new EndOfDayTransaction();
            Populate(transaction);
            return transaction;
        }

        protected  void Populate(EndOfDayTransaction transaction)
        {
            base.Populate(transaction);
            transaction.Cancelled = Cancelled;
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
                XElement xEOD = new XElement("EndOfDayTransaction",
                    new XElement("cancelled", Cancelled)
                );

                xEOD.Add(base.ToXML());
                return xEOD;
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
                            case "cancelled":
                                Cancelled = Conversion.ToBool(transElem.Value);
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
                            errorLogger.LogMessage(LogMessageType.Error, transElem.ToString(), ex);
                        }
                    }
                }
            }
        }
    }
}