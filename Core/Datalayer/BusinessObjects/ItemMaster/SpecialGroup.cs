#if !MONO
#endif
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Validation;


namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    /// <summary>
    /// Data entity class for a special group.
    /// </summary>
    public class SpecialGroup : DataEntity
    {
       

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier MasterID { get; set; }

        /// <summary>
        /// The unique ID of the retail group
        /// </summary>
        [RecordIdentifierValidation(20)]
        public override RecordIdentifier ID
        {
            get { return base.ID; }
            set { base.ID = value; }
        }

        /// <summary>
        /// The description of the retail group
        /// </summary>
        [StringLength(60)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

    }
}
 