using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using LSRetail.Utilities.DataTypes;
using LSRetail.StoreController.BusinessObjects.Transactions.Line;
using LSRetail.Utilities.ErrorHandling;
using LSRetail.POS.BusinessObjects.Transactions;

namespace LSRetail.POS.BusinessObjects.Transaction.Line
{
    /// <summary>
    /// Is used as a basic class for other tender types and for cash payments.
    /// </summary>
    [Serializable]
    public class TenderLineItem : LineItem
    {
        #region Member variables

        /// <summary>
        /// The transaction this line belongs to
        /// </summary>
        PosTransaction transaction;
        /// <summary>
        /// The id for the Tendertype
        /// </summary>
        private string tenderTypeId;
        /// <summary>
        /// Payment amount
        /// </summary>
        private decimal amount;
        /// <summary>
        /// The amount in a foreign currency.
        /// </summary>
        private decimal foreignCurrencyAmount;
        /// <summary>
        /// The amount in the company currency as calculated from the entry in CompanyInfo table.
        /// </summary>
        private decimal companyCurrencyAmount = 0M;
        /// <summary>
        /// The exchange rate between any paid amount and the company currency.
        /// </summary>
        private decimal exchrateMST = 0M;
        /// <summary>
        /// The exchange rate used to calculate the amount.
        /// </summary>
        private decimal exchangeRate;
        /// <summary>
        /// The code to identify the currency.
        /// </summary>
        private string currencyCode;
        /// <summary>
        /// Should the drawer be opened?
        /// </summary>
        private bool openDrawer;
        /// <summary>
        /// If 0 > change - amount >= minimumChangeAmount, then
        /// </summary>
        private string changeTenderID;
        /// <summary>
        /// changeTenderID will be used as changeID, else tenderTypeID will be used 
        /// </summary>
        private decimal minimumChangeAmount;
        private string aboveMinimumTenderId;
        /// <summary>
        /// Is the tender item a change back tender line - needed for Return Transactions
        /// </summary>                                     
        private bool changeBack;

        /// <summary>
        /// Infocode comment on the tender line
        /// </summary>
        private string comment;                     //Comment field used for infocode comments

        #endregion

        #region Properties

        public string AboveMinimumTenderId
        {

            get { return aboveMinimumTenderId; }
            set { aboveMinimumTenderId = value; }
        }

        public decimal MinimumChangeAmount
        {
            get { return minimumChangeAmount; }
            set { minimumChangeAmount = value; }
        }

        public string ChangeTenderID
        {
            get { return changeTenderID; }
            set { changeTenderID = value; }
        }
        /// <summary>
        /// The transaction this line belongs to
        /// </summary>
        public PosTransaction Transaction
        {
            get { return transaction; }
            set { transaction = value; }
        }

        /// <summary>
        /// The id for the tendertype
        /// </summary>
        public string TenderTypeId
        {
            get { return tenderTypeId; }
            set { tenderTypeId = value; }
        }

        /// <summary>
        /// The payment amount
        /// </summary>
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        /// <summary>
        /// The amount in a foreign currency
        /// </summary>
        public decimal ForeignCurrencyAmount
        {
            get { return foreignCurrencyAmount; }
            set { foreignCurrencyAmount = value; }
        }
        /// <summary>
        /// The exchange rate used to calculate the amount.
        /// </summary>
        public decimal ExchangeRate
        {
            get { return exchangeRate; }
            set { exchangeRate = value; }
        }
        /// <summary>
        /// The code to identify the currency.
        /// </summary>
        public string CurrencyCode
        {
            get { return currencyCode; }
            set { currencyCode = value; }
        }
        /// <summary>
        /// The Company exchange amount used at the Head Office (as retrieved from the companyinfo table).
        /// </summary>
        public decimal CompanyCurrencyAmount
        {
            get { return companyCurrencyAmount; }
            set { companyCurrencyAmount = value; }
        }
        /// <summary>
        /// The exchange rate between any paid amount and the company currency.
        /// </summary>
        public decimal ExchrateMST
        {
            get { return exchrateMST; }
            set { exchrateMST = value; }
        }
        /// <summary>
        /// Should the drawer be opened.
        /// </summary>
        public bool OpenDrawer
        {
            get { return openDrawer; }
            set { openDrawer = value; }
        }

        /// <summary>
        /// If true then the tender item was created as a Change back tender item
        /// </summary>
        public bool ChangeBack
        {
            get { return changeBack; }
            set { changeBack = value; }
        }

