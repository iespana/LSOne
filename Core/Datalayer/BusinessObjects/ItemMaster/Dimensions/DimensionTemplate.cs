using LSOne.Utilities.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions
{
    public class DimensionTemplate : DataEntity
    {

        [RecordIdentifierValidation(Utilities.DataTypes.RecordIdentifier.RecordIdentifierType.Guid)]
        public override Utilities.DataTypes.RecordIdentifier ID
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
    }
}
