using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.TransactionObjects.DiscountItems;
using LSOne.DataLayer.TransactionObjects.Line.Hospitality;
using LSOne.DataLayer.TransactionObjects.Line.Loyalty;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportClasses.Employee;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;

namespace LSOne.DataLayer.TransactionObjects.Line.SaleItem
{
    /// <summary>
    /// A standard sale item line in a transaction.
    /// </summary>
    [Serializable]
    public class SaleLineItem : BaseSaleItem, ISaleLineItem
    {        
        #region Member variables

        //SaleType
        private SaleType typeOfSale;                //The type of item sale
        private BarCode.BarcodeEntryType entryType; //The method of item entry, scanned,keyboard,selected
        //Quantity
        private bool qtyBecomesNegative;             //If set to true the quantity sign will be changed
        //Price
        private bool zeroPriceValid;                //Is a zero price valid for the product.
        private bool usedForPriceCheck;             //Is set to true if doing a price check operation, which will skip certain functionalities such as serial numbers.
        //SaleDetails
        private bool weightManuallyEntered;         //Is set to true if weight was manully entered.
        private bool scaleItem;                     //Is set to true if the item was weight on a scale.
        private bool priceInBarcode;                //Is set to true if the price was read from the barcode.
        private bool weightInBarcode;               //Is set to true if weight of the item was read from the barcode.   
        private int tareWeight;                 //Packaging weight to be subtracted from weighted value
        
        //Key in
        private KeyInPrices keyInPrice;             //Rules for keying in prices for the item
        private KeyInQuantities keyInQuantity;      //Rules for keying in quantities for the item
        private bool mustKeyInComment;                           //If a comment must be entered for the item
        private bool mustSelectUOM;                 //If a UOM must be selected for the item
        //Dimensions
        private Dimension dimensions;               //The item dimensions
        //Linked items
        private bool isLinkedItem;                  //Is set to true if item is a linked item
        private bool isInfoCodeItem;                //Is set to true if item is an info code item
        private int linkedToLineId;                 //Is the line id of the item the linked item is linked to.
        //private SaleLineItem linkedToSaleItem;      //The saleItem the line is linked to.
        private bool isReferencedByLinkItems;       //Is set to true if other lines are linked to this item.
        private int paymentIndex;        //The index of the tender that has already been used to pay the item

        private string fleetCardNumber;              //Fleet card number used on this item payment

        private LineMultilineDiscountOnItem lineMultiLineDiscOnItem; //Tells us if the item has customer line and/or multiline discount items

        private string batchId;                     // The id of the batch which the item belongs to.
        private DateTime batchExpDate = new DateTime(1900, 1, 1);            // The exp date of the batch which the item belongs to.
        private decimal oiltax;                     // The oiltax amount
        private bool shouldBeManuallyRemoved;       // Notifies the cahier that this item should be removed in order to continue
        private string returnReceiptId = "";
        private string returnTransId = "";          // If the item is being returned, this field stores the id of the original transaction the item was sold in.
        private int returnLineId;                   // If the item is being returned, this field stores the item's line id in the original transaction the item was sold in.
        private decimal returnedQty;                // If the item is being returned, thsi field stores the allowed return qty for this item line. 
        private string returnStoreId = "";          // If the item is being return, this field stores the store id where the item was originally bought
        private string returnTerminalId = "";       // If the item is being return, this field stores the terminal id where the item was originally bought
        private bool receiptReturnItem;             // True if the item is a return item returned through the "Return Transaction" operation, thereby from a receipt
        private bool returnItem;                    // True if the item is a return item returned through the "Return Item" operation
        private decimal returnableAmount = 0M;      // Added March 2010 to be able to keep track of the amount of that item that can be returned when it comes to returning a transaction
        private ReasonCode reasonCode;              // If the item is being returned, this field stores the reason code for return

        private SalesTransaction.ItemClassTypeEnum itemClassType;
        
        private DiscountVoucherItem discountVoucher;        

        //Hospitality        
        private bool splitMarked;                        //Is the item marked during split bill and transfer
        private int coverNo;                        //The guest the item is assigned to        
        private bool splitItem;                     //Has this item been split in SplitBill operation
        private int splitGroup;                     //Which split group does the item belong to
        private int splitOriginLineNo;              //if the item is a split item then this is the original line no
        private int splitParentLineId;                      
        private int splitLineId;
        private IMenuTypeItem menuTypeItem;          // The menu type of the item
        private SalesTransaction.PrintStatus printingStatus;  // Has this item been sent to the station printer.
        private bool originatesFromForecourt = false;       // Was the item injected by a forecourt controller
        private RecordIdentifier activeHospitalitySalesType = "";   // Used to identify the hospitalityType where the item is sold
        private ILoyaltyItem loyaltyPoints;

        #endregion

        #region properties

        /// <summary>
        /// Has the points that were calculated for this specific line item
        /// </summary>
        public ILoyaltyItem LoyaltyPoints 
        { 
            get { return loyaltyPoints; } 
            set { loyaltyPoints = value; } 
        }

        /// <summary>
        /// //The type of item sale, i.e selling item,item group, etc.
        /// </summary>
        public SaleType TypeOfSale
        {
            get { return typeOfSale; }
            set { typeOfSale = value; }
        }

        /// <summary>
        /// The method of item entry, scanned,keyboard,selected.
        /// </summary>
        //internal ItemEntryType EntryType
        public BarCode.BarcodeEntryType EntryType
        {
            get { return entryType; }
            set { entryType = value; }
        }

        /// <summary>
        /// If set to true the quantity sign will be changed
        /// </summary>
        public bool QtyBecomesNegative
        {
            get { return qtyBecomesNegative; }
            set { qtyBecomesNegative = value; }
        }

        /// <summary>
        /// Is a zero price valid for the product.
        /// </summary>
        public bool ZeroPriceValid
        {
            get { return zeroPriceValid; }
            set { zeroPriceValid = value; }
        }

        /// <summary>
        /// Is set to true if doing a price check operation, which will skip certain functionalities such as serial numbers.
        /// </summary>
        public bool UsedForPriceCheck
        {
            get { return usedForPriceCheck; }
            set { usedForPriceCheck = value; }
        }

        /// <summary>
        /// Is set to true if weight was manully entered.
        /// </summary>
        public bool WeightManuallyEntered
        {
            get { return weightManuallyEntered; }
            set { weightManuallyEntered = value; }
        }

