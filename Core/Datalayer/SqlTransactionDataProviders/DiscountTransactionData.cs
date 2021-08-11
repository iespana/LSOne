using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects.DiscountItems;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class DiscountTransactionData : SqlServerDataProviderBase, IDiscountTransactionData
    {
        public virtual void Insert(IConnectionManager entry, ISaleLineItem saleLineItem, IDiscountItem discItem,int counter)
        {
            ValidateSecurity(entry);

            var statement = new SqlServerStatement("RBOTRANSACTIONDISCOUNTTRANS", StatementType.Insert, false);

            statement.AddKey("TRANSACTIONID", saleLineItem.Transaction.TransactionId);
            statement.AddKey("LINENUM", (decimal)saleLineItem.LineId, SqlDbType.Decimal);
            statement.AddKey("DISCLINENUM", (decimal)counter, SqlDbType.Decimal);
            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddKey("STORE", saleLineItem.Transaction.StoreId);
            statement.AddKey("TERMINAL", saleLineItem.Transaction.TerminalId);

            statement.AddField("DISCOUNTTYPE", discItem.DiscountType, SqlDbType.Int);

            if (discItem.DiscountType != DiscountTransTypes.Periodic)
            {
                statement.AddField("DISCOFFERNAME", discItem.DiscountName);
                statement.AddField("DISCOFFERID", (string)discItem.DiscountID);
            }
            else
            {
                statement.AddField("DISCOFFERNAME", saleLineItem.PeriodicDiscountOfferName);
                statement.AddField("DISCOFFERID", saleLineItem.PeriodicDiscountOfferId);
            }

            statement.AddField("PARTNERINFO", discItem.PartnerInfo);
            statement.AddField("ORIGIN", (int)discItem.Origin, SqlDbType.Int);

            switch (discItem.DiscountType)
            {
                case DiscountTransTypes.Periodic:
                    statement.AddField("DISCOUNTAMTWITHTAX", Math.Abs(saleLineItem.PeriodicDiscountWithTax), SqlDbType.Decimal);
                    statement.AddField("PERIODICDISCTYPE", (int)saleLineItem.PeriodicDiscType, SqlDbType.Int);
                    statement.AddField("QTYDISCOUNTED", saleLineItem.QuantityDiscounted, SqlDbType.Int);
                    statement.AddField("DISCOUNTPCT", discItem.Percentage, SqlDbType.Decimal);
                    statement.AddField("DISCOUNTAMT", Math.Abs(discItem.Amount), SqlDbType.Decimal);
                    break;

                case DiscountTransTypes.Customer:
                    statement.AddField("DISCOUNTAMTWITHTAX", Math.Abs(discItem.AmountWithTax), SqlDbType.Decimal);
                    statement.AddField("PERIODICDISCTYPE", (int)((CustomerDiscountItem)discItem).CustomerDiscountType, SqlDbType.Int);
                    statement.AddField("QTYDISCOUNTED", (int)saleLineItem.Quantity, SqlDbType.Int);
                    statement.AddField("DISCOUNTPCT", discItem.Percentage, SqlDbType.Decimal);
                    statement.AddField("DISCOUNTAMT", Math.Abs(discItem.Amount), SqlDbType.Decimal);
                    break;

                case DiscountTransTypes.LineDisc:
                case DiscountTransTypes.TotalDisc:
                    statement.AddField("DISCOUNTAMTWITHTAX", Math.Abs(discItem.AmountWithTax), SqlDbType.Decimal);
                    statement.AddField("PERIODICDISCTYPE", 0, SqlDbType.Int);
                    statement.AddField("QTYDISCOUNTED", (int)saleLineItem.Quantity, SqlDbType.Int);
                    statement.AddField("DISCOUNTPCT", discItem.Percentage, SqlDbType.Decimal);
                    statement.AddField("DISCOUNTAMT", Math.Abs(discItem.Amount), SqlDbType.Decimal);
                    break;
                case DiscountTransTypes.LoyaltyDisc:
                    statement.AddField("DISCOUNTAMTWITHTAX", Math.Abs(saleLineItem.LoyaltyDiscountWithTax), SqlDbType.Decimal);
                    statement.AddField("PERIODICDISCTYPE", 0, SqlDbType.Int);
                    statement.AddField("QTYDISCOUNTED", (int)saleLineItem.Quantity, SqlDbType.Int);
                    statement.AddField("DISCOUNTPCT", saleLineItem.LoyaltyPctDiscount, SqlDbType.Decimal);
                    statement.AddField("DISCOUNTAMT", Math.Abs(saleLineItem.LoyaltyDiscount), SqlDbType.Decimal);
                    break;

            }

            entry.Connection.ExecuteStatement(statement);
        }

        private static IDiscountItem PopulateDiscountItem(IConnectionManager entry, IDataReader dr, ISaleLineItem saleLine)
        {
            DiscountItem item;
            var discountType = (DiscountTransTypes)dr["DISCOUNTTYPE"];

            switch (discountType)
            {
                case DiscountTransTypes.Periodic:

                    item = new PeriodicDiscountItem();

                    var tempPerDisc = (PeriodicDiscountItem)item;
                    saleLine.PeriodicPctDiscount = (decimal)dr["DISCOUNTPCT"];                    
                    saleLine.PeriodicDiscountWithTax = (decimal)dr["DISCOUNTAMTWITHTAX"];
                    saleLine.PeriodicDiscType = (LineEnums.PeriodicDiscountType)(int)dr["PERIODICDISCTYPE"];
                    saleLine.PeriodicDiscountOfferId = (string)dr["DISCOFFERID"];
                    saleLine.PeriodicDiscountOfferName = (string)dr["DISCOFFERNAME"];
                    saleLine.QuantityDiscounted = (int)dr["QTYDISCOUNTED"];

                    switch (saleLine.PeriodicDiscType)
                    {
                        case LineEnums.PeriodicDiscountType.Multibuy:
                            tempPerDisc.PeriodicDiscountType = PeriodicDiscOfferType.Multibuy;
                            break;
                        case LineEnums.PeriodicDiscountType.MixAndMatch:
                            tempPerDisc.PeriodicDiscountType = PeriodicDiscOfferType.MixAndMatch;
                            break;
                        case LineEnums.PeriodicDiscountType.DiscountOffer:
                            tempPerDisc.PeriodicDiscountType = PeriodicDiscOfferType.Offer;
                            break;
                    }
                    break;

                case DiscountTransTypes.Customer:
                    item = new CustomerDiscountItem();

                    var tempCustomerDiscItem = (CustomerDiscountItem)item;
                    tempCustomerDiscItem.CustomerDiscountType = (CustomerDiscountTypes)(int)dr["PERIODICDISCTYPE"];
                    break;

                case DiscountTransTypes.LineDisc:
                    item = new LineDiscountItem();
                    break;

                case DiscountTransTypes.TotalDisc:
                    item = new TotalDiscountItem();
                    break;

                case DiscountTransTypes.LoyaltyDisc:
                    item = new LoyaltyDiscountItem();
                    break;

                default:
                    item = new LineDiscountItem(); // This point should never hit
                    break;
            }

            item.Percentage = (decimal)dr["DISCOUNTPCT"];
            item.Amount = (decimal)dr["DISCOUNTAMT"];
            item.AmountWithTax = (decimal)dr["DISCOUNTAMTWITHTAX"];
            item.DiscountName = (string)dr["DISCOFFERNAME"];
            item.DiscountID = (string)dr["DISCOFFERID"];
            item.PartnerInfo = (string)dr["PARTNERINFO"];
            item.Origin = (DiscountOrigin)(int)dr["ORIGIN"];

            return item;
        }

        public virtual List<IDiscountItem> GetDiscountItems(IConnectionManager entry, RecordIdentifier transactionID, IRetailTransaction transaction, ISaleLineItem saleLine, int lineNum)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"select LINENUM,DISCLINENUM,ISNULL(DISCOUNTTYPE,0) as DISCOUNTTYPE,
                                    ISNULL(DISCOUNTPCT,0.0) as DISCOUNTPCT,ISNULL(DISCOUNTAMT,0.0) as DISCOUNTAMT,
                                    ISNULL(DISCOUNTAMTWITHTAX,0.0) as DISCOUNTAMTWITHTAX,ISNULL(PERIODICDISCTYPE,0) as PERIODICDISCTYPE,
                                    ISNULL(DISCOFFERID,'') as DISCOFFERID,ISNULL(DISCOFFERNAME,'') as DISCOFFERNAME,
                                    ISNULL(QTYDISCOUNTED,0) as QTYDISCOUNTED,ISNULL(PARTNERINFO,'') as PARTNERINFO,
                                    ISNULL(ORIGIN,0) as ORIGIN
                                    from RBOTRANSACTIONDISCOUNTTRANS
                                    where TRANSACTIONID = @transactionID and STORE = @storeID and TERMINAL = @terminalID
                                    and LINENUM = @lineNum and DATAAREAID = @dataAreaID
                                    order by DISCLINENUM";

                MakeParam(cmd, "transactionID", (string)transactionID);
                MakeParam(cmd, "storeID", transaction.StoreId);
                MakeParam(cmd, "terminalID", transaction.TerminalId);
                MakeParam(cmd, "lineNum", (decimal)lineNum, SqlDbType.Decimal);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return Execute(entry, cmd, CommandType.Text, saleLine, PopulateDiscountItem);
            }
        }
    }
}
