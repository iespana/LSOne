using System;
using System.Windows.Forms.VisualStyles;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.LookupValues
{
    public class PaymentMethodLimitation : DataEntity
    {
                
        public PaymentMethodLimitation()
        {
            ID = Guid.Empty;
            TenderID = RecordIdentifier.Empty;
            RestrictionCode = RecordIdentifier.Empty;
            RelationMasterID = Guid.Empty;
            LimitationMasterID = Guid.Empty;
            RelationReadableID = RecordIdentifier.Empty;
            Type = LimitationType.Everything;
            Include = true;
            Description = "";
            VariantDescription = "";
        }

        public PaymentMethodLimitation(RecordIdentifier tenderID, RecordIdentifier restrictionCode, RecordIdentifier relationMasterID, RecordIdentifier relationReadableID, LimitationType type, bool include, string description, string variantDescription)
        {
            TenderID = tenderID;
            RestrictionCode = restrictionCode;
            RelationMasterID = relationMasterID;
            RelationReadableID = relationReadableID;
            Type = type;
            Include = include;
            Description = description;
            VariantDescription = variantDescription;
        }

        public PaymentMethodLimitation(RecordIdentifier tenderID, RecordIdentifier restrictionCode, RecordIdentifier relationMasterID, RecordIdentifier relationReadableID, LimitationType type, bool include, string description)
            : this(tenderID, restrictionCode, relationMasterID, relationReadableID, type, include, description, "")
        {
            
        }
        
        /// <summary>
        /// The ID of the payment type this limitation is for
        /// </summary>
        public RecordIdentifier TenderID { get; set; }

        /// <summary>
        /// The code for the limitation
        /// </summary>
        public RecordIdentifier RestrictionCode { get; set; }

        /// <summary>
        /// The master ID for the related record. This is based on the value in <see cref="Type"/>/>
        /// </summary>
        public RecordIdentifier RelationMasterID { get; set; }

        /// <summary>
        /// The readable ID for the related record. This is based on the value in <see cref="Type"/>
        /// </summary>
        public RecordIdentifier RelationReadableID { get; set; }

        /// <summary>
        /// The master ID for this limitation
        /// </summary>
        public RecordIdentifier LimitationMasterID { get; set; }

        /// <summary>
        /// The type that this limitation applies to (Item, group etc.)
        /// </summary>
        public LimitationType Type { get; set; }

        /// <summary>
        /// If true then this limits they payment type to the records that are included based on <see cref="Type"/> and <see cref="RelationMasterID"/>. If false, the payment type is
        /// should apply to everything else.
        /// </summary>
        public bool Include { get; set; }

        /// <summary>
        /// The description of relation for this limitation. E.g. the item, group- or department name that the limitation applies to
        /// </summary>        
        public string Description { get; set; }

        /// <summary>
        /// If the limitatio applies to a variant item, this shows the description of the variant
        /// </summary>
        public string VariantDescription { get; set; }

        /// <summary>
        /// If true then items paid with this limitation will become tax exempt
        /// </summary>
        public bool TaxExempt { get; set; }

        /// <summary>
        /// Returns true if the limitation has no <see cref="TenderID"/> and no <see cref="RestrictionCode"/>
        /// </summary>
        /// <returns></returns>
        public bool Empty()
        {
            return TenderID == RecordIdentifier.Empty && RestrictionCode.IsEmpty;
        }
    }
}