        /// <summary>
        /// Is set to true if the item was weight on a scale
        /// </summary>
        public bool ScaleItem
        {
            get { return scaleItem; }
            set { scaleItem = value; }
        }

        /// <summary>
        /// Packaging weight to be subtracted from weighted value
        /// </summary>
        public int TareWeight
        {
            get { return tareWeight; }
            set { tareWeight = value; }
        }

        /// <summary>
        /// Is set to true if the price was read from the barcode.
        /// </summary>
        public bool PriceInBarcode
        {
            get { return priceInBarcode; }
            set { priceInBarcode = value; }
        }

        /// <summary>
        /// Is set to true if weight of the item was read from the barcode.
        /// </summary>
        public bool WeightInBarcode
        {
            get { return weightInBarcode; }
            set { weightInBarcode = value; }
        }

        /// <summary>
        /// The sales person assigned to this item
        /// </summary>
        public Employee SalesPerson { get; set; }
       
        /// <summary>
        /// Rules for keying in prices for the item
        /// </summary>
        public KeyInPrices KeyInPrice
        {
            get { return keyInPrice; }
            set { keyInPrice = value; }
        }
        /// <summary>
        /// Rules for keying in quantities for the item
        /// </summary>
        public KeyInQuantities KeyInQuantity
        {
            get { return keyInQuantity; }
            set { keyInQuantity = value; }
        }

        public bool MustKeyInComment
        {
            get { return mustKeyInComment; }
            set { mustKeyInComment = value; }
        }

        public bool MustSelectUOM
        {
            get { return mustSelectUOM; }
            set { mustSelectUOM = value; }
        }

        /// <summary>
        /// The item dimensions, color,size,style
        /// </summary>
        public Dimension Dimension
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        /// <summary>
        /// Is set to true if item is a linked item
        /// </summary>
        public bool IsLinkedItem
        {
            get { return isLinkedItem; }
            set { isLinkedItem = value; }
        }

        /// <summary>
        /// Is set to true if item is a linked item
        /// </summary>
        public bool IsInfoCodeItem
        {
            get { return isInfoCodeItem; }
            set { isInfoCodeItem = value; }
        }

        /// <summary>
        /// Is the line id of the item the linked item is linked to.
        /// </summary>
        public int LinkedToLineId
        {
            get { return linkedToLineId; }
            set { linkedToLineId = value; }
        }

        /// <summary>
        /// Is set to true if other lines are linked to this item.
        /// </summary>
        public bool IsReferencedByLinkItems
        {
            get { return isReferencedByLinkItems; }
            set { isReferencedByLinkItems = value; }
        }

        /// <summary>
        /// The limitation master ID that is attached to the item when paying with a tender type that has limits on which items can be paid for
        /// </summary>
        public RecordIdentifier LimitationMasterID { get; set; }

        /// <summary>
        /// The limitation code that is attached to the item when paying with a tender type that has limits on which items can be paid for
        /// </summary>
        public string LimitationCode { get; set; }

        /// <summary>
        /// The active assembly for the current store for the sold item if it is an assembly item
        /// </summary>
        public RetailItemAssembly ItemAssembly { get; set; }

        /// <summary>
        /// The active assembly for the current store that the sold item is a component of
        /// </summary>
        public RetailItemAssembly ParentAssembly { get; set; }

        /// <summary>
        /// Is assembly
        /// </summary>
        public bool IsAssembly { get; set; }

        /// <summary>
        /// Is assembly component
        /// </summary>
        public bool IsAssemblyComponent { get; set; }

        /// <summary>
        /// ID of the assembly
        /// </summary>
        public RecordIdentifier AssemblyID { get; set; }

        /// <summary>
        /// ID of the assembly component
        /// </summary>
        public RecordIdentifier AssemblyComponentID { get; set; }

        /// <summary>
        /// Line ID of the parent assembly item
        /// </summary>
        public int AssemblyParentLineID { get; set; }

        /// <summary>
        /// The ID of the tender that has already been used to pay the item
        /// </summary>
        public int PaymentIndex
        {
            get { return paymentIndex; }
            set { paymentIndex = value; }
        }
        /// <summary>
        /// Fleet card number used on this item payment
        /// </summary>        
        public string FleetCardNumber
        {
            get { return fleetCardNumber; }
            set { fleetCardNumber = value; }
        }
                
        public string BatchId
        {
            get { return batchId ?? ""; }
            set { batchId = value; }
        }

        public bool ShouldBeManuallyRemoved
        {
            get { return shouldBeManuallyRemoved; }
            set { shouldBeManuallyRemoved = value; }
        }

        public DateTime BatchExpDate
        {
            get { return batchExpDate; }
            set { batchExpDate = value; }
        }
        
        /// <summary>
        /// The calculated oiltax amount for the item.
        /// </summary>
        public decimal Oiltax
        {
            get { return oiltax; }
            set { oiltax = value; }
        }

        /// <summary>
        /// Does the item have customer line and/or multiline discount item on it
        /// </summary>  
        public LineMultilineDiscountOnItem LineMultiLineDiscOnItem
        {
            get { return lineMultiLineDiscOnItem; }
            set { lineMultiLineDiscOnItem = value; }
        }

        public LineMultilineDiscountOnItem ILineMultiLineDiscOnItem
        {
            get { return lineMultiLineDiscOnItem; }
        }

        /// <summary>
        /// If the item is being returned (with a receipt) this field contains the id of the receipt the
        /// item was originally sold in.
        /// </summary>  
        public string ReturnReceiptId { get { return returnReceiptId; } set { returnReceiptId = value; } }

        /// <summary>
        /// If the item is being returned (with a receipt) this field contains the id of the transaction the
        /// item was originally sold in.
        /// </summary>  
        public string ReturnTransId
        {
            get { return returnTransId; }
            set { returnTransId = value; }
        }
         
        /// <summary>
        /// // If the item is being returned, this field stores the item's line id in the original transaction the item was sold in.
        /// </summary>  
        public int ReturnLineId
        {
            get { return returnLineId; }
            set { returnLineId = value; }
        }

        /// <summary>
        /// // If the item is being returned, this field stores the allowed return qty for this item line. 
        /// </summary>  
        public decimal ReturnedQty
        {
            get { return returnedQty; }
            set { returnedQty = value; }
        }

        /// <summary>
        /// If the item is being return, this field stores the store id where the item was originally bought
        /// </summary>
        public string ReturnStoreId
        {
            get { return returnStoreId; }
            set { returnStoreId = value; }
        }

