using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.TransactionObjects.Line.CreditMemoItem;
using LSOne.DataLayer.TransactionObjects.Line.CustomerDepositItem;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects
{
    /// <summary>
    /// Used when paying into a customer account.
    /// </summary>
    [Serializable]
    public class CustomerPaymentTransaction : PosTransaction, ICustomerPaymentTransaction
    {
        //The id of the number sequence for the receipt ids

        public List<TenderLineItem> TenderLines;
        public List<TenderLineItem> OriginalTenderLines;
        public List<InfoCodeLineItem> InfoCodeLines;

        #region Properties

        /// <summary>
        /// Information about the customer on the transaction. Read-only property
        /// To set the customer use the Add function
        /// </summary>
        public Customer Customer { get; private set; }        

        public decimal Payment { get; set; }
        public CreditMemoItem CreditMemoItem { get; set; }

        public decimal Amount { get; set; }

        public int NoOfDeposits { get; set; }

        public decimal TransSalePmtDiff { get; set; }

        public decimal RoundingSalePmtDiff { get; set; }

        public ICustomerDepositItem ICustomerDepositItem
        {
            get
            {
                return CustomerDepositItem;
            } 
            set
            {
                CustomerDepositItem = (CustomerDepositItem)value;
            }
        }

        public CustomerDepositItem CustomerDepositItem { get; set; }

        /// <summary>
        /// The id of the number sequence for the receipt id's
        /// </summary>
        public string ReceiptIdNumberSequence { get; set; }

        public override IEnumerable<ITenderLineItem> ITenderLines
        {
            get
            {
                return TenderLines.Cast<ITenderLineItem>();
            }
        }

        public override IEnumerable<ITenderLineItem> IOriginalTenderLines
        {
            get
            {
                return OriginalTenderLines.Cast<ITenderLineItem>();
            }
            set
            {
                OriginalTenderLines = value.Cast<TenderLineItem>().ToList();
            }
        }

        #endregion

        public CustomerPaymentTransaction(string currencyCode)
        {
            StoreCurrencyCode = currencyCode;
            Initialize();
        }

        public override void Save()
        {
        }

        public override TypeOfTransaction TransactionType()
        {
            return TypeOfTransaction.Payment;
        }

        public override object Clone()
        {
            CustomerPaymentTransaction transaction = new CustomerPaymentTransaction(StoreCurrencyCode);
            Populate(transaction);
            return transaction;
        }

        protected void Populate(CustomerPaymentTransaction transaction)
        {
            base.Populate(transaction);
            transaction.Customer = (Customer)Customer.Clone();
            transaction.Payment = Payment;
            transaction.CreditMemoItem = CreditMemoItem;
            transaction.Amount = Amount;
            transaction.NoOfDeposits = NoOfDeposits;
            transaction.TransSalePmtDiff = TransSalePmtDiff;
            transaction.ReceiptIdNumberSequence = ReceiptIdNumberSequence;
            transaction.RoundingSalePmtDiff = RoundingSalePmtDiff;
            if (CustomerDepositItem != null) //Null is a state that is being used.
            {
                transaction.CustomerDepositItem = (CustomerDepositItem) CustomerDepositItem.Clone();
            }
            transaction.TenderLines = CollectionHelper.Clone<TenderLineItem, List<TenderLineItem>>(TenderLines);
            transaction.InfoCodeLines = CollectionHelper.Clone<InfoCodeLineItem, List<InfoCodeLineItem>>(InfoCodeLines);
        }

        private void Initialize()
        {
            BeginDateTime = DateTime.Now;
            TenderLines = new List<TenderLineItem>();
            InfoCodeLines = new List<InfoCodeLineItem>();
            Customer = new Customer();
            CreditMemoItem = new CreditMemoItem(this);
        }

        ~CustomerPaymentTransaction()
        {
            TenderLines.Clear();
        }

        /// <summary>
        /// Adds a tendeline to the collection of tenderlines that belong to this transaction
        /// </summary>
        /// <param name="tenderLineItem"></param>
        public void Add(TenderLineItem tenderLineItem)
        {
            tenderLineItem.Transaction = this;
            Payment += tenderLineItem.Amount;
            TransSalePmtDiff = Amount - Payment;
            tenderLineItem.EndDateTime = DateTime.Now;
            TenderLines.Add(tenderLineItem);
            tenderLineItem.LineId = TenderLines.Count;
        }

        /// <summary>
        /// Return a tender item for a certain tender line id.
        /// </summary>
        /// <param name="lineId">The unique line id of the tender line.</param>
        /// <returns>The sale line item.</returns>
        public TenderLineItem GetTenderItem(int lineId)
        {
            return TenderLines.FirstOrDefault(tenderLineItem => tenderLineItem.LineId == lineId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public TenderLineItem VoidPaymentLine(int lineId)
        {
            TenderLineItem result = null;
            foreach (TenderLineItem tenderLineItem in
                TenderLines.Where(tenderLineItem => tenderLineItem.LineId == lineId))
            {
                decimal sign;
                if (tenderLineItem.Voided)
                {
                    sign = 1;
                    tenderLineItem.Voided = false;
                }
                else
                {
                    sign = -1;
                    tenderLineItem.Voided = true;
                }

                Payment += sign * tenderLineItem.Amount;
                TransSalePmtDiff = Amount - Payment;


                result = tenderLineItem;
            }
            return result;
        }

        public void Add(Customer customer)
        {
            //If the customer is null then there is no need to move forwards
            if (customer == null)
            {
                customer = new Customer();
            }
            Customer = customer;            
        }

        public void Add(Customer customer, bool returnCustomer)
        {
            Add(customer);
            if (Customer.ID != RecordIdentifier.Empty)
                Customer.ReturnCustomer = returnCustomer;
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
                        TenderLineItem tli = new TenderLineItem();

                        switch (xTender.Name.ToString())
                        {
                            case "TenderLineItem":
                                break;
                            case "CardTenderLineItem":
                                tli = new CardTenderLineItem();
                                break;
                            case "ChequeTenderLineItem":
                                tli = new ChequeTenderLineItem();
                                break;
                            case "CorporateCardTenderLineItem":
                                tli = new CorporateCardTenderLineItem();
                                break;
                            case "CouponTenderLineItem":
                                tli = new CouponTenderLineItem();
                                break;
                            case "CreditMemoTenderLineItem":
                                tli = new CreditMemoTenderLineItem();
                                break;
                            case "CustomerTenderLineItem":
                                tli = new CustomerTenderLineItem();
                                break;
                            case "GiftCertificateTenderLineItem":
                                tli = new GiftCertificateTenderLineItem();
                                break;
                            case "LoyaltyTenderLineItem":
                                tli = new LoyaltyTenderLineItem();
                                break;
                            case "TradeInTenderLineItem":
                                tli = new TradeInTenderLineItem();
                                break;
                        }
                        tli.ToClass(xTender);
                        tli.Transaction = this;
                        TenderLines.Add(tli);
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
                XElement xCustomerPaym;

                if (CustomerDepositItem == null)
                {
                    xCustomerPaym = new XElement("CustomerPaymentTransaction",
                        new XElement("customer", Customer.ToXML()),
                        new XElement("payment", Payment.ToString()),
                        new XElement("amount", Amount.ToString()),
                        new XElement("RoundingSalePmtDiff", RoundingSalePmtDiff.ToString()),
                        new XElement("noOfDeposits", NoOfDeposits),
                        new XElement("transSalePmtDiff", TransSalePmtDiff.ToString()),
                        new XElement("receiptIdNumberSequence", ReceiptIdNumberSequence),
                        new XElement("customerDepositItem")
                    );
                }
                else
                {
                    xCustomerPaym = new XElement("CustomerPaymentTransaction",
                        new XElement("customer", Customer.ToXML()),
                        new XElement("payment", Payment.ToString()),
                        new XElement("amount", Amount.ToString()),
                        new XElement("RoundingSalePmtDiff", RoundingSalePmtDiff.ToString()),
                        new XElement("noOfDeposits", NoOfDeposits),
                        new XElement("transSalePmtDiff", TransSalePmtDiff.ToString()),
                        new XElement("receiptIdNumberSequence", ReceiptIdNumberSequence),
                        new XElement("customerDepositItem", CustomerDepositItem.ToXML())
                    );

                }

                XElement xTenderLines = new XElement("TenderLines");
                foreach (TenderLineItem tli in TenderLines)
                {
                    xTenderLines.Add(tli.ToXML());
                }
                xCustomerPaym.Add(xTenderLines);

                XElement xInfocodes = new XElement("InfocodeLines");
                foreach (InfoCodeLineItem ici in InfoCodeLines)
                {
                    xInfocodes.Add(ici.ToXML());
                }
                xCustomerPaym.Add(xInfocodes);

                xCustomerPaym.Add(base.ToXML());
                return xCustomerPaym;
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
                            case "customer":
                                Customer.ToClass(transElem, errorLogger);
                                break;
                            case "payment":
                                Payment = Convert.ToDecimal(transElem.Value);
                                break;
                            case "amount":
                                Amount = Convert.ToDecimal(transElem.Value);
                                break;
                            case "RoundingSalePmtDiff":
                                RoundingSalePmtDiff = Convert.ToDecimal(transElem.Value);
                                break;
                            case "noOfDeposits":
                                NoOfDeposits = Convert.ToInt32(transElem.Value);
                                break;
                            case "transSalePmtDiff":
                                TransSalePmtDiff = Convert.ToDecimal(transElem.Value);
                                break;
                            case "receiptIdNumberSequence":
                                ReceiptIdNumberSequence = transElem.Value;
                                break;
                            case "customerDepositItem":
                                CustomerDepositItem = new CustomerDepositItem();
                                CustomerDepositItem.ToClass(transElem, errorLogger);
                                break;
                            case "PosTransaction":
                                base.ToClass(transElem);
                                break;
                            case "TenderLines":
                                TenderLines = CreateTenderLines(transElem);
                                break;
                            case "InfocodeLines":
                                InfoCodeLines = CreateInfocodeLines(transElem);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error, ex.Message, ex);
                        }
                    }
                }
            }

        }
    }
}
