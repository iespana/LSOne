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
    /// Used when ending a shift.
    /// </summary>
    [Serializable]
    public class EndOfShiftTransaction : PosTransaction, ICloneable, IEndOfShiftTransaction
    {
        public bool Cancelled { get; set; }

        public override TypeOfTransaction TransactionType()
        {
            return TypeOfTransaction.EndOfShift;
        }

        public override object Clone()
        {
            var transaction = new EndOfShiftTransaction();
            Populate(transaction);
            return transaction;
        }

        protected void Populate(EndOfShiftTransaction transaction)
        {
            base.Populate(transaction);
            transaction.Cancelled = Cancelled;
        }

        public override void Save() { }

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

                var xEOS = new XElement("EndOfShiftTransaction",
                    new XElement("cancelled", Cancelled)
                );

                xEOS.Add(base.ToXML());
                return xEOS;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, ex.Message, ex);
                }
                throw;
            }
        }

        public override void ToClass(XElement xmlTrans, IErrorLog errorLogger = null)
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
    