        /// <summary>
        /// If the item is being return, this field stores the terminal id where the item was originally bought
        /// </summary>
        public string ReturnTerminalId
        {
            get { return returnTerminalId; }
            set { returnTerminalId = value; }
        }

        public ReasonCode ReasonCode
        {
            get { return reasonCode; }
            set { reasonCode = value; }
        }

        /// <summary>
        /// // True if the item is a return item returned through the "Return Transaction" operation, thereby from a receipt
        /// </summary>  
        public bool ReceiptReturnItem
        {
            get { return receiptReturnItem; }
            set { receiptReturnItem = value; }
        }

        public bool ReturnItem
        {
            get { return returnItem; }
            set { returnItem = value; }
        }

        public IDiscountVoucherItem DiscountVoucher
        {
            get { return (IDiscountVoucherItem)discountVoucher; }
            set { discountVoucher = (DiscountVoucherItem)value; }
        }

        public SalesTransaction.ItemClassTypeEnum ItemClassType
        {
            get { return itemClassType; }
            set { itemClassType = value; }
        }

        /// <summary>
        /// To keep track of the amount that is left and can be returned without having to access the original Transaction repeatedly (added March 2010)
        /// </summary>
        public decimal ReturnableAmount
        {
            get { return returnableAmount; }
            set { returnableAmount = value; }
        }

        /// <summary>
        /// Is the line marked for a split bill or transfer
        /// </summary>
        public bool SplitMarked
        {
            get { return splitMarked; }
            set { splitMarked = value; }
        }

        /// <summary>
        /// The guest the item is assigned to
        /// </summary>
        public int CoverNo
        {
            get { return coverNo; }
            set { coverNo = value; }
        }

        /// <summary>
        /// If true then the quantity of the item has been split up in the SplitBill operation.
        /// </summary>
        public bool SplitItem
        {
            get { return splitItem; }
            set { splitItem = value; }
        }

        public int SplitGroup
        {
            get { return splitGroup; }
            set { splitGroup = value; }
        }

        public int SplitOriginLineNo
        {
            get { return splitOriginLineNo; }
            set { splitOriginLineNo = value; }
        }

        public int SplitParentLineId
        {
            get { return splitParentLineId; }
            set { splitParentLineId = value; }
        }

        public int SplitLineId
        {
            get { return splitLineId; }
            set { splitLineId = value; }
        }

        /// <summary>
        /// The menu type of the item
        /// </summary>
        public IMenuTypeItem MenuTypeItem
        {
            get { return menuTypeItem; }
            set { menuTypeItem = value; }
        }

        /// <summary>
        /// Has this item been sent to the station printer.
        /// </summary>
        public SalesTransaction.PrintStatus PrintingStatus
        {
            get { return printingStatus; }
            set { printingStatus = value; }
        }

        /// <summary>
        /// Used in hospitality to know what the quantity was the last time the item was sent to the kitchen.
        /// Needs to be known for SetQty and ClearQty combinations between printouts
        /// </summary>
        public decimal LastPrintedQty { get; set; }

        public bool OriginatesFromForecourt
        {
            get { return originatesFromForecourt; }
            set { originatesFromForecourt = value; }
        }

        public RecordIdentifier ActiveHospitalitySalesType
        {
            get { return activeHospitalitySalesType; }
            set { activeHospitalitySalesType = value; }
        }

        public int LimitationSplitParentLineId { get; set; }
        public int LimitationSplitChildLineId { get; set; }
        public bool BlockDiscountLinkItem { get; set; }

        /// <summary>
        /// ID of an item used by the KDS system
        /// </summary>
        public Guid KdsId { get; set; }

        public decimal TotalDiscountExact { get; set; }
        public decimal TotalDiscountWithTaxExact { get; set; }
        public decimal LineDiscountExact { get; set; }
        public decimal LineDiscountWithTaxExact { get; set; }
        public decimal NetAmountExact { get; set; }
        public decimal NetAmountWithTaxExact { get; set; }

        /// <summary>
        /// The production time of an item. This tells how long it takes to cook and prepare the item.
        /// </summary>
        public int ProductionTime { get; set; }

        public decimal PaymentAmount
        {
            get
            {
                return PaymentIndex > 0 ? Transaction.GetTenderItem(PaymentIndex).Amount : 0;
            }
        }        

        #endregion

        /// <summary>
        /// Further implimentation needed.
        /// </summary>
        protected enum DiscountAdjType
        {
            UseCustomerNotPeriodicDisc = 0,
            UsePeriodicNotCustomerDisc = 1,
            UseCustomerAndPeriodicDisc = 2,
        }

        public SaleLineItem(IRetailTransaction transaction)
            : base()
        {
            dimensions = new Dimension();
            reasonCode = new ReasonCode();

            if (transaction != null)
            {
                menuTypeItem = new MenuTypeItem((MenuTypeItem)transaction.MenuTypeItem);
                this.TaxIncludedInItemPrice = transaction.TaxIncludedInPrice;
                this.Transaction = transaction;
            }
            
            this.PeriodicDiscType = LineEnums.PeriodicDiscountType.None;
            this.NoManualDiscountAllowed = false;
            this.DiscountsWereRemoved = false;
            this.CanBeModified = true;
            this.LineMultiLineDiscOnItem = LineMultilineDiscountOnItem.None;           
            this.itemClassType = GetItemClassType(this);
            this.SplitItem = false;
            this.splitLineId = 0;
            this.PrintingStatus = SalesTransaction.PrintStatus.Printable;
            this.LastPrintedQty = decimal.Zero;
            this.linkedToLineId = -1;
            this.Comment = "";
            this.UnitOfMeasureChanged = false;
            this.LoyaltyPoints = new LoyaltyItem();
            this.SalesPerson = new Employee();     
            this.LimitationMasterID = RecordIdentifier.Empty;            
            this.LimitationCode = "";
            this.paymentIndex = -1;
            this.usedForPriceCheck = false;
            this.LimitationSplitParentLineId = -1;
            this.LimitationSplitChildLineId = -1;
            this.BlockDiscountLinkItem = false;
            this.AssemblyID = RecordIdentifier.Empty;
            this.AssemblyComponentID = RecordIdentifier.Empty;
            this.AssemblyParentLineID = -1;

            KdsId = Guid.NewGuid();
        }

