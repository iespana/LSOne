using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Profiles
{
    /// <summary>
    /// A business object that holds information about a <see cref="StyleProfile"/>
    /// </summary>
    public class StyleProfile : DataEntity
    {
        /// <summary>
        /// A constructor for a specific ID and description
        /// </summary>
        /// <param name="id">The unique ID to use for the style profile</param>
        /// <param name="text">The description of the profile being created</param>
        public StyleProfile(RecordIdentifier id, string text)
            : this()
        {
            ID = id;
            Text = text;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public StyleProfile()
            : base() { }

    }
}
