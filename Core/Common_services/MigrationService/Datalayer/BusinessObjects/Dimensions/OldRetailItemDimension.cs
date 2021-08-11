using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.Services.Datalayer.BusinessObjects.Dimensions
{
    public class OldRetailItemDimension : OldDimensionTemplate
    {
        public OldRetailItemDimension()
        {
            Deleted = false;
        }

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier RetailItemMasterID { get; set; }

        public int Sequence { get; set; }

        public bool Deleted { get; set; }

    }
}
