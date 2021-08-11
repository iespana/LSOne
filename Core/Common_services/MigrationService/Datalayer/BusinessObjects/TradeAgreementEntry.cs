using System;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.BusinessObjects
{
   
    /// <summary>
    /// Data class for a trade agreement entry. Trade agreement entries represent either discounts or prices and connect f.x. items/item groups to customers/customer groups.
    /// </summary>
    public class OLDTradeAgreementEntry : DataEntity
    {
       

      


        /// <summary>
        /// Initializes a new instance of the <see cref="OLDTradeAgreementEntry"/> class.
        /// </summary>
        public OLDTradeAgreementEntry()
            : base()
        {
            ItemCode = TradeAgreementEntry.TradeAgreementEntryItemCode.Table;
            ItemID = "";
            ItemName = "";
            AccountCode = TradeAgreementEntryAccountCode.Table;
            AccountName = "";
            ItemRelation = "";
            AccountRelation = "";
            QuantityAmount = 0;
            FromDate = new Date(DateTime.Now);
            Currency = "";
            Relation = TradeAgreementEntry.TradeAgreementEntryRelation.PriceSales;
            UnitID = "";
            InventDimID = "";
            Amount = 0;
            Percent1 = 0;
            Percent2 = 0;
            SearchAgain = true;
            AmountIncludingTax = 0;
            Markup = 0;

            SizeID = "";
            ColorID = "";
            StyleID = "";

            SizeName = "";
            ColorName = "";
            StyleName = "";
            //recID = 0;
        }
        
        // On this entity we override the ID since the ID has massive 10 fold primary key.
        /// <summary>
        /// Old primary key that was used on this class before it was replaced by a GUID. This key should still remain unique.
        /// </summary>
        public RecordIdentifier OldID
        {
            get
            {
                return new RecordIdentifier((int)ItemCode,
                    new RecordIdentifier((int)AccountCode,
                    new RecordIdentifier(ItemRelation,
                    new RecordIdentifier(QuantityAmount,
                    new RecordIdentifier(AccountRelation,
                    new RecordIdentifier(FromDate,
                    new RecordIdentifier(Currency,
                    new RecordIdentifier((int)Relation,
                    new RecordIdentifier(UnitID, InventDimID)))))))));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Specifies if the trade agreement applies to a specific item, group of items or all items
        /// </summary>
        public TradeAgreementEntry.TradeAgreementEntryItemCode ItemCode { get; set; }

        /// <summary>
        /// Specifies if the trade agreement applies to a specific customer, group of customers or all customer
        /// </summary>
        public TradeAgreementEntryAccountCode AccountCode { get; set; }

        /// <summary>
        /// Returns the description of the account code selection
        /// </summary>
        public string AccountCodeText
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Returns the description of the account code selection
        /// </summary>
        /// <param name="allMeans">If account code is set to All then this string is returned</param>
        /// <returns>Returns the description of the account code selection</returns>
        public string GetAccountCodeText(string allMeans)
        {
            if (AccountCode == TradeAgreementEntryAccountCode.All)
                return allMeans;
            else
                return AccountCodeText;
        }

        /// <summary>
        /// Gets or sets the item relation (item code) on the trade agreement entry
        /// </summary>
        public RecordIdentifier ItemRelation { get; set; }

        /// <summary>
        /// Gets or sets the account relation (account code) on the trade agreement entry
        /// </summary>
        public RecordIdentifier AccountRelation { get; set; }

        /// <summary>
        /// Gets or sets the quantity needed to activate the line/multiline trade agreements. When using total discount trade agreement this field refers to a minimum amount of purchase to activate the discount.
        /// </summary>
        public decimal QuantityAmount { get; set; }
        /// <summary>
        /// Gets or sets the discount amount on the trade agreement when it's a line/multiline/total discount. For sales price trade agreements this gets or sets the price
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Gets or sets the markup that should be added to the price
        /// </summary>
        public decimal Markup { get; set; }
        /// <summary>
        /// Gets or sets the discount amount with tax for line/multiline/total discounts and price with tax for sales price trade agreements.
        /// </summary>
        public decimal AmountIncludingTax { get; set; }
        /// <summary>
        /// Gets or sets the first percentage discount value for the line/mulitline/total discounts
        /// </summary>        
        public decimal Percent1 { get; set; }
        /// <summary>
        /// Gets or sets the first percentage discount value for the line/mulitline/total
        /// discounts
        /// </summary>
        /// <remarks>
        /// Percentage 2 will be calculated after the percentage 1 has been calculated.
        /// Example: the item costs € 10,00, percentage 1 is set to 10%, percentage 2 is set
        /// to 20%. The amount to be paid is calculated as follows: 10 * 0,9 = 9; 9 * 0,8 =
        /// 7,20.
        /// </remarks>
        public decimal Percent2 { get; set; }

        /// <summary>
        /// End of trade agreement valid date. A blank value means the trade agreement has no end date (is valid from the FromDate). 
        /// Because DateTime objects can not be nullable, the value '01.01.1900' is considered blank
        /// </summary>
        public Date ToDate { get; set; }

        /// <summary>
        /// If true then the POS will continue to search for a trade agreement otherwise the price/discount calculations will stop at this trade agreement.
        /// </summary>
        public bool SearchAgain { get; set; }

        /// <summary>
        /// ID of size used in item dimension in trade agreement. NOT SAVED.
        /// </summary>
        public RecordIdentifier SizeID { get; set; }

        /// <summary>
        /// ID of style used in item dimension in trade agreement. NOT SAVED.
        /// </summary>
        public RecordIdentifier StyleID { get; set; }

        /// <summary>
        /// Display trade agreement size name. NOT SAVED.
        /// </summary>
        public string SizeName { get; set; }
        
        /// <summary>
        /// Display trade agreement color name. NOT SAVED.
        /// </summary>
        public string ColorName { get; set; }
        
        /// <summary>
        /// Display trade agreement style name. NOT SAVED.
        /// </summary>
        public string StyleName { get; set; }

        /// <summary>
        /// Display trade agreement account name. NOT SAVED.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// ID of item in trade agreement. NOT SAVED.
        /// </summary>
        public RecordIdentifier ItemID { get; set; }

        /// <summary>
        /// ID of color used in item dimension in trade agreement. NOT SAVED.
        /// </summary>
        public RecordIdentifier ColorID { get; set; }

        /// <summary>
        /// Display trade agreement item name. NOT SAVED.
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Start of trade agreement valid date.A blank value means the trade agreement has no start date (is always valid until the ToDate). 
        /// Because DateTime objects can not be nullable, the value '01.01.1900' is considered blank
        /// </summary>
        public Date FromDate { get; set; }        

        /// <summary>
        /// ID of the currency code used in trade agreement
        /// </summary>
        public RecordIdentifier Currency { get; set; }

        /// <summary>
        /// Display trade agreement currency description. NOT SAVED.
        /// </summary>
        public string CurrencyDescription { get; set; }
        /// <summary>
        /// What type of trade agreement is this entry; <see cref="TradeAgreementEntry.TradeAgreementEntryRelation"/>
        /// </summary>
        public TradeAgreementEntry.TradeAgreementEntryRelation Relation { get; set; }
        /// <summary>
        /// What unit does this trade agreement apply to.
        /// </summary>
        public RecordIdentifier UnitID { get; set; }

        /// <summary>
        /// Display trade agreement unit description. NOT SAVED.
        /// </summary>
        public string UnitDescription { get; internal set; }
        /// <summary>
        /// The unique ID of the item dimension the trade agreement applies to
        /// </summary>
        public RecordIdentifier InventDimID { get; set; }
        /// <summary>
        /// The name of the trade agreemen type; <see cref="TradeAgreementEntry.TradeAgreementEntryRelation"/>
        /// </summary>
        public string ItemRelationName { get; set; }

        /// <summary>
        /// The variant ID tied to the Trade agreement
        /// </summary>
        public RecordIdentifier VariantID { get; internal set; }

        
    }
}
