using System;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    #region Fuelling Point
    /// <summary>
    /// Pending.
    /// </summary>
    public enum TypeOfTotals
    {
        /// <summary>
        /// 0
        /// </summary>
        FuellingPointTotals = 0,
        /// <summary>
        /// 1
        /// </summary>
        PumpTotals = 1,
    }
    /// <summary>
    /// Pending.
    /// </summary>
    public enum FpMainStates
    {
        /// <summary>
        /// 0
        /// </summary>
        Unconfigured = 0,
        /// <summary>
        /// 1
        /// </summary>
        Closed = 1,
        /// <summary>
        /// 2
        /// </summary>
        Idle = 2,
        /// <summary>
        /// 3
        /// </summary>
        Error = 3,
        /// <summary>
        /// 4
        /// </summary>
        Calling = 4,
        /// <summary>
        /// 5
        /// </summary>
        PreAuthorized = 5,
        /// <summary>
        /// 6
        /// </summary>
        Starting = 6,
        /// <summary>
        /// 7
        /// </summary>
        StartingPaused = 7,
        /// <summary>
        /// 8
        /// </summary>
        StartingTerminated = 8,
        /// <summary>
        /// 9
        /// </summary>
        Fuelling = 9,
        /// <summary>
        /// 10
        /// </summary>
        FuellingPaused = 10,
        /// <summary>
        /// 11
        /// </summary>
        FuellingTerminated = 11,
        /// <summary>
        /// 12
        /// </summary>
        Unavailable = 12,
        /// <summary>
        /// 13
        /// </summary>
        UnavailableAndCalling = 13,
        /// <summary>
        /// 14
        /// </summary>
        Unknown = 14
    }

    [Flags]
    public enum FpSubStates
    {
        /// <summary>
        /// 1
        /// </summary>
        LockedByPos = 1,
        /// <summary>
        /// 2
        /// </summary>
        IsSupervised = 2,
        /// <summary>
        /// 4
        /// </summary>
        IsOnline = 4,
        /// <summary>
        /// 8
        /// </summary>
        IsInEmergencyStop = 8,
        /// <summary>
        /// 16
        /// </summary>
        HasFreeMemory = 16,
        /// <summary>
        /// 32
        /// </summary>
        IsInErrorState = 32,
        /// <summary>
        /// 64
        /// </summary>
        HasActiveGrades = 64,
        /// <summary>
        /// 128
        /// </summary>
        HasPresetAuthorization = 128
    }


    [Flags]
    public enum FpSubStates2
    {
        /// <summary>
        /// 1
        /// </summary>
        PumpTotalsReady = 1,
        /// <summary>
        /// 2
        /// </summary>
        FuellingStoppedByTimeOut = 2,
        /// <summary>
        /// 4
        /// </summary>
        FullingStoppedAtLimit = 4,
        /// <summary>
        /// 8
        /// </summary>
        FuellingStoppedByOperator = 8,
        /// <summary>
        /// 16
        /// </summary>
        Undefined1 = 16,
        /// <summary>
        /// 32
        /// </summary>
        Undefined2 = 32,
        /// <summary>
        /// 64
        /// </summary>
        Undefined3 = 64,
        /// <summary>
        /// 128
        /// </summary>
        Undefined4 = 128
    }


    public enum FpErrors
    {
        /// <summary>
        /// 0
        /// </summary>
        NoError = 0,
        /// <summary>
        /// 1
        /// </summary>
        UnspecifiedHardwareError = 1,
        /// <summary>
        /// 2
        /// </summary>
        UnspecifiedSoftwareError = 2,
        /// <summary>
        /// 3
        /// </summary>
        PROMError = 3,
        /// <summary>
        /// 4
        /// </summary>
        RAMError = 4,
        /// <summary>
        /// 5
        /// </summary>
        NotUsed1 = 5,
        /// <summary>
        /// 6
        /// </summary>
        PulseError = 6,
        /// <summary>
        /// 7
        /// </summary>
        DisplayError = 7,
        /// <summary>
        /// 8
        /// </summary>
        OutputControlError = 8,
        /// <summary>
        /// 9
        /// </summary>
        NotUsed2 = 9,
        /// <summary>
        /// 10
        /// </summary>
        PresetOverRunError = 10,
        /// <summary>
        /// 11
        /// </summary>
        PresetGradeError = 11,
        /// <summary>
        /// 12
        /// </summary>
        CalculationError = 12,
        /// <summary>
        /// 13
        /// </summary>
        BlendError = 13,
        /// <summary>
        /// 14
        /// </summary>
        UnexpectedPumpStart = 14,
        /// <summary>
        /// 15
        /// </summary>
        TransactionDataError = 15,
        /// <summary>
        /// 16
        /// </summary>
        PumpDataSequenceError = 16,
        /// <summary>
        /// 17
        /// </summary>
        FpAndPumpInstallationMismatch = 17,
        /// <summary>
        /// 18
        /// </summary>
        ErrorCodeUnavailable = 18,
        /// <summary>
        /// 19
        /// </summary>
        SwitchInWrongPosition = 19,
        /// <summary>
        /// 20
        /// </summary>
        SecurityTelegramError = 20,
        /// <summary>
        /// 21
        /// </summary>
        FpResetFromPos = 21,
        /// <summary>
        /// 22
        /// </summary>
        PumpTotalMismatch = 22,
        /// <summary>
        /// 23
        /// </summary>
        GradeMismatch = 23,
        /// <summary>
        /// 24
        /// </summary>
        SubPumpError = 24,
        /// <summary>
        /// 25
        /// </summary>
        BatteryError = 25,
        /// <summary>
        /// 26
        /// </summary>
        TransResetError = 26,
        /// <summary>
        /// 27
        /// </summary>
        FuellingDataUsed = 27
    }

    public class FpStatusEventArgs : EventArgs
    {
        int fuellingPointId;
        FpMainStates fpMainState;
        FpSubStates fpSubState;

        public int FuellingPointId
        {
            get { return fuellingPointId; }
        }

        public FpMainStates FpMainState
        {
            get { return fpMainState; }
        }

        public FpSubStates FpSubState
        {
            get { return fpSubState; }
        }

        public FpStatusEventArgs(int fuellingPointId, FpMainStates fpMainState, FpSubStates FpSubState)
        {
            this.fuellingPointId = fuellingPointId;
            this.fpMainState = fpMainState;
            this.fpSubState = FpSubState;
        }
    }

    public class FpFuellingDataEventArgs : EventArgs
    {
        int fuellingPointId;
        decimal amount;
        decimal volume;

        public int FuellingPointId
        {
            get { return fuellingPointId; }
        }

        public decimal Amount
        {
            get { return amount; }
        }

        public decimal Volume
        {
            get { return volume; }
        }

        public FpFuellingDataEventArgs(int fuellingPointId, decimal amount, decimal volume)
        {
            this.fuellingPointId = fuellingPointId;
            this.amount = amount;
            this.volume = volume;
        }
    }

    public class MessageRejectedEventArgs : EventArgs
    {
        ForecourtOperations forecourtOperation;
        int errorId;

        public ForecourtOperations ForecourtOperation
        {
            get { return forecourtOperation; }
        }

        public int ErrorId
        {
            get { return errorId; }
        }

        public MessageRejectedEventArgs(ForecourtOperations forecourtOperation, int errorId)
        {
            this.forecourtOperation = forecourtOperation;
            this.errorId = errorId;
        }
    }
    #endregion

    #region Terminal

    public enum TerminalMainStates
    {
        /// <summary>
        /// 0
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// 1
        /// </summary>
        Closed = 1,
        /// <summary>
        /// 2
        /// </summary>
        Error = 2,
        /// <summary>
        /// 3
        /// </summary>
        Idle = 3,
        /// <summary>
        /// 4
        /// </summary>
        Busy = 4
    }

    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum TerminalSubStates
    {
        /// <summary>
        /// 1
        /// </summary>
        AcceptingCards = 1,
        /// <summary>
        /// 2
        /// </summary>
        AcceptingNotes = 2,
        /// <summary>
        /// 4
        /// </summary>
        IsOnline = 4,
        /// <summary>
        /// 8
        /// </summary>
        HasErrors = 8,
        /// <summary>
        /// 16
        /// </summary>
        ErrorInSubSystem = 16,
        /// <summary>
        /// 32
        /// </summary>
        OpenDoor = 32,
        /// <summary>
        /// 64
        /// </summary>
        Alarm = 64,
        /// <summary>
        /// 128
        /// </summary>
        PaperLow = 128
    }

    [Flags]
    public enum TerminalErrorStates
    {
        /// <summary>
        /// 1
        /// </summary>
        CardReaderError = 1,
        /// <summary>
        /// 2
        /// </summary>
        NoteReaderError = 2,
        /// <summary>
        /// 4
        /// </summary>
        ReceiptError = 4,
        /// <summary>
        /// 8
        /// </summary>
        Undefined2 = 8,
        /// <summary>
        /// 16
        /// </summary>
        Undefined3 = 16,
        /// <summary>
        /// 32
        /// </summary>
        Undefined4 = 32,
        /// <summary>
        /// 64
        /// </summary>
        Undefined5 = 64,
        /// <summary>
        /// 128
        /// </summary>
        OtherError = 128              //Display,Keyboard,Power supply error
    }

    #endregion

    #region TankGauge
    public enum TankGaugeMainStates
    {
        /// <summary>
        /// 0
        /// </summary>
        Unconfigured = 0,
        /// <summary>
        /// 1
        /// </summary>
        Undefined1 = 1,
        /// <summary>
        /// 2
        /// </summary>
        Operative = 2,
        /// <summary>
        /// 3
        /// </summary>
        Alarm = 3,
        /// <summary>
        /// 4
        /// </summary>
        Error = 4
    }

    [Flags]
    public enum TankGaugeSubStates
    {
        /// <summary>
        /// 1
        /// </summary>
        Online = 1,
        /// <summary>
        /// 2
        /// </summary>
        AlarmActive = 2,
        /// <summary>
        /// 4
        /// </summary>
        ErrorActive = 4,
        /// <summary>
        /// 8
        /// </summary>
        Undefined1 = 8,
        /// <summary>
        /// 16
        /// </summary>
        Undefined2 = 16,
        /// <summary>
        /// 32
        /// </summary>
        DeliveryInProgress = 32,
        /// <summary>
        /// 64
        /// </summary>
        DeliveryDataReady = 64,
        /// <summary>
        /// 128
        /// </summary>
        InventoryDataReady = 128
    }
    /// <summary>
    /// Pending
    /// </summary>
    [Flags]
    public enum TankGaugeAlarmStatus
    {
        /// <summary>
        /// 1
        /// </summary>
        HighLevelAlarm = 1,
        /// <summary>
        /// 2
        /// </summary>
        HighHighLevelAlarm = 2,
        /// <summary>
        /// 4
        /// </summary>
        LowLevelAlarm = 4,
        /// <summary>
        /// 8
        /// </summary>
        LowLowLevelAlarm = 8,
        /// <summary>
        /// 16
        /// </summary>
        HighWaterAlarm = 16,
        /// <summary>
        /// 32
        /// </summary>
        TankLeakAlarm = 32,
        /// <summary>
        /// 64
        /// </summary>
        TankDataMissing = 64,
        /// <summary>
        /// 128
        /// </summary>
        Undefined1 = 128,
        /// <summary>
        /// 256
        /// </summary>
        Undefined2 = 256,
        /// <summary>
        /// 512
        /// </summary>
        Undefined3 = 512,
        /// <summary>
        /// 1024
        /// </summary>
        Undefined4 = 1024,
        /// <summary>
        /// 2048
        /// </summary>
        Undefined5 = 2048,
        /// <summary>
        /// 4096
        /// </summary>
        Undefined6 = 4096,
        /// <summary>
        /// 8192
        /// </summary>
        Undefined7 = 8192,
        /// <summary>
        /// 16384
        /// </summary>
        DeliveryDataLost = 16384,
        /// <summary>
        /// 32768
        /// </summary>
        OtherAlarm = 32768
    }

    #endregion

    #region Forecourt
    [Flags]
    public enum ForecourtFlag1
    {
        /// <summary>
        /// 1
        /// </summary>
        PumpTotalsReady = 1,
        /// <summary>
        /// 2
        /// </summary>
        InstallationDataReceived = 2,
        /// <summary>
        /// 4
        /// </summary>
        FallbackMode = 4,
        /// <summary>
        /// 8
        /// </summary>
        FallbackTotalsNonZero = 8,
        /// <summary>
        /// 16
        /// </summary>
        RamErrorDetectedInFc = 16,
        /// <summary>
        /// 32
        /// </summary>
        OperationWithStoredTransDisabled = 32,
        /// <summary>
        /// 64
        /// </summary>
        TerminalSaleDisabled = 64,
        /// <summary>
        /// 128
        /// </summary>
        CurrencyCodeIsEuro = 128
    }

    [Flags]
    public enum ForecourtFlag2
    {
        /// <summary>
        /// 1
        /// </summary>
        ServiceMsgReady = 1,
        /// <summary>
        /// 2
        /// </summary>
        UnsolicitedStatusUpdateOn = 2,
        /// <summary>
        /// 4
        /// </summary>
        HwSwIncompatibilityWithInFc = 4,
        /// <summary>
        /// 8
        /// </summary>
        RtcError = 8,
        /// <summary>
        /// 16
        /// </summary>
        NoAdditionalParamAssignedToGrades = 16,
        /// <summary>
        /// 32
        /// </summary>
        BackOfficeRecordExists = 32,
        /// <summary>
        /// 64
        /// </summary>
        Reserved = 64,
        /// <summary>
        /// 128
        /// </summary>
        Undefined = 128
    }

    public enum ForecourtMode
    {
        /// <summary>
        /// 0
        /// </summary>
        Day = 0,
        /// <summary>
        /// 1
        /// </summary>
        Night = 1,
        /// <summary>
        /// 2
        /// </summary>
        Fallback = 2,
        /// <summary>
        /// 3
        /// </summary>
        Verification = 3
    }

    [Flags]
    public enum SupportedOperations
    {
        /// <summary>
        /// 1
        /// </summary>
        EmergencyStopAndRecall = 1,
        /// <summary>
        /// 2
        /// </summary>
        FpAuthorizieAndCancel = 2,
        /// <summary>
        /// 4
        /// </summary>
        FpOpenAndClose = 4,
        /// <summary>
        /// 8
        /// </summary>
        FpPrePaid = 8,
        /// <summary>
        /// 16
        /// </summary>
        FpPresetVolume = 16,
        /// <summary>
        /// 32
        /// </summary>
        FpPresetAmount = 32,
        /// <summary>
        /// 64
        /// </summary>
        PriceOperations = 64,
        /// <summary>
        /// 128
        /// </summary>
        TerminalOperations = 128,
        /// <summary>
        /// 256
        /// </summary>
        TankTotals = 256,
        /// <summary>
        /// 512
        /// </summary>
        TankStatus = 512,
        /// <summary>
        /// 1024
        /// </summary>
        PricePoleOperations = 1024
    }

    #endregion

    #region Price Pole
    public enum PricePoleMainStates
    {
        /// <summary>
        /// 0
        /// </summary>
        Unconfigured = 0,
        /// <summary>
        /// 1
        /// </summary>
        Closed = 1,
        /// <summary>
        /// 2
        /// </summary>
        Idle = 2,
        /// <summary>
        /// 3
        /// </summary>
        Error = 3,
        /// <summary>
        /// 4
        /// </summary>
        Updating = 4,
        /// <summary>
        /// 5
        /// </summary>
        Suspended = 5
    }

    [Flags]
    public enum PricePoleSubStates
    {
        /// <summary>
        /// 1
        /// </summary>
        Online = 1,
        /// <summary>
        /// 2
        /// </summary>
        Error = 2,
        /// <summary>
        /// 4
        /// </summary>
        Undefined1 = 4,
        /// <summary>
        /// 8
        /// </summary>
        Undefined2 = 8,
        /// <summary>
        /// 16
        /// </summary>
        Undefined3 = 16,
        /// <summary>
        /// 32
        /// </summary>
        Undefined4 = 32,
        /// <summary>
        /// 64
        /// </summary>
        Undefined5 = 64,
        /// <summary>
        /// 128
        /// </summary>
        Undefined6 = 128,

    }
    public enum PricePoleErrorStates
    {
        /// <summary>
        /// 0
        /// </summary>
        NoError = 0,
        /// <summary>
        /// 1
        /// </summary>
        HWerror = 1,
        /// <summary>
        /// 2
        /// </summary>
        SWerror = 2,
        /// <summary>
        /// 3
        /// </summary>
        PROMerror = 3,
        /// <summary>
        /// 4
        /// </summary>
        RAMerror = 4,
        /// <summary>
        /// 7
        /// </summary>
        DisplayError = 7,
        /// <summary>
        /// 8
        /// </summary>
        OutputControlError = 8,
        /// <summary>
        /// 17
        /// </summary>
        ConfigError = 17
    }

    #endregion
    public enum ForecourtOperations
    {
        /// <summary>
        /// 0
        /// </summary>
        FpOpen = 0,
        /// <summary>
        /// 1
        /// </summary>
        FpClose = 1,
        /// <summary>
        /// 2
        /// </summary>
        FpAuthorize = 2,
        /// <summary>
        /// 3
        /// </summary>
        FpAuthorizeCancel = 3,
        /// <summary>
        /// 4
        /// </summary>
        FpErrorRead = 4,
        /// <summary>
        /// 5
        /// </summary>
        FpErrorClear = 5,
        /// <summary>
        /// 6
        /// </summary>
        FpPresetAmount = 6,
        /// <summary>
        /// 7
        /// </summary>
        FpPresetVolume = 7,
        /// <summary>
        /// 8
        /// </summary>
        FpPrepaid = 8,
        /// <summary>
        /// 9
        /// </summary>
        FpEmergencyStop = 9,
        /// <summary>
        /// 10
        /// </summary>
        FpEmergencyStopRecall = 10,
        /// <summary>
        /// 11
        /// </summary>
        TransactionLock = 11,
        /// <summary>
        /// 12
        /// </summary>
        TransactionUnLock = 12,
        /// <summary>
        /// 13
        /// </summary>
        TransactionClear = 13,
        /// <summary>
        /// 14
        /// </summary>
        EmergencyStop = 14,
        /// <summary>
        /// 15
        /// </summary>
        EmergencyStopRecall = 15,
        /// <summary>
        /// 16
        /// </summary>
        TerminalOpen = 16,
        /// <summary>
        /// 17
        /// </summary>
        TerminalClose = 17,
        /// <summary>
        /// 18
        /// </summary>
        TerminalClearError = 18,
        /// <summary>
        /// 19
        /// </summary>
        TerminalReadError = 19,
        /// <summary>
        /// 20
        /// </summary>
        SetForeCourtMode = 20,
        /// <summary>
        /// 21
        /// </summary>
        SetPrice = 21,
        /// <summary>
        /// 22
        /// </summary>
        GetPrice = 22,
        /// <summary>
        /// 23
        /// </summary>
        GetGrades = 23,
        /// <summary>
        /// 24
        /// </summary>
        PricePoleOpen = 24,
        /// <summary>
        /// 25
        /// </summary>
        PricePoleClose = 25,
        /// <summary>
        /// 26
        /// </summary>
        NoOperation = 26
    }
}
