namespace LSOne.DataLayer.BusinessObjects.UserManagement
{
    public class ActionPermission : DataEntity
    {
        public enum Access
        {
            Everyone = 0,
            Manager = 1,
            AllowXReportPriting = 1001,
            AllowTenderDeclaration = 1003,
            AllowFloatingDeclaration = 1004,
            AllowTransactionSuspension = 1015, // Was 1007
            AllowTransactionVoiding = 1010,
            AllowChangeNoVoid = 1011,
            AllowOpenDrawerWithoutSale = 1012
        }
         
        Access accessRights;
        
        public ActionPermission()
            : base("","")
        {
            accessRights = Access.Everyone;
        }

        public Access AccessRights
        {
            get { return accessRights; }
            set { accessRights = value; }
        }

        public bool CheckUserAcccess { get; set; }
    }
}
