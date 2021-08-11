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
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.PricesAndDiscounts
{
    public class DiscountOfferData : SqlServerDataProviderBase, IDiscountOfferData
    {
        
        private string sequenceType;

        private static List<TableColumn> selectionColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "MASTERID", TableAlias = "G"},
            new TableColumn {ColumnName = "OFFERID", TableAlias = "G"},
            new TableColumn {ColumnName = "ISNULL(G.DESCRIPTION, '')", ColumnAlias = "DESCRIPTION"},
            new TableColumn {ColumnName = "ISNULL(G.DISCOUNTPCTVALUE, 0)", ColumnAlias = "DISCOUNTPCTVALUE"},
            new TableColumn {ColumnName = "ISNULL(G.DEALPRICEVALUE, 0)", ColumnAlias = "DEALPRICEVALUE"},
            new TableColumn {ColumnName = "ISNULL(G.DISCOUNTAMOUNTVALUE, 0)", ColumnAlias = "DISCOUNTAMOUNTVALUE"},
            new TableColumn {ColumnName = "ISNULL(G.NOOFLEASTEXPITEMS, 0)", ColumnAlias = "NOOFLEASTEXPITEMS"},
            new TableColumn {ColumnName = "ISNULL(G.STATUS, 0)", ColumnAlias = "STATUS"},
            new TableColumn {ColumnName = "ISNULL(G.PRIORITY, 0)", ColumnAlias = "PRIORITY"},
            new TableColumn {ColumnName = "ISNULL(G.PDTYPE, 0)", ColumnAlias = "PDTYPE"},
            new TableColumn {ColumnName = "ISNULL(G.DISCOUNTTYPE, 0)", ColumnAlias = "DISCOUNTTYPE"},
            new TableColumn {ColumnName = "ISNULL(G.DISCVALIDPERIODID, '')", ColumnAlias = "DISCVALIDPERIODID"},
            new TableColumn {ColumnName = "ISNULL(G.PRICEGROUP, '')", ColumnAlias = "PRICEGROUP"},
            new TableColumn {ColumnName = "ISNULL(h.NAME,'')", ColumnAlias = "PRICEGROUPNAME"},
            new TableColumn
            {
                ColumnName = "ISNULL(p.DESCRIPTION,'')",
                ColumnAlias = "DISCOUNTVALIDATIONPERIODIDDESCRIPTION"
            },
            new TableColumn {ColumnName = "STARTINGDATE", TableAlias = "P"},
            new TableColumn {ColumnName = "ENDINGDATE", TableAlias = "P"},
            new TableColumn {ColumnName = "ISNULL(G.ACCOUNTCODE, 0)", ColumnAlias = "ACCOUNTCODE"},
            new TableColumn {ColumnName = "ISNULL(G.ACCOUNTRELATION, '')", ColumnAlias = "ACCOUNTRELATION"},
            new TableColumn {ColumnName = "ISNULL(G.TRIGGERED, 0)", ColumnAlias = "TRIGGERED"},
            new TableColumn {ColumnName = "ISNULL(c.NAME, '')", ColumnAlias = "CUSTOMERNAME"},
            new TableColumn {ColumnName = "ISNULL(pdG.NAME, '')", ColumnAlias = "CUSTOMERGROUPNAME"},
            new TableColumn {ColumnName = "ISNULL(G.BARCODE, '')", ColumnAlias = "BARCODE"},
        };

        private TableColumn mmColumn = new TableColumn
        {
            ColumnName = "ISNULL(l.NOOFITEMSNEEDED,0)",
            ColumnAlias = "NOOFITEMSNEEDED"
        };

        private List<Join> fixedJoins = new List<Join>
        {
           
             new Join
            {
                Condition = " G.DISCVALIDPERIODID = p.ID",
                JoinType = "LEFT OUTER",
                Table = "POSDISCVALIDATIONPERIOD",
                TableAlias = "p"
            },
            new Join
            {
                Condition = " G.PRICEGROUP = h.GROUPID  and h.MODULE = 1 and h.TYPE = 0",
                JoinType = "LEFT OUTER",
                Table = "PRICEDISCGROUP",
                TableAlias = "h"
            },

            new Join
            {
                Condition = " c.ACCOUNTNUM = G.ACCOUNTRELATION ",
                JoinType = "LEFT OUTER",
                Table = "CUSTOMER",
                TableAlias = "c"
            },
            new Join
            {
                Condition = " pdG.MODULE = 1 and pdG.TYPE = 1 and pdG.GROUPID = G.ACCOUNTRELATION",
                JoinType = "LEFT OUTER",
                Table = "PRICEDISCGROUP",
                TableAlias = "pdg"
            }

        };

        private Join mmJoin = new Join
        {
            Condition = " G.OFFERID = l.OFFERID",
            JoinType = "LEFT OUTER",
            Table = @"(Select
                         PLG.OFFERID,
                         SUM(PLG.NOOFITEMSNEEDED) as NOOFITEMSNEEDED
                            from POSMMLineGroups PLG
                            Where PLG.LINEGROUP in (select LINEGROUP from PERIODICDISCOUNTLINE where OFFERID = plG.OFFERID)
                            GROUP BY PLG.OFFERID
                        )",
            TableAlias = "l"
        };

        private static string ResolveSort(DiscountOfferSorting sortBy, bool backwards)
        {
            switch (sortBy)
            {
                case DiscountOfferSorting.OfferNumber:
                    return backwards ? "G.OFFERID DESC" : "G.OFFERID ASC";
                    
                case DiscountOfferSorting.Description:
                    return backwards ? "G.DESCRIPTION DESC" : "G.DESCRIPTION ASC";
                    
                case DiscountOfferSorting.Priority:
                    return backwards ? "G.PRIORITY DESC" : "G.PRIORITY ASC";
                    
                case DiscountOfferSorting.OfferType:
                    return backwards ? "G.PDTYPE DESC" : "G.PDTYPE ASC";

                case DiscountOfferSorting.Status:
                    return backwards ? "G.STATUS DESC" : "G.STATUS ASC";

                case DiscountOfferSorting.DiscountType:
                    return backwards ? "G.DISCOUNTTYPE DESC" : "G.DISCOUNTTYPE ASC";

                case DiscountOfferSorting.DiscountValidationPeriod:
                    return backwards ? "G.DISCVALIDPERIODID DESC" : "G.DISCVALIDPERIODID ASC";

                case DiscountOfferSorting.StartingDate:
                    return backwards ? "p.STARTINGDATE DESC" : "p.STARTINGDATE ASC";

                case DiscountOfferSorting.EndingDate:
                    return backwards ? "p.ENDINGDATE DESC" : "p.ENDINGDATE ASC";

                case DiscountOfferSorting.NumberOfItemsNeeded:
                    return backwards ? "l.NOOFITEMSNEEDED DESC" : "l.NOOFITEMSNEEDED ASC";

                case DiscountOfferSorting.DiscountPercentValue:
                    return backwards ? "G.DISCOUNTPCTVALUE DESC" : "G.DISCOUNTPCTVALUE ASC";

                default:
                    throw new ArgumentOutOfRangeException("sortBy");
            }
        }

        private void SetSequenceType(DiscountOffer.PeriodicDiscountOfferTypeEnum type)
	    {
            switch(type)
            {
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch:
                    sequenceType = "R-MIXM";
                    break;

                case DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer:
                    sequenceType = "R-DISCO";
                    break;

                case DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy:
                    sequenceType = "R-MULT-G";
                    break;

                case DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion:
                    sequenceType = "Promotions";
                    break;
            }
	    }

        private static void PopulateOffer(IDataReader dr, DiscountOffer offer)
        {
            offer.ID = (string)dr["OFFERID"];
            offer.MasterID = (Guid)dr["MASTERID"];
            offer.Text = (string)dr["DESCRIPTION"];
            offer.DiscountPercent = (decimal)dr["DISCOUNTPCTVALUE"];
            offer.DealPrice = (decimal)dr["DEALPRICEVALUE"];
            offer.DiscountAmount = (decimal)dr["DISCOUNTAMOUNTVALUE"];
            offer.NumberOfLeastExpensiveLines = (int)dr["NOOFLEASTEXPITEMS"];
            offer.Enabled = ((byte)dr["STATUS"] != 0);
            offer.StartingDate = Date.FromAxaptaDate(dr["STARTINGDATE"]);
            offer.EndingDate = Date.FromAxaptaDate(dr["ENDINGDATE"]);
            offer.Priority = (int)dr["PRIORITY"];
            offer.OfferType = (DiscountOffer.PeriodicDiscountOfferTypeEnum)dr["PDTYPE"];
            offer.DiscountTypeValue = (int)dr["DISCOUNTTYPE"];
            offer.ValidationPeriod = (string)dr["DISCVALIDPERIODID"];
            offer.ValidationPeriodDescription = (string)dr["DISCOUNTVALIDATIONPERIODIDDESCRIPTION"];
            offer.PriceGroup = (string)dr["PRICEGROUP"];
            offer.PriceGroupName = (string)dr["PRICEGROUPNAME"];
            offer.AccountCode = (DiscountOffer.AccountCodeEnum)((int) dr["ACCOUNTCODE"]);
            offer.AccountRelation = (string)dr["ACCOUNTRELATION"];
            offer.CustomerName = (string)dr["CUSTOMERNAME"];
            offer.CustomerGroupName = (string)dr["CUSTOMERGROUPNAME"];
            offer.Triggering = (DiscountOffer.TriggeringEnum)dr["TRIGGERED"];
            offer.BarCode = (string)dr["BARCODE"];
        }

        private static void PopulateMixMatchOffer(IDataReader dr, DiscountOffer offer)
        {
            PopulateOffer(dr,offer);

            offer.NumberOfItemsNeeded = (int)dr["NOOFITEMSNEEDED"];
        }

        /// <summary>
        /// Gets name of a discount offer from a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerID">ID of the discount offer</param>
        /// <returns>The discount offer name or empty string if not found</returns>
        public virtual string GetDiscountName(IConnectionManager entry,RecordIdentifier offerID)
        {
            if(offerID == "")
            {
                return "";
            }

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT COALESCE(DESCRIPTION, '') as OFFERNAME FROM PERIODICDISCOUNT
                                    WHERE OFFERID = @offerID ";

                MakeParam(cmd, "offerID", (string)offerID);

                object value = entry.Connection.ExecuteScalar(cmd);

                return (value == DBNull.Value) ? "" : (string)value;
            }
        }

        public virtual string GetFromBarcode(IConnectionManager entry, string barcode)
        {
            ValidateSecurity(entry);
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT OFFERID FROM PERIODICDISCOUNT WHERE BARCODE = @barcode ";

                MakeParam(cmd, "barcode", (string)barcode);

                var result = entry.Connection.ExecuteScalar(cmd);

                return ((string)result != "") ? (string)result : "";
            }
        }

        public virtual DiscountOffer Get(IConnectionManager entry, RecordIdentifier offerID,
            DiscountOffer.PeriodicDiscountOfferTypeEnum type)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = selectionColumns.ToList();
                List<Join> joins = fixedJoins.ToList();

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "G.OFFERID = @offerID", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "G.DELETED = 0 " });

                DataPopulator<DiscountOffer> populator;
                if (type == DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch)
                {
                    columns.Add(mmColumn);
                    joins.Add(mmJoin);
                    populator = PopulateMixMatchOffer;
                }
                else
                {
                    populator = PopulateOffer;
                }
                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNT ", "G"),
                   QueryPartGenerator.InternalColumnGenerator(columns),
                   QueryPartGenerator.JoinGenerator(joins),
                   QueryPartGenerator.ConditionGenerator(conditions),
                   string.Empty);

                MakeParam(cmd, "offerID", (string) offerID);

                var results = Execute(entry, cmd, CommandType.Text, populator);
                return results.Count > 0 ? results[0] : null;
            }
        }

        /// <summary>
        /// Returns a sorted list of all periodic discounts (Discount offer, Mix and Match, Multibuy, Promotines)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="excludePromotions">Controls wether promotions are excluded from the results</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns></returns>
        public virtual List<DiscountOffer> GetPeriodicDiscounts(IConnectionManager entry, bool excludePromotions, DiscountOfferSorting sortBy, bool sortBackwards)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "PDTYPE <> 3", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "G.DELETED = 0 " });
                

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNT ", "G"),
                   QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                   QueryPartGenerator.JoinGenerator(fixedJoins),
                   QueryPartGenerator.ConditionGenerator(conditions),
                    " ORDER BY " + ResolveSort(sortBy, sortBackwards));

                return Execute<DiscountOffer>(entry, cmd, CommandType.Text, PopulateOffer);
            }
        }

        public virtual int GetNumberOfDiscountsExpiringOverTheNext7Days(IConnectionManager entry)
        {
            DateTime today = DateTime.Now;

            today = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);

            DateTime weekFromNow = today.AddDays(7);

            weekFromNow = new DateTime(weekFromNow.Year, weekFromNow.Month, weekFromNow.Day, 23, 59, 59);

            List<Condition> conditions = new List<Condition>();

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                conditions.Add(new Condition {ConditionValue = "PDTYPE <> 3", Operator = "AND"});
                conditions.Add(new Condition {ConditionValue = "discount.STATUS = 1", Operator = "AND"});
                conditions.Add(new Condition
                {
                    ConditionValue = "period.ENDINGDATE between @today and @weekFromNow",
                    Operator = "AND"
                });

                Join join = new Join
                {
                Condition = " period.ID = discount.DISCVALIDPERIODID AND discount.DELETED = 0",
                    JoinType = "RIGHT OUTER",
                    Table = "PERIODICDISCOUNT",
                    TableAlias = "discount"
                };
                TableColumn column = new TableColumn {ColumnName = "COUNT(1)"};
                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("POSDISCVALIDATIONPERIOD ", "period"),
                    column,
                    join,
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

                MakeParam(cmd, "today", today, SqlDbType.DateTime);
                MakeParam(cmd, "weekFromNow", weekFromNow, SqlDbType.DateTime);

                return (int) entry.Connection.ExecuteScalar(cmd);
            }
        }

        public virtual int GetNumberOfActiveDiscounts(IConnectionManager entry)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { ConditionValue = "PDTYPE <> 3", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = "discount.STATUS = 1", Operator = "AND" });
                conditions.Add(new Condition
                {
                    // Note that "CONVERT(VARCHAR(10),GETDATE(),111)" here above is SQL Server technique to get current date without time.
                    ConditionValue = "(discount.DISCVALIDPERIODID = '' or (CONVERT(VARCHAR(10),GETDATE(),111) between period.STARTINGDATE and period.ENDINGDATE ))",
                    Operator = "AND"
                });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "discount.DELETED = 0 " });

                Join join = new Join
                {
                    Condition = " period.ID = discount.DISCVALIDPERIODID ",
                    JoinType = "LEFT OUTER",
                    Table = "POSDISCVALIDATIONPERIOD",
                    TableAlias = "period"
                };
                TableColumn column = new TableColumn { ColumnName = "COUNT(1)" };
                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNT ", "discount"),
                    column,
                    join,
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

                return (int)entry.Connection.ExecuteScalar(cmd);
            }
        }

        public virtual bool DiscountsAreConfigured(IConnectionManager entry)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"If Exists(select 'x' from PERIODICDISCOUNT discount
                      where discount.PDTYPE <> 3) select 1 else select 0";

                return ((int)entry.Connection.ExecuteScalar(cmd) == 1);
            }
        }

        /// <summary>
        /// Returns a sorted list of all periodic discounts (Discount offer, Mix and Match, Multibuy, Promotines)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns></returns>
        public virtual List<DiscountOffer> GetManuallyTriggeredPeriodicDiscounts(IConnectionManager entry, DiscountOfferSorting sortBy, bool sortBackwards)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "G.TRIGGERED = 1", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "G.DELETED = 0 " });
                

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNT ", "G"),
                   QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                   QueryPartGenerator.JoinGenerator(fixedJoins),
                   QueryPartGenerator.ConditionGenerator(conditions),
                    " ORDER BY " + ResolveSort(sortBy, sortBackwards));

                return Execute<DiscountOffer>(entry, cmd, CommandType.Text, PopulateOffer);
            }
        }

        /// <summary>
        /// Returns a sorted list of all periodic dicsounts for a given line discount group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="lineDiscountGroupID">The ID of the line discount group</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns></returns>
        public virtual List<DiscountOffer> GetOffersForLineDiscountGroup(IConnectionManager entry, RecordIdentifier lineDiscountGroupID, DiscountOfferSorting sortBy, bool sortBackwards)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add( new Condition { ConditionValue = "PDTYPE <> 3", Operator = "AND" });
                conditions.Add( new Condition { ConditionValue = " G.ACCOUNTRELATION = @accountRelation", Operator = "AND" });
                conditions.Add( new Condition { ConditionValue = " G.ACCOUNTCODE = 2", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "G.DELETED = 0 " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNT ", "G"),
                   QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                   QueryPartGenerator.JoinGenerator(fixedJoins),
                   QueryPartGenerator.ConditionGenerator(conditions),
                    " ORDER BY " + ResolveSort(sortBy, sortBackwards));

                MakeParam(cmd, "accountRelation", (string)lineDiscountGroupID);

                return Execute<DiscountOffer>(entry, cmd, CommandType.Text, PopulateOffer);
            }            
        }

        /// <summary>
        /// Returns a sorted list of all promotions for a given line discount group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="lineDiscountGroupID">The ID of the line discount group</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns></returns>
        public virtual List<DiscountOffer> GetPromotionsForLineDiscountGroup(IConnectionManager entry, RecordIdentifier lineDiscountGroupID, DiscountOfferSorting sortBy, bool sortBackwards)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "PDTYPE = 3", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = " G.ACCOUNTRELATION = @accountRelation", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = " G.ACCOUNTCODE = 2", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "G.DELETED = 0 " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNT ", "G"),
                   QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                   QueryPartGenerator.JoinGenerator(fixedJoins),
                   QueryPartGenerator.ConditionGenerator(conditions),
                    " ORDER BY " + ResolveSort(sortBy, sortBackwards));

                MakeParam(cmd, "accountRelation", (string)lineDiscountGroupID);

                return Execute<DiscountOffer>(entry, cmd, CommandType.Text, PopulateOffer);
            } 
        }

        /// <summary>
        /// Returns a sorted list of all periodic discounts for a given customer
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="filter">Controls wether promotions are excluded from the results</param>
        /// <param name="customerID">The ID of the customer</param>
        /// <returns></returns>
        public virtual List<DiscountOffer> GetForCustomer(IConnectionManager entry, DiscountOfferFilter filter, RecordIdentifier customerID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                switch (filter)
                {
                    case DiscountOfferFilter.AllDiscountOffers:
                        break;
                    case DiscountOfferFilter.AllExceptPromotions:
                        conditions.Add(new Condition { ConditionValue = "PDTYPE <> 3", Operator = "AND" });
                        break;
                    case DiscountOfferFilter.OnlyPromotions:
                        conditions.Add(new Condition { ConditionValue = "PDTYPE = 3", Operator = "AND" });
                        break;
                }
                
                conditions.Add(new Condition { ConditionValue = " G.ACCOUNTRELATION = @accountRelation", Operator = "AND" });
                conditions.Add(new Condition { ConditionValue = " G.ACCOUNTCODE = 1", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "G.DELETED = 0 " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNT ", "G"),
                   QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                   QueryPartGenerator.JoinGenerator(fixedJoins),
                   QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

                MakeParam(cmd, "accountRelation", (string)customerID);

                return Execute<DiscountOffer>(entry, cmd, CommandType.Text, PopulateOffer);
            }   
        }

        /// <summary>
        /// Gets a sorted list of all Discount offers, Mix and Match and Multiby periodic discounts.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns></returns>
        public virtual List<DiscountOffer> GetAllOffers(IConnectionManager entry, DiscountOfferSorting sortBy, bool sortBackwards)
        {
            return null;
        }
        
        /// <summary>
        /// Gets the next valid priority value that can be used for a periodic discount. 
        /// By default new priority values increment in steps of 10.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        public virtual int GetNextPriority(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"select ISNULL(MAX(PRIORITY), 0) AS PRIORITY
                      from PERIODICDISCOUNT
                     ";

                // Gets the next value that is divisible by 10.
                // This creates a priority series like 10, 20, 30... even if you have values like 22 or 34 in between.
                var maxPriority = (int)entry.Connection.ExecuteScalar(cmd);
                return maxPriority - (maxPriority % 10) + 10;
            }
        }        

        /// <summary>
        /// Gets a list of all priority values currently in use ordered in ascending order.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        public virtual List<int> GetPrioritiesInUse(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"select distinct PRIORITY
                      from PERIODICDISCOUNT
                      ORDER BY PRIORITY asc";

                var dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                var result = new List<int>();
                while (dr.Read())
                {
                    result.Add((int)dr["PRIORITY"]);
                }

                dr.Close();
                dr.Dispose();

                return result;
            }
        }

        /// <summary>
        /// Gets a list of periodic discounts
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="type">The periodic discount type to get</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="backwardsSort">Determines in what order the result is sorted</param>
        /// <returns></returns>
        public virtual List<DiscountOffer> GetOffers(IConnectionManager entry,
            DiscountOffer.PeriodicDiscountOfferTypeEnum type, DiscountOfferSorting sortBy, bool backwardsSort)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = selectionColumns.ToList();
                List<Join> joins = fixedJoins.ToList();
                DataPopulator<DiscountOffer> populator;

                if (type == DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch)
                {
                    columns.Add(mmColumn);
                    joins.Add(mmJoin);
                    populator = PopulateMixMatchOffer;
                }
                else
                {
                    populator = PopulateOffer;
                }
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "G.PDTYPE = @type", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "G.DELETED = 0 " });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNT ", "G"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    " ORDER BY " + ResolveSort(sortBy, backwardsSort));

                MakeParam(cmd, "type", (int) type, SqlDbType.Int);

                return Execute(entry, cmd, CommandType.Text, populator);
            }
        }

        public virtual List<DiscountOffer> GetOffers(IConnectionManager entry, DiscountOffer.PeriodicDiscountOfferTypeEnum type, int sortColumn, bool backwardsSort)
        {
            string sort = "";
            string[] columns;

            ValidateSecurity(entry);

            if (type == DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy)
            {
                columns = new[] { "G.OFFERID", "G.DESCRIPTION", "G.STATUS", "G.DISCOUNTTYPE", "G.DISCVALIDPERIODID", "p.STARTINGDATE", "p.ENDINGDATE" };
            }
            else if (type == DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch)
            {
                columns = new[] { "G.OFFERID", "G.DESCRIPTION", "G.STATUS", "G.DISCOUNTTYPE", "G.DISCOUNTTYPE", "p.NOOFITEMSNEEDED", "G.DISCVALIDPERIODID" };
            }
            else
            {
                columns = new[] { "G.OFFERID", "G.DESCRIPTION", "G.STATUS", "G.DISCOUNTPCTVALUE", "G.DISCVALIDPERIODID", "p.STARTINGDATE", "p.ENDINGDATE" };
            }

            if (sortColumn < columns.Length)
            {
                sort = " ORDER BY " + columns[sortColumn] + (backwardsSort ? " DESC" : " ASC");
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> queryColumns = selectionColumns.ToList();
                List<Join> joins = fixedJoins.ToList();

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "G.PDTYPE = @type", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "G.DELETED = 0 " });
                

                DataPopulator<DiscountOffer> populator;
                if (type == DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch)
                {
                    queryColumns.Add(mmColumn);
                    joins.Add(mmJoin);
                    populator = PopulateMixMatchOffer;
                }
                else
                {
                    populator = PopulateOffer;
                }
                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNT ", "G"),
                    QueryPartGenerator.InternalColumnGenerator(queryColumns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    sort);
              
                MakeParam(cmd, "type", (int)type, SqlDbType.Int);

                return Execute(entry, cmd, CommandType.Text, populator);
            }
        }

        public virtual DiscountOffer GetOfferFromLine(IConnectionManager entry, RecordIdentifier offerId)
        { 
            ValidateSecurity(entry);
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { ConditionValue = "G.OFFERID = @offerID", Operator = "AND" });
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "G.DELETED = 0 " });

              
                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNT ", "G"),
                   QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                   QueryPartGenerator.JoinGenerator(fixedJoins),
                   QueryPartGenerator.ConditionGenerator(conditions),
                   string.Empty);

                MakeParam(cmd, "offerID", (string)offerId);

                var result = Execute<DiscountOffer>(entry, cmd, CommandType.Text, PopulateOffer);

                return result.Count > 0 ? result[0] : null;
            }
        }

        /// <summary>
        /// Gets a list of all offers and promotions for the given relation i.e all offers for a give retail group or retail item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="relationID">The relation ID, f.ex a retial item ID</param>
        /// <param name="relationType">The type of relation</param>
        /// <returns></returns>
        public virtual List<DiscountOffer> GetOffersAndPromotionsFromRelation(IConnectionManager entry, RecordIdentifier relationID, DiscountOfferLine.DiscountOfferTypeEnum relationType)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Join> joins = new List<Join>(fixedJoins);
                if (relationID.IsGuid)
                {
                    joins.Add(new Join
                    {
                        Table = "PERIODICDISCOUNTLINE",
                        Condition = "(pl.OFFERID = G.OFFERID and pl.PRODUCTTYPE = @relationType AND PL.TARGETMASTERID = @relationID AND  PL.DELETED = 0)",
                        TableAlias = "PL"
                    });

                    MakeParam(cmd, "relationID", (Guid)relationID, SqlDbType.UniqueIdentifier);
                }
                else
                {
                    joins.Add(new Join
                    {
                        Table = "PERIODICDISCOUNTLINE",
                        Condition = "(pl.OFFERID = G.OFFERID and pl.PRODUCTTYPE = @relationType AND PL.TARGETID = @relationID AND  PL.DELETED = 0)",
                        TableAlias = "PL"
                    });

                    MakeParam(cmd, "relationID", (string)relationID);
                }

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PERIODICDISCOUNT ", "G", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                    QueryPartGenerator.JoinGenerator(joins), 
                    QueryPartGenerator.ConditionGenerator(new Condition { Operator = "AND", ConditionValue = "G.DELETED = 0 " }),
                    "ORDER BY PDTYPE");


                MakeParam(cmd, "relationType", (int)relationType, SqlDbType.Int);

                return Execute<DiscountOffer>(entry, cmd, CommandType.Text, PopulateOffer);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier offerID)
        {
            ValidateSecurity(entry, Permission.ManageDiscounts);
            var statement = new SqlServerStatement("PERIODICDISCOUNT");

            statement.StatementType = StatementType.Update;

            Guid masterID = GetMasterID(entry, offerID, "PERIODICDISCOUNT", "OFFERID");

            statement.AddCondition("MASTERID", masterID, SqlDbType.UniqueIdentifier);
            statement.AddCondition("OFFERID", (string)offerID);
            statement.AddField("DELETED", true, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);           
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier offerID)
        {
            return RecordExists(entry, "PERIODICDISCOUNT", "OFFERID", offerID, false);
        }

        public virtual bool BarcodeExists(IConnectionManager entry, RecordIdentifier barcode)
        {
            return RecordExists(entry, "PERIODICDISCOUNT", "BARCODE", barcode, false);
        }

        public virtual void UpdateStatus(IConnectionManager entry, RecordIdentifier offerID,bool enabled)
        {
            var statement = new SqlServerStatement("PERIODICDISCOUNT") {StatementType = StatementType.Update};

            ValidateSecurity(entry, Permission.ManageDiscounts);

            Guid masterID = GetMasterID(entry, offerID, "PERIODICDISCOUNT", "OFFERID");

            statement.AddCondition("MASTERID", masterID, SqlDbType.UniqueIdentifier);
            statement.AddCondition("OFFERID", (string)offerID);

            statement.AddField("STATUS", enabled ? 1 : 0, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void Save(IConnectionManager entry, DiscountOffer offer)
        {
            var statement = new SqlServerStatement("PERIODICDISCOUNT");

            ValidateSecurity(entry, Permission.ManageDiscounts);

            bool isNew = false;
            if (offer.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                SetSequenceType(offer.OfferType);
                offer.ID = DataProviderFactory.Instance.GenerateNumber(entry, this);
            }

            if (isNew || !Exists(entry, offer.ID))
            {
                statement.StatementType = StatementType.Insert;
                offer.MasterID = Guid.NewGuid();
                statement.AddKey("MASTERID", (Guid)offer.MasterID, SqlDbType.UniqueIdentifier);
                if (offer.ID == RecordIdentifier.Empty)
                {
                    // Should we create a new item or update what exists
                    offer.ID = DataProviderFactory.Instance.GenerateNumber<IDiscountOfferData, DiscountOffer>(entry);
                }
                statement.AddKey("OFFERID", (string)offer.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("MASTERID", (Guid)offer.MasterID, SqlDbType.UniqueIdentifier);
                statement.AddCondition("OFFERID", (string)offer.ID);
            }

            statement.AddField("DESCRIPTION", offer.Text);
            statement.AddField("DISCOUNTPCTVALUE", offer.DiscountPercent, SqlDbType.Decimal);
            statement.AddField("DEALPRICEVALUE", offer.DealPrice, SqlDbType.Decimal);
            statement.AddField("DISCOUNTAMOUNTVALUE", offer.DiscountAmount, SqlDbType.Decimal);
            statement.AddField("NOOFLEASTEXPITEMS", offer.NumberOfLeastExpensiveLines, SqlDbType.Int);
            statement.AddField("STATUS", offer.Enabled ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("PRIORITY", offer.Priority,SqlDbType.Int);
            statement.AddField("PDTYPE", (int)offer.OfferType, SqlDbType.Int);
            statement.AddField("DISCVALIDPERIODID", (string)offer.ValidationPeriod);
            statement.AddField("DISCOUNTTYPE", offer.DiscountTypeValue, SqlDbType.Int);
            statement.AddField("PRICEGROUP", (string)offer.PriceGroup);
            statement.AddField("ACCOUNTCODE", (int)offer.AccountCode, SqlDbType.Int);
            statement.AddField("ACCOUNTRELATION", (string)offer.AccountRelation);
            statement.AddField("TRIGGERED", offer.Triggering, SqlDbType.Int);
            statement.AddField("BARCODE", offer.BarCode);

            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return sequenceType; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "PERIODICDISCOUNT", "OFFERID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion        
    }
}
