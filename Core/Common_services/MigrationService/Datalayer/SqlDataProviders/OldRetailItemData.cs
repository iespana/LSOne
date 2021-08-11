using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Services.Datalayer.BusinessObjects;
using LSOne.Services.Datalayer.BusinessObjects.ListItems;
using LSOne.Services.Datalayer.DataProviders;
using LSOne.Utilities.DataTypes;
using RetailItemSearchEnum = LSOne.DataLayer.BusinessObjects.ItemMaster.RetailItemSearchEnum;

namespace LSOne.Services.Datalayer.SqlDataProviders
{
    /// <summary>
    /// Data provider class for a retail item.
    /// </summary>
    public partial class OldRetailItemData : SqlServerDataProviderBase, IOldRetailItemData
    {
        public const string CustomColumns = "<CUSTOMCOLUMNS>";
        public const string CustomConditions = "<CUSTOMCONDITIONS>";

        /// <summary>
        /// Used for the AdvancedSearch function. The string contains a number of tags that are replaced by appropriate search string segments
        /// in the AdvancedSearch function. NOTE: this string contains the entire query, and cannot be appended.    
        /// </summary>
        protected virtual string AdvancedSearchString
        {
            //The tags in the string are:
            // <orderBy> = the name of the column that should be used to sort the result
            //
            // <retailGroupSelect> = the select segment for retail groups
            // <retailGroupJoin> = the join segment for retail group
            // <retailGroupCondition> = the WHERE condition for retail group search
            //
            // <retailDepartmentSelect> = the select segment for retail departments
            // <retailDepartmentJoin> = the join segment for retail department
            // <retailDepartmentCondition> = the WHERE condition for retail department search
            //
            // <taxGroupSelect> = the select segment for tax groups
            // <taxGroupJoin> = the join segment for tax groups
            // <taxGroupCondition> = the WHERE condition for tax group search                        
            //
            // <variantGroupSelect> = the select segment for variant groups, note that the variant group ID is already included in the query
            // <variantGroupJoin> = the join segment for variant group
            // <variantGroupCondition> = the WHERE condition for variant group search
            // 
            // <vendorSelect> = the select segment for default vendor
            // <vendorJoin> = the join segment for the vendor
            // <vendorCondition> = the WHERE condition for the vendor search
            // 
            // <barCodeSelect> = the select segment for default barcode
            // <barCodeJoin> = the join segment for the barcode
            // <barCodeCondition> = the WHERE condition for the barcode search
            //
            // <specialGroupJoin> = the join segment for the special groups
            // <specialGroupCondition> = the WHERE condition for the special group search

            get
            {
                return @"select ss.* from(
                        Select s.*, ROW_NUMBER() OVER(order by <orderBy> ) AS ROW  from (
                        select distinct it.ITEMID, 
                        ISNULL(it.ITEMGROUPID,'') as ITEMGROUPID,
                        ISNULL(it.ITEMNAME,'') as ITEMNAME,
                        ISNULL(it.NAMEALIAS,'') as NAMEALIAS,
                        ISNULL(it.DIMGROUPID,'') as VARIANTGROUP,
						ISNULL(itmod.PRICEINCLTAX,0) as PRICEINCLTAX, 

                        <variantGroupSelect>

                        ISNULL(it.ITEMTYPE,-1) as ITEMTYPE, 
                        ISNULL(itg.NAME,'') as ITEMGROUPNAME, 

                        <retailGroupSelect>

                        <retailDepartmentSelect>

                        <taxGroupSelect>

                        <vendorSelect>

                        <barCodeSelect>
                        
                        from INVENTTABLE it 
                        left outer join INVENTITEMGROUP itg on it.DATAAREAID = itg.DATAAREAID and it.ITEMGROUPID = itg.ITEMGROUPID 
                        <variantGroupJoin>

                        -- RBOINVENTTABLE
                        join RBOINVENTTABLE rboit on it.DATAAREAID = rboit.DATAAREAID and it.ITEMID = rboit.ITEMID
                                            
                        <retailGroupJoin>
                        <retailDepartmentJoin>

                        -- INVENTTABLEMODULE
                        left outer join INVENTTABLEMODULE itmod on it.DATAAREAID = itmod.DATAAREAID and itmod.ITEMID = it.itemid and itmod.moduletype = 2 
                        
                        <taxGroupJoin>

                        <vendorJoin>
                        
                        <barCodeJoin>

                        <specialGroupJoin>

                        where it.DATAAREAID = @dataAreaId
                        and (it.ITEMNAME Like @searchString or it.ITEMID Like @searchString or it.NAMEALIAS Like @searchString)
                        -- RetailGroup
                        <retailGroupCondition>
                        -- RetailDepartment
                        <retailDepartmentCondition>
                        -- TaxGroup
                        <taxGroupCondition>
                        -- VariantGroup
                        <variantGroupCondition>
                        -- Vendor
                        <vendorCondition>
                        -- Bar code
                        <barCodeCondition>
                        -- Special groups
                        <specialGroupCondition>
                            
                        ) s 
                        ) ss
                        where ss.ROW between  @rowFrom  and @rowTo";
            }
        }

        protected virtual void PopulateItem(IDataReader dr, OldRetailItem item)
        {
            PopulateItemListItem(dr,item); // We can do this because Item inherits from ItemListItem

            item.Notes = (string)dr["Notes"];
            item.ModelGroupID = (string)dr["MODELGROUPID"];
            item.DimensionGroupID = (string)dr["DIMGROUPID"];
            item.DimensionGroupName = (string)dr["DIMGROUPNAME"];
            item.ItemType = (OldRetailItem.ItemTypeEnum)dr["ITEMTYPE"];

            // From RBOINVENTTABLE
            item.RetailItemType = (OldRetailItem.OldRetailItemTypeEnum)dr["RETAILITEMTYPE"];
            item.DateToBeBlocked = Date.FromAxaptaDate(dr["DATETOBEBLOCKED"]);
            item.BarCodeSetupID = (string)dr["BARCODESETUPID"];
            item.BarCodeSetupDescription = (string)dr["BARCODESETUPDESCRIPTION"];
            item.ScaleItem = ((byte)dr["SCALEITEM"] != 0);
            item.KeyInPrice = (OldRetailItem.KeyInPriceEnum)dr["KEYINGINPRICE"];
            item.KeyInQuantity = (OldRetailItem.KeyInQuantityEnum)dr["KEYINGINQTY"];
            item.MustKeyInComment = ((byte)dr["MUSTKEYINCOMMENT"] != 0);
            item.MustSelectUOM = ((byte)dr["MUSTSELECTUOM"] != 0);
            item.ZeroPriceValid = ((byte)dr["ZEROPRICEVALID"] != 0);
            item.QuantityBecomesNegative = ((byte)dr["QTYBECOMESNEGATIVE"] != 0);
            item.NoDiscountAllowed = ((byte)dr["NODISCOUNTALLOWED"] != 0);
            item.DateToActivateItem = Date.FromAxaptaDate(dr["DATETOACTIVATEITEM"]); 
            item.IsFuelItem = ((byte)dr["FUELITEM"] != 0);
            item.GradeID = (string)dr["GRADEID"];
            item.SizeGroupID = (string)dr["SIZEGROUP"];
            item.ColorGroupID = (string)dr["COLORGROUP"];
            item.StyleGroupID = (string)dr["STYLEGROUP"];
            item.PrintVariantsShelfLabels = ((byte)dr["PRINTVARIANTSSHELFLABELS"] != 0);
            item.DefaultVendorItemId = (string)dr["PRIMARYVENDORID"];
            item.ValidationPeriod = (string)dr["POSPERIODICID"];
 
            item.SizeGroupName = (string)dr["SIZEGROUPNAME"];
            item.ColorGroupName = (string)dr["COLORGROUPNAME"];
            item.StyleGroupName = (string)dr["STYLEGROUPNAME"];

            item.RetailDivisionID = AsString(dr["RETAILDIVISIONID"]);
            item.RetailDivisionName = (string)dr["RETAILDIVISIONNAME"];
            item.RetailGroupID = (string)dr["RETAILGROUPID"];
            item.RetailGroupName = (string)dr["RETAILGROUPNAME"];
            item.RetailDepartmentID = (string)dr["RETAILDEPARTMENTID"];
            item.RetailDepartmentName = (string)dr["RETAILDEPARTMENTNAME"];

            item.ProfitMargin = (decimal)dr["DEFAULTPROFIT"];

            item.Dirty = false;

            item.ValidationPeriod = (string)dr["POSPERIODICID"];
            item.ValidationPeriodDescription = (string)dr["VALIDATIONPERIODDISCOUNTDESCRIPTION"];
            item.BlockedOnPOS = (byte)dr["BLOCKEDONPOS"] != 0;
        }

        protected virtual void PopulateModule(IDataReader dr, OldRetailItem.RetailItemModule module)
        {
            module.ItemID = (string)dr["ITEMID"];
            module.ModuleType = (OldRetailItem.ModuleTypeEnum)dr["MODULETYPE"];
            module.Unit = (string)dr["UNITID"];
            module.UnitText = (string)dr["UNITTEXT"];
            module.Price = (decimal)dr["PRICE"];
            module.PriceUnit = (decimal)dr["PRICEUNIT"];
            module.Markup = (decimal)dr["MARKUP"];
            module.LineDiscount = (string)dr["LINEDISC"];
            module.LineDiscountName = (string)dr["LINEDISCNAME"];
            module.MultilineDiscount = (string)dr["MULTILINEDISC"];
            module.MultiLineDiscountName = (string)dr["MULTILINEDISCNAME"];
            module.TotalDiscount = (((byte)dr["ENDDISC"]) != 0);
            module.PriceDate = Date.FromAxaptaDate(dr["PRICEDATE"]);
            module.PriceQty = (decimal)dr["PRICEQTY"];
            module.AllocateMarkup = (((byte)dr["ALLOCATEMARKUP"]) != 0);
            module.TaxItemGroupID = (string)dr["TAXITEMGROUPID"];
            module.TaxItemGroupName = (string)dr["TAXITEMGROUPNAME"];
            module.LastKnownPriceWithTax = (decimal)dr["PRICEINCLTAX"];
            module.Dirty = false;
        }

        protected virtual void PopulateItemListItem(IDataReader dr, OldItemListItem item)
        {
            item.ID = (string)dr["ITEMID"];
            item.Text = (string)dr["ITEMNAME"];
            item.NameAlias = (string)dr["NAMEALIAS"];
            item.ItemType = (OldRetailItem.ItemTypeEnum)dr["ITEMTYPE"];
        }