        /// <summary>
        /// Infocode comment on the tender line
        /// </summary>
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        #endregion

        public TenderLineItem()
        {
            changeBack = false;
        }

        //public void Add(InfoCodeLineItem infoCodeLineItem)
        //{
        //    base.Add(infoCodeLineItem);            
        //}

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
                                    case "tenderTypeId":
                                        tenderTypeId = xVariable.Value.ToString();
                                        break;
                                    case "amount":
                                        amount = Convert.ToDecimal(xVariable.Value.ToString());
                                        break;
                                    case "foreignCurrencyAmount":
                                        foreignCurrencyAmount = Convert.ToDecimal(xVariable.Value.ToString());
                                        break;
                                    case "companyCurrencyAmount":
                                        companyCurrencyAmount = Convert.ToDecimal(xVariable.Value.ToString());
                                        break;
                                    case "exchrateMST":
                                        exchrateMST = Convert.ToDecimal(xVariable.Value.ToString());
                                        break;
                                    case "exchangeRate":
                                        exchangeRate = Convert.ToDecimal(xVariable.Value.ToString());
                                        break;
                                    case "currencyCode":
                                        currencyCode = xVariable.Value.ToString();
                                        break;
                                    case "openDrawer":
                                        openDrawer = Conversion.ToBool(xVariable.Value.ToString());
                                        break;
                                    case "changeTenderID":
                                        changeTenderID = xVariable.Value.ToString();
                                        break;
                                    case "minimumChangeAmount":
                                        minimumChangeAmount = Convert.ToDecimal(xVariable.Value.ToString());
                                        break;
                                    case "aboveMinimumTenderId":
                                        aboveMinimumTenderId = xVariable.Value.ToString();
                                        break;
                                    case "changeBack":
                                        changeBack = Conversion.ToBool(xVariable.Value.ToString());
                                        break;
                                    case "comment":
                                        comment = xVariable.Value.ToString();
                                        break;
                                    case "InfocodeLines":
                                        InfoCodeLines = CreateInfocodeLines(xVariable);
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
                                    errorLogger.LogMessage(LogMessageType.Error, "TenderLineItem:" + xVariable.Name.ToString(), ex);
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
                    errorLogger.LogMessage(LogMessageType.Error, "TenderLineItem.ToClass", ex);
                }

                throw ex;
            }
        }

        private LinkedList<InfoCodeLineItem> CreateInfocodeLines(XElement xItems)
        {
            try
            {
                LinkedList<InfoCodeLineItem> InfocodeLines = new LinkedList<InfoCodeLineItem>();

                if (xItems.HasElements)
                {
                    IEnumerable<XElement> xInfocodeItems = xItems.Elements("InfoCodeLineItem");
                    foreach (XElement xInfocodeItem in xInfocodeItems)
                    {
                        if (xInfocodeItem.HasElements)
                        {
                            InfoCodeLineItem newInfocode = new InfoCodeLineItem();
                            newInfocode.ToClass(xInfocodeItem);
                            InfocodeLines.AddLast(newInfocode);
                        }
                    }
                }

                return InfocodeLines;
            }
            catch (Exception ex)
            {
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
                XElement xTender = new XElement("TenderLineItem",
                    new XElement("tenderTypeId", tenderTypeId),
                    new XElement("amount", amount.ToString()),
                    new XElement("foreignCurrencyAmount", foreignCurrencyAmount.ToString()),
                    new XElement("companyCurrencyAmount", companyCurrencyAmount.ToString()),
                    new XElement("exchrateMST", exchrateMST.ToString()),
                    new XElement("exchangeRate", exchangeRate.ToString()),
                    new XElement("currencyCode", currencyCode),
                    new XElement("openDrawer", openDrawer),
                    new XElement("changeTenderID", changeTenderID),
                    new XElement("minimumChangeAmount", minimumChangeAmount.ToString()),
                    new XElement("aboveMinimumTenderId", aboveMinimumTenderId),
                    new XElement("changeBack", changeBack),
                    new XElement("comment", comment)
                );

                #region Infocodes
                XElement xInfocodes = new XElement("InfocodeLines");
                foreach (InfoCodeLineItem ici in InfoCodeLines)
                {
                    xInfocodes.Add(ici.ToXML());
                }
                xTender.Add(xInfocodes);
                #endregion

                xTender.Add(base.ToXML());
                return xTender;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "TenderLineItem.ToXml", ex);
                }

                throw ex;
            }
        }
    }
}
