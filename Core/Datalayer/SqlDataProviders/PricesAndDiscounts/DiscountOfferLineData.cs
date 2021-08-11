using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.PricesAndDiscounts
{
    /// <summary>
    /// Data provider class for a discount offer line in periodic discounts.
    /// </summary>
    public class DiscountOfferLineData : SqlServerDataProviderBase, IDiscountOfferLineData
    {
        private List<TableColumn> promotionColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "COALESCE(L.OFFERPRICEINCLTAX, 0) ",  ColumnAlias = "OFFERPRICEINCLUDETAX"},
            new TableColumn {ColumnName = "COALESCE(L.DISCPCT, 0) ",  ColumnAlias = "DISCPCT"},
            new TableColumn {ColumnName = "COALESCE(L.DISCAMOUNT, 0) ",  ColumnAlias = "DISCOUNTAMOUNT"},
            new TableColumn {ColumnName = "COALESCE(L.OFFERPRICE,0 ) ",  ColumnAlias = "OFFERPRICE"},
            new TableColumn {ColumnName = "COALESCE(L.DISCAMOUNTINCLTAX, 0)  ",  ColumnAlias = "DISCOUNTAMOUNTINCLUDETAX"},
            new TableColumn {ColumnName = "COALESCE(P.DISCVALIDPERIODID, '') ",  ColumnAlias = "DISCVALIDPERIODID"},
            new TableColumn {ColumnName = "COALESCE(p.STATUS,0)",ColumnAlias = "STATUS"},
        };


        private List<TableColumn> selectionColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "TARGETID", TableAlias = "L"},
            new TableColumn {ColumnName = "ISNULL(L.UNIT, '')", ColumnAlias = "UNIT"},
            new TableColumn {ColumnName = "TARGETMASTERID", TableAlias = "L"},
            new TableColumn {ColumnName = "OFFERID", TableAlias = "L"},
            new TableColumn {ColumnName = "OFFERMASTERID", TableAlias = "L"},
            new TableColumn {ColumnName = "POSPERIODICDISCOUNTLINEGUID", TableAlias = "L"},
            new TableColumn {ColumnName = "COALESCE(L.LINEID,0)", ColumnAlias = "LINEID"},
            new TableColumn {ColumnName = "COALESCE(L.PRODUCTTYPE,0)", ColumnAlias = "PRODUCTTYPE"},
            new TableColumn {ColumnName = "COALESCE(L.LINEGROUP,'') ", ColumnAlias = "LINEGROUP"},

            new TableColumn {ColumnName = @"CASE L.PRODUCTTYPE
					                        WHEN 0 THEN ISNULL(it.ITEMNAME, '') -- Retail item
					                        WHEN 1 THEN ISNULL(rg.NAME, '') -- Retail group
					                        WHEN 2 THEN ISNULL(rd.NAME, '') -- Retail department
					                        WHEN 5 THEN ISNULL(sp.NAME, '') -- Special group
					                        WHEN 10 THEN ISNULL(it.ITEMNAME, '') -- Variant description
					                        ELSE ''
				                        END ", ColumnAlias = "DESCRIPTION"},
            new TableColumn {ColumnName = @"CASE L.PRODUCTTYPE
					                        WHEN 0 THEN ISNULL(it.VARIANTNAME, '') -- Retail item
					                        WHEN 1 THEN '' -- Retail group
					                        WHEN 2 THEN '' -- Retail department
					                        WHEN 5 THEN '' -- Special group
					                        WHEN 10 THEN ISNULL(it.VARIANTNAME, '') -- Variant description
					                        ELSE ''
				                        END ", ColumnAlias = "VARIANTNAME"},
            new TableColumn {ColumnName = "COALESCE(L.DEALPRICEORDISCPCT,0.0) ", ColumnAlias = "DEALPRICEORDISCPCT"},
            new TableColumn {ColumnName = "COALESCE(DISCTYPE,0) ", ColumnAlias = "DISCTYPE"},
            new TableColumn {ColumnName = "COALESCE(it.SALESPRICE,0.0) ", ColumnAlias = "STDPRICE"},
            new TableColumn {ColumnName = "SALESTAXITEMGROUPID ", TableAlias = "it"},
        };

        private List<TableColumn> mmgColumns = new List<TableColumn>
        {

            new TableColumn {ColumnName = "COALESCE(mmg.DESCRIPTION,'') ", ColumnAlias = "MMGDESCRIPTION"},
            new TableColumn {ColumnName = "COALESCE(mmg.COLOR,-1) ", ColumnAlias = "LINECOLOR"},
        };

        private List<Join> fixedJoins = new List<Join>
        {
            new Join
            {
                Condition = " P.MASTERID = L.OFFERMASTERID",
                Table = "PERIODICDISCOUNT",
                TableAlias = "P"
            },
             new Join
            {
                Condition = " it.MASTERID = L.TARGETMASTERID AND IT.DELETED = 0",
                JoinType = "LEFT OUTER",
                Table = "RETAILITEM",
                TableAlias = "it"
            },
            new Join
            {
                Condition = " it.RETAILGROUPMASTERID = ITRETAILGROUPS.MASTERID AND ITRETAILGROUPS.DELETED = 0",
                JoinType = "LEFT OUTER",
                Table = "RETAILGROUP",
                TableAlias = "ITRETAILGROUPS"
            },
            new Join
            {
                Condition = " rd.MASTERID = L.TARGETMASTERID AND RD.DELETED = 0",
                JoinType = "LEFT OUTER",
                Table = "RETAILDEPARTMENT",
                TableAlias = "rd"
            },
          
            new Join
            {
                Condition = " rg.MASTERID = L.TARGETMASTERID AND RG.DELETED = 0",
                JoinType = "LEFT OUTER",
                Table = "RETAILGROUP",
                TableAlias = "rg"
            },
            new Join
            {
                Condition = " sp.MASTERID = L.TARGETMASTERID AND SP.DELETED = 0",
                JoinType = "LEFT OUTER",
                Table = "SPECIALGROUP",
                TableAlias = "sp"
            },         
        
        };        

        private List<Join> discountOfferLinesForItemJoins = new List<Join>
        {
            new Join
            {
                Condition = " P.MASTERID = L.OFFERMASTERID AND P.DELETED = 0",
                Table = "PERIODICDISCOUNT",
                TableAlias = "P"
            },
             new Join
            {
                Condition = " (L.PRODUCTTYPE = 0 and it.ITEMID = L.TARGETID) or (L.PRODUCTTYPE <> 0 and it.ITEMID = @itemID) AND IT.DELETED = 0",
                JoinType = "LEFT OUTER",
                Table = "RETAILITEM",
                TableAlias = "it"
            },
            new Join
            {
                Condition = " it.RETAILGROUPMASTERID = ITRETAILGROUPS.MASTERID AND ITRETAILGROUPS.DELETED = 0",
                JoinType = "LEFT OUTER",
                Table = "RETAILGROUP",
                TableAlias = "ITRETAILGROUPS"
            },
            new Join
            {
                Condition = " rd.MASTERID = L.TARGETMASTERID AND RD.DELETED = 0",
                JoinType = "LEFT OUTER",
                Table = "RETAILDEPARTMENT",
                TableAlias = "rd"
            },

            new Join
            {
                Condition = " rg.MASTERID = L.TARGETMASTERID AND RG.DELETED = 0",
                JoinType = "LEFT OUTER",
                Table = "RETAILGROUP",
                TableAlias = "rg"
            },
            new Join
            {
                Condition = " sp.MASTERID = L.TARGETMASTERID AND SP.DELETED = 0",
                JoinType = "LEFT OUTER",
                Table = "SPECIALGROUP",
                TableAlias = "sp"
            },
              new Join
            {
                Condition = "sgi.GROUPMASTERID = sp.MASTERID ",
                JoinType = "LEFT OUTER",
                Table = "SPECIALGROUPITEMS ",
                TableAlias = "SGI"
            },


        };

        private List<TableColumn> simpleColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "TARGETID", TableAlias = "L"},
            new TableColumn {ColumnName = "TARGETMASTERID", TableAlias = "L"},
            new TableColumn {ColumnName = @"CASE L.PRODUCTTYPE
					                        WHEN 0 THEN ISNULL(it.ITEMNAME, '') -- Retail item
					                        WHEN 1 THEN ISNULL(rg.NAME, '') -- Retail group
					                        WHEN 2 THEN ISNULL(rd.NAME, '') -- Retail department
					                        WHEN 5 THEN ISNULL(sp.NAME, '') -- Special group
					                        WHEN 10 THEN ISNULL(it.ITEMNAME, '') -- Variant description
					                        ELSE ''
				                        END ",  ColumnAlias = "DESCRIPTION"},

            new TableColumn {ColumnName = "VARIANTNAME", TableAlias = "IT"},

        };

        private List<Join> simpleJoins = new List<Join>
        {
               new Join
            {
                Condition = " it.MASTERID = L.TARGETMASTERID AND IT.DELETED = 0",
                JoinType = "LEFT OUTER",
                Table = "RETAILITEM",
                TableAlias = "it"
            },
            new Join
            {
                Condition = " rd.MASTERID = L.TARGETMASTERID AND RD.DELETED = 0",
                JoinType = "LEFT OUTER",
                Table = "RETAILDEPARTMENT",
                TableAlias = "rd"
            },

            new Join
            {
                Condition = " rg.MASTERID = L.TARGETMASTERID AND RG.DELETED = 0",
                JoinType = "LEFT OUTER",
                Table = "RETAILGROUP",
                TableAlias = "rg"
            },
            new Join
            {
                Condition = " sp.MASTERID = L.TARGETMASTERID AND SP.DELETED = 0",
                JoinType = "LEFT OUTER",
                Table = "SPECIALGROUP",
                TableAlias = "sp"
            },
            

        };

        private static Dictionary<DiscountLineSortEnum, TableColumn> SortColumns = new Dictionary<DiscountLineSortEnum, TableColumn>
        {

            {DiscountLineSortEnum.None, new TableColumn()},
            {DiscountLineSortEnum.ProductType, new TableColumn {ColumnName = "PRODUCTTYPE"}},
            {DiscountLineSortEnum.TargetID, new TableColumn {ColumnName = "TARGETID"}},
            {DiscountLineSortEnum.Description, new TableColumn {ColumnName = "DESCRIPTION"}},
            {DiscountLineSortEnum.VariantName, new TableColumn {ColumnName = "VARIANTNAME",TableAlias = "IT"}},
            {DiscountLineSortEnum.LineGroup,new TableColumn {ColumnName = "LINEGROUP"}},
            {DiscountLineSortEnum.StandardPrice, new TableColumn {ColumnName = "STDPRICE"}},
            {DiscountLineSortEnum.DealPriceOrPercent, new TableColumn {ColumnName = "DEALPRICEORDISCPCT"}},
            {DiscountLineSortEnum.OfferPriceIncludingTax, new TableColumn {ColumnName = "OFFERPRICEINCLTAX"}},
            {DiscountLineSortEnum.DiscountAmountIncludingTax, new TableColumn {ColumnName = "DISCAMOUNTINCLTAX"}},
            {DiscountLineSortEnum.ProductTypeDesc, new TableColumn {ColumnName = "PRODUCTTYPE", SortDescending = true}},
            {DiscountLineSortEnum.TargetIDDesc, new TableColumn {ColumnName = "TARGETID", SortDescending = true}},
            {DiscountLineSortEnum.DescriptionDesc, new TableColumn {ColumnName = "DESCRIPTION", SortDescending = true}},
            {DiscountLineSortEnum.VariantNameDesc, new TableColumn {ColumnName = "VARIANTNAME",TableAlias = "IT", SortDescending = true}},
            {DiscountLineSortEnum.LineGroupDesc,new TableColumn{ColumnName = "LINEGROUP",SortDescending = true}},
            {DiscountLineSortEnum.StandardPriceDesc,new TableColumn{ColumnName = "STDPRICE",SortDescending = true}},
            {DiscountLineSortEnum.DealPriceOrPercentDesc,new TableColumn{ColumnName = "DEALPRICEORDISCPCT",SortDescending = true}},
            {DiscountLineSortEnum.OfferPriceIncludingTaxDesc,new TableColumn{ColumnName = "OFFERPRICEINCLTAX",SortDescending = true}},
            {DiscountLineSortEnum.DiscountAmountIncludingTaxDesc,new TableColumn{ColumnName = "DISCAMOUNTINCLTAX",SortDescending = true}},

            
        };

        private static void PopulateListMasterID(IDataReader dr, MasterIDEntity group)
        {
            group.ReadadbleID = (string)dr["TARGETID"];
            group.ID = (Guid)dr["TARGETMASTERID"];
            group.Text = (string)dr["DESCRIPTION"];
            group.ExtendedText = dr["VARIANTNAME"] == DBNull.Value ? string.Empty : (string) dr["VARIANTNAME"];

        }
        private static void PopulateLine(IDataReader dr, DiscountOfferLine line)
        {
            line.OfferID = (string)dr["OFFERID"];
            line.OfferMasterID = (Guid)dr["OFFERMASTERID"];
            line.ID = (Guid)dr["POSPERIODICDISCOUNTLINEGUID"];
            line.LineID = (int)dr["LINEID"];
            line.Type = (DiscountOfferLine.DiscountOfferTypeEnum)dr["PRODUCTTYPE"];
            line.Text = (string)dr["DESCRIPTION"];
            line.LineGroup = (string)dr["LINEGROUP"];
            line.ItemRelation = (string)dr["TARGETID"];
            line.Unit = (string)dr["UNIT"];
            line.TargetMasterID = (Guid)dr["TARGETMASTERID"];
            line.DiscountPercent = (decimal)dr["DEALPRICEORDISCPCT"];          
            line.StandardPrice = (decimal)dr["STDPRICE"];
            line.DiscountType = (DiscountOfferLine.MixAndMatchDiscountTypeEnum)dr["DISCTYPE"];
            line.TaxItemGroupID = dr["SALESTAXITEMGROUPID"] == DBNull.Value ? RecordIdentifier.Empty : (string)dr["SALESTAXITEMGROUPID"];

            if (dr["VARIANTNAME"] != DBNull.Value)
            {
                line.VariantName = (string)dr["VARIANTNAME"];
            }
        }
        private static void PopulatePromotionLine(IDataReader dr, PromotionOfferLine line)
        {
            line.ID = (Guid)dr["POSPERIODICDISCOUNTLINEGUID"];
            line.OfferID = (string)dr["OFFERID"];
            line.OfferMasterID = (Guid)dr["OFFERMASTERID"];
            line.Type = (DiscountOfferLine.DiscountOfferTypeEnum)dr["PRODUCTTYPE"];
            line.Text = (string)dr["DESCRIPTION"];
            line.ItemRelation = (string)dr["TARGETID"];
            line.Unit = (string)dr["UNIT"];
            line.TargetMasterID = (Guid)dr["TARGETMASTERID"];
            line.DiscountPercent = (decimal)dr["DISCPCT"];
            line.StandardPrice = (decimal)dr["STDPRICE"];
            line.DiscountAmount = (decimal)dr["DISCOUNTAMOUNT"];
            line.OfferPrice = (decimal)dr["OFFERPRICE"];
            line.OfferPriceIncludeTax = (decimal)dr["OFFERPRICEINCLUDETAX"];
            line.DiscountamountIncludeTax = (decimal)dr["DISCOUNTAMOUNTINCLUDETAX"];
            line.ValidationPeriodID = (string)dr["DISCVALIDPERIODID"];
            line.InActiveDiscount = Convert.ToBoolean(dr["STATUS"]);
            line.LineID = (int)dr["LINEID"];
            if (dr["VARIANTNAME"] != DBNull.Value)
            {
                line.VariantName = (string)dr["VARIANTNAME"];
                line.ItemIsVariant = true;
            }


        }

        private static void PopulateLineWithCount(IConnectionManager entry, IDataReader dr, DiscountOfferLine line, ref int rowCount)
        {
            PopulateLine(dr, line);
            PopulateRowCount(entry,dr,ref rowCount);
        }

        protected static void PopulateRowCount(IConnectionManager entry, IDataReader dr, ref int rowCount)
        {
            if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 || entry.Connection.DatabaseVersion == ServerVersion.Unknown)
            {
                rowCount = (int)dr["Row_Count"];
            }
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
                    "PERIODICDISCOUNTLINE " +
                    "where OFFERID = @offerID";

                MakeParam(cmd, "offerID", (string)offerID);

                return (int)entry.Connection.ExecuteScalar(cmd);
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
                    return "TARGETID" + direction;

                case PromotionOfferLineSorting.Type:
                    return "PRODUCTTYPE" + direction;

            }

            return "";
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
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "L.POSPERIODICDISCOUNTLINEGUID = @lineGuid", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });

                cmd.CommandText =
                    string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L"),
                        QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                        QueryPartGenerator.JoinGenerator(fixedJoins),
                        QueryPartGenerator.ConditionGenerator(conditions),
                        string.Empty
                        );

                MakeParam(cmd, "lineGuid", (Guid)lineGuid);

                var result = Execute<DiscountOfferLine>(entry, cmd, CommandType.Text, PopulateLine);

                return result.Count > 0 ? result[0] : null;
            }
        }
        /// <summary>
        /// Gets a single promotion offer line by a given id. 
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="lineID">The id of the line to fetch</param>
        /// <returns>The discount offer line or null if not found</returns>
        public PromotionOfferLine GetPromotion(IConnectionManager entry, RecordIdentifier lineID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "L.POSPERIODICDISCOUNTLINEGUID = @lineGuid", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });

                List<TableColumn> columns = new List<TableColumn>(selectionColumns);
                columns.AddRange(promotionColumns);

                List<Join> joins = new List<Join>(fixedJoins);
               

                cmd.CommandText =
                    string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L"),
                        QueryPartGenerator.InternalColumnGenerator(columns),
                        QueryPartGenerator.JoinGenerator(joins),
                        QueryPartGenerator.ConditionGenerator(conditions),
                        string.Empty
                        );

                MakeParam(cmd, "lineGuid", (Guid)lineID, SqlDbType.UniqueIdentifier);

                var result = Execute<PromotionOfferLine>(entry, cmd, CommandType.Text, PopulatePromotionLine);

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
        public PromotionOfferLine GetPromotion(IConnectionManager entry,
                                             RecordIdentifier offerID,
                                             RecordIdentifier relationID,
                                             DiscountOfferLine.DiscountOfferTypeEnum relationType)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "L.OFFERID = @offerID", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = "L.TARGETID = @relationID", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = " L.PRODUCTTYPE = @relationType", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });

                List<TableColumn> columns = new List<TableColumn>(selectionColumns);
                columns.AddRange(promotionColumns);

                List<Join> joins = new List<Join>(fixedJoins);
              

                cmd.CommandText =
                    string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L"),
                        QueryPartGenerator.InternalColumnGenerator(columns),
                        QueryPartGenerator.JoinGenerator(joins),
                        QueryPartGenerator.ConditionGenerator(conditions),
                        string.Empty
                        );


                MakeParam(cmd, "offerID", (string)offerID);
                MakeParam(cmd, "relationID", (string)relationID);
                MakeParam(cmd, "relationType", (int)relationType, SqlDbType.Int);

                var result = Execute<PromotionOfferLine>(entry, cmd, CommandType.Text, PopulatePromotionLine);

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
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "L.OFFERID = @offerID", Operator = "AND" });
                if (relationID.IsGuid)
                {
                    conditions.Add(new Condition { ConditionValue = "L.TARGETMASTERID = @relationID", Operator = "AND" });
                    MakeParam(cmd, "relationID", (Guid)relationID, SqlDbType.UniqueIdentifier);

                }
                else
                {
                    conditions.Add(new Condition { ConditionValue = "L.TARGETID = @relationID", Operator = "AND" });
                    MakeParam(cmd, "relationID", (string)relationID);

                }
                conditions.Add(new Condition { ConditionValue = " L.PRODUCTTYPE = @productType", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });

                cmd.CommandText =
                    string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L"),
                        QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                        QueryPartGenerator.JoinGenerator(fixedJoins),
                        QueryPartGenerator.ConditionGenerator(conditions),
                        string.Empty
                        );

                MakeParam(cmd, "offerID", (string)offerID);
                MakeParam(cmd, "productType", (int)relationType, SqlDbType.Int);

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
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "OFFERID = @offerID", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });

                TableColumn column = new TableColumn {ColumnName = "COUNT(1)"};

                cmd.CommandText =
                    string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L"),
                        column,
                        string.Empty,
                        QueryPartGenerator.ConditionGenerator(conditions),
                        string.Empty
                        );

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
            //TODO Drop it like it's hot
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"
                       Select DISTINCT 
	                        L.id,
	                        'Variant' Name
                        From PERIODICDISCOUNTLINE L
                           
                            where L.OFFERID = @offerID and L.DATAAREAID = @dataAreaId ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "offerID", (string)offerID);

                var list = Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "TARGETID");

                return list;
            }
        }



        /// <summary>
        /// Gets all periodic discount lines for a given offer for all discount types except mix and match
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerID">The id of the offer</param>
        /// <param name="sortEnum">Sort Settings</param> 
        /// <param name="maxLines">Maximum number of lines to retrieve</param>
        /// <param name="totalLines">Total available lines</param>
        /// <returns>List of discount lines for a given offer</returns>
        public List<DiscountOfferLine> GetLines(IConnectionManager entry, RecordIdentifier offerID, DiscountLineSortEnum sortEnum,
            int maxLines, out int totalLines)
        {

            ValidateSecurity(entry);

            //var sortColumns = new[] { "PRODUCTTYPE", "TARGETID", "DESCRIPTION","IT.VARIANTNAME", "LINEGROUP", "STDPRICE", "DEALPRICEORDISCPCT", "OFFERPRICEINCLTAX", "DISCAMOUNTINCLTAX" };
            

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>();
                foreach (var selectionColumn in selectionColumns)
                {
                    columns.Add(selectionColumn);
                }
                columns.Add(new TableColumn
                {
                    ColumnName = string.Format("ROW_NUMBER() OVER(order by {0})", sortEnum == DiscountLineSortEnum.None ? "PRODUCTTYPE" : SortColumns[sortEnum].ToSortString(true)),
                    ColumnAlias = "ROW"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = string.Format("COUNT(1) OVER ( ORDER BY {0} RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )", sortEnum == DiscountLineSortEnum.None ? "PRODUCTTYPE" : SortColumns[sortEnum].ToSortString(true)),
                    ColumnAlias = "ROW_COUNT"
                });

                List<Condition> conditions = new List<Condition>();
                conditions.Add( new Condition {ConditionValue = "L.OFFERID = @offerID",Operator = "AND"});

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });
                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("PERIODICDISCOUNTLINE", "L", "S",maxLines),
                    QueryPartGenerator.ExternalColumnGenerator(columns, "S"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(fixedJoins),
                    QueryPartGenerator.ConditionGenerator( conditions),
                    string.Empty,
                    sortEnum == DiscountLineSortEnum.None ? string.Empty : string.Format("ORDER BY {0}", SortColumns[sortEnum].ToSortString(true)));

                MakeParam(cmd, "offerID", (string)offerID);
                totalLines = 0;

                var list = Execute< DiscountOfferLine, int>(entry, cmd, CommandType.Text,
                    ref totalLines, PopulateLineWithCount);

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
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition {ConditionValue = "P.PDTYPE = 2",Operator = "AND"});
                conditions.Add(new Condition { ConditionValue = @"(
			                (L.PRODUCTTYPE = 0 AND 
                              (L.TARGETID =@itemID OR
                                L.TARGETID IN (
                                   SELECT ITHEADER.ITEMID
                                    FROM RETAILITEM ITVARIANT
                                    JOIN RETAILITEM ITHEADER ON ITHEADER.MASTERID = ITVARIANT.HEADERITEMID
                                    WHERE ITVARIANT.ITEMID = @itemID) 
                              )
                            ) 
			                OR (L.PRODUCTTYPE = 1 AND L.TARGETMASTERID = IT.RETAILGROUPMASTERID AND it.ITEMID=  @itemID )
			                OR (L.PRODUCTTYPE = 2 AND L.TARGETMASTERID = ITRETAILGROUPS.DEPARTMENTMASTERID AND it.ITEMID=  @itemID )  
			                OR (L.PRODUCTTYPE = 3) -- All
			                OR (L.PRODUCTTYPE = 5 AND L.TARGETMASTERID = sp.MASTERID AND sgi.ITEMID =  @itemID )
			                OR (L.PRODUCTTYPE = 10 AND L.TARGETID = @itemID)
			                )", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                    QueryPartGenerator.JoinGenerator(discountOfferLinesForItemJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

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
            string[] columnNames = {"OFFERMASTERID", "TARGETMASTERID", "PRODUCTTYPE"};

            string tableName =  "PERIODICDISCOUNTLINE";

            return RecordExists(entry,
                                tableName,
                                columnNames,
                                new RecordIdentifier(offerID, relation, (int)relationType),false, true);
        }

        /// <summary>
        /// Gets all discount offer lines for a given retail group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="retailGroupID">The id of the group</param>
        public virtual List<DiscountOfferLine> GetDiscountOfferLinesForRetailGroup(IConnectionManager entry,
            RecordIdentifier retailGroupID)
        {

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition {ConditionValue = "P.PDTYPE = 2", Operator = "AND"});
                conditions.Add(new Condition {ConditionValue = @" (
                                  (L.PRODUCTTYPE = 1 AND L.ID =  @retailGroupID) 
                                  OR (L.PRODUCTTYPE = 2 AND L.ID =  ISNULL(rg.DEPARTMENTID, ''))
                                  OR (L.PRODUCTTYPE = 3) -- All
                                  ) ", Operator = "AND"});
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                    QueryPartGenerator.JoinGenerator(fixedJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);
                
                MakeParam(cmd, "retailGroupID", (string) retailGroupID);

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
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { ConditionValue = "P.PDTYPE = 2", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = "L.PRODUCTTYPE = 5", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = "L.ID = @specialGroupID", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = @" (
                                  (L.PRODUCTTYPE = 1 AND L.ID =  @retailGroupID) 
                                  OR (L.PRODUCTTYPE = 2 AND L.ID =  ISNULL(rg.DEPARTMENTID, ''))
                                  OR (L.PRODUCTTYPE = 3) -- All
                                  ) ", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                    QueryPartGenerator.JoinGenerator(fixedJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

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
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { ConditionValue = "P.PDTYPE = 2", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = "L.PRODUCTTYPE = 2", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = "L.ID = @retailDepartmentID", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = @" (
                                  (L.PRODUCTTYPE = 1 AND L.ID =  @retailGroupID) 
                                  OR (L.PRODUCTTYPE = 2 AND L.ID =  ISNULL(rg.DEPARTMENTID, ''))
                                  OR (L.PRODUCTTYPE = 3) -- All
                                  ) ", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                    QueryPartGenerator.JoinGenerator(fixedJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);
           
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
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { ConditionValue = "P.PDTYPE = 0", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = " L.TARGETID = @itemID", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = @"(
			                (L.PRODUCTTYPE = 0 AND L.TARGETID =@itemID ) 
			                OR (L.PRODUCTTYPE = 1 AND L.TARGETMASTERID = IT.RETAILGROUPMASTERID AND it.ITEMID=  @itemID )
			                OR (L.PRODUCTTYPE = 2 AND L.TARGETMASTERID = ITRETAILGROUPS.DEPARTMENTMASTERID AND it.ITEMID=  @itemID )  
			                OR (L.PRODUCTTYPE = 3) -- All
			                --OR (L.PRODUCTTYPE = 5 AND L.TARGETMASTERID = sp.MASTERID AND SGI.ITEMID =  @itemID )
			                OR (L.PRODUCTTYPE = 10 AND L.TARGETID = @itemID)
			                )", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });
                List<Join> joins = new List<Join>(fixedJoins);
                joins.Add(new Join
                {
                    Condition = "sgi.GROUPMASTERID = sp.MASTERID ",
                    JoinType = "LEFT OUTER",
                    Table = "SPECIALGROUPITEMS ",
                    TableAlias = "SGI"
                });
                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

                MakeParam(cmd, "itemID", (string)itemID);

                return Execute<DiscountOfferLine>(entry, cmd, CommandType.Text, PopulateLine);
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
        public virtual List<PromotionOfferLine> GetPromotionLines(IConnectionManager entry, RecordIdentifier offerID, PromotionOfferLineSorting sortBy, bool backwardsSort)
        {
            ValidateSecurity(entry);

            var sort = ResolveSort(sortBy, backwardsSort);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>(selectionColumns);
                columns.AddRange(promotionColumns);

                List<Join> joins = new List<Join>(fixedJoins);
                
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "L.OFFERID = @offerID", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    $"ORDER BY {sort}");

                MakeParam(cmd, "offerID", (string)offerID);

                return Execute<PromotionOfferLine>(entry, cmd, CommandType.Text, PopulatePromotionLine);
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
        public List<PromotionOfferLine> GetPromotionLines(IConnectionManager entry,
                                                        RecordIdentifier offerID,
                                                        DiscountOfferLine.DiscountOfferTypeEnum type,
                                                        PromotionOfferLineSorting sortBy,
                                                        bool backwardsSort)
        {
            ValidateSecurity(entry);

            var sort = ResolveSort(sortBy, backwardsSort);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>(selectionColumns);
                columns.AddRange(promotionColumns);

               
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { ConditionValue = "L.OFFERID = @offerID", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = "L.PRODUCTTYPE = @type", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });
                List<Join> joins = new List<Join>(fixedJoins);
               

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    sort);

                MakeParam(cmd, "offerID", (string)offerID);
                MakeParam(cmd, "type", (int)type);

                return Execute<PromotionOfferLine>(entry, cmd, CommandType.Text, PopulatePromotionLine);
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
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { ConditionValue = "P.PDTYPE = 1", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = " L.TARGETID = @itemID", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = @"(
			                (L.PRODUCTTYPE = 0 AND L.TARGETID =@itemID ) 
			                OR (L.PRODUCTTYPE = 1 AND L.TARGETMASTERID = IT.RETAILGROUPMASTERID AND it.ITEMID=  @itemID )
			                OR (L.PRODUCTTYPE = 2 AND L.TARGETMASTERID = ITRETAILGROUPS.DEPARTMENTMASTERID AND it.ITEMID=  @itemID )  
			                OR (L.PRODUCTTYPE = 3) -- All
			                OR (L.PRODUCTTYPE = 5 AND L.TARGETMASTERID = sp.MASTERID AND SGI.ITEMID =  @itemID )
			                OR (L.PRODUCTTYPE = 10 AND L.TARGETID = @itemID)
			                )", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });
                List<Join> joins = new List<Join>(fixedJoins);
                joins.Add(
                    new Join
                    {
                        Condition = " mmg.OFFERID = L.OFFERID and mmg.LINEGROUP = L.LINEGROUP",
                        JoinType = "LEFT OUTER",
                        Table = "POSMMLINEGROUPS",
                        TableAlias = "mmg"
                    });
                joins.Add(new Join
                {
                    Condition = "sgi.GROUPMASTERID = sp.MASTERID ",
                    JoinType = "LEFT OUTER",
                    Table = "SPECIALGROUPITEMS ",
                    TableAlias = "SGI"
                });
                List<TableColumn> columns = new List<TableColumn>(selectionColumns);
                columns.AddRange(mmgColumns);

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

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
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { ConditionValue = "P.PDTYPE = 1", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = @" (
                                  (L.PRODUCTTYPE = 1 AND L.ID =  @retailGroupID) 
                                  OR (L.PRODUCTTYPE = 2 AND L.ID =  ISNULL(rg.DEPARTMENTID, ''))
                                  OR (L.PRODUCTTYPE = 3) -- All
                                  ) ", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });

                List<Join> joins = new List<Join>(fixedJoins);
                joins.Add(
                    new Join
                    {
                        Condition = " mmg.OFFERID = L.OFFERID and mmg.LINEGROUP = L.LINEGROUP",
                        JoinType = "LEFT OUTER",
                        Table = "POSMMLINEGROUPS",
                        TableAlias = "mmg"
                    });
                List<TableColumn> columns = new List<TableColumn>(selectionColumns);
                columns.AddRange(mmgColumns);

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

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
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "L.POSPERIODICDISCOUNTLINEGUID = @POSPERIODICDISCOUNTLINEGUID", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });

                List<Join> joins = new List<Join>(fixedJoins);
                joins.Add(
                    new Join
                    {
                        Condition = " mmg.OFFERID = L.OFFERID and mmg.LINEGROUP = L.LINEGROUP",
                        JoinType = "LEFT OUTER",
                        Table = "POSMMLINEGROUPS",
                        TableAlias = "mmg"
                    });

                List<TableColumn> columns = new List<TableColumn>(selectionColumns);
                columns.AddRange(mmgColumns);

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Format("ORDER BY {0}", "LINEGROUP,DESCRIPTION"));

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
        public virtual List<DiscountOfferLine> GetMixAndMatchLines(IConnectionManager entry, RecordIdentifier offerID,
            DiscountOffer.MixAndMatchDiscountTypeEnum mixAndMatchType)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "L.OFFERID = @offerID", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });

                List<Join> joins = new List<Join>(fixedJoins);
                joins.Add(
                    new Join
                    {
                        Condition = " mmg.OFFERID = L.OFFERID and mmg.LINEGROUP = L.LINEGROUP",
                        JoinType = "LEFT OUTER",
                        Table = "POSMMLINEGROUPS",
                        TableAlias = "mmg"
                    });
                List<TableColumn> columns = new List<TableColumn>(selectionColumns);
                columns.AddRange(mmgColumns);

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Format("ORDER BY {0}", "LINEGROUP,DESCRIPTION"));

                MakeParam(cmd, "offerID", (string) offerID);

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
        public virtual List<MasterIDEntity> GetSimpleLines(IConnectionManager entry, RecordIdentifier offerID, DiscountOfferLine.DiscountOfferTypeEnum type)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add( new Condition { ConditionValue = "L.OFFERID = @offerID", Operator = "AND" });
                conditions.Add( new Condition { ConditionValue = "L.PRODUCTTYPE = @productType", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L"),
                    QueryPartGenerator.InternalColumnGenerator(simpleColumns),
                    QueryPartGenerator.JoinGenerator(simpleJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

                MakeParam(cmd, "offerID", (string)offerID);
                MakeParam(cmd, "productType", (int)type);

                return Execute<MasterIDEntity>(entry, cmd, CommandType.Text, PopulateListMasterID);
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
        public virtual List<MasterIDEntity> GetSimpleLines(IConnectionManager entry, RecordIdentifier offerID, RecordIdentifier lineGroupID, DiscountOfferLine.DiscountOfferTypeEnum type)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "L.OFFERID = @offerID", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = "L.PRODUCTTYPE = @productType", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = "L.LINEGROUP = @lineGroup", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L"),
                    QueryPartGenerator.InternalColumnGenerator(simpleColumns),
                    QueryPartGenerator.JoinGenerator(simpleJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

                MakeParam(cmd, "offerID", (string)offerID);
                MakeParam(cmd, "productType", (int)type);
                MakeParam(cmd, "lineGroup", (string)lineGroupID);

                return Execute<MasterIDEntity>(entry, cmd, CommandType.Text, PopulateListMasterID);
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
                "PERIODICDISCOUNTLINE",
                "POSPERIODICDISCOUNTLINEGUID", 
                offerLineID,false, true);
        }

        /// <summary>
        /// Deletes a discount line.
        /// </summary>
        /// <remarks>Permission needed: Manage discounts</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerLineID">The id of the line to delete. Note this is double identifier, OFFERID and LINEID</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier offerLineID)
        {
            DiscountOfferLine discountOfferLine = Get(entry, offerLineID);

            ValidateSecurity(entry, Permission.ManageDiscounts);
            var statement = new SqlServerStatement("PERIODICDISCOUNTLINE");

            statement.StatementType = StatementType.Update;

            statement.AddCondition("OFFERID", (string)discountOfferLine.OfferID);
            statement.AddCondition("POSPERIODICDISCOUNTLINEGUID", (Guid)offerLineID,SqlDbType.UniqueIdentifier);
            statement.AddCondition("LINEID", (int)discountOfferLine.LineID, SqlDbType.Int);

            statement.AddField("DELETED", true, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void DeletePromotion(IConnectionManager entry, RecordIdentifier id)
        {
            ValidateSecurity(entry, Permission.ManageDiscounts);
            var statement = new SqlServerStatement("PERIODICDISCOUNTLINE");

            statement.StatementType = StatementType.Update;

            statement.AddCondition("POSPERIODICDISCOUNTLINEGUID", (Guid)id, SqlDbType.UniqueIdentifier);
            statement.AddField("DELETED", true, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);

        }
        /// <summary>
        /// Deletes a discount line by relation and relation type. For example by retail item id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The promotion offer ID</param>
        /// <param name="relation">The relation ID</param>
        /// <param name="relationType">The relation type</param>
        public virtual void DeletePromotionByRelation(IConnectionManager entry, RecordIdentifier offerID, RecordIdentifier relation, DiscountOfferLine.DiscountOfferTypeEnum relationType)
        {
            ValidateSecurity(entry, Permission.ManageDiscounts);
            var statement = new SqlServerStatement("PERIODICDISCOUNTLINE");

            statement.StatementType = StatementType.Update;

            statement.AddCondition("OFFERID", (string)offerID);
            if (relation.IsGuid)
            {
                statement.AddCondition("TARGETMASTERID", (Guid)relation, SqlDbType.UniqueIdentifier);

            }
            else
            {
                statement.AddCondition("TARGETID", (string)relation);
            }
            statement.AddCondition("PRODUCTTYPE", (int)relationType, SqlDbType.Int);
            statement.AddField("DELETED", true, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);
        }
        /// <summary>
        /// Saves a offer line into the database.
        /// </summary>
        /// <remarks>Permission needed: Manage discounts</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerLine">The offer line to be saved</param>
        public virtual void Save(IConnectionManager entry, DiscountOfferLine offerLine)
        {
            var statement = new SqlServerStatement("PERIODICDISCOUNTLINE");

            ValidateSecurity(entry, Permission.ManageDiscounts);
            
            bool isNew = false;
            if (offerLine.ID == RecordIdentifier.Empty || offerLine.ID == null)
            {
                isNew = true;
                offerLine.ID = Guid.NewGuid();
                offerLine.LineID = GenerateLineID(entry, offerLine.OfferID);
            }

            if (isNew || !Exists(entry, offerLine.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("OFFERID", (string)offerLine.OfferID);

                statement.AddKey("POSPERIODICDISCOUNTLINEGUID", (Guid)offerLine.ID, SqlDbType.UniqueIdentifier);
                statement.AddKey("LINEID", (int)offerLine.LineID, SqlDbType.Int);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("OFFERID", (string)offerLine.OfferID);
                statement.AddCondition("POSPERIODICDISCOUNTLINEGUID", (Guid)offerLine.ID, SqlDbType.UniqueIdentifier);
                statement.AddCondition("LINEID", (int)offerLine.LineID, SqlDbType.Int);
            }
            if (offerLine.OfferMasterID == null)
            {
                offerLine.OfferMasterID = GetMasterID(entry, offerLine.OfferID, "PERIODICDISCOUNT","OFFERID");
            }
            statement.AddField("OFFERMASTERID", (Guid) offerLine.OfferMasterID, SqlDbType.UniqueIdentifier);
            statement.AddField("DISCTYPE", (int)offerLine.DiscountType, SqlDbType.Int);
            statement.AddField("PRODUCTTYPE", (int)offerLine.Type, SqlDbType.Int);
            statement.AddField("LINEGROUP", (string)offerLine.LineGroup);
            if (RecordIdentifier.IsEmptyOrNull(offerLine.ItemRelation))
            {
                switch (offerLine.Type)
                {
                    case DiscountOfferLine.DiscountOfferTypeEnum.Item:
                        offerLine.ItemRelation = GetReadableIDFromMasterID(entry, offerLine.TargetMasterID, "RETAILITEM", "ITEMID");
                        break;
                    case DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup:
                        offerLine.ItemRelation = GetReadableIDFromMasterID(entry, offerLine.TargetMasterID, "RETAILGROUP", "GROUPID");
                        break;
                    case DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment:
                        offerLine.ItemRelation = GetReadableIDFromMasterID(entry, offerLine.TargetMasterID, "RETAILDEPARTMENT", "DEPARTMENTID");
                        break;
                    case DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup:
                        offerLine.ItemRelation = GetReadableIDFromMasterID(entry, offerLine.TargetMasterID, "SPECIALGROUP", "GROUPID");
                        break;

                }
            }

            statement.AddField("TARGETID", (string)offerLine.ItemRelation);
            statement.AddField("UNIT", (string)offerLine.Unit);
            statement.AddField("TARGETMASTERID", (Guid) offerLine.TargetMasterID, SqlDbType.UniqueIdentifier);
            statement.AddField("DEALPRICEORDISCPCT", offerLine.DiscountPercent, SqlDbType.Decimal);
            if (offerLine is PromotionOfferLine)
            {
                PromotionOfferLine promotionOfferLine = (PromotionOfferLine) offerLine;
                statement.AddField("DISCPCT", offerLine.DiscountPercent, SqlDbType.Decimal);
                statement.AddField("DISCAMOUNT", promotionOfferLine.DiscountAmount, SqlDbType.Decimal);
                statement.AddField("DISCAMOUNTINCLTAX", promotionOfferLine.DiscountamountIncludeTax, SqlDbType.Decimal);
                statement.AddField("OFFERPRICE", promotionOfferLine.OfferPrice, SqlDbType.Decimal);
                statement.AddField("OFFERPRICEINCLTAX", promotionOfferLine.OfferPriceIncludeTax, SqlDbType.Decimal);

            }
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
            if (entry.HasPermission(Permission.ManageDiscounts))
            {

                SqlServerStatement statement = new SqlServerStatement("PERIODICDISCOUNTLINE");
                statement.StatementType = StatementType.Update;

                statement.AddField("DELETED", true, SqlDbType.Bit);
                statement.AddCondition("OFFERID", (string)offerID, SqlDbType.VarChar);
                statement.AddCondition("TARGETMASTERID", (Guid)relation,SqlDbType.UniqueIdentifier);
                statement.AddCondition("PRODUCTTYPE", (int)relationType, SqlDbType.Int);

                entry.Connection.ExecuteStatement(statement);
            }
            else
            {
                throw new PermissionException(Permission.ManageDiscounts);
            }
            
        }
            
        public virtual void DeleteDiscountLinesForVariant(IConnectionManager entry, RecordIdentifier variant)
        {
          //  DeleteRecord(entry,
                         //"PERIODICDISCOUNTLINE",
                         //new[] { "TARGETID", "PRODUCTTYPE" },
                         //new RecordIdentifier(variant, (int)DiscountOfferLine.DiscountOfferTypeEnum.Variant),
                         //BusinessObjects.Permission.ManageDiscounts);
        }

        public List<PromotionOfferLine> GetPromotionsForItem(
           IConnectionManager entry,
           RecordIdentifier itemID,
           RecordIdentifier storeID,
           RecordIdentifier customerID,
           bool checkPriceGroup = true,
           bool checkCustomerConnection = true)
        {
            var item = Providers.RetailItemData.Get(entry, itemID);

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>(selectionColumns);
                columns.AddRange(promotionColumns);

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "P.PDTYPE = 3", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = "P.STATUS = 1", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });

                List<Condition> relationConditions = new List<Condition>();

                if (item.ItemType == BusinessObjects.Enums.ItemTypeEnum.MasterItem)
                {
                    relationConditions.Add(new Condition
                    {
                        Operator = "OR",
                        ConditionValue = @"(L.PRODUCTTYPE = 0 AND (
                                     L.TARGETID = @ITEMID OR
                                     L.TARGETID IN (SELECT
                                        ITVARIANT.ITEMID
                                        FROM RETAILITEM ITVARIANT
                                        JOIN RETAILITEM ITHEADER ON ITHEADER.MASTERID = ITVARIANT.HEADERITEMID
                                        WHERE ITHEADER.ITEMID = @ITEMID)  )
                                    ) "
                    });
                }
                else if (item.ItemType != BusinessObjects.Enums.ItemTypeEnum.MasterItem)
                {
                    relationConditions.Add(new Condition
                    {
                        Operator = "OR",
                        ConditionValue = @"(L.PRODUCTTYPE = 0 AND L.TARGETID = @ITEMID OR L.TARGETMASTERID = @ITEMHEADERMASTERID) "
                    });
                    MakeParam(cmd, "ITEMHEADERMASTERID", (Guid)item.HeaderItemID);
                }

                relationConditions.Add(new Condition
                {
                    Operator = "OR",
                    ConditionValue = "(L.PRODUCTTYPE = 1 AND L.TARGETMASTERID = @RETAILGROUPID) "
                });
                relationConditions.Add(new Condition
                {
                    Operator = "OR",
                    ConditionValue = "(L.PRODUCTTYPE = 2 AND L.TARGETMASTERID = @RETAILDEPARTMENTID) "
                });
                relationConditions.Add(new Condition { Operator = "OR", ConditionValue = "(L.PRODUCTTYPE = 3) " });
                relationConditions.Add(new Condition
                {
                    Operator = "OR",
                    ConditionValue =
                        "(L.PRODUCTTYPE = 5 AND L.TARGETID IN (SELECT GROUPID FROM SPECIALGROUPITEMS WHERE ITEMID = @ITEMID)) "
                });
                
                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = $"({QueryPartGenerator.ConditionGenerator(relationConditions, true)})"
                });

                List<Condition> priceGroupConditions = new List<Condition>();

                if (checkPriceGroup)
                {
                    priceGroupConditions.Add(new Condition { Operator = "OR", ConditionValue = "(P.PRICEGROUP = '') " });
                }

                if (customerID != RecordIdentifier.Empty)
                {
                    priceGroupConditions.Add(new Condition
                    {
                        Operator = "OR",
                        ConditionValue =
                            "(P.PRICEGROUP in (SELECT PRICEGROUP FROM CUSTOMER WHERE ACCOUNTNUM = @CUSTOMERID)) "
                    });
                    MakeParam(cmd, "CUSTOMERID", (string)customerID);

                }
                if (storeID != RecordIdentifier.Empty)
                {
                    priceGroupConditions.Add(new Condition
                    {
                        Operator = "OR",
                        ConditionValue =
                            "(P.PRICEGROUP in (SELECT PRICEGROUPID FROM RBOLOCATIONPRICEGROUP WHERE STOREID = @STOREID)) "
                    });
                    MakeParam(cmd, "STOREID", (string)storeID);
                }
                if (priceGroupConditions.Count > 0)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = $"({QueryPartGenerator.ConditionGenerator(priceGroupConditions, true)})"
                    });
                }
                if (checkCustomerConnection)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = @"(
                            P.ACCOUNTCODE = 0 OR
                            P.ACCOUNTCODE = 1 AND P.ACCOUNTRELATION = @ACCOUNTRELATION OR
                            P.ACCOUNTCODE = 2 AND @ACCOUNTRELATION IN (SELECT ACCOUNTNUM FROM CUSTOMER WHERE LINEDISC = P.ACCOUNTRELATION)                         
                          )"
                    });

                    MakeParam(cmd, "ACCOUNTRELATION", (string)customerID);
                }

                List<Join> joins = new List<Join>(fixedJoins);
                
                cmd.CommandText =
                    string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L"),
                        QueryPartGenerator.InternalColumnGenerator(columns),
                        QueryPartGenerator.JoinGenerator(joins),
                        QueryPartGenerator.ConditionGenerator(conditions),
                        string.Empty
                        );              

                MakeParam(cmd, "ITEMID", (string)item.ID);
                MakeParam(cmd, "RETAILGROUPID", (Guid)item.RetailGroupMasterID);
                MakeParam(cmd, "RETAILDEPARTMENTID", (Guid)item.RetailDepartmentMasterID);

                return Execute<PromotionOfferLine>(entry, cmd, CommandType.Text, PopulatePromotionLine);
            }
        }

        /// <summary>
        /// Returns a list of valid promotions that the selected item is in.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The ID of the item to check for</param>
        /// <param name="storeID">The ID of the store we are working in. Used to check for valid price group settings. Set to RecordIdentifier.Empty if no store context is available</param>
        /// <param name="customerID">The ID of the customer we are using. Used to check for valid price group settings. Set to RecordIdentifier.Empty if no customer context is available</param>
        public List<PromotionOfferLine> GetValidAndEnabledPromotionsForItem(
            IConnectionManager entry,
            RecordIdentifier itemID,
            RecordIdentifier storeID,
            RecordIdentifier customerID)
        {
            var allPromotionsForItem = GetPromotionsForItem(entry, itemID,  storeID, customerID);
            var validPromotionOfferLines = allPromotionsForItem.Where(x => (((string)x.ValidationPeriodID == ""
                                                                            || Providers.DiscountPeriodData.IsDiscountPeriodValid(entry, x.ValidationPeriodID, DateTime.Now))
                                                                            && x.InActiveDiscount)).ToList();

            return validPromotionOfferLines;
        }

        /// <summary>
        /// Gets a list of data entities containing the IDs and names of the retail items and groups belonging to the promotion offer lines
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The promotion offer ID</param>
        /// <param name="type">The type of promotion offer line to get</param>
        /// <returns></returns>
        public List<MasterIDEntity> GetSimplePromotionLines(IConnectionManager entry, RecordIdentifier offerID,
                                                      DiscountOfferLine.DiscountOfferTypeEnum type)

        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "L.OFFERID = @offerID", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = "PRODUCTTYPE = @type", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "L.DELETED = 0 " });

                List<TableColumn> columns = new List<TableColumn>(simpleColumns);                

                cmd.CommandText =
                    string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNTLINE", "L"),
                        QueryPartGenerator.InternalColumnGenerator(columns),
                        QueryPartGenerator.JoinGenerator(simpleJoins),
                        QueryPartGenerator.ConditionGenerator(conditions),
                        string.Empty
                        );

                MakeParam(cmd, "offerID", (string)offerID);
                MakeParam(cmd, "type", (int)type);

                return Execute<MasterIDEntity>(entry, cmd, CommandType.Text,  PopulateListMasterID);
            }
        }
    }


}
