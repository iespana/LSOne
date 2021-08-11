using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects
{
    /// <summary>
    /// A class used when no items are sold but one tender is changed to another.
    /// The tender taken from the drawer should equal the tender paid.
    /// </summary>
    [Serializable]
    public class TenderChangeTransaction : PosTransaction
    {
        public List<TenderLineItem> TenderLines;
        
        public TenderChangeTransaction()
        {
            TenderLines = new List<TenderLineItem>();
        }

        ~TenderChangeTransaction()
        {
            TenderLines.Clear();
        }

        public override void Save()
        {
        }
        public override TypeOfTransaction TransactionType()
        {
            return TypeOfTransaction.ChangeTender;
        }

        public override object Clone()
        {
            TenderChangeTransaction transaction = new TenderChangeTransaction();
            Populate(transaction);
            return transaction;
        }

        protected void Populate(TenderChangeTransaction transaction)
        {
            base.Populate(transaction);
            transaction.TenderLines = CollectionHelper.Clone<TenderLineItem, List<TenderLineItem>>(TenderLines);
        }
        /// <summary>
        /// Adds a tendeline to the collection of tenderlines that belong to this transaction
        /// </summary>
        /// <param name="tenderLineItem"></param>
        public void Add(TenderLineItem tenderLineItem)
        {
            this.EndDateTime = DateTime.Now;
            this.TenderLines.Add(tenderLineItem);
        }

        public List<TenderLineItem> CreateTenderLines(XElement xItems)
        {
            List<TenderLineItem> TenderLines = new List<TenderLineItem>();

            if (xItems.HasElements)
            {
                IEnumerable<XElement> xTenderItems = xItems.Elements();
                foreach (XElement xTender in xTenderItems)
                {
                    if (xTender.HasElements)
                    {
                        switch (xTender.Name.ToString())
                        {
                            case "TenderLineItem":
                                TenderLineItem tli = new TenderLineItem();
                                tli.ToClass(xTender);
                                tli.Transaction = this;
                                TenderLines.Add(tli);
                                break;
                            case "CardTenderLineItem":
                                CardTenderLineItem ctli = new CardTenderLineItem();
                                ctli.ToClass(xTender);
                                ctli.Transaction = this;
                                TenderLines.Add(ctli);
                                break;
                            case "ChequeTenderLineItem":
                                ChequeTenderLineItem cqtli = new ChequeTenderLineItem();
                                cqtli.ToClass(xTender);
                                cqtli.Transaction = this;
                                TenderLines.Add(cqtli);
                                break;
                            case "CorporateCardTenderLineItem":
                                CorporateCardTenderLineItem cctli = new CorporateCardTenderLineItem();
                                cctli.ToClass(xTender);
                                cctli.Transaction = this;
                                TenderLines.Add(cctli);
                                break;
                            case "CouponTenderLineItem":
                                CouponTenderLineItem cotli = new CouponTenderLineItem();
                                cotli.ToClass(xTender);
                                cotli.Transaction = this;
                                TenderLines.Add(cotli);
                                break;
                            case "CreditMemoTenderLineItem":
                                CreditMemoTenderLineItem cmtli = new CreditMemoTenderLineItem();
                                cmtli.ToClass(xTender);
                                cmtli.Transaction = this;
                                TenderLines.Add(cmtli);
                                break;
                            case "CustomerTenderLineItem":
                                CustomerTenderLineItem cutli = new CustomerTenderLineItem();
                                cutli.ToClass(xTender);
                                cutli.Transaction = this;
                                TenderLines.Add(cutli);
                                break;
                            case "GiftCertificateTenderLineItem":
                                GiftCertificateTenderLineItem gctli = new GiftCertificateTenderLineItem();
                                gctli.ToClass(xTender);
                                gctli.Transaction = this;
                                TenderLines.Add(gctli);
                                break;
                            case "LoyaltyTenderLineItem":
                                LoyaltyTenderLineItem ltli = new LoyaltyTenderLineItem();
                                ltli.ToClass(xTender);
                                ltli.Transaction = this;
                                TenderLines.Add(ltli);
                                break;
                            case "TradeInTenderLineItem":
                                TradeInTenderLineItem trtli = new TradeInTenderLineItem();
                                trtli.ToClass(xTender);
                                trtli.Transaction = this;
                                TenderLines.Add(trtli);
                                break;
                        }
                    }
                }
            }
            return TenderLines;
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
                                case "TenderLines":
                                    TenderLines = CreateTenderLines(transElem);
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
                XElement xTenderChange = new XElement("TenderChangeTransaction");

                XElement xItems = new XElement("TenderLines");
                foreach (TenderLineItem ili in TenderLines)
                {
                    xItems.Add(ili.ToXML());
                }
                xTenderChange.Add(xItems);

                xTenderChange.Add(base.ToXML());
                return xTenderChange;
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
