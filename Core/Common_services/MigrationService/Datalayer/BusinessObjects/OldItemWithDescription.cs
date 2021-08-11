using LSOne.DataLayer.BusinessObjects;

namespace LSOne.Services.Datalayer.BusinessObjects
{
    /// <summary>
    /// A simple data entity that is used in dimmensions like Color, Style and Size
    /// </summary>
    public class OldItemWithDescription : DataEntity
    {
        string description;

        /// <summary>
        /// Default constructor
        /// </summary>
        public OldItemWithDescription()
            : base()
        {
            description = "";
        }

        /// <summary>
        /// Description of the Color, Style or Size
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Name of the Color, Style or Size
        /// </summary>
        public string Name
        {
            get { return Text; }
            set { Text = value; }
        }

    }
}