        public SaleLineItem(ICustomerPaymentTransaction transaction, RecordIdentifier storeID, string currencyCode, bool taxIncludedInPrice)
            : base()
        {
            dimensions = new Dimension();
            reasonCode = new ReasonCode();

            if (transaction != null)
            {
                this.Transaction = new RetailTransaction((string)storeID, currencyCode, taxIncludedInPrice);
            }
            
            this.PeriodicDiscType = LineEnums.PeriodicDiscountType.None;
            this.NoManualDiscountAllowed = false;
            this.DiscountsWereRemoved = false;
            this.CanBeModified = true;
            this.LineMultiLineDiscOnItem = LineMultilineDiscountOnItem.None;           
            this.itemClassType = GetItemClassType(this);
            this.SplitItem = false;
            this.splitLineId = 0;
            this.PrintingStatus = SalesTransaction.PrintStatus.Printable;
            this.LastPrintedQty = decimal.Zero;
            this.linkedToLineId = -1;
            this.Comment = "";
            this.UnitOfMeasureChanged = false;
            this.LoyaltyPoints = new LoyaltyItem();
            this.SalesPerson = new Employee();     
            this.LimitationMasterID = RecordIdentifier.Empty;
            this.LimitationCode = "";
            this.paymentIndex = -1;
            this.LimitationSplitParentLineId = -1;
            this.LimitationSplitChildLineId = -1;
            this.BlockDiscountLinkItem = false;
            this.AssemblyID = RecordIdentifier.Empty;
            this.AssemblyComponentID = RecordIdentifier.Empty;
            this.AssemblyParentLineID = -1;

            KdsId = Guid.NewGuid();
        }


        public SaleLineItem(SaleLineItem item, RetailTransaction transaction = null)
        {            
            item.Populate(this, transaction ?? (RetailTransaction)Transaction);
        }

        public SaleLineItem(BarCode barCode, IRetailTransaction transaction)
            : this(transaction)
        {
            this.Found = barCode.Found;

            if (barCode.Found)
            {
                this.ItemId = (string)barCode.ItemID;
                this.BarcodeId = (string)barCode.ItemBarCode;
                this.ItemDepartmentId = barCode.ItemDepartmentId;
                this.ItemGroupId = barCode.ItemGroupId;
                this.entryType = barCode.EntryType;
                this.Description = barCode.Description;
                this.ScaleItem = barCode.ScaleItem;
                this.ZeroPriceValid = barCode.ZeroPriceValid;
                this.QtyBecomesNegative = barCode.QtyBecomesNegative;
                this.NoDiscountAllowed = barCode.NoDiscountAllowed;
                this.KeyInPrice = barCode.KeyInPrice;
                this.KeyInQuantity = barCode.KeyInQuantity;
                this.IncludedInTotalDiscount = barCode.IncludedInTotalDiscount;
                this.LineDiscountGroup = barCode.LineDiscountGroup;
                this.MultiLineDiscountGroup = barCode.MultiLineDiscountGroup;
                this.UnitQuantity = barCode.QtySold;
                this.SalesOrderUnitOfMeasure = (string)barCode.UnitID;
                this.Blocked = barCode.Blocked;
                this.DateToBeBlocked = barCode.DateToBeBlocked;
                this.DateToActivateItem = new Date(barCode.DateToBeActivated);
                this.dimensions.VariantNumber = barCode.VariantID;
                this.dimensions.EnterDimensions = barCode.EnterDimensions;
                this.BatchId = barCode.BatchId;                

                if (barCode.BarcodePrice != 0)
                {
                    PriceInBarcode = true;
                    PriceWithTax = barCode.BarcodePrice;
                }
                if (barCode.BarcodeQuantity != 0)
                {
                    this.Quantity = barCode.BarcodeQuantity;
                }

                this.ItemType = (ItemTypeEnum)barCode.ItemType;
                this.itemClassType = GetItemClassType(this);
            }
            
            this.menuTypeItem = new MenuTypeItem();
        }

        ~SaleLineItem()
        {
            if (DiscountLines != null)
                DiscountLines.Clear();
            dimensions = null;
            menuTypeItem = null;
            reasonCode = null;
        }

        /// <summary>
        /// Returns the type of item class this is
        /// </summary>
        protected virtual SalesTransaction.ItemClassTypeEnum GetItemClassType(SaleLineItem saleItem)
        {
            return SalesTransaction.ItemClassTypeEnum.SaleLineItem;
        }

        public override object Clone()
        {
            var item = new SaleLineItem((RetailTransaction)Transaction);
            Populate(item, (RetailTransaction)Transaction);
            return item;
        }

        public virtual SaleLineItem Clone(IRetailTransaction transaction)
        {
            var item = new SaleLineItem(transaction);
            Populate(item, transaction);
            return item;
        }

