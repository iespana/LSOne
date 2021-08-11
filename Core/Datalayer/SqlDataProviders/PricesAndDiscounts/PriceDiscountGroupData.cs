using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.PricesAndDiscounts
{
    public class PriceDiscountGroupData : SqlServerDataProviderBase, IPriceDiscountGroupData
    {
        private PriceDiscountModuleEnum module;
        private PriceDiscGroupEnum type;

        public PriceDiscountGroupData()
        { }

        private void SetSequence(PriceDiscountModuleEnum module, PriceDiscGroupEnum type)
        {
            this.module = module;
            this.type = type;
        }

        private static void PopulatePriceDiscountGroup(IDataReader dr, PriceDiscountGroup group)
        {
            group.Module = (PriceDiscountModuleEnum)dr["MODULE"];
            group.Type = (PriceDiscGroupEnum)dr["TYPE"];
            group.GroupID = (string)dr["GROUPID"];
            group.Text = (string)dr["NAME"];
            group.IncludeTax = ((byte)dr["INCLTAX"] != 0);
        }

        private static void PopulatePriceDiscountGroupListItem(IDataReader dr, DataEntity group)
        {
            group.ID = (string)dr["GROUPID"];
            group.Text = (string)dr["NAME"];
        }

        private static void PopulateStoreInGroup(IDataReader dr, StoreInPriceGroup store)
        {
            store.StoreID = (string)dr["STOREID"];
            store.Text = (string)dr["STORENAME"];
            store.Level = (int)dr["LEVEL"];
        }

        private static void PopulateMinimumGroupInStore(IDataReader dr, StoreInPriceGroup line)
        {
            line.StoreID = (string)dr["STOREID"];
            line.Level = (int)dr["LEVEL"];
            line.PriceGroupID = (string)dr["PRICEGROUPID"];
            line.Dirty = false;
        }

        private static void PopulateGroupInStore(IDataReader dr, StoreInPriceGroup line)
        {
            line.StoreID = (string)dr["STOREID"];
            line.Level = (int)dr["LEVEL"];
            line.PriceGroupID = (string)dr["PRICEGROUPID"];
            line.PriceGroupName = (string)dr["PRICEGROUPNAME"];
            line.IncludeTaxForPriceGroup = Convert.ToBoolean(dr["INCLTAX"]);
            line.Dirty = false;
        }

        public List<DataEntity> GetGroupList(IConnectionManager entry, PriceDiscountModuleEnum module, PriceDiscGroupEnum type, bool orderByName = false)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    "Select g.GROUPID,ISNULL(g.NAME,'') as NAME " +
                    "from PRICEDISCGROUP g " +
                    "where g.DATAAREAID = @dataAreaId and g.TYPE = @type and g.MODULE = @module order by " + (orderByName ? "NAME" : "GROUPID");

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "type", (int)type, SqlDbType.Int);
                MakeParam(cmd, "module", (int)module, SqlDbType.Int);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, PopulatePriceDiscountGroupListItem);
            }
        }

        public Dictionary<RecordIdentifier, string> GetGroupDictionary(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"Select g.MODULE,g.TYPE,g.GROUPID,ISNULL(g.NAME,'') as NAME from PRICEDISCGROUP g";

                var result = new Dictionary<RecordIdentifier, string>();

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    while (dr.Read())
                    {
                        result.Add(new RecordIdentifier((int)dr["MODULE"], (int)dr["TYPE"], (string)dr["GROUPID"]), (string)dr["NAME"]);
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }
                return result;
            }
        }

        public List<PriceDiscountGroup> GetGroups(IConnectionManager entry, PriceDiscountModuleEnum module,
                                                         PriceDiscGroupEnum type, int sortColumn, bool backwardsSort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                string sort;
                switch (sortColumn)
                {
                    case 0:
                        sort = "g.GROUPID ASC";
                        break;

                    case 1:
                        sort = "g.NAME ASC";
                        break;

                    case 2:
                        sort = "g.INCLTAX ASC";
                        break;

                    default:
                        sort = "";
                        break;
                }

                if (sort != "")
                {
                    if (backwardsSort)
                    {
                        sort = sort.Replace("ASC", "DESC");
                    }

                    sort = " order by " + sort;
                }

                cmd.CommandText =
                    "Select g.MODULE,g.TYPE,g.GROUPID,ISNULL(g.NAME,'') as NAME,ISNULL(g.INCLTAX,0) as INCLTAX " +
                    "from PRICEDISCGROUP g " +
                    "where g.DATAAREAID = @dataAreaId and g.TYPE = @type and g.MODULE = @module" + sort;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "type", (int)type, SqlDbType.Int);
                MakeParam(cmd, "module", (int)module, SqlDbType.Int);

                return Execute<PriceDiscountGroup>(entry, cmd, CommandType.Text, PopulatePriceDiscountGroup);
            }
        }

        public virtual PriceDiscountGroup Get(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    "Select g.MODULE,g.TYPE,g.GROUPID,ISNULL(g.NAME,'') as NAME,ISNULL(g.INCLTAX,0) as INCLTAX " +
                    "from PRICEDISCGROUP g " +
                    "where g.DATAAREAID = @dataAreaId and g.TYPE = @type and g.MODULE = @module and GroupID = @groupID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "module", (int)id, SqlDbType.Int);
                MakeParam(cmd, "type", (int)id.SecondaryID, SqlDbType.Int);
                MakeParam(cmd, "groupID", (string)id.SecondaryID.SecondaryID);

                var groups = Execute<PriceDiscountGroup>(entry, cmd, CommandType.Text, PopulatePriceDiscountGroup);

                return (groups.Count > 0) ? groups[0] : null;
            }
        }

        public virtual DataEntity GetGroupID(IConnectionManager entry, RecordIdentifier groupId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    "Select g.GROUPID,ISNULL(g.NAME,'') as NAME " +
                    "from PRICEDISCGROUP g " +
                    "where GroupID = @groupID";

                MakeParam(cmd, "groupID", (string)groupId);

                var groups = Execute<DataEntity>(entry, cmd, CommandType.Text, PopulatePriceDiscountGroupListItem);

                return (groups.Count > 0) ? groups[0] : null;
            }
        }

        public virtual bool ExistsForStore(IConnectionManager entry, RecordIdentifier storeID)
        {
            return RecordExists(entry, "RBOLOCATIONPRICEGROUP", "STOREID", storeID);
        }

        /// <summary>
        /// Tells you if a store is in a price group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeId">The ID of the store to check for</param>
        /// <param name="groupId">The ID of the group to check</param>
        /// <returns>Whether the store with the given ID is in the price group with the given ID</returns>
        public virtual bool StoreIsInPriceGroup(IConnectionManager entry, RecordIdentifier storeId, RecordIdentifier groupId)
        {
            return RecordExists(entry, "RBOLOCATIONPRICEGROUP", new[] { "STOREID", "PRICEGROUPID" }, new RecordIdentifier(storeId, groupId));
        }

        /// <summary>
        /// Removes a store with a given ID from a price group with a given ID
        /// </summary>
        /// <remarks>Requires the 'Edit price group' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeId">The ID of the store to remove from price group</param>
        /// <param name="groupId">The ID of the price group to remove from</param>
        public virtual void RemoveStoreFromPriceGroup(IConnectionManager entry, RecordIdentifier storeId, RecordIdentifier groupId)
        {
            DeleteRecord(entry, "RBOLOCATIONPRICEGROUP", new[] { "STOREID", "PRICEGROUPID" }, new RecordIdentifier(storeId, groupId), BusinessObjects.Permission.StoreEdit);
        }

        public void RemoveStoresFromPriceGroup(
            IConnectionManager entry,
            RecordIdentifier groupId)
        {
            var allStoresInGroup = GetStoresInPriceGroup(entry, groupId);
            foreach (var storeInGroup in allStoresInGroup)
            {
                RemoveStoreFromPriceGroup(entry, storeInGroup.StoreID, groupId);
            }
        }

        /// <summary>
        /// Adds a store with a given ID to a price group with a given ID. The level of the record is one higher than 
        /// 
        /// </summary>
        /// <remarks>Requires the 'Edit price group' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeId">The ID of the store to add to a price group</param>
        /// <param name="groupId">The ID of the price group to add a store to</param>
        public virtual void AddStoreToPriceGroup(IConnectionManager entry, RecordIdentifier storeId, RecordIdentifier groupId)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.StoreEdit);

            if (!StoreIsInPriceGroup(entry, storeId, groupId))
            {
                var statement = new SqlServerStatement("RBOLOCATIONPRICEGROUP") { StatementType = StatementType.Insert };

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("STOREID", (string)storeId);
                statement.AddKey("PRICEGROUPID", (string)groupId);

                // Gets the next level for the selected store
                int level = GetNextLevel(entry, storeId);
                statement.AddField("LEVEL_", level, SqlDbType.Int);

                entry.Connection.ExecuteStatement(statement);
            }
        }

        public virtual void UpdateStoreInPriceGroup(IConnectionManager entry, StoreInPriceGroup line)
        {
            var statement = new SqlServerStatement("RBOLOCATIONPRICEGROUP") { StatementType = StatementType.Update };

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("STOREID", (string)line.ID);
            statement.AddCondition("PRICEGROUPID", (string)line.PriceGroupID);

            statement.AddField("LEVEL_", Convert.ToString(line.Level));

            entry.Connection.ExecuteStatement(statement);
        }

        private static int GetNextLevel(IConnectionManager entry, RecordIdentifier storeId)
        {
            int level = 0;

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select ISNULL(MAX(LEVEL_), 0) as LEVEL " +
                    "From RBOLOCATIONPRICEGROUP " +
                    "Where STOREID = @storeId and DATAAREAID = @dataAreaId ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeId", (string)storeId);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.Read())
                    {
                        level = (int)dr["LEVEL"];
                    }

                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }
                return level + 1;
            }
        }

        /// <summary>
        /// Gets the price group line with the given store id and price group id
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeId">The ID of the store</param>
        /// <param name="groupId">The ID of the price group to get</param>
        /// <returns>The price group line for the given store and price group id</returns>
        public StoreInPriceGroup GetStoreInPriceGroup(IConnectionManager entry, RecordIdentifier storeId,
                                                             RecordIdentifier groupId)
        {
            var line = new StoreInPriceGroup();

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {

                cmd.CommandText =
                    "select lpg.STOREID, ISNULL(lpg.LEVEL_,0) as LEVEL , ISNULL(pg.NAME, '') as PRICEGROUPNAME, lpg.PRICEGROUPID " +
                    "from RBOLOCATIONPRICEGROUP lpg " +
                    "left outer join PRICEDISCGROUP pg on pg.DATAAREAID = lpg.DATAAREAID and pg.GROUPID = lpg.PRICEGROUPID " +
                    "where lpg.STOREID = @storeId and lpg.PRICEGROUPID = @groupId and lpg.DATAAREAID = @dataAreaId " +
                    "order by LEVEL";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeId", storeId);
                MakeParam(cmd, "groupId", groupId);

                SqlDataReader dr = null;
                try
                {
                    dr = (SqlDataReader)entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.HasRows)
                    {
                        dr.Read();

                        line.ID = (string)dr["STOREID"];
                        line.Level = (int)dr["LEVEL"];
                        line.PriceGroupID = (string)dr["PRICEGROUPID"];
                        line.PriceGroupName = (string)dr["PRICEGROUPNAME"];
                        line.Dirty = false;
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }

                return line;
            }
        }

        /// <summary>
        /// Gets a list of stores in a price group, ordered by store name.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupId">Id of the price group</param>
        /// <returns>A list of stores in a price group, ordered by store name</returns>
        public virtual List<StoreInPriceGroup> GetStoresInPriceGroup(IConnectionManager entry, RecordIdentifier groupId)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select lpg.STOREID, ISNULL(st.NAME,'') as STORENAME, ISNULL(lpg.LEVEL_,0) as LEVEL " +
                    "From RBOLOCATIONPRICEGROUP lpg " +
                    "left outer join RBOSTORETABLE st on st.DATAAREAID = lpg.DATAAREAID and st.STOREID = lpg.STOREID " +
                    "Where lpg.PRICEGROUPID = @groupId and lpg.DATAAREAID = @dataAreaId " +
                    "Order by STORENAME";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "groupId", (string)groupId);

                return Execute<StoreInPriceGroup>(entry, cmd, CommandType.Text, PopulateStoreInGroup);
            }
        }

        /// <summary>
        /// Gets a list of price group lines for a given store id.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeId">Id of the store</param>
        /// <param name="cacheType">Cache</param>
        /// <param name="usageIntent">Specifies how much extra data should be loaded</param>
        /// <returns>A list of price group lines, ordered by level</returns>
        public virtual List<StoreInPriceGroup> GetPriceGroupsForStore(IConnectionManager entry, RecordIdentifier storeId, CacheType cacheType = CacheType.CacheTypeNone, UsageIntentEnum usageIntent = UsageIntentEnum.Normal)
        {
            ValidateSecurity(entry);

            RecordIdentifier id = new RecordIdentifier("GetPriceGroupsForStore", new RecordIdentifier(storeId), (int)usageIntent);

            using (var cmd = entry.Connection.CreateCommand())
            {
                if (usageIntent == UsageIntentEnum.Minimal)
                {
                    cmd.CommandText = @"select lpg.STOREID,
                    ISNULL(lpg.LEVEL_,0) as LEVEL, 
                    lpg.PRICEGROUPID
                    from RBOLOCATIONPRICEGROUP lpg 
                    where lpg.STOREID = @storeId and lpg.DATAAREAID = @dataAreaId 
                    order by LEVEL";

                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                    MakeParam(cmd, "storeId", storeId);

                    return GetList<StoreInPriceGroup>(entry, cmd, id, PopulateMinimumGroupInStore, cacheType);
                }

                cmd.CommandText =
                    "select lpg.STOREID, ISNULL(lpg.LEVEL_,0) as LEVEL , ISNULL(pdg.NAME, '') as PRICEGROUPNAME, lpg.PRICEGROUPID, ISNULL(pdg.INCLTAX, 0) as INCLTAX " +
                    "from RBOLOCATIONPRICEGROUP lpg " +
                    "left outer join PRICEDISCGROUP pdg on pdg.DATAAREAID = lpg.DATAAREAID and pdg.GROUPID = lpg.PRICEGROUPID " +
                    "where lpg.STOREID = @storeId and lpg.DATAAREAID = @dataAreaId and pdg.Module = 1 and pdg.Type = 0 " +
                    "order by LEVEL";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeId", storeId);

                return GetList<StoreInPriceGroup>(entry, cmd, id, PopulateGroupInStore, cacheType);
            }
        }

        private static void PopulateCustomer(IDataReader dr, CustomerInGroup customer)
        {
            customer.ID = (string)dr["ACCOUNTNUM"];
            customer.Text = (string)dr["CUSTOMERNAME"];
            customer.GroupName = (string)dr["GROUPNAME"];
            customer.Name.First = (string)dr["FIRSTNAME"];
            customer.Name.Last = (string)dr["LASTNAME"];
            customer.Name.Middle = (string)dr["MIDDLENAME"];
            customer.Name.Prefix = (string)dr["NAMEPREFIX"];
            customer.Name.Suffix = (string)dr["NAMESUFFIX"];
        }

        /// <summary>
        /// Checks if a customer belongs in the specified price/discount group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="groupType">The group type to check for</param>
        /// <param name="groupID">The ID of the group</param>
        /// <param name="customerID">The ID of the customer</param>
        /// <returns></returns>
        public virtual bool CustomerExistsInGroup(IConnectionManager entry, PriceDiscGroupEnum groupType, RecordIdentifier groupID, RecordIdentifier customerID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"if exists
                      (
                           select * 
		                   from PRICEDISCGROUP p
		                   join CUSTOMER c on c.ACCOUNTNUM = @customerID  and c.LINEDISC = p.GROUPID and c.DATAAREAID = p.DATAAREAID
		                   where p.GROUPID = @groupID and p.MODULE = 1 and p.TYPE = @groupType and p.DATAAREAID = @dataAreaID
                       )
	                       select 1
                      else
	                       select 0";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "groupID", (string)groupID);
                MakeParam(cmd, "groupType", (int)groupType, SqlDbType.Int);
                MakeParam(cmd, "customerID", (string)customerID);

                return (int)entry.Connection.ExecuteScalar(cmd) == 1;
            }
        }

        public List<CustomerInGroup> GetCustomersInGroupList(IConnectionManager entry,
                                                            PriceDiscGroupEnum type,
                                                            string customerGroup,
                                                            int? recordFrom,
                                                            int? recordTo)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                string groupFieldInDb = "";

                switch (type)
                {
                    case PriceDiscGroupEnum.LineDiscountGroup:
                        groupFieldInDb = "LINEDISC";
                        break;
                    case PriceDiscGroupEnum.MultilineDiscountGroup:
                        groupFieldInDb = "MULTILINEDISC";
                        break;
                    case PriceDiscGroupEnum.PriceGroup:
                        groupFieldInDb = "PRICEGROUP";
                        break;
                    case PriceDiscGroupEnum.TotalDiscountGroup:
                        groupFieldInDb = "ENDDISC";
                        break;
                }

                cmd.CommandText =
                    "Select * " +
                    "From (Select ACCOUNTNUM, ISNULL(cust.NAME,'') AS CUSTOMERNAME, ISNULL(pdg.NAME,'') as GROUPNAME, ROW_NUMBER() OVER(order by cust.Name) as ROWNR,  " +
                    "ISNULL(cust.FIRSTNAME,'') as FIRSTNAME,ISNULL(cust.MIDDLENAME,'') as MIDDLENAME,ISNULL(cust.LASTNAME,'') as LASTNAME, " +
                    "ISNULL(cust.NAMEPREFIX,'') as NAMEPREFIX,ISNULL(cust.NAMESUFFIX,'') as NAMESUFFIX " +
                    "From CUSTOMER cust Join PRICEDISCGROUP pdg on pdg.GROUPID = cust." + groupFieldInDb +
                    " and pdg.DATAAREAID = cust.DATAAREAID " +
                    "Where pdg.DATAAREAID = @dataAreaId and pdg.TYPE = @type and pdg.MODULE = 1 and pdg.GROUPID = @customerGroup) ct ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "type", (int)type, SqlDbType.Int);
                MakeParam(cmd, "customerGroup", customerGroup);

                if (recordFrom.HasValue && recordTo.HasValue)
                {
                    cmd.CommandText += "Where ct.ROWNR between @recordFrom and @recordTo ";
                    MakeParam(cmd, "recordFrom", recordFrom.Value, SqlDbType.Int);
                    MakeParam(cmd, "recordTo", recordTo.Value, SqlDbType.Int);
                }

                return Execute<CustomerInGroup>(entry, cmd, CommandType.Text, PopulateCustomer);
            }
        }

        public List<CustomerInGroup> SearchCustomersNotInGroup(
            IConnectionManager entry,
            string searchText,
            int recordFrom,
            int recordTo,
            int type,
            string groupId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                string groupFieldInDb = "";

                switch (type)
                {
                    case 0:
                        groupFieldInDb = "PRICEGROUP";
                        break;
                    case 1:
                        groupFieldInDb = "LINEDISC";
                        break;
                    case 2:
                        groupFieldInDb = "MULTILINEDISC";
                        break;
                    case 3:
                        groupFieldInDb = "ENDDISC";
                        break;
                }

                cmd.CommandText =
                    "Select * From " +
                    "( " +
                    "Select ISNULL(ct.NAME,'') as CUSTOMERNAME, ct.ACCOUNTNUM, ISNULL(pdg.NAME,'') as GROUPNAME, ROW_NUMBER() OVER(order by ct.Name) as ROWNR, " +
                    "ISNULL(ct.FIRSTNAME,'') as FIRSTNAME,ISNULL(ct.MIDDLENAME,'') as MIDDLENAME,ISNULL(ct.LASTNAME,'') as LASTNAME, " +
                    "ISNULL(ct.NAMEPREFIX,'') as NAMEPREFIX,ISNULL(ct.NAMESUFFIX,'') as NAMESUFFIX " +
                    "From CUSTOMER ct " +
                    "left outer join PRICEDISCGROUP pdg on pdg.GROUPID = ct." + groupFieldInDb +
                    " and pdg.DATAAREAID = ct.DATAAREAID  " +
                    "Where " +
                    "(pdg.TYPE = @type OR pdg.TYPE is null) and " +
                    "(pdg.MODULE = 1 OR pdg.MODULE is null) and " +
                    "ct.DATAAREAID = @dataAreaID and " +
                    "ct.DELETED = 0 and " +
                    " ct." + groupFieldInDb + " not like @groupId and " +
                    "(ct.ACCOUNTNUM like @SearchString or ct.NAME like @SearchString) " +
                    ") t " +
                    "Where  t.ROWNR between @recordFrom and @recordTo";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "type", type, SqlDbType.Int);
                MakeParam(cmd, "groupId", groupId);
                MakeParam(cmd, "SearchString", "%" + PreProcessSearchText(searchText, false, false) + "%");
                MakeParam(cmd, "recordFrom", recordFrom, SqlDbType.Int);
                MakeParam(cmd, "recordTo", recordTo, SqlDbType.Int);

                return Execute<CustomerInGroup>(entry, cmd, CommandType.Text, PopulateCustomer);
            }
        }

        public virtual bool CustomerExists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "CUSTOMER", "ACCOUNTNUM", id);
        }

        public virtual void RemoveCustomerFromGroup(IConnectionManager entry, RecordIdentifier customerAccountNumber, PriceDiscGroupEnum type)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.CustomerEdit);

            var statement = new SqlServerStatement("CUSTOMER");

            string groupFieldinDb = "";

            switch (type)
            {
                case PriceDiscGroupEnum.LineDiscountGroup:
                    groupFieldinDb = "LINEDISC";
                    break;
                case PriceDiscGroupEnum.MultilineDiscountGroup:
                    groupFieldinDb = "MULTILINEDISC";
                    break;
                case PriceDiscGroupEnum.PriceGroup:
                    groupFieldinDb = "PRICEGROUP";
                    break;
                case PriceDiscGroupEnum.TotalDiscountGroup:
                    groupFieldinDb = "ENDDISC";
                    break;
            }

            if (CustomerExists(entry, customerAccountNumber))
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ACCOUNTNUM", customerAccountNumber.ToString());
                statement.AddField(groupFieldinDb, "");

                entry.Connection.ExecuteStatement(statement);
            }
        }

        public void RemoveCustomersFromGroup(
            IConnectionManager entry,
            RecordIdentifier groupId,
            PriceDiscGroupEnum type)
        {
            var allCustomersInGroup = GetCustomersInGroupList(entry, type, (string)groupId, null, null);
            foreach (var customerInGroup in allCustomersInGroup)
            {
                RemoveCustomerFromGroup(entry, customerInGroup.ID, type);
            }
        }

        public virtual void AddCustomerToGroup(IConnectionManager entry, RecordIdentifier customerAccountNumber, PriceDiscGroupEnum type, string groupId)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.CustomerEdit);

            var statement = new SqlServerStatement("CUSTOMER");

            string groupFieldinDb = "";

            switch (type)
            {
                case PriceDiscGroupEnum.LineDiscountGroup:
                    groupFieldinDb = "LINEDISC";
                    break;
                case PriceDiscGroupEnum.MultilineDiscountGroup:
                    groupFieldinDb = "MULTILINEDISC";
                    break;
                case PriceDiscGroupEnum.PriceGroup:
                    groupFieldinDb = "PRICEGROUP";
                    break;
                case PriceDiscGroupEnum.TotalDiscountGroup:
                    groupFieldinDb = "ENDDISC";
                    break;
            }

            if (CustomerExists(entry, customerAccountNumber))
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ACCOUNTNUM", customerAccountNumber.ToString());
                statement.AddField(groupFieldinDb, groupId);

                entry.Connection.ExecuteStatement(statement);
            }
        }

        public virtual bool Exists(IConnectionManager entry, PriceDiscountModuleEnum module, PriceDiscGroupEnum type, RecordIdentifier groupID)
        {
            return RecordExists(entry, "PRICEDISCGROUP", new[] { "MODULE", "TYPE", "GROUPID" }, new RecordIdentifier((int)module, new RecordIdentifier((int)type, groupID)));
        }

        public virtual void Delete(IConnectionManager entry, PriceDiscountGroup group)
        {
            EditAllowed(entry, group);
            //Price group ids are saved either in ITEMRELATION or in ACCOUNTRELATION fields in PRICEDISCTABLE table depending on the discount type  
            DeleteRecord(entry, "PRICEDISCTABLE", new[] { "ITEMRELATION" }, group.ID.SecondaryID.SecondaryID, "");
            DeleteRecord(entry, "PRICEDISCTABLE", new[] { "ACCOUNTRELATION" }, group.ID.SecondaryID.SecondaryID, "");
            DeleteRecord(entry, "PRICEDISCGROUP", new[] { "MODULE", "TYPE", "GROUPID" }, group.ID, "");
        }

        public virtual RecordIdentifier GetIDFromGroupID(IConnectionManager entry, RecordIdentifier groupID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"select MODULE, TYPE, GROUPID from PRICEDISCGROUP where GROUPID = @groupID";

                MakeParam(cmd, "groupID", (string)groupID);

                RecordIdentifier IDFromGroupID = RecordIdentifier.Empty;

                using (IDataReader reader = entry.Connection.ExecuteReader(cmd, CommandType.Text))
                {
                    if (reader.Read())
                    {
                        IDFromGroupID = new RecordIdentifier((int)reader["MODULE"], (int)reader["TYPE"], (string)reader["GROUPID"]);
                    }
                }

                return IDFromGroupID;
            }
        }

        private void EditAllowed(IConnectionManager entry, PriceDiscountGroup group)
        {
            if (group.Module == PriceDiscountModuleEnum.Customer &&
                (group.Type == PriceDiscGroupEnum.LineDiscountGroup ||
                 group.Type == PriceDiscGroupEnum.MultilineDiscountGroup ||
                 group.Type == PriceDiscGroupEnum.TotalDiscountGroup))
            {
                ValidateSecurity(entry, Permission.EditCustomerDiscGroups);
            }

            if (group.Module == PriceDiscountModuleEnum.Item &&
                (group.Type == PriceDiscGroupEnum.LineDiscountGroup ||
                 group.Type == PriceDiscGroupEnum.MultilineDiscountGroup ||
                 group.Type == PriceDiscGroupEnum.TotalDiscountGroup))
            {
                ValidateSecurity(entry, Permission.EditItemDiscGroups);
            }

            if (group.Module == PriceDiscountModuleEnum.Customer && group.Type == PriceDiscGroupEnum.PriceGroup)
            {
                ValidateSecurity(entry, Permission.EditCustomerDiscGroups);
            }

            if (group.Module == PriceDiscountModuleEnum.Item && group.Type == PriceDiscGroupEnum.PriceGroup)
            {
                ValidateSecurity(entry, Permission.EditItemDiscGroups);
            }
        }

        public virtual void Save(IConnectionManager entry, PriceDiscountGroup group)
        {
            var statement = new SqlServerStatement("PRICEDISCGROUP");

            EditAllowed(entry, group);

            statement.UpdateColumnOptimizer = group;

            bool isNew = false;
            if (group.GroupID == RecordIdentifier.Empty)
            {
                isNew = true;
                SetSequence(group.Module, group.Type);
                group.GroupID = DataProviderFactory.Instance.GenerateNumber(entry, this);
            }

            if (isNew || !Exists(entry, group.Module, group.Type, group.GroupID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("MODULE", (int)group.Module, SqlDbType.Int);
                statement.AddKey("TYPE", (int)group.Type, SqlDbType.Int);
                statement.AddKey("GROUPID", (string)group.GroupID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("MODULE", (int)group.Module, SqlDbType.Int);
                statement.AddCondition("TYPE", (int)group.Type, SqlDbType.Int);
                statement.AddCondition("GROUPID", (string)group.GroupID);
            }

            statement.AddField("NAME", group.Text);
            statement.AddField("INCLTAX", group.IncludeTax ? 1 : 0, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Gets all the items from the database matching the list of <see cref="itemsToCompare" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemsToCompare">List of items you want to get from the database matching items <see cref="itemsToCompare"</param>
        /// <returns>List of items</returns>
        public virtual List<PriceDiscountGroup> GetCompareList(IConnectionManager entry, List<PriceDiscountGroup> itemsToCompare)
        {
            var selectionColumns = new List<TableColumn>
            {
                new TableColumn {ColumnName = "MODULE", TableAlias = "P"},
                new TableColumn {ColumnName = "TYPE", TableAlias = "P"},
                new TableColumn {ColumnName = "GROUPID", TableAlias = "P"},
                new TableColumn {ColumnName = "ISNULL(P.NAME,'')", TableAlias = "", ColumnAlias = "NAME"},
                new TableColumn {ColumnName = "ISNULL(P.INCLTAX,0)", TableAlias = "", ColumnAlias = "INCLTAX"},
            };

            return GetCompareListInBatches(entry, itemsToCompare, "PRICEDISCGROUP", new string[] { "MODULE", "TYPE", "GROUPID" }, selectionColumns, null, PopulatePriceDiscountGroup);
        }

        #region ISequenceable Members

        /// <summary>
        /// Checks if a sequence with a given ID exists for Price groups
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">ID to check for</param>
        /// <returns>True if it exists, else false</returns>
        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, module, type, id);
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "PRICEDISCGROUP", "GROUPID", sequenceFormat, startingRecord, numberOfRecords);
        }

        public virtual RecordIdentifier SequenceID
        {
            get
            {
                switch (type)
                {
                    case PriceDiscGroupEnum.PriceGroup:
                        return "PriceGroup";
                    case PriceDiscGroupEnum.LineDiscountGroup:
                        return "LineDiscountG";
                    case PriceDiscGroupEnum.MultilineDiscountGroup:
                        return "MultiLineDiscountG";
                    case PriceDiscGroupEnum.TotalDiscountGroup:
                        return "TotalDiscountG";
                    default:
                        return "";
                }
            }
        }

        #endregion
    }
}
