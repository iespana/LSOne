using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.GUI;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.UserInterface
{
    [Serializable]
    [DataContract]
    public class UIStyle : DataEntity
    {
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        [DataMember]
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

        [StringLength(60)]
        [DataMember]
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
        public RecordIdentifier ParentStyleID {get; set;}

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier ContextID { get; set; }

        [DataMember]
        public BaseStyle Style { get; set; }

        public bool Deleted { get; set; }

        public UIStyle()
        {
            ParentStyleID = RecordIdentifier.Empty;
            ContextID = RecordIdentifier.Empty;
        }
    }
}
