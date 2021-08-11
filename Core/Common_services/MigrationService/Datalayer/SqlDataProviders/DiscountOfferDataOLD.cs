using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Services.Datalayer.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.SqlDataProviders
{
    public class DiscountOfferDataOLD : SqlServerDataProviderBase, IDiscountOfferDataOLD
    {
        

        private string sequenceType;

        private static string BasePeriodicDiscountSql
        {
            get
            {               
                return @"select g.OFFERID as OFFERID,
	                            ISNULL(g.DESCRIPTION, '') as DESCRIPTION,
	                            ISNULL(g.DISCOUNTPCTVALUE, 0) as DISCOUNTPCTVALUE,
	                            ISNULL(g.DEALPRICEVALUE, 0) as DEALPRICEVALUE,
	                            ISNULL(g.DISCOUNTAMOUNTVALUE, 0) as DISCOUNTAMOUNTVALUE,
	                            ISNULL(g.NOOFLEASTEXPITEMS, 0) as NOOFLEASTEXPITEMS,
	                            ISNULL(g.STATUS, 0) as STATUS,
	                            ISNULL(g.PRIORITY, 0) as PRIORITY,
	                            ISNULL(g.PDTYPE, 0) as PDTYPE,
	                            ISNULL(g.DISCOUNTTYPE, 0) as DISCOUNTTYPE,
	                            ISNULL(g.DISCVALIDPERIODID, '') as DISCVALIDPERIODID,
	                            ISNULL(g.PRICEGROUP, '') as PRICEGROUP,
	                            ISNULL(h.NAME,'') as PRICEGROUPNAME,
	                            ISNULL(p.DESCRIPTION,'') as DISCOUNTVALIDATIONPERIODIDDESCRIPTION,
	                            p.STARTINGDATE,
	                            p.ENDINGDATE,
	                            ISNULL(g.ACCOUNTCODE, 0) as ACCOUNTCODE,
                                ISNULL(g.ACCOUNTRELATION, '') as ACCOUNTRELATION,
                                ISNULL(g.TRIGGERED, 0) as TRIGGERED,
                                ISNULL(c.NAME, '') as CUSTOMERNAME,
	                            ISNULL(pdg.NAME, '') as CUSTOMERGROUPNAME
                         from POSPERIODICDISCOUNT g
                         left outer join POSDISCVALIDATIONPERIOD p on g.DISCVALIDPERIODID = p.ID and g.DATAAREAID = p.DATAAREAID 
                         left outer join PRICEDISCGROUP h on g.PRICEGROUP = h.GROUPID and g.DATAAREAID = h.DATAAREAID and h.MODULE = 1 and h.TYPE = 0 
                         left join CUSTTABLE c on c.ACCOUNTNUM = g.ACCOUNTRELATION and c.DATAAREAID = g.DATAAREAID
                         left join PRICEDISCGROUP pdg on pdg.MODULE = 1 and pdg.TYPE = 1 and pdg.GROUPID = g.ACCOUNTRELATION and pdg.DATAAREAID = g.DATAAREAID ";
            }
        }

        private static string DistinctPeriodicDiscountSql
        {
            get
            {
                return @"select DISTINCT
                                g.OFFERID as OFFERID,
	                            ISNULL(g.DESCRIPTION, '') as DESCRIPTION,
	                            ISNULL(g.DISCOUNTPCTVALUE, 0) as DISCOUNTPCTVALUE,
	                            ISNULL(g.DEALPRICEVALUE, 0) as DEALPRICEVALUE,
	                            ISNULL(g.DISCOUNTAMOUNTVALUE, 0) as DISCOUNTAMOUNTVALUE,
	                            ISNULL(g.NOOFLEASTEXPITEMS, 0) as NOOFLEASTEXPITEMS,
	                            ISNULL(g.STATUS, 0) as STATUS,
	                            ISNULL(g.PRIORITY, 0) as PRIORITY,
	                            ISNULL(g.PDTYPE, 0) as PDTYPE,
	                            ISNULL(g.DISCOUNTTYPE, 0) as DISCOUNTTYPE,
	                            ISNULL(g.DISCVALIDPERIODID, '') as DISCVALIDPERIODID,
	                            ISNULL(g.PRICEGROUP, '') as PRICEGROUP,
	                            ISNULL(h.NAME,'') as PRICEGROUPNAME,
	                            ISNULL(p.DESCRIPTION,'') as DISCOUNTVALIDATIONPERIODIDDESCRIPTION,
	                            p.STARTINGDATE,
	                            p.ENDINGDATE,
	                            ISNULL(g.ACCOUNTCODE, 0) as ACCOUNTCODE,
                                ISNULL(g.ACCOUNTRELATION, '') as ACCOUNTRELATION,
                                ISNULL(g.TRIGGERED, 0) as TRIGGERED,
                                ISNULL(c.NAME, '') as CUSTOMERNAME,
	                            ISNULL(pdg.NAME, '') as CUSTOMERGROUPNAME
                         from POSPERIODICDISCOUNT g
                         left outer join POSDISCVALIDATIONPERIOD p on g.DISCVALIDPERIODID = p.ID and g.DATAAREAID = p.DATAAREAID 
                         left outer join PRICEDISCGROUP h on g.PRICEGROUP = h.GROUPID and g.DATAAREAID = h.DATAAREAID and h.MODULE = 1 and h.TYPE = 0 
                         left join CUSTTABLE c on c.ACCOUNTNUM = g.ACCOUNTRELATION and c.DATAAREAID = g.DATAAREAID
                         left join PRICEDISCGROUP pdg on pdg.MODULE = 1 and pdg.TYPE = 1 and pdg.GROUPID = g.ACCOUNTRELATION and pdg.DATAAREAID = g.DATAAREAID ";
            }
        }

        private static string BaseMixAndMatchSql
        {
            get
            {
                return @"Select 
                        g.OFFERID,
                        ISNULL(g.DESCRIPTION,'') as DESCRIPTION,
                        ISNULL(g.DISCOUNTPCTVALUE,0.0) as DISCOUNTPCTVALUE,
                        ISNULL(g.STATUS,0) as STATUS,
                        ISNULL(g.PRIORITY,0) as PRIORITY,
                        ISNULL(g.PDTYPE,0) as PDTYPE,
                        ISNULL(g.DISCOUNTTYPE,1) as DISCOUNTTYPE,
                        ISNULL(g.DISCVALIDPERIODID,'') as DISCVALIDPERIODID,
                        ISNULL(p.DESCRIPTION,'') as DISCOUNTVALIDATIONPERIODIDDESCRIPTION,
                        ISNULL(g.PRICEGROUP,'') as PRICEGROUP,ISNULL(h.NAME,'') as PRICEGROUPNAME,
                        ISNULL(DEALPRICEVALUE,0.0) as DEALPRICEVALUE,
                        ISNULL(g.DISCOUNTAMOUNTVALUE,0.0) as DISCOUNTAMOUNTVALUE,
                        ISNULL(g.NOOFLEASTEXPITEMS,1) as NOOFLEASTEXPITEMS, 
                        p.STARTINGDATE, 
                        p.ENDINGDATE,
                        ISNULL(l.NOOFITEMSNEEDED,0) as NOOFITEMSNEEDED,
                        ISNULL(g.ACCOUNTCODE, 0) as ACCOUNTCODE,
                        ISNULL(g.ACCOUNTRELATION, '') as ACCOUNTRELATION,
                        ISNULL(g.TRIGGERED, 0) as TRIGGERED,
                        ISNULL(c.NAME, '') as CUSTOMERNAME,
                        ISNULL(pdg.NAME, '') as CUSTOMERGROUPNAME
                        from POSPERIODICDISCOUNT g 
                        left outer join POSDISCVALIDATIONPERIOD p on g.DISCVALIDPERIODID = p.ID and g.DATAAREAID = p.DATAAREAID 
                        left outer join 
                        (
                            Select 
                            PLG.OFFERID, 
                            SUM(PLG.NOOFITEMSNEEDED) as NOOFITEMSNEEDED, 
                            PLG.DATAAREAID 
                            from POSMMLineGroups PLG 
                            Where PLG.LINEGROUP in (select LINEGROUP from POSPERIODICDISCOUNTLINE where OFFERID = plg.OFFERID) 
                            group by PLG.DATAAREAID,PLG.OFFERID
                        ) l 
                        on g.OFFERID = l.OFFERID and g.DATAAREAID = l.DATAAREAID 
                        left outer join PRICEDISCGROUP h on g.PRICEGROUP = h.GROUPID and g.DATAAREAID = h.DATAAREAID and h.MODULE = 1 and h.TYPE = 0 
                        left join CUSTTABLE c on c.ACCOUNTNUM = g.ACCOUNTRELATION and c.DATAAREAID = g.DATAAREAID
                        left join PRICEDISCGROUP pdg on pdg.MODULE = 1 and pdg.TYPE = 1 and pdg.GROUPID = g.ACCOUNTRELATION and pdg.DATAAREAID = g.DATAAREAID ";  
            }

        }

        private static string ResolveSort(DiscountOfferSorting sortBy, bool backwards)
        {
            switch (sortBy)
            {
                case DiscountOfferSorting.OfferNumber:
                    return backwards ? "g.OFFERID DESC" : "g.OFFERID ASC";
                    
                case DiscountOfferSorting.Description:
                    return backwards ? "g.DESCRIPTION DESC" : "g.DESCRIPTION ASC";
                    
                case DiscountOfferSorting.Priority:
                    return backwards ? "g.PRIORITY DESC" : "g.PRIORITY ASC";
                    
                case DiscountOfferSorting.OfferType:
                    return backwards ? "g.PDTYPE DESC" : "g.PDTYPE ASC";

                case DiscountOfferSorting.Status:
                    return backwards ? "g.STATUS DESC" : "g.STATUS ASC";

                case DiscountOfferSorting.DiscountType:
                    return backwards ? "g.DISCOUNTTYPE DESC" : "g.DISCOUNTTYPE ASC";

                case DiscountOfferSorting.DiscountValidationPeriod:
                    return backwards ? "g.DISCVALIDPERIODID DESC" : "g.DISCVALIDPERIODID ASC";

                case DiscountOfferSorting.StartingDate:
                    return backwards ? "p.STARTINGDATE DESC" : "p.STARTINGDATE ASC";

                case DiscountOfferSorting.EndingDate:
                    return backwards ? "p.ENDINGDATE DESC" : "p.ENDINGDATE ASC";

                case DiscountOfferSorting.NumberOfItemsNeeded:
                    return backwards ? "l.NOOFITEMSNEEDED DESC" : "l.NOOFITEMSNEEDED ASC";

                case DiscountOfferSorting.DiscountPercentValue:
                    return backwards ? "g.DISCOUNTPCTVALUE DESC" : "g.DISCOUNTPCTVALUE ASC";

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
                cmd.CommandText = @"SELECT COALESCE(DESCRIPTION, '') as OFFERNAME FROM POSPERIODICDISCOUNT
                                    WHERE OFFERID = @offerID and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "offerID", (string)offerID);

                object value = entry.Connection.ExecuteScalar(cmd);

                return (value == DBNull.Value) ? "" : (string)value;
            }
        }

        public virtual DiscountOffer Get(IConnectionManager entry, RecordIdentifier offerID,DiscountOffer.PeriodicDiscountOfferTypeEnum type)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<DiscountOffer> results;
                if(type == DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch)
                {
                    cmd.CommandText = BaseMixAndMatchSql + "where g.DATAAREAID = @dataAreaId and g.OFFERID = @offerID";

                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                    MakeParam(cmd, "offerID", (string)offerID);

                    results = Execute<DiscountOffer>(entry, cmd, CommandType.Text, PopulateMixMatchOffer);
                }
                else
                {
                    cmd.CommandText =
                       @"Select g.OFFERID,ISNULL(g.DESCRIPTION,'') as DESCRIPTION,ISNULL(g.DISCOUNTPCTVALUE,0.0) as DISCOUNTPCTVALUE,
                         ISNULL(g.STATUS,0) as STATUS,ISNULL(g.PRIORITY,0) as PRIORITY,ISNULL(g.PDTYPE,0) as PDTYPE,ISNULL(g.DISCOUNTTYPE,1) as DISCOUNTTYPE,
                        ISNULL(g.DISCVALIDPERIODID,'') as DISCVALIDPERIODID,ISNULL(p.DESCRIPTION,'') as DISCOUNTVALIDATIONPERIODIDDESCRIPTION,
                        ISNULL(g.PRICEGROUP,'') as PRICEGROUP,ISNULL(h.NAME,'') as PRICEGROUPNAME,ISNULL(DEALPRICEVALUE,0.0) as DEALPRICEVALUE,
                        ISNULL(g.DISCOUNTAMOUNTVALUE,0.0) as DISCOUNTAMOUNTVALUE,ISNULL(g.NOOFLEASTEXPITEMS,1) as NOOFLEASTEXPITEMS,p.STARTINGDATE, p.ENDINGDATE, 
                        ISNULL(g.ACCOUNTCODE, 0) as ACCOUNTCODE,
                        ISNULL(g.ACCOUNTRELATION, '') as ACCOUNTRELATION,
                        ISNULL(g.TRIGGERED, 0) as TRIGGERED,
                        ISNULL(c.NAME, '') as CUSTOMERNAME,
                        ISNULL(pdg.NAME, '') as CUSTOMERGROUPNAME
                        from POSPERIODICDISCOUNT g 
                        left outer join POSDISCVALIDATIONPERIOD p on g.DISCVALIDPERIODID = p.ID and g.DATAAREAID = p.DATAAREAID 
                        left outer join PRICEDISCGROUP h on g.PRICEGROUP = h.GROUPID and g.DATAAREAID = h.DATAAREAID and h.MODULE = 1 and h.TYPE = 0 
                        left join CUSTTABLE c on c.ACCOUNTNUM = g.ACCOUNTRELATION and c.DATAAREAID = g.DATAAREAID
                        left join PRICEDISCGROUP pdg on pdg.MODULE = 1 and pdg.TYPE = 1 and pdg.GROUPID = g.ACCOUNTRELATION and pdg.DATAAREAID = g.DATAAREAID  
                        where g.DATAAREAID = @dataAreaId and OFFERID = @offerID";

                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                    MakeParam(cmd, "offerID", (string)offerID);

                    results = Execute<DiscountOffer>(entry, cmd, CommandType.Text, PopulateOffer);
                }

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
                cmd.CommandText = BasePeriodicDiscountSql +
                                  "where g.DATAAREAID = @dataAreaId ";

                if (excludePromotions)
                {
                    cmd.CommandText += "and PDTYPE <> 3 ";
                }

                cmd.CommandText += " order by " + ResolveSort(sortBy, sortBackwards);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<DiscountOffer>(entry, cmd, CommandType.Text, PopulateOffer);
            }
        }

        public virtual int GetNumberOfDiscountsExpiringOverTheNext7Days(IConnectionManager entry)
        {
            DateTime today = DateTime.Now;

            today = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);

            DateTime weekFromNow = today.AddDays(7);

            weekFromNow = new DateTime(weekFromNow.Year, weekFromNow.Month, weekFromNow.Day, 23, 59, 59);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"select Count('x') from POSDISCVALIDATIONPERIOD period
                      right outer join POSPERIODICDISCOUNT discount on period.ID = discount.DISCVALIDPERIODID
                      where discount.STATUS = 1 and discount.PDTYPE <> 3 and period.ENDINGDATE between @today and @weekFromNow";


                MakeParam(cmd, "today", today, SqlDbType.DateTime);
                MakeParam(cmd, "weekFromNow", weekFromNow, SqlDbType.DateTime);

                return (int)entry.Connection.ExecuteScalar(cmd);
            }


        }

        public virtual int GetNumberOfActiveDiscounts(IConnectionManager entry)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"select Count('x') from POSPERIODICDISCOUNT discount
                      left outer join POSDISCVALIDATIONPERIOD period  on period.ID = discount.DISCVALIDPERIODID
                      where discount.STATUS = 1 and discount.PDTYPE <> 3 and (discount.DISCVALIDPERIODID = '' or (CONVERT(VARCHAR(10),GETDATE(),111) between period.STARTINGDATE and period.ENDINGDATE ))";

                // Note that "CONVERT(VARCHAR(10),GETDATE(),111)" here above is SQL Server technique to get current date without time.

                return (int)entry.Connection.ExecuteScalar(cmd);
            }
        }

        public virtual bool DiscountsAreConfigured(IConnectionManager entry)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"If Exists(select 'x' from POSPERIODICDISCOUNT discount
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
                cmd.CommandText = BasePeriodicDiscountSql +
                                  "where g.DATAAREAID = @dataAreaId AND g.TRIGGERED = 1";

                cmd.CommandText += " order by " + ResolveSort(sortBy, sortBackwards);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

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
                cmd.CommandText = BasePeriodicDiscountSql +
                                  "where g.DATAAREAID = @dataAreaId and g.ACCOUNTCODE = 2 and g.ACCOUNTRELATION = @accountRelation and PDTYPE <> 3";

                cmd.CommandText += " order by " + ResolveSort(sortBy, sortBackwards);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
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
                cmd.CommandText = BasePeriodicDiscountSql +
                                  "where g.DATAAREAID = @dataAreaId and g.ACCOUNTCODE = 2 and g.ACCOUNTRELATION = @accountRelation and PDTYPE = 3";

                cmd.CommandText += " order by " + ResolveSort(sortBy, sortBackwards);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
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
                cmd.CommandText = BasePeriodicDiscountSql +
                                  "where g.DATAAREAID = @dataAreaId and g.ACCOUNTCODE = 1 and g.ACCOUNTRELATION = @accountRelation ";

                switch (filter)
                {
                    case DiscountOfferFilter.AllDiscountOffers:
                        break;
                    case DiscountOfferFilter.AllExceptPromotions:
                        cmd.CommandText += "and PDTYPE <> 3 ";
                        break;
                    case DiscountOfferFilter.OnlyPromotions:
                        cmd.CommandText += "and PDTYPE = 3 ";
                        break;
                }

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
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
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BasePeriodicDiscountSql;

                cmd.CommandText += " order by " + ResolveSort(sortBy, sortBackwards);

                return Execute<DiscountOffer>(entry, cmd, CommandType.Text, PopulateOffer);
            }
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
                      from POSPERIODICDISCOUNT
                      where DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

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
                      from POSPERIODICDISCOUNT
                      where DATAAREAID = @dataAreaId 
                      order by PRIORITY asc";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

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
        public virtual List<DiscountOffer> GetOffers(IConnectionManager entry, DiscountOffer.PeriodicDiscountOfferTypeEnum type, DiscountOfferSorting sortBy, bool backwardsSort)
        {
            string sort;

            ValidateSecurity(entry);

            sort = " order by " + ResolveSort(sortBy, backwardsSort);            
            
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                if (type == DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch)
                {
                    cmd.CommandText = BaseMixAndMatchSql + "where g.DATAAREAID = @dataAreaId and g.PDTYPE = @type" + sort;

                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                    MakeParam(cmd, "type", (int)type, SqlDbType.Int);

                    return Execute<DiscountOffer>(entry, cmd, CommandType.Text, PopulateMixMatchOffer);
                }

                cmd.CommandText =
                    @"Select g.OFFERID,ISNULL(g.DESCRIPTION,'') as DESCRIPTION,ISNULL(g.DISCOUNTPCTVALUE,0.0) as DISCOUNTPCTVALUE,
                         ISNULL(g.STATUS,0) as STATUS,ISNULL(g.PRIORITY,0) as PRIORITY,ISNULL(g.PDTYPE,0) as PDTYPE,ISNULL(g.DISCOUNTTYPE,1) as DISCOUNTTYPE,
                         ISNULL(g.DISCVALIDPERIODID,'') as DISCVALIDPERIODID,ISNULL(p.DESCRIPTION,'') as DISCOUNTVALIDATIONPERIODIDDESCRIPTION,
                         ISNULL(g.PRICEGROUP,'') as PRICEGROUP,ISNULL(h.NAME,'') as PRICEGROUPNAME,ISNULL(DEALPRICEVALUE,0.0) as DEALPRICEVALUE,
                         ISNULL(g.DISCOUNTAMOUNTVALUE,0.0) as DISCOUNTAMOUNTVALUE,ISNULL(g.NOOFLEASTEXPITEMS,1) as NOOFLEASTEXPITEMS, p.STARTINGDATE, p.ENDINGDATE,
                         ISNULL(g.ACCOUNTCODE, 0) as ACCOUNTCODE,
                         ISNULL(g.ACCOUNTRELATION, '') as ACCOUNTRELATION,
                         ISNULL(g.TRIGGERED, 0) as TRIGGERED,
                         ISNULL(c.NAME, '') as CUSTOMERNAME,
                         ISNULL(pdg.NAME, '') as CUSTOMERGROUPNAME 
                         from POSPERIODICDISCOUNT g 
                         left outer join POSDISCVALIDATIONPERIOD p on g.DISCVALIDPERIODID = p.ID and g.DATAAREAID = p.DATAAREAID 
                         left outer join PRICEDISCGROUP h on g.PRICEGROUP = h.GROUPID and g.DATAAREAID = h.DATAAREAID and h.MODULE = 1 and h.TYPE = 0 
                         left join CUSTTABLE c on c.ACCOUNTNUM = g.ACCOUNTRELATION and c.DATAAREAID = g.DATAAREAID
                         left join PRICEDISCGROUP pdg on pdg.MODULE = 1 and pdg.TYPE = 1 and pdg.GROUPID = g.ACCOUNTRELATION and pdg.DATAAREAID = g.DATAAREAID 
                         where g.DATAAREAID = @dataAreaId and g.PDTYPE = @type" + sort;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "type", (int)type, SqlDbType.Int);

                return Execute<DiscountOffer>(entry, cmd, CommandType.Text, PopulateOffer);
            }
        }

        public virtual List<DiscountOffer> GetOffers(IConnectionManager entry, DiscountOffer.PeriodicDiscountOfferTypeEnum type, int sortColumn, bool backwardsSort)
        {
            string sort = "";
            string[] columns;

            ValidateSecurity(entry);

            if (type == DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy)
            {
                columns = new[] { "g.OFFERID", "g.DESCRIPTION", "g.STATUS", "g.DISCOUNTTYPE", "g.DISCVALIDPERIODID", "p.STARTINGDATE", "p.ENDINGDATE" };
            }
            else if (type == DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch)
            {
                columns = new[] { "g.OFFERID", "g.DESCRIPTION", "g.STATUS", "g.DISCOUNTTYPE", "g.DISCOUNTTYPE", "p.NOOFITEMSNEEDED", "g.DISCVALIDPERIODID" };
            }
            else
            {
                columns = new[] { "g.OFFERID", "g.DESCRIPTION", "g.STATUS", "g.DISCOUNTPCTVALUE", "g.DISCVALIDPERIODID", "p.STARTINGDATE", "p.ENDINGDATE" };
            }

            if (sortColumn < columns.Length)
            {
                sort = " order by " + columns[sortColumn] + (backwardsSort ? " DESC" : " ASC");
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                if(type == DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch)
                {
                    cmd.CommandText = BaseMixAndMatchSql + "where g.DATAAREAID = @dataAreaId and g.PDTYPE = @type" + sort;

                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                    MakeParam(cmd, "type", (int)type, SqlDbType.Int);

                    return Execute<DiscountOffer>(entry, cmd, CommandType.Text, PopulateMixMatchOffer);
                }
                
                cmd.CommandText =
                    @"Select g.OFFERID,ISNULL(g.DESCRIPTION,'') as DESCRIPTION,ISNULL(g.DISCOUNTPCTVALUE,0.0) as DISCOUNTPCTVALUE,
                         ISNULL(g.STATUS,0) as STATUS,ISNULL(g.PRIORITY,0) as PRIORITY,ISNULL(g.PDTYPE,0) as PDTYPE,ISNULL(g.DISCOUNTTYPE,1) as DISCOUNTTYPE,
                         ISNULL(g.DISCVALIDPERIODID,'') as DISCVALIDPERIODID,ISNULL(p.DESCRIPTION,'') as DISCOUNTVALIDATIONPERIODIDDESCRIPTION,
                         ISNULL(g.PRICEGROUP,'') as PRICEGROUP,ISNULL(h.NAME,'') as PRICEGROUPNAME,ISNULL(DEALPRICEVALUE,0.0) as DEALPRICEVALUE,
                         ISNULL(g.DISCOUNTAMOUNTVALUE,0.0) as DISCOUNTAMOUNTVALUE,ISNULL(g.NOOFLEASTEXPITEMS,1) as NOOFLEASTEXPITEMS, p.STARTINGDATE, p.ENDINGDATE,
                         ISNULL(g.ACCOUNTCODE, 0) as ACCOUNTCODE,
                         ISNULL(g.ACCOUNTRELATION, '') as ACCOUNTRELATION,
                         ISNULL(g.TRIGGERED, 0) as TRIGGERED,
                         ISNULL(c.NAME, '') as CUSTOMERNAME,
                         ISNULL(pdg.NAME, '') as CUSTOMERGROUPNAME 
                         from POSPERIODICDISCOUNT g 
                         left outer join POSDISCVALIDATIONPERIOD p on g.DISCVALIDPERIODID = p.ID and g.DATAAREAID = p.DATAAREAID 
                         left outer join PRICEDISCGROUP h on g.PRICEGROUP = h.GROUPID and g.DATAAREAID = h.DATAAREAID and h.MODULE = 1 and h.TYPE = 0 
                         left join CUSTTABLE c on c.ACCOUNTNUM = g.ACCOUNTRELATION and c.DATAAREAID = g.DATAAREAID
                         left join PRICEDISCGROUP pdg on pdg.MODULE = 1 and pdg.TYPE = 1 and pdg.GROUPID = g.ACCOUNTRELATION and pdg.DATAAREAID = g.DATAAREAID 
                         where g.DATAAREAID = @dataAreaId and g.PDTYPE = @type" + sort;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "type", (int)type, SqlDbType.Int);

                return Execute<DiscountOffer>(entry, cmd, CommandType.Text, PopulateOffer);
            }
        }

        public virtual DiscountOffer GetOfferFromLine(IConnectionManager entry, RecordIdentifier offerId)
        { 
            ValidateSecurity(entry);
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                   @"Select DISTINCT g.OFFERID,ISNULL(g.DESCRIPTION,'') as DESCRIPTION,ISNULL(g.DISCOUNTPCTVALUE,0.0) as DISCOUNTPCTVALUE,
                     ISNULL(g.STATUS,0) as STATUS,ISNULL(g.PRIORITY,0) as PRIORITY,ISNULL(g.PDTYPE,0) as PDTYPE,ISNULL(g.DISCOUNTTYPE,1) as DISCOUNTTYPE,
                     ISNULL(g.DISCVALIDPERIODID,'') as DISCVALIDPERIODID,ISNULL(p.DESCRIPTION,'') as DISCOUNTVALIDATIONPERIODIDDESCRIPTION,
                     ISNULL(g.PRICEGROUP,'') as PRICEGROUP,ISNULL(h.NAME,'') as PRICEGROUPNAME,ISNULL(DEALPRICEVALUE,0.0) as DEALPRICEVALUE,
                     ISNULL(g.DISCOUNTAMOUNTVALUE,0.0) as DISCOUNTAMOUNTVALUE,ISNULL(g.NOOFLEASTEXPITEMS,1) as NOOFLEASTEXPITEMS, p.STARTINGDATE, p.ENDINGDATE,
                     ISNULL(g.ACCOUNTCODE, 0) as ACCOUNTCODE,
                     ISNULL(g.ACCOUNTRELATION, '') as ACCOUNTRELATION,
                     ISNULL(g.TRIGGERED, 0) as TRIGGERED,
                     ISNULL(c.NAME, '') as CUSTOMERNAME,
                     ISNULL(pdg.NAME, '') as CUSTOMERGROUPNAME  
                     from POSPERIODICDISCOUNT g 
                     left outer join POSDISCVALIDATIONPERIOD p on g.DISCVALIDPERIODID = p.ID and g.DATAAREAID = p.DATAAREAID 
                     left outer join PRICEDISCGROUP h on g.PRICEGROUP = h.GROUPID and g.DATAAREAID = h.DATAAREAID and h.MODULE = 1 and h.TYPE = 0
                     left join CUSTTABLE c on c.ACCOUNTNUM = g.ACCOUNTRELATION and c.DATAAREAID = g.DATAAREAID
                     left join PRICEDISCGROUP pdg on pdg.MODULE = 1 and pdg.TYPE = 1 and pdg.GROUPID = g.ACCOUNTRELATION and pdg.DATAAREAID = g.DATAAREAID  
                     where g.OFFERID = @offerId and g.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "offerId", (string)offerId);

                var result = Execute<DiscountOffer>(entry, cmd, CommandType.Text, PopulateOffer);

                return result.Count > 0 ? result[0] : null;
            }
        }

        /// <summary>
        /// Gets a list of all discount offers for the given relation i.e all offers for a give retail group or retail item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="relationID">The relation ID, f.ex a retial item ID</param>
        /// <param name="relationType">The type of relation</param>
        /// <returns></returns>
        public List<DiscountOffer> GetOffersFromRelation(IConnectionManager entry, RecordIdentifier relationID,
                                                                DiscountOfferLine.DiscountOfferTypeEnum relationType)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BasePeriodicDiscountSql +
                                  @"join POSPERIODICDISCOUNTLINE pl on 
