using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using LSOne.Utilities.DataTypes;
[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.PricesAndDiscounts")]

namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts
{
    /// <summary>
    /// Tells you what the trade agreement entry is applied to; All, Group, Table (single customer and/or item)
    /// </summary>
    public enum TradeAgreementEntryAccountCode
    {
        /// <summary>
        /// Trade agreement applies to a specific item or a specific customer
        /// </summary>
        Table = 0, 
        /// <summary>
        /// Trade agreement applies to a group of either items or customers
        /// </summary>
        Group = 1, 
        /// <summary>
        /// Trade agreement applies to all items or customers
        /// </summary>
        All = 2,   
        /// <summary>
        /// Trade agreement applies to a customer - same as Table
        /// </summary>
        Customer = 0        
    }

    /// <summary>
    /// Data class for a trade agreement entry. Trade agreement entries represent either discounts or prices and connect f.x. items/item groups to customers/customer groups.
    /// </summary>
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class TradeAgreementEntry : OptimizedUpdateDataEntity
    {
        private TradeAgreementEntryItemCode itemCode;
        private RecordIdentifier itemID;
        private string itemName;
        private TradeAgreementEntryAccountCode accountCode;
        private string accountName;
        private RecordIdentifier itemRelation;
        private RecordIdentifier priceID;
        private RecordIdentifier accountRelation;
        private decimal quantityAmount;
        private Date fromDate;
        private Date toDate;
        private RecordIdentifier currency;
        private TradeAgreementEntryRelation relation;
        private RecordIdentifier unitID;
        private decimal amount;
        private decimal percent1;
        private decimal percent2;
        private bool searchAgain;
        private decimal amountIncludingTax;
        private decimal markup;

        /// <summary>
        /// Tells you what the trade agreement entry is applied to; All, Group, Table (single customer and/or item)
        /// </summary>
        public enum TradeAgreementEntryItemCode
        {
            /// <summary>
            /// Trade agreement applies to a specific item or a specific customer
            /// </summary>
            Table = 0,
            /// <summary>
            /// Trade agreement applies to a group of either items or customers
            /// </summary>
            Group = 1,
            /// <summary>
            /// Trade agreement applies to all items or customers
            /// </summary>
            All = 2
        }

        /// <summary>
        /// Enum specifying the type of a trade agreement discount/price
        /// </summary>
        public enum TradeAgreementEntryRelation
        {
            /// <summary>
            /// Not used
            /// </summary>
            PricePurch = 0,
            /// <summary>
            /// Not used
            /// </summary>
            LineDiscPurch = 1,
            /// <summary>
            /// Not used
            /// </summary>
            MultiLineDiscPurch = 2,
            /// <summary>
            /// Not used
            /// </summary>
            EndDiscPurch = 3,
            /// <summary>
            /// Trade agreement is a sales price
            /// </summary>
            PriceSales = 4,
            /// <summary>
            /// Trade agreement is a line discount
            /// </summary>
            LineDiscSales = 5,
            /// <summary>
            /// Trade agreement is a multiline discount
            /// </summary>
            MultiLineDiscSales = 6,
            /// <summary>
            /// Trade agreement is a total discount
            /// </summary>
            EndDiscSales = 7
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TradeAgreementEntry"/> class.
        /// </summary>
        public TradeAgreementEntry()
            : base()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            itemCode = TradeAgreementEntryItemCode.Table;
            itemID = RecordIdentifier.Empty;
            itemName = "";
            accountCode = TradeAgreementEntryAccountCode.Table;
            accountName = "";
            itemRelation = RecordIdentifier.Empty;
            priceID = RecordIdentifier.Empty;
            accountRelation = RecordIdentifier.Empty;
            quantityAmount = 0;
            fromDate = new Date(DateTime.Now);
            toDate = new Date(DateTime.Now);
            currency = RecordIdentifier.Empty;
            relation = TradeAgreementEntryRelation.PriceSales;
            unitID = RecordIdentifier.Empty;
            amount = 0;
            percent1 = 0;
            percent2 = 0;
            searchAgain = true;
            amountIncludingTax = 0;
            markup = 0;

            SizeID = RecordIdentifier.Empty;
            ColorID = RecordIdentifier.Empty;
            StyleID = RecordIdentifier.Empty;

            SizeName = "";
            ColorName = "";
            StyleName = "";
        }

        public override string Text
        {
            get
            {
                return $"ItemCode: {ItemCode}, AccountCode: {AccountCode}, ItemRelation: {ItemRelation}, PiceID: {PriceID}, QuantityAmount: {QuantityAmount}, AccountRelation: {AccountRelation}, FromDate: {FromDate.ToAxaptaSQLDate()}, Currency: {Currency}, Relation: {Relation}, UnitID: {UnitID}";
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
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
                    new RecordIdentifier(UnitID)))))))));
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Specifies if the trade agreement applies to a specific item, group of items or all items
        /// </summary>
        [DataMember(IsRequired = true)]
        public TradeAgreementEntryItemCode ItemCode
        {
            get
            {
                return itemCode;
            }
            set
            {
                if (itemCode != value)
                {
                    PropertyChanged("ITEMCODE", value);
                    itemCode = value;
                }
            }
        }

        /// <summary>
        /// Specifies if the trade agreement applies to a specific customer, group of customers or all customer
        /// </summary>
        [DataMember(IsRequired = true)]
        public TradeAgreementEntryAccountCode AccountCode
        {
            get
            {
                return accountCode;
            }
            set
            {
                if (accountCode != value)
                {
                    PropertyChanged("ACCOUNTCODE", value);
                    accountCode = value;
                }
            }
        }

        /// <summary>
        /// Returns the description of the account code selection
        /// </summary>
        [IgnoreDataMember]
        public string AccountCodeText
        {
            get
            {
                switch (AccountCode)
                {
                    case TradeAgreementEntryAccountCode.Table:
                        return Properties.Resources.Customer;

                    case TradeAgreementEntryAccountCode.Group:
                        return Properties.Resources.Group;

                    default:
                        return Properties.Resources.AllCustomers;
                }
            }
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
        [DataMember(IsRequired = true)]
        public RecordIdentifier ItemRelation
        {
            get
            {
                return itemRelation;
            }
            set
            {
                if (itemRelation != value)
                {
                    PropertyChanged("ITEMRELATION", value);
                    itemRelation = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the price id on the trade agreement entry
        /// </summary>
        [DataMember]
        public RecordIdentifier PriceID
        {
            get
            {
                return priceID;
            }
            set
            {
                priceID = value;
            }
        }

        /// <summary>
        /// Gets or sets the account relation (account code) on the trade agreement entry
        /// </summary>
        [DataMember(IsRequired = true)]
        public RecordIdentifier AccountRelation
        {
            get
            {
                return accountRelation;
            }
            set
            {
                if (accountRelation != value)
                {
                    PropertyChanged("ACCOUNTRELATION", value);
                    accountRelation = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the quantity needed to activate the line/multiline trade agreements. When using total discount trade agreement this field refers to a minimum amount of purchase to activate the discount.
        /// </summary>
        [DataMember(IsRequired = true)]
        public decimal QuantityAmount
        {
            get
            {
                return quantityAmount;
            }
            set
            {
                if (quantityAmount != value)
                {
                    PropertyChanged("QUANTITYAMOUNT", value);
                    quantityAmount = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the discount amount on the trade agreement when it's a line/multiline/total discount. For sales price trade agreements this gets or sets the price
        /// </summary>
        [DataMember]
        public decimal Amount
        {
            get
            {
                return amount;
            }
            set
            {
                if (amount != value)
                {
                    PropertyChanged("AMOUNT", value);
                    amount = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the markup that should be added to the price
        /// </summary>
        [DataMember]
        public decimal Markup
        {
            get
            {
                return markup;
            }
            set
            {
                if (markup != value)
                {
                    PropertyChanged("MARKUP", value);
                    markup = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the discount amount with tax for line/multiline/total discounts and price with tax for sales price trade agreements.
        /// </summary>
        [DataMember]
        public decimal AmountIncludingTax
        {
            get
            {
                return amountIncludingTax;
            }
            set
            {
                if (amountIncludingTax != value)
                {
                    PropertyChanged("AMOUNTINCLTAX", value);
                    amountIncludingTax = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the first percentage discount value for the line/mulitline/total discounts
        /// </summary>        
        [DataMember]
        public decimal Percent1
        {
            get
            {
                return percent1;
            }
            set
            {
                if (percent1 != value)
                {
                    PropertyChanged("PERCENT1", value);
                    percent1 = value;
                }
            }
        }

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
        [DataMember]
        public decimal Percent2
        {
            get
            {
                return percent2;
            }
            set
            {
                if (percent2 != value)
                {
                    PropertyChanged("PERCENT2", value);
                    percent2 = value;
                }
            }
        }

        /// <summary>
        /// End of trade agreement valid date. A blank value means the trade agreement has no end date (is valid from the FromDate). 
        /// Because DateTime objects can not be nullable, the value '01.01.1900' is considered blank
        /// </summary>
        [DataMember]
        public Date ToDate
        {
            get
            {
                return toDate;
            }
            set
            {
                if (toDate != Date.FromAxaptaDate(value.DateTime))
                {
                    PropertyChanged("TODATE", value);
                    toDate = value;
                }
            }
        }

        /// <summary>
        /// If true then the POS will continue to search for a trade agreement otherwise the price/discount calculations will stop at this trade agreement.
        /// </summary>
        [DataMember]
        public bool SearchAgain
        {
            get
            {
                return searchAgain;
            }
            set
            {
                if (searchAgain != value)
                {
                    PropertyChanged("SEARCHAGAIN", value);
                    searchAgain = value;
                }
            }
        }

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
        public string AccountName
        {
            get
            {
                return accountName;
            }
            set
            {
                accountName = value;
            }
        }

        /// <summary>
        /// ID of item in trade agreement. NOT SAVED.
        /// </summary>
        public RecordIdentifier ItemID
        {
            get
            {
                return itemID;
            }
            set
            {
                itemID = value;
            }
        }

        /// <summary>
        /// ID of color used in item dimension in trade agreement. NOT SAVED.
        /// </summary>
        public RecordIdentifier ColorID { get; set; }

        /// <summary>
        /// Display trade agreement item name. NOT SAVED.
        /// </summary>
        public string ItemName
        {
            get
            {
                return itemName;
            }
            set
            {
                itemName = value;
            }
        }

        /// <summary>
        /// Start of trade agreement valid date.A blank value means the trade agreement has no start date (is always valid until the ToDate). 
        /// Because DateTime objects can not be nullable, the value '01.01.1900' is considered blank
        /// </summary>
        [DataMember(IsRequired = true)]
        public Date FromDate
        {
            get
            {
                return fromDate;
            }
            set
            {
                if (fromDate != Date.FromAxaptaDate(value.DateTime))
                {
                    PropertyChanged("FROMDATE", value);
                    fromDate = value;
                }
            }
        }

        /// <summary>
        /// ID of the currency code used in trade agreement
        /// </summary>
        [DataMember(IsRequired = true)]
        public RecordIdentifier Currency
        {
            get
            {
                return currency;
            }
            set
            {
                if (currency != value)
                {
                    PropertyChanged("CURRENCY", value);
                    currency = value;
                }
            }
        }

        /// <summary>
        /// Display trade agreement currency description. NOT SAVED.
        /// </summary>
        public string CurrencyDescription { get; set; }

        /// <summary>
        /// What type of trade agreement is this entry; <see cref="TradeAgreementEntryRelation"/>
        /// </summary>
        [DataMember(IsRequired = true)]
        public TradeAgreementEntryRelation Relation
        {
            get
            {
                return relation;
            }
            set
            {
                if (relation != value)
                {
                    PropertyChanged("RELATION", value);
                    relation = value;
                }
            }
        }

        /// <summary>
        /// What unit does this trade agreement apply to.
        /// </summary>
        [DataMember(IsRequired = true)]
        public RecordIdentifier UnitID
        {
            get
            {
                return unitID;
            }
            set
            {
                if (unitID != value)
                {
                    PropertyChanged("UNITID", value);
                    unitID = value;
                }
            }
        }

        /// <summary>
        /// Display trade agreement unit description. NOT SAVED.
        /// </summary>
        public string UnitDescription { get; internal set; }

        /// <summary>
        /// The name of the trade agreemen type; <see cref="TradeAgreementEntryRelation"/>
        /// </summary>
        public string ItemRelationName { get; set; }

        /// <summary>
        /// The variant description tied to the Trade agreement
        /// </summary>
        public string VariantName { get; internal set; }

        public bool IsVariantItem;

        public override List<string> GetIgnoredColumns()
        {
            // Ignore AMOUNTINCLTAX because it is calculated when saving anyway and 
			// if we don't ignore it, the item loaded from DB 
			// will look different than the one we want to save via a web request
            return new List<string> { "AMOUNTINCLTAX" };
        }
    }
}