using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.Attributes;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.StoreManagement
{
    [RecordIdentifierConstruction("ID", typeof(string), typeof(string))]
    public class StorePaymentMethod : DataEntity, ICloneable
    {
        public enum PaymentRoundMethod
        {
            RoundNearest = 0,
            RoundUp = 1,
            RoundDown = 2
        }

        public StorePaymentMethod()
            : base()
        {
            InheritedName = RecordIdentifier.Empty;
            ID = RecordIdentifier.Empty;
            ChangeTenderID = "";
            AboveMinimumTenderID = "";
            ChangeTenderName = "";
            AboveMinimumTenderName = "";
            MinimumChangeAmount = 0;
            PosOperation = 0;
            Rounding = 0.01M;
            RoundingMethod = PaymentRoundMethod.RoundNearest;
            OpenDrawer = false;
            AllowOverTender = false;
            AllowUnderTender = false;
            UnderTenderAmount = 0;
            MaximumOverTenderAmount = 0;
            MaximumAmountEntered = 0;
            MaximumAmountAllowed = 0;
            MinimumAmountEntered = 0;
            MinimumAmountAllowed = 0;
            MaxRecount = 0;
            MaxCountingDifference = 0;
            AnotherMethodIfAmountHigher = false;
            PaymentTypeCanBeVoided = true;
            AllowNegativePaymentAmounts = true;
            PaymentTypeCanBePartOfSplitPayment = true;
            BlindRecountAllowed = true;
            MaximumAmountInPOSEnabled = false;
            MaximumAmountInPOS = 0;
            ForceRefundToThisPaymentType = false;
            PaymentLimitations = new List<StorePaymentLimitation>();
        }

        public StorePaymentMethod(RecordIdentifier storeAndPaymentTypeID)
            : this()
        {
            this.ID = storeAndPaymentTypeID;
        }

        public RecordIdentifier StoreAndTenderTypeID
        {
            get { return ID; }
        }
        public RecordIdentifier StoreID
        {
            get { return ID.PrimaryID; }
            set
            {
                ID = new RecordIdentifier(value, ID.SecondaryID);
            }
        }
        public RecordIdentifier PaymentTypeID
        {
            get { return ID.SecondaryID; }
            set
            {
                ID.SecondaryID = value;
            }
        }
        
        public RecordIdentifier InheritedName { get; set; }
        public RecordIdentifier AboveMinimumTenderID { get; set; }
        public RecordIdentifier ChangeTenderID { get; set; }
        public string AboveMinimumTenderName { get; set; }     
        public string ChangeTenderName { get; set; }
        public decimal MinimumChangeAmount { get; set; }     
        [RecordIdentifierConstruction(typeof(int))]
        public RecordIdentifier PosOperation { get; set; }
        public decimal Rounding { get; set; }
        public PaymentRoundMethod RoundingMethod { get; set; }
        public bool OpenDrawer { get; set; }
        public decimal UnderTenderAmount { get; set; }
        public decimal MaximumOverTenderAmount { get; set; }
        public bool AllowUnderTender { get; set; }
        public bool AllowOverTender { get; set; }
        public decimal MaximumAmountEntered { get; set; }
        public decimal MaximumAmountAllowed { get; set; }
        public decimal MinimumAmountEntered { get; set; }
        public decimal MinimumAmountAllowed { get; set; }     
        public bool AllowFloat { get; set; }
        public bool AllowBankDrop { get; set; }
        public bool AllowSafeDrop { get; set; }
        public bool CountingRequired { get; set; }
        public int POSOperationID { get; set; }
        public int MaxRecount { get; set; }
        public decimal MaxCountingDifference { get; set; }
        public bool AnotherMethodIfAmountHigher { get; set; }
        public bool PaymentTypeCanBeVoided { get; set; }
        public bool AllowNegativePaymentAmounts { get; set; }
        public bool PaymentTypeCanBePartOfSplitPayment { get; set; }
        public PaymentMethodDefaultFunctionEnum DefaultFunction { get; set; }
        public bool BlindRecountAllowed { get; set; }

        public bool MaximumAmountInPOSEnabled { get; set; }
        public decimal MaximumAmountInPOS { get; set; }

        /// <summary>
        /// If this property is true, then the refund is only allowed with the same tender type
        /// </summary>
        public bool ForceRefundToThisPaymentType { get; set; }
        public List<StorePaymentLimitation> PaymentLimitations { get; set; }

        public override object Clone()
        {
            var storePaymentMethod = new StorePaymentMethod();
            Populate(storePaymentMethod);
            return storePaymentMethod;
        }

        public void Populate(StorePaymentMethod storePaymentMethod)
        {
            storePaymentMethod.PaymentTypeID = (RecordIdentifier)PaymentTypeID?.Clone();
            storePaymentMethod.StoreID = (RecordIdentifier)StoreID.Clone();
            storePaymentMethod.Text = Text;
            storePaymentMethod.InheritedName = (RecordIdentifier)InheritedName.Clone();
            storePaymentMethod.ChangeTenderID = (RecordIdentifier)ChangeTenderID.Clone();            
            storePaymentMethod.AboveMinimumTenderID = (RecordIdentifier)AboveMinimumTenderID.Clone();
            storePaymentMethod.ChangeTenderName = ChangeTenderName;
            storePaymentMethod.AboveMinimumTenderName = AboveMinimumTenderName;
            storePaymentMethod.MinimumChangeAmount = MinimumChangeAmount;
            storePaymentMethod.PosOperation = (RecordIdentifier)PosOperation.Clone();
            storePaymentMethod.Rounding = Rounding;
            storePaymentMethod.RoundingMethod = RoundingMethod;
            storePaymentMethod.OpenDrawer = OpenDrawer;
            storePaymentMethod.AllowOverTender = AllowOverTender;
            storePaymentMethod.AllowUnderTender = AllowUnderTender;
            storePaymentMethod.MaximumOverTenderAmount = MaximumOverTenderAmount;
            storePaymentMethod.UnderTenderAmount = UnderTenderAmount;
            storePaymentMethod.MinimumAmountAllowed = MinimumAmountAllowed;
            storePaymentMethod.MinimumAmountEntered = MinimumAmountEntered;
            storePaymentMethod.MaximumAmountAllowed = MaximumAmountAllowed;
            storePaymentMethod.MaximumAmountEntered = MaximumAmountEntered;
            storePaymentMethod.CountingRequired = CountingRequired;
            storePaymentMethod.AllowFloat = AllowFloat;
            storePaymentMethod.AllowBankDrop = AllowBankDrop;
            storePaymentMethod.AllowSafeDrop = AllowSafeDrop;
            storePaymentMethod.MaxRecount = MaxRecount;
            storePaymentMethod.MaxCountingDifference = MaxCountingDifference;
            storePaymentMethod.AllowNegativePaymentAmounts = AllowNegativePaymentAmounts;
            storePaymentMethod.PaymentTypeCanBeVoided = PaymentTypeCanBeVoided;
            storePaymentMethod.AnotherMethodIfAmountHigher = AnotherMethodIfAmountHigher;
            storePaymentMethod.PaymentTypeCanBePartOfSplitPayment = PaymentTypeCanBePartOfSplitPayment;
            storePaymentMethod.DefaultFunction = DefaultFunction;
            storePaymentMethod.BlindRecountAllowed = BlindRecountAllowed;
            storePaymentMethod.MaximumAmountInPOSEnabled = MaximumAmountInPOSEnabled;
            storePaymentMethod.MaximumAmountInPOS = MaximumAmountInPOS;
            storePaymentMethod.ForceRefundToThisPaymentType = ForceRefundToThisPaymentType;
            storePaymentMethod.PaymentLimitations = CollectionHelper.Clone<StorePaymentLimitation, List<StorePaymentLimitation>>(PaymentLimitations);
        }

            //public override void ToClass(XElement element, IErrorLog errorLogger = null)
            //{
            //    IEnumerable<XElement> elements = element.Elements();
            //    foreach (XElement current in elements)
            //    {
            //        if (!current.IsEmpty)
            //        {
            //            try
            //            {
            //                switch (current.Name.ToString())
            //                {
            //                    case "name":
            //                        Text = current.Value;
            //                        break;
            //                    case "storeID":
            //                        StoreID = current.Value;
            //                        break;
            //                    case "paymentTypeID":
            //                        PaymentTypeID = current.Value;
            //                        break;
            //                    case "aboveMinimumTenderID":
            //                        AboveMinimumTenderID = current.Value;
            //                        break;
            //                    case "changeTenderID":
            //                        ChangeTenderID = current.Value;
            //                        break;
            //                    case "minimumChangeAmount":
            //                        MinimumChangeAmount = Convert.ToDecimal(current.Value, XmlCulture);
            //                        break;
            //                    case "posOperation":
            //                        PosOperation = Convert.ToInt32(current.Value);
            //                        break;
            //                    case "rounding":
            //                        Rounding = Convert.ToDecimal(current.Value,XmlCulture);
            //                        break;
            //                    case "roundingMethod":
            //                        RoundingMethod = (PaymentRoundMethod)Convert.ToInt32(current.Value);
            //                        break;
            //                    case "openDrawer":
            //                        OpenDrawer = current.Value != "false";
            //                        break;
            //                    case "underTenderAmount":
            //                        UnderTenderAmount = Convert.ToDecimal(current.Value,XmlCulture);
            //                        break;
            //                    case "maximumOverTenderAmount":
            //                        MaximumOverTenderAmount = Convert.ToDecimal(current.Value, XmlCulture);
            //                        break;
            //                    case "allowUnderTender":
            //                        AllowUnderTender = current.Value != "false";
            //                        break;
            //                    case "allowOverTender":
            //                        AllowOverTender = current.Value != "false";
            //                        break;
            //                    case "maximumAmountEntered":
            //                        MaximumAmountEntered = Convert.ToDecimal(current.Value, XmlCulture);
            //                        break;
            //                    case "maximumAmountAllowed":
            //                        MaximumAmountAllowed = Convert.ToDecimal(current.Value, XmlCulture);
            //                        break;
            //                    case "minimumAmountEntered":
            //                        MinimumAmountEntered = Convert.ToDecimal(current.Value, XmlCulture);
            //                        break;
            //                    case "minimumAmountAllowed":
            //                        MinimumAmountAllowed = Convert.ToDecimal(current.Value, XmlCulture);
            //                        break;
            //                    case "allowFloat":
            //                        AllowFloat = current.Value != "false";
            //                        break;
            //                    case "allowBankDrop":
            //                        AllowBankDrop = current.Value != "false";
            //                        break;
            //                    case "allowSafeDrop":
            //                        AllowSafeDrop = current.Value != "false";
            //                        break;
            //                    case "countingRequired":
            //                        CountingRequired = current.Value != "false";
            //                        break;
            //                    case "pOSOperationID":
            //                        POSOperationID = Convert.ToInt32(current.Value);
            //                        break;
            //                    case "maxRecount":
            //                        MaxRecount = Convert.ToInt32(current.Value);
            //                        break;
            //                    case "maxCountingDifference":
            //                        MaxCountingDifference = Convert.ToDecimal(current.Value, XmlCulture);
            //                        break;
            //                    case "paymentTypeCanBeVoided":
            //                        PaymentTypeCanBeVoided = current.Value != "false";
            //                        break;
            //                    case "allowNegativePaymentAmounts":
            //                        AllowNegativePaymentAmounts = current.Value != "false";
            //                        break;
            //                    case "paymentTypeCanBePartOfSplitPayment":
            //                        PaymentTypeCanBePartOfSplitPayment = current.Value != "false";
            //                        break;
            //                    case "maximumAmountInPOSEnabled":
            //                        MaximumAmountInPOSEnabled = current.Value != "false";
            //                        break;
            //                    case "maximumAmountInPOS":
            //                        MaximumAmountInPOS = Convert.ToDecimal(current.Value, XmlCulture);
            //                        break;
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                if (errorLogger != null)
            //                {
            //                    errorLogger.LogMessage(LogMessageType.Error, current.Name.ToString(), ex);
            //                }
            //            }
            //        }
            //    }
            //}

            //public override XElement ToXML(IErrorLog errorLogger = null)
            //{
            //    XElement xml = new XElement("storePaymentMethods",
            //            new XElement("name", Text),
            //            new XElement("storeID", (string)StoreID),
            //            new XElement("paymentTypeID", (string)PaymentTypeID),
            //            new XElement("aboveMinimumTenderID", (string)AboveMinimumTenderID),
            //            new XElement("changeTenderID", (string)ChangeTenderID),
            //            new XElement("minimumChangeAmount", MinimumChangeAmount.ToString(XmlCulture)),
            //            new XElement("posOperation", (int)PosOperation),
            //            new XElement("rounding", Rounding),
            //            new XElement("roundingMethod", (int)RoundingMethod),
            //            new XElement("openDrawer", OpenDrawer),
            //            new XElement("underTenderAmount", UnderTenderAmount.ToString( XmlCulture)),
            //            new XElement("maximumOverTenderAmount", MaximumOverTenderAmount.ToString(XmlCulture)),
            //            new XElement("allowUnderTender", AllowUnderTender),
            //            new XElement("allowOverTender", AllowOverTender),
            //            new XElement("maximumAmountEntered", MaximumAmountEntered.ToString(XmlCulture)),
            //            new XElement("maximumAmountAllowed", MaximumAmountAllowed.ToString(XmlCulture)),
            //            new XElement("minimumAmountEntered", MinimumAmountEntered.ToString(XmlCulture)),
            //            new XElement("minimumAmountAllowed", MinimumAmountAllowed.ToString(XmlCulture)),
            //            new XElement("allowFloat", AllowFloat),
            //            new XElement("allowBankDrop", AllowBankDrop),
            //            new XElement("allowSafeDrop", AllowSafeDrop),
            //            new XElement("countingRequired", CountingRequired),
            //            new XElement("pOSOperationID", POSOperationID),
            //            new XElement("maxRecount", MaxRecount),
            //            new XElement("maxCountingDifference", MaxCountingDifference.ToString(XmlCulture)),
            //            new XElement("paymentTypeCanBeVoided", PaymentTypeCanBeVoided),
            //            new XElement("allowNegativePaymentAmounts", AllowNegativePaymentAmounts),
            //            new XElement("paymentTypeCanBePartOfSplitPayment", PaymentTypeCanBePartOfSplitPayment),
            //            new XElement("maximumAmountInPOSEnabled", MaximumAmountInPOSEnabled),
            //            new XElement("maximumAmountInPOS", MaximumAmountInPOS.ToString(XmlCulture)));
            //    return xml;
            //}
        }
}