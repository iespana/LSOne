using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions
{
    public class RetailItemDimension : DimensionTemplate
    {
        public RetailItemDimension()
        {
            Deleted = false;
        }

        [RecordIdentifierValidation(Utilities.DataTypes.RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier RetailItemMasterID { get; set; }

        public int Sequence { get; set; }

        public bool Deleted { get; set; }

    }
}
