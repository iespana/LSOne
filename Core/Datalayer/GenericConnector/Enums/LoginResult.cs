namespace LSOne.DataLayer.GenericConnector.Enums
{
    public enum LoginResult
    {
        Success = 0,
        UserAuthenticationFailed = 1,
        UserLockedOut = 2,
        UnknownServerError = 3,
        CouldNotConnectToDatabase = 4,
        UserDoesNotMatchConnectionIntent = 5,
        TokenNotFound = 6,
        TokenIsUser = 7,
        LoginDisabled = 8
    };
}
