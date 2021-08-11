using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Profiles
{
    /// <summary>
    /// A business objects that holds information about each line within the <see cref="StyleProfile"/>
    /// </summary>
    public class PosStyleProfileLine : DataEntity
    {
        /// <summary>
        /// The combined ID of the profile and profile line
        /// </summary>
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(PosStyleProfileLineId, ProfileID);
            }
        }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public PosStyleProfileLine()
            : base()
        {
            PosStyleProfileLineId = RecordIdentifier.Empty;
            ProfileID = RecordIdentifier.Empty;
            MenuID = RecordIdentifier.Empty;
            StyleID = RecordIdentifier.Empty;
            ContextID = RecordIdentifier.Empty;
            System = false;
            MenuDescription = "";
            StyleDescription = "";
            ContextDescription = "";
        }

        /// <summary>
        /// The unique ID of the profile line
        /// </summary>
        public RecordIdentifier PosStyleProfileLineId { get; set; }

        /// <summary>
        /// The unqiue ID of the profile the line belongs to
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier ProfileID { get; set; }
        /// <summary>
        /// The menu ID that is included in the profile line
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier MenuID { get; set; }
        /// <summary>
        /// The style ID that is included in the profile line
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier StyleID { get; set; }
        /// <summary>
        /// The context ID tha tis included in the profile line
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier ContextID { get; set; }

        /// <summary>
        /// The name of the menu included in the profile line
        /// </summary>
        public string MenuDescription { get; internal set; }
        /// <summary>
        /// The name of the style included in the profile line
        /// </summary>
        public string StyleDescription { get; internal set; }
        /// <summary>
        /// The name of the context included in the profile line
        /// </summary>
        public string ContextDescription { get; internal set; }

        /// <summary>
        /// Is this profile line a system line. A system line can be edited but not deleted
        /// </summary>
        public bool System { get; set; }
    }
}
