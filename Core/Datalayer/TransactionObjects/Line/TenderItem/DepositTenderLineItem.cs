using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.TenderItem
{
    public class DepositTenderLineItem : TenderLineItem, IDepositTenderLineItem
    {
        /// <summary>
        /// If true then this is a tender line with information about the sum of previously paid deposits for the items that are being sold
        /// </summary>
        public bool RedeemedDeposit { get; set; }

        /// <summary>
        /// How much of the original payment has been redeemed
        /// </summary>
        public decimal RedeemedAmount { get; set; }

        public DepositTenderLineItem() : base()
        {
            RedeemedDeposit = false;
            RedeemedAmount = decimal.Zero;

            internalTenderType = TenderTypeEnum.DepositTender;
        }
        

        protected void Populate(DepositTenderLineItem item)
        {
            base.Populate(item);
            item.RedeemedDeposit = RedeemedDeposit;
            item.RedeemedAmount = RedeemedAmount;
            item.internalTenderType = TypeOfTender;
        }

        public override object Clone()
        {
            var item = new DepositTenderLineItem();
            Populate(item);
            return item;
        }

        public override void ToClass(XElement xItem, IErrorLog errorLogger = null)
        {
            try
            {
                if (xItem.HasElements)
                {
                    IEnumerable<XElement> classVariables = xItem.Elements();
                    foreach (XElement xVariable in classVariables)
                    {
                        if (!xVariable.IsEmpty)
                        {
                            try
                            {
                                switch (xVariable.Name.ToString())
                                {
                                    case "RedeemedDeposit":
                                        RedeemedDeposit = Conversion.ToBool(xVariable.Value);
                                        break;
                                    case "RedeemedAmount":
                                        RedeemedAmount = Convert.ToDecimal(xVariable.Value);
                                        break;
                                    default:
                                        base.ToClass(xVariable);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, "DepositTenderLineItem:" + xVariable.Name.ToString(), ex);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "DepositTenderLineItem.ToClass", ex);
                }

                throw ex;
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
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
                XElement xTender = new XElement("DepositTenderLineItem",
                    new XElement("RedeemedDeposit", RedeemedDeposit),
                    new XElement("RedeemedAmount", RedeemedAmount.ToString())
                );
                

                xTender.Add(base.ToXML());
                return xTender;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "DepositTenderLineItem.ToXml", ex);
                }

                throw;
            }
        }

    }
}