        protected virtual void PopulateAdvancedItemListItem(IDataReader dr, OldItemListItem item)
        {
            item.ID = (string)dr["ITEMID"];
            item.Text = (string)dr["ITEMNAME"];
            item.NameAlias = (string)dr["NAMEALIAS"];
            item.ItemType = (OldRetailItem.ItemTypeEnum)dr["ITEMTYPE"];
            item.RetailGroupName = (string)dr["RETAILGROUPNAME"];
            item.RetailDepartmentName = (string)dr["RETAILDEPARTMENTNAME"];
            item.TaxGroupName = (string)dr["TAXGROUPNAME"];
            item.Price = (decimal)dr["PRICEINCLTAX"];
        }

        protected virtual void PopulateAdvancedItemListItemWithRowCount(IDataReader dr, OldItemListItem item)
        {
            PopulateAdvancedItemListItem(dr,item);
            item.RowCount = (int)dr["Row_Count"];
        }

        protected virtual void PopulateItemID(IDataReader dr, RecordIdentifier id)
        {
            id = (string)dr["ITEMID"];
        }

        protected virtual string BaseGetString
        {
            get
            {
                return "select a.ITEMID, " +
                       "ISNULL(a.ITEMGROUPID,'') as ITEMGROUPID," +
                       "ISNULL(a.ITEMNAME,'') as ITEMNAME," + 
                       "ISNULL(it.POSPERIODICID, '') AS POSPERIODICID,  " +
                    "ISNULL(a.NAMEALIAS,'') as NAMEALIAS,ISNULL(a.ITEMTYPE,-1) as ITEMTYPE,ISNULL(it.ITEMTYPE,1) as RETAILITEMTYPE,ISNULL(a.Notes,'') as Notes," +
                    "ISNULL(a.MODELGROUPID,'') as MODELGROUPID,ISNULL(a.DIMGROUPID,'') as DIMGROUPID," +
                    "ISNULL(it.BLOCKEDONPOS,0) as BLOCKEDONPOS, it.DATEBLOCKED," +
                    "ISNULL(it.SCALEITEM,0) as SCALEITEM,ISNULL(it.KEYINGINPRICE,0) as KEYINGINPRICE," +
                    "ISNULL(it.KEYINGINQTY,0) as KEYINGINQTY,ISNULL(it.MUSTKEYINCOMMENT,0) as MUSTKEYINCOMMENT," +
                    "ISNULL(it.MUSTSELECTUOM,0) as MUSTSELECTUOM," +
                    "ISNULL(it.ZEROPRICEVALID,0) as ZEROPRICEVALID,ISNULL(it.QTYBECOMESNEGATIVE,0) as QTYBECOMESNEGATIVE," +
                    "ISNULL(it.NODISCOUNTALLOWED,0) as NODISCOUNTALLOWED,it.DATETOACTIVATEITEM," +
                    "ISNULL(it.FUELITEM,0) as FUELITEM,ISNULL(GRADEID,'') as GRADEID," +
                    "ISNULL(it.POSPERIODICID, '') AS POSPERIODICID, "+
                    "ISNULL(it.SIZEGROUP,'') as SIZEGROUP,ISNULL(sz.DESCRIPTION,'') as SIZEGROUPNAME," +
                    "ISNULL(it.COLORGROUP,'') as COLORGROUP,ISNULL(co.DESCRIPTION,'') as COLORGROUPNAME," +
                    "ISNULL(it.STYLEGROUP,'') as STYLEGROUP,ISNULL(st.DESCRIPTION,'') as STYLEGROUPNAME," +
                    "ISNULL(it.DIVISIONID,'') as RETAILDIVISIONID,ISNULL(div.NAME,'') as RETAILDIVISIONNAME," +
                    "ISNULL(it.ITEMGROUP,'') as RETAILGROUPID,ISNULL(iir.NAME,'') as RETAILGROUPNAME," +
                    "ISNULL(it.ITEMDEPARTMENT,'') as RETAILDEPARTMENTID,ISNULL(iid.NAME,'') as RETAILDEPARTMENTNAME," +
                    "ISNULL(it.PRINTVARIANTSSHELFLABELS,0) as PRINTVARIANTSSHELFLABELS, ISNULL(a.PRIMARYVENDORID,'') as PRIMARYVENDORID," +
                    "it.DATETOBEBLOCKED,ISNULL(it.ITEMTYPE,0) as ITEMTYPE,ISNULL(it.BARCODESETUPID,'') as BARCODESETUPID, " +
                    "ISNULL(itg.NAME,'') as ITEMGROUPNAME,ISNULL(idg.NAME,'') as DIMGROUPNAME,ISNULL(bs.DESCRIPTION,'') as BARCODESETUPDESCRIPTION, " +
                    "ISNULL(it.DEFAULTPROFIT,0) as DEFAULTPROFIT, " +
                    "ISNULL(PDV.ID, '') AS POSPERIODICID, ISNULL(PDV.DESCRIPTION, '') AS VALIDATIONPERIODDISCOUNTDESCRIPTION " + 
                    CustomColumns +
                    
                    " from INVENTTABLE a " +
                    "left outer join RBOINVENTTABLE it on a.ITEMID = it.ITEMID and a.DATAAREAID = it.DATAAREAID " +
                    "left outer join RBOSIZEGROUPTABLE sz on it.SIZEGROUP = sz.SIZEGROUP and it.DATAAREAID = sz.DATAAREAID " +
                    "left outer join RBOCOLORGROUPTABLE co on it.COLORGROUP = co.COLORGROUP and it.DATAAREAID = co.DATAAREAID " +
                    "left outer join RBOSTYLEGROUPTABLE st on it.STYLEGROUP = st.STYLEGROUP and it.DATAAREAID = st.DATAAREAID " +
                    "left outer join RBOINVENTITEMRETAILDIVISION div on it.DIVISIONID = div.DIVISIONID and it.DATAAREAID = div.DATAAREAID " +
                    "left outer join RBOINVENTITEMRETAILGROUP iir on it.ITEMGROUP = iir.GROUPID and it.DATAAREAID = iir.DATAAREAID " +
                    "left outer join RBOINVENTITEMDEPARTMENT iid on it.ITEMDEPARTMENT = iid.DEPARTMENTID and it.DATAAREAID = iid.DATAAREAID " +
                    "left outer join INVENTITEMGROUP itg on a.ITEMGROUPID = itg.ITEMGROUPID and it.DATAAREAID = itg.DATAAREAID " +
                    "left outer join INVENTDIMGROUP idg on a.DIMGROUPID = idg.DIMGROUPID and a.DATAAREAID = idg.DATAAREAID " +
                    "left outer join BARCODESETUP bs on it.BARCODESETUPID = bs.BARCODESETUPID and a.DATAAREAID = bs.DATAAREAID " +
                    "left outer join POSDISCVALIDATIONPERIOD PDV on a.DATAAREAID = PDV.DATAAREAID and it.POSPERIODICID = PDV.ID ";
            }
        }

        protected virtual string BaseGetModuleString
        {
            get
            {
                return "Select ITEMID,MODULETYPE,ISNULL(iu.UNITID,'') as UNITID,ISNULL(u.TXT,'') as UNITTEXT," +
                        "ISNULL(PRICE,0.0) as PRICE,ISNULL(PRICEUNIT,0.0) as PRICEUNIT,ISNULL(MARKUP,0.0) as MARKUP," +
                        "ISNULL(LINEDISC,'') as LINEDISC,ISNULL(ld.NAME,'') as LINEDISCNAME," +
                        "ISNULL(MULTILINEDISC,'') as MULTILINEDISC,ISNULL(mld.NAME,'') as MULTILINEDISCNAME," +
                        "ISNULL(ENDDISC,0) as ENDDISC,PRICEDATE,ISNULL(PRICEQTY,0.0) as PRICEQTY," +
                        "ISNULL(ALLOCATEMARKUP,0) as ALLOCATEMARKUP," +
                        "ISNULL(PRICEINCLTAX,0.0) as PRICEINCLTAX," +
                        "ISNULL(iu.TAXITEMGROUPID,'') as TAXITEMGROUPID,ISNULL(tgh.NAME,'') as TAXITEMGROUPNAME " +
                        "from INVENTTABLEMODULE iu " +
                        "left outer join UNIT u on iu.UNITID = u.UNITID and iu.DATAAREAID = u.DATAAREAID " +
                        "left outer join PRICEDISCGROUP ld on iu.LINEDISC = ld.GROUPID and ld.TYPE = 1 and ld.MODULE = 0 and iu.DATAAREAID = ld.DATAAREAID " +
                        "left outer join PRICEDISCGROUP mld on iu.MULTILINEDISC = mld.GROUPID and mld.TYPE = 2 and mld.MODULE = 0 and iu.DATAAREAID = mld.DATAAREAID " +
                        "left outer join TAXITEMGROUPHEADING tgh on tgh.TAXITEMGROUP = iu.TAXITEMGROUPID and tgh.DATAAREAID = iu.DATAAREAID ";
            }
        }

        /// <summary>
        /// Looks up the unit id for a item by a given id. The type of unit depends on the module type.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">Id of the retail item in the database</param>
        /// <param name="module">Module type enum which determines what type of unit id we are returning</param>
        /// <returns>The unit ID of an item depending on the module type</returns>
        public virtual RecordIdentifier GetItemUnitID(IConnectionManager entry, RecordIdentifier itemID, OldRetailItem.ModuleTypeEnum module)
        {
            using(var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select UNITID from INVENTTABLEMODULE " +
                    "where ITEMID = @itemID and MODULETYPE = @module and DATAAREAID = @dataAreaID";

                    MakeParam(cmd,"itemID",(string)itemID);
                    MakeParam(cmd, "module", (int)module, SqlDbType.Int);
                    MakeParam(cmd,"dataAreaID",entry.Connection.DataAreaId);

                return (string) entry.Connection.ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// Updates the unit information on a specific item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item to update</param>
        /// <param name="unitID">The new unit ID information</param>
        /// <param name="module">Which module information to update (inventory, purchase,
        /// sales)</param>
        public virtual void UpdateUnitID(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier unitID, OldRetailItem.ModuleTypeEnum module)
        {
            var item = Get(entry, itemID);
            if (item[module].Unit != unitID)
            {
                item[module].Unit = unitID;
                item[module].Dirty = true;
                Save(entry, item);
            }
        }

        /// <summary>
        /// Searched for retail items that contain a given search text, and returns the results as a List of <see cref="ItemListItem"/>
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchString">The text to search for. Searches in item name, in the ID field and the search alias field</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the text or if the text may contain the search text.</param>
        /// <param name="sort">A string defining the sort column</param>
        /// <returns>A list of found items</returns>
        public virtual List<OldItemListItem> Search(IConnectionManager entry, string searchString, int rowFrom, int rowTo, bool beginsWith, string sort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                //ValidateSecurity(entry, BusinessObjects.Permission.CustomerView);

                string modifiedSearchString = PreProcessSearchText(searchString, true, beginsWith);

                cmd.CommandText =
                    "Select s.* from (" +
                    "select it.ITEMID, ISNULL(it.ITEMGROUPID,'') as ITEMGROUPID,ISNULL(it.ITEMNAME,'') as ITEMNAME," +
                    "ISNULL(it.NAMEALIAS,'') as NAMEALIAS,ISNULL(it.ITEMTYPE,-1) as ITEMTYPE, ISNULL(itg.NAME,'') as ITEMGROUPNAME, " +
                    "ROW_NUMBER() OVER(order by " + sort + ") AS ROW " +
                    "from INVENTTABLE it left outer join INVENTITEMGROUP itg on it.DATAAREAID = itg.DATAAREAID and it.ITEMGROUPID = itg.ITEMGROUPID " +
                    "where it.DATAAREAID = @dataAreaId " +
                    " and (it.ITEMNAME Like @searchString or it.ITEMID Like @searchString or it.NAMEALIAS Like @searchString)) s " +
                    "where s.ROW between " + rowFrom + " and " + rowTo;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "searchString", modifiedSearchString);

                return Execute<OldItemListItem>(entry, cmd, CommandType.Text, PopulateItemListItem);
            }
        }

