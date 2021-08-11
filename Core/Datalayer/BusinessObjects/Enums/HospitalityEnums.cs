namespace LSOne.DataLayer.BusinessObjects.Enums
{
    public enum MainViewEnum
    {
        POS = 0,
        Hospitality = 1,
        Logon = 2
    }

    public enum DiningTableStatus
    {
        Available = 0,
        Unavailable = 1,
        GuestsSeated = 2,
        OrderNotPrinted = 3,
        OrderPartiallyPrinted = 4,
        OrderPrinted = 5,
        OrderStarted = 6,
        OrderFinished = 7,
        OrderConfirmed = 8,
        Locked = 9,
        AlertNotServed = 10,
        OrderSent = 11,
        OrderNotSent = 12,
        OrderPartiallySent = 13,
    }

    public enum HospitalityOperationResult
    {
        Cancel = 0,
        Pay = 1,
        PrintAll = 2,
        PrintSplit = 3,
        SaveAndExit = 4
    }   
}