pl.OFFERID = g.OFFERID and 
pl.PRODUCTTYPE = @relationType and 
pl.ID = @relationID and 
pl.DATAAREAID = g.DATAAREAID 
                                   where g.DATAAREAID = @dataAreaId order by g.PDTYPE";

                MakeParam(cmd, "relationType", (int)relationType, SqlDbType.Int);
                MakeParam(cmd, "relationID", (string)relationID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<DiscountOffer>(entry, cmd, CommandType.Text, PopulateOffer);
            }
        }

        /// <summary>
        /// Gets a list of all promotion offers for the given relation i.e all offers for a give retail group or retail item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="relationID">The relation ID, f.ex a retial item ID</param>
        /// <param name="relationType">The type of relation</param>
        /// <returns></returns>
        public virtual List<DiscountOffer> GetPromotionsFromRelation(IConnectionManager entry, RecordIdentifier relationID, DiscountOfferLine.DiscountOfferTypeEnum relationType)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BasePeriodicDiscountSql +
                                  @"join RBODISCOUNTOFFERLINE pl on 
pl.OFFERID = g.OFFERID and 
pl.TYPE = @relationType and 
pl.ITEMRELATION = @relationID and 
pl.DATAAREAID = g.DATAAREAID 
                                   where g.DATAAREAID = @dataAreaId order by g.PDTYPE";

                MakeParam(cmd, "relationType", (int)relationType, SqlDbType.Int);
                MakeParam(cmd, "relationID", (string)relationID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<DiscountOffer>(entry, cmd, CommandType.Text, PopulateOffer);
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
                cmd.CommandText = DistinctPeriodicDiscountSql +
                                  @" ,POSPERIODICDISCOUNTLINE pl, RBODISCOUNTOFFERLINE rl
                                     where g.DATAAREAID = @dataAreaId and
                                    (pl.OFFERID = g.OFFERID and pl.PRODUCTTYPE = @relationType and pl.ID = @relationID) or
                                    (rl.OFFERID = g.OFFERID and rl.TYPE = @relationType and rl.ITEMRELATION = @relationID)
                                    order by PDTYPE";

                MakeParam(cmd, "relationType", (int)relationType, SqlDbType.Int);
                MakeParam(cmd, "relationID", (string)relationID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<DiscountOffer>(entry, cmd, CommandType.Text, PopulateOffer);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier offerID, DiscountOffer.PeriodicDiscountOfferTypeEnum type)
        {
            DeleteRecord(entry, "POSPERIODICDISCOUNT", "OFFERID", offerID, DataLayer.BusinessObjects.Permission.ManageDiscounts);
            DeleteRecord(entry, "POSPERIODICDISCOUNTLINE", "OFFERID", offerID, DataLayer.BusinessObjects.Permission.ManageDiscounts);

            if (type == DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy)
            {
                DeleteRecord(entry, "POSMULTIBUYDISCOUNTLINE", "OFFERID", offerID, DataLayer.BusinessObjects.Permission.ManageDiscounts);
            }
            else if (type == DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch)
            {
                DeleteRecord(entry, "POSMMLINEGROUPS", "OFFERID", offerID, DataLayer.BusinessObjects.Permission.ManageDiscounts);
            }
            else if (type == DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion)
            {
                DeleteRecord(entry, "RBODISCOUNTOFFERLINE", "OFFERID", offerID, DataLayer.BusinessObjects.Permission.ManageDiscounts);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier offerID)
        {
            return RecordExists(entry, "POSPERIODICDISCOUNT", "OFFERID", offerID);
        }

        public virtual void UpdateStatus(IConnectionManager entry, RecordIdentifier offerID,bool enabled)
        {
            var statement = new SqlServerStatement("POSPERIODICDISCOUNT") {StatementType = StatementType.Update};

            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ManageDiscounts);

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("OFFERID", (string)offerID);

            statement.AddField("STATUS", enabled ? 1 : 0, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void Save(IConnectionManager entry, DiscountOffer offer)
        {
            var statement = new SqlServerStatement("POSPERIODICDISCOUNT");

            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ManageDiscounts);

            bool isNew = false;
            if (offer.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                SetSequenceType(offer.OfferType);
                //offer.ID = DataProviderFactory.Instance.GenerateNumber(entry, this);
            }

            if (isNew || !Exists(entry, offer.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("OFFERID", (string)offer.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
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
            statement.AddField("SAMEDIFFMMLINES", 0, SqlDbType.Int);
            statement.AddField("NOOFLINESTOTRIGGER", 0, SqlDbType.Int);
            //statement.AddField("NOOFTIMESAPPLICABLE", 0, SqlDbType.Int);
            statement.AddField("PRICEGROUP", (string)offer.PriceGroup);
            statement.AddField("ACCOUNTCODE", (int)offer.AccountCode, SqlDbType.Int);
            statement.AddField("ACCOUNTRELATION", (string)offer.AccountRelation);
            statement.AddField("TRIGGERED", offer.Triggering, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            throw new NotImplementedException();
        }

        public RecordIdentifier SequenceID
        {
            get { return sequenceType; }
        }

        #endregion        
    }
}