        protected void Populate(SaleLineItem item, IRetailTransaction transaction)
        {
            base.Populate(item);
            item.Transaction = transaction;
            item.EntryType = EntryType;
            item.IsLinkedItem = IsLinkedItem;
            item.IsInfoCodeItem = IsInfoCodeItem;
            item.KeyInPrice = KeyInPrice;
            item.KeyInQuantity = KeyInQuantity;
            item.MustKeyInComment = MustKeyInComment;
            item.MustSelectUOM = MustSelectUOM;
            item.LinkedToLineId = LinkedToLineId;
            item.PriceInBarcode = PriceInBarcode;
            item.QtyBecomesNegative = QtyBecomesNegative;            
            item.ScaleItem = ScaleItem;
            item.TareWeight = TareWeight;
            item.TypeOfSale = TypeOfSale;
            item.WeightInBarcode = WeightInBarcode;
            item.WeightManuallyEntered = WeightManuallyEntered;
            item.ZeroPriceValid = ZeroPriceValid;
            item.BatchId = BatchId;
            item.BatchExpDate = BatchExpDate;
            item.LineMultiLineDiscOnItem = LineMultiLineDiscOnItem;
            item.SplitItem = SplitItem;
            item.SplitLineId = SplitLineId;
            item.menuTypeItem = (MenuTypeItem)menuTypeItem.Clone();
            item.printingStatus = printingStatus;
            item.LastPrintedQty = LastPrintedQty;
            item.activeHospitalitySalesType = (RecordIdentifier)activeHospitalitySalesType.Clone();
            item.typeOfSale = typeOfSale;
            item.entryType = entryType;
            item.zeroPriceValid = zeroPriceValid;
            item.weightManuallyEntered = weightManuallyEntered;
            item.scaleItem = scaleItem;
            item.priceInBarcode = priceInBarcode;
            item.weightInBarcode = weightInBarcode;
            item.SalesPerson = (Employee)SalesPerson.Clone();            
            item.keyInPrice = keyInPrice;
            item.keyInQuantity = keyInQuantity;
            item.mustKeyInComment = mustKeyInComment;
            item.mustSelectUOM = mustSelectUOM;
            item.dimensions = (Dimension)dimensions.Clone();
            item.isLinkedItem = isLinkedItem;
            item.isReferencedByLinkItems = isReferencedByLinkItems;
            item.LimitationMasterID = LimitationMasterID;
            item.LimitationCode = LimitationCode;
            item.paymentIndex = paymentIndex;
            item.fleetCardNumber = fleetCardNumber;
            item.lineMultiLineDiscOnItem = lineMultiLineDiscOnItem;
            item.batchId = batchId;
            item.batchExpDate = batchExpDate;
            item.oiltax = oiltax;
            item.shouldBeManuallyRemoved = shouldBeManuallyRemoved;
            item.returnTransId = returnTransId;
            item.returnLineId = returnLineId;
            item.returnedQty = returnedQty;
            item.returnStoreId = returnStoreId;
            item.returnTerminalId = returnTerminalId;
            item.reasonCode = ReasonCode == null ? new ReasonCode() : (ReasonCode)ReasonCode.Clone();
            item.returnReceiptId = returnReceiptId;
            item.returnableAmount = returnableAmount;
            item.coverNo = coverNo;
            item.SplitOriginLineNo = SplitOriginLineNo;
            item.SplitGroup = SplitGroup;
            item.SplitParentLineId = SplitParentLineId;
            item.OriginatesFromForecourt = OriginatesFromForecourt;
            item.OriginalDiscountVoucherPriceWithTax = OriginalDiscountVoucherPriceWithTax;
            item.itemClassType = itemClassType;
            item.LoyaltyPoints = (LoyaltyItem)LoyaltyPoints.Clone();
            item.receiptReturnItem = receiptReturnItem;
            item.returnItem = returnItem;
            item.KdsId = KdsId;
            item.UnitOfMeasureChanged = UnitOfMeasureChanged;
            item.ValidationPeriod = ValidationPeriod;
            item.NoPriceCalculation = NoPriceCalculation;
            item.TotalDiscountExact = TotalDiscountExact;
            item.TotalDiscountWithTaxExact = TotalDiscountWithTaxExact;
            item.LineDiscountExact = LineDiscountExact;
            item.LineDiscountWithTaxExact = LineDiscountWithTaxExact;
            item.NetAmountExact = NetAmountExact;
            item.NetAmountWithTaxExact = NetAmountWithTaxExact;
            item.LimitationSplitParentLineId = LimitationSplitParentLineId;
            item.LimitationSplitChildLineId = LimitationSplitChildLineId;
            item.BlockDiscountLinkItem = BlockDiscountLinkItem;
            item.ItemAssembly = ItemAssembly;
            item.ParentAssembly = ParentAssembly;
            item.IsAssemblyComponent = IsAssemblyComponent;
            item.IsAssembly = IsAssembly;
            item.AssemblyID = AssemblyID;
            item.AssemblyComponentID = AssemblyComponentID;
            item.AssemblyParentLineID = AssemblyParentLineID;
        }

        /// <summary>
        /// Updates the loyalty points on the sale line item. 
        /// </summary>
        /// <param name="loyaltyPoints">The points </param>
        public void Add(ILoyaltyItem loyaltyPoints)
        {
            this.LoyaltyPoints.CalculatedPoints = loyaltyPoints.CalculatedPoints;
        }

        /// <summary>
        ///  Adding a customer discount line item to the transactionline
        /// </summary>
        public void Add(ICustomerDiscountItem discountItem)
        {
            discountItem.EndDateTime = DateTime.Now;

            ResetLineMultiLineDiscountSettings(discountItem);
            this.DiscountLines.Add(discountItem);
        }

        public void ResetLineMultiLineDiscountSettings()
        {
            foreach (IDiscountItem line in DiscountLines.Where(w => w is ICustomerDiscountItem))
            {
                ResetLineMultiLineDiscountSettings((ICustomerDiscountItem)line);
            }
        }

        public void ResetLineMultiLineDiscountSettings(ICustomerDiscountItem discountItem)
        {
            if (discountItem == null)
            {
                return;
            }
            
            /*
             * It depends on settings in table SalesParameters.Disc how the combination of 
             * customer line discount + customer multiline discount
             * is handled i.e. only line discount value used, only multiline value used, 
             * they're added to gether, highest discount found and etc...
             * so we need to know what the combination of these two discounts (if any) is on the sale item
             * before the discount calculations are done
             */

            if (discountItem.CustomerDiscountType == CustomerDiscountTypes.LineDiscount)
            {
                if (this.LineMultiLineDiscOnItem == LineMultilineDiscountOnItem.Multiline)
                {
                    this.LineMultiLineDiscOnItem = LineMultilineDiscountOnItem.Both;
                }
                else if (this.LineMultiLineDiscOnItem == LineMultilineDiscountOnItem.None)
                {
                    this.LineMultiLineDiscOnItem = LineMultilineDiscountOnItem.Line;
                }
            }
            else if (discountItem.CustomerDiscountType == CustomerDiscountTypes.MultiLineDiscount)
            {
                if (this.LineMultiLineDiscOnItem == LineMultilineDiscountOnItem.Line)
                {
                    this.LineMultiLineDiscOnItem = LineMultilineDiscountOnItem.Both;
                }
                else if (this.LineMultiLineDiscOnItem == LineMultilineDiscountOnItem.None)
                {
                    this.LineMultiLineDiscOnItem = LineMultilineDiscountOnItem.Multiline;
                }
            }
        }

        public bool ShouldCalculateAndDisplayAssemblyPrice()
        {
            // Note that an item can be assembly item and at the same time 
            // be a component of another assembly item
            return (!IsAssembly || !ItemAssembly.CalculatePriceFromComponents) && 
                   (!IsAssemblyComponent || ParentAssembly.CalculatePriceFromComponents);
        }

        public bool ShouldDisplayTotalAssemblyComponentPrice(ExpandAssemblyLocation assemblyLocation)
        {
            return IsAssembly && ItemAssembly.CalculatePriceFromComponents && !ItemAssembly.ShallDisplayWithComponents(assemblyLocation);
        }

