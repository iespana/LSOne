using System.ComponentModel.DataAnnotations;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.Services.Datalayer.BusinessObjects.Dimensions
{
    public class OldDimensionAttribute : DataEntity
    {
        public OldDimensionAttribute()
        {
            Code = "";
            Deleted = false;
        }

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public override RecordIdentifier ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        [StringLength(15)]
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


        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier DimensionID
        {
            get;
            set;
        }

        [StringLength(3)]
        public string Code { get; set; }

        public int Sequence { get; set; }

        public bool Deleted { get; set; }
    }
}