        public List<RecordIdentifier> AdvancedSearchIDOnly(IConnectionManager entry,
            string idOrDescription = null,
            bool idOrDescriptionBeginsWith = true,
            RecordIdentifier retailGroupID = null,
            RecordIdentifier retailDepartmentID = null,
            RecordIdentifier taxGroupID = null,
            RecordIdentifier variantGroupID = null,
            RecordIdentifier vendorID = null,
            string barCode = null,
            bool barCodeBeginsWith = true,
            RecordIdentifier specialGroup = null)
        {
            string whereConditions = "";
            string joins = "";

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"
                    select distinct it.ITEMID
                    from INVENTTABLE it 
                    join RBOINVENTTABLE rboit on it.DATAAREAID = rboit.DATAAREAID and it.ITEMID = rboit.ITEMID 
                    left outer join INVENTTABLEMODULE itmod on it.DATAAREAID = itmod.DATAAREAID and it.ITEMID = itmod.ITEMID and itmod.MODULETYPE = 2
                    <joins>
                    where it.DATAAREAID = @dataAreaId <whereConditions>";

                if (idOrDescription != null)
                {
                    idOrDescription = PreProcessSearchText(idOrDescription, true, idOrDescriptionBeginsWith); 

                    whereConditions += " and (it.ITEMNAME Like @searchString or it.ITEMID Like @searchString or it.NAMEALIAS Like @searchString) ";

                    MakeParamNoCheck(cmd, "searchString", idOrDescription);
                }

                if (retailGroupID != null)
                {
                    whereConditions += " and rboit.ITEMGROUP = @retailGroup ";

                    MakeParamNoCheck(cmd, "retailGroup", (string)retailGroupID);
                }

                if (retailDepartmentID != null)
                {
                    whereConditions += " and rboit.ITEMDEPARTMENT = @retailDepartment ";

                    MakeParamNoCheck(cmd, "retailDepartment", (string)retailDepartmentID);
                }

                if (taxGroupID != null)
                {
                    whereConditions += " and itmod.TAXITEMGROUPID = @taxGroup ";

                    MakeParamNoCheck(cmd, "taxGroup", (string)taxGroupID);
                }

                if (variantGroupID != null)
                {
                    whereConditions += " and it.DIMGROUPID = @variantGroupID ";

                    MakeParamNoCheck(cmd, "variantGroupID", (string)variantGroupID);
                }

                if (vendorID != null)
                {
                    joins += " left outer join VENDORITEMS vi on vi.RETAILITEMID = it.ITEMID and vi.DATAAREAID = it.DATAAREAID and vi.VENDORID = @vendorID ";
                    //whereConditions += " and vi.VENDORID = @vendorID ";

                    MakeParamNoCheck(cmd, "vendorID", (string)vendorID);
                }

                if (barCode != null)
                {
                    joins += " left outer join INVENTITEMBARCODE ibarcode on it.DATAAREAID = ibarcode.DATAAREAID and it.ITEMID = ibarcode.ITEMID";

                    barCode = (barCodeBeginsWith ? "" : "%") + barCode + "%";

                    whereConditions += " and ibarcode.ITEMBARCODE like @barCodeSearchString ";

                    MakeParamNoCheck(cmd, "barCodeSearchString", barCode);
                }

                if (specialGroup != null)
                {
                    joins += " left outer join RBOSPECIALGROUPITEMS sg on sg.ITEMID = it.ITEMID and sg.DATAAREAID = it.DATAAREAID and sg.GROUPID = @specialGroupID ";
                    whereConditions += " and sg.GROUPID = @specialGroupID ";

                    MakeParamNoCheck(cmd, "specialGroupID", (string)specialGroup);
                }

                cmd.CommandText = cmd.CommandText.Replace("<joins>", joins);
                cmd.CommandText = cmd.CommandText.Replace("<whereConditions>", whereConditions);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                List<RecordIdentifier> result = new List<RecordIdentifier>();

                ValidateSecurity(entry);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    while (dr.Read())
                    {
                        result.Add((RecordIdentifier)(string)dr["ITEMID"]);
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


        public List<OldItemListItem> AdvancedSearchOptimized(IConnectionManager entry,
                                                                 int rowFrom, int rowTo, string sort,
                                                                 out int totalRecordsMatching,
                                                                 string idOrDescription = null,
                                                                 bool idOrDescriptionBeginsWith = true,
                                                                 RecordIdentifier retailGroupID = null,
                                                                 RecordIdentifier retailDepartmentID = null,
                                                                 RecordIdentifier taxGroupID = null,
                                                                 RecordIdentifier variantGroupID = null,
                                                                 RecordIdentifier vendorID = null,
                                                                 string barCode = null,
                                                                 bool barCodeBeginsWith = true,
                                                                 RecordIdentifier specialGroup = null)
        {
            // DO NOT change this function without also changing the AdvancedSearchIDOnly function so Excel Export will match what was actually searched.

            string whereConditions = "";

            bool rboInventTable = false;
            bool itmodTable = false;

            string columns =
                @"      ss.ITEMID,
                        ss.ITEMNAME,
                        ss.NAMEALIAS,
                        ss.ITEMTYPE,
	                    <rboitexternalCoumns>
                        <itmodExternalColumns>	
                        ss.PRICEINCLTAX,
	                    ss.ROW ";

            string rboitJoin =
                @"join RBOINVENTTABLE rboit 
                        on {1}.DATAAREAID = rboit.DATAAREAID 
                        and {1}.ITEMID = rboit.ITEMID {0}
                    {3} join RBOINVENTITEMRETAILGROUP ritretgrp 
                        on {1}.DATAAREAID = ritretgrp.DATAAREAID 
                        and rboit.ITEMGROUP = ritretgrp.GROUPID {2}
                    left outer join RBOINVENTITEMDEPARTMENT ritdep 
                        on {1}.DATAAREAID = ritdep.DATAAREAID 
                        and rboit.ITEMDEPARTMENT = ritdep.DEPARTMENTID";

            string rboitColumns =
                @"ISNULL(ritretgrp.NAME,'') as RETAILGROUPNAME,
	              ISNULL(ritdep.NAME,'') as RETAILDEPARTMENTNAME,";

            string itmodJoin =
                @"  left outer join TAXITEMGROUPHEADING theading 
                        on {0}.DATAAREAID = theading.DATAAREAID 
                        and {1}.TAXITEMGROUPID = theading.TAXITEMGROUP";

            string itmodColumns = "ISNULL(theading.NAME,'') as TAXGROUPNAME,";


            using (var cmd = entry.Connection.CreateCommand())
            using (var cmdCount = entry.Connection.CreateCommand())
            {
                if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012)
                {
                    columns += ",ss.Row_Count";

                }
                if (retailGroupID != null || retailDepartmentID != null)
                {
                    rboInventTable = true;
                    rboitJoin = string.Format(rboitJoin,
                                              retailGroupID != null
                                                  ? " and rboit.ITEMGROUP = @retailGroupID "
                                                  : "",
                                              "it",
                                              retailDepartmentID != null
                                                  ? " and rboit.ITEMDEPARTMENT = @retailDepartmentID "
                                                  : "",
                                              retailGroupID != null
                                                  ? " inner "
                                                  : " left outer");
                    if (retailGroupID != null)
                    {

                        MakeParam(cmd, "retailGroupID", retailGroupID);
                        MakeParam(cmdCount, "retailGroupID", retailGroupID);
                    }
                    if (retailDepartmentID != null)
                    {
                        whereConditions += " and rboit.ITEMDEPARTMENT = @retailDepartmentID ";

                        MakeParam(cmd, "retailDepartmentID", retailDepartmentID);
                        MakeParam(cmdCount, "retailDepartmentID", retailDepartmentID);
                    }
                }
                else
                {
                    rboitJoin = string.Format(rboitJoin, "", "ss", "", " left outer", " left outer");

                }

                if (taxGroupID != null)
                {
                    itmodTable = true;
                    itmodJoin = string.Format(itmodJoin,
                                              "it", "itmod"
                                              );
                    whereConditions += " and itmod.TAXITEMGROUPID = @taxGroupID ";
                    MakeParam(cmd, "taxGroupID", taxGroupID);
                    MakeParam(cmdCount, "taxGroupID", taxGroupID);
                }
                else
                {
                    itmodJoin = string.Format(itmodJoin, "ss", "ss");
                }



                // Use the optimized query, with joins only on the outer statement
                string commandText = @"select 
                        <columns>
	                    from(
                            Select 
                                s.*, 
                                ROW_NUMBER() OVER(order by <sort>) AS ROW
                                <RowCount> 
                                from (
                                    select distinct it.ITEMID, 
	                                    ISNULL(it.ITEMNAME,'') as ITEMNAME, 
	                                    ISNULL(it.NAMEALIAS,'') as NAMEALIAS,
	                                    ISNULL(it.ITEMTYPE,-1) as ITEMTYPE,
                                        <rboitinternalColumns>
                                        <itmodInternalColumns>
						                it.DATAAREAID,
						                it.ITEMGROUPID,
                                        ISNULL(itmod.PRICEINCLTAX,0) as PRICEINCLTAX, 
										itmod.TAXITEMGROUPID
                                        from INVENTTABLE it
                                        inner join INVENTTABLEMODULE itmod 
                                            on it.DATAAREAID = itmod.DATAAREAID 
                                            and it.ITEMID = itmod.ITEMID and itmod.MODULETYPE = 2
                                        <rboinventtablejoin>
                                        <itmodjoin>
                                        <vendortablejoin>
                                        <barcodetablejoin>
                                        <specialgroupjoin>
                                    where it.DATAAREAID = @dataAreaId <whereConditions> 
                                ) s
                            ) ss
                            <rboinventtablejoinouter>
                            <itmodjoinouter>
                        <rowConfinementAndOrder>";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmdCount, "dataAreaId", entry.Connection.DataAreaId);

                if (rboInventTable)
                {
                    string rboitExternalColumns =
                        @"RETAILGROUPNAME,
	                RETAILDEPARTMENTNAME,";
                    commandText = commandText.Replace("<rboinventtablejoin>", rboitJoin)
                                             .Replace("<rboinventtablejoinouter>", "")
                                             .Replace("<rboitinternalColumns>", rboitColumns);

                    columns = columns.Replace("<rboitexternalCoumns>", rboitExternalColumns);
                }
                else
                {
                    commandText = commandText.Replace("<rboinventtablejoin>", "")
                                             .Replace("<rboinventtablejoinouter>", rboitJoin)
                                             .Replace("<rboitinternalColumns>", "");

                    columns = columns.Replace("<rboitexternalCoumns>", rboitColumns);
                }
                if (itmodTable)
                {
                    string itmodExternalColumns = "TAXGROUPNAME,";
                    commandText = commandText.Replace("<itmodjoin>", itmodJoin)
                                             .Replace("<itmodjoinouter>", "")
                                             .Replace("<itmodInternalColumns>", itmodColumns);
                    columns = columns.Replace("<itmodExternalColumns>", itmodExternalColumns);
                }
                else
                {
                    commandText = commandText.Replace("<itmodjoin>", "")
                                             .Replace("<itmodjoinouter>", itmodJoin)
                                             .Replace("<itmodInternalColumns>", "");
                    columns = columns.Replace("<itmodExternalColumns>", itmodColumns);
                }

                if (vendorID != null)
                {
                    commandText = commandText.Replace("<vendortablejoin>",
                                                      @"inner join VENDORITEMS vi 
                                                            on vi.RETAILITEMID = it.ITEMID 
                                                            and vi.DATAAREAID = it.DATAAREAID 
                                                            and vi.VENDORID = @vendorID ");

                    MakeParam(cmd, "vendorID", vendorID);
                    MakeParam(cmdCount, "vendorID", vendorID);
                }
                else
                {
                    commandText = commandText.Replace("<vendortablejoin>", "");
                }

                if (barCode != null && barCode.Trim().Length > 0)
                {
                    barCode = (barCodeBeginsWith ? "" : "%") + barCode + "%";

                    commandText = commandText.Replace("<barcodetablejoin>",
                                                      @"inner join INVENTITEMBARCODE ibarcode 
                                                        on it.DATAAREAID = ibarcode.DATAAREAID 
                                                        and it.ITEMID = ibarcode.ITEMID 
                                                        and ibarcode.ITEMBARCODE like @barCode");

                    MakeParam(cmd, "barCode", barCode);
                    MakeParam(cmdCount, "barCode", barCode);
                }
                else
                {
                    commandText = commandText.Replace("<barcodetablejoin>", "");
                }

                if (specialGroup != null)
                {
                    commandText = commandText.Replace("<specialgroupjoin>",
                                                      @"inner join RBOSPECIALGROUPITEMS sg 
                                                        on sg.ITEMID = it.ITEMID 
                                                        and sg.DATAAREAID = it.DATAAREAID 
                                                        and sg.GROUPID = @specialGroup ");

                    MakeParam(cmd, "specialGroup", specialGroup);
                    MakeParam(cmdCount, "specialGroup", specialGroup);
                }
                else
                {
                    commandText = commandText.Replace("<specialgroupjoin>", "");
                }

                commandText = commandText.Replace("<sort>", sort);
               

                if (idOrDescription != null && idOrDescription.Trim().Length > 0)
                {
                    string searchString = PreProcessSearchText(idOrDescription, true, idOrDescriptionBeginsWith);
                    
                    whereConditions +=
                        " and (it.ITEMNAME Like @description or it.ITEMID Like @description or it.NAMEALIAS Like @description) ";


                    MakeParam(cmd, "description", searchString);
                    MakeParam(cmdCount, "description", searchString);
                }

                if (variantGroupID != null)
                {
                    whereConditions += " and it.DIMGROUPID = @variantGroupID ";

                    MakeParam(cmd, "variantGroupID", variantGroupID);
                    MakeParam(cmdCount, "variantGroupID", variantGroupID);
                }
                commandText = commandText.Replace("<whereConditions>", whereConditions);
                cmd.CommandText = commandText.Replace("<columns>", columns)
                                             .Replace("<rowConfinementAndOrder>",
                                                      @" where 
                                                    ss.ROW between @rowFrom and @rowTo
		                                        ORDER by ss.ROW");
                MakeParam(cmd, "rowFrom", rowFrom);
                MakeParam(cmd, "rowTo", rowTo);

                cmdCount.CommandText = commandText.Replace("<columns>", "Count(1)")
                                                  .Replace("<rowConfinementAndOrder>","")
                                                  .Replace("<RowCount>", "");

                // Do a count first
                if (entry.Connection.DatabaseVersion != ServerVersion.SQLServer2012)
                {
                    cmd.CommandText = cmd.CommandText.Replace("<RowCount>", "");
                    totalRecordsMatching = (int) entry.Connection.ExecuteScalar(cmdCount);
                    return Execute<OldItemListItem>(entry, cmd, CommandType.Text, PopulateAdvancedItemListItem);
                }
                cmd.CommandText = cmd.CommandText.Replace("<RowCount>",
                    ",COUNT(1) OVER ( ORDER BY <sort> RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING ) Row_Count")
                    .Replace("<sort>", sort);
                   
                List<OldItemListItem> result = Execute<OldItemListItem>(entry, cmd, CommandType.Text,
                                                                  PopulateAdvancedItemListItemWithRowCount);
                totalRecordsMatching = result.Count > 0 ? result[0].RowCount : 0;
                return result;
            }
        }

        public List<OldItemListItem> AdvancedSearch(IConnectionManager entry,
                                                        int rowFrom, int rowTo, string sort,
                                                        out int totalRecordsMatching,
                                                        string idOrDescription = null,
                                                        bool idOrDescriptionBeginsWith = true,
                                                        RecordIdentifier retailGroupID = null,
                                                        RecordIdentifier retailDepartmentID = null,
                                                        RecordIdentifier taxGroupID = null,
                                                        RecordIdentifier variantGroupID = null,
                                                        RecordIdentifier vendorID = null,
                                                        string barCode = null,
                                                        bool barCodeBeginsWith = true,
                                                        RecordIdentifier specialGroup = null)
        {
            // DO NOT change this function without also changing the AdvancedSearchIDOnly function so Excel Export will match what was actually searched.

            string whereConditions = "";
            string joins = "";
            bool optimized = (sort == null || sort.StartsWith("ITEMID") || sort.StartsWith("ITEMNAME") ||
                              sort.StartsWith("NAMEALIAS"));
            if (optimized)
            {
                return AdvancedSearchOptimized(entry, rowFrom, rowTo, sort, out totalRecordsMatching, idOrDescription,
                                               idOrDescriptionBeginsWith, retailGroupID, retailDepartmentID, taxGroupID,
                                               variantGroupID, vendorID, barCode, barCodeBeginsWith, specialGroup);
            }
            else
            {

                using (var cmd = entry.Connection.CreateCommand())
                using (var cmdCount = entry.Connection.CreateCommand())
                {

                    if (idOrDescription != null && idOrDescription.Trim().Length > 0)
                    {
                        idOrDescription = PreProcessSearchText(idOrDescription, true, idOrDescriptionBeginsWith);

                        whereConditions +=
                            " and (it.ITEMNAME Like @searchString or it.ITEMID Like @searchString or it.NAMEALIAS Like @searchString) ";

                        MakeParamNoCheck(cmd, "searchString", idOrDescription);
                        MakeParamNoCheck(cmdCount, "searchString", idOrDescription);
                    }

                    if (retailGroupID != null)
                    {
                        whereConditions +=
                            " AND rboit.ITEMGROUP = @retailGroupID ";

                        MakeParamNoCheck(cmd, "retailGroupID", (string)retailGroupID);
                        MakeParamNoCheck(cmdCount, "retailGroupID", (string)retailGroupID);
                    }

                    if (retailDepartmentID != null)
                    {
                        whereConditions +=
                            " AND rboit.ITEMDEPARTMENT = @departmentID ";

                        MakeParamNoCheck(cmd, "departmentID", (string)retailDepartmentID);
                        MakeParamNoCheck(cmdCount, "departmentID", (string)retailDepartmentID);
                    }

                    if (taxGroupID != null)
                    {
                        whereConditions +=
                            " AND itmod.TAXITEMGROUPID = @taxGroupID ";

                        MakeParamNoCheck(cmd, "taxGroupID", (string)taxGroupID);
                        MakeParamNoCheck(cmdCount, "taxGroupID", (string)taxGroupID);
                    }

                    if (variantGroupID != null)
                    {
                        whereConditions += " and it.DIMGROUPID = @variantGroupID ";

                        MakeParamNoCheck(cmd, "variantGroupID", (string) variantGroupID);
                        MakeParamNoCheck(cmdCount, "variantGroupID", (string) variantGroupID);
                    }

                    if (vendorID != null)
                    {
                        whereConditions += " and vi.VENDORID = @vendorID ";

                        joins +=
                            " left outer join VENDORITEMS vi on vi.RETAILITEMID = it.ITEMID and vi.DATAAREAID = it.DATAAREAID and vi.VENDORID = @vendorID ";
                        MakeParamNoCheck(cmd, "vendorID", (string) vendorID);
                        MakeParamNoCheck(cmdCount, "vendorID", (string) vendorID);
                    }

                    if (barCode != null && barCode.Trim().Length > 0)
                    {
                        joins +=
                            " left outer join INVENTITEMBARCODE ibarcode on it.DATAAREAID = ibarcode.DATAAREAID and it.ITEMID = ibarcode.ITEMID";
                        barCode = (barCodeBeginsWith ? "" : "%") + barCode + "%";

                        whereConditions += " and ibarcode.ITEMBARCODE like @barCodeSearchString ";

                        MakeParamNoCheck(cmd, "barCodeSearchString", barCode);
                        MakeParamNoCheck(cmdCount, "barCodeSearchString", barCode);
                    }

                    if (specialGroup != null)
                    {
                        joins +=
                            " left outer join RBOSPECIALGROUPITEMS sg on sg.ITEMID = it.ITEMID and sg.DATAAREAID = it.DATAAREAID and sg.GROUPID = @specialGroupID ";
                        whereConditions += " and sg.GROUPID = @specialGroupID ";

                        MakeParamNoCheck(cmd, "specialGroupID", (string) specialGroup);
                        MakeParamNoCheck(cmdCount, "specialGroupID", (string) specialGroup);
                    }

                    cmd.CommandText = @"select ss.* from(
                        Select s.*, ROW_NUMBER() OVER(order by <sort>) AS ROW from (

                        select distinct it.ITEMID, 
	                        ISNULL(it.ITEMNAME,'') as ITEMNAME, 
	                        ISNULL(it.NAMEALIAS,'') as NAMEALIAS,
	                        ISNULL(it.ITEMTYPE,-1) as ITEMTYPE,
	                        ISNULL(ritretgrp.NAME,'') as RETAILGROUPNAME,
	                        ISNULL(ritdep.NAME,'') as RETAILDEPARTMENTNAME,
	                        ISNULL(theading.NAME,'') as TAXGROUPNAME,
                            ISNULL(itmod.PRICEINCLTAX,0) as PRICEINCLTAX
                        from INVENTTABLE it 
                        join RBOINVENTTABLE rboit on it.DATAAREAID = rboit.DATAAREAID and it.ITEMID = rboit.ITEMID 
                        left outer join RBOINVENTITEMRETAILGROUP ritretgrp on it.DATAAREAID = ritretgrp.DATAAREAID and rboit.ITEMGROUP = ritretgrp.GROUPID
                        left outer join RBOINVENTITEMDEPARTMENT ritdep on it.DATAAREAID = ritdep.DATAAREAID and rboit.ITEMDEPARTMENT = ritdep.DEPARTMENTID
                        left outer join INVENTTABLEMODULE itmod on it.DATAAREAID = itmod.DATAAREAID and it.ITEMID = itmod.ITEMID and itmod.MODULETYPE = 2
                        left outer join TAXITEMGROUPHEADING theading on it.DATAAREAID = theading.DATAAREAID and itmod.TAXITEMGROUPID = theading.TAXITEMGROUP
                        
                                             
                        <joins>
                        where it.DATAAREAID = @dataAreaId <whereConditions>
                        ) s
                        ) ss
                        where ss.ROW between @rowFrom and @rowTo";


                    cmd.CommandText = cmd.CommandText.Replace("<sort>", sort);
                    cmd.CommandText = cmd.CommandText.Replace("<joins>", joins);
                    cmd.CommandText = cmd.CommandText.Replace("<whereConditions>", whereConditions);

                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                    MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
                    MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);

                    // Do a count first
                    cmdCount.CommandText = @"select count(*) from INVENTTABLE it 
                                            join RBOINVENTTABLE rboit on it.DATAAREAID = rboit.DATAAREAID and it.ITEMID = rboit.ITEMID 
                                            left outer join RBOINVENTITEMRETAILGROUP ritretgrp on it.DATAAREAID = ritretgrp.DATAAREAID and rboit.ITEMGROUP = ritretgrp.GROUPID
                                            left outer join RBOINVENTITEMDEPARTMENT ritdep on it.DATAAREAID = ritdep.DATAAREAID and rboit.ITEMDEPARTMENT = ritdep.DEPARTMENTID
                                            left outer join INVENTTABLEMODULE itmod on it.DATAAREAID = itmod.DATAAREAID and it.ITEMID = itmod.ITEMID and itmod.MODULETYPE = 2
                                            left outer join TAXITEMGROUPHEADING theading on it.DATAAREAID = theading.DATAAREAID and itmod.TAXITEMGROUPID = theading.TAXITEMGROUP " + 
                                            joins +
                                            " where it.DATAAREAID = @dataAreaId " +
                                            whereConditions;
                    MakeParam(cmdCount, "dataAreaId", entry.Connection.DataAreaId);
                    totalRecordsMatching = (int) entry.Connection.ExecuteScalar(cmdCount);

                    return Execute<OldItemListItem>(entry, cmd, CommandType.Text, PopulateAdvancedItemListItem);

                }
            }
        }

        /// <summary>
        /// Searched for retail items that contain a given search text, and returns the results as a List of <see cref="ItemListItem"/> . 
        /// Additionally this search function accepts a Dictionary with a combination of <see cref="DataLayer.BusinessObjects.ItemMaster.RetailItemSearchEnum"/> and a string and resolves this list to add additional 
        /// search filters to the query
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The item id or item name to search for</param>
        /// <param name="rowFrom"></param>
        /// <param name="rowTo"></param>
        /// <param name="beginsWith">Indicates wether you only want to search for items that begin with the given item name or item id</param>
        /// <param name="sort">The name of the column you want to sort by</param>
        /// <param name="advancedSearchParameters">Additional search parameters</param>
        /// <returns></returns>
        public virtual List<OldItemListItem> AdvancedSearch(IConnectionManager entry, string searchString, int rowFrom, int rowTo, bool beginsWith, string sort, Dictionary<OldRetailItemSearchEnum, string> advancedSearchParameters)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                string modifiedSearchString = PreProcessSearchText(searchString, true, beginsWith);

                string modifiedAdvancedSearchString = AdvancedSearchString;

                // Add the sort column
                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<orderBy>", sort);

                cmd.CommandText = modifiedAdvancedSearchString;

                // <retailGroupSelect>
                cmd.CommandText =
                    cmd.CommandText.Replace("<retailGroupSelect>",
                                            @"ISNULL(rboit.ITEMGROUP,'') as RETAILGROUP,
                                                          ISNULL(ritretgrp.NAME,'') as RETAILGROUPNAME,");

                // <retailGroupJoin>
                cmd.CommandText =
                    cmd.CommandText.Replace("<retailGroupJoin>",
                                            @"left outer join RBOINVENTITEMRETAILGROUP ritretgrp on it.DATAAREAID = ritretgrp.DATAAREAID and rboit.ITEMGROUP = ritretgrp.GROUPID");


                // <retailDepartmentSelect>
                cmd.CommandText =
                    cmd.CommandText.Replace("<retailDepartmentSelect>",
                                            @"ISNULL(rboit.ITEMDEPARTMENT,'') as RETAILDEPARTMENT,
                                                          ISNULL(ritdep.NAME,'') as RETAILDEPARTMENTNAME,");

                // <retailDepartmentJoin>
                cmd.CommandText =
                   cmd.CommandText.Replace("<retailDepartmentJoin>",
                                           @"left outer join RBOINVENTITEMDEPARTMENT ritdep on it.DATAAREAID = ritdep.DATAAREAID and rboit.ITEMDEPARTMENT = ritdep.DEPARTMENTID");


                // <taxGroupSelect>
                cmd.CommandText =
                    cmd.CommandText.Replace("<taxGroupSelect>",
                                            @"ISNULL(itmod.TAXITEMGROUPID, '') as TAXGROUP,
                                                          ISNULL(theading.NAME,'') as TAXGROUPNAME,");

                // <taxGroupJoin>
                // Moduletype is set to 2 = sales
                cmd.CommandText =
                  cmd.CommandText.Replace("<taxGroupJoin>",
                                          @"left outer join TAXITEMGROUPHEADING theading on it.DATAAREAID = theading.DATAAREAID and itmod.TAXITEMGROUPID = theading.TAXITEMGROUP");


                // Go through the advanced search conditions
                foreach (var pair in advancedSearchParameters)
                {
                    switch (pair.Key)
                    {
                        case OldRetailItemSearchEnum.RetailGroup:

                            // <retailGroupCondition>
                            cmd.CommandText =
                                cmd.CommandText.Replace("<retailGroupCondition>",
                                                        @"and (rboit.ITEMGROUP like @retailGroupSearchString or ritretgrp.NAME like @retailGroupSearchString)");

                            MakeParam(cmd, "retailGroupSearchString", pair.Value);
                            break;

                        case OldRetailItemSearchEnum.RetailDepartment:

                            // <retailDepartmentCondition>
                            cmd.CommandText =
                                cmd.CommandText.Replace("<retailDepartmentCondition>",
                                                        @"and (rboit.ITEMDEPARTMENT like @retailDepartmentSearchString or ritdep.NAME like @retailDepartmentSearchString)");

                            MakeParam(cmd, "retailDepartmentSearchString", pair.Value);
                            break;

                        case OldRetailItemSearchEnum.TaxGroup:

                            // <taxGroupCondition>
                            cmd.CommandText =
                               cmd.CommandText.Replace("<taxGroupCondition>",
                                                       @"and (itmod.TAXITEMGROUPID like @taxGroupSearchString or theading.NAME like @taxGroupSearchString)");

                            MakeParam(cmd, "taxGroupSearchString", pair.Value);
                            break;

                        case OldRetailItemSearchEnum.VariantGroup:
                            // <variantGroupSelect>
                            cmd.CommandText =
                                cmd.CommandText.Replace("<variantGroupSelect>",
                                                        @"ISNULL(idimgrp.NAME,'') as VARIANTNAME,");

                            // <variantGroupJoin>
                            cmd.CommandText =
                              cmd.CommandText.Replace("<variantGroupJoin>",
                                                      @"left outer join INVENTDIMGROUP idimgrp on it.DATAAREAID = idimgrp.DATAAREAID and it.DIMGROUPID = idimgrp.DIMGROUPID");


                            // <variantGroupCondition>
                            cmd.CommandText =
                               cmd.CommandText.Replace("<variantGroupCondition>",
                                                       @"and (it.DIMGROUPID like @variantGroupSearchString or idimgrp.NAME like @variantGroupSearchString)");

                            MakeParam(cmd, "variantGroupSearchString", pair.Value);
                            break;

                        case OldRetailItemSearchEnum.Vendor:
                            // <vendorSelect>
                            cmd.CommandText =
                                cmd.CommandText.Replace("<vendorSelect>",
                                                        @"ISNULL (vendtbl.ACCOUNTNUM, '') as VENDORID,
                                                          ISNULL (vendtbl.NAME, '') as VENDORNAME,");

                            // <vendorJoin>
                            cmd.CommandText =
                              cmd.CommandText.Replace("<vendorJoin>",
                                                      @"left outer join VENDTABLE vendtbl on it.DATAAREAID = vendtbl.DATAAREAID and it.PRIMARYVENDORID = vendtbl.ACCOUNTNUM");


                            // <vendorCondition>
                            cmd.CommandText =
                               cmd.CommandText.Replace("<vendorCondition>",
                                                       @"and (vendtbl.ACCOUNTNUM like @vendorSearchString or vendtbl.NAME like @vendorSearchString)");

                            MakeParam(cmd, "vendorSearchString", pair.Value);
                            break;

                        case OldRetailItemSearchEnum.BarCode:
                            // <barCodeSelect>
                            cmd.CommandText =
                                cmd.CommandText.Replace("<barCodeSelect>",
                                                        @"ISNULL(ibarcode.ITEMBARCODE, '') as ITEMBARCODE,");

                            // <barCodeJoin>
                            cmd.CommandText =
                              cmd.CommandText.Replace("<barCodeJoin>",
                                                      @"left outer join INVENTITEMBARCODE ibarcode on it.DATAAREAID = ibarcode.DATAAREAID and it.ITEMID = ibarcode.ITEMID");


                            // <barCodeCondition>
                            cmd.CommandText =
                               cmd.CommandText.Replace("<barCodeCondition>",
                                                       @"and (ibarcode.ITEMBARCODE like @barCodeSearchString)");

                            MakeParam(cmd, "barCodeSearchString", pair.Value);
                            break;

                        case OldRetailItemSearchEnum.SpecialGroup:
                            // <specialGroupJoin>
                            cmd.CommandText =
                              cmd.CommandText.Replace("<specialGroupJoin>",
                                                      @"left outer join RBOSPECIALGROUPITEMS rbospecit on it.DATAAREAID = rbospecit.DATAAREAID and it.ITEMID = rbospecit.ITEMID
                                                        left outer join RBOSPECIALGROUP rbospec on it.DATAAREAID = rbospec.DATAAREAID and rbospecit.GROUPID = rbospec.GROUPID");


                            // <specialGroupCondition>
                            cmd.CommandText =
                               cmd.CommandText.Replace("<specialGroupCondition>",
                                                       @"and (rbospec.GROUPID like @specialGroupSearchString or rbospec.NAME like @specialGroupSearchString)");

                            MakeParam(cmd, "specialGroupSearchString", pair.Value);
                            break;
                    }
                }

                // Clean up remaining search tags (i.e <retailGroupSelect> etc) that were not used
                modifiedAdvancedSearchString = cmd.CommandText;

                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<retailGroupSelect>", "");
                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<retailGroupJoin>", "");
                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<retailGroupCondition>", "");

                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<retailDepartmentSelect>", "");
                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<retailDepartmentJoin>", "");
                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<retailDepartmentCondition>", "");

                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<taxGroupSelect>", "");
                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<taxGroupJoin>", "");
                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<taxGroupCondition>", "");

                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<variantGroupSelect>", "");
                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<variantGroupJoin>", "");
                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<variantGroupCondition>", "");

                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<vendorSelect>", "");
                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<vendorJoin>", "");
                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<vendorCondition>", "");

                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<barCodeSelect>", "");
                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<barCodeJoin>", "");
                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<barCodeCondition>", "");

                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<specialGroupJoin>", "");
                modifiedAdvancedSearchString = modifiedAdvancedSearchString.Replace("<specialGroupCondition>", "");

                // Remove the trailing comma character before the FROM clause
                const string commaFromPattern = @",\s+from";
                var rgx = new Regex(commaFromPattern);

                if (rgx.IsMatch(modifiedAdvancedSearchString))
                {
                    modifiedAdvancedSearchString = rgx.Replace(modifiedAdvancedSearchString, " from");
                }

                cmd.CommandText = modifiedAdvancedSearchString;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "searchString", modifiedSearchString);
                MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
                MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);

                return Execute<OldItemListItem>(entry, cmd, CommandType.Text, PopulateAdvancedItemListItem);
            }
        }

        public virtual List<DataEntity> GetForecourtItems(IConnectionManager entry, RecordIdentifier gradeID)
        {
            ValidateSecurity(entry);
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"Select R.[ITEMID], I.[ITEMNAME] 
                                    FROM RBOInventTable R,InventTable I
                                    WHERE R.GRADEID=@gradeID AND 
                                    R.ItemId=I.ItemId AND R.DATAAREAID=@dataAreaID AND I.DATAAREAID=@dataAreaID";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "gradeID", gradeID);
                return Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMNAME", "ITEMID");
            }
        }

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see cref="ItemListItem" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="culture">Language code of the included item translations</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        public virtual List<DataEntity> Search(IConnectionManager entry, string searchString, int rowFrom, int rowTo, string culture, bool beginsWith)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                //ValidateSecurity(entry, BusinessObjects.Permission.CustomerView);

                string modifiedSearchString = PreProcessSearchText(searchString, true, beginsWith);

                cmd.CommandText =
                    "Select s.* from (" +
                    "select it.ITEMID, ISNULL(it.ITEMNAME,'') as ITEMNAME, ISNULL(it.NAMEALIAS,'') as NAMEALIAS, " +
                    "ROW_NUMBER() OVER(order by ITEMNAME) AS ROW " +
                    "from INVENTTABLE it ";
                if (culture != null && culture != "")
                {
                    cmd.CommandText +="left outer join RBOINVENTTRANSLATIONS tr ON tr.ITEMID = it.ITEMID and tr.DATAAREAID = it.DATAAREAID and tr.CULTURENAME = @culture ";
                    MakeParam(cmd, "culture", culture);
                }
                cmd.CommandText += " where it.DATAAREAID = @dataAreaId ";

                cmd.CommandText += " and (it.ITEMNAME Like @searchString or it.ITEMID Like @searchString or it.NAMEALIAS Like @searchString";
                if (culture != null && culture != "")
                {
                    cmd.CommandText += " or tr.DESCRIPTION like @searchString";
                }

                cmd.CommandText += ")) s where s.ROW between " + rowFrom + " and " + rowTo;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "searchString", modifiedSearchString);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMNAME", "ITEMID");
            }
        }
       
