using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Customers
{
    public class CustomersInGroupData : SqlServerDataProviderBase, ICustomersInGroupData
    {
        /// <summary>
        /// Adds a customer to a customer group
        /// </summary>
        /// <param name="entry">Entry to the database</param>
        /// <param name="inGroup">The customer and group information</param>
        public virtual void Save(IConnectionManager entry, CustomerInGroup inGroup)
        {
            var statement = new SqlServerStatement("CUSTINGROUP");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageCustomerGroups);

            if (!Exists(entry, inGroup.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ACCOUNTNUM", (string)inGroup.ID.PrimaryID);
                statement.AddKey("GROUPID", (string)inGroup.GroupID);
                statement.AddField("DEFAULTGROUP", inGroup.Default ? 1 : 0, SqlDbType.TinyInt);

                entry.Connection.ExecuteStatement(statement);
            }
        }

        public virtual void ClearDefaultValueForCustomer(IConnectionManager entry, CustomerInGroup inGroup)
        {
            var statement = new SqlServerStatement("CUSTINGROUP");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageCustomerGroups);

            if (Exists(entry, inGroup.ID))
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ACCOUNTNUM", (string)inGroup.ID.PrimaryID);
                statement.AddCondition("GROUPID", (string)inGroup.GroupID);

                statement.AddField("DEFAULTGROUP", 0, SqlDbType.TinyInt);

                entry.Connection.ExecuteStatement(statement);
            }
        }

        public virtual void SetGroupAsDefault(IConnectionManager entry, CustomerInGroup inGroup)
        {
            var statement = new SqlServerStatement("CUSTINGROUP");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageCustomerGroups);

            if (Exists(entry, inGroup.ID))
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ACCOUNTNUM", (string)inGroup.ID.PrimaryID);
                statement.AddCondition("GROUPID", (string)inGroup.GroupID);

                statement.AddField("DEFAULTGROUP", 1, SqlDbType.TinyInt);

                entry.Connection.ExecuteStatement(statement);
            }
        }

        /// <summary>
        /// Removes a customer from a customer group
        /// </summary>
        /// <param name="entry">Entry to the database</param>
        /// <param name="ID">The combines ID for customer and group that is to be deleted</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            DeleteRecord(entry, "CUSTINGROUP", new string[] { "ACCOUNTNUM", "GROUPID" }, ID, BusinessObjects.Permission.ManageCustomerGroups);
        }

        /// <summary>
        /// Returns true if the combined id of customer id and group id exists already
        /// </summary>
        /// <param name="entry">Entry to the database</param>
        /// <param name="id">Combined id of customer and group</param>
        /// <returns></returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "CUSTINGROUP", new string[] { "ACCOUNTNUM", "GROUPID" }, id);
        }

        public List<CustomerInGroup> GetCustomersInGroupList(IConnectionManager entry,
                                                            RecordIdentifier customerGroupID,
                                                            int? recordFrom,
                                                            int? recordTo)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT * 
                      FROM 
                      (SELECT CGR.ACCOUNTNUM, CGR.GROUPID, ISNULL(CUST.NAME,'') AS CUSTOMERNAME, ISNULL(CGR.DEFAULTGROUP, 0) DEFAULTGROUP,
                      ISNULL(GR.NAME,'') AS GROUPNAME, 
                      ROW_NUMBER() OVER(ORDER BY CUST.NAME) AS ROWNR,  
                      ISNULL(CUST.FIRSTNAME,'') AS FIRSTNAME,ISNULL(CUST.MIDDLENAME,'') AS MIDDLENAME,ISNULL(CUST.LASTNAME,'') AS LASTNAME, 
                      ISNULL(CUST.NAMEPREFIX,'') AS NAMEPREFIX,ISNULL(CUST.NAMESUFFIX,'') AS NAMESUFFIX 
                      FROM CUSTOMER CUST 
                      JOIN CUSTINGROUP CGR ON CGR.ACCOUNTNUM = CUST.ACCOUNTNUM AND CGR.DATAAREAID = CUST.DATAAREAID 
                      JOIN CUSTGROUP GR ON GR.ID = CGR.GROUPID AND GR.DATAAREAID = CGR.DATAAREAID
                      WHERE CGR.DATAAREAID = @DATAAREAID AND CGR.GROUPID = @GROUPID) CT ";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "GROUPID", (string)customerGroupID);

                if (recordFrom.HasValue && recordTo.HasValue)
                {
                    cmd.CommandText += "WHERE CT.ROWNR BETWEEN @RECORDFROM AND @RECORDTO ";
                    MakeParam(cmd, "RECORDFROM", recordFrom.Value, SqlDbType.Int);
                    MakeParam(cmd, "RECORDTO", recordTo.Value, SqlDbType.Int);
                }

                return Execute<CustomerInGroup>(entry, cmd, CommandType.Text, PopulateCustomer);
            }
        }

        public virtual List<CustomerInGroup> GetGroupsForCustomerList(IConnectionManager entry, RecordIdentifier customerID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT * 
                      FROM 
                      (SELECT CGR.ACCOUNTNUM, CGR.GROUPID, ISNULL(CUST.NAME,'') AS CUSTOMERNAME, ISNULL(CGR.DEFAULTGROUP, 0) DEFAULTGROUP,
                      ISNULL(GR.NAME,'') AS GROUPNAME,                       
                      ISNULL(CUST.FIRSTNAME,'') AS FIRSTNAME,ISNULL(CUST.MIDDLENAME,'') AS MIDDLENAME,ISNULL(CUST.LASTNAME,'') AS LASTNAME, 
                      ISNULL(CUST.NAMEPREFIX,'') AS NAMEPREFIX,ISNULL(CUST.NAMESUFFIX,'') AS NAMESUFFIX 
                      FROM CUSTOMER CUST 
                      JOIN CUSTINGROUP CGR ON CGR.ACCOUNTNUM = CUST.ACCOUNTNUM AND CGR.DATAAREAID = CUST.DATAAREAID 
                      JOIN CUSTGROUP GR ON GR.ID = CGR.GROUPID AND GR.DATAAREAID = CGR.DATAAREAID
                      WHERE CGR.DATAAREAID = @DATAAREAID AND CGR.ACCOUNTNUM = @CUSTOMERID) CT ";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "CUSTOMERID", (string)customerID);

                return Execute<CustomerInGroup>(entry, cmd, CommandType.Text, PopulateCustomer);
            }
        }

        private static void PopulateCustomer(IDataReader dr, CustomerInGroup customer)
        {
            customer.ID = new RecordIdentifier((string)dr["ACCOUNTNUM"], (string)dr["GROUPID"]);
            customer.Text = (string)dr["CUSTOMERNAME"];
            customer.GroupName = (string)dr["GROUPNAME"];
            customer.Default = ((byte)dr["DEFAULTGROUP"] != 0);
            customer.Name.First = (string)dr["FIRSTNAME"];
            customer.Name.Last = (string)dr["LASTNAME"];
            customer.Name.Middle = (string)dr["MIDDLENAME"];
            customer.Name.Prefix = (string)dr["NAMEPREFIX"];
            customer.Name.Suffix = (string)dr["NAMESUFFIX"];
        }
        
    }
}
