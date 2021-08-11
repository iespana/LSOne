
namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// A list of all sections of the task bar (status bar) of the main POS dialog
    /// </summary>
    public enum TaskbarSection
    {
        Logo = 0,
        Version = 1,
        Terminal = 2,
        OperatorName = 3,
        Message = 4,
        FreeText1 = 5,
        FreeText2 = 6,
        FreeText3 = 7,
        FreeText4 = 8,
        FreeText5 = 9,

        //This is the order for the sections that are right aligned
        //but listed here below from left to right
        LocalCustomers = 14,
        Suspended = 13,
        BusinessDate = 12,
        Time = 11,
        NetworkStatus = 10,
    }
}
