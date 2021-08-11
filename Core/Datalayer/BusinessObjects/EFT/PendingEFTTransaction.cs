using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LSOne.DataLayer.BusinessObjects.EFT
{
    /// <summary>
    /// Contains temporary data saved before performing an EFT payment and used for recovery in case of errors.
    /// </summary>
    public class PendingEFTTransaction
    {
        /// <summary>
        /// Unique ID of the EFT payment that will be processed. Generated before doing an EFT payment and saved in case we need to recover the payment.
        /// </summary>
        public RecordIdentifier PendingEFTPaymentID { get; set; }

        /// <summary>
        /// Tender ID of the EFT payment that will be processed.
        /// </summary>
        public RecordIdentifier PendingEFTTenderID { get; set; }

        /// <summary>
        /// ID of the payment which should be returned
        /// </summary>
        public RecordIdentifier PendingEFTRefundPaymentID { get; set; }

        /// <summary>
        /// Amount that will be paid
        /// </summary>
        public decimal Amount { get; set; }

        public PendingEFTTransaction()
        {
            Clear();
        }

        public void Clear()
        {
            PendingEFTPaymentID = "";
            PendingEFTRefundPaymentID = "";
            PendingEFTTenderID = "";
            Amount = 0;
        }

        public virtual object Clone()
        {
            PendingEFTTransaction eft = new PendingEFTTransaction();
            eft.PendingEFTPaymentID = PendingEFTPaymentID;
            eft.PendingEFTRefundPaymentID = PendingEFTRefundPaymentID;
            eft.PendingEFTTenderID = PendingEFTTenderID;
            eft.Amount = Amount;
            return eft;
        }

        public XElement ToXML(IErrorLog errorLogger = null)
        {
            try
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
                XElement xItem = new XElement("PendingEFTTransaction",
                    new XElement("PendingEFTPaymentID", PendingEFTPaymentID),
                    new XElement("PendingEFTTenderID", PendingEFTTenderID),
                    new XElement("PendingEFTRefundPaymentID", PendingEFTRefundPaymentID),
                    new XElement("Amount", Conversion.ToXmlString(Amount))
                );

                return xItem;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "PendingEFTTransaction.ToXML", ex);

                throw;
            }
        }

        public void ToClass(XElement xItem, IErrorLog errorLogger = null)
        {
            try
            {
                if (xItem.HasElements)
                {
                    IEnumerable<XElement> xElements = xItem.Elements();
                    foreach (XElement xElem in xElements)
                    {
                        if (!xElem.IsEmpty)
                        {
                            try
                            {
                                switch (xElem.Name.ToString())
                                {
                                    case "PendingEFTPaymentID":
                                        PendingEFTPaymentID = xElem.Value;
                                        break;
                                    case "PendingEFTTenderID":
                                        PendingEFTTenderID = xElem.Value;
                                        break;
                                    case "PendingEFTRefundPaymentID":
                                        PendingEFTRefundPaymentID = xElem.Value;
                                        break;
                                    case "Amount":
                                        Amount = Conversion.XmlStringToDecimal(xElem.Value);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                errorLogger?.LogMessage(LogMessageType.Error, "PendingEFTTransaction:" + xElem.Name, ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "PendingEFTTransaction.ToClass", ex);
                throw;
            }
        }
    }
}
