using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Sequencable;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.DataProviders.Sequences;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Customers
{
    public class CustomerGroupData : SqlServerDataProviderBase, ISequenceable, ICustomerGroupData
    {
        private static string SelectStringWithoutDefaultGroup
        {
            get 
            {
                return @"SELECT GR.ID, ISNULL(NAME, '') NAME, 
                                    ISNULL(EXCLUSIVE, 0) EXCLUSIVE, 
                                    ISNULL(CATEGORY, '') CATEGORY, 
                                    ISNULL(DESCRIPTION, '') CATEGORYNAME, 
                                    ISNULL(PURCHASEAMOUNT, 0) PURCHASEAMOUNT,  
                                    ISNULL(USEPURCHASELIMIT, 0) USEPURCHASELIMIT,
                                    ISNULL(PURCHASEPERIOD, 0) PURCHASEPERIOD,
                                    CAST(0 AS TINYINT) AS DEFAULTGROUP
                                    FROM CUSTGROUP GR
                                    LEFT OUTER JOIN CUSTGROUPCATEGORY CAT ON CAT.ID = GR.CATEGORY AND CAT.DATAAREAID = GR.DATAAREAID ";
            }
        }

        /// <summary>
        /// Returns a list of all Customer groups 
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of groups</returns>
        public virtual List<CustomerGroup> GetList(IConnectionManager entry)
        {
            
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = SelectStringWithoutDefaultGroup +
                                    @"WHERE GR.DATAAREAID = @DATAAREAID
                                    ORDER BY NAME";


                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Execute<CustomerGroup>(entry, cmd, CommandType.Text, PopulateCustomerGroup);
            }
        }

        public virtual CustomerGroup Get(IConnectionManager entry, RecordIdentifier customerGroupID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = SelectStringWithoutDefaultGroup + @"WHERE GR.DATAAREAID = @DATAAREAID AND GR.ID = @customerGroupID";


                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "customerGroupID", (string)customerGroupID);

                return Get<CustomerGroup>(entry, cmd, customerGroupID, PopulateCustomerGroup, cacheType, UsageIntentEnum.Normal);
            }
        }

        public virtual List<CustomerGroup> GetGroupsForCustomer(IConnectionManager entry, RecordIdentifier customerID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT GR.ID, ISNULL(NAME, '') NAME, 
                                    ISNULL(EXCLUSIVE, 0) EXCLUSIVE, 
                                    ISNULL(CATEGORY, '') CATEGORY, 
                                    ISNULL(DESCRIPTION, '') CATEGORYNAME, 
                                    ISNULL(PURCHASEAMOUNT, 0) PURCHASEAMOUNT,  
                                    ISNULL(USEPURCHASELIMIT, 0) USEPURCHASELIMIT,
                                    ISNULL(PURCHASEPERIOD, 0) PURCHASEPERIOD,
                                    ISNULL(CGR.DEFAULTGROUP, 0) AS DEFAULTGROUP
                                    FROM CUSTGROUP GR
                                    LEFT OUTER JOIN CUSTGROUPCATEGORY CAT ON CAT.ID = GR.CATEGORY AND CAT.DATAAREAID = GR.DATAAREAID 
                                    JOIN CUSTINGROUP CGR ON CGR.GROUPID = GR.ID AND CGR.DATAAREAID = GR.DATAAREAID 
                                    WHERE GR.DATAAREAID = @DATAAREAID
                                    AND CGR.ACCOUNTNUM = @CUSTOMERID
                                    ORDER BY NAME";


                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "CUSTOMERID", (string)customerID);

                return Execute<CustomerGroup>(entry, cmd, CommandType.Text, PopulateCustomerGroup);
            }
        }

        public virtual CustomerGroup GetDefaultCustomerGroup(IConnectionManager entry, RecordIdentifier customerID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT GR.ID, ISNULL(NAME, '') NAME, 
                                    ISNULL(EXCLUSIVE, 0) EXCLUSIVE, 
                                    ISNULL(CATEGORY, '') CATEGORY, 
                                    ISNULL(DESCRIPTION, '') CATEGORYNAME, 
                                    ISNULL(PURCHASEAMOUNT, 0) PURCHASEAMOUNT,  
                                    ISNULL(USEPURCHASELIMIT, 0) USEPURCHASELIMIT,
                                    ISNULL(PURCHASEPERIOD, 0) PURCHASEPERIOD,
                                    ISNULL(CGR.DEFAULTGROUP, 0) AS DEFAULTGROUP
                                    FROM CUSTGROUP GR
                                    LEFT OUTER JOIN CUSTGROUPCATEGORY CAT ON CAT.ID = GR.CATEGORY AND CAT.DATAAREAID = GR.DATAAREAID 
                                    JOIN CUSTINGROUP CGR ON CGR.GROUPID = GR.ID AND CGR.DATAAREAID = GR.DATAAREAID 
                                    WHERE GR.DATAAREAID = @DATAAREAID
                                    AND CGR.ACCOUNTNUM = @CUSTOMERID
                                    AND CGR.DEFAULTGROUP = 1
                                    ORDER BY NAME";


                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "CUSTOMERID", (string)customerID);

                var result = Execute<CustomerGroup>(entry, cmd, CommandType.Text, PopulateCustomerGroup);
                return (result.Count == 1) ? result[0] : null;
            }
        }

        /// <summary>
        /// Deletes a customer group with a given ID. All customers (if any) that were in that group are also removed from it
        /// </summary>
        /// <remarks>Requires the 'Edit customer group' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The ID of the group to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier groupID)
        {
            DeleteRecord(entry, "CUSTGROUP", "ID", groupID, BusinessObjects.Permission.ManageCustomerGroups);
            DeleteRecord(entry, "CUSTINGROUP", "GROUPID", groupID, BusinessObjects.Permission.ManageCustomerGroups);
        }

        public virtual void DeleteCustomerFromGroup(IConnectionManager entry, RecordIdentifier customerID, RecordIdentifier groupID)
        {
            var statement = new SqlServerStatement("CUSTINGROUP");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageCustomerGroups);
            
            statement.StatementType = StatementType.Delete;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("ACCOUNTNUM", (string)customerID);
            statement.AddCondition("GROUPID", (string)groupID);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual bool GroupHasCustomers(IConnectionManager entry, RecordIdentifier groupID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT ISNULL(COUNT(GROUPID), 0) GRCNT 
                    FROM CUSTINGROUP
                    WHERE GROUPID = @GROUPID 
                    AND DATAAREAID = @DATAAREAID ";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "GROUPID", (string)groupID);

                return (int)entry.Connection.ExecuteScalar(cmd) > 0;
            }
        }

        private static void PopulateCustomerGroup(IDataReader dr, CustomerGroup group)
        {
            group.ID = (string)dr["ID"];
            group.Text = (string)dr["NAME"];

            group.Exclusive = ((byte)dr["EXCLUSIVE"] != 0);
            group.MaxDiscountedPurchases = (decimal)dr["PURCHASEAMOUNT"];
            group.UsesDiscountedPurchaseLimit = ((byte) dr["USEPURCHASELIMIT"] != 0);
            group.Period = (CustomerGroup.PeriodEnum) ((byte)dr["PURCHASEPERIOD"]);
            group.DefaultGroup = ((byte)dr["DEFAULTGROUP"] != 0);

            group.Category = new GroupCategory();
            group.Category.ID = (string)dr["CATEGORY"];
            group.Category.Text = (string)dr["CATEGORYNAME"];

            
        }

        /// <summary>
        /// Checks if a customer group by a given ID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">ID of the customer to check for</param>
        /// <returns>True if the customer exists, else false</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<CustomerGroup>(entry, "CUSTGROUP", "ID", id);
        }

        public virtual void Save(IConnectionManager entry, CustomerGroup group)
        {
            var statement = new SqlServerStatement("CUSTGROUP");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageCustomerGroups);

            bool isNew = false;
            if (group.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                group.ID = DataProviderFactory.Instance.Get<INumberSequenceData, NumberSequence>().GenerateNumberFromSequence(entry, new CustomerGroupData()); 
            }

            if (isNew || !Exists(entry, group.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (string)group.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)group.ID);
            }

            statement.AddField("NAME", group.Text);
            statement.AddField("EXCLUSIVE", group.Exclusive ? 1 : 0, SqlDbType.Int);
            statement.AddField("CATEGORY", (string)group.Category.ID);
            statement.AddField("PURCHASEAMOUNT", group.MaxDiscountedPurchases, SqlDbType.Decimal);
            statement.AddField("USEPURCHASELIMIT", group.UsesDiscountedPurchaseLimit ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("PURCHASEPERIOD", (int)group.Period, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="CustomerGroup" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        /// <param name="sort">A string defining the sort column</param>
        public List<CustomerGroup> Search(IConnectionManager entry, string searchString,
                                               int rowFrom, int rowTo,
                                               bool beginsWith, CustomerGroupSorting sort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                var modifiedSearchString = (beginsWith ? "" : "%") + searchString + "%";

                cmd.CommandText =
                    @" SELECT * FROM (
                        SELECT A.ID, 
                        ISNULL(A.NAME, '') NAME, 
                        ISNULL(A.EXCLUSIVE, 0) EXCLUSIVE, 
                        ISNULL(A.CATEGORY, '') CATEGORY, 
                        ISNULL(B.DESCRIPTION, '') CATEGORYNAME, 
                        ISNULL(A.PURCHASEAMOUNT, 0) PURCHASEAMOUNT, 
                        ISNULL(A.USEPURCHASELIMIT, 0) USEPURCHASELIMIT,
                        ISNULL(A.PURCHASEPERIOD, 0) PURCHASEPERIOD,
                        CAST(0 AS TINYINT) DEFAULTGROUP,                        
                        ROW_NUMBER() OVER(ORDER BY A." + GetSortColumn(sort) + @") AS ROW
                        FROM CUSTGROUP A
                        LEFT JOIN CUSTGROUPCATEGORY B ON B.ID = A.CATEGORY AND B.DATAAREAID = A.DATAAREAID 
                        WHERE A.DATAAREAID = @DATAAREAID
                        AND (A.NAME Like @SEARCHSTRING)) S
                        WHERE S.ROW BETWEEN " + rowFrom + " AND " + rowTo;


                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "SEARCHSTRING", modifiedSearchString);

                return Execute<CustomerGroup>(entry, cmd, CommandType.Text, PopulateCustomerGroup);
            }
        }

        private static string GetSortColumn(CustomerGroupSorting sortEnum)
        {
            var sortColumn = "";

            switch (sortEnum)
            {
                case CustomerGroupSorting.ID:
                    sortColumn = "ID";
                    break;
                case CustomerGroupSorting.Name:
                    sortColumn = "NAME";
                    break;
            }

            return sortColumn;
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "CUSTGROUP"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "CUSTGROUP", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