        /// <summary>
        /// Gets a retail item from the database by a given ID.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The id of the retail item to fetch</param>
        /// <param name="cacheType">Cache</param>
        /// <returns>The retail item, or null if not found</returns>
        public virtual OldRetailItem Get(IConnectionManager entry, RecordIdentifier itemID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            OldRetailItem result;
            if (cacheType != CacheType.CacheTypeNone)
            {
                result = (OldRetailItem)entry.Cache.GetEntityFromCache(typeof(OldRetailItem), itemID);
                if (result != null)
                {
                    return result;
                }
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = AddCustomHandling(
                    BaseGetString
                    + "where a.DATAAREAID = @dataAreaId and a.ITEMID = @itemID " 
                    + CustomConditions);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", (string)itemID);

                var records = Execute<OldRetailItem>(entry, cmd, CommandType.Text, PopulateItem);

                result = (records.Count > 0) ? records[0] : null;
            }

            if (result != null)
            {
                using (var cmd = entry.Connection.CreateCommand())
                {
                    cmd.CommandText = BaseGetModuleString + "where iu.DATAAREAID = @dataAreaId and iu.ITEMID = @itemID";

                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                    MakeParam(cmd, "itemID", (string) itemID);
 
                    var moduleRecords = Execute<OldRetailItem.RetailItemModule>(entry, cmd, CommandType.Text, PopulateModule);

                    OldRetailItem.RetailItemModule inventModule = null;
                    bool purchaseUnitEmpty = false;
                    foreach (var module in moduleRecords)
                    {
                        if (module.ModuleType == OldRetailItem.ModuleTypeEnum.Purchase)
                            purchaseUnitEmpty = (module.Unit.IsEmpty || string.IsNullOrEmpty((string) module.Unit));
                        else if (module.ModuleType == OldRetailItem.ModuleTypeEnum.Inventory)
                            inventModule = module;
                        result.AddModule(module);
                    }

                    // Make sure we have a valid purchase module 
                    if (purchaseUnitEmpty && inventModule != null)
                    {
                        var purchaseModule = inventModule.Clone() as OldRetailItem.RetailItemModule;
                        purchaseModule.ModuleType = OldRetailItem.ModuleTypeEnum.Purchase;
                        purchaseModule.ID = new RecordIdentifier("", (int)purchaseModule.ModuleType);
                        result.AddModule(purchaseModule);
                    }
                }
            }

            if (result != null && cacheType != CacheType.CacheTypeNone)
            {
                entry.Cache.AddEntityToCache(itemID, result, cacheType);
            }

            return result;
        }

