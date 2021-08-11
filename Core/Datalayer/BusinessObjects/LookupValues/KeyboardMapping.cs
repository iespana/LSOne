using System.ComponentModel.DataAnnotations;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;
#if !MONO
#endif

namespace LSOne.DataLayer.BusinessObjects.LookupValues
{
    public class KeyboardMapping : DataEntity
    {

        public KeyboardMapping()
            : base()
        {
            ActionProperty = "";
        }

        [RecordIdentifierValidation(20,Depth = 2)]
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

        RecordIdentifier KeyboardMappingID
        {
            get
            {
                return ID.PrimaryID;
            }
        }


        public int ASCIIValue
        {
            get
            {
                return (int)ID.SecondaryID;
            }
        }

        /// <summary>
        /// The pos operation ID that the Keyboard mapping maps to
        /// </summary>
        public int Action
        {
            get;
            set;
        }

        [StringLength(50)]
        public string ActionProperty
        {
            get;
            set;
        }
    }
}
