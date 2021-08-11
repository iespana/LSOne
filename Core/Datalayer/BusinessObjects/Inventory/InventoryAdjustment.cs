using System;
using System.Runtime.Serialization;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    [KnownType(typeof(Date))]
    public class InventoryAdjustment : DataEntity
    {
        public InventoryAdjustment()
        {
            JournalType = InventoryJournalTypeEnum.Adjustment;
            PostedDateTime = Date.Empty;
            CreatedDateTime = DateTime.Now;
            StoreId = "";
            MasterID = Guid.NewGuid();
            TemplateID = "";
            StaffID = "";
        }

        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.String)]
        public RecordIdentifier StoreId { get; set; }
        [DataMember]
        public string StoreName { get; internal set; }
        [DataMember]
        public InventoryJournalStatus Posted { get; set; }
        [DataMember]
        public Date PostedDateTime { get; set; }
        [DataMember]
        public InventoryJournalTypeEnum JournalType { get; set; }
        [DataMember]
        public int DeletePostedLines { get; set; }
        [DataMember]
        public DateTime CreatedDateTime { get; set; }
        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier MasterID { get; set; }
        [DataMember]
        public bool CreatedFromOmni { get; set; }
        [DataMember]
        public RecordIdentifier TemplateID { get; set; }
        [DataMember]
        public RecordIdentifier StaffID { get; set; }
        [DataMember]
        public bool IsPartiallyPosted { get; set; }
        /// <summary>
        /// Current processing status of the journal
        /// </summary>
        [DataMember]
        public InventoryProcessingStatus ProcessingStatus { get; set; }
    }
}
