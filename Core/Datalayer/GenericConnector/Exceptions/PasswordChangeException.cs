namespace LSOne.DataLayer.GenericConnector.Exceptions
{
    public class PasswordChangeException : LSOneException
    {
        public PasswordChangeException()
            : base("Password has to be changed.")
        {

        }
    }
}
