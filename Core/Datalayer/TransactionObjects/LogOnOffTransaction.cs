using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects
{
    /// <summary>
    /// Used to register when a operator logs on or off.
    /// </summary>
    [Serializable]
    public class LogOnOffTransaction : PosTransaction
    {
        private bool logon;         //Is set to true if the user is login on to the terminal, else set to false.
        private bool applicationExit;
        /// <summary>
        /// Is set to true if the user is login on to the terminal, else set to false.
        /// </summary>
        public bool Logon
        {
            get { return logon; }
            set { logon = value; }
        }
        public bool ApplicationExit
        {
            get { return applicationExit; }
            set { applicationExit = value; }
        }

        public override void Save()
        {
        }

        public override TypeOfTransaction TransactionType()
        {
            return logon ? TypeOfTransaction.LogOn : TypeOfTransaction.LogOff;
        }


        public override object Clone()
        {
            LogOnOffTransaction transaction = new LogOnOffTransaction();
            Populate(transaction);
            return transaction;
        }

        protected void Populate(LogOnOffTransaction transaction)
        {
            base.Populate(transaction);
            transaction.applicationExit = applicationExit;
            transaction.logon = logon;
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
                                    case "logon":
                                        logon = Conversion.ToBool(transElem.Value.ToString());
                                        break;
                                    case "applicationExit":
                                        applicationExit = Conversion.ToBool(transElem.Value.ToString());
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
                XElement xLog = new XElement("LogOnOffTransaction",
                    new XElement("logon", logon),
                    new XElement("applicationExit", applicationExit)
                );

                xLog.Add(base.ToXML());
                return xLog;
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
