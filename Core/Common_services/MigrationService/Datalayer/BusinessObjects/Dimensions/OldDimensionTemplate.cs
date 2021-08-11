using System.ComponentModel.DataAnnotations;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.Validation;

namespace LSOne.Services.Datalayer.BusinessObjects.Dimensions
{
    public class OldDimensionTemplate : DataEntity
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