        /// <summary>
        /// Gets retail items in the system for a specific ID using a LIKE query
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">Item ID</param>
        /// <returns>The retail item, or null if not found</returns>
        public virtual List<OldRetailItem> GetItemsByItemPattern(IConnectionManager entry, string itemID)
        {
            List<OldRetailItem> records;

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = AddCustomHandling(
                    BaseGetString 
                    + "where a.DATAAREAID = @dataAreaId and a.ITEMID LIKE @itemId "
                    + CustomConditions);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemId", itemID + "%");

                records = Execute<OldRetailItem>(entry, cmd, CommandType.Text, PopulateItem);
            }

            if (records.Count > 0)
            {
                foreach (var item in records)
                {
                    using (var cmd = entry.Connection.CreateCommand())
                    {

                        cmd.CommandText = BaseGetModuleString + "where iu.DATAAREAID = @dataAreaId and iu.ITEMID = @itemID";

                        MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                        MakeParam(cmd, "itemID", (string)item.ID);

                        var moduleRecords = Execute<OldRetailItem.RetailItemModule>(entry, cmd, CommandType.Text, PopulateModule);

                        foreach (var module in moduleRecords)
                        {
                            item.AddModule(module);
                        }
                    }

                }
            }

            return records;
        }


        /// <summary>
        /// Gets all retail items in the system
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>The retail item, or null if not found</returns>
        public virtual List<OldRetailItem> GetAllItems(IConnectionManager entry)
        {
            List<OldRetailItem> records;

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandTimeout = 360000;
                ValidateSecurity(entry);

                cmd.CommandText = AddCustomHandling(
                    BaseGetString 
                    + "where a.DATAAREAID = @dataAreaId " 
                    + CustomConditions);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                records = Execute<OldRetailItem>(entry, cmd, CommandType.Text, PopulateItem);
            }

            if (records.Count > 0)
            {
                foreach (var item in records)
                {
                    using (var cmd = entry.Connection.CreateCommand())
                    {
                    
                        cmd.CommandText = BaseGetModuleString + "where iu.DATAAREAID = @dataAreaId and iu.ITEMID = @itemID";

                        MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                        MakeParam(cmd, "itemID", (string)item.ID);

                        var moduleRecords = Execute<OldRetailItem.RetailItemModule>(entry, cmd, CommandType.Text, PopulateModule);

                        foreach (var module in moduleRecords)
                        {
                            item.AddModule(module);
                        }
                    }
                    
                }
            }

            return records;
        }


        public bool ItemCanBeDeleted(IConnectionManager entry, RecordIdentifier id)
        {
            if (
                RecordExists(entry, "PURCHASEORDERLINE", "RETAILITEMID", id) ||
                RecordExists(entry, "PURCHASEORDERLINE", "VENDORITEMID", id) ||
                RecordExists(entry, "RBOTRANSACTIONFUELTRANS", "ITEMID", id) ||
                RecordExists(entry, "RBOTRANSACTIONSALESTRANS", "ITEMID", id) 

                )
            {
                return false;
            }
            return true;

        }

        /// <summary>
        /// Deletes a retail item by a given ID.
        /// </summary>
        /// <remarks>Edit retail items permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">The ID of the retail item to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            if (ItemCanBeDeleted(entry, id))
            {
                DeleteItem(entry, id);
            }
            else
            {
                var item = Get(entry, id);
                item.DateToBeBlocked = new Date( DateTime.Now);
                item.Dirty = true;
                Save(entry,item);
            }
        }

        private void DeleteItem(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "INVENTTABLE", "ITEMID", id, DataLayer.BusinessObjects.Permission.ItemsEdit);
            DeleteRecord(entry, "RBOINVENTTABLE", "ITEMID", id, DataLayer.BusinessObjects.Permission.ItemsEdit);
            DeleteRecord(entry, "INVENTTABLEMODULE", "ITEMID", id, DataLayer.BusinessObjects.Permission.ItemsEdit);
            DeleteRecord(entry, "INVENTDIMCOMBINATION", "ITEMID", id, DataLayer.BusinessObjects.Permission.ItemsEdit);
            DeleteRecord(entry, "INVENTITEMBARCODE", "ITEMID", id, DataLayer.BusinessObjects.Permission.ItemsEdit);
            DeleteRecord(entry, "INVENTITEMBARCODE", "ITEMID", id, DataLayer.BusinessObjects.Permission.ItemsEdit);
            DeleteRecord(entry, "VENDORITEMS", "RETAILITEMID", id, "");
            DeleteRecord(entry, "RBOSPECIALGROUPITEMS", "ITEMID", id, DataLayer.BusinessObjects.Permission.ItemsEdit);

            var secondaryIDBackup = id.SecondaryID;
            id.SecondaryID = new RecordIdentifier(1);
            DeleteRecord(entry, "RBOINFOCODETABLESPECIFIC", new[] { "REFRELATION", "REFTABLEID" }, id, DataLayer.BusinessObjects.Permission.ItemsEdit);
            id.SecondaryID = secondaryIDBackup;
        }

        public virtual int ItemCount(IConnectionManager entry)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"select count(1) from INVENTTABLE";

                return (int)entry.Connection.ExecuteScalar(cmd);
            }
        }


        

        /// <summary>
        ///  Checks if a retail item with a given ID exists in the database.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The ID of the retail item to check for</param>
        /// <returns></returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier itemID)
        {
            return RecordExists(entry, "INVENTTABLE", "ITEMID", itemID);
        }

        /// <summary>
        /// Checks if any item is using a given tax group id.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxgroupID">ID of the tax group</param>
        /// <returns>True if any item uses the tax group, else false</returns>
        public virtual bool TaxGroupExists(IConnectionManager entry, RecordIdentifier taxgroupID)
        {
            return RecordExists(entry, "INVENTTABLEMODULE", new[]{"MODULETYPE","TAXITEMGROUPID"}, new RecordIdentifier(2, taxgroupID));
        }

        private static bool ItemRecordExists(IConnectionManager entry, RecordIdentifier itemID)
        {
            return RecordExists(entry, "RBOINVENTTABLE", "ITEMID", itemID);
        }

        private static bool InventTableModuleExists(IConnectionManager entry, RecordIdentifier itemID,int moduleID)
        {
            return RecordExists(entry, "INVENTTABLEMODULE", new[] { "ITEMID", "MODULETYPE" }, new RecordIdentifier(itemID, moduleID));
        }

        /// <summary>
        /// Saves the retail item and its related module records, it only saves those that are in need of saving.
        /// (Thus master record or sub record that has the Dirty property as true get saved)
        /// </summary>
        /// <remarks>Edit retail items permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="item">The retail item to save</param>
        public virtual void Save(IConnectionManager entry, OldRetailItem item)
        {
            var statement = new SqlServerStatement("INVENTTABLE");
            
            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ItemsEdit);

            bool isNew = false;
            if (item.Dirty)
            {
                if (item.ID == RecordIdentifier.Empty)
                {
                    isNew = true;
                    item.ID = DataProviderFactory.Instance.GenerateNumber<IOldRetailItemData, OldRetailItem>(entry); 
                }

                if (isNew || !Exists(entry, item.ID))
                {
                    statement.StatementType = StatementType.Insert;

                    statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                    statement.AddKey("ITEMID", (string)item.ID);
                }
                else
                {
                    statement.StatementType = StatementType.Update;

                    statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                    statement.AddCondition("ITEMID", (string)item.ID);
                }

                statement.AddField("ITEMNAME", item.Text);
                statement.AddField("DIMGROUPID", (string)item.DimensionGroupID);
                statement.AddField("NAMEALIAS", item.NameAlias);
                statement.AddField("NOTES", item.Notes);
                statement.AddField("ITEMTYPE", (int)item.ItemType, SqlDbType.Int);
                statement.AddField("PRIMARYVENDORID", (string)item.DefaultVendorItemId);

                SaveCustomA(entry, statement, item);

                entry.Connection.ExecuteStatement(statement);

                // From RBOINVENTTABLE

                statement = new SqlServerStatement("RBOINVENTTABLE");

                if (!ItemRecordExists(entry, item.ID))
                {
                    statement.StatementType = StatementType.Insert;

                    statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                    statement.AddKey("ITEMID", (string)item.ID);
                }
                else
                {
                    statement.StatementType = StatementType.Update;

                    statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                    statement.AddCondition("ITEMID", (string)item.ID);
                }

                statement.AddField("DATETOBEBLOCKED", item.DateToBeBlocked.ToAxaptaSQLDate(), SqlDbType.DateTime);
                statement.AddField("ITEMTYPE", (int)item.RetailItemType, SqlDbType.Int);
                statement.AddField("BARCODESETUPID", (string)item.BarCodeSetupID);
                statement.AddField("SCALEITEM", item.ScaleItem ? 1 : 0, SqlDbType.TinyInt);
                statement.AddField("KEYINGINPRICE", (int)item.KeyInPrice, SqlDbType.Int);
                statement.AddField("KEYINGINQTY", (int)item.KeyInQuantity, SqlDbType.Int);
                statement.AddField("MUSTKEYINCOMMENT", item.MustKeyInComment ? 1 : 0, SqlDbType.TinyInt);
                statement.AddField("MUSTSELECTUOM", item.MustSelectUOM ? 1 : 0, SqlDbType.TinyInt);
                statement.AddField("ZEROPRICEVALID", item.ZeroPriceValid ? 1 : 0, SqlDbType.TinyInt);
                statement.AddField("QTYBECOMESNEGATIVE", item.QuantityBecomesNegative ? 1 : 0, SqlDbType.TinyInt);
                statement.AddField("DATETOACTIVATEITEM", item.DateToActivateItem.ToAxaptaSQLDate(), SqlDbType.DateTime);
                statement.AddField("FUELITEM", item.IsFuelItem ? 1 : 0, SqlDbType.TinyInt);
                statement.AddField("GRADEID", item.GradeID);
                statement.AddField("SIZEGROUP", (string)item.SizeGroupID);
                statement.AddField("COLORGROUP", (string)item.ColorGroupID);
                statement.AddField("STYLEGROUP", (string)item.StyleGroupID);
                statement.AddField("PRINTVARIANTSSHELFLABELS", item.PrintVariantsShelfLabels ? 1 : 0, SqlDbType.TinyInt);
                statement.AddField("ITEMDEPARTMENT", (string)item.RetailDepartmentID);
                statement.AddField("DIVISIONID", (string)item.RetailDivisionID);
                statement.AddField("ITEMGROUP", (string)item.RetailGroupID);
                statement.AddField("NODISCOUNTALLOWED", item.NoDiscountAllowed ? 1 : 0, SqlDbType.TinyInt);
                statement.AddField("DEFAULTPROFIT", item.ProfitMargin, SqlDbType.Decimal);

                statement.AddField("POSPERIODICID", item.ValidationPeriod);

                SaveCustomB(entry, statement, item);

                entry.Connection.ExecuteStatement(statement);
            }

            item.Dirty = false;

            for (int i = 0; i < 3; i++)
            {
                var module = item[(OldRetailItem.ModuleTypeEnum)i];

                if (module.Dirty)
                {
                    statement = new SqlServerStatement("INVENTTABLEMODULE");

                    if (!InventTableModuleExists(entry, item.ID,i))
                    {
                        statement.StatementType = StatementType.Insert;

                        statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                        statement.AddKey("ITEMID", (string)item.ID);
                        statement.AddKey("MODULETYPE", i, SqlDbType.Int);
                    }
                    else
                    {
                        statement.StatementType = StatementType.Update;

                        statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                        statement.AddCondition("ITEMID", (string)item.ID);
                        statement.AddCondition("MODULETYPE", i, SqlDbType.Int);
                    }

                    statement.AddField("UNITID", (string)module.Unit);
                    statement.AddField("PRICE", module.Price, SqlDbType.Decimal);
                    statement.AddField("PRICEUNIT", module.PriceUnit, SqlDbType.Decimal);
                    statement.AddField("MARKUP", module.Markup, SqlDbType.Decimal);
                    statement.AddField("LINEDISC", (string)module.LineDiscount);
                    statement.AddField("MULTILINEDISC", (string)module.MultilineDiscount);
                    statement.AddField("ENDDISC", module.TotalDiscount ? 1 : 0, SqlDbType.TinyInt);
                    statement.AddField("PRICEDATE", module.PriceDate.ToAxaptaSQLDate(), SqlDbType.DateTime);
                    statement.AddField("PRICEQTY", module.PriceQty, SqlDbType.Decimal);
                    statement.AddField("ALLOCATEMARKUP", module.AllocateMarkup ? 1 : 0, SqlDbType.TinyInt);
                    statement.AddField("TAXITEMGROUPID", (string)module.TaxItemGroupID);
                    statement.AddField("PRICEINCLTAX", module.LastKnownPriceWithTax, SqlDbType.Decimal);

                    SaveCustomModule(entry, statement, module);

                    entry.Connection.ExecuteStatement(statement);
                }

                module.Dirty = false;
            }
        }

        /// <summary>
        /// Gets an items item sales tax group. Returns an empty record identifier if item has no item sales tax group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemId">The items ID</param>
        /// <returns>Items item sales tax group</returns>
        public virtual RecordIdentifier GetItemsItemSalesTaxGroupID(IConnectionManager entry, RecordIdentifier itemId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select ISNULL(TAXITEMGROUPID,'') as TAXITEMGROUPID " +
                    "from INVENTTABLEMODULE " +
                    "where MODULETYPE = 2 and ITEMID = @itemId and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemId", (string)itemId);

                RecordIdentifier result = "";
                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.Read())
                    {
                        result = (string)dr["TAXITEMGROUPID"];
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the latest purchase price of an item
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        /// <param name="itemID">The item who's purchase price we want</param>
        /// <returns>The latest purchase price of an item</returns>
        public virtual decimal GetLatestPurchasePrice(IConnectionManager entry, RecordIdentifier itemID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select PRICE from INVENTTABLEMODULE " +
                    "where ITEMID = @itemID and MODULETYPE = 1 and DATAAREAID = @dataAreaID";

                MakeParam(cmd, "itemID", (string)itemID);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return (decimal)entry.Connection.ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// Returns the default vendor for a given item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <returns>
        /// Returns the vendor ID
        /// </returns>
        public virtual string GetItemsDefaultVendor(IConnectionManager entry, RecordIdentifier itemID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select PRIMARYVENDORID from INVENTTABLE " +
                    "where ITEMID = @itemID and DATAAREAID = @dataAreaID";

                MakeParam(cmd, "itemID", (string)itemID);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return (string)entry.Connection.ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// Returns true if the item has a default vendor
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <returns>
        /// Returns true if the item has a default vendor
        /// </returns>
        public virtual bool ItemHasDefaultVendor(IConnectionManager entry, RecordIdentifier itemID)
        {
            var defaultVendor = GetItemsDefaultVendor(entry, itemID);

            return (defaultVendor != "");
        }

        public virtual bool ItemHasDimentionGroup(IConnectionManager entry, OldItemListItem itemID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"SELECT COLORGROUP + SIZEGROUP + STYLEGROUP 
                                  from RBOINVENTTABLE
                                  where ((Colorgroup <> '' and colorgroup is not null)
                                  or    (sizegroup <> '' and sizegroup is not null)
                                  or    (stylegroup <> '' and stylegroup is not null))
                                  and itemid = @itemID";

                MakeParam(cmd, "itemID", (string)itemID.ID);
                string dimension = (string)entry.Connection.ExecuteScalar(cmd);
                return (!string.IsNullOrEmpty(dimension));
            }
        }

        //public virtual bool ItemHasDimentionGroup(IConnectionManager entry, ItemListItem itemID)
        //{
        //    var hasDimentionGroup = GetItemDimentionGroup(entry, itemID);

        //    return (hasDimentionGroup != "");
        //}

        /// <summary>
        /// Sets a vendor as a default on a given item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <param name="vendorItemID">The unique ID of the vendor item</param>
        public virtual void SetItemsDefaultVendor(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier vendorItemID)
        {
            var statement = new SqlServerStatement("INVENTTABLE");

            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.CurrencyEdit);

            if (Exists(entry, itemID))
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ITEMID", (string)itemID);

                statement.AddField("PRIMARYVENDORID", (string)vendorItemID);

                entry.Connection.ExecuteStatement(statement);
            }

        }

        public virtual OldRetailItem.RetailItemModule GetPriceModule(IConnectionManager entry, RecordIdentifier itemID)
        {
            using(var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseGetModuleString + @" WHERE iu.DATAAREAID = @dataAreaID AND iu.ITEMID = @itemID AND iu.MODULETYPE = 2";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", itemID);
                
                var list = Execute<OldRetailItem.RetailItemModule>(entry, cmd, CommandType.Text, PopulateModule);
                return list.Count > 0 ? list[0] : null;
            }
        }

        /// <summary>
        /// Returns item ids for all items that have a tax group with the given tax code ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxCodeID">ID of the tax code</param>
        public virtual List<RecordIdentifier> GetItemIDsFromTaxCode(IConnectionManager entry, RecordIdentifier taxCodeID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"
                                    SELECT itm.ITEMID 
                                    FROM INVENTTABLE it
                                    JOIN INVENTTABLEMODULE itm on it.ITEMID = itm.ITEMID and it.DATAAREAID = itm.DATAAREAID
                                    JOIN TAXONITEM toi on itm.TAXITEMGROUPID = toi.TAXITEMGROUP and itm.DATAAREAID = toi.DATAAREAID
                                    Where toi.TAXCODE = @taxCodeID 
                                    AND itm.MODULETYPE = 2
                                    AND it.DATAAREAID = @dataAreaID";


                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "taxCodeID", (string)taxCodeID);

                var dataEntities = Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMID", "ITEMID");
                return (from x in dataEntities
                        select x.ID).ToList();
            }
        }

        public virtual List<RecordIdentifier> GetItemIDsFromTaxGroup(IConnectionManager entry, RecordIdentifier itemSalesTaxGroupID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                                @"SELECT itm.ITEMID 
                                FROM INVENTTABLEMODULE itm
                                Where itm.TAXITEMGROUPID = @itemSalesTaxGroupID 
                                AND itm.MODULETYPE = 2
                                AND itm.DATAAREAID = @dataAreaID";


                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemSalesTaxGroupID", (string)itemSalesTaxGroupID);

                var dataEntities = Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMID", "ITEMID");
                return (from x in dataEntities
                        select x.ID).ToList();
            }
        }

        public virtual List<RecordIdentifier> GetItemIDsOfItemsWithTaxGroup(IConnectionManager entry)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                                @"SELECT itm.ITEMID 
                                FROM INVENTTABLEMODULE itm
                                inner join INVENTTABLE it on it.ITEMID = itm.ITEMID and it.DATAAREAID = itm.DATAAREAID
                                Where itm.TAXITEMGROUPID <> '' 
                                AND itm.MODULETYPE = 2
                                AND itm.DATAAREAID = @dataAreaID";


                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                var dataEntities = Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMID", "ITEMID");
                return (from x in dataEntities
                        select x.ID).ToList();
            }
        }

        public virtual int Count(IConnectionManager entry)
        {
            return Count(entry, "RBOINVENTTABLE");
        }

        public virtual List<DataEntity> FindItem(IConnectionManager entry, string searchText)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select ITEMID, ITEMNAME from INVENTTABLE " +
                                  "where (ITEMID Like @itemID or ITEMNAME Like @itemName) and DATAAREAID = @dataAreaId order by ITEMNAME";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", "%" + searchText + "%");
                MakeParam(cmd, "itemName", "%" + searchText + "%");

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMNAME", "ITEMID");
            }
        }
         
        public virtual List<DataEntity> FindItemDepartment(IConnectionManager entry, string searchText)
        {
            ValidateSecurity(entry);

            // Stupility here since sometimes in the database you have items with group even if the groups do not exist
            // so we need to both look in the groups and in the items
            // Down the road when we have ensured that groups always exist then this should be refactured

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select distinct k.* from " +
                                  "(Select ITEMGROUPID,ISNULL(Name,'') as Name " +
                                  "from INVENTITEMGROUP " +
                                  "where (ITEMGROUPID Like @itemGroupID or Name Like @Name) and DATAAREAID = @dataAreaId " +
                                  "union " +
                                  "Select i.ITEMGROUPID,ISNULL(ig.Name,'') as Name " +
                                  "from INVENTTABLE i " +
                                  "left outer join INVENTITEMGROUP ig on i.DATAAREAID = ig.DATAAREAID and i.ITEMGROUPID = ig.ITEMGROUPID " +
                                  "where (i.ITEMGROUPID Like @itemGroupID or ig.Name Like @name) and i.DATAAREAID = @dataAreaId) k " +
                                  "order by ITEMGROUPID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemGroupID", "%" + searchText + "%");
                MakeParam(cmd, "name", "%" + searchText + "%");

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "Name", "ITEMGROUPID");
            }
        }

        public virtual void ReAssignItemDepartmentAndDivision(IConnectionManager entry)
        {

            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ItemsEdit);

            using (var cmd = entry.Connection.CreateCommand(
                @"
                    UPDATE 
                        rboinventtable
                    SET 
                        DIVISIONID = div.DIVISIONID,
                        ITEMDEPARTMENT = dep.DEPARTMENTID
                    FROM RBOINVENTITEMRETAILGROUP rg
                    JOIN RBOINVENTITEMDEPARTMENT dep on rg.DEPARTMENTID = dep.DEPARTMENTID AND rg.DATAAREAID = dep.DATAAREAID
                    JOIN RBOINVENTITEMRETAILDIVISION div  on dep.divisionid = div.DIVISIONID AND div.DATAAREAID = dep.DATAAREAID
                    WHERE 
                        RBOINVENTTABLE.ITEMGROUP = rg.GROUPID 
                        and rboinventtable.DATAAREAID = @DATAAREAID
                        and RG.DATAAREAID = rboinventtable.DATAAREAID"))
            {
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
            }

        }

        #region Custom overrideables
        protected virtual string AddCustomHandling(string sql)
        {
            return sql.Replace(CustomColumns, "").Replace(CustomConditions, "");
        }

        protected virtual void SaveCustomA(IConnectionManager entry, SqlServerStatement statement, OldRetailItem retailItem)
        {
        }

        protected virtual void SaveCustomB(IConnectionManager entry, SqlServerStatement statement, OldRetailItem retailItem)
        {
        }

        protected virtual void SaveCustomModule(IConnectionManager entry, SqlServerStatement statement, OldRetailItem.RetailItemModule module)
        {
        }
        #endregion

        #region ISequenceable Members

        /// <summary>
        /// Returns true if information about the given class exists in the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The unique sequence ID to search for</param>
        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return ItemRecordExists(entry, id);
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a unique ID for the class
        /// </summary>
        public RecordIdentifier SequenceID
        {
            get { return "ITEM"; }
        }

        #endregion
    }
}
