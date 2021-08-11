using System;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Transactions
{
    [Serializable]
    public class SalesTransaction : BaseTransaction
    {
        public enum PeriodicDiscountTypeEnum
        {
            /// <summary>
            /// 0
            /// </summary>
            None = 0,
            /// <summary>
            /// 1
            /// </summary>
            Multibuy = 1,
            /// <summary>
            /// 2
            /// </summary>
            MixAndMatch = 2,
            /// <summary>
            /// 3
            /// </summary>
            DiscountOffer = 3
        }

        /// <summary>
        /// Defines what type of item it is in data i.e. RboTransactionSalesTrans
        /// </summary>
        public enum ItemClassTypeEnum
        {
            /// <summary>
            /// 0
            /// </summary>
            SaleLineItem = 0,
            /// <summary>
            /// 1
            /// </summary>
            DiscountVoucherItem = 1,
            /// <summary>
            /// 2
            /// </summary>
            GiftCertificateItem = 2,
            /// <summary>
            /// 3
            /// </summary>
            FuelSalesLineItem = 3,
            /// <summary>
            /// 4
            /// </summary>
            PharmacySalesLineItem = 4,
            /// <summary>
            /// 5
            /// </summary>
            CreditMemo = 5,
            /// <summary>
            /// 6
            /// </summary>
            IncomeExpenseItem = 6,
            /// <summary>
            /// 7
            /// </summary>
            SalesOrder = 7,
            /// <summary>
            /// 8
            /// </summary>
            SalesInvoice = 8

        }

        public enum PrintStatus
        {
            /// <summary>
            /// Item can be printed
            /// </summary>
            Printable = 0,
            /// <summary>
            /// Item has been printed
            /// </summary>
            Printed = 1,
            /// <summary>
            /// Item will not be 
            /// </summary>
            Unprintable = 2,
        }

        public SalesTransaction()
        {
            PrescriptionId = "";
            ItemID = "";
            ItemName = "";
            Comment = "";
            DosageType = "";
            DosageStrengthUnit = "";
            CreditMemoNumber = "";
        }

        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(TransactionID, LineNum);
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public decimal LineNum { get; set; }
        public ItemClassTypeEnum ItemType { get; set; }
        public bool HasItemType { get; set; }
        public bool GiftCard { get; set; }
        public RecordIdentifier PrescriptionId { get; set; }
        public int PumpID { get; set; }
        public int SourceLineNum { get; set; }
        public RecordIdentifier ItemID { get; set; }
        public string ItemName { get; set; }
        public string CreditMemoNumber { get; set; }
        public decimal Price { get; set; }
        public string Comment { get; set; }
        public string DosageType { get; set; }
        public string DosageStrengthUnit { get; set; }
        public decimal DosageStrength { get; set; }
        public decimal DosageUnitQuantity { get; set; }
        public TransactionStatus EntryStatus { get; set; }
        public string BarCode { get; set; }
        public RecordIdentifier TaxGroup { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetPrice { get; set; }
        public decimal NetAmount { get; set; }
        public decimal NetAmountIncludeTax { get; set; }
        public bool HasNetAmountIncludeTax { get; set; }
        public decimal Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool ItemIDScanned { get; set; }
        public RecordIdentifier SalesPersonID { get; set; }
        public RecordIdentifier Unit { get; set; }
        public decimal UnitQuantity { get; set; }
        public RecordIdentifier InventSerialID { get; set; }
        public RecordIdentifier RFIDTagID { get; set; }
        public bool TaxIncludedInPrice { get; set; }
        public bool HasTaxIncludedInPrice { get; set; }
        public RecordIdentifier VariantID { get; set; }
        public RecordIdentifier BatchID { get; set; }
        public bool HasBatchExpDate { get; set; }
        public Date BatchExpDate { get; set; }
        public decimal OilTax { get; set; }
        public decimal ReturnLineID { get; set; }
        public decimal ReturnQuantity { get; set; }
        public bool IsLinkedItem { get; set; }
        public bool IsAssembly { get; set; }
        public bool IsAssemblyComponent { get; set; }
        public decimal AssemblyParentLineID { get; set; }
        public decimal LinkedToLineID { get; set; }
        public RecordIdentifier ReturnTransActionID { get; set; }
        public bool IsInfoCodeItem { get; set; }
        public RecordIdentifier ItemDepartmentID { get; set; }
        public RecordIdentifier ItemGroupID { get; set; }
        public bool PriceInBarCode { get; set; }
        public bool PriceOverridden { get; set; }
        public bool WeightManuallyEntered { get; set; }
        public bool LineWasDiscounted { get; set; }
        public bool ScaleItem { get; set; }
        public bool WeightInBarcode { get; set; }
        public RecordIdentifier LimitationMasterID { get; set; }
        public decimal PriceUnit { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal TotalDiscountPercent { get; set; }
        public decimal TotalDiscountWithTax { get; set; }
        public decimal LineDiscount { get; set; }
        public decimal LineDiscountPercent { get; set; }
        public decimal LineDiscountAmountWithTax { get; set; }
        public PeriodicDiscountTypeEnum PeriodicDiscountType { get; set; }
        public decimal PeriodicDiscountAmount { get; set; }
        public decimal PeriodicDiscountAmountWithTax { get; set; }
        public RecordIdentifier DiscountOfferID { get; set; }

        public decimal CostAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal WholeDiscountAmountWithTax { get; set; }
        public RecordIdentifier CustomerAccountID { get; set; }
    }
}
