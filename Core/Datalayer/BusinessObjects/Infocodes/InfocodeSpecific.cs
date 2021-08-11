using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Infocodes
{
    public class InfocodeSpecific : DataEntity
    {
        public InfocodeSpecific()
        {
            InfocodeId = RecordIdentifier.Empty;
            RefRelation = RecordIdentifier.Empty;
            Sequence = 0;
            RefRelation2 = "";
            RefRelation3 = "";
            UnitOfMeasure = RecordIdentifier.Empty;
            SalesTypeFilter = RecordIdentifier.Empty;
            Triggering = TriggeringEnum.Automatic;
            UsageCategory = UsageCategoriesEnum.None;
            RefTableId = RefTableEnum.All;
            RefRelationDescription = "";
        }

        private RecordIdentifier infocodeId;
        private RecordIdentifier refRelation;


        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(infocodeId,
                       new RecordIdentifier(refRelation, 
                       new RecordIdentifier(RefRelation2, 
                       new RecordIdentifier(RefRelation3, new RecordIdentifier((int)RefTableId)))));
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public RecordIdentifier InfocodeId
        {
            get { return infocodeId; }
            set { infocodeId = value; }
        }
        public RecordIdentifier RefRelation
        {
            get { return refRelation; }
            set { refRelation = value; }
        }
        public int Sequence { get; set; }
        public RecordIdentifier RefRelation2 { get; set; }
        public RecordIdentifier RefRelation3 { get; set; }
        public RecordIdentifier UnitOfMeasure { get; set; }
        public RecordIdentifier SalesTypeFilter { get; set; }
        public TriggeringEnum Triggering { get; set; }
        public UsageCategoriesEnum UsageCategory { get; set; }
        public RefTableEnum RefTableId { get; set; }
        public string InfocodeDescription { get; set; }
        public InputTypesEnum InfocodeInputType { get; set; }
        public bool InputRequired { get; set; }



        public string RefRelationDescription { get; set; }
    }
}
