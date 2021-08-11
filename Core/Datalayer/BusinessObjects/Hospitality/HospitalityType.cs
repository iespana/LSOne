using System;
using System.Drawing;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.Utilities.DataTypes;
#if !MONO
#endif

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    public class HospitalityType : DataEntity
    {
        /// <summary>
        /// ID is composed of : RestaurantID, Sequence, Salestype
        /// </summary>
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(RestaurantID, new RecordIdentifier(Sequence, SalesType));
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }


        public HospitalityType()
        {
            RestaurantID = "";
            Sequence = 0;
#if !MONO
            GraphicalLayout = null;
#endif
            Text = "";
            Overview = OverviewEnum.ButtonFormat;
            SalesType = "";
            UpdateTableFromPOS = false;
            RequestNoOfGuests = false;
            StationPrinting = 0;
            AccessToOtherRestaurant = "";
            PosLogonMenuID = "";
            AllowNewEntries = false;
            TipsAmtLine1 = "";
            TipsAmtLine2 = "";
            TipsTotalLine = "";
            StayInPosAfterTrans = false;
            SendVoidedItemsToStation = true;
            SendTransfersToStation = true;
            SendSuspensionsToStation = true;
            SendOrderNoToStation = true;
            TipsIncomeAcc1 = "";
            TipsIncomeAcc2 = "";
            NoOfDineInTables = 0;
            TableButtonPosMenuID = "";
            TableButtonDescription = TableButtonDescriptionEnum.Description;
            TableButtonStaffDescription = TableButtonStaffDescriptionEnum.ReceiptName;
            StaffTakeOverInTrans = StaffTakeOverInTransEnum.Always;
            ManagerTakeOverInTrans = ManagerTakeOverInTransEnum.Always;
            ViewSalesStaff = false;
            ViewTransDate = false;
            ViewTransTime = false;
            ViewDeliveryAddress = false;
            ViewListTotals = false;
            OrderBy = OrderByEnum.Default;
            ViewRestaurant = false;
            ViewGrid = false;
            ViewCountDown = false;
            ViewProgressStatus = false;
            DirectEditOperation = 0;
            SettingsFromHospType = "";
            SettingsFromSequence = 0;
            SharingSalesTypeFilter = "";
            SettingsFromRestaurant = "";
            GuestButtons = GuestButtonsEnum.Floating;
            MaxGuestButtonsShown = 0;
            MaxGuestsPerTable = 12;
            MaxNumberOfSplits = 5;
            SplitBillLookupID = "";
            SelectGuestOnSplitting = false;
            CombineSplitLinesAction = CombineSplitLinesActionEnum.AlwaysCombine;
            TransferLinesLookupID = "";
            PrintTrainingTransactions = false;
            DefaultType = false;
            LayoutID = "";
            TopPosMenuID = "";
            DiningTableLayoutID = "";
            AutomaticJoiningCheck = false;
            SalesTypeDescription = "";
            PromptForCustomer = false;
            DisplayCustomerOnTable = CustomerOnTable.NoCustomerInfoDisplayed;
        }

        public RecordIdentifier RestaurantID { get; set; }
        public RecordIdentifier Sequence { get; set; }
#if !MONO
        public Image GraphicalLayout { get; set; }        
