using LSOne.Utilities.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    public class FloorLayout : DataEntity
    {
        [RecordIdentifierValidation(20)]
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

        [StringLength(60)]
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

        public string JSONDesignData { get; set; }
    }
}