        /// <summary>
        ///  Adding a periodic discount line item to the transactionline
        /// </summary>
        public void Add(IPeriodicDiscountItem discountItem)
        {
            discountItem.EndDateTime = DateTime.Now;
            this.DiscountLines.Add(discountItem);           
        }

        /// <summary>
        ///  Adding a periodic discount line item to the transactionline
        /// </summary>
        public void Add(IDiscountItem discountItem)
        {
            discountItem.EndDateTime = DateTime.Now; 
            this.DiscountLines.Add(discountItem);
        }

        /// <summary>
        /// Clear discount lines of certain type
        /// </summary>
        /// <param name="discountTypeToBeCleared">The type of discount to be cleared</param>
        public void ClearDiscountLines(Type discountTypeToBeCleared)
        {
            //Remove periodic discounts.
            for(int i = 0; i < this.DiscountLines.Count; i++)
            {
                DiscountItem discountLine = (DiscountItem)this.DiscountLines[i];

                if (((discountTypeToBeCleared == typeof(ILoyaltyDiscountItem) || discountTypeToBeCleared == typeof(LoyaltyDiscountItem)) && discountLine.GetType() == typeof(LoyaltyDiscountItem))
                    || ((discountTypeToBeCleared == typeof(ITotalDiscountItem) || discountTypeToBeCleared == typeof(TotalDiscountItem)) && discountLine.GetType() == typeof(TotalDiscountItem)) 
                    || ((discountTypeToBeCleared == typeof(IPeriodicDiscountItem) || discountTypeToBeCleared == typeof(PeriodicDiscountItem)) && discountLine.GetType() == typeof(PeriodicDiscountItem)) 
                    || ((discountTypeToBeCleared == typeof(ICustomerDiscountItem) || discountTypeToBeCleared == typeof(CustomerDiscountItem)) && discountLine.GetType() == typeof(CustomerDiscountItem))
                    || ((discountTypeToBeCleared == typeof(ILineDiscountItem) || discountTypeToBeCleared == typeof(LineDiscountItem)) && discountLine.GetType() == typeof(LineDiscountItem)))
                {
                    this.DiscountLines.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Clear all customer discount lines 
        /// </summary>
        /// <param name="deleteTotalCustDisc">Should the Customer Total Discount be deleted too</param>
        public void ClearCustomerDiscountLines(bool deleteTotalCustDisc)
        {
            //Create a list for item´s to be removed
            List<DiscountItem> deleteList = new List<DiscountItem>();
            foreach (DiscountItem discountLine in this.DiscountLines)
            {
                if (discountLine is CustomerDiscountItem)
                {
                    CustomerDiscountItem custDiscItem = (CustomerDiscountItem)discountLine;
                    if (custDiscItem.CustomerDiscountType == CustomerDiscountTypes.TotalDiscount && deleteTotalCustDisc)
                    {
                        deleteList.Add(custDiscItem);
                    }
                    else if (custDiscItem.CustomerDiscountType != CustomerDiscountTypes.TotalDiscount)
                    {
                        deleteList.Add(custDiscItem);
                    }
                }                
            }

            //Remove peridic discounts.
            foreach (DiscountItem discountLine in deleteList)
            {
                this.DiscountLines.Remove(discountLine);
            }
        }

        /// <summary>
        /// Finds out if some types of discount needs to be removed
        /// </summary>
        protected void AdjustDiscount(DiscountAdjType discountAjdType)
        {
            bool periodicDiscFound = false;
            bool customerDiscFound = false;
 
            switch (discountAjdType)
            {
                case DiscountAdjType.UseCustomerAndPeriodicDisc:
                    {
                    }
                    break;
                case DiscountAdjType.UseCustomerNotPeriodicDisc:
                    {
                        foreach (DiscountItem discountLine in this.DiscountLines)
                        {
                            if (discountLine.GetType() == typeof(PeriodicDiscountItem))
                            {
                                PeriodicDiscountItem periodicDiscLineItem = (PeriodicDiscountItem)discountLine;
                                if (periodicDiscLineItem.PeriodicDiscountType != PeriodicDiscOfferType.MixAndMatch)
                                {
                                    periodicDiscFound = true;
                                }
                            }
                            if (discountLine.GetType() == typeof(CustomerDiscountItem))
                            {
                                customerDiscFound = true;
                            }

                        }
                        if (customerDiscFound && periodicDiscFound) { ClearDiscountLines(typeof(CustomerDiscountItem)); }
                    }
                    break;
                case DiscountAdjType.UsePeriodicNotCustomerDisc:
                    {
                        foreach (DiscountItem discountLine in this.DiscountLines)
                        {
                            if (discountLine.GetType() == typeof(PeriodicDiscountItem))
                            {
                                periodicDiscFound = true;
                            }
                            if (discountLine is CustomerDiscountItem)
                            {
                                customerDiscFound = true;
                            }

                        }
                        if (customerDiscFound && periodicDiscFound) { ClearDiscountLines(typeof(CustomerDiscountItem)); }
                    }
                    break;
            }
        }



        public bool HasDiscounts()
        {
            bool discExists = false;
            foreach (IDiscountItem ldi in this.DiscountLines)
            {                
                if (ldi.Amount != 0M || ldi.Percentage != 0)
                {
                    discExists = true;
                    break;
                }
            }
            return discExists;
        }


        public ISaleLineItem CreateSaleLineItem(ISaleLineItem saleLineItem)
        {
            return new SaleLineItem((RetailTransaction)saleLineItem.Transaction);
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            try
            {
                /*
                * Strings      added as is
                * Int          added as is
                * Bool         added as is
                * 
                * Decimal      added with ToString()
                * DateTime     added with DevUtilities.Utility.DateTimeToXmlString()
                * 
                * Enums        added with an (int) cast   
                * 
               */
                XElement xSaleItem = new XElement("SaleLineItem",
                    new XElement("typeOfSale", Conversion.ToXmlString((int)typeOfSale)),
                    new XElement("entryType", Conversion.ToXmlString((int)entryType)),
                    new XElement("qtyBecomesNegative", Conversion.ToXmlString(qtyBecomesNegative)),
                    new XElement("zeroPriceValid", Conversion.ToXmlString(zeroPriceValid)),
                    new XElement("weightManuallyEntered", Conversion.ToXmlString(weightManuallyEntered)),
                    new XElement("scaleItem", Conversion.ToXmlString(scaleItem)),
                    new XElement("tareWeight", Conversion.ToXmlString(tareWeight)),
                    new XElement("priceInBarcode", Conversion.ToXmlString(priceInBarcode)),
                    new XElement("weightInBarcode", Conversion.ToXmlString(weightInBarcode)),
                    new XElement("employee", SalesPerson.ToXML()),
                    new XElement("keyInPrice", Conversion.ToXmlString((int)keyInPrice)),
                    new XElement("keyInQuantity", Conversion.ToXmlString((int)keyInQuantity)),
                    new XElement("mustKeyInComment", Conversion.ToXmlString(mustKeyInComment)),
                    new XElement("mustSelectUOM", Conversion.ToXmlString(mustSelectUOM)),
                    new XElement("dimension", dimensions.ToXML()),
                    new XElement("reasonCode", reasonCode.ToXML()),
                    new XElement("isLinkedItem", Conversion.ToXmlString(isLinkedItem)),
                    new XElement("isInfoCodeItem", Conversion.ToXmlString(isInfoCodeItem)),
                    new XElement("linkedToLineId", Conversion.ToXmlString(linkedToLineId)),
                    new XElement("isReferencedByLinkItems", Conversion.ToXmlString(isReferencedByLinkItems)),
                    new XElement("TenderRestrictionCode", LimitationMasterID),
                    new XElement("LimitationCode", LimitationCode),
                    new XElement("paymentIndex", Conversion.ToXmlString(paymentIndex)),
                    new XElement("fleetCardNumber", fleetCardNumber),
                    new XElement("lineMultiLineDiscOnItem", Conversion.ToXmlString((int)lineMultiLineDiscOnItem)),
                    new XElement("batchId", batchId),
                    new XElement("batchExpDate", Conversion.ToXmlString(batchExpDate)),
                    new XElement("oiltax", Conversion.ToXmlString(oiltax)),
                    new XElement("shouldBeManuallyRemoved", Conversion.ToXmlString(shouldBeManuallyRemoved)),
                    new XElement("returnTransId", returnTransId),
                    new XElement("returnLineId", Conversion.ToXmlString(returnLineId)),
                    new XElement("returnedQty", Conversion.ToXmlString(returnedQty)),
                    new XElement("returnStoreId", returnStoreId),
                    new XElement("returnTerminalId", returnTerminalId),
                    new XElement("returnReceiptId", returnReceiptId),
                    new XElement("receiptReturnItem", Conversion.ToXmlString(receiptReturnItem)),
                    new XElement("returnItem", Conversion.ToXmlString(returnItem)),
                    new XElement("returnableAmount", Conversion.ToXmlString(returnableAmount)),
                    new XElement("itemClassType", Conversion.ToXmlString((int)itemClassType)),
                    new XElement("splitMarked", Conversion.ToXmlString(splitMarked)),
                    new XElement("coverNo", Conversion.ToXmlString(coverNo)),
                    new XElement("splititem", Conversion.ToXmlString(splitItem)),
                    new XElement("splitGroup", Conversion.ToXmlString(splitGroup)),
                    new XElement("splitOriginLineNo", Conversion.ToXmlString(splitOriginLineNo)),
                    new XElement("splitParentLineId", Conversion.ToXmlString(splitParentLineId)),
                    new XElement("splitLineId", Conversion.ToXmlString(splitLineId)),
                    menuTypeItem.ToXML(errorLogger),
                    new XElement("sentToStationPrinter", Conversion.ToXmlString((int)printingStatus)),
                    new XElement("LastPrintedQty", Conversion.ToXmlString(LastPrintedQty)),
                    new XElement("originatesFromForecourt", Conversion.ToXmlString(originatesFromForecourt)),
                    new XElement("hospitalitySalesType", activeHospitalitySalesType),
                    new XElement("LoyaltyPointsItem", LoyaltyPoints.ToXML()),
                    new XElement("KdsId", Conversion.ToXmlString(KdsId)),
                    new XElement("UnitOfMeasureChanged", Conversion.ToXmlString(UnitOfMeasureChanged)),
                    new XElement("limitationSplitParentLineId", Conversion.ToXmlString(LimitationSplitParentLineId)),
                    new XElement("limitationSplitChildLineId", Conversion.ToXmlString(LimitationSplitChildLineId)),
                    new XElement("discountSplitLineId", Conversion.ToXmlString(BlockDiscountLinkItem)),
                    new XElement("itemAssembly", ItemAssembly?.ToXML()),
                    new XElement("parentAssembly", ParentAssembly?.ToXML()),
                    new XElement("isAssemblyComponent", Conversion.ToXmlString(IsAssemblyComponent)),
                    new XElement("isAssembly", Conversion.ToXmlString(IsAssembly)),
                    new XElement("assemblyID", AssemblyID.StringValue),
                    new XElement("assemblyComponentID", AssemblyComponentID.StringValue),
                    new XElement("assemblyParentLineID", Conversion.ToXmlString(AssemblyParentLineID)),
                new XElement("productionTime", Conversion.ToXmlString(ProductionTime))
                );

                xSaleItem.Add(base.ToXML(errorLogger));
                return xSaleItem;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "SaleLineItem.ToXml", ex);

                throw;
            }
        }

        public override void ToClass(XElement xItem, IErrorLog errorLogger = null)
        {
            try
            {
                if (xItem.HasElements)
                {                    
                    IEnumerable<XElement> classVariables = xItem.Elements();
                    foreach (XElement xVariable in classVariables)
                    {
                        if (!xVariable.IsEmpty)
                        {
                            try
                            {
                                switch (xVariable.Name.ToString())
                                {
                                    case "typeOfSale":
                                        typeOfSale = (SaleType)Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "entryType":
                                        entryType = (BarCode.BarcodeEntryType)Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "qtyBecomesNegative":
                                        qtyBecomesNegative = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "zeroPriceValid":
                                        zeroPriceValid = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "weightManuallyEntered":
                                        weightManuallyEntered = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "scaleItem":
                                        scaleItem = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "tareWeight":
                                        tareWeight = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "priceInBarcode":
                                        priceInBarcode = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "weightInBarcode":
                                        weightInBarcode = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "employee":
                                        SalesPerson.ToClass(xVariable, errorLogger);
                                        break;
                                    case "salespersonId": //FOR BACKWARDS COMPATABILITY SUSPENDED TRANSACTIONS F.EX.
                                        SalesPerson.ID = xVariable.Value;
                                        break;
                                    case "salespersonName": //FOR BACKWARDS COMPATABILITY SUSPENDED TRANSACTIONS F.EX.
                                        SalesPerson.Name = xVariable.Value;
                                        break;
                                    case "keyInPrice":
                                        keyInPrice = (KeyInPrices)Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "keyInQuantity":
                                        keyInQuantity = (KeyInQuantities)Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "mustKeyInComment":
                                        mustKeyInComment = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "mustSelectUOM":
                                        mustSelectUOM = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "dimension":
                                        dimensions.ToClass(xVariable, errorLogger);
                                        break;
                                    case "reasonCode":
                                        reasonCode.ToClass(xVariable, errorLogger);
                                        break;
                                    case "isLinkedItem":
                                        isLinkedItem = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "isInfoCodeItem":
                                        isInfoCodeItem = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "linkedToLineId":
                                        linkedToLineId = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "isReferencedByLinkItems":
                                        isReferencedByLinkItems = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "tenderRestrictionId":
                                    case "TenderRestrictionCode":
                                        LimitationMasterID = Conversion.XmlStringToGuid(xVariable.Value);
                                        break;
                                    case "LimitationCode":
                                        LimitationCode = xVariable.Value;
                                        break;
                                    case "paymentIndex":
                                        paymentIndex = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "fleetCardNumber":
                                        fleetCardNumber = xVariable.Value;
                                        break;
                                    case "lineMultiLineDiscOnItem":
                                        lineMultiLineDiscOnItem = (LineMultilineDiscountOnItem)Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "batchId":
                                        batchId = xVariable.Value;
                                        break;
                                    case "batchExpDate":
                                        batchExpDate = Conversion.XmlStringToDateTime(xVariable.Value);
                                        break;
                                    case "oiltax":
                                        oiltax = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "shouldBeManuallyRemoved":
                                        shouldBeManuallyRemoved = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "returnTransId":
                                        returnTransId = xVariable.Value;
                                        break;
                                    case "returnLineId":
                                        returnLineId = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "returnedQty":
                                        returnedQty = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "returnStoreId":
                                        returnStoreId = xVariable.Value;
                                        break;
                                    case "returnTerminalId":
                                        returnTerminalId = xVariable.Value;
                                        break;
                                    case "returnReceiptId":
                                        returnReceiptId = xVariable.Value;
                                        break;
                                    case "receiptReturnItem":
                                        receiptReturnItem = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "returnItem":
                                        returnItem = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "returnableAmount":
                                        returnableAmount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "itemClassType":
                                        itemClassType = (SalesTransaction.ItemClassTypeEnum)Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "splitMarked":
                                        splitMarked = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "coverNo":
                                        coverNo = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "splititem":
                                        splitItem = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "splitGroup":
                                        splitGroup = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "splitOriginLineNo":
                                        splitOriginLineNo = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "splitParentLineId":
                                        splitParentLineId = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "splitLineId":
                                        splitLineId = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "menuTypeItem":
                                        MenuTypeItem.ToClass(xVariable, errorLogger);
                                        break;
                                    case "sentToStationPrinter":
                                        printingStatus = (SalesTransaction.PrintStatus)Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "LastPrintedQty":
                                        LastPrintedQty = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "originatesFromForecourt":
                                        originatesFromForecourt = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "hospitalitySalesType":
                                        activeHospitalitySalesType = xVariable.Value;
                                        break;
                                    case "LoyaltyPointsItem":
                                        LoyaltyPoints.ToClass(xVariable);
                                        break;
                                    case "KdsId":
                                        KdsId = Conversion.XmlStringToGuid(xVariable.Value);
                                        break;
                                    case "UnitOfMeasureChanged":
                                        UnitOfMeasureChanged = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "limitationSplitParentLineId":
                                        LimitationSplitParentLineId = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "limitationSplitChildLineId":
                                        LimitationSplitChildLineId = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "discountSplitLineId":
                                        BlockDiscountLinkItem = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "itemAssembly":
                                        if (ItemAssembly == null)
                                        {
                                            ItemAssembly = new RetailItemAssembly();
                                        }
                                        ItemAssembly.ToClass(xVariable);
                                        break;
                                    case "parentAssembly":
                                        if (ParentAssembly == null)
                                        {
                                            ParentAssembly = new RetailItemAssembly();
                                        }
                                        ParentAssembly.ToClass(xVariable);
                                        break;
                                    case "isAssemblyComponent":
                                        IsAssemblyComponent = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "isAssembly":
                                        IsAssembly = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "assemblyID":
                                        AssemblyID = xVariable.Value;
                                        break;
                                    case "assemblyComponentID":
                                        AssemblyComponentID = xVariable.Value;
                                        break;
                                    case "assemblyParentLineID":
                                        AssemblyParentLineID = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "productionTime":
                                        ProductionTime = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    default:
                                        base.ToClass(xVariable);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                errorLogger?.LogMessage(LogMessageType.Error, "SaleLineItem:" + xVariable.Name, ex);
                            }
                        }
                    }                                         
                }                
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "SaleLineItem.ToClass", ex);

                throw;
            }            
        }

        public decimal GetCalculatedNetAmount(bool withTax)
        {
            decimal netAmount = 0;

            if (IsAssembly && ItemAssembly.CalculatePriceFromComponents)
            {
                foreach (var componentItemLine in GetAllAssemblyComponentsThatHasPrice())
                {
                        netAmount += withTax ? componentItemLine.NetAmountWithTax : componentItemLine.NetAmount;
                }
            }
            else
            {
                netAmount = withTax ? NetAmountWithTax : NetAmount;
            }

            return netAmount;
        }

        public decimal GetCalculatedPrice(bool withTax)
        {
            decimal price = 0;

            if (IsAssembly && ItemAssembly.CalculatePriceFromComponents)
            {
                foreach (var componentItemLine in GetAllAssemblyComponentsThatHasPrice())
                {
                    price += withTax ? componentItemLine.PriceWithTax : componentItemLine.Price;
                }
            }
            else
            {
                price = withTax ? PriceWithTax : Price;
            }

            return price;
        }

        public ISaleLineItem GetRootAssemblyLineItem()
        {
            return IsAssemblyComponent ? Transaction.SaleItems.First(item => item.LineId == LinkedToLineId) : null;
        }

        private IEnumerable<ISaleLineItem> GetAllAssemblyComponentsThatHasPrice()
        {
            return Transaction.SaleItems.Where(item => item.ShouldCalculateAndDisplayAssemblyPrice() && item.IsAssemblyComponent && item.LinkedToLineId == LineId);
        }
    }
}
