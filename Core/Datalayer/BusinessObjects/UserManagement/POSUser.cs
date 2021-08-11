using System.ComponentModel.DataAnnotations;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using System;

namespace LSOne.DataLayer.BusinessObjects.UserManagement
{
    public class POSUser : DataEntity
    {
        string password;
        bool managerPrivileges;
        bool needsPasswordChange;

        public POSUser()
            : base()
        {
            ID = "";
            password = "";
            managerPrivileges = false;
            UserProfileID = "";

            AllowXReportPrinting = false;
            AllowTenderDeclaration = false;
            AllowFloatingDeclaration = false;
            AllowTransactionSuspension = false;
            AllowTransactionVoiding = false;
            AllowChangeNoVoid = false;
            AllowOpenDrawerWithoutSale = false;

            needsPasswordChange = true;

            NameOnReceipt = "";

            Name = new Name();
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public bool ManagerPrivileges
        {
            get { return managerPrivileges; }
            set { managerPrivileges = value; }
        }

        public bool AllowXReportPrinting { get; set; }

        public bool AllowTenderDeclaration { get; set; }

        public bool AllowFloatingDeclaration { get; set; }

        public bool AllowTransactionSuspension { get; set; }

        public bool AllowTransactionVoiding { get; set; }

        public bool AllowChangeNoVoid { get; set; }

        public bool AllowOpenDrawerWithoutSale { get; set; }

        public RecordIdentifier UserProfileID { get; set; }

        public bool NeedsPasswordChange
        {
            get { return needsPasswordChange; }
            set { needsPasswordChange = value; }
        }

        [StringLength(15)]
        public string NameOnReceipt { get; set; }

        public bool ContinueOnTSErrors { get; set; }

        public bool AllowMultipleLogins { get; set; }

        public PriceOverrideEnum AllowPriceOverride { get; set; }

        public Name Name { get; set; }

        public RecordIdentifier Login { get; set; }
    }
}
