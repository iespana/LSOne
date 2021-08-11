using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.ItemMaster
{
    /// <summary>
    /// Data provider class for retail groups
    /// </summary>
    public class RetailGroupDataOLD : SqlServerDataProviderBase
    {
        private static string BaseSql
        {
            get
            {
                return "Select " +
                       "a.GROUPID as GROUPID, " +
                       "ISNULL(a.NAME,'') AS NAME, " +
                       "ISNULL(a.DEFAULTPROFIT,0) AS DEFAULTPROFIT, " +
                " ISNULL(a.POSPERIODICID, '') AS POSPERIODICID,  " +
                "    ISNULL(iid.DEPARTMENTID,'') AS RetailDepartmentId, " +
                       "ISNULL(iid.NAME,'') AS RetailDepartmentName, " +
                "    ISNULL(sig.SIZEGROUP,'') AS SizeGroupId, ISNULL(sig.DESCRIPTION,'') AS SizeGroupName, " +
                "    ISNULL(cg.COLORGROUP,'') AS ColorGroupId, ISNULL(cg.DESCRIPTION,'') AS ColorGroupName, " +
                "    ISNULL(stg.STYLEGROUP,'') AS StyleGroupID, ISNULL(stg.DESCRIPTION,'') AS StyleGroupName, " +
                "    ISNULL(idg.DIMGROUPID,'') AS DimensionGroupId, ISNULL(idg.NAME,'') AS DimensionGroupName, " +
                "    ISNULL(tigh.TAXITEMGROUP,'') AS TaxItemGroupId, ISNULL(tigh.NAME,'') AS TaxItemGroupName, " +
                "    ISNULL(PDV.ID, '') AS POSPERIODICID, ISNULL(PDV.DESCRIPTION, '') AS VALIDATIONPERIODDISCOUNTDESCRIPTION " + 



                "From RBOINVENTITEMRETAILGROUP a " +
                
                "    left outer join RBOINVENTITEMDEPARTMENT iid on a.DATAAREAID = iid.DATAAREAID AND a.DEPARTMENTID = iid.DEPARTMENTID " +
                "    left outer join INVENTITEMGROUP iig on a.DATAAREAID = iig.DATAAREAID AND a.ITEMGROUPID = iig.ITEMGROUPID " +
                "    left outer join RBOSIZEGROUPTABLE sig on a.DATAAREAID = sig.DATAAREAID AND a.SIZEGROUPID = sig.SIZEGROUP " +
                "    left outer join RBOCOLORGROUPTABLE cg on a.DATAAREAID = cg.DATAAREAID AND a.COLORGROUPID = cg.COLORGROUP " +
                "    left outer join RBOSTYLEGROUPTABLE  stg on a.DATAAREAID = stg.DATAAREAID AND a.STYLEGROUPID = stg.STYLEGROUP " +
                "    left outer join INVENTDIMGROUP idg on a.DATAAREAID = idg.DATAAREAID AND a.INVENTDIMGROUPID = idg.DIMGROUPID " +
                "    left outer join TAXITEMGROUPHEADING tigh on a.DATAAREAID = tigh.DATAAREAID AND a.SALESTAXITEMGROUP = tigh.TAXITEMGROUP " +
                "    left outer join POSDISCVALIDATIONPERIOD PDV on a.DATAAREAID = PDV.DATAAREAID and a.POSPERIODICID = PDV.ID ";


            }
        }

        private static string GetSortColumn(RetailGroupSorting sortEnum)
        {
            var sortColumn = "";

            switch (sortEnum)
            {
                case RetailGroupSorting.RetailGroupId:
                    sortColumn = "GROUPID";
                    break;
                case RetailGroupSorting.RetailGroupName:
                    sortColumn = "NAME";
                    break;
                case RetailGroupSorting.RetailDepartmentName:
                    sortColumn = "RetailDepartmentName";
                    break;
            }          

            return sortColumn;
        }

        private static string ResolveSort(RetailGroupSorting sortEnum, bool sortBackwards)
        {
            var sortString = " Order By " + GetSortColumn(sortEnum) + " ASC";

            if (sortBackwards)
            {
                sortString = sortString.Replace("ASC", "DESC");
            }

            return sortString;
        }

        private static void PopulateDetailedList(IDataReader dr, RetailGroup group)
        {
            group.ID = (string)dr["GROUPID"];
            group.Text = (string)dr["NAME"];
            group.RetailDepartmentID = (string)dr["DEPARTMENTID"];
            group.RetailDepartmentName = (string)dr["RETAILDEPARTMENTNAME"];
        }

        private static void PopulateRetailGroup(IDataReader dr, RetailGroup group)
        {
            group.ID = (string)dr["GROUPID"];
            group.Text = (string)dr["NAME"];
            group.RetailDepartmentID = (string)dr["RetailDepartmentId"];
            group.RetailDepartmentName = (string)dr["RetailDepartmentName"];
            //group.SizeGroupId = (string)dr["SizeGroupId"];
            //group.SizeGroupName = (string)dr["SizeGroupName"];
            //group.StyleGroupId = (string)dr["StyleGroupId"];
            //group.StyleGroupName = (string)dr["StyleGroupName"];
            //group.ColorGroupId = (string)dr["ColorGroupId"];
            //group.ColorGroupName = (string)dr["ColorGroupName"];
            ////group.DimensionGroupId = (string)dr["DimensionGroupId"];
            //group.DimensionGroupName = (string)dr["DimensionGroupName"];
            group.ItemSalesTaxGroupId = (string)dr["TaxItemGroupId"];
            group.ItemSalesTaxGroupName = (string)dr["TaxItemGroupName"];
            group.ProfitMargin = (dr["DEFAULTPROFIT"] != DBNull.Value) ? Convert.ToDecimal(dr["DEFAULTPROFIT"]) : 0;
            group.ValidationPeriod = (string)dr["POSPERIODICID"];
            group.ValidationPeriodDescription = (string)dr["VALIDATIONPERIODDISCOUNTDESCRIPTION"];
        }

        private static void PopulateRetailGroupList(IDataReader dr, RetailGroup group)
        {
            group.ID = (string)dr["GROUPID"];
            group.Text = (string)dr["NAME"];
            group.RetailDepartmentName = (string)dr["RetailDepartmentName"];
        }

        private static void PopulateItemInGroup(IDataReader dr, ItemInGroup item)
        {
            item.ID = (string)dr["ITEMID"];
            item.Text = (string)dr["ITEMNAME"];
            item.Group = (string)dr["GROUPNAME"];
        }

        /// <summary>
        /// Gets a list of data entities containing ID and name for each retail group, ordered by a chosen field
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortEnum">A sort enum that defines how the result should be sorted </param>
        /// <returns>A list of all retail groups, ordered by a chosen field</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry, RetailGroupSorting sortEnum)
        {
            if (sortEnum != RetailGroupSorting.RetailGroupId && sortEnum != RetailGroupSorting.RetailGroupName)
            {
                throw new NotSupportedException();
            }

            return GetList<DataEntity>(entry, "RBOINVENTITEMRETAILGROUP", "NAME", "GROUPID", GetSortColumn(sortEnum));
        }

        /// <summary>
        /// Gets a list of data entities containing IDs and names for all retail group, ordered by retail group name
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of data entities contaning IDs and names of retail groups, ordered by retail group name</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "RBOINVENTITEMRETAILGROUP", "NAME", "GROUPID", "NAME");
        }

        /// <summary>
        /// Gets a list of retail groups, ordered by a given sort column index and a reversed ordering based on a parameter
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortEnum">A sort enum that defines how the result should be sorted</param>
        /// <param name="backwardsSort">Whether to reverse the result set or not</param>
        /// <returns>A list of retail groups meeting the above criteria</returns>
        public virtual List<RetailGroup> GetDetailedList(IConnectionManager entry, RetailGroupSorting sortEnum, bool backwardsSort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"Select a.GROUPID as GROUPID, 
                                           ISNULL(a.NAME,'') as NAME, 
                                           ISNULL(a.NAMEALIAS,'') as SEARCHNAME, 
                                           ISNULL(a.DEPARTMENTID,'') as DEPARTMENTID,  
                                           ISNULL(iid.NAME,'') as RETAILDEPARTMENTNAME              
                                    From RBOINVENTITEMRETAILGROUP a 
                                    left outer join RBOINVENTITEMDEPARTMENT iid on iid.DATAAREAID = a.DATAAREAID and iid.DEPARTMENTID = a.DEPARTMENTID 
                                    Where a.DATAAREAID = @dataAreaId";                

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<RetailGroup>(entry, cmd, CommandType.Text, PopulateDetailedList);
            }
        }

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="RetailGroup" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="retailDepartmentID">The unique ID of the department to search
        /// for</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        /// <param name="sort">A string defining the sort column</param>
        public List<RetailGroup> Search(IConnectionManager entry, string searchString,
                                               RecordIdentifier retailDepartmentID, int rowFrom, int rowTo,
                                               bool beginsWith, RetailGroupSorting sort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                var modifiedSearchString = (beginsWith ? "" : "%") + searchString + "%";

                cmd.CommandText =
                    " Select s.* from ( " +
                    "Select a.GROUPID as GROUPID, " +
                    "ISNULL(a.NAME,'') as NAME, " +
                    "ISNULL(iid.NAME,'') as RetailDepartmentName, " +
                    "ROW_NUMBER() OVER(order by a." + GetSortColumn(sort) + ") AS ROW " +
                    "From RBOINVENTITEMRETAILGROUP a " +
                    "left outer join INVENTITEMGROUP iig on iig.DATAAREAID = a.DATAAREAID and iig.ITEMGROUPID = a.ITEMGROUPID " +
                    "left outer join RBOINVENTITEMDEPARTMENT iid on iid.DATAAREAID = a.DATAAREAID and iid.DEPARTMENTID = a.DEPARTMENTID " +
                    "WHERE a.DATAAREAID = @DATAAREAID ";

                if ((string) retailDepartmentID != "")
                {
                    cmd.CommandText += "AND iid.DEPARTMENTID = @RETAILDEPARTMENTID ";
                    MakeParam(cmd, "RETAILDEPARTMENTID", (string) retailDepartmentID);
                }

                cmd.CommandText +=
                    "AND (a.NAME Like @SEARCHSTRING or a.GROUPID Like @SEARCHSTRING)) s " +
                    "WHERE s.ROW between " + rowFrom + " and " + rowTo;

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "SEARCHSTRING", modifiedSearchString);

                return Execute<RetailGroup>(entry, cmd, CommandType.Text, PopulateRetailGroupList);
            }
        }

        public List<RetailGroup> AdvancedSearch(IConnectionManager entry,
                                                        int rowFrom, int rowTo, string sort,
                                                        out int totalRecordsMatching,
                                                        string idOrDescription = null,
                                                        bool idOrDescriptionBeginsWith = true,
                                                        RecordIdentifier retailDepartmentID = null,
                                                        RecordIdentifier taxGroupID = null,
                                                        RecordIdentifier variantGroupID = null,
                                                        RecordIdentifier sizeGroupID = null,
                                                        RecordIdentifier colorGroupID = null,
                                                        RecordIdentifier styleGroupID = null,
                                                        string validationPeriod = null)
        {
            string whereConditions = "";
            using (var cmd = entry.Connection.CreateCommand())
            using (var cmdCount = entry.Connection.CreateCommand())
            {

                if (idOrDescription != null && idOrDescription.Trim().Length > 0)
                {
                    idOrDescription = (idOrDescriptionBeginsWith ? "" : "%") + idOrDescription + "%";

                    whereConditions +=
                        " and (a.NAME Like @searchString or a.GROUPID Like @searchString or a.NAMEALIAS Like @searchString) ";

                    MakeParamNoCheck(cmd, "searchString", idOrDescription);
                    MakeParamNoCheck(cmdCount, "searchString", idOrDescription);
                }

                if (retailDepartmentID != null)
                {
                    whereConditions +=
                        " AND iid.DEPARTMENTID = @departmentID ";

                    MakeParamNoCheck(cmd, "departmentID", (string)retailDepartmentID);
                    MakeParamNoCheck(cmdCount, "departmentID", (string)retailDepartmentID);
                }

                if (taxGroupID != null)
                {
                    whereConditions +=
                        " AND tigh.TAXITEMGROUP = @taxGroupID ";

                    MakeParamNoCheck(cmd, "taxGroupID", (string)taxGroupID);
                    MakeParamNoCheck(cmdCount, "taxGroupID", (string)taxGroupID);
                }

                if (variantGroupID != null)
                {
                    whereConditions += " and idg.DIMGROUPID = @variantGroupID ";

                    MakeParamNoCheck(cmd, "variantGroupID", (string)variantGroupID);
                    MakeParamNoCheck(cmdCount, "variantGroupID", (string)variantGroupID);
                }

                if (sizeGroupID != null)
                {
                    whereConditions += " and sig.SIZEGROUP = @sizeGroupID ";

                    MakeParamNoCheck(cmd, "sizeGroupID", (string)sizeGroupID);
                    MakeParamNoCheck(cmdCount, "sizeGroupID", (string)sizeGroupID);
                }

                if (colorGroupID != null)
                {
                    whereConditions += " and cg.COLORGROUP = @colorGroupID ";

                    MakeParamNoCheck(cmd, "colorGroupID", (string)colorGroupID);
                    MakeParamNoCheck(cmdCount, "colorGroupID", (string)colorGroupID);
                }

                if (styleGroupID != null)
                {
                    whereConditions += " and stg.STYLEGROUP = @styleGroupID ";

                    MakeParamNoCheck(cmd, "styleGroupID", (string)styleGroupID);
                    MakeParamNoCheck(cmdCount, "styleGroupID", (string)styleGroupID);
                }

                if (validationPeriod != null && validationPeriod.Trim().Length > 0)
                {
                    whereConditions += " and PDV.ID = @validationPeriod ";

                    MakeParamNoCheck(cmd, "validationPeriod", validationPeriod);
                    MakeParamNoCheck(cmdCount, "validationPeriod", validationPeriod);
                }

                cmd.CommandText = @"select ss.* from(
                    Select s.*, ROW_NUMBER() OVER(order by <sort>) AS ROW from (

                    Select a.GROUPID as GROUPID, 
                    ISNULL(a.NAME,'') AS NAME, 
                    ISNULL(a.DEFAULTPROFIT,0) AS DEFAULTPROFIT, 
                    ISNULL(iid.DEPARTMENTID,'') AS RetailDepartmentId, 
                    ISNULL(iid.NAME,'') AS RetailDepartmentName, 
                    ISNULL(sig.SIZEGROUP,'') AS SizeGroupId, 
                    ISNULL(sig.DESCRIPTION,'') AS SizeGroupName, 
                    ISNULL(cg.COLORGROUP,'') AS ColorGroupId, 
                    ISNULL(cg.DESCRIPTION,'') AS ColorGroupName, 
                    ISNULL(stg.STYLEGROUP,'') AS StyleGroupID, 
                    ISNULL(stg.DESCRIPTION,'') AS StyleGroupName, 
                    ISNULL(idg.DIMGROUPID,'') AS DimensionGroupId, 
                    ISNULL(idg.NAME,'') AS DimensionGroupName, 
                    ISNULL(tigh.TAXITEMGROUP,'') AS TaxItemGroupId, 
                    ISNULL(tigh.NAME,'') AS TaxItemGroupName, 
                    ISNULL(PDV.ID, '') AS POSPERIODICID, 
                    ISNULL(PDV.DESCRIPTION, '') AS VALIDATIONPERIODDISCOUNTDESCRIPTION 

                    From RBOINVENTITEMRETAILGROUP a 

                    left outer join RBOINVENTITEMDEPARTMENT iid on a.DATAAREAID = iid.DATAAREAID AND a.DEPARTMENTID = iid.DEPARTMENTID 
                    left outer join INVENTITEMGROUP iig on a.DATAAREAID = iig.DATAAREAID AND a.ITEMGROUPID = iig.ITEMGROUPID 
                    left outer join RBOSIZEGROUPTABLE sig on a.DATAAREAID = sig.DATAAREAID AND a.SIZEGROUPID = sig.SIZEGROUP 
                    left outer join RBOCOLORGROUPTABLE cg on a.DATAAREAID = cg.DATAAREAID AND a.COLORGROUPID = cg.COLORGROUP 
                    left outer join RBOSTYLEGROUPTABLE  stg on a.DATAAREAID = stg.DATAAREAID AND a.STYLEGROUPID = stg.STYLEGROUP 
                    left outer join INVENTDIMGROUP idg on a.DATAAREAID = idg.DATAAREAID AND a.INVENTDIMGROUPID = idg.DIMGROUPID 
                    left outer join TAXITEMGROUPHEADING tigh on a.DATAAREAID = tigh.DATAAREAID AND a.SALESTAXITEMGROUP = tigh.TAXITEMGROUP 
                    left outer join POSDISCVALIDATIONPERIOD PDV on a.DATAAREAID = PDV.DATAAREAID and a.POSPERIODICID = PDV.ID

                    WHERE a.DATAAREAID = @dataAreaId <whereConditions>
                    ) s
                    ) ss
                    WHERE ss.ROW between @rowFrom and @rowTo";


                cmd.CommandText = cmd.CommandText.Replace("<sort>", sort);
                cmd.CommandText = cmd.CommandText.Replace("<whereConditions>", whereConditions);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
                MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);

                // Do a count first
                cmdCount.CommandText = @"select count(*) from RBOINVENTITEMRETAILGROUP a 
                                        left outer join RBOINVENTITEMDEPARTMENT iid on a.DATAAREAID = iid.DATAAREAID AND a.DEPARTMENTID = iid.DEPARTMENTID 
                                        left outer join INVENTITEMGROUP iig on a.DATAAREAID = iig.DATAAREAID AND a.ITEMGROUPID = iig.ITEMGROUPID 
                                        left outer join RBOSIZEGROUPTABLE sig on a.DATAAREAID = sig.DATAAREAID AND a.SIZEGROUPID = sig.SIZEGROUP 
                                        left outer join RBOCOLORGROUPTABLE cg on a.DATAAREAID = cg.DATAAREAID AND a.COLORGROUPID = cg.COLORGROUP 
                                        left outer join RBOSTYLEGROUPTABLE  stg on a.DATAAREAID = stg.DATAAREAID AND a.STYLEGROUPID = stg.STYLEGROUP 
                                        left outer join INVENTDIMGROUP idg on a.DATAAREAID = idg.DATAAREAID AND a.INVENTDIMGROUPID = idg.DIMGROUPID 
                                        left outer join TAXITEMGROUPHEADING tigh on a.DATAAREAID = tigh.DATAAREAID AND a.SALESTAXITEMGROUP = tigh.TAXITEMGROUP 
                                        left outer join POSDISCVALIDATIONPERIOD PDV on a.DATAAREAID = PDV.DATAAREAID and a.POSPERIODICID = PDV.ID
                                        where a.DATAAREAID = @dataAreaId " +
                                        whereConditions;
                MakeParam(cmdCount, "dataAreaId", entry.Connection.DataAreaId);
                totalRecordsMatching = (int)entry.Connection.ExecuteScalar(cmdCount);

                return Execute<RetailGroup>(entry, cmd, CommandType.Text, PopulateRetailGroup);

            }
        }

        /// <summary>
        /// Gets a list of data entities containing ID and name of a retail items in a given retail group. The list is
        /// ordered by retail item names and items numbered between recordFrom and recordTo will be returned.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupId">Id of the retail group</param>
        /// <param name="recordFrom">The result number of the first item to be retrieved</param>
        /// <param name="recordTo">The result number of the last item to be retrieved</param>
        /// <returns>A list of data entities containing IDs and names of retail items in a given retail group meeting the above criteria</returns>
        public List<DataEntity> ItemsInGroup(IConnectionManager entry, RecordIdentifier groupId, int recordFrom,
                                                    int recordTo)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select item.ITEMID, item.ItemName " +
                    "From " +
                    "(Select it.ITEMID, ISNULL(it.ITEMNAME,'') as ItemName, ROW_NUMBER() OVER(order by ITEMNAME) as ROW " +
                    "From RBOINVENTTABLE rit  " +
                    "join INVENTTABLE it on rit.DATAAREAID = it.DATAAREAID and rit.ITEMID = it.ITEMID  " +
                    "Where rit.ITEMGROUP = @GROUPID and it.DATAAREAID = @DATAAREAID) item " +
                    "Where item.ROW between @RECORDFROM and @RECORDTO";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "GROUPID", (string) groupId);
                MakeParam(cmd, "RECORDFROM", recordFrom, SqlDbType.Int);
                MakeParam(cmd, "RECORDTO", recordTo, SqlDbType.Int);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMNAME", "ITEMID");
            }
        }

        /// <summary>
        /// Gets a list of data entities containing ID and name of a retail items in a given retail group. The list is
        /// ordered by retail item names and items numbered between recordFrom and recordTo will be returned.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupId">Id of the retail group</param>
        /// <returns>A list of data entities containing IDs and names of retail items in a given retail group meeting the above criteria</returns>
        public virtual List<RetailItem> ItemsInGroup(IConnectionManager entry, RecordIdentifier groupId)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select item.ITEMID, item.ItemName " +
                    "From " +
                    "(Select it.ITEMID, ISNULL(it.ITEMNAME,'') as ItemName, ROW_NUMBER() OVER(order by ITEMNAME) as ROW " +
                    "From RBOINVENTTABLE rit " +
                    "join INVENTTABLE it on rit.DATAAREAID = it.DATAAREAID and rit.ITEMID = it.ITEMID  " +
                    "Where rit.ITEMGROUP = @GROUPID and it.DATAAREAID = @DATAAREAID) item";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "GROUPID", (string) groupId);

                return Execute<RetailItem>(entry, cmd, CommandType.Text, "ITEMNAME", "ITEMID");
            }
        }

        /// <summary>
        /// Get a list of retail items not in a selected retail group, that contain a given search text.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchText">The text to search for. Searches in item name, the ID field and the name alias field</param>
        /// <param name="numberOfRecords">The number of items to return. Items are ordered by retail item name</param>
        /// <param name="excludedGroupId">ID of the retail group which the items are not supposed to be in</param>
        /// <returns>A list of items meeting the above criteria</returns>
        public List<ItemInGroup> ItemsNotInRetailGroup(
            IConnectionManager entry,
            string searchText,
            int numberOfRecords,
            RecordIdentifier excludedGroupId)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select TOP " + numberOfRecords +
                    " a.ITEMID, ISNULL(a.ITEMNAME,'') AS ITEMNAME, ISNULL(retailGroup.NAME,'') AS GROUPNAME " +
                    "From INVENTTABLE a " +
                    "Join RBOINVENTTABLE ra on a.DATAAREAID = ra.DATAAREAID AND a.ITEMID = ra.ITEMID " +
                    "Left outer join RBOINVENTITEMRETAILGROUP retailGroup on retailGroup.DATAAREAID = ra.DATAAREAID and ra.ITEMGROUP = retailGroup.GROUPID " +
                    "Where ra.ITEMGROUP <> @excludedGroupId and (a.ITEMID like @searchString or a.ITEMNAME like @searchString or a.NAMEALIAS like @searchString) " +
                    "and a.DATAAREAID = @DATAAREAID " +
                    "Order By a.ITEMNAME";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "excludedGroupId", (string) excludedGroupId);
                MakeParam(cmd, "searchString", searchText + "%");

                return Execute<ItemInGroup>(entry, cmd, CommandType.Text, PopulateItemInGroup);
            }
        }

        /// <summary>
        /// Removes a retail item with a given Id from the retail group it is currently in.
        /// </summary>
        /// <remarks>Requires the 'Edit retail items' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemId">The ID of the item to be removed</param>
        public virtual void RemoveItemFromRetailGroup(IConnectionManager entry, RecordIdentifier itemId)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.ItemsEdit);

            var statement = new SqlServerStatement("RBOINVENTTABLE") {StatementType = StatementType.Update};

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("ITEMID", itemId.ToString());
            statement.AddField("ITEMGROUP", "");
            statement.AddField("POSPERIODICID", "");
            //statement.AddField("VALIDATIONPERIODDISCOUNTDESCRIPTION", "");

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Adds a retail item with a given Id to a retail group with a given id.
        /// </summary>
        /// <remarks>Requires the 'Edit retail items' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemId">The ID of the item to add</param>
        /// <param name="groupId">The ID of the retail group to add an item to</param>
        public virtual void AddItemToRetailGroup(IConnectionManager entry, RecordIdentifier itemId, string groupId)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.ItemsEdit);

            var statement = new SqlServerStatement("RBOINVENTTABLE");
            var group = Get(entry, groupId);

            statement.StatementType = StatementType.Update;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("ITEMID", itemId.ToString());
            statement.AddField("ITEMGROUP", groupId);
            statement.AddField("POSPERIODICID", group.ValidationPeriod);
            //statement.AddField("VALIDATIONPERIODDISCOUNTDESCRIPTION", group.ValidationPeriodDescription);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Gets a retail group with a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The ID of the retail group to get</param>
        /// <returns>A retail group with a given ID, or null if not found</returns>
        public virtual RetailGroup Get(IConnectionManager entry, RecordIdentifier groupID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    BaseSql +
                    "Where a.GROUPID = @groupId AND a.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "groupId", (string) groupID);

                var groups = Execute<RetailGroup>(entry, cmd, CommandType.Text, PopulateRetailGroup);

                return (groups.Count > 0) ? groups[0] : null;
            }
        }

        /// <summary>
        /// Returns a list of retail groups that are within a specific retail department
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailDepartmentID">The unique ID of the retail deparment</param>
        /// <param name="sortEnum">Controls which columns will be sorted by in the SQL
        /// statement</param>
        /// <param name="sortBackwards">If true then the ordering will be descending
        /// otherwise it will be ascending</param>
        public virtual List<RetailGroup> GetRetailGroupsInRetailDepartment(IConnectionManager entry, RecordIdentifier retailDepartmentID, RetailGroupSorting sortEnum, bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    BaseSql +
                    "Where a.DEPARTMENTID = @retailDepartmentID AND a.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "retailDepartmentID", (string) retailDepartmentID);

                return Execute<RetailGroup>(entry, cmd, CommandType.Text, PopulateRetailGroup);
            }
        }

        /// <summary>
        /// Returns a list of retail groups that are not included in the retail department
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="excludedRetailDepartmentID">The unique ID of the retail
        /// department</param>
        /// <param name="searchText">Full or partial name of a retail group to be
        /// found</param>
        /// <param name="sortEnum">Controls which columns will be sorted by in the SQL
        /// statement</param>
        /// <param name="sortBackwards">If true then the ordering will be descending
        /// otherwise it will be ascending</param>
        public List<RetailGroup> GetRetailGroupsNotInRetailDepartment(
            IConnectionManager entry, 
            RecordIdentifier excludedRetailDepartmentID,
            string searchText,
            RetailGroupSorting sortEnum, 
            bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    BaseSql +
                    "Where a.DEPARTMENTID <> @excludedRetailDepartmentID AND a.DATAAREAID = @dataAreaId " +
                    "and (a.NAME like @searchString or a.NAMEALIAS like @searchString) " +
                    ResolveSort(sortEnum, sortBackwards);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "excludedRetailDepartmentID", (string) excludedRetailDepartmentID);
                MakeParam(cmd, "searchString", "%" + searchText + "%");

                return Execute<RetailGroup>(entry, cmd, CommandType.Text, PopulateRetailGroup);
            }
        }

        /// <summary>
        /// Adds a specific retail group to the retail department
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailGroupID">The unique ID of the retail group</param>
        /// <param name="retailDepartmentID">The unique ID of the retail department</param>
        public virtual void AddRetailGroupToRetailDepartment(IConnectionManager entry, RecordIdentifier retailGroupID, RecordIdentifier retailDepartmentID)
        {
            var retailGroup = Get(entry, retailGroupID);
            retailGroup.RetailDepartmentID = (string)retailDepartmentID;
            Save(entry, retailGroup);
        }

        /// <summary>
        /// Clears the retail department value of the specific retail group 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailGroupID">The unique ID of the retail group</param>
        public virtual void RemoveRetailGroupFromRetailDepartment(IConnectionManager entry, RecordIdentifier retailGroupID)
        {
            var retailGroup = Get(entry, retailGroupID);
            retailGroup.RetailDepartmentID = "";
            Save(entry, retailGroup);
        }

        /// <summary>
        /// Checks if a retail group with a given ID exists in the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The ID of the group to check for</param>
        /// <returns>Whether the given group exists</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier groupID)
        {
            return RecordExists(entry, "RBOINVENTITEMRETAILGROUP", "GROUPID", groupID);
        }

        /// <summary>
        /// Deletes a retail group with a given ID
        /// </summary>
        /// <remarks>Requires the 'Manage retail groups' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The ID of the retail group to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier groupID)
        {
            DeleteRecord(entry, "RBOINVENTITEMRETAILGROUP", "GROUPID", groupID, BusinessObjects.Permission.ManageRetailGroups);
        }

        /// <summary>
        /// Saves a given retail group to the database
        /// </summary>
        /// <remarks>Requires the 'Manage retail groups' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="group">The retail group to save</param>
        public virtual void Save(IConnectionManager entry, RetailGroup group)
        {
            var statement = new SqlServerStatement("RBOINVENTITEMRETAILGROUP");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageRetailGroups);

            group.Validate();

            bool isNew = false;
            if (group.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                group.ID = DataProviderFactory.Instance.GenerateNumber<IRetailGroupData, RetailGroup>(entry); 
            }

            if (isNew || !Exists(entry,group.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("GROUPID", (string)group.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("GROUPID", (string)group.ID);
            }

            statement.AddField("NAME", group.Text);
            statement.AddField("DEPARTMENTID", (string)group.RetailDepartmentID);
            //statement.AddField("SIZEGROUPID", (string)group.SizeGroupId);
            //statement.AddField("COLORGROUPID", (string)group.ColorGroupId);
            //statement.AddField("STYLEGROUPID", (string)group.StyleGroupId);
            statement.AddField("SALESTAXITEMGROUP", (string)group.ItemSalesTaxGroupId);
            //statement.AddField("INVENTDIMGROUPID", (string)group.DimensionGroupId);
            statement.AddField("DEFAULTPROFIT", group.ProfitMargin, SqlDbType.Decimal);
            statement.AddField("POSPERIODICID", group.ValidationPeriod); 
            
            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Returns true if the retail group is in the retail department
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailGroupID">The unique ID of the retail group</param>
        /// <param name="retailDepartmentID">The unique ID of the retail department</param>
        public virtual bool RetailGroupInDepartment(IConnectionManager entry, RecordIdentifier retailGroupID, RecordIdentifier retailDepartmentID)
        {
            var groupsInDepartment = GetRetailGroupsInRetailDepartment(entry, retailDepartmentID, RetailGroupSorting.RetailDepartmentName, false);

            return groupsInDepartment.Any(x => x.ID == retailGroupID);
        }

        #region ISequenceable Members

        /// <summary>
        /// Returns true if information about the given class exists in the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The unique sequence ID to search for</param>
        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        /// <summary>
        /// Returns a unique ID for the class
        /// </summary>
        public RecordIdentifier SequenceID
        {
            get { return "RETAILGROUP"; }
        }

        #endregion
    }
}
