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
    /// Used when tranfering one tender from or to the till.
    /// </summary>
    [Serializable]
    public class TenderTransferTransaction :  PosTransaction
    {
        #region Enums
        public enum TransferToFromTypes : int
        {
            /// <summary>
            /// 0
            /// </summary>
            Till = 0,
            /// <summary>
            /// 1
            /// </summary>
            StoreSafe = 1,
            /// <summary>
            /// 2
            /// </summary>
            ExternalDepository = 2
        }
        #endregion

        private bool transferedToTill;              //Is true if tender was transfer to the till, else false if tender transfer from the till
        private TransferToFromTypes transferToFromType; //The source or destination the tender is tranfered to or from.
        private string sourceOrDestinationId;       //The source or destionation id.

        #region Properties
        /// <summary>
        /// Is true if tender was transfer to the till, else false if 
        /// tender transfer from the till
        /// </summary>
        public bool TransferedToTill
        {
            get { return transferedToTill; }
            set { transferedToTill = value; }
        }
        /// <summary>
        /// The source or destination the tender is tranfered to or from.
        /// </summary>
        public TransferToFromTypes TransferToFromType
        {
            get { return transferToFromType; }
            set { transferToFromType = value; }
        }
        /// <summary>
        /// The source or destionation id.
        /// </summary>
        public string SourceOrDestinationId
        {
            get { return sourceOrDestinationId; }
            set { sourceOrDestinationId = value; }
        }
        #endregion

        public List<TenderLineItem> TenderLines { get; set; }
        
        public TenderTransferTransaction()
        {
            TenderLines = new List<TenderLineItem>();
        }

        ~TenderTransferTransaction()
        {
            TenderLines.Clear();
        }

        public override void Save()
        {
        }

        public override TypeOfTransaction TransactionType()
        {
            return TypeOfTransaction.RemoveTender;
        }

        public override object Clone()
        {
            TenderTransferTransaction transaction = new TenderTransferTransaction();
            Populate(transaction);
            return transaction;
        }

        protected void Populate(TenderTransferTransaction transaction)
        {
            base.Populate(transaction);
            transaction.TenderLines = CollectionHelper.Clone<TenderLineItem, List<TenderLineItem>>(TenderLines);
            transaction.sourceOrDestinationId = sourceOrDestinationId;
            transaction.transferedToTill = transferedToTill;
            transaction.transferToFromType = transferToFromType;
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
                                case "transferedToTill":
                                    transferedToTill = Conversion.ToBool(transElem.Value.ToString());
                                    break;

                                case "transferToFromType":
                                    transferToFromType = (TransferToFromTypes)Convert.ToInt32(transElem.Value.ToString());
                                    break;

                                case "sourceOrDestinationId":
                                    sourceOrDestinationId = transElem.Value.ToString();
                                    break;

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
                XElement xTenderTrans = new XElement("TenderTransferTransaction",
                    new XElement("transferedToTill", transferedToTill),
                    new XElement("transferToFromType", (int)transferToFromType),
                    new XElement("sourceOrDestinationId", sourceOrDestinationId)
                );

                XElement xItems = new XElement("TenderLines");
                foreach (TenderLineItem ili in TenderLines)
                {
                    xItems.Add(ili.ToXML());
                }
                xTenderTrans.Add(xItems);

                xTenderTrans.Add(base.ToXML());
                return xTenderTrans;
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
