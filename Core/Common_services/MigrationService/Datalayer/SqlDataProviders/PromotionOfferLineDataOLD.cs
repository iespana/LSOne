using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Services.Datalayer.BusinessObjects;
using LSOne.Services.Datalayer.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.SqlDataProviders
{
    /// <summary>
    /// Data provider class for a discount offer line in periodic discounts.
    /// </summary>
    public class PromotionOfferLineDataOLD : SqlServerDataProviderBase, IPromotionOfferLineDataOLD  
    {
        private static string BaseSQL
        {
            get
            {
                return @"Select DISTINCT
                            L.ID,
                            P.OFFERID as OFFERID, 
                            COALESCE(p.STATUS,0) as STATUS,
                            COALESCE(L.TYPE,0) as TYPE,  
                            NAME = 
			                        CASE L.TYPE
				                        WHEN 0 THEN ISNULL(it.ITEMNAME, '') -- Retail item
				                        WHEN 1 THEN ISNULL(rg.NAME, '') -- Retail group
				                        WHEN 2 THEN ISNULL(rd.NAME, '') -- Retail department
				                        WHEN 5 THEN ISNULL(sp.NAME, '') -- Special group
				                        WHEN 10 THEN ISNULL(v.NAME, '') -- Variant description
				                        ELSE ''
			                        END,
                            COALESCE(L.ITEMRELATION, '') as ITEMRELATION,
                            COALESCE(L.OFFERPRICEINCLTAX, 0) AS OFFERPRICEINCLUDETAX, 
                            COALESCE(L.DISCPCT, 0) AS DISCPCT,
                            COALESCE(im.PRICE,0.0) as STDPRICE,
                            COALESCE(L.DISCAMOUNT, 0) AS DISCOUNTAMOUNT,
                            COALESCE(L.OFFERPRICE,0 ) AS OFFERPRICE,
                            COALESCE(L.DISCAMOUNTINCLTAX, 0) AS DISCOUNTAMOUNTINCLUDETAX,
                            COALESCE(P.DISCVALIDPERIODID, '') AS DISCVALIDPERIODID
                        From RBODISCOUNTOFFERLINE L
                        join PERIODICDISCOUNT P on P.OFFERID = L.OFFERID     
                        left outer join INVENTTABLEMODULE im on L.ITEMRELATION = im.ITEMID and im.MODULETYPE = 2 
                        left join INVENTTABLE it on it.ITEMID = L.ITEMRELATION and it.DATAAREAID = L.DATAAREAID
                        left join RBOINVENTITEMDEPARTMENT rd on rd.DEPARTMENTID = L.ITEMRELATION and rd.DATAAREAID = L.DATAAREAID
                        left join 
                            (
		                        select ic.RBOVARIANTID, ISNULL(vit.ITEMNAME, '') as NAME, ic.DATAAREAID
		                        from INVENTDIMCOMBINATION ic
		                        join INVENTTABLE vit on vit.ITEMID = ic.ITEMID and ic.DATAAREAID = vit.DATAAREAID
                            ) v on v.RBOVARIANTID = L.ITEMRELATION and v.DATAAREAID = L.DATAAREAID
                        left join RBOINVENTITEMRETAILGROUP rg on rg.GROUPID = L.ITEMRELATION and rg.DATAAREAID = L.DATAAREAID
                        left join RBOSPECIALGROUP sp on sp.GROUPID = L.ITEMRELATION and sp.DATAAREAID = L.DATAAREAID ";
            }
        }

        private static string ResolveSort(PromotionOfferLineSorting sort, bool backwards)
        {
            string direction = backwards ? " DESC" : " ASC";

            switch (sort)
            {
                case PromotionOfferLineSorting.OfferID:
                    return "OFFERID" + direction;

                case PromotionOfferLineSorting.Name:
                    return "NAME" + direction;

                case PromotionOfferLineSorting.DiscountPercentage:
                    return "DISCPCT" + direction;

                case PromotionOfferLineSorting.StandardPrice:
                    return "STDPRICE" + direction;

                case PromotionOfferLineSorting.ItemId:
                    return "ITEMRELATION" + direction;

                case PromotionOfferLineSorting.Type:
                    return "TYPE" + direction;

            }

            return "";
        }

        private static void PopulateLine(IDataReader dr, PromotionOfferLine line)
        {
            line.ID = (Guid)dr["ID"];
            line.OfferID = (string)dr["OFFERID"];
            line.Type = (DiscountOfferLine.DiscountOfferTypeEnum)dr["TYPE"];           
            line.Text = (string)dr["NAME"];
            line.ItemRelation = (string)dr["ITEMRELATION"];
            line.DiscountPercent = (decimal)dr["DISCPCT"];
            line.StandardPrice = (decimal)dr["STDPRICE"];

            line.DiscountAmount = (decimal)dr["DISCOUNTAMOUNT"];
            line.OfferPrice = (decimal)dr["OFFERPRICE"];
            line.OfferPriceIncludeTax = (decimal)dr["OFFERPRICEINCLUDETAX"];
            line.DiscountamountIncludeTax = (decimal)dr["DISCOUNTAMOUNTINCLUDETAX"];
            line.ValidationPeriodID = (string)dr["DISCVALIDPERIODID"];
            line.InActiveDiscount = Convert.ToBoolean(dr["STATUS"]);
        }

        /// <summary>
        /// Gets a single discount offer line by a given id. 
        /// </summary>
        /// <remarks>Do not use this for mix and match discount line, use GetMixAndMatchLine instead for that.</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="lineID">The id of the line to fetch</param>
        /// <returns>The discount offer line or null if not found</returns>
        public virtual PromotionOfferLine Get(IConnectionManager entry, RecordIdentifier lineID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSQL +
                                  "where L.ID = @lineID ";

                MakeParam(cmd, "lineID", (Guid)lineID, SqlDbType.UniqueIdentifier);

                var result = Execute<PromotionOfferLine>(entry, cmd, CommandType.Text, PopulateLine);

                return result.Count > 0 ? result[0] : null;
            }
        }

        /// <summary>
        /// Gets a single promotion line for the given offer, relation and relation type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The promotion offer ID</param>
        /// <param name="relationID">The relation ID, f.ex a retail item ID or retail group ID</param>
        /// <param name="relationType">The type of relation</param>
        /// <returns></returns>
        public PromotionOfferLine Get(IConnectionManager entry,
                                             RecordIdentifier offerID,
                                             RecordIdentifier relationID,
                                             DiscountOfferLine.DiscountOfferTypeEnum relationType)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSQL +
                                  @"where L.OFFERID = @offerID and L.ITEMRELATION = @relationID and L.TYPE = @relationType ";

                MakeParam(cmd, "offerID", (string)offerID);
                MakeParam(cmd, "relationID", (string)relationID);
                MakeParam(cmd, "relationType", (int)relationType, SqlDbType.Int);

                var result = Execute<PromotionOfferLine>(entry, cmd, CommandType.Text, PopulateLine);

                return result.Count > 0 ? result[0] : null;
            }
        }

        /// <summary>
        /// Gets all promotion discount lines for a given offer
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="offerID"></param>
        /// <param name="sortBy">The sorting column</param>
        /// <param name="backwardsSort">"True" if sorted backwards</param>
        /// <returns>A list of discount offer lines</returns>
        public virtual List<PromotionOfferLine> GetLines(IConnectionManager entry, RecordIdentifier offerID, PromotionOfferLineSorting sortBy, bool backwardsSort)
        {
            ValidateSecurity(entry);

            var sort = ResolveSort(sortBy, backwardsSort);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSQL + 
                                  "where P.OFFERID = @offerID order by " + sort;

                MakeParam(cmd, "offerID", (string)offerID);

                return Execute<PromotionOfferLine>(entry, cmd, CommandType.Text, PopulateLine);
            }
        }

        /// <summary>
        /// Gets promotion discount lines for a given offer and line type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The promotion offer ID</param>
        /// <param name="type">The type of promotion offer line to get</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="backwardsSort">Determines in what order the result is sorted</param>
        /// <returns></returns>
        public List<PromotionOfferLine> GetLines(IConnectionManager entry, 
                                                        RecordIdentifier offerID, 
                                                        DiscountOfferLine.DiscountOfferTypeEnum type,
                                                        PromotionOfferLineSorting sortBy,
                                                        bool backwardsSort)
        {
            ValidateSecurity(entry);

            var sort = ResolveSort(sortBy, backwardsSort);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSQL +
                    "where P.OFFERID = @offerID and L.Type = @type order by " + sort;

                MakeParam(cmd, "offerID", (string)offerID);
                MakeParam(cmd, "type", (int)type);

                return Execute<PromotionOfferLine>(entry, cmd, CommandType.Text, PopulateLine);
            }

        }

        /// <summary>
        /// Gets a list of data entities containing the IDs and names of the retail items and groups belonging to the promotion offer lines
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The promotion offer ID</param>
        /// <param name="type">The type of promotion offer line to get</param>
        /// <returns></returns>
        public List<DataEntity> GetSimpleLines(IConnectionManager entry, RecordIdentifier offerID,
                                                      DiscountOfferLine.DiscountOfferTypeEnum type)

        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"Select  ISNULL(g.ITEMRELATION,'') as ITEMRELATION, 
                                            ISNULL(g.NAME,'') as NAME      
                                    from RBODISCOUNTOFFERLINE g 
                                    where g.OFFERID = @offerID and TYPE = @type";

                MakeParam(cmd, "offerID", (string)offerID);
                MakeParam(cmd, "type", (int)type);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "ITEMRELATION");
            }
        }

        public List<PromotionOfferLine> GetPromotionsForItem(
            IConnectionManager entry,
            RecordIdentifier itemID,
            RecordIdentifier itemVariantID,
            RecordIdentifier storeID,
            RecordIdentifier customerID,
            RecordIdentifier groupID, 
            RecordIdentifier departmentID,
            bool checkPriceGroup = true,
            bool checkCustomerConnection = true)
        {
            //var item = Providers.RetailItemData.Get(entry, itemID);

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSQL + 
                                  @"Where 
                                    P.PDTYPE = 3 -- Promotion
                                   
                                    AND
                                    (
                                    (L.TYPE = 0 AND L.ITEMRELATION = @itemID) 
                                    OR (L.TYPE = 1 AND L.ITEMRELATION = @retailGroupID) 
                                    OR (L.TYPE = 2 AND L.ITEMRELATION = @retailDepartmentID)  
                                    OR (L.TYPE = 3) -- All
                                    OR (L.TYPE = 5 AND L.ITEMRELATION in (Select GROUPID From RBOSPECIALGROUPITEMS Where ITEMID = @itemID)) 
                                    ";

                if (itemVariantID != null && itemVariantID != RecordIdentifier.Empty)
                {
                    cmd.CommandText += "OR (L.TYPE = 10 AND L.ITEMRELATION = @variantID)  ";
                    MakeParam(cmd, "variantID", (string)itemVariantID);
                }
                else
                {
                    cmd.CommandText += "OR (L.TYPE = 10 AND L.ITEMRELATION in (Select RBOVARIANTID from INVENTDIMCOMBINATION Where ITEMID = @itemID))  ";
                }
                if (checkPriceGroup)
                {
                    cmd.CommandText += @")
                                    AND 
                                    ( -- Check if price group is ok
                                    (P.PRICEGROUP = '')";
                }
                if (customerID != RecordIdentifier.Empty) cmd.CommandText += "OR (P.PRICEGROUP in (Select PRICEGROUP From CUSTTABLE Where ACCOUNTNUM = @customerID)) ";                
                if (storeID != RecordIdentifier.Empty) cmd.CommandText += "OR (P.PRICEGROUP in (Select PRICEGROUPID From RBOLOCATIONPRICEGROUP Where STOREID = @storeID))";

                cmd.CommandText += ")";

                if (checkCustomerConnection)
                {
                    cmd.CommandText +=
                        @"
                          AND 
                          (
                            P.ACCOUNTCODE = 0 or
                            P.ACCOUNTCODE = 1 and P.ACCOUNTRELATION = @accountRelation or
                            P.ACCOUNTCODE = 2 and @accountRelation in (select ACCOUNTNUM from CUSTTABLE where LINEDISC = p.ACCOUNTRELATION)                         
                          ) ";

                    MakeParam(cmd, "accountRelation", (string)customerID);
                }

                MakeParam(cmd, "itemID", (string)itemID);
                MakeParam(cmd, "retailGroupID", (string)groupID);
                MakeParam(cmd, "retailDepartmentID", (string)departmentID);

                if (customerID != RecordIdentifier.Empty) MakeParam(cmd, "customerID", (string)customerID);
                if (storeID != RecordIdentifier.Empty) MakeParam(cmd, "storeID", (string)storeID);

                return Execute<PromotionOfferLine>(entry, cmd, CommandType.Text, PopulateLine);
            }
        }

        /// <summary>
        /// Gets all promotion discount lines for a given retail group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The id of the group</param>
        /// <returns>A list of discount offer lines</returns>
        public List<PromotionOfferLine> GetPromotionsForRetailGroup(
            IConnectionManager entry,
            RecordIdentifier groupID)
        {
            var group = Providers.RetailGroupData.Get(entry, groupID);

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                    cmd.CommandText = BaseSQL +                         
                                      @"Where 
                                        P.PDTYPE = 3 -- Promotion
                                        AND
                                        ((L.TYPE = 1 AND L.ITEMRELATION = @retailGroupID) 
                                        OR (L.TYPE = 2 AND L.ITEMRELATION = @retailDepartmentID)  
                                        OR (L.TYPE = 3) -- All
                                        )
                                        ";                                

                MakeParam(cmd, "retailGroupID", (string)group.ID);
                MakeParam(cmd, "retailDepartmentID", (string)group.RetailDepartmentID);
                
                return Execute<PromotionOfferLine>(entry, cmd, CommandType.Text, PopulateLine);
            }
        }

        /// <summary>
        /// Gets all promotion discount lines for a given special group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The id of the group</param>
        /// <returns>A list of discount offer lines</returns>
        public List<PromotionOfferLine> GetPromotionsForSpecialGroup(
            IConnectionManager entry,
            RecordIdentifier groupID)
        {
            var group = Providers.SpecialGroupData.Get(entry, groupID);

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSQL +
                                  @"Where 
                                    P.PDTYPE = 3 -- Promotion
                                    AND L.TYPE = 5
                                    AND L.ITEMRELATION = @specialGroupID
                                    ";



                MakeParam(cmd, "specialGroupID", (string)group.ID);

                return Execute<PromotionOfferLine>(entry, cmd, CommandType.Text, PopulateLine);
            }
        }

        /// <summary>
        /// Gets all promotion discount lines for a given retail group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="departmentID">The id of the group</param>
        /// <returns>A list of discount offer lines</returns>
        public List<PromotionOfferLine> GetPromotionsForRetailDepartment(
            IConnectionManager entry,
            RecordIdentifier departmentID)
        {
            var department = Providers.RetailDepartmentData.Get(entry, departmentID);

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {

                cmd.CommandText = BaseSQL +
                                  @"Where P.PDTYPE = 3 -- Promotion
                                    AND L.TYPE = 2 
                                    AND L.ITEMRELATION = @retailDepartmentID
                                    ";  


                MakeParam(cmd, "retailDepartmentID", (string)department.ID);

                return Execute<PromotionOfferLine>(entry, cmd, CommandType.Text, PopulateLine);
            }
        }

        /// <summary>
        /// Returns a list of valid promotions that the selected item is in.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The ID of the item to check for</param>
        /// <param name="itemVariantID">The variant ID of the item. Set to RecordIdentifier.Empty if item has no variant</param>
        /// <param name="storeID">The ID of the store we are working in. Used to check for valid price group settings. Set to RecordIdentifier.Empty if no store context is available</param>
        /// <param name="customerID">The ID of the customer we are using. Used to check for valid price group settings. Set to RecordIdentifier.Empty if no customer context is available</param>
        /// <param name="groupID"></param>
        /// <param name="departmentID"></param>
        public List<PromotionOfferLine> GetValidAndEnabledPromotionsForItem(
            IConnectionManager entry, 
            RecordIdentifier itemID, 
            RecordIdentifier itemVariantID, 
            RecordIdentifier storeID, 
            RecordIdentifier customerID,
             RecordIdentifier groupID,
            RecordIdentifier departmentID
            )
        {
            var allPromotionsForItem = GetPromotionsForItem(entry, itemID, itemVariantID, storeID, customerID,groupID,departmentID);
            var validPromotionOfferLines = allPromotionsForItem.Where(x => (((string)x.ValidationPeriodID == "" 
                                                                            || Providers.DiscountPeriodData.IsDiscountPeriodValid(entry, x.ValidationPeriodID, DateTime.Now))
                                                                            && x.InActiveDiscount)).ToList();

            return validPromotionOfferLines;
        }

        /// <summary>
        /// Checks if a discount line exists.
        /// </summary>
        /// <param name="entry">Entry into the database></param>
        /// <param name="id">ID of the line to check for. Note this is double identifier, OFFERID and LINEID</param>
        /// <returns>"True" if the discount line existed, else "False"</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(
                entry,
                "RBODISCOUNTOFFERLINE",
                "ID",
                id);
        }

        /// <summary>
        /// Deletes a discount line.
        /// </summary>
        /// <remarks>Permission needed: "Manage discounts"</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">The id of the line to delete. Note this is double identifier, OFFERID and LINEID</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "RBODISCOUNTOFFERLINE", 
                "ID",
                id,
                Permission.ManageDiscounts);       
        }

        /// <summary>
        /// Deletes a discount line by relation and relation type. For example by retail item id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The promotion offer ID</param>
        /// <param name="relation">The relation ID</param>
        /// <param name="relationType">The relation type</param>
        public virtual void DeleteByRelation(IConnectionManager entry, RecordIdentifier offerID, RecordIdentifier relation, DiscountOfferLine.DiscountOfferTypeEnum relationType)
        {
            DeleteRecord(entry,
                         "RBODISCOUNTOFFERLINE",
                         new[] {"OFFERID", "ITEMRELATION", "TYPE"},
                         new RecordIdentifier(offerID, relation, (int)relationType),
                         Permission.ManageDiscounts);
        }

        /// <summary>
        /// Saves a offer line into the database.
        /// </summary>
        /// <remarks>Permission needed: Manage discounts</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerLine">The offer line to be saved</param>
        public virtual void Save(IConnectionManager entry, PromotionOfferLine offerLine)
        {
            var statement = new SqlServerStatement("RBODISCOUNTOFFERLINE");

            ValidateSecurity(entry, Permission.ManageDiscounts);
            bool isNew = false;
            if (offerLine.ID == RecordIdentifier.Empty || offerLine.ID == null)
            {
                offerLine.ID = Guid.NewGuid();
                isNew = true;
            }

            if (isNew || !Exists(entry, offerLine.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (Guid)offerLine.ID, SqlDbType.UniqueIdentifier);
                statement.AddKey("OFFERID", (string)offerLine.OfferID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("OFFERID", (string)offerLine.OfferID);
                statement.AddCondition("ID", (Guid)offerLine.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("TYPE", (int)offerLine.Type, SqlDbType.Int);
            statement.AddField("NAME", offerLine.Text);
            statement.AddField("ITEMRELATION", (string)offerLine.ItemRelation);
            statement.AddField("DISCPCT", offerLine.DiscountPercent, SqlDbType.Decimal);

            statement.AddField("DISCAMOUNT", offerLine.DiscountAmount, SqlDbType.Decimal);
            statement.AddField("DISCAMOUNTINCLTAX", offerLine.DiscountamountIncludeTax, SqlDbType.Decimal);
            statement.AddField("OFFERPRICE", offerLine.OfferPrice, SqlDbType.Decimal);
            statement.AddField("OFFERPRICEINCLTAX", offerLine.OfferPriceIncludeTax, SqlDbType.Decimal);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
