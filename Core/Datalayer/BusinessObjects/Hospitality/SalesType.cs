using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{    
    public class SalesType : DataEntity
    {
        public bool RequestSalesperson { get; set; }
        public int RequestDepositPerc { get; set; }
        public bool RequestChargeAccount { get; set; }
        public string PurchasingCode { get; set; }
        public decimal DefaultOrderLimit { get; set; }
        public LimitSettingEnum LimitSetting { get; set; }
        public bool RequestConfirmation { get; set; }
        public bool RequestDescription { get; set; }
        public string NewGlobalDimension2 { get; set; }
        public SuspendPrintingEnum SuspendPrinting { get; set; }
        public SuspendTypeEnum SuspendType { get; set; }
        public string PrePaymentAccountNo { get; set; }
        public decimal MinimumDeposit { get; set; }
        public bool PrintItemLinesOnPosSlip { get; set; }
        public string VoidedPrepaymentAccountNo { get; set; }
        public int DaysOpenTransExist { get; set; }
        public string TaxGroupID { get; set; }
        public string PriceGroup { get; set; }
        public int TransDeleteReminder { get; set; }
        public string LocationCode { get; set; }
        public bool PaymentIsPrepayment { get; set; }
        public bool CalcPriceFromVatPrice { get; set; }

        public enum LimitSettingEnum
        {
            None = 0,
            ByDefault = 1,
            ByTender = 2,
            ByRequest = 3
        }

        public enum SuspendPrintingEnum
        {
            Default = 0,
            NoPrinting = 1,
            SalesReportID = 2,
            PosReportID = 3
        }

        public enum SuspendTypeEnum
        {
            PosTransation = 0,
            SalesQuote = 1,
            SalesOrder = 2
        }

        public SalesType()
            : this(RecordIdentifier.Empty, "")
        {

        }

        public SalesType(RecordIdentifier code, string description)
            : base(code, description)
        {
            RequestSalesperson = false;
            RequestDepositPerc = 0;
            RequestChargeAccount = false;
            PurchasingCode = "";
            DefaultOrderLimit = 0;
            LimitSetting = LimitSettingEnum.ByDefault;
            RequestConfirmation = false;
            RequestDescription = false;
            NewGlobalDimension2 = "";
            SuspendPrinting = SuspendPrintingEnum.Default;
            SuspendType = SuspendTypeEnum.PosTransation;
            PrePaymentAccountNo = "";
            MinimumDeposit = 0;
            PrintItemLinesOnPosSlip = false;
            VoidedPrepaymentAccountNo = "";
            DaysOpenTransExist = 0;
            TaxGroupID = "";
            PriceGroup = "";
            TransDeleteReminder = 0;
            LocationCode = "";
            PaymentIsPrepayment = false;
            CalcPriceFromVatPrice = false;

        }

    }
}
