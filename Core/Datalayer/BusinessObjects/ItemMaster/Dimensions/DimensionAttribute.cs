using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions
{
    public class DimensionAttribute : DataEntity
    {
        public DimensionAttribute()
        {
            Code = "";
            Deleted = false;
        }

        [RecordIdentifierValidation(Utilities.DataTypes.RecordIdentifier.RecordIdentifierType.Guid)]
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

        [StringLength(30)]
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

        [StringLength(20)]
        public string Code { get; set; }

        public int Sequence { get; set; }

        public bool Deleted { get; set; }
    }
}
