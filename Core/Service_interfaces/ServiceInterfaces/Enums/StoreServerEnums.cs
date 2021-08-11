namespace LSOne.Services.Interfaces.Enums
{
    public enum GiftCardValidationEnum
    {
        ValidationSuccess = 1,
        ValidationCardNotFound = 2,
        ValidationCardNotActive = 3,
        ValidationBalanceToLow = 4,
        ValidationUnknownError = 5,
        ValidationCardHasZeroBalance = 6
    }

    public enum CreditVoucherValidationEnum
    {
        ValidationSuccess = 1,
        ValidationVoucherNotFound = 2,
        ValidationBalanceToLow = 4,
        ValidationUnknownError = 5,
        ValidationVoucherHasZeroBalance = 6
    }
}
