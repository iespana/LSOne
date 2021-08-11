using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.UserManagement
{
    public class UserMigrationCommands : SqlServerDataProviderBase, IUserMigrationCommands
    {
        private static void PopulatePOSeUser(IDataReader dr, POSUser user)
        {
            user.ID = (string)dr["STAFFID"];
            user.Text = (string)dr["NAME"];
            user.NameOnReceipt = (string)dr["NAMEONRECEIPT"];
            //user.StoreID = (string)dr["STOREID"];
            user.Password = (string)dr["PASSWORD"];
            user.NeedsPasswordChange = ((byte)dr["CHANGEPASSWORD"] != 0);

            user.ManagerPrivileges = ((byte)dr["MANAGERPRIVILEGES"] != 0);

            //user.StoreDescription = (string)dr["STORENAME"];
            //user.LayoutID = (string)dr["LAYOUTID"];
            //user.LayoutDescription = (string)dr["LAYOUTNAME"];
            //user.VisualProfileID = (string)dr["VISUALPROFILE"];
            //user.VisualProfileDescription = (string)dr["VisualProfileName"];

            user.Name.First = (string)dr["FIRSTNAME"];
            user.Name.Middle = (string)dr["MIDDLENAME"];
            user.Name.Last = (string)dr["LASTNAME"];
            user.Name.Prefix = (string)dr["NAMEPREFIX"];
            user.Name.Suffix = (string)dr["NAMESUFFIX"];
        }

        private static void PopulateUser(IDataReader dr, LSOne.DataLayer.BusinessObjects.UserManagement.User user)
        {
            Name name;

            user.Guid = (Guid)dr["GUID"];
            user.Login = (string)dr["Login"];
            user.IsDomainUser = (bool)dr["IsDomainUser"];
            user.Disabled = ((int)dr["Disabled"] == 1);
            user.StaffID = (string)dr["STAFFID"];

            name = user.Name;

            name.First = (string)dr["FirstName"];
            name.Middle = (string)dr["MiddleName"];
            name.Last = (string)dr["LastName"];
            name.Prefix = (string)dr["NamePrefix"];
            name.Suffix = (string)dr["NameSuffix"];
        }

        private static string GetSelectPartForPosUser()
        {
            return @"Select s.STAFFID, ISNULL(s.NAME,'') as NAME,
                    ISNULL(s.PASSWORD,'') as PASSWORD,ISNULL(s.CHANGEPASSWORD,0) as CHANGEPASSWORD,
                    ISNULL(s.MANAGERPRIVILEGES,0) as MANAGERPRIVILEGES,
                    ISNULL(s.ALLOWTRANSACTIONVOIDING,0) as ALLOWTRANSACTIONVOIDING, ISNULL(s.ALLOWXREPORTPRINTING,0) as ALLOWXREPORTPRINTING,
                    ISNULL(s.ALLOWTENDERDECLARATION,0) as ALLOWTENDERDECLARATION,ISNULL(s.ALLOWFLOATINGDECLARATION,0) as ALLOWFLOATINGDECLARATION,
                    ISNULL(s.ALLOWCHANGENOVOID,0) as ALLOWCHANGENOVOID,ISNULL(s.ALLOWTRANSACTIONSUSPENSION,0) as ALLOWTRANSACTIONSUSPENSION,
                    ISNULL(s.ALLOWOPENDRAWERONLY,0) as ALLOWOPENDRAWERONLY,
                    ISNULL(s.NAMEONRECEIPT,0) as NAMEONRECEIPT,ISNULL(s.CONTINUEONTSERRORS,'') as CONTINUEONTSERRORS, 
                    ISNULL(s.PRICEOVERRIDE,0) as ALLOWPRICEOVERRIDE,
                    COALESCE(us.FirstName, s.FIRSTNAME, '') as FIRSTNAME,
                    ISNULL(us.MiddleName,'') as MIDDLENAME,
                    COALESCE(us.LastName, s.LASTNAME, '') as LASTNAME,
                    ISNULL(us.NamePrefix,'') as NAMEPREFIX,
                    ISNULL(us.NameSuffix,'') as NAMESUFFIX 
                    from RBOSTAFFTABLE s 
                    left outer join USERS us on s.DATAAREAID = us.DATAAREAID and s.STAFFID = us.STAFFID ";
        }

        public virtual List<POSUser> GetNonUserPosUsers(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                // No permission needed to search users
                ValidateSecurity(entry);

                cmd.CommandText = GetSelectPartForPosUser() +
                                  "where s.DATAAREAID = @dataAreaId and us.STAFFID IS NULL";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var result = Execute<POSUser>(entry, cmd, CommandType.Text, PopulatePOSeUser);

                return result;
            }
        }

        public virtual List<LSOne.DataLayer.BusinessObjects.UserManagement.User> GetNonPosUserUsers(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                // No permission needed to search users
                ValidateSecurity(entry);

                cmd.CommandText =
                    @"Select s.GUID, s.Login, s.IsDomainUser,s.IsServerUser, s.FirstName, s.MiddleName, s.LastName, s.NamePrefix,s.NameSuffix,s.Disabled,ISNULL(s.STAFFID,'') as STAFFID
                                from vSECURITY_AllUsers_1_0 s
                                left outer join RBOSTAFFTABLE u on s.DATAAREAID = u.DATAAREAID and s.STAFFID = u.STAFFID
                                where u.STAFFID is null";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return Execute<LSOne.DataLayer.BusinessObjects.UserManagement.User>(entry, cmd, CommandType.Text,
                    PopulateUser);
            }
        }
    }
}
