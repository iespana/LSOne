namespace LSOne.DataLayer.GenericConnector.Exceptions
{
    public class PermissionException : LSOneException
    {

        public PermissionException()
            : base("No permission for that operation.")
        {

        }

        public PermissionException(string permission)
            : base("No permission for that operation. The needed permission is: " + permission)
        {

        }
    }
}
