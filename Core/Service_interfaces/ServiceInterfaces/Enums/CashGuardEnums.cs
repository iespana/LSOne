namespace LSOne.Services.Interfaces.Enums
{
    public enum CashGuardReturn
    {
        CG_STATUS_OK = 0,               // The command was successfully executed
        CG_STATUS_BUSY = 1,             // The cash changer was busy so the command couldn’t be sent to it
        CG_STATUS_ERROR = 2,            // An unspecified error occurred
        CG_STATUS_WARNING = 3,          // The request could not be performed
        CG_STATUS_INPUT_PARAM_ERROR = 4,// Some input parameter is incorrect
        CG_STATUS_CLOSED = 5,           // The cash changer was closed so the command couldn’t be sent to it
        CG_STATUS_TIMEOUT = 6,          // Time out occurred during a request
        CG_STATUS_SENDFAILED = 7,       // Some kind of communication send error
        CG_STATUS_PAYOUT_REST = 8,      // Payout not possible due to lack of suitable denominations
        CG_STATUS_PORT_ERROR = 9,       // Some kind of communication receive error
        CG_STATUS_PAYCLEAR_ERROR = 10,  // Amount to deposit is too big (bigger than the amount inserted into the cash changer)
        CG_STATUS_PAYOUT_LIMIT = 11     // The payout is refused because it would result in a
        // too big amount being paid out during the last
        // time period (the amount and time limits are
        // configured in cglogics.ini, see chapter 7.3).
    }

    public enum CashGuardRegretType
    {
        REGRETTYPE_ONE = 0,
        REGRETTYPE_ALL = 1,
    }

    public enum CashGuardStaus
    {
        CG_STATUS_OK = 0,               // Everything is ok
        CG_STATUS_BUSY = 1,             // The cash changer is performing some task
        CG_STATUS_ERROR = 2,            // The cash changer has error(s)
    }

    public enum CashGuardMode
    {
        CGCLOSED = 0,                   // The cash changer is closed
        CGOPENFROMCR = 1,               // The cash changer is opened from cash register
        CGOPENFROMBO = 2                // The cash changer is opened from BackOffice
    }

    public enum CashGuardWarningType
    {
        LEVEL_EMPTY = 1,                // Empty No coins or notes left of this denomination
        LEVEL_LOW = 2,                  // Low The level is below the low warning level configured from BackOffice
        LEVEL_HIGH = 3,                 // High The level is above the high warning level configured from BackOffice
        LEVEL_BLOCK = 4,                // Blocked The denomination is completely full and no more coins or notes of that denomination can be inserted.
    }

    public enum CashGuardInfoType
    {
        BO_WITHDRAWAL = 0,              // Money paid out during BO transaction
        BO_REFILL = 1,                  // Money inserted during BO transaction
        CR_WITHDRAWAL = 2,              // Money withdrawn during CR transactions
        CR_REFILL = 3,                  // Money paid in during CR transactions
        CG_BUSY = 4                     // CG is busy (allocated by second CR in DualPOS operation)
    }

    public enum CashGuardDeviceType
    {
        None = 0,                       // Device not connected
        SingleMode = 1,                 // Device connected to a single device
        DualMode = 2                    // Device connected with two pos applications
    }
}
