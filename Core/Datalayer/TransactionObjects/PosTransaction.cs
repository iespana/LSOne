using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Auditing;
using LSOne.DataLayer.BusinessObjects.EFT;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.TransactionObjects.Receipts;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportClasses.Employee;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects
{
    /// <summary>
    /// A basic abstract class for the retail- and controltransaction classes.
    /// </summary>
    [Serializable]
    public abstract class PosTransaction : IPosTransaction
    {
        #region Member variables

        //TimeOfTransaction
        private Date beginDateTime = Date.Empty;     //The start date and time of the transaction
        private Date endDateTime = Date.Empty;       //The end date and time of the transaction
        //Shift
        private Date shiftDate = Date.Empty;         //The date the shift belongs to

        private Date businessDay = Date.Empty;
        private Date businessSystemDay = Date.Empty;


        #endregion

        #region Properties

        /// <summary>
        /// Should the receipt for the transaction be printed, emailed or even both.
        /// </summary>
        public ReceiptSettingsEnum ReceiptSettings { get; set; }

        /// <summary>
        /// If the receipt for the transaction should be emailed the adderss is kept here.
        /// </summary>
        public string ReceiptEmailAddress { get; set; }

        public List<POSOperations> OperationStack { get; set; }

        /// <summary>
        /// If true then the transaction has been returned using the local database
        /// </summary>
        public bool ReturnedLocally { get; set; }

        /// <summary>
        /// Used in Hospitality to know if the transaction has already been sent to kitchen or printing station
        /// </summary>
        public bool HasBeenSentToStation { get; set; }

        /// <summary>
        /// If true then this transaction was recalled and re-created when the POS was started
        /// </summary>
        public bool UnconcludedTransaction { get; set; }

        /// <summary>
        /// Controls if the periodic discounts should always be calculated when an item is added to the transaction or only at payment
        /// </summary>
        public CalculatePeriodicDiscountEnums CalcPeriodicDiscounts { get; set; }

        /// <summary>
        /// Controls if the customer discounts should always be calculated when an item is added to the transaction or only at payment
        /// </summary>
        public CalculateCustomerDiscountEnums CalcCustomerDiscounts { get; set; }

        /// <summary>
        /// Controls if amounts are displayed with or without tax in the POS
        /// </summary>
        public bool DisplayAmountsIncludingTax { get; set; }

        public Store.CalculateDiscountsFromEnum CalculateDiscountFrom { get; set; }

        /// <summary>
        /// It is assigned quite late in the transaction process and thus not available from the beginning.
        /// The assignment happens in TransactionData.cs with the call to its method 
        /// "public string GetNextTransactionId(string transactionIdNumberSequence)"
        /// that fetches the next number from the table NUMBERSEQUENCETABLE.
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// Holds the SUSPENDEDTRANS value
        /// </summary>
        public string SuspendedId { get; set; }

        /// <summary>
        /// A running number according to the SALESSEQUENCE number series
        /// </summary>
        public string SalesSequenceID { get; set; }

        /// <summary>
        /// The store id
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// The store name
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// The store address
        /// </summary>
        public string StoreAddress { get; set; }

        /// <summary>
        /// The terminal id. A unique id for each terminal inside each store.
        /// </summary>
        public string TerminalId { get; set; }

        /// <summary>
        /// This is the internal primary key in the POSTRANSACTION table. Consists of the terminal id and a sequential number.
        /// </summary>
        public string ReceiptId { get; set; }

        /// <summary>
        /// The Operator entering the transaction on the POS
        /// </summary>
        public Employee Cashier { get; set; }

        /// <summary>
        /// The orginal terminal the transaction was created on, if the transaction was transfered from another terminal
        /// </summary>
        public string CreatedOnTerminalId { get; set; }

        /// <summary>
        /// The status of the transaction, i.e voided, posted, training etc.
        /// </summary>
        public TransactionStatus EntryStatus { get; set; }

        public virtual IEnumerable<ITenderLineItem> ITenderLines { get; }

        public virtual IEnumerable<ITenderLineItem> IOriginalTenderLines { get; set; }

        public List<OperationAuditing> AuditingLines { get; set; }

        /// <summary>
        /// The start date and time of the transaction
        /// </summary>
        public DateTime BeginDateTime
        {
            get { return beginDateTime.DateTime; }
            set { beginDateTime = new Date(value, true); }
        }

        /// <summary>
        /// The finishing date and time of the transaction
        /// </summary>
        public DateTime EndDateTime
        {
            get { return endDateTime.DateTime; }
            set { endDateTime = new Date(value, true); }
        }

        /// <summary>
        /// The shift id
        /// </summary>
        public string ShiftId { get; set; }

        /// <summary>
        /// The shift date
        /// </summary>
        public DateTime ShiftDate
        {
            get { return shiftDate.DateTime; }
            set { shiftDate = new Date(value, true); }
        }

        /// <summary>
        /// Was drawer opened during the transaction
        /// </summary>
        public bool OpenDrawer { get; set; }

        /// <summary>
        /// A comment insertable by the operator
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// If the till is in training mode.
        /// </summary>
        public bool Training { get; set; }

        /// <summary>
        /// If the last run operation was a valid payment operation, that is a successful one.
        /// </summary>
        public bool LastRunOperationIsValidPayment { get; set; }

        /// <summary>
        /// The last operation that was run on the transaction
        /// </summary>
        public POSOperations LastRunOperation { get; set; }

        /// <summary>
        /// The way a statement is done for the store (staff, terminal, total).
        /// </summary>
        public StatementGroupingMethod StatementMethod { get; set; }

        /// <summary>
        /// The id of the number sequence for the transaction ids
        /// </summary>
        public string TransactionIdNumberSequence { get; set; }

        /// <summary>
        /// The exchange rate between HO currency and Store currency.        
        /// </summary>
        public decimal StoreExchangeRate { get; set; }

        /// <summary>
        /// The Store currency code - i.e. GBP, USD, EUR etc.
        /// </summary>
        public string StoreCurrencyCode { get; set; }

        /// <summary>
        /// The store tax group
        /// </summary>
        public string StoreTaxGroup { get; set; }

        /// <summary>
        /// This is used for price override and when it is required to key in an item price.
        /// When true, then the value entered contains tax such that tax will not be added on top of this value.
        /// </summary>
        public bool KeyedInPriceContainsTax { get; set; }

        /// <summary>
        /// Gets or sets the original number of lines (sales, payments, tax, discounts, infocodes, etc.) created with the transaction.
        /// This should only be set when the transaction is saved on the POS.
        /// This value can be used to check if the current transaction contains the correct amount of lines as when it was created (ex. replication).
        /// </summary>
        public int OriginalNumberOfTransactionLines { get; set; }

        /// <summary>
        /// Set to true if the current selected item in the transaction should remain selected after executing a blank operation.
        /// This value is not saved to the database and must be set after each blank operation.
        /// </summary>
        public bool KeepRowSelectionOnBlankOperation { get; set; }

        public DateTime BusinessDay
        {
            get { return businessDay.DateTime; }
            set { businessDay = new Date(value, true); }
        }

        public DateTime BusinessSystemDay
        {
            get { return businessSystemDay.DateTime; }
            set { businessSystemDay = new Date(value, true); }
        }

        public List<IReceiptInfo> Receipts { get; set; }

        public IEnumerable<IReceiptInfo> IReceipts
        {
            get
            {
                return Receipts.Cast<IReceiptInfo>();
            }
        }

        /// <summary>
        /// Transaction information in HTML format that can be displayed in a HTML information panel
        /// </summary>
        public string HTMLInformation { get; set; }

        /// <summary>
        /// Hospitality information if the transaction was created through the hospitality functionality
        /// </summary>
        public HospitalityItem Hospitality { get; set; }

        /// <summary>
        /// Temporary data saved before performing an EFT payment
        /// </summary>
        public PendingEFTTransaction PendingEFTTransaction { get; set; }

        /// <summary>
        /// A list of all generated EFT transactions
        /// </summary>
        public List<PendingEFTTransaction> EFTTransactions { get; set; }

        /// <summary>
        /// The unique ID of the transaction
        /// </summary>
        public Guid ID { get; set; }



        #endregion

        protected PosTransaction()
        {
            TransactionIdNumberSequence = "";
            Comment = "";
            ShiftId = "";
            CreatedOnTerminalId = "";
            ReceiptId = "";
            TerminalId = "";
            StoreAddress = "";
            StoreName = "";
            StoreId = "";
            TransactionId = "";
            BeginDateTime = DateTime.Now;
            BusinessDay = DateTime.Now;
            BusinessSystemDay = DateTime.Now;
            StoreExchangeRate = 1M;
            UnconcludedTransaction = false;
            HasBeenSentToStation = false;
            ReturnedLocally = false;
            Cashier = new Employee();
            Receipts = new List<IReceiptInfo>();
            ReceiptSettings = ReceiptSettingsEnum.Printed;
            ReceiptEmailAddress = "";
            OperationStack = new List<POSOperations>();
            TransactionSubType = 0;
            OriginalNumberOfTransactionLines = 0;
            HTMLInformation = "";
            Hospitality = new HospitalityItem();
            PendingEFTTransaction = new PendingEFTTransaction();
            EFTTransactions = new List<PendingEFTTransaction>();
            ID = Guid.NewGuid();
        }

        public virtual int TransactionSubType { get; set; }

        public abstract void Save();

        public abstract TypeOfTransaction TransactionType();

        public abstract object Clone();

        protected void Populate(PosTransaction transaction)
        {
            transaction.BeginDateTime = beginDateTime.DateTime;
            transaction.CalcCustomerDiscounts = CalcCustomerDiscounts;
            transaction.CalcPeriodicDiscounts = CalcPeriodicDiscounts;
            transaction.CalculateDiscountFrom = CalculateDiscountFrom;
            transaction.Comment = Comment;
            transaction.CreatedOnTerminalId = CreatedOnTerminalId;
            transaction.DisplayAmountsIncludingTax = DisplayAmountsIncludingTax;
            transaction.endDateTime = endDateTime;
            transaction.EntryStatus = EntryStatus;
            transaction.KeyedInPriceContainsTax = KeyedInPriceContainsTax;
            transaction.LastRunOperationIsValidPayment = LastRunOperationIsValidPayment;
            transaction.OpenDrawer = OpenDrawer;
            transaction.Cashier = (Employee)Cashier.Clone();
            transaction.ReceiptId = ReceiptId;
            transaction.shiftDate = shiftDate;
            transaction.ShiftId = ShiftId;
            transaction.StatementMethod = StatementMethod;
            transaction.StoreAddress = StoreAddress;
            transaction.StoreCurrencyCode = StoreCurrencyCode;
            transaction.StoreExchangeRate = StoreExchangeRate;
            transaction.StoreId = StoreId;
            transaction.StoreName = StoreName;
            transaction.StoreTaxGroup = StoreTaxGroup;
            transaction.TerminalId = TerminalId;
            transaction.Training = Training;
            transaction.TransactionId = TransactionId;
            transaction.TransactionIdNumberSequence = TransactionIdNumberSequence;
            transaction.UnconcludedTransaction = UnconcludedTransaction;
            transaction.LastRunOperation = LastRunOperation;
            transaction.HasBeenSentToStation = HasBeenSentToStation;
            transaction.ReturnedLocally = ReturnedLocally;
            transaction.BusinessDay = BusinessDay;
            transaction.BusinessSystemDay = BusinessSystemDay;
            transaction.ReceiptSettings = ReceiptSettings;
            transaction.ReceiptEmailAddress = ReceiptEmailAddress;
            transaction.TransactionSubType = TransactionSubType;
            transaction.OriginalNumberOfTransactionLines = OriginalNumberOfTransactionLines;
            if (AuditingLines != null)
            {
                transaction.AuditingLines = CollectionHelper.Clone<OperationAuditing, List<OperationAuditing>>(AuditingLines);
            }
            if (Receipts != null)
            {
                transaction.Receipts = CloneReceipts(Receipts);
            }
            if (OperationStack != null)
            {
                transaction.OperationStack = CloneOperationStack(OperationStack);
            }
            transaction.HTMLInformation = HTMLInformation;
            transaction.Hospitality = (HospitalityItem) Hospitality.Clone();
            transaction.PendingEFTTransaction = (PendingEFTTransaction) PendingEFTTransaction.Clone();
            if(EFTTransactions != null)
            {
                transaction.EFTTransactions = CloneEFTTransactions(EFTTransactions);
            }
            transaction.ID = ID;
        }

        private List<IReceiptInfo> CloneReceipts(List<IReceiptInfo> receiptItems)
        {
            List<IReceiptInfo> cloneItems = new List<IReceiptInfo>();

            foreach (ReceiptInfo items in receiptItems)
            {
                cloneItems.Add((ReceiptInfo)items.Clone());
            }

            return cloneItems;
        }

        private List<PendingEFTTransaction> CloneEFTTransactions(List<PendingEFTTransaction> eftTransactions)
        {
            List<PendingEFTTransaction> cloneItems = new List<PendingEFTTransaction>();

            foreach (PendingEFTTransaction items in eftTransactions)
            {
                cloneItems.Add((PendingEFTTransaction)items.Clone());
            }

            return cloneItems;
        }

        private List<POSOperations> CloneOperationStack(List<POSOperations> operationStack)
        {
            List<POSOperations> cloneItems = new List<POSOperations>();

            foreach (POSOperations operation in operationStack)
            {
                cloneItems.Add(operation);
            }

            return cloneItems;
        }

        public bool SelectiveDiscountCalculation()
        {
            return CalcCustomerDiscounts == CalculateCustomerDiscountEnums.CalculateOnTotal || CalcPeriodicDiscounts == CalculatePeriodicDiscountEnums.CalculateOnTotal;
        }

        public void AddReceipt(string receiptString, RecordIdentifier formType, string documentName, string documentLocation, int formWidth, bool isEmailReceipt)
        {
            ReceiptInfo receipt = new ReceiptInfo()
            {
                FormType = formType,
                PrintString = receiptString,
                LineID = this.Receipts.Count + 1,
                DocumentLocation = documentLocation,
                DocumentName = documentName,
                FormWidth = formWidth,
                IsEmailReceipt = isEmailReceipt
            };

            this.Receipts.Add(receipt);
        }

        /// <summary>
        /// Sums up all transaction lines from the current transaction. This value should be set as the <see cref="OriginalNumberOfTransactionLines"/> before saving the new transaction.
        /// This should be overriden and implemented based on the type of transaction.
        /// </summary>
        /// <returns>Total transaction lines</returns>
        public virtual int CalculateTotalNumberOfTransactionLines()
        {
            return 0;
        }

        /// <summary>
        /// Serialize the transaction
        /// </summary>
        public XDocument CreateXmlTransaction()
        {
            XDocument xTrans = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));

            xTrans.Add(this.ToXML());
            return xTrans;
        }

        public static PosTransaction CreateTransFromXML(string xml, PosTransaction posTrans, PartnerObjectBase partnerObject)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return posTrans;
            }

            XDocument xTrans = null;

            try
            {
                xTrans = XDocument.Parse(xml);
            }
            catch (Exception)
            {
                return posTrans;
            }

            XElement xRoot = xTrans.Root;

            XAttribute xTransType = null;

            try
            {
                xTransType = xRoot.Element("PosTransaction").Attribute("TransactionType");
            }
            catch
            {
                return posTrans;
            }

            if (xTransType == null)
            {
                return posTrans;
            }

            if (posTrans == null)
            {
                switch ((TypeOfTransaction)Convert.ToInt32(xTransType.Value))
                {
                    case TypeOfTransaction.Sales:
                        posTrans = new RetailTransaction("", "", false);
                        break;
                    case TypeOfTransaction.BankDrop:
                        posTrans = new BankDropTransaction();
                        break;
                    case TypeOfTransaction.EndOfDay:
                        posTrans = new EndOfDayTransaction();
                        break;
                    case TypeOfTransaction.EndOfShift:
                        posTrans = new EndOfShiftTransaction();
                        break;
                    case TypeOfTransaction.FloatEntry:
                        posTrans = new FloatEntryTransaction();
                        break;
                    case TypeOfTransaction.LogOff:
                        posTrans = new LogOnOffTransaction();
                        break;
                    case TypeOfTransaction.LogOn:
                        posTrans = new LogOnOffTransaction();
                        break;
                    case TypeOfTransaction.OpenDrawer:
                        posTrans = new NoSaleTransaction();
                        break;
                    case TypeOfTransaction.Payment:
                        posTrans = new CustomerPaymentTransaction("");
                        break;
                    case TypeOfTransaction.RemoveTender:
                        posTrans = new RemoveTenderTransaction();
                        break;
                    case TypeOfTransaction.SafeDrop:
                        posTrans = new SafeDropTransaction();
                        break;
                    case TypeOfTransaction.TenderDeclaration:
                        posTrans = new TenderDeclarationTransaction();
                        break;
                    case TypeOfTransaction.SafeDropReversal:
                        posTrans = new SafeDropReversalTransaction();
                        break;
                    case TypeOfTransaction.BankDropReversal:
                        posTrans = new BankDropReversalTransaction();
                        break;
                    case TypeOfTransaction.Log:
                        posTrans = new LogTransaction();
                        break;
                }
            }
            if (posTrans != null)
            {
                posTrans.ToClass(xRoot);
            }
            if (posTrans is RetailTransaction)
            {
                ((RetailTransaction)posTrans).PartnerObject = partnerObject;
                if (((RetailTransaction)posTrans).PartnerObject != null && ((RetailTransaction)posTrans).PartnerXElement != null)
                {
                    ((RetailTransaction)posTrans).PartnerObject.ToClass(((RetailTransaction)posTrans).PartnerXElement);
                }
            }
            return posTrans;
        }

        /// <summary>
        /// Instantiates all variables according to the values in the xml
        /// </summary>
        /// <param name="xmlTrans">The xml with the object values</param>
        /// <param name="errorLogger"></param>        
        public virtual void ToClass(XElement xmlTrans, IErrorLog errorLogger = null)
        {
            if (xmlTrans.HasElements)
            {
                IEnumerable<XElement> classVariables = xmlTrans.Elements();
                foreach (XElement xVariable in classVariables)
                {
                    try
                    {
                        if (!xVariable.IsEmpty)
                        {

                            switch (xVariable.Name.ToString())
                            {
                                case "transactionId":
                                    TransactionId = xVariable.Value;
                                    break;
                                case "salesSequenceId":
                                    SalesSequenceID = xVariable.Value;
                                    break;
                                case "storeId":
                                    StoreId = xVariable.Value;
                                    break;
                                case "storeName":
                                    StoreName = xVariable.Value;
                                    break;
                                case "storeAddress":
                                    StoreAddress = xVariable.Value;
                                    break;
                                case "terminalId":
                                    TerminalId = xVariable.Value;
                                    break;
                                case "receiptId":
                                    ReceiptId = xVariable.Value;
                                    break;
                                case "employee":
                                    Cashier.ToClass(xVariable, errorLogger);
                                    break;
                                case "operatorId":  //FOR BACKWARDS COMPATABILITY SUSPENDED TRANSACTIONS F.EX.
                                    Cashier.ID = xVariable.Value;
                                    break;
                                case "operatorName": //FOR BACKWARDS COMPATABILITY SUSPENDED TRANSACTIONS F.EX.
                                    Cashier.Name = xVariable.Value;
                                    break;
                                case "operatorNameOnReceipt": //FOR BACKWARDS COMPATABILITY SUSPENDED TRANSACTIONS F.EX.
                                    Cashier.NameOnReceipt = xVariable.Value;
                                    break;
                                case "createdOnTerminalId":
                                    CreatedOnTerminalId = xVariable.Value;
                                    break;
                                case "entryStatus":
                                    EntryStatus = (TransactionStatus)Conversion.XmlStringToInt(xVariable.Value);
                                    break;
                                case "beginDateTime":
                                    beginDateTime = new Date(Conversion.XmlStringToDateTime(xVariable.Value));
                                    break;
                                case "endDateTime":
                                    endDateTime = new Date(Conversion.XmlStringToDateTime(xVariable.Value));
                                    break;
                                case "shiftId":
                                    ShiftId = xVariable.Value;
                                    break;
                                case "shiftDate":
                                    shiftDate = new Date(Conversion.XmlStringToDateTime(xVariable.Value));
                                    break;
                                case "openDrawer":
                                    OpenDrawer = Conversion.XmlStringToBool(xVariable.Value);
                                    break;
                                case "comment":
                                    Comment = xVariable.Value;
                                    break;
                                case "training":
                                    Training = Conversion.XmlStringToBool(xVariable.Value);
                                    break;
                                case "lastRunOperationIsValidPayment":
                                    LastRunOperationIsValidPayment = Conversion.XmlStringToBool(xVariable.Value);
                                    break;
                                case "lastRunOperation":
                                    LastRunOperation = (POSOperations)Conversion.XmlStringToInt(xVariable.Value);
                                    break;
                                case "operationStack":
                                    OperationStack = CreateOperationStack(xVariable);
                                    break;
                                case "statementMethod":
                                    StatementMethod = (StatementGroupingMethod)Conversion.XmlStringToInt(xVariable.Value);
                                    break;
                                case "transactionIdNumberSequence":
                                    TransactionIdNumberSequence = xVariable.Value;
                                    break;
                                case "storeExchangeRate":
                                    StoreExchangeRate = Conversion.XmlStringToDecimal(xVariable.Value);
                                    break;
                                case "storeCurrencyCode":
                                    StoreCurrencyCode = xVariable.Value;
                                    break;
                                case "storeTaxGroup":
                                    StoreTaxGroup = xVariable.Value;
                                    break;
                                case "keyedInPriceContainsTax":
                                    KeyedInPriceContainsTax = Conversion.XmlStringToBool(xVariable.Value);
                                    break;
                                case "calculateDiscountFrom":
                                    CalculateDiscountFrom = (Store.CalculateDiscountsFromEnum)Conversion.XmlStringToInt(xVariable.Value);
                                    break;
                                case "displayAmountsIncludingTax":
                                    DisplayAmountsIncludingTax = Conversion.XmlStringToBool(xVariable.Value);
                                    break;
                                case "CalcPeriodicDiscounts":
                                    CalcPeriodicDiscounts = (CalculatePeriodicDiscountEnums)Conversion.XmlStringToInt(xVariable.Value);
                                    break;
                                case "CalcCustomerDiscounts":
                                    CalcCustomerDiscounts = (CalculateCustomerDiscountEnums)Conversion.XmlStringToInt(xVariable.Value);
                                    break;
                                case "UnconcludedTransaction":
                                    UnconcludedTransaction = Conversion.XmlStringToBool(xVariable.Value);
                                    break;
                                case "HasBeenSentToStation":
                                    HasBeenSentToStation = Conversion.XmlStringToBool(xVariable.Value);
                                    break;
                                case "ReturnedLocally":
                                    ReturnedLocally = Conversion.XmlStringToBool(xVariable.Value);
                                    break;
                                case OperationAuditing.XmlElementName + "s":
                                    AuditingLines = CreateAuditLines(xVariable);
                                    break;
                                case ReceiptInfo.XmlElementName + "s":
                                    Receipts = CreateReceiptLines(xVariable);
                                    break;
                                case "receiptSettings":
                                    ReceiptSettings = (ReceiptSettingsEnum)Conversion.XmlStringToInt(xVariable.Value);
                                    break;
                                case "receiptEmailAddress":
                                    ReceiptEmailAddress = xVariable.Value;
                                    break;
                                case "BusinessDay":
                                    businessDay = new Date(Conversion.XmlStringToDateTime(xVariable.Value));
                                    break;
                                case "BusinessSystemDay":
                                    businessSystemDay = new Date(Conversion.XmlStringToDateTime(xVariable.Value));
                                    break;
                                case "subType":
                                    TransactionSubType = Conversion.XmlStringToInt(xVariable.Value);
                                    break;
                                case "originalNumberOfTransactionLines":
                                    OriginalNumberOfTransactionLines = Conversion.XmlStringToInt(xVariable.Value);
                                    break;
                                case "htmlInformation":
                                    HTMLInformation = xVariable.Value.CDataToString();
                                    break;
                                case "HospitalityItem":
                                    Hospitality.ToClass(xVariable, errorLogger);
                                    break;
                                case "PendingEFTTransaction":
                                    PendingEFTTransaction.ToClass(xVariable, errorLogger);
                                    break;
                                case "EFTTransactions":
                                    EFTTransactions = CreateEFTTransactions(xVariable);
                                    break;
                                case "ID":
                                    ID = Conversion.XmlStringToGuid(xVariable.Value);
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errorLogger?.LogMessage(LogMessageType.Error, xVariable.Name.ToString(), ex);
                    }
                }
            }
        }

        private List<OperationAuditing> CreateAuditLines(XElement xItems)
        {
            List<OperationAuditing> lines = new List<OperationAuditing>();

            if (xItems.HasElements)
            {
                IEnumerable<XElement> xTenderItems = xItems.Elements();
                foreach (XElement xTender in xTenderItems)
                {
                    if (xTender.HasElements)
                    {
                        OperationAuditing tli = new OperationAuditing();
                        tli.ToClass(xTender);
                        lines.Add(tli);
                    }
                }
            }

            return lines;
        }

        private List<IReceiptInfo> CreateReceiptLines(XElement xItems)
        {
            List<IReceiptInfo> lines = new List<IReceiptInfo>();

            if (xItems.HasElements)
            {
                IEnumerable<XElement> xReceiptItems = xItems.Elements();
                foreach (XElement xReceipt in xReceiptItems)
                {
                    if (xReceipt.HasElements)
                    {
                        ReceiptInfo tli = new ReceiptInfo();
                        tli.ToClass(xReceipt);
                        lines.Add(tli);
                    }
                }
            }

            return lines;
        }

        private List<PendingEFTTransaction> CreateEFTTransactions(XElement xItems)
        {
            List<PendingEFTTransaction> lines = new List<PendingEFTTransaction>();

            if (xItems.HasElements)
            {
                IEnumerable<XElement> xEftTransItems = xItems.Elements();
                foreach (XElement xEftTrans in xEftTransItems)
                {
                    if (xEftTrans.HasElements)
                    {
                        PendingEFTTransaction eftTrans = new PendingEFTTransaction();
                        eftTrans.ToClass(xEftTrans);
                        lines.Add(eftTrans);
                    }
                }
            }

            return lines;
        }

        private List<POSOperations> CreateOperationStack(XElement xItems)
        {
            List<POSOperations> lines = new List<POSOperations>();

            if (xItems.HasElements)
            {
                IEnumerable<XElement> xOperations = xItems.Elements();
                foreach (XElement xOperation in xOperations)
                {
                    if (xOperation.HasElements)
                    {
                        lines.Add((POSOperations)Conversion.XmlStringToInt(xOperation.Value));
                    }
                }
            }
            return lines;
        }

        public virtual XElement ToXML(IErrorLog errorLogger = null)
        {
            try
            {
                /*
                 * Strings      added as is
                 * Int          added as is
                 * Decimal      added with ToString()
                 * DateTime     added with ToString()
                 * Enums        addid with an (int) cast                 
                 * 
                */
                XElement xPosTrans = new XElement("PosTransaction",
                            new XAttribute("TransactionType", Conversion.ToXmlString((int)TransactionType())),
                            new XElement("transactionId", TransactionId),
                            new XElement("subType", Conversion.ToXmlString(TransactionSubType)),
                            new XElement("salesSequenceID", SalesSequenceID),
                            new XElement("storeId", StoreId),
                            new XElement("storeName", StoreName),
                            new XElement("storeAddress", StoreAddress),
                            new XElement("terminalId", TerminalId),
                            new XElement("receiptId", ReceiptId),
                            new XElement("employee", Cashier.ToXML()),
                            new XElement("createdOnTerminalId", CreatedOnTerminalId),
                            new XElement("entryStatus", Conversion.ToXmlString((int)EntryStatus)),
                            new XElement("beginDateTime", beginDateTime.ToXmlString()),
                            new XElement("endDateTime", endDateTime.ToXmlString()),
                            new XElement("shiftId", ShiftId),
                            new XElement("shiftDate", shiftDate.ToXmlString()),
                            new XElement("openDrawer", Conversion.ToXmlString(OpenDrawer)),
                            new XElement("comment", Comment),
                            new XElement("training", Conversion.ToXmlString(Training)),
                            new XElement("lastRunOpererationIsValidPayment", Conversion.ToXmlString(LastRunOperationIsValidPayment)),
                            new XElement("lastRunOperation", Conversion.ToXmlString((int)LastRunOperation)),
                            new XElement("statementMethod", Conversion.ToXmlString((int)StatementMethod)),
                            new XElement("transactionIdNumberSequence", TransactionIdNumberSequence),
                            new XElement("storeExchangeRate", Conversion.ToXmlString(StoreExchangeRate)),
                            new XElement("storeCurrencyCode", StoreCurrencyCode),
                            new XElement("storeTaxGroup", StoreTaxGroup),
                            new XElement("keyedInPriceContainsTax", Conversion.ToXmlString(KeyedInPriceContainsTax)),
                            new XElement("calculateDiscountFrom", Conversion.ToXmlString((int)CalculateDiscountFrom)),
                            new XElement("displayAmountsIncludingTax", Conversion.ToXmlString(DisplayAmountsIncludingTax)),
                            new XElement("CalcPeriodicDiscounts", Conversion.ToXmlString((int)CalcPeriodicDiscounts)),
                            new XElement("CalcCustomerDiscounts", Conversion.ToXmlString((int)CalcCustomerDiscounts)),
                            new XElement("UnconcludedTransaction", Conversion.ToXmlString(UnconcludedTransaction)),
                            new XElement("HasBeenSentToStation", Conversion.ToXmlString(HasBeenSentToStation)),
                            new XElement("ReturnedLocally", Conversion.ToXmlString(ReturnedLocally)),
                            new XElement("receiptSettings", Conversion.ToXmlString((int)ReceiptSettings)),
                            new XElement("receiptEmailAddress", ReceiptEmailAddress),
                            new XElement("BusinessDay", businessDay.ToXmlString()),
                            new XElement("BusinessSystemDay", businessSystemDay.ToXmlString()),
                            new XElement("originalNumberOfTransactionLines", Conversion.ToXmlString(OriginalNumberOfTransactionLines)),
                            new XElement("ID", Conversion.ToXmlString(ID)),
                            Hospitality.ToXML(),
                            new XElement("htmlInformation", new XCData(HTMLInformation)),
                            PendingEFTTransaction.ToXML()
                );

                if (AuditingLines != null)
                {
                    XElement xAuditLines = new XElement(OperationAuditing.XmlElementName + "s");
                    foreach (OperationAuditing audit in AuditingLines)
                    {
                        xAuditLines.Add(audit.ToXML());
                    }
                    xPosTrans.Add(xAuditLines);
                }

                if (Receipts != null)
                {
                    XElement xReceipts = new XElement(ReceiptInfo.XmlElementName + "s");

                    foreach (IReceiptInfo receipt in Receipts)
                    {
                        xReceipts.Add(receipt.ToXML());
                    }
                    xPosTrans.Add(xReceipts);
                }

                if (OperationStack != null)
                {
                    XElement xOperation = new XElement("operationStack");

                    foreach (POSOperations operation in OperationStack)
                    {
                        xOperation.Add((int)operation);
                    }
                    xPosTrans.Add(xOperation);
                }

                if (EFTTransactions != null)
                {
                    XElement xEftTrans = new XElement("EFTTransactions");

                    foreach (PendingEFTTransaction eftTransaction in EFTTransactions)
                    {
                        xEftTrans.Add(eftTransaction.ToXML());
                    }
                    xPosTrans.Add(xEftTrans);
                }

                return xPosTrans;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, ex.Message, ex);
                throw;
            }
        }
    }
}