#endif
        public OverviewEnum Overview { get; set; }
        public RecordIdentifier SalesType { get; set; }
        public bool UpdateTableFromPOS { get; set; }
        public bool RequestNoOfGuests { get; set; }
        public StationPrintingEnum StationPrinting { get; set; }
        public RecordIdentifier AccessToOtherRestaurant { get; set; }
        public RecordIdentifier PosLogonMenuID { get; set; }
        public bool AllowNewEntries { get; set; }
        public string TipsAmtLine1 { get; set; }
        public string TipsAmtLine2 { get; set; }
        public string TipsTotalLine { get; set; }
        public bool StayInPosAfterTrans { get; set; }
        public bool SendVoidedItemsToStation { get; set; }
        public bool SendTransfersToStation { get; set; }
        public bool SendSuspensionsToStation { get; set; }
        public bool SendOrderNoToStation { get; set; }
        public RecordIdentifier TipsIncomeAcc1 { get; set; }
        public RecordIdentifier TipsIncomeAcc2 { get; set; }
        public int NoOfDineInTables { get; set; }
        public RecordIdentifier TableButtonPosMenuID { get; set; }
        public TableButtonDescriptionEnum TableButtonDescription { get; set; }
        public TableButtonStaffDescriptionEnum TableButtonStaffDescription { get; set; }
        public StaffTakeOverInTransEnum StaffTakeOverInTrans { get; set; }
        public ManagerTakeOverInTransEnum ManagerTakeOverInTrans { get; set; }
        public bool ViewSalesStaff { get; set; }
        public bool ViewTransDate { get; set; }
        public bool ViewTransTime { get; set; }
        public bool ViewDeliveryAddress { get; set; }
        public bool ViewListTotals { get; set; }
        public OrderByEnum OrderBy { get; set; }
        public bool ViewRestaurant { get; set; }
        public bool ViewGrid { get; set; }
        public bool ViewCountDown { get; set; }
        public bool ViewProgressStatus { get; set; }
        public int DirectEditOperation { get; set; }
        public RecordIdentifier SettingsFromHospType { get; set; }
        public RecordIdentifier SettingsFromSequence { get; set; }
        public string SharingSalesTypeFilter { get; set; }
        public RecordIdentifier SettingsFromRestaurant { get; set; }
        public GuestButtonsEnum GuestButtons { get; set; }
        public int MaxGuestButtonsShown { get; set; }
        public int MaxGuestsPerTable { get; set; }
        public int MaxNumberOfSplits { get; set; }
        public RecordIdentifier SplitBillLookupID { get; set; }
        public bool SelectGuestOnSplitting { get; set; }
        public CombineSplitLinesActionEnum CombineSplitLinesAction { get; set; }
        public RecordIdentifier TransferLinesLookupID { get; set; }
        public bool PrintTrainingTransactions { get; set; }
        public bool DefaultType { get; set; }
        public RecordIdentifier LayoutID { get; set; }
        public RecordIdentifier TopPosMenuID { get; set; }
        public RecordIdentifier DiningTableLayoutID { get; set; }
        public bool AutomaticJoiningCheck { get; set; }
        public string SalesTypeDescription { get; set; }
        public bool PromptForCustomer { get; set; }
        public CustomerOnTable DisplayCustomerOnTable { get; set; }
        
        #region Enums
        public enum OverviewEnum
        {
            ButtonFormat = 0,            
            Listing = 1
        }

        /// <summary>
        /// Configures when and how items are to be printed on kitchen stations
        /// </summary>
        public enum StationPrintingEnum
        {
            /// <summary>
            /// The user should have to press a button to run the <see cref="LSOne.DataLayer.BusinessObjects.Enums.POSOperations.PrintHospitalityMenuType"/> operation
            /// </summary>
            Manual = 0,
            /// <summary>
            /// The user will only be prompted to print when a payment is to be made
            /// </summary>
            AtPosPayment = 1,
            /// <summary>
            /// The user will be prompted both when a payment is to made and when exiting to the table view
            /// </summary>
            AtPosPaymentOrPosExit = 2,
            /// <summary>
            /// Items are immediately sent to the kitchen stations without prompting the user
            /// </summary>
            AtItemAdded = 3,
            /// <summary>
            /// The second-to-last item is sent to the kitchen stations without prompting the user
            /// </summary>
            AtItemAddedOneDelay = 4,
            /// <summary>
            /// When exiting to table view or a payment is made, all items are sent to the kitchen stations without prompting the user
            /// </summary>
            AlwaysPrintAll = 5
        }

        /// <summary>
        /// A configuration to decide if the customer name should be displayed on the table
        /// </summary>
        public enum CustomerOnTable
        {
            /// <summary>
            /// If there is a customer on the table no customer information would be displayed
            /// </summary>
            NoCustomerInfoDisplayed,
            /// <summary>
            /// If there is a customer on the table then only the name should be displayed
            /// </summary>
            CustomerOnly,
            /// <summary>
            /// If there is a customer on the table then the customer name should be displayed and then the normal table description
            /// </summary>
            CustomerPlusButtonDescription
        }

        public enum TableButtonDescriptionEnum
        {
            Number = 0,
            Description = 1,
            NumberPlusDescription = 2,
            DescriptionPlusNumber = 3
        }

        public enum TableButtonStaffDescriptionEnum
        {
            StaffID = 0,
            ReceiptName = 1
        }

        public enum StaffTakeOverInTransEnum
        {
            Always = 0,
            WithConfirmation = 1,
            Never = 2
        }

        public enum ManagerTakeOverInTransEnum
        {
            Always = 0,
            WithConfirmation = 1,
            Never = 2
        }

        public enum OrderByEnum
        {
            Default = 0,
            Grid = 1
        }

        public enum GuestButtonsEnum
        {
            NotShown = 0,
            PerCover = 1,
            Floating = 2
        }

        public enum CombineSplitLinesActionEnum
        {
            NeverCombine = 0,
            AlwaysCombine = 1,
            CombineOnConfirmation = 2
        }
        #endregion 

    }
}
