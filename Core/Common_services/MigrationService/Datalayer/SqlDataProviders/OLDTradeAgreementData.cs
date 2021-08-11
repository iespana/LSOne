using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.UserManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Services.Datalayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using TradeAgreementEntry = LSOne.DataLayer.BusinessObjects.PricesAndDiscounts.TradeAgreementEntry;
using TradeAgreementEntryAccountCode = LSOne.DataLayer.BusinessObjects.PricesAndDiscounts.TradeAgreementEntryAccountCode;

namespace LSOne.Services.Datalayer.SqlDataProviders
{
    public interface IOLDTradeAgreementData : IDataProviderBase<OLDTradeAgreementEntry>
    {
        List<OLDTradeAgreementEntry> GetAgreementsForVariant(
            IConnectionManager entry,
            RecordIdentifier variantID);
    }


    public class OLDTradeAgreementData : SqlServerDataProviderBase, IOLDTradeAgreementData
    {

        /// <summary>
        /// Data provider class for trade agreement entries
        /// </summary>

        private static string BaseColumnSelection
        {
            get { return @"t.ID,  
                    t.ITEMCODE,  
                    t.ACCOUNTCODE,  
                    t.ITEMRELATION,  
                    t.ACCOUNTRELATION,  
                    t.QUANTITYAMOUNT,  
                    t.FROMDATE,  
                    t.TODATE,  
                    t.AMOUNT,  
                    t.CURRENCY,  
                    ISNULL(t.PERCENT1,0) as PERCENT1,  
                    ISNULL(t.PERCENT2,0) as PERCENT2,  
                    ISNULL(t.SEARCHAGAIN,0) as SEARCHAGAIN,  
                    t.RELATION,  
                    t.UNITID,  
                    t.INVENTDIMID,  
                    ISNULL(AMOUNTINCLTAX,0) as AMOUNTINCLTAX,  
                    ISNULL(t.MARKUP,0) as MARKUP,  
                    COALESCE(curr.TXT,'') as CURRENCYDESCRIPTION,
                    COALESCE(u.TXT,'') as UNITDESCRIPTION,  
                    COALESCE(cu.NAME,pr.NAME,'') as ACCOUNTNAME "; }
        }

        private static string ExtraColumnSelection
        {
            get { return @"ISNULL(i.INVENTSIZEID,'') as INVENTSIZEID,  
                    ISNULL(s.NAME,'') as INVENTSIZENAME,  
                    ISNULL(i.INVENTCOLORID,'') as INVENTCOLORID,  
                    ISNULL(c.NAME,'') as INVENTCOLORNAME,  
                    ISNULL(i.INVENTSTYLEID,'') as INVENTSTYLEID,  
                    ISNULL(st.NAME,'') as INVENTSTYLENAME,  
                    ISNULL(inv.ITEMNAME,'') as ITEMNAME,  
                    ISNULL(inv.ITEMID,'') as ITEMID,  
                    COALESCE(pr2.NAME,'') as ITEMRELATIONNAME,  
                    COALESCE(id.RBOVARIANTID,'') as VARIANTID "; }
        }

        private static string AllColumnSelection
        {
            get { return BaseColumnSelection + ", " + ExtraColumnSelection; }
        }

        private static string SelectPartForAll
        {
            get
            {
                return "select " + AllColumnSelection +
                       @"from PRICEDISCTABLE t " +
                       GetCommonJoins(null);
            }
        }

        private static string GetPriceSelectPart(TradeAgreementRelation relation)
        {
            string type;

            if (relation == TradeAgreementRelation.PriceSales)
            {
                type = "0";
            }
            else if (relation == TradeAgreementRelation.LineDiscSales)
            {
                type = "1";
            }
            else
            {
                type = "2"; // Multi line discount
            }

            return "select " + AllColumnSelection +
                   "from PRICEDISCTABLE t " +
                   GetCommonJoins(type);
        }

        private static string TotalDiscountSelectPart
        {
            get
            {
                return "select " + BaseColumnSelection + ", COALESCE(pr.NAME,'') as ITEMRELATIONNAME " +
                       "from PRICEDISCTABLE t " +
                       "left outer join CURRENCY curr on curr.CURRENCYCODE = t.CURRENCY and curr.DATAAREAID = t.DATAAREAID " +
                       "left outer join CUSTTABLE cu on t.ACCOUNTCODE = 0 and cu.ACCOUNTNUM = t.ACCOUNTRELATION and cu.DATAAREAID = t.DATAAREAID " +
                       "left outer join PRICEDISCGROUP pr on t.ACCOUNTCODE = 1 and pr.MODULE = 1 and pr.TYPE = 3 and pr.GROUPID = t.ACCOUNTRELATION and pr.DATAAREAID = t.DATAAREAID " +
                       "left outer join UNIT u on u.UNITID = t.UNITID and u.DATAAREAID = t.DATAAREAID ";
            }
        }

        private static string GetCommonJoins(string priceDiscType)
        {
            string typePart = priceDiscType == null ? "" : (" and pr.TYPE = " + priceDiscType);
            return
                "left outer join CURRENCY curr on curr.CURRENCYCODE = t.CURRENCY and curr.DATAAREAID = t.DATAAREAID " +
                "left outer join INVENTDIM i on i.INVENTDIMID = t.INVENTDIMID and i.DATAAREAID = t.DATAAREAID " +
                "left outer join INVENTDIMCOMBINATION id on id.INVENTDIMID = t.INVENTDIMID and id.DATAAREAID = t.DATAAREAID " +
                "left outer join RBOSIZES s on s.SIZE_ = i.INVENTSIZEID and s.DATAAREAID = t.DATAAREAID " +
                "left outer join RBOCOLORS c on c.COLOR = i.INVENTCOLORID and c.DATAAREAID = t.DATAAREAID " +
                "left outer join RBOSTYLES st on st.STYLE = i.INVENTSTYLEID and st.DATAAREAID = t.DATAAREAID " +
                "left outer join CUSTTABLE cu on t.ACCOUNTCODE = 0 and cu.ACCOUNTNUM = t.ACCOUNTRELATION and cu.DATAAREAID = t.DATAAREAID " +
                "left outer join PRICEDISCGROUP pr on t.ACCOUNTCODE = 1 and pr.MODULE = 1 " + typePart +
                " and pr.GROUPID = t.ACCOUNTRELATION and pr.DATAAREAID = t.DATAAREAID " +
                "left outer join INVENTTABLE inv on inv.ITEMID = t.ITEMRELATION and inv.DATAAREAID = t.DATAAREAID " +
                "left outer join UNIT u on u.UNITID = t.UNITID and u.DATAAREAID = t.DATAAREAID " +
                "left outer join PRICEDISCGROUP pr2 on t.ITEMRELATION = pr2.GROUPID and t.DATAAREAID = pr2.DATAAREAID and pr2.MODULE = 0 " +
                typePart + " ";
        }

        private static void PopulateTradeAgreement(IDataReader dr, OLDTradeAgreementEntry agreement)
        {
            agreement.ItemCode = (TradeAgreementEntry.TradeAgreementEntryItemCode) dr["ITEMCODE"];
            agreement.AccountCode = (TradeAgreementEntryAccountCode) dr["ACCOUNTCODE"];

            agreement.ID = (Guid) dr["ID"];
            agreement.ItemRelation = (string) dr["ITEMRELATION"];
            agreement.AccountRelation = (string) dr["ACCOUNTRELATION"];
            agreement.QuantityAmount = (decimal) dr["QUANTITYAMOUNT"];
            agreement.FromDate = Date.FromAxaptaDate(dr["FROMDATE"]);
            agreement.ToDate = Date.FromAxaptaDate(dr["TODATE"]);
            agreement.Amount = (decimal) dr["AMOUNT"];
            agreement.AmountIncludingTax = (decimal) dr["AMOUNTINCLTAX"];
            agreement.Currency = (string) dr["CURRENCY"];
            agreement.CurrencyDescription = (string) dr["CURRENCYDESCRIPTION"];
            agreement.Percent1 = (decimal) dr["PERCENT1"];
            agreement.Percent2 = (decimal) dr["PERCENT2"];
            agreement.SearchAgain = ((byte) dr["SEARCHAGAIN"] != 0);
            agreement.Relation = (TradeAgreementEntry.TradeAgreementEntryRelation) dr["RELATION"];
            agreement.UnitID = (string) dr["UNITID"];
            agreement.UnitDescription = (string) dr["UNITDESCRIPTION"];
            agreement.InventDimID = (string) dr["INVENTDIMID"];
            agreement.Markup = (decimal) dr["MARKUP"];
            agreement.AccountName = (string) dr["ACCOUNTNAME"];
            agreement.ItemRelationName = (string) dr["ITEMRELATIONNAME"];
        }

        private static void PopulateTradeAgreementWithVariantID(IDataReader dr, OLDTradeAgreementEntry agreement)
        {
            PopulateTradeAgreement(dr, agreement);

            agreement.VariantID = (string) dr["VARIANTID"];

            agreement.ItemName = (string) dr["ITEMNAME"];
            agreement.ItemID = (string) dr["ITEMID"];
        }

        /* not used
            private static void PopulateTradeAgreementPrices(IDataReader dr, TradeAgreementEntry agreement)
            {
                PopulateTradeAgreement(dr, agreement);

                agreement.SizeID = (string)dr["INVENTSIZEID"];
                agreement.ColorID = (string)dr["INVENTCOLORID"];
                agreement.StyleID = (string)dr["INVENTSTYLEID"];
                agreement.SizeName = (string)dr["INVENTSIZENAME"];
                agreement.ColorName = (string)dr["INVENTCOLORNAME"];
                agreement.StyleName = (string)dr["INVENTSTYLENAME"];

                agreement.ItemName = (string)dr["ITEMNAME"];
                agreement.ItemID = (string)dr["ITEMID"];
            }
            */

        private static string ResolveSort(TradeAgreementRelation relation)
        {
            if (relation == TradeAgreementRelation.PriceSales)
            {
                return " ORDER BY t.Amount ASC, t.ItemCode, t.ItemRelation, t.AccountCode," +
                       " t.AccountRelation, t.Currency , t.QuantityAmount ";
            }
            return " ORDER BY t.ItemCode, t.ItemRelation, t.AccountCode," +
                   " t.AccountRelation, t.Currency , t.QuantityAmount ";
        }


        /// <summary>
        /// Gets all trade agreement entries for an item that have a specific trade agreement type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="variantID">The unique ID of the item</param>
        /// <returns>A list of trade agreement entries for an item and a trade agreement type</returns>
        ///
        public List<OLDTradeAgreementEntry> GetAgreementsForVariant(
            IConnectionManager entry,
            RecordIdentifier variantID)
        {
            ValidateSecurity(entry);
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "select " + AllColumnSelection +
                                  "from PRICEDISCTABLE t " +
                                  GetCommonJoins(null) +
                                  "where id.RBOVARIANTID = @variantID ";
                MakeParam(cmd, "variantID", (string) variantID);

                return Execute<OLDTradeAgreementEntry>(entry, cmd, CommandType.Text, PopulateTradeAgreementWithVariantID);
            }
        }

    }
}


