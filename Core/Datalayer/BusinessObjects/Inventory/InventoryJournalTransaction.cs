using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    [KnownType(typeof(Date))]
    public class InventoryJournalTransaction : DataEntity
    {
        public InventoryJournalTransaction() : base()
        {
            LineNum = RecordIdentifier.Empty;
            ItemName = "";
            ReasonId = "";
            AreaID = Guid.Empty;
            LineStatus = JournalTransStatusEnum.AdjustmentCalculated;
            PictureID = RecordIdentifier.Empty;
            OmniLineID = "";
            OmniTransactionID = "";
        }

        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.String)]
        public RecordIdentifier StoreId { get; set; }

        //Replaced by Status property
        [DataMember]
        public bool Posted { get; set; }

        [DataMember]
        public InventoryJournalStatus Status { get; set; }

        [DataMember]
        public DateTime PostedDateTime { get; set; }

        [DataMember]
        public DateTime TransDate { get; set; }

        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.String)]
        public RecordIdentifier ItemId { get; set; }

        [DataMember]
        public string ItemName { get; internal set; }

        [DataMember]
        public decimal Adjustment { get; set; }

        [DataMember]
        public decimal AdjustmentInInventoryUnit { get; set; }

        [DataMember]
        public decimal CostPrice { get; set; }

        [DataMember]
        public decimal PriceUnit { get; set; }

        [DataMember]
        public decimal CostMarkup { get; set; }

        [DataMember]
        public decimal CostAmount { get; set; }

        [DataMember]
        public decimal SalesAmount { get; set; }

        [DataMember]
        public decimal InventOnHandInInventoryUnits { get; set; }

        [DataMember]
        public decimal Counted { get; set; }

        [DataMember]
        public string VariantName { get; set; }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier UnitID { get; set; }

        [DataMember]
        public string UnitDescription { get; internal set; }

        [DataMember]
        public DecimalLimit UnitQuantityLimiter { get; set; }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier InventoryUnitID { get; set; }

        [DataMember]
        public string InventoryUnitDescription { get; internal set; }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(JournalId, LineNum);
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
        /// Unique indentifier of the inventory journal line/transaction
        /// </summary>
        [DataMember]
        public RecordIdentifier MasterID { get; set; }

        /// <summary>
        ///Parked inventory item/line identifier that was moved partially or completely to main inventory.
        /// </summary>
        [DataMember]
        public RecordIdentifier ParentMasterID { get; set; }

        /// <summary>
        /// Parked inventory item/line quantity moved to main inventory.
        /// </summary>
        [DataMember]
        public decimal MovedQuantity { get; set; }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier ReasonId { get; set; }
        [DataMember]
        public string ReasonText { get; set; }

        /// <summary>
        /// Gets or sets the Profit/Loss journal row identifier
        /// </summary>
        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier JournalId { get; set; }
        /// <summary>
        /// Gets or sets the current line number within a Profit/Loss Journal entry
        /// </summary>
        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier LineNum { get; set; }

        [DataMember]
        public string RetailGroup;

        [DataMember]
        public string RetailDepartment;
        [DataMember]
        public bool ItemDeleted { get; set; }
        [DataMember]
        public ItemTypeEnum ItemType { get; set; }
        [DataMember]
        public bool ItemInventoryExcluded
        {
            get
            {
                return ItemDeleted || ItemType == ItemTypeEnum.Service;
            }
            set
            {

            }
        }

        [DataMember]
        public RecordIdentifier StaffID { get; set; }
        [DataMember]
        public RecordIdentifier StaffLogin { get; set; }
        [DataMember]
        public Guid AreaID { get; set; }
        [DataMember]
        public string StaffName { get; set; }
        [DataMember]
        public string AreaName { get; set; }

        [DataMember]
        public JournalTransStatusEnum LineStatus { get; set; }

        [DataMember]
        public string Barcode { get; set; }

        /// <summary>
        /// The ID of the image associated with this journal line
        /// </summary>       
        [DataMember]
        public RecordIdentifier PictureID { get; set; }

        /// <summary>
        /// The ID of the line that was assigned to it by the inventory app.         
        /// </summary>
        /// <remarks>This value is only  valid while the stock counting journal is not posted. As soon as the user posts the journal the lines are 
        /// compressed and this value will be meaningless</remarks>
        [DataMember]
        public string OmniLineID { get; set; }

        /// <summary>
        /// The ID of the transaction in the inventory app that this line was created on
        /// </summary>
        /// <remarks>This value is only  valid while the stock counting journal is not posted. As soon as the user posts the journal the lines are 
        /// compressed and this value will be meaningless</remarks>
        [DataMember]
        public string OmniTransactionID { get; set; }

        public void SetUnit(Unit unit)
        {
            UnitID = unit.ID;
            UnitDescription = unit.Text;
        }

        public override object Clone()
        {
            var journalLine = new InventoryJournalTransaction();
            Populate(journalLine);

            return journalLine;
        }

        protected void Populate(InventoryJournalTransaction item)
        {
            item.RetailDepartment = RetailDepartment;
            item.RetailGroup = RetailGroup;

            item.Adjustment = Adjustment;
            item.AdjustmentInInventoryUnit = AdjustmentInInventoryUnit;
            item.CostAmount = CostAmount;
            item.CostMarkup = CostMarkup;
            item.CostPrice = CostPrice;
            item.Counted = Counted;
            item.InventOnHandInInventoryUnits = InventOnHandInInventoryUnits;
            item.InventoryUnitDescription = InventoryUnitDescription;
            item.InventoryUnitID = (RecordIdentifier)InventoryUnitID.Clone();
            item.ItemDeleted = ItemDeleted;
            item.ItemId = (RecordIdentifier)ItemId.Clone();
            item.ItemInventoryExcluded = ItemInventoryExcluded;
            item.ItemName = ItemName;
            item.ItemType = ItemType;
            item.JournalId = (RecordIdentifier)JournalId.Clone();
            item.LineNum = (RecordIdentifier)LineNum.Clone();
            item.MasterID = (RecordIdentifier)MasterID.Clone();
            item.MovedQuantity = MovedQuantity;
            item.ParentMasterID = (RecordIdentifier)ParentMasterID.Clone();
            item.Posted = Posted;
            item.PostedDateTime = PostedDateTime;
            item.PriceUnit = PriceUnit;
            item.ReasonId = (RecordIdentifier)ReasonId.Clone();
            item.ReasonText = ReasonText;
            item.SalesAmount = SalesAmount;
            item.Status = Status;
            item.StoreId = (RecordIdentifier)StoreId.Clone();
            item.TransDate = TransDate;
            item.UnitDescription = UnitDescription;
            item.UnitID = (RecordIdentifier)UnitID.Clone();
            item.UnitQuantityLimiter = new DecimalLimit { Max = UnitQuantityLimiter.Max, Min = UnitQuantityLimiter.Min };
            item.VariantName = VariantName;
            item.StaffID = (RecordIdentifier)StaffID.Clone();
            item.AreaID = AreaID;
            item.StaffID = StaffName;
            item.StaffLogin = (RecordIdentifier)StaffLogin.Clone();
            item.AreaName = AreaName;
            item.LineStatus = LineStatus;
            item.Barcode = Barcode;
            item.PictureID = PictureID;
            item.OmniLineID = OmniLineID;
            item.OmniTransactionID = OmniTransactionID;
        }
    }

    public enum InventoryJournalTransactionSorting
    {
        IdentificationNumber = 0,
        LineNumber = 1,
        TransactionDate = 2,
        ItemId = 3,
        Quantity = 4,
        CostPrice = 5,
        PriceUnit = 6,
        CostMarkup = 7,
        CostAmount = 8,
        SalesAmount = 9,
        InventOnHand = 10,
        Counted = 11,
        Reason = 12,
        InventoryDimension = 13,
        ItemName = 14,
        UnitId = 15,
        Posted = 16,
        Variant = 17,
        PostedDate = 18,
        CountedDifference = 19,
        CountedDifferencePercantage = 20,
        RetailGroup = 21,
        RetailDepartment = 22,
        Staff = 23,
        Area = 24,
        Barcode = 25
    }
}
