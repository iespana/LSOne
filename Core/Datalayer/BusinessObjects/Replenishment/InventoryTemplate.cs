using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;
using System;
using System.ComponentModel.DataAnnotations;

namespace LSOne.DataLayer.BusinessObjects.Replenishment
{
    public class InventoryTemplate : DataEntity
    {
        public InventoryTemplate()
        {
            ChangeVendorInLine = true;
            ChangeUomInLine = true;
            CalculateSuggestedQuantity = true;
            SetQuantityToSuggestedQuantity = true;
            DisplayReorderPoint = true;
            DisplayMaximumInventory = true;
            UseBarcodeUom = true;
            AllowAddNewLine = true;
            TemplateEntryType = TemplateEntryTypeEnum.PurchaseOrder;
            UnitSelection = UnitSelectionEnum.InventoryUnit;
            EnteringType = EnteringTypeEnum.AddToQty;
            QuantityMethod = QuantityMethodEnum.Ask;
            DefaultQuantity = 1;
            AreaID = Guid.Empty;
            DefaultStore = RecordIdentifier.Empty;
            DefaultVendor = RecordIdentifier.Empty;
            AutoPopulateTransferOrderReceivingQuantity = false;
            AllowImageImport = false;
            CreateGoodsReceivingDocument = false;
            AutoPopulateGoodsReceivingDocument = false;
        }

        public bool ChangeVendorInLine { get; set; }
        public bool ChangeUomInLine { get; set; }
        public bool CalculateSuggestedQuantity { get; set; }
        public bool SetQuantityToSuggestedQuantity { get; set; }
        public bool DisplayReorderPoint { get; set; }
        public bool DisplayMaximumInventory { get; set; }
        public bool DisplayBarcode { get; set; }
        public bool AddLinesWithZeroSuggestedQuantity { get; set; }
        public bool AllStores { get; set; }

        /// <summary>
        /// If true, when adding a new line in a document (Stock counting, purchase order, store transfer) in the mobile inventory app, the barcode unit of measure will be used if available.
        /// </summary>
        [LSOneUsage(CodeUsage.LSCommerce)]
        public bool UseBarcodeUom { get; set; }

        /// <summary>
        /// If true, allows adding a new line in an inventory document (Stock counting, purchase order, store transfer) in the mobile inventory app.
        /// </summary>
        [LSOneUsage(CodeUsage.LSCommerce)]
        public bool AllowAddNewLine { get; set; }

        /// <summary>
        /// Type of document
        /// </summary>
        public TemplateEntryTypeEnum TemplateEntryType { get; set; }

        /// <summary>
        /// Specifies which unit of measure is used in a document (Stock counting, purchase order, store transfer)
        /// </summary>
        public UnitSelectionEnum UnitSelection { get; set; }

        /// <summary>
        /// Specified if a line is added or merged with an existing line in an inventory document (Stock counting, purchase order, store transfer) in the inventory app.
        /// </summary>
        [LSOneUsage(CodeUsage.LSCommerce)]
        public EnteringTypeEnum EnteringType { get; set; }

        /// <summary>
        /// Specifies how the quantity is set when a new line is added to an inventory document (Stock counting, purchase order, store transfer) in the inventory app.
        /// </summary>
        [LSOneUsage(CodeUsage.LSCommerce)]
        public QuantityMethodEnum QuantityMethod { get; set; }

        /// <summary>
        /// Default quantity to be set when <see cref="QuantityMethod"></see> is not Ask./>
        /// </summary>
        /// <remarks>Default value is 1.</remarks>
        [LSOneUsage(CodeUsage.LSCommerce)]
        public decimal DefaultQuantity { get; set; }

        /// <summary>
        /// If true, allows adding images to inventory document (Stock counting, purchase order, store transfer) lines in the inventory app.
        /// </summary>
        [LSOneUsage(CodeUsage.LSCommerce)]
        public bool AllowImageImport { get; set; }

        public Guid AreaID { get; set; }

        /// <summary>
        /// The default store attached to the template. Only used in transfer order templates
        /// </summary>
        public RecordIdentifier DefaultStore { get; set; }

        /// <summary>
        /// The default vendor attached to the template. Only used in purchase order templates
        /// </summary>
        public RecordIdentifier DefaultVendor { get; set; }

        /// <summary>
        /// If true, sending a transfer order will set the receiving quantity equal to the sending quantity. Only used in transfer order templates
        /// </summary>
        public bool AutoPopulateTransferOrderReceivingQuantity { get; set; }

        /// <summary>
        /// If true, a goods receiving document will be created when a purchase order is created based on this template or when posting a worksheet
        /// </summary>
        public bool CreateGoodsReceivingDocument { get; set; }

        /// <summary>
        /// If true and <see cref="CreateGoodsReceivingDocument"/> is also true, then the goods receiving document will be auto populated with the lines from the purchase order
        /// </summary>
        public bool AutoPopulateGoodsReceivingDocument { get; set; }

        public bool Dirty { get; set; }
   
        [StringLength(100)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

    }
}
