using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Services.Datalayer.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.SqlDataProviders
{
    /// <summary>
    /// Data provider class for a discount offer line in periodic discounts.
    /// </summary>
    public class DiscountOfferLineDataOLD : SqlServerDataProviderBase, IDiscountOfferLineDataOLD
    {
      private static string EndBaseSQL 
        {
            get
            {
                return @"From POSPERIODICDISCOUNTLINE L
                        join POSPERIODICDISCOUNT P ON P.OfferId = L.OfferId 
                        left outer join INVENTTABLEMODULE im on L.ID = im.ITEMID and im.MODULETYPE = 2 
                        left join INVENTTABLE it on it.ITEMID = L.ID 
                        left join RBOINVENTITEMDEPARTMENT rd on rd.DEPARTMENTID = L.ID 
                        left join 
                            (
		                        select ic.RBOVARIANTID, ISNULL(vit.ITEMNAME, '') as NAME
		                        from INVENTDIMCOMBINATION ic
		                        join INVENTTABLE vit on vit.ITEMID = ic.ITEMID
                            ) v on v.RBOVARIANTID = L.ID 
                        left join RBOINVENTITEMRETAILGROUP rg on rg.GROUPID = L.ID 
                        left join RBOSPECIALGROUP sp on sp.GROUPID = L.ID 
                        left outer join POSMMLINEGROUPS mmg on mmg.OFFERID = L.OFFERID and mmg.LINEGROUP = L.LINEGROUP  ";
            }
        }

        

        private static string BaseLineSQL
        {
            get
            {
                return @"Select DISTINCT <top> 
	                        L.ID,
	                        L.OFFERID as OFFERID,  
	                        L.POSPERIODICDISCOUNTLINEGUID,
	                        COALESCE(L.LINEID,0) as LINEID,
	                        COALESCE(L.PRODUCTTYPE,0) as PRODUCTTYPE,
	                        COALESCE(L.LINEGROUP,'') as LINEGROUP,
	                        DESCRIPTION = 
				                        CASE L.PRODUCTTYPE
					                        WHEN 0 THEN ISNULL(it.ITEMNAME, '') -- Retail item
					                        WHEN 1 THEN ISNULL(rg.NAME, '') -- Retail group
					                        WHEN 2 THEN ISNULL(rd.NAME, '') -- Retail department
					                        WHEN 5 THEN ISNULL(sp.NAME, '') -- Special group
					                        WHEN 10 THEN ISNULL(v.NAME, '') -- Variant description
					                        ELSE ''
				                        END,
	                        COALESCE(L.DEALPRICEORDISCPCT,0.0) as DEALPRICEORDISCPCT,
	                        COALESCE(DISCTYPE,0) as DISCTYPE, 
	                        COALESCE(im.PRICE,0.0) as STDPRICE,
                            COALESCE(mmg.DESCRIPTION,'') as MMGDESCRIPTION,
                            COALESCE(mmg.COLOR,-1) as LINECOLOR,
                            im.TAXITEMGROUPID
                            " + EndBaseSQL;
            }
        }


        private static string LineSQLWithTop(int topRows)
        {
            return BaseLineSQL.Replace("<top>", "TOP (" + topRows + ")");
        }

        private static string LineSQL
        {
            get
            {
                return BaseLineSQL.Replace("<top>", "");
            }
        }
        
        private static string BaseCountLineSQL 
        {
            get
            {
                return @"Select COUNT(*) From POSPERIODICDISCOUNTLINE ";
            }
        }

        private static string SimpleLineSQL
        {
            get 
            { 
                return @"select L.ID, 
                                DESCRIPTION = 
                                CASE L.PRODUCTTYPE
                                    WHEN 0 THEN ISNULL(it.ITEMNAME, '') -- Retail item
                                    WHEN 1 THEN ISNULL(rg.NAME, '') -- Retail group
                                    WHEN 2 THEN ISNULL(rd.NAME, '') -- Retail department
                                    WHEN 5 THEN ISNULL(sp.NAME, '') -- Special group
                                    WHEN 10 THEN ISNULL(v.NAME, '') -- Variant description
                                    ELSE ''
                                END
                        from POSPERIODICDISCOUNTLINE L
                        left join INVENTTABLE it on it.ITEMID = L.ID and it.DATAAREAID = L.DATAAREAID
                        left join RBOINVENTITEMDEPARTMENT rd on rd.DEPARTMENTID = L.ID and rd.DATAAREAID = L.DATAAREAID
                        left join 
                            (
                                select ic.RBOVARIANTID, ISNULL(vit.ITEMNAME, '') as NAME, ic.DATAAREAID
                                from INVENTDIMCOMBINATION ic
                                join INVENTTABLE vit on vit.ITEMID = ic.ITEMID
                            ) v on v.RBOVARIANTID = L.ID and v.DATAAREAID = L.DATAAREAID
                        left join RBOINVENTITEMRETAILGROUP rg on rg.GROUPID = L.ID and rg.DATAAREAID = L.DATAAREAID
                        left join RBOSPECIALGROUP sp on sp.GROUPID = L.ID and sp.DATAAREAID = L.DATAAREAID "; 
            }
        }

        private static void PopulateLine(IDataReader dr, DiscountOfferLine line)
        {
            line.OfferID = (string)dr["OFFERID"];
            line.ID = (Guid)dr["POSPERIODICDISCOUNTLINEGUID"];
            line.LineID = (int)dr["LINEID"];
            line.Type = (DiscountOfferLine.DiscountOfferTypeEnum)dr["PRODUCTTYPE"];
            line.Text = (string)dr["DESCRIPTION"];
            line.LineGroup = (string)dr["LINEGROUP"];
            line.ItemRelation = (string)dr["ID"];
            line.DiscountPercent = (decimal)dr["DEALPRICEORDISCPCT"];          
            line.StandardPrice = (decimal)dr["STDPRICE"];
            line.DiscountType = (DiscountOfferLine.MixAndMatchDiscountTypeEnum)dr["DISCTYPE"];
            line.TaxItemGroupID = dr["TAXITEMGROUPID"] == DBNull.Value ? RecordIdentifier.Empty : (string)dr["TAXITEMGROUPID"]; ;
        }

       

        private static void PopulateMixAndMaxLine(IDataReader dr, DiscountOfferLine line)
        {
            PopulateLine(dr, line);

            line.LineColor = Color.FromArgb((int)dr["LINECOLOR"]);
            line.MMGDescription = (string)dr["MMGDESCRIPTION"];
        }

        private static int GenerateLineID(IConnectionManager entry, RecordIdentifier offerID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select IsNull(Max(LineID)+1,1) as LineID from " +
                    "POSPERIODICDISCOUNTLINE " +
                    "where OFFERID = @offerID";

                MakeParam(cmd, "offerID", (string)offerID);

                return (int)entry.Connection.ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// Gets a single discount offer line by a given id. 
        /// </summary>
        /// <remarks>Do not use this for mix and match discount line, use GetMixAndMatchLine instead for that.</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="lineGuid">The id of the line to fetch</param>
        /// <returns>The discount offer line or null if not found</returns>
        public virtual DiscountOfferLine Get(IConnectionManager entry, RecordIdentifier lineGuid)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = LineSQL +
                                  "where L.POSPERIODICDISCOUNTLINEGUID = @lineGuid and L.DATAAREAID = @dataAreaId  ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "lineGuid", (Guid)lineGuid);

                var result = Execute<DiscountOfferLine>(entry, cmd, CommandType.Text, PopulateLine);

                return result.Count > 0 ? result[0] : null;
            }
        }


        /// <summary>
        /// Gets a single discount offer line for the given offer, relation and relation type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The discount offer ID</param>
        /// <param name="relationID">The relation ID, f.ex a retail item ID or a retail group ID</param>
        /// <param name="relationType">The type of relation</param>
        /// <returns></returns>
        public virtual DiscountOfferLine Get(IConnectionManager entry, RecordIdentifier offerID, RecordIdentifier relationID, DiscountOfferLine.DiscountOfferTypeEnum relationType)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = LineSQL +
                                  "where L.OFFERID = @offerID and L.ID = @relationID and L.PRODUCTTYPE = @productType and L.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "offerID", (string)offerID);
                MakeParam(cmd, "relationID", (string)relationID);
                MakeParam(cmd, "productType", (int)relationType, SqlDbType.Int);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var result = Execute<DiscountOfferLine>(entry, cmd, CommandType.Text, PopulateLine);

                return result.Count > 0 ? result[0] : null;
            }
        }

        /// <summary>
        /// Gets all periodic discount lines for a given offer for all discount types except mix and match
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerID">The id of the offer</param>
        /// <returns>Gets the count of lines for a given offer</returns>
        public virtual int GetLineCount(IConnectionManager entry, RecordIdentifier offerID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseCountLineSQL +
                                  "where OFFERID = @offerID and DATAAREAID = @dataAreaId ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "offerID", (string)offerID);

                return (int) entry.Connection.ExecuteScalar(cmd);
            }
        }
        /// <summary>
        /// Gets all variants on discount in a a given offer for all discount types except mix and match
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerID">The id of the offer</param>
        /// <returns>List of variant idsfor a given offer</returns>
        public virtual List<DataEntity> GetVariants(IConnectionManager entry, RecordIdentifier offerID)
        {

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"
                       Select DISTINCT 
	                        L.id,
	                        'Variant' Name
                        From POSPERIODICDISCOUNTLINE L
                           
                            where L.OFFERID = @offerID and L.DATAAREAID = @dataAreaId ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "offerID", (string)offerID);

                var list = Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "id");

                return list;
            }
        }


        /// <summary>
        /// Gets all periodic discount lines for a given offer for all discount types except mix and match
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerID">The id of the offer</param>
        /// <param name="sortColumn">The number of the sort colum. 0 = PRODUCTTYPE, 1 = ID, 2 = LINEGROUP, 3 = STDPRICE, 4 = DEALPRICEORDISCPCT, 5 = OFFERPRICEINCLTAX, 6 = DISCAMOUNTINCLTAX</param>
        /// <param name="backwardsSort">True if sorting should be backwards</param>
        /// <param name="maxLines">Maximum number of lines to retrieve</param>
        /// <param name="totalLines">Total available lines</param>
        /// <returns>List of discount lines for a given offer</returns>
        public List<DiscountOfferLine> GetLines(IConnectionManager entry, RecordIdentifier offerID, int sortColumn, bool backwardsSort,
            int maxLines, out int totalLines)
        {
            string sort = "";

            ValidateSecurity(entry);

            var cnt = Int32.MaxValue;
            if (maxLines > 0 && maxLines != Int32.MaxValue)
                cnt = GetLineCount(entry, offerID);

            var columns = new[] { "PRODUCTTYPE", "ID", "LINEGROUP", "STDPRICE", "DEALPRICEORDISCPCT", "OFFERPRICEINCLTAX", "DISCAMOUNTINCLTAX" };

            if (sortColumn < columns.Length)
            {
                sort = " order by " + columns[sortColumn] + (backwardsSort ? " DESC" : " ASC");
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = (cnt > maxLines ? LineSQLWithTop(maxLines) : LineSQL) +
                                  "where L.OFFERID = @offerID  " + sort;

                MakeParam(cmd, "offerID", (string)offerID);

                var list = Execute<DiscountOfferLine>(entry, cmd, CommandType.Text, PopulateLine);

                if (cnt > maxLines)
                    totalLines = cnt;
                else
                    totalLines = list.Count;

                return list;
            }
        }


        /// <summary>
        /// Gets all discount offer lines for a given item
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The id of the item</param>
        public virtual List<DiscountOfferLine> GetDiscountOfferLinesForItem(IConnectionManager entry, RecordIdentifier itemID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                                @"Select DISTINCT             L.ID,
	                        L.OFFERID as OFFERID,  
	                        L.POSPERIODICDISCOUNTLINEGUID,
	                        COALESCE(L.LINEID,0) as LINEID,
	                        COALESCE(L.PRODUCTTYPE,0) as PRODUCTTYPE,
	                        COALESCE(L.LINEGROUP,'') as LINEGROUP,
	                        DESCRIPTION = 
				                        CASE L.PRODUCTTYPE
					                        WHEN 0 THEN  (select ISNULL(it.ITEMNAME, '') -- Retail item
											from INVENTTABLE it where  it.ITEMID = L.ID )
					                        WHEN 1 THEN (select ISNULL(rg.NAME, '') -- Retail group
											from RBOINVENTITEMRETAILGROUP rg where  rg.GROUPID = L.ID )
					                        WHEN 2 THEN ( select ISNULL(rd.NAME, '') -- Retail department
											from RBOINVENTITEMDEPARTMENT rd where  rd.DEPARTMENTID = L.ID )
					                        WHEN 5 THEN (select ISNULL(sp.NAME, '')  -- Special group					 
											from  RBOSPECIALGROUP sp where  sp.GROUPID = L.ID )
					                        WHEN 10 THEN 
												(
												   select 
													ISNULL(vit.ITEMNAME, '') as NAME -- Variant description
													from INVENTDIMCOMBINATION ic
													join INVENTTABLE vit on vit.ITEMID = ic.ITEMID
													where ic.RBOVARIANTID = L.ID 
												) 
					                        ELSE ''
				                        END,
	                        COALESCE(L.DEALPRICEORDISCPCT,0.0) as DEALPRICEORDISCPCT,
	                        COALESCE(DISCTYPE,0) as DISCTYPE, 
	                        COALESCE(im.PRICE,0.0) as STDPRICE,
                            im.TAXITEMGROUPID
                            --COALESCE(mmg.DESCRIPTION,'') as MMGDESCRIPTION,
                            --COALESCE(mmg.COLOR,-1) as LINECOLOR                        
							From POSPERIODICDISCOUNTLINE L
                        join POSPERIODICDISCOUNT P ON P.OfferId = L.OfferId AND P.
                        left outer join INVENTTABLEMODULE im on L.ID = im.ITEMID and im.MODULETYPE = 2
                                where 
                                  
                                  P.PDTYPE = 2 -- Discount offer
                                  and
                                  (
                                  (L.PRODUCTTYPE = 0 AND L.ID =@itemID ) 
                                  OR (L.PRODUCTTYPE = 1 AND L.ID = (select ISNULL(RIT.ITEMGROUP, '') from RBOINVENTTABLE RIT where RIT.ITEMID = @itemID AND RIT.DATAAREAID =  L.DATAAREAID) )
                                  OR (L.PRODUCTTYPE = 2 AND L.ID = (select ISNULL(RIT.ITEMDEPARTMENT, '') from RBOINVENTTABLE RIT where RIT.ITEMID = @itemID AND RIT.DATAAREAID =  L.DATAAREAID))  
                                  OR (L.PRODUCTTYPE = 3) -- All
                                  OR (L.PRODUCTTYPE = 5 AND L.ID in (Select GROUPID From RBOSPECIALGROUPITEMS Where ITEMID =@itemID and DATAAREAID = L.DATAAREAID  ))
                                  OR (L.PRODUCTTYPE = 10 AND L.ID in (Select RBOVARIANTID from INVENTDIMCOMBINATION Where ITEMID =@itemID and DATAAREAID = L.DATAAREAID)))
 ";  

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", (string)itemID);

                return Execute<DiscountOfferLine>(entry, cmd, CommandType.Text, PopulateLine);
            }
        }

        /// <summary>
        /// Checks if an relation exists for a given periodic discount offer   
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The offer ID</param>
        /// <param name="relation">The relation ID</param>
        /// <param name="relationType">The type of relation <see cref="DiscountOfferLine.DiscountOfferTypeEnum"/></param>
        /// <param name="discountType">The type of discount offer <see cref="DiscountOffer.PeriodicDiscountOfferTypeEnum"/></param>
        /// <returns></returns>
        public virtual bool RelationExists(IConnectionManager entry, RecordIdentifier offerID, RecordIdentifier relation, DiscountOfferLine.DiscountOfferTypeEnum relationType, DiscountOffer.PeriodicDiscountOfferTypeEnum discountType)
        {
            string[] columnNames = discountType == DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion
                                       ? new[] {"OFFERID", "ITEMRELATION", "TYPE"}
                                       : new[] {"OFFERID", "ID", "PRODUCTTYPE"};

            string tableName = discountType == DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion
                                   ? "RBODISCOUNTOFFERLINE"
                                   : "POSPERIODICDISCOUNTLINE";

            return RecordExists(entry,
                                tableName,
                                columnNames,
                                new RecordIdentifier(offerID, relation, (int)relationType));
        }

        /// <summary>
        /// Gets all discount offer lines for a given retail group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="retailGroupID">The id of the group</param>
        public virtual List<DiscountOfferLine> GetDiscountOfferLinesForRetailGroup(IConnectionManager entry, RecordIdentifier retailGroupID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = LineSQL +
                                @"where L.DATAAREAID = @dataAreaId
                                  and
                                  P.PDTYPE = 2
                                  and
                                  (
                                  (L.PRODUCTTYPE = 1 AND L.ID =  @retailGroupID) 
                                  OR (L.PRODUCTTYPE = 2 AND L.ID =  ISNULL(rg.DEPARTMENTID, ''))
                                  OR (L.PRODUCTTYPE = 3) -- All
                                  )    ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "retailGroupID", (string)retailGroupID);

                return Execute<DiscountOfferLine>(entry, cmd, CommandType.Text, PopulateLine);
            }
        }

        /// <summary>
        /// Gets all discount offer lines for a given special group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The id of the group</param>
        public virtual List<DiscountOfferLine> GetDiscountOfferLinesForSpecialGroup(IConnectionManager entry, RecordIdentifier groupID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = LineSQL +
                                @"where L.DATAAREAID = @dataAreaId
                                  and
                                  P.PDTYPE = 2 -- Discount offer
                                  and L.PRODUCTTYPE = 5
                                  and L.ID = @specialGroupID ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "specialGroupID", (string)groupID);

                return Execute<DiscountOfferLine>(entry, cmd, CommandType.Text, PopulateLine);
            }
        }

        /// <summary>
        /// Gets all discount offer lines for a given retail department
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="departmentID">The id of the department</param>
        public virtual List<DiscountOfferLine> GetDiscountOfferLinesForRetailDepartment(IConnectionManager entry, RecordIdentifier departmentID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = LineSQL +
                                @"where L.DATAAREAID = @dataAreaId
                                  AND P.PDTYPE = 2 -- Discount offer
                                  AND L.PRODUCTTYPE = 2
                                  AND L.ID = @retailDepartmentID
                                  ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "retailDepartmentID", (string)departmentID);

                return Execute<DiscountOfferLine>(entry, cmd, CommandType.Text, PopulateLine);
            }
        }

        /// <summary>
        /// Gets all multibuy lines for a given item
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The id of the item</param>
        public virtual List<DiscountOfferLine> GetMultiBuyLinesForItem(IConnectionManager entry, RecordIdentifier itemID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = LineSQL +
                    @"LEFT OUTER JOIN RBOINVENTTABLE RIT ON RIT.ITEMID = @itemID AND RIT.DATAAREAID = @dataAreaId "+
                                @"where L.DATAAREAID = @dataAreaId
                                  and
                                  L.ID = @itemID
                                  and
                                  P.PDTYPE = 0 -- Multibuy
                                  and
                                  (
                                  (L.PRODUCTTYPE = 0 AND L.ID = @itemID) 
                                  OR (L.PRODUCTTYPE = 1 AND L.ID = ISNULL(RIT.ITEMGROUP, '')) 
                                  OR (L.PRODUCTTYPE = 2 AND L.ID = ISNULL(RIT.ITEMDEPARTMENT, ''))  
                                  OR (L.PRODUCTTYPE = 3) -- All
                                  OR (L.PRODUCTTYPE = 5 AND L.ID in (Select GROUPID From RBOSPECIALGROUPITEMS Where ITEMID = @itemID))
                                  OR (L.PRODUCTTYPE = 10 AND L.ID in (Select RBOVARIANTID from INVENTDIMCOMBINATION Where ITEMID = @itemID)))
                                  ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", (string)itemID);

                return Execute<DiscountOfferLine>(entry, cmd, CommandType.Text, PopulateLine);
            }
        }

        /// <summary>
        /// Gets all mix and match lines for a given item
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The id of the item</param>
        public virtual List<DiscountOfferLine> GetMixMatchLinesForItem(IConnectionManager entry, RecordIdentifier itemID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = LineSQL +
                    @"LEFT OUTER JOIN RBOINVENTTABLE RIT ON RIT.ITEMID = @itemID AND RIT.DATAAREAID = @dataAreaId "+
                                @"where L.DATAAREAID = @dataAreaId
                                  and
                                  L.ID = @itemID
                                  and
                                  P.PDTYPE = 1 -- Mix and match
                                  and
                                  (
                                  (L.PRODUCTTYPE = 0 AND L.ID = @itemID) 
                                  OR (L.PRODUCTTYPE = 1 AND L.ID = ISNULL(RIT.ITEMGROUP, '')) 
                                  OR (L.PRODUCTTYPE = 2 AND L.ID = ISNULL(RIT.ITEMDEPARTMENT, ''))  
                                  OR (L.PRODUCTTYPE = 3) -- All
                                  OR (L.PRODUCTTYPE = 5 AND L.ID in (Select GROUPID From RBOSPECIALGROUPITEMS Where ITEMID = @itemID))
                                  OR (L.PRODUCTTYPE = 10 AND L.ID in (Select RBOVARIANTID from INVENTDIMCOMBINATION Where ITEMID = @itemID)))
                                  ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", (string)itemID);

                return Execute<DiscountOfferLine>(entry, cmd, CommandType.Text, PopulateLine);
            }
        }

        /// <summary>
        /// Gets all mix and match lines for a given retail group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The id of the group</param>
        public virtual List<DiscountOfferLine> GetMixMatchLinesForRetailGroup(IConnectionManager entry, RecordIdentifier groupID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = LineSQL +
                                @"where L.DATAAREAID = @dataAreaId
                                  and
                                  P.PDTYPE = 1
                                  and
                                  (
                                  (L.PRODUCTTYPE = 1 AND L.ID =  @retailGroupID) 
                                  OR (L.PRODUCTTYPE = 2 AND L.ID =  ISNULL(rg.DEPARTMENTID, ''))
                                  OR (L.PRODUCTTYPE = 3) -- All
                                  ) ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "retailGroupID", (string)groupID);

                return Execute<DiscountOfferLine>(entry, cmd, CommandType.Text, PopulateLine);
            }
        }

        /// <summary>
        /// Gets a single mix and match discount offer line by a given id.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="lineGuid">The id of the line to fetch</param>
        /// <param name="mixAndMatchType">A enum stating what kind of mix and match offer we are dealing with</param>
        /// <returns>The mix and match discount offer line or null if not found</returns>
        public virtual DiscountOfferLine GetMixAndMatchLine(IConnectionManager entry, RecordIdentifier lineGuid, DiscountOffer.MixAndMatchDiscountTypeEnum mixAndMatchType)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    LineSQL +
                    "where L.POSPERIODICDISCOUNTLINEGUID = @POSPERIODICDISCOUNTLINEGUID and L.DATAAREAID = @dataAreaId";


                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "POSPERIODICDISCOUNTLINEGUID", (Guid)lineGuid);

                var result = Execute<DiscountOfferLine>(entry, cmd, CommandType.Text, PopulateMixAndMaxLine);

                return result.Count > 0 ? result[0] : null;
            }
        }

        /// <summary>
        /// Gets all periodic discount lines for a given mix and match offer
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerID">The id of the offer</param>
        /// <param name="mixAndMatchType">A enum stating what kind of mix and match offer we are dealing with</param>
        /// <returns>List of mix and match discount lines for a given offer</returns>
        public virtual List<DiscountOfferLine> GetMixAndMatchLines(IConnectionManager entry, RecordIdentifier offerID, DiscountOffer.MixAndMatchDiscountTypeEnum mixAndMatchType)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = LineSQL + 
                                  "where L.OFFERID = @offerID and L.DATAAREAID = @dataAreaId order By LINEGROUP,DESCRIPTION";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "offerID", (string)offerID);

                return Execute<DiscountOfferLine>(entry, cmd, CommandType.Text, PopulateMixAndMaxLine);
            }
        }

        /// <summary>
        /// Gets a list of data entities conaining the IDs and names of the rtail items and groups belonging to the discount offer
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The discount offer ID</param>
        /// <param name="type">The type of discount offer line to get</param>
        /// <returns></returns>
        public virtual List<DataEntity> GetSimpleLines(IConnectionManager entry, RecordIdentifier offerID, DiscountOfferLine.DiscountOfferTypeEnum type)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = SimpleLineSQL +
                                  "where L.OFFERID = @offerID and L.DATAAREAID = @dataAreaID and L.PRODUCTTYPE = @productType ";

                MakeParam(cmd, "offerID", (string)offerID);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "productType", (int)type);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "DESCRIPTION", "ID");
            }
        }

        /// <summary>
        /// Gets a list of data entities conaining the IDs and names of the rtail items and groups belonging to the discount offer for the given line group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The discount offer ID</param>
        /// <param name="lineGroupID">The line group ID</param>
        /// <param name="type">The type of discount offer line to get</param>
        /// <returns></returns>
        public virtual List<DataEntity> GetSimpleLines(IConnectionManager entry, RecordIdentifier offerID, RecordIdentifier lineGroupID, DiscountOfferLine.DiscountOfferTypeEnum type)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = SimpleLineSQL +
                                  "where L.OFFERID = @offerID and L.DATAAREAID = @dataAreaID and L.PRODUCTTYPE = @productType and L.LINEGROUP = @lineGroup";

                MakeParam(cmd, "offerID", (string)offerID);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "productType", (int)type);
                MakeParam(cmd, "lineGroup", (string)lineGroupID);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "DESCRIPTION", "ID");
            }
        }

        /// <summary>
        /// Checks if a discount line exists.
        /// </summary>
        /// <param name="entry">Entry into the database></param>
        /// <param name="offerLineID">ID of the line to check for. Note this is double identifier, OFFERID and LINEID</param>
        /// <returns>True if the discount line existed, else false</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier offerLineID)
        {
            return RecordExists(entry, 
                "POSPERIODICDISCOUNTLINE",
                "POSPERIODICDISCOUNTLINEGUID", 
                offerLineID);
        }

        /// <summary>
        /// Deletes a discount line.
        /// </summary>
        /// <remarks>Permission needed: Manage discounts</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerLineID">The id of the line to delete. Note this is double identifier, OFFERID and LINEID</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier offerLineID)
        {
            DeleteRecord(entry,
                "POSPERIODICDISCOUNTLINE", 
                 "POSPERIODICDISCOUNTLINEGUID" ,
                 offerLineID,
                 LSOne.DataLayer.BusinessObjects.Permission.ManageDiscounts);
        }

        /// <summary>
        /// Saves a offer line into the database.
        /// </summary>
        /// <remarks>Permission needed: Manage discounts</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerLine">The offer line to be saved</param>
        public virtual void Save(IConnectionManager entry, DiscountOfferLine offerLine)
        {
            var statement = new SqlServerStatement("POSPERIODICDISCOUNTLINE");

            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ManageDiscounts);
            
            bool isNew = false;
            if (offerLine.ID == RecordIdentifier.Empty || offerLine.ID == null)
            {
                isNew = true;
                offerLine.ID = Guid.NewGuid();
                offerLine.LineID = GenerateLineID(entry, offerLine.OfferID);
                statement.AddKey("LINEID", (int)offerLine.LineID, SqlDbType.Int);
            }

            if (isNew || !Exists(entry, offerLine.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("OFFERID", (string)offerLine.OfferID);

                statement.AddKey("POSPERIODICDISCOUNTLINEGUID", (Guid)offerLine.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("OFFERID", (string)offerLine.OfferID);
                statement.AddCondition("POSPERIODICDISCOUNTLINEGUID", (Guid)offerLine.ID, SqlDbType.UniqueIdentifier);
                statement.AddCondition("LINEID", (int)offerLine.LineID, SqlDbType.Int);
            }

            statement.AddField("DISCTYPE", (int)offerLine.DiscountType, SqlDbType.Int);
            statement.AddField("PRODUCTTYPE", (int)offerLine.Type, SqlDbType.Int);
            statement.AddField("LINEGROUP", (string)offerLine.LineGroup);
            statement.AddField("ID", (string)offerLine.ItemRelation);
            statement.AddField("DEALPRICEORDISCPCT", offerLine.DiscountPercent, SqlDbType.Decimal);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Deletes a discount offer line by relation and relation type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The discount offer ID</param>
        /// <param name="relation">The relation ID</param>
        /// <param name="relationType">The relation type</param>
        public virtual void DeleteByRelation(IConnectionManager entry, RecordIdentifier offerID, RecordIdentifier relation, DiscountOfferLine.DiscountOfferTypeEnum relationType)
        {
            DeleteRecord(entry,
                         "POSPERIODICDISCOUNTLINE",
                         new[]{"OFFERID", "ID", "PRODUCTTYPE"},
                         new RecordIdentifier(offerID, relation, (int)relationType),
                         DataLayer.BusinessObjects.Permission.ManageDiscounts);
        }

        public virtual void DeleteDiscountLinesForVariant(IConnectionManager entry, RecordIdentifier variant)
        {
            DeleteRecord(entry,
                         "POSPERIODICDISCOUNTLINE",
                         new[] {  "ID", "PRODUCTTYPE" },
                         new RecordIdentifier(variant, (int)DiscountOfferLine.DiscountOfferTypeEnum.Variant),
                         DataLayer.BusinessObjects.Permission.ManageDiscounts);
        }
    }
}
