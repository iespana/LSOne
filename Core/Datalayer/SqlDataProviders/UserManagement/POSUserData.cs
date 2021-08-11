using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.UserManagement
{
    public partial class POSUserData : SqlServerDataProviderBase, IPOSUserData
    {
        private static void PopulateMinimalUser(IDataReader dr, POSUser user)
        {
            user.ID = (string)dr["STAFFID"];
            user.Text = (string)dr["NAME"];
            user.NameOnReceipt = (string)dr["NAMEONRECEIPT"];
            user.Password = (string)dr["PASSWORD"];
            user.NeedsPasswordChange = ((byte)dr["CHANGEPASSWORD"] != 0);
            user.ManagerPrivileges = ((byte)dr["MANAGERPRIVILEGES"] != 0);
            user.AllowTransactionVoiding = ((byte)dr["ALLOWTRANSACTIONVOIDING"] != 0);
            user.AllowXReportPrinting = ((byte)dr["ALLOWXREPORTPRINTING"] != 0);
            user.AllowTenderDeclaration = ((byte)dr["ALLOWTENDERDECLARATION"] != 0);
            user.AllowFloatingDeclaration = ((byte)dr["ALLOWFLOATINGDECLARATION"] != 0);
            user.AllowChangeNoVoid = ((byte)dr["ALLOWCHANGENOVOID"] != 0);
            user.AllowTransactionSuspension = ((byte)dr["ALLOWTRANSACTIONSUSPENSION"] != 0);
            user.AllowOpenDrawerWithoutSale = ((byte)dr["ALLOWOPENDRAWERONLY"] != 0);
            user.ContinueOnTSErrors = ((byte)dr["CONTINUEONTSERRORS"] != 0);
            user.AllowPriceOverride = (PriceOverrideEnum)(int)dr["ALLOWPRICEOVERRIDE"];
            user.UserProfileID = (string)dr["USERPROFILE"];
            user.Login = (string)dr["LOGIN"];
        }

        private static void PopulateUser(IDataReader dr, POSUser user)
        {
            PopulateMinimalUser(dr,user);

            user.Name.First = (string)dr["FIRSTNAME"];
            user.Name.Middle = (string)dr["MIDDLENAME"];
            user.Name.Last = (string)dr["LASTNAME"];
            user.Name.Prefix = (string)dr["NAMEPREFIX"];
            user.Name.Suffix = (string)dr["NAMESUFFIX"];
        }

        /// <summary>
        /// Get all POS users
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>All POS users</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "RBOSTAFFTABLE", "NAME", "STAFFID", "NAME");
        }

        /// <summary>
        /// Get POS users based on the filter parameters
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">If specified, the method returns all POS users whose STAFFID matches the <paramref name="id"/> value</param>
        /// <param name="description">If specified, the method returns all POS users whose NAME matches the <paramref name="description"/> value</param>
        /// <param name="maxCount">If maxCount is positive and the number of found POS users is bigger than maxCount, then maxCount will limit the number of returned POS users</param>
        /// <returns>POS users based on the filter parameters</returns>
        public virtual List<DataEntity> Search(IConnectionManager entry, string id, string description, int maxCount)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "SELECT " +
                    (maxCount > 0 ? "TOP " + maxCount : "") +
                    "ISNULL(u.LOGIN,'') AS STAFFLOGIN , ISNULL(NAME,'') AS NAME " +
                    "FROM RBOSTAFFTABLE T " +
                    "LEFT OUTER JOIN USERS u " +
                    "ON T.STAFFID = u.STAFFID " +
                    "WHERE T.DATAAREAID = @dataAreaId AND (NAME LIKE @name OR u.LOGIN LIKE @id) ORDER BY NAME";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", "%" + id + "%");
                MakeParam(cmd, "name", "%" + description + "%");

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "STAFFLOGIN");
            }
        }

        private static string GetSelectPart()
        {
            return @"SELECT S.STAFFID, ISNULL(S.NAME, '') AS NAME,
                    ISNULL(S.PASSWORD, '') AS PASSWORD, ISNULL(S.CHANGEPASSWORD, 0) AS CHANGEPASSWORD,
                    ISNULL(S.MANAGERPRIVILEGES, 0) AS MANAGERPRIVILEGES,
                    ISNULL(S.ALLOWTRANSACTIONVOIDING, 0) AS ALLOWTRANSACTIONVOIDING, ISNULL(S.ALLOWXREPORTPRINTING, 0) AS ALLOWXREPORTPRINTING,
                    ISNULL(S.ALLOWTENDERDECLARATION, 0) AS ALLOWTENDERDECLARATION, ISNULL(S.ALLOWFLOATINGDECLARATION, 0) AS ALLOWFLOATINGDECLARATION,
                    ISNULL(S.ALLOWCHANGENOVOID, 0) AS ALLOWCHANGENOVOID, ISNULL(S.ALLOWTRANSACTIONSUSPENSION, 0) AS ALLOWTRANSACTIONSUSPENSION,
                    ISNULL(S.ALLOWOPENDRAWERONLY, 0) AS ALLOWOPENDRAWERONLY,
                    ISNULL(S.NAMEONRECEIPT, 0) AS NAMEONRECEIPT, ISNULL(S.CONTINUEONTSERRORS, '') AS CONTINUEONTSERRORS, 
                    ISNULL(S.PRICEOVERRIDE, 0) AS ALLOWPRICEOVERRIDE,
                    ISNULL(S.USERPROFILE, '') AS USERPROFILE,
                    COALESCE(US.FirstName, S.FIRSTNAME, '') AS FIRSTNAME,
                    ISNULL(US.MiddleName, '') AS MIDDLENAME,
                    COALESCE(US.LastName, S.LASTNAME, '') AS LASTNAME,
                    ISNULL(US.NamePrefix, '') AS NAMEPREFIX,
                    ISNULL(US.NameSuffix, '') AS NAMESUFFIX,
                    ISNULL(US.LOGIN, '') AS LOGIN 
                    FROM RBOSTAFFTABLE S
                    INNER JOIN POSUSERPROFILE P ON S.USERPROFILE = P.PROFILEID 
                    LEFT OUTER JOIN USERS US ON S.DATAAREAID = US.DATAAREAID AND S.STAFFID = US.STAFFID ";
        }

        private static string GetMinimalSelectPart()
        {
            return  "SELECT S.STAFFID, ISNULL(S.NAME,'') AS NAME, " +
                    "ISNULL(S.PASSWORD,'') AS PASSWORD,ISNULL(S.CHANGEPASSWORD,0) AS CHANGEPASSWORD, " +
                    "ISNULL(S.MANAGERPRIVILEGES,0) AS MANAGERPRIVILEGES, " +
                    "ISNULL(S.ALLOWTRANSACTIONVOIDING,0) AS ALLOWTRANSACTIONVOIDING, ISNULL(S.ALLOWXREPORTPRINTING,0) AS ALLOWXREPORTPRINTING, " +
                    "ISNULL(S.ALLOWTENDERDECLARATION,0) AS ALLOWTENDERDECLARATION,ISNULL(S.ALLOWFLOATINGDECLARATION,0) AS ALLOWFLOATINGDECLARATION, " +
                    "ISNULL(S.ALLOWCHANGENOVOID,0) AS ALLOWCHANGENOVOID,ISNULL(S.ALLOWTRANSACTIONSUSPENSION,0) AS ALLOWTRANSACTIONSUSPENSION, " +
                    "ISNULL(S.ALLOWOPENDRAWERONLY,0) AS ALLOWOPENDRAWERONLY, " +
                    "ISNULL(S.NAMEONRECEIPT,0) AS NAMEONRECEIPT,ISNULL(S.CONTINUEONTSERRORS,'') AS CONTINUEONTSERRORS, " +
                    "ISNULL(S.PRICEOVERRIDE,0) AS ALLOWPRICEOVERRIDE, " +
                    "ISNULL(S.USERPROFILE, '') AS USERPROFILE, " +
                    "ISNULL(US.LOGIN, '') AS LOGIN " +
                    "FROM RBOSTAFFTABLE S " +
                    "INNER JOIN POSUSERPROFILE P ON S.USERPROFILE = P.PROFILEID " +
                    "LEFT OUTER JOIN USERS US ON S.DATAAREAID = US.DATAAREAID AND S.STAFFID = US.STAFFID ";
        }

        /// <summary>
        /// Get POS users based on the filter parameters
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">If specified, the method returns all POS users whose STAFFID matches the <paramref name="id"/> value</param>
        /// <param name="description">If specified, the method returns all POS users whose NAME matches the <paramref name="description"/> value</param>
        /// <returns>POS users based on the filter parameters</returns>
        public virtual List<POSUser> Search(IConnectionManager entry, RecordIdentifier id, string description)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = GetSelectPart() +
                                  "WHERE S.DATAAREAID = @dataAreaId AND (S.NAME LIKE @name OR S.STAFFID LIKE @id) ORDER BY S.NAME";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", "%" + (string) id + "%");
                MakeParam(cmd, "name", "%" + description + "%");

                return Execute<POSUser>(entry, cmd, CommandType.Text, PopulateUser);
            }
        }

        /// <summary>
        /// Get all POS users for the given store and usage intent
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The store ID</param>
        /// <param name="usageIntent">The usage intent</param>
        /// <param name="cacheType">The cache type. This parameter has no effect</param>
        /// <returns>All POS users for the given store and usage intent</returns>
        public virtual List<POSUser> GetList(IConnectionManager entry, RecordIdentifier storeID, UsageIntentEnum usageIntent, CacheType cacheType = CacheType.CacheTypeNone)
        {
            storeID = storeID == "" ? RecordIdentifier.Empty : storeID;

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                if ((usageIntent & UsageIntentEnum.Normal) == UsageIntentEnum.Normal)
                {
                    cmd.CommandText = GetSelectPart() + " WHERE S.DATAAREAID = @dataAreaId AND US.DELETED = 0";

                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                    if (storeID != RecordIdentifier.Empty)
                    {
                        cmd.CommandText += " AND P.STOREID = @STOREID ";
                        MakeParam(cmd, "STOREID", storeID);
                    }
                    
                    return Execute<POSUser>(entry, cmd, CommandType.Text, PopulateUser);
                }

                cmd.CommandText = GetMinimalSelectPart() + " WHERE S.DATAAREAID = @dataAreaId AND US.DELETED = 0";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                if (storeID != RecordIdentifier.Empty)
                {
                    cmd.CommandText += " AND P.STOREID = @STOREID ";
                    MakeParam(cmd, "STOREID", storeID);
                }

                return Execute<POSUser>(entry, cmd, CommandType.Text, PopulateMinimalUser);                
            }
        }

        /// <summary>
        /// Get POS user by ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The staff ID</param>
        /// <param name="usageIntent">The usage intent</param>
        /// <param name="cacheType">The cache type</param>
        /// <returns>POS user</returns>
        public virtual POSUser Get(IConnectionManager entry, RecordIdentifier id, UsageIntentEnum usageIntent, CacheType cacheType = CacheType.CacheTypeNone)
        {
            if (id == "" || id == RecordIdentifier.Empty)
            {
                return null;
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                if ((usageIntent & UsageIntentEnum.Normal) == UsageIntentEnum.Normal)
                {
                    cmd.CommandText = GetSelectPart() + "WHERE S.DATAAREAID = @dataAreaId AND S.STAFFID = @id";

                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                    MakeParam(cmd, "id", (string) id);

                    return Get<POSUser>(entry, cmd, id, PopulateUser, cacheType, UsageIntentEnum.Normal);
                }

                cmd.CommandText = GetMinimalSelectPart() + "WHERE S.DATAAREAID = @dataAreaId AND S.STAFFID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string) id);

                return Get<POSUser>(entry, cmd, id, PopulateMinimalUser, cacheType, usageIntent);
            }
        }

        /// <summary>
        /// Get POS user by ID with option to restrict the query to the given store
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The staff ID</param>
        /// <param name="storeID">The store ID</param>
        /// <param name="staffListLimitedToStore">If true, the restriction to the given store is applied</param>
        /// <param name="cacheType">The cache type</param>
        /// <returns>POS user</returns>
        public virtual POSUser GetPerStore(IConnectionManager entry, RecordIdentifier id, RecordIdentifier storeID, bool staffListLimitedToStore, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = GetSelectPart() + "WHERE S.DATAAREAID = @dataAreaId AND S.STAFFID = @id";

                if (staffListLimitedToStore)
                {
                    cmd.CommandText += " AND P.STOREID = @store";
                    MakeParam(cmd, "store", (string) storeID);
                }
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);                
                MakeParam(cmd, "id", (string)id);                

                return Get<POSUser>(entry, cmd, id, PopulateUser, cacheType, UsageIntentEnum.Normal);
            }
        }

        /// <summary>
        /// Gets the name and the name on the receipt of the staff based on the given staff ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The staff I</param>
        /// <param name="name">The staff name</param>
        /// <param name="nameOnReceipt">The staff name on the receipt</param>
        /// <returns>true if the operation is successful; false if the operation fails</returns>
        public virtual bool GetNameAndNameOnReceipt(IConnectionManager entry, RecordIdentifier id, ref string name, ref string nameOnReceipt)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =   "SELECT ISNULL(S.NAME,'') AS NAME, ISNULL(S.NAMEONRECEIPT,'') AS NAMEONRECEIPT " +
                                    "FROM RBOSTAFFTABLE s " +
                                    "WHERE S.DATAAREAID = @dataAreaId AND S.STAFFID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string)id);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.Read())
                    {
                        return true;
                    }

                    name = "";
                    nameOnReceipt = "";
                    return false;
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<POSUser>(entry, "RBOSTAFFTABLE", "STAFFID", id);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord<POSUser>(entry, "RBOSTAFFTABLE", "STAFFID", id, Permission.SecurityDeleteUser);
        }

        public virtual void Save(IConnectionManager entry, POSUser user)
        {
            var statement = new SqlServerStatement("RBOSTAFFTABLE");

            if (!Exists(entry, user.ID))
            {
                ValidateSecurity(entry, Permission.SecurityCreateNewUsers);

                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("STAFFID", (string)user.ID);
            }
            else
            {
                ValidateSecurity(entry, Permission.SecurityEditUser);

                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("STAFFID", (string)user.ID);
            }

            statement.AddField("NAME", user.Text);
            statement.AddField("NAMEONRECEIPT", user.NameOnReceipt);
            statement.AddField("PASSWORD", user.Password);
            statement.AddField("CHANGEPASSWORD", user.NeedsPasswordChange ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("MANAGERPRIVILEGES", user.ManagerPrivileges ? 1 : 0, SqlDbType.TinyInt);

            statement.AddField("ALLOWTRANSACTIONVOIDING", user.AllowTransactionVoiding ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ALLOWXREPORTPRINTING", user.AllowXReportPrinting ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ALLOWTENDERDECLARATION", user.AllowTenderDeclaration ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ALLOWFLOATINGDECLARATION", user.AllowFloatingDeclaration ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ALLOWCHANGENOVOID", user.AllowChangeNoVoid ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ALLOWTRANSACTIONSUSPENSION", user.AllowTransactionSuspension ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ALLOWOPENDRAWERONLY", user.AllowOpenDrawerWithoutSale ? 1 : 0, SqlDbType.TinyInt);

            statement.AddField("CONTINUEONTSERRORS", user.ContinueOnTSErrors ? 1 : 0, SqlDbType.TinyInt);

            statement.AddField("PRICEOVERRIDE", (int)user.AllowPriceOverride, SqlDbType.Int);
            statement.AddField("USERPROFILE", (string)user.UserProfileID);

            Save(entry, user, statement);
        }
    }
}