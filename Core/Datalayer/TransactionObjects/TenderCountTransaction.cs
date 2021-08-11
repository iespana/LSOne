using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects
{
    [Serializable]
    public abstract class TenderCountTransaction : PosTransaction
    {
        protected List<TenderLineItem> tenderLines;
        protected List<InfoCodeLineItem> infoCodeLines;

        public List<TenderLineItem> TenderLines
        {
            set { tenderLines = value; }
            get { return tenderLines; }
        }

        public List<InfoCodeLineItem> InfoCodeLines
        {
            set { infoCodeLines = value; }
            get { return infoCodeLines; }
        }
        
        public TenderCountTransaction()
        {
            tenderLines = new List<TenderLineItem>();
            infoCodeLines = new List<InfoCodeLineItem>();
        }

        ~TenderCountTransaction()
        {
            tenderLines.Clear();

            if (infoCodeLines != null)
                infoCodeLines.Clear();
        }

        protected void Populate(TenderCountTransaction transaction)
        {
            base.Populate(transaction);
            transaction.tenderLines = CollectionHelper.Clone<TenderLineItem, List<TenderLineItem>>(tenderLines);
            transaction.infoCodeLines = CollectionHelper.Clone<InfoCodeLineItem, List<InfoCodeLineItem>>(infoCodeLines);
        }

        public override void Save()
        {
        }

        /// <summary>
        /// Adds a tenderline to the collection of tenderlines that belong to this transaction
        /// </summary>
        /// <param name="tenderLineItem"></param>
        public void Add(TenderLineItem tenderLineItem)
        {
            this.EndDateTime = DateTime.Now;
            this.tenderLines.Add(tenderLineItem);
            tenderLineItem.LineId = this.tenderLines.Count;
        }

        /// <summary>
        /// Adds an infocode line to the collection of infocode lines that belongs to this transaction
        /// </summary>
        /// <param name="infoCodeLineItem"></param>
        public void Add(InfoCodeLineItem infoCodeLineItem)
        {
            infoCodeLineItem.LineId = this.infoCodeLines.Count + 1;
            this.infoCodeLines.Add(infoCodeLineItem);
        }

        /// <summary>
        /// Looks through existing infocodes and check if the infocode already exists on the transaction.
        /// </summary>
        /// <param name="infoCodeId"></param>
        /// <returns></returns>
        public Boolean InfoCodeNeeded(string infoCodeId)
        {
            foreach (InfoCodeLineItem infoCodeLineItem in this.infoCodeLines)
            {
                if (infoCodeLineItem.InfocodeId == infoCodeId)
                {
                    return false;
                }
            }
            return true;
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


        public List<InfoCodeLineItem> CreateInfocodeLines(XElement xItems)
        {
            List<InfoCodeLineItem> SaleLines = new List<InfoCodeLineItem>();

            if (xItems.HasElements)
            {
                IEnumerable<XElement> xInfocodeItems = xItems.Elements("InfoCodeLineItem");
                foreach (XElement xInfocodeItem in xInfocodeItems)
                {
                    if (xInfocodeItem.HasElements)
                    {
                        InfoCodeLineItem newInfocode = new InfoCodeLineItem();
                        newInfocode.ToClass(xInfocodeItem);
                        SaleLines.Add(newInfocode);
                    }
                }
            }
            return SaleLines;
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
                XElement xTenderCount = new XElement("TenderCountTransaction");
                XElement xTenderLines = new XElement("TenderLines");
                foreach (TenderLineItem tli in tenderLines)
                {
                    xTenderLines.Add(tli.ToXML());
                }
                xTenderCount.Add(xTenderLines);
                XElement xInfocodes = new XElement("InfocodeLines");
                foreach (InfoCodeLineItem ici in infoCodeLines)
                {
                    xInfocodes.Add(ici.ToXML());
                }
                xTenderCount.Add(xInfocodes);                

                xTenderCount.Add(base.ToXML());
                return xTenderCount;
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
                foreach (XElement transElem in transElements)
                {
                    try
                    {
                        if (!transElem.IsEmpty)
                        {
                            switch (transElem.Name.ToString())
                            {
                                case "TenderLines":
                                    tenderLines = CreateTenderLines(transElem);
                                    break;

                                case "InfocodeLines":
                                    infoCodeLines = CreateInfocodeLines(transElem);
                                    break;

                                case "PosTransaction":
                                    base.ToClass(transElem, errorLogger);
                                    break;
                            }
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
