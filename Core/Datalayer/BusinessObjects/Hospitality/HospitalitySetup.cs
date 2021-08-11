using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    public class HospitalitySetup : DataEntity
    {
        public enum SetupStationPrinting
        {
            None = 0,
            AtPOSExit = 1,
            AtPOSPosting = 2,
            AtPOSExitAndPOSPosting = 3,
            AtItemAdded = 4
        }

        public enum SetupDineInTableLocking
        {
            ByPOS = 0,
            ByStaff = 1
        }

        public enum SetupDineInTableSelection
        {            
            ClickTwice = 0,
            DoubleClick = 1
        }

        public HospitalitySetup()
            : base()
        {
            DeliverySalesType = "";
            DineInSalesType = "";
            OrderProcessTimeMin = 0;
            TableFreeColorB = 0;
            TableNotAvailColorB = 0;
            TableLockedColorB = 0;
            OrderNotPrintedColorB = 0;
            OrderPrintedColorB = 0;
            OrderStartedColorB = 0;
            OrderFinishedColorB = 0;
            OrderConfirmedColorB = 0;
            TableFreeColorF = "";
            TableNotAvailColorF = "";
            TableLockedColorF = "";
            OrderNotPrintedColorF = "";
            OrderPrintedColorF = "";
            OrderStartedColorF = "";
            OrderFinishedColorF = "";
            OrderConfirmedColorF = "";
            ConfirmStationPrinting = false;
            RequestNoOfGuests = false;
            NoOfDineInTablesCol = 0;
            NoOfDineInTablesRows = 0;
            StationPrinting = SetupStationPrinting.AtPOSExit;
            DineInTableLocking = SetupDineInTableLocking.ByPOS;
            DineInTableSelection = SetupDineInTableSelection.ClickTwice;
            Period1TimeFrom = new TimeSpan();
            Period1TimeTo = new TimeSpan();
            Period2TimeFrom = new TimeSpan();
            Period2TimeTo = new TimeSpan();
            Period3TimeFrom = new TimeSpan();
            Period3TimeTo = new TimeSpan();
            Period4TimeFrom = new TimeSpan();
            Period4TimeTo = new TimeSpan();
            AutoLogoffAtPOSExit = false;
            TakeOutSalesType = "";
            PreOrderSalesType = "";
            LogStationPrinting = false;
            PopulateDeliveryInfocodes = false;
            AllowPreOrders = false;
            TakeoutNoNameNo = "";
            AdvPreOrdPrintMin = 0;
            CloseTripOnDepart = false;
            DelProgressStatusInUse = false;
            DaysBOMMonitorExist = 0;
            DaysDriverTripsExist = 0;
            PosTerminalPrintPreOrders = "";
            DisplayTimeAtOrderTaking = false;
            NormalPOSSalesType = "";
            OrdListScrollPageSize = 0;
            TableUpdateTimerInterval = 30;
        }

        public RecordIdentifier DeliverySalesType { get; set; }
        public RecordIdentifier DineInSalesType { get; set; }
        public int OrderProcessTimeMin { get; set; }
        public int TableFreeColorB { get; set; }
        public int TableNotAvailColorB { get; set; }
        public int TableLockedColorB { get; set; }
        public int OrderNotPrintedColorB { get; set; }
        public int OrderPrintedColorB { get; set; }
        public int OrderStartedColorB { get; set; }
        public int OrderFinishedColorB { get; set; }
        public int OrderConfirmedColorB { get; set; }
        public string TableFreeColorF { get; set; }
        public string TableNotAvailColorF { get; set; }
        public string TableLockedColorF { get; set; }
        public string OrderNotPrintedColorF { get; set; }
        public string OrderPrintedColorF { get; set; }
        public string OrderStartedColorF { get; set; }
        public string OrderFinishedColorF { get; set; }
        public string OrderConfirmedColorF { get; set; }
        public bool ConfirmStationPrinting { get; set; }
        public bool RequestNoOfGuests { get; set; }
        public int NoOfDineInTablesCol { get; set; }
        public int NoOfDineInTablesRows { get; set; }
        public SetupStationPrinting StationPrinting { get; set; }
        public SetupDineInTableLocking DineInTableLocking { get; set; }
        public SetupDineInTableSelection DineInTableSelection { get; set; }
        public TimeSpan Period1TimeFrom { get; set; }
        public TimeSpan Period1TimeTo { get; set; }
        public TimeSpan Period2TimeFrom { get; set; }
        public TimeSpan Period2TimeTo { get; set; }
        public TimeSpan Period3TimeFrom { get; set; }
        public TimeSpan Period3TimeTo { get; set; }
        public TimeSpan Period4TimeFrom { get; set; }
        public TimeSpan Period4TimeTo { get; set; }
        public bool AutoLogoffAtPOSExit { get; set; }
        public RecordIdentifier TakeOutSalesType { get; set; }
        public RecordIdentifier PreOrderSalesType { get; set; }
        public bool LogStationPrinting { get; set; }
        public bool PopulateDeliveryInfocodes { get; set; }
        public bool AllowPreOrders { get; set; }
        public string TakeoutNoNameNo { get; set; }
        public int AdvPreOrdPrintMin { get; set; }
        public bool CloseTripOnDepart { get; set; }
        public bool DelProgressStatusInUse { get; set; }
        public int DaysBOMPrintExist { get; set; }
        public int DaysBOMMonitorExist { get; set; }
        public int DaysDriverTripsExist { get; set; }
        public string PosTerminalPrintPreOrders { get; set; }
        public bool DisplayTimeAtOrderTaking { get; set; }
        public RecordIdentifier NormalPOSSalesType { get; set; }
        public int OrdListScrollPageSize { get; set; }
        public int TableUpdateTimerInterval { get; set; }
    }
